/// <summary> </summary>
using System;
namespace org.javarosa.core.reference
{
	
	/// <summary> A Resource Reference is a reference to a file which 
	/// is a Java Resource, which is accessible with the
	/// 'getResourceAsStream' method from the Java Classloader.
	/// 
	/// Resource References are read only, and can identify with
	/// certainty whether a binary file is located at them. 
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public class ResourceReference : Reference
	{
		virtual public System.IO.Stream Stream
		{
			/*
			* (non-Javadoc)
			* @see org.javarosa.core.reference.Reference#getStream()
			*/
			
			get
			{
				//UPGRADE_ISSUE: Method 'java.lang.Class.getResourceAsStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetResourceAsStream_javalangString'"
				//UPGRADE_ISSUE: Class 'java.lang.System' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
				System.IO.Stream is_Renamed = typeof(System_Renamed).getResourceAsStream(URI);
				return is_Renamed;
			}
			
		}
		virtual public bool ReadOnly
		{
			/*
			* (non-Javadoc)
			* @see org.javarosa.core.reference.Reference#isReadOnly()
			*/
			
			get
			{
				return true;
			}
			
		}
		virtual public System.IO.Stream OutputStream
		{
			/*
			* (non-Javadoc)
			* @see org.javarosa.core.reference.Reference#getOutputStream()
			*/
			
			get
			{
				throw new System.IO.IOException("Resource references are read-only URI's");
			}
			
		}
		virtual public System.String LocalURI
		{
			get
			{
				return URI;
			}
			
		}
		
		internal System.String URI;
		
		/// <summary> Creates a new resource reference with URI in the format
		/// of a fully global resource URI, IE: "/path/file.ext".
		/// 
		/// </summary>
		/// <param name="URI">
		/// </param>
		public ResourceReference(System.String URI)
		{
			this.URI = URI;
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.reference.Reference#doesBinaryExist()
		*/
		public virtual bool doesBinaryExist()
		{
			//Figure out if there's a file by trying to open
			//a stream to it and determining if it's null.
			//UPGRADE_ISSUE: Method 'java.lang.Class.getResourceAsStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetResourceAsStream_javalangString'"
			//UPGRADE_ISSUE: Class 'java.lang.System' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			System.IO.Stream is_Renamed = typeof(System_Renamed).getResourceAsStream(URI);
			if (is_Renamed == null)
			{
				return false;
			}
			else
			{
				is_Renamed.Close();
				return true;
			}
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.reference.Reference#getURI()
		*/
		public virtual System.String getURI()
		{
			return "jr://" + "resource" + this.URI;
		}
		
		/*
		* (non-Javadoc)
		* @see java.lang.Object#equals(java.lang.Object)
		*/
		public  override bool Equals(System.Object o)
		{
			if (o is ResourceReference)
			{
				return URI.Equals(((ResourceReference) o).URI);
			}
			else
			{
				return false;
			}
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.reference.Reference#remove()
		*/
		public virtual void  remove()
		{
			throw new System.IO.IOException("Resource references are read-only URI's");
		}
		
		public virtual Reference[] probeAlternativeReferences()
		{
			//We can't poll the JAR for resources, unfortunately. It's possible
			//we could try to figure out something about the file and poll alternatives
			//based on type (PNG-> JPG, etc)
			return new Reference[0];
		}
		//UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}