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
namespace org.javarosa.core.services.locale
{
	
	/// <summary> Localizable objects are able to update their text
	/// based on the current locale.
	/// 
	/// </summary>
	/// <author>  Drew Roos
	/// 
	/// </author>
	public interface Localizable
	{
		/// <summary> Updates the current object with the locate given.</summary>
		/// <param name="locale">
		/// </param>
		/// <param name="localizer">
		/// </param>
		void  localeChanged(System.String locale, Localizer localizer);
	}
}