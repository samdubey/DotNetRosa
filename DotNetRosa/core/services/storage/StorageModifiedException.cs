using System;
namespace org.javarosa.core.services.storage
{
	
	[Serializable]
	public class StorageModifiedException:System.SystemException
	{
		public StorageModifiedException():base()
		{
		}
		
		public StorageModifiedException(System.String message):base(message)
		{
		}
	}
}