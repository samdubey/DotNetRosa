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
	
	public class Token
	{
		public const int AND = 1;
		public const int AT = 2;
		public const int COMMA = 3;
		public const int DBL_COLON = 4;
		public const int DBL_DOT = 5;
		public const int DBL_SLASH = 6;
		public const int DIV = 7;
		public const int DOT = 8;
		public const int EQ = 9;
		public const int GT = 10;
		public const int GTE = 11;
		public const int LBRACK = 12;
		public const int LPAREN = 13;
		public const int LT = 14;
		public const int LTE = 15;
		public const int MINUS = 16;
		public const int MOD = 17;
		public const int MULT = 18;
		public const int NEQ = 19;
		public const int NSWILDCARD = 20;
		public const int NUM = 21;
		public const int OR = 22;
		public const int PLUS = 23;
		public const int QNAME = 24;
		public const int RBRACK = 25;
		public const int RPAREN = 26;
		public const int SLASH = 27;
		public const int STR = 28;
		public const int UMINUS = 29;
		public const int UNION = 30;
		public const int VAR = 31;
		public const int WILDCARD = 32;
		
		public int type;
		public System.Object val;
		
		public Token(int type):this(type, null)
		{
		}
		
		public Token(int type, System.Object val)
		{
			this.type = type;
			this.val = val;
		}
		
		public override System.String ToString()
		{
			System.String s = null;
			
			switch (type)
			{
				
				case AND:  s = "AND"; break;
				
				case AT:  s = "AT"; break;
				
				case COMMA:  s = "COMMA"; break;
				
				case DBL_COLON:  s = "DBL_COLON"; break;
				
				case DBL_DOT:  s = "DBL_DOT"; break;
				
				case DBL_SLASH:  s = "DBL_SLASH"; break;
				
				case DIV:  s = "DIV"; break;
				
				case DOT:  s = "DOT"; break;
				
				case EQ:  s = "EQ"; break;
				
				case GT:  s = "GT"; break;
				
				case GTE:  s = "GTE"; break;
				
				case LBRACK:  s = "LBRACK"; break;
				
				case LPAREN:  s = "LPAREN"; break;
				
				case LT:  s = "LT"; break;
				
				case LTE:  s = "LTE"; break;
				
				case MINUS:  s = "MINUS"; break;
				
				case MOD:  s = "MOD"; break;
				
				case MULT:  s = "MULT"; break;
				
				case NEQ:  s = "NEQ"; break;
				
				case NSWILDCARD:  s = "NSWILDCARD(" + ((System.String) val) + ")"; break;
				
				case NUM:  s = "NUM(" + ((System.Double) val).ToString() + ")"; break;
				
				case OR:  s = "OR"; break;
				
				case PLUS:  s = "PLUS"; break;
				
				case QNAME:  s = "QNAME(" + ((XPathQName) val).ToString() + ")"; break;
				
				case RBRACK:  s = "RBRACK"; break;
				
				case RPAREN:  s = "RPAREN"; break;
				
				case SLASH:  s = "SLASH"; break;
				
				case STR:  s = "STR(" + ((System.String) val) + ")"; break;
				
				case UMINUS:  s = "UMINUS"; break;
				
				case UNION:  s = "UNION"; break;
				
				case VAR:  s = "VAR(" + ((XPathQName) val).ToString() + ")"; break;
				
				case WILDCARD:  s = "WILDCARD"; break;
				}
			
			return s;
		}
	}
}