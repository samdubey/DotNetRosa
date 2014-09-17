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
using CmpPivot = org.javarosa.core.model.condition.pivot.CmpPivot;
using UnpivotableExpressionException = org.javarosa.core.model.condition.pivot.UnpivotableExpressionException;
using DecimalData = org.javarosa.core.model.data.DecimalData;
using UncastData = org.javarosa.core.model.data.UncastData;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using XPathNodeset = org.javarosa.xpath.XPathNodeset;
namespace org.javarosa.xpath.expr
{
	
	public class XPathCmpExpr:XPathBinaryOpExpr
	{
		private void  InitBlock()
		{
			System.Object aval = a.pivot(model, evalContext, pivots, sentinal);
			System.Object bval = b.pivot(model, evalContext, pivots, sentinal);
			if (bval is XPathNodeset)
			{
				bval = ((XPathNodeset) bval).unpack();
			}
			
			if (handled(aval, bval, sentinal, pivots) || handled(bval, aval, sentinal, pivots))
			{
				return null;
			}
			
			return this.eval(model, evalContext);
			if (sentinal == a)
			{
				if (b == null)
				{
					//Can't pivot on an expression which is derived from pivoted expressions
					throw new UnpivotableExpressionException();
				}
				else if (sentinal == b)
				{
					//WTF?
					throw new UnpivotableExpressionException();
				}
				else
				{
					//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
					System.Double val = null;
					//either of
					if (b is System.Double)
					{
						val = (System.Double) b;
					}
					else
					{
						//These are probably the 
						if (b is System.Int32)
						{
							val = (double) ((System.Int32) b);
						}
						else if (b is System.Int64)
						{
							val = (double) ((System.Int64) b);
						}
						else if (b is System.Single)
						{
							//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.doubleValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
							val = (double) ((System.Single) b);
						}
						else if (b is System.Int16)
						{
							val = (double) ((System.Int16) b);
						}
						else if (b is System.SByte)
						{
							val = (double) ((System.SByte) b);
						}
						else
						{
							if (b is System.String)
							{
								try
								{
									//TODO: Too expensive?
									val = (System.Double) new DecimalData().cast(new UncastData((System.String) b)).Value;
								}
								catch (System.Exception e)
								{
									//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
									throw new UnpivotableExpressionException("Unrecognized numeric data in cmp expression: " + b);
								}
							}
							else
							{
								//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
								throw new UnpivotableExpressionException("Unrecognized numeric data in cmp expression: " + b);
							}
						}
					}
					
					
					pivots.addElement(new CmpPivot(val, op));
					return true;
				}
			}
			return false;
		}
		public const int LT = 0;
		public const int GT = 1;
		public const int LTE = 2;
		public const int GTE = 3;
		
		public int op;
		
		public XPathCmpExpr()
		{
			InitBlock();
		} //for deserialization
		
		public XPathCmpExpr(int op, XPathExpression a, XPathExpression b):base(a, b)
		{
			InitBlock();
			this.op = op;
		}
		
		public override System.Object eval(FormInstance model, EvaluationContext evalContext)
		{
			System.Object aval = a.eval(model, evalContext);
			System.Object bval = b.eval(model, evalContext);
			bool result = false;
			
			//xpath spec says comparisons only defined for numbers (not defined for strings)
			aval = XPathFuncExpr.toNumeric(aval);
			bval = XPathFuncExpr.toNumeric(bval);
			
			double fa = ((System.Double) aval);
			double fb = ((System.Double) bval);
			
			switch (op)
			{
				
				case LT:  result = fa < fb; break;
				
				case GT:  result = fa > fb; break;
				
				case LTE:  result = fa <= fb; break;
				
				case GTE:  result = fa >= fb; break;
				}
			
			return result;
		}
		
		public override System.String ToString()
		{
			System.String sOp = null;
			
			switch (op)
			{
				
				case LT:  sOp = "<"; break;
				
				case GT:  sOp = ">"; break;
				
				case LTE:  sOp = "<="; break;
				
				case GTE:  sOp = ">="; break;
				}
			
			return base.toString(sOp);
		}
		
		public  override bool Equals(System.Object o)
		{
			if (o is XPathCmpExpr)
			{
				XPathCmpExpr x = (XPathCmpExpr) o;
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
		
		
		new public System.Object pivot;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		(FormInstance model, EvaluationContext evalContext, Vector < Object > pivots, Object sentinal) throws UnpivotableExpressionException
		
		private bool handled;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		(Object a, Object b, Object sentinal, Vector < Object > pivots) throws UnpivotableExpressionException
		//UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}