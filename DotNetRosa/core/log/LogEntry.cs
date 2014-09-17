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
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.log
{
	
	/// <author>  Clayton Sims
	/// </author>
	/// <date>  Apr 10, 2009  </date>
	/// <summary> 
	/// </summary>
	public class LogEntry
	{
		/// <returns> the time
		/// </returns>
		virtual public System.DateTime Time
		{
			get
			{
				return time;
			}
			
		}
		/// <returns> the type
		/// </returns>
		virtual public System.String Type
		{
			get
			{
				return type;
			}
			
		}
		/// <returns> the message
		/// </returns>
		virtual public System.String Message
		{
			get
			{
				return message;
			}
			
		}
		
		public const System.String STORAGE_KEY = "LOG";
		
		public static System.String LOG_TYPE_APPLICATION = "APP";
		public static System.String LOG_TYPE_ACTIVITY = "ACTIVITY";
		
		internal System.DateTime time;
		
		internal System.String type;
		
		internal System.String message;
		
		/// <summary> NOTE: For serialization purposes only</summary>
		public LogEntry()
		{
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public LogEntry(System.String type, System.String message, ref System.DateTime time)
		{
			this.time = time;
			this.type = type;
			this.message = message;
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.externalizable.Externalizable#readExternal(java.io.DataInputStream, org.javarosa.core.util.externalizable.PrototypeFactory)
		*/
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			time = ExtUtil.readDate(in_Renamed);
			type = ExtUtil.readString(in_Renamed);
			message = ExtUtil.readString(in_Renamed);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.externalizable.Externalizable#writeExternal(java.io.DataOutputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			ExtUtil.writeDate(out_Renamed, ref time);
			ExtUtil.writeString(out_Renamed, ExtUtil.emptyIfNull(type));
			ExtUtil.writeString(out_Renamed, ExtUtil.emptyIfNull(message));
		}
	}
}