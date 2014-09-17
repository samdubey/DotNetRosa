/*
* Copyright (C) 2009 JavaRosa
*
* Licensed under the Apache License, Version 2.0 (the "License"); you may not
* use this file except in compliance with the License. You may obtain a copy of
* the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
* WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
* License for the specific language governing permissions and limitations under
* the License.
*/
using System;
using NoLocalizedTextException = org.javarosa.core.util.NoLocalizedTextException;
using OrderedMap = org.javarosa.core.util.OrderedMap;
using PrefixTree = org.javarosa.core.util.PrefixTree;
using PrefixTreeNode = org.javarosa.core.util.PrefixTreeNode;
using UnregisteredLocaleException = org.javarosa.core.util.UnregisteredLocaleException;
using org.javarosa.core.util.externalizable;
namespace org.javarosa.core.services.locale
{
	
	/// <summary> The Localizer object maintains mappings for locale ID's and Object
	/// ID's to the String values associated with them in different
	/// locales.
	/// 
	/// </summary>
	/// <author>  Drew Roos/Clayton Sims
	/// 
	/// </author>
	public class Localizer
	{
		private void  InitBlock()
		{
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(String key: source.keySet())
			{
				destination.put(key, stringTree.addString(source.get_Renamed(key)));
			}
			if (locale == null || !this.locales.contains(locale))
			{
				return null;
			}
			stringTree.clear();
			
			//It's very important that any default locale contain the appropriate strings to localize the interface
			//for any possible language. As such, we'll keep around a table with only the default locale keys to
			//ensure that there are no localizations which are only present in another locale, which causes ugly
			//and difficult to trace errors.
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			OrderedMap < String, Boolean > defaultLocaleKeys = new OrderedMap < String, Boolean >();
			
			//This table will be loaded with the default values first (when applicable), and then with any
			//language specific translations overwriting the existing values.
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			OrderedMap < String, PrefixTreeNode > data = new OrderedMap < String, PrefixTreeNode >();
			
			// If there's a default locale, we load all of its elements into memory first, then allow
			// the current locale to overwrite any differences between the two.
			if (fallbackDefaultLocale && defaultLocale != null)
			{
				//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
				for (int i = 0; i < defaultResources.size(); ++i)
				{
					loadTable(data, ((LocaleDataSource) defaultResources.elementAt(i)).getLocalizedText());
				}
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				for(String key: data.keySet())
				{
					defaultLocaleKeys.put(key, true);
				}
			}
			
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			for (int i = 0; i < resources.size(); ++i)
			{
				loadTable(data, ((LocaleDataSource) resources.elementAt(i)).getLocalizedText());
			}
			
			//Strings are now immutable
			stringTree.seal();
			
			//If we're using a default locale, now we want to make sure that it has all of the keys
			//that the locale we want to use does. Otherwise, the app will crash when we switch to
			//a locale that doesn't contain the key.
			if (fallbackDefaultLocale && defaultLocale != null)
			{
				System.String missingKeys = "";
				int keysmissing = 0;
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				for(String key: data.keySet())
				{
					if (!defaultLocaleKeys.containsKey(key))
					{
						missingKeys += (key + ",");
						keysmissing++;
					}
				}
				if (keysmissing > 0)
				{
					//Is there a good way to localize these exceptions?
					throw new NoLocalizedTextException("Error loading locale " + locale + ". There were " + keysmissing + " keys which were contained in this locale, but were not " + "properly registered in the default Locale. Any keys which are added to a locale should always " + "be added to the default locale to ensure appropriate functioning.\n" + "The missing translations were for the keys: " + missingKeys, missingKeys, defaultLocale);
				}
			}
			
			return data;
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			OrderedMap < String, PrefixTreeNode > mapping = getLocaleData(locale);
			if (mapping == null)
				throw new UnregisteredLocaleException("Attempted to access an undefined locale.");
			return mapping;
			currentLocale = (System.String) ExtUtil.read(dis, new ExtWrapNullable(typeof(System.String)), pf);
			Locale = currentLocale;
		}
		/// <summary> Get default locale fallback mode
		/// 
		/// </summary>
		/// <returns> default locale fallback mode
		/// </returns>
		virtual public bool FallbackLocale
		{
			get
			{
				return fallbackDefaultLocale;
			}
			
		}
		/// <summary> Get default form fallback mode
		/// 
		/// </summary>
		/// <returns> default form fallback mode
		/// </returns>
		virtual public bool FallbackForm
		{
			get
			{
				return fallbackDefaultForm;
			}
			
		}
		/// <summary> Get a list of defined locales.
		/// 
		/// </summary>
		/// <returns> Array of defined locales, in order they were created.
		/// </returns>
		virtual public System.String[] AvailableLocales
		{
			get
			{
				System.String[] data = new System.String[locales.size()];
				locales.copyInto(data);
				return data;
			}
			
		}
		/// <summary> Return the next locale in order, for cycling through locales.
		/// 
		/// </summary>
		/// <returns> Next locale following the current locale (if the current locale is the last, cycle back to the beginning).
		/// If the current locale is not set, return the default locale. If the default locale is not set, return null.
		/// </returns>
		virtual public System.String NextLocale
		{
			get
			{
				return currentLocale == null?defaultLocale:locales.elementAt((locales.indexOf(currentLocale) + 1) % locales.size());
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Get the current locale.
		/// 
		/// </summary>
		/// <returns> Current locale.
		/// </returns>
		/// <summary> Set the current locale. The locale must be defined. Will notify all registered ILocalizables of the change in locale.
		/// 
		/// </summary>
		/// <param name="currentLocale">Locale. Must be defined and not null.
		/// </param>
		/// <throws>  UnregisteredLocaleException If locale is null or not defined. </throws>
		virtual public System.String Locale
		{
			get
			{
				return currentLocale;
			}
			
			set
			{
				if (!hasLocale(value))
				{
					throw new UnregisteredLocaleException("Attempted to set to a locale that is not defined. Attempted Locale: " + value);
				}
				
				bool alert = false;
				if (!value.Equals(this.currentLocale))
				{
					this.currentLocale = value;
					alert = true;
				}
				loadCurrentLocaleResources();
				if (alert)
				{
					alertLocalizables();
				}
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Get the default locale.
		/// 
		/// </summary>
		/// <returns> Default locale.
		/// </returns>
		/// <summary> Set the default locale. The locale must be defined.
		/// 
		/// </summary>
		/// <param name="defaultLocale">Default locale. Must be defined. May be null, in which case there will be no default locale.
		/// </param>
		/// <throws>  UnregisteredLocaleException If locale is not defined. </throws>
		virtual public System.String DefaultLocale
		{
			get
			{
				return defaultLocale;
			}
			
			set
			{
				if (value != null && !hasLocale(value))
					throw new UnregisteredLocaleException("Attempted to set default to a locale that is not defined");
				
				this.defaultLocale = value;
			}
			
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private Vector < String > locales; /* Vector<String> */
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private OrderedMap < String, Vector < LocaleDataSource >> localeResources; /* String -> Vector<LocaleDataSource> */
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private OrderedMap < String, PrefixTreeNode > currentLocaleData;
		/* HashMap{ String -> String } */
		private PrefixTree stringTree;
		private System.String defaultLocale;
		private System.String currentLocale;
		private bool fallbackDefaultLocale;
		private bool fallbackDefaultForm;
		private System.Collections.ArrayList observers;
		
		/// <summary> Default constructor. Disables all fallback modes.</summary>
		public Localizer():this(false, false)
		{
		}
		
		/// <summary> Full constructor.
		/// 
		/// </summary>
		/// <param name="fallbackDefaultLocale">If true, search the default locale when no translation for a particular text handle
		/// is found in the current locale.
		/// </param>
		/// <param name="fallbackDefaultForm">If true, search the default text form when no translation is available for the
		/// specified text form ('long', 'short', etc.). Note: form is specified by appending ';[form]' onto the text ID.
		/// </param>
		public Localizer(bool fallbackDefaultLocale, bool fallbackDefaultForm)
		{
			InitBlock();
			stringTree = new PrefixTree(10);
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			localeResources = new OrderedMap < String, Vector < LocaleDataSource >>();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			currentLocaleData = new OrderedMap < String, PrefixTreeNode >();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			locales = new Vector < String >();
			defaultLocale = null;
			currentLocale = null;
			observers = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			this.fallbackDefaultLocale = fallbackDefaultLocale;
			this.fallbackDefaultForm = fallbackDefaultForm;
		}
		
		public  override bool Equals(System.Object o)
		{
			if (o is Localizer)
			{
				Localizer l = (Localizer) o;
				
				//TODO: Compare all resources
				return (ExtUtil.equals(locales, locales) && ExtUtil.equals(localeResources, l.localeResources) && ExtUtil.equals(defaultLocale, l.defaultLocale) && ExtUtil.equals(currentLocale, l.currentLocale) && fallbackDefaultLocale == l.fallbackDefaultLocale && fallbackDefaultForm == l.fallbackDefaultForm);
			}
			else
			{
				return false;
			}
		}
		
		/* === INFORMATION ABOUT AVAILABLE LOCALES === */
		
		/// <summary> Create a new locale (with no mappings). Do nothing if the locale is already defined.
		/// 
		/// </summary>
		/// <param name="locale">Locale to add. Must not be null.
		/// </param>
		/// <returns> True if the locale was not already defined.
		/// </returns>
		/// <throws>  NullPointerException if locale is null </throws>
		public virtual bool addAvailableLocale(System.String locale)
		{
			if (locale == null)
			{
				throw new System.NullReferenceException("unexpected null locale");
			}
			if (hasLocale(locale))
			{
				return false;
			}
			else
			{
				locales.addElement(locale);
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				localeResources.put(locale, new Vector < LocaleDataSource >());
				return true;
			}
		}
		
		/// <summary> Get whether a locale is defined. The locale need not have any mappings.
		/// 
		/// </summary>
		/// <param name="locale">Locale
		/// </param>
		/// <returns> Whether the locale is defined. False if null
		/// </returns>
		public virtual bool hasLocale(System.String locale)
		{
			return (locale == null?false:locales.contains(locale));
		}
		
		/* === MANAGING CURRENT AND DEFAULT LOCALES === */
		
		/// <summary> Set the current locale to the default locale. The default locale must be set.
		/// 
		/// </summary>
		/// <throws>  IllegalStateException If default locale is not set. </throws>
		public virtual void  setToDefault()
		{
			if (defaultLocale == null)
				throw new System.SystemException("Attempted to set to default locale when default locale not set");
			
			Locale = defaultLocale;
		}
		
		/// <summary> Constructs a body of local resources to be the set of Current Locale Data.
		/// 
		/// After loading, the current locale data will contain definitions for each
		/// entry defined by the current locale resources, as well as definitions for any
		/// entry present in the fallback resources but not in those of the current locale.
		/// 
		/// The procedure to accomplish this set is as follows, with overwritting occuring
		/// when a collision occurs:
		/// 
		/// 1. Load all of the in memory definitions for the default locale if fallback is enabled
		/// 2. For each resource file for the default locale, load each definition if fallback is enabled
		/// 3. Load all of the in memory definitions for the current locale
		/// 4. For each resource file for the current locale, load each definition
		/// </summary>
		private void  loadCurrentLocaleResources()
		{
			this.currentLocaleData = getLocaleData(currentLocale);
		}
		
		/// <summary> Moves all relevant entries in the source dictionary into the destination dictionary</summary>
		/// <param name="destination">A dictionary of key/value locale pairs that will be modified
		/// </param>
		/// <param name="source">A dictionary of key/value locale pairs that will be copied into
		/// destination
		/// </param>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		void loadTable(OrderedMap < String, PrefixTreeNode > destination, OrderedMap < String, String > source)
		
		/* === MANAGING LOCALE DATA (TEXT MAPPINGS) === */
		
		/// <summary> Registers a resource file as a source of locale data for the specified
		/// locale.
		/// 
		/// </summary>
		/// <param name="locale">The locale of the definitions provided.
		/// </param>
		/// <param name="resource">A LocaleDataSource containing string data for the locale provided
		/// </param>
		/// <throws>  NullPointerException if resource or locale are null </throws>
		public virtual void  registerLocaleResource(System.String locale, LocaleDataSource resource)
		{
			if (locale == null)
			{
				throw new System.NullReferenceException("Attempt to register a data source to a null locale in the localizer");
			}
			if (resource == null)
			{
				throw new System.NullReferenceException("Attempt to register a null data source in the localizer");
			}
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Vector < LocaleDataSource > resources = new Vector < LocaleDataSource >();
			if (localeResources.containsKey(locale))
			{
				resources = localeResources.get_Renamed(locale);
			}
			resources.addElement(resource);
			localeResources.put(locale, resources);
			
			if (locale.Equals(currentLocale) || locale.Equals(defaultLocale))
			{
				loadCurrentLocaleResources();
			}
		}
		
		/// <summary> Get the set of mappings for a locale.
		/// 
		/// </summary>
		/// <param name="locale">Locale
		/// </param>
		/// <returns>s HashMap representing text mappings for this locale. Returns null if locale not defined or null.
		/// </returns>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public OrderedMap < String, PrefixTreeNode > getLocaleData(String locale)
		
		/// <summary> Get the mappings for a locale, but throw an exception if locale is not defined.
		/// 
		/// </summary>
		/// <param name="locale">Locale
		/// </param>
		/// <returns> Text mappings for locale.
		/// </returns>
		/// <throws>  UnregisteredLocaleException If locale is not defined or null. </throws>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public OrderedMap < String, PrefixTreeNode > getLocaleMap(String locale)
		
		/// <summary> Determine whether a locale has a mapping for a given text handle. Only tests the specified locale and form; does
		/// not fallback to any default locale or text form.
		/// 
		/// </summary>
		/// <param name="locale">Locale. Must be defined and not null.
		/// </param>
		/// <param name="textID">Text handle.
		/// </param>
		/// <returns> True if a mapping exists for the text handle in the given locale.
		/// </returns>
		/// <throws>  UnregisteredLocaleException If locale is not defined. </throws>
		public virtual bool hasMapping(System.String locale, System.String textID)
		{
			if (locale == null || !locales.contains(locale))
			{
				throw new UnregisteredLocaleException("Attempted to access an undefined locale (" + locale + ") while checking for a mapping for  " + textID);
			}
			System.Collections.ArrayList resources = (System.Collections.ArrayList) localeResources.get_Renamed(locale);
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator en = resources.GetEnumerator(); en.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				LocaleDataSource source = (LocaleDataSource) en.Current;
				if (source.getLocalizedText().containsKey(textID))
				{
					return true;
				}
			}
			return false;
		}
		
		/// <summary> Undefine a locale and remove all its data. Cannot be called on the current locale. If called on the default
		/// locale, no default locale will be set afterward.
		/// 
		/// </summary>
		/// <param name="locale">Locale to remove. Must not be null. Need not be defined. Must not be the current locale.
		/// </param>
		/// <returns> Whether the locale existed in the first place.
		/// </returns>
		/// <throws>  IllegalArgumentException If locale is the current locale. </throws>
		/// <throws>  NullPointerException if locale is null </throws>
		public virtual bool destroyLocale(System.String locale)
		{
			if (locale.Equals(currentLocale))
				throw new System.ArgumentException("Attempted to destroy the current locale");
			
			bool removed = hasLocale(locale);
			locales.removeElement(locale);
			localeResources.remove(locale);
			
			if (locale.Equals(defaultLocale))
				defaultLocale = null;
			
			return removed;
		}
		
		/* === RETRIEVING LOCALIZED TEXT === */
		
		/// <summary> Retrieve the localized text for a text handle in the current locale. See getText(String, String) for details.
		/// 
		/// </summary>
		/// <param name="textID">Text handle (text ID appended with optional text form). Must not be null.
		/// </param>
		/// <returns> Localized text. If no text is found after using all fallbacks, return null.
		/// </returns>
		/// <throws>  UnregisteredLocaleException If current locale is not set. </throws>
		/// <throws>  NullPointerException if textID is null </throws>
		public virtual System.String getText(System.String textID)
		{
			return getText(textID, currentLocale);
		}
		
		/// <summary> Retrieve the localized text for a text handle in the current locale. See getText(String, String) for details.
		/// 
		/// </summary>
		/// <param name="textID">Text handle (text ID appended with optional text form). Must not be null.
		/// </param>
		/// <param name="args">arguments for string variables.
		/// </param>
		/// <returns> Localized text
		/// </returns>
		/// <throws>  UnregisteredLocaleException If current locale is not set. </throws>
		/// <throws>  NullPointerException if textID is null </throws>
		/// <throws>  NoLocalizedTextException If there is no text for the specified id </throws>
		public virtual System.String getText(System.String textID, System.String[] args)
		{
			System.String text = getText(textID, currentLocale);
			if (text != null)
			{
				text = processArguments(text, args);
			}
			else
			{
				throw new NoLocalizedTextException("The Localizer could not find a definition for ID: " + textID + " in the '" + currentLocale + "' locale.", textID, currentLocale);
			}
			return text;
		}
		/// <summary> Retrieve the localized text for a text handle in the current locale. See getText(String, String) for details.
		/// 
		/// </summary>
		/// <param name="textID">Text handle (text ID appended with optional text form). Must not be null.
		/// </param>
		/// <param name="args">arguments for string variables.
		/// </param>
		/// <returns> Localized text. If no text is found after using all fallbacks, return null.
		/// </returns>
		/// <throws>  UnregisteredLocaleException If current locale is not set. </throws>
		/// <throws>  NullPointerException if textID is null </throws>
		/// <throws>  NoLocalizedTextException If there is no text for the specified id </throws>
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public virtual System.String getText(System.String textID, System.Collections.Hashtable args)
		{
			System.String text = getText(textID, currentLocale);
			if (text != null)
			{
				text = processArguments(text, args);
			}
			else
			{
				throw new NoLocalizedTextException("The Localizer could not find a definition for ID: " + textID + " in the '" + currentLocale + "' locale.", textID, currentLocale);
			}
			return text;
		}
		
		/// <summary> Retrieve localized text for a text handle in the current locale. Like getText(String), however throws exception
		/// if no localized text is found.
		/// 
		/// </summary>
		/// <param name="textID">Text handle (text ID appended with optional text form). Must not be null.
		/// </param>
		/// <returns> Localized text
		/// </returns>
		/// <throws>  NoLocalizedTextException If there is no text for the specified id </throws>
		/// <throws>  UnregisteredLocaleException If current locale is not set </throws>
		/// <throws>  NullPointerException if textID is null </throws>
		public virtual System.String getLocalizedText(System.String textID)
		{
			System.String text = getText(textID);
			if (text == null)
				throw new NoLocalizedTextException("Can't find localized text for current locale! text id: [" + textID + "] locale: [" + currentLocale + "]", textID, currentLocale);
			return text;
		}
		
		/// <summary> Retrieve the localized text for a text handle in the given locale. If no mapping is found initially, then,
		/// depending on enabled fallback modes, other places will be searched until a mapping is found.
		/// <p>
		/// The search order is thus:
		/// 1) Specified locale, specified text form
		/// 2) Specified locale, default text form
		/// 3) Default locale, specified text form
		/// 4) Default locale, default text form
		/// <p>
		/// (1) and (3) are only searched if a text form ('long', 'short', etc.) is specified.
		/// If a text form is specified, (2) and (4) are only searched if default-form-fallback mode is enabled.
		/// (3) and (4) are only searched if default-locale-fallback mode is enabled. It is not an error in this situation
		/// if no default locale is set; (3) and (4) will simply not be searched.
		/// 
		/// </summary>
		/// <param name="textID">Text handle (text ID appended with optional text form). Must not be null.
		/// </param>
		/// <param name="locale">Locale. Must be defined and not null.
		/// </param>
		/// <returns> Localized text. If no text is found after using all fallbacks, return null.
		/// </returns>
		/// <throws>  UnregisteredLocaleException If the locale is not defined or null. </throws>
		/// <throws>  NullPointerException if textID is null </throws>
		public virtual System.String getText(System.String textID, System.String locale)
		{
			System.String text = getRawText(locale, textID);
			if (text == null && fallbackDefaultForm && textID.IndexOf(";") != - 1)
				text = getRawText(locale, textID.Substring(0, (textID.IndexOf(";")) - (0)));
			//Update: We handle default text without forms without needing to do this. We still need it for default text with default forms, though
			if (text == null && fallbackDefaultLocale && !locale.Equals(defaultLocale) && defaultLocale != null && fallbackDefaultForm)
				text = getText(textID, defaultLocale);
			return text;
		}
		
		/// <summary> Get text for locale and exact text ID only, not using any fallbacks.
		/// 
		/// NOTE: This call will only return the full compliment of available strings if and
		/// only if the requested locale is current. Otherwise it will only retrieve strings
		/// declared at runtime.
		/// 
		/// </summary>
		/// <param name="locale">Locale. Must be defined and not null.
		/// </param>
		/// <param name="textID">Text handle (text ID appended with optional text form). Must not be null.
		/// </param>
		/// <returns> Localized text. Return null if none found.
		/// </returns>
		/// <throws>  UnregisteredLocaleException If the locale is not defined or null. </throws>
		/// <throws>  NullPointerException if textID is null </throws>
		public virtual System.String getRawText(System.String locale, System.String textID)
		{
			if (locale == null)
			{
				throw new UnregisteredLocaleException("Null locale when attempting to fetch text id: " + textID);
			}
			if (textID == null)
			{
				throw new System.NullReferenceException("Null textId passed to localizer");
			}
			if (locale.Equals(currentLocale))
			{
				PrefixTreeNode data = currentLocaleData.get_Renamed(textID);
				return data == null?null:data.render();
			}
			else
			{
				PrefixTreeNode data = getLocaleMap(locale).get_Renamed(textID);
				return data == null?null:data.render();
			}
		}
		
		/* === MANAGING LOCALIZABLE OBSERVERS === */
		
		/// <summary> Register a Localizable to receive updates when the locale is changed. If the Localizable is already
		/// registered, nothing happens. If a locale is currently set, the new Localizable will receive an
		/// immediate 'locale changed' event.
		/// 
		/// </summary>
		/// <param name="l">Localizable to register.
		/// </param>
		public virtual void  registerLocalizable(Localizable l)
		{
			if (!observers.Contains(l))
			{
				observers.Add(l);
				if (currentLocale != null)
				{
					l.localeChanged(currentLocale, this);
				}
			}
		}
		
		/// <summary> Unregister an Localizable from receiving locale change updates. No effect if the Localizable was never
		/// registered in the first place.
		/// 
		/// </summary>
		/// <param name="l">Localizable to unregister.
		/// </param>
		public virtual void  unregisterLocalizable(Localizable l)
		{
			observers.Remove(l);
		}
		
		/// <summary> Unregister all ILocalizables.</summary>
		public virtual void  unregisterAll()
		{
			observers.Clear();
		}
		
		/// <summary> Send a locale change update to all registered ILocalizables.</summary>
		private void  alertLocalizables()
		{
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator e = observers.GetEnumerator(); e.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				((Localizable) e.Current).localeChanged(currentLocale, this);
			}
		}
		
		/* === Managing Arguments === */
		
		private static System.String arg(System.String in_Renamed)
		{
			return "${" + in_Renamed + "}";
		}
		
		public static System.Collections.ArrayList getArgs(System.String text)
		{
			System.Collections.ArrayList args = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			int i = text.IndexOf("${");
			while (i != - 1)
			{
				//UPGRADE_WARNING: Method 'java.lang.String.indexOf' was converted to 'System.String.IndexOf' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
				int j = text.IndexOf("}", i);
				if (j == - 1)
				{
					System.Console.Error.WriteLine("Warning: unterminated ${...} arg");
					break;
				}
				
				System.String arg = text.Substring(i + 2, (j) - (i + 2));
				if (!args.Contains(arg))
				{
					args.Add(arg);
				}
				
				//UPGRADE_WARNING: Method 'java.lang.String.indexOf' was converted to 'System.String.IndexOf' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
				i = text.IndexOf("${", j + 1);
			}
			return args;
		}
		
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public static System.String processArguments(System.String text, System.Collections.Hashtable args)
		{
			int i = text.IndexOf("${");
			while (i != - 1)
			{
				//UPGRADE_WARNING: Method 'java.lang.String.indexOf' was converted to 'System.String.IndexOf' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
				int j = text.IndexOf("}", i);
				if (j == - 1)
				{
					System.Console.Error.WriteLine("Warning: unterminated ${...} arg");
					break;
				}
				
				System.String argName = text.Substring(i + 2, (j) - (i + 2));
				//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
				System.String argVal = (System.String) args[argName];
				if (argVal != null)
				{
					text = text.Substring(0, (i) - (0)) + argVal + text.Substring(j + 1);
					j = i + argVal.Length - 1;
				}
				
				//UPGRADE_WARNING: Method 'java.lang.String.indexOf' was converted to 'System.String.IndexOf' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
				i = text.IndexOf("${", j + 1);
			}
			return text;
		}
		
		public static System.String processArguments(System.String text, System.String[] args)
		{
			return processArguments(text, args, 0);
		}
		
		// enhanced to not re-process substituted text
		public static System.String processArguments(System.String text, System.String[] args, int currentArg)
		{
			System.String working = text;
			if (working.IndexOf("${") != - 1 && args.Length > currentArg)
			{
				int index = extractNextIndex(working, args);
				if (index == - 1)
				{
					index = currentArg;
					currentArg++;
				}
				System.String value_Renamed = args[index];
				System.String[] replaced = replaceFirstValue(working, value_Renamed);
				return replaced[0] + processArguments(replaced[1], args, currentArg);
			}
			else
			{
				return working;
			}
		}
		
		
		public static System.String clearArguments(System.String text)
		{
			System.Collections.ArrayList v = getArgs(text);
			System.String[] empty = new System.String[v.Count];
			for (int i = 0; i < empty.Length; ++i)
			{
				empty[i] = "";
			}
			return processArguments(text, empty);
		}
		
		private static int extractNextIndex(System.String text, System.String[] args)
		{
			int start = text.IndexOf("${");
			//UPGRADE_WARNING: Method 'java.lang.String.indexOf' was converted to 'System.String.IndexOf' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
			int end = text.IndexOf("}", start);
			
			if (start != - 1 && end != - 1)
			{
				System.String val = text.Substring(start + "${".Length, (end) - (start + "${".Length));
				try
				{
					int index = System.Int32.Parse(val);
					if (index >= 0 && index < args.Length)
					{
						return index;
					}
				}
				catch (System.FormatException nfe)
				{
					return - 1;
				}
			}
			
			return - 1;
		}
		
		private static System.String[] replaceFirstValue(System.String text, System.String value_Renamed)
		{
			int start = text.IndexOf("${");
			//UPGRADE_WARNING: Method 'java.lang.String.indexOf' was converted to 'System.String.IndexOf' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
			int end = text.IndexOf("}", start);
			
			return new System.String[]{text.Substring(0, (start) - (0)) + value_Renamed, text.Substring(end + 1, (text.Length) - (end + 1))};
		}
		
		/* === (DE)SERIALIZATION === */
		
		/// <summary> Read the object from stream.</summary>
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader dis, PrototypeFactory pf)
		{
			fallbackDefaultLocale = ExtUtil.readBool(dis);
			fallbackDefaultForm = ExtUtil.readBool(dis);
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			localeResources =(OrderedMap < String, Vector < LocaleDataSource >>) ExtUtil.read(dis, new ExtWrapMap(String.
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		class, new ExtWrapListPoly(), ExtWrapMap.TYPE_ORDERED), pf);
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		locales =(Vector) ExtUtil.read(dis, new ExtWrapList(String.
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		class));
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		setDefaultLocale((String) ExtUtil.read(dis, new ExtWrapNullable(String.
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		class), pf));
		//UPGRADE_NOTE: The initialization of  'currentLocale' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal System.String currentLocale;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		if(currentLocale != null)
		//UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
	
	/// <summary> Write the object to stream.</summary>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void writeExternal(DataOutputStream dos) throws IOException
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		ExtUtil.writeBool(dos, fallbackDefaultLocale);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.writeBool(dos, fallbackDefaultForm);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(dos, new ExtWrapMap(localeResources, new ExtWrapListPoly()));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(dos, new ExtWrapList(locales));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(dos, new ExtWrapNullable(defaultLocale));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(dos, new ExtWrapNullable(currentLocale));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
}