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
using OrderedMap = org.javarosa.core.util.OrderedMap;
namespace org.javarosa.core.util.externalizable
{
	
	//map of objects where elements are multiple types, keys are still assumed to be of a single (non-polymorphic) type
	//if elements are compound types (i.e., need wrappers), they must be pre-wrapped before invoking this wrapper, because... come on now.
	public class ExtWrapMapPoly:ExternalizableWrapper
	{
		public ExternalizableWrapper keyType;
		public bool ordered;
		
		/* serialization */
		
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public ExtWrapMapPoly(System.Collections.Hashtable val):this(val, null)
		{
		}
		
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public ExtWrapMapPoly(System.Collections.Hashtable val, ExternalizableWrapper keyType)
		{
			if (val == null)
			{
				throw new System.NullReferenceException();
			}
			
			this.val = val;
			this.keyType = keyType;
			this.ordered = (val is OrderedMap);
		}
		
		/* deserialization */
		
		public ExtWrapMapPoly()
		{
		}
		
		public ExtWrapMapPoly(System.Type keyType):this(keyType, false)
		{
		}
		
		public ExtWrapMapPoly(ExternalizableWrapper keyType):this(keyType, false)
		{
		}
		
		public ExtWrapMapPoly(System.Type keyType, bool ordered):this(new ExtWrapBase(keyType), ordered)
		{
		}
		
		public ExtWrapMapPoly(ExternalizableWrapper keyType, bool ordered)
		{
			if (keyType == null)
			{
				throw new System.NullReferenceException();
			}
			
			this.keyType = keyType;
			this.ordered = ordered;
		}
		
		public override ExternalizableWrapper clone(System.Object val)
		{
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			return new ExtWrapMapPoly((System.Collections.Hashtable) val, keyType);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			System.Collections.Hashtable h = ordered?new OrderedMap():new System.Collections.Hashtable();
			
			long size = ExtUtil.readNumeric(in_Renamed);
			for (int i = 0; i < size; i++)
			{
				System.Object key = ExtUtil.read(in_Renamed, keyType, pf);
				System.Object elem = ExtUtil.read(in_Renamed, new ExtWrapTagged(), pf);
				h[key] = elem;
			}
			
			val = h;
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			System.Collections.Hashtable h = (System.Collections.Hashtable) val;
			
			ExtUtil.writeNumeric(out_Renamed, h.Count);
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(Object key: h.keySet())
			{
				//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
				System.Object elem = h[key];
				
				ExtUtil.write(out_Renamed, keyType == null?key:keyType.clone(key));
				ExtUtil.write(out_Renamed, new ExtWrapTagged(elem));
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  metaReadExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			ordered = ExtUtil.readBool(in_Renamed);
			keyType = ExtWrapTagged.readTag(in_Renamed, pf);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  metaWriteExternal(System.IO.BinaryWriter out_Renamed)
		{
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			System.Collections.Hashtable h = (System.Collections.Hashtable) val;
			System.Object keyTagObj;
			
			ExtUtil.writeBool(out_Renamed, ordered);
			
			//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
			//UPGRADE_TODO: Method 'java.util.HashMap.keySet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapkeySet'"
			keyTagObj = (keyType == null?(h.Count == 0?new System.Object():new SupportClass.HashSetSupport(h.Keys).GetEnumerator().Current):keyType);
			ExtWrapTagged.writeTag(out_Renamed, keyTagObj);
		}
	}
}