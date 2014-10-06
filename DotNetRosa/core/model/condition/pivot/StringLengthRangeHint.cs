/// <summary> </summary>
using System;
using StringData = org.javarosa.core.model.data.StringData;
namespace org.javarosa.core.model.condition.pivot
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class StringLengthRangeHint:RangeHint
	{
		
		< StringData >
		
		protected internal override StringData castToValue(double value_Renamed)
		{
			StringBuilder sb = new StringBuilder();
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			for (int i = 0; i < ((int) value_Renamed); ++i)
			{
				sb.append("X");
			}
			return new StringData(sb.toString());
		}
		
		protected internal override double unit()
		{
			return 1;
		}
	}
}