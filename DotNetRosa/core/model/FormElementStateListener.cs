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
using TreeElement = org.javarosa.core.model.instance.TreeElement;
namespace org.javarosa.core.model
{
	
	/// <author>  Drew Roos?
	/// 
	/// </author>
	public struct FormElementStateListener_Fields{
		public readonly static int CHANGE_INIT = 0x00;
		public readonly static int CHANGE_DATA = 0x01;
		public readonly static int CHANGE_LOCALE = 0x02;
		public readonly static int CHANGE_ENABLED = 0x04;
		public readonly static int CHANGE_RELEVANT = 0x08;
		public readonly static int CHANGE_REQUIRED = 0x10;
		//	static final int CHANGE_LOCKED = 0x20;
		public readonly static int CHANGE_OTHER = 0x40;
	}
	public interface FormElementStateListener
	{
		//UPGRADE_NOTE: Members of interface 'FormElementStateListener' were extracted into structure 'FormElementStateListener_Fields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1045'"
		
		void  formElementStateChanged(IFormElement question, int changeFlags);
		
		void  formElementStateChanged(TreeElement question, int changeFlags);
	}
}