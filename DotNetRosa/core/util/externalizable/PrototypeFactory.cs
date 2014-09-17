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
using MD5 = org.javarosa.core.util.MD5;
using PrefixTree = org.javarosa.core.util.PrefixTree;
namespace org.javarosa.core.util.externalizable
{
	
	public class PrototypeFactory
	{
		public const int CLASS_HASH_SIZE = 4;
		
		private System.Collections.ArrayList classes;
		private System.Collections.ArrayList hashes;
		
		//lazy evaluation
		private PrefixTree classNames;
		private bool initialized;
		
		public PrototypeFactory():this(null)
		{
		}
		
		public PrototypeFactory(PrefixTree classNames)
		{
			this.classNames = classNames;
			initialized = false;
		}
		
		private void  lazyInit()
		{
			initialized = true;
			
			classes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			hashes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			
			addDefaultClasses();
			
			if (classNames != null)
			{
				System.Collections.ArrayList vClasses = classNames.getStrings();
				
				for (int i = 0; i < vClasses.Count; i++)
				{
					System.String name = (System.String) vClasses[i];
					try
					{
						//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
						addClass(System.Type.GetType(name));
					}
					//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
					catch (System.Exception cnfe)
					{
						throw new CannotCreateObjectException(name + ": not found");
					}
				}
				classNames = null;
			}
		}
		
		private void  addDefaultClasses()
		{
			System.Type[] baseTypes = new System.Type[]{typeof(System.Object), typeof(System.Int32), typeof(System.Int64), typeof(System.Int16), typeof(System.SByte), typeof(System.Char), typeof(System.Boolean), typeof(System.Single), typeof(System.Double), typeof(System.String), typeof(System.DateTime)};
			
			for (int i = 0; i < baseTypes.Length; i++)
			{
				addClass(baseTypes[i]);
			}
		}
		
		public virtual void  addClass(System.Type c)
		{
			if (!initialized)
			{
				lazyInit();
			}
			
			sbyte[] hash = getClassHash(c);
			
			if (compareHash(hash, ExtWrapTagged.WRAPPER_TAG))
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new System.ApplicationException("Hash collision! " + c.FullName + " and reserved wrapper tag");
			}
			
			System.Type d = getClass(hash);
			if (d != null && d != c)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new System.ApplicationException("Hash collision! " + c.FullName + " and " + d.FullName);
			}
			
			classes.Add(c);
			hashes.Add(SupportClass.ToByteArray(hash));
		}
		
		public virtual System.Type getClass(sbyte[] hash)
		{
			if (!initialized)
			{
				lazyInit();
			}
			
			for (int i = 0; i < classes.Count; i++)
			{
				if (compareHash(hash, (sbyte[]) hashes[i]))
				{
					return (System.Type) classes[i];
				}
			}
			
			return null;
		}
		
		public virtual System.Object getInstance(sbyte[] hash)
		{
			return getInstance(getClass(hash));
		}
		
		public static System.Object getInstance(System.Type c)
		{
			try
			{
				//UPGRADE_TODO: Method 'java.lang.Class.newInstance' was converted to 'System.Activator.CreateInstance' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangClassnewInstance'"
				return System.Activator.CreateInstance(c);
			}
			catch (System.UnauthorizedAccessException iae)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new CannotCreateObjectException(c.FullName + ": not accessible or no empty constructor");
			}
			//UPGRADE_NOTE: Exception 'java.lang.InstantiationException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new CannotCreateObjectException(c.FullName + ": not instantiable");
			}
		}
		
		public static sbyte[] getClassHash(System.Type type)
		{
			sbyte[] hash = new sbyte[CLASS_HASH_SIZE];
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			sbyte[] md5 = MD5.hash(SupportClass.ToSByteArray(SupportClass.ToByteArray(type.FullName))); //add support for a salt, in case of collision?
			
			for (int i = 0; i < hash.Length; i++)
				hash[i] = md5[i];
			sbyte[] badHash = new sbyte[]{0, 4, 78, 97};
			if (PrototypeFactory.compareHash(badHash, hash))
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine("BAD CLASS: " + type.FullName);
			}
			
			return hash;
		}
		
		public static bool compareHash(sbyte[] a, sbyte[] b)
		{
			if (a.Length != b.Length)
			{
				return false;
			}
			
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
					return false;
			}
			
			return true;
		}
	}
}