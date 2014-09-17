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
using WrappedException = org.javarosa.core.log.WrappedException;
using org.javarosa.core.model.condition;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using SelectMultiData = org.javarosa.core.model.data.SelectMultiData;
using SelectOneData = org.javarosa.core.model.data.SelectOneData;
using Selection = org.javarosa.core.model.data.helper.Selection;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using InstanceInitializationFactory = org.javarosa.core.model.instance.InstanceInitializationFactory;
using InvalidReferenceException = org.javarosa.core.model.instance.InvalidReferenceException;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using RestoreUtils = org.javarosa.core.model.util.restorable.RestoreUtils;
using QuestionPreloader = org.javarosa.core.model.utils.QuestionPreloader;
using Localizable = org.javarosa.core.services.locale.Localizable;
using Localizer = org.javarosa.core.services.locale.Localizer;
using IMetaData = org.javarosa.core.services.storage.IMetaData;
using Persistable = org.javarosa.core.services.storage.Persistable;
using org.javarosa.core.util.externalizable;
using XPathReference = org.javarosa.model.xform.XPathReference;
using XPathException = org.javarosa.xpath.XPathException;
namespace org.javarosa.core.model
{
	
	/// <summary> Definition of a form. This has some meta data about the form definition and a
	/// collection of groups together with question branching or skipping rules.
	/// 
	/// </summary>
	/// <author>  Daniel Kayiwa, Drew Roos
	/// 
	/// </author>
	public class FormDef : IFormElement, Localizable, Persistable, IMetaData
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassIFunctionHandler' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassIFunctionHandler : IFunctionHandler
		{
			public AnonymousClassIFunctionHandler(org.javarosa.core.model.FormDef f, FormDef enclosingInstance)
			{
				InitBlock(f, enclosingInstance);
			}
			private void  InitBlock(org.javarosa.core.model.FormDef f, FormDef enclosingInstance)
			{
				this.f = f;
				this.enclosingInstance = enclosingInstance;
			}
			//UPGRADE_NOTE: Final variable f was copied into class AnonymousClassIFunctionHandler. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1023'"
			private org.javarosa.core.model.FormDef f;
			private FormDef enclosingInstance;
			virtual public System.String Name
			{
				get
				{
					return "jr:itext";
				}
				
			}
			virtual public System.Collections.ArrayList Prototypes
			{
				get
				{
					System.Type[] proto = new System.Type[]{typeof(System.String)};
					System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
					v.Add(proto);
					return v;
				}
				
			}
			public FormDef Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public virtual System.Object eval(System.Object[] args, EvaluationContext ec)
			{
				System.String textID = (System.String) args[0];
				try
				{
					//SUUUUPER HACKY
					System.String form = ec.OutputTextForm;
					if (form != null)
					{
						textID = textID + ";" + form;
						System.String result = f.getLocalizer().getRawText(f.getLocalizer().getLocale(), textID);
						return result == null?"":result;
					}
					else
					{
						System.String text = f.getLocalizer().getText(textID);
						return text == null?"[itext:" + textID + "]":text;
					}
				}
				catch (System.ArgumentOutOfRangeException nsee)
				{
					return "[nolocale]";
				}
			}
			
			public virtual bool rawArgs()
			{
				return false;
			}
			
			public virtual bool realTime()
			{
				return false;
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassIFunctionHandler1' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassIFunctionHandler1 : IFunctionHandler
		{
			public AnonymousClassIFunctionHandler1(org.javarosa.core.model.FormDef f, FormDef enclosingInstance)
			{
				InitBlock(f, enclosingInstance);
			}
			private void  InitBlock(org.javarosa.core.model.FormDef f, FormDef enclosingInstance)
			{
				this.f = f;
				this.enclosingInstance = enclosingInstance;
			}
			//UPGRADE_NOTE: Final variable f was copied into class AnonymousClassIFunctionHandler1. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1023'"
			private org.javarosa.core.model.FormDef f;
			private FormDef enclosingInstance;
			virtual public System.String Name
			{
				get
				{
					return "jr:choice-name";
				}
				
			}
			virtual public System.Collections.ArrayList Prototypes
			{
				get
				{
					System.Type[] proto = new System.Type[]{typeof(System.String), typeof(System.String)};
					System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
					v.Add(proto);
					return v;
				}
				
			}
			public FormDef Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public virtual System.Object eval(System.Object[] args, EvaluationContext ec)
			{
				try
				{
					System.String value_Renamed = (System.String) args[0];
					System.String questionXpath = (System.String) args[1];
					TreeReference ref_Renamed = RestoreUtils.xfFact.ref_Renamed(questionXpath);
					
					QuestionDef q = f.findQuestionByRef(ref_Renamed, f);
					if (q == null || (q.ControlType != Constants.CONTROL_SELECT_ONE && q.ControlType != Constants.CONTROL_SELECT_MULTI))
					{
						return "";
					}
					
					// NOTE: this is highly suspect. We have no context against which to evaluate
					// a dynamic selection list. This will generally cause that evaluation to break
					// if any filtering is done, or, worst case, give unexpected results.
					//
					// We should hook into the existing code (FormEntryPrompt) for pulling
					// display text for select choices. however, it's hard, because we don't really have
					// any context to work with, and all the situations where that context would be used
					// don't make sense for trying to reverse a select value back to a label in an unrelated
					// expression
					;
					ItemsetBinding itemset = q.DynamicChoices;
					if (itemset != null)
					{
						if (itemset.getChoices() == null)
						{
							// NOTE: this will return incorrect results if the list is filtered.
							// fortunately, they are ignored by FormEntryPrompt
							f.populateDynamicChoices(itemset, ref_Renamed);
						}
						choices = itemset.getChoices();
					}
					else
					{
						//static choices
						choices = q.getChoices();
					}
					if (choices != null)
					{
						//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
						for(SelectChoice ch: choices)
						{
							if (ch.getValue().equals(value_Renamed))
							{
								//this is really not ideal. we should hook into the existing code (FormEntryPrompt) for pulling
								//display text for select choices. however, it's hard, because we don't really have
								//any context to work with, and all the situations where that context would be used
								//don't make sense for trying to reverse a select value back to a label in an unrelated
								//expression
								
								System.String textID = ch.getTextID();
								System.String templateStr;
								if (textID != null)
								{
									templateStr = f.getLocalizer().getText(textID);
								}
								else
								{
									templateStr = ch.getLabelInnerText();
								}
								System.String label = Enclosing_Instance.fillTemplateString(templateStr, ref_Renamed);
								return label;
							}
						}
					}
					return "";
				}
				catch (System.Exception e)
				{
					throw new WrappedException("error in evaluation of xpath function [choice-name]", e);
				}
			}
			
			public virtual bool rawArgs()
			{
				return false;
			}
			
			public virtual bool realTime()
			{
				return false;
			}
		}
		private void  InitBlock()
		{
			return Collections.enumeration(formInstances.values());
			if (t.canCascade())
			{
				for (int j = 0; j < t.getTargets().size(); j++)
				{
					TreeReference target = (TreeReference) t.getTargets().elementAt(j);
					//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
					Vector < TreeReference > updatedNodes = new Vector < TreeReference >();
					updatedNodes.addElement(target);
					
					//For certain types of triggerables, the update will affect not only the target, but
					//also the children of the target. In that case, we want to add all of those nodes
					//to the list of updated elements as well.
					if (t.CascadingToChildren)
					{
						addChildrenOfReference(target, updatedNodes);
					}
					
					//Now go through each of these updated nodes (generally just 1 for a normal calculation,
					//multiple nodes if there's a relevance cascade.
					//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
					for(TreeReference ref: updatedNodes)
					{
						//Check our index to see if that target is a Trigger for other conditions
						//IE: if they are an element of a different calculation or relevancy calc
						
						//We can't make this reference generic before now or we'll lose the target information,
						//so we'll be more inclusive than needed and see if any of our triggers are keyed on
						//the predicate-less path of this ref
						//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
						Vector < Triggerable > triggered =(Vector < Triggerable >) triggerIndex.get(ref.hasPredicates() ? ref.removePredicates(): ref);
						
						if (triggered != null)
						{
							//If so, walk all of these triggerables that we found
							for (int k = 0; k < triggered.size(); k++)
							{
								Triggerable u = (Triggerable) triggered.elementAt(k);
								
								//And add them to the queue if they aren't there already
								if (!destination.contains(u))
									destination.addElement(u);
							}
						}
					}
				}
			}
			//add all cascaded triggerables to queue
			
			//Iterate through all of the currently known triggerables to be triggered
			for (int i = 0; i < tv.size(); i++)
			{
				Triggerable t = tv.elementAt(i);
				fillTriggeredElements(t, tv);
			}
			
			//tv should now contain all of the triggerable components which are going to need to be addressed
			//by this update.
			//'triggerables' is topologically-ordered by dependencies, so evaluate the triggerables in 'tv'
			//in the order they appear in 'triggerables'
			for (int i = 0; i < triggerables.size(); i++)
			{
				Triggerable t = triggerables.elementAt(i);
				if (tv.contains(t))
				{
					evaluateTriggerable(t, anchorRef);
				}
			}
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(TreeReference ref: exprEvalContext.expandReference(original))
			{
				addChildrenOfElement(exprEvalContext.resolveReference(ref_Renamed), toAdd);
			}
			for (int i = 0; i < el.getNumChildren(); ++i)
			{
				TreeElement child = el.getChildAt(i);
				toAdd.addElement(child.Ref.genericize());
				addChildrenOfElement(child, toAdd);
			}
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			System.Collections.Hashtable args = new System.Collections.Hashtable();
			
			int depth = 0;
			System.Collections.ArrayList outstandingArgs = Localizer.getArgs(template);
			while (outstandingArgs.Count > 0)
			{
				for (int i = 0; i < outstandingArgs.Count; i++)
				{
					System.String argName = (System.String) outstandingArgs[i];
					if (!args.ContainsKey(argName))
					{
						int ix = - 1;
						try
						{
							ix = System.Int32.Parse(argName);
						}
						catch (System.FormatException nfe)
						{
							System.Console.Error.WriteLine("Warning: expect arguments to be numeric [" + argName + "]");
						}
						
						if (ix < 0 || ix >= outputFragments.size())
							continue;
						
						IConditionExpr expr = outputFragments.elementAt(ix);
						EvaluationContext ec = new EvaluationContext(exprEvalContext, contextRef);
						ec.setOriginalContext(contextRef);
						ec.setVariables(variables);
						System.String value_Renamed = expr.evalReadable(this.MainInstance, ec);
						args[argName] = value_Renamed;
					}
				}
				
				template = Localizer.processArguments(template, args);
				outstandingArgs = Localizer.getArgs(template);
				
				depth++;
				if (depth >= TEMPLATING_RECURSION_LIMIT)
				{
					throw new System.SystemException("Dependency cycle in <output>s; recursion limit exceeded!!");
				}
			}
			
			return template;
		}
		/// <summary> Set the main instance</summary>
		/// <param name="fi">
		/// </param>
		virtual public FormInstance Instance
		{
			get
			{
				return MainInstance;
			}
			
			set
			{
				mainInstance = value;
				value.FormId = ID;
				this.EvaluationContext = new EvaluationContext(null);
				attachControlsToInstanceData();
			}
			
		}
		/// <summary> Get the main instance</summary>
		/// <returns>
		/// </returns>
		virtual public FormInstance MainInstance
		{
			get
			{
				return mainInstance;
			}
			
		}
		virtual public Localizer Localizer
		{
			set
			{
				if (this.localizer != null)
				{
					this.localizer.unregisterLocalizable(this);
				}
				
				this.localizer = value;
				if (this.localizer != null)
				{
					this.localizer.registerLocalizable(this);
				}
			}
			
		}
		virtual public IDataReference Bind
		{
			// don't think this should ever be called(!)
			
			get
			{
				throw new System.SystemException("method not implemented");
			}
			
		}
		/// <param name="ec">The new Evaluation Context
		/// </param>
		virtual public EvaluationContext EvaluationContext
		{
			get
			{
				return this.exprEvalContext;
			}
			
			set
			{
				value = new EvaluationContext(mainInstance, formInstances, value);
				initEvalContext(value);
				this.exprEvalContext = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <returns> the preloads
		/// </returns>
		/// <param name="preloads">the preloads to set
		/// </param>
		virtual public QuestionPreloader Preloader
		{
			get
			{
				return preloader;
			}
			
			set
			{
				this.preloader = value;
			}
			
		}
		public const System.String STORAGE_KEY = "FORMDEF";
		public const int TEMPLATING_RECURSION_LIMIT = 10;
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private Vector < IFormElement > children;
		// <IFormElement>
		/// <summary>A collection of group definitions. </summary>
		private int id;
		/// <summary>The numeric unique identifier of the form definition on the local device </summary>
		private System.String title;
		/// <summary>The display title of the form. </summary>
		private System.String name;
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private Vector < XFormExtension > extensions;
		
		/// <summary> A unique external name that is used to identify the form between machines</summary>
		private Localizer localizer;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public Vector < Triggerable > triggerables;
		// <Triggerable>; this list is topologically ordered, meaning for any tA and tB in
		//the list, where tA comes before tB, evaluating tA cannot depend on any result from evaluating tB
		private bool triggerablesInOrder; //true if triggerables has been ordered topologically (DON'T DELETE ME EVEN THOUGH I'M UNUSED)
		
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private Vector < IConditionExpr > outputFragments; // <IConditionExpr> contents of <output>
		// tags that serve as parameterized
		// arguments to captions
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public HashMap < TreeReference, Vector < Triggerable >> triggerIndex; // <TreeReference, Vector<Triggerable>>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private HashMap < TreeReference, Condition > conditionRepeatTargetIndex;
		// <TreeReference, Condition>;
		// associates repeatable
		// nodes with the Condition
		// that determines their
		// relevancy
		public EvaluationContext exprEvalContext;
		
		private QuestionPreloader preloader = new QuestionPreloader();
		
		//XML ID's cannot start with numbers, so this should never conflict
		private static System.String DEFAULT_SUBMISSION_PROFILE = "1";
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private HashMap < String, SubmissionProfile > submissionProfiles;
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private HashMap < String, FormInstance > formInstances;
		private FormInstance mainInstance = null;
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private HashMap < String, Vector < Action >> eventListeners;
		
		/// <summary> </summary>
		public FormDef()
		{
			InitBlock();
			ID = - 1;
			setChildren(null);
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			triggerables = new Vector < Triggerable >();
			triggerablesInOrder = true;
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			triggerIndex = new HashMap < TreeReference, Vector < Triggerable >>();
			//This is kind of a wreck...
			EvaluationContext = new EvaluationContext(null);
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			outputFragments = new Vector < IConditionExpr >();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			submissionProfiles = new HashMap < String, SubmissionProfile >();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			formInstances = new HashMap < String, FormInstance >();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			eventListeners = new HashMap < String, Vector < Action >>();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			extensions = new Vector < XFormExtension >();
		}
		
		
		
		
		
		/// <summary> Getters and setters for the vectors tha</summary>
		public virtual void  addNonMainInstance(FormInstance instance)
		{
			formInstances.put(instance.Name, instance);
			this.EvaluationContext = new EvaluationContext(null);
		}
		
		/// <summary> Get an instance based on a name</summary>
		/// <param name="name">string name
		/// </param>
		/// <returns>
		/// </returns>
		public virtual FormInstance getNonMainInstance(System.String name)
		{
			if (!formInstances.containsKey(name))
			{
				return null;
			}
			
			return formInstances.get_Renamed(name);
		}
		
		/// <summary> Get the non main instances</summary>
		/// <returns>
		/// </returns>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public Enumeration < FormInstance > getNonMainInstances()
		
		public virtual void  fireEvent()
		{
			
		}
		
		
		// ---------- child elements
		public virtual void  addChild(IFormElement fe)
		{
			this.children.addElement(fe);
		}
		
		public virtual IFormElement getChild(int i)
		{
			if (i < this.children.size())
				return (IFormElement) this.children.elementAt(i);
			
			throw new System.IndexOutOfRangeException("FormDef: invalid child index: " + i + " only " + children.size() + " children");
		}
		
		public virtual IFormElement getChild(FormIndex index)
		{
			IFormElement element = this;
			while (index != null && index.InForm)
			{
				element = element.getChild(index.LocalIndex);
				index = index.NextLevel;
			}
			return element;
		}
		
		/// <summary> Dereference the form index and return a Vector of all interstitial nodes
		/// (top-level parent first; index target last)
		/// 
		/// Ignore 'new-repeat' node for now; just return/stop at ref to
		/// yet-to-be-created repeat node (similar to repeats that already exist)
		/// 
		/// </summary>
		/// <param name="index">
		/// </param>
		/// <returns>
		/// </returns>
		public virtual System.Collections.ArrayList explodeIndex(FormIndex index)
		{
			System.Collections.ArrayList indexes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList multiplicities = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList elements = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			
			collapseIndex(index, indexes, multiplicities, elements);
			return elements;
		}
		
		// take a reference, find the instance node it refers to (factoring in
		// multiplicities)
		/// <param name="index">
		/// </param>
		/// <returns>
		/// </returns>
		public virtual TreeReference getChildInstanceRef(FormIndex index)
		{
			System.Collections.ArrayList indexes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList multiplicities = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList elements = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			
			collapseIndex(index, indexes, multiplicities, elements);
			return getChildInstanceRef(elements, multiplicities);
		}
		
		/// <param name="elements">
		/// </param>
		/// <param name="multiplicities">
		/// </param>
		/// <returns>
		/// </returns>
		public virtual TreeReference getChildInstanceRef(System.Collections.ArrayList elements, System.Collections.ArrayList multiplicities)
		{
			if (elements.Count == 0)
				return null;
			
			// get reference for target element
			TreeReference ref_Renamed = FormInstance.unpackReference(((IFormElement) elements[elements.Count - 1]).Bind).clone();
			for (int i = 0; i < ref_Renamed.size(); i++)
			{
				//There has to be a better way to encapsulate this
				if (ref_Renamed.getMultiplicity(i) != TreeReference.INDEX_ATTRIBUTE)
				{
					ref_Renamed.setMultiplicity(i, 0);
				}
			}
			
			// fill in multiplicities for repeats along the way
			for (int i = 0; i < elements.Count; i++)
			{
				IFormElement temp = (IFormElement) elements[i];
				if (temp is GroupDef && ((GroupDef) temp).Repeat)
				{
					TreeReference repRef = FormInstance.unpackReference(temp.Bind);
					if (repRef.isParentOf(ref_Renamed, false))
					{
						int repMult = ((System.Int32) multiplicities[i]);
						ref_Renamed.setMultiplicity(repRef.size() - 1, repMult);
					}
					else
					{
						return null; // question/repeat hierarchy is not consistent
						// with instance instance and bindings
					}
				}
			}
			
			return ref_Renamed;
		}
		
		public virtual void  setValue(IAnswerData data, TreeReference ref_Renamed)
		{
			setValue(data, ref_Renamed, mainInstance.resolveReference(ref_Renamed));
		}
		
		public virtual void  setValue(IAnswerData data, TreeReference ref_Renamed, TreeElement node)
		{
			setAnswer(data, node);
			triggerTriggerables(ref_Renamed);
			//TODO: pre-populate fix-count repeats here?
		}
		
		public virtual void  setAnswer(IAnswerData data, TreeReference ref_Renamed)
		{
			setAnswer(data, mainInstance.resolveReference(ref_Renamed));
		}
		
		public virtual void  setAnswer(IAnswerData data, TreeElement node)
		{
			node.setAnswer(data);
		}
		
		/// <summary> Deletes the inner-most repeat that this node belongs to and returns the
		/// corresponding FormIndex. Behavior is currently undefined if you call this
		/// method on a node that is not contained within a repeat.
		/// 
		/// </summary>
		/// <param name="index">
		/// </param>
		/// <returns>
		/// </returns>
		public virtual FormIndex deleteRepeat(FormIndex index)
		{
			System.Collections.ArrayList indexes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList multiplicities = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList elements = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			collapseIndex(index, indexes, multiplicities, elements);
			
			// loop backwards through the elements, removing objects from each
			// vector, until we find a repeat
			// TODO: should probably check to make sure size > 0
			for (int i = elements.Count - 1; i >= 0; i--)
			{
				IFormElement e = (IFormElement) elements[i];
				if (e is GroupDef && ((GroupDef) e).Repeat)
				{
					break;
				}
				else
				{
					indexes.RemoveAt(i);
					multiplicities.RemoveAt(i);
					elements.RemoveAt(i);
				}
			}
			
			// build new formIndex which includes everything
			// up to the node we're going to remove
			FormIndex newIndex = buildIndex(indexes, multiplicities, elements);
			
			TreeReference deleteRef = getChildInstanceRef(newIndex);
			TreeElement deleteElement = mainInstance.resolveReference(deleteRef);
			TreeReference parentRef = deleteRef.ParentRef;
			TreeElement parentElement = mainInstance.resolveReference(parentRef);
			
			int childMult = deleteElement.Mult;
			parentElement.removeChild(deleteElement);
			
			// update multiplicities of other child nodes
			for (int i = 0; i < parentElement.NumChildren; i++)
			{
				TreeElement child = parentElement.getChildAt(i);
				if (child.Name.Equals(deleteElement.Name) && child.Mult > childMult)
				{
					child.Mult = child.Mult - 1;
				}
			}
			
			triggerTriggerables(deleteRef);
			return newIndex;
		}
		
		public virtual void  createNewRepeat(FormIndex index)
		{
			TreeReference destRef = getChildInstanceRef(index);
			TreeElement template = mainInstance.getTemplate(destRef);
			
			mainInstance.copyNode(template, destRef);
			
			preloadInstance(mainInstance.resolveReference(destRef));
			
			//2013-05-14 - ctsims - Events should get fired _before_ calculate stuff is fired, moved
			//this above triggering triggerables
			//Grab any actions listening to this event
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(Action a: listeners)
			{
				a.processAction(this, destRef);
			}
			
			triggerTriggerables(destRef); // trigger conditions that depend on the creation of this new node
			initializeTriggerables(destRef); // initialize conditions for the node (and sub-nodes)
		}
		
		public virtual bool isRepeatRelevant(TreeReference repeatRef)
		{
			bool relev = true;
			
			Condition c = conditionRepeatTargetIndex.get_Renamed(repeatRef.genericize());
			if (c != null)
			{
				relev = c.evalBool(mainInstance, new EvaluationContext(exprEvalContext, repeatRef));
			}
			
			//check the relevancy of the immediate parent
			if (relev)
			{
				TreeElement templNode = mainInstance.getTemplate(repeatRef);
				TreeReference parentPath = templNode.Parent.getRef().genericize();
				TreeElement parentNode = mainInstance.resolveReference(parentPath.contextualize(repeatRef));
				relev = parentNode.isRelevant();
			}
			
			return relev;
		}
		
		public virtual bool canCreateRepeat(TreeReference repeatRef, FormIndex repeatIndex)
		{
			GroupDef repeat = (GroupDef) this.getChild(repeatIndex);
			
			//Check to see if this repeat can have children added by the user
			if (repeat.noAddRemove)
			{
				//Check to see if there's a count to use to determine how many children this repeat
				//should have
				if (repeat.CountReference != null)
				{
					int currentMultiplicity = repeatIndex.ElementMultiplicity;
					
					// Lu Gram: the count XPath needs to be contextualized for nested repeat groups...
					TreeReference countRef = FormInstance.unpackReference(repeat.CountReference);
					TreeElement countNode = this.MainInstance.resolveReference(countRef.contextualize(repeatRef));
					if (countNode == null)
					{
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						throw new System.SystemException("Could not locate the repeat count value expected at " + repeat.CountReference.Reference.ToString());
					}
					//get the total multiplicity possible
					IAnswerData count = countNode.Value;
					long fullcount = count == null?0:(System.Int32) count.Value;
					
					if (fullcount <= currentMultiplicity)
					{
						return false;
					}
				}
				else
				{
					//Otherwise the user can never add repeat instances
					return false;
				}
			}
			
			//TODO: If we think the node is still relevant, we also need to figure out a way to test that assumption against
			//the repeat's constraints.
			
			
			return true;
		}
		
		public virtual void  copyItemsetAnswer(QuestionDef q, TreeElement targetNode, IAnswerData data)
		{
			ItemsetBinding itemset = q.DynamicChoices;
			TreeReference targetRef = targetNode.Ref;
			TreeReference destRef = itemset.getDestRef().contextualize(targetRef);
			
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Vector < String > selectedValues = new Vector < String >();
			if (data is SelectMultiData)
			{
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				selections =(Vector < Selection >) data.getValue();
			}
			else if (data is SelectOneData)
			{
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				selections = new Vector < Selection >();
				selections.addElement((Selection) data.Value);
			}
			if (itemset.valueRef != null)
			{
				for (int i = 0; i < selections.size(); i++)
				{
					selectedValues.addElement(selections.elementAt(i).choice.getValue());
				}
			}
			
			//delete existing dest nodes that are not in the answer selection
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			HashMap < String, TreeElement > existingValues = new HashMap < String, TreeElement >();
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			for (int i = 0; i < existingNodes.size(); i++)
			{
				TreeElement node = MainInstance.resolveReference(existingNodes.elementAt(i));
				
				if (itemset.valueRef != null)
				{
					System.String value_Renamed = itemset.RelativeValue.evalReadable(this.MainInstance, new EvaluationContext(exprEvalContext, node.Ref));
					if (selectedValues.contains(value_Renamed))
					{
						existingValues.put(value_Renamed, node); //cache node if in selection and already exists
					}
				}
				
				//delete from target
				targetNode.removeChild(node);
			}
			
			//copy in nodes for new answer; preserve ordering in answer
			for (int i = 0; i < selections.size(); i++)
			{
				Selection s = selections.elementAt(i);
				SelectChoice ch = s.choice;
				
				TreeElement cachedNode = null;
				if (itemset.valueRef != null)
				{
					System.String value_Renamed = ch.Value;
					if (existingValues.containsKey(value_Renamed))
					{
						cachedNode = existingValues.get_Renamed(value_Renamed);
					}
				}
				
				if (cachedNode != null)
				{
					cachedNode.Mult = i;
					targetNode.addChild(cachedNode);
				}
				else
				{
					MainInstance.copyItemsetNode(ch.copyNode, destRef, this);
				}
			}
			
			triggerTriggerables(destRef); // trigger conditions that depend on the creation of these new nodes
			initializeTriggerables(destRef); // initialize conditions for the node (and sub-nodes)
			//not 100% sure this will work since destRef is ambiguous as the last step, but i think it's supposed to work
		}
		
		/// <summary> Add a Condition to the form's Collection.
		/// 
		/// </summary>
		/// <param name="t">the condition to be set
		/// </param>
		public virtual Triggerable addTriggerable(Triggerable t)
		{
			int existingIx = triggerables.indexOf(t);
			if (existingIx >= 0)
			{
				//one node may control access to many nodes; this means many nodes effectively have the same condition
				//let's identify when conditions are the same, and store and calculate it only once
				
				//nov-2-2011: ctsims - We need to merge the context nodes together whenever we do this (finding the highest
				//common ground between the two), otherwise we can end up failing to trigger when the ignored context
				//exists and the used one doesn't
				
				Triggerable existingTriggerable = triggerables.elementAt(existingIx);
				
				existingTriggerable.contextRef = existingTriggerable.contextRef.intersect(t.contextRef);
				
				return existingTriggerable;
				
				//note, if the contextRef is unnecessarily deep, the condition will be evaluated more times than needed
				//perhaps detect when 'identical' condition has a shorter contextRef, and use that one instead?
			}
			else
			{
				triggerables.addElement(t);
				triggerablesInOrder = false;
				
				//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				for(TreeReference trigger: triggers)
				{
					if (!triggerIndex.containsKey(trigger))
					{
						//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
						triggerIndex.put(trigger.clone(), new Vector < Triggerable >());
					}
					//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
					if (!triggered.contains(t))
					{
						triggered.addElement(t);
					}
				}
				
				return t;
			}
		}
		
		/// <summary> Finalize the DAG associated with the form's triggered conditions. This will create
		/// the appropriate ordering and dependencies to ensure the conditions will be evaluated
		/// in the appropriate orders.
		/// 
		/// </summary>
		/// <throws>  IllegalStateException - If the trigger ordering contains an illegal cycle and the </throws>
		/// <summary> triggers can't be laid out appropriately
		/// </summary>
		public virtual void  finalizeTriggerables()
		{
			//
			//DAGify the triggerables based on dependencies and sort them so that
			//trigbles come only after the trigbles they depend on
			//
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Vector < Triggerable [] > partialOrdering = new Vector < Triggerable [] >();
			for (int i = 0; i < triggerables.size(); i++)
			{
				Triggerable t = triggerables.elementAt(i);
				
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				Vector < Triggerable > deps = new Vector < Triggerable >();
				fillTriggeredElements(t, deps);
				
				for (int j = 0; j < deps.size(); j++)
				{
					Triggerable u = deps.elementAt(j);
					Triggerable[] edge = new Triggerable[]{t, u};
					partialOrdering.addElement(edge);
				}
			}
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Vector < Triggerable > vertices = new Vector < Triggerable >();
			for (int i = 0; i < triggerables.size(); i++)
				vertices.addElement(triggerables.elementAt(i));
			triggerables.removeAllElements();
			
			while (vertices.size() > 0)
			{
				//determine root nodes
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				Vector < Triggerable > roots = new Vector < Triggerable >();
				for (int i = 0; i < vertices.size(); i++)
				{
					roots.addElement(vertices.elementAt(i));
				}
				for (int i = 0; i < partialOrdering.size(); i++)
				{
					Triggerable[] edge = partialOrdering.elementAt(i);
					roots.removeElement(edge[1]);
				}
				
				//if no root nodes while graph still has nodes, graph has cycles
				if (roots.size() == 0)
				{
					System.String hints = "";
					//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
					for(Triggerable t: vertices)
					{
						//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
						for(TreeReference r: t.getTargets())
						{
							hints += ("\n" + r.toString(true));
						}
					}
					System.String message = "Cycle detected in form's relevant and calculation logic!";
					if (!hints.Equals(""))
					{
						message += ("\nThe following nodes are likely involved in the loop:" + hints);
					}
					throw new System.SystemException(message);
				}
				
				//remove root nodes and edges originating from them
				for (int i = 0; i < roots.size(); i++)
				{
					Triggerable root = roots.elementAt(i);
					triggerables.addElement(root);
					vertices.removeElement(root);
				}
				for (int i = partialOrdering.size() - 1; i >= 0; i--)
				{
					Triggerable[] edge = partialOrdering.elementAt(i);
					if (roots.contains(edge[0]))
						partialOrdering.removeElementAt(i);
				}
			}
			
			triggerablesInOrder = true;
			
			//
			//build the condition index for repeatable nodes
			//
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			conditionRepeatTargetIndex = new HashMap < TreeReference, Condition >();
			for (int i = 0; i < triggerables.size(); i++)
			{
				Triggerable t = triggerables.elementAt(i);
				if (t is Condition)
				{
					System.Collections.ArrayList targets = t.getTargets();
					for (int j = 0; j < targets.Count; j++)
					{
						TreeReference target = (TreeReference) targets[j];
						if (mainInstance.getTemplate(target) != null)
						{
							conditionRepeatTargetIndex.put(target, (Condition) t);
						}
					}
				}
			}
		}
		
		/// <summary> Get all of the elements which will need to be evaluated (in order) when the
		/// triggerable is fired.
		/// </summary>
		/// <param name="t">
		/// </param>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		void fillTriggeredElements(Triggerable t, Vector < Triggerable > destination)
		
		
		
		
		
		public virtual void  initializeTriggerables()
		{
			initializeTriggerables(TreeReference.rootRef());
		}
		
		/// <summary> Walks the current set of conditions, and evaluates each of them with the
		/// current context.
		/// </summary>
		private void  initializeTriggerables(TreeReference rootRef)
		{
			TreeReference genericRoot = rootRef.genericize();
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Vector < Triggerable > applicable = new Vector < Triggerable >();
			for (int i = 0; i < triggerables.size(); i++)
			{
				Triggerable t = triggerables.elementAt(i);
				for (int j = 0; j < t.getTargets().size(); j++)
				{
					TreeReference target = t.getTargets().elementAt(j);
					if (genericRoot.isParentOf(target, false))
					{
						applicable.addElement(t);
						break;
					}
				}
			}
			
			evaluateTriggerables(applicable, rootRef);
		}
		
		/// <summary> The entry point for the DAG cascade after a value is changed in the model.
		/// 
		/// </summary>
		/// <param name="ref">The full contextualized unambiguous reference of the value that was
		/// changed.
		/// </param>
		public virtual void  triggerTriggerables(TreeReference ref_Renamed)
		{
			
			//turn unambiguous ref into a generic ref
			//to identify what nodes should be triggered by this
			//reference changing
			TreeReference genericRef = ref_Renamed.genericize();
			
			//get triggerables which are activated by the generic reference
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			if (triggered == null)
			{
				return ;
			}
			
			//Our vector doesn't have a shallow copy op, so make one
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Vector < Triggerable > triggeredCopy = new Vector < Triggerable >();
			for (int i = 0; i < triggered.size(); i++)
			{
				triggeredCopy.addElement(triggered.elementAt(i));
			}
			
			//Evaluate all of the triggerables in our new vector
			evaluateTriggerables(triggeredCopy, ref_Renamed);
		}
		
		/// <summary> Step 2 in evaluating DAG computation updates from a value being changed in
		/// the instance. This step is responsible for taking the root set of directly
		/// triggered conditions, identifying which conditions should further be triggered
		/// due to their update, and then dispatching all of the evaluations.
		/// 
		/// </summary>
		/// <param name="tv">A vector of all of the trigerrables directly triggered by the
		/// value changed
		/// </param>
		/// <param name="anchorRef">The reference to original value that was updated
		/// </param>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		void evaluateTriggerables(Vector < Triggerable > tv, TreeReference anchorRef)
		
		/// <summary> Step 3 in DAG cascade. evaluate the individual triggerable expressions against
		/// the anchor (the value that changed which triggered recomputation)
		/// 
		/// </summary>
		/// <param name="t">The triggerable to be updated
		/// </param>
		/// <param name="anchorRef">The reference to the value which was changed.
		/// </param>
		private void  evaluateTriggerable(Triggerable t, TreeReference anchorRef)
		{
			
			//Contextualize the reference used by the triggerable against the anchor
			TreeReference contextRef = t.contextRef.contextualize(anchorRef);
			try
			{
				
				//Now identify all of the fully qualified nodes which this triggerable
				//updates. (Multiple nodes can be updated by the same trigger)
				//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
				
				//Go through each one and evaluate the trigger expresion
				for (int i = 0; i < v.size(); i++)
				{
					EvaluationContext ec = new EvaluationContext(exprEvalContext, v.elementAt(i));
					t.apply(mainInstance, ec, v.elementAt(i), this);
				}
			}
			catch (System.Exception e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new WrappedException("Error evaluating field '" + contextRef.NameLast + "': " + e.Message, e);
			}
		}
		
		/// <summary> This is a utility method to get all of the references of a node. It can be replaced
		/// when we support dependent XPath Steps (IE: /path/to//)
		/// </summary>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		void addChildrenOfReference(TreeReference original, Vector < TreeReference > toAdd)
		
		//Recursive step of utility method
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		void addChildrenOfElement(TreeElement el, Vector < TreeReference > toAdd)
		
		public virtual bool evaluateConstraint(TreeReference ref_Renamed, IAnswerData data)
		{
			if (data == null)
			{
				return true;
			}
			
			TreeElement node = mainInstance.resolveReference(ref_Renamed);
			Constraint c = node.Constraint;
			
			if (c == null)
			{
				return true;
			}
			EvaluationContext ec = new EvaluationContext(exprEvalContext, ref_Renamed);
			ec.isConstraint = true;
			ec.candidateValue = data;
			
			return c.constraint.eval(mainInstance, ec);
		}
		
		private void  initEvalContext(EvaluationContext ec)
		{
			if (!ec.FunctionHandlers.ContainsKey("jr:itext"))
			{
				//UPGRADE_NOTE: Final was removed from the declaration of 'f '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
				FormDef f = this;
				ec.addFunctionHandler(new AnonymousClassIFunctionHandler(f, this));
			}
			
			/* function to reverse a select value into the display label for that choice in the question it came from
			*
			* arg 1: select value
			* arg 2: string xpath referring to origin question; must be absolute path
			*
			* this won't work at all if the original label needed to be processed/calculated in some way (<output>s, etc.) (is this even allowed?)
			* likely won't work with multi-media labels
			* _might_ work for itemsets, but probably not very well or at all; could potentially work better if we had some context info
			* DOES work with localization
			*
			* it's mainly intended for the simple case of reversing a question with compile-time-static fields, for use inside an <output>
			*/
			if (!ec.FunctionHandlers.ContainsKey("jr:choice-name"))
			{
				//UPGRADE_NOTE: Final was removed from the declaration of 'f '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
				FormDef f = this;
				ec.addFunctionHandler(new AnonymousClassIFunctionHandler1(f, this));
			}
		}
		
		public virtual System.String fillTemplateString(System.String template, TreeReference contextRef)
		{
			return fillTemplateString_Renamed_Field;
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			, new HashMap < String, Object >());
		}
		
		public System.String fillTemplateString_Renamed_Field;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		(String template, TreeReference contextRef, HashMap < String, ? > variables)
		
		/// <summary> Identify the itemset in the backend model, and create a set of SelectChoice
		/// objects at the current question reference based on the data in the model.
		/// 
		/// Will modify the itemset binding to contain the relevant choices
		/// 
		/// </summary>
		/// <param name="itemset">The binding for an itemset, where the choices will be populated
		/// </param>
		/// <param name="curQRef">A reference to the current question's element, which will be
		/// used to determine the values to be chosen from.
		/// </param>
		public virtual void  populateDynamicChoices(ItemsetBinding itemset, TreeReference curQRef)
		{
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Vector < SelectChoice > choices = new Vector < SelectChoice >();
			
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			
			FormInstance fi = null;
			if (itemset.nodesetRef.InstanceName != null)
			//We're not dealing with the default instance
			{
				fi = getNonMainInstance(itemset.nodesetRef.InstanceName);
				if (fi == null)
				{
					throw new XPathException("Instance " + itemset.nodesetRef.InstanceName + " not found");
				}
			}
			else
			{
				fi = MainInstance;
			}
			
			
			if (matches == null)
			{
				throw new XPathException("Could not find references depended on by" + itemset.nodesetRef.InstanceName);
			}
			
			for (int i = 0; i < matches.size(); i++)
			{
				TreeReference item = matches.elementAt(i);
				
				//String label = itemset.labelExpr.evalReadable(this.getMainInstance(), new EvaluationContext(exprEvalContext, item));
				System.String label = itemset.labelExpr.evalReadable(fi, new EvaluationContext(exprEvalContext, item));
				System.String value_Renamed = null;
				TreeElement copyNode = null;
				
				if (itemset.copyMode)
				{
					copyNode = this.MainInstance.resolveReference(itemset.copyRef.contextualize(item));
				}
				if (itemset.valueRef != null)
				{
					//value = itemset.valueExpr.evalReadable(this.getMainInstance(), new EvaluationContext(exprEvalContext, item));
					value_Renamed = itemset.valueExpr.evalReadable(fi, new EvaluationContext(exprEvalContext, item));
				}
				//			SelectChoice choice = new SelectChoice(labelID,labelInnerText,value,isLocalizable);
				SelectChoice choice = new SelectChoice(label, value_Renamed != null?value_Renamed:"dynamic:" + i, itemset.labelIsItext);
				choice.Index = i;
				if (itemset.copyMode)
					choice.copyNode = copyNode;
				
				choices.addElement(choice);
			}
			
			if (choices.size() == 0)
			{
				//throw new RuntimeException("dynamic select question has no choices! [" + itemset.nodesetRef + "]");
				//When you exit a survey mid way through and want to save it, it seems that Collect wants to
				//go through all the questions. Well of course not all the questions are going to have answers
				//to chose from if the user hasn't filled them out. So I'm just going to make a note of this
				//and not throw an exception.
				System.Console.Out.WriteLine("Dynamic select question has no choices! [" + itemset.nodesetRef + "]. If this occurs while filling out a form (and not while saving an incomplete form), the filter condition may have eliminated all the choices. Is that what you intended?\n");
			}
			
			itemset.setChoices(choices, this.Localizer);
		}
		
		/*
		* (non-Javadoc)
		*
		* @see
		* org.javarosa.core.model.utils.Localizable#localeChanged(java.lang.String,
		* org.javarosa.core.model.utils.Localizer)
		*/
		public virtual void  localeChanged(System.String locale, Localizer localizer)
		{
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator e = children.elements(); e.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				((IFormElement) e.Current).localeChanged(locale, localizer);
			}
		}
		
		public override System.String ToString()
		{
			return getTitle();
		}
		
		/// <summary> Preload the Data Model with the preload values that are enumerated in the
		/// data bindings.
		/// </summary>
		public virtual void  preloadInstance(TreeElement node)
		{
			// if (node.isLeaf()) {
			IAnswerData preload = null;
			if (node.PreloadHandler != null)
			{
				preload = preloader.getQuestionPreload(node.PreloadHandler, node.PreloadParams);
			}
			if (preload != null)
			{
				// what if we want to wipe out a value in the
				// instance?
				node.setAnswer(preload);
			}
			// } else {
			if (!node.Leaf)
			{
				for (int i = 0; i < node.NumChildren; i++)
				{
					TreeElement child = node.getChildAt(i);
					if (child.Mult != TreeReference.INDEX_TEMPLATE)
					// don't preload templates; new repeats are preloaded as they're created
						preloadInstance(child);
				}
			}
			// }
		}
		
		public virtual bool postProcessInstance()
		{
			dispatchFormEvent(Action.EVENT_XFORMS_REVALIDATE);
			return postProcessInstance(mainInstance.getRoot());
		}
		
		/// <summary> Iterate over the form's data bindings, and evaluate all post procesing
		/// calls.
		/// 
		/// </summary>
		/// <returns> true if the instance was modified in any way. false otherwise.
		/// </returns>
		private bool postProcessInstance(TreeElement node)
		{
			// we might have issues with ordering, for example, a handler that writes a value to a node,
			// and a handler that does something external with the node. if both handlers are bound to the
			// same node, we need to make sure the one that alters the node executes first. deal with that later.
			// can we even bind multiple handlers to the same node currently?
			
			// also have issues with conditions. it is hard to detect what conditions are affected by the actions
			// of the post-processor. normally, it wouldn't matter because we only post-process when we are exiting
			// the form, so the result of any triggered conditions is irrelevant. however, if we save a form in the
			// interim, post-processing occurs, and then we continue to edit the form. it seems like having conditions
			// dependent on data written during post-processing is a bad practice anyway, and maybe we shouldn't support it.
			
			if (node.Leaf)
			{
				if (node.PreloadHandler != null)
				{
					return preloader.questionPostProcess(node, node.PreloadHandler, node.PreloadParams);
				}
				else
				{
					return false;
				}
			}
			else
			{
				bool instanceModified = false;
				for (int i = 0; i < node.NumChildren; i++)
				{
					TreeElement child = node.getChildAt(i);
					if (child.Mult != TreeReference.INDEX_TEMPLATE)
						instanceModified |= postProcessInstance(child);
				}
				return instanceModified;
			}
		}
		
		/// <summary> Reads the form definition object from the supplied stream.
		/// 
		/// Requires that the instance has been set to a prototype of the instance that
		/// should be used for deserialization.
		/// 
		/// </summary>
		/// <param name="dis">- the stream to read from.
		/// </param>
		/// <throws>  IOException </throws>
		/// <throws>  InstantiationException </throws>
		/// <throws>  IllegalAccessException </throws>
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader dis, PrototypeFactory pf)
		{
			ID = ExtUtil.readInt(dis);
			setName(ExtUtil.nullIfEmpty(ExtUtil.readString(dis)));
			setTitle((System.String) ExtUtil.read(dis, new ExtWrapNullable(typeof(System.String)), pf));
			setChildren((System.Collections.ArrayList) ExtUtil.read(dis, new ExtWrapListPoly(), pf));
			Instance = (FormInstance) ExtUtil.read(dis, typeof(FormInstance), pf);
			
			Localizer = (Localizer) ExtUtil.read(dis, new ExtWrapNullable(typeof(Localizer)), pf);
			
			System.Collections.ArrayList vcond = (System.Collections.ArrayList) ExtUtil.read(dis, new ExtWrapList(typeof(Condition)), pf);
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator e = vcond.GetEnumerator(); e.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				addTriggerable((Condition) e.Current);
			}
			System.Collections.ArrayList vcalc = (System.Collections.ArrayList) ExtUtil.read(dis, new ExtWrapList(typeof(Recalculate)), pf);
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator e = vcalc.GetEnumerator(); e.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				addTriggerable((Recalculate) e.Current);
			}
			finalizeTriggerables();
			
			outputFragments = (System.Collections.ArrayList) ExtUtil.read(dis, new ExtWrapListPoly(), pf);
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			submissionProfiles =(HashMap < String, SubmissionProfile >) ExtUtil.read(dis, new ExtWrapMap(String.
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		class, SubmissionProfile.
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		class));
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		formInstances =(HashMap < String, FormInstance >) ExtUtil.read(dis, new ExtWrapMap(String.
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		class, FormInstance.
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		class));
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		eventListeners =(HashMap < String, Vector < Action >>) ExtUtil.read(dis, new ExtWrapMap(String.
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		class, new ExtWrapListPoly()), pf);
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		extensions =(Vector) ExtUtil.read(dis, new ExtWrapListPoly(), pf);
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		setEvaluationContext(new EvaluationContext(null));
	}
	
	/// <summary> meant to be called after deserialization and initialization of handlers
	/// 
	/// </summary>
	/// <param name="newInstance">true if the form is to be used for a new entry interaction,
	/// false if it is using an existing IDataModel
	/// </param>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void initialize(boolean newInstance, InstanceInitializationFactory factory)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		for(String instanceId: formInstances.keySet())
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		FormInstance instance = formInstances.get(instanceId);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	instance.initialize(factory, instanceId);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	if(newInstance)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ // only preload new forms (we may have to revisit
	// this)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	preloadInstance(mainInstance.getRoot());
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	if(getLocalizer() != null && getLocalizer().getLocale() == null)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		getLocalizer().setToDefault();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//TODO: Hm, not 100% sure that this is right. Maybe we should be
	//using a slightly different event for "First Load" which doesn't
	//get fired again, but always fire this one?
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	if(newInstance)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		dispatchFormEvent(Action.EVENT_XFORMS_READY);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	initializeTriggerables();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	/// <summary> Writes the form definition object to the supplied stream.
	/// 
	/// </summary>
	/// <param name="dos">- the stream to write to.
	/// </param>
	/// <throws>  IOException </throws>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void writeExternal(DataOutputStream dos) throws IOException
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		ExtUtil.writeNumeric(dos, getID());
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.writeString(dos, ExtUtil.emptyIfNull(getName()));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(dos, new ExtWrapNullable(getTitle()));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(dos, new ExtWrapListPoly(getChildren()));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(dos, getMainInstance());
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(dos, new ExtWrapNullable(localizer));
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	Vector conditions = new Vector();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	Vector recalcs = new Vector();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	for(int i = 0;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	i < triggerables.size();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	i ++)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		Triggerable t =(Triggerable) triggerables.elementAt(i);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	if(t instanceof Condition)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		conditions.addElement(t);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} else if(t instanceof Recalculate)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		recalcs.addElement(t);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	ExtUtil.write(dos, new ExtWrapList(conditions));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(dos, new ExtWrapList(recalcs));
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(dos, new ExtWrapListPoly(outputFragments));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(dos, new ExtWrapMap(submissionProfiles));
	
	//for support of multi-instance forms
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(dos, new ExtWrapMap(formInstances));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(dos, new ExtWrapMap(eventListeners, new ExtWrapListPoly()));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(dos, new ExtWrapListPoly(extensions));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void collapseIndex(FormIndex index, Vector indexes, Vector multiplicities, Vector elements)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		if(!index.isInForm())
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		return;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	IFormElement element = this;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	while(index != null)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		int i = index.getLocalIndex();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	element = element.getChild(i);
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	indexes.addElement(new Integer(i));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	multiplicities.addElement(new Integer(index.getInstanceIndex() == - 1 ? 0: index.getInstanceIndex()));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	elements.addElement(element);
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	index = index.getNextLevel();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public FormIndex buildIndex(Vector indexes, Vector multiplicities, Vector elements)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		FormIndex cur = null;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	Vector curMultiplicities = new Vector();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	for(int j = 0;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	j < multiplicities.size();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	++ j)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		curMultiplicities.addElement(multiplicities.elementAt(j));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	Vector curElements = new Vector();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	for(int j = 0;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	j < elements.size();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	++ j)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		curElements.addElement(elements.elementAt(j));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	for(int i = indexes.size() - 1;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	i >= 0;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	i --)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		int ix =((Integer) indexes.elementAt(i)).intValue();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	int mult =((Integer) multiplicities.elementAt(i)).intValue();
	
	//----begin unclear why this is here... side effects???
	//TODO: ... No words. Just fix it.
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	IFormElement ife =(IFormElement) elements.elementAt(i);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	XPathReference xpr =(ife != null) ?(XPathReference) ife.getBind(): null;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	TreeReference ref =(xpr != null) ?(TreeReference) xpr.getReference(): null;
	//----end
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	if(!(elements.elementAt(i) instanceof GroupDef &&((GroupDef) elements.elementAt(i)).getRepeat()))
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		mult = - 1;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	cur = new FormIndex(cur, ix, mult, getChildInstanceRef(curElements, curMultiplicities));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	curMultiplicities.removeElementAt(curMultiplicities.size() - 1);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	curElements.removeElementAt(curElements.size() - 1);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	return cur;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public int getNumRepetitions(FormIndex index)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		Vector indexes = new Vector();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	Vector multiplicities = new Vector();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	Vector elements = new Vector();
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	if(!index.isInForm())
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		throw new RuntimeException(not an in-form index);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	collapseIndex(index, indexes, multiplicities, elements);
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	if(!(elements.lastElement() instanceof GroupDef) || !((GroupDef) elements.lastElement()).getRepeat())
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		throw new RuntimeException(current element not a repeat);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//so painful
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	TreeElement templNode = mainInstance.getTemplate(index.getReference());
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	TreeReference parentPath = templNode.getParent().getRef().genericize();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	TreeElement parentNode = mainInstance.resolveReference(parentPath.contextualize(index.getReference()));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	return parentNode.getChildMultiplicity(templNode.getName());
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//repIndex == -1 => next repetition about to be created
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public FormIndex descendIntoRepeat(FormIndex index, int repIndex)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		int numRepetitions = getNumRepetitions(index);
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	Vector indexes = new Vector();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	Vector multiplicities = new Vector();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	Vector elements = new Vector();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	collapseIndex(index, indexes, multiplicities, elements);
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	if(repIndex == - 1)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		repIndex = numRepetitions;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} else
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		if(repIndex < 0 || repIndex >= numRepetitions)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		throw new RuntimeException(selection exceeds current number of repetitions);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	multiplicities.setElementAt(new Integer(repIndex), multiplicities.size() - 1);
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	return buildIndex(indexes, multiplicities, elements);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	/*
	* (non-Javadoc)
	*
	* @see org.javarosa.core.model.IFormElement#getDeepChildCount()
	*/
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public int getDeepChildCount()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		int total = 0;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	Enumeration e = children.elements();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	while(e.hasMoreElements())
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		total +=((IFormElement) e.nextElement()).getDeepChildCount();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	return total;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void registerStateObserver(FormElementStateListener qsl)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{
	// NO. (Or at least not yet).
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void unregisterStateObserver(FormElementStateListener qsl)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{
	// NO. (Or at least not yet).
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public Vector getChildren()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		return children;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void setChildren(Vector < IFormElement > children)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		this.children =(children == null ? new Vector(): children);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public String getTitle()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		return title;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void setTitle(String title)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		this.title = title;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public int getID()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		return id;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void setID(int id)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		this.id = id;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public String getName()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		return name;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void setName(String name)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		this.name = name;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public Localizer getLocalizer()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		return localizer;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public Vector getOutputFragments()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		return outputFragments;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void setOutputFragments(Vector outputFragments)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		this.outputFragments = outputFragments;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public HashMap getMetaData()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		HashMap metadata = new HashMap();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	String [] fields = getMetaDataFields();
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	for(int i = 0;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	i < fields.length;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	i ++)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	try
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		metadata.put(fields [i], getMetaData(fields [i]));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	catch(NullPointerException npe)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		if(getMetaData(fields [i]) == null)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		System.out.println(ERROR! XFORM MUST HAVE A NAME!);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	npe.printStackTrace();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	return metadata;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public Object getMetaData(String fieldName)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		if(fieldName.equals(DESCRIPTOR))
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		return name;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} if(fieldName.equals(XMLNS))
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		return ExtUtil.emptyIfNull(mainInstance.schema);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} else
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		throw new IllegalArgumentException();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public String [] getMetaDataFields()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		return new String []
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ DESCRIPTOR, XMLNS
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	};
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	/// <summary> Link a deserialized instance back up with its parent FormDef. this allows select/select1 questions to be
	/// internationalizable in chatterbox, and (if using CHOICE_INDEX mode) allows the instance to be serialized
	/// to xml
	/// </summary>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void attachControlsToInstanceData()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		attachControlsToInstanceData(getMainInstance().getRoot());
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	private
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void attachControlsToInstanceData(TreeElement node)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		for(int i = 0;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	i < node.getNumChildren();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	i ++)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		attachControlsToInstanceData(node.getChildAt(i));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	IAnswerData val = node.getValue();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	Vector selections = null;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	if(val instanceof SelectOneData)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		selections = new Vector();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	selections.addElement(val.getValue());
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} else if(val instanceof SelectMultiData)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		selections =(Vector) val.getValue();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	if(selections != null)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		QuestionDef q = findQuestionByRef(node.getRef(), this);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	if(q == null)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		throw new RuntimeException(FormDef.attachControlsToInstanceData: can't find question to link);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	if(q.getDynamicChoices() != null)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{
	//droos: i think we should do something like initializing the itemset here, so that default answers
	//can be linked to the selectchoices. however, there are complications. for example, the itemset might
	//not be ready to be evaluated at form initialization; it may require certain questions to be answered
	//first. e.g., if we evaluate an itemset and it has no choices, the xform engine will throw an error
	//itemset TODO
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	for(int i = 0;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	i < selections.size();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	i ++)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		Selection s =(Selection) selections.elementAt(i);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	s.attachChoice(q);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public static QuestionDef findQuestionByRef(TreeReference ref, IFormElement fe)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		if(fe instanceof FormDef)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		ref = ref.genericize();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	if(fe instanceof QuestionDef)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		QuestionDef q =(QuestionDef) fe;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	TreeReference bind = FormInstance.unpackReference(q.getBind());
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	return(ref.equals(bind) ? q: null);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} else
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		for(int i = 0;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	i < fe.getChildren().size();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	i ++)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		QuestionDef ret = findQuestionByRef(ref, fe.getChild(i));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	if(ret != null) 
	return ret;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	return null;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	
	
	/// <summary> Appearance isn't a valid attribute for form, but this method must be included
	/// as a result of conforming to the IFormElement interface.
	/// </summary>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public String getAppearanceAttr()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		throw new RuntimeException(This method call is not relevant for FormDefs getAppearanceAttr ());
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	/// <summary> Appearance isn't a valid attribute for form, but this method must be included
	/// as a result of conforming to the IFormElement interface.
	/// </summary>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void setAppearanceAttr(String appearanceAttr)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		throw new RuntimeException(This method call is not relevant for FormDefs setAppearanceAttr());
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	/// <summary> Not applicable here.</summary>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public String getLabelInnerText()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		return null;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	/// <summary> Not applicable</summary>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public String getTextID()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		return null;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	/// <summary> Not applicable</summary>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void setTextID(String textID)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		throw new RuntimeException(This method call is not relevant for FormDefs [setTextID()]);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void setDefaultSubmission(SubmissionProfile profile)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		submissionProfiles.put(DEFAULT_SUBMISSION_PROFILE, profile);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void addSubmissionProfile(String submissionId, SubmissionProfile profile)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		submissionProfiles.put(submissionId, profile);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public SubmissionProfile getSubmissionProfile()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{
	//At some point these profiles will be set by the <submit> control in the form.
	//In the mean time, though, we can only promise that the default one will be used.
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	return submissionProfiles.get(DEFAULT_SUBMISSION_PROFILE);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	
	@ Override
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void setAdditionalAttribute(String namespace, String name, 
	String value)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{
	// Do nothing. Not supported.
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	
	@ Override
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public String getAdditionalAttribute(String namespace, String name)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{
	// Not supported.
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	return null;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	
	
	@ Override
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public Vector < TreeElement > getAdditionalAttributes()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{
	// Not supported.
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	return new Vector < TreeElement >();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public Vector < Action > getEventListeners(String event)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		if(this.eventListeners.containsKey(event))
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		return eventListeners.get(event);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	return new Vector < Action >();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void registerEventListener(String event, Action action)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		Vector < Action > actions;
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	if(this.eventListeners.containsKey(event))
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		actions = eventListeners.get(event);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} else
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		actions = new Vector < Action >();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	actions.addElement(action);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	this.eventListeners.put(event, actions);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void dispatchFormEvent(String event)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		for(Action action: getEventListeners(event))
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		action.processAction(this, null);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public < X extends XFormExtension > X getExtension(Class < X > extension)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		for(XFormExtension ex: extensions)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		if(ex.getClass().isAssignableFrom(extension))
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		return(X) ex;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	X newEx;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	try
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		newEx = extension.newInstance();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	catch(InstantiationException e)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		throw new RuntimeException(Illegally Structured XForm Extension  + extension.getName());
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	catch(IllegalAccessException e)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		throw new RuntimeException(Illegally Structured XForm Extension  + extension.getName());
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	extensions.addElement(newEx);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	return newEx;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	
	/// <summary> Frees all of the components of this form which are no longer needed once it is completed.
	/// 
	/// Once this is called, the form is no longer capable of functioning, but all data should be retained.
	/// </summary>
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void seal()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		triggerables = null;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	triggerIndex = null;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	conditionRepeatTargetIndex = null;
	//We may need ths one, actually
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	exprEvalContext = null;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
}