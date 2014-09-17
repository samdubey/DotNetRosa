/// <summary> </summary>
using System;
namespace org.javarosa.core.reference
{
	
	/// <summary> A Reference is essentially a pointer to interact in a limited
	/// fashion with an external resource of some kind (images, xforms,
	/// etc). 
	/// 
	/// References are retrieved from the ReferenceManager, which is
	/// responsible for turning different URI's (either normal http://,
	/// etc URI's or JavaRosa jr:// URI's) into a reference to an actual
	/// resource.
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public interface Reference
	{
		/// <returns> A Stream of data which is the binary resource's
		/// definition.
		/// 
		/// </returns>
		/// <throws>  IOException If there is a problem reading the </throws>
		/// <summary> stream.
		/// </summary>
		System.IO.Stream Stream
		{
			get;
			
		}
		/// <returns> A URI which may or may not exist in the local context
		/// which will resolves to this reference. This method should be
		/// used with caution: There is no guarantee that a local URI
		/// can be constructed or used in a general way.
		/// </returns>
		System.String LocalURI
		{
			get;
			
		}
		/// <returns> True if the remote data is only available to
		/// be read from (using getStream), False if the remote
		/// data can also be modified or written to.
		/// </returns>
		bool ReadOnly
		{
			get;
			
		}
		/// <returns> A stream which can be written to at the
		/// reference location to define the binary content there.
		/// 
		/// </returns>
		/// <throws>  IOException If there is a problem writing or the </throws>
		/// <summary> reference is read only
		/// </summary>
		System.IO.Stream OutputStream
		{
			//Should possibly throw another type of exception here
			//for invalid reference operation (Read only)
			
			get;
			
		}
		/// <returns> True if the binary does (or might) exist at
		/// the remote location. False if the binary definitely
		/// does not exist.
		/// </returns>
		/// <throws>  IOException If there is a problem identifying </throws>
		/// <summary> the status of the resource
		/// </summary>
		bool doesBinaryExist();
		
		
		/// <returns> A URI which will evaluate to this same reference
		/// in the future.
		/// </returns>
		System.String getURI();
		
		/// <summary> Removes the binary data located by this reference.</summary>
		/// <throws>  IOException If there is a problem deleting or the </throws>
		/// <summary> reference is read only
		/// </summary>
		void  remove();
		
		/// <summary> Determines any platform-specific and reference-type specific
		/// alternatives versions of this reference which may exist. Useful
		/// when only certain media or references are available on a platform
		/// and you need to figure out whether a platform-specific version
		/// might be present.
		/// 
		/// NOTE: There is no guarantee that returned references will exist, 
		/// they should be tested.  
		/// </summary>
		Reference[] probeAlternativeReferences();
	}
}