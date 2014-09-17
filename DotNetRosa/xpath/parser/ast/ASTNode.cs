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
using XPathExpression = org.javarosa.xpath.expr.XPathExpression;
using Parser = org.javarosa.xpath.parser.Parser;
using Token = org.javarosa.xpath.parser.Token;
using XPathSyntaxException = org.javarosa.xpath.parser.XPathSyntaxException;
namespace org.javarosa.xpath.parser.ast
{
	
	public abstract class ASTNode
	{
		public abstract System.Collections.ArrayList Children{get;}
		public abstract XPathExpression build();
		
		//horrible debugging code
		
		internal int indent;
		
		private void  printStr(System.String s)
		{
			for (int i = 0; i < 2 * indent; i++)
				System.Console.Out.Write(" ");
			System.Console.Out.WriteLine(s);
		}
		
		public virtual void  print()
		{
			indent = - 1;
			print(this);
		}
		
		public virtual void  print(System.Object o)
		{
			indent += 1;
			
			if (o is ASTNodeAbstractExpr)
			{
				ASTNodeAbstractExpr x = (ASTNodeAbstractExpr) o;
				printStr("abstractexpr {");
				for (int i = 0; i < x.content.Count; i++)
				{
					if (x.getType(i) == ASTNodeAbstractExpr.CHILD)
						print(x.content[i]);
					else
					{
						printStr(x.getToken(i).ToString());
					}
				}
				printStr("}");
			}
			else if (o is ASTNodePredicate)
			{
				ASTNodePredicate x = (ASTNodePredicate) o;
				printStr("predicate {");
				print(x.expr);
				printStr("}");
			}
			else if (o is ASTNodeFunctionCall)
			{
				ASTNodeFunctionCall x = (ASTNodeFunctionCall) o;
				if (x.args.Count == 0)
				{
					printStr("func {" + x.name.ToString() + ", args {none}}");
				}
				else
				{
					printStr("func {" + x.name.ToString() + ", args {{");
					for (int i = 0; i < x.args.Count; i++)
					{
						print(x.args[i]);
						if (i < x.args.Count - 1)
							printStr(" } {");
					}
					printStr("}}}");
				}
			}
			else if (o is ASTNodeBinaryOp)
			{
				ASTNodeBinaryOp x = (ASTNodeBinaryOp) o;
				printStr("opexpr {");
				for (int i = 0; i < x.exprs.Count; i++)
				{
					print(x.exprs[i]);
					if (i < x.exprs.Count - 1)
					{
						switch (Parser.vectInt(x.ops, i))
						{
							
							case Token.AND:  printStr("and:"); break;
							
							case Token.OR:  printStr("or:"); break;
							
							case Token.EQ:  printStr("eq:"); break;
							
							case Token.NEQ:  printStr("neq:"); break;
							
							case Token.LT:  printStr("lt:"); break;
							
							case Token.LTE:  printStr("lte:"); break;
							
							case Token.GT:  printStr("gt:"); break;
							
							case Token.GTE:  printStr("gte:"); break;
							
							case Token.PLUS:  printStr("plus:"); break;
							
							case Token.MINUS:  printStr("minus:"); break;
							
							case Token.DIV:  printStr("div:"); break;
							
							case Token.MOD:  printStr("mod:"); break;
							
							case Token.MULT:  printStr("mult:"); break;
							
							case Token.UNION:  printStr("union:"); break;
							}
					}
				}
				printStr("}");
			}
			else if (o is ASTNodeUnaryOp)
			{
				ASTNodeUnaryOp x = (ASTNodeUnaryOp) o;
				printStr("opexpr {");
				switch (x.op)
				{
					
					case Token.UMINUS:  printStr("num-neg:"); break;
					}
				print(x.expr);
				printStr("}");
			}
			else if (o is ASTNodeLocPath)
			{
				ASTNodeLocPath x = (ASTNodeLocPath) o;
				printStr("pathexpr {");
				int offset = x.Absolute?1:0;
				for (int i = 0; i < x.clauses.Count + offset; i++)
				{
					if (offset == 0 || i > 0)
						print(x.clauses[i - offset]);
					if (i < x.separators.Count)
					{
						switch (Parser.vectInt(x.separators, i))
						{
							
							case Token.DBL_SLASH:  printStr("dbl-slash:"); break;
							
							case Token.SLASH:  printStr("slash:"); break;
							}
					}
				}
				printStr("}");
			}
			else if (o is ASTNodePathStep)
			{
				ASTNodePathStep x = (ASTNodePathStep) o;
				printStr("step {axis: " + x.axisType + " node test type: " + x.nodeTestType);
				if (x.axisType == ASTNodePathStep.AXIS_TYPE_EXPLICIT)
					printStr("  axis type: " + x.axisVal);
				if (x.nodeTestType == ASTNodePathStep.NODE_TEST_TYPE_QNAME)
				{
					printStr("  node test name: " + x.nodeTestQName.ToString());
				}
				if (x.nodeTestType == ASTNodePathStep.NODE_TEST_TYPE_FUNC)
					print(x.nodeTestFunc);
				printStr("predicates...");
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				for (System.Collections.IEnumerator e = x.predicates.GetEnumerator(); e.MoveNext(); )
				{
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					print(e.Current);
				}
				printStr("}");
			}
			else if (o is ASTNodeFilterExpr)
			{
				ASTNodeFilterExpr x = (ASTNodeFilterExpr) o;
				printStr("filter expr {");
				print(x.expr);
				printStr("predicates...");
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				for (System.Collections.IEnumerator e = x.predicates.GetEnumerator(); e.MoveNext(); )
				{
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					print(e.Current);
				}
				printStr("}");
			}
			
			indent -= 1;
		}
	}
}