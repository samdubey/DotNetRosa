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
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.xpath.expr
{
	
	public class XPathArithExpr:XPathBinaryOpExpr
	{
		public const int ADD = 0;
		public const int SUBTRACT = 1;
		public const int MULTIPLY = 2;
		public const int DIVIDE = 3;
		public const int MODULO = 4;
		
		public int op;
		
		public XPathArithExpr()
		{
		} //for deserialization
		
		public XPathArithExpr(int op, XPathExpression a, XPathExpression b):base(a, b)
		{
			this.op = op;
		}
		
		public override System.Object eval(FormInstance model, EvaluationContext evalContext)
		{
			double aval = XPathFuncExpr.toNumeric(a.eval(model, evalContext));
			double bval = XPathFuncExpr.toNumeric(b.eval(model, evalContext));
			
			double result = 0;
			switch (op)
			{
				
				case ADD:  result = aval + bval; break;
				
				case SUBTRACT:  result = aval - bval; break;
				
				case MULTIPLY:  result = aval * bval; break;
				
				case DIVIDE:  result = aval / bval; break;
				
				case MODULO:  result = aval % bval; break;
				}
			return (double) result;
		}
		
		public override System.String ToString()
		{
			System.String sOp = null;
			
			switch (op)
			{
				
				case ADD:  sOp = "+"; break;
				
				case SUBTRACT:  sOp = "-"; break;
				
				case MULTIPLY:  sOp = "*"; break;
				
				case DIVIDE:  sOp = "/"; break;
				
				case MODULO:  sOp = "%"; break;
				}
			
			return base.toString(sOp);
		}
		
		public  override bool Equals(System.Object o)
		{
			if (o is XPathArithExpr)
			{
				XPathArithExpr x = (XPathArithExpr) o;
				return base.Equals(o) && op == x.op;
			}
			else
			{
				return false;
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			op = ExtUtil.readInt(in_Renamed);
			base.readExternal(in_Renamed, pf);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeNumeric(out_Renamed, op);
			base.writeExternal(out_Renamed);
		}
		//UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}