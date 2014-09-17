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
using EvaluationContext = org.javarosa.core.model.condition.EvaluationContext;
using DateData = org.javarosa.core.model.data.DateData;
using DateTimeData = org.javarosa.core.model.data.DateTimeData;
using DecimalData = org.javarosa.core.model.data.DecimalData;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using IntegerData = org.javarosa.core.model.data.IntegerData;
using LongData = org.javarosa.core.model.data.LongData;
using SelectMultiData = org.javarosa.core.model.data.SelectMultiData;
using StringData = org.javarosa.core.model.data.StringData;
using TimeData = org.javarosa.core.model.data.TimeData;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
//UPGRADE_TODO: The type 'org.javarosa.core.services.storage.IStorageIterator' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using IStorageIterator = org.javarosa.core.services.storage.IStorageIterator;
//UPGRADE_TODO: The type 'org.javarosa.core.services.storage.IStorageUtility' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using IStorageUtility = org.javarosa.core.services.storage.IStorageUtility;
using Persistable = org.javarosa.core.services.storage.Persistable;
using ByteArrayPayload = org.javarosa.core.services.transport.payload.ByteArrayPayload;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.model.util.restorable
{
	
	public class RestoreUtils
	{
		public const System.String RECORD_ID_TAG = "rec-id";
		
		public static IXFormyFactory xfFact;
		
		public static TreeReference ref_Renamed(System.String refStr)
		{
			return xfFact.ref_Renamed(refStr);
		}
		
		public static TreeReference absRef(System.String refStr, FormInstance dm)
		{
			TreeReference ref_Renamed = ref_Renamed(refStr);
			if (!ref_Renamed.Absolute)
			{
				ref_Renamed = ref_Renamed.parent(topRef(dm));
			}
			return ref_Renamed;
		}
		
		public static TreeReference topRef(FormInstance dm)
		{
			return ref_Renamed("/" + dm.getRoot().Name);
		}
		
		public static TreeReference childRef(System.String childPath, TreeReference parentRef)
		{
			return ref_Renamed(childPath).parent(parentRef);
		}
		
		private static FormInstance newDataModel(System.String topTag)
		{
			FormInstance dm = new FormInstance();
			dm.addNode(ref_Renamed("/" + topTag));
			return dm;
		}
		
		public static FormInstance createDataModel(Restorable r)
		{
			FormInstance dm = newDataModel(r.RestorableType);
			
			if (r is Persistable)
			{
				addData(dm, RECORD_ID_TAG, (System.Object) ((Persistable) r).ID);
			}
			
			return dm;
		}
		
		public static FormInstance createRootDataModel(Restorable r)
		{
			FormInstance inst = createDataModel(r);
			inst.schema = "http://openrosa.org/backup";
			addData(inst, "timestamp", System.DateTime.Now, Constants.DATATYPE_DATE_TIME);
			return inst;
		}
		
		public static void  addData(FormInstance dm, System.String xpath, System.Object data)
		{
			addData(dm, xpath, data, getDataType(data));
		}
		
		public static void  addData(FormInstance dm, System.String xpath, System.Object data, int dataType)
		{
			if (data == null)
			{
				dataType = - 1;
			}
			
			IAnswerData val;
			switch (dataType)
			{
				
				case - 1:  val = null; break;
				
				case Constants.DATATYPE_TEXT:  val = new StringData((System.String) data); break;
				
				case Constants.DATATYPE_INTEGER:  //UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					val = new IntegerData(ref new System.Int32[]{(System.Int32) data}[0]); break;
				
				case Constants.DATATYPE_LONG:  //UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					val = new LongData(ref new System.Int64[]{(System.Int64) data}[0]); break;
				
				case Constants.DATATYPE_DECIMAL:  //UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					val = new DecimalData(ref new System.Double[]{(System.Double) data}[0]); break;
				
				case Constants.DATATYPE_BOOLEAN:  val = new StringData(((System.Boolean) data)?"t":"f"); break;
				
				case Constants.DATATYPE_DATE:  //UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					val = new DateData(ref new System.DateTime[]{(System.DateTime) data}[0]); break;
				
				case Constants.DATATYPE_DATE_TIME:  //UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					val = new DateTimeData(ref new System.DateTime[]{(System.DateTime) data}[0]); break;
				
				case Constants.DATATYPE_TIME:  //UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					val = new TimeData(ref new System.DateTime[]{(System.DateTime) data}[0]); break;
				
				case Constants.DATATYPE_CHOICE_LIST:  val = (SelectMultiData) data; break;
				
				default:  throw new System.ArgumentException("Don't know how to handle data type [" + dataType + "]");
				
			}
			
			TreeReference ref_Renamed = absRef(xpath, dm);
			if (dm.addNode(ref_Renamed, val, dataType) == null)
			{
				throw new System.SystemException("error setting value during object backup [" + xpath + "]");
			}
		}
		
		//used for outgoing data
		public static int getDataType(System.Object o)
		{
			int dataType = - 1;
			if (o is System.String)
			{
				dataType = Constants.DATATYPE_TEXT;
			}
			else if (o is System.Int32)
			{
				dataType = Constants.DATATYPE_INTEGER;
			}
			else if (o is System.Int64)
			{
				dataType = Constants.DATATYPE_LONG;
			}
			else if (o is System.Single || o is System.Double)
			{
				dataType = Constants.DATATYPE_DECIMAL;
			}
			else if (o is System.DateTime)
			{
				dataType = Constants.DATATYPE_DATE;
			}
			else if (o is System.Boolean)
			{
				dataType = Constants.DATATYPE_BOOLEAN; //booleans are serialized as a literal 't'/'f'
			}
			else if (o is SelectMultiData)
			{
				dataType = Constants.DATATYPE_CHOICE_LIST;
			}
			return dataType;
		}
		
		//used for incoming data
		public static int getDataType(System.Type c)
		{
			int dataType;
			if (c == typeof(System.String))
			{
				dataType = Constants.DATATYPE_TEXT;
			}
			else if (c == typeof(System.Int32))
			{
				dataType = Constants.DATATYPE_INTEGER;
			}
			else if (c == typeof(System.Int64))
			{
				dataType = Constants.DATATYPE_LONG;
			}
			else if (c == typeof(System.Single) || c == typeof(System.Double))
			{
				dataType = Constants.DATATYPE_DECIMAL;
			}
			else if (c == typeof(System.DateTime))
			{
				dataType = Constants.DATATYPE_DATE;
				//Clayton Sims - Jun 16, 2009 - How are we handling Date v. Time v. DateTime?
			}
			else if (c == typeof(System.Boolean))
			{
				dataType = Constants.DATATYPE_TEXT; //booleans are serialized as a literal 't'/'f'
			}
			else
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new System.SystemException("Can't handle data type " + c.FullName);
			}
			
			return dataType;
		}
		
		public static System.Object getValue(System.String xpath, FormInstance tree)
		{
			return getValue(xpath, topRef(tree), tree);
		}
		
		public static System.Object getValue(System.String xpath, TreeReference context, FormInstance tree)
		{
			TreeElement node = tree.resolveReference(ref_Renamed(xpath).contextualize(context));
			if (node == null)
			{
				throw new System.SystemException("Could not find node [" + xpath + "] when parsing saved instance!");
			}
			
			if (node.isRelevant())
			{
				IAnswerData val = node.Value;
				return (val == null?null:val.Value);
			}
			else
			{
				return null;
			}
		}
		
		public static void  applyDataType(FormInstance dm, System.String path, TreeReference parent, System.Type type)
		{
			applyDataType(dm, path, parent, getDataType(type));
		}
		
		public static void  applyDataType(FormInstance dm, System.String path, TreeReference parent, int dataType)
		{
			TreeReference ref_Renamed = childRef(path, parent);
			
			System.Collections.ArrayList v = new EvaluationContext(dm).expandReference(ref_Renamed);
			for (int i = 0; i < v.Count; i++)
			{
				TreeElement e = dm.resolveReference((TreeReference) v[i]);
				e.DataType = dataType;
			}
		}
		
		public static void  templateChild(FormInstance dm, System.String prefixPath, TreeReference parent, Restorable r)
		{
			TreeReference childRef = (prefixPath == null?parent:RestoreUtils.childRef(prefixPath, parent));
			childRef = org.javarosa.core.model.util.restorable.RestoreUtils.childRef(r.RestorableType, childRef);
			
			templateData(r, dm, childRef);
		}
		
		public static void  templateData(Restorable r, FormInstance dm, TreeReference parent)
		{
			if (parent == null)
			{
				parent = topRef(dm);
				applyDataType(dm, "timestamp", parent, typeof(System.DateTime));
			}
			
			if (r is Persistable)
			{
				applyDataType(dm, RECORD_ID_TAG, parent, typeof(System.Int32));
			}
			
			r.templateData(dm, parent);
		}
		
		public static void  mergeDataModel(FormInstance parent, FormInstance child, System.String xpathParent)
		{
			mergeDataModel(parent, child, absRef(xpathParent, parent));
		}
		
		public static void  mergeDataModel(FormInstance parent, FormInstance child, TreeReference parentRef)
		{
			TreeElement parentNode = parent.resolveReference(parentRef);
			//ugly
			if (parentNode == null)
			{
				parentRef = parent.addNode(parentRef);
				parentNode = parent.resolveReference(parentRef);
			}
			TreeElement childNode = child.getRoot();
			
			int mult = parentNode.getChildMultiplicity(childNode.Name);
			childNode.Mult = mult;
			
			parentNode.addChild(childNode);
		}
		
		public static FormInstance exportRMS(IStorageUtility storage, System.Type type, System.String parentTag, IRecordFilter filter)
		{
			if (!typeof(Externalizable).IsAssignableFrom(type) || !typeof(Restorable).IsAssignableFrom(type))
			{
				return null;
			}
			
			FormInstance dm = newDataModel(parentTag);
			
			IStorageIterator ri = storage.iterate();
			while (ri.hasMore())
			{
				System.Object obj = ri.nextRecord();
				
				if (filter == null || filter.filter(obj))
				{
					FormInstance objModel = ((Restorable) obj).exportData();
					mergeDataModel(dm, objModel, topRef(dm));
				}
			}
			
			return dm;
		}
		
		public static FormInstance subDataModel(TreeElement top)
		{
			TreeElement newTop = top.shallowCopy();
			newTop.Mult = 0;
			return new FormInstance(newTop);
		}
		
		public static void  exportRMS(FormInstance parent, System.Type type, System.String grouperName, IStorageUtility storage, IRecordFilter filter)
		{
			FormInstance entities = RestoreUtils.exportRMS(storage, type, grouperName, filter);
			RestoreUtils.mergeDataModel(parent, entities, ".");
		}
		
		public static void  importRMS(FormInstance dm, IStorageUtility storage, System.Type type, System.String path)
		{
			if (!typeof(Externalizable).IsAssignableFrom(type) || !typeof(Restorable).IsAssignableFrom(type))
			{
				return ;
			}
			
			bool idMatters = typeof(Persistable).IsAssignableFrom(type);
			
			System.String childName = ((Restorable) PrototypeFactory.getInstance(type)).RestorableType;
			TreeElement e = dm.resolveReference(absRef(path, dm));
			System.Collections.ArrayList children = e.getChildrenWithName(childName);
			
			for (int i = 0; i < children.Count; i++)
			{
				FormInstance child = subDataModel((TreeElement) children[i]);
				
				Restorable inst = (Restorable) PrototypeFactory.getInstance(type);
				
				//restore record id first so 'importData' has access to it
				int recID = - 1;
				if (idMatters)
				{
					recID = ((System.Int32) getValue(RECORD_ID_TAG, child));
					((Persistable) inst).ID = recID;
				}
				
				inst.importData(child);
				
				try
				{
					if (idMatters)
					{
						storage.write((Persistable) inst);
					}
					else
					{
						storage.add((Externalizable) inst);
					}
				}
				catch (System.Exception ex)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					throw new System.SystemException("Error importing RMS during restore! [" + type.FullName + ":" + recID + "]; " + ex.Message);
				}
			}
		}
		
		public static ByteArrayPayload dispatch(FormInstance dm)
		{
			return (ByteArrayPayload) xfFact.serializeInstance(dm);
		}
		
		public static FormInstance receive(sbyte[] payload, System.Type restorableType)
		{
			return xfFact.parseRestore(payload, restorableType);
		}
		
		public static bool getBoolean(System.Object o)
		{
			if (o is System.String)
			{
				System.String bool_Renamed = (System.String) o;
				if ("t".Equals(bool_Renamed))
				{
					return true;
				}
				else if ("f".Equals(bool_Renamed))
				{
					return false;
				}
				else
				{
					throw new System.SystemException("boolean string must be t or f");
				}
			}
			else
			{
				throw new System.SystemException("booleans are encoded as strings");
			}
		}
	}
}