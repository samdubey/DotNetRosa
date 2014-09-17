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
using FormDef = org.javarosa.core.model.FormDef;
using EvaluationContext = org.javarosa.core.model.condition.EvaluationContext;
using UnpivotableExpressionException = org.javarosa.core.model.condition.pivot.UnpivotableExpressionException;
using BooleanData = org.javarosa.core.model.data.BooleanData;
using DateData = org.javarosa.core.model.data.DateData;
using DecimalData = org.javarosa.core.model.data.DecimalData;
using GeoTraceData = org.javarosa.core.model.data.GeoTraceData;
using GeoPointData = org.javarosa.core.model.data.GeoPointData;
using GeoShapeData = org.javarosa.core.model.data.GeoShapeData;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using IntegerData = org.javarosa.core.model.data.IntegerData;
using LongData = org.javarosa.core.model.data.LongData;
using SelectMultiData = org.javarosa.core.model.data.SelectMultiData;
using SelectOneData = org.javarosa.core.model.data.SelectOneData;
using StringData = org.javarosa.core.model.data.StringData;
using UncastData = org.javarosa.core.model.data.UncastData;
using Selection = org.javarosa.core.model.data.helper.Selection;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapList = org.javarosa.core.util.externalizable.ExtWrapList;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using XFormAnswerDataSerializer = org.javarosa.xform.util.XFormAnswerDataSerializer;
using XPathException = org.javarosa.xpath.XPathException;
using XPathMissingInstanceException = org.javarosa.xpath.XPathMissingInstanceException;
using XPathNodeset = org.javarosa.xpath.XPathNodeset;
using XPathTypeMismatchException = org.javarosa.xpath.XPathTypeMismatchException;
using XPathUnsupportedException = org.javarosa.xpath.XPathUnsupportedException;
namespace org.javarosa.xpath.expr
{
	
	public class XPathPathExpr:XPathExpression
	{
		private void  InitBlock()
		{
			TreeReference ref_Renamed = this.getReference();
			//Either concretely the sentinal, or "."
			if (ref_Renamed.Equals(sentinal) || (ref_Renamed.RefLevel == 0))
			{
				return sentinal;
			}
			else
			{
				//It's very, very hard to figure out how to pivot predicates. For now, just skip it
				for (int i = 0; i < ref_Renamed.size(); ++i)
				{
					if (ref_Renamed.getPredicate(i) != null && ref_Renamed.getPredicate(i).size() > 0)
					{
						throw new UnpivotableExpressionException("Can't pivot filtered treereferences. Ref: " + ref_Renamed.toString(true) + " has predicates.");
					}
				}
				return this.eval(model, evalContext);
			}
		}
		public const int INIT_CONTEXT_ROOT = 0;
		public const int INIT_CONTEXT_RELATIVE = 1;
		public const int INIT_CONTEXT_EXPR = 2;
		
		public int init_context;
		public XPathStep[] steps;
		
		//for INIT_CONTEXT_EXPR only
		public XPathFilterExpr filtExpr;
		
		public XPathPathExpr()
		{
			InitBlock();
		} //for deserialization
		
		public XPathPathExpr(int init_context, XPathStep[] steps)
		{
			InitBlock();
			this.init_context = init_context;
			this.steps = steps;
		}
		
		public XPathPathExpr(XPathFilterExpr filtExpr, XPathStep[] steps):this(INIT_CONTEXT_EXPR, steps)
		{
			this.filtExpr = filtExpr;
		}
		
		public virtual TreeReference getReference()
		{
			return getReference(false);
		}
		
		/// <summary> translate an xpath path reference into a TreeReference
		/// TreeReferences only support a subset of true xpath paths; restrictions are:
		/// simple child name tests 'child::name', '.', and '..' allowed only
		/// no predicates
		/// all '..' steps must come before anything else
		/// </summary>
		public virtual TreeReference getReference(bool allowPredicates)
		{
			TreeReference ref_Renamed = new TreeReference();
			bool parentsAllowed;
			switch (init_context)
			{
				
				case XPathPathExpr.INIT_CONTEXT_ROOT: 
					ref_Renamed.RefLevel = TreeReference.REF_ABSOLUTE;
					parentsAllowed = false;
					break;
				
				case XPathPathExpr.INIT_CONTEXT_RELATIVE: 
					ref_Renamed.RefLevel = 0;
					parentsAllowed = true;
					break;
				
				case XPathPathExpr.INIT_CONTEXT_EXPR: 
					if (this.filtExpr.x != null && this.filtExpr.x is XPathFuncExpr)
					{
						XPathFuncExpr func = (XPathFuncExpr) (this.filtExpr.x);
						if (func.id.ToString().Equals("instance"))
						{
							ref_Renamed.RefLevel = TreeReference.REF_ABSOLUTE; //i assume when refering the non main instance you have to be absolute
							parentsAllowed = false;
							if (func.args.Length != 1)
							{
								throw new XPathUnsupportedException("instance() function used with " + func.args.Length + " arguements. Expecting 1 arguement");
							}
							if (!(func.args[0] is XPathStringLiteral))
							{
								throw new XPathUnsupportedException("instance() function expecting 1 string literal arguement arguement");
							}
							XPathStringLiteral strLit = (XPathStringLiteral) (func.args[0]);
							//we've got a non-standard instance in play, watch out
							if (strLit.s == null)
							{
								// absolute reference to the main instance
								ref_Renamed.Context = TreeReference.CONTEXT_ABSOLUTE;
								ref_Renamed.InstanceName = null;
							}
							else
							{
								ref_Renamed.Context = TreeReference.CONTEXT_INSTANCE;
								ref_Renamed.InstanceName = strLit.s;
							}
						}
						else
						{
							if (func.id.ToString().Equals("current"))
							{
								parentsAllowed = true;
								ref_Renamed.Context = TreeReference.CONTEXT_ORIGINAL;
							}
							else
							{
								//We only support expression root contexts for instance refs, everything else is an illegal filter
								throw new XPathUnsupportedException("filter expression");
							}
						}
					}
					else
					{
						//We only support expression root contexts for instance refs, everything else is an illegal filter
						throw new XPathUnsupportedException("filter expression");
					}
					
					break;
				
				default:  throw new XPathUnsupportedException("filter expression");
				
			}
			for (int i = 0; i < steps.Length; i++)
			{
				XPathStep step = steps[i];
				if (step.axis == XPathStep.AXIS_SELF)
				{
					if (step.test != XPathStep.TEST_TYPE_NODE)
					{
						throw new XPathUnsupportedException("step other than 'child::name', '.', '..'");
					}
				}
				else if (step.axis == XPathStep.AXIS_PARENT)
				{
					if (!parentsAllowed || step.test != XPathStep.TEST_TYPE_NODE)
					{
						throw new XPathUnsupportedException("step other than 'child::name', '.', '..'");
					}
					else
					{
						ref_Renamed.incrementRefLevel();
					}
				}
				else if (step.axis == XPathStep.AXIS_ATTRIBUTE)
				{
					if (step.test == XPathStep.TEST_NAME)
					{
						ref_Renamed.add(step.name.ToString(), TreeReference.INDEX_ATTRIBUTE);
						parentsAllowed = false;
						//TODO: Can you step back from an attribute, or should this always be
						//the last step?
					}
					else
					{
						throw new XPathUnsupportedException("attribute step other than 'attribute::name");
					}
				}
				else if (step.axis == XPathStep.AXIS_CHILD)
				{
					if (step.test == XPathStep.TEST_NAME)
					{
						ref_Renamed.add(step.name.ToString(), TreeReference.INDEX_UNBOUND);
						parentsAllowed = true;
					}
					else if (step.test == XPathStep.TEST_NAME_WILDCARD)
					{
						ref_Renamed.add(TreeReference.NAME_WILDCARD, TreeReference.INDEX_UNBOUND);
						parentsAllowed = true;
					}
					else
					{
						throw new XPathUnsupportedException("step other than 'child::name', '.', '..'");
					}
				}
				else
				{
					throw new XPathUnsupportedException("step other than 'child::name', '.', '..'");
				}
				
				if (step.predicates.Length > 0)
				{
					int refLevel = ref_Renamed.RefLevel;
					//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
					Vector < XPathExpression > v = new Vector < XPathExpression >();
					for (int j = 0; j < step.predicates.Length; j++)
					{
						v.addElement(step.predicates[j]);
					}
					ref_Renamed.addPredicate(i, v);
				}
			}
			return ref_Renamed;
		}
		
		public override XPathNodeset eval(FormInstance m, EvaluationContext ec)
		{
			TreeReference genericRef = getReference();
			
			TreeReference ref_Renamed;
			if (genericRef.Context == TreeReference.CONTEXT_ORIGINAL)
			{
				ref_Renamed = genericRef.contextualize(ec.OriginalContext);
			}
			else
			{
				ref_Renamed = genericRef.contextualize(ec.ContextRef);
			}
			
			//We don't necessarily know the model we want to be working with until we've contextualized the
			//node
			
			//check if this nodeset refers to a non-main instance
			if (ref_Renamed.InstanceName != null && ref_Renamed.Absolute)
			{
				FormInstance nonMain = ec.getInstance(ref_Renamed.InstanceName);
				if (nonMain != null)
				{
					m = nonMain;
				}
				else
				{
					throw new XPathMissingInstanceException(ref_Renamed.InstanceName, "Instance referenced by " + ref_Renamed.toString(true) + " does not exist");
				}
			}
			else
			{
				//TODO: We should really stop passing 'm' around and start just getting the right instance from ec
				//at a more central level
				m = ec.MainInstance;
				
				if (m == null)
				{
					System.String refStr = ref_Renamed == null?"":ref_Renamed.toString(true);
					throw new XPathException("Cannot evaluate the reference [" + refStr + "] in the current evaluation context. No default instance has been declared!");
				}
			}
			
			// regardless of the above, we want to ensure there is a definition
			if (m.getRoot() == null)
			{
				//This instance is _declared_, but doesn't actually have any data in it.
				throw new XPathMissingInstanceException(ref_Renamed.InstanceName, "Instance referenced by " + ref_Renamed.toString(true) + " has not been loaded");
			}
			
			// this makes no sense...
			//		if (ref.isAbsolute() && m.getTemplatePath(ref) == null) {
			//			Vector<TreeReference> nodesetRefs = new Vector<TreeReference>();
			//			return new XPathNodeset(nodesetRefs, m, ec);
			//		}
			
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			
			//to fix conditions based on non-relevant data, filter the nodeset by relevancy
			for (int i = 0; i < nodesetRefs.size(); i++)
			{
				if (!m.resolveReference((TreeReference) nodesetRefs.elementAt(i)).isRelevant())
				{
					nodesetRefs.removeElementAt(i);
					i--;
				}
			}
			
			return new XPathNodeset(nodesetRefs, m, ec);
		}
		
		//
		//	boolean nodeset = forceNodeset;
		//	if (!nodeset) {
		//		//is this a nodeset? it is if the ref contains any unbound multiplicities AND the unbound nodes are repeatable
		//		//the way i'm calculating this sucks; there has got to be an easier way to find out if a node is repeatable
		//		TreeReference repeatTestRef = TreeReference.rootRef();
		//		for (int i = 0; i < ref.size(); i++) {
		//			repeatTestRef.add(ref.getName(i), ref.getMultiplicity(i));
		//			if (ref.getMultiplicity(i) == TreeReference.INDEX_UNBOUND) {
		//				if (m.getTemplate(repeatTestRef) != null) {
		//					nodeset = true;
		//					break;
		//				}
		//			}
		//		}
		//	}
		
		public static System.Object getRefValue(FormInstance model, EvaluationContext ec, TreeReference ref_Renamed)
		{
			if (ec.isConstraint && ref_Renamed.Equals(ec.ContextRef))
			{
				//ITEMSET TODO: need to update this; for itemset/copy constraints, need to simulate a whole xml sub-tree here
				return unpackValue(ec.candidateValue);
			}
			else
			{
				TreeElement node = model.resolveReference(ref_Renamed);
				if (node == null)
				{
					//shouldn't happen -- only existent nodes should be in nodeset
					throw new XPathTypeMismatchException("Node " + ref_Renamed.ToString() + " does not exist!");
				}
				
				return unpackValue(node.isRelevant()?node.Value:null);
			}
		}
		
		public static System.Object unpackValue(IAnswerData val)
		{
			if (val == null)
			{
				return "";
			}
			else if (val is UncastData)
			{
				return val.Value;
			}
			else if (val is IntegerData)
			{
				return (double) ((System.Int32) val.Value);
			}
			else if (val is LongData)
			{
				return (double) ((System.Int64) val.Value);
			}
			else if (val is DecimalData)
			{
				return val.Value;
			}
			else if (val is StringData)
			{
				return val.Value;
			}
			else if (val is SelectOneData)
			{
				return ((Selection) val.Value).Value;
			}
			else if (val is SelectMultiData)
			{
				return (new XFormAnswerDataSerializer()).serializeAnswerData(val);
			}
			else if (val is DateData)
			{
				return val.Value;
			}
			else if (val is BooleanData)
			{
				return val.Value;
			}
			else if (val is GeoPointData)
			{
				// we have no access fns that interact with double[4] arrays (the getValue() data type)...
				return val.DisplayText;
			}
			else if (val is GeoShapeData)
			{
				// we have no access fns that interact with GeoShape objects (the getValue() data type)...
				return val.DisplayText;
			}
			else if (val is GeoTraceData)
			{
				// we have no access fns that interact with GeoTrace objects (the getValue() data type)...
				return val.DisplayText;
			}
			else
			{
				System.Console.Out.WriteLine("warning: unrecognized data type in xpath expr: " + val.GetType().FullName);
				return val.Value; //is this a good idea?
			}
		}
		
		public override System.String ToString()
		{
			StringBuilder sb = new StringBuilder();
			
			sb.append("{path-expr:");
			switch (init_context)
			{
				
				case INIT_CONTEXT_ROOT:  sb.append("abs"); break;
				
				case INIT_CONTEXT_RELATIVE:  sb.append("rel"); break;
				
				case INIT_CONTEXT_EXPR:  sb.append(filtExpr.ToString()); break;
				}
			sb.append(",{");
			for (int i = 0; i < steps.Length; i++)
			{
				sb.append(steps[i].ToString());
				if (i < steps.Length - 1)
					sb.append(",");
			}
			sb.append("}}");
			
			return sb.toString();
		}
		
		public  override bool Equals(System.Object o)
		{
			if (o is XPathPathExpr)
			{
				XPathPathExpr x = (XPathPathExpr) o;
				
				//Shortcuts for easily comparable values
				if (init_context != x.init_context || steps.Length != x.steps.Length)
				{
					return false;
				}
				
				return ExtUtil.arrayEquals(steps, x.steps) && (init_context == INIT_CONTEXT_EXPR?filtExpr.Equals(x.filtExpr):true);
			}
			else
			{
				return false;
			}
		}
		
		/// <summary> Warning: this method has somewhat unclear semantics.
		/// 
		/// "matches" follows roughly the same process as equals(), in that it goes
		/// through the path step by step and compares whether each step can refer to the same node.
		/// The only difference is that match() will allow for a named step to match a step who's name
		/// is a wildcard.
		/// 
		/// So
		/// \/data\/path\/to
		/// will "match"
		/// \/data\/*\/to
		/// 
		/// even though they are not equal.
		/// 
		/// Matching is reflexive, consistent, and symmetric, but _not_ transitive.
		/// 
		/// </summary>
		/// <param name="o">
		/// </param>
		/// <returns> true if the expression is a path that matches this one
		/// </returns>
		public virtual bool matches(XPathExpression o)
		{
			if (o is XPathPathExpr)
			{
				XPathPathExpr x = (XPathPathExpr) o;
				
				//Shortcuts for easily comparable values
				if (init_context != x.init_context || steps.Length != x.steps.Length)
				{
					return false;
				}
				
				if (steps.Length != x.steps.Length)
				{
					return false;
				}
				else
				{
					for (int i = 0; i < steps.Length; i++)
					{
						if (!steps[i].matches(x.steps[i]))
						{
							return false;
						}
					}
				}
				
				// If all steps match, we still need to make sure we're in the same "context" if this
				// is a normal expression.
				return (init_context == INIT_CONTEXT_EXPR?filtExpr.Equals(x.filtExpr):true);
			}
			else
			{
				return false;
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			init_context = ExtUtil.readInt(in_Renamed);
			if (init_context == INIT_CONTEXT_EXPR)
			{
				filtExpr = (XPathFilterExpr) ExtUtil.read(in_Renamed, typeof(XPathFilterExpr), pf);
			}
			
			System.Collections.ArrayList v = (System.Collections.ArrayList) ExtUtil.read(in_Renamed, new ExtWrapList(typeof(XPathStep)), pf);
			steps = new XPathStep[v.Count];
			for (int i = 0; i < steps.Length; i++)
				steps[i] = ((XPathStep) v[i]).intern();
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeNumeric(out_Renamed, init_context);
			if (init_context == INIT_CONTEXT_EXPR)
			{
				ExtUtil.write(out_Renamed, filtExpr);
			}
			
			System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			for (int i = 0; i < steps.Length; i++)
				v.Add(steps[i]);
			ExtUtil.write(out_Renamed, new ExtWrapList(v));
		}
		
		public static XPathPathExpr fromRef(TreeReference ref_Renamed)
		{
			XPathPathExpr path = new XPathPathExpr();
			path.init_context = (ref_Renamed.Absolute?INIT_CONTEXT_ROOT:INIT_CONTEXT_RELATIVE);
			path.steps = new XPathStep[ref_Renamed.size()];
			for (int i = 0; i < path.steps.Length; i++)
			{
				if (ref_Renamed.getName(i).Equals(TreeReference.NAME_WILDCARD))
				{
					path.steps[i] = new XPathStep(XPathStep.AXIS_CHILD, XPathStep.TEST_NAME_WILDCARD).intern();
				}
				else
				{
					path.steps[i] = new XPathStep(XPathStep.AXIS_CHILD, new XPathQName(ref_Renamed.getName(i))).intern();
				}
			}
			return path;
		}
		
		new public System.Object pivot;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		(FormInstance model, EvaluationContext evalContext, Vector < Object > pivots, Object sentinal) throws UnpivotableExpressionException
		//UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}