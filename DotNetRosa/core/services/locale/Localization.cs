using System;
using ReferenceDataSource = org.javarosa.core.reference.ReferenceDataSource;
namespace org.javarosa.core.services.locale
{
	
	public class Localization
	{
		public static Localizer GlobalLocalizerAdvanced
		{
			get
			{
				init(false);
				return globalLocalizer;
			}
			
		}
		public static System.String Locale
		{
			set
			{
				checkRep();
				globalLocalizer.Locale = value;
			}
			
		}
		public static System.String DefaultLocale
		{
			set
			{
				checkRep();
				globalLocalizer.DefaultLocale = value;
			}
			
		}
		public static Localizer LocalizationData
		{
			set
			{
				globalLocalizer = value;
			}
			
		}
		private static Localizer globalLocalizer;
		
		public static System.String get_Renamed(System.String key)
		{
			return get_Renamed(key, new System.String[]{});
		}
		
		public static System.String get_Renamed(System.String key, System.String[] args)
		{
			checkRep();
			return globalLocalizer.getText(key, args);
		}
		
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public static System.String get_Renamed(System.String key, System.Collections.Hashtable args)
		{
			checkRep();
			return globalLocalizer.getText(key, args);
		}
		
		public static void  registerLanguageFile(System.String localeName, System.String resourceFileURI)
		{
			init(false);
			if (!globalLocalizer.hasLocale(localeName))
			{
				globalLocalizer.addAvailableLocale(localeName);
			}
			globalLocalizer.registerLocaleResource(localeName, new ResourceFileDataSource(resourceFileURI));
			if (globalLocalizer.DefaultLocale == null)
			{
				globalLocalizer.DefaultLocale = localeName;
			}
		}
		
		public static void  registerLanguageReference(System.String localeName, System.String referenceUri)
		{
			init(false);
			if (!globalLocalizer.hasLocale(localeName))
			{
				globalLocalizer.addAvailableLocale(localeName);
			}
			globalLocalizer.registerLocaleResource(localeName, new ReferenceDataSource(referenceUri));
			if (globalLocalizer.DefaultLocale == null)
			{
				globalLocalizer.DefaultLocale = localeName;
			}
		}
		
		/// <summary> </summary>
		public static void  init(bool force)
		{
			if (globalLocalizer == null || force)
			{
				globalLocalizer = new Localizer(true, true);
			}
		}
		
		/// <summary> </summary>
		private static void  checkRep()
		{
			init(false);
			if (globalLocalizer.AvailableLocales.Length == 0)
			{
				throw new LocaleTextException("There are no locales defined for the application. Please make sure to register locale text using the Locale.register() method");
			}
		}
	}
}