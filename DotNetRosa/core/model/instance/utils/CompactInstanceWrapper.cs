/*
* Copyright (C) 2009 JavaRosa
*
* Licensed under the Apache License, Version 2.0 (the "License"); you may not
* use this file except in compliance with the License. You may obtain a copy of
* the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
* WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
* License for the specific language governing permissions and limitations under
* the License.
*/
using System;
using Constants = org.javarosa.core.model.Constants;
using FormDef = org.javarosa.core.model.FormDef;
using BooleanData = org.javarosa.core.model.data.BooleanData;
using DateData = org.javarosa.core.model.data.DateData;
using DateTimeData = org.javarosa.core.model.data.DateTimeData;
using DecimalData = org.javarosa.core.model.data.DecimalData;
using GeoTraceData = org.javarosa.core.model.data.GeoTraceData;
using GeoPointData = org.javarosa.core.model.data.GeoPointData;
using GeoShapeData = org.javarosa.core.model.data.GeoShapeData;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using IntegerData = org.javarosa.core.model.data.IntegerData;
using LongData = org.javarosa.core.model.data.LongData;
using SelectMultiData = org.javarosa.core.model.data.SelectMultiData;
using SelectOneData = org.javarosa.core.model.data.SelectOneData;
using StringData = org.javarosa.core.model.data.StringData;
using TimeData = org.javarosa.core.model.data.TimeData;
using Selection = org.javarosa.core.model.data.helper.Selection;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using InvalidReferenceException = org.javarosa.core.model.instance.InvalidReferenceException;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
//UPGRADE_TODO: The type 'org.javarosa.core.services.storage.IStorageUtility' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using IStorageUtility = org.javarosa.core.services.storage.IStorageUtility;
using StorageManager = org.javarosa.core.services.storage.StorageManager;
using WrappingStorageUtility = org.javarosa.core.services.storage.WrappingStorageUtility;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapBase = org.javarosa.core.util.externalizable.ExtWrapBase;
using ExtWrapList = org.javarosa.core.util.externalizable.ExtWrapList;
using ExtWrapNullable = org.javarosa.core.util.externalizable.ExtWrapNullable;
using ExtWrapTagged = org.javarosa.core.util.externalizable.ExtWrapTagged;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using ExternalizableWrapper = org.javarosa.core.util.externalizable.ExternalizableWrapper;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.model.instance.utils
{
	
	/// <summary> An alternate serialization format for FormInstances (saved form instances) that drastically reduces the
	/// resultant record size by cutting out redundant information. Size savings are typically 90-95%. The trade-off is
	/// that in order to deserialize, a template FormInstance (typically from the original FormDef) must be provided.
	/// 
	/// In general, the format is thus:
	/// 1) write the fields from the FormInstance object (e.g., date saved), excluding those that never change for a given
	/// form type (e.g., schema).
	/// 2) walk the tree depth-first. for each node: if repeatable, write the number of repetitions at the current level; if
	/// not, write a boolean indicating if the node is relevant. non-relevant nodes are not descended into. repeated nodes
	/// (i.e., several nodes with the same name at the current level) are handled in order
	/// 3) for each leaf (data) node, write a boolean whether the node is empty or has data
	/// 4) if the node has data, serialize the data. do not specify the data type -- it can be determined from the template.
	/// multiple choice questions use a more compact format than normal.
	/// 4a) in certain situations where the data differs from its prescribed data type (can happen as the result of 'calculate'
	/// expressions), flag the actual data type by hijacking the 'empty' flag above
	/// 
	/// </summary>
	/// <author>  Drew Roos
	/// 
	/// </author>
	public class CompactInstanceWrapper : WrappingStorageUtility.SerializationWrapper
	{
		virtual public Externalizable Data
		{
			get
			{
				return instance;
			}
			
			set
			{
				this.instance = (FormInstance) value;
			}
			
		}
		public const int CHOICE_VALUE = 0; /* serialize multiple-select choices by writing out the <value> */
		public const int CHOICE_INDEX = 1; /* serialize multiple-select choices by writing out only the index of the
		* choice; much more compact than CHOICE_VALUE, but the deserialized
		* instance must be explicitly re-attached to the parent FormDef (not just
		* the template data instance) before the instance can be serialized to xml
		* (otherwise the actual xml <value>s are still unknown)
		*/
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'CHOICE_MODE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly int CHOICE_MODE = CHOICE_INDEX;
		
		private InstanceTemplateManager templateMgr; /* instance template provider; provides templates needed for deserialization. */
		private FormInstance instance; /* underlying FormInstance to serialize/deserialize */
		
		public CompactInstanceWrapper():this(null)
		{
		}
		
		/// <summary> </summary>
		/// <param name="templateMgr">template provider; if null, template is always fetched on-demand from RMS (slow!)
		/// </param>
		public CompactInstanceWrapper(InstanceTemplateManager templateMgr)
		{
			this.templateMgr = templateMgr;
		}
		
		public virtual System.Type baseType()
		{
			return typeof(FormInstance);
		}
		
		/// <summary> deserialize a compact instance. note the retrieval of the template data instance</summary>
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			int formID = ExtUtil.readInt(in_Renamed);
			instance = getTemplateInstance(formID).Clone();
			
			instance.ID = ExtUtil.readInt(in_Renamed);
			instance.DateSaved = (System.DateTime) ExtUtil.read(in_Renamed, new ExtWrapNullable(typeof(System.DateTime)));
			//formID, name, schema, versions, and namespaces are all invariants of the template instance
			
			TreeElement root = instance.getRoot();
			readTreeElement(root, in_Renamed, pf);
		}
		
		/// <summary> serialize a compact instance</summary>
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			if (instance == null)
			{
				throw new System.SystemException("instance has not yet been set via setData()");
			}
			
			ExtUtil.writeNumeric(out_Renamed, instance.FormId);
			ExtUtil.writeNumeric(out_Renamed, instance.ID);
			ExtUtil.write(out_Renamed, new ExtWrapNullable(instance.DateSaved));
			
			writeTreeElement(out_Renamed, instance.getRoot());
		}
		
		private FormInstance getTemplateInstance(int formID)
		{
			if (templateMgr != null)
			{
				return templateMgr.getTemplateInstance(formID);
			}
			else
			{
				FormInstance template = loadTemplateInstance(formID);
				if (template == null)
				{
					throw new System.SystemException("no formdef found for form id [" + formID + "]");
				}
				return template;
			}
		}
		
		/// <summary> load a template instance fresh from the original FormDef, retrieved from RMS</summary>
		/// <param name="formID">
		/// </param>
		/// <returns>
		/// </returns>
		public static FormInstance loadTemplateInstance(int formID)
		{
			IStorageUtility forms = StorageManager.getStorage(FormDef.STORAGE_KEY);
			FormDef f = (FormDef) forms.read(formID);
			return (f != null?f.MainInstance:null);
		}
		
		/// <summary> recursively read in a node of the instance, by filling out the template instance</summary>
		/// <param name="e">
		/// </param>
		/// <param name="ref">
		/// </param>
		/// <param name="in">
		/// </param>
		/// <param name="pf">
		/// </param>
		/// <throws>  IOException </throws>
		/// <throws>  DeserializationException </throws>
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		private void  readTreeElement(TreeElement e, System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			TreeElement templ = instance.getTemplatePath(e.Ref);
			bool isGroup = !templ.Leaf;
			
			if (isGroup)
			{
				System.Collections.ArrayList childTypes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				for (int i = 0; i < templ.NumChildren; i++)
				{
					System.String childName = templ.getChildAt(i).Name;
					if (!childTypes.Contains(childName))
					{
						childTypes.Add(childName);
					}
				}
				
				for (int i = 0; i < childTypes.Count; i++)
				{
					System.String childName = (System.String) childTypes[i];
					
					TreeReference childTemplRef = e.Ref.extendRef(childName, 0);
					TreeElement childTempl = instance.getTemplatePath(childTemplRef);
					
					bool repeatable = childTempl.Repeatable;
					int n = ExtUtil.readInt(in_Renamed);
					
					bool relevant = (n > 0);
					if (!repeatable && n > 1)
					{
						throw new DeserializationException("Detected repeated instances of a non-repeatable node");
					}
					
					if (repeatable)
					{
						int mult = e.getChildMultiplicity(childName);
						for (int j = mult - 1; j >= 0; j--)
						{
							e.removeChild(childName, j);
						}
						
						for (int j = 0; j < n; j++)
						{
							TreeReference dstRef = e.Ref.extendRef(childName, j);
							try
							{
								instance.copyNode(childTempl, dstRef);
							}
							catch (InvalidReferenceException ire)
							{
								//If there is an invalid reference, this is a malformed instance,
								//so we'll throw a Deserialization exception.
								TreeReference r = ire.InvalidReference;
								if (r == null)
								{
									//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
									throw new DeserializationException("Null Reference while attempting to deserialize! " + ire.Message);
								}
								else
								{
									//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
									throw new DeserializationException("Invalid Reference while attemtping to deserialize! Reference: " + r.toString(true) + " | " + ire.Message);
								}
							}
							
							TreeElement child = e.getChild(childName, j);
							child.setRelevant(true);
							readTreeElement(child, in_Renamed, pf);
						}
					}
					else
					{
						TreeElement child = e.getChild(childName, 0);
						child.setRelevant(relevant);
						if (relevant)
						{
							readTreeElement(child, in_Renamed, pf);
						}
					}
				}
			}
			else
			{
				e.Value = (IAnswerData) ExtUtil.read(in_Renamed, new ExtWrapAnswerData(this, e.DataType));
			}
		}
		
		/// <summary> recursively write out a node of the instance</summary>
		/// <param name="out">
		/// </param>
		/// <param name="e">
		/// </param>
		/// <param name="ref">
		/// </param>
		/// <throws>  IOException </throws>
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		private void  writeTreeElement(System.IO.BinaryWriter out_Renamed, TreeElement e)
		{
			TreeElement templ = instance.getTemplatePath(e.Ref);
			bool isGroup = !templ.Leaf;
			
			if (isGroup)
			{
				System.Collections.ArrayList childTypesHandled = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				for (int i = 0; i < templ.NumChildren; i++)
				{
					System.String childName = templ.getChildAt(i).Name;
					if (!childTypesHandled.Contains(childName))
					{
						childTypesHandled.Add(childName);
						
						int mult = e.getChildMultiplicity(childName);
						if (mult > 0 && !e.getChild(childName, 0).isRelevant())
						{
							mult = 0;
						}
						
						ExtUtil.writeNumeric(out_Renamed, mult);
						for (int j = 0; j < mult; j++)
						{
							writeTreeElement(out_Renamed, e.getChild(childName, j));
						}
					}
				}
			}
			else
			{
				ExtUtil.write(out_Renamed, new ExtWrapAnswerData(this, e.DataType, e.Value));
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'ExtWrapAnswerData' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/// <summary> ExternalizableWrapper to handle writing out a node's data. In particular, handles:
		/// * empty nodes
		/// * ultra-compact serialization of multiple-choice answers
		/// * tagging with extra type information when the template alone will not contain sufficient information
		/// 
		/// </summary>
		/// <author>  Drew Roos
		/// 
		/// </author>
		private class ExtWrapAnswerData:ExternalizableWrapper
		{
			private void  InitBlock(CompactInstanceWrapper enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private CompactInstanceWrapper enclosingInstance;
			public CompactInstanceWrapper Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal int dataType;
			
			public ExtWrapAnswerData(CompactInstanceWrapper enclosingInstance, int dataType, IAnswerData val)
			{
				InitBlock(enclosingInstance);
				this.val = val;
				this.dataType = dataType;
			}
			
			public ExtWrapAnswerData(CompactInstanceWrapper enclosingInstance, int dataType)
			{
				InitBlock(enclosingInstance);
				this.dataType = dataType;
			}
			
			//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
			public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
			{
				sbyte flag = (sbyte) in_Renamed.ReadByte();
				if (flag == 0x00)
				{
					val = null;
				}
				else
				{
					System.Type answerType = org.javarosa.core.model.instance.utils.CompactInstanceWrapper.classForDataType(dataType);
					
					if (answerType == null)
					{
						//custom data types
						val = ExtUtil.read(in_Renamed, new ExtWrapTagged(), pf);
					}
					else if (answerType == typeof(SelectOneData))
					{
						val = Enclosing_Instance.getSelectOne(ExtUtil.read(in_Renamed, org.javarosa.core.model.instance.utils.CompactInstanceWrapper.CHOICE_MODE == org.javarosa.core.model.instance.utils.CompactInstanceWrapper.CHOICE_VALUE?typeof(System.String):typeof(System.Int32)));
					}
					else if (answerType == typeof(SelectMultiData))
					{
						val = Enclosing_Instance.getSelectMulti((System.Collections.ArrayList) ExtUtil.read(in_Renamed, new ExtWrapList(org.javarosa.core.model.instance.utils.CompactInstanceWrapper.CHOICE_MODE == org.javarosa.core.model.instance.utils.CompactInstanceWrapper.CHOICE_VALUE?typeof(System.String):typeof(System.Int32))));
					}
					else
					{
						switch (flag)
						{
							
							case (sbyte) (0x40):  answerType = typeof(StringData); break;
							
							case (sbyte) (0x41):  answerType = typeof(IntegerData); break;
							
							case (sbyte) (0x42):  answerType = typeof(DecimalData); break;
							
							case (sbyte) (0x43):  answerType = typeof(DateData); break;
							
							case (sbyte) (0x44):  answerType = typeof(BooleanData); break;
							}
						
						val = (IAnswerData) ExtUtil.read(in_Renamed, answerType);
					}
				}
			}
			
			//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
			public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
			{
				if (val == null)
				{
					out_Renamed.Write((System.Byte) 0x00);
				}
				else
				{
					sbyte prefix = (sbyte) (0x01);
					Externalizable serEntity;
					
					if (dataType < 0 || dataType >= 100)
					{
						//custom data types
						serEntity = new ExtWrapTagged(val);
					}
					else if (val is SelectOneData)
					{
						serEntity = new ExtWrapBase(Enclosing_Instance.compactSelectOne((SelectOneData) val));
					}
					else if (val is SelectMultiData)
					{
						serEntity = new ExtWrapList(Enclosing_Instance.compactSelectMulti((SelectMultiData) val));
					}
					else
					{
						serEntity = (IAnswerData) val;
						
						//flag when data type differs from the default data type in the <bind> (can happen with 'calculate's)
						if (val.GetType() != org.javarosa.core.model.instance.utils.CompactInstanceWrapper.classForDataType(dataType))
						{
							if (val is StringData)
							{
								prefix = (sbyte) (0x40);
							}
							else if (val is IntegerData)
							{
								prefix = (sbyte) (0x41);
							}
							else if (val is DecimalData)
							{
								prefix = (sbyte) (0x42);
							}
							else if (val is DateData)
							{
								prefix = (sbyte) (0x43);
							}
							else if (val is BooleanData)
							{
								prefix = (sbyte) (0x44);
							}
							else
							{
								throw new System.SystemException("divergent data type not allowed");
							}
						}
					}
					
					out_Renamed.Write((byte) prefix);
					ExtUtil.write(out_Renamed, serEntity);
				}
			}
			
			public override ExternalizableWrapper clone(System.Object val)
			{
				throw new System.SystemException("not supported");
			}
			
			//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
			public override void  metaReadExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
			{
				throw new System.SystemException("not supported");
			}
			
			//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
			public override void  metaWriteExternal(System.IO.BinaryWriter out_Renamed)
			{
				throw new System.SystemException("not supported");
			}
		}
		
		/// <summary> reduce a SelectOneData to an integer (index mode) or string (value mode)</summary>
		/// <param name="data">
		/// </param>
		/// <returns> Integer or String
		/// </returns>
		private System.Object compactSelectOne(SelectOneData data)
		{
			Selection val = (Selection) data.Value;
			return extractSelection(val);
		}
		
		/// <summary> reduce a SelectMultiData to a vector of integers (index mode) or strings (value mode)</summary>
		/// <param name="data">
		/// </param>
		/// <returns>
		/// </returns>
		private System.Collections.ArrayList compactSelectMulti(SelectMultiData data)
		{
			System.Collections.ArrayList val = (System.Collections.ArrayList) data.Value;
			System.Collections.ArrayList choices = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			for (int i = 0; i < val.Count; i++)
			{
				choices.Add(extractSelection((Selection) val[i]));
			}
			return choices;
		}
		
		/// <summary> create a SelectOneData from an integer (index mode) or string (value mode)</summary>
		private SelectOneData getSelectOne(System.Object o)
		{
			return new SelectOneData(makeSelection(o));
		}
		
		/// <summary> create a SelectMultiData from a vector of integers (index mode) or strings (value mode)</summary>
		private SelectMultiData getSelectMulti(System.Collections.ArrayList v)
		{
			System.Collections.ArrayList choices = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			for (int i = 0; i < v.Count; i++)
			{
				choices.Add(makeSelection(v[i]));
			}
			return new SelectMultiData(choices);
		}
		
		/// <summary> extract the value out of a Selection according to the current CHOICE_MODE</summary>
		/// <param name="s">
		/// </param>
		/// <returns> Integer or String
		/// </returns>
		private System.Object extractSelection(Selection s)
		{
			switch (CHOICE_MODE)
			{
				
				case CHOICE_VALUE: 
					return s.Value;
				
				case CHOICE_INDEX: 
					if (s.index == - 1)
					{
						throw new System.SystemException("trying to serialize in choice-index mode but selections do not have indexes set!");
					}
					return (System.Int32) s.index;
				
				default:  throw new System.ArgumentException();
				
			}
		}
		
		/// <summary> build a Selection from an integer or string, according to the current CHOICE_MODE</summary>
		/// <param name="o">
		/// </param>
		/// <returns>
		/// </returns>
		private Selection makeSelection(System.Object o)
		{
			if (o is System.String)
			{
				return new Selection((System.String) o);
			}
			else if (o is System.Int32)
			{
				return new Selection(((System.Int32) o));
			}
			else
			{
				throw new System.SystemException();
			}
		}
		
		public virtual void  clean()
		{
			// TODO Auto-generated method stub
		}
		
		/// <summary> map xforms data types to the Class that represents that data in a FormInstance</summary>
		/// <param name="dataType">
		/// </param>
		/// <returns>
		/// </returns>
		public static System.Type classForDataType(int dataType)
		{
			switch (dataType)
			{
				
				case Constants.DATATYPE_NULL:  return typeof(StringData);
				
				case Constants.DATATYPE_TEXT:  return typeof(StringData);
				
				case Constants.DATATYPE_INTEGER:  return typeof(IntegerData);
				
				case Constants.DATATYPE_LONG:  return typeof(LongData);
				
				case Constants.DATATYPE_DECIMAL:  return typeof(DecimalData);
				
				case Constants.DATATYPE_BOOLEAN:  return typeof(BooleanData);
				
				case Constants.DATATYPE_DATE:  return typeof(DateData);
				
				case Constants.DATATYPE_TIME:  return typeof(TimeData);
				
				case Constants.DATATYPE_DATE_TIME:  return typeof(DateTimeData);
				
				case Constants.DATATYPE_CHOICE:  return typeof(SelectOneData);
				
				case Constants.DATATYPE_CHOICE_LIST:  return typeof(SelectMultiData);
				
				case Constants.DATATYPE_GEOPOINT:  return typeof(GeoPointData);
				
				case Constants.DATATYPE_GEOSHAPE:  return typeof(GeoShapeData);
				
				case Constants.DATATYPE_GEOTRACE:  return typeof(GeoTraceData);
				
				default:  return null;
				
			}
		}
	}
}