/// <summary> </summary>
using System;
namespace org.javarosa.xform.parse
{
	
	/// <summary> A Parser Reporter is provided to the XFormParser to receive
	/// warnings and errors from the parser. 
	/// 
	/// </summary>
	/// <author>  ctsims
	/// </author>
	public class XFormParserReporter
	{
		public const System.String TYPE_UNKNOWN_MARKUP = "markup";
		public const System.String TYPE_INVALID_STRUCTURE = "invalid-structure";
		public const System.String TYPE_ERROR_PRONE = "dangerous";
		public const System.String TYPE_TECHNICAL = "technical";
		protected internal const System.String TYPE_ERROR = "error";
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.PrintStream' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		internal System.IO.StreamWriter errorStream;
		
		public XFormParserReporter():this(temp_writer)
		{
			System.IO.StreamWriter temp_writer;
			temp_writer = new System.IO.StreamWriter(System.Console.OpenStandardError(), System.Console.Error.Encoding);
			temp_writer.AutoFlush = true;
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.PrintStream' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public XFormParserReporter(System.IO.StreamWriter errorStream)
		{
			this.errorStream = errorStream;
		}
		
		public virtual void  warning(System.String type, System.String message, System.String xmlLocation)
		{
			errorStream.WriteLine("XForm Parse Warning: " + message + (xmlLocation == null?"":xmlLocation));
		}
		
		public virtual void  error(System.String message)
		{
			errorStream.WriteLine("XForm Parse Error: " + message);
		}
	}
}