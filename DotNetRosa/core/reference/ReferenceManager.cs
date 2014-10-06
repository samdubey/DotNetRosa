/// <summary> </summary>
using System;
namespace org.javarosa.core.reference
{
	
	
	/// <summary> <p>The reference manager is a singleton class which
	/// is responsible for deriving reference URI's into
	/// references at runtime.</p>
	/// 
	/// <p>Raw reference factories
	/// (which are capable of actually creating fully
	/// qualified reference objects) are added with the
	/// addFactory() method. The most common method
	/// of doing so is to implement the PrefixedRootFactory
	/// as either a full class, or an anonymous inner class,
	/// providing the roots available in the current environment
	/// and the code for constructing a reference from them.</p>
	/// 
	/// <p>RootTranslators (which rely on other factories) are
	/// used to describe that a particular reference style (generally
	/// a high level reference like "jr://media/" or "jr://images/"
	/// should be translated to another available reference in this
	/// environment like "jr://file/". Root Translators do not
	/// directly derive references, but rather translate them to what
	/// the reference should look like in the current circumstances.</p>
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public class ReferenceManager
	{
		/// <returns> The available reference factories
		/// </returns>
		virtual public ReferenceFactory[] Factories
		{
			get
			{
				ReferenceFactory[] roots = new ReferenceFactory[translators.size()];
				translators.copyInto(roots);
				return roots;
			}
			
		}
		
		private static ReferenceManager instance;
		
		
		private List< RootTranslator > translators;
		
		private List< ReferenceFactory > factories;
		
		private List< RootTranslator > sessionTranslators;
		
		private ReferenceManager()
		{
			
			translators = new List< RootTranslator >();
			
			factories = new List< ReferenceFactory >();
			
			sessionTranslators = new List< RootTranslator >();
		}
		
		/// <returns> Singleton accessor to the global
		/// ReferenceManager.
		/// </returns>
		public static ReferenceManager _()
		{
			if (instance == null)
			{
				instance = new ReferenceManager();
			}
			return instance;
		}
		
		/// <summary> Adds a new Translator to the current environment.</summary>
		/// <param name="translator">
		/// </param>
		public virtual void  addRootTranslator(RootTranslator translator)
		{
			if (!translators.contains(translator))
			{
				translators.addElement(translator);
			}
		}
		
		/// <summary> Adds a factory for deriving reference URI's into references</summary>
		/// <param name="factory">A raw ReferenceFactory capable of creating
		/// a reference.
		/// </param>
		public virtual void  addReferenceFactory(ReferenceFactory factory)
		{
			if (!factories.contains(factory))
			{
				factories.addElement(factory);
			}
		}
		
		public virtual bool removeReferenceFactory(ReferenceFactory factory)
		{
			return factories.removeElement(factory);
		}
		
		/// <summary> Derives a global reference from a URI in the current environment.
		/// 
		/// </summary>
		/// <param name="uri">The URI representing a global reference.
		/// </param>
		/// <returns> A reference which is identified by the provided URI.
		/// </returns>
		/// <throws>  InvalidReferenceException If the current reference could </throws>
		/// <summary> not be derived by the current environment
		/// </summary>
		public virtual Reference DeriveReference(System.String uri)
		{
			return DeriveReference(uri, (System.String) null);
		}
		
		/// <summary> Derives a reference from a URI in the current environment.
		/// 
		/// </summary>
		/// <param name="uri">The URI representing a reference.
		/// </param>
		/// <param name="context">A reference which provides context for any
		/// relative reference accessors.
		/// </param>
		/// <returns> A reference which is identified by the provided URI.
		/// </returns>
		/// <throws>  InvalidReferenceException If the current reference could </throws>
		/// <summary> not be derived by the current environment
		/// </summary>
		public virtual Reference DeriveReference(System.String uri, Reference context)
		{
			return DeriveReference(uri, context.getURI());
		}
		
		/// <summary> Derives a reference from a URI in the current environment.
		/// 
		/// </summary>
		/// <param name="uri">The URI representing a reference.
		/// </param>
		/// <param name="context">A reference URI which provides context for any
		/// relative reference accessors.
		/// </param>
		/// <returns> A reference which is identified by the provided URI.
		/// </returns>
		/// <throws>  InvalidReferenceException If the current reference could </throws>
		/// <summary> not be derived by the current environment, or if the context URI
		/// is not valid in the current environment.
		/// </summary>
		public virtual Reference DeriveReference(System.String uri, System.String context)
		{
			if (uri == null)
			{
				throw new InvalidReferenceException("Null references aren't valid", uri);
			}
			
			//Relative URI's need to determine their context first.
			if (isRelative(uri))
			{
				//Clean up the relative reference to lack any leading separators.
				if (uri.StartsWith("./"))
				{
					uri = uri.Substring(2);
				}
				
				if (context == null)
				{
					throw new System.SystemException("Attempted to retrieve local reference with no context");
				}
				else
				{
					return derivingRoot(context).derive(uri, context);
				}
			}
			else
			{
				return derivingRoot(uri).derive(uri);
			}
		}
		
		/// <summary> Adds a root translator that is maintained over the course of a session. It will be globally
		/// available until the session is cleared using the "clearSession" method.
		/// 
		/// </summary>
		/// <param name="translator">A Root Translator that will be added to the current session
		/// </param>
		public virtual void  addSessionRootTranslator(RootTranslator translator)
		{
			sessionTranslators.addElement(translator);
		}
		
		/// <summary> Wipes out all of the translators being maintained in the current session (IE: Any translators
		/// added via "addSessionRootTranslator". Used to manage a temporary set of translations for a limited
		/// amount of time.
		/// </summary>
		public virtual void  clearSession()
		{
			sessionTranslators.removeAllElements();
		}
		
		private ReferenceFactory derivingRoot(System.String uri)
		{
			
			//First, try any/all roots which are put in the temporary session stack
			
			for(RootTranslator root: sessionTranslators)
			{
				if (root.derives(uri))
				{
					return root;
				}
			}
			
			//Now, try any/all roots referenced at runtime.
			
			for(RootTranslator root: translators)
			{
				if (root.derives(uri))
				{
					return root;
				}
			}
			
			//Now try all of the raw connectors available
			
			for(ReferenceFactory root: factories)
			{
				if (root.derives(uri))
				{
					return root;
				}
			}
			
			throw new InvalidReferenceException(getPrettyPrintException(uri), uri);
		}
		
		private System.String getPrettyPrintException(System.String uri)
		{
			if ((System.Object) uri == (System.Object) "")
			{
				return "Attempt to derive a blank reference";
			}
			try
			{
				System.String uriRoot = uri;
				System.String jrRefMessagePortion = "reference type";
				if (uri.IndexOf("jr://") != - 1)
				{
					uriRoot = uri.Substring("jr://".Length);
					jrRefMessagePortion = "javarosa jr:// reference root";
				}
				//For http:// style uri's
				int endOfRoot = uriRoot.IndexOf("://") + "://".Length;
				if (endOfRoot == "://".Length - 1)
				{
					endOfRoot = uriRoot.IndexOf("/");
				}
				if (endOfRoot != - 1)
				{
					uriRoot = uriRoot.Substring(0, (endOfRoot) - (0));
				}
				System.String message = "The reference \"" + uri + "\" was invalid and couldn't be understood. The " + jrRefMessagePortion + " \"" + uriRoot + "\" is not available on this system and may have been mis-typed. Some available roots: ";
				
				for(RootTranslator root: sessionTranslators)
				{
					message += ("\n" + root.prefix);
				}
				
				//Now, try any/all roots referenced at runtime.
				
				for(RootTranslator root: translators)
				{
					message += ("\n" + root.prefix);
				}
				
				//Now try all of the raw connectors available
				
				for(ReferenceFactory root: factories)
				{
					
					//TODO: Skeeeeeeeeeeeeetch
					try
					{
						
						if (root is PrefixedRootFactory)
						{
							
							for(String rootName:((PrefixedRootFactory) root).roots)
							{
								message += ("\n" + rootName);
							}
						}
						else
						{
							message += ("\n" + root.derive("").getURI());
						}
					}
					catch (System.Exception e)
					{
						
					}
				}
				return message;
			}
			catch (System.Exception e)
			{
				return "Couldn't process the reference " + uri + " . It may have been entered incorrectly. " + "Note that this doesn't mean that this doesn't mean the file or location referenced " + "couldn't be found, the reference itself was not understood.";
			}
		}
		
		/// <param name="URI">
		/// </param>
		/// <returns> Whether the provided URI describe a relative reference.
		/// </returns>
		public static bool isRelative(System.String URI)
		{
			if (URI.StartsWith("./"))
			{
				return true;
			}
			return false;
		}
	}
}