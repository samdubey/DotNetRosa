/// <summary> </summary>
using System;
namespace org.javarosa.core.reference
{
	
	/// <summary> A ResourceReferenceFactory is a Raw Reference Accessor
	/// which provides a factory for references of the form
	/// <pre>jr://resource/</pre>.
	/// 
	/// TODO: Configure this factory to also work for raw resource
	/// accessors like "/something".
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public class ResourceReferenceFactory:PrefixedRootFactory
	{
		
		public ResourceReferenceFactory():base(new System.String[]{"resource"})
		{
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.reference.PrefixedRootFactory#factory(java.lang.String, java.lang.String)
		*/
		protected internal override Reference factory(System.String terminal, System.String URI)
		{
			return new ResourceReference(terminal);
		}
	}
}