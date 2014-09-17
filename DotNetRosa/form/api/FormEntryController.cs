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
using FormIndex = org.javarosa.core.model.FormIndex;
using QuestionDef = org.javarosa.core.model.QuestionDef;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using InvalidReferenceException = org.javarosa.core.model.instance.InvalidReferenceException;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
namespace org.javarosa.form.api
{
	
	/// <summary> This class is used to navigate through an xform and appropriately manipulate
	/// the FormEntryModel's state.
	/// </summary>
	public class FormEntryController
	{
		virtual public FormEntryModel Model
		{
			get
			{
				return model;
			}
			
		}
		/// <summary> Sets the current language.</summary>
		/// <param name="language">
		/// </param>
		virtual public System.String Language
		{
			set
			{
				model.setLanguage(value);
			}
			
		}
		public const int ANSWER_OK = 0;
		public const int ANSWER_REQUIRED_BUT_EMPTY = 1;
		public const int ANSWER_CONSTRAINT_VIOLATED = 2;
		
		public const int EVENT_BEGINNING_OF_FORM = 0;
		public const int EVENT_END_OF_FORM = 1;
		public const int EVENT_PROMPT_NEW_REPEAT = 2;
		public const int EVENT_QUESTION = 4;
		public const int EVENT_GROUP = 8;
		public const int EVENT_REPEAT = 16;
		public const int EVENT_REPEAT_JUNCTURE = 32;
		
		internal FormEntryModel model;
		
		/// <summary> Creates a new form entry controller for the model provided
		/// 
		/// </summary>
		/// <param name="model">
		/// </param>
		public FormEntryController(FormEntryModel model)
		{
			this.model = model;
		}
		
		
		/// <summary> Attempts to save answer at the current FormIndex into the datamodel.
		/// 
		/// </summary>
		/// <param name="data">
		/// </param>
		/// <returns>
		/// </returns>
		public virtual int answerQuestion(IAnswerData data)
		{
			return answerQuestion(model.FormIndex, data);
		}
		
		
		/// <summary> Attempts to save the answer at the specified FormIndex into the
		/// datamodel.
		/// 
		/// </summary>
		/// <param name="index">
		/// </param>
		/// <param name="data">
		/// </param>
		/// <returns> OK if save was successful, error if a constraint was violated.
		/// </returns>
		public virtual int answerQuestion(FormIndex index, IAnswerData data)
		{
			QuestionDef q = model.getQuestionPrompt(index).Question;
			if (model.getEvent(index) != FormEntryController.EVENT_QUESTION)
			{
				throw new System.SystemException("Non-Question object at the form index.");
			}
			TreeElement element = model.getTreeElement(index);
			bool complexQuestion = q.Complex;
			
			bool hasConstraints = false;
			if (element.Required && data == null)
			{
				return ANSWER_REQUIRED_BUT_EMPTY;
			}
			else if (!complexQuestion && !model.Form.evaluateConstraint(index.Reference, data))
			{
				return ANSWER_CONSTRAINT_VIOLATED;
			}
			else if (!complexQuestion)
			{
				commitAnswer(element, index, data);
				return ANSWER_OK;
			}
			else if (complexQuestion && hasConstraints)
			{
				//TODO: itemsets: don't currently evaluate constraints for itemset/copy -- haven't figured out how handle it yet
				throw new System.SystemException("Itemsets do not currently evaluate constraints. Your constraint will not work, please remove it before proceeding.");
			}
			else
			{
				try
				{
					model.Form.copyItemsetAnswer(q, element, data);
				}
				catch (InvalidReferenceException ire)
				{
					SupportClass.WriteStackTrace(ire, Console.Error);
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					throw new System.SystemException("Invalid reference while copying itemset answer: " + ire.Message);
				}
				return ANSWER_OK;
			}
		}
		
		
		/// <summary> saveAnswer attempts to save the current answer into the data model
		/// without doing any constraint checking. Only use this if you know what
		/// you're doing. For normal form filling you should always use
		/// answerQuestion or answerCurrentQuestion.
		/// 
		/// </summary>
		/// <param name="index">
		/// </param>
		/// <param name="data">
		/// </param>
		/// <returns> true if saved successfully, false otherwise.
		/// </returns>
		public virtual bool saveAnswer(FormIndex index, IAnswerData data)
		{
			if (model.getEvent(index) != FormEntryController.EVENT_QUESTION)
			{
				throw new System.SystemException("Non-Question object at the form index.");
			}
			TreeElement element = model.getTreeElement(index);
			return commitAnswer(element, index, data);
		}
		
		
		/// <summary> saveAnswer attempts to save the current answer into the data model
		/// without doing any constraint checking. Only use this if you know what
		/// you're doing. For normal form filling you should always use
		/// answerQuestion().
		/// 
		/// </summary>
		/// <param name="index">
		/// </param>
		/// <param name="data">
		/// </param>
		/// <returns> true if saved successfully, false otherwise.
		/// </returns>
		public virtual bool saveAnswer(IAnswerData data)
		{
			return saveAnswer(model.FormIndex, data);
		}
		
		
		/// <summary> commitAnswer actually saves the data into the datamodel.
		/// 
		/// </summary>
		/// <param name="element">
		/// </param>
		/// <param name="index">
		/// </param>
		/// <param name="data">
		/// </param>
		/// <returns> true if saved successfully, false otherwise
		/// </returns>
		private bool commitAnswer(TreeElement element, FormIndex index, IAnswerData data)
		{
			if (data != null || element.Value != null)
			{
				// we should check if the data to be saved is already the same as
				// the data in the model, but we can't (no IAnswerData.equals())
				model.Form.setValue(data, index.Reference, element);
				return true;
			}
			else
			{
				return false;
			}
		}
		
		
		/// <summary> Navigates forward in the form.
		/// 
		/// </summary>
		/// <returns> the next event that should be handled by a view.
		/// </returns>
		public virtual int stepToNextEvent()
		{
			return stepEvent(true);
		}
		
		
		/// <summary> Navigates backward in the form.
		/// 
		/// </summary>
		/// <returns> the next event that should be handled by a view.
		/// </returns>
		public virtual int stepToPreviousEvent()
		{
			return stepEvent(false);
		}
		
		
		/// <summary> Moves the current FormIndex to the next/previous relevant position.
		/// 
		/// </summary>
		/// <param name="forward">
		/// </param>
		/// <returns>
		/// </returns>
		private int stepEvent(bool forward)
		{
			FormIndex index = model.FormIndex;
			
			do 
			{
				if (forward)
				{
					index = model.incrementIndex(index);
				}
				else
				{
					index = model.decrementIndex(index);
				}
			}
			while (index.InForm && !model.isIndexRelevant(index));
			
			return jumpToIndex(index);
		}
		
		
		/// <summary> Jumps to a given FormIndex.
		/// 
		/// </summary>
		/// <param name="index">
		/// </param>
		/// <returns> EVENT for the specified Index.
		/// </returns>
		public virtual int jumpToIndex(FormIndex index)
		{
			model.QuestionIndex = index;
			return model.getEvent(index);
		}
		
		public virtual FormIndex descendIntoRepeat(int n)
		{
			jumpToIndex(model.Form.descendIntoRepeat(model.FormIndex, n));
			return model.FormIndex;
		}
		
		public virtual FormIndex descendIntoNewRepeat()
		{
			jumpToIndex(model.Form.descendIntoRepeat(model.FormIndex, - 1));
			newRepeat(model.FormIndex);
			return model.FormIndex;
		}
		
		/// <summary> Creates a new repeated instance of the group referenced by the specified
		/// FormIndex.
		/// 
		/// </summary>
		/// <param name="questionIndex">
		/// </param>
		public virtual void  newRepeat(FormIndex questionIndex)
		{
			try
			{
				model.Form.createNewRepeat(questionIndex);
			}
			catch (InvalidReferenceException ire)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new System.SystemException("Invalid reference while copying itemset answer: " + ire.Message);
			}
		}
		
		
		/// <summary> Creates a new repeated instance of the group referenced by the current
		/// FormIndex.
		/// 
		/// </summary>
		/// <param name="questionIndex">
		/// </param>
		public virtual void  newRepeat()
		{
			newRepeat(model.FormIndex);
		}
		
		
		/// <summary> Deletes a repeated instance of a group referenced by the specified
		/// FormIndex.
		/// 
		/// </summary>
		/// <param name="questionIndex">
		/// </param>
		/// <returns>
		/// </returns>
		public virtual FormIndex deleteRepeat(FormIndex questionIndex)
		{
			return model.Form.deleteRepeat(questionIndex);
		}
		
		
		/// <summary> Deletes a repeated instance of a group referenced by the current
		/// FormIndex.
		/// 
		/// </summary>
		/// <param name="questionIndex">
		/// </param>
		/// <returns>
		/// </returns>
		public virtual FormIndex deleteRepeat()
		{
			return deleteRepeat(model.FormIndex);
		}
		
		public virtual void  deleteRepeat(int n)
		{
			deleteRepeat(model.Form.descendIntoRepeat(model.FormIndex, n));
		}
	}
}