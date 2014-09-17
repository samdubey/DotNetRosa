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
//UPGRADE_TODO: The type 'org.kxml2.kdom.Element' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Element = org.kxml2.kdom.Element;
namespace org.javarosa.xform.parse
{
	
	/// <summary> Exception thrown when an XForms Parsing error occurs.
	/// 
	/// </summary>
	/// <author>  Drew Roos
	/// 
	/// </author>
	// Clayton Sims - Aug 18, 2008 : This doesn't actually seem
	// to be a RuntimeException to me. Is there justification
	// as to why it is?
	[Serializable]
	public class XFormParseException:System.SystemException
	{
		public override System.String Message
		{
			get
			{
				if (element == null)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					return base.Message;
				}
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				return base.Message + XFormParser.getVagueLocation(element);
			}
			
		}
		internal Element element;
		
		public XFormParseException()
		{
		}
		
		public XFormParseException(System.String msg):base(msg)
		{
			element = null;
		}
		
		public XFormParseException(System.String msg, Element e):base(msg)
		{
			element = e;
		}
	}
}