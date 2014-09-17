/// <summary> </summary>
using System;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using UncastData = org.javarosa.core.model.data.UncastData;
namespace org.javarosa.core.model.data.helper
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	[Serializable]
	public class InvalidDataException:System.Exception
	{
		virtual public IAnswerData UncastStandin
		{
			get
			{
				return standin;
			}
			
		}
		internal UncastData standin;
		
		public InvalidDataException(System.String message, UncastData standin):base(message)
		{
			this.standin = standin;
		}
	}
}