/// <summary> </summary>
using System;
using EvaluationContext = org.javarosa.core.model.condition.EvaluationContext;
using IConditionExpr = org.javarosa.core.model.condition.IConditionExpr;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using XPathFuncExpr = org.javarosa.xpath.expr.XPathFuncExpr;
namespace org.javarosa.core.model.condition.pivot
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public abstract class RangeHint
	{
		public RangeHint()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			
			System.Double min;
			System.Double max;
			
			T minCast;
			T maxCast;
			
			bool minInclusive;
			bool maxInclusive;
		}
		virtual public T Min
		{
			get
			{
				//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
				return min == null?null:minCast;
			}
			
		}
		virtual public bool MinInclusive
		{
			get
			{
				return minInclusive;
			}
			
		}
		virtual public T Max
		{
			get
			{
				//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
				return max == null?null:maxCast;
			}
			
		}
		virtual public bool MaxInclusive
		{
			get
			{
				return maxInclusive;
			}
			
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		< T extends IAnswerData > implements ConstraintHint
		
		public virtual void  init(EvaluationContext c, IConditionExpr conditional, FormInstance instance)
		{
			
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Vector < CmpPivot > internalPivots = new Vector < CmpPivot >();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(Object p: pivots)
			{
				if (!(p is CmpPivot))
				{
					throw new UnpivotableExpressionException();
				}
				internalPivots.addElement((CmpPivot) p);
			}
			
			if (internalPivots.size() > 2)
			{
				//For now.
				throw new UnpivotableExpressionException();
			}
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(CmpPivot pivot: internalPivots)
			{
				evaluatePivot(pivot, conditional, c, instance);
			}
		}
		
		private void  evaluatePivot(CmpPivot pivot, IConditionExpr conditional, EvaluationContext c, FormInstance instance)
		{
			double unit = unit();
			double val = pivot.Val;
			double lt = val - unit;
			double gt = val + unit;
			
			c.isConstraint = true;
			
			c.candidateValue = castToValue(val);
			bool eq = XPathFuncExpr.toBoolean(conditional.eval(instance, c)).booleanValue();
			
			c.candidateValue = castToValue(lt);
			bool ltr = XPathFuncExpr.toBoolean(conditional.eval(instance, c)).booleanValue();
			
			c.candidateValue = castToValue(gt);
			bool gtr = XPathFuncExpr.toBoolean(conditional.eval(instance, c)).booleanValue();
			
			if (ltr && !gtr)
			{
				max = (double) val;
				maxInclusive = eq;
				maxCast = castToValue(max);
			}
			
			if (!ltr && gtr)
			{
				min = (double) val;
				minInclusive = eq;
				minCast = castToValue(min);
			}
		}
		
		protected internal abstract T castToValue(double value_Renamed);
		
		protected internal abstract double unit();
	}
}