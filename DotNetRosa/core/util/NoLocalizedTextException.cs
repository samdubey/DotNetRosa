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
namespace org.javarosa.core.util
{
	
	/// <author>  Clayton Sims
	/// </author>
	/// <date>  May 27, 2009  </date>
	/// <summary> 
	/// </summary>
	[Serializable]
	public class NoLocalizedTextException:System.SystemException
	{
		virtual public System.String MissingKeyNames
		{
			get
			{
				return keynames;
			}
			
		}
		virtual public System.String LocaleMissingKey
		{
			get
			{
				return locale;
			}
			
		}
		private System.String keynames;
		private System.String locale;
		public NoLocalizedTextException(System.String message, System.String keynames, System.String locale):base(message)
		{
			this.keynames = keynames;
			this.locale = locale;
		}
	}
}