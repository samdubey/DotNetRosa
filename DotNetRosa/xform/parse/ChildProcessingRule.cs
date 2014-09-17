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
	
	public class ChildProcessingRule
	{
		public const int MULT_NOT_ALLOWED = 1;
		public const int MULT_PROCESS_FIRST_ONLY = 2;
		public const int MULT_PROCESS_ALL = 3;
		
		public System.String name;
		public IElementHandler handler;
		public bool required;
		public bool anyLevel;
		public int multiplicity;
		
		public ChildProcessingRule(System.String name, IElementHandler handler, bool required, bool anyLevel, int multiplicity)
		{
			this.name = name;
			this.handler = handler;
			this.required = required;
			this.anyLevel = anyLevel;
			this.multiplicity = multiplicity;
		}
	}
}