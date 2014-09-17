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
using PropertyManager = org.javarosa.core.services.PropertyManager;
using Localization = org.javarosa.core.services.locale.Localization;
using Localizer = org.javarosa.core.services.locale.Localizer;
namespace org.javarosa.core.services.properties
{
	
	/// <summary> A set of rules governing the allowable properties for JavaRosa's
	/// core funtionality. 
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public class JavaRosaPropertyRules : IPropertyRules
	{
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		internal System.Collections.Hashtable rules;
		
		internal System.Collections.ArrayList readOnlyProperties;
		
		public const System.String DEVICE_ID_PROPERTY = "DeviceID";
		public const System.String CURRENT_LOCALE = "cur_locale";
		
		public const System.String LOGS_ENABLED = "logenabled";
		
		public const System.String LOGS_ENABLED_YES = "Enabled";
		public const System.String LOGS_ENABLED_NO = "Disabled";
		
		/// <summary>The expected compliance version for the OpenRosa API set *</summary>
		public const System.String OPENROSA_API_LEVEL = "jr_openrosa_api";
		
		/// <summary> Creates the JavaRosa set of property rules</summary>
		public JavaRosaPropertyRules()
		{
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			rules = new System.Collections.Hashtable();
			readOnlyProperties = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			
			//DeviceID Property
			rules[DEVICE_ID_PROPERTY] = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList logs = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			logs.Add(LOGS_ENABLED_NO);
			logs.Add(LOGS_ENABLED_YES);
			rules[LOGS_ENABLED] = logs;
			
			rules[CURRENT_LOCALE] = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			
			rules[OPENROSA_API_LEVEL] = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			
			readOnlyProperties.Add(DEVICE_ID_PROPERTY);
			readOnlyProperties.Add(OPENROSA_API_LEVEL);
		}
		
		/// <summary>(non-Javadoc)</summary>
		/// <seealso cref="org.javarosa.core.services.properties.IPropertyRules.allowableValues(String)">
		/// </seealso>
		public virtual System.Collections.ArrayList allowableValues(System.String propertyName)
		{
			if (CURRENT_LOCALE.Equals(propertyName))
			{
				Localizer l = Localization.GlobalLocalizerAdvanced;
				System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				System.String[] locales = l.AvailableLocales;
				for (int i = 0; i < locales.Length; ++i)
				{
					v.Add(locales[i]);
				}
				return v;
			}
			//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
			return (System.Collections.ArrayList) rules[propertyName];
		}
		
		/// <summary>(non-Javadoc)</summary>
		/// <seealso cref="org.javarosa.core.services.properties.IPropertyRules.checkValueAllowed(String, String)">
		/// </seealso>
		public virtual bool checkValueAllowed(System.String propertyName, System.String potentialValue)
		{
			if (CURRENT_LOCALE.Equals(propertyName))
			{
				return Localization.GlobalLocalizerAdvanced.hasLocale(potentialValue);
			}
			//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
			System.Collections.ArrayList prop = ((System.Collections.ArrayList) rules[propertyName]);
			if (prop.Count != 0)
			{
				//Check whether this is a dynamic property
				if (prop.Count == 1 && checkPropertyAllowed((System.String) prop[0]))
				{
					// If so, get its list of available values, and see whether the potentival value is acceptable.
					return ((System.Collections.ArrayList) PropertyManager._().getProperty((System.String) prop[0])).Contains(potentialValue);
				}
				else
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					return ((System.Collections.ArrayList) rules[propertyName]).Contains(potentialValue);
				}
			}
			else
				return true;
		}
		
		/// <summary>(non-Javadoc)</summary>
		/// <seealso cref="org.javarosa.core.services.properties.IPropertyRules.allowableProperties()">
		/// </seealso>
		public virtual System.Collections.ArrayList allowableProperties()
		{
			System.Collections.ArrayList propList = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			//UPGRADE_TODO: Method 'java.util.HashMap.keySet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapkeySet'"
			System.Collections.IEnumerator iter = new System.Collections.ArrayList(new SupportClass.HashSetSupport(rules.Keys)).GetEnumerator();
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			while (iter.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				propList.Add(iter.Current);
			}
			return propList;
		}
		
		/// <summary>(non-Javadoc)</summary>
		/// <seealso cref="org.javarosa.core.services.properties.IPropertyRules.checkPropertyAllowed)">
		/// </seealso>
		public virtual bool checkPropertyAllowed(System.String propertyName)
		{
			//UPGRADE_TODO: Method 'java.util.HashMap.keySet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapkeySet'"
			System.Collections.IEnumerator iter = new System.Collections.ArrayList(new SupportClass.HashSetSupport(rules.Keys)).GetEnumerator();
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			while (iter.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				if (propertyName.Equals(iter.Current))
				{
					return true;
				}
			}
			return false;
		}
		
		/// <summary>(non-Javadoc)</summary>
		/// <seealso cref="org.javarosa.core.services.properties.IPropertyRules.checkPropertyUserReadOnly)">
		/// </seealso>
		public virtual bool checkPropertyUserReadOnly(System.String propertyName)
		{
			return readOnlyProperties.Contains(propertyName);
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.services.properties.IPropertyRules#getHumanReadableDescription(java.lang.String)
		*/
		public virtual System.String getHumanReadableDescription(System.String propertyName)
		{
			if (DEVICE_ID_PROPERTY.Equals(propertyName))
			{
				return "Unique Device ID";
			}
			else if (LOGS_ENABLED.Equals(propertyName))
			{
				return "Device Logging";
			}
			else if (CURRENT_LOCALE.Equals(propertyName))
			{
				return Localization.get_Renamed("settings.language");
			}
			else if (OPENROSA_API_LEVEL.Equals(propertyName))
			{
				return "OpenRosa API Level";
			}
			return propertyName;
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.services.properties.IPropertyRules#getHumanReadableValue(java.lang.String, java.lang.String)
		*/
		public virtual System.String getHumanReadableValue(System.String propertyName, System.String value_Renamed)
		{
			if (CURRENT_LOCALE.Equals(propertyName))
			{
				System.String name = Localization.GlobalLocalizerAdvanced.getText(value_Renamed);
				if (name != null)
				{
					return name;
				}
			}
			return value_Renamed;
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.services.properties.IPropertyRules#handlePropertyChanges(java.lang.String)
		*/
		public virtual void  handlePropertyChanges(System.String propertyName)
		{
			if (CURRENT_LOCALE.Equals(propertyName))
			{
				System.String locale = PropertyManager._().getSingularProperty(propertyName);
				Localization.Locale = locale;
			}
		}
	}
}