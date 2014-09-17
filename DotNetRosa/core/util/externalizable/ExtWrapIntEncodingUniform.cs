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
	
	
	public class ExtWrapIntEncodingUniform:ExtWrapIntEncoding
	{
		/* serialization */
		
		public ExtWrapIntEncodingUniform(long l)
		{
			val = (long) l;
		}
		
		/* deserialization */
		
		public ExtWrapIntEncodingUniform()
		{
		}
		
		public override ExternalizableWrapper clone(System.Object val)
		{
			return new ExtWrapIntEncodingUniform(ExtUtil.toLong(val));
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			long l = 0;
			sbyte b;
			bool firstByte = true;
			
			do 
			{
				b = (sbyte) in_Renamed.ReadByte();
				
				if (firstByte)
				{
					firstByte = false;
					l = (((b >> 6) & 0x01) == 0?0:- 1); //set initial sign
				}
				
				l = (l << 7) | (b & 0x7f);
			}
			while (((b >> 7) & 0x01) == 1);
			
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
			long l = (long) ((System.Int64) val);
			
			int sig = - 1;
			long k;
			do 
			{
				sig++;
				k = l >> (sig * 7);
			}
			while (k < (- 1 << 6) || k > (1 << 6) - 1); //[-64,63] -- the range we can fit into one byte
			
			for (int i = sig; i >= 0; i--)
			{
				sbyte chunk = (sbyte) ((l >> (i * 7)) & 0x7f);
				out_Renamed.Write((System.Byte) ((i > 0?0x80:0x00) | chunk));
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  metaReadExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			//do nothing
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  metaWriteExternal(System.IO.BinaryWriter out_Renamed)
		{
			//do nothing
		}
	}
}