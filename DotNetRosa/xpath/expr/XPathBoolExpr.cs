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
	
	public class XPathBoolExpr:XPathBinaryOpExpr
	{
		public const int AND = 0;
		public const int OR = 1;
		
		public int op;
		
		public XPathBoolExpr()
		{
		} //for deserialization
		
		public XPathBoolExpr(int op, XPathExpression a, XPathExpression b):base(a, b)
		{
			this.op = op;
		}
		
		public override System.Object eval(FormInstance model, EvaluationContext evalContext)
		{
			bool aval = XPathFuncExpr.toBoolean(a.eval(model, evalContext));
			
			//short-circuiting
			if ((!aval && op == AND) || (aval && op == OR))
			{
				return aval;
			}
			
			bool bval = XPathFuncExpr.toBoolean(b.eval(model, evalContext));
			
			bool result = false;
			switch (op)
			{
				
				case AND:  result = aval && bval; break;
				
				case OR:  result = aval || bval; break;
				}
			return result;
		}
		
		public override System.String ToString()
		{
			System.String sOp = null;
			
			switch (op)
			{
				
				case AND:  sOp = "and"; break;
				
				case OR:  sOp = "or"; break;
				}
			
			return base.toString(sOp);
		}
		
		public  override bool Equals(System.Object o)
		{
			if (o is XPathBoolExpr)
			{
				XPathBoolExpr x = (XPathBoolExpr) o;
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