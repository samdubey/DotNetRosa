using System;
namespace org.javarosa.core.log
{
	
	[Serializable]
	public class FatalException:WrappedException
	{
		
		public FatalException():this("")
		{
		}
		
		public FatalException(System.String message):base(message)
		{
		}
		
		public FatalException(System.Exception child):base(child)
		{
		}
		
		public FatalException(System.String message, System.Exception child):base(message, child)
		{
		}
	}
}