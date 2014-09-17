/// <summary> </summary>
using System;
namespace org.javarosa.core.io
{
	
	/// <summary> An implementation of a Buffered Stream for j2me compatible libraries.
	/// 
	/// Very basic, no mark support (Pretty much only for web related streams
	/// anyway).
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public class BufferedInputStream:System.IO.Stream
	{
		
		//TODO: Better close semantics
		//TODO: Threadsafety
		
		private System.IO.Stream in_Renamed;
		private sbyte[] buffer;
		
		private int position;
		private int count;
		
		public BufferedInputStream(System.IO.Stream in_Renamed):this(in_Renamed, 2048)
		{
		}
		
		public BufferedInputStream(System.IO.Stream in_Renamed, int size)
		{
			this.in_Renamed = in_Renamed;
			this.buffer = new sbyte[size];
			cleanBuffer();
		}
		
		private void  cleanBuffer()
		{
			this.position = 0;
			this.count = 0;
		}
		
		
		/* (non-Javadoc)
		* @see java.io.InputStream#available()
		*/
		//UPGRADE_NOTE: The equivalent of method 'java.io.InputStream.available' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public int available()
		{
			if (count == - 1)
			{
				return 0;
			}
			//Size of our stream + the number of bytes we haven't yet read.
			long available;
			available = in_Renamed.Length - in_Renamed.Position;
			return ((in_Renamed is org.javarosa.core.util.MultiInputStream || in_Renamed is org.javarosa.core.io.BufferedInputStream)?(int) SupportClass.InvokeMethodAsVirtual(in_Renamed, "available", new System.Object[]{}):(int) available) + (count - position);
		}
		
		/* (non-Javadoc)
		* @see java.io.InputStream#close()
		*/
		public override void  Close()
		{
			in_Renamed.Close();
			//clear up buffer
			buffer = null;
		}
		
		/* (non-Javadoc)
		* @see java.io.InputStream#mark(int)
		*/
		//UPGRADE_NOTE: The equivalent of method 'java.io.InputStream.mark' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public void  mark(int arg0)
		{
			//nothing
		}
		
		/* (non-Javadoc)
		* @see java.io.InputStream#markSupported()
		*/
		//UPGRADE_NOTE: The equivalent of method 'java.io.InputStream.markSupported' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public bool markSupported()
		{
			return false;
		}
		
		/* (non-Javadoc)
		* @see java.io.InputStream#read(byte[], int, int)
		*/
		//UPGRADE_NOTE: The equivalent of method 'java.io.InputStream.read' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public int read(sbyte[] b, int off, int len)
		{
			//If we've reached EOF, signal that.
			if (count == - 1)
			{
				return - 1;
			}
			
			if (len == 0)
			{
				return 0;
			}
			
			if (off == - 1 || len == - 1 || off + len > b.Length)
			{
				throw new System.IndexOutOfRangeException("Bad inputs to input stream read");
			}
			
			int counter = 0;
			bool quitEarly = false;
			while (counter != len && !quitEarly)
			{
				//TODO: System.arraycopy here?
				for (; position < count && counter < len; ++position)
				{
					b[off + counter] = buffer[position];
					counter++;
				}
				
				//we read in as much as was requested;
				if (counter == len)
				{
					//don't need to do anything. We'll get bumped out of the loop
				}
				else if (position == count)
				{
					
					//If we didn't fill the buffer last time, we might be blocking on IO, so return
					//what we have and let the magic happen
					if (quitEarly)
					{
						continue;
					}
					
					//otherwise, try to fill that buffer 
					if (!fillBuffer())
					{
						//Ok, so we didn't fill the whole thing. Either we're at the end of our stream (possible)
						//or there was an incomplete read.
						
						//EOF
						if (count == - 1)
						{
							//We're at EOF. Two possible conditions here.
							
							//1) This was actually our first attempt on the end of stream. signal EOF 
							if (counter == 0)
							{
								return - 1;
							}
							//2) This was the last pile of bits. Return the ones we read.
							else
							{
								return counter;
							}
						}
						
						//Incomplete read. Get the bits back. Hopefully the stream won't be blocked next time they try to read.  
						quitEarly = true;
					}
				}
			}
			return counter;
		}
		
		private bool fillBuffer()
		{
			if (count == - 1)
			{
				//do nothing
				return false;
			}
			position = 0;
			count = in_Renamed is org.javarosa.core.io.BufferedInputStream?((org.javarosa.core.io.BufferedInputStream) in_Renamed).read(buffer):SupportClass.ReadInput(in_Renamed, buffer, 0, buffer.Length);
			return count == buffer.Length;
		}
		
		/* (non-Javadoc)
		* @see java.io.InputStream#read(byte[])
		*/
		//UPGRADE_NOTE: The equivalent of method 'java.io.InputStream.read' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public int read(sbyte[] b)
		{
			return this.read(b, 0, b.Length);
		}
		
		/* (non-Javadoc)
		* @see java.io.InputStream#reset()
		*/
		//UPGRADE_NOTE: The equivalent of method 'java.io.InputStream.reset' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public void  reset()
		{
			//mark is unsupported
		}
		
		/* (non-Javadoc)
		* @see java.io.InputStream#skip(long)
		*/
		//UPGRADE_NOTE: The equivalent of method 'java.io.InputStream.skip' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public long skip(long len)
		{
			//TODO: Something smarter here?
			System.IO.Stream temp_Stream;
			System.Int64 temp_Int64;
			temp_Stream = in_Renamed;
			temp_Int64 = temp_Stream.Position;
			temp_Int64 = temp_Stream.Seek(len, System.IO.SeekOrigin.Current) - temp_Int64;
			long skipped = in_Renamed is org.javarosa.core.io.BufferedInputStream?((org.javarosa.core.io.BufferedInputStream) in_Renamed).skip(len):temp_Int64;
			if (skipped > count - position)
			{
				//need to reset our buffer positions, this buffer
				//is now expired.
				cleanBuffer();
			}
			else
			{
				//we skipped some number of bytes that just pushes us further
				//into the existing buffer
				
				//this has to be an integer-bound size, because it's smaller than
				//count - position, which is an integer size
				int bytesSkipped = (int) skipped;
				position += bytesSkipped;
			}
			return skipped;
		}
		
		/* (non-Javadoc)
		* @see java.io.InputStream#read()
		*/
		public override int ReadByte()
		{
			//If we've read all of the available buffer, fill
			//'er up.
			if (position == count)
			{
				//This has to return at _least_ 1 byte, unless
				//the stream has ended
				fillBuffer();
			}
			
			//either this was true when we got here, or it's true
			//now. Either way, signal EOF
			if (count == - 1)
			{
				return - 1;
			}
			
			//Otherwise, bump and return
			return buffer[position++] & 0xFF;
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		public override void  Flush()
		{
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		public override System.Int64 Seek(System.Int64 offset, System.IO.SeekOrigin origin)
		{
			return 0;
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		public override void  SetLength(System.Int64 value)
		{
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		public override System.Int32 Read(System.Byte[] buffer, System.Int32 offset, System.Int32 count)
		{
			return 0;
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		public override void  Write(System.Byte[] buffer, System.Int32 offset, System.Int32 count)
		{
		}
		//UPGRADE_TODO: The following property was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		public override System.Boolean CanRead
		{
			get
			{
				return false;
			}
			
		}
		//UPGRADE_TODO: The following property was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		public override System.Boolean CanSeek
		{
			get
			{
				return false;
			}
			
		}
		//UPGRADE_TODO: The following property was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		public override System.Boolean CanWrite
		{
			get
			{
				return false;
			}
			
		}
		//UPGRADE_TODO: The following property was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		public override System.Int64 Length
		{
			get
			{
				return 0;
			}
			
		}
		//UPGRADE_TODO: The following property was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		public override System.Int64 Position
		{
			get
			{
				return 0;
			}
			
			set
			{
			}
			
		}
	}
}