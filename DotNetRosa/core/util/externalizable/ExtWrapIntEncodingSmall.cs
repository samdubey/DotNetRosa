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
	
	
	public class ExtWrapIntEncodingSmall:ExtWrapIntEncoding
	{
		public const int DEFAULT_BIAS = 1;
		
		/* max magnitude of negative number encodable in one byte; allowed range [0,254]
		* increasing this steals from the max positive range
		* ex.: BIAS = 0   -> [0,254] will fit in one byte; all other values will overflow
		*      BIAS = 30  -> [-30,224]
		*      BIAS = 254 -> [-254,0]
		*/
		public int bias;
		
		/* serialization */
		
		public ExtWrapIntEncodingSmall(long l):this(l, DEFAULT_BIAS)
		{
		}
		
		public ExtWrapIntEncodingSmall(long l, int bias)
		{
			val = (long) l;
			this.bias = bias;
		}
		
		/* deserialization */
		
		public ExtWrapIntEncodingSmall():this(null, DEFAULT_BIAS)
		{
		}
		
		//need the garbage param or else it conflicts with (long) constructor
		public ExtWrapIntEncodingSmall(System.Object ignore, int bias)
		{
			this.bias = bias;
		}
		
		public override ExternalizableWrapper clone(System.Object val)
		{
			return new ExtWrapIntEncodingSmall(ExtUtil.toLong(val), bias);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			sbyte b = (sbyte) in_Renamed.ReadByte();
			long l;
			
			if (b == (sbyte) SupportClass.Identity(0xff))
			{
				l = in_Renamed.ReadInt32();
			}
			else
			{
				l = (b < 0?b + 256:b) - bias;
			}
			
			val = (long) l;
		}
		
		/// <summary> serialize a numeric value, only using as many bytes as needed. splits up the value into
		/// chunks of 7 bits, using as many chunks as needed to unambiguously represent the value. each
		/// chunk is serialized as a single byte, where the most-significant bit is set to 1 to indicate
		/// there are more bytes to follow, or 0 to indicate the last byte
		/// 
		/// </summary>
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			int n = ExtUtil.toInt((long) ((System.Int64) val));
			
			if (n >= - bias && n < 255 - bias)
			{
				n += bias;
				out_Renamed.Write((byte) (n >= 128?n - 256:n));
			}
			else
			{
				out_Renamed.Write((System.Byte) 0xff);
				out_Renamed.Write(n);
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  metaReadExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			bias = in_Renamed.ReadByte();
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  metaWriteExternal(System.IO.BinaryWriter out_Renamed)
		{
			out_Renamed.Write((byte) bias);
		}
	}
}