/// <summary> </summary>
using System;
using SerializationWrapper = org.javarosa.core.services.storage.WrappingStorageUtility.SerializationWrapper;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.services.storage
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class FauxIndexedPersistable : Persistable, IMetaData
	{
		virtual public int ID
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.services.storage.Persistable#getID()
			*/
			
			get
			{
				return p.ID;
			}
			
			/* (non-Javadoc)
			* @see org.javarosa.core.services.storage.Persistable#setID(int)
			*/
			
			set
			{
				p.ID = value;
			}
			
		}
		virtual public System.String[] MetaDataFields
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.services.storage.IMetaData#getMetaDataFields()
			*/
			
			get
			{
				if (m != null)
				{
					return m.MetaDataFields;
				}
				throw new System.SystemException("Attempt to index unindexible " + p.GetType().FullName);
			}
			
		}
		
		internal Persistable p;
		internal WrappingStorageUtility.SerializationWrapper w;
		internal IMetaData m;
		public FauxIndexedPersistable(Persistable p, WrappingStorageUtility.SerializationWrapper w)
		{
			this.p = p;
			this.w = w;
			this.m = null;
		}
		
		public FauxIndexedPersistable(Persistable p, WrappingStorageUtility.SerializationWrapper w, IMetaData m)
		{
			this.p = p;
			this.w = w;
			this.m = m;
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.externalizable.Externalizable#readExternal(java.io.DataInputStream, org.javarosa.core.util.externalizable.PrototypeFactory)
		*/
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			w.readExternal(in_Renamed, pf);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.externalizable.Externalizable#writeExternal(java.io.DataOutputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			w.writeExternal(out_Renamed);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IMetaData#getMetaData()
		*/
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public virtual System.Collections.Hashtable getMetaData()
		{
			if (m != null)
			{
				return m.getMetaData();
			}
			throw new System.SystemException("Attempt to index unindexible " + p.GetType().FullName);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.IMetaData#getMetaData(java.lang.String)
		*/
		public virtual System.Object getMetaData(System.String fieldName)
		{
			if (m != null)
			{
				return m.getMetaData(fieldName);
			}
			throw new System.SystemException("Attempt to index unindexible " + p.GetType().FullName);
		}
	}
}