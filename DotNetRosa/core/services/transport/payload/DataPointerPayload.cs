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
using System;
using IDataPointer = org.javarosa.core.data.IDataPointer;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapTagged = org.javarosa.core.util.externalizable.ExtWrapTagged;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.services.transport.payload
{
	
	/// <summary> A payload for a Pointer to some data.
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// </author>
	/// <date>  Dec 29, 2008 </date>
	/// <summary> 
	/// </summary>
	public class DataPointerPayload : IDataPayload
	{
		private void  InitBlock()
		{
			return visitor.visit(this);
		}
		virtual public long Length
		{
			/*
			* (non-Javadoc)
			* @see org.javarosa.core.services.transport.IDataPayload#getLength()
			*/
			
			get
			{
				//Unimplemented. This method will eventually leave the contract
				return pointer.Length;
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
				return pointer.DisplayText;
			}
			
		}
		virtual public System.IO.Stream PayloadStream
		{
			/*
			* (non-Javadoc)
			* @see org.javarosa.core.services.transport.IDataPayload#getPayloadStream()
			*/
			
			get
			{
				return pointer.DataStream;
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
				System.String display = pointer.DisplayText;
				if (display == null || display.LastIndexOf('.') == - 1)
				{
					//uhhhh....?
					return org.javarosa.core.services.transport.payload.IDataPayload_Fields.PAYLOAD_TYPE_TEXT;
				}
				
				System.String ext = display.Substring(display.LastIndexOf('.') + 1);
				
				if (ext.Equals("jpg") || ext.Equals("jpeg"))
				{
					return org.javarosa.core.services.transport.payload.IDataPayload_Fields.PAYLOAD_TYPE_JPG;
				}
				
				return org.javarosa.core.services.transport.payload.IDataPayload_Fields.PAYLOAD_TYPE_JPG;
			}
			
		}
		virtual public int TransportId
		{
			get
			{
				return - 1;
			}
			
		}
		internal IDataPointer pointer;
		
		/// <summary> Note: Only useful for serialization.</summary>
		public DataPointerPayload()
		{
			InitBlock();
		}
		
		public DataPointerPayload(IDataPointer pointer)
		{
			InitBlock();
			this.pointer = pointer;
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.services.transport.IDataPayload#accept(org.javarosa.core.services.transport.IDataPayloadVisitor)
		*/
		
		public < T > T accept(IDataPayloadVisitor < T > visitor)
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.util.externalizable.Externalizable#readExternal(java.io.DataInputStream, org.javarosa.core.util.externalizable.PrototypeFactory)
		*/
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			pointer = (IDataPointer) ExtUtil.read(in_Renamed, new ExtWrapTagged());
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.util.externalizable.Externalizable#writeExternal(java.io.DataOutputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.write(out_Renamed, new ExtWrapTagged(pointer));
		}
	}
}