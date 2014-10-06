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
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using InvalidReferenceException = org.javarosa.core.model.instance.InvalidReferenceException;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
namespace org.javarosa.form.api
{
	
	/// <summary> The data model used during form entry. Represents the current state of the
	/// form and provides access to the objects required by the view and the
	/// controller.
	/// </summary>
	public class FormEntryModel
	{
		/// <returns> Form title
		/// </returns>
		virtual public System.String FormTitle
		{
			get
			{
				return form.getTitle();
			}
			
		}
		/// <summary> </summary>
		/// <returns> an array of Strings of the current langauges. Null if there are
		/// none.
		/// </returns>
		virtual public System.String[] Languages
		{
			get
			{
				if (form.getLocalizer() != null)
				{
					return form.getLocalizer().getAvailableLocales();
				}
				return null;
			}
			
		}
		/// <summary> Not yet implemented
		/// 
		/// Should get the number of completed questions to this point.
		/// </summary>
		virtual public int CompletedRelevantQuestionCount
		{
			get
			{
				// TODO: Implement me.
				return 0;
			}
			
		}
		/// <summary> Not yet implemented
		/// 
		/// Should get the total possible questions given the current path through the form.
		/// </summary>
		virtual public int TotalRelevantQuestionCount
		{
			get
			{
				// TODO: Implement me.
				return 0;
			}
			
		}
		/// <returns> total number of questions in the form, regardless of relevancy
		/// </returns>
		virtual public int NumQuestions
		{
			get
			{
				return form.getDeepChildCount();
			}
			
		}
		/// <summary> </summary>
		/// <returns> Returns the current FormIndex referenced by the FormEntryModel.
		/// </returns>
		virtual public FormIndex FormIndex
		{
			get
			{
				return currentFormIndex;
			}
			
		}
		/// <summary> Set the FormIndex for the current question.
		/// 
		/// </summary>
		/// <param name="index">
		/// </param>
		virtual public FormIndex QuestionIndex
		{
			set
			{
				if (!currentFormIndex.Equals(value))
				{
					// See if a hint exists that says we should have a model for this
					// already
					createModelIfNecessary(value);
					currentFormIndex = value;
				}
			}
			
		}
		/// <summary> </summary>
		/// <returns>
		/// </returns>
		virtual public FormDef Form
		{
			get
			{
				return form;
			}
			
		}
		/// <returns> The Current Repeat style which should be used.
		/// </returns>
		virtual public int RepeatStructure
		{
			get
			{
				return this.repeatStructure;
			}
			
		}
		private FormDef form;
		private FormIndex currentFormIndex;
		
		/// <summary> One of "REPEAT_STRUCUTRE_" in this class's static types,
		/// represents what abstract structure repeat events should
		/// be broadacast as.
		/// </summary>
		private int repeatStructure = - 1;
		
		/// <summary> Repeats should be a prompted linear set of questions, either
		/// with a fixed set of repetitions, or a prompt for creating a
		/// new one.
		/// </summary>
		public const int REPEAT_STRUCTURE_LINEAR = 1;
		
		/// <summary> Repeats should be a custom juncture point with centralized
		/// "Create/Remove/Interact" hub.
		/// </summary>
		public const int REPEAT_STRUCTURE_NON_LINEAR = 2;
		
		
		public FormEntryModel(FormDef form):this(form, REPEAT_STRUCTURE_LINEAR)
		{
		}
		
		/// <summary> Creates a new entry model for the form with the appropriate
		/// repeat structure
		/// 
		/// </summary>
		/// <param name="form">
		/// </param>
		/// <param name="repeatStructure">The structure of repeats (the repeat signals which should
		/// be sent during form entry)
		/// </param>
		/// <throws>  IllegalArgumentException If repeatStructure is not valid </throws>
		public FormEntryModel(FormDef form, int repeatStructure)
		{
			this.form = form;
			if (repeatStructure != REPEAT_STRUCTURE_LINEAR && repeatStructure != REPEAT_STRUCTURE_NON_LINEAR)
			{
				throw new System.ArgumentException(repeatStructure + ": does not correspond to a valid repeat structure");
			}
			//We need to see if there are any guessed repeat counts in the form, which prevents
			//us from being able to use the new repeat style
			//Unfortunately this is probably (A) slow and (B) might overflow the stack. It's not the only
			//recursive walk of the form, though, so (B) isn't really relevant
			if (repeatStructure == REPEAT_STRUCTURE_NON_LINEAR && containsRepeatGuesses(form))
			{
				repeatStructure = REPEAT_STRUCTURE_LINEAR;
			}
			this.repeatStructure = repeatStructure;
			this.currentFormIndex = FormIndex.createBeginningOfFormIndex();
		}
		
		/// <summary> Given a FormIndex, returns the event this FormIndex represents.
		/// 
		/// </summary>
		/// <seealso cref="FormEntryController">
		/// </seealso>
		public virtual int getEvent(FormIndex index)
		{
			if (index.BeginningOfFormIndex)
			{
				return FormEntryController.EVENT_BEGINNING_OF_FORM;
			}
			else if (index.EndOfFormIndex)
			{
				return FormEntryController.EVENT_END_OF_FORM;
			}
			
			// This came from chatterbox, and is unclear how correct it is,
			// commented out for now.
			// DELETEME: If things work fine
			// Vector defs = form.explodeIndex(index);
			// IFormElement last = (defs.size() == 0 ? null : (IFormElement)
			// defs.lastElement());
			IFormElement element = form.getChild(index);
			if (element is GroupDef)
			{
				if (((GroupDef) element).Repeat)
				{
					if (repeatStructure != REPEAT_STRUCTURE_NON_LINEAR && form.MainInstance.resolveReference(form.getChildInstanceRef(index)) == null)
					{
						return FormEntryController.EVENT_PROMPT_NEW_REPEAT;
					}
					else if (repeatStructure == REPEAT_STRUCTURE_NON_LINEAR && index.ElementMultiplicity == TreeReference.INDEX_REPEAT_JUNCTURE)
					{
						return FormEntryController.EVENT_REPEAT_JUNCTURE;
					}
					else
					{
						return FormEntryController.EVENT_REPEAT;
					}
				}
				else
				{
					return FormEntryController.EVENT_GROUP;
				}
			}
			else
			{
				return FormEntryController.EVENT_QUESTION;
			}
		}
		
		/// <summary> </summary>
		/// <param name="index">
		/// </param>
		/// <returns>
		/// </returns>
		protected internal virtual TreeElement getTreeElement(FormIndex index)
		{
			return form.MainInstance.resolveReference(index.Reference);
		}
		
		
		/// <returns> the event for the current FormIndex
		/// </returns>
		/// <seealso cref="FormEntryController">
		/// </seealso>
		public virtual int getEvent()
		{
			return getEvent(currentFormIndex);
		}
		
		
		/// <summary> </summary>
		/// <param name="index">
		/// </param>
		/// <returns> Returns the FormEntryPrompt for the specified FormIndex if the
		/// index represents a question.
		/// </returns>
		public virtual FormEntryPrompt getQuestionPrompt(FormIndex index)
		{
			if (form.getChild(index) is QuestionDef)
			{
				return new FormEntryPrompt(form, index);
			}
			else
			{
				throw new System.SystemException("Invalid query for Question prompt. Non-Question object at the form index");
			}
		}
		
		
		/// <summary> </summary>
		/// <param name="index">
		/// </param>
		/// <returns> Returns the FormEntryPrompt for the current FormIndex if the
		/// index represents a question.
		/// </returns>
		public virtual FormEntryPrompt getQuestionPrompt()
		{
			return getQuestionPrompt(currentFormIndex);
		}
		
		
		/// <summary> When you have a non-question event, a CaptionPrompt will have all the
		/// information needed to display to the user.
		/// 
		/// </summary>
		/// <param name="index">
		/// </param>
		/// <returns> Returns the FormEntryCaption for the given FormIndex if is not a
		/// question.
		/// </returns>
		public virtual FormEntryCaption getCaptionPrompt(FormIndex index)
		{
			return new FormEntryCaption(form, index);
		}
		
		
		
		/// <summary> When you have a non-question event, a CaptionPrompt will have all the
		/// information needed to display to the user.
		/// 
		/// </summary>
		/// <param name="index">
		/// </param>
		/// <returns> Returns the FormEntryCaption for the current FormIndex if is not
		/// a question.
		/// </returns>
		public virtual FormEntryCaption getCaptionPrompt()
		{
			return getCaptionPrompt(currentFormIndex);
		}
		
		
		protected internal virtual void  setLanguage(System.String language)
		{
			if (form.getLocalizer() != null)
			{
				form.getLocalizer().setLocale(language);
			}
		}
		
		
		/// <summary> </summary>
		/// <returns> Returns the currently selected language.
		/// </returns>
		public virtual System.String getLanguage()
		{
			return form.getLocalizer().getLocale();
		}
		
		
		/// <summary> Returns a hierarchical list of FormEntryCaption objects for the given
		/// FormIndex
		/// 
		/// </summary>
		/// <param name="index">
		/// </param>
		/// <returns> list of FormEntryCaptions in hierarchical order
		/// </returns>
		public virtual FormEntryCaption[] getCaptionHierarchy(FormIndex index)
		{
			
			List< FormEntryCaption > captions = new List< FormEntryCaption >();
			FormIndex remaining = index;
			while (remaining != null)
			{
				remaining = remaining.NextLevel;
				FormIndex localIndex = index.diff(remaining);
				IFormElement element = form.getChild(localIndex);
				if (element != null)
				{
					FormEntryCaption caption = null;
					if (element is GroupDef)
						caption = new FormEntryCaption(Form, localIndex);
					else if (element is QuestionDef)
						caption = new FormEntryPrompt(Form, localIndex);
					
					if (caption != null)
					{
						captions.addElement(caption);
					}
				}
			}
			FormEntryCaption[] captionArray = new FormEntryCaption[captions.size()];
			captions.copyInto(captionArray);
			return captionArray;
		}
		
		
		/// <summary> Returns a hierarchical list of FormEntryCaption objects for the current
		/// FormIndex
		/// 
		/// </summary>
		/// <param name="index">
		/// </param>
		/// <returns> list of FormEntryCaptions in hierarchical order
		/// </returns>
		public virtual FormEntryCaption[] getCaptionHierarchy()
		{
			return getCaptionHierarchy(currentFormIndex);
		}
		
		
		/// <param name="index">
		/// </param>
		/// <returns> true if the element at the specified index is read only
		/// </returns>
		public virtual bool isIndexReadonly(FormIndex index)
		{
			if (index.BeginningOfFormIndex || index.EndOfFormIndex)
				return true;
			
			TreeReference ref_Renamed = form.getChildInstanceRef(index);
			bool isAskNewRepeat = (getEvent(index) == FormEntryController.EVENT_PROMPT_NEW_REPEAT || getEvent(index) == FormEntryController.EVENT_REPEAT_JUNCTURE);
			
			if (isAskNewRepeat)
			{
				return false;
			}
			else
			{
				TreeElement node = form.MainInstance.resolveReference(ref_Renamed);
				return !node.isEnabled();
			}
		}
		
		
		/// <param name="index">
		/// </param>
		/// <returns> true if the element at the current index is read only
		/// </returns>
		public virtual bool isIndexReadonly()
		{
			return isIndexReadonly(currentFormIndex);
		}
		
		
		/// <summary> Determine if the current FormIndex is relevant. Only relevant indexes
		/// should be returned when filling out a form.
		/// 
		/// </summary>
		/// <param name="index">
		/// </param>
		/// <returns> true if current element at FormIndex is relevant
		/// </returns>
		public virtual bool isIndexRelevant(FormIndex index)
		{
			TreeReference ref_Renamed = form.getChildInstanceRef(index);
			bool isAskNewRepeat = (getEvent(index) == FormEntryController.EVENT_PROMPT_NEW_REPEAT);
			bool isRepeatJuncture = (getEvent(index) == FormEntryController.EVENT_REPEAT_JUNCTURE);
			
			bool relevant;
			if (isAskNewRepeat)
			{
				relevant = form.isRepeatRelevant(ref_Renamed) && form.canCreateRepeat(ref_Renamed, index);
				//repeat junctures are still relevant if no new repeat can be created; that option
				//is simply missing from the menu
			}
			else if (isRepeatJuncture)
			{
				relevant = form.isRepeatRelevant(ref_Renamed);
			}
			else
			{
				TreeElement node = form.MainInstance.resolveReference(ref_Renamed);
				relevant = node.isRelevant(); // check instance flag first
			}
			
			if (relevant)
			{
				// if instance flag/condition says relevant, we still
				// have to check the <group>/<repeat> hierarchy
				
				FormIndex ancestorIndex = index;
				while (!ancestorIndex.isTerminal())
				{
					// This should be safe now that the TreeReference is contained
					// in the ancestor index itself
					TreeElement ancestorNode = form.MainInstance.resolveReference(ancestorIndex.LocalReference);
					
					if (!ancestorNode.isRelevant())
					{
						relevant = false;
						break;
					}
					ancestorIndex = ancestorIndex.NextLevel;
				}
			}
			
			return relevant;
		}
		
		
		/// <summary> Determine if the current FormIndex is relevant. Only relevant indexes
		/// should be returned when filling out a form.
		/// 
		/// </summary>
		/// <param name="index">
		/// </param>
		/// <returns> true if current element at FormIndex is relevant
		/// </returns>
		public virtual bool isIndexRelevant()
		{
			return isIndexRelevant(currentFormIndex);
		}
		
		
		/// <summary> For the current index: Checks whether the index represents a node which
		/// should exist given a non-interactive repeat, along with a count for that
		/// repeat which is beneath the dynamic level specified.
		/// 
		/// If this index does represent such a node, the new model for the repeat is
		/// created behind the scenes and the index for the initial question is
		/// returned.
		/// 
		/// Note: This method will not prevent the addition of new repeat elements in
		/// the interface, it will merely use the xforms repeat hint to create new
		/// nodes that are assumed to exist
		/// 
		/// </summary>
		/// <param name="index">The index to be evaluated as to whether the underlying model is
		/// hinted to exist
		/// </param>
		private void  createModelIfNecessary(FormIndex index)
		{
			if (index.InForm)
			{
				IFormElement e = Form.getChild(index);
				if (e is GroupDef)
				{
					GroupDef g = (GroupDef) e;
					if (g.Repeat && g.CountReference != null)
					{
						// Lu Gram: repeat count XPath needs to be contextualized for nested repeat groups
						TreeReference countRef = FormInstance.unpackReference(g.CountReference);
						TreeReference contextualized = countRef.contextualize(index.Reference);
						IAnswerData count = Form.MainInstance.resolveReference(contextualized).getValue();
						if (count != null)
						{
							long fullcount = ((System.Int32) count.Value);
							TreeReference ref_Renamed = Form.getChildInstanceRef(index);
							TreeElement element = Form.MainInstance.resolveReference(ref_Renamed);
							if (element == null)
							{
								if (index.getTerminal().InstanceIndex < fullcount)
								{
									
									try
									{
										Form.createNewRepeat(index);
									}
									catch (InvalidReferenceException ire)
									{
										SupportClass.WriteStackTrace(ire, Console.Error);
										//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
										throw new System.SystemException("Invalid Reference while creting new repeat!" + ire.Message);
									}
								}
							}
						}
					}
				}
			}
		}
		
		
		public virtual bool isIndexCompoundContainer()
		{
			return isIndexCompoundContainer(FormIndex);
		}
		
		public virtual bool isIndexCompoundContainer(FormIndex index)
		{
			FormEntryCaption caption = getCaptionPrompt(index);
			return getEvent(index) == FormEntryController.EVENT_GROUP && caption.AppearanceHint != null && caption.AppearanceHint.ToLower().Equals("full");
		}
		
		public virtual bool isIndexCompoundElement()
		{
			return isIndexCompoundElement(FormIndex);
		}
		
		public virtual bool isIndexCompoundElement(FormIndex index)
		{
			//Can't be a subquestion if it's not even a question!
			if (getEvent(index) != FormEntryController.EVENT_QUESTION)
			{
				return false;
			}
			
			//get the set of nested groups that this question is in.
			FormEntryCaption[] captions = getCaptionHierarchy(index);
			
			for(FormEntryCaption caption: captions)
			{
				
				//If one of this question's parents is a group, this question is inside of it.
				if (isIndexCompoundContainer(caption.getIndex()))
				{
					return true;
				}
			}
			return false;
		}
		
		public virtual FormIndex[] getCompoundIndices()
		{
			return getCompoundIndices(FormIndex);
		}
		
		public virtual FormIndex[] getCompoundIndices(FormIndex container)
		{
			//ArrayLists are a no-go for J2ME
			
			List< FormIndex > indices = new List< FormIndex >();
			FormIndex walker = incrementIndex(container);
			while (FormIndex.isSubElement(container, walker))
			{
				if (isIndexRelevant(walker))
				{
					indices.addElement(walker);
				}
				walker = incrementIndex(walker);
			}
			FormIndex[] array = new FormIndex[indices.size()];
			for (int i = 0; i < indices.size(); ++i)
			{
				array[i] = indices.elementAt(i);
			}
			return array;
		}
		
		public virtual FormIndex incrementIndex(FormIndex index)
		{
			return incrementIndex(index, true);
		}
		
		public virtual FormIndex incrementIndex(FormIndex index, bool descend)
		{
			System.Collections.ArrayList indexes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList multiplicities = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList elements = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			
			if (index.EndOfFormIndex)
			{
				return index;
			}
			else if (index.BeginningOfFormIndex)
			{
				if (form.getChildren() == null || form.getChildren().size() == 0)
				{
					return FormIndex.createEndOfFormIndex();
				}
			}
			else
			{
				form.collapseIndex(index, indexes, multiplicities, elements);
			}
			
			incrementHelper(indexes, multiplicities, elements, descend);
			
			if (indexes.Count == 0)
			{
				return FormIndex.createEndOfFormIndex();
			}
			else
			{
				return form.buildIndex(indexes, multiplicities, elements);
			}
		}
		
		private void  incrementHelper(System.Collections.ArrayList indexes, System.Collections.ArrayList multiplicities, System.Collections.ArrayList elements, bool descend)
		{
			int i = indexes.Count - 1;
			bool exitRepeat = false; //if exiting a repetition? (i.e., go to next repetition instead of one level up)
			
			if (i == - 1 || elements[i] is GroupDef)
			{
				// current index is group or repeat or the top-level form
				
				if (i >= 0)
				{
					// find out whether we're on a repeat, and if so, whether the
					// specified instance actually exists
					GroupDef group = (GroupDef) elements[i];
					if (group.Repeat)
					{
						if (repeatStructure == REPEAT_STRUCTURE_NON_LINEAR)
						{
							
							if (((System.Int32) multiplicities[multiplicities.Count - 1]) == TreeReference.INDEX_REPEAT_JUNCTURE)
							{
								
								descend = false;
								exitRepeat = true;
							}
						}
						else
						{
							
							if (form.MainInstance.resolveReference(form.getChildInstanceRef(elements, multiplicities)) == null)
							{
								descend = false; // repeat instance does not exist; do not descend into it
								exitRepeat = true;
							}
						}
					}
				}
				
				if (descend)
				{
					IFormElement ife = (i == - 1)?null:(IFormElement) elements[i];
					if ((i == - 1) || (ife != null && ife.getChildren() != null && ife.getChildren().size() > 0))
					{
						indexes.Add(0);
						multiplicities.Add(0);
						elements.Add((i == - 1?form:(IFormElement) elements[i]).getChild(0));
						
						if (repeatStructure == REPEAT_STRUCTURE_NON_LINEAR)
						{
							if (elements[elements.Count - 1] is GroupDef && ((GroupDef) elements[elements.Count - 1]).Repeat)
							{
								multiplicities[multiplicities.Count - 1] = (System.Int32) TreeReference.INDEX_REPEAT_JUNCTURE;
							}
						}
						
						return ;
					}
				}
			}
			
			while (i >= 0)
			{
				// if on repeat, increment to next repeat EXCEPT when we're on a
				// repeat instance that does not exist and was not created
				// (repeat-not-existing can only happen at lowest level; exitRepeat
				// will be true)
				if (!exitRepeat && elements[i] is GroupDef && ((GroupDef) elements[i]).Repeat)
				{
					if (repeatStructure == REPEAT_STRUCTURE_NON_LINEAR)
					{
						
						multiplicities[i] = (System.Int32) TreeReference.INDEX_REPEAT_JUNCTURE;
					}
					else
					{
						
						multiplicities[i] = (System.Int32) (((System.Int32) multiplicities[i]) + 1);
					}
					return ;
				}
				
				IFormElement parent = (i == 0?form:(IFormElement) elements[i - 1]);
				int curIndex = ((System.Int32) indexes[i]);
				
				// increment to the next element on the current level
				if (curIndex + 1 >= parent.getChildren().size())
				{
					// at the end of the current level; move up one level and start
					// over
					indexes.RemoveAt(i);
					multiplicities.RemoveAt(i);
					elements.RemoveAt(i);
					i--;
					exitRepeat = false;
				}
				else
				{
					indexes[i] = (System.Int32) (curIndex + 1);
					multiplicities[i] = 0;
					elements[i] = parent.getChild(curIndex + 1);
					
					if (repeatStructure == REPEAT_STRUCTURE_NON_LINEAR)
					{
						if (elements[elements.Count - 1] is GroupDef && ((GroupDef) elements[elements.Count - 1]).Repeat)
						{
							multiplicities[multiplicities.Count - 1] = (System.Int32) TreeReference.INDEX_REPEAT_JUNCTURE;
						}
					}
					
					return ;
				}
			}
		}
		
		public virtual FormIndex decrementIndex(FormIndex index)
		{
			System.Collections.ArrayList indexes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList multiplicities = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList elements = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			
			if (index.BeginningOfFormIndex)
			{
				return index;
			}
			else if (index.EndOfFormIndex)
			{
				if (form.getChildren() == null || form.getChildren().size() == 0)
				{
					return FormIndex.createBeginningOfFormIndex();
				}
			}
			else
			{
				form.collapseIndex(index, indexes, multiplicities, elements);
			}
			
			decrementHelper(indexes, multiplicities, elements);
			
			if (indexes.Count == 0)
			{
				return FormIndex.createBeginningOfFormIndex();
			}
			else
			{
				return form.buildIndex(indexes, multiplicities, elements);
			}
		}
		
		private void  decrementHelper(System.Collections.ArrayList indexes, System.Collections.ArrayList multiplicities, System.Collections.ArrayList elements)
		{
			int i = indexes.Count - 1;
			
			if (i != - 1)
			{
				int curIndex = ((System.Int32) indexes[i]);
				int curMult = ((System.Int32) multiplicities[i]);
				
				if (repeatStructure == REPEAT_STRUCTURE_NON_LINEAR && elements[elements.Count - 1] is GroupDef && ((GroupDef) elements[elements.Count - 1]).Repeat && ((System.Int32) multiplicities[multiplicities.Count - 1]) != TreeReference.INDEX_REPEAT_JUNCTURE)
				{
					multiplicities[i] = (System.Int32) TreeReference.INDEX_REPEAT_JUNCTURE;
					return ;
				}
				else if (repeatStructure != REPEAT_STRUCTURE_NON_LINEAR && curMult > 0)
				{
					multiplicities[i] = (System.Int32) (curMult - 1);
				}
				else if (curIndex > 0)
				{
					// set node to previous element
					indexes[i] = (System.Int32) (curIndex - 1);
					multiplicities[i] = 0;
					elements[i] = (i == 0?form:(IFormElement) elements[i - 1]).getChild(curIndex - 1);
					
					if (setRepeatNextMultiplicity(elements, multiplicities))
						return ;
				}
				else
				{
					// at absolute beginning of current level; index to parent
					indexes.RemoveAt(i);
					multiplicities.RemoveAt(i);
					elements.RemoveAt(i);
					return ;
				}
			}
			
			IFormElement element = (i < 0?form:(IFormElement) elements[i]);
			while (!(element is QuestionDef))
			{
				if (element.getChildren() == null || element.getChildren().size() == 0)
				{
					//if there are no children we just return the current index (the group itself)
					return ;
				}
				int subIndex = element.getChildren().size() - 1;
				element = element.getChild(subIndex);
				
				indexes.Add((System.Int32) subIndex);
				multiplicities.Add(0);
				elements.Add(element);
				
				if (setRepeatNextMultiplicity(elements, multiplicities))
					return ;
			}
		}
		
		private bool setRepeatNextMultiplicity(System.Collections.ArrayList elements, System.Collections.ArrayList multiplicities)
		{
			// find out if node is repeatable
			TreeReference nodeRef = form.getChildInstanceRef(elements, multiplicities);
			TreeElement node = form.MainInstance.resolveReference(nodeRef);
			if (node == null || node.Repeatable)
			{
				// node == null if there are no
				// instances of the repeat
				int mult;
				if (node == null)
				{
					mult = 0; // no repeats; next is 0
				}
				else
				{
					System.String name = node.Name;
					TreeElement parentNode = form.MainInstance.resolveReference(nodeRef.ParentRef);
					mult = parentNode.getChildMultiplicity(name);
				}
				multiplicities[multiplicities.Count - 1] = (System.Int32) (repeatStructure == REPEAT_STRUCTURE_NON_LINEAR?TreeReference.INDEX_REPEAT_JUNCTURE:mult);
				return true;
			}
			else
			{
				return false;
			}
		}
		
		/// <summary> This method does a recursive check of whether there are any repeat guesses
		/// in the element or its subtree. This is a necessary step when initializing
		/// the model to be able to identify whether new repeats can be used.
		/// 
		/// </summary>
		/// <param name="parent">The form element to begin checking
		/// </param>
		/// <returns> true if the element or any of its descendants is a repeat
		/// which has a count guess, false otherwise.
		/// </returns>
		private bool containsRepeatGuesses(IFormElement parent)
		{
			if (parent is GroupDef)
			{
				GroupDef g = (GroupDef) parent;
				if (g.Repeat && g.CountReference != null)
				{
					return true;
				}
			}
			
			
			if (children == null)
			{
				return false;
			}
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator en = children.elements(); en.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				if (containsRepeatGuesses((IFormElement) en.Current))
				{
					return true;
				}
			}
			return false;
		}
	}
}