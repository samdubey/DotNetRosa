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
using IPropertyRules = org.javarosa.core.services.properties.IPropertyRules;
namespace org.javarosa.core.services
{
	
	/// <summary> An IProperty Manager is responsible for setting and retrieving name/value pairs
	/// 
	/// </summary>
	/// <author>  Yaw Anokwa
	/// 
	/// </author>
	public interface IPropertyManager
	{
		
		System.Collections.ArrayList getProperty(System.String propertyName);
		void  setProperty(System.String propertyName, System.String propertyValue);
		void  setProperty(System.String propertyName, System.Collections.ArrayList propertyValue);
		System.String getSingularProperty(System.String propertyName);
		void  addRules(IPropertyRules rules);
		System.Collections.ArrayList getRules();
	}
}