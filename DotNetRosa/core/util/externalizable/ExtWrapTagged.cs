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
namespace org.javarosa.core.util.externalizable
{
	
	public class ExtWrapTagged:ExternalizableWrapper
	{
		public static sbyte[] WRAPPER_TAG = new sbyte[]{(sbyte) SupportClass.Identity(0xff), (sbyte) SupportClass.Identity(0xff), (sbyte) SupportClass.Identity(0xff), (sbyte) SupportClass.Identity(0xff)}; //must be same length as PrototypeFactory.CLASS_HASH_SIZE
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public static HashMap < Class, Integer > WRAPPER_CODES;
		
		/* serialization */
		
		public ExtWrapTagged(System.Object val)
		{
			if (val == null)
			{
				throw new System.NullReferenceException();
			}
			else if (val is ExtWrapTagged)
			{
				throw new System.ArgumentException("Wrapping tagged with tagged is redundant");
			}
			
			this.val = val;
		}
		
		/* deserialization */
		
		public ExtWrapTagged()
		{
		}
		
		public override ExternalizableWrapper clone(System.Object val)
		{
			return new ExtWrapTagged(val);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			ExternalizableWrapper type = readTag(in_Renamed, pf);
			val = ExtUtil.read(in_Renamed, type, pf);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			writeTag(out_Renamed, val);
			ExtUtil.write(out_Renamed, val);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public static ExternalizableWrapper readTag(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			sbyte[] tag = new sbyte[PrototypeFactory.CLASS_HASH_SIZE];
			SupportClass.ReadInput(in_Renamed.BaseStream, tag, 0, tag.Length);
			
			if (PrototypeFactory.compareHash(tag, WRAPPER_TAG))
			{
				int wrapperCode = ExtUtil.readInt(in_Renamed);
				
				//find wrapper indicated by code
				ExternalizableWrapper type = null;
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				for(Object key: WRAPPER_CODES.keySet())
				{
					System.Type t = (System.Type) key;
					if (WRAPPER_CODES.get_Renamed(t) == wrapperCode)
					{
						try
						{
							type = (ExternalizableWrapper) PrototypeFactory.getInstance(t);
						}
						catch (CannotCreateObjectException ccoe)
						{
							//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
							throw new CannotCreateObjectException("Serious problem: cannot create built-in ExternalizableWrapper [" + t.FullName + "]");
						}
					}
				}
				if (type == null)
				{
					throw new DeserializationException("Unrecognized ExternalizableWrapper type [" + wrapperCode + "]");
				}
				
				type.metaReadExternal(in_Renamed, pf);
				return type;
			}
			else
			{
				System.Type type = pf.getClass(tag);
				if (type == null)
				{
					throw new DeserializationException("No datatype registered to serialization code " + ExtUtil.printBytes(tag));
				}
				
				return new ExtWrapBase(type);
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public static void  writeTag(System.IO.BinaryWriter out_Renamed, System.Object o)
		{
			if (o is ExternalizableWrapper && !(o is ExtWrapBase))
			{
				out_Renamed.Write(SupportClass.ToByteArray(WRAPPER_TAG), 0, PrototypeFactory.CLASS_HASH_SIZE);
				ExtUtil.writeNumeric(out_Renamed, WRAPPER_CODES.get_Renamed(o.GetType()));
				((ExternalizableWrapper) o).metaWriteExternal(out_Renamed);
			}
			else
			{
				System.Type type = null;
				
				if (o is ExtWrapBase)
				{
					ExtWrapBase extType = (ExtWrapBase) o;
					if (extType.val != null)
					{
						o = extType.val;
					}
					else
					{
						type = extType.type;
					}
				}
				if (type == null)
				{
					type = o.GetType();
				}
				
				sbyte[] tag = PrototypeFactory.getClassHash(type); //cache this?
				out_Renamed.Write(SupportClass.ToByteArray(tag), 0, tag.Length);
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  metaReadExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			throw new System.SystemException("Tagged wrapper should never be tagged");
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  metaWriteExternal(System.IO.BinaryWriter out_Renamed)
		{
			throw new System.SystemException("Tagged wrapper should never be tagged");
		}
		static ExtWrapTagged()
		{
			{
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				WRAPPER_CODES = new HashMap < Class, Integer >();
				WRAPPER_CODES.put(typeof(ExtWrapNullable), 0x00);
				WRAPPER_CODES.put(typeof(ExtWrapList), 0x20);
				WRAPPER_CODES.put(typeof(ExtWrapListPoly), 0x21);
				WRAPPER_CODES.put(typeof(ExtWrapMap), 0x22);
				WRAPPER_CODES.put(typeof(ExtWrapMapPoly), 0x23);
				WRAPPER_CODES.put(typeof(ExtWrapIntEncodingUniform), 0x40);
				WRAPPER_CODES.put(typeof(ExtWrapIntEncodingSmall), 0x41);
			}
		}
	}
}