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
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapList = org.javarosa.core.util.externalizable.ExtWrapList;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.util
{
	
	/// <summary> A Map is a data object that maintains a map from one set of data
	/// objects to another. This data object is superior to a Hashtable
	/// in instances where O(1) lookups are not a priority, due to its
	/// smaller memory footprint.
	/// 
	/// Lookups in a map are accomplished in O(n) time.
	/// 
	/// 
	/// TODO: Figure out if this actually works anymore!
	/// (Is actually smaller in memory than a hashtable)
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public class Map
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassEnumeration' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassEnumeration : System.Collections.IEnumerator
		{
			public AnonymousClassEnumeration(Map enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Map enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private System.Object tempAuxObj;
			public virtual bool MoveNext()
			{
				bool result = hasMoreElements();
				if (result)
				{
					tempAuxObj = nextElement();
				}
				return result;
			}
			public virtual void  Reset()
			{
				tempAuxObj = null;
			}
			public virtual System.Object Current
			{
				get
				{
					return tempAuxObj;
				}
				
			}
			private Map enclosingInstance;
			public Map Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal int id = 0;
			
			//UPGRADE_NOTE: The equivalent of method 'java.util.Enumeration.hasMoreElements' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
			public virtual bool hasMoreElements()
			{
				return id < Enclosing_Instance.size();
			}
			
			//UPGRADE_NOTE: The equivalent of method 'java.util.Enumeration.nextElement' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
			public virtual System.Object nextElement()
			{
				int val = id;
				id++;
				return Enclosing_Instance.elementAt(val);
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassEnumeration1' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassEnumeration1 : System.Collections.IEnumerator
		{
			public AnonymousClassEnumeration1(Map enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Map enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private System.Object tempAuxObj;
			public virtual bool MoveNext()
			{
				bool result = hasMoreElements();
				if (result)
				{
					tempAuxObj = nextElement();
				}
				return result;
			}
			public virtual void  Reset()
			{
				tempAuxObj = null;
			}
			public virtual System.Object Current
			{
				get
				{
					return tempAuxObj;
				}
				
			}
			private Map enclosingInstance;
			public Map Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal int id = 0;
			
			//UPGRADE_NOTE: The equivalent of method 'java.util.Enumeration.hasMoreElements' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
			public virtual bool hasMoreElements()
			{
				return id < Enclosing_Instance.size();
			}
			
			//UPGRADE_NOTE: The equivalent of method 'java.util.Enumeration.nextElement' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
			public virtual System.Object nextElement()
			{
				int val = id;
				id++;
				return Enclosing_Instance.keyAt(val);
			}
		}
		private void  InitBlock()
		{
			;;
			
			bool sealed_Renamed = false;
			
			K[] keysSealed;
			V[] elementsSealed;
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'isEmpty'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		virtual public bool Empty
		{
			/* (non-Javadoc)
			* @see java.util.Hashtable#isEmpty()
			*/
			
			get
			{
				lock (this)
				{
					return this.size() > 0;
				}
			}
			
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		< K, V > extends OrderedMap < K, V >
		
		public Map()
		{
			InitBlock();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			keys = new Vector < K >();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			elements = new Vector < V >();
		}
		
		public Map(int sizeHint)
		{
			InitBlock();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			keys = new Vector < K >(sizeHint);
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			elements = new Vector < V >(sizeHint);
		}
		
		public Map(K[] keysSealed, V[] elementsSealed)
		{
			InitBlock();
			keys = null;
			elements = null;
			
			sealed_Renamed = true;
			this.keysSealed = keysSealed;
			this.elementsSealed = elementsSealed;
		}
		
		/// <summary> Places the key/value pair in this map. Any existing
		/// mapping keyed by the key parameter is removed.
		/// 
		/// </summary>
		/// <param name="key">
		/// </param>
		/// <param name="value">
		/// </param>
		public virtual V put(K key, V value_Renamed)
		{
			if (sealed_Renamed)
			{
				throw new System.SystemException("Trying to add element to sealed map");
			}
			if (containsKey(key))
			{
				remove(key);
			}
			keys.addElement(key);
			elements.addElement(value_Renamed);
			return value_Renamed;
		}
		
		public virtual int size()
		{
			if (!sealed_Renamed)
			{
				return keys.size();
			}
			else
			{
				return keysSealed.length;
			}
		}
		
		/// <param name="key">
		/// </param>
		/// <returns> The object bound to the given key, if one exists.
		/// null otherwise.
		/// </returns>
		public virtual V get_Renamed(System.Object key)
		{
			int index = getIndex((K) key);
			if (index == - 1)
			{
				return null;
			}
			if (!sealed_Renamed)
			{
				return elements.elementAt(index);
			}
			else
			{
				return elementsSealed[index];
			}
		}
		
		/// <summary> Removes any mapping from the given key</summary>
		/// <param name="key">
		/// </param>
		public virtual V remove(System.Object key)
		{
			if (sealed_Renamed)
			{
				throw new System.SystemException("Trying to remove element from sealed map");
			}
			int index = getIndex((K) key);
			if (index == - 1)
			{
				return null;
			}
			V v = this.elementAt(index);
			keys.removeElementAt(index);
			elements.removeElementAt(index);
			if (keys.size() != elements.size())
			{
				//This is _really bad_,
				throw new System.SystemException("Map in bad state!");
			}
			return v;
		}
		
		/// <summary> Removes all keys and values from this map.</summary>
		public virtual void  reset()
		{
			if (!sealed_Renamed)
			{
				keys.removeAllElements();
				elements.removeAllElements();
			}
			else
			{
				keysSealed = null;
				elementsSealed = null;
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				keys = new Vector < K >();
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				elements = new Vector < V >();
			}
		}
		
		/// <summary> Whether or not the key is bound in this map</summary>
		/// <param name="key">
		/// </param>
		/// <returns> True if there is an object bound to the given
		/// key in this map. False otherwise.
		/// </returns>
		public virtual bool containsKey(System.Object key)
		{
			return getIndex((K) key) != - 1;
		}
		
		private int getIndex(K key)
		{
			if (!sealed_Renamed)
			{
				for (int i = 0; i < keys.size(); ++i)
				{
					if (keys.elementAt(i).equals(key))
					{
						return i;
					}
				}
			}
			else
			{
				for (int i = 0; i < keysSealed.length; ++i)
				{
					if (keysSealed[i].equals(key))
					{
						return i;
					}
				}
			}
			return - 1;
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.OrderedHashtable#clear()
		*/
		public virtual void  clear()
		{
			this.reset();
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.OrderedHashtable#elementAt(int)
		*/
		public virtual V elementAt(int index)
		{
			if (!sealed_Renamed)
			{
				return elements.elementAt(index);
			}
			else
			{
				return elementsSealed[index];
			}
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.OrderedHashtable#elements()
		*/
		public virtual System.Collections.IEnumerator elements()
		{
			if (!sealed_Renamed)
			{
				return elements.elements();
			}
			else
			{
				return new AnonymousClassEnumeration(this);
			}
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.OrderedHashtable#indexOfKey(java.lang.Object)
		*/
		public virtual int indexOfKey(K key)
		{
			return this.getIndex(key);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.OrderedHashtable#keyAt(int)
		*/
		public virtual System.Object keyAt(int index)
		{
			if (!sealed_Renamed)
			{
				return keys.elementAt(index);
			}
			else
			{
				return keysSealed[index];
			}
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.OrderedHashtable#keys()
		*/
		public virtual System.Collections.IEnumerator keys()
		{
			if (!sealed_Renamed)
			{
				return keys.elements();
			}
			else
			{
				return new AnonymousClassEnumeration1(this);
			}
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.OrderedHashtable#removeAt(int)
		*/
		public virtual void  removeAt(int i)
		{
			remove(this.keyAt(i));
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.OrderedHashtable#toString()
		*/
		public override System.String ToString()
		{
			return "MAP!";
		}
		
		/* (non-Javadoc)
		* @see java.util.Hashtable#contains(java.lang.Object)
		*/
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'contains'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public virtual bool contains(System.Object value_Renamed)
		{
			lock (this)
			{
				if (!sealed_Renamed)
				{
					return elements.contains((V) value_Renamed);
				}
				else
				{
					for (int i = 0; i < elementsSealed.length; ++i)
					{
						if (elementsSealed[i].equals(value_Renamed))
						{
							return true;
						}
					}
				}
				return false;
			}
		}
		
		public virtual void  seal()
		{
			
		}
	}
}