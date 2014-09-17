/// <summary> </summary>
using System;
namespace org.javarosa.core.util
{
	
	/// <summary> Thrown when an index used contains an invalid value
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	[Serializable]
	public class InvalidIndexException:System.SystemException
	{
		virtual public System.String Index
		{
			get
			{
				return index;
			}
			
		}
		internal System.String index;
		public InvalidIndexException(System.String message, System.String index):base(message)
		{
			this.index = index;
		}
	}
}