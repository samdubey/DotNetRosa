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
namespace org.javarosa.core.data
{
	
	/// <summary> A data pointer representing a pointer to a (usually) larger object in memory.
	/// 
	/// </summary>
	/// <author>  Cory Zue
	/// 
	/// </author>
	public interface IDataPointer:Externalizable
	{
		/// <summary> Get a display string that represents this data.</summary>
		/// <returns>
		/// </returns>
		System.String DisplayText
		{
			
			
			get;
			
		}
		/// <summary> Get the data from the underlying storage.  This should maybe be a stream instead of a byte[]</summary>
		/// <returns>
		/// </returns>
		/// <throws>  IOException </throws>
		sbyte[] Data
		{
			get;
			
		}
		/// <summary> Get the data from the underlying storage.</summary>
		/// <returns>
		/// </returns>
		/// <throws>  IOException </throws>
		System.IO.Stream DataStream
		{
			get;
			
		}
		/// <returns> Gets the length of the data payload
		/// </returns>
		long Length
		{
			get;
			
		}
		
		/// <summary> Deletes the underlying data from storage.</summary>
		bool deleteData();
	}
}