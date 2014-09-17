/// <summary> </summary>
using System;
//UPGRADE_TODO: The type 'org.javarosa.core.services.storage.IStorageIterator' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using IStorageIterator = org.javarosa.core.services.storage.IStorageIterator;
using Persistable = org.javarosa.core.services.storage.Persistable;
using DataUtil = org.javarosa.core.util.DataUtil;
namespace org.javarosa.core.services.storage.util
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class DummyStorageIterator
	{
		public DummyStorageIterator()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Hashtable < Integer, T > data;
			int count;
			System.Int32[] keys;
			this.data = data;
			keys = new System.Int32[data.size()];
			int i = 0;
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator en = data.keys(); en.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				keys[i] = (System.Int32) en.Current;
				++i;
			}
			count = 0;
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		< T extends Persistable > implements IStorageIterator < T >
		
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public DummyStorageIterator(Hashtable < Integer, T > data)
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageIterator#hasMore()
		*/
		public virtual bool hasMore()
		{
			return count < keys.Length;
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageIterator#nextID()
		*/
		public virtual int nextID()
		{
			count++;
			return ((System.Int32) keys[count - 1]);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageIterator#nextRecord()
		*/
		public virtual T nextRecord()
		{
			return data.get_Renamed(DataUtil.integer(nextID()));
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageIterator#numRecords()
		*/
		public virtual int numRecords()
		{
			return data.size();
		}
		
		public virtual int peekID()
		{
			return (System.Int32) keys[count];
		}
	}
}