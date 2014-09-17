/// <summary> </summary>
using System;
namespace org.javarosa.core.model.condition.pivot
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	[Serializable]
	public class UnpivotableExpressionException:System.Exception
	{
		
		/// <summary> Default constructor. Should be used for semanticly unpivotable
		/// expressions which are expected
		/// </summary>
		public UnpivotableExpressionException()
		{
		}
		
		/// <summary> Message constructor. Should be used when something unusual happens.</summary>
		/// <param name="message">
		/// </param>
		public UnpivotableExpressionException(System.String message):base(message)
		{
		}
	}
}