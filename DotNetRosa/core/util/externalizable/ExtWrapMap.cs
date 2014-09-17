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
using Map = org.javarosa.core.util.Map;
using OrderedMap = org.javarosa.core.util.OrderedMap;
namespace org.javarosa.core.util.externalizable
{
	
	//map of objects where key and data are all of single (non-polymorphic) type (key and value can be of separate types)
	public class ExtWrapMap:ExternalizableWrapper
	{
		
		public const int TYPE_NORMAL = 0;
		public const int TYPE_ORDERED = 1;
		public const int TYPE_SLOW_COMPACT = 2;
		public const int TYPE_SLOW_READ_ONLY = 4;
		
		public ExternalizableWrapper keyType;
		public ExternalizableWrapper dataType;
		public int type;
		
		/* serialization */
		
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public ExtWrapMap(System.Collections.Hashtable val):this(val, null, null)
		{
		}
		
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public ExtWrapMap(System.Collections.Hashtable val, ExternalizableWrapper dataType):this(val, null, dataType)
		{
		}
		
		public ExtWrapMap(System.Collections.IDictionary val, ExternalizableWrapper keyType, ExternalizableWrapper dataType)
		{
			if (val == null)
			{
				throw new System.NullReferenceException();
			}
			
			this.val = val;
			this.keyType = keyType;
			this.dataType = dataType;
			if (val is Map)
			{
				//TODO: check for sealed
				type = TYPE_SLOW_READ_ONLY;
			}
			else if (val is OrderedMap)
			{
				type = TYPE_ORDERED;
			}
			else
			{
				type = TYPE_NORMAL;
			}
		}
		
		/* deserialization */
		
		public ExtWrapMap()
		{
		}
		
		public ExtWrapMap(System.Type keyType, System.Type dataType):this(keyType, dataType, TYPE_NORMAL)
		{
		}
		
		public ExtWrapMap(System.Type keyType, ExternalizableWrapper dataType):this(keyType, dataType, TYPE_NORMAL)
		{
		}
		
		public ExtWrapMap(ExternalizableWrapper keyType, ExternalizableWrapper dataType):this(keyType, dataType, TYPE_NORMAL)
		{
		}
		
		public ExtWrapMap(System.Type keyType, System.Type dataType, int type):this(new ExtWrapBase(keyType), new ExtWrapBase(dataType), type)
		{
		}
		
		public ExtWrapMap(System.Type keyType, ExternalizableWrapper dataType, int type):this(new ExtWrapBase(keyType), dataType, type)
		{
		}
		
		public ExtWrapMap(ExternalizableWrapper keyType, ExternalizableWrapper dataType, int type)
		{
			if (keyType == null || dataType == null)
			{
				throw new System.NullReferenceException();
			}
			
			this.keyType = keyType;
			this.dataType = dataType;
			this.type = type;
		}
		
		public override ExternalizableWrapper clone(System.Object val)
		{
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			return new ExtWrapMap((System.Collections.Hashtable) val, keyType, dataType);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			if (type != TYPE_SLOW_READ_ONLY)
			{
				System.Collections.IDictionary h;
				long size = ExtUtil.readNumeric(in_Renamed);
				switch (type)
				{
					
					case (TYPE_NORMAL): 
						//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
						h = new System.Collections.Hashtable((int) size);
						break;
					
					case (TYPE_ORDERED): 
						h = new OrderedMap();
						break;
					
					case (TYPE_SLOW_COMPACT): 
						h = new Map((int) size);
						break;
					
					default: 
						//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
						h = new System.Collections.Hashtable((int) size);
						break;
					
				}
				
				for (int i = 0; i < size; i++)
				{
					System.Object key = ExtUtil.read(in_Renamed, keyType, pf);
					System.Object elem = ExtUtil.read(in_Renamed, dataType, pf);
					h[key] = elem;
				}
				
				val = h;
			}
			else
			{
				int size = ExtUtil.readInt(in_Renamed);
				System.Object[] k = new System.Object[size];
				System.Object[] v = new System.Object[size];
				for (int i = 0; i < size; i++)
				{
					k[i] = ExtUtil.read(in_Renamed, keyType, pf);
					v[i] = ExtUtil.read(in_Renamed, dataType, pf);
				}
				val = new Map(k, v);
			}
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
				ExtUtil.write(out_Renamed, dataType == null?elem:dataType.clone(elem));
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  metaReadExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			type = ExtUtil.readInt(in_Renamed);
			keyType = ExtWrapTagged.readTag(in_Renamed, pf);
			dataType = ExtWrapTagged.readTag(in_Renamed, pf);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  metaWriteExternal(System.IO.BinaryWriter out_Renamed)
		{
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			System.Collections.Hashtable h = (System.Collections.Hashtable) val;
			System.Object keyTagObj, elemTagObj;
			
			//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
			//UPGRADE_TODO: Method 'java.util.HashMap.keySet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapkeySet'"
			keyTagObj = (keyType == null?(h.Count == 0?new System.Object():new SupportClass.HashSetSupport(h.Keys).GetEnumerator().Current):keyType);
			//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
			elemTagObj = (dataType == null?(h.Count == 0?new System.Object():h.Values.GetEnumerator().Current):dataType);
			
			ExtUtil.writeNumeric(out_Renamed, type);
			ExtWrapTagged.writeTag(out_Renamed, keyTagObj);
			ExtWrapTagged.writeTag(out_Renamed, elemTagObj);
		}
	}
}