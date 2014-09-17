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
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
namespace org.javarosa.core.services.transport.payload
{
	
	/// <summary> IDataPayload is an interface that specifies a piece of data
	/// that will be transmitted over the wire to
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// </author>
	/// <date>  Dec 18, 2008 </date>
	/// <summary> 
	/// </summary>
	public struct IDataPayload_Fields{
		/// <summary> Data payload codes</summary>
		public readonly static int PAYLOAD_TYPE_TEXT = 0;
		public readonly static int PAYLOAD_TYPE_XML = 1;
		public readonly static int PAYLOAD_TYPE_JPG = 2;
		public readonly static int PAYLOAD_TYPE_HEADER = 3;
		public readonly static int PAYLOAD_TYPE_MULTI = 4;
		public readonly static int PAYLOAD_TYPE_SMS = 5; // sms's are a variant of TEXT having a limit on length.
	}
	public interface IDataPayload:Externalizable
	{
		//UPGRADE_NOTE: Members of interface 'IDataPayload' were extracted into structure 'IDataPayload_Fields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1045'"
		/// <summary> Gets the stream for this payload.
		/// 
		/// </summary>
		/// <returns> A stream for the data in this payload.
		/// </returns>
		/// <throws>  IOException </throws>
		System.IO.Stream PayloadStream
		{
			get;
			
		}
		/// <returns> A string identifying the contents of the payload
		/// </returns>
		System.String PayloadId
		{
			get;
			
		}
		/// <returns> The type of the data encapsulated by this
		/// payload.
		/// </returns>
		int PayloadType
		{
			get;
			
			/// <summary> Visitor pattern accept method.</summary>
			/// <param name="visitor">The visitor to visit this payload.
			/// </param>
			
		}
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public < T > T accept(IDataPayloadVisitor < T > visitor);
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public long getLength();
	
	/// <returns> A unique Id for the transport manager to use to be able to identify
	/// the status of transmissions related to this payload
	/// </returns>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public int getTransportId();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
}