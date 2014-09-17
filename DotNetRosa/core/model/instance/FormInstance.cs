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
using IDataReference = org.javarosa.core.model.IDataReference;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using ITreeVisitor = org.javarosa.core.model.instance.utils.ITreeVisitor;
using Restorable = org.javarosa.core.model.util.restorable.Restorable;
using RestoreUtils = org.javarosa.core.model.util.restorable.RestoreUtils;
using IInstanceVisitor = org.javarosa.core.model.utils.IInstanceVisitor;
using IMetaData = org.javarosa.core.services.storage.IMetaData;
using Persistable = org.javarosa.core.services.storage.Persistable;
using org.javarosa.core.util.externalizable;
namespace org.javarosa.core.model.instance
{
	
	
	/// <summary> This class represents the xform model instance</summary>
	public class FormInstance:DataInstance, System.ICloneable
	{
		private void  InitBlock()
		{
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			HashMap < String, Object > data = new HashMap < String, Object >();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(String key: getMetaDataFields())
			{
				data.put(key, getMetaData(key));
			}
			return data;
		}
		virtual public TreeElement Base
		{
			get
			{
				return root;
			}
			
		}
		virtual public System.DateTime DateSaved
		{
			get
			{
				return this.dateSaved;
			}
			
			set
			{
				this.dateSaved = value;
			}
			
		}
		virtual public System.String[] NamespacePrefixes
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.instance.FormInstanceAdapter#getNamespacePrefixes()
			*/
			
			get
			{
				System.String[] prefixes = new System.String[namespaces.size()];
				int i = 0;
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				for(String key: namespaces.keySet())
				{
					prefixes[i] = key;
					++i;
				}
				return prefixes;
			}
			
		}
		virtual public System.String[] MetaDataFields
		{
			get
			{
				return new System.String[]{META_XMLNS, META_ID};
			}
			
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		< TreeElement > implements Persistable, IMetaData
		
		public const System.String STORAGE_KEY = "FORMDATA";
		
		/// <summary>The date that this model was taken and recorded </summary>
		private System.DateTime dateSaved;
		
		public System.String schema;
		public System.String formVersion;
		public System.String uiVersion;
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private HashMap < String, Object > namespaces = new HashMap < String, Object >();
		
		/// <summary>The root of this tree </summary>
		protected internal TreeElement root = new TreeElement();
		
		public FormInstance()
		{
			InitBlock();
		}
		
		public FormInstance(TreeElement root):this(root, null)
		{
		}
		
		/// <summary> Creates a new data model using the root given.
		/// 
		/// </summary>
		/// <param name="root">The root of the tree for this data model.
		/// </param>
		public FormInstance(TreeElement root, System.String id):base(id)
		{
			InitBlock();
			ID = - 1;
			FormId = - 1;
			setRoot(root);
		}
		
		public override TreeElement getRoot()
		{
			
			if (root.NumChildren == 0)
				throw new System.SystemException("root node has no children");
			
			return root.getChildAt(0);
		}
		
		
		
		/// <summary> Sets the root element of this Model's tree
		/// 
		/// </summary>
		/// <param name="topLevel">The root of the tree for this data model.
		/// </param>
		public virtual void  setRoot(TreeElement topLevel)
		{
			root = new TreeElement();
			if (this.Name != null)
			{
				root.InstanceName = this.Name;
			}
			if (topLevel != null)
			{
				root.addChild(topLevel);
			}
		}
		
		public virtual TreeReference copyNode(TreeReference from, TreeReference to)
		{
			if (!from.Absolute)
			{
				throw new InvalidReferenceException("Source reference must be absolute for copying", from);
			}
			
			TreeElement src = resolveReference(from);
			if (src == null)
			{
				throw new InvalidReferenceException("Null Source reference while attempting to copy node", from);
			}
			
			return copyNode(src, to).Ref;
		}
		
		// for making new repeat instances; 'from' and 'to' must be unambiguous
		// references EXCEPT 'to' may be ambiguous at its final step
		// return true is successfully copied, false otherwise
		public virtual TreeElement copyNode(TreeElement src, TreeReference to)
		{
			if (!to.Absolute)
				throw new InvalidReferenceException("Destination reference must be absolute for copying", to);
			
			// strip out dest node info and get dest parent
			System.String dstName = to.NameLast;
			int dstMult = to.MultLast;
			TreeReference toParent = to.ParentRef;
			
			TreeElement parent = resolveReference(toParent);
			if (parent == null)
			{
				throw new InvalidReferenceException("Null parent reference whle attempting to copy", toParent);
			}
			if (!parent.Childable)
			{
				throw new InvalidReferenceException("Invalid Parent Node: cannot accept children.", toParent);
			}
			
			if (dstMult == TreeReference.INDEX_UNBOUND)
			{
				dstMult = parent.getChildMultiplicity(dstName);
			}
			else if (parent.getChild(dstName, dstMult) != null)
			{
				throw new InvalidReferenceException("Destination already exists!", to);
			}
			
			TreeElement dest = src.deepCopy(false);
			dest.Name = dstName;
			dest.Mult = dstMult;
			parent.addChild(dest);
			return dest;
		}
		
		public virtual TreeReference addNode(TreeReference ambigRef)
		{
			TreeReference ref_Renamed = ambigRef.Clone();
			if (createNode(ref_Renamed) != null)
			{
				return ref_Renamed;
			}
			else
			{
				return null;
			}
		}
		
		public virtual TreeReference addNode(TreeReference ambigRef, IAnswerData data, int dataType)
		{
			TreeReference ref_Renamed = ambigRef.Clone();
			TreeElement node = createNode(ref_Renamed);
			if (node != null)
			{
				if (dataType >= 0)
				{
					node.DataType = dataType;
				}
				
				node.Value = data;
				return ref_Renamed;
			}
			else
			{
				return null;
			}
		}
		
		/*
		* create the specified node in the tree, creating all intermediary nodes at
		* each step, if necessary. if specified node already exists, return null
		*
		* creating a duplicate node is only allowed at the final step. it will be
		* done if the multiplicity of the last step is ALL or equal to the count of
		* nodes already there
		*
		* at intermediate steps, the specified existing node is used; if
		* multiplicity is ALL: if no nodes exist, a new one is created; if one node
		* exists, it is used; if multiple nodes exist, it's an error
		*
		* return the newly-created node; modify ref so that it's an unambiguous ref
		* to the node
		*/
		private TreeElement createNode(TreeReference ref_Renamed)
		{
			
			TreeElement node = root;
			
			for (int k = 0; k < ref_Renamed.size(); k++)
			{
				System.String name = ref_Renamed.getName(k);
				int count = node.getChildMultiplicity(name);
				int mult = ref_Renamed.getMultiplicity(k);
				
				TreeElement child;
				if (k < ref_Renamed.size() - 1)
				{
					if (mult == TreeReference.INDEX_UNBOUND)
					{
						if (count > 1)
						{
							return null; // don't know which node to use
						}
						else
						{
							// will use existing (if one and only one) or create new
							mult = 0;
							ref_Renamed.setMultiplicity(k, 0);
						}
					}
					
					// fetch
					child = node.getChild(name, mult);
					if (child == null)
					{
						if (mult == 0)
						{
							// create
							child = new TreeElement(name, count);
							node.addChild(child);
							ref_Renamed.setMultiplicity(k, count);
						}
						else
						{
							return null; // intermediate node does not exist
						}
					}
				}
				else
				{
					if (mult == TreeReference.INDEX_UNBOUND || mult == count)
					{
						if (k == 0 && root.NumChildren != 0)
						{
							return null; // can only be one top-level node, and it
							// already exists
						}
						
						if (!node.Childable)
						{
							return null; // current node can't have children
						}
						
						// create new
						child = new TreeElement(name, count);
						node.addChild(child);
						ref_Renamed.setMultiplicity(k, count);
					}
					else
					{
						return null; // final node must be a newly-created node
					}
				}
				
				node = child;
			}
			
			return node;
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.FormInstanceAdapter#addNamespace(java.lang.String, java.lang.String)
		*/
		public virtual void  addNamespace(System.String prefix, System.String URI)
		{
			namespaces.put(prefix, URI);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.instance.FormInstanceAdapter#getNamespaceURI(java.lang.String)
		*/
		public virtual System.String getNamespaceURI(System.String prefix)
		{
			return (System.String) namespaces.get_Renamed(prefix);
		}
		
		public virtual TreeElement processSaved(FormInstance template, FormDef f)
		{
			TreeElement fixedInstanceRoot = template.getRoot().deepCopy(true);
			TreeElement incomingRoot = root.getChildAt(0);
			
			if (!fixedInstanceRoot.Name.Equals(incomingRoot.Name) || incomingRoot.Mult != 0)
			{
				throw new System.SystemException("Saved form instance to restore does not match form definition");
			}
			
			fixedInstanceRoot.populate(incomingRoot, f);
			return fixedInstanceRoot;
		}
		
		
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.lang.Object.clone' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		public virtual System.Object Clone()
		{
			FormInstance cloned = new FormInstance(this.getRoot().deepCopy(true));
			
			cloned.ID = this.ID;
			cloned.FormId = this.FormId;
			cloned.Name = this.Name;
			cloned.DateSaved = this.DateSaved;
			cloned.schema = this.schema;
			cloned.formVersion = this.formVersion;
			cloned.uiVersion = this.uiVersion;
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			cloned.namespaces = new HashMap < String, Object >();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(String key: namespaces.keySet())
			{
				cloned.namespaces.put(key, this.namespaces.get_Renamed(key));
			}
			
			return cloned;
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			base.readExternal(in_Renamed, pf);
			schema = ((System.String) ExtUtil.read(in_Renamed, new ExtWrapNullable(typeof(System.String)), pf));
			dateSaved = (System.DateTime) ExtUtil.read(in_Renamed, new ExtWrapNullable(typeof(System.DateTime)), pf);
			
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			namespaces = (System.Collections.Hashtable) ExtUtil.read(in_Renamed, new ExtWrapMap(typeof(System.String), typeof(System.String)));
			setRoot((TreeElement) ExtUtil.read(in_Renamed, typeof(TreeElement), pf));
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			base.writeExternal(out_Renamed);
			ExtUtil.write(out_Renamed, new ExtWrapNullable(schema));
			ExtUtil.write(out_Renamed, new ExtWrapNullable(dateSaved));
			ExtUtil.write(out_Renamed, new ExtWrapMap(namespaces));
			
			ExtUtil.write(out_Renamed, getRoot());
		}
		
		public virtual void  copyItemsetNode(TreeElement copyNode, TreeReference destRef, FormDef f)
		{
			TreeElement templateNode = getTemplate(destRef);
			TreeElement newNode = copyNode(templateNode, destRef);
			newNode.populateTemplate(copyNode, f);
		}
		
		public virtual void  accept(IInstanceVisitor visitor)
		{
			visitor.visit(this);
			
			if (visitor is ITreeVisitor)
			{
				root.accept((ITreeVisitor) visitor);
			}
		}
		
		
		// determine if nodes are homogeneous, meaning their descendant structure is 'identical' for repeat purposes
		// identical means all children match, and the children's children match, and so on
		// repeatable children are ignored; as they do not have to exist in the same quantity for nodes to be homogeneous
		// however, the child repeatable nodes MUST be verified amongst themselves for homogeneity later
		// this function ignores the names of the two nodes
		public static bool isHomogeneous(TreeElement a, TreeElement b)
		{
			if (a.Leaf && b.Leaf)
			{
				return true;
			}
			else if (a.Childable && b.Childable)
			{
				// verify that every (non-repeatable) node in a exists in b and vice
				// versa
				for (int k = 0; k < 2; k++)
				{
					TreeElement n1 = (k == 0?a:b);
					TreeElement n2 = (k == 0?b:a);
					
					for (int i = 0; i < n1.NumChildren; i++)
					{
						TreeElement child1 = n1.getChildAt(i);
						if (child1.Repeatable)
							continue;
						TreeElement child2 = n2.getChild(child1.Name, 0);
						if (child2 == null)
							return false;
						if (child2.Repeatable)
							throw new System.SystemException("shouldn't happen");
					}
				}
				
				// compare children
				for (int i = 0; i < a.NumChildren; i++)
				{
					TreeElement childA = a.getChildAt(i);
					if (childA.Repeatable)
						continue;
					TreeElement childB = b.getChild(childA.Name, 0);
					if (!isHomogeneous(childA, childB))
						return false;
				}
				
				return true;
			}
			else
			{
				return false;
			}
		}
		
		public override void  initialize(InstanceInitializationFactory initializer, System.String instanceId)
		{
			setInstanceId(instanceId);
			root.InstanceName = instanceId;
		}
		
		public const System.String META_XMLNS = "XMLNS";
		public const System.String META_ID = "instance_id";
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public HashMap < String, Object > getMetaData()
		
		public virtual System.Object getMetaData(System.String fieldName)
		{
			if (META_XMLNS.Equals(fieldName))
			{
				return ExtUtil.emptyIfNull(schema);
			}
			else if (META_ID.Equals(fieldName))
			{
				return ExtUtil.emptyIfNull(this.getInstanceId());
			}
			throw new System.ArgumentException("No metadata field " + fieldName + " in the form instance storage system");
		}
	}
}