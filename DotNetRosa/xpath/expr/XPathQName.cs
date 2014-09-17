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
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapNullable = org.javarosa.core.util.externalizable.ExtWrapNullable;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.xpath.expr
{
	
	public class XPathQName
	{
		public System.String namespace_Renamed;
		public System.String name;
		
		public XPathQName()
		{
		} //for deserialization
		
		public XPathQName(System.String qname)
		{
			int sep = (qname == null?- 1:qname.IndexOf(":"));
			if (sep == - 1)
			{
				init(null, qname);
			}
			else
			{
				init(qname.Substring(0, (sep) - (0)), qname.Substring(sep + 1));
			}
		}
		
		public XPathQName(System.String namespace_Renamed, System.String name)
		{
			init(namespace_Renamed, name);
		}
		
		public override int GetHashCode()
		{
			return name.GetHashCode() | (namespace_Renamed == null?0:namespace_Renamed.GetHashCode());
		}
		
		private void  init(System.String namespace_Renamed, System.String name)
		{
			if (name == null || (name != null && name.Length == 0) || (namespace_Renamed != null && namespace_Renamed.Length == 0))
				throw new System.ArgumentException("Invalid QName");
			
			this.namespace_Renamed = namespace_Renamed;
			this.name = name;
		}
		
		public override System.String ToString()
		{
			return (namespace_Renamed == null?name:namespace_Renamed + ":" + name);
		}
		
		public  override bool Equals(System.Object o)
		{
			if (o is XPathQName)
			{
				XPathQName x = (XPathQName) o;
				return ExtUtil.equals(namespace_Renamed, x.namespace_Renamed) && name.Equals(x.name);
			}
			else
			{
				return false;
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			namespace_Renamed = ((System.String) ExtUtil.read(in_Renamed, new ExtWrapNullable(typeof(System.String))));
			name = ExtUtil.readString(in_Renamed);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.write(out_Renamed, new ExtWrapNullable(namespace_Renamed));
			ExtUtil.writeString(out_Renamed, name);
		}
	}
}