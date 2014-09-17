/*
* Copyright (C) 2014 University of Washington
*
* Originally developed by Dobility, Inc. (as part of SurveyCTO)
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
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using RestoreUtils = org.javarosa.core.model.util.restorable.RestoreUtils;
namespace org.javarosa.core.model.instance.utils
{
	
	/// <summary> This class just wraps the default behaviour.
	/// 
	/// Author: Meletis Margaritis
	/// Date: 20/5/2013
	/// Time: 12:50 πμ
	/// </summary>
	public class DefaultAnswerResolver : IAnswerResolver
	{
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		public virtual IAnswerData resolveAnswer(System.String textVal, TreeElement treeElement, FormDef formDef)
		{
			return RestoreUtils.xfFact.parseData(textVal, treeElement.DataType, treeElement.Ref, formDef);
		}
	}
}