using System;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
namespace org.javarosa.core.services.storage
{
	
	/// <summary> IStorageUtility
	/// 
	/// Implementations of this interface provide persistent records-based storage in which records are stored
	/// and retrieved using record IDs.
	/// 
	/// IStorageUtility can be used in two flavors: you manage the IDs, or the utility manages the IDs:
	/// 
	/// If you manage the IDs, the objects you are storing must implement Persistable, which provides the ID from the object
	/// itself. You then use the functions read(), write(), and remove() when dealing with storage.
	/// 
	/// If the utility manages the IDs, your objects need only implement Externalizable. You use the functions read(), add(),
	/// update(), and remove(). add() will return a new ID for the record, which you then explicitly provide to all subsequent
	/// calls to update().
	/// 
	/// These two schemes should not be mixed within the same StorageUtility.
	/// 
	/// </summary>
	
	public
	
	interface IStorageUtility < E extends Externalizable >
	
	{
	
	/// <summary> Read and return the record corresponding to 'id'.
	/// 
	/// </summary>
	/// <param name="id">id of the object
	/// </param>
	/// <returns> object for 'id'. null if no object is stored under that ID
	/// </returns>
	
	E read(int id);
	
	/// <summary> Read and return the raw bytes for the record corresponding to 'id'.
	/// 
	/// </summary>
	/// <param name="id">id of the object
	/// </param>
	/// <returns> raw bytes for the record. null if no record is stored under that ID
	/// </returns>
	
	byte [] readBytes(int id);
	
	/// <summary> Write an object to the store. Will either add a new record, or update the existing record (if one exists) for the
	/// object's ID. This function should never be used in conjunction with add() and update() within the same StorageUtility
	/// 
	/// </summary>
	/// <param name="p">object to store
	/// </param>
	/// <throws>  StorageFullException if there is not enough room to store the object </throws>
	
	void write(Persistable p) throws StorageFullException;
	
	/// <summary> Add a new record to the store. This function always adds a new record; it never updates an existing record. The
	/// record ID under which this record is added is allocated by the StorageUtility. If this StorageUtility stores
	/// Persistables, you should almost certainly use write() instead.
	/// 
	/// </summary>
	/// <param name="e">object to add
	/// </param>
	/// <returns> record ID for newly added object
	/// </returns>
	/// <throws>  StorageFullException if not enough space available </throws>
	
	int add(E e) throws StorageFullException;
	
	/// <summary> Update a record in the store. The record must have previously been added to the store using add(). If this
	/// StorageUtility stores Persistables, you should almost certainly use write() instead.
	/// 
	/// </summary>
	/// <param name="id">ID of record to update
	/// </param>
	/// <param name="e">updated object
	/// </param>
	/// <throws>  StorageFullException if not enough space available to update </throws>
	/// <throws>  IllegalArgumentException if no record exists for ID </throws>
	
	void update(int id, E e) throws StorageFullException;
	
	/// <summary> Remove record with the given ID from the store.
	/// 
	/// </summary>
	/// <param name="id">ID of record to remove
	/// </param>
	/// <throws>  IllegalArgumentException if no record with that ID exists </throws>
	
	void remove(int id);
	
	/// <summary> Remove object from the store
	/// 
	/// </summary>
	/// <param name="p">object to remove
	/// </param>
	/// <throws>  IllegalArgumentException if object is not in the store </throws>
	
	void remove(Persistable p);
	
	
	void removeAll();
	
	
	List< Integer > removeAll(EntityFilter ef);
	
	/// <summary> Return the number of records in the store
	/// 
	/// </summary>
	/// <returns> number of records
	/// </returns>
	
	int getNumRecords();
	
	/// <summary> Return whether the store is empty
	/// 
	/// </summary>
	/// <returns> true if there are no records in the store
	/// </returns>
	
	boolean isEmpty();
	
	/// <summary> Return whether a record exists in the store
	/// 
	/// </summary>
	/// <param name="id">record ID
	/// </param>
	/// <returns> true if a record exists for that ID in the store
	/// </returns>
	
	boolean exists(int id);
	
	/// <summary> Return total size of device storage consumed by this StorageUtility
	/// 
	/// </summary>
	/// <returns> total size (bytes)
	/// </returns>
	
	int getTotalSize();
	
	/// <summary> Get the size of a record
	/// 
	/// </summary>
	/// <param name="id">record ID
	/// </param>
	/// <returns> size of that record, in bytes
	/// </returns>
	/// <throws>  IllegalArgumentException if no record exists for that ID </throws>
	
	int getRecordSize(int id);
	
	/// <summary> Return an iterator to iterate through all records in this store
	/// 
	/// </summary>
	/// <returns> record iterator
	/// </returns>
	
	IStorageIterator < E > iterate();
	
	/// <summary> Close all resources associated with this StorageUtility. Any attempt to use this StorageUtility after this call will result
	/// in error. Though not strictly necessary, it is a good idea to call this when you are done with the StorageUtility, as closing
	/// may trigger clean-up in the underlying device storage (reclaiming unused space, etc.).
	/// </summary>
	
	void close();
	
	/// <summary> Delete the storage utility itself, along with all stored records and meta-data</summary>
	
	void destroy();
	
	/// <summary> Perform any clean-up/consolidation of the StorageUtility's underlying datastructures that is too expensive to do during
	/// normal usage (e.g., if all the records are scattered among 10 half-empty RMSes, repack them into 5 full RMSes)
	/// </summary>
	
	void repack();
	
	/// <summary> If the StorageUtility has been left in a corrupt/inconsistent state, restore it to a non-corrupt state, even if it results
	/// in data loss. If the integrity is intact, do nothing
	/// </summary>
	
	void repair();
	
	/// <summary> Fetch the object that acts as the synchronization lock for this StorageUtility
	/// 
	/// </summary>
	/// <returns> lock object
	/// </returns>
	
	Object getAccessLock();
	
	
	void setReadOnly();
	
	}
}