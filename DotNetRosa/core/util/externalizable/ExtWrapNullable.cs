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
	
	
	public class ExtWrapNullable:ExternalizableWrapper
	{
		public ExternalizableWrapper type;
		
		/* serialization */
		
		public ExtWrapNullable(System.Object val)
		{
			this.val = val;
		}
		
		/* deserialization */
		
		public ExtWrapNullable()
		{
		}
		
		public ExtWrapNullable(System.Type type)
		{
			this.type = new ExtWrapBase(type);
		}
		
		/* serialization or deserialization, depending on context */
		
		public ExtWrapNullable(ExternalizableWrapper type)
		{
			if (type is ExtWrapNullable)
			{
				throw new System.ArgumentException("Wrapping nullable with nullable is redundant");
			}
			else if (type != null && type.Empty)
			{
				this.type = type;
			}
			else
			{
				this.val = type;
			}
		}
		
		public override ExternalizableWrapper clone(System.Object val)
		{
			return new ExtWrapNullable(val);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			if (in_Renamed.ReadBoolean())
			{
				val = ExtUtil.read(in_Renamed, type, pf);
			}
			else
			{
				val = null;
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			if (val != null)
			{
				out_Renamed.Write(true);
				ExtUtil.write(out_Renamed, val);
			}
			else
			{
				out_Renamed.Write(false);
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  metaReadExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			type = ExtWrapTagged.readTag(in_Renamed, pf);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  metaWriteExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtWrapTagged.writeTag(out_Renamed, val == null?new System.Object():val);
		}
	}
}