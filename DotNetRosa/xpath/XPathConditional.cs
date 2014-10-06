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
using FatalException = org.javarosa.core.log.FatalException;
using EvaluationContext = org.javarosa.core.model.condition.EvaluationContext;
using IConditionExpr = org.javarosa.core.model.condition.IConditionExpr;
using UnpivotableExpressionException = org.javarosa.core.model.condition.pivot.UnpivotableExpressionException;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapTagged = org.javarosa.core.util.externalizable.ExtWrapTagged;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using XPathBinaryOpExpr = org.javarosa.xpath.expr.XPathBinaryOpExpr;
using XPathExpression = org.javarosa.xpath.expr.XPathExpression;
using XPathFuncExpr = org.javarosa.xpath.expr.XPathFuncExpr;
using XPathPathExpr = org.javarosa.xpath.expr.XPathPathExpr;
using XPathUnaryOpExpr = org.javarosa.xpath.expr.XPathUnaryOpExpr;
using XPathSyntaxException = org.javarosa.xpath.parser.XPathSyntaxException;
namespace org.javarosa.xpath
{
	
	public class XPathConditional : IConditionExpr
	{
		private void  InitBlock()
		{
			if (expr is XPathPathExpr)
			{
				return ((XPathPathExpr) expr).eval(model, evalContext).getReferences();
			}
			else
			{
				throw new FatalException("evalNodeset: must be path expression");
			}
			
			Set < TreeReference > triggers = new HashSet < TreeReference >();
			getTriggers(expr, triggers, contextRef);
			return triggers;
			if (x is XPathPathExpr)
			{
				TreeReference ref_Renamed = ((XPathPathExpr) x).getReference();
				TreeReference contextualized = ref_Renamed;
				if (contextRef != null)
				{
					contextualized = ref_Renamed.contextualize(contextRef);
				}
				
				//TODO: It's possible we should just handle this the same way as "genericize". Not entirely clear.
				if (contextualized.hasPredicates())
				{
					contextualized = contextualized.removePredicates();
				}
				
				v.add(contextualized);
				
				for (int i = 0; i < ref_Renamed.size(); i++)
				{
					
					if (predicates == null)
					{
						continue;
					}
					
					//we can't generate this properly without an absolute reference
					if (!ref_Renamed.Absolute)
					{
						throw new System.ArgumentException("can't get triggers for relative references");
					}
					TreeReference predicateContext = ref_Renamed.getSubReference(i);
					
					
					for(XPathExpression predicate: predicates)
					{
						getTriggers(predicate, v, predicateContext);
					}
				}
			}
			else if (x is XPathBinaryOpExpr)
			{
				getTriggers(((XPathBinaryOpExpr) x).a, v, contextRef);
				getTriggers(((XPathBinaryOpExpr) x).b, v, contextRef);
			}
			else if (x is XPathUnaryOpExpr)
			{
				getTriggers(((XPathUnaryOpExpr) x).a, v, contextRef);
			}
			else if (x is XPathFuncExpr)
			{
				XPathFuncExpr fx = (XPathFuncExpr) x;
				for (int i = 0; i < fx.args.Length; i++)
					getTriggers(fx.args[i], v, contextRef);
			}
			return expr.pivot(model, evalContext);
		}
		virtual public XPathExpression Expr
		{
			get
			{
				return expr;
			}
			
		}
		private XPathExpression expr;
		public System.String xpath; //not serialized!
		public bool hasNow; //indicates whether this XpathConditional contains the now() function (used for timestamping)
		
		public XPathConditional(System.String xpath)
		{
			InitBlock();
			hasNow = false;
			if (xpath.IndexOf("now()") > - 1)
			{
				hasNow = true;
			}
			this.expr = XPathParseTool.parseXPath(xpath);
			this.xpath = xpath;
		}
		
		public XPathConditional(XPathExpression expr)
		{
			InitBlock();
			this.expr = expr;
		}
		
		public XPathConditional()
		{
			InitBlock();
		}
		
		public virtual System.Object evalRaw(FormInstance model, EvaluationContext evalContext)
		{
			try
			{
				return XPathFuncExpr.unpack(expr.eval(model, evalContext));
			}
			catch (XPathUnsupportedException e)
			{
				if (xpath != null)
				{
					throw new XPathUnsupportedException(xpath);
				}
				else
				{
					throw e;
				}
			}
		}
		
		public virtual bool eval(FormInstance model, EvaluationContext evalContext)
		{
			return XPathFuncExpr.toBoolean(evalRaw(model, evalContext));
		}
		
		public virtual System.String evalReadable(FormInstance model, EvaluationContext evalContext)
		{
			return XPathFuncExpr.toString(evalRaw(model, evalContext));
		}
		
		
		public List< TreeReference > evalNodeset(FormInstance model, EvaluationContext evalContext)
		
		
		public Set < TreeReference > getTriggers(TreeReference contextRef)
		
		
		private static
		
		void getTriggers(XPathExpression x, Set < TreeReference > v, TreeReference contextRef)
		
		public  override bool Equals(System.Object o)
		{
			if (o is XPathConditional)
			{
				XPathConditional cond = (XPathConditional) o;
				return expr.Equals(cond.expr);
			}
			else
			{
				return false;
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			expr = (XPathExpression) ExtUtil.read(in_Renamed, new ExtWrapTagged(), pf);
			hasNow = (bool) ExtUtil.readBool(in_Renamed);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.write(out_Renamed, new ExtWrapTagged(expr));
			ExtUtil.writeBool(out_Renamed, hasNow);
		}
		
		public override System.String ToString()
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			return "xpath[" + expr.ToString() + "]";
		}
		
		
		public List< Object > pivot(FormInstance model, EvaluationContext evalContext) throws UnpivotableExpressionException
		//UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}