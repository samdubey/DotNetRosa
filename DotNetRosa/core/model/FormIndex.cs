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
using TreeReference = org.javarosa.core.model.instance.TreeReference;
namespace org.javarosa.core.model
{
	
	/// <summary> A Form Index is an immutable index into a specific question definition that
	/// will appear in an interaction with a user.
	/// 
	/// An index is represented by different levels into hierarchical groups.
	/// 
	/// Indices can represent both questions and groups.
	/// 
	/// It is absolutely essential that there be no circularity of reference in
	/// FormIndex's, IE, no form index's ancestor can be itself.
	/// 
	/// Datatype Productions:
	/// FormIndex = BOF | EOF | CompoundIndex(nextIndex:FormIndex,Location)
	/// Location = Empty | Simple(localLevel:int) | WithMult(localLevel:int, multiplicity:int)
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public class FormIndex
	{
		/// <summary> </summary>
		/// <returns> true if the index is neither before the start or after the end of the form
		/// </returns>
		virtual public bool InForm
		{
			get
			{
				return !beginningOfForm && !endOfForm;
			}
			
		}
		/// <returns> The index of the element in the current context
		/// </returns>
		virtual public int LocalIndex
		{
			get
			{
				return localIndex;
			}
			
		}
		/// <returns> The multiplicity of the current instance of a repeated question or group
		/// </returns>
		virtual public int InstanceIndex
		{
			get
			{
				return instanceIndex;
			}
			
		}
		/// <summary> For the fully qualified element, get the multiplicity of the element's reference</summary>
		/// <returns> The terminal element (fully qualified)'s instance index
		/// </returns>
		virtual public int ElementMultiplicity
		{
			get
			{
				return getTerminal().instanceIndex;
			}
			
		}
		/// <returns> An index into the next level of specificity past the current context. An
		/// example would be an index  into an element that is a child of the element referenced
		/// by the local index.
		/// </returns>
		virtual public FormIndex NextLevel
		{
			get
			{
				return nextLevel;
			}
			
		}
		virtual public TreeReference LocalReference
		{
			get
			{
				return reference;
			}
			
		}
		/// <returns> The TreeReference of the fully qualified element described by this
		/// FormIndex.
		/// </returns>
		virtual public TreeReference Reference
		{
			get
			{
				return getTerminal().reference;
			}
			
		}
		/// <summary> </summary>
		/// <returns> true if we are after the end of the form
		/// </returns>
		virtual public bool EndOfFormIndex
		{
			get
			{
				return endOfForm;
			}
			
		}
		/// <summary> </summary>
		/// <returns> true if we are before the start of the form
		/// </returns>
		virtual public bool BeginningOfFormIndex
		{
			get
			{
				return beginningOfForm;
			}
			
		}
		/// <returns> the level of this index relative to the top level of the form
		/// </returns>
		virtual public int Depth
		{
			get
			{
				
				int depth = 0;
				FormIndex ref_Renamed = this;
				while (ref_Renamed != null)
				{
					ref_Renamed = ref_Renamed.nextLevel;
					depth++;
				}
				return depth;
			}
			
		}
		
		private bool beginningOfForm = false;
		
		private bool endOfForm = false;
		
		/// <summary>The index of the questiondef in the current context </summary>
		private int localIndex;
		
		/// <summary>The multiplicity of the current instance of a repeated question or group </summary>
		private int instanceIndex = - 1;
		
		/// <summary>The next level of this index </summary>
		private FormIndex nextLevel;
		
		private TreeReference reference;
		
		/// <summary> </summary>
		/// <returns> an index before the start of the form
		/// </returns>
		public static FormIndex createBeginningOfFormIndex()
		{
			FormIndex begin = new FormIndex(- 1, null);
			begin.beginningOfForm = true;
			return begin;
		}
		
		/// <summary> </summary>
		/// <returns> an index after the end of the form
		/// </returns>
		public static FormIndex createEndOfFormIndex()
		{
			FormIndex end = new FormIndex(- 1, null);
			end.endOfForm = true;
			return end;
		}
		
		/// <summary> Constructs a simple form index that references a specific element in
		/// a list of elements.
		/// </summary>
		/// <param name="localIndex">An integer index into a flat list of elements
		/// </param>
		/// <param name="reference">A reference to the instance element identified by this index;
		/// </param>
		public FormIndex(int localIndex, TreeReference reference)
		{
			this.localIndex = localIndex;
			this.reference = reference;
		}
		/// <summary> Constructs a simple form index that references a specific element in
		/// a list of elements.
		/// </summary>
		/// <param name="localIndex">An integer index into a flat list of elements
		/// </param>
		/// <param name="instanceIndex">An integer index expressing the multiplicity
		/// of the current level
		/// </param>
		/// <param name="reference">A reference to the instance element identified by this index;
		/// 
		/// </param>
		public FormIndex(int localIndex, int instanceIndex, TreeReference reference)
		{
			this.localIndex = localIndex;
			this.instanceIndex = instanceIndex;
			this.reference = reference;
		}
		
		/// <summary> Constructs an index which indexes an element, and provides an index
		/// into that elements children
		/// 
		/// </summary>
		/// <param name="nextLevel">An index into the referenced element's index
		/// </param>
		/// <param name="localIndex">An index to an element at the current level, a child
		/// element of which will be referenced by the nextLevel index.
		/// </param>
		/// <param name="reference">A reference to the instance element identified by this index;
		/// </param>
		public FormIndex(FormIndex nextLevel, int localIndex, TreeReference reference):this(localIndex, reference)
		{
			this.nextLevel = nextLevel;
		}
		
		/// <summary> Constructs an index which references an element past the level of
		/// specificity of the current context, founded by the currentLevel
		/// index.
		/// (currentLevel, (nextLevel...))
		/// </summary>
		public FormIndex(FormIndex nextLevel, FormIndex currentLevel)
		{
			if (currentLevel == null)
			{
				this.nextLevel = nextLevel.nextLevel;
				this.localIndex = nextLevel.localIndex;
				this.instanceIndex = nextLevel.instanceIndex;
				this.reference = nextLevel.reference;
			}
			else
			{
				this.nextLevel = nextLevel;
				this.localIndex = currentLevel.LocalIndex;
				this.instanceIndex = currentLevel.InstanceIndex;
				this.reference = currentLevel.reference;
			}
		}
		
		/// <summary> Constructs an index which indexes an element, and provides an index
		/// into that elements children, along with the current index of a
		/// repeated instance.
		/// 
		/// </summary>
		/// <param name="nextLevel">An index into the referenced element's index
		/// </param>
		/// <param name="localIndex">An index to an element at the current level, a child
		/// element of which will be referenced by the nextLevel index.
		/// </param>
		/// <param name="instanceIndex">How many times the element referenced has been
		/// repeated.
		/// </param>
		/// <param name="reference">A reference to the instance element identified by this index;
		/// </param>
		public FormIndex(FormIndex nextLevel, int localIndex, int instanceIndex, TreeReference reference):this(nextLevel, localIndex, reference)
		{
			this.instanceIndex = instanceIndex;
		}
		
		public virtual FormIndex getTerminal()
		{
			FormIndex walker = this;
			while (walker.nextLevel != null)
			{
				walker = walker.nextLevel;
			}
			return walker;
		}
		
		/// <summary> Identifies whether this is a terminal index, in other words whether this
		/// index references with more specificity than the current context
		/// </summary>
		public virtual bool isTerminal()
		{
			return nextLevel == null;
		}
		
		public  override bool Equals(System.Object o)
		{
			if (!(o is FormIndex))
				return false;
			
			FormIndex a = this;
			FormIndex b = (FormIndex) o;
			
			return (a.compareTo(b) == 0);
			
			//		//TODO: while(true) loops freak me out, this should probably
			//		//get written more safely. -ctsims
			//
			//		//Iterate over each level of reference, and identify whether
			//		//each object stays in sync
			//		while(true) {
			//			if(index.isTerminal() != local.isTerminal() ||
			//					index.getLocalIndex() != local.getLocalIndex() ||
			//					index.getInstanceIndex() != local.getInstanceIndex()) {
			//				return false;
			//			}
			//			if(index.isTerminal()) {
			//				return true;
			//			}
			//			local = local.getNextLevel();
			//			index = index.getNextLevel();
			//		}
			//
		}
		
		public virtual int compareTo(System.Object o)
		{
			if (!(o is FormIndex))
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new System.ArgumentException("Attempt to compare Object of type " + o.GetType().FullName + " to a FormIndex");
			}
			
			FormIndex a = this;
			FormIndex b = (FormIndex) o;
			
			if (a.beginningOfForm)
			{
				return (b.beginningOfForm?0:- 1);
			}
			else if (a.endOfForm)
			{
				return (b.endOfForm?0:1);
			}
			else
			{
				//a is in form
				if (b.beginningOfForm)
				{
					return 1;
				}
				else if (b.endOfForm)
				{
					return - 1;
				}
			}
			
			if (a.localIndex != b.localIndex)
			{
				return (a.localIndex < b.localIndex?- 1:1);
			}
			else if (a.instanceIndex != b.instanceIndex)
			{
				return (a.instanceIndex < b.instanceIndex?- 1:1);
			}
			else if ((a.NextLevel == null) != (b.NextLevel == null))
			{
				return (a.NextLevel == null?- 1:1);
			}
			else if (a.NextLevel != null)
			{
				return a.NextLevel.compareTo(b.NextLevel);
			}
			else
			{
				return 0;
			}
			
			//		int comp = 0;
			//
			//		//TODO: while(true) loops freak me out, this should probably
			//		//get written more safely. -ctsims
			//		while(comp == 0) {
			//			if(index.isTerminal() != local.isTerminal() ||
			//					index.getLocalIndex() != local.getLocalIndex() ||
			//					index.getInstanceIndex() != local.getInstanceIndex()) {
			//				if(local.localIndex > index.localIndex) {
			//					return 1;
			//				} else if(local.localIndex < index.localIndex) {
			//					return -1;
			//				} else if (local.instanceIndex > index.instanceIndex) {
			//					return 1;
			//				} else if (local.instanceIndex < index.instanceIndex) {
			//					return -1;
			//				}
			//
			//				//This case is here as a fallback, but it shouldn't really
			//				//ever be the case that two references have the same chain
			//				//of indices without terminating at the same level.
			//				else if (local.isTerminal() && !index.isTerminal()) {
			//					return -1;
			//				} else {
			//					return 1;
			//				}
			//			}
			//			else if(local.isTerminal()) {
			//				break;
			//			}
			//			local = local.getNextLevel();
			//			index = index.getNextLevel();
			//		}
			//		return comp;
		}
		
		/// <returns> Only the local component of this Form Index.
		/// </returns>
		public virtual FormIndex snip()
		{
			FormIndex retval = new FormIndex(localIndex, instanceIndex, reference);
			return retval;
		}
		
		/// <summary> Takes in a form index which is a subset of this index, and returns the
		/// total difference between them. This is useful for stepping up the level
		/// of index specificty. If the subIndex is not a valid subIndex of this index,
		/// null is returned. Since the FormIndex represented by null is always a subset,
		/// if null is passed in as a subIndex, the full index is returned
		/// 
		/// For example:
		/// Indices
		/// a = 1_0,2,1,3
		/// b = 1,3
		/// 
		/// a.diff(b) = 1_0,2
		/// 
		/// </summary>
		/// <param name="subIndex">
		/// </param>
		/// <returns>
		/// </returns>
		public virtual FormIndex diff(FormIndex subIndex)
		{
			if (subIndex == null)
			{
				return this;
			}
			if (!isSubIndex(this, subIndex))
			{
				return null;
			}
			if (subIndex.Equals(this))
			{
				return null;
			}
			return new FormIndex(nextLevel.diff(subIndex), this.snip());
		}
		
		public override System.String ToString()
		{
			StringBuilder b = new StringBuilder();
			FormIndex ref_Renamed = this;
			while (ref_Renamed != null)
			{
				b.append(ref_Renamed.LocalIndex);
				if (ref_Renamed.InstanceIndex != - 1)
				{
					b.append("_").append(ref_Renamed.InstanceIndex);
				}
				b.append(", ");
				ref_Renamed = ref_Renamed.nextLevel;
			}
			return b.toString();
		}
		
		/// <summary> Trims any negative indices from the end of the passed in index.
		/// 
		/// </summary>
		/// <param name="index">
		/// </param>
		/// <returns>
		/// </returns>
		public static FormIndex trimNegativeIndices(FormIndex index)
		{
			if (!index.isTerminal())
			{
				return new FormIndex(trimNegativeIndices(index.nextLevel), index);
			}
			else
			{
				if (index.LocalIndex < 0)
				{
					return null;
				}
				else
				{
					return index;
				}
			}
		}
		
		public static bool isSubIndex(FormIndex parent, FormIndex child)
		{
			if (child.Equals(parent))
			{
				return true;
			}
			else
			{
				if (parent == null)
				{
					return false;
				}
				return isSubIndex(parent.nextLevel, child);
			}
		}
		
		public static bool isSubElement(FormIndex parent, FormIndex child)
		{
			while (!parent.isTerminal() && !child.isTerminal())
			{
				if (parent.LocalIndex != child.LocalIndex)
				{
					return false;
				}
				if (parent.InstanceIndex != child.InstanceIndex)
				{
					return false;
				}
				parent = parent.nextLevel;
				child = child.nextLevel;
			}
			//If we've gotten this far, at least one of the two is terminal
			if (!parent.isTerminal() && child.isTerminal())
			{
				//can't be the parent if the child is earlier on
				return false;
			}
			else if (parent.LocalIndex != child.LocalIndex)
			{
				//Either they're at the same level, in which case only
				//identical indices should match, or they should have
				//the same root
				return false;
			}
			else if (parent.InstanceIndex != - 1 && (parent.InstanceIndex != child.InstanceIndex))
			{
				return false;
			}
			//Barring all of these cases, it should be true.
			return true;
		}
		
		public virtual void  assignRefs(FormDef f)
		{
			FormIndex cur = this;
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Vector < Integer > indexes = new Vector < Integer >();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Vector < Integer > multiplicities = new Vector < Integer >();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Vector < IFormElement > elements = new Vector < IFormElement >();
			f.collapseIndex(this, indexes, multiplicities, elements);
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Vector < Integer > curMults = new Vector < Integer >();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Vector < IFormElement > curElems = new Vector < IFormElement >();
			
			int i = 0;
			while (cur != null)
			{
				curMults.addElement(multiplicities.elementAt(i));
				curElems.addElement(elements.elementAt(i));
				
				TreeReference ref_Renamed = f.getChildInstanceRef(curElems, curMults);
				cur.reference = ref_Renamed;
				
				cur = cur.NextLevel;
				i++;
			}
		}
		//UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}