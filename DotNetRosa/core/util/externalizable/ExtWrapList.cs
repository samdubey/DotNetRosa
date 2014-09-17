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
	
	
	//list of objects of single (non-polymorphic) type
	public class ExtWrapList:ExternalizableWrapper
	{
		public ExternalizableWrapper type;
		private bool sealed_Renamed;
		
		/* serialization */
		
		public ExtWrapList(System.Collections.ArrayList val):this(val, null)
		{
		}
		
		public ExtWrapList(System.Collections.ArrayList val, ExternalizableWrapper type)
		{
			if (val == null)
			{
				throw new System.NullReferenceException();
			}
			
			this.val = val;
			this.type = type;
			this.sealed_Renamed = false;
		}
		
		/* deserialization */
		
		public ExtWrapList()
		{
			this.sealed_Renamed = false;
		}
		
		public ExtWrapList(System.Type type):this(type, false)
		{
		}
		
		public ExtWrapList(System.Type type, bool sealed_Renamed)
		{
			this.type = new ExtWrapBase(type);
			this.sealed_Renamed = sealed_Renamed;
		}
		
		public ExtWrapList(ExternalizableWrapper type)
		{
			if (type == null)
			{
				throw new System.NullReferenceException();
			}
			
			this.type = type;
			this.sealed_Renamed = false;
		}
		
		public override ExternalizableWrapper clone(System.Object val)
		{
			return new ExtWrapList((System.Collections.ArrayList) val, type);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			if (!sealed_Renamed)
			{
				long size = ExtUtil.readNumeric(in_Renamed);
				System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(0));
				for (int i = 0; i < size; i++)
				{
					v.Add(ExtUtil.read(in_Renamed, type, pf));
				}
				val = v;
			}
			else
			{
				int size = (int) ExtUtil.readNumeric(in_Renamed);
				System.Object[] theval = new System.Object[size];
				for (int i = 0; i < size; i++)
				{
					theval[i] = ExtUtil.read(in_Renamed, type, pf);
				}
				val = theval;
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			System.Collections.ArrayList v = (System.Collections.ArrayList) val;
			
			ExtUtil.writeNumeric(out_Renamed, v.Count);
			for (int i = 0; i < v.Count; i++)
			{
				ExtUtil.write(out_Renamed, type == null?v[i]:type.clone(v[i]));
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
			System.Collections.ArrayList v = (System.Collections.ArrayList) val;
			System.Object tagObj;
			
			if (type == null)
			{
				if (v.Count == 0)
				{
					tagObj = new System.Object();
				}
				else
				{
					tagObj = v[0];
				}
			}
			else
			{
				tagObj = type;
			}
			
			ExtWrapTagged.writeTag(out_Renamed, tagObj);
		}
	}
}