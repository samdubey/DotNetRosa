using System;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
namespace org.javarosa.core.model.instance.utils
{
	
	/// <summary> Used by CompactInstanceWrapper to retrieve the template FormInstances (from the original FormDef)
	/// necessary to unambiguously deserialize the compact models
	/// 
	/// </summary>
	/// <author>  Drew Roos
	/// 
	/// </author>
	public interface InstanceTemplateManager
	{
		
		/// <summary> return FormInstance for the FormDef with the given form ID</summary>
		/// <param name="formID">
		/// </param>
		/// <returns>
		/// </returns>
		FormInstance getTemplateInstance(int formID);
	}
}