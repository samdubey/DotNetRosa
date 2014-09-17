/// <summary> </summary>
using System;
using org.javarosa.core.util.externalizable;
namespace org.javarosa.core.model
{
	
	/// <summary> A Submission Profile is a class which is responsible for
	/// holding and processing the details of how a submission
	/// should be handled.
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public class SubmissionProfile
	{
		private void  InitBlock()
		{
			this.method = method;
			this.ref_Renamed = ref_Renamed;
			this.action = action;
			this.mediaType = mediatype;
			this.attributeMap = attributeMap;
		}
		virtual public IDataReference Ref
		{
			get
			{
				return ref_Renamed;
			}
			
		}
		virtual public System.String Method
		{
			get
			{
				return method;
			}
			
		}
		virtual public System.String Action
		{
			get
			{
				return action;
			}
			
		}
		virtual public System.String MediaType
		{
			get
			{
				return mediaType;
			}
			
		}
		
		internal IDataReference ref_Renamed;
		internal System.String method;
		internal System.String action;
		internal System.String mediaType;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		HashMap < String, String > attributeMap;
		
		public SubmissionProfile()
		{
			InitBlock();
		}
		
		public SubmissionProfile(IDataReference ref_Renamed, System.String method, System.String action, System.String mediatype)
		{
			InitBlock();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			this(ref, method, action, mediatype, new HashMap < String, String >());
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public SubmissionProfile(IDataReference ref, String method, String action, String mediatype, HashMap < String, String > attributeMap)
		
		public virtual System.String getAttribute(System.String name)
		{
			return attributeMap.get_Renamed(name);
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		SuppressWarnings(unchecked)
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			ref_Renamed = (IDataReference) ExtUtil.read(in_Renamed, new ExtWrapTagged(typeof(IDataReference)));
			method = ExtUtil.readString(in_Renamed);
			action = ExtUtil.nullIfEmpty(ExtUtil.readString(in_Renamed));
			mediaType = ExtUtil.nullIfEmpty(ExtUtil.readString(in_Renamed));
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			attributeMap =(HashMap < String, String >) ExtUtil.read(in, new ExtWrapMap(String.
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		class, String.
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		class));
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void writeExternal(DataOutputStream out) throws IOException
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		ExtUtil.write(out, new ExtWrapTagged(ref));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.writeString(out, method);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.writeString(out, ExtUtil.emptyIfNull(action));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.writeString(out, ExtUtil.emptyIfNull(mediaType));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(out, new ExtWrapMap(attributeMap));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
}