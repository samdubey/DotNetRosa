using System;
using InvalidIndexException = org.javarosa.core.util.InvalidIndexException;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
namespace org.javarosa.core.services.storage
{
	
	/* TEMPORARY / EXPERIMENTAL */
	
	
	public
	
	interface IStorageUtilityIndexed < E extends Externalizable > extends IStorageUtility < E >
	
	{
	
	/// <summary> Retrieves a Vector of IDs of Externalizable objects in storage for which the field
	/// specified contains the value specified.
	/// 
	/// </summary>
	/// <param name="fieldName">The name of a field which should be evaluated
	/// </param>
	/// <param name="value">The value which should be contained by the field specified
	/// </param>
	/// <returns> A Vector of Integers such that retrieving the Externalizable object with any
	/// of those integer IDs will result in an object for which the field specified is equal
	/// to the value provided.
	/// </returns>
	/// <throws>  RuntimeException (Fix this exception type) if the field is unrecognized by the </throws>
	/// <summary> meta data
	/// </summary>
	
	Vector getIDsForValue(String fieldName, Object value);
	/// <summary> 
	/// Retrieves a Externalizable object from the storage which is reference by the unique index fieldName.
	/// 
	/// </summary>
	/// <param name="fieldName">The name of the index field which will be evaluated
	/// </param>
	/// <param name="value">The value which should be set in the index specified by fieldName for the returned
	/// object.
	/// </param>
	/// <returns> An Externalizable object e, such that e.getMetaData(fieldName).equals(value);
	/// </returns>
	/// <throws>  NoSuchElementException If no objects reside in storage for which the return condition </throws>
	/// <summary> can be successful.
	/// </summary>
	/// <throws>  InvalidIndexException If the field used is an invalid index, because more than one field in the Storage </throws>
	/// <summary> contains the value of the index requested.
	/// </summary>
	
	E getRecordForValue(String fieldName, Object value) throws NoSuchElementException, InvalidIndexException;
	
	/// <summary> Optional. Register a new index for this storage which may optionally be able for indexed operations
	/// going forward. This will likely take a substantial amount of time for larger storage utilities.
	/// 
	/// </summary>
	/// <param name="filterIndex">
	/// </param>
	
	void registerIndex(String filterIndex);
	
	
	}
}