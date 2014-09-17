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
using XPathQName = org.javarosa.xpath.expr.XPathQName;
namespace org.javarosa.xpath.parser
{
	
	public class Lexer
	{
		
		private const int CONTEXT_LENGTH = 15;
		
		public const int LEX_CONTEXT_VAL = 1;
		public const int LEX_CONTEXT_OP = 2;
		
		public static System.Collections.ArrayList lex(System.String expr)
		{
			System.Collections.ArrayList tokens = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			
			int i = 0;
			int context = LEX_CONTEXT_VAL;
			
			while (i < expr.Length)
			{
				int c = expr[i];
				int d = getChar(expr, i + 1);
				
				Token token = null;
				int skip = 1;
				
				if (" \n\t\f\r".IndexOf((System.Char) c) >= 0)
				{
					/* whitespace; do nothing */
				}
				else if (c == '=')
				{
					token = new Token(Token.EQ);
				}
				else if (c == '!' && d == '=')
				{
					token = new Token(Token.NEQ);
					skip = 2;
				}
				else if (c == '<')
				{
					if (d == '=')
					{
						token = new Token(Token.LTE);
						skip = 2;
					}
					else
					{
						token = new Token(Token.LT);
					}
				}
				else if (c == '>')
				{
					if (d == '=')
					{
						token = new Token(Token.GTE);
						skip = 2;
					}
					else
					{
						token = new Token(Token.GT);
					}
				}
				else if (c == '+')
				{
					token = new Token(Token.PLUS);
				}
				else if (c == '-')
				{
					token = new Token(context == LEX_CONTEXT_VAL?Token.UMINUS:Token.MINUS); //not sure this is entirely correct
				}
				else if (c == '*')
				{
					token = new Token(context == LEX_CONTEXT_VAL?Token.WILDCARD:Token.MULT);
				}
				else if (c == '|')
				{
					token = new Token(Token.UNION);
				}
				else if (c == '/')
				{
					if (d == '/')
					{
						token = new Token(Token.DBL_SLASH);
						skip = 2;
					}
					else
					{
						token = new Token(Token.SLASH);
					}
				}
				else if (c == '[')
				{
					token = new Token(Token.LBRACK);
				}
				else if (c == ']')
				{
					token = new Token(Token.RBRACK);
				}
				else if (c == '(')
				{
					token = new Token(Token.LPAREN);
				}
				else if (c == ')')
				{
					token = new Token(Token.RPAREN);
				}
				else if (c == '.')
				{
					if (d == '.')
					{
						token = new Token(Token.DBL_DOT);
						skip = 2;
					}
					else if (isDigit(d))
					{
						skip = matchNumeric(expr, i);
						token = new Token(Token.NUM, (System.Object) System.Double.Parse(expr.Substring(i, (i + skip) - (i))));
					}
					else
					{
						token = new Token(Token.DOT);
					}
				}
				else if (c == '@')
				{
					token = new Token(Token.AT);
				}
				else if (c == ',')
				{
					token = new Token(Token.COMMA);
				}
				else if (c == ':' && d == ':')
				{
					token = new Token(Token.DBL_COLON);
					skip = 2;
				}
				else if (context == LEX_CONTEXT_OP && i + 3 <= expr.Length && "and".Equals(expr.Substring(i, (i + 3) - (i))))
				{
					token = new Token(Token.AND);
					skip = 3;
				}
				else if (context == LEX_CONTEXT_OP && i + 2 <= expr.Length && "or".Equals(expr.Substring(i, (i + 2) - (i))))
				{
					token = new Token(Token.OR);
					skip = 2;
				}
				else if (context == LEX_CONTEXT_OP && i + 3 <= expr.Length && "div".Equals(expr.Substring(i, (i + 3) - (i))))
				{
					token = new Token(Token.DIV);
					skip = 3;
				}
				else if (context == LEX_CONTEXT_OP && i + 3 <= expr.Length && "mod".Equals(expr.Substring(i, (i + 3) - (i))))
				{
					token = new Token(Token.MOD);
					skip = 3;
				}
				else if (c == '$')
				{
					int len = matchQName(expr, i + 1);
					if (len == 0)
					{
						badParse(expr, i, (char) c);
					}
					else
					{
						token = new Token(Token.VAR, new XPathQName(expr.Substring(i + 1, (i + len + 1) - (i + 1))));
						skip = len + 1;
					}
				}
				else if (c == '\'' || c == '\"')
				{
					//UPGRADE_WARNING: Method 'java.lang.String.indexOf' was converted to 'System.String.IndexOf' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
					int end = expr.IndexOf((System.Char) c, i + 1);
					if (end == - 1)
					{
						badParse(expr, i, (char) c);
					}
					else
					{
						token = new Token(Token.STR, expr.Substring(i + 1, (end) - (i + 1)));
						skip = (end - i) + 1;
					}
				}
				else if (isDigit(c))
				{
					skip = matchNumeric(expr, i);
					token = new Token(Token.NUM, (System.Object) System.Double.Parse(expr.Substring(i, (i + skip) - (i))));
				}
				else if (context == LEX_CONTEXT_VAL && (isAlpha(c) || c == '_'))
				{
					int len = matchQName(expr, i);
					System.String name = expr.Substring(i, (i + len) - (i));
					if (name.IndexOf(':') == - 1 && getChar(expr, i + len) == ':' && getChar(expr, i + len + 1) == '*')
					{
						token = new Token(Token.NSWILDCARD, name);
						skip = len + 2;
					}
					else
					{
						token = new Token(Token.QNAME, new XPathQName(name));
						skip = len;
					}
				}
				else
				{
					badParse(expr, i, (char) c);
				}
				if (token != null)
				{
					if (token.type == Token.WILDCARD || token.type == Token.NSWILDCARD || token.type == Token.QNAME || token.type == Token.VAR || token.type == Token.NUM || token.type == Token.STR || token.type == Token.RBRACK || token.type == Token.RPAREN || token.type == Token.DOT || token.type == Token.DBL_DOT)
					{
						context = LEX_CONTEXT_OP;
					}
					else
					{
						context = LEX_CONTEXT_VAL;
					}
					
					tokens.Add(token);
				}
				i += skip;
			}
			
			return tokens;
		}
		
		private static void  badParse(System.String expr, int i, char c)
		{
			
			System.String start = "\u034E" + c;
			System.String preContext = (System.Math.Max(0, i - CONTEXT_LENGTH) != 0?"...":"") + expr.Substring(System.Math.Max(0, i - CONTEXT_LENGTH), (System.Math.Max(0, i)) - (System.Math.Max(0, i - CONTEXT_LENGTH))).Trim();
			System.String postcontext = i == expr.Length - 1?"":expr.Substring(System.Math.Min(i + 1, expr.Length - 1), (System.Math.Min(i + CONTEXT_LENGTH, expr.Length)) - (System.Math.Min(i + 1, expr.Length - 1))).Trim() + (System.Math.Min(i + CONTEXT_LENGTH, expr.Length) != expr.Length?"...":"");
			
			throw new XPathSyntaxException("Couldn't understand the expression starting at this point: " + (preContext + start + postcontext));
		}
		
		private static int matchNumeric(System.String expr, int i)
		{
			bool seenDecimalPoint = false;
			int start = i;
			int c;
			
			for (; i < expr.Length; i++)
			{
				c = expr[i];
				
				if (!(isDigit(c) || (!seenDecimalPoint && c == '.')))
					break;
				
				if (c == '.')
					seenDecimalPoint = true;
			}
			
			return i - start;
		}
		
		private static int matchQName(System.String expr, int i)
		{
			int len = matchNCName(expr, i);
			
			if (len > 0 && getChar(expr, i + len) == ':')
			{
				int len2 = matchNCName(expr, i + len + 1);
				
				if (len2 > 0)
					len += len2 + 1;
			}
			
			return len;
		}
		
		private static int matchNCName(System.String expr, int i)
		{
			int start = i;
			int c;
			
			for (; i < expr.Length; i++)
			{
				c = expr[i];
				
				if (!(isAlpha(c) || c == '_' || (i > start && (isDigit(c) || c == '.' || c == '-'))))
					break;
			}
			
			return i - start;
		}
		
		//get char from string, return -1 for EOF
		private static int getChar(System.String expr, int i)
		{
			return (i < expr.Length?expr[i]:- 1);
		}
		
		private static bool isDigit(int c)
		{
			return (c < 0?false:System.Char.IsDigit((char) c));
		}
		
		private static bool isAlpha(int c)
		{
			return (c < 0?false:System.Char.IsLower((char) c) || System.Char.IsUpper((char) c));
		}
	}
}