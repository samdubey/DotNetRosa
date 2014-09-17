/// <summary> </summary>
using System;
using CacheTable = org.javarosa.core.util.CacheTable;
//UPGRADE_TODO: The type 'org.kxml2.io.KXmlParser' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using KXmlParser = org.kxml2.io.KXmlParser;
namespace org.javarosa.xform.util
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class InterningKXmlParser:KXmlParser
	{
		public InterningKXmlParser()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			base();
			this.stringCache = stringCache;
		}
		virtual public System.String Text
		{
			/* (non-Javadoc)
			* @see org.kxml2.io.KXmlParser#getText()
			*/
			
			get
			{
				return stringCache.intern(base.Text);
			}
			
		}
		virtual public System.String Name
		{
			get
			{
				return stringCache.intern(base.Name);
			}
			
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		CacheTable < String > stringCache;
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public InterningKXmlParser(CacheTable < String > stringCache)
		public virtual void  release()
		{
			//Anything?
		}
		
		/* (non-Javadoc)
		* @see org.kxml2.io.KXmlParser#getAttributeName(int)
		*/
		public virtual System.String getAttributeName(int arg0)
		{
			return stringCache.intern(base.getAttributeName(arg0));
		}
		
		/* (non-Javadoc)
		* @see org.kxml2.io.KXmlParser#getAttributeNamespace(int)
		*/
		public virtual System.String getAttributeNamespace(int arg0)
		{
			return stringCache.intern(base.getAttributeNamespace(arg0));
		}
		
		/* (non-Javadoc)
		* @see org.kxml2.io.KXmlParser#getAttributePrefix(int)
		*/
		public virtual System.String getAttributePrefix(int arg0)
		{
			return stringCache.intern(base.getAttributePrefix(arg0));
		}
		
		/* (non-Javadoc)
		* @see org.kxml2.io.KXmlParser#getAttributeValue(int)
		*/
		public virtual System.String getAttributeValue(int arg0)
		{
			return stringCache.intern(base.getAttributeValue(arg0));
		}
		
		/* (non-Javadoc)
		* @see org.kxml2.io.KXmlParser#getNamespace(java.lang.String)
		*/
		public virtual System.String getNamespace(System.String arg0)
		{
			return stringCache.intern(base.getNamespace(arg0));
		}
		
		/* (non-Javadoc)
		* @see org.kxml2.io.KXmlParser#getNamespaceUri(int)
		*/
		public virtual System.String getNamespaceUri(int arg0)
		{
			return stringCache.intern(base.getNamespaceUri(arg0));
		}
	}
}