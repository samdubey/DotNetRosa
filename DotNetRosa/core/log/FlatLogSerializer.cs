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
namespace org.javarosa.core.log
{
	
	/// <author>  Clayton Sims
	/// </author>
	/// <date>  Apr 10, 2009 </date>
	/// <summary> 
	/// </summary>
	public class FlatLogSerializer : IFullLogSerializer
	{
		public FlatLogSerializer()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			
			/* (non-Javadoc)
			* @see org.javarosa.core.log.ILogSerializer#serializeLog(org.javarosa.core.log.IncidentLog)
			*/
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		< String >
		private System.String serializeLog(LogEntry log)
		{
			//UPGRADE_TODO: Method 'java.util.Date.toString' was converted to 'System.DateTime.ToString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDatetoString'"
			return "[" + log.Type + "] " + log.Time.ToString("r") + ": " + log.message + "\n";
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.log.ILogSerializer#serializeLogs(org.javarosa.core.log.IncidentLog[])
		*/
		public virtual System.String serializeLogs(LogEntry[] logs)
		{
			StringBuilder log = new StringBuilder();
			for (int i = 0; i < logs.Length; ++i)
			{
				log.append(this.serializeLog(logs[i]));
			}
			return log.toString();
		}
	}
}