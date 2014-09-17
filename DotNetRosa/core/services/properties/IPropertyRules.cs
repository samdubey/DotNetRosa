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
namespace org.javarosa.core.services.properties
{
	
	/// <summary> The IPropertyRules interface is used to describe a set of rules for what properties are allowed for a given
	/// property manager, and what values are are acceptable for a given property.
	/// 
	/// Essentially, individual properties should be considered to be actual persistent storage
	/// for a device's specific configuration, and a set of property rules should be considered
	/// to be the non-persistent meta-data surrounding what those configurations mean.
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public interface IPropertyRules
	{
		/// <summary> Identifies what values are acceptable for a given property
		/// 
		/// </summary>
		/// <param name="propertyName">The name of the property that is being identified
		/// </param>
		/// <returns> A Vector containing all of the values that this property may be set to
		/// </returns>
		System.Collections.ArrayList allowableValues(System.String propertyName);
		
		/// <summary> Identifies whether the given value is an acceptable for a property.
		/// 
		/// </summary>
		/// <param name="propertyName">The name of the property that is being identified
		/// </param>
		/// <param name="potentialValue">The value that is being tested 
		/// 
		/// </param>
		/// <returns> True if the property specified may be set to potentialValue, False otherwise
		/// </returns>
		bool checkValueAllowed(System.String propertyName, System.String potentialValue);
		
		/// <summary> Identifies what properties are acceptable for this rules set
		/// 
		/// </summary>
		/// <returns> A Vector containing all of the properties that may be set
		/// </returns>
		System.Collections.ArrayList allowableProperties();
		
		/// <summary> Identifies whether the given property is usable
		/// 
		/// </summary>
		/// <param name="propertyName">The name of the property that is being tested
		/// 
		/// </param>
		/// <returns> True if the property specified may used. False otherwise
		/// </returns>
		bool checkPropertyAllowed(System.String propertyName);
		
		/// <summary> Identifies whether the property should be revealed to users. Note
		/// that this does not govern whether the value can be set, simply
		/// whether it should be set manually by users.
		/// 
		/// </summary>
		/// <param name="propertyName">The name of the property that is being tested
		/// 
		/// </param>
		/// <returns> True if the property specified may not be modified by the user. false otherwise
		/// </returns>
		bool checkPropertyUserReadOnly(System.String propertyName);
		
		/// <summary> Returns a human readable string representing the description of a
		/// property.
		/// 
		/// </summary>
		/// <param name="propertyName">The name of the property to be described
		/// </param>
		/// <returns> A string that describes the meaning of the property name
		/// </returns>
		System.String getHumanReadableDescription(System.String propertyName);
		
		/// <summary> Returns a human readable string representing the value of a specific
		/// property. This allows multiple choice answers to be stored in a concise
		/// format, while offering a standardized way to present those options to
		/// a user.
		/// 
		/// </summary>
		/// <param name="propertyName">The name of the property whose value is to be 
		/// interpreted.
		/// </param>
		/// <param name="value">The value to be interpreted as a String
		/// </param>
		/// <returns> A string representing the passed in value that can be parsed by 
		/// a user to determine what its significance is.
		/// </returns>
		System.String getHumanReadableValue(System.String propertyName, System.String value_Renamed);
		
		/// <summary> Handles any state changes that would be required upon a specific value
		/// being changed.
		/// 
		/// </summary>
		/// <param name="propertyName">The name of the property that has changed
		/// </param>
		void  handlePropertyChanges(System.String propertyName);
	}
}