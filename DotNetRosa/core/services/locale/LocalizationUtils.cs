/// <summary> </summary>
using System;
using OrderedMap = org.javarosa.core.util.OrderedMap;
namespace org.javarosa.core.services.locale
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class LocalizationUtils
	{
		public LocalizationUtils()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			// TODO: This might very well fail. Best way to handle?
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			OrderedMap < String, String > locale = new OrderedMap < String, String >();
			int chunk = 100;
			System.IO.StreamReader isr;
			isr = new InputStreamReader(is_Renamed, "UTF-8");
			bool done = false;
			char[] cbuf = new char[chunk];
			int offset = 0;
			int curline = 0;
			
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
			is_Renamed.close();
			return locale;
			
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
		/// <param name="is">A path to a resource file provided in the current environment
		/// 
		/// </param>
		/// <returns> a dictionary of key/value locale pairs from a file in the resource directory 
		/// </returns>
		/// <throws>  IOException  </throws>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public static OrderedMap < String, String > parseLocaleInput(InputStream is) throws IOException
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private static
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		void parseAndAdd(OrderedMap < String, String > locale, String line, int curline)
	}
}