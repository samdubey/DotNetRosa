using System;
namespace org.javarosa.core.services.storage
{
	
	public interface IStorageFactory
	{
		IStorageUtility newStorage(System.String name, System.Type type);
	}
}