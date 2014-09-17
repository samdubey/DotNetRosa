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
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.services.transport.payload
{
	
	/// <summary> A ByteArrayPayload is a simple payload consisting of a
	/// byte array.
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// </author>
	/// <date>  Dec 18, 2008 </date>
	/// <summary> 
	/// </summary>
	public class ByteArrayPayload : IDataPayload
	{
		virtual public System.IO.Stream PayloadStream
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.services.transport.IDataPayload#getPayloadStream()
			*/
			
			get
			{
				
				return new System.IO.MemoryStream(SupportClass.ToByteArray(payload));
			}
			
		}
		virtual public System.String PayloadId
		{
			/*
			* (non-Javadoc)
			* @see org.javarosa.core.services.transport.IDataPayload#getPayloadId()
			*/
			
			get
			{
				return id;
			}
			
		}
		virtual public int PayloadType
		{
			/*
			* (non-Javadoc)
			* @see org.javarosa.core.services.transport.IDataPayload#getPayloadType()
			*/
			
			get
			{
				return type;
			}
			
		}
		virtual public long Length
		{
			get
			{
				return payload.Length;
			}
			
		}
		virtual public int TransportId
		{
			get
			{
				//TODO: Most messages can include this data
				return - 1;
			}
			
		}
		internal sbyte[] payload;
		
		internal System.String id;
		
		internal int type;
		
		/// <summary> Note: Only useful for serialization.</summary>
		public ByteArrayPayload()
		{
		}
		
		/// <summary> </summary>
		/// <param name="payload">The byte array for this payload.
		/// </param>
		/// <param name="id">An optional id identifying the payload
		/// </param>
		/// <param name="type">The type of data for this byte array
		/// </param>
		public ByteArrayPayload(sbyte[] payload, System.String id, int type)
		{
			this.payload = payload;
			this.id = id;
			this.type = type;
		}
		
		/// <summary> </summary>
		/// <param name="payload">The byte array for this payload.
		/// </param>
		public ByteArrayPayload(sbyte[] payload)
		{
			this.payload = payload;
			this.id = null;
			this.type = org.javarosa.core.services.transport.payload.IDataPayload_Fields.PAYLOAD_TYPE_XML;
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.externalizable.Externalizable#readExternal(java.io.DataInputStream, org.javarosa.core.util.externalizable.PrototypeFactory)
		*/
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			int length = in_Renamed.ReadInt32();
			if (length > 0)
			{
				this.payload = new sbyte[length];
				SupportClass.ReadInput(in_Renamed.BaseStream, this.payload, 0, this.payload.Length);
			}
			id = ExtUtil.nullIfEmpty(ExtUtil.readString(in_Renamed));
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.externalizable.Externalizable#writeExternal(java.io.DataOutputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			out_Renamed.Write(payload.Length);
			if (payload.Length > 0)
			{
				out_Renamed.Write(SupportClass.ToByteArray(payload));
			}
			ExtUtil.writeString(out_Renamed, ExtUtil.emptyIfNull(id));
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.services.transport.IDataPayload#accept(org.javarosa.core.services.transport.IDataPayloadVisitor)
		*/
		public virtual System.Object accept(IDataPayloadVisitor visitor)
		{
			return visitor.visit(this);
		}
	}
}