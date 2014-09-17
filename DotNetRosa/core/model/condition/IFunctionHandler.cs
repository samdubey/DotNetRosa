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
namespace org.javarosa.core.model.condition
{
	
	public interface IFunctionHandler
	{
		/// <returns> The name of function being handled
		/// </returns>
		System.String Name
		{
			get;
			
		}
		/// <returns> Vector of allowed prototypes for this function. Each prototype is
		/// an array of Class, corresponding to the types of the expected
		/// arguments. The first matching prototype is used.
		/// </returns>
		System.Collections.ArrayList Prototypes
		{
			get;
			
		}
		
		/// <returns> true if this handler should be fed the raw argument list if no
		/// prototype matches it
		/// </returns>
		bool rawArgs();
		
		/// <returns> true if the result of this handler depends on some dynamic data
		/// source, and the expression cannot be pre-computed before the
		/// question is reached (un-supported)
		/// 
		/// </returns>
		bool realTime();
		
		/// <summary> Evaluate the function</summary>
		System.Object eval(System.Object[] args, EvaluationContext ec);
	}
}