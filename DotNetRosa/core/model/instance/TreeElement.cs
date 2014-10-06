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
using Constants = org.javarosa.core.model.Constants;
using FormDef = org.javarosa.core.model.FormDef;
using FormElementStateListener = org.javarosa.core.model.FormElementStateListener;
using Constraint = org.javarosa.core.model.condition.Constraint;
using EvaluationContext = org.javarosa.core.model.condition.EvaluationContext;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using SelectMultiData = org.javarosa.core.model.data.SelectMultiData;
using SelectOneData = org.javarosa.core.model.data.SelectOneData;
using UncastData = org.javarosa.core.model.data.UncastData;
using CompactInstanceWrapper = org.javarosa.core.model.instance.utils.CompactInstanceWrapper;
using DefaultAnswerResolver = org.javarosa.core.model.instance.utils.DefaultAnswerResolver;
using IAnswerResolver = org.javarosa.core.model.instance.utils.IAnswerResolver;
using ITreeVisitor = org.javarosa.core.model.instance.utils.ITreeVisitor;
using RestoreUtils = org.javarosa.core.model.util.restorable.RestoreUtils;
using DataUtil = org.javarosa.core.util.DataUtil;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapNullable = org.javarosa.core.util.externalizable.ExtWrapNullable;
using ExtWrapTagged = org.javarosa.core.util.externalizable.ExtWrapTagged;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using XPathReference = org.javarosa.model.xform.XPathReference;
using XFormParser = org.javarosa.xform.parse.XFormParser;
using XPathEqExpr = org.javarosa.xpath.expr.XPathEqExpr;
using XPathExpression = org.javarosa.xpath.expr.XPathExpression;
using XPathPathExpr = org.javarosa.xpath.expr.XPathPathExpr;
using XPathStringLiteral = org.javarosa.xpath.expr.XPathStringLiteral;
namespace org.javarosa.core.model.instance
{
	
	/// <summary> An element of a FormInstance.
	/// 
	/// TreeElements represent an XML node in the instance. It may either have a value (e.g., <name>Drew</name>),
	/// a number of TreeElement children (e.g., <meta><device /><timestamp /><user_id /></meta>), or neither (e.g.,
	/// <empty_node />)
	/// 
	/// TreeElements can also represent attributes. Attributes are unique from normal elements in that they are
	/// not "children" of their parent, and are always leaf nodes: IE cannot have children.
	/// 
	/// TODO: Split out the bind-able session data from this class and leave only the mandatory values to speed up
	/// new DOM-like models
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	
	public class TreeElement : AbstractTreeElement
	{
		private void  InitBlock()
		{
			dataType = Constants.DATATYPE_NULL;
			
			for(TreeElement attribute: attributes)
			{
				if (attribute.Name.equals(name) && (namespace_Renamed == null || namespace_Renamed.Equals(attribute.namespace_Renamed)))
				{
					return attribute;
				}
			}
			return null;
			
			TreeElement attribut = getAttribute(attrs, namespace_Renamed, name);
			if (attribut != null)
			{
				if (value_Renamed == null)
				{
					attrs.removeElement(attribut);
				}
				else
				{
					attribut.Value = new UncastData(value_Renamed);
				}
				return ;
			}
			
			// null-valued attributes are a "remove-this" instruction... ignore them
			if (value_Renamed == null)
				return ;
			
			// create an attribute...
			TreeElement attr = TreeElement.constructAttributeElement(namespace_Renamed, name, value_Renamed);
			attr.Parent = parent;
			
			attrs.addElement(attr);
			return getChildrenWithName(name, false);
			
			List< TreeElement > v = new List< TreeElement >();
			if (children == null)
			{
				return v;
			}
			
			for (int i = 0; i < this.children.Count; i++)
			{
				TreeElement child = (TreeElement) this.children[i];
				if ((child.Name.Equals(name) || name.Equals(TreeReference.NAME_WILDCARD)) && (includeTemplate || child.multiplicity != TreeReference.INDEX_TEMPLATE))
					v.addElement(child);
			}
			
			return v;
			// create new tree elements for all the bind definitions...
			
			for(TreeElement ref: bindAttributes)
			{
				setBindAttribute(ref_Renamed.Namespace, ref_Renamed.Name, ref_Renamed.getAttributeValue());
			}
			return bindAttributes;
			//Only do for predicates
			if (mult != TreeReference.INDEX_UNBOUND || predicates == null)
			{
				return null;
			}
			
			
			List< Integer > toRemove = new List< Integer >();
			
			
			//Lazy init these until we've determined that our predicate is hintable
			
			HashMap < XPathPathExpr, String > indices = null;
			
			
			//UPGRADE_NOTE: Label 'predicate' was moved. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1014'"
			for (int i = 0; i < predicates.size(); ++i)
			{
				XPathExpression xpe = predicates.elementAt(i);
				//what we want here is a static evaluation of the expression to see if it consists of evaluating
				//something we index with something static.
				if (xpe is XPathEqExpr)
				{
					XPathExpression left = ((XPathEqExpr) xpe).a;
					XPathExpression right = ((XPathEqExpr) xpe).b;
					
					//For now, only cheat when this is a string literal (this basically just means that we're
					//handling attribute based referencing with very reasonable timing, but it's complex otherwise)
					if (left is XPathPathExpr && right is XPathStringLiteral)
					{
						
						//We're lazily initializing this, since it might actually take a while, and we
						//don't want the overhead if our predicate is too complex anyway
						if (indices == null)
						{
							
							indices = new HashMap < XPathPathExpr, String >();
							kids = this.getChildrenWithName(name);
							
							if (kids.size() == 0)
							{
								return null;
							}
							
							//Anything that we're going to use across elements should be on all of them
							TreeElement kid = kids.elementAt(0);
							for (int j = 0; j < kid.AttributeCount; ++j)
							{
								System.String attribute = kid.getAttributeName(j);
								indices.put(XPathReference.getPathExpr("@" + attribute), attribute);
							}
						}
						
						
						for(XPathPathExpr expr: indices.keySet())
						{
							if (expr.equals(left))
							{
								System.String attributeName = indices.get_Renamed(expr);
								
								for (int kidI = 0; kidI < kids.size(); ++kidI)
								{
									if (kids.elementAt(kidI).getAttributeValue(null, attributeName).equals(((XPathStringLiteral) right).s))
									{
										if (selectedChildren == null)
										{
											
											selectedChildren = new List< TreeReference >();
										}
										selectedChildren.addElement(kids.elementAt(kidI).Ref);
									}
								}
								
								
								//Note that this predicate is evaluated and doesn't need to be evaluated in the future.
								toRemove.addElement(DataUtil.integer(i));
								//UPGRADE_NOTE: Labeled continue statement was changed to a goto statement. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1015'"
								goto predicate;
							}
						}
					}
				}
				//There's only one case where we want to keep moving along, and we would have triggered it if it were going to happen,
				//so otherwise, just get outta here.
				break;
				//UPGRADE_NOTE: Label 'predicate' was moved. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1014'"

predicate: ;
			}
			
			//if we weren't able to evaluate any predicates, signal that.
			if (selectedChildren == null)
			{
				return null;
			}
			
			//otherwise, remove all of the predicates we've already evaluated
			for (int i = toRemove.size() - 1; i >= 0; i--)
			{
				predicates.removeElementAt(toRemove.elementAt(i).intValue());
			}
			
			return selectedChildren;
		}
		private bool IsAttribute
		{
			set
			{
				setMaskVar(MASK_ATTRIBUTE, value);
			}
			
		}
		virtual public bool Leaf
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#isLeaf()
			*/
			
			get
			{
				return (children == null || children.Count == 0);
			}
			
		}
		virtual public bool Childable
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#isChildable()
			*/
			
			get
			{
				return (value_Renamed == null);
			}
			
		}
		virtual public System.String InstanceName
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#getInstanceName()
			*/
			
			get
			{
				//CTS: I think this is a better way to do this, although I really, really don't like the duplicated code
				if (parent != null)
				{
					return parent.InstanceName;
				}
				return instanceName;
			}
			
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#setInstanceName(java.lang.String)
			*/
			
			set
			{
				this.instanceName = value;
			}
			
		}
		virtual public IAnswerData Value
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#getValue()
			*/
			
			get
			{
				return value_Renamed;
			}
			
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#setValue(org.javarosa.core.model.data.IAnswerData)
			*/
			
			set
			{
				if (Leaf)
				{
					this.value_Renamed = value;
				}
				else
				{
					throw new System.SystemException("Can't set data value for node that has children!");
				}
			}
			
		}
		virtual public int NumChildren
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#getNumChildren()
			*/
			
			get
			{
				return children == null?0:this.children.Count;
			}
			
		}
		virtual public bool Repeatable
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#isRepeatable()
			*/
			
			get
			{
				return getMaskVar(MASK_REPEATABLE);
			}
			
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#setRepeatable(boolean)
			*/
			
			set
			{
				setMaskVar(MASK_REPEATABLE, value);
			}
			
		}
		virtual public int DataType
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#getDataType()
			*/
			
			get
			{
				return dataType;
			}
			
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#setDataType(int)
			*/
			
			set
			{
				this.dataType = value;
			}
			
		}
		virtual public bool Required
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#isRequired()
			*/
			
			get
			{
				return getMaskVar(MASK_REQUIRED);
			}
			
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#setRequired(boolean)
			*/
			
			set
			{
				if (getMaskVar(MASK_REQUIRED) != value)
				{
					setMaskVar(MASK_REQUIRED, value);
					alertStateObservers(org.javarosa.core.model.FormElementStateListener_Fields.CHANGE_REQUIRED);
				}
			}
			
		}
		virtual public int AttributeCount
		{
			/* (non-Javadoc)
			* Returns the number of attributes of this element.
			* @see org.javarosa.core.model.instance.AbstractTreeElement#getAttributeCount()
			*/
			
			get
			{
				return attributes == null?0:attributes.size();
			}
			
		}
		virtual public TreeReference Ref
		{
			//return the tree reference that corresponds to this tree element
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#getRef()
			*/
			
			get
			{
				//TODO: Expire cache somehow;
				lock (refCache)
				{
					if (refCache[0] == null)
					{
						refCache[0] = TreeElement.BuildRef(this);
					}
					return refCache[0];
				}
			}
			
		}
		virtual public int Depth
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#getDepth()
			*/
			
			get
			{
				return TreeElement.CalculateDepth(this);
			}
			
		}
		virtual public System.String PreloadHandler
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#getPreloadHandler()
			*/
			
			get
			{
				return preloadHandler;
			}
			
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#setPreloadHandler(java.lang.String)
			*/
			
			set
			{
				this.preloadHandler = value;
			}
			
		}
		virtual public Constraint Constraint
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#getConstraint()
			*/
			
			get
			{
				return constraint;
			}
			
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#setConstraint(org.javarosa.core.model.condition.Constraint)
			*/
			
			set
			{
				this.constraint = value;
			}
			
		}
		virtual public System.String PreloadParams
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#getPreloadParams()
			*/
			
			get
			{
				return preloadParams;
			}
			
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#setPreloadParams(java.lang.String)
			*/
			
			set
			{
				this.preloadParams = value;
			}
			
		}
		virtual public System.String Name
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#getName()
			*/
			
			get
			{
				return name;
			}
			
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#setName(java.lang.String)
			*/
			
			set
			{
				expireReferenceCache();
				this.name = value;
			}
			
		}
		virtual public int Mult
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#getMult()
			*/
			
			get
			{
				return multiplicity;
			}
			
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#setMult(int)
			*/
			
			set
			{
				expireReferenceCache();
				this.multiplicity = value;
			}
			
		}
		virtual public AbstractTreeElement Parent
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#getParent()
			*/
			
			get
			{
				return parent;
			}
			
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.AbstractTreeElement#setParent(org.javarosa.core.model.instance.TreeElement)
			*/
			
			set
			{
				expireReferenceCache();
				this.parent = value;
			}
			
		}
		virtual public int Multiplicity
		{
			get
			{
				return multiplicity;
			}
			
		}
		virtual public System.String Namespace
		{
			get
			{
				return namespace_Renamed;
			}
			
			set
			{
				this.namespace_Renamed = value;
			}
			
		}
		
		< TreeElement >
		private System.String name; // can be null only for hidden root node
		protected internal int multiplicity = - 1; // see TreeReference for special values
		private AbstractTreeElement parent;
		
		private IAnswerData value_Renamed;
		
		private System.Collections.ArrayList observers;
		
		private List< TreeElement > attributes;
		private System.Collections.ArrayList children = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
		
		/* model properties */
		//UPGRADE_NOTE: The initialization of  'dataType' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		protected internal int dataType; //TODO
		
		private Constraint constraint = null;
		private System.String preloadHandler = null;
		private System.String preloadParams = null;
		
		private List< TreeElement > bindAttributes = new List< TreeElement >();
		
		//private boolean required = false;// TODO
		//protected boolean repeatable;
		//protected boolean isAttribute;
		//private boolean relevant = true;
		//private boolean enabled = true;
		// inherited properties
		//private boolean relevantInherited = true;
		//private boolean enabledInherited = true;
		
		private const int MASK_REQUIRED = 0x01;
		private const int MASK_REPEATABLE = 0x02;
		private const int MASK_ATTRIBUTE = 0x04;
		private const int MASK_RELEVANT = 0x08;
		private const int MASK_ENABLED = 0x10;
		private const int MASK_RELEVANT_INH = 0x20;
		private const int MASK_ENABLED_INH = 0x40;
		
		private int flags = MASK_RELEVANT | MASK_ENABLED | MASK_RELEVANT_INH | MASK_ENABLED_INH;
		
		private System.String namespace_Renamed;
		
		private System.String instanceName = null;
		
		/// <summary> TreeElement with null name and 0 multiplicity? (a "hidden root" node?)</summary>
		public TreeElement():this(null, TreeReference.DEFAULT_MUTLIPLICITY)
		{
		}
		
		public TreeElement(System.String name):this(name, TreeReference.DEFAULT_MUTLIPLICITY)
		{
		}
		
		public TreeElement(System.String name, int multiplicity)
		{
			InitBlock();
			this.name = name;
			this.multiplicity = multiplicity;
			this.parent = null;
			
			attributes = new List< TreeElement >(0);
		}
		
		/// <summary> Construct a TreeElement which represents an attribute with the provided
		/// namespace and name.
		/// 
		/// </summary>
		/// <param name="namespace">- if null will be converted to empty string
		/// </param>
		/// <param name="name">
		/// </param>
		/// <param name="value">
		/// </param>
		/// <returns> A new instance of a TreeElement
		/// </returns>
		public static TreeElement constructAttributeElement(System.String namespace_Renamed, System.String name, System.String value_Renamed)
		{
			TreeElement element = new TreeElement(name);
			element.IsAttribute = true;
			element.namespace_Renamed = (namespace_Renamed == null)?"":namespace_Renamed;
			element.multiplicity = TreeReference.INDEX_ATTRIBUTE;
			element.value_Renamed = new UncastData(value_Renamed);
			return element;
		}
		
		/// <summary> Retrieves the TreeElement representing the attribute for
		/// the provided namespace and name, or null if none exists.
		/// 
		/// If 'null' is provided for the namespace, it will match the first
		/// attribute with the matching name.
		/// 
		/// </summary>
		/// <param name="attributes">- list of attributes to search
		/// </param>
		/// <param name="namespace">
		/// </param>
		/// <param name="name">
		/// </param>
		/// <returns> TreeElement
		/// </returns>
		public static TreeElement getAttribute_Renamed_Field;
		
		(List< TreeElement > attributes, String namespace, String name)
		
		
		public static
		
		void setAttribute(TreeElement parent, List< TreeElement > attrs, String namespace, String name, String value)
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#getChild(java.lang.String, int)
		*/
		public virtual TreeElement getChild(System.String name, int multiplicity)
		{
			if (this.children == null)
			{
				return null;
			}
			
			if (name.Equals(TreeReference.NAME_WILDCARD))
			{
				if (multiplicity == TreeReference.INDEX_TEMPLATE || this.children.Count < multiplicity + 1)
				{
					return null;
				}
				return (TreeElement) this.children[multiplicity]; //droos: i'm suspicious of this
			}
			else
			{
				for (int i = 0; i < this.children.Count; i++)
				{
					TreeElement child = (TreeElement) this.children[i];
					if (name.Equals(child.Name) && child.Mult == multiplicity)
					{
						return child;
					}
				}
			}
			
			return null;
		}
		
		/* (non-Javadoc)
		*
		* Get all the child nodes of this element, with specific name
		*
		* @param name
		* @return
		*
		* @see org.javarosa.core.model.instance.AbstractTreeElement#getChildrenWithName(java.lang.String)
		*/
		
		public List< TreeElement > getChildrenWithName(String name)
		
		
		private List< TreeElement > getChildrenWithName(String name, boolean includeTemplate)
		
		public virtual bool hasChildren()
		{
			if (NumChildren > 0)
			{
				return true;
			}
			return false;
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#getChildAt(int)
		*/
		public virtual TreeElement getChildAt(int i)
		{
			return (TreeElement) children[i];
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#isAttribute()
		*/
		public virtual bool isAttribute()
		{
			return getMaskVar(MASK_ATTRIBUTE);
		}
		
		/* (non-Javadoc)
		* Add a child to this element
		*
		* @param child
		* @see org.javarosa.core.model.instance.AbstractTreeElement#addChild(org.javarosa.core.model.instance.TreeElement)
		*/
		public virtual void  addChild(TreeElement child)
		{
			addChild(child, false);
		}
		
		private void  addChild(TreeElement child, bool checkDuplicate)
		{
			if (!Childable)
			{
				throw new System.SystemException("Can't add children to node that has data value!");
			}
			
			if (child.multiplicity == TreeReference.INDEX_UNBOUND)
			{
				throw new System.SystemException("Cannot add child with an unbound index!");
			}
			
			if (checkDuplicate)
			{
				TreeElement existingChild = getChild(child.name, child.multiplicity);
				if (existingChild != null)
				{
					throw new System.SystemException("Attempted to add duplicate child!");
				}
			}
			if (children == null)
			{
				children = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			}
			
			// try to keep things in order
			int i = children.Count;
			if (child.Mult == TreeReference.INDEX_TEMPLATE)
			{
				TreeElement anchor = getChild(child.Name, 0);
				if (anchor != null)
					i = children.IndexOf(anchor);
			}
			else
			{
				TreeElement anchor = getChild(child.Name, (child.Mult == 0?TreeReference.INDEX_TEMPLATE:child.Mult - 1));
				if (anchor != null)
					i = children.IndexOf(anchor) + 1;
			}
			children.Insert(i, child);
			child.Parent = this;
			
			child.setRelevant(isRelevant(), true);
			child.setEnabled(isEnabled(), true);
			child.InstanceName = InstanceName;
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#removeChild(org.javarosa.core.model.instance.TreeElement)
		*/
		public virtual void  removeChild(TreeElement child)
		{
			if (children == null)
			{
				return ;
			}
			children.Remove(child);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#removeChild(java.lang.String, int)
		*/
		public virtual void  removeChild(System.String name, int multiplicity)
		{
			TreeElement child = getChild(name, multiplicity);
			if (child != null)
			{
				removeChild(child);
			}
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#removeChildren(java.lang.String)
		*/
		public virtual void  removeChildren(System.String name)
		{
			removeChildren(name, false);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#removeChildren(java.lang.String, boolean)
		*/
		public virtual void  removeChildren(System.String name, bool includeTemplate)
		{
			System.Collections.ArrayList v = getChildrenWithName(name, includeTemplate);
			for (int i = 0; i < v.Count; i++)
			{
				removeChild((TreeElement) v[i]);
			}
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#removeChildAt(int)
		*/
		public virtual void  removeChildAt(int i)
		{
			children.RemoveAt(i);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#getChildMultiplicity(java.lang.String)
		*/
		public virtual int getChildMultiplicity(System.String name)
		{
			return getChildrenWithName(name, false).size();
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#shallowCopy()
		*/
		public virtual TreeElement shallowCopy()
		{
			TreeElement newNode = new TreeElement(name, multiplicity);
			newNode.parent = parent;
			newNode.Repeatable = this.Repeatable;
			newNode.dataType = dataType;
			
			// Just set the flag? side effects?
			newNode.setMaskVar(MASK_RELEVANT, this.getMaskVar(MASK_RELEVANT));
			newNode.setMaskVar(MASK_REQUIRED, this.getMaskVar(MASK_REQUIRED));
			newNode.setMaskVar(MASK_ENABLED, this.getMaskVar(MASK_ENABLED));
			
			newNode.constraint = constraint;
			newNode.preloadHandler = preloadHandler;
			newNode.preloadParams = preloadParams;
			newNode.instanceName = instanceName;
			newNode.namespace_Renamed = namespace_Renamed;
			newNode.bindAttributes = bindAttributes;
			
			
			newNode.attributes = new List< TreeElement >();
			for (int i = 0; i < attributes.size(); i++)
			{
				TreeElement attr = (TreeElement) attributes.elementAt(i);
				newNode.setAttribute(attr.Namespace, attr.Name, attr.getAttributeValue());
			}
			
			if (value_Renamed != null)
			{
				newNode.value_Renamed = value_Renamed.Clone();
			}
			
			newNode.children = children;
			return newNode;
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#deepCopy(boolean)
		*/
		public virtual TreeElement deepCopy(bool includeTemplates)
		{
			TreeElement newNode = shallowCopy();
			
			if (children != null)
			{
				newNode.children = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				for (int i = 0; i < children.Count; i++)
				{
					TreeElement child = (TreeElement) children[i];
					if (includeTemplates || child.Mult != TreeReference.INDEX_TEMPLATE)
					{
						newNode.addChild(child.deepCopy(includeTemplates));
					}
				}
			}
			
			return newNode;
		}
		
		/* ==== MODEL PROPERTIES ==== */
		
		// factoring inheritance rules
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#isRelevant()
		*/
		public virtual bool isRelevant()
		{
			return getMaskVar(MASK_RELEVANT_INH) && getMaskVar(MASK_RELEVANT);
		}
		
		// factoring in inheritance rules
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#isEnabled()
		*/
		public virtual bool isEnabled()
		{
			return getMaskVar(MASK_ENABLED_INH) && getMaskVar(MASK_ENABLED);
		}
		
		/* ==== SPECIAL SETTERS (SETTERS WITH SIDE-EFFECTS) ==== */
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#setAnswer(org.javarosa.core.model.data.IAnswerData)
		*/
		public virtual bool setAnswer(IAnswerData answer)
		{
			if (value_Renamed != null || answer != null)
			{
				Value = answer;
				alertStateObservers(org.javarosa.core.model.FormElementStateListener_Fields.CHANGE_DATA);
				return true;
			}
			else
			{
				return false;
			}
		}
		
		private bool getMaskVar(int mask)
		{
			return (flags & mask) == mask;
		}
		
		private void  setMaskVar(int mask, bool value_Renamed)
		{
			if (value_Renamed)
			{
				flags = flags | mask;
			}
			else
			{
				flags = flags & (System.Int32.MaxValue - mask);
			}
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#setRelevant(boolean)
		*/
		public virtual void  setRelevant(bool relevant)
		{
			setRelevant(relevant, false);
		}
		
		private void  setRelevant(bool relevant, bool inherited)
		{
			bool oldRelevancy = isRelevant();
			if (inherited)
			{
				setMaskVar(MASK_RELEVANT_INH, relevant);
			}
			else
			{
				setMaskVar(MASK_RELEVANT, relevant);
			}
			
			bool newRelevant = isRelevant();
			if (newRelevant != oldRelevancy)
			{
				if (attributes != null)
				{
					for (int i = 0; i < attributes.size(); ++i)
					{
						attributes.elementAt(i).setRelevant(newRelevant, true);
					}
				}
				if (children != null)
				{
					for (int i = 0; i < children.Count; i++)
					{
						((TreeElement) children[i]).setRelevant(newRelevant, true);
					}
				}
				alertStateObservers(org.javarosa.core.model.FormElementStateListener_Fields.CHANGE_RELEVANT);
			}
		}
		
		
		public
		
		void setBindAttributes(List< TreeElement > bindAttributes)
		
		
		public List< TreeElement > getBindAttributes()
		
		/// <summary> Retrieves the TreeElement representing an arbitrary bind attribute
		/// for this element at the provided namespace and name, or null if none exists.
		/// 
		/// If 'null' is provided for the namespace, it will match the first
		/// attribute with the matching name.
		/// 
		/// </summary>
		/// <param name="index">
		/// </param>
		/// <returns> TreeElement
		/// </returns>
		public virtual TreeElement getBindAttribute(System.String namespace_Renamed, System.String name)
		{
			return getAttribute(bindAttributes, namespace_Renamed, name);
		}
		
		/// <summary> get value of the bind attribute with namespace:name' in the vector
		/// 
		/// </summary>
		/// <param name="index">
		/// </param>
		/// <returns> String
		/// </returns>
		public virtual System.String getBindAttributeValue(System.String namespace_Renamed, System.String name)
		{
			TreeElement element = getBindAttribute(namespace_Renamed, name);
			return element == null?null:getAttributeValue(element);
		}
		
		public virtual void  setBindAttribute(System.String namespace_Renamed, System.String name, System.String value_Renamed)
		{
			setAttribute(this, bindAttributes, namespace_Renamed, name, value_Renamed);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#setEnabled(boolean)
		*/
		public virtual void  setEnabled(bool enabled)
		{
			setEnabled(enabled, false);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#setEnabled(boolean, boolean)
		*/
		public virtual void  setEnabled(bool enabled, bool inherited)
		{
			bool oldEnabled = isEnabled();
			if (inherited)
			{
				setMaskVar(MASK_ENABLED_INH, enabled);
			}
			else
			{
				setMaskVar(MASK_ENABLED, enabled);
			}
			
			if (isEnabled() != oldEnabled)
			{
				if (children != null)
				{
					for (int i = 0; i < children.Count; i++)
					{
						((TreeElement) children[i]).setEnabled(isEnabled(), true);
					}
				}
				alertStateObservers(org.javarosa.core.model.FormElementStateListener_Fields.CHANGE_ENABLED);
			}
		}
		
		/* ==== OBSERVER PATTERN ==== */
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#registerStateObserver(org.javarosa.core.model.FormElementStateListener)
		*/
		public virtual void  registerStateObserver(FormElementStateListener qsl)
		{
			if (observers == null)
				observers = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			
			if (!observers.Contains(qsl))
			{
				observers.Add(qsl);
			}
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#unregisterStateObserver(org.javarosa.core.model.FormElementStateListener)
		*/
		public virtual void  unregisterStateObserver(FormElementStateListener qsl)
		{
			if (observers != null)
			{
				observers.Remove(qsl);
				if ((observers.Count == 0))
					observers = null;
			}
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#unregisterAll()
		*/
		public virtual void  unregisterAll()
		{
			observers = null;
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#alertStateObservers(int)
		*/
		public virtual void  alertStateObservers(int changeFlags)
		{
			if (observers != null)
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				for (System.Collections.IEnumerator e = observers.GetEnumerator(); e.MoveNext(); )
				{
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					((FormElementStateListener) e.Current).formElementStateChanged(this, changeFlags);
				}
			}
		}
		
		/* ==== VISITOR PATTERN ==== */
		
		/* (non-Javadoc)
		* Visitor pattern acceptance method.
		*
		* @param visitor
		*            The visitor traveling this tree
		* @see org.javarosa.core.model.instance.AbstractTreeElement#accept(org.javarosa.core.model.instance.utils.ITreeVisitor)
		*/
		public virtual void  accept(ITreeVisitor visitor)
		{
			visitor.visit(this);
			
			if (children == null)
			{
				return ;
			}
			System.Collections.IEnumerator en = children.GetEnumerator();
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			while (en.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				((TreeElement) en.Current).accept(visitor);
			}
		}
		
		/* ==== Attributes ==== */
		
		/* (non-Javadoc)
		* get namespace of attribute at 'index' in the vector
		*
		* @param index
		* @return String
		* @see org.javarosa.core.model.instance.AbstractTreeElement#getAttributeNamespace(int)
		*/
		public virtual System.String getAttributeNamespace(int index)
		{
			return attributes.elementAt(index).namespace_Renamed;
		}
		
		/* (non-Javadoc)
		* get name of attribute at 'index' in the vector
		*
		* @param index
		* @return String
		* @see org.javarosa.core.model.instance.AbstractTreeElement#getAttributeName(int)
		*/
		public virtual System.String getAttributeName(int index)
		{
			return attributes.elementAt(index).name;
		}
		
		/* (non-Javadoc)
		* get value of attribute at 'index' in the vector
		*
		* @param index
		* @return String
		* @see org.javarosa.core.model.instance.AbstractTreeElement#getAttributeValue(int)
		*/
		public virtual System.String getAttributeValue(int index)
		{
			return getAttributeValue(attributes.elementAt(index));
		}
		
		/// <summary> Get the String value of the provided attribute
		/// 
		/// </summary>
		/// <param name="attribute">
		/// </param>
		/// <returns>
		/// </returns>
		private System.String getAttributeValue(TreeElement attribute)
		{
			if (attribute.Value == null)
			{
				return null;
			}
			else
			{
				return attribute.Value.uncast().String;
			}
		}
		
		public virtual System.String getAttributeValue()
		{
			if (!isAttribute())
			{
				throw new System.SystemException("this is not an attribute");
			}
			return Value.uncast().String;
		}
		
		/* (non-Javadoc)
		* Retrieves the TreeElement representing the attribute at
		* the provided namespace and name, or null if none exists.
		*
		* If 'null' is provided for the namespace, it will match the first
		* attribute with the matching name.
		*
		* @param index
		* @return TreeElement
		* @see org.javarosa.core.model.instance.AbstractTreeElement#getAttribute(java.lang.String, java.lang.String)
		*/
		public virtual TreeElement getAttribute(System.String namespace_Renamed, System.String name)
		{
			return getAttribute(attributes, namespace_Renamed, name);
		}
		
		/* (non-Javadoc)
		* get value of attribute with namespace:name' in the vector
		*
		* @param index
		* @return String
		* @see org.javarosa.core.model.instance.AbstractTreeElement#getAttributeValue(java.lang.String, java.lang.String)
		*/
		public virtual System.String getAttributeValue(System.String namespace_Renamed, System.String name)
		{
			TreeElement element = getAttribute(namespace_Renamed, name);
			return element == null?null:getAttributeValue(element);
		}
		
		/* (non-Javadoc)
		* Sets the given attribute; a value of null removes the attribute
		*
		* @see org.javarosa.core.model.instance.AbstractTreeElement#setAttribute(java.lang.String, java.lang.String, java.lang.String)
		*/
		public virtual void  setAttribute(System.String namespace_Renamed, System.String name, System.String value_Renamed)
		{
			setAttribute(this, attributes, namespace_Renamed, name, value_Renamed);
		}
		
		/* ==== SERIALIZATION ==== */
		
		/*
		* TODO:
		*
		* this new serialization scheme is kind of lame. ideally, we shouldn't have
		* to sub-class TreeElement at all; we should have an API that can
		* seamlessly represent complex data model objects (like weight history or
		* immunizations) as if they were explicity XML subtrees underneath the
		* parent TreeElement
		*
		* failing that, we should wrap this scheme in an ExternalizableWrapper
		*/
		
		/*
		* (non-Javadoc)
		*
		* @see
		* org.javarosa.core.services.storage.utilities.Externalizable#readExternal
		* (java.io.DataInputStream)
		*/
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#readExternal(java.io.DataInputStream, org.javarosa.core.util.externalizable.PrototypeFactory)
		*/
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			name = ExtUtil.nullIfEmpty(ExtUtil.readString(in_Renamed));
			multiplicity = ExtUtil.readInt(in_Renamed);
			flags = ExtUtil.readInt(in_Renamed);
			value_Renamed = (IAnswerData) ExtUtil.read(in_Renamed, new ExtWrapNullable(new ExtWrapTagged()), pf);
			
			// children = ExtUtil.nullIfEmpty((Vector)ExtUtil.read(in, new
			// ExtWrapList(TreeElement.class), pf));
			
			// Jan 22, 2009 - csims@dimagi.com
			// old line: children = ExtUtil.nullIfEmpty((Vector)ExtUtil.read(in, new
			// ExtWrapList(TreeElement.class), pf));
			// New Child deserialization
			// 1. read null status as boolean
			// 2. read number of children
			// 3. for i < number of children
			// 3.1 if read boolean true , then create TreeElement and deserialize
			// directly.
			// 3.2 if read boolean false then create tagged element and deserialize
			// child
			if (!ExtUtil.readBool(in_Renamed))
			{
				// 1.
				children = null;
			}
			else
			{
				children = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				// 2.
				int numChildren = (int) ExtUtil.readNumeric(in_Renamed);
				// 3.
				for (int i = 0; i < numChildren; ++i)
				{
					bool normal = ExtUtil.readBool(in_Renamed);
					TreeElement child;
					
					if (normal)
					{
						// 3.1
						child = new TreeElement();
						child.readExternal(in_Renamed, pf);
					}
					else
					{
						// 3.2
						child = (TreeElement) ExtUtil.read(in_Renamed, new ExtWrapTagged(), pf);
					}
					child.Parent = this;
					children.Add(child);
				}
			}
			
			// end Jan 22, 2009
			
			dataType = ExtUtil.readInt(in_Renamed);
			instanceName = ExtUtil.nullIfEmpty(ExtUtil.readString(in_Renamed));
			constraint = (Constraint) ExtUtil.read(in_Renamed, new ExtWrapNullable(typeof(Constraint)), pf);
			preloadHandler = ExtUtil.nullIfEmpty(ExtUtil.readString(in_Renamed));
			preloadParams = ExtUtil.nullIfEmpty(ExtUtil.readString(in_Renamed));
			namespace_Renamed = ExtUtil.nullIfEmpty(ExtUtil.readString(in_Renamed));
			
			bindAttributes = ExtUtil.readAttributes(in_Renamed, this);
			
			attributes = ExtUtil.readAttributes(in_Renamed, this);
		}
		
		/*
		* (non-Javadoc)
		*
		* @see
		* org.javarosa.core.services.storage.utilities.Externalizable#writeExternal
		* (java.io.DataOutputStream)
		*/
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#writeExternal(java.io.DataOutputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeString(out_Renamed, ExtUtil.emptyIfNull(name));
			ExtUtil.writeNumeric(out_Renamed, multiplicity);
			ExtUtil.writeNumeric(out_Renamed, flags);
			ExtUtil.write(out_Renamed, new ExtWrapNullable(value_Renamed == null?null:new ExtWrapTagged(value_Renamed)));
			
			// Jan 22, 2009 - csims@dimagi.com
			// old line: ExtUtil.write(out, new
			// ExtWrapList(ExtUtil.emptyIfNull(children)));
			// New Child serialization
			// 1. write null status as boolean
			// 2. write number of children
			// 3. for all child in children
			// 3.1 if child type == TreeElement write boolean true , then serialize
			// directly.
			// 3.2 if child type != TreeElement, write boolean false, then tagged
			// child
			if (children == null)
			{
				// 1.
				ExtUtil.writeBool(out_Renamed, false);
			}
			else
			{
				// 1.
				ExtUtil.writeBool(out_Renamed, true);
				// 2.
				ExtUtil.writeNumeric(out_Renamed, children.Count);
				// 3.
				System.Collections.IEnumerator en = children.GetEnumerator();
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				while (en.MoveNext())
				{
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					TreeElement child = (TreeElement) en.Current;
					if (child.GetType() == typeof(TreeElement))
					{
						// 3.1
						ExtUtil.writeBool(out_Renamed, true);
						child.writeExternal(out_Renamed);
					}
					else
					{
						// 3.2
						ExtUtil.writeBool(out_Renamed, false);
						ExtUtil.write(out_Renamed, new ExtWrapTagged(child));
					}
				}
			}
			
			// end Jan 22, 2009
			
			ExtUtil.writeNumeric(out_Renamed, dataType);
			ExtUtil.writeString(out_Renamed, ExtUtil.emptyIfNull(instanceName));
			ExtUtil.write(out_Renamed, new ExtWrapNullable(constraint)); // TODO: inefficient for repeats
			ExtUtil.writeString(out_Renamed, ExtUtil.emptyIfNull(preloadHandler));
			ExtUtil.writeString(out_Renamed, ExtUtil.emptyIfNull(preloadParams));
			ExtUtil.writeString(out_Renamed, ExtUtil.emptyIfNull(namespace_Renamed));
			
			ExtUtil.writeAttributes(out_Renamed, bindAttributes);
			
			ExtUtil.writeAttributes(out_Renamed, attributes);
		}
		
		//rebuilding a node from an imported instance
		//  there's a lot of error checking we could do on the received instance, but it's
		//  easier to just ignore the parts that are incorrect
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#populate(org.javarosa.core.model.instance.TreeElement, org.javarosa.core.model.FormDef)
		*/
		public virtual void  populate(TreeElement incoming, FormDef f)
		{
			if (this.Leaf)
			{
				// check that incoming doesn't have children?
				
				IAnswerData value_Renamed = incoming.Value;
				if (value_Renamed == null)
				{
					this.Value = null;
				}
				else if (this.dataType == Constants.DATATYPE_TEXT || this.dataType == Constants.DATATYPE_NULL)
				{
					this.Value = value_Renamed; // value is a StringData
				}
				else
				{
					System.String textVal = (System.String) value_Renamed.Value;
					
					// if there is no other IAnswerResolver, use the default one.
					IAnswerResolver answerResolver = XFormParser.AnswerResolver;
					if (answerResolver == null)
					{
						answerResolver = new DefaultAnswerResolver();
					}
					this.Value = answerResolver.resolveAnswer(textVal, this, f);
				}
			}
			else
			{
				System.Collections.ArrayList names = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				for (int i = 0; i < this.NumChildren; i++)
				{
					TreeElement child = this.getChildAt(i);
					if (!names.Contains(child.Name))
					{
						names.Add(child.Name);
					}
				}
				
				// remove all default repetitions from skeleton data model (_preserving_ templates, though)
				for (int i = 0; i < this.NumChildren; i++)
				{
					TreeElement child = this.getChildAt(i);
					if (child.getMaskVar(MASK_REPEATABLE) && child.Mult != TreeReference.INDEX_TEMPLATE)
					{
						this.removeChildAt(i);
						i--;
					}
				}
				
				// make sure ordering is preserved (needed for compliance with xsd schema)
				if (this.NumChildren != names.Count)
				{
					throw new System.SystemException("sanity check failed");
				}
				
				for (int i = 0; i < this.NumChildren; i++)
				{
					TreeElement child = this.getChildAt(i);
					System.String expectedName = (System.String) names[i];
					
					if (!child.Name.Equals(expectedName))
					{
						TreeElement child2 = null;
						int j;
						
						for (j = i + 1; j < this.NumChildren; j++)
						{
							child2 = this.getChildAt(j);
							if (child2.Name.Equals(expectedName))
							{
								break;
							}
						}
						if (j == this.NumChildren)
						{
							throw new System.SystemException("sanity check failed");
						}
						
						this.removeChildAt(j);
						if (children == null)
						{
							children = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
						}
						this.children.Insert(i, child2);
					}
				}
				// java i hate you so much
				
				for (int i = 0; i < this.NumChildren; i++)
				{
					TreeElement child = this.getChildAt(i);
					System.Collections.ArrayList newChildren = incoming.getChildrenWithName(child.Name);
					
					if (child.getMaskVar(MASK_REPEATABLE))
					{
						for (int k = 0; k < newChildren.Count; k++)
						{
							TreeElement newChild = child.deepCopy(true);
							newChild.Mult = k;
							if (children == null)
							{
								children = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
							}
							this.children.Insert(i + k + 1, newChild);
							newChild.populate((TreeElement) newChildren[k], f);
						}
						i += newChildren.Count;
					}
					else
					{
						
						if (newChildren.Count == 0)
						{
							child.setRelevant(false);
						}
						else
						{
							child.populate((TreeElement) newChildren[0], f);
						}
					}
				}
				for (int i = 0; i < incoming.AttributeCount; i++)
				{
					System.String name = incoming.getAttributeName(i);
					System.String ns = incoming.getAttributeNamespace(i);
					System.String value_Renamed = incoming.getAttributeValue(i);
					
					this.setAttribute(ns, name, value_Renamed);
				}
			}
		}
		
		//this method is for copying in the answers to an itemset. the template node of the destination
		//is used for overall structure (including data types), and the itemset source node is used for
		//raw data. note that data may be coerced across types, which may result in type conversion error
		//very similar in structure to populate()
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.AbstractTreeElement#populateTemplate(org.javarosa.core.model.instance.TreeElement, org.javarosa.core.model.FormDef)
		*/
		public virtual void  populateTemplate(TreeElement incoming, FormDef f)
		{
			if (this.Leaf)
			{
				IAnswerData value_Renamed = incoming.Value;
				if (value_Renamed == null)
				{
					this.Value = null;
				}
				else
				{
					System.Type classType = CompactInstanceWrapper.classForDataType(this.dataType);
					
					if (classType == null)
					{
						throw new System.SystemException("data type [" + value_Renamed.GetType().FullName + "] not supported inside itemset");
					}
					else if (classType.IsAssignableFrom(value_Renamed.GetType()) && !(value_Renamed is SelectOneData || value_Renamed is SelectMultiData))
					{
						this.Value = value_Renamed;
					}
					else
					{
						System.String textVal = RestoreUtils.xfFact.serializeData(value_Renamed);
						IAnswerData typedVal = RestoreUtils.xfFact.parseData(textVal, this.dataType, this.Ref, f);
						this.Value = typedVal;
					}
				}
			}
			else
			{
				for (int i = 0; i < this.NumChildren; i++)
				{
					TreeElement child = this.getChildAt(i);
					System.Collections.ArrayList newChildren = incoming.getChildrenWithName(child.Name);
					
					if (child.getMaskVar(MASK_REPEATABLE))
					{
						for (int k = 0; k < newChildren.Count; k++)
						{
							TreeElement template = f.MainInstance.getTemplate(child.Ref);
							TreeElement newChild = template.deepCopy(false);
							newChild.Mult = k;
							if (children == null)
							{
								children = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
							}
							this.children.Insert(i + k + 1, newChild);
							newChild.populateTemplate((TreeElement) newChildren[k], f);
						}
						i += newChildren.Count;
					}
					else
					{
						child.populateTemplate((TreeElement) newChildren[0], f);
					}
				}
			}
		}
		
		//TODO: This is probably silly because this object is likely already
		//not thread safe in any way. Also, we should be wrapping all of the
		//setters.
		internal TreeReference[] refCache = new TreeReference[1];
		
		private void  expireReferenceCache()
		{
			lock (refCache)
			{
				refCache[0] = null;
			}
		}
		
		public static TreeReference BuildRef(AbstractTreeElement elem)
		{
			TreeReference ref_Renamed = TreeReference.selfRef();
			
			while (elem != null)
			{
				TreeReference step;
				
				if (elem.Name != null)
				{
					step = TreeReference.selfRef();
					step.add(elem.Name, elem.Mult);
				}
				else
				{
					step = TreeReference.rootRef();
					//All TreeElements are part of a consistent tree, so the root should be in the same instance
				}
				
				step.InstanceName = elem.InstanceName;
				if (elem.InstanceName != null)
				{
					// it is a named instance; it should not inherit runtime context...
					step.Context = TreeReference.CONTEXT_INSTANCE;
				}
				
				ref_Renamed = ref_Renamed.parent(step);
				elem = elem.Parent;
			}
			return ref_Renamed;
		}
		
		public static int CalculateDepth(AbstractTreeElement elem)
		{
			int depth = 0;
			
			while (elem.Name != null)
			{
				depth++;
				elem = elem.Parent;
			}
			
			return depth;
		}
		
		/* (non-Javadoc)
		* Because I'm tired of not knowing what a TreeElement object has just by looking at it.
		* @see org.javarosa.core.model.instance.AbstractTreeElement#toString()
		*/
		public override System.String ToString()
		{
			System.String name = "NULL";
			if (this.name != null)
			{
				name = this.name;
			}
			
			System.String childrenCount = "-1";
			if (this.children != null)
			{
				childrenCount = System.Convert.ToString(this.children.Count);
			}
			
			return name + " - Children: " + childrenCount;
		}
		
		public virtual void  clearCaches()
		{
			expireReferenceCache();
		}
		
		
		public List< TreeReference > tryBatchChildFetch(String name, int mult, List< XPathExpression > predicates, EvaluationContext evalContext)
	}
}