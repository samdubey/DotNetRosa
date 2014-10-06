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
namespace org.javarosa.core.util
{
	
	/// <summary> </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public class ArrayUtilities
	{
		public ArrayUtilities()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			
			for(E e: a)
			{
				if (b.indexOf(e) != - 1)
				{
					return e;
				}
			}
			return null;
			if (a == null)
			{
				return null;
			}
			
			List< E > b = new List< E >();
			
			for(E e: a)
			{
				b.addElement(e);
			}
			return b;
			int i = 0;
			
			for(E e: v)
			{
				a[i++] = e;
			}
			return a;
			
			List< E > v = new List< E >();
			
			for(E e: a)
			{
				v.addElement(e);
			}
			return v;
		}
		public static bool arraysEqual(System.Object[] array1, System.Object[] array2)
		{
			if (array1.Length != array2.Length)
			{
				return false;
			}
			
			for (int i = 0; i < array1.Length; ++i)
			{
				if (!array1[i].Equals(array2[i]))
				{
					return false;
				}
			}
			return true;
		}
		
		public static bool arraysEqual(sbyte[] array1, sbyte[] array2)
		{
			if (array1.Length != array2.Length)
			{
				return false;
			}
			
			for (int i = 0; i < array1.Length; ++i)
			{
				if (array1[i] != array2[i])
				{
					return false;
				}
			}
			return true;
		}
		
		
		public static bool arraysEqual(char[] array1, int a1start, char[] array2, int a2start)
		{
			if (array1.Length - a1start != array2.Length - a2start)
			{
				return false;
			}
			
			for (int i = 0; i < array1.Length - a1start; ++i)
			{
				if (array1[i + a1start] != array2[i + a2start])
				{
					return false;
				}
			}
			return true;
		}
		
		/// <summary> Find a single intersecting element common to two lists, or null if none
		/// exists. Note that no unique condition will be reported if there are multiple
		/// elements which intersect, so this should likely only be used if the possible
		/// size of intersection is 0 or 1
		/// 
		/// </summary>
		/// <param name="a">
		/// </param>
		/// <param name="b">
		/// </param>
		/// <returns>
		/// </returns>
		
		public static < E > E intersectSingle(List< E > a, List< E > b)
		
		
		public static < E > List< E > vectorCopy(List< E > a)
		
		
		public static < E > E [] copyIntoArray(List< E > v, E [] a)
		
		
		public static < E > List< E > toVector(E [] a)
	}
}