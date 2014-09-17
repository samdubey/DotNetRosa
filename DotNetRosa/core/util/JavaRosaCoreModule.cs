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

/// <summary> </summary>
using System;
using IModule = org.javarosa.core.api.IModule;
using ReferenceManager = org.javarosa.core.reference.ReferenceManager;
using ResourceReferenceFactory = org.javarosa.core.reference.ResourceReferenceFactory;
using PrototypeManager = org.javarosa.core.services.PrototypeManager;
namespace org.javarosa.core.util
{
	
	/// <author>  Clayton Sims
	/// </author>
	/// <date>  Jun 1, 2009  </date>
	/// <summary> 
	/// </summary>
	public class JavaRosaCoreModule : IModule
	{
		
		/* (non-Javadoc)
		* @see org.javarosa.core.api.IModule#registerModule(org.javarosa.core.Context)
		*/
		public virtual void  registerModule()
		{
			System.String[] classes = new System.String[]{"org.javarosa.core.services.locale.ResourceFileDataSource", "org.javarosa.core.services.locale.TableLocaleSource"};
			PrototypeManager.registerPrototypes(classes);
			ReferenceManager._().addReferenceFactory(new ResourceReferenceFactory());
		}
	}
}