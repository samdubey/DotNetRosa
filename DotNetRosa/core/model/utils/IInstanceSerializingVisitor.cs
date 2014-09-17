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
using FormDef = org.javarosa.core.model.FormDef;
using IAnswerDataSerializer = org.javarosa.core.model.IAnswerDataSerializer;
using IDataReference = org.javarosa.core.model.IDataReference;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using IDataPayload = org.javarosa.core.services.transport.payload.IDataPayload;
namespace org.javarosa.core.model.utils
{
	
	/// <summary> An IInstanceSerializingVisitor serializes a DataModel
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public interface IInstanceSerializingVisitor:IInstanceVisitor
	{
		IAnswerDataSerializer AnswerDataSerializer
		{
			set;
			
		}
		
		//LEGACY: Should remove
		sbyte[] serializeInstance(FormInstance model, FormDef formDef);
		
		sbyte[] serializeInstance(FormInstance model, IDataReference ref_Renamed);
		sbyte[] serializeInstance(FormInstance model);
		
		IDataPayload createSerializedPayload(FormInstance model, IDataReference ref_Renamed);
		IDataPayload createSerializedPayload(FormInstance model);
		
		IInstanceSerializingVisitor newInstance();
	}
}