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
//UPGRADE_TODO: The type 'org.javarosa.core.log.IFullLogSerializer' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using IFullLogSerializer = org.javarosa.core.log.IFullLogSerializer;
using StreamLogSerializer = org.javarosa.core.log.StreamLogSerializer;
using SortedIntSet = org.javarosa.core.util.SortedIntSet;
namespace org.javarosa.core.api
{
	
	/// <summary> IIncidentLogger's are used for instrumenting applications to identify usage
	/// patterns, usability errors, and general trajectories through applications.
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// </author>
	/// <date>  Apr 10, 2009  </date>
	/// <summary> 
	/// </summary>
	public interface ILogger
	{
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		void  log(System.String type, System.String message, ref System.DateTime logDate);
		
		void  clearLogs();
	}
	
	
	public < T > T serializeLogs(IFullLogSerializer < T > serializer);
	
	
	public
	
	void serializeLogs(StreamLogSerializer serializer) throws IOException;
	
	public
	
	void serializeLogs(StreamLogSerializer serializer, int limit) throws IOException;
	
	
	public
	
	void panic();
	
	
	public int logSize();
	
	
	public
	
	void halt();
	
	}
}