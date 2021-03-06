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
	
	
	//list of objects of multiple types
	//if elements are compound types (i.e., need wrappers), they must be pre-wrapped before invoking this wrapper, because... come on now.
	public class ExtWrapListPoly:ExternalizableWrapper
	{
		/* serializaiton */
		
		public ExtWrapListPoly(System.Collections.ArrayList val)
		{
			if (val == null)
			{
				throw new System.NullReferenceException();
			}
			
			this.val = val;
		}
		
		/* deserialization */
		
		public ExtWrapListPoly()
		{
		}
		
		public override ExternalizableWrapper clone(System.Object val)
		{
			return new ExtWrapListPoly((System.Collections.ArrayList) val);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(0));
			
			long size = ExtUtil.readNumeric(in_Renamed);
			for (int i = 0; i < size; i++)
			{
				v.Add(ExtUtil.read(in_Renamed, new ExtWrapTagged(), pf));
			}
			
			val = v;
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			System.Collections.ArrayList v = (System.Collections.ArrayList) val;
			
			ExtUtil.writeNumeric(out_Renamed, v.Count);
			for (int i = 0; i < v.Count; i++)
			{
				ExtUtil.write(out_Renamed, new ExtWrapTagged(v[i]));
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