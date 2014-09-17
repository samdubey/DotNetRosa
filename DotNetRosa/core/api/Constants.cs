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
namespace org.javarosa.core.api
{
	
	/// <summary> This file is a set of constants for the JavaRosa Core platform.
	/// 
	/// It should contain constants only pertaining to core usage of JavaRosa's core
	/// classes, including Module and Shell return codes, and indexes for the core
	/// context.
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public class Constants
	{
		/// <summary> Activity Return Codes</summary>
		public const System.String ACTIVITY_CANCEL = "activity_cancel";
		public const System.String ACTIVITY_COMPLETE = "activity_complete";
		public const System.String ACTIVITY_ERROR = "activity_error";
		public const System.String ACTIVITY_SUSPEND = "activity_suspend";
		public const System.String ACTIVITY_NEEDS_RESOLUTION = "activity_needs_resolution";
		
		public const System.String USER_KEY = "username";
		public const System.String USER_ID_KEY = "userid";
		
		/// <summary> Return arg codes</summary>
		public const System.String ACTIVITY_LAUNCH_KEY = "activity_to_launch";
		public const System.String RETURN_ARG_KEY = "return_arg";
		public const System.String RETURN_ARG_TYPE_KEY = "return_arg_type";
		public const System.String RETURN_ARG_TYPE_DATA_POINTER = "data_pointer";
		public const System.String RETURN_ARG_TYPE_DATA_POINTER_LIST = "data_pointer_list";
		
		/// <summary> Activity codes</summary>
		public const System.String ACTIVITY_TYPE_GET_IMAGES = "get_images";
		public const System.String ACTIVITY_TYPE_GET_AUDIO = "get_audio";
		
		/// <summary> Service codes</summary>
		public const System.String TRANSPORT_MANAGER = "Transport Manager";
		public const System.String PROPERTY_MANAGER = "Property Manager";
	}
}