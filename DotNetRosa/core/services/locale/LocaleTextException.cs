/// <summary> </summary>
using System;
namespace org.javarosa.core.services.locale
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	[Serializable]
	public class LocaleTextException:System.SystemException
	{
		public LocaleTextException(System.String message):base(message)
		{
		}
	}
}