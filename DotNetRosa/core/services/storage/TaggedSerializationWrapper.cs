/// <summary> </summary>
using System;
using SerializationWrapper = org.javarosa.core.services.storage.WrappingStorageUtility.SerializationWrapper;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapTagged = org.javarosa.core.util.externalizable.ExtWrapTagged;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.services.storage
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class TaggedSerializationWrapper : WrappingStorageUtility.SerializationWrapper
	{
		virtual public Externalizable Data
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.services.storage.WrappingStorageUtility.SerializationWrapper#getData()
			*/
			
			get
			{
				return e;
			}
			
			/* (non-Javadoc)
			* @see org.javarosa.core.services.storage.WrappingStorageUtility.SerializationWrapper#setData(org.javarosa.core.util.externalizable.Externalizable)
			*/
			
			set
			{
				this.e = value;
			}
			
		}
		
		internal Externalizable e;
		
		public TaggedSerializationWrapper()
		{
		}
		
		public virtual System.Type baseType()
		{
			return typeof(Externalizable);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.externalizable.Externalizable#readExternal(java.io.DataInputStream, org.javarosa.core.util.externalizable.PrototypeFactory)
		*/
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			e = (Externalizable) ExtUtil.read(in_Renamed, new ExtWrapTagged());
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.externalizable.Externalizable#writeExternal(java.io.DataOutputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.write(out_Renamed, new ExtWrapTagged(e));
		}
		
		public virtual void  clean()
		{
			e = null;
		}
	}
}