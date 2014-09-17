/// <summary> </summary>
using System;
using IDataReference = org.javarosa.core.model.IDataReference;
using SubmissionProfile = org.javarosa.core.model.SubmissionProfile;
//UPGRADE_TODO: The type 'org.kxml2.kdom.Element' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Element = org.kxml2.kdom.Element;
namespace org.javarosa.xform.parse
{
	
	/// <summary> A Submission Profile 
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public class SubmissionParser
	{
		
		public virtual SubmissionProfile parseSubmission(System.String method, System.String action, IDataReference ref_Renamed, Element element)
		{
			System.String mediatype = element.getAttributeValue(null, "mediatype");
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			HashMap < String, String > attributeMap = new HashMap < String, String >();
			int nAttr = element.getAttributeCount();
			for (int i = 0; i < nAttr; ++i)
			{
				System.String name = element.getAttributeName(i);
				if (name.Equals("ref"))
					continue;
				if (name.Equals("bind"))
					continue;
				if (name.Equals("method"))
					continue;
				if (name.Equals("action"))
					continue;
				System.String value_Renamed = element.getAttributeValue(i);
				attributeMap.put(name, value_Renamed);
			}
			return new SubmissionProfile(ref_Renamed, method, action, mediatype, attributeMap);
		}
		
		public virtual bool matchesCustomMethod(System.String method)
		{
			return false;
		}
	}
}