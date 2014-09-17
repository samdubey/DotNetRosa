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
using IDataPointer = org.javarosa.core.data.IDataPointer;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
namespace org.javarosa.core.model
{
	
	
	/// <summary> An IAnswerDataSerializer returns an object that can be serialized
	/// into some external storage format, IE: XForm, from an AnswerData
	/// object. Each serializer is responsible for identifying what 
	/// implementations of AnswerData it is able to serialize properly. 
	/// 
	/// Additionally, each serialzer is responsible for extending the types
	/// that it can serialize by registering other serializers.   
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public interface IAnswerDataSerializer
	{
		
		/// <summary> Identifies whether this serializer can turn the AnswerData 
		/// object inside of the given element into an external format.
		/// 
		/// </summary>
		/// <param name="element">The element whose data is to be serialzed
		/// </param>
		/// <returns> true if this can meaningfully serialze the provided
		/// object. false otherwise
		/// </returns>
		bool canSerialize(IAnswerData element);
		
		/// <summary> Serializes the given data object into a format that can
		/// be stored externally using the defined schemas
		/// 
		/// </summary>
		/// <param name="element">The element whose data is to be serialzed
		/// </param>
		/// <param name="schema">the schema containing the necessary bindings to determine
		/// the elements data type
		/// </param>
		/// <returns> An implementation-specific representation of the
		/// given object if canSerialize() would return true for that
		/// object. False otherwise.
		/// </returns>
		System.Object serializeAnswerData(IAnswerData data, int dataType);
		
		/// <summary> Serializes the given data object into a format that can
		/// be stored externally.
		/// 
		/// </summary>
		/// <param name="data">The element whose data is to be serialzed
		/// </param>
		/// <returns> An implementation-specific representation of the
		/// given object if canSerialize() would return true for that
		/// object. False otherwise.
		/// </returns>
		System.Object serializeAnswerData(IAnswerData data);
		
		/// <summary> Extends the serializing capabilities of this serializer
		/// by registering another, and allowing this serializer
		/// to operate on all of the data types that the argument
		/// can.
		/// 
		/// </summary>
		/// <param name="ads">An IAnswerDataSerializer
		/// </param>
		void  registerAnswerSerializer(IAnswerDataSerializer ads);
		
		/// <summary> Identifies whether an answer data object contains data
		/// that needs to be extracted to be handled differently
		/// than the serialized answer data.
		/// 
		/// </summary>
		/// <param name="data">The answer data that should be checked.
		/// </param>
		/// <returns> True if this data contains information that can
		/// be extracted. False if it does not. Null if this serializer
		/// cannot process the data type.
		/// </returns>
		System.Boolean containsExternalData(IAnswerData data);
		
		/// <summary> Retrieves a pointer to external data contained in the
		/// provided answer data, if one exists.
		/// </summary>
		/// <param name="data">The answer data that contains the pointer.
		/// containsExternalData should return true for this data.
		/// </param>
		/// <returns> An IDataPointer to an external piece of data
		/// that couldn't (or shouldn't) be serialized directly. Null
		/// if containsExternalData() does not return true for this
		/// answer data.
		/// </returns>
		IDataPointer[] retrieveExternalDataPointer(IAnswerData data);
	}
}