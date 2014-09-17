/// <summary> </summary>
using System;
namespace org.javarosa.core.reference
{
	
	/// <summary> An invalid reference exception is thrown whenever
	/// a URI string cannot be resolved to a reference in
	/// the current environment. Just because an invalid 
	/// reference exception is not thrown does not mean
	/// that there is a binary data blob at the created
	/// reference, only that it has meaning and could refer
	/// to something in the current environment.
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	[Serializable]
	public class InvalidReferenceException:System.Exception
	{
		virtual public System.String ReferenceString
		{
			get
			{
				return reference;
			}
			
		}
		private System.String reference;
		
		/// <summary> A new exception implying that a URI could not be resolved to
		/// a reference.
		/// </summary>
		/// <param name="message">The failure message for why the URI could not be
		/// resolved.
		/// </param>
		/// <param name="reference">The URI which was unable to be resolved.
		/// </param>
		public InvalidReferenceException(System.String message, System.String reference):base(message)
		{
			this.reference = reference;
		}
	}
}