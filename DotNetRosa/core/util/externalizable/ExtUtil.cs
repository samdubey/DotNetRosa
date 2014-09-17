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
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using PrototypeManager = org.javarosa.core.services.PrototypeManager;
using CacheTable = org.javarosa.core.util.CacheTable;
using OrderedMap = org.javarosa.core.util.OrderedMap;
namespace org.javarosa.core.util.externalizable
{
	
	public class ExtUtil
	{
		public ExtUtil()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			ExtUtil.writeNumeric(out_Renamed, attributes.size());
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(TreeElement e: attributes)
			{
				ExtUtil.write(out_Renamed, e.getNamespace());
				ExtUtil.write(out_Renamed, e.getName());
				ExtUtil.write(out_Renamed, e.getAttributeValue());
			}
			int size = (int) ExtUtil.readNumeric(in_Renamed);
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Vector < TreeElement > attributes = new Vector < TreeElement >();
			for (int i = 0; i < size; ++i)
			{
				System.String namespace_Renamed = ExtUtil.readString(in_Renamed);
				System.String name = ExtUtil.readString(in_Renamed);
				System.String value_Renamed = ExtUtil.readString(in_Renamed);
				
				TreeElement attr = TreeElement.constructAttributeElement(namespace_Renamed, name, value_Renamed);
				attr.setParent(parent);
				attributes.addElement(attr);
			}
			return attributes;
			ExtUtil.stringCache = stringCache;
		}
		public static bool interning = true;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public static CacheTable < String > stringCache;
		public static sbyte[] serialize(System.Object o)
		{
			System.IO.MemoryStream baos = new System.IO.MemoryStream();
			try
			{
				//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
				write(new System.IO.BinaryWriter(baos), o);
			}
			catch (System.IO.IOException ioe)
			{
				throw new System.SystemException("IOException writing to ByteArrayOutputStream; shouldn't happen!");
			}
			return SupportClass.ToSByteArray(baos.ToArray());
		}
		
		public static System.Object deserialize(sbyte[] data, System.Type type)
		{
			System.IO.MemoryStream bais = new System.IO.MemoryStream(SupportClass.ToByteArray(data));
			try
			{
				return read(new System.IO.BinaryReader(bais), type);
			}
			catch (System.IO.EndOfStreamException eofe)
			{
				throw new DeserializationException("Unexpectedly reached end of stream when deserializing");
			}
			catch (System.IO.IOException udfe)
			{
				throw new DeserializationException("Unexpectedly reached end of stream when deserializing");
			}
			catch (System.IO.IOException e)
			{
				throw new System.SystemException("Unknown IOException reading from ByteArrayInputStream; shouldn't happen!");
			}
			finally
			{
				try
				{
					bais.Close();
				}
				catch (System.IO.IOException e)
				{
					//already closed. Don't sweat it
				}
			}
		}
		
		public static System.Object deserialize(sbyte[] data, ExternalizableWrapper ew)
		{
			System.IO.MemoryStream bais = new System.IO.MemoryStream(SupportClass.ToByteArray(data));
			try
			{
				return read(new System.IO.BinaryReader(bais), ew);
			}
			catch (System.IO.EndOfStreamException eofe)
			{
				throw new DeserializationException("Unexpectedly reached end of stream when deserializing");
			}
			catch (System.IO.IOException udfe)
			{
				throw new DeserializationException("Unexpectedly reached end of stream when deserializing");
			}
			catch (System.IO.IOException e)
			{
				throw new System.SystemException("Unknown IOException reading from ByteArrayInputStream; shouldn't happen!");
			}
			finally
			{
				try
				{
					bais.Close();
				}
				catch (System.IO.IOException e)
				{
					//already closed. Don't sweat it
				}
			}
		}
		
		public static int getSize(System.Object o)
		{
			return serialize(o).Length;
		}
		
		public static PrototypeFactory defaultPrototypes()
		{
			return PrototypeManager.Default;
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public static void  write(System.IO.BinaryWriter out_Renamed, System.Object data)
		{
			if (data is Externalizable)
			{
				((Externalizable) data).writeExternal(out_Renamed);
			}
			else if (data is System.SByte)
			{
				writeNumeric(out_Renamed, (sbyte) ((System.SByte) data));
			}
			else if (data is System.Int16)
			{
				writeNumeric(out_Renamed, (short) ((System.Int16) data));
			}
			else if (data is System.Int32)
			{
				writeNumeric(out_Renamed, ((System.Int32) data));
			}
			else if (data is System.Int64)
			{
				writeNumeric(out_Renamed, (long) ((System.Int64) data));
			}
			else if (data is System.Char)
			{
				writeChar(out_Renamed, ((System.Char) data));
			}
			else if (data is System.Single)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				writeDecimal(out_Renamed, (float) ((System.Single) data));
			}
			else if (data is System.Double)
			{
				writeDecimal(out_Renamed, ((System.Double) data));
			}
			else if (data is System.Boolean)
			{
				writeBool(out_Renamed, ((System.Boolean) data));
			}
			else if (data is System.String)
			{
				writeString(out_Renamed, (System.String) data);
			}
			else if (data is System.DateTime)
			{
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
				writeDate(out_Renamed, ref new System.DateTime[]{(System.DateTime) data}[0]);
			}
			else if (data is sbyte[])
			{
				writeBytes(out_Renamed, (sbyte[]) data);
			}
			else
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new System.InvalidCastException("Not a serializable datatype: " + data.GetType().FullName);
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public static void  writeNumeric(System.IO.BinaryWriter out_Renamed, long val)
		{
			writeNumeric(out_Renamed, val, new ExtWrapIntEncodingUniform());
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public static void  writeNumeric(System.IO.BinaryWriter out_Renamed, long val, ExtWrapIntEncoding encoding)
		{
			write(out_Renamed, encoding.clone((System.Object) val));
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public static void  writeChar(System.IO.BinaryWriter out_Renamed, char val)
		{
			out_Renamed.Write((System.Char) val);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public static void  writeDecimal(System.IO.BinaryWriter out_Renamed, double val)
		{
			out_Renamed.Write(val);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public static void  writeBool(System.IO.BinaryWriter out_Renamed, bool val)
		{
			out_Renamed.Write(val);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public static void  writeString(System.IO.BinaryWriter out_Renamed, System.String val)
		{
			//UPGRADE_ISSUE: Method 'java.io.DataOutputStream.writeUTF' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioDataOutputStreamwriteUTF_javalangString'"
			out_Renamed.writeUTF(val);
			//we could easily come up with more efficient default encoding for string
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static void  writeDate(System.IO.BinaryWriter out_Renamed, ref System.DateTime val)
		{
			//UPGRADE_TODO: Method 'java.util.Date.getTime' was converted to 'System.DateTime.Ticks' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDategetTime'"
			writeNumeric(out_Renamed, val.Ticks);
			//time zone?
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public static void  writeBytes(System.IO.BinaryWriter out_Renamed, sbyte[] bytes)
		{
			ExtUtil.writeNumeric(out_Renamed, bytes.Length);
			if (bytes.Length > 0)
			//i think writing zero-length array might close the stream
				out_Renamed.Write(SupportClass.ToByteArray(bytes));
		}
		
		//functions like these are bad; they should use the built-in list serialization facilities
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public static void  writeInts(System.IO.BinaryWriter out_Renamed, int[] ints)
		{
			ExtUtil.writeNumeric(out_Renamed, ints.Length);
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(int i: ints)
			{
				ExtUtil.writeNumeric(out_Renamed, i);
			}
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public static
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		void writeAttributes(DataOutputStream out, Vector < TreeElement > attributes) throws IOException
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static System.Object read(System.IO.BinaryReader in_Renamed, System.Type type)
		{
			return read(in_Renamed, type, null);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static System.Object read(System.IO.BinaryReader in_Renamed, System.Type type, PrototypeFactory pf)
		{
			if (typeof(Externalizable).IsAssignableFrom(type))
			{
				Externalizable ext = (Externalizable) PrototypeFactory.getInstance(type);
				ext.readExternal(in_Renamed, pf == null?defaultPrototypes():pf);
				return ext;
			}
			else if (type == typeof(System.SByte))
			{
				return (sbyte) readByte(in_Renamed);
			}
			else if (type == typeof(System.Int16))
			{
				return (short) readShort(in_Renamed);
			}
			else if (type == typeof(System.Int32))
			{
				return (System.Int32) readInt(in_Renamed);
			}
			else if (type == typeof(System.Int64))
			{
				return (long) readNumeric(in_Renamed);
			}
			else if (type == typeof(System.Char))
			{
				return readChar(in_Renamed);
			}
			else if (type == typeof(System.Single))
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				return (float) readDecimal(in_Renamed);
			}
			else if (type == typeof(System.Double))
			{
				return (double) readDecimal(in_Renamed);
			}
			else if (type == typeof(System.Boolean))
			{
				return readBool(in_Renamed);
			}
			else if (type == typeof(System.String))
			{
				return readString(in_Renamed);
			}
			else if (type == typeof(System.DateTime))
			{
				return readDate(in_Renamed);
			}
			else if (type == typeof(sbyte[]))
			{
				return readBytes(in_Renamed);
			}
			else
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new System.InvalidCastException("Not a deserializable datatype: " + type.FullName);
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static System.Object read(System.IO.BinaryReader in_Renamed, ExternalizableWrapper ew)
		{
			return read(in_Renamed, ew, null);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static System.Object read(System.IO.BinaryReader in_Renamed, ExternalizableWrapper ew, PrototypeFactory pf)
		{
			ew.readExternal(in_Renamed, pf == null?defaultPrototypes():pf);
			return ew.val;
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static long readNumeric(System.IO.BinaryReader in_Renamed)
		{
			return readNumeric(in_Renamed, new ExtWrapIntEncodingUniform());
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static long readNumeric(System.IO.BinaryReader in_Renamed, ExtWrapIntEncoding encoding)
		{
			try
			{
				return (long) ((System.Int64) read(in_Renamed, encoding));
			}
			catch (DeserializationException de)
			{
				throw new System.SystemException("Shouldn't happen: Base-type encoding wrappers should never touch prototypes");
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static int readInt(System.IO.BinaryReader in_Renamed)
		{
			return toInt(readNumeric(in_Renamed));
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static short readShort(System.IO.BinaryReader in_Renamed)
		{
			return toShort(readNumeric(in_Renamed));
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static sbyte readByte(System.IO.BinaryReader in_Renamed)
		{
			return toByte(readNumeric(in_Renamed));
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static char readChar(System.IO.BinaryReader in_Renamed)
		{
			return in_Renamed.ReadChar();
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static double readDecimal(System.IO.BinaryReader in_Renamed)
		{
			return in_Renamed.ReadDouble();
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static bool readBool(System.IO.BinaryReader in_Renamed)
		{
			return in_Renamed.ReadBoolean();
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static System.String readString(System.IO.BinaryReader in_Renamed)
		{
			//UPGRADE_ISSUE: Method 'java.io.DataInputStream.readUTF' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioDataInputStreamreadUTF'"
			System.String s = in_Renamed.readUTF();
			return (interning && stringCache != null)?stringCache.intern(s):s;
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static System.DateTime readDate(System.IO.BinaryReader in_Renamed)
		{
			//UPGRADE_TODO: Constructor 'java.util.Date.Date' was converted to 'System.DateTime.DateTime' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDateDate_long'"
			return new System.DateTime(readNumeric(in_Renamed));
			//time zone?
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static sbyte[] readBytes(System.IO.BinaryReader in_Renamed)
		{
			int size = (int) ExtUtil.readNumeric(in_Renamed);
			sbyte[] bytes = new sbyte[size];
			int read = 0;
			int toread = size;
			while (read != size)
			{
				read = SupportClass.ReadInput(in_Renamed.BaseStream, bytes, 0, toread);
				toread -= read;
			}
			return bytes;
		}
		
		//bad
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static int[] readInts(System.IO.BinaryReader in_Renamed)
		{
			int size = (int) ExtUtil.readNumeric(in_Renamed);
			int[] ints = new int[size];
			for (int i = 0; i < size; ++i)
			{
				ints[i] = (int) ExtUtil.readNumeric(in_Renamed);
			}
			return ints;
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public static Vector < TreeElement > readAttributes(DataInputStream in, TreeElement parent) throws IOException
		
		public static int toInt(long l)
		{
			if (l < System.Int32.MinValue || l > System.Int32.MaxValue)
				throw new System.ArithmeticException("Value (" + l + ") cannot fit into int");
			return (int) l;
		}
		
		public static short toShort(long l)
		{
			if (l < System.Int16.MinValue || l > System.Int16.MaxValue)
				throw new System.ArithmeticException("Value (" + l + ") cannot fit into short");
			return (short) l;
		}
		
		public static sbyte toByte(long l)
		{
			if (l < (sbyte) System.SByte.MinValue || l > (sbyte) System.SByte.MaxValue)
				throw new System.ArithmeticException("Value (" + l + ") cannot fit into byte");
			return (sbyte) l;
		}
		
		public static long toLong(System.Object o)
		{
			if (o is System.SByte)
			{
				return (sbyte) ((System.SByte) o);
			}
			else if (o is System.Int16)
			{
				return (short) ((System.Int16) o);
			}
			else if (o is System.Int32)
			{
				return ((System.Int32) o);
			}
			else if (o is System.Int64)
			{
				return (long) ((System.Int64) o);
			}
			else if (o is System.Char)
			{
				return ((System.Char) o);
			}
			else
			{
				throw new System.InvalidCastException();
			}
		}
		
		public static sbyte[] nullIfEmpty(sbyte[] ba)
		{
			return (ba == null?null:(ba.Length == 0?null:ba));
		}
		
		public static System.String nullIfEmpty(System.String s)
		{
			return (s == null?null:(s.Length == 0?null:s));
		}
		
		public static System.Collections.ArrayList nullIfEmpty(System.Collections.ArrayList v)
		{
			return (v == null?null:(v.Count == 0?null:v));
		}
		
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public static System.Collections.Hashtable nullIfEmpty(System.Collections.Hashtable h)
		{
			return (h == null?null:(h.Count == 0?null:h));
		}
		
		public static sbyte[] emptyIfNull(sbyte[] ba)
		{
			return ba == null?new sbyte[0]:ba;
		}
		
		public static System.String emptyIfNull(System.String s)
		{
			return s == null?"":s;
		}
		
		public static System.Collections.ArrayList emptyIfNull(System.Collections.ArrayList v)
		{
			return v == null?System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10)):v;
		}
		
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public static System.Collections.Hashtable emptyIfNull(System.Collections.Hashtable h)
		{
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			return h == null?new System.Collections.Hashtable():h;
		}
		
		public static System.Object unwrap(System.Object o)
		{
			return (o is ExternalizableWrapper?((ExternalizableWrapper) o).baseValue():o);
		}
		
		public static bool equals(System.Object a, System.Object b)
		{
			a = unwrap(a);
			b = unwrap(b);
			
			if (a == null)
			{
				return b == null;
			}
			else if (a is System.Collections.ArrayList)
			{
				return (b is System.Collections.ArrayList && vectorEquals((System.Collections.ArrayList) a, (System.Collections.ArrayList) b));
			}
			else
			{
				//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
				if (a is System.Collections.Hashtable)
				{
					//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
					return (b is System.Collections.Hashtable && hashMapEquals((System.Collections.Hashtable) a, (System.Collections.Hashtable) b));
				}
				else
				{
					return a.Equals(b);
				}
			}
		}
		
		public static bool vectorEquals(System.Collections.ArrayList a, System.Collections.ArrayList b)
		{
			if (a.Count != b.Count)
			{
				return false;
			}
			else
			{
				for (int i = 0; i < a.Count; i++)
				{
					if (!equals(a[i], b[i]))
					{
						return false;
					}
				}
				
				return true;
			}
		}
		
		public static bool arrayEquals(System.Object[] a, System.Object[] b)
		{
			if (a.Length != b.Length)
			{
				return false;
			}
			else
			{
				for (int i = 0; i < a.Length; i++)
				{
					if (!equals(a[i], b[i]))
					{
						return false;
					}
				}
				
				return true;
			}
		}
		
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public static bool hashMapEquals(System.Collections.Hashtable a, System.Collections.Hashtable b)
		{
			if (a.Count != b.Count)
			{
				return false;
			}
			else if (a is OrderedMap != b is OrderedMap)
			{
				return false;
			}
			else
			{
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				for(Object keyA: a.keySet())
				{
					
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					if (!equals(a[keyA], b[keyA]))
					{
						return false;
					}
				}
				
				if (a is OrderedMap && b is OrderedMap)
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.keySet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapkeySet'"
					System.Collections.IEnumerator ea = new SupportClass.HashSetSupport(a.Keys).GetEnumerator();
					//UPGRADE_TODO: Method 'java.util.HashMap.keySet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapkeySet'"
					System.Collections.IEnumerator eb = new SupportClass.HashSetSupport(b.Keys).GetEnumerator();
					
					//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
					while (ea.MoveNext())
					{
						//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
						System.Object keyA = ea.Current;
						//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
						System.Object keyB = eb.Current;
						
						if (!keyA.Equals(keyB))
						{
							//must use built-in equals for keys, as that's what HashMap uses
							return false;
						}
					}
				}
				
				return true;
			}
		}
		
		public static System.String printBytes(sbyte[] data)
		{
			StringBuilder sb = new StringBuilder();
			sb.append("[");
			for (int i = 0; i < data.Length; i++)
			{
				System.String hex = System.Convert.ToString(data[i], 16);
				if (hex.Length == 1)
					hex = "0" + hex;
				else
					hex = hex.Substring(hex.Length - 2);
				sb.append(hex);
				if (i < data.Length - 1)
				{
					if ((i + 1) % 30 == 0)
						sb.append("\n ");
					else if ((i + 1) % 10 == 0)
						sb.append("  ");
					else
						sb.append(" ");
				}
			}
			sb.append("]");
			return sb.toString();
		}
		
		
		
		
		
		//**REMOVE THESE TWO FUNCTIONS//
		//original deserialization API (whose limits made us make this whole new framework!); here for backwards compatibility
		public static void  deserialize(sbyte[] data, Externalizable ext)
		{
			ext.readExternal(new System.IO.BinaryReader(new System.IO.MemoryStream(SupportClass.ToByteArray(data))), defaultPrototypes());
		}
		public static System.Object deserialize(sbyte[] data, System.Type type, PrototypeFactory pf)
		{
			return read(new System.IO.BinaryReader(new System.IO.MemoryStream(SupportClass.ToByteArray(data))), type, pf);
		}
		////
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public static
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		void attachCacheTable(CacheTable < String > stringCache)
	}
}