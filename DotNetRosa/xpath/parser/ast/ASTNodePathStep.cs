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
using XPathStep = org.javarosa.xpath.expr.XPathStep;
using Token = org.javarosa.xpath.parser.Token;
using XPathSyntaxException = org.javarosa.xpath.parser.XPathSyntaxException;
namespace org.javarosa.xpath.parser.ast
{
	
	public class ASTNodePathStep:ASTNode
	{
		override public System.Collections.ArrayList Children
		{
			get
			{
				return predicates;
			}
			
		}
		virtual public XPathStep Step
		{
			get
			{
				if (nodeTestType == NODE_TEST_TYPE_ABBR_DOT)
				{
					return XPathStep.ABBR_SELF();
				}
				else if (nodeTestType == NODE_TEST_TYPE_ABBR_DBL_DOT)
				{
					return XPathStep.ABBR_PARENT();
				}
				else
				{
					XPathStep step;
					
					if (axisType == AXIS_TYPE_NULL)
						axisVal = XPathStep.AXIS_CHILD;
					else if (axisType == AXIS_TYPE_ABBR)
						axisVal = XPathStep.AXIS_ATTRIBUTE;
					
					if (nodeTestType == NODE_TEST_TYPE_QNAME)
						step = new XPathStep(axisVal, nodeTestQName);
					else if (nodeTestType == NODE_TEST_TYPE_WILDCARD)
						step = new XPathStep(axisVal, XPathStep.TEST_NAME_WILDCARD);
					else if (nodeTestType == NODE_TEST_TYPE_NSWILDCARD)
						step = new XPathStep(axisVal, nodeTestNamespace);
					else
					{
						System.String funcName = nodeTestFunc.name.ToString();
						int type;
						if (funcName.Equals("node"))
							type = XPathStep.TEST_TYPE_NODE;
						else if (funcName.Equals("text"))
							type = XPathStep.TEST_TYPE_TEXT;
						else if (funcName.Equals("comment"))
							type = XPathStep.TEST_TYPE_COMMENT;
						else if (funcName.Equals("processing-instruction"))
							type = XPathStep.TEST_TYPE_PROCESSING_INSTRUCTION;
						else
							throw new System.SystemException();
						
						step = new XPathStep(axisVal, type);
						if (nodeTestFunc.args.Count > 0)
						{
							step.literal = ((System.String) ((ASTNodeAbstractExpr) nodeTestFunc.args[0]).getToken(0).val);
						}
					}
					
					XPathExpression[] preds = new XPathExpression[predicates.Count];
					for (int i = 0; i < preds.Length; i++)
						preds[i] = ((ASTNode) predicates[i]).build();
					step.predicates = preds;
					
					return step;
				}
			}
			
		}
		public const int AXIS_TYPE_ABBR = 1;
		public const int AXIS_TYPE_EXPLICIT = 2;
		public const int AXIS_TYPE_NULL = 3;
		
		public const int NODE_TEST_TYPE_QNAME = 1;
		public const int NODE_TEST_TYPE_WILDCARD = 2;
		public const int NODE_TEST_TYPE_NSWILDCARD = 3;
		public const int NODE_TEST_TYPE_ABBR_DOT = 4;
		public const int NODE_TEST_TYPE_ABBR_DBL_DOT = 5;
		public const int NODE_TEST_TYPE_FUNC = 6;
		
		public int axisType;
		public int axisVal;
		public int nodeTestType;
		public ASTNodeFunctionCall nodeTestFunc;
		public XPathQName nodeTestQName;
		public System.String nodeTestNamespace;
		public System.Collections.ArrayList predicates;
		
		public ASTNodePathStep()
		{
			predicates = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
		}
		
		public override XPathExpression build()
		{
			return null;
		}
		
		public static int validateAxisName(System.String axisName)
		{
			int axis = - 1;
			
			if (axisName.Equals("child"))
				axis = XPathStep.AXIS_CHILD;
			else if (axisName.Equals("descendant"))
				axis = XPathStep.AXIS_DESCENDANT;
			else if (axisName.Equals("parent"))
				axis = XPathStep.AXIS_PARENT;
			else if (axisName.Equals("ancestor"))
				axis = XPathStep.AXIS_ANCESTOR;
			else if (axisName.Equals("following-sibling"))
				axis = XPathStep.AXIS_FOLLOWING_SIBLING;
			else if (axisName.Equals("preceding-sibling"))
				axis = XPathStep.AXIS_PRECEDING_SIBLING;
			else if (axisName.Equals("following"))
				axis = XPathStep.AXIS_FOLLOWING;
			else if (axisName.Equals("preceding"))
				axis = XPathStep.AXIS_PRECEDING;
			else if (axisName.Equals("attribute"))
				axis = XPathStep.AXIS_ATTRIBUTE;
			else if (axisName.Equals("namespace"))
				axis = XPathStep.AXIS_NAMESPACE;
			else if (axisName.Equals("self"))
				axis = XPathStep.AXIS_SELF;
			else if (axisName.Equals("descendant-or-self"))
				axis = XPathStep.AXIS_DESCENDANT_OR_SELF;
			else if (axisName.Equals("ancestor-or-self"))
				axis = XPathStep.AXIS_ANCESTOR_OR_SELF;
			
			return axis;
		}
		
		public static bool validateNodeTypeTest(ASTNodeFunctionCall f)
		{
			System.String name = f.name.ToString();
			if (name.Equals("node") || name.Equals("text") || name.Equals("comment") || name.Equals("processing-instruction"))
			{
				if (f.args.Count == 0)
				{
					return true;
				}
				else if (name.Equals("processing-instruction") && f.args.Count == 1)
				{
					ASTNodeAbstractExpr x = (ASTNodeAbstractExpr) f.args[0];
					return x.content.Count == 1 && x.getTokenType(0) == Token.STR;
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
	}
}