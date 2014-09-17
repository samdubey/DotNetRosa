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
namespace org.javarosa.core.util
{
	
	/// <summary> MultiInputStream allows for concatenating multiple
	/// input streams together to be read serially in the
	/// order that they were added.
	/// 
	/// A MultiInputStream must have all of its component
	/// streams added to it before it can be read from. Once
	/// the stream is ready, it should be prepare()d before
	/// the first read.
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// </author>
	/// <date>  Dec 18, 2008  </date>
	/// <summary> 
	/// </summary>
	public class MultiInputStream:System.IO.Stream
	{
		
		/// <summary>InputStream *</summary>
		internal System.Collections.ArrayList streams = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
		
		internal int currentStream = - 1;
		
		public virtual void  addStream(System.IO.Stream stream)
		{
			streams.Add(stream);
		}
		
		/// <summary> Finalize the stream and allow it to be read
		/// from.
		/// 
		/// </summary>
		/// <returns> True if the stream is ready to be read
		/// from. False if the stream couldn't be prepared
		/// because it was empty.
		/// </returns>
		public virtual bool prepare()
		{
			if (streams.Count == 0)
			{
				return false;
			}
			else
			{
				currentStream = 0;
			}
			return true;
		}
		
		/* (non-Javadoc)
		* @see java.io.InputStream#read()
		*/
		public override int ReadByte()
		{
			if (currentStream == - 1)
			{
				throw new System.IO.IOException("Cannot read from unprepared MultiInputStream!");
			}
			System.IO.Stream cur = ((System.IO.Stream) streams[currentStream]);
			int next = cur.ReadByte();
			
			if (next != - 1)
			{
				return next;
			}
			
			//Otherwise, end of Stream
			
			//Loop through the available streams until we read something that isn't 
			//an end of stream
			while (next == - 1 && currentStream + 1 < streams.Count)
			{
				currentStream++;
				cur = ((System.IO.Stream) streams[currentStream]);
				next = cur.ReadByte();
			}
			
			//Will be either a valid value or -1 if we've run out of streams.
			return next;
		}
		
		/* (non-Javadoc)
		* @see java.io.InputStream#available()
		*/
		//UPGRADE_NOTE: The equivalent of method 'java.io.InputStream.available' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public int available()
		{
			if (currentStream == - 1)
			{
				throw new System.IO.IOException("Cannot read from unprepared MultiInputStream!");
			}
			long available;
			available = ((System.IO.Stream) streams[currentStream]).Length - ((System.IO.Stream) streams[currentStream]).Position;
			return (int) available;
		}
		
		/* (non-Javadoc)
		* @see java.io.InputStream#close()
		*/
		public override void  Close()
		{
			if (currentStream == - 1)
			{
				throw new System.IO.IOException("Cannot read from unprepared MultiInputStream!");
			}
			System.Collections.IEnumerator en = streams.GetEnumerator();
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			while (en.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				((System.IO.Stream) en.Current).Close();
			}
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