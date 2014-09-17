/// <summary> </summary>
using System;
using TrivialTransitions = org.javarosa.core.util.TrivialTransitions;
namespace org.javarosa.core.api
{
	
	/// <summary> A state represents a particular state of the application. Each state has an
	/// associated view and controller and a set of transitions to new states.
	/// 
	/// </summary>
	/// <seealso cref="TrivialTransitions">
	/// </seealso>
	/// <author>  ctsims
	/// 
	/// </author>
	public interface State
	{
		
		void  start();
	}
}