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
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapListPoly = org.javarosa.core.util.externalizable.ExtWrapListPoly;
using ExtWrapTagged = org.javarosa.core.util.externalizable.ExtWrapTagged;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using XPathUnsupportedException = org.javarosa.xpath.XPathUnsupportedException;
namespace org.javarosa.xpath.expr
{
	
	public class XPathFilterExpr:XPathExpression
	{
		private void  InitBlock()
		{
			throw new UnpivotableExpressionException();
		}
		public XPathExpression x;
		public XPathExpression[] predicates;
		
		public XPathFilterExpr()
		{
			InitBlock();
		} //for deserialization
		
		public XPathFilterExpr(XPathExpression x, XPathExpression[] predicates)
		{
			InitBlock();
			this.x = x;
			this.predicates = predicates;
		}
		
		public override System.Object eval(FormInstance model, EvaluationContext evalContext)
		{
			throw new XPathUnsupportedException("filter expression");
		}
		
		public override System.String ToString()
		{
			StringBuilder sb = new StringBuilder();
			
			sb.append("{filt-expr:");
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			sb.append(x.ToString());
			sb.append(",{");
			for (int i = 0; i < predicates.Length; i++)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				sb.append(predicates[i].ToString());
				if (i < predicates.Length - 1)
					sb.append(",");
			}
			sb.append("}}");
			
			return sb.toString();
		}
		
		public  override bool Equals(System.Object o)
		{
			if (o is XPathFilterExpr)
			{
				XPathFilterExpr fe = (XPathFilterExpr) o;
				
				System.Collections.ArrayList a = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				for (int i = 0; i < predicates.Length; i++)
					a.Add(predicates[i]);
				System.Collections.ArrayList b = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				for (int i = 0; i < fe.predicates.Length; i++)
					b.Add(fe.predicates[i]);
				
				return x.Equals(fe.x) && ExtUtil.vectorEquals(a, b);
			}
			else
			{
				return false;
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			x = (XPathExpression) ExtUtil.read(in_Renamed, new ExtWrapTagged(), pf);
			System.Collections.ArrayList v = (System.Collections.ArrayList) ExtUtil.read(in_Renamed, new ExtWrapListPoly(), pf);
			
			predicates = new XPathExpression[v.Count];
			for (int i = 0; i < predicates.Length; i++)
				predicates[i] = (XPathExpression) v[i];
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			for (int i = 0; i < predicates.Length; i++)
				v.Add(predicates[i]);
			
			ExtUtil.write(out_Renamed, new ExtWrapTagged(x));
			ExtUtil.write(out_Renamed, new ExtWrapListPoly(v));
		}
		
		new public System.Object pivot;
		
		(FormInstance model, EvaluationContext evalContext, List< Object > pivots, Object sentinal) throws UnpivotableExpressionException
		//UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}