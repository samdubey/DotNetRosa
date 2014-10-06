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
using DataUtil = org.javarosa.core.util.DataUtil;
using org.javarosa.core.util.externalizable;
using XPathException = org.javarosa.xpath.XPathException;
using XPathExpression = org.javarosa.xpath.expr.XPathExpression;
using System.Collections.Generic;
namespace org.javarosa.core.model.instance
{
	
	public class TreeReference : System.ICloneable
	{
		private void  InitBlock()
		{
			data.setElementAt(data.elementAt(key).setPredicates(xpe), key);
			return data.elementAt(key).getPredicates();
		}
		virtual public System.String InstanceName
		{
			get
			{
				return instanceName;
			}
			
			set
			{
				this.instanceName = value;
			}
			
		}
		virtual public int MultLast
		{
			get
			{
				return data.lastElement().getMultiplicity();
			}
			
		}
		virtual public System.String NameLast
		{
			get
			{
				return data.lastElement().getName();
			}
			
		}
		virtual public int RefLevel
		{
			get
			{
				return refLevel;
			}
			
			set
			{
				this.refLevel = value;
			}
			
		}
		virtual public bool Absolute
		{
			get
			{
				return (refLevel == REF_ABSOLUTE);
			}
			
		}
		virtual public bool Ambiguous
		{
			//return true if this ref contains any unbound multiplicities... ie, there is ANY chance this ref
			//could ambiguously refer to more than one instance node.
			
			get
			{
				//ignore level 0, as /data implies /data[0]
				for (int i = 1; i < size(); i++)
				{
					if (getMultiplicity(i) == INDEX_UNBOUND)
					{
						return true;
					}
				}
				return false;
			}
			
		}
		virtual public TreeReference ParentRef
		{
			get
			{
				//TODO: level
				TreeReference ref_Renamed = this.Clone();
				if (ref_Renamed.removeLastLevel())
				{
					return ref_Renamed;
				}
				else
				{
					return null;
				}
			}
			
		}
		virtual public int Context
		{
			get
			{
				return this.contextType;
			}
			
			//TODO: This should be in construction
			
			set
			{
				this.contextType = value;
			}
			
		}
		public const int DEFAULT_MUTLIPLICITY = 0; //multiplicity
		public const int INDEX_UNBOUND = - 1; //multiplicity
		public const int INDEX_TEMPLATE = - 2; //multiplicity
		public const int INDEX_ATTRIBUTE = - 4; //multiplicity flag for an attribute
		public const int INDEX_REPEAT_JUNCTURE = - 10;
		
		//TODO: Roll these into RefLevel? Or more likely, take absolute
		//ref out of refLevel
		public const int CONTEXT_ABSOLUTE = 0;
		public const int CONTEXT_INHERITED = 1;
		public const int CONTEXT_ORIGINAL = 2;
		public const int CONTEXT_INSTANCE = 4;
		
		
		public const int REF_ABSOLUTE = - 1;
		
		public const System.String NAME_WILDCARD = "*";
		
		private int refLevel; //0 = context node, 1 = parent, 2 = grandparent ...
		private int contextType;
		private System.String instanceName = null;
		
		private List< TreeReferenceLevel > data = null;
		
		
		public static TreeReference rootRef()
		{
			TreeReference root = new TreeReference();
			root.refLevel = REF_ABSOLUTE;
			root.contextType = CONTEXT_ABSOLUTE;
			return root;
		}
		
		public static TreeReference selfRef()
		{
			TreeReference self = new TreeReference();
			self.refLevel = 0;
			self.contextType = CONTEXT_INHERITED;
			return self;
		}
		
		public TreeReference()
		{
			InitBlock();
			instanceName = null; // null means the default instance
			refLevel = 0;
			contextType = CONTEXT_ABSOLUTE;
			
			data = new List< TreeReferenceLevel >();
		}
		
		public virtual int getMultiplicity(int index)
		{
			return data.elementAt(index).getMultiplicity();
		}
		
		public virtual System.String getName(int index)
		{
			return data.elementAt(index).getName();
		}
		
		public virtual void  setMultiplicity(int i, int mult)
		{
			data.setElementAt(data.elementAt(i).setMultiplicity(mult), i);
		}
		
		public virtual int size()
		{
			return data.size();
		}
		
		private void  add(TreeReferenceLevel level)
		{
			data.addElement(level);
		}
		
		public virtual void  add(System.String name, int mult)
		{
			add(new TreeReferenceLevel(name, mult).intern());
		}
		
		
		public		void addPredicate(int key, List< XPathExpression > xpe);


        public List<XPathExpression> getPredicate(int key);
		
		public virtual void  incrementRefLevel()
		{
			if (!Absolute)
			{
				refLevel++;
			}
		}
		
		//return a copy of the ref
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.lang.Object.clone' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		public virtual System.Object Clone()
		{
			TreeReference newRef = new TreeReference();
			newRef.RefLevel = this.refLevel;
			
			
			for(TreeReferenceLevel l: data)
			{
				newRef.add(l.shallowCopy());
			}
			
			//copy instances
			newRef.InstanceName = instanceName;
			newRef.Context = this.contextType;
			return newRef;
		}
		
		/*
		* chop the lowest level off the ref so that the ref now represents the parent of the original ref
		* return true if we successfully got the parent, false if there were no higher levels
		*/
		public virtual bool removeLastLevel()
		{
			int size = size();
			if (size == 0)
			{
				if (Absolute)
				{
					return false;
				}
				else
				{
					refLevel++;
					return true;
				}
			}
			else
			{
				data.removeElementAt(size - 1);
				return true;
			}
		}
		
		//return a new reference that is this reference anchored to a passed-in parent reference
		//if this reference is absolute, return self
		//if this ref has 'parent' steps (..), it can only be anchored if the parent ref is a relative ref consisting only of other 'parent' steps
		//return null in these invalid situations
		public virtual TreeReference parent(TreeReference parentRef)
		{
			if (Absolute)
			{
				return this;
			}
			else
			{
				TreeReference newRef = parentRef.Clone();
				
				if (refLevel > 0)
				{
					if (!parentRef.Absolute && parentRef.size() == 0)
					{
						parentRef.refLevel += refLevel;
					}
					else
					{
						return null;
					}
				}
				
				
				for(TreeReferenceLevel l: data)
				{
					newRef.add(l.shallowCopy());
				}
				
				return newRef;
			}
		}
		
		
		//very similar to parent(), but assumes contextRef refers to a singular, existing node in the model
		//this means we can do '/a/b/c + ../../d/e/f = /a/d/e/f', which we couldn't do in parent()
		//return null if context ref is not absolute, or we parent up past the root node
		//NOTE: this function still works even when contextRef contains INDEX_UNBOUND multiplicites... conditions depend on this behavior,
		//  even though it's slightly icky
		public virtual TreeReference anchor(TreeReference contextRef)
		{
			//TODO: Technically we should possibly be modifying context stuff here
			//instead of in the xpath stuff;
			
			if (Absolute)
			{
				return this.Clone();
			}
			else if (!contextRef.Absolute)
			{
				throw new XPathException("Could not resolve " + this.toString(true));
			}
			else
			{
				TreeReference newRef = contextRef.Clone();
				int contextSize = contextRef.size();
				if (refLevel > contextSize)
				{
					//tried to do '/..'
					throw new XPathException("Could not resolve " + this.toString(true));
				}
				else
				{
					for (int i = 0; i < refLevel; i++)
					{
						newRef.removeLastLevel();
					}
					for (int i = 0; i < size(); i++)
					{
						newRef.add(data.elementAt(i).shallowCopy());
					}
					return newRef;
				}
			}
		}
		
		//TODO: merge anchor() and parent()
		
		public virtual TreeReference contextualize(TreeReference contextRef)
		{
			//TODO: Technically we should possibly be modifying context stuff here
			//instead of in the xpath stuff;
			if (!contextRef.Absolute)
			{
				return null;
			}
			
			// I think contextualizing of absolute nodes still needs to be done.
			// They may contain predicates that need to be contextualized.
			
			TreeReference newRef = anchor(contextRef);
			// unclear...
			newRef.Context = contextRef.Context;
			
			//apply multiplicites and fill in wildcards as necessary based on the context ref
			for (int i = 0; i < contextRef.size() && i < newRef.size(); i++)
			{
				
				//If the the contextRef can provide a definition for a wildcard, do so
				if (TreeReference.NAME_WILDCARD.Equals(newRef.getName(i)) && !TreeReference.NAME_WILDCARD.Equals(contextRef.getName(i)))
				{
					newRef.data.setElementAt(newRef.data.elementAt(i).setName(contextRef.getName(i)), i);
				}
				
				if (contextRef.getName(i).Equals(newRef.getName(i)))
				{
					//We can't actually merge nodes if the newRef has predicates or filters
					//on this expression, since those reset any existing resolutions which
					//may have been done.
					if (newRef.getPredicate(i) == null)
					{
						newRef.setMultiplicity(i, contextRef.getMultiplicity(i));
					}
				}
				else
				{
					break;
				}
			}
			
			return newRef;
		}
		
		public virtual TreeReference relativize(TreeReference parent)
		{
			if (parent.isParentOf(this, false))
			{
				TreeReference relRef = selfRef();
				for (int i = parent.size(); i < this.size(); i++)
				{
					relRef.add(this.getName(i), INDEX_UNBOUND);
				}
				return relRef;
			}
			else
			{
				return null;
			}
		}
		
		//turn unambiguous ref into a generic ref
		public virtual TreeReference genericize()
		{
			TreeReference genericRef = Clone();
			for (int i = 0; i < genericRef.size(); i++)
			{
				//TODO: It's not super clear whether template refs should get
				//genericized or not
				genericRef.setMultiplicity(i, INDEX_UNBOUND);
			}
			return genericRef;
		}
		
		//returns true if 'this' is parent of 'child'
		//return true if 'this' equals 'child' only if properParent is false
		public virtual bool isParentOf(TreeReference child, bool properParent)
		{
			//Instances and context types;
			if (refLevel != child.refLevel)
				return false;
			if (child.size() < size() + (properParent?1:0))
				return false;
			
			for (int i = 0; i < size(); i++)
			{
				if (!this.getName(i).Equals(child.getName(i)))
				{
					return false;
				}
				
				int parMult = this.getMultiplicity(i);
				int childMult = child.getMultiplicity(i);
				if (parMult != INDEX_UNBOUND && parMult != childMult && !(i == 0 && parMult == 0 && childMult == INDEX_UNBOUND))
				{
					return false;
				}
			}
			
			return true;
		}
		
		/// <summary> clone and extend a reference by one level</summary>
		/// <param name="name">
		/// </param>
		/// <param name="mult">
		/// </param>
		/// <returns>
		/// </returns>
		public virtual TreeReference extendRef(System.String name, int mult)
		{
			//TODO: Shouldn't work for this if this is an attribute ref;
			TreeReference childRef = this.Clone();
			childRef.add(name, mult);
			return childRef;
		}
		
		public  override bool Equals(System.Object o)
		{
			if (this == o)
			{
				return true;
			}
			else if (o is TreeReference)
			{
				TreeReference ref_Renamed = (TreeReference) o;
				
				if (this.refLevel == ref_Renamed.refLevel && this.size() == ref_Renamed.size())
				{
					for (int i = 0; i < this.size(); i++)
					{
						System.String nameA = this.getName(i);
						System.String nameB = ref_Renamed.getName(i);
						int multA = this.getMultiplicity(i);
						int multB = ref_Renamed.getMultiplicity(i);
						
						
						
						
						if (!nameA.Equals(nameB))
						{
							return false;
						}
						else if (multA != multB)
						{
							if (i == 0 && (multA == 0 || multA == INDEX_UNBOUND) && (multB == 0 || multB == INDEX_UNBOUND))
							{
								// /data and /data[0] are functionally the same
							}
							else
							{
								return false;
							}
						}
						else if (predA != null && predB != null)
						{
							if (predA.size() != predB.size())
							{
								return false;
							}
							for (int j = 0; j < predA.size(); ++j)
							{
								if (!predA.elementAt(j).equals(predB.elementAt(j)))
								{
									return false;
								}
							}
						}
						else if ((predA == null && predB != null) || (predA != null && predB == null))
						{
							return false;
						}
					}
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		
		public override int GetHashCode()
		{
			int hash = ((System.Int32) refLevel).GetHashCode();
			for (int i = 0; i < size(); i++)
			{
				//NOTE(ctsims): It looks like this is only using Integer to
				//get the hashcode method, but that method
				//is just returning the int value, I think, so
				//this should potentially just be replaced by
				//an int.
				System.Int32 mult = DataUtil.integer(getMultiplicity(i));
				if (i == 0 && mult == INDEX_UNBOUND)
					mult = DataUtil.integer(0);
				
				hash ^= getName(i).GetHashCode();
				hash ^= mult.GetHashCode();
				
				if (predicates == null)
				{
					continue;
				}
				int val = 0;
				
				for(XPathExpression xpe: predicates)
				{
					hash ^= val;
					hash ^= xpe.hashCode();
					++val;
				}
			}
			return hash;
		}
		
		public override System.String ToString()
		{
			return toString(true);
		}
		
		public virtual System.String toString(bool includePredicates)
		{
			StringBuilder sb = new StringBuilder();
			if (instanceName != null)
			{
				sb.append("instance(" + instanceName + ")");
			}
			else if (contextType == CONTEXT_ORIGINAL)
			{
				sb.append("current()");
			}
			else if (contextType == CONTEXT_INHERITED)
			{
				sb.append("inherited()");
			}
			if (Absolute)
			{
				sb.append("/");
			}
			else
			{
				for (int i = 0; i < refLevel; i++)
					sb.append("../");
			}
			for (int i = 0; i < size(); i++)
			{
				System.String name = getName(i);
				int mult = getMultiplicity(i);
				
				if (mult == INDEX_ATTRIBUTE)
				{
					sb.append("@");
				}
				sb.append(name);
				
				if (includePredicates)
				{
					switch (mult)
					{
						
						case INDEX_UNBOUND:  break;
						
						case INDEX_TEMPLATE:  sb.append("[@template]"); break;
						
						case INDEX_REPEAT_JUNCTURE:  sb.append("[@juncture]"); break;
						
						default: 
							if ((i > 0 || mult != 0) && mult != - 4)
								sb.append("[" + (mult + 1) + "]");
							break;
						
					}
				}
				
				if (i < size() - 1)
					sb.append("/");
			}
			return sb.toString();
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			refLevel = ExtUtil.readInt(in_Renamed);
			instanceName = ((System.String) ExtUtil.read(in_Renamed, new ExtWrapNullable(typeof(System.String)), pf));
			contextType = ExtUtil.readInt(in_Renamed);
			int size = ExtUtil.readInt(in_Renamed);
			for (int i = 0; i < size; ++i)
			{
				TreeReferenceLevel level = (TreeReferenceLevel) ExtUtil.read(in_Renamed, typeof(TreeReferenceLevel));
				this.add(level.intern());
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeNumeric(out_Renamed, refLevel);
			ExtUtil.write(out_Renamed, new ExtWrapNullable(instanceName));
			ExtUtil.writeNumeric(out_Renamed, contextType);
			ExtUtil.writeNumeric(out_Renamed, size());
			
			for(TreeReferenceLevel l: data)
			{
				ExtUtil.write(out_Renamed, l);
			}
		}
		
		/// <summary>Intersect this tree reference with another, returning a new tree reference
		/// which contains all of the common elements, starting with the root element.
		/// 
		/// Note that relative references by their nature can't share steps, so intersecting
		/// any (or by any) relative ref will result in the root ref. Additionally, if the
		/// two references don't share any steps, the intersection will consist of the root
		/// reference.
		/// 
		/// </summary>
		/// <param name="b">The tree reference to intersect
		/// </param>
		/// <returns> The tree reference containing the common basis of this ref and b
		/// </returns>
		public virtual TreeReference intersect(TreeReference b)
		{
			if (!this.Absolute || !b.Absolute)
			{
				return TreeReference.rootRef();
			}
			if (this.Equals(b))
			{
				return this;
			}
			
			
			TreeReference a;
			//A should always be bigger if one ref is larger than the other
			if (this.size() < b.size())
			{
				a = b.Clone(); b = this.Clone();
			}
			else
			{
				a = this.Clone(); b = b.Clone();
			}
			
			//Now, trim the refs to the same length.
			int diff = a.size() - b.size();
			for (int i = 0; i < diff; ++i)
			{
				a.removeLastLevel();
			}
			
			int aSize = a.size();
			//easy, but requires a lot of re-evaluation.
			for (int i = 0; i <= aSize; ++i)
			{
				if (a.Equals(b))
				{
					return a;
				}
				else if (a.size() == 0)
				{
					return TreeReference.rootRef();
				}
				else
				{
					if (!a.removeLastLevel() || !b.removeLastLevel())
					{
						//I don't think it should be possible for us to get here, so flip if we do
						throw new System.SystemException("Dug too deply into TreeReference during intersection");
					}
				}
			}
			
			//The only way to get here is if a's size is -1
			throw new System.SystemException("Impossible state");
		}
		
		/// <summary> Returns the subreference of this reference up to the level specified.
		/// 
		/// Used to identify the reference context for a predicate at the same level
		/// 
		/// Must be an absolute reference, otherwise will throw IllegalArgumentException
		/// 
		/// </summary>
		/// <param name="level">
		/// </param>
		/// <returns>
		/// </returns>
		public virtual TreeReference getSubReference(int level)
		{
			if (!this.Absolute)
			{
				throw new System.ArgumentException("Cannot subreference a non-absolute ref");
			}
			
			//Copy construct
			TreeReference ret = new TreeReference();
			ret.refLevel = this.refLevel;
			ret.contextType = this.contextType;
			ret.instanceName = this.instanceName;
			
			ret.data = new List< TreeReferenceLevel >();
			for (int i = 0; i <= level; ++i)
			{
				ret.data.addElement(this.data.elementAt(i));
			}
			return ret;
		}
		
		public virtual bool hasPredicates()
		{
			
			for(TreeReferenceLevel level: data)
			{
				if (level.getPredicates() != null)
				{
					return true;
				}
			}
			return false;
		}
		
		public virtual TreeReference removePredicates()
		{
			TreeReference predicateless = Clone();
			for (int i = 0; i < predicateless.data.size(); ++i)
			{
				predicateless.data.setElementAt(predicateless.data.elementAt(i).setPredicates(null), i);
			}
			return predicateless;
		}
	}
}