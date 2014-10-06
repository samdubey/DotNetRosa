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
	
	
	/// <summary> A response to a question requesting an Long Numeric Value</summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public class LongData : IAnswerData, System.ICloneable
	{
		virtual public System.String DisplayText
		{
			get
			{
				return System.Convert.ToString(n);
			}
			
		}
		virtual public System.Object Value
		{
			get
			{
				return (long) n;
			}
			
			set
			{
				if (value == null)
				{
					throw new System.NullReferenceException("Attempt to set an IAnswerData class to null.");
				}
				n = (long) ((System.Int64) value);
			}
			
		}
		internal long n;
		
		/// <summary> Empty Constructor, necessary for dynamic construction during deserialization.
		/// Shouldn't be used otherwise.
		/// </summary>
		public LongData()
		{
		}
		
		public LongData(long n)
		{
			this.n = n;
		}
		//UPGRADE_ISSUE: Parameter of type 'java.lang.Long' was migrated to type 'Int64' which is identical to 'long'. You will get a compilation error with method overloads. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1205'"
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public LongData(ref System.Int64 n)
		{
			Value = n;
		}
		
		Override
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.lang.Object.clone' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		public virtual System.Object Clone()
		{
			return new LongData(n);
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
			n = ExtUtil.readNumeric(in_Renamed);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.utilities.Externalizable#writeExternal(java.io.DataOutputStream)
		*/
		
		Override
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeNumeric(out_Renamed, n);
		}
		
		Override
		public virtual UncastData uncast()
		{
			return new UncastData(((long) n).ToString());
		}
		
		Override
		public virtual LongData cast(UncastData data)
		{
			try
			{
				return new LongData(System.Int64.Parse(data.value_Renamed));
			}
			catch (System.FormatException nfe)
			{
				throw new System.ArgumentException("Invalid cast of data [" + data.value_Renamed + "] to type Long");
			}
		}
	}
}