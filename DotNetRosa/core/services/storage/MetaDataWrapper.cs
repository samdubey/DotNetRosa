/// <summary> </summary>
using System;
namespace org.javarosa.core.services.storage
{
	
	/// <summary> An internal-use class to keep track of metadata records without requiring
	/// the original object to remain in memory
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public class MetaDataWrapper : IMetaData
	{
		public MetaDataWrapper()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			this.data = data;
		}
		virtual public System.String[] MetaDataFields
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.services.storage.IMetaData#getMetaDataFields()
			*/
			
			get
			{
				System.String[] fields = new System.String[data.size()];
				int count = 0;
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				for(String field: data.keySet())
				{
					fields[count] = field;
				}
				return fields;
			}
			
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private HashMap < String, Object > data;
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public MetaDataWrapper(HashMap < String, Object > data)
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IMetaData#getMetaData()
		*/
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public virtual System.Collections.Hashtable getMetaData()
		{
			return data;
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IMetaData#getMetaData(java.lang.String)
		*/
		public virtual System.Object getMetaData(System.String fieldName)
		{
			return data.get_Renamed(fieldName);
		}
	}
}