/// <summary> </summary>
using System;
using EntityFilter = org.javarosa.core.services.storage.EntityFilter;
using IMetaData = org.javarosa.core.services.storage.IMetaData;
//UPGRADE_TODO: The type 'org.javarosa.core.services.storage.IStorageIterator' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using IStorageIterator = org.javarosa.core.services.storage.IStorageIterator;
//UPGRADE_TODO: The type 'org.javarosa.core.services.storage.IStorageUtilityIndexed' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using IStorageUtilityIndexed = org.javarosa.core.services.storage.IStorageUtilityIndexed;
using Persistable = org.javarosa.core.services.storage.Persistable;
using StorageFullException = org.javarosa.core.services.storage.StorageFullException;
using DataUtil = org.javarosa.core.util.DataUtil;
using InvalidIndexException = org.javarosa.core.util.InvalidIndexException;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
namespace org.javarosa.core.services.storage.util
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class DummyIndexedStorageUtility
	{
		private void  InitBlock()
		{
			//We should really find a way to invalidate old iterators first here
			return ;
			
			new DummyStorageIterator < T >(data);
			
			List< Integer > removed = new List< Integer >();
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator en = data.keys(); en.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				System.Int32 i = (System.Int32) en.Current;
				switch (ef.preFilter(i, null))
				{
					
					case EntityFilter.PREFILTER_INCLUDE: 
						removed.addElement(i);
						break;
					
					case EntityFilter.PREFILTER_EXCLUDE: 
						continue;
					}
				if (ef.matches(data.get_Renamed(i)))
				{
					removed.addElement(i);
				}
			}
			
			for(Integer i: removed)
			{
				data.remove(i);
			}
			
			syncMeta();
			
			return removed;
		}
		virtual public System.Object AccessLock
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.services.storage.IStorageUtility#getAccessLock()
			*/
			
			get
			{
				// TODO Auto-generated method stub
				return null;
			}
			
		}
		virtual public int NumRecords
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.services.storage.IStorageUtility#getNumRecords()
			*/
			
			get
			{
				return data.size();
			}
			
		}
		virtual public int TotalSize
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.services.storage.IStorageUtility#getTotalSize()
			*/
			
			get
			{
				//serialize and test blah blah.
				return 0;
			}
			
		}
		virtual public bool Empty
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.services.storage.IStorageUtility#isEmpty()
			*/
			
			get
			{
				if (data.size() > 0)
				{
					return true;
				}
				return false;
			}
			
		}
		
		< T extends Persistable > implements IStorageUtilityIndexed < T >
		
		
		private Hashtable < String, Hashtable < Object, List< Integer >>> meta;
		
		
		private Hashtable < Integer, T > data;
		
		internal int curCount;
		
		public DummyIndexedStorageUtility()
		{
			InitBlock();
			
			meta = new Hashtable < String, Hashtable < Object, List< Integer >>>();
			
			data = new Hashtable < Integer, T >();
			curCount = 0;
		}
		
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtilityIndexed#getIDsForValue(java.lang.String, java.lang.Object)
		*/
		public virtual System.Collections.ArrayList getIDsForValue(System.String fieldName, System.Object value_Renamed)
		{
			if (meta.get_Renamed(fieldName) == null || meta.get_Renamed(fieldName).get_Renamed(value_Renamed) == null)
			{
				
				
				new List< Integer >();
			}
			return meta.get_Renamed(fieldName).get_Renamed(value_Renamed);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtilityIndexed#getRecordForValue(java.lang.String, java.lang.Object)
		*/
		public virtual T getRecordForValue(System.String fieldName, System.Object value_Renamed)
		{
			
			if (meta.get_Renamed(fieldName) == null)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new System.ArgumentOutOfRangeException("No record matching meta index " + fieldName + " with value " + value_Renamed);
			}
			
			
			
			if (matches == null || matches.size() == 0)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new System.ArgumentOutOfRangeException("No record matching meta index " + fieldName + " with value " + value_Renamed);
			}
			if (matches.size() > 1)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new InvalidIndexException("Multiple records matching meta index " + fieldName + " with value " + value_Renamed, fieldName);
			}
			
			return data.get_Renamed(matches.elementAt(0));
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtility#add(org.javarosa.core.util.externalizable.Externalizable)
		*/
		public virtual int add(T e)
		{
			data.put(DataUtil.integer(curCount), e);
			
			//This is not a legit pair of operations;
			curCount++;
			
			syncMeta();
			
			return curCount - 1;
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtility#close()
		*/
		public virtual void  close()
		{
			// TODO Auto-generated method stub
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtility#destroy()
		*/
		public virtual void  destroy()
		{
			// TODO Auto-generated method stub
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtility#exists(int)
		*/
		public virtual bool exists(int id)
		{
			if (data.containsKey(DataUtil.integer(id)))
			{
				return true;
			}
			return false;
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtility#getRecordSize(int)
		*/
		public virtual int getRecordSize(int id)
		{
			//serialize and test blah blah.
			return 0;
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtility#iterate()
		*/
		
		public IStorageIterator < T > iterate()
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtility#read(int)
		*/
		public virtual T read(int id)
		{
			return data.get_Renamed(DataUtil.integer(id));
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtility#readBytes(int)
		*/
		public virtual sbyte[] readBytes(int id)
		{
			System.IO.MemoryStream stream = new System.IO.MemoryStream();
			try
			{
				//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
				data.get_Renamed(DataUtil.integer(id)).writeExternal(new System.IO.BinaryWriter(stream));
				return SupportClass.ToSByteArray(stream.ToArray());
			}
			catch (System.IO.IOException e)
			{
				throw new System.SystemException("Couldn't serialize data to return to readBytes");
			}
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtility#remove(int)
		*/
		public virtual void  remove(int id)
		{
			data.remove(DataUtil.integer(id));
			
			syncMeta();
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtility#remove(org.javarosa.core.services.storage.Persistable)
		*/
		public virtual void  remove(Persistable p)
		{
			this.read(p.ID);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtility#removeAll()
		*/
		public virtual void  removeAll()
		{
			data.clear();
			
			meta.clear();
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtility#removeAll(org.javarosa.core.services.storage.EntityFilter)
		*/
		
		public List< Integer > removeAll(EntityFilter ef)
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtility#repack()
		*/
		public virtual void  repack()
		{
			//Unecessary!
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtility#repair()
		*/
		public virtual void  repair()
		{
			//Unecessary!
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtility#update(int, org.javarosa.core.util.externalizable.Externalizable)
		*/
		public virtual void  update(int id, T e)
		{
			data.put(DataUtil.integer(id), e);
			syncMeta();
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IStorageUtility#write(org.javarosa.core.services.storage.Persistable)
		*/
		public virtual void  write(Persistable p)
		{
			if (p.ID != - 1)
			{
				this.data.put(DataUtil.integer(p.ID), (T) p);
				syncMeta();
			}
			else
			{
				p.ID = curCount;
				this.add((T) p);
			}
		}
		
		private void  syncMeta()
		{
			meta.clear();
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator en = data.keys(); en.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				System.Int32 i = (System.Int32) en.Current;
				Externalizable e = (Externalizable) data.get_Renamed(i);
				
				if (e is IMetaData)
				{
					
					IMetaData m = (IMetaData) e;
					
					for(String key: m.getMetaDataFields())
					{
						if (!meta.containsKey(key))
						{
							
							meta.put(key, new Hashtable < Object, List< Integer >>());
						}
					}
					
					for(String key: dynamicIndices)
					{
						if (!meta.containsKey(key))
						{
							
							meta.put(key, new Hashtable < Object, List< Integer >>());
						}
					}
					//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
					for (System.Collections.IEnumerator keys = meta.keys(); keys.MoveNext(); )
					{
						//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
						System.String key = (System.String) keys.Current;
						
						System.Object value_Renamed = m.getMetaData(key);
						
						
						Hashtable < Object, List< Integer >> records = meta.get(key);
						
						if (!records.containsKey(value_Renamed))
						{
							
							records.put(value, new List< Integer >());
						}
						
						if (!indices.contains(i))
						{
							records.get_Renamed(value_Renamed).addElement(i);
						}
					}
				}
			}
		}
		
		
		public virtual void  setReadOnly()
		{
			//TODO: This should have a clear contract.
		}
		
		
		
		List< String > dynamicIndices = new List< String >();
		public virtual void  registerIndex(System.String filterIndex)
		{
			dynamicIndices.addElement(filterIndex);
		}
	}
}