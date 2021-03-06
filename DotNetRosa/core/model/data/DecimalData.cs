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
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.model.data
{
	
	
	/// <summary> A response to a question requesting an Decimal Value.  Adapted from IntegerData</summary>
	/// <author>  Brian DeRenzi
	/// 
	/// </author>
	public class DecimalData : IAnswerData, System.ICloneable
	{
		virtual public System.String DisplayText
		{
			get
			{
				return System.Convert.ToString(d);
			}
			
		}
		virtual public System.Object Value
		{
			get
			{
				return (double) d;
			}
			
			set
			{
				if (value == null)
				{
					throw new System.NullReferenceException("Attempt to set an IAnswerData class to null.");
				}
				d = ((System.Double) value);
			}
			
		}
		internal double d;
		
		/// <summary> Empty Constructor, necessary for dynamic construction during deserialization.
		/// Shouldn't be used otherwise.
		/// </summary>
		public DecimalData()
		{
		}
		
		public DecimalData(double d)
		{
			this.d = d;
		}
		//UPGRADE_ISSUE: Parameter of type 'java.lang.Double' was migrated to type 'Double' which is identical to 'double'. You will get a compilation error with method overloads. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1205'"
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public DecimalData(ref System.Double d)
		{
			Value = d;
		}
		
		Override
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.lang.Object.clone' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		public virtual System.Object Clone()
		{
			return new DecimalData(d);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.data.IAnswerData#getDisplayText()
		*/
		
		Override
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.data.IAnswerData#getValue()
		*/
		
		Override
		
		Override
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.utilities.Externalizable#readExternal(java.io.DataInputStream)
		*/
		
		Override
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			d = ExtUtil.readDecimal(in_Renamed);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.utilities.Externalizable#writeExternal(java.io.DataOutputStream)
		*/
		
		Override
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeDecimal(out_Renamed, d);
		}
		
		Override
		public virtual UncastData uncast()
		{
			return new UncastData(((System.Double) Value).ToString());
		}
		
		Override
		public virtual DecimalData cast(UncastData data)
		{
			try
			{
				return new DecimalData(System.Double.Parse(data.value_Renamed));
			}
			catch (System.FormatException nfe)
			{
				throw new System.ArgumentException("Invalid cast of data [" + data.value_Renamed + "] to type Decimal");
			}
		}
	}
}