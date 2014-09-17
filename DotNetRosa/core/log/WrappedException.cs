using System;
namespace org.javarosa.core.log
{
	
	[Serializable]
	public class WrappedException:System.SystemException
	{
		
		internal System.String message;
		internal System.Exception child;
		
		public WrappedException(System.String message):this(message, null)
		{
		}
		
		public WrappedException(System.Exception child):this(null, child)
		{
		}
		
		public WrappedException(System.String message, System.Exception child):base(constructMessage(message, child))
		{
			this.message = message;
			this.child = child;
		}
		
		public static System.String constructMessage(System.String message, System.Exception child)
		{
			System.String str = "";
			if (message != null)
			{
				str += message;
			}
			if (child != null)
			{
				str += ((message != null?" => ":"") + printException(child));
			}
			
			if (str.Equals(""))
				str = "[exception]";
			return str;
		}
		
		public static System.String printException(System.Exception e)
		{
			if (e is WrappedException)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				return (e is FatalException?"FATAL: ":"") + e.Message;
			}
			else
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				return e.GetType().FullName + "[" + e.Message + "]";
			}
		}
	}
}