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
	public class XPathLazyNodeset:XPathNodeset
	{
		private void  InitBlock()
		{
			performEvaluation();
			return base.getReferences();
		}
		
		internal System.Boolean evaluated = false;
		internal TreeReference unExpandedRef;
		
		/// <summary> Construct an XPath nodeset.
		/// 
		/// </summary>
		/// <param name="nodes">
		/// </param>
		/// <param name="instance">
		/// </param>
		/// <param name="ec">
		/// </param>
		public XPathLazyNodeset(TreeReference unExpandedRef, FormInstance instance, EvaluationContext ec):base(instance, ec)
		{
			InitBlock();
			this.unExpandedRef = unExpandedRef;
		}
		
		
		private void  performEvaluation()
		{
			lock (evaluated)
			{
				if (evaluated)
				{
					return ;
				}
				
				
				//to fix conditions based on non-relevant data, filter the nodeset by relevancy
				for (int i = 0; i < nodes.size(); i++)
				{
					if (!instance.resolveReference((TreeReference) nodes.elementAt(i)).isRelevant())
					{
						nodes.removeElementAt(i);
						i--;
					}
				}
				this.setReferences(nodes);
				evaluated = true;
			}
		}
		
		
		/// <returns> The value represented by this xpath. Can only be evaluated when this xpath represents exactly one
		/// reference, or when it represents 0 references after a filtering operation (a reference which _could_ have
		/// existed, but didn't, rather than a reference which could not represent a real node).
		/// </returns>
		public override System.Object unpack()
		{
			lock (evaluated)
			{
				if (evaluated)
				{
					return base.unpack();
				}
				
				//this element is the important one. For Basic nodeset evaluations (referring to one node with no
				//multiplicites) we should be able to do this without doing the expansion
				
				//first, see if this treeref is usable without expansion
				int size = unExpandedRef.size();
				bool safe = true; ;
				for (int i = 0; i < size; ++i)
				{
					//We can't evaluated any predicates for sure
					if (unExpandedRef.getPredicate(i) != null)
					{
						safe = false;
						break;
					}
					int mult = unExpandedRef.getMultiplicity(i);
					if (!(mult >= 0 || mult == TreeReference.INDEX_UNBOUND))
					{
						safe = false;
						break;
					}
				}
				if (!safe)
				{
					performEvaluation();
					return base.unpack();
				}
				
				//TOOD: Evaluate error fallbacks, here. I don't know whether this handles the 0 case
				//the same way, although invalid multiplicities should be fine.
				try
				{
					//TODO: This doesn't handle templated nodes (repeats which may exist in the future)
					//figure out if we can roll that in easily. For now the catch handles it
					return XPathPathExpr.getRefValue(instance, ec, unExpandedRef);
				}
				catch (XPathException xpe)
				{
					//This isn't really a best effort attempt, so if we can, see if evaluating cleany works.
					performEvaluation();
					return base.unpack();
				}
			}
		}
		
		public override System.Object[] toArgList()
		{
			performEvaluation();
			return base.toArgList();
		}
		
		
		protected List< TreeReference > getReferences()
		
		public override int size()
		{
			performEvaluation();
			return base.size();
		}
		
		public override TreeReference getRefAt(int i)
		{
			performEvaluation();
			return base.getRefAt(i);
		}
		
		public override System.Object getValAt(int i)
		{
			performEvaluation();
			return base.getValAt(i);
		}
		
		protected internal override System.String nodeContents()
		{
			performEvaluation();
			return base.nodeContents();
		}
	}
}