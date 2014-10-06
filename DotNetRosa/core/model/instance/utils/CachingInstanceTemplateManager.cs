using System;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
namespace org.javarosa.core.model.instance.utils
{
	
	/// <summary> Instance template manager that caches the template instances in memory. Useful for when deserializing
	/// many saved forms of the same form type at once.
	/// 
	/// Instance templates are lazily loaded into the cache upon the first request for the instance of that type.
	/// 
	/// Instances stay cached until explicitly cleared.
	/// 
	/// Keeping too many FormInstances cached at once may exhaust your memory. It's best if all saved forms
	/// being deserialized in bulk belong to a set of a few, known form types. It is possible to explicitly
	/// set the allowed form types, such that any attempt to deserialize a form of a different type will throw
	/// an error, instead of caching the new instance template.
	/// 
	/// </summary>
	/// <author>  Drew Roos
	/// 
	/// </author>
	public class CachingInstanceTemplateManager : InstanceTemplateManager
	{
		
		
		private HashMap < Integer, FormInstance > templateCache;
		
		private List< Integer > allowedFormTypes;
		private bool restrictFormTypes;
		
		public CachingInstanceTemplateManager():this(true)
		{
		}
		
		/// <summary> </summary>
		/// <param name="restrictFormTypes">if true, only allowed form types will be cached; any requests for the templates
		/// for other form types will throw an error; the list of allowed types starts out empty; register allowed
		/// form types with addFormType(). if false, all form types will be handled and cached
		/// </param>
		public CachingInstanceTemplateManager(bool restrictFormTypes)
		{
			
			this.templateCache = new HashMap < Integer, FormInstance >();
			this.restrictFormTypes = restrictFormTypes;
			
			this.allowedFormTypes = new List< Integer >();
		}
		
		/// <summary> Remove all model templates from the cache. Frees up memory.</summary>
		public virtual void  clearCache()
		{
			templateCache.clear();
		}
		
		/// <summary> Set a form type as allowed for caching. Only has an effect if this class has been set to restrict form types</summary>
		/// <param name="formID">
		/// </param>
		public virtual void  addFormType(int formID)
		{
			if (!allowedFormTypes.contains((System.Int32) formID))
			{
				allowedFormTypes.addElement((System.Int32) formID);
			}
		}
		
		/// <summary> Empty the list of allowed form types</summary>
		public virtual void  resetFormTypes()
		{
			allowedFormTypes.removeAllElements();
		}
		
		/// <summary> Return the template model for the given form type. Serves the template out of the cache, if cached; fetches it
		/// fresh and caches it otherwise. If form types are restricted and the given form type is not allowed, throw an error
		/// </summary>
		public virtual FormInstance getTemplateInstance(int formID)
		{
			if (restrictFormTypes && !allowedFormTypes.contains((System.Int32) formID))
			{
				throw new System.SystemException("form ID [" + formID + "] is not an allowed form type!");
			}
			
			FormInstance template = templateCache.get_Renamed((System.Int32) formID);
			if (template == null)
			{
				template = CompactInstanceWrapper.loadTemplateInstance(formID);
				if (template == null)
				{
					throw new System.SystemException("no formdef found for form id [" + formID + "]");
				}
				templateCache.put((System.Int32) formID, template);
			}
			return template;
		}
	}
}