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
	
	
	/// <summary> Gives objects control over serialization. A replacement for the interfaces
	/// <code>Externalizable</code> and <code>Serializable</code>, which are
	/// missing in CLDC.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:m.nuessler@gmail.com">Matthias Nuessler</a>
	/// </author>
	public interface Externalizable
	{
		
		/// <param name="in">
		/// </param>
		/// <throws>  IOException </throws>
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf);
		
		/// <param name="out">
		/// </param>
		/// <throws>  IOException </throws>
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		void  writeExternal(System.IO.BinaryWriter out_Renamed);
	}
}