using System;
namespace org.javarosa.core.util
{
	
	public class StreamsUtil
	{
		
		private StreamsUtil()
		{
			// private constructor
		}
		
		/// <summary> 
		/// Write everything from input stream to output stream, byte by byte then
		/// close the streams
		/// 
		/// 
		/// </summary>
		/// <param name="in">
		/// </param>
		/// <param name="out">
		/// </param>
		/// <throws>  IOException </throws>
		public static void  writeFromInputToOutput(System.IO.Stream in_Renamed, System.IO.Stream out_Renamed, long[] tally)
		{
			int val = in_Renamed.ReadByte();
			while (val != - 1)
			{
				out_Renamed.WriteByte((System.Byte) val);
				incr(tally);
				val = in_Renamed.ReadByte();
			}
			in_Renamed.Close();
		}
		
		public static void  writeFromInputToOutput(System.IO.Stream in_Renamed, System.IO.Stream out_Renamed)
		{
			writeFromInputToOutput(in_Renamed, out_Renamed, null);
		}
		
		/// <summary> 
		/// Write the byte array to the output stream
		/// 
		/// </summary>
		/// <param name="bytes">
		/// </param>
		/// <param name="out">
		/// </param>
		/// <throws>  IOException </throws>
		public static void  writeToOutput(sbyte[] bytes, System.IO.Stream out_Renamed, long[] tally)
		{
			
			for (int i = 0; i < bytes.Length; i++)
			{
				out_Renamed.WriteByte((byte) bytes[i]);
				incr(tally);
			}
		}
		
		public static void  writeToOutput(sbyte[] bytes, System.IO.Stream out_Renamed)
		{
			writeToOutput(bytes, out_Renamed, null);
		}
		
		private static void  incr(long[] tally)
		{
			if (tally != null)
			{
				tally[0]++;
			}
		}
		
		/// <summary> 
		/// Read bytes from an input stream into a byte array then close the input
		/// stream
		/// 
		/// </summary>
		/// <param name="in">
		/// </param>
		/// <param name="len">
		/// </param>
		/// <returns>
		/// </returns>
		/// <throws>  IOException </throws>
		public static sbyte[] readFromStream(System.IO.Stream in_Renamed, int len)
		{
			
			sbyte[] data;
			int read;
			if (len >= 0)
			{
				data = new sbyte[len];
				read = 0;
				while (read < len)
				{
					int k = in_Renamed is org.javarosa.core.io.BufferedInputStream?((org.javarosa.core.io.BufferedInputStream) in_Renamed).read(data, read, len - read):SupportClass.ReadInput(in_Renamed, data, read, len - read);
					if (k == - 1)
						break;
					read += k;
				}
			}
			else
			{
				System.IO.MemoryStream buffer = new System.IO.MemoryStream();
				while (true)
				{
					int b = in_Renamed.ReadByte();
					if (b == - 1)
					{
						break;
					}
					buffer.WriteByte((System.Byte) b);
				}
				data = SupportClass.ToSByteArray(buffer.ToArray());
				read = data.Length;
			}
			
			if (len > 0 && read < len)
			{
				// System.out.println("WARNING: expected " + len + "!!");
				throw new System.SystemException("expected: " + len + " bytes but read " + read);
			}
			// replyS
			// System.out.println(new String(data, "UTF-8"));
			
			return data;
		}
	}
}