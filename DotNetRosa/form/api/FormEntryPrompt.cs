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
using Constants = org.javarosa.core.model.Constants;
using FormDef = org.javarosa.core.model.FormDef;
using FormIndex = org.javarosa.core.model.FormIndex;
using ItemsetBinding = org.javarosa.core.model.ItemsetBinding;
using QuestionDef = org.javarosa.core.model.QuestionDef;
using SelectChoice = org.javarosa.core.model.SelectChoice;
using Constraint = org.javarosa.core.model.condition.Constraint;
using EvaluationContext = org.javarosa.core.model.condition.EvaluationContext;
using ConstraintHint = org.javarosa.core.model.condition.pivot.ConstraintHint;
using UnpivotableExpressionException = org.javarosa.core.model.condition.pivot.UnpivotableExpressionException;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using SelectMultiData = org.javarosa.core.model.data.SelectMultiData;
using SelectOneData = org.javarosa.core.model.data.SelectOneData;
using Selection = org.javarosa.core.model.data.helper.Selection;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using Logger = org.javarosa.core.services.Logger;
using NoLocalizedTextException = org.javarosa.core.util.NoLocalizedTextException;
using UnregisteredLocaleException = org.javarosa.core.util.UnregisteredLocaleException;
using IQuestionWidget = org.javarosa.formmanager.view.IQuestionWidget;
namespace org.javarosa.form.api
{
	
	
	
	/// <summary> This class gives you all the information you need to display a question when
	/// your current FormIndex references a QuestionEvent.
	/// 
	/// </summary>
	/// <author>  Yaw Anokwa
	/// </author>
	public class FormEntryPrompt:FormEntryCaption
	{
		private void  InitBlock()
		{
			return mTreeElement.getBindAttributes();
			QuestionDef q = Question;
			
			ItemsetBinding itemset = q.DynamicChoices;
			if (itemset != null)
			{
				if (!dynamicChoicesPopulated)
				{
					form.populateDynamicChoices(itemset, mTreeElement.Ref);
					dynamicChoicesPopulated = true;
				}
				return itemset.getChoices();
			}
			else
			{
				//static choices
				return q.getChoices();
			}
		}
		virtual public int ControlType
		{
			get
			{
				return Question.ControlType;
			}
			
		}
		virtual public int DataType
		{
			get
			{
				return mTreeElement.DataType;
			}
			
		}
		virtual public System.String PromptAttributes
		{
			// attributes available in the bind, instance and body
			
			get
			{
				// TODO: implement me.
				return null;
			}
			
		}
		virtual public IAnswerData AnswerValue
		{
			//note: code overlap with FormDef.copyItemsetAnswer
			
			get
			{
				QuestionDef q = Question;
				
				ItemsetBinding itemset = q.DynamicChoices;
				if (itemset != null)
				{
					if (itemset.valueRef != null)
					{
						//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
						//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
						Vector < String > preselectedValues = new Vector < String >();
						
						//determine which selections are already present in the answer
						if (itemset.copyMode)
						{
							TreeReference destRef = itemset.getDestRef().contextualize(mTreeElement.Ref);
							//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
							for (int i = 0; i < subNodes.size(); i++)
							{
								TreeElement node = form.MainInstance.resolveReference(subNodes.elementAt(i));
								System.String value_Renamed = itemset.RelativeValue.evalReadable(form.MainInstance, new EvaluationContext(form.EvaluationContext, node.Ref));
								preselectedValues.addElement(value_Renamed);
							}
						}
						else
						{
							//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
							Vector < Selection > sels = new Vector < Selection >();
							IAnswerData data = mTreeElement.Value;
							if (data is SelectMultiData)
							{
								//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
								sels =(Vector < Selection >) data.getValue();
							}
							else if (data is SelectOneData)
							{
								//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
								sels = new Vector < Selection >();
								sels.addElement((Selection) data.Value);
							}
							for (int i = 0; i < sels.size(); i++)
							{
								preselectedValues.addElement(sels.elementAt(i).xmlValue);
							}
						}
						
						//populate 'selection' with the corresponding choices (matching 'value') from the dynamic choiceset
						//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
						Vector < Selection > selection = new Vector < Selection >();
						for (int i = 0; i < preselectedValues.size(); i++)
						{
							System.String value_Renamed = preselectedValues.elementAt(i);
							SelectChoice choice = null;
							for (int j = 0; j < choices.size(); j++)
							{
								SelectChoice ch = choices.elementAt(j);
								if (value_Renamed.Equals(ch.Value))
								{
									choice = ch;
									break;
								}
							}
							//if it's a dynamic question, then there's a good choice what they selected last time
							//will no longer be an option this go around
							if (choice != null)
							{
								selection.addElement(choice.selection());
							}
						}
						
						//convert to IAnswerData
						if (selection.size() == 0)
						{
							return null;
						}
						else if (q.ControlType == Constants.CONTROL_SELECT_MULTI)
						{
							return new SelectMultiData(selection);
						}
						else if (q.ControlType == Constants.CONTROL_SELECT_ONE)
						{
							return new SelectOneData(selection.elementAt(0)); //do something if more than one selected?
						}
						else
						{
							throw new System.SystemException("can't happen");
						}
					}
					else
					{
						return null; //cannot map up selections without <value>
					}
				}
				else
				{
					//static choices
					return mTreeElement.Value;
				}
			}
			
		}
		virtual public System.String AnswerText
		{
			get
			{
				IAnswerData data = this.AnswerValue;
				
				if (data == null)
					return null;
				else
				{
					System.String text;
					
					//csims@dimagi.com - Aug 11, 2010 - Added special logic to
					//capture and display the appropriate value for selections
					//and multi-selects.
					if (data is SelectOneData)
					{
						text = this.getSelectItemText((Selection) data.Value);
					}
					else if (data is SelectMultiData)
					{
						StringBuilder b = new StringBuilder();
						//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
						Vector < Selection > values =(Vector < Selection >) data.getValue();
						//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
						for(Selection value: values)
						{
							b.append(this.getSelectItemText(value_Renamed)).append(" ");
						}
						text = b.toString();
					}
					else
					{
						text = data.DisplayText;
					}
					
					if (ControlType == Constants.CONTROL_SECRET)
					{
						StringBuilder b = new StringBuilder();
						for (int i = 0; i < text.Length; ++i)
						{
							b.append("*");
						}
						text = b.toString();
					}
					return text;
				}
			}
			
		}
		virtual public bool Required
		{
			get
			{
				return mTreeElement.Required;
			}
			
		}
		virtual public bool ReadOnly
		{
			get
			{
				return !mTreeElement.isEnabled();
			}
			
		}
		virtual public QuestionDef Question
		{
			get
			{
				return (QuestionDef) element;
			}
			
		}
		/// <summary> ONLY RELEVANT to Question elements!
		/// Will throw runTimeException if this is called for anything that isn't a Question.
		/// Returns null if no help text is available
		/// </summary>
		/// <returns>
		/// </returns>
		virtual public System.String HelpText
		{
			get
			{
				if (!(element is QuestionDef))
				{
					throw new System.SystemException("Can't get HelpText for Elements that are not Questions!");
				}
				
				System.String textID = ((QuestionDef) element).HelpTextID;
				System.String helpText = ((QuestionDef) element).HelpText;
				System.String helpInnerText = ((QuestionDef) element).HelpInnerText;
				
				try
				{
					if (textID != null)
					{
						helpText = substituteStringArgs(localizer().getLocalizedText(textID));
					}
					else
					{
						helpText = substituteStringArgs(((QuestionDef) element).HelpInnerText);
					}
				}
				catch (NoLocalizedTextException nlt)
				{
					//use fallback helptext
				}
				catch (UnregisteredLocaleException ule)
				{
					System.Console.Error.WriteLine("Warning: No Locale set yet (while attempting to getHelpText())");
				}
				catch (System.Exception e)
				{
					Logger.exception("FormEntryPrompt.getHelpText", e);
					if (e is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
						((org.javarosa.core.io.StreamsUtil.DirectionalIOException) e).printStackTrace();
					else
						SupportClass.WriteStackTrace(e, Console.Error);
				}
				
				return helpText;
			}
			
		}
		
		internal TreeElement mTreeElement;
		internal bool dynamicChoicesPopulated = false;
		
		/// <summary> This empty constructor exists for convenience of any supertypes of this prompt</summary>
		protected internal FormEntryPrompt()
		{
			InitBlock();
		}
		
		/// <summary> Creates a FormEntryPrompt for the element at the given index in the form.
		/// 
		/// </summary>
		/// <param name="form">
		/// </param>
		/// <param name="index">
		/// </param>
		public FormEntryPrompt(FormDef form, FormIndex index):base(form, index)
		{
			InitBlock();
			if (!(element is QuestionDef))
			{
				throw new System.ArgumentException("FormEntryPrompt can only be created for QuestionDef elements");
			}
			this.mTreeElement = form.MainInstance.resolveReference(index.Reference);
		}
		
		public virtual System.String getConstraintText()
		{
			return getConstraintText(null);
		}
		
		public virtual System.String getConstraintText(IAnswerData attemptedValue)
		{
			return getConstraintText(null, attemptedValue);
		}
		
		public virtual System.String getConstraintText(System.String textForm, IAnswerData attemptedValue)
		{
			if (mTreeElement.Constraint == null)
			{
				return null;
			}
			else
			{
				EvaluationContext ec = new EvaluationContext(form.exprEvalContext, mTreeElement.Ref);
				if (textForm != null)
				{
					ec.OutputTextForm = textForm;
				}
				if (attemptedValue != null)
				{
					ec.isConstraint = true;
					ec.candidateValue = attemptedValue;
				}
				return mTreeElement.Constraint.getConstraintMessage(ec, form.MainInstance, textForm);
			}
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public Vector < TreeElement > getBindAttributes()
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public Vector < SelectChoice > getSelectChoices()
		
		public virtual void  expireDynamicChoices()
		{
			dynamicChoicesPopulated = false;
			ItemsetBinding itemset = Question.DynamicChoices;
			if (itemset != null)
			{
				itemset.clearChoices();
			}
		}
		
		//==== observer pattern ====//
		
		public override void  register(IQuestionWidget viewWidget)
		{
			base.register(viewWidget);
			mTreeElement.registerStateObserver(this);
		}
		
		public override void  unregister()
		{
			mTreeElement.unregisterStateObserver(this);
			base.unregister();
		}
		
		public override void  formElementStateChanged(TreeElement instanceNode, int changeFlags)
		{
			if (this.mTreeElement != instanceNode)
				throw new System.SystemException("Widget received event from foreign question");
			if (viewWidget != null)
				viewWidget.refreshWidget(changeFlags);
		}
		
		
		
		/// <summary> Attempts to return the specified Item (from a select or 1select) text.
		/// Will check for text in the following order:<br/>
		/// Localized Text (long form) -> Localized Text (no special form) <br />
		/// If no textID is available, method will return this item's labelInnerText.
		/// </summary>
		/// <param name="sel">the selection (item), if <code>null</code> will throw a IllegalArgumentException
		/// </param>
		/// <returns> Question Text.  <code>null</code> if no text for this element exists (after all fallbacks).
		/// </returns>
		/// <throws>  RunTimeException if this method is called on an element that is NOT a QuestionDef </throws>
		/// <throws>  IllegalArgumentException if Selection is <code>null</code> </throws>
		public virtual System.String getSelectItemText(Selection sel)
		{
			//throw tantrum if this method is called when it shouldn't be or sel==null
			if (!(FormElement is QuestionDef))
				throw new System.SystemException("Can't retrieve question text for non-QuestionDef form elements!");
			if (sel == null)
				throw new System.ArgumentException("Cannot use null as an argument!");
			
			//Just in case the selection hasn't had a chance to be initialized yet.
			if (sel.index == - 1)
			{
				sel.attachChoice(this.Question);
			}
			
			//check for the null id case and return labelInnerText if it is so.
			System.String tid = sel.choice.TextID;
			if (tid == null || (System.Object) tid == (System.Object) "")
				return substituteStringArgs(sel.choice.getLabelInnerText());
			
			//otherwise check for 'long' form of the textID, then for the default form and return
			System.String returnText;
			returnText = getIText(tid, "long");
			if (returnText == null)
				returnText = getIText(tid, null);
			
			return substituteStringArgs(returnText);
		}
		
		/// <seealso cref="getSelectItemText(Selection sel)">
		/// </seealso>
		public virtual System.String getSelectChoiceText(SelectChoice selection)
		{
			return getSelectItemText(selection.selection());
		}
		
		/// <summary> This method is generally used to retrieve special forms for a
		/// (select or 1select) item, e.g. "audio", "video", etc.
		/// 
		/// </summary>
		/// <param name="sel">- The Item whose text you're trying to retrieve.
		/// </param>
		/// <param name="form">- Special text form of Item you're trying to retrieve.
		/// </param>
		/// <returns> Special Form Text. <code>null</code> if no text for this element exists (with the specified special form).
		/// </returns>
		/// <throws>  RunTimeException if this method is called on an element that is NOT a QuestionDef </throws>
		/// <throws>  IllegalArgumentException if <code>sel == null</code> </throws>
		public virtual System.String getSpecialFormSelectItemText(Selection sel, System.String form)
		{
			if (sel == null)
				throw new System.ArgumentException("Cannot use null as an argument for Selection!");
			
			//Just in case the selection hasn't had a chance to be initialized yet.
			if (sel.index == - 1)
			{
				sel.attachChoice(this.Question);
			}
			
			System.String textID = sel.choice.TextID;
			if (textID == null || textID.Equals(""))
				return null;
			
			System.String returnText = getIText(textID, form);
			
			return substituteStringArgs(returnText);
		}
		
		public virtual System.String getSpecialFormSelectChoiceText(SelectChoice sel, System.String form)
		{
			return getSpecialFormSelectItemText(sel.selection(), form);
		}
		
		public virtual void  requestConstraintHint(ConstraintHint hint)
		{
			//NOTE: Technically there's some rep exposure, here. People could use this mechanism to expose the instance.
			//We could hide it by dispatching hints through a final abstract class instead.
			Constraint c = mTreeElement.Constraint;
			if (c != null)
			{
				hint.init(new EvaluationContext(form.exprEvalContext, mTreeElement.Ref), c.constraint, this.form.MainInstance);
			}
			else
			{
				//can't pivot what ain't there.
				throw new UnpivotableExpressionException();
			}
		}
	}
}