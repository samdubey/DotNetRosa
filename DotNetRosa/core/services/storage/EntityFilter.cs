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
namespace org.javarosa.core.services.storage
{
	
	public abstract class EntityFilter
	{
		public EntityFilter()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			return PREFILTER_FILTER;
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		< E >
		
		public const int PREFILTER_EXCLUDE = - 1;
		public const int PREFILTER_INCLUDE = 1;
		public const int PREFILTER_FILTER = 0;
		
		/// <summary> filter based just on ID and metadata (metadata not supported yet!! will always be 'null', currently)
		/// 
		/// </summary>
		/// <param name="id">
		/// </param>
		/// <param name="metaData">
		/// </param>
		/// <returns> if PREFILTER_INCLUDE, record will be included, matches() not called
		/// if PREFILTER_EXCLUDE, record will be excluded, matches() not called
		/// if PREFILTER_FILTER, matches() will be called and record will be included or excluded based on return value
		/// </returns>
		public int preFilter;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		(int id, HashMap < String, Object > metaData)
		
		public abstract bool matches(E e);
	}
}