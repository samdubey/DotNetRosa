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
using Condition = org.javarosa.core.model.condition.Condition;
using IConditionExpr = org.javarosa.core.model.condition.IConditionExpr;
using Recalculate = org.javarosa.core.model.condition.Recalculate;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapNullable = org.javarosa.core.util.externalizable.ExtWrapNullable;
using ExtWrapTagged = org.javarosa.core.util.externalizable.ExtWrapTagged;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.model
{
	
	/// <summary> A data binding is an object that represents how a
	/// data element is to be used in a form entry interaction.
	/// 
	/// It contains a reference to where the data should be retreived
	/// and stored, as well as the preload parameters, and the
	/// conditional logic for the question.
	/// 
	/// The class relies on any Data References that are used
	/// in a form to be registered with the FormDefRMSUtility's
	/// prototype factory in order to properly deserialize.
	/// 
	/// </summary>
	/// <author>  Drew Roos
	/// 
	/// </author>
	public class DataBinding
	{
		private void  InitBlock()
		{
			return additionalAttrs;
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <returns> The data reference
		/// </returns>
		/// <param name="ref">the reference to set
		/// </param>
		virtual public IDataReference Reference
		{
			get
			{
				return ref_Renamed;
			}
			
			set
			{
				this.ref_Renamed = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <returns> the id
		/// </returns>
		/// <param name="id">the id to set
		/// </param>
		virtual public System.String Id
		{
			get
			{
				return id;
			}
			
			set
			{
				this.id = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <returns> the dataType
		/// </returns>
		/// <param name="dataType">the dataType to set
		/// </param>
		virtual public int DataType
		{
			get
			{
				return dataType;
			}
			
			set
			{
				this.dataType = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <returns> the preload
		/// </returns>
		/// <param name="preload">the preload to set
		/// </param>
		virtual public System.String Preload
		{
			get
			{
				return preload;
			}
			
			set
			{
				this.preload = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <returns> the preloadParams
		/// </returns>
		/// <param name="preloadParams">the preloadParams to set
		/// </param>
		virtual public System.String PreloadParams
		{
			get
			{
				return preloadParams;
			}
			
			set
			{
				this.preloadParams = value;
			}
			
		}
		private System.String id;
		private IDataReference ref_Renamed;
		private int dataType;
		
		public Condition relevancyCondition;
		public bool relevantAbsolute;
		public Condition requiredCondition;
		public bool requiredAbsolute;
		public Condition readonlyCondition;
		public bool readonlyAbsolute;
		public IConditionExpr constraint;
		public Recalculate calculate;
		
		private System.String preload;
		private System.String preloadParams;
		public System.String constraintMessage;
		
		
		private List< TreeElement > additionalAttrs = new List< TreeElement >();
		
		public DataBinding()
		{
			InitBlock();
			relevantAbsolute = true;
			requiredAbsolute = false;
			readonlyAbsolute = false;
		}
		
		public virtual void  setAdditionalAttribute(System.String namespace_Renamed, System.String name, System.String value_Renamed)
		{
			TreeElement.setAttribute(null, additionalAttrs, namespace_Renamed, name, value_Renamed);
		}
		
		
		public List< TreeElement > getAdditionalAttributes()
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.utilities.Externalizable#readExternal(java.io.DataInputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			Id = ((System.String) ExtUtil.read(in_Renamed, new ExtWrapNullable(typeof(System.String)), pf));
			DataType = ExtUtil.readInt(in_Renamed);
			Preload = ((System.String) ExtUtil.read(in_Renamed, new ExtWrapNullable(typeof(System.String)), pf));
			PreloadParams = ((System.String) ExtUtil.read(in_Renamed, new ExtWrapNullable(typeof(System.String)), pf));
			ref_Renamed = (IDataReference) ExtUtil.read(in_Renamed, new ExtWrapTagged());
			
			//don't bother reading relevancy/required/readonly/constraint/calculate/additionalAttrs right now; they're only used during parse anyway		
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.utilities.Externalizable#writeExternal(java.io.DataOutputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.write(out_Renamed, new ExtWrapNullable(Id));
			ExtUtil.writeNumeric(out_Renamed, DataType);
			ExtUtil.write(out_Renamed, new ExtWrapNullable(Preload));
			ExtUtil.write(out_Renamed, new ExtWrapNullable(PreloadParams));
			ExtUtil.write(out_Renamed, new ExtWrapTagged(ref_Renamed));
			
			//don't bother writing relevancy/required/readonly/constraint/calculate/additionalAttrs right now; they're only used during parse anyway
		}
	}
}