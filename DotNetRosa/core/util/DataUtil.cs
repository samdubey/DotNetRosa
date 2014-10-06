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
			
			List< Integer > u = new List< Integer >();
			//Efficiency?
			
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
		
		
		
		public static List< Integer > union(List< Integer > a, List< Integer > b)
	}
}