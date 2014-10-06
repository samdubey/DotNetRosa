using System;
//UPGRADE_TODO: The type 'org.javarosa.core.util.Iterator' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Iterator = org.javarosa.core.util.Iterator;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
namespace org.javarosa.core.services.storage
{
	
	/// <summary> Interface for iterating through a set of records from an IStorageUtility</summary>
	
	public
	
	interface IStorageIterator < E extends Externalizable > extends Iterator < E >
	
	{
	
	
	}
}