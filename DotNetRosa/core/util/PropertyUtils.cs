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
namespace org.javarosa.core.util
{
	
	public class PropertyUtils
	{
		
		//need 'addpropery' too.
		public static System.String initializeProperty(System.String propName, System.String defaultValue)
		{
			System.Collections.ArrayList propVal = PropertyManager._().getProperty(propName);
			if (propVal == null || propVal.Count == 0)
			{
				propVal = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				propVal.Add(defaultValue);
				PropertyManager._().setProperty(propName, propVal);
				//#if debug.output==verbose
				System.Console.Out.WriteLine("No default value for [" + propName + "]; setting to [" + defaultValue + "]"); // debug
				//#endif
				return defaultValue;
			}
			return (System.String) propVal[0];
		}
		
		
		/// <summary> Generate an RFC 1422 Version 4 UUID.
		/// 
		/// </summary>
		/// <returns> a uuid
		/// </returns>
		public static System.String genUUID()
		{
			return randHex(8) + "-" + randHex(4) + "-4" + randHex(3) + "-" + System.Convert.ToString(8 + MathUtils.Rand.Next(4), 16) + randHex(3) + "-" + randHex(12);
		}
		
		/// <summary> Create a globally unique identifier string in no particular format
		/// with len characters of randomness.
		/// 
		/// </summary>
		/// <param name="len">The length of the string identifier requested.
		/// </param>
		/// <returns> A string containing len characters of random data.
		/// </returns>
		public static System.String genGUID(int len)
		{
			StringBuilder b = new StringBuilder();
			for (int i = 0; i < len; i++)
			{
				// 25 == 128 bits of entropy
				b.append(System.Convert.ToString(MathUtils.Rand.Next(36), 36));
			}
			return b.toString().toUpperCase();
		}
		
		public static System.String randHex(int len)
		{
			StringBuilder b = new StringBuilder();
			System.Random r = MathUtils.Rand;
			for (int i = 0; i < len; ++i)
			{
				b.append(System.Convert.ToString(r.Next(16), 16));
			}
			return b.toString();
		}
		
		public static System.String trim(System.String guid, int len)
		{
			return guid.Substring(0, (System.Math.Min(len, guid.Length)) - (0));
		}
	}
}