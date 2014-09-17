/// <summary> </summary>
using System;
namespace org.javarosa.core.model.condition.pivot
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class CmpPivot : Pivot
	{
		virtual public bool Outcome
		{
			get
			{
				return outcome;
			}
			
			set
			{
				this.outcome = value;
			}
			
		}
		virtual public double Val
		{
			get
			{
				return val;
			}
			
		}
		virtual public int Op
		{
			get
			{
				return op;
			}
			
		}
		private double val;
		private int op;
		private bool outcome;
		
		public CmpPivot(double val, int op)
		{
			this.val = val;
			this.op = op;
		}
	}
}