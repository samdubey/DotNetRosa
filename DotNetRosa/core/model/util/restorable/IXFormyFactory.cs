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
using IConditionExpr = org.javarosa.core.model.condition.IConditionExpr;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using IDataPayload = org.javarosa.core.services.transport.payload.IDataPayload;
namespace org.javarosa.core.model.util.restorable
{
	
	public interface IXFormyFactory
	{
		TreeReference ref_Renamed(System.String refStr);
		IDataPayload serializeInstance(FormInstance dm);
		FormInstance parseRestore(sbyte[] data, System.Type restorableType);
		IAnswerData parseData(System.String textVal, int dataType, TreeReference ref_Renamed, FormDef f);
		System.String serializeData(IAnswerData data);
		
		//kinda ghetto
		IConditionExpr refToPathExpr(TreeReference ref_Renamed);
	}
}