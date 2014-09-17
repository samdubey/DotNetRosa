/// <summary> </summary>
using System;
namespace org.javarosa.core.util
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class DataUtil
	{
		public DataUtil()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Vector < Integer > u = new Vector < Integer >();
			//Efficiency?
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(Integer i: a)
			{
				if (b.contains(i))
				{
					u.addElement(i);
				}
			}
			return u;
		}
		internal const int offset = 10;
		internal const int low = - 10;
		internal const int high = 400;
		internal static System.Int32[] iarray;
		
		
		public static System.Int32 integer(int ivalue)
		{
			if (iarray == null)
			{
				iarray = new System.Int32[high - low];
				for (int i = 0; i < iarray.Length; ++i)
				{
					iarray[i] = (System.Int32) (i + low);
				}
			}
			return ivalue < high && ivalue >= low?iarray[ivalue + offset]:(System.Int32) ivalue;
		}
		
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public static Vector < Integer > union(Vector < Integer > a, Vector < Integer > b)
	}
}