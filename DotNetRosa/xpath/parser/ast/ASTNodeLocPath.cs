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
using XPathFilterExpr = org.javarosa.xpath.expr.XPathFilterExpr;
using XPathPathExpr = org.javarosa.xpath.expr.XPathPathExpr;
using XPathStep = org.javarosa.xpath.expr.XPathStep;
using Parser = org.javarosa.xpath.parser.Parser;
using Token = org.javarosa.xpath.parser.Token;
using XPathSyntaxException = org.javarosa.xpath.parser.XPathSyntaxException;
namespace org.javarosa.xpath.parser.ast
{
	
	public class ASTNodeLocPath:ASTNode
	{
		override public System.Collections.ArrayList Children
		{
			get
			{
				return clauses;
			}
			
		}
		virtual public bool Absolute
		{
			get
			{
				return (clauses.Count == separators.Count) || (clauses.Count == 0 && separators.Count == 1);
			}
			
		}
		public System.Collections.ArrayList clauses;
		public System.Collections.ArrayList separators;
		
		public ASTNodeLocPath()
		{
			clauses = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			separators = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
		}
		
		public override XPathExpression build()
		{
			System.Collections.ArrayList steps = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			XPathExpression filtExpr = null;
			int offset = Absolute?1:0;
			for (int i = 0; i < clauses.Count + offset; i++)
			{
				if (offset == 0 || i > 0)
				{
					if (clauses[i - offset] is ASTNodePathStep)
					{
						steps.Add(((ASTNodePathStep) clauses[i - offset]).Step);
					}
					else
					{
						filtExpr = ((ASTNode) clauses[i - offset]).build();
					}
				}
				
				if (i < separators.Count)
				{
					if (Parser.vectInt(separators, i) == Token.DBL_SLASH)
					{
						steps.Add(XPathStep.ABBR_DESCENDANTS());
					}
				}
			}
			
			XPathStep[] stepArr = new XPathStep[steps.Count];
			for (int i = 0; i < stepArr.Length; i++)
				stepArr[i] = (XPathStep) steps[i];
			
			if (filtExpr == null)
			{
				return new XPathPathExpr(Absolute?XPathPathExpr.INIT_CONTEXT_ROOT:XPathPathExpr.INIT_CONTEXT_RELATIVE, stepArr);
			}
			else
			{
				if (filtExpr is XPathFilterExpr)
				{
					return new XPathPathExpr((XPathFilterExpr) filtExpr, stepArr);
				}
				else
				{
					return new XPathPathExpr(new XPathFilterExpr(filtExpr, new XPathExpression[0]), stepArr);
				}
			}
		}
	}
}