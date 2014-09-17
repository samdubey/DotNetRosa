using System;
namespace org.javarosa.core.io
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
			//TODO: God this is naive
			int val;
			try
			{
				val = in_Renamed.ReadByte();
			}
			catch (System.IO.IOException e)
			{
				throw new StreamsUtil.InputIOException(new StreamsUtil(), e);
			}
			while (val != - 1)
			{
				try
				{
					out_Renamed.WriteByte((System.Byte) val);
				}
				catch (System.IO.IOException e)
				{
					throw new StreamsUtil.OutputIOException(new StreamsUtil(), e);
				}
				incr(tally);
				try
				{
					val = in_Renamed.ReadByte();
				}
				catch (System.IO.IOException e)
				{
					throw new StreamsUtil.InputIOException(new StreamsUtil(), e);
				}
			}
		}
		
		public static void  writeFromInputToOutputSpecific(System.IO.Stream in_Renamed, System.IO.Stream out_Renamed)
		{
			writeFromInputToOutput(in_Renamed, out_Renamed, null);
		}
		
		public static void  writeFromInputToOutput(System.IO.Stream in_Renamed, System.IO.Stream out_Renamed)
		{
			try
			{
				writeFromInputToOutput(in_Renamed, out_Renamed, null);
			}
			catch (InputIOException e)
			{
				throw e.internal_Renamed;
			}
			catch (OutputIOException e)
			{
				throw e.internal_Renamed;
			}
		}
		
		private const int CHUNK_SIZE = 2048;
		
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
			int offset = 0;
			int remain = bytes.Length;
			
			while (remain > 0)
			{
				int toRead = (remain < CHUNK_SIZE)?remain:CHUNK_SIZE;
				out_Renamed.Write(SupportClass.ToByteArray(bytes), offset, toRead);
				remain -= toRead;
				offset += toRead;
				if (tally != null)
				{
					tally[0] += toRead;
				}
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
		
		
		//Unify the functional aspects here
		
		//UPGRADE_NOTE: The access modifier for this class or class field has been changed in order to prevent compilation errors due to the visibility level. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1296'"
		[Serializable]
		abstract public class DirectionalIOException:System.IO.IOException
		{
			virtual public System.IO.IOException Wrapped
			{
				get
				{
					return internal_Renamed;
				}
				
			}
			internal System.IO.IOException internal_Renamed;
			
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			public DirectionalIOException(System.IO.IOException internal_Renamed):base(internal_Renamed.Message)
			{
				this.internal_Renamed = internal_Renamed;
			}
			
			//UPGRADE_NOTE: The equivalent of method 'java.lang.Throwable.printStackTrace' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
			public void  printStackTrace()
			{
				if (internal_Renamed is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
					((org.javarosa.core.io.StreamsUtil.DirectionalIOException) internal_Renamed).printStackTrace();
				else
					SupportClass.WriteStackTrace(internal_Renamed, Console.Error);
			}
			
			//TODO: Override all common methodss
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'InputIOException' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		public class InputIOException:DirectionalIOException
		{
			private void  InitBlock(StreamsUtil enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private StreamsUtil enclosingInstance;
			public StreamsUtil Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public InputIOException(StreamsUtil enclosingInstance, System.IO.IOException internal_Renamed):base(internal_Renamed)
			{
				InitBlock(enclosingInstance);
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'OutputIOException' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		public class OutputIOException:DirectionalIOException
		{
			private void  InitBlock(StreamsUtil enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private StreamsUtil enclosingInstance;
			public StreamsUtil Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public OutputIOException(StreamsUtil enclosingInstance, System.IO.IOException internal_Renamed):base(internal_Renamed)
			{
				InitBlock(enclosingInstance);
			}
		}
	}
}