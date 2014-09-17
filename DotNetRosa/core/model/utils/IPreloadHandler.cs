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
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
namespace org.javarosa.core.model.utils
{
	
	/// <summary> An IPreloadHandler is capable of taking in a set of parameters
	/// for a question's preloaded value, and returning an IAnswerData
	/// object that should be preloaded for a question. 
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public interface IPreloadHandler
	{
		
		/// <returns> A String representing the preload handled by this handler
		/// </returns>
		System.String preloadHandled();
		
		/// <summary> Takes in a set of preload parameters, and determines the 
		/// proper IAnswerData to be preloaded for a question.
		/// 
		/// </summary>
		/// <param name="preloadParams">the parameters determining the preload value
		/// </param>
		/// <returns> An IAnswerData to be used as the default, preloaded value
		/// for a Question.
		/// </returns>
		IAnswerData handlePreload(System.String preloadParams);
		
		/// <summary> Handles any post processing tasks that should be completed after the form entry
		/// interaction is completed.
		/// 
		/// </summary>
		/// <param name="model">The completed data model.
		/// </param>
		/// <param name="ref">The reference to be processed
		/// </param>
		/// <param name="params">Processing paramaters.
		/// </param>
		/// <returns> true if any post-processing occurs, false otherwise.
		/// </returns>
		bool handlePostProcess(TreeElement node, System.String params_Renamed);
	}
}