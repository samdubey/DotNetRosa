using System;
using IDataReference = org.javarosa.core.model.IDataReference;
using EvaluationContext = org.javarosa.core.model.condition.EvaluationContext;
using Persistable = org.javarosa.core.services.storage.Persistable;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapNullable = org.javarosa.core.util.externalizable.ExtWrapNullable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using XPathExpression = org.javarosa.xpath.expr.XPathExpression;
namespace org.javarosa.core.model.instance
{
	
	/// <summary> A data instance represents a tree structure of abstract tree
	/// elements which can be accessed and read with tree references. It is
	/// a supertype of different types of concrete models which may or may not
	/// be read only.
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public abstract class DataInstance
	{
		/// <summary>The integer Id of the model </summary>
		private void  InitBlock()
		{
			
			
		}
		/// <summary> Whether the structure of this instance is only available at runtime.
		/// 
		/// </summary>
		/// <returns> true if the instance structure is available and runtime and can't
		/// be checked for consistency until the reference is made available. False
		/// otherwise.
		/// 
		/// </returns>
		virtual public bool RuntimeEvaluated
		{
			get
			{
				return false;
			}
			
		}
		virtual public int FormId
		{
			get
			{
				return this.formId;
			}
			
			set
			{
				this.formId = value;
			}
			
		}
		virtual public System.String Name
		{
			get
			{
				return name;
			}
			
			set
			{
				this.name = value;
			}
			
		}
		virtual public int ID
		{
			get
			{
				return recordid;
			}
			
			set
			{
				this.recordid = value;
			}
			
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		< T extends AbstractTreeElement < T >> implements Persistable
		private int recordid = - 1;
		
		/// <summary>The name for this data model </summary>
		private System.String name;
		/// <summary>The ID of the form that this is a model for </summary>
		private int formId;
		
		private System.String instanceid;
		
		public DataInstance()
		{
			InitBlock();
		}
		
		
		public DataInstance(System.String instanceid)
		{
			InitBlock();
			this.instanceid = instanceid;
		}
		
		public static TreeReference unpackReference(IDataReference ref_Renamed)
		{
			return (TreeReference) ref_Renamed.Reference;
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		abstract AbstractTreeElement < T > getBase();
		
		public abstract T getRoot();
		
		public virtual System.String getInstanceId()
		{
			return instanceid;
		}
		
		protected internal virtual void  setInstanceId(System.String instanceid)
		{
			this.instanceid = instanceid;
		}
		
		public virtual T resolveReference(TreeReference ref_Renamed)
		{
			if (!ref_Renamed.Absolute)
			{
				return null;
			}
			
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			T result = null;
			for (int i = 0; i < ref_Renamed.size(); i++)
			{
				System.String name = ref_Renamed.getName(i);
				int mult = ref_Renamed.getMultiplicity(i);
				
				if (mult == TreeReference.INDEX_ATTRIBUTE)
				{
					//Should we possibly just return here?
					//I guess technically we could step back...
					node = result = node.getAttribute(null, name);
					continue;
				}
				if (mult == TreeReference.INDEX_UNBOUND)
				{
					if (node.getChildMultiplicity(name) == 1)
					{
						mult = 0;
					}
					else
					{
						// reference is not unambiguous
						node = result = null;
						break;
					}
				}
				
				node = result = node.getChild(name, mult);
				if (node == null)
				{
					break;
				}
			}
			
			return (node == getBase()?null:result); // never return a reference to '/'
		}
		
		public virtual System.Collections.ArrayList explodeReference(TreeReference ref_Renamed)
		{
			if (!ref_Renamed.Absolute)
				return null;
			
			System.Collections.ArrayList nodes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			for (int i = 0; i < ref_Renamed.size(); i++)
			{
				System.String name = ref_Renamed.getName(i);
				int mult = ref_Renamed.getMultiplicity(i);
				
				//If the next node down the line is an attribute
				if (mult == TreeReference.INDEX_ATTRIBUTE)
				{
					//This is not the attribute we're testing
					if (cur != getBase())
					{
						//Add the current node
						nodes.Add(cur);
					}
					cur = cur.getAttribute(null, name);
				}
				//Otherwise, it's another child element
				else
				{
					if (mult == TreeReference.INDEX_UNBOUND)
					{
						if (cur.getChildMultiplicity(name) == 1)
						{
							mult = 0;
						}
						else
						{
							// reference is not unambiguous
							return null;
						}
					}
					
					if (cur != getBase())
					{
						nodes.Add(cur);
					}
					
					cur = cur.getChild(name, mult);
					if (cur == null)
					{
						return null;
					}
				}
			}
			return nodes;
		}
		
		public virtual T getTemplate(TreeReference ref_Renamed)
		{
			T node = getTemplatePath(ref_Renamed);
			return (node == null?null:((node.isRepeatable() || node.isAttribute())?node:null));
		}
		
		public virtual T getTemplatePath(TreeReference ref_Renamed)
		{
			if (!ref_Renamed.Absolute)
				return null;
			
			T walker = null;
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			for (int i = 0; i < ref_Renamed.size(); i++)
			{
				System.String name = ref_Renamed.getName(i);
				
				if (ref_Renamed.getMultiplicity(i) == TreeReference.INDEX_ATTRIBUTE)
				{
					node = walker = node.getAttribute(null, name);
				}
				else
				{
					
					T newNode = node.getChild(name, TreeReference.INDEX_TEMPLATE);
					if (newNode == null)
					{
						newNode = node.getChild(name, 0);
					}
					if (newNode == null)
					{
						return null;
					}
					node = walker = newNode;
				}
			}
			
			return walker;
		}
		
		public virtual T resolveReference(IDataReference binding)
		{
			return resolveReference(unpackReference(binding));
		}
		
		public override System.String ToString()
		{
			System.String name = "NULL";
			if (this.name != null)
			{
				name = this.name;
			}
			return name;
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			recordid = ExtUtil.readInt(in_Renamed);
			formId = ExtUtil.readInt(in_Renamed);
			name = ((System.String) ExtUtil.read(in_Renamed, new ExtWrapNullable(typeof(System.String)), pf));
			instanceid = ((System.String) ExtUtil.nullIfEmpty(ExtUtil.readString(in_Renamed)));
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeNumeric(out_Renamed, recordid);
			ExtUtil.writeNumeric(out_Renamed, formId);
			ExtUtil.write(out_Renamed, new ExtWrapNullable(name));
			ExtUtil.write(out_Renamed, ExtUtil.emptyIfNull(instanceid));
		}
		
		
		
		public abstract void  initialize(InstanceInitializationFactory initializer, System.String instanceId);
	}
}