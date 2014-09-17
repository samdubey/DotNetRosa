/// <summary> </summary>
using System;
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
					return (State) SupportClass.StackSupport.Pop(statesToReturnTo);
				}
				catch (System.ArgumentOutOfRangeException e)
				{
					throw new System.SystemException("Tried to return to a saved state, but no state to return to had been set earlier in the workflow");
				}
			}
			
			set
			{
				statesToReturnTo.Add(value);
			}
			
		}
		private static System.Collections.ArrayList statesToReturnTo = new System.Collections.ArrayList();
	}
}