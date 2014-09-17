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
using IMetaData = org.javarosa.core.services.storage.IMetaData;
using Persistable = org.javarosa.core.services.storage.Persistable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.services.properties
{
	
	/// <summary> Property is an encapsulation of a record containing a property in the J2ME
	/// RMS persistent storage system. It is responsible for serializing a name
	/// value pair.
	/// 
	/// </summary>
	/// <author>  ctsims
	/// </author>
	/// <date>  Jan-20-2008 </date>
	/// <summary> 
	/// </summary>
	public class Property : Persistable, IMetaData
	{
		/// <summary>(non-Javadoc)</summary>
		/// <seealso cref="org.javarosa.singlequestionscreen.storage.IDRecordable.setRecordId(int)">
		/// </seealso>
		virtual public int ID
		{
			get
			{
				return recordId;
			}
			
			set
			{
				this.recordId = value;
			}
			
		}
		virtual public System.String[] MetaDataFields
		{
			get
			{
				return new System.String[]{"NAME"};
			}
			
		}
		public System.String name;
		public System.Collections.ArrayList value_Renamed;
		public int recordId = - 1;
		
		/// <summary>(non-Javadoc)</summary>
		/// <seealso cref="org.javarosa.core.util.externalizable.singlequestionscreen.storage.Externalizable.readExternal(DataInputStream)">
		/// </seealso>
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			StringBuilder b = new StringBuilder();
			
			long available;
			available = in_Renamed.BaseStream.Length - in_Renamed.BaseStream.Position;
			sbyte[] inputarray = new sbyte[(int) available];
			SupportClass.ReadInput(in_Renamed.BaseStream, inputarray, 0, inputarray.Length);
			
			for (int i = 0; i < inputarray.Length; i++)
			{
				char c = (char) inputarray[i];
				b.append(c);
			}
			System.String fullString = b.toString();
			int nameindex = fullString.IndexOf(",");
			value_Renamed = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			if (nameindex == - 1)
			{
				//#if debug.output==verbose
				System.Console.Out.WriteLine("WARNING: Property in RMS with no value:" + fullString);
				//#endif
				name = fullString.Substring(0, (fullString.Length) - (0));
			}
			else
			{
				name = fullString.Substring(0, (nameindex) - (0));
				// The format of the properties should be each one in a list, comma delimited
				System.String packedvalue = fullString.Substring(fullString.IndexOf(",") + 1, (fullString.Length) - (fullString.IndexOf(",") + 1));
				while (packedvalue.Length != 0)
				{
					int index = packedvalue.IndexOf(",");
					if (index == - 1)
					{
						value_Renamed.Add(packedvalue);
						packedvalue = "";
					}
					else
					{
						value_Renamed.Add(packedvalue.Substring(0, (index) - (0)));
						packedvalue = packedvalue.Substring(index + 1, (packedvalue.Length) - (index + 1));
					}
				}
			}
			//UPGRADE_TODO: Method 'java.io.FilterInputStream.close' was converted to 'System.IO.BinaryReader.Close' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFilterInputStreamclose'"
			in_Renamed.Close();
		}
		
		/// <summary>(non-Javadoc)</summary>
		/// <seealso cref="org.javarosa.core.util.externalizable.singlequestionscreen.storage.Externalizable.writeExternal(DataOutputStream)">
		/// </seealso>
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			System.String outputString = name;
			// Note that this enumeration should contain at least one element, otherwise the
			// deserialization is invalid
			System.Collections.IEnumerator en = value_Renamed.GetEnumerator();
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			while (en.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				outputString += ("," + ((System.String) en.Current));
			}
			
			for (int i = 0; i < outputString.Length; ++i)
			{
				out_Renamed.Write((byte) outputString[i]);
			}
			out_Renamed.Close();
		}
		
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public virtual System.Collections.Hashtable getMetaData()
		{
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			System.Collections.Hashtable metadata = new System.Collections.Hashtable();
			System.String[] fields = MetaDataFields;
			for (int i = 0; i < fields.Length; i++)
			{
				metadata[fields[i]] = getMetaData(fields[i]);
			}
			return metadata;
		}
		
		public virtual System.Object getMetaData(System.String fieldName)
		{
			if (fieldName.Equals("NAME"))
			{
				return name;
			}
			else
			{
				throw new System.ArgumentException();
			}
		}
	}
}