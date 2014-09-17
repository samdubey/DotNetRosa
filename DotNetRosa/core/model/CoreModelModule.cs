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
using IModule = org.javarosa.core.api.IModule;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using PrototypeManager = org.javarosa.core.services.PrototypeManager;
using StorageManager = org.javarosa.core.services.storage.StorageManager;
namespace org.javarosa.core.model
{
	
	public class CoreModelModule : IModule
	{
		
		public virtual void  registerModule()
		{
			StorageManager.registerStorage(FormDef.STORAGE_KEY, typeof(FormDef));
			StorageManager.registerStorage(FormInstance.STORAGE_KEY, typeof(FormInstance));
			
			System.String[] classes = new System.String[]{"org.javarosa.core.model.SubmissionProfile", "org.javarosa.core.model.QuestionDef", "org.javarosa.core.model.GroupDef", "org.javarosa.core.model.instance.FormInstance", "org.javarosa.core.model.data.BooleanData", "org.javarosa.core.model.data.DateData", "org.javarosa.core.model.data.DateTimeData", "org.javarosa.core.model.data.DecimalData", "org.javarosa.core.model.data.GeoLineData", "org.javarosa.core.model.data.GeoPointData", "org.javarosa.core.model.data.GeoShapeData", "org.javarosa.core.model.data.IntegerData", "org.javarosa.core.model.data.LongData", "org.javarosa.core.model.data.MultiPointerAnswerData", "org.javarosa.core.model.data.PointerAnswerData", "org.javarosa.core.model.data.SelectMultiData", "org.javarosa.core.model.data.SelectOneData", "org.javarosa.core.model.data.StringData", "org.javarosa.core.model.data.TimeData", "org.javarosa.core.model.data.UncastData", "org.javarosa.core.model.data.helper.BasicDataPointer", "org.javarosa.core.model.Action", "org.javarosa.core.model.actions.SetValueAction"};
			PrototypeManager.registerPrototypes(classes);
		}
	}
}