/// <summary> </summary>
using System;
using System.Collections;
namespace org.javarosa.core.api
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class StateMachine
	{
		public static State StateToReturnTo
		{
			get
			{
				try
				{
					return (State) statesToReturnTo.Pop();
				}
				catch (System.ArgumentOutOfRangeException e)
				{
					throw new System.SystemException("Tried to return to a saved state, but no state to return to had been set earlier in the workflow");
				}
			}
			
			set
			{
				statesToReturnTo.Push(value);
			}
			
		}
        private static Stack statesToReturnTo = new Stack();
	}
}