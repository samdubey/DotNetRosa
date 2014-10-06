/// <summary> </summary>
using System;
namespace org.javarosa.core.util
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class CacheTable
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassRunnable' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class AnonymousClassRunnable : IThreadRunnable
		{
			public AnonymousClassRunnable(CacheTable enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(CacheTable enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private CacheTable enclosingInstance;
			public CacheTable Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  Run()
			{
				
				List< Integer > toRemove = new List< Integer >();
				while (true)
				{
					try
					{
						toRemove.removeAllElements();
						for (int i = 0; i < caches.size(); ++i)
						{
							CacheTable cache = (CacheTable) caches.elementAt(i).get_Renamed();
							if (cache == null)
							{
								toRemove.addElement(DataUtil.integer(i));
							}
							else
							{
								
								Hashtable < Integer, WeakReference > table = cache.currentTable;
								//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
								for (System.Collections.IEnumerator en = table.keys(); en.MoveNext(); )
								{
									//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
									System.Object key = en.Current;
									
									lock (cache)
									{
										//See whether or not the cached reference has been cleared by the GC
										//UPGRADE_ISSUE: Method 'java.lang.ref.Reference.get' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangrefReference'"
										if (((System.WeakReference) table.get_Renamed(key)).get_Renamed() == null)
										{
											//If so, remove the entry, it's no longer useful.
											table.remove(key);
										}
									}
								}
								
								lock (cache)
								{
									//See if our current size is 25% the size of the largest size we've been
									//and compact (clone to a new table) if so, since the table maintains the
									//largest size it has ever been.
									//TODO: 50 is a super arbitrary upper bound
									if (cache.totalAdditions > 50 && cache.totalAdditions - cache.currentTable.size() > (cache.currentTable.size() >> 2))
									{
										System.Collections.Hashtable newTable = new Hashtable(cache.currentTable.size());
										int oldMax = cache.totalAdditions;
										//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
										for (System.Collections.IEnumerator en = table.keys(); en.MoveNext(); )
										{
											//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
											System.Object key = en.Current;
											newTable[key] = cache.currentTable.get_Renamed(key);
										}
										cache.currentTable = newTable;
										cache.totalAdditions = cache.currentTable.size();
									}
								}
							}
						}
						for (int id = toRemove.size() - 1; id >= 0; --id)
						{
							caches.removeElementAt(toRemove.elementAt(id));
						}
						try
						{
							//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
							System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 3000));
						}
						catch (System.Threading.ThreadInterruptedException e)
						{
							// TODO Auto-generated catch block
							SupportClass.WriteStackTrace(e, Console.Error);
						}
					}
					catch (System.Exception e)
					{
						if (e is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
							((org.javarosa.core.io.StreamsUtil.DirectionalIOException) e).printStackTrace();
						else
							SupportClass.WriteStackTrace(e, Console.Error);
					}
				}
			}
		}
		private void  InitBlock()
		{
			int totalAdditions = 0;
		}
		
		< K >
		
		
		private Hashtable < Integer, WeakReference > currentTable;
		
		
		private static List< WeakReference > caches = new List< WeakReference >();
		
		//UPGRADE_NOTE: The initialization of  'cleaner' was moved to static method 'org.javarosa.core.util.CacheTable'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static SupportClass.ThreadClass cleaner;
		
		private static void  registerCache(CacheTable table)
		{
			caches.addElement(new System.WeakReference(table));
			lock (cleaner)
			{
				if (!cleaner.IsAlive)
				{
					cleaner.Start();
				}
			}
		}
		
		public CacheTable():base()
		{
			InitBlock();
			
			currentTable = new Hashtable < Integer, WeakReference >();
			registerCache(this);
		}
		
		public virtual K intern(K k)
		{
			lock (this)
			{
				int hash = k.hashCode();
				K nk = retrieve(hash);
				if (nk == null)
				{
					register(hash, k);
					return k;
				}
				if (k.equals(nk))
				{
					return nk;
				}
				else
				{
					//Collision. We should deal with this better for interning (and not manually caching) tables.
				}
				return k;
			}
		}
		
		
		public virtual K retrieve(int key)
		{
			lock (this)
			{
				if (!currentTable.containsKey(DataUtil.integer(key)))
				{
					return null;
				}
				K retVal = (K) currentTable.get_Renamed(DataUtil.integer(key)).get_Renamed();
				if (retVal == null)
				{
					currentTable.remove(DataUtil.integer(key));
				}
				return retVal;
			}
		}
		
		public virtual void  register(int key, K item)
		{
			lock (this)
			{
				currentTable.put(DataUtil.integer(key), new System.WeakReference(item));
				totalAdditions++;
			}
		}
		static CacheTable()
		{
			cleaner = new SupportClass.ThreadClass(new System.Threading.ThreadStart(new AnonymousClassRunnable(this).Run));
		}
	}
}