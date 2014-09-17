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
using EvaluationContext = org.javarosa.core.model.condition.EvaluationContext;
using UnpivotableExpressionException = org.javarosa.core.model.condition.pivot.UnpivotableExpressionException;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using Logger = org.javarosa.core.services.Logger;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
namespace org.javarosa.xpath.expr
{
	
	public abstract class XPathExpression
	{
		public XPathExpression()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			try
			{
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				Vector < Object > pivots = new Vector < Object >();
				this.pivot(model, evalContext, pivots, evalContext.getContextRef());
				return pivots;
			}
			catch (UnpivotableExpressionException uee)
			{
				//Rethrow unpivotable (expected)
				throw uee;
			}
			catch (System.Exception e)
			{
				//Pivots aren't critical, if there was a problem getting one, log the exception
				//so we can fix it, and then just report that.
				Logger.exception(e);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new UnpivotableExpressionException(e.Message);
			}
			return eval(model, evalContext);
		}
		
		public virtual System.Object eval(EvaluationContext evalContext)
		{
			return this.eval(evalContext.MainInstance, evalContext);
		}
		
		public abstract System.Object eval(FormInstance model, EvaluationContext evalContext);
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public final Vector < Object > pivot(FormInstance model, EvaluationContext evalContext) throws UnpivotableExpressionException
		
		/// <summary> Pivot this expression, returning values if appropriate, and adding any pivots to the list.
		/// 
		/// </summary>
		/// <param name="model">The model to evaluate the current expression against
		/// </param>
		/// <param name="evalContext">The evaluation context to evaluate against
		/// </param>
		/// <param name="pivots">The list of pivot points in the xpath being evaluated. Pivots should be added to this list.
		/// </param>
		/// <param name="sentinal">The value which is being pivoted around.
		/// </param>
		/// <returns>
		/// null - If a pivot was identified in this expression
		/// sentinal - If the current expression represents the sentinal being pivoted
		/// any other value - The result of the expression if no pivots are detected
		/// </returns>
		/// <throws>  UnpivotableExpressionException If the expression is too complex to pivot </throws>
		public System.Object pivot;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		(FormInstance model, EvaluationContext evalContext, Vector < Object > pivots, Object sentinal) throws UnpivotableExpressionException
		
		/*======= DEBUGGING ========*/
		// should not compile onto phone
		
		/* print out formatted expression tree */
		
		internal int indent;
		
		private void  printStr(System.String s)
		{
			for (int i = 0; i < 2 * indent; i++)
				System.Console.Out.Write(" ");
			System.Console.Out.WriteLine(s);
		}
		
		public virtual void  printParseTree()
		{
			indent = - 1;
			print(this);
		}
		
		public virtual void  print(System.Object o)
		{
			indent += 1;
			
			if (o is XPathStringLiteral)
			{
				XPathStringLiteral x = (XPathStringLiteral) o;
				printStr("strlit {" + x.s + "}");
			}
			else if (o is XPathNumericLiteral)
			{
				XPathNumericLiteral x = (XPathNumericLiteral) o;
				printStr("numlit {" + x.d + "}");
			}
			else if (o is XPathVariableReference)
			{
				XPathVariableReference x = (XPathVariableReference) o;
				printStr("var {" + x.id.ToString() + "}");
			}
			else if (o is XPathArithExpr)
			{
				XPathArithExpr x = (XPathArithExpr) o;
				System.String op = null;
				switch (x.op)
				{
					
					case XPathArithExpr.ADD:  op = "add"; break;
					
					case XPathArithExpr.SUBTRACT:  op = "subtr"; break;
					
					case XPathArithExpr.MULTIPLY:  op = "mult"; break;
					
					case XPathArithExpr.DIVIDE:  op = "div"; break;
					
					case XPathArithExpr.MODULO:  op = "mod"; break;
					}
				printStr(op + " {{");
				print(x.a);
				printStr(" } {");
				print(x.b);
				printStr("}}");
			}
			else if (o is XPathBoolExpr)
			{
				XPathBoolExpr x = (XPathBoolExpr) o;
				System.String op = null;
				switch (x.op)
				{
					
					case XPathBoolExpr.AND:  op = "and"; break;
					
					case XPathBoolExpr.OR:  op = "or"; break;
					}
				printStr(op + " {{");
				print(x.a);
				printStr(" } {");
				print(x.b);
				printStr("}}");
			}
			else if (o is XPathCmpExpr)
			{
				XPathCmpExpr x = (XPathCmpExpr) o;
				System.String op = null;
				switch (x.op)
				{
					
					case XPathCmpExpr.LT:  op = "lt"; break;
					
					case XPathCmpExpr.LTE:  op = "lte"; break;
					
					case XPathCmpExpr.GT:  op = "gt"; break;
					
					case XPathCmpExpr.GTE:  op = "gte"; break;
					}
				printStr(op + " {{");
				print(x.a);
				printStr(" } {");
				print(x.b);
				printStr("}}");
			}
			else if (o is XPathEqExpr)
			{
				XPathEqExpr x = (XPathEqExpr) o;
				System.String op = x.equal?"eq":"neq";
				printStr(op + " {{");
				print(x.a);
				printStr(" } {");
				print(x.b);
				printStr("}}");
			}
			else if (o is XPathUnionExpr)
			{
				XPathUnionExpr x = (XPathUnionExpr) o;
				printStr("union {{");
				print(x.a);
				printStr(" } {");
				print(x.b);
				printStr("}}");
			}
			else if (o is XPathNumNegExpr)
			{
				XPathNumNegExpr x = (XPathNumNegExpr) o;
				printStr("neg {");
				print(x.a);
				printStr("}");
			}
			else if (o is XPathFuncExpr)
			{
				XPathFuncExpr x = (XPathFuncExpr) o;
				if (x.args.Length == 0)
				{
					printStr("func {" + x.id.ToString() + ", args {none}}");
				}
				else
				{
					printStr("func {" + x.id.ToString() + ", args {{");
					for (int i = 0; i < x.args.Length; i++)
					{
						print(x.args[i]);
						if (i < x.args.Length - 1)
							printStr(" } {");
					}
					printStr("}}}");
				}
			}
			else if (o is XPathPathExpr)
			{
				XPathPathExpr x = (XPathPathExpr) o;
				System.String init = null;
				
				switch (x.init_context)
				{
					
					case XPathPathExpr.INIT_CONTEXT_ROOT:  init = "root"; break;
					
					case XPathPathExpr.INIT_CONTEXT_RELATIVE:  init = "relative"; break;
					
					case XPathPathExpr.INIT_CONTEXT_EXPR:  init = "expr"; break;
					}
				
				printStr("path {init-context:" + init + ",");
				
				if (x.init_context == XPathPathExpr.INIT_CONTEXT_EXPR)
				{
					printStr(" init-expr:{");
					print(x.filtExpr);
					printStr(" }");
				}
				
				if (x.steps.Length == 0)
				{
					printStr(" steps {none}");
					printStr("}");
				}
				else
				{
					printStr(" steps {{");
					for (int i = 0; i < x.steps.Length; i++)
					{
						print(x.steps[i]);
						if (i < x.steps.Length - 1)
							printStr(" } {");
					}
					printStr("}}}");
				}
			}
			else if (o is XPathFilterExpr)
			{
				XPathFilterExpr x = (XPathFilterExpr) o;
				
				printStr("filter-expr:{{");
				print(x.x);
				
				if (x.predicates.Length == 0)
				{
					printStr(" } predicates {none}}");
				}
				else
				{
					printStr(" } predicates {{");
					for (int i = 0; i < x.predicates.Length; i++)
					{
						print(x.predicates[i]);
						if (i < x.predicates.Length - 1)
							printStr(" } {");
					}
					printStr(" }}}");
				}
			}
			else if (o is XPathStep)
			{
				XPathStep x = (XPathStep) o;
				System.String axis = null;
				System.String test = null;
				
				axis = XPathStep.axisStr(x.axis);
				test = x.testStr();
				
				if (x.predicates.Length == 0)
				{
					printStr("step {axis:" + axis + " test:" + test + " predicates {none}}");
				}
				else
				{
					printStr("step {axis:" + axis + " test:" + test + " predicates {{");
					for (int i = 0; i < x.predicates.Length; i++)
					{
						print(x.predicates[i]);
						if (i < x.predicates.Length - 1)
							printStr(" } {");
					}
					printStr("}}}");
				}
			}
			
			indent -= 1;
		}
		
		public override int GetHashCode()
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			return this.ToString().GetHashCode();
		}
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public abstract void  readExternal(System.IO.BinaryReader param1, org.javarosa.core.util.externalizable.PrototypeFactory param2);
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public abstract void  writeExternal(System.IO.BinaryWriter param1);
	}
}