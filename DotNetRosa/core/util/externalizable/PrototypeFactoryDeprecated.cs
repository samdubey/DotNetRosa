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
using Map = org.javarosa.core.util.Map;
namespace org.javarosa.core.util.externalizable
{
	
	/// <summary> The PrototypeFactory is a factory class for instantiating classes
	/// based on their class name. This class fills a hole created by J2ME's
	/// lack of reflection. 
	/// 
	/// The most common use of PrototypeFactories in JavaRoa is to instantiate
	/// objects in order to deserialize them from RMS.
	/// 
	/// Note that due to the nature of instantiating classes dynamically,
	/// prototypes registered with this class must maintain a constructor with
	/// no arguments.
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public class PrototypeFactoryDeprecated
	{
		public Map prototypes = new Map();
		
		/// <summary> Adds a new class to be able to retrieve instances of</summary>
		/// <param name="name">The name of the prototype. Generally prototype.getClass().getName()
		/// </param>
		/// <param name="prototype">The class object to be used for instantiation. Should be a class
		/// with a constructor that takes 0 arguments.
		/// </param>
		public virtual void  addNewPrototype(System.String name, System.Type prototype)
		{
			prototypes.put(name, prototype);
		}
		
		/// <param name="prototypeName">The name of the prototype to be instantiated
		/// </param>
		/// <returns> a new object of the type linked to the name given in this factory. Null
		/// if the name is not associated with any class in this factory.
		/// </returns>
		/// <throws>  IllegalAccessException If the empty constructor of the class given is not </throws>
		/// <summary> allowed to be accessed.
		/// </summary>
		/// <throws>  InstantiationException </throws>
		public virtual System.Object getNewInstance(System.String prototypeName)
		{
			if (prototypes.get_Renamed(prototypeName) == null)
			{
				return null;
			}
			try
			{
				//UPGRADE_TODO: Method 'java.lang.Class.newInstance' was converted to 'System.Activator.CreateInstance' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangClassnewInstance'"
				return System.Activator.CreateInstance(((System.Type) prototypes.get_Renamed(prototypeName)));
			}
			//UPGRADE_NOTE: Exception 'java.lang.InstantiationException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				throw new CannotCreateObjectException(prototypeName);
			}
			catch (System.UnauthorizedAccessException e)
			{
				throw new CannotCreateObjectException(prototypeName);
			}
		}
	}
}