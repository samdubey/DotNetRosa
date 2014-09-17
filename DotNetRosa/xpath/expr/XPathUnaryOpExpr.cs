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
using UnpivotableExpressionException = org.javarosa.core.model.condition.pivot.UnpivotableExpressionException;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapTagged = org.javarosa.core.util.externalizable.ExtWrapTagged;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
namespace org.javarosa.xpath.expr
{
	
	public abstract class XPathUnaryOpExpr:XPathOpExpr
	{
		public XPathExpression a;
		
		public XPathUnaryOpExpr()
		{
		} //for deserialization of children
		
		public XPathUnaryOpExpr(XPathExpression a)
		{
			this.a = a;
		}
		
		public  override bool Equals(System.Object o)
		{
			if (o is XPathUnaryOpExpr)
			{
				XPathUnaryOpExpr x = (XPathUnaryOpExpr) o;
				return a.Equals(x.a);
			}
			else
			{
				return false;
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			a = (XPathExpression) ExtUtil.read(in_Renamed, new ExtWrapTagged(), pf);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.write(out_Renamed, new ExtWrapTagged(a));
		}
		//UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}