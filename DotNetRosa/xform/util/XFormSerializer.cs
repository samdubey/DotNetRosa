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
//UPGRADE_TODO: The type 'org.kxml2.io.KXmlSerializer' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using KXmlSerializer = org.kxml2.io.KXmlSerializer;
//UPGRADE_TODO: The type 'org.kxml2.kdom.Document' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Document = org.kxml2.kdom.Document;
//UPGRADE_TODO: The type 'org.kxml2.kdom.Element' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Element = org.kxml2.kdom.Element;
namespace org.javarosa.xform.util
{
	
	/* this is just a big dump of serialization-related code */
	
	/* basically, anything that didn't belong in XFormParser */
	
	public class XFormSerializer
	{
		
		public static System.IO.MemoryStream getStream(Document doc)
		{
			KXmlSerializer serializer = new KXmlSerializer();
			System.IO.MemoryStream bos = new System.IO.MemoryStream();
			//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
			System.IO.BinaryWriter dos = new System.IO.BinaryWriter(bos);
			try
			{
				serializer.setOutput(dos, null);
				doc.write(serializer);
				serializer.flush();
				return bos;
			}
			catch (System.Exception e)
			{
				if (e is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
					((org.javarosa.core.io.StreamsUtil.DirectionalIOException) e).printStackTrace();
				else
					SupportClass.WriteStackTrace(e, Console.Error);
				return null;
			}
		}
		
		public static System.String elementToString(Element e)
		{
			KXmlSerializer serializer = new KXmlSerializer();
			
			System.IO.MemoryStream bos = new System.IO.MemoryStream();
			//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
			System.IO.BinaryWriter dos = new System.IO.BinaryWriter(bos);
			System.String s = null;
			try
			{
				serializer.setOutput(dos, null);
				e.write(serializer);
				serializer.flush();
				//UPGRADE_TODO: The differences in the Format  of parameters for constructor 'java.lang.String.String'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				s = System.Text.Encoding.GetEncoding("UTF-8").GetString(SupportClass.ToByteArray(SupportClass.ToSByteArray(bos.ToArray())));
				return s;
			}
			catch (System.IO.IOException uce)
			{
				SupportClass.WriteStackTrace(uce, Console.Error);
			}
			catch (System.Exception ex)
			{
				if (ex is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
					((org.javarosa.core.io.StreamsUtil.DirectionalIOException) ex).printStackTrace();
				else
					SupportClass.WriteStackTrace(ex, Console.Error);
				return null;
			}
			
			return null;
		}
		
		public static System.String getString(Document doc)
		{
			System.IO.MemoryStream bos = getStream(doc);
			
			sbyte[] byteArr = SupportClass.ToSByteArray(bos.ToArray());
			char[] charArray = new char[byteArr.Length];
			for (int i = 0; i < byteArr.Length; i++)
				charArray[i] = (char) byteArr[i];
			
			return System.Convert.ToString(charArray);
		}
		
		public static sbyte[] getUtfBytes(Document doc)
		{
			KXmlSerializer serializer = new KXmlSerializer();
			System.IO.MemoryStream bos = new System.IO.MemoryStream();
			try
			{
				//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Writer' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
				//UPGRADE_TODO: Constructor 'java.io.OutputStreamWriter.OutputStreamWriter' was converted to 'System.IO.StreamWriter.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioOutputStreamWriterOutputStreamWriter_javaioOutputStream_javalangString'"
				System.IO.StreamWriter osw = new System.IO.StreamWriter(bos, System.Text.Encoding.GetEncoding("UTF-8"));
				serializer.setOutput(osw);
				doc.write(serializer);
				serializer.flush();
				return SupportClass.ToSByteArray(bos.ToArray());
			}
			catch (System.Exception e)
			{
				if (e is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
					((org.javarosa.core.io.StreamsUtil.DirectionalIOException) e).printStackTrace();
				else
					SupportClass.WriteStackTrace(e, Console.Error);
				return null;
			}
		}
	}
}