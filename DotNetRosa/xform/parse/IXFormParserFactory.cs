using System;
//UPGRADE_TODO: The type 'org.kxml2.kdom.Document' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Document = org.kxml2.kdom.Document;
namespace org.javarosa.xform.parse
{
	
	/// <summary> Interface for class factory for creating an XFormParser.
	/// Supports experimental extensions of XFormParser.
	/// 
	/// </summary>
	/// <author>  mitchellsundt@gmail.com
	/// 
	/// </author>
	public interface IXFormParserFactory
	{
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		XFormParser getXFormParser(System.IO.StreamReader reader);
		
		XFormParser getXFormParser(Document doc);
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		XFormParser getXFormParser(System.IO.StreamReader form, System.IO.StreamReader instance);
		
		XFormParser getXFormParser(Document form, Document instance);
	}
}