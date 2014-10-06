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
using Selection = org.javarosa.core.model.data.helper.Selection;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.model.data
{
	
	/// <summary> A response to a question requesting a selection
	/// of one and only one item from a list
	/// 
	/// </summary>
	/// <author>  Drew Roos
	/// 
	/// </author>
	public class SelectOneData : IAnswerData, System.ICloneable
	{
		virtual public System.Object Value
		{
			get
			{
				return s;
			}
			
			set
			{
				if (value == null)
				{
					throw new System.NullReferenceException("Attempt to set an IAnswerData class to null.");
				}
				s = (Selection) value;
			}
			
		}
		virtual public System.String DisplayText
		{
			get
			{
				return s.Value;
			}
			
		}
		internal Selection s;
		
		/// <summary> Empty Constructor, necessary for dynamic construction during deserialization.
		/// Shouldn't be used otherwise.
		/// </summary>
		public SelectOneData()
		{
		}
		
		public SelectOneData(Selection s)
		{
			Value = s;
		}
		
		Override
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.lang.Object.clone' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		public virtual System.Object Clone()
		{
			return new SelectOneData(s.Clone());
		}
		
		Override
		
		Override
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.utilities.Externalizable#readExternal(java.io.DataInputStream)
		*/
		
		Override
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			s = (Selection) ExtUtil.read(in_Renamed, typeof(Selection), pf);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.utilities.Externalizable#writeExternal(java.io.DataOutputStream)
		*/
		
		Override
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.write(out_Renamed, s);
		}
		
		Override
		public virtual UncastData uncast()
		{
			return new UncastData(s.Value);
		}
		
		Override
		public virtual SelectOneData cast(UncastData data)
		{
			return new SelectOneData(new Selection(data.value_Renamed));
		}
	}
}