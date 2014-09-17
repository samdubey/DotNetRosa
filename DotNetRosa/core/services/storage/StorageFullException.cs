using System;
namespace org.javarosa.core.services.storage
{
	
	/// <summary> An exception thrown by a StorageUtility when the requested action cannot be completed because there is not enough
	/// space in the underlying device storage.
	/// </summary>
	[Serializable]
	public class StorageFullException:System.SystemException
	{
	}
}