/// <summary> </summary>
using System;
using DecimalData = org.javarosa.core.model.data.DecimalData;
namespace org.javarosa.core.model.condition.pivot
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class DecimalRangeHint:RangeHint
	{
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		< DecimalData >
		
		protected internal override DecimalData castToValue(double value_Renamed)
		{
			return new DecimalData(value_Renamed);
		}
		
		protected internal override double unit()
		{
			//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			return System.Double.MinValue;
		}
	}
}