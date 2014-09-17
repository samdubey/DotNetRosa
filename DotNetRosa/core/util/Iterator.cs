/// <summary> </summary>
using System;
namespace org.javarosa.core.util
{
	
	/// <summary> Interface for iterating through a set of records from an IStorageUtility</summary>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	interface Iterator < E >
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{
	
	/// <summary> NOTE: if the underlying IStorageUtility is modified while this iterator is being iterated through,
	/// any calls to nextID() or nextRecord() after the modification will throw a StorageModifiedException
	/// (this is a RuntimeException). To prevent this from happening, you can lock the entire StorageUtility
	/// *before* calling iterate(), and release the lock only after you have iterated through all records.
	/// However, this will prevent all other threads from access the StorageUtility for the entire duration
	/// of the iteration. Also, it will not protect against you modifying the storage in the same thread you
	/// are doing the iteration in.
	/// 
	/// Also, if you call nextID(), then call StorageUtility.read() yourself, there is a very slight risk that
	/// another thread will change or invalidate the record with that ID in between your call to nextID() and
	/// read(). To prevent against that, you can lock the StorageUtility before calling nextID and release it
	/// after calling read(). This risk does not exist when calling nextRecord().
	/// </summary>
	
	/// <summary> Number of records in the set
	/// 
	/// </summary>
	/// <returns> number of records
	/// </returns>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	int numRecords();
	
	/// <summary> Return the ID of the next record in the set without advancing the iterator.
	/// 
	/// </summary>
	/// <returns> ID of next record
	/// </returns>
	/// <throws>  StorageModifiedException if the underlying StorageUtility has been modified since this iterator </throws>
	/// <summary> was created
	/// </summary>
	/// <throws>  IllegalStateException if all records have already been iterated through </throws>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	int peekID();
	
	/// <summary> Return the ID of the next record in the set. Advance the iteration cursor by one.
	/// 
	/// </summary>
	/// <returns> ID of next record
	/// </returns>
	/// <throws>  StorageModifiedException if the underlying StorageUtility has been modified since this iterator </throws>
	/// <summary> was created
	/// </summary>
	/// <throws>  IllegalStateException if all records have already been iterated through </throws>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	int nextID();
	
	/// <summary> Return the next record in the set. Advance the iteration cursor by one.
	/// 
	/// </summary>
	/// <returns> object representation of next record
	/// </returns>
	/// <throws>  IllegalStateException if all records have already been iterated through </throws>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	E nextRecord();
	
	/// <summary> Return whether the set has more records to iterate through
	/// 
	/// </summary>
	/// <returns> true if there are more records to iterate though.
	/// </returns>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	boolean hasMore();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
}