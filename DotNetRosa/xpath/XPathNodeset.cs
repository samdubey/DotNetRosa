using System;
using EvaluationContext = org.javarosa.core.model.condition.EvaluationContext;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using XPathPathExpr = org.javarosa.xpath.expr.XPathPathExpr;
namespace org.javarosa.xpath
{
	
	/// <summary> Represents a set of XPath nodes returned from a path or other operation which acts on multiple
	/// paths.
	/// 
	/// Current encompasses two states.
	/// 
	/// 1) A nodeset which references between 0 and N nodes which are known about (but, for instance,
	/// don't match any predicates or are irrelevant). Some operations cannot be evaluated in this state
	/// directly. If more than one node is referenced, it is impossible to return a normal evaluation, for
	/// instance.
	/// 
	/// 2) A nodeset which wasn't able to reference into any known model (generally a reference which is
	/// written in error). In this state, the size of the nodeset can be evaluated, but the acual reference
	/// cannot be returned, since it doesn't have any semantic value.
	/// 
	/// (2) may be a deviation from normal XPath. This should be evaluated in the future.
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public class XPathNodeset
	{
		private void  InitBlock()
		{
			if (nodes == null)
			{
				throw new System.NullReferenceException("Node list cannot be null when constructing a nodeset");
			}
			this.nodes = nodes;
			this.instance = instance;
			this.ec = ec;
			this.nodes = nodes;
			return this.nodes;
		}
		virtual protected internal XPathTypeMismatchException InvalidNodesetException
		{
			get
			{
				if (!pathEvaluated.Equals(originalPath))
				{
					throw new XPathTypeMismatchException("The path " + originalPath + " refers to the location " + pathEvaluated + " which was not found");
				}
				else
				{
					throw new XPathTypeMismatchException("Location " + pathEvaluated + " was not found");
				}
			}
			
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private Vector < TreeReference > nodes;
		protected internal FormInstance instance;
		protected internal EvaluationContext ec;
		// these are purely for improved error messages
		private System.String pathEvaluated;
		private System.String originalPath;
		
		private XPathNodeset()
		{
			InitBlock();
		}
		
		/// <summary> for lazy evaluation
		/// 
		/// </summary>
		/// <param name="instance">
		/// </param>
		/// <param name="ec">
		/// </param>
		protected internal XPathNodeset(FormInstance instance, EvaluationContext ec)
		{
			InitBlock();
			this.instance = instance;
			this.ec = ec;
		}
		
		
		/// <summary> Construct an XPath nodeset.
		/// 
		/// </summary>
		/// <param name="nodes">
		/// </param>
		/// <param name="instance">
		/// </param>
		/// <param name="ec">
		/// </param>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public XPathNodeset(Vector < TreeReference > nodes, FormInstance instance, EvaluationContext ec)
		
		public static XPathNodeset ConstructInvalidPathNodeset(System.String pathEvaluated, System.String originalPath)
		{
			XPathNodeset nodeset = new XPathNodeset();
			nodeset.nodes = null;
			nodeset.instance = null;
			nodeset.ec = null;
			nodeset.pathEvaluated = pathEvaluated;
			nodeset.originalPath = originalPath;
			return nodeset;
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		protected
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		void setReferences(Vector < TreeReference > nodes)
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		protected Vector < TreeReference > getReferences()
		
		
		/// <returns> The value represented by this xpath. Can only be evaluated when this xpath represents exactly one
		/// reference, or when it represents 0 references after a filtering operation (a reference which _could_ have
		/// existed, but didn't, rather than a reference which could not represent a real node).
		/// </returns>
		public virtual System.Object unpack()
		{
			if (nodes == null)
			{
				throw InvalidNodesetException;
			}
			
			if (size() == 0)
			{
				return XPathPathExpr.unpackValue(null);
			}
			else if (size() > 1)
			{
				throw new XPathTypeMismatchException("This field is repeated: \n\n" + nodeContents() + "\n\nYou may need to use the indexed-repeat() function to specify which value you want.");
			}
			else
			{
				return getValAt(0);
			}
		}
		
		public virtual System.Object[] toArgList()
		{
			if (nodes == null)
			{
				throw InvalidNodesetException;
			}
			
			System.Object[] args = new System.Object[size()];
			
			for (int i = 0; i < size(); i++)
			{
				System.Object val = getValAt(i);
				
				//sanity check
				if (val == null)
				{
					throw new System.SystemException("retrived a null value out of a nodeset! shouldn't happen!");
				}
				
				args[i] = val;
			}
			
			return args;
		}
		
		public virtual int size()
		{
			if (nodes == null)
			{
				return 0;
			}
			return nodes.size();
		}
		
		public virtual TreeReference getRefAt(int i)
		{
			if (nodes == null)
			{
				throw InvalidNodesetException;
			}
			
			return nodes.elementAt(i);
		}
		
		public virtual System.Object getValAt(int i)
		{
			return XPathPathExpr.getRefValue(instance, ec, getRefAt(i));
		}
		
		protected internal virtual System.String nodeContents()
		{
			if (nodes == null)
			{
				return "Invalid Path: " + pathEvaluated;
			}
			
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < nodes.size(); i++)
			{
				sb.append(nodes.elementAt(i).toString());
				if (i < nodes.size() - 1)
				{
					sb.append(";");
				}
			}
			return sb.toString();
		}
	}
}