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
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.services.locale
{
	
	/// <author>  Clayton Sims
	/// </author>
	/// <date>  Jun 1, 2009  </date>
	/// <summary> 
	/// </summary>
	public class ResourceFileDataSource : LocaleDataSource
	{
		virtual public OrderedMap LocalizedText
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.services.locale.LocaleDataSource#getLocalizedText()
			*/
			
			get
			{
				return loadLocaleResource(resourceURI);
			}
			
		}
		
		internal System.String resourceURI;
		
		/// <summary> NOTE: FOR SERIALIZATION ONLY!</summary>
		public ResourceFileDataSource()
		{
		}
		
		/// <summary> Creates a new Data Source for Locale data with the given resource URI.
		/// 
		/// </summary>
		/// <param name="resourceURI">a URI to the resource file from which data should be loaded
		/// </param>
		/// <throws>  NullPointerException if resourceURI is null </throws>
		public ResourceFileDataSource(System.String resourceURI)
		{
			if (resourceURI == null)
			{
				throw new System.NullReferenceException("Resource URI cannot be null when creating a Resource File Data Source");
			}
			this.resourceURI = resourceURI;
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.externalizable.Externalizable#readExternal(java.io.DataInputStream, org.javarosa.core.util.externalizable.PrototypeFactory)
		*/
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			//UPGRADE_ISSUE: Method 'java.io.DataInputStream.readUTF' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioDataInputStreamreadUTF'"
			resourceURI = in_Renamed.readUTF();
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.externalizable.Externalizable#writeExternal(java.io.DataOutputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			//UPGRADE_ISSUE: Method 'java.io.DataOutputStream.writeUTF' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioDataOutputStreamwriteUTF_javalangString'"
			out_Renamed.writeUTF(resourceURI);
		}
		
		/// <param name="resourceName">A path to a resource file provided in the current environment
		/// 
		/// </param>
		/// <returns> a dictionary of key/value locale pairs from a file in the resource directory 
		/// </returns>
		private OrderedMap loadLocaleResource(System.String resourceName)
		{
			//UPGRADE_ISSUE: Method 'java.lang.Class.getResourceAsStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetResourceAsStream_javalangString'"
			//UPGRADE_ISSUE: Class 'java.lang.System' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			System.IO.Stream is_Renamed = typeof(System_Renamed).getResourceAsStream(resourceName);
			// TODO: This might very well fail. Best way to handle?
			OrderedMap locale = new OrderedMap();
			int chunk = 100;
			System.IO.StreamReader isr;
			try
			{
				//UPGRADE_TODO: Constructor 'java.io.InputStreamReader.InputStreamReader' was converted to 'System.IO.StreamReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioInputStreamReaderInputStreamReader_javaioInputStream_javalangString'"
				isr = new System.IO.StreamReader(is_Renamed, System.Text.Encoding.GetEncoding("UTF-8"));
			}
			catch (System.Exception e)
			{
				throw new System.SystemException("Failed to load locale resource " + resourceName + ". Is it in the jar?");
			}
			bool done = false;
			char[] cbuf = new char[chunk];
			int offset = 0;
			int curline = 0;
			
			try
			{
				System.String line = "";
				while (!done)
				{
					//UPGRADE_TODO: Method 'java.io.InputStreamReader.read' was converted to 'System.IO.StreamReader.Read' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioInputStreamReaderread_char[]_int_int'"
					int read = isr.Read(cbuf, offset, chunk - offset);
					if (read == - 1)
					{
						done = true;
						if ((System.Object) line != (System.Object) "")
						{
							parseAndAdd(locale, line, curline);
						}
						break;
					}
					System.String stringchunk = new System.String(cbuf, offset, read);
					
					int index = 0;
					
					while (index != - 1)
					{
						//UPGRADE_WARNING: Method 'java.lang.String.indexOf' was converted to 'System.String.IndexOf' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
						int nindex = stringchunk.IndexOf('\n', index);
						//UTF-8 often doesn't encode with newline, but with CR, so if we 
						//didn't find one, we'll try that
						if (nindex == - 1)
						{
							//UPGRADE_WARNING: Method 'java.lang.String.indexOf' was converted to 'System.String.IndexOf' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
							nindex = stringchunk.IndexOf('\r', index);
						}
						if (nindex == - 1)
						{
							line += stringchunk.Substring(index);
							break;
						}
						else
						{
							line += stringchunk.Substring(index, (nindex) - (index));
							//Newline. process our string and start the next one.
							curline++;
							parseAndAdd(locale, line, curline);
							line = "";
						}
						index = nindex + 1;
					}
				}
			}
			catch (System.IO.IOException e)
			{
				// TODO Auto-generated catch block
				if (e is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
					((org.javarosa.core.io.StreamsUtil.DirectionalIOException) e).printStackTrace();
				else
					SupportClass.WriteStackTrace(e, Console.Error);
			}
			finally
			{
				try
				{
					is_Renamed.Close();
				}
				catch (System.IO.IOException e)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					System.Console.Out.WriteLine("Input Stream for resource file " + resourceURI + " failed to close. This will eat up your memory! Fix Problem! [" + e.Message + "]");
					if (e is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
						((org.javarosa.core.io.StreamsUtil.DirectionalIOException) e).printStackTrace();
					else
						SupportClass.WriteStackTrace(e, Console.Error);
				}
			}
			return locale;
		}
		
		private void  parseAndAdd(OrderedMap locale, System.String line, int curline)
		{
			
			//trim whitespace.
			line = line.Trim();
			
			//clear comments
			while (line.IndexOf("#") != - 1)
			{
				line = line.Substring(0, (line.IndexOf("#")) - (0));
			}
			if (line.IndexOf('=') == - 1)
			{
				// TODO: Invalid line. Empty lines are fine, especially with comments,
				// but it might be hard to get all of those.
				if (line.Trim().Equals(""))
				{
					//Empty Line
				}
				else
				{
					System.Console.Out.WriteLine("Invalid line (#" + curline + ") read: " + line);
				}
			}
			else
			{
				//Check to see if there's anything after the '=' first. Otherwise there
				//might be some big problems.
				if (line.IndexOf('=') != line.Length - 1)
				{
					System.String value_Renamed = line.Substring(line.IndexOf('=') + 1, (line.Length) - (line.IndexOf('=') + 1));
					locale.put(line.Substring(0, (line.IndexOf('=')) - (0)), value_Renamed);
				}
				else
				{
					System.Console.Out.WriteLine("Invalid line (#" + curline + ") read: '" + line + "'. No value follows the '='.");
				}
			}
		}
	}
}