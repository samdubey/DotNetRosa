/// <summary> </summary>
using System;
using IntegerData = org.javarosa.core.model.data.IntegerData;
namespace org.javarosa.core.model.condition.pivot
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class IntegerRangeHint:RangeHint
	{
		
		< IntegerData >
		
		protected internal override IntegerData castToValue(double value_Renamed)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return new IntegerData((int) System.Math.Floor(value_Renamed));
		}
		
		protected internal override double unit()
		{
			return 1;
		}
	}
}