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
namespace org.javarosa.xform.parse
{
	
	//unused for now
	
	public class NodeProcessingRule
	{
		public System.String name;
		public bool allowUnknownChildren;
		public bool allowChildText;
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public System.Collections.Hashtable childRules;
		
		public NodeProcessingRule(System.String name, bool allowUnknownChildren, bool allowChildText)
		{
			this.name = name;
			this.allowUnknownChildren = allowUnknownChildren;
			this.allowChildText = allowChildText;
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			childRules = new System.Collections.Hashtable();
		}
		
		public virtual void  addChild(ChildProcessingRule cpr)
		{
			childRules[cpr.name] = cpr;
		}
	}
}