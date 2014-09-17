/// <summary> </summary>
using System;
using SortedIntSet = org.javarosa.core.util.SortedIntSet;
namespace org.javarosa.core.log
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public abstract class StreamLogSerializer
	{
		
		internal SortedIntSet logIDs;
		internal StreamLogSerializer.Purger purger = null;
		
		public interface Purger
		{
			void  purge(SortedIntSet IDs);
		}
		
		public StreamLogSerializer()
		{
			logIDs = new SortedIntSet();
		}
		
		public void  serializeLog(int id, LogEntry entry)
		{
			logIDs.add(id);
			serializeLog(entry);
		}
		
		protected internal abstract void  serializeLog(LogEntry entry);
		
		public virtual void  setPurger(StreamLogSerializer.Purger purger)
		{
			this.purger = purger;
		}
		
		public virtual void  purge()
		{
			//The purger is optional, not mandatory.
			if (purger != null)
			{
				this.purger.purge(logIDs);
			}
		}
	}
}