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
using XPathNumericLiteral = org.javarosa.xpath.expr.XPathNumericLiteral;
using XPathQName = org.javarosa.xpath.expr.XPathQName;
using XPathStringLiteral = org.javarosa.xpath.expr.XPathStringLiteral;
using XPathVariableReference = org.javarosa.xpath.expr.XPathVariableReference;
using Parser = org.javarosa.xpath.parser.Parser;
using Token = org.javarosa.xpath.parser.Token;
using XPathSyntaxException = org.javarosa.xpath.parser.XPathSyntaxException;
namespace org.javarosa.xpath.parser.ast
{
	
	public class ASTNodeAbstractExpr:ASTNode
	{
		override public System.Collections.ArrayList Children
		{
			get
			{
				System.Collections.ArrayList children = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				for (int i = 0; i < content.Count; i++)
				{
					if (getType(i) == CHILD)
					{
						children.Add(content[i]);
					}
				}
				return children;
			}
			
		}
		virtual public bool Terminal
		{
			get
			{
				if (content.Count == 1)
				{
					int type = getTokenType(0);
					return (type == Token.NUM || type == Token.STR || type == Token.VAR);
				}
				else
				{
					return false;
				}
			}
			
		}
		virtual public bool Normalized
		{
			get
			{
				if (content.Count == 1 && getType(0) == CHILD)
				{
					ASTNode child = (ASTNode) content[0];
					if (child is ASTNodePathStep || child is ASTNodePredicate)
						throw new System.SystemException("shouldn't happen");
					return true;
				}
				else
				{
					return Terminal;
				}
			}
			
		}
		public const int CHILD = 1;
		public const int TOKEN = 2;
		
		public System.Collections.ArrayList content; //mixture of tokens and ASTNodes
		
		public ASTNodeAbstractExpr()
		{
			content = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
		}
		
		public override XPathExpression build()
		{
			if (content.Count == 1)
			{
				if (getType(0) == CHILD)
				{
					return ((ASTNode) content[0]).build();
				}
				else
				{
					switch (getTokenType(0))
					{
						
						case Token.NUM:  //UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
							return new XPathNumericLiteral(ref new System.Double[]{(System.Double) getToken(0).val}[0]);
						
						case Token.STR:  return new XPathStringLiteral((System.String) getToken(0).val);
						
						case Token.VAR:  return new XPathVariableReference((XPathQName) getToken(0).val);
						
						default:  throw new XPathSyntaxException();
						
					}
				}
			}
			else
			{
				throw new XPathSyntaxException();
			}
		}
		
		public virtual int getType(int i)
		{
			System.Object o = content[i];
			if (o is Token)
				return TOKEN;
			else if (o is ASTNode)
				return CHILD;
			else
				return - 1;
		}
		
		public virtual Token getToken(int i)
		{
			return (getType(i) == TOKEN?(Token) content[i]:null);
		}
		
		public virtual int getTokenType(int i)
		{
			Token t = getToken(i);
			return (t == null?- 1:t.type);
		}
		
		//create new node containing children from [start,end)
		public virtual ASTNodeAbstractExpr extract(int start, int end)
		{
			ASTNodeAbstractExpr node = new ASTNodeAbstractExpr();
			for (int i = start; i < end; i++)
			{
				node.content.Add(content[i]);
			}
			return node;
		}
		
		//remove children from [start,end) and replace with node n
		public virtual void  condense(ASTNode node, int start, int end)
		{
			for (int i = end - 1; i >= start; i--)
			{
				content.RemoveAt(i);
			}
			content.Insert(start, node);
		}
		
		//find the next incidence of 'target' at the current stack level
		//start points to the opening of the current stack level
		public virtual int indexOfBalanced(int start, int target, int leftPush, int rightPop)
		{
			int depth = 0;
			int i = start + 1;
			bool found = false;
			
			while (depth >= 0 && i < content.Count)
			{
				int type = getTokenType(i);
				
				if (depth == 0 && type == target)
				{
					found = true;
					break;
				}
				
				if (type == leftPush)
					depth++;
				else if (type == rightPop)
					depth--;
				
				i++;
			}
			
			return (found?i:- 1);
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'Partition' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		public class Partition
		{
			private void  InitBlock(ASTNodeAbstractExpr enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private ASTNodeAbstractExpr enclosingInstance;
			public ASTNodeAbstractExpr Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public Partition(ASTNodeAbstractExpr enclosingInstance)
			{
				InitBlock(enclosingInstance);
				pieces = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				separators = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			}
			
			public System.Collections.ArrayList pieces;
			public System.Collections.ArrayList separators;
		}
		
		//paritition the range [start,end), separating by any occurrence of separator
		public virtual Partition partition(int[] separators, int start, int end)
		{
			Partition part = new Partition(this);
			System.Collections.ArrayList sepIdxs = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			
			for (int i = start; i < end; i++)
			{
				for (int j = 0; j < separators.Length; j++)
				{
					if (getTokenType(i) == separators[j])
					{
						part.separators.Add((System.Int32) separators[j]);
						sepIdxs.Add((System.Int32) i);
						break;
					}
				}
			}
			
			for (int i = 0; i <= sepIdxs.Count; i++)
			{
				int pieceStart = (i == 0?start:Parser.vectInt(sepIdxs, i - 1) + 1);
				int pieceEnd = (i == sepIdxs.Count?end:Parser.vectInt(sepIdxs, i));
				part.pieces.Add(extract(pieceStart, pieceEnd));
			}
			
			return part;
		}
		
		//partition by sep, to the end of the current stack level
		//start is the opening token of the current stack level
		public virtual Partition partitionBalanced(int sep, int start, int leftPush, int rightPop)
		{
			Partition part = new Partition(this);
			System.Collections.ArrayList sepIdxs = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			int end = indexOfBalanced(start, rightPop, leftPush, rightPop);
			if (end == - 1)
				return null;
			
			int k = start;
			do 
			{
				k = indexOfBalanced(k, sep, leftPush, rightPop);
				if (k != - 1)
				{
					sepIdxs.Add((System.Int32) k);
					part.separators.Add((System.Int32) sep);
				}
			}
			while (k != - 1);
			
			for (int i = 0; i <= sepIdxs.Count; i++)
			{
				int pieceStart = (i == 0?start + 1:Parser.vectInt(sepIdxs, i - 1) + 1);
				int pieceEnd = (i == sepIdxs.Count?end:Parser.vectInt(sepIdxs, i));
				part.pieces.Add(extract(pieceStart, pieceEnd));
			}
			
			return part;
		}
	}
}