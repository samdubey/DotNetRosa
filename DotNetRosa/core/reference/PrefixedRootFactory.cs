/// <summary> </summary>
using System;
namespace org.javarosa.core.reference
{
	
	/// <summary> PrefixedRootFactory provides a clean way to implement
	/// the vast majority of behavior for a reference factory.
	/// 
	/// A PrefixedRootFactory defines a set of roots which it can
	/// provide references for. Roots can either be a full URI root
	/// like "http://", or "file://", or can be local roots like
	/// "resource" or "file", in which case the assumed protocol
	/// parent will be "jr://". 
	/// 
	/// For example: a PrefixedRootFactory with the roots "media"
	/// and "resource" will be used by the ReferenceManager to derive
	/// any URI with the roots
	/// <ul><li>jr://media/</li><li>jr://resource/</li></ul> like 
	/// <pre>jr://media/checkmark.png</pre>
	/// and a PrefixedRootFactory with roots "file" and "file://" will
	/// be used by the ReferenceManager to derive any URI with the roots
	/// <ul><li>jr://file/</li><li>file://</li></ul> like 
	/// <pre>jr://file/myxform.xhtml</pre> or <pre>file://myxform.xhtml</pre>
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public abstract class PrefixedRootFactory : ReferenceFactory
	{
		
		internal System.String[] roots;
		
		/// <summary> Construct a PrefixedRootFactory which handles the roots 
		/// provided. Implementing subclasses will actually do the
		/// root construction by implementing the "factory()" method.
		/// 
		/// </summary>
		/// <param name="roots">The roots of URI's which should be derived by
		/// this factory.
		/// </param>
		public PrefixedRootFactory(System.String[] roots)
		{
			this.roots = new System.String[roots.Length];
			for (int i = 0; i < this.roots.Length; ++i)
			{
				if (roots[i].IndexOf("://") != - 1)
				{
					//If this is the kind of root which is handling non-jr
					//stuff, we should leave it alone.
					this.roots[i] = roots[i];
				}
				else
				{
					//Otherwise it's just floating, so we should clearly
					//append our jr root for now.
					this.roots[i] = "jr://" + roots[i];
				}
			}
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.reference.ReferenceFactory#derive(java.lang.String)
		*/
		public virtual Reference derive(System.String URI)
		{
			
			for(String root: roots)
			{
				if (URI.indexOf(root) != - 1)
				{
					return factory(URI.substring(root.length()), URI);
				}
			}
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			throw new InvalidReferenceException("Invalid attempt to derive a reference from a prefixed root. Valid prefixes for this factory are " + roots, URI);
		}
		
		/// <summary> Creates a Reference using the most locally available part of a 
		/// URI. 
		/// 
		/// </summary>
		/// <param name="terminal">The local part of the URI for this prefixed root
		/// (excluding the root itself)
		/// </param>
		/// <param name="URI">The full URI 
		/// </param>
		/// <returns> A reference which describes the URI 
		/// </returns>
		protected internal abstract Reference factory(System.String terminal, System.String URI);
		
		/* (non-Javadoc)
		* @see org.javarosa.core.reference.ReferenceFactory#derive(java.lang.String, java.lang.String)
		*/
		public virtual Reference derive(System.String URI, System.String context)
		{
			System.String referenceURI = context.Substring(0, (context.LastIndexOf('/') + 1) - (0)) + URI;
			return ReferenceManager._().DeriveReference(referenceURI);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.reference.ReferenceFactory#derives(java.lang.String)
		*/
		public virtual bool derives(System.String URI)
		{
			
			for(String root: roots)
			{
				if (URI.indexOf(root) != - 1)
				{
					return true;
				}
			}
			return false;
		}
	}
}