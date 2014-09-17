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
	
	
	/// <summary> issues
	/// 
	/// * conflicting constructors... null(listwrapper())... confuses listwrapper as type, even though it contains val
	/// </summary>
	
	/// <summary>  constructor guidelines: each child of this class should follow these rules with its constructors
	/// 
	/// 1) every constructor that sets 'val' should have a matching constructor for deserialization that
	/// leaves 'val' null
	/// 2) every constructor that accepts an ExternalizableWrapper should also have a convenience constructor
	/// that accepts a Class, and wraps the Class in an ExtWrapBase (the identity wrapper)
	/// 3) there must exist a null constructor for meta-deserialization (for applicable wrappers)
	/// 4) be careful about properly disambiguating constructors
	/// </summary>
	
	public abstract class ExternalizableWrapper
	{
		virtual public bool Empty
		{
			get
			{
				return baseValue() == null;
			}
			
		}
		/* core data that is being wrapped; will be null when shell wrapper is created for deserialization */
		public System.Object val;
		
		/* create a copy of a wrapper, but with new val (but all the same type annotations */
		public abstract ExternalizableWrapper clone(System.Object val);
		
		/* deserialize the state of the externalizable wrapper */
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public abstract void  metaReadExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf);
		
		/* serialize the state of the externalizable wrapper (type information only, not value) */
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public abstract void  metaWriteExternal(System.IO.BinaryWriter out_Renamed);
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public abstract void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf);
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public abstract void  writeExternal(System.IO.BinaryWriter out_Renamed);
		
		public virtual System.Object baseValue()
		{
			System.Object baseVal = val;
			while (baseVal is ExternalizableWrapper)
			{
				baseVal = ((ExternalizableWrapper) baseVal).val;
			}
			return baseVal;
		}
	}
}