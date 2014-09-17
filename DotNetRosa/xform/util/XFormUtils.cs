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
using FormDef = org.javarosa.core.model.FormDef;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using IXFormParserFactory = org.javarosa.xform.parse.IXFormParserFactory;
using XFormParseException = org.javarosa.xform.parse.XFormParseException;
using XFormParser = org.javarosa.xform.parse.XFormParser;
using XFormParserFactory = org.javarosa.xform.parse.XFormParserFactory;
//UPGRADE_TODO: The type 'org.kxml2.kdom.Element' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Element = org.kxml2.kdom.Element;
namespace org.javarosa.xform.util
{
	
	/// <summary> Static Utility methods pertaining to XForms.
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public class XFormUtils
	{
		private static IXFormParserFactory _factory = new XFormParserFactory();
		
		public static IXFormParserFactory setXFormParserFactory(IXFormParserFactory factory)
		{
			IXFormParserFactory oldFactory = _factory;
			_factory = factory;
			return oldFactory;
		}
		
		public static FormDef getFormFromResource(System.String resource)
		{
			//UPGRADE_ISSUE: Method 'java.lang.Class.getResourceAsStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetResourceAsStream_javalangString'"
			//UPGRADE_ISSUE: Class 'java.lang.System' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			System.IO.Stream is_Renamed = typeof(System_Renamed).getResourceAsStream(resource);
			if (is_Renamed == null)
			{
				System.Console.Error.WriteLine("Can't find form resource \"" + resource + "\". Is it in the JAR?");
				return null;
			}
			
			return getFormFromInputStream(is_Renamed);
		}
		
		
		public static FormDef getFormRaw(System.IO.StreamReader isr)
		{
			return _factory.getXFormParser(isr).parse();
		}
		
		/*
		* This method throws XFormParseException when the form has errors.
		*/
		public static FormDef getFormFromInputStream(System.IO.Stream is_Renamed)
		{
			System.IO.StreamReader isr = null;
			try
			{
				try
				{
					//UPGRADE_TODO: Constructor 'java.io.InputStreamReader.InputStreamReader' was converted to 'System.IO.StreamReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioInputStreamReaderInputStreamReader_javaioInputStream_javalangString'"
					isr = new System.IO.StreamReader(is_Renamed, System.Text.Encoding.GetEncoding("UTF-8"));
				}
				catch (System.IO.IOException uee)
				{
					System.Console.Out.WriteLine("UTF 8 encoding unavailable, trying default encoding");
					isr = new System.IO.StreamReader(is_Renamed, System.Text.Encoding.Default);
				}
				
				return _factory.getXFormParser(isr).parse();
			}
			catch (System.IO.IOException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new XFormParseException("IO Exception during parse! " + e.Message);
			}
			finally
			{
				try
				{
					if (isr != null)
					{
						isr.Close();
					}
				}
				catch (System.IO.IOException e)
				{
					System.Console.Error.WriteLine("IO Exception while closing stream.");
					if (e is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
						((org.javarosa.core.io.StreamsUtil.DirectionalIOException) e).printStackTrace();
					else
						SupportClass.WriteStackTrace(e, Console.Error);
				}
			}
		}
		
		public static FormDef getFormFromSerializedResource(System.String resource)
		{
			FormDef returnForm = null;
			//UPGRADE_ISSUE: Method 'java.lang.Class.getResourceAsStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetResourceAsStream_javalangString'"
			//UPGRADE_ISSUE: Class 'java.lang.System' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			System.IO.Stream is_Renamed = typeof(System_Renamed).getResourceAsStream(resource);
			//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
			System.IO.BinaryReader dis = null;
			try
			{
				if (is_Renamed != null)
				{
					dis = new System.IO.BinaryReader(is_Renamed);
					returnForm = (FormDef) ExtUtil.read(dis, typeof(FormDef));
				}
				else
				{
					//#if debug.output==verbose
					System.Console.Out.WriteLine("ResourceStream NULL");
					//#endif
				}
			}
			catch (System.IO.IOException e)
			{
				if (e is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
					((org.javarosa.core.io.StreamsUtil.DirectionalIOException) e).printStackTrace();
				else
					SupportClass.WriteStackTrace(e, Console.Error);
			}
			catch (DeserializationException e)
			{
				SupportClass.WriteStackTrace(e, Console.Error);
			}
			finally
			{
				if (is_Renamed != null)
				{
					try
					{
						is_Renamed.Close();
					}
					catch (System.IO.IOException e)
					{
						if (e is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
							((org.javarosa.core.io.StreamsUtil.DirectionalIOException) e).printStackTrace();
						else
							SupportClass.WriteStackTrace(e, Console.Error);
					}
				}
				if (dis != null)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.io.FilterInputStream.close' was converted to 'System.IO.BinaryReader.Close' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFilterInputStreamclose'"
						dis.Close();
					}
					catch (System.IO.IOException e)
					{
						if (e is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
							((org.javarosa.core.io.StreamsUtil.DirectionalIOException) e).printStackTrace();
						else
							SupportClass.WriteStackTrace(e, Console.Error);
					}
				}
			}
			return returnForm;
		}
		
		
		/////Parser Attribute warning stuff
		
		public static System.Collections.ArrayList getAttributeList(Element e)
		{
			System.Collections.ArrayList atts = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			for (int i = 0; i < e.getAttributeCount(); i++)
			{
				atts.Add(e.getAttributeName(i));
			}
			
			return atts;
		}
		
		public static System.Collections.ArrayList getUnusedAttributes(Element e, System.Collections.ArrayList usedAtts)
		{
			System.Collections.ArrayList unusedAtts = getAttributeList(e);
			for (int i = 0; i < usedAtts.Count; i++)
			{
				if (unusedAtts.Contains(usedAtts[i]))
				{
					unusedAtts.Remove(usedAtts[i]);
				}
			}
			
			return unusedAtts;
		}
		
		public static System.String unusedAttWarning(Element e, System.Collections.ArrayList usedAtts)
		{
			System.String warning = "Warning: ";
			System.Collections.ArrayList ua = getUnusedAttributes(e, usedAtts);
			warning += (ua.Count + " Unrecognized attributes found in Element [" + e.getName() + "] and will be ignored: ");
			warning += "[";
			for (int i = 0; i < ua.Count; i++)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				warning += ua[i];
				if (i != ua.Count - 1)
					warning += ",";
			}
			warning += "] ";
			warning += ("Location:\n" + XFormParser.getVagueLocation(e));
			
			return warning;
		}
		
		public static bool showUnusedAttributeWarning(Element e, System.Collections.ArrayList usedAtts)
		{
			return getUnusedAttributes(e, usedAtts).size() > 0;
		}
		
		/// <summary> Is this element an Output tag?</summary>
		/// <param name="e">
		/// </param>
		/// <returns>
		/// </returns>
		public static bool isOutput(Element e)
		{
			if (e.getName().toLowerCase().equals("output"))
				return true;
			else
				return false;
		}
	}
}