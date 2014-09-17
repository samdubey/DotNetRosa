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
using DateData = org.javarosa.core.model.data.DateData;
using DecimalData = org.javarosa.core.model.data.DecimalData;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using IntegerData = org.javarosa.core.model.data.IntegerData;
using LongData = org.javarosa.core.model.data.LongData;
using SelectMultiData = org.javarosa.core.model.data.SelectMultiData;
using StringData = org.javarosa.core.model.data.StringData;
namespace org.javarosa.core.model.utils
{
	
	/// <author>  Clayton Sims
	/// </author>
	/// <date>  Mar 30, 2009  </date>
	/// <summary> 
	/// </summary>
	public class PreloadUtils
	{
		
		/// <summary> Note: This method is a hack to fix the problem that we don't know what
		/// data type we're using when we have a preloader. That should get fixed, 
		/// and this method should be removed.
		/// </summary>
		/// <param name="o">
		/// </param>
		/// <returns>
		/// </returns>
		public static IAnswerData wrapIndeterminedObject(System.Object o)
		{
			if (o == null)
			{
				return null;
			}
			
			//TODO: Replace this all with an uncast data
			if (o is System.String)
			{
				return new StringData((System.String) o);
			}
			else if (o is System.DateTime)
			{
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
				return new DateData(ref new System.DateTime[]{(System.DateTime) o}[0]);
			}
			else if (o is System.Int32)
			{
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
				return new IntegerData(ref new System.Int32[]{(System.Int32) o}[0]);
			}
			else if (o is System.Int64)
			{
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
				return new LongData(ref new System.Int64[]{(System.Int64) o}[0]);
			}
			else if (o is System.Double)
			{
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
				return new DecimalData(ref new System.Double[]{(System.Double) o}[0]);
			}
			else if (o is System.Collections.ArrayList)
			{
				return new SelectMultiData((System.Collections.ArrayList) o);
			}
			else if (o is IAnswerData)
			{
				return (IAnswerData) o;
			}
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			return new StringData(o.ToString());
		}
	}
}