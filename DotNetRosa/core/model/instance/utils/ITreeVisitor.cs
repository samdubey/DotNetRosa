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
//UPGRADE_TODO: The type 'org.javarosa.core.model.instance.AbstractTreeElement' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AbstractTreeElement = org.javarosa.core.model.instance.AbstractTreeElement;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using IInstanceVisitor = org.javarosa.core.model.utils.IInstanceVisitor;
namespace org.javarosa.core.model.instance.utils
{
	
	/// <summary> ITreeVisitor is a visitor interface for the elements of the
	/// FormInstance tree elements. In the case of composite elements,
	/// method dispatch for composite members occurs following dispatch
	/// for the composing member.
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public interface ITreeVisitor:IInstanceVisitor
	{
		new void  visit(FormInstance tree);
		void  visit(AbstractTreeElement element);
	}
}