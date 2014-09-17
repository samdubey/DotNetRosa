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
using OrderedMap = org.javarosa.core.util.OrderedMap;
using UnregisteredLocaleException = org.javarosa.core.util.UnregisteredLocaleException;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapMap = org.javarosa.core.util.externalizable.ExtWrapMap;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.services.locale
{
	
	/// <author>  Clayton Sims
	/// </author>
	/// <date>  May 26, 2009 </date>
	/// <summary> 
	/// </summary>
	public class TableLocaleSource : LocaleDataSource
	{
		private void  InitBlock()
		{
			this.localeData = localeData;
			return localeData;
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private OrderedMap < String, String > localeData;
		/*{ String -> String } */
		
		public TableLocaleSource()
		{
			InitBlock();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			localeData = new OrderedMap < String, String >();
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public TableLocaleSource(OrderedMap < String, String > localeData)
		
		
		/// <summary> Set a text mapping for a single text handle for a given locale.
		/// 
		/// </summary>
		/// <param name="textID">Text handle. Must not be null. Need not be previously defined for this locale.
		/// </param>
		/// <param name="text">Localized text for this text handle and locale. Will overwrite any previous mapping, if one existed.
		/// If null, will remove any previous mapping for this text handle, if one existed.
		/// </param>
		/// <throws>  UnregisteredLocaleException If locale is not defined or null. </throws>
		/// <throws>  NullPointerException if textID is null </throws>
		public virtual void  setLocaleMapping(System.String textID, System.String text)
		{
			if (textID == null)
			{
				throw new System.NullReferenceException("Null textID when attempting to register " + text + " in locale table");
			}
			if (text == null)
			{
				localeData.remove(textID);
			}
			else
			{
				localeData.put(textID, text);
			}
		}
		
		/// <summary> Determine whether a locale has a mapping for a given text handle. Only tests the specified locale and form; does
		/// not fallback to any default locale or text form.
		/// 
		/// </summary>
		/// <param name="textID">Text handle.
		/// </param>
		/// <returns> True if a mapping exists for the text handle in the given locale.
		/// </returns>
		/// <throws>  UnregisteredLocaleException If locale is not defined. </throws>
		public virtual bool hasMapping(System.String textID)
		{
			return (textID == null?false:localeData.get_Renamed(textID) != null);
		}
		
		
		public  override bool Equals(System.Object o)
		{
			if (!(o is TableLocaleSource))
			{
				return false;
			}
			TableLocaleSource l = (TableLocaleSource) o;
			return ExtUtil.equals(localeData, l.localeData);
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public OrderedMap < String, String > getLocalizedText()
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			localeData =(OrderedMap < String, String >) ExtUtil.read(in, new ExtWrapMap(String.
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		class, String.
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		class, ExtWrapMap.TYPE_ORDERED), pf);
		//UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void writeExternal(DataOutputStream out) throws IOException
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		ExtUtil.write(out, new ExtWrapMap(localeData));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
}