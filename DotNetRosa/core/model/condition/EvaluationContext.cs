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
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using IExprDataType = org.javarosa.xpath.IExprDataType;
using XPathExpression = org.javarosa.xpath.expr.XPathExpression;
using XPathFuncExpr = org.javarosa.xpath.expr.XPathFuncExpr;
namespace org.javarosa.core.model.condition
{
	
	/* a collection of objects that affect the evaluation of an expression, like function handlers
	* and (not supported) variable bindings
	*/
	public class EvaluationContext
	{
		private void  InitBlock()
		{
			this(base_Renamed, context);
			this.formInstances = formInstances;
			this(base_Renamed);
			this.formInstances = formInstances;
			this.instance = instance;
			this.formInstances = formInstances;
			this.instance = instance;
			this.contextNode = TreeReference.rootRef();
			
			functionHandlers = new HashMap < String, IFunctionHandler >();
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			variables = new System.Collections.Hashtable();
			
			for(String var: variables.keySet())
			{
				//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
				setVariable(var, variables[var]);
			}
			return expandReference(ref_Renamed, false);
			if (!ref_Renamed.isAbsolute())
			{
				return null;
			}
			
			FormInstance baseInstance;
			if (ref_Renamed.getInstanceName() != null)
			{
				baseInstance = getInstance(ref_Renamed.getInstanceName());
			}
			else
			{
				baseInstance = instance;
			}
			
			if (baseInstance == null)
			{
				throw new System.SystemException("Unable to expand reference " + ref_Renamed.toString(true) + ", no appropriate instance in evaluation context");
			}
			
			
			List< TreeReference > v = new List< TreeReference >();
			expandReference(ref_Renamed, baseInstance, baseInstance.getRoot().Ref, v, includeTemplates);
			return v;
			int depth = workingRef.size();
			
			
			//check to see if we've matched fully
			if (depth == sourceRef.size())
			{
				//TODO: Do we need to clone these references?
				refs.addElement(workingRef);
			}
			else
			{
				//Otherwise, need to get the next set of matching references
				
				System.String name = sourceRef.getName(depth);
				predicates = sourceRef.getPredicate(depth);
				
				//Copy predicates for batch fetch
				if (predicates != null)
				{
					
					List< XPathExpression > predCopy = new List< XPathExpression >();
					
					for(XPathExpression xpe: predicates)
					{
						predCopy.addElement(xpe);
					}
					predicates = predCopy;
				}
				//ETHERTON: Is this where we should test for predicates?
				int mult = sourceRef.getMultiplicity(depth);
				
				List< TreeReference > set = new List< TreeReference >();
				
				TreeElement node = instance.resolveReference(workingRef);
				
				List< TreeReference > passingSet = new List< TreeReference >();
				
				{
					if (node.NumChildren > 0)
					{
						if (mult == TreeReference.INDEX_UNBOUND)
						{
							int count = node.getChildMultiplicity(name);
							for (int i = 0; i < count; i++)
							{
								TreeElement child = node.getChild(name, i);
								if (child != null)
								{
									set_Renamed.addElement(child.Ref);
								}
								else
								{
									throw new System.SystemException("Missing or non-sequntial nodes expanding a reference"); // missing/non-sequential
									// nodes
								}
							}
							if (includeTemplates)
							{
								TreeElement template = node.getChild(name, TreeReference.INDEX_TEMPLATE);
								if (template != null)
								{
									set_Renamed.addElement(template.Ref);
								}
							}
						}
						else if (mult != TreeReference.INDEX_ATTRIBUTE)
						{
							//TODO: Make this test mult >= 0?
							//If the multiplicity is a simple integer, just get
							//the appropriate child
							TreeElement child = node.getChild(name, mult);
							if (child != null)
							{
								set_Renamed.addElement(child.Ref);
							}
						}
					}
					
					if (mult == TreeReference.INDEX_ATTRIBUTE)
					{
						TreeElement attribute = node.getAttribute(null, name);
						if (attribute != null)
						{
							set_Renamed.addElement(attribute.Ref);
						}
					}
				}
				
				if (predicates != null && predicateEvaluationProgress != null)
				{
					predicateEvaluationProgress[1] += set_Renamed.size();
				}
				
				if (predicates != null)
				{
					bool firstTime = true;
					
					List< TreeReference > passed = new List< TreeReference >();
					
					for(XPathExpression xpe: predicates)
					{
						for (int i = 0; i < set_Renamed.size(); ++i)
						{
							//if there are predicates then we need to see if e.nextElement meets the standard of the predicate
							TreeReference treeRef = set_Renamed.elementAt(i);
							
							//test the predicate on the treeElement
							EvaluationContext evalContext = rescope(treeRef, (firstTime?treeRef.MultLast:i));
							System.Object o = xpe.eval(instance, evalContext);
							if (o is System.Boolean)
							{
								bool testOutcome = ((System.Boolean) o);
								if (testOutcome)
								{
									passed.addElement(treeRef);
								}
							}
						}
						firstTime = false;
						set_Renamed.clear();
						set_Renamed.addAll(passed);
						passed.clear();
						
						if (predicateEvaluationProgress != null)
						{
							predicateEvaluationProgress[0]++;
						}
					}
				}
				
				for (int i = 0; i < set_Renamed.size(); ++i)
				{
					TreeReference treeRef = set_Renamed.elementAt(i);
					expandReference(sourceRef, instance, treeRef, refs, includeTemplates);
				}
			}
		}
		virtual public TreeReference ContextRef
		{
			get
			{
				return contextNode;
			}
			
		}
		virtual public TreeReference OriginalContext
		{
			get
			{
				if (this.original == null)
				{
					return this.contextNode;
				}
				else
				{
					return this.original;
				}
			}
			
			set
			{
				this.original = value;
			}
			
		}
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		virtual public System.Collections.Hashtable FunctionHandlers
		{
			get
			{
				return functionHandlers;
			}
			
		}
		virtual public System.String OutputTextForm
		{
			get
			{
				return outputTextForm;
			}
			
			set
			{
				this.outputTextForm = value;
			}
			
		}
		virtual public FormInstance MainInstance
		{
			get
			{
				return instance;
			}
			
		}
		virtual public int ContextPosition
		{
			get
			{
				return currentContextPosition;
			}
			
		}
		virtual public int[] PredicateProcessSet
		{
			set
			{
				if (value != null && value.Length == 2)
				{
					predicateEvaluationProgress = value;
				}
			}
			
		}
		private TreeReference contextNode; //unambiguous ref used as the anchor for relative paths
		
		private HashMap < String, IFunctionHandler > functionHandlers;
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		private System.Collections.Hashtable variables;
		
		public bool isConstraint; //true if we are evaluating a constraint
		public IAnswerData candidateValue; //if isConstraint, this is the value being validated
		public bool isCheckAddChild; //if isConstraint, true if we are checking the constraint of a parent node on how
		//  many children it may have
		
		private System.String outputTextForm = null; //Responsible for informing itext what form is requested if relevant
		
		
		private HashMap < String, FormInstance > formInstances;
		
		private TreeReference original;
		private int currentContextPosition = - 1;
		
		internal FormInstance instance;
		internal int[] predicateEvaluationProgress;
		
		/// <summary>Copy Constructor *</summary>
		private EvaluationContext(EvaluationContext base_Renamed)
		{
			InitBlock();
			//TODO: These should be deep, not shallow
			this.functionHandlers = base_Renamed.functionHandlers;
			this.formInstances = base_Renamed.formInstances;
			this.variables = base_Renamed.variables;
			
			this.contextNode = base_Renamed.contextNode;
			this.instance = base_Renamed.instance;
			
			this.isConstraint = base_Renamed.isConstraint;
			this.candidateValue = base_Renamed.candidateValue;
			this.isCheckAddChild = base_Renamed.isCheckAddChild;
			
			this.outputTextForm = base_Renamed.outputTextForm;
			this.original = base_Renamed.original;
			
			//Hrm....... not sure about this one. this only happens after a rescoping,
			//and is fixed on the context. Anything that changes the context should
			//invalidate this
			this.currentContextPosition = base_Renamed.currentContextPosition;
		}
		
		public EvaluationContext(EvaluationContext base_Renamed, TreeReference context):this(base_Renamed)
		{
			this.contextNode = context;
		}
		
		
		public EvaluationContext(EvaluationContext base, HashMap < String, FormInstance > formInstances, TreeReference context)
		
		
		public EvaluationContext(FormInstance instance, HashMap < String, FormInstance > formInstances, EvaluationContext base)
		
		public EvaluationContext(FormInstance instance)
		{
			InitBlock();
			
			this(instance, new HashMap < String, FormInstance >());
		}
		
		
		public EvaluationContext(FormInstance instance, HashMap < String, FormInstance > formInstances)
		
		public virtual FormInstance getInstance(System.String id)
		{
			return formInstances.containsKey(id)?formInstances.get_Renamed(id):(instance != null && id.Equals(instance.Name)?instance:null);
		}
		
		public virtual void  addFunctionHandler(IFunctionHandler fh)
		{
			functionHandlers.put(fh.Name, fh);
		}
		
		
		public
		
		void setVariables(HashMap < String, ? > variables)
		
		public virtual void  setVariable(System.String name, System.Object value_Renamed)
		{
			//No such thing as a null xpath variable. Empty
			//values in XPath just get converted to ""
			if (value_Renamed == null)
			{
				variables[name] = "";
				return ;
			}
			//Otherwise check whether the value is one of the normal first
			//order datatypes used in xpath evaluation
			if (value_Renamed is System.Boolean || value_Renamed is System.Double || value_Renamed is System.String || value_Renamed is System.DateTime || value_Renamed is IExprDataType)
			{
				variables[name] = value_Renamed;
				return ;
			}
			
			//Some datatypes can be trivially converted to a first order
			//xpath datatype
			if (value_Renamed is System.Int32)
			{
				variables[name] = (double) ((System.Int32) value_Renamed);
				return ;
			}
			if (value_Renamed is System.Single)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				variables[name] = (double) ((System.Single) value_Renamed);
				return ;
			}
			//Otherwise we just hope for the best, I suppose? Should we log this?
			else
			{
				variables[name] = value_Renamed;
			}
		}
		
		public virtual System.Object getVariable(System.String name)
		{
			//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
			return variables[name];
		}
		
		
		public List< TreeReference > expandReference(TreeReference ref)
		
		// take in a potentially-ambiguous ref, and return a vector of refs for all nodes that match the passed-in ref
		// meaning, search out all repeated nodes that match the pattern of the passed-in ref
		// every ref in the returned vector will be unambiguous (no index will ever be INDEX_UNBOUND)
		// does not return template nodes when matching INDEX_UNBOUND, but will match templates when INDEX_TEMPLATE is explicitly set
		// return null if ref is relative, otherwise return vector of refs (but vector will be empty is no refs match)
		// '/' returns {'/'}
		// can handle sub-repetitions (e.g., {/a[1]/b[1], /a[1]/b[2], /a[2]/b[1]})
		
		public List< TreeReference > expandReference(TreeReference ref, boolean includeTemplates)
		
		// recursive helper function for expandReference
		// sourceRef: original path we're matching against
		// node: current node that has matched the sourceRef thus far
		// workingRef: explicit path that refers to the current node
		// refs: Vector to collect matching paths; if 'node' is a target node that
		// matches sourceRef, templateRef is added to refs
		
		private
		
		void expandReference(TreeReference sourceRef, FormInstance instance, TreeReference workingRef, List< TreeReference > refs, boolean includeTemplates)
		
		private EvaluationContext rescope(TreeReference treeRef, int currentContextPosition)
		{
			EvaluationContext ec = new EvaluationContext(this, treeRef);
			// broken:
			ec.currentContextPosition = currentContextPosition;
			//If there was no original context position, we'll want to set the next original
			//context to be this rescoping (which would be the backup original one).
			if (this.original != null)
			{
				ec.OriginalContext = this.OriginalContext;
			}
			else
			{
				//Check to see if we have a context, if not, the treeRef is the original declared
				//nodeset.
				if (TreeReference.rootRef().Equals(this.ContextRef))
				{
					ec.OriginalContext = treeRef;
				}
				else
				{
					//If we do have a legit context, use it!
					ec.OriginalContext = this.ContextRef;
				}
			}
			return ec;
		}
		
		public virtual TreeElement resolveReference(TreeReference qualifiedRef)
		{
			FormInstance instance = this.MainInstance;
			if (qualifiedRef.InstanceName != null)
			{
				instance = this.getInstance(qualifiedRef.InstanceName);
			}
			return instance.resolveReference(qualifiedRef);
		}
	}
}