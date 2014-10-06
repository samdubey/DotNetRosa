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
using org.javarosa.core.model;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using Localizer = org.javarosa.core.services.locale.Localizer;
using IQuestionWidget = org.javarosa.formmanager.view.IQuestionWidget;
namespace org.javarosa.form.api
{
	
	/// <summary> This class gives you all the information you need to display a caption when
	/// your current FormIndex references a GroupEvent, RepeatPromptEvent, or
	/// RepeatEvent.
	/// 
	/// </summary>
	/// <author>  Simon Kelly
	/// </author>
	public class FormEntryCaption : FormElementStateListener
	{
		private void  InitBlock()
		{
			GroupDef g = (GroupDef) element;
			if (!g.Repeat)
			{
				throw new System.SystemException("not a repeat");
			}
			
			int numRepetitions = NumRepetitions;
			
			List< String > reps = new List< String >();
			for (int i = 0; i < numRepetitions; i++)
			{
				reps.addElement(getRepetitionText("choose", form.descendIntoRepeat(index, i), false));
			}
			return reps;
		}
		/// <summary> Convenience method
		/// Get longText form of text for THIS element (if available) 
		/// !!Falls back to default form if 'long' form does not exist.!!
		/// Use getSpecialFormQuestionText() if you want short form only.
		/// </summary>
		/// <returns> longText form 
		/// </returns>
		virtual public System.String LongText
		{
			get
			{
				return getQuestionText(TextID);
			}
			
		}
		/// <summary> Convenience method
		/// Get shortText form of text for THIS element (if available) 
		/// !!Falls back to default form if 'short' form does not exist.!!
		/// Use getSpecialFormQuestionText() if you want short form only.
		/// </summary>
		/// <returns> shortText form 
		/// </returns>
		virtual public System.String ShortText
		{
			get
			{
				System.String returnText = getSpecialFormQuestionText(TextID, TEXT_FORM_SHORT);
				if (returnText == null)
				{
					returnText = LongText;
				}
				return returnText;
			}
			
		}
		/// <summary> Convenience method
		/// Get audio URI from Text form for THIS element (if available)
		/// </summary>
		/// <returns> audio URI form stored in current locale of Text, returns null if not available
		/// </returns>
		virtual public System.String AudioText
		{
			get
			{
				return getSpecialFormQuestionText(TextID, TEXT_FORM_AUDIO);
			}
			
		}
		/// <summary> Convenience method
		/// Get image URI form of text for THIS element (if available)
		/// </summary>
		/// <returns> URI of image form stored in current locale of Text, returns null if not available
		/// </returns>
		virtual public System.String ImageText
		{
			get
			{
				return getSpecialFormQuestionText(TextID, TEXT_FORM_IMAGE);
			}
			
		}
		virtual public int NumRepetitions
		{
			//this should probably be somewhere better
			
			get
			{
				return form.NumRepetitions;
			}
			
		}
		virtual public System.String AppearanceHint
		{
			get
			{
				return element.getAppearanceAttr();
			}
			
		}
		virtual public int Multiplicity
		{
			get
			{
				return index.ElementMultiplicity;
			}
			
		}
		virtual public IFormElement FormElement
		{
			get
			{
				return element;
			}
			
		}
		virtual public FormIndex Index
		{
			get
			{
				return index;
			}
			
		}
		virtual protected internal System.String TextID
		{
			get
			{
				return this.textID;
			}
			
		}
		
		internal FormDef form;
		internal FormIndex index;
		protected internal IFormElement element;
		private System.String textID;
		
		public const System.String TEXT_FORM_LONG = "long";
		public const System.String TEXT_FORM_SHORT = "short";
		public const System.String TEXT_FORM_AUDIO = "audio";
		public const System.String TEXT_FORM_IMAGE = "image";
		public const System.String TEXT_FORM_VIDEO = "video";
		
		protected internal IQuestionWidget viewWidget;
		
		/// <summary> This empty constructor exists for convenience of any supertypes of this
		/// prompt
		/// </summary>
		public FormEntryCaption()
		{
			InitBlock();
		}
		
		/// <summary> Creates a FormEntryCaption for the element at the given index in the form.
		/// 
		/// </summary>
		/// <param name="form">
		/// </param>
		/// <param name="index">
		/// </param>
		public FormEntryCaption(FormDef form, FormIndex index)
		{
			InitBlock();
			this.form = form;
			this.index = index;
			this.element = form.getChild(index);
			this.viewWidget = null;
			this.textID = this.element.TextID;
		}
		
		
		
		/// <summary> Attempts to return question text for this element.
		/// Will check for text in the following order:<br/>
		/// Localized Text (long form) -> Localized Text (no special form) <br />
		/// If no textID is specified, method will return THIS element's labelInnerText.
		/// </summary>
		/// <param name="textID">- The textID of the text you're trying to retrieve. if <code>textID == null</code> will get LabelInnerText for current element
		/// </param>
		/// <returns> Question Text.  <code>null</code> if no text for this element exists (after all fallbacks).
		/// </returns>
		/// <throws>  RunTimeException if this method is called on an element that is NOT a QuestionDef </throws>
		public virtual System.String getQuestionText(System.String textID)
		{
			System.String tid = textID;
			if ((System.Object) tid == (System.Object) "")
				tid = null; //to make things look clean
			
			//check for the null id case and return labelInnerText if it is so.
			if (tid == null)
				return substituteStringArgs(element.getLabelInnerText());
			
			//otherwise check for 'long' form of the textID, then for the default form and return
			System.String returnText;
			returnText = getIText(tid, "long");
			if (returnText == null)
				returnText = getIText(tid, null);
			
			return substituteStringArgs(returnText);
		}
		
		/// <summary> Same as getQuestionText(String textID), but for the current element textID;</summary>
		/// <seealso cref="getQuestionText(String textID)">
		/// </seealso>
		/// <returns> Question Text
		/// </returns>
		public virtual System.String getQuestionText()
		{
			return getQuestionText(TextID);
		}
		
		/// <summary> This method is generally used to retrieve special forms of a
		/// textID, e.g. "audio", "video", etc.
		/// 
		/// </summary>
		/// <param name="textID">- The textID of the text you're trying to retrieve.
		/// </param>
		/// <param name="form">- special text form of textID you're trying to retrieve. 
		/// </param>
		/// <returns> Special Form Question Text. <code>null</code> if no text for this element exists (with the specified special form).
		/// </returns>
		/// <throws>  RunTimeException if this method is called on an element that is NOT a QuestionDef </throws>
		public virtual System.String getSpecialFormQuestionText(System.String textID, System.String form)
		{
			if (textID == null || textID.Equals(""))
				return null;
			
			System.String returnText = getIText(textID, form);
			
			return substituteStringArgs(returnText);
		}
		
		/// <summary> Same as getSpecialFormQuestionText(String textID,String form) except that the
		/// textID defaults to the textID of the current element.
		/// </summary>
		/// <param name="form">- special text form of textID you're trying to retrieve. 
		/// </param>
		/// <returns> Special Form Question Text. <code>null</code> if no text for this element exists (with the specified special form).
		/// </returns>
		/// <throws>  RunTimeException if this method is called on an element that is NOT a QuestionDef </throws>
		public virtual System.String getSpecialFormQuestionText(System.String form)
		{
			return getSpecialFormQuestionText(TextID, form);
		}
		
		
		
		
		/// <param name="textID">- the textID of the text you'd like to retrieve
		/// </param>
		/// <param name="form">- the special form (e.g. "audio","long", etc) of the text
		/// </param>
		/// <returns> the IText for the parameters specified.
		/// </returns>
		protected internal virtual System.String getIText(System.String textID, System.String form)
		{
			System.String returnText = null;
			if (textID == null || textID.Equals(""))
				return null;
			if (form != null && !form.Equals(""))
			{
				try
				{
					returnText = localizer().getRawText(localizer().Locale, textID + ";" + form);
				}
				catch (System.NullReferenceException npe)
				{
				}
			}
			else
			{
				try
				{
					returnText = localizer().getRawText(localizer().Locale, textID);
				}
				catch (System.NullReferenceException npe)
				{
				}
			}
			return returnText;
		}
		
		//TODO: this is explicitly missing integration with the new multi-media support
		//TODO: localize the default captions
		public virtual System.String getRepeatText(System.String typeKey)
		{
			GroupDef g = (GroupDef) element;
			if (!g.Repeat)
			{
				throw new System.SystemException("not a repeat");
			}
			
			System.String title = LongText;
			int count = NumRepetitions;
			
			System.String caption = null;
			if ("mainheader".Equals(typeKey))
			{
				caption = g.mainHeader;
				if (caption == null)
				{
					return title;
				}
			}
			else if ("add".Equals(typeKey))
			{
				caption = g.addCaption;
				if (caption == null)
				{
					return "Add another " + title;
				}
			}
			else if ("add-empty".Equals(typeKey))
			{
				caption = g.addEmptyCaption;
				if (caption == null)
				{
					caption = g.addCaption;
				}
				if (caption == null)
				{
					return "None - Add " + title;
				}
			}
			else if ("del".Equals(typeKey))
			{
				caption = g.delCaption;
				if (caption == null)
				{
					return "Delete " + title;
				}
			}
			else if ("done".Equals(typeKey))
			{
				caption = g.doneCaption;
				if (caption == null)
				{
					return "Done";
				}
			}
			else if ("done-empty".Equals(typeKey))
			{
				caption = g.doneEmptyCaption;
				if (caption == null)
				{
					caption = g.doneCaption;
				}
				if (caption == null)
				{
					return "Skip";
				}
			}
			else if ("delheader".Equals(typeKey))
			{
				caption = g.delHeader;
				if (caption == null)
				{
					return "Delete which " + title + "?";
				}
			}
			
			
			HashMap < String, Object > vars = new HashMap < String, Object >();
			vars.put("name", title);
			vars.put("n", (System.Int32) count);
			return form.fillTemplateString(caption, index.Reference, vars);
		}
		
		public virtual System.String getRepetitionText(bool newrep)
		{
			return getRepetitionText("header", index, newrep);
		}
		
		private System.String getRepetitionText(System.String type, FormIndex index, bool newrep)
		{
			if (element is GroupDef && ((GroupDef) element).Repeat && index.ElementMultiplicity >= 0)
			{
				GroupDef g = (GroupDef) element;
				
				System.String title = LongText;
				int ix = index.ElementMultiplicity + 1;
				int count = NumRepetitions;
				
				System.String caption = null;
				if ("header".Equals(type))
				{
					caption = g.entryHeader;
				}
				else if ("choose".Equals(type))
				{
					caption = g.chooseCaption;
					if (caption == null)
					{
						caption = g.entryHeader;
					}
				}
				if (caption == null)
				{
					return title + " " + ix + "/" + count;
				}
				
				
				HashMap < String, Object > vars = new HashMap < String, Object >();
				vars.put("name", title);
				vars.put("i", (System.Int32) ix);
				vars.put("n", (System.Int32) count);
				vars.put("new", newrep);
				return form.fillTemplateString(caption, index.Reference, vars);
			}
			else
			{
				return null;
			}
		}
		
		
		public List< String > getRepetitionsText()
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'RepeatOptions' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		public class RepeatOptions
		{
			public RepeatOptions(FormEntryCaption enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(FormEntryCaption enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private FormEntryCaption enclosingInstance;
			public FormEntryCaption Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public System.String header;
			public System.String add;
			public System.String delete;
			public System.String done;
			public System.String delete_header;
		}
		
		public virtual RepeatOptions getRepeatOptions()
		{
			RepeatOptions ro = new RepeatOptions(this);
			bool has_repetitions = (NumRepetitions > 0);
			
			ro.header = getRepeatText("mainheader");
			
			ro.add = null;
			if (form.canCreateRepeat(form.getChildInstanceRef(index), index))
			{
				ro.add = getRepeatText(has_repetitions?"add":"add-empty");
			}
			ro.delete = null;
			ro.delete_header = null;
			if (has_repetitions)
			{
				ro.delete = getRepeatText("del");
				ro.delete_header = getRepeatText("delheader");
			}
			ro.done = getRepeatText(has_repetitions?"done":"done-empty");
			
			return ro;
		}
		
		protected internal virtual System.String substituteStringArgs(System.String templateStr)
		{
			if (templateStr == null)
			{
				return null;
			}
			return form.fillTemplateString(templateStr, index.Reference);
		}
		
		/// <returns> true if this represents a <repeat> element
		/// </returns>
		public virtual bool repeats()
		{
			if (element is GroupDef)
			{
				return ((GroupDef) element).Repeat;
			}
			else
			{
				return false;
			}
		}
		
		protected internal virtual Localizer localizer()
		{
			return this.form.getLocalizer();
		}
		
		// ==== observer pattern ====//
		
		public virtual void  register(IQuestionWidget viewWidget)
		{
			this.viewWidget = viewWidget;
			element.registerStateObserver(this);
		}
		
		public virtual void  unregister()
		{
			this.viewWidget = null;
			element.unregisterStateObserver(this);
		}
		
		public virtual void  formElementStateChanged(IFormElement element, int changeFlags)
		{
			if (this.element != element)
			{
				throw new System.SystemException("Widget received event from foreign question");
			}
			if (viewWidget != null)
			{
				viewWidget.refreshWidget(changeFlags);
			}
		}
		
		public virtual void  formElementStateChanged(TreeElement instanceNode, int changeFlags)
		{
			throw new System.SystemException("cannot happen");
		}
	}
}