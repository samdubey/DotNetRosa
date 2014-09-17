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
using XPathQName = org.javarosa.xpath.expr.XPathQName;
using ASTNode = org.javarosa.xpath.parser.ast.ASTNode;
using ASTNodeAbstractExpr = org.javarosa.xpath.parser.ast.ASTNodeAbstractExpr;
using ASTNodeBinaryOp = org.javarosa.xpath.parser.ast.ASTNodeBinaryOp;
using ASTNodeFilterExpr = org.javarosa.xpath.parser.ast.ASTNodeFilterExpr;
using ASTNodeFunctionCall = org.javarosa.xpath.parser.ast.ASTNodeFunctionCall;
using ASTNodeLocPath = org.javarosa.xpath.parser.ast.ASTNodeLocPath;
using ASTNodePathStep = org.javarosa.xpath.parser.ast.ASTNodePathStep;
using ASTNodePredicate = org.javarosa.xpath.parser.ast.ASTNodePredicate;
using ASTNodeUnaryOp = org.javarosa.xpath.parser.ast.ASTNodeUnaryOp;
namespace org.javarosa.xpath.parser
{
	
	/* if you try to edit this code, you will likely break it */
	
	public class Parser
	{
		private class AnonymousClassSubNodeFactory:SubNodeFactory
		{
			public override ASTNode newNode(ASTNodeAbstractExpr node)
			{
				return node;
			}
		}
		private class AnonymousClassSubNodeFactory1:SubNodeFactory
		{
			public override ASTNode newNode(ASTNodeAbstractExpr node)
			{
				ASTNodePredicate p = new ASTNodePredicate();
				p.expr = node;
				return p;
			}
		}
		
		public static XPathExpression parse(System.Collections.ArrayList tokens)
		{
			ASTNode tree = buildParseTree(tokens);
			return tree.build();
		}
		
		public static ASTNode buildParseTree(System.Collections.ArrayList tokens)
		{
			ASTNodeAbstractExpr root = new ASTNodeAbstractExpr();
			for (int i = 0; i < tokens.Count; i++)
				root.content.Add(tokens[i]);
			
			parseFuncCalls(root);
			parseParens(root);
			parsePredicates(root);
			parseOperators(root);
			parsePathExpr(root);
			verifyBaseExpr(root);
			
			return root;
		}
		
		private static void  parseOperators(ASTNode root)
		{
			int[] orOp = new int[]{Token.OR};
			int[] andOp = new int[]{Token.AND};
			int[] eqOps = new int[]{Token.EQ, Token.NEQ};
			int[] cmpOps = new int[]{Token.LT, Token.LTE, Token.GT, Token.GTE};
			int[] addOps = new int[]{Token.PLUS, Token.MINUS};
			int[] multOps = new int[]{Token.MULT, Token.DIV, Token.MOD};
			int[] unionOp = new int[]{Token.UNION};
			
			parseBinaryOp(root, orOp, ASTNodeBinaryOp.ASSOC_RIGHT);
			parseBinaryOp(root, andOp, ASTNodeBinaryOp.ASSOC_RIGHT);
			parseBinaryOp(root, eqOps, ASTNodeBinaryOp.ASSOC_LEFT);
			parseBinaryOp(root, cmpOps, ASTNodeBinaryOp.ASSOC_LEFT);
			parseBinaryOp(root, addOps, ASTNodeBinaryOp.ASSOC_LEFT);
			parseBinaryOp(root, multOps, ASTNodeBinaryOp.ASSOC_LEFT);
			parseUnaryOp(root, Token.UMINUS);
			parseBinaryOp(root, unionOp, ASTNodeBinaryOp.ASSOC_LEFT); /* 'a|-b' parses weird (as in, doesn't), but i think that's correct */
		}
		
		//find and condense all function calls in the current level, then do the same in lower levels
		private static void  parseFuncCalls(ASTNode node)
		{
			if (node is ASTNodeAbstractExpr)
			{
				ASTNodeAbstractExpr absNode = (ASTNodeAbstractExpr) node;
				
				int i = 0;
				while (i < absNode.content.Count - 1)
				{
					if (absNode.getTokenType(i + 1) == Token.LPAREN && absNode.getTokenType(i) == Token.QNAME)
						condenseFuncCall(absNode, i);
					i++;
				}
			}
			
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator e = node.Children.GetEnumerator(); e.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				parseFuncCalls((ASTNode) e.Current);
			}
		}
		
		//i == index of token beginning func call (func name)
		private static void  condenseFuncCall(ASTNodeAbstractExpr node, int funcStart)
		{
			ASTNodeFunctionCall funcCall = new ASTNodeFunctionCall((XPathQName) node.getToken(funcStart).val);
			
			int funcEnd = node.indexOfBalanced(funcStart + 1, Token.RPAREN, Token.LPAREN, Token.RPAREN);
			if (funcEnd == - 1)
			{
				throw new XPathSyntaxException("Mismatched brackets or parentheses"); //mismatched parens
			}
			
			ASTNodeAbstractExpr.Partition args = node.partitionBalanced(Token.COMMA, funcStart + 1, Token.LPAREN, Token.RPAREN);
			if (args.pieces.Count == 1 && ((ASTNodeAbstractExpr) args.pieces[0]).content.Count == 0)
			{
				//no arguments
			}
			else
			{
				//process arguments
				funcCall.args = args.pieces;
			}
			
			node.condense(funcCall, funcStart, funcEnd + 1);
		}
		
		private static void  parseParens(ASTNode node)
		{
			parseBalanced(node, new AnonymousClassSubNodeFactory(), Token.LPAREN, Token.RPAREN);
		}
		
		private static void  parsePredicates(ASTNode node)
		{
			parseBalanced(node, new AnonymousClassSubNodeFactory1(), Token.LBRACK, Token.RBRACK);
		}
		
		private abstract class SubNodeFactory
		{
			public abstract ASTNode newNode(ASTNodeAbstractExpr node);
		}
		
		private static void  parseBalanced(ASTNode node, SubNodeFactory snf, int lToken, int rToken)
		{
			if (node is ASTNodeAbstractExpr)
			{
				ASTNodeAbstractExpr absNode = (ASTNodeAbstractExpr) node;
				
				int i = 0;
				while (i < absNode.content.Count)
				{
					int type = absNode.getTokenType(i);
					if (type == rToken)
					{
						throw new XPathSyntaxException("Unbalanced brackets or parentheses!"); //unbalanced
					}
					else if (type == lToken)
					{
						int j = absNode.indexOfBalanced(i, rToken, lToken, rToken);
						if (j == - 1)
						{
							throw new XPathSyntaxException("mismatched brackets or parentheses!"); //mismatched
						}
						
						absNode.condense(snf.newNode(absNode.extract(i + 1, j)), i, j + 1);
					}
					i++;
				}
			}
			
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator e = node.Children.GetEnumerator(); e.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				parseBalanced((ASTNode) e.Current, snf, lToken, rToken);
			}
		}
		
		private static void  parseBinaryOp(ASTNode node, int[] ops, int associativity)
		{
			if (node is ASTNodeAbstractExpr)
			{
				ASTNodeAbstractExpr absNode = (ASTNodeAbstractExpr) node;
				ASTNodeAbstractExpr.Partition part = absNode.partition(ops, 0, absNode.content.Count);
				
				if (part.separators.Count == 0)
				{
					//no occurrences of op
				}
				else
				{
					ASTNodeBinaryOp binOp = new ASTNodeBinaryOp();
					binOp.associativity = associativity;
					binOp.exprs = part.pieces;
					binOp.ops = part.separators;
					
					absNode.condense(binOp, 0, absNode.content.Count);
				}
			}
			
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator e = node.Children.GetEnumerator(); e.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				parseBinaryOp((ASTNode) e.Current, ops, associativity);
			}
		}
		
		private static void  parseUnaryOp(ASTNode node, int op)
		{
			if (node is ASTNodeAbstractExpr)
			{
				ASTNodeAbstractExpr absNode = (ASTNodeAbstractExpr) node;
				
				if (absNode.content.Count > 0 && absNode.getTokenType(0) == op)
				{
					ASTNodeUnaryOp unOp = new ASTNodeUnaryOp();
					unOp.op = op;
					unOp.expr = (absNode.content.Count > 1?absNode.extract(1, absNode.content.Count):new ASTNodeAbstractExpr());
					absNode.condense(unOp, 0, absNode.content.Count);
				}
			}
			
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator e = node.Children.GetEnumerator(); e.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				parseUnaryOp((ASTNode) e.Current, op);
			}
		}
		
		private static void  parsePathExpr(ASTNode node)
		{
			if (node is ASTNodeAbstractExpr)
			{
				ASTNodeAbstractExpr absNode = (ASTNodeAbstractExpr) node;
				int[] pathOps = new int[]{Token.SLASH, Token.DBL_SLASH};
				ASTNodeAbstractExpr.Partition part = absNode.partition(pathOps, 0, absNode.content.Count);
				
				if (part.separators.Count == 0)
				{
					//filter expression or standalone step
					if (isStep(absNode))
					{
						ASTNodePathStep step = parseStep(absNode);
						ASTNodeLocPath path = new ASTNodeLocPath();
						path.clauses.Add(step);
						absNode.condense(path, 0, absNode.content.Count);
					}
					else
					{
						//filter expr
						ASTNodeFilterExpr filt = parseFilterExp(absNode);
						if (filt != null)
						{
							absNode.condense(filt, 0, absNode.content.Count);
						}
					}
				}
				else
				{
					//path expression (but first clause may be filter expr)
					ASTNodeLocPath path = new ASTNodeLocPath();
					path.separators = part.separators;
					
					if (part.separators.Count == 1 && absNode.content.Count == 1 && vectInt(part.separators, 0) == Token.SLASH)
					{
						//empty absolute path
					}
					else
					{
						for (int i = 0; i < part.pieces.Count; i++)
						{
							ASTNodeAbstractExpr x = (ASTNodeAbstractExpr) part.pieces[i];
							if (isStep(x))
							{
								ASTNodePathStep step = parseStep(x);
								path.clauses.Add(step);
							}
							else
							{
								if (i == 0)
								{
									if (x.content.Count == 0)
									{
										//absolute path expr; first clause is null
										/* do nothing */
									}
									else
									{
										//filter expr
										ASTNodeFilterExpr filt = parseFilterExp(x);
										if (filt != null)
											path.clauses.Add(filt);
										else
											path.clauses.Add(x);
									}
								}
								else
								{
									throw new XPathSyntaxException("Unexpected beginning of path");
								}
							}
						}
					}
					absNode.condense(path, 0, absNode.content.Count);
				}
			}
			
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator e = node.Children.GetEnumerator(); e.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				parsePathExpr((ASTNode) e.Current);
			}
		}
		
		//true if 'node' is potentially a step, as opposed to a filter expr
		private static bool isStep(ASTNodeAbstractExpr node)
		{
			if (node.content.Count > 0)
			{
				int type = node.getTokenType(0);
				if (type == Token.QNAME || type == Token.WILDCARD || type == Token.NSWILDCARD || type == Token.AT || type == Token.DOT || type == Token.DBL_DOT)
				{
					return true;
				}
				else if (node.content[0] is ASTNodeFunctionCall)
				{
					System.String name = ((ASTNodeFunctionCall) node.content[0]).name.ToString();
					return (name.Equals("node") || name.Equals("text") || name.Equals("comment") || name.Equals("processing-instruction"));
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		
		//please kill me
		private static ASTNodePathStep parseStep(ASTNodeAbstractExpr node)
		{
			ASTNodePathStep step = new ASTNodePathStep();
			if (node.content.Count == 1 && node.getTokenType(0) == Token.DOT)
			{
				step.axisType = ASTNodePathStep.AXIS_TYPE_NULL;
				step.nodeTestType = ASTNodePathStep.NODE_TEST_TYPE_ABBR_DOT;
			}
			else if (node.content.Count == 1 && node.getTokenType(0) == Token.DBL_DOT)
			{
				step.axisType = ASTNodePathStep.AXIS_TYPE_NULL;
				step.nodeTestType = ASTNodePathStep.NODE_TEST_TYPE_ABBR_DBL_DOT;
			}
			else
			{
				int i = 0;
				if (node.content.Count > 0 && node.getTokenType(0) == Token.AT)
				{
					step.axisType = ASTNodePathStep.AXIS_TYPE_ABBR;
					i += 1;
				}
				else if (node.content.Count > 1 && node.getTokenType(0) == Token.QNAME && node.getTokenType(1) == Token.DBL_COLON)
				{
					int axisVal = ASTNodePathStep.validateAxisName(((XPathQName) node.getToken(0).val).ToString());
					if (axisVal == - 1)
					{
						throw new XPathSyntaxException("Invalid Axis: " + ((XPathQName) node.getToken(0).val).ToString());
					}
					step.axisType = ASTNodePathStep.AXIS_TYPE_EXPLICIT;
					step.axisVal = axisVal;
					i += 2;
				}
				else
				{
					step.axisType = ASTNodePathStep.AXIS_TYPE_NULL;
				}
				
				if (node.content.Count > i && node.getTokenType(i) == Token.WILDCARD)
				{
					step.nodeTestType = ASTNodePathStep.NODE_TEST_TYPE_WILDCARD;
				}
				else if (node.content.Count > i && node.getTokenType(i) == Token.NSWILDCARD)
				{
					step.nodeTestType = ASTNodePathStep.NODE_TEST_TYPE_NSWILDCARD;
					step.nodeTestNamespace = ((System.String) node.getToken(i).val);
				}
				else if (node.content.Count > i && node.getTokenType(i) == Token.QNAME)
				{
					step.nodeTestType = ASTNodePathStep.NODE_TEST_TYPE_QNAME;
					step.nodeTestQName = (XPathQName) node.getToken(i).val;
				}
				else if (node.content.Count > i && node.content[i] is ASTNodeFunctionCall)
				{
					if (!ASTNodePathStep.validateNodeTypeTest((ASTNodeFunctionCall) node.content[i]))
					{
						throw new XPathSyntaxException();
					}
					step.nodeTestType = ASTNodePathStep.NODE_TEST_TYPE_FUNC;
					step.nodeTestFunc = (ASTNodeFunctionCall) node.content[i];
				}
				else
				{
					throw new XPathSyntaxException();
				}
				i += 1;
				
				while (i < node.content.Count)
				{
					if (node.content[i] is ASTNodePredicate)
					{
						step.predicates.Add(node.content[i]);
					}
					else
					{
						throw new XPathSyntaxException();
					}
					i++;
				}
			}
			
			return step;
		}
		
		private static ASTNodeFilterExpr parseFilterExp(ASTNodeAbstractExpr node)
		{
			ASTNodeFilterExpr filt = new ASTNodeFilterExpr();
			int i;
			for (i = node.content.Count - 1; i >= 0; i--)
			{
				if (node.content[i] is ASTNodePredicate)
				{
					filt.predicates.Insert(0, node.content[i]);
				}
				else
				{
					break;
				}
			}
			
			if (filt.predicates.Count == 0)
				return null;
			
			filt.expr = node.extract(0, i + 1);
			return filt;
		}
		
		public static void  verifyBaseExpr(ASTNode node)
		{
			if (node is ASTNodeAbstractExpr)
			{
				ASTNodeAbstractExpr absNode = (ASTNodeAbstractExpr) node;
				
				if (!absNode.Normalized)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					throw new XPathSyntaxException("Bad node: " + absNode.ToString());
				}
			}
			
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator e = node.Children.GetEnumerator(); e.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				verifyBaseExpr((ASTNode) e.Current);
			}
		}
		
		public static int vectInt(System.Collections.ArrayList v, int i)
		{
			return ((System.Int32) v[i]);
		}
	}
}