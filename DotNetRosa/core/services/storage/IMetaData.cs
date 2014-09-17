using System;
namespace org.javarosa.core.services.storage
{
	
	public interface IMetaData
	{
		System.String[] MetaDataFields
		{
			//for the indefinite future, no meta-data field can have a value of null
			
			
			get;
			
		}
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		System.Collections.Hashtable getMetaData(); //<String, E>
		System.Object getMetaData(System.String fieldName);
	}
}