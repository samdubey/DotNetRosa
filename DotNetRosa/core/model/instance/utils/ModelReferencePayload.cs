/*
* Copyright (C) 2009 JavaRosa
*
* Licensed under the Apache License, Version 2.0 (the "License"); you may not
* use this file except in compliance with the License. You may obtain a copy of
* the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
* WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
* License for the specific language governing permissions and limitations under
* the License.
*/

/// <summary> </summary>
using System;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using IInstanceSerializingVisitor = org.javarosa.core.model.utils.IInstanceSerializingVisitor;
//UPGRADE_TODO: The type 'org.javarosa.core.services.storage.IStorageUtility' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using IStorageUtility = org.javarosa.core.services.storage.IStorageUtility;
using StorageManager = org.javarosa.core.services.storage.StorageManager;
using IDataPayload = org.javarosa.core.services.transport.payload.IDataPayload;
//UPGRADE_TODO: The type 'org.javarosa.core.services.transport.payload.IDataPayloadVisitor' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using IDataPayloadVisitor = org.javarosa.core.services.transport.payload.IDataPayloadVisitor;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.model.instance.utils
{
	
	/// <summary> The ModelReferencePayload essentially provides a wrapper functionality
	/// around a ModelTree to allow it to be used as a payload, but only to
	/// actually perform the various computationally expensive functions
	/// of serialization when required.
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// </author>
	/// <date>  Apr 27, 2009 </date>
	/// <summary> 
	/// </summary>
	public class ModelReferencePayload : IDataPayload
	{
		/// <param name="serializer">the serializer to set
		/// </param>
		virtual public IInstanceSerializingVisitor Serializer
		{
			set
			{
				this.serializer = value;
			}
			
		}
		virtual public long Length
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.services.transport.IDataPayload#getLength()
			*/
			
			get
			{
				memoize();
				return payload.Length;
			}
			
		}
		virtual public System.String PayloadId
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.services.transport.IDataPayload#getPayloadId()
			*/
			
			get
			{
				memoize();
				return payload.PayloadId;
			}
			
		}
		virtual public System.IO.Stream PayloadStream
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.services.transport.IDataPayload#getPayloadStream()
			*/
			
			get
			{
				memoize();
				return payload.PayloadStream;
			}
			
		}
		virtual public int PayloadType
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.services.transport.IDataPayload#getPayloadType()
			*/
			
			get
			{
				memoize();
				return payload.PayloadType;
			}
			
		}
		virtual public int TransportId
		{
			get
			{
				return recordId;
			}
			
		}
		virtual public System.String Destination
		{
			get
			{
				return destination;
			}
			
			set
			{
				this.destination = value;
			}
			
		}
		
		internal int recordId;
		internal IDataPayload payload;
		internal System.String destination = null;
		
		internal IInstanceSerializingVisitor serializer;
		
		//NOTE: Should only be used for serializaiton.
		public ModelReferencePayload()
		{
		}
		
		public ModelReferencePayload(int modelRecordId)
		{
			this.recordId = modelRecordId;
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.transport.IDataPayload#accept(org.javarosa.core.services.transport.IDataPayloadVisitor)
		*/
		public virtual System.Object accept(IDataPayloadVisitor visitor)
		{
			memoize();
			return payload.accept(visitor);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.externalizable.Externalizable#readExternal(java.io.DataInputStream, org.javarosa.core.util.externalizable.PrototypeFactory)
		*/
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			recordId = in_Renamed.ReadInt32();
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.externalizable.Externalizable#writeExternal(java.io.DataOutputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			out_Renamed.Write(recordId);
		}
		
		private void  memoize()
		{
			if (payload == null)
			{
				IStorageUtility instances = StorageManager.getStorage(FormInstance.STORAGE_KEY);
				try
				{
					FormInstance tree = (FormInstance) instances.read(recordId);
					payload = serializer.createSerializedPayload(tree);
				}
				catch (System.IO.IOException e)
				{
					//Assertion, do not catch!
					if (e is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
						((org.javarosa.core.io.StreamsUtil.DirectionalIOException) e).printStackTrace();
					else
						SupportClass.WriteStackTrace(e, Console.Error);
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					throw new System.SystemException("ModelReferencePayload failed to retrieve its model from rms [" + e.Message + "]");
				}
			}
		}
	}
}