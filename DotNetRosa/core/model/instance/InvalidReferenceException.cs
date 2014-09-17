/// <summary> </summary>
using System;
namespace org.javarosa.core.model.instance
{
	
	/// <summary> An Invalid Reference exception is thrown whenever
	/// a valid TreeReference is expected by an operation.
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	[Serializable]
	public class InvalidReferenceException:System.Exception
	{
		virtual public TreeReference InvalidReference
		{
			get
			{
				return invalid;
			}
			
		}
		internal TreeReference invalid;
		
		public InvalidReferenceException(System.String message, TreeReference reference):base(message)
		{
			this.invalid = reference;
		}
	}
}