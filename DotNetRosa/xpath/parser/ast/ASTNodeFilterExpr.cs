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
using XPathSyntaxException = org.javarosa.xpath.parser.XPathSyntaxException;
namespace org.javarosa.xpath.parser.ast
{
	
	public class ASTNodeFilterExpr:ASTNode
	{
		override public System.Collections.ArrayList Children
		{
			get
			{
				System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				v.Add(expr);
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				for (System.Collections.IEnumerator e = predicates.GetEnumerator(); e.MoveNext(); )
				{
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					v.Add(e.Current);
				}
				return v;
			}
			
		}
		public ASTNodeAbstractExpr expr;
		public System.Collections.ArrayList predicates;
		
		public ASTNodeFilterExpr()
		{
			predicates = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
		}
		
		public override XPathExpression build()
		{
			XPathExpression[] preds = new XPathExpression[predicates.Count];
			for (int i = 0; i < preds.Length; i++)
				preds[i] = ((ASTNode) predicates[i]).build();
			
			return new XPathFilterExpr(expr.build(), preds);
		}
	}
}