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
using IDataPointer = org.javarosa.core.data.IDataPointer;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.model.data.helper
{
	
	/// <summary> Basic implementor of the IDataPointer interface that keeps everything in memory</summary>
	/// <author>  Cory Zue
	/// 
	/// </author>
	public class BasicDataPointer : IDataPointer
	{
		virtual public sbyte[] Data
		{
			/*
			* (non-Javadoc)
			* @see org.javarosa.core.model.data.IDataPointer#getData()
			*/
			
			get
			{
				return data;
			}
			
		}
		virtual public System.String DisplayText
		{
			/*
			* (non-Javadoc)
			* @see org.javarosa.core.model.data.IDataPointer#getDisplayText()
			*/
			
			get
			{
				return name;
			}
			
		}
		virtual public System.IO.Stream DataStream
		{
			/*
			* (non-Javadoc)
			* @see org.javarosa.core.model.data.IDataPointer#getDataStream()
			*/
			
			get
			{
				System.IO.MemoryStream bis = new System.IO.MemoryStream(SupportClass.ToByteArray(data));
				return bis;
			}
			
		}
		virtual public long Length
		{
			get
			{
				// TODO Auto-generated method stub
				return data.Length;
			}
			
		}
		
		private sbyte[] data;
		private System.String name;
		
		/// <summary> NOTE: Only for serialization use.</summary>
		public BasicDataPointer()
		{
			//You shouldn't be calling this unless you are deserializing.
		}
		
		public BasicDataPointer(System.String name, sbyte[] data)
		{
			this.name = name;
			this.data = data;
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.model.data.IDataPointer#deleteData()
		*/
		public virtual bool deleteData()
		{
			
			this.data = null;
			return true;
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			int size = in_Renamed.ReadInt32();
			if (size != - 1)
			{
				data = new sbyte[size];
				SupportClass.ReadInput(in_Renamed.BaseStream, data, 0, data.Length);
			}
			name = ExtUtil.readString(in_Renamed);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			if (data == null || data.Length < 0)
			{
				out_Renamed.Write(- 1);
			}
			else
			{
				out_Renamed.Write(data.Length);
				out_Renamed.Write(SupportClass.ToByteArray(data));
			}
			ExtUtil.writeString(out_Renamed, name);
		}
	}
}