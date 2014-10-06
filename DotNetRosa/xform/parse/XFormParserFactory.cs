using System;
using CacheTable = org.javarosa.core.util.CacheTable;
//UPGRADE_TODO: The type 'org.kxml2.kdom.Document' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Document = org.kxml2.kdom.Document;
namespace org.javarosa.xform.parse
{
	
	/// <summary> Class factory for creating an XFormParser.
	/// 
	/// This factory allows you to provide a custom string cache
	/// to be used during parsing, which should be helpful
	/// in conserving memories in environments where there might be
	/// multiple parsed forms in memory at the same time.
	/// 
	/// </summary>
	/// <author>  mitchellsundt@gmail.com / csims@dimagi.com
	/// 
	/// </author>
	public class XFormParserFactory : IXFormParserFactory
	{
		private void  InitBlock()
		{
			this.stringCache = stringCache;
		}
		
		CacheTable < String > stringCache;
		
		public XFormParserFactory()
		{
			InitBlock();
		}
		
		
		public XFormParserFactory(CacheTable < String > stringCache)
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public virtual XFormParser getXFormParser(System.IO.StreamReader reader)
		{
			XFormParser parser = new XFormParser(reader);
			init(parser);
			return parser;
		}
		
		private void  init(XFormParser parser)
		{
			if (stringCache != null)
			{
				parser.setStringCache(stringCache);
			}
		}
		
		public virtual XFormParser getXFormParser(Document doc)
		{
			XFormParser parser = new XFormParser(doc);
			init(parser);
			return parser;
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public virtual XFormParser getXFormParser(System.IO.StreamReader form, System.IO.StreamReader instance)
		{
			XFormParser parser = new XFormParser(form, instance);
			init(parser);
			return parser;
		}
		
		public virtual XFormParser getXFormParser(Document form, Document instance)
		{
			XFormParser parser = new XFormParser(form, instance);
			init(parser);
			return parser;
		}
	}
}