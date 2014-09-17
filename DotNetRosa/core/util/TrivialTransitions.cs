using System;
namespace org.javarosa.core.util
{
	
	/// <summary> Convenience interface for states that do not have any transitions. </summary>
	public interface TrivialTransitions
	{
		
		void  done();
	}
}