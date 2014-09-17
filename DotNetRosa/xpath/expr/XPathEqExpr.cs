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
using XPathTypeMismatchException = org.javarosa.xpath.XPathTypeMismatchException;
namespace org.javarosa.xpath.expr
{
	
	public class XPathEqExpr:XPathBinaryOpExpr
	{
		public bool equal;
		
		public XPathEqExpr()
		{
		} //for deserialization
		
		public XPathEqExpr(bool equal, XPathExpression a, XPathExpression b):base(a, b)
		{
			this.equal = equal;
		}
		
		public override System.Object eval(FormInstance model, EvaluationContext evalContext)
		{
			System.Object aval = XPathFuncExpr.unpack(a.eval(model, evalContext));
			System.Object bval = XPathFuncExpr.unpack(b.eval(model, evalContext));
			bool eq = false;
			
			if (aval is System.Boolean || bval is System.Boolean)
			{
				if (!(aval is System.Boolean))
				{
					aval = XPathFuncExpr.toBoolean(aval);
				}
				else if (!(bval is System.Boolean))
				{
					bval = XPathFuncExpr.toBoolean(bval);
				}
				
				bool ba = ((System.Boolean) aval);
				bool bb = ((System.Boolean) bval);
				eq = (ba == bb);
			}
			else if (aval is System.Double || bval is System.Double)
			{
				if (!(aval is System.Double))
				{
					aval = XPathFuncExpr.toNumeric(aval);
				}
				else if (!(bval is System.Double))
				{
					bval = XPathFuncExpr.toNumeric(bval);
				}
				
				double fa = ((System.Double) aval);
				double fb = ((System.Double) bval);
				eq = System.Math.Abs(fa - fb) < 1.0e-12;
			}
			else
			{
				aval = XPathFuncExpr.toString(aval);
				bval = XPathFuncExpr.toString(bval);
				eq = (aval.Equals(bval));
			}
			
			return equal?eq:!eq;
		}
		
		public override System.String ToString()
		{
			return base.toString(equal?"==":"!=");
		}
		
		public  override bool Equals(System.Object o)
		{
			if (o is XPathEqExpr)
			{
				XPathEqExpr x = (XPathEqExpr) o;
				return base.Equals(o) && equal == x.equal;
			}
			else
			{
				return false;
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			equal = ExtUtil.readBool(in_Renamed);
			base.readExternal(in_Renamed, pf);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeBool(out_Renamed, equal);
			base.writeExternal(out_Renamed);
		}
		//UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}