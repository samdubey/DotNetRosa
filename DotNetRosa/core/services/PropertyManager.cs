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
using IPropertyRules = org.javarosa.core.services.properties.IPropertyRules;
using Property = org.javarosa.core.services.properties.Property;
//UPGRADE_TODO: The type 'org.javarosa.core.services.storage.IStorageUtilityIndexed' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using IStorageUtilityIndexed = org.javarosa.core.services.storage.IStorageUtilityIndexed;
using StorageFullException = org.javarosa.core.services.storage.StorageFullException;
using StorageManager = org.javarosa.core.services.storage.StorageManager;
namespace org.javarosa.core.services
{
	
	/// <summary> PropertyManager is a class that is used to set and retrieve name/value pairs
	/// from persistent storage.
	/// 
	/// Which properties are allowed, and what they can be set to, can be specified by an implementation of
	/// the IPropertyRules interface, any number of which can be registered with a property manager. All 
	/// property rules are inclusive, and can only increase the number of potential properties or property
	/// values.
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public class PropertyManager : IPropertyManager
	{
		
		///// manage global property manager /////
		
		private static IPropertyManager instance; //a global instance of the property manager
		
		public static void  setPropertyManager(IPropertyManager pm)
		{
			instance = pm;
		}
		
		public static void  initDefaultPropertyManager()
		{
			StorageManager.registerStorage(PropertyManager.STORAGE_KEY, typeof(Property));
			setPropertyManager(new PropertyManager());
		}
		
		public static IPropertyManager _()
		{
			if (instance == null)
			{
				initDefaultPropertyManager();
			}
			return instance;
		}
		
		//////////////////////////////////////////
		
		/// <summary> The name for the Persistent storage utility name</summary>
		public const System.String STORAGE_KEY = "PROPERTY";
		
		/// <summary> The list of rules </summary>
		private System.Collections.ArrayList rulesList;
		
		/// <summary> The persistent storage utility</summary>
		private IStorageUtilityIndexed properties;
		
		/// <summary> Constructor for this PropertyManager</summary>
		public PropertyManager()
		{
			this.properties = (IStorageUtilityIndexed) StorageManager.getStorage(STORAGE_KEY);
			rulesList = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
		}
		
		/// <summary> Retrieves the singular property specified, as long as it exists in one of the current rulesets
		/// 
		/// </summary>
		/// <param name="propertyName">the name of the property being retrieved
		/// </param>
		/// <returns> The String value of the property specified if it exists, is singluar, and is in one the current
		/// rulessets. null if the property is denied by the current ruleset, or is a vector.
		/// </returns>
		public virtual System.String getSingularProperty(System.String propertyName)
		{
			System.String retVal = null;
			if ((rulesList.Count == 0 || checkPropertyAllowed(propertyName)))
			{
				System.Collections.ArrayList value_Renamed = getValue(propertyName);
				if (value_Renamed != null && value_Renamed.Count == 1)
				{
					retVal = ((System.String) value_Renamed[0]);
				}
			}
			if (retVal == null)
			{
				//#if debug.output==verbose
				System.Console.Out.WriteLine("Warning: Singular property request failed for property " + propertyName);
				//#endif
			}
			return retVal;
		}
		
		
		/// <summary> Retrieves the property specified, as long as it exists in one of the current rulesets
		/// 
		/// </summary>
		/// <param name="propertyName">the name of the property being retrieved
		/// </param>
		/// <returns> The String value of the property specified if it exists, and is the current ruleset, if one exists.
		/// null if the property is denied by the current ruleset.
		/// </returns>
		public virtual System.Collections.ArrayList getProperty(System.String propertyName)
		{
			if (rulesList.Count == 0)
			{
				return getValue(propertyName);
			}
			else
			{
				if (checkPropertyAllowed(propertyName))
				{
					return getValue(propertyName);
				}
				else
				{
					return null;
				}
			}
		}
		
		/// <summary> Sets the given property to the given string value, if both are allowed by any existing ruleset</summary>
		/// <param name="propertyName">The property to be set
		/// </param>
		/// <param name="propertyValue">The value that the property will be set to
		/// </param>
		public virtual void  setProperty(System.String propertyName, System.String propertyValue)
		{
			System.Collections.ArrayList wrapper = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			wrapper.Add(propertyValue);
			setProperty(propertyName, wrapper);
		}
		
		/// <summary> Sets the given property to the given vector value, if both are allowed by any existing ruleset</summary>
		/// <param name="propertyName">The property to be set
		/// </param>
		/// <param name="propertyValue">The value that the property will be set to
		/// </param>
		public virtual void  setProperty(System.String propertyName, System.Collections.ArrayList propertyValue)
		{
			System.Collections.ArrayList oldValue = getProperty(propertyName);
			if (oldValue != null && vectorEquals(oldValue, propertyValue))
			{
				//No point in redundantly setting values!
				return ;
			}
			if (rulesList.Count == 0)
			{
				writeValue(propertyName, propertyValue);
			}
			else
			{
				bool valid = true;
				System.Collections.IEnumerator en = propertyValue.GetEnumerator();
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				while (en.MoveNext())
				{
					// RL - checkPropertyAllowed is implicit in checkValueAllowed
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					if (!checkValueAllowed(propertyName, (System.String) en.Current))
					{
						valid = false;
					}
				}
				if (valid)
				{
					writeValue(propertyName, propertyValue);
					notifyChanges(propertyName);
				}
				//#if debug.output==verbose
				else
				{
					System.Console.Out.WriteLine("Property Manager: Unable to write value (" + SupportClass.CollectionToString(propertyValue) + ") to " + propertyName);
				}
				//#endif
			}
		}
		
		private bool vectorEquals(System.Collections.ArrayList v1, System.Collections.ArrayList v2)
		{
			if (v1.Count != v2.Count)
			{
				return false;
			}
			else
			{
				for (int i = 0; i < v1.Count; ++i)
				{
					if (!v1[i].Equals(v2[i]))
					{
						return false;
					}
				}
			}
			return true;
		}
		
		/// <summary> Retrieves the set of rules being used by this property manager if any exist.
		/// 
		/// </summary>
		/// <returns> The rulesets being used by this property manager
		/// </returns>
		public virtual System.Collections.ArrayList getRules()
		{
			return rulesList;
		}
		
		/// <summary> Sets the rules that should be used by this PropertyManager, removing any other
		/// existing rules sets.
		/// 
		/// </summary>
		/// <param name="rules">The rules to be used. 
		/// </param>
		public virtual void  setRules(IPropertyRules rules)
		{
			this.rulesList.Clear();
			this.rulesList.Add(rules);
		}
		
		/// <summary> Adds a set of rules to be used by this PropertyManager.
		/// Note that rules sets are inclusive, they add new possible
		/// values, never remove possible values.
		/// 
		/// </summary>
		/// <param name="rules">The set of rules to be added to the permitted list
		/// </param>
		public virtual void  addRules(IPropertyRules rules)
		{
			if (rules != null)
			{
				this.rulesList.Add(rules);
			}
		}
		
		/// <summary> Checks that a property is permitted to exist by any of the existing rules sets
		/// 
		/// </summary>
		/// <param name="propertyName">The name of the property to be set
		/// </param>
		/// <returns> true if the property is permitted to store values. false otherwise
		/// </returns>
		public virtual bool checkPropertyAllowed(System.String propertyName)
		{
			if (rulesList.Count == 0)
			{
				return true;
			}
			else
			{
				bool allowed = false;
				System.Collections.IEnumerator en = rulesList.GetEnumerator();
				//We're fine if we return true, inclusive rules sets
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				while (en.MoveNext() && !allowed)
				{
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					IPropertyRules rules = (IPropertyRules) en.Current;
					if (rules.checkPropertyAllowed(propertyName))
					{
						allowed = true;
					}
				}
				return allowed;
			}
		}
		
		/// <summary> Checks that a property is allowed to store a certain value.
		/// 
		/// </summary>
		/// <param name="propertyName">The name of the property to be set
		/// </param>
		/// <param name="propertyValue">The value to be stored in the given property
		/// </param>
		/// <returns> true if the property given is allowed to be stored. false otherwise.
		/// </returns>
		public virtual bool checkValueAllowed(System.String propertyName, System.String propertyValue)
		{
			if (rulesList.Count == 0)
			{
				return true;
			}
			else
			{
				bool allowed = false;
				System.Collections.IEnumerator en = rulesList.GetEnumerator();
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				while (en.MoveNext() && !allowed)
				{
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					IPropertyRules rules = (IPropertyRules) en.Current;
					if (rules.checkPropertyAllowed(propertyName))
					{
						if (rules.checkValueAllowed(propertyName, propertyValue))
						{
							allowed = true;
						}
					}
				}
				return allowed;
			}
		}
		
		/// <summary> Identifies the property rules set that the property belongs to, and notifies
		/// it about the property change.
		/// 
		/// </summary>
		/// <param name="property">The property that has been changed 
		/// </param>
		private void  notifyChanges(System.String property)
		{
			if (rulesList.Count == 0)
			{
				return ;
			}
			
			bool notified = false;
			System.Collections.IEnumerator rules = rulesList.GetEnumerator();
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			while (rules.MoveNext() && !notified)
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				IPropertyRules therules = (IPropertyRules) rules.Current;
				if (therules.checkPropertyAllowed(property))
				{
					therules.handlePropertyChanges(property);
				}
			}
		}
		
		public virtual System.Collections.ArrayList getValue(System.String name)
		{
			try
			{
				Property p = (Property) properties.getRecordForValue("NAME", name);
				return p.value_Renamed;
			}
			catch (System.ArgumentOutOfRangeException nsee)
			{
				return null;
			}
		}
		
		public virtual void  writeValue(System.String propertyName, System.Collections.ArrayList value_Renamed)
		{
			Property theProp = new Property();
			theProp.name = propertyName;
			theProp.value_Renamed = value_Renamed;
			
			System.Collections.ArrayList IDs = properties.getIDsForValue("NAME", propertyName);
			if (IDs.Count == 1)
			{
				theProp.ID = ((System.Int32) IDs[0]);
			}
			
			try
			{
				properties.write(theProp);
			}
			catch (StorageFullException e)
			{
				throw new System.SystemException("uh-oh, storage full [properties]"); //TODO: handle this
			}
		}
	}
}