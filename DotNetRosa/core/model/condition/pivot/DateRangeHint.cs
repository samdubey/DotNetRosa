/// <summary> </summary>
using System;
using DateData = org.javarosa.core.model.data.DateData;
using XPathFuncExpr = org.javarosa.xpath.expr.XPathFuncExpr;
namespace org.javarosa.core.model.condition.pivot
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class DateRangeHint:RangeHint
	{
		
		< DateData >
		
		protected internal override DateData castToValue(double value_Renamed)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return new DateData(ref new System.DateTime[]{(System.DateTime) XPathFuncExpr.toDate((System.Object) System.Math.Floor(value_Renamed), false)}[0]);
		}
		
		protected internal override double unit()
		{
			return 1;
		}
	}
}