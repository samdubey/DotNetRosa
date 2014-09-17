/// <summary> </summary>
using System;
namespace org.javarosa.core.util
{
	
	/// <summary> Static utility functions for mathematical operations
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public class MathUtils
	{
		public static System.Random Rand
		{
			get
			{
				if (r == null)
				{
					r = new System.Random();
				}
				return r;
			}
			
		}
		private static System.Random r;
		
		//a - b * floor(a / b)
		public static long modLongNotSuck(long a, long b)
		{
			return ((a % b) + b) % b;
		}
		
		public static long divLongNotSuck(long a, long b)
		{
			return (a - modLongNotSuck(a, b)) / b;
		}
	}
}