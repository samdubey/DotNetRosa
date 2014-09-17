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
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
namespace org.javarosa.core.model
{
	
	/// <summary> An IDataReference is a reference to a value in a data
	/// model.
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// </author>
	public interface IDataReference:Externalizable
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <returns> The data reference value
		/// </returns>
		/// <param name="reference">the data reference value to be set
		/// </param>
		System.Object Reference
		{
			get;
			
			set;
			
			//	/**
			//	 * @param reference A reference to be evaluated against this reference
			//	 * @return true if the given data reference is associated with the same
			//	 * data value as this reference.
			//	 */
			//	boolean referenceMatches(IDataReference reference);
			//	
			//	/** 
			//	 * @return a new object that is a copy of this data reference
			//	 */
			//	IDataReference clone();
			
		}
	}
}