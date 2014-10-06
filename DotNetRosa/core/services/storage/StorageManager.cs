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
using Logger = org.javarosa.core.services.Logger;
using SerializationWrapper = org.javarosa.core.services.storage.WrappingStorageUtility.SerializationWrapper;
namespace org.javarosa.core.services.storage
{
	
	/// <summary> Manages StorageProviders for JavaRosa, which maintain persistent
	/// data on a device.
	/// 
	/// Largely derived from Cell Life's RMSManager
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public class StorageManager
	{
		
		
		private static HashMap < String, IStorageUtility > storageRegistry = new HashMap < String, IStorageUtility >();
		private static IStorageFactory storageFactory;
		
		/// <summary> Attempts to set the storage factory for the current environment. Will fail silently
		/// if a storage factory has already been set. Should be used by default environment.
		/// 
		/// </summary>
		/// <param name="fact">An available storage factory.
		/// </param>
		public static void  setStorageFactory(IStorageFactory fact)
		{
			StorageManager.setStorageFactory(fact, false);
		}
		
		/// <summary> Attempts to set the storage factory for the current environment and fails and dies if there
		/// is already a storage factory set if specified. Should be used by actual applications who need to use
		/// a specific storage factory and shouldn't tolerate being pre-empted. 
		/// 
		/// </summary>
		/// <param name="fact">An available storage factory.
		/// </param>
		/// <param name="mustWork">true if it is intolerable for another storage factory to have been set. False otherwise
		/// </param>
		public static void  setStorageFactory(IStorageFactory fact, bool mustWork)
		{
			if (storageFactory == null)
			{
				storageFactory = fact;
			}
			else
			{
				if (mustWork)
				{
					Logger.die("A Storage Factory had already been set when storage factory " + fact.GetType().FullName + " attempted to become the only storage factory", new System.SystemException("Duplicate Storage Factory set"));
				}
				else
				{
					//Not an issue
				}
			}
		}
		
		public static void  registerStorage(System.String key, System.Type type)
		{
			registerStorage(key, key, type);
		}
		
		public static void  registerStorage(System.String storageKey, System.String storageName, System.Type type)
		{
			if (storageFactory == null)
			{
				throw new System.SystemException("No storage factory has been set; I don't know what kind of storage utility to create. Either set a storage factory, or register your StorageUtilitys directly.");
			}
			
			registerStorage(storageKey, storageFactory.newStorage(storageName, type));
		}
		
		/// <summary> It is strongly, strongly advised that you do not register storage in this way.
		/// 
		/// </summary>
		/// <param name="key">
		/// </param>
		/// <param name="storage">
		/// </param>
		public static void  registerStorage(System.String key, IStorageUtility storage)
		{
			storageRegistry.put(key, storage);
		}
		
		public static void  registerWrappedStorage(System.String key, System.String storeName, WrappingStorageUtility.SerializationWrapper wrapper)
		{
			StorageManager.registerStorage(key, new WrappingStorageUtility(storeName, wrapper, storageFactory));
		}
		
		public static IStorageUtility getStorage(System.String key)
		{
			if (storageRegistry.containsKey(key))
			{
				return (IStorageUtility) storageRegistry.get_Renamed(key);
			}
			else
			{
				throw new System.SystemException("No storage utility has been registered to handle \"" + key + "\"; you must register one first with StorageManager.registerStorage()");
			}
		}
		
		public static void  repairAll()
		{
			
			for(IStorageUtility storageUtility: storageRegistry.values())
			{
				storageUtility.repair();
			}
		}
		
		public static System.String[] listRegisteredUtilities()
		{
			System.String[] returnVal = new System.String[storageRegistry.size()];
			int i = 0;
			
			for(String key: storageRegistry.keySet())
			{
				returnVal[i] = key;
				i++;
			}
			return returnVal;
		}
		
		public static void  halt()
		{
			
			for(IStorageUtility storageUtility: storageRegistry.values())
			{
				storageUtility.close();
			}
		}
	}
}