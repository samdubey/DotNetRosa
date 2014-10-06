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
					
					List< TreeReference > updatedNodes = new List< TreeReference >();
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
					
					for(TreeReference ref: updatedNodes)
					{
						//Check our index to see if that target is a Trigger for other conditions
						//IE: if they are an element of a different calculation or relevancy calc
						
						//We can't make this reference generic before now or we'll lose the target information,
						//so we'll be more inclusive than needed and see if any of our triggers are keyed on
						//the predicate-less path of this ref
						
						List< Triggerable > triggered =(List< Triggerable >) triggerIndex.get(ref.hasPredicates() ? ref.removePredicates(): ref);
						
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
		
		
		private List< IFormElement > children;
		// <IFormElement>
		/// <summary>A collection of group definitions. </summary>
		private int id;
		/// <summary>The numeric unique identifier of the form definition on the local device </summary>
		private System.String title;
		/// <summary>The display title of the form. </summary>
		private System.String name;
		
		
		private List< XFormExtension > extensions;
		
		/// <summary> A unique external name that is used to identify the form between machines</summary>
		private Localizer localizer;
		
		public List< Triggerable > triggerables;
		// <Triggerable>; this list is topologically ordered, meaning for any tA and tB in
		//the list, where tA comes before tB, evaluating tA cannot depend on any result from evaluating tB
		private bool triggerablesInOrder; //true if triggerables has been ordered topologically (DON'T DELETE ME EVEN THOUGH I'M UNUSED)
		
		
		
		private List< IConditionExpr > outputFragments; // <IConditionExpr> contents of <output>
		// tags that serve as parameterized
		// arguments to captions
		
		
		public HashMap < TreeReference, List< Triggerable >> triggerIndex; // <TreeReference, Vector<Triggerable>>
		
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
		
		
		private HashMap < String, SubmissionProfile > submissionProfiles;
		
		
		private HashMap < String, FormInstance > formInstances;
		private FormInstance mainInstance = null;
		
		
		private HashMap < String, List< Action >> eventListeners;
		
		/// <summary> </summary>
		public FormDef()
		{
			InitBlock();
			ID = - 1;
			setChildren(null);
			
			triggerables = new List< Triggerable >();
			triggerablesInOrder = true;
			
			triggerIndex = new HashMap < TreeReference, List< Triggerable >>();
			//This is kind of a wreck...
			EvaluationContext = new EvaluationContext(null);
			
			outputFragments = new List< IConditionExpr >();
			
			submissionProfiles = new HashMap < String, SubmissionProfile >();
			
			formInstances = new HashMap < String, FormInstance >();
			
			eventListeners = new HashMap < String, List< Action >>();
			
			extensions = new List< XFormExtension >();
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
			
			
			
			List< String > selectedValues = new List< String >();
			if (data is SelectMultiData)
			{
				
				selections =(List< Selection >) data.getValue();
			}
			else if (data is SelectOneData)
			{
				
				selections = new List< Selection >();
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
			
			HashMap < String, TreeElement > existingValues = new HashMap < String, TreeElement >();
			
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
				
				
				
				for(TreeReference trigger: triggers)
				{
					if (!triggerIndex.containsKey(trigger))
					{
						
						triggerIndex.put(trigger.clone(), new List< Triggerable >());
					}
					
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
			
			
			List< Triggerable [] > partialOrdering = new List< Triggerable [] >();
			for (int i = 0; i < triggerables.size(); i++)
			{
				Triggerable t = triggerables.elementAt(i);
				
				
				List< Triggerable > deps = new List< Triggerable >();
				fillTriggeredElements(t, deps);
				
				for (int j = 0; j < deps.size(); j++)
				{
					Triggerable u = deps.elementAt(j);
					Triggerable[] edge = new Triggerable[]{t, u};
					partialOrdering.addElement(edge);
				}
			}
			
			
			List< Triggerable > vertices = new List< Triggerable >();
			for (int i = 0; i < triggerables.size(); i++)
				vertices.addElement(triggerables.elementAt(i));
			triggerables.removeAllElements();
			
			while (vertices.size() > 0)
			{
				//determine root nodes
				
				List< Triggerable > roots = new List< Triggerable >();
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
					
					for(Triggerable t: vertices)
					{
						
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
		
		public
		
		void fillTriggeredElements(Triggerable t, List< Triggerable > destination)
		
		
		
		
		
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
			
			
			List< Triggerable > applicable = new List< Triggerable >();
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
			
			if (triggered == null)
			{
				return ;
			}
			
			//Our vector doesn't have a shallow copy op, so make one
			
			List< Triggerable > triggeredCopy = new List< Triggerable >();
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
		
		private
		
		void evaluateTriggerables(List< Triggerable > tv, TreeReference anchorRef)
		
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
		
		public
		
		void addChildrenOfReference(TreeReference original, List< TreeReference > toAdd)
		
		//Recursive step of utility method
		
		private
		
		void addChildrenOfElement(TreeElement el, List< TreeReference > toAdd)
		
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
			
			, new HashMap < String, Object >());
		}
		
		public System.String fillTemplateString_Renamed_Field;
		
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
			
			List< SelectChoice > choices = new List< SelectChoice >();
			
			
			
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
			
			
			submissionProfiles =(HashMap < String, SubmissionProfile >) ExtUtil.read(dis, new ExtWrapMap(String.
		}
		
		class, SubmissionProfile.
		
		class));
		
		
		formInstances =(HashMap < String, FormInstance >) ExtUtil.read(dis, new ExtWrapMap(String.
		
		class, FormInstance.
		
		class));
		
		
		eventListeners =(HashMap < String, List< Action >>) ExtUtil.read(dis, new ExtWrapMap(String.
		
		class, new ExtWrapListPoly()), pf);
		
		
		extensions =(Vector) ExtUtil.read(dis, new ExtWrapListPoly(), pf);
		
		
		setEvaluationContext(new EvaluationContext(null));
	}
	
	/// <summary> meant to be called after deserialization and initialization of handlers
	/// 
	/// </summary>
	/// <param name="newInstance">true if the form is to be used for a new entry interaction,
	/// false if it is using an existing IDataModel
	/// </param>
	
	public
	
	void initialize(boolean newInstance, InstanceInitializationFactory factory)
	
	{ 
		for(String instanceId: formInstances.keySet())
	
	{ 
		FormInstance instance = formInstances.get(instanceId);
	
	instance.initialize(factory, instanceId);
	
	} 
	if(newInstance)
	
	{ // only preload new forms (we may have to revisit
	// this)
	
	preloadInstance(mainInstance.getRoot());
	
	} 
	
	if(getLocalizer() != null && getLocalizer().getLocale() == null)
	
	{ 
		getLocalizer().setToDefault();
	
	}
	
	//TODO: Hm, not 100% sure that this is right. Maybe we should be
	//using a slightly different event for "First Load" which doesn't
	//get fired again, but always fire this one?
	
	if(newInstance)
	
	{ 
		dispatchFormEvent(Action.EVENT_XFORMS_READY);
	
	} 
	
	initializeTriggerables();
	
	}
	
	/// <summary> Writes the form definition object to the supplied stream.
	/// 
	/// </summary>
	/// <param name="dos">- the stream to write to.
	/// </param>
	/// <throws>  IOException </throws>
	
	public
	
	void writeExternal(DataOutputStream dos) throws IOException
	
	{ 
		ExtUtil.writeNumeric(dos, getID());
	
	ExtUtil.writeString(dos, ExtUtil.emptyIfNull(getName()));
	
	ExtUtil.write(dos, new ExtWrapNullable(getTitle()));
	
	ExtUtil.write(dos, new ExtWrapListPoly(getChildren()));
	
	ExtUtil.write(dos, getMainInstance());
	
	ExtUtil.write(dos, new ExtWrapNullable(localizer));
	
	
	Vector conditions = new Vector();
	
	Vector recalcs = new Vector();
	
	for(int i = 0;
	
	i < triggerables.size();
	
	i ++)
	
	{ 
		Triggerable t =(Triggerable) triggerables.elementAt(i);
	
	if(t instanceof Condition)
	
	{ 
		conditions.addElement(t);
	
	} else if(t instanceof Recalculate)
	
	{ 
		recalcs.addElement(t);
	
	}
	
	} 
	ExtUtil.write(dos, new ExtWrapList(conditions));
	
	ExtUtil.write(dos, new ExtWrapList(recalcs));
	
	
	ExtUtil.write(dos, new ExtWrapListPoly(outputFragments));
	
	ExtUtil.write(dos, new ExtWrapMap(submissionProfiles));
	
	//for support of multi-instance forms
	
	
	ExtUtil.write(dos, new ExtWrapMap(formInstances));
	
	ExtUtil.write(dos, new ExtWrapMap(eventListeners, new ExtWrapListPoly()));
	
	ExtUtil.write(dos, new ExtWrapListPoly(extensions));
	
	}
	
	
	public
	
	void collapseIndex(FormIndex index, Vector indexes, Vector multiplicities, Vector elements)
	
	{ 
		if(!index.isInForm())
	
	{ 
		return;
	
	} 
	
	IFormElement element = this;
	
	while(index != null)
	
	{ 
		int i = index.getLocalIndex();
	
	element = element.getChild(i);
	
	
	indexes.addElement(new Integer(i));
	
	multiplicities.addElement(new Integer(index.getInstanceIndex() == - 1 ? 0: index.getInstanceIndex()));
	
	elements.addElement(element);
	
	
	index = index.getNextLevel();
	
	}
	
	}
	
	
	public FormIndex buildIndex(Vector indexes, Vector multiplicities, Vector elements)
	
	{ 
		FormIndex cur = null;
	
	Vector curMultiplicities = new Vector();
	
	for(int j = 0;
	
	j < multiplicities.size();
	
	++ j)
	
	{ 
		curMultiplicities.addElement(multiplicities.elementAt(j));
	
	} 
	
	Vector curElements = new Vector();
	
	for(int j = 0;
	
	j < elements.size();
	
	++ j)
	
	{ 
		curElements.addElement(elements.elementAt(j));
	
	} 
	
	for(int i = indexes.size() - 1;
	
	i >= 0;
	
	i --)
	
	{ 
		int ix =((Integer) indexes.elementAt(i)).intValue();
	
	int mult =((Integer) multiplicities.elementAt(i)).intValue();
	
	//----begin unclear why this is here... side effects???
	//TODO: ... No words. Just fix it.
	
	IFormElement ife =(IFormElement) elements.elementAt(i);
	
	XPathReference xpr =(ife != null) ?(XPathReference) ife.getBind(): null;
	
	TreeReference ref =(xpr != null) ?(TreeReference) xpr.getReference(): null;
	//----end
	
	if(!(elements.elementAt(i) instanceof GroupDef &&((GroupDef) elements.elementAt(i)).getRepeat()))
	
	{ 
		mult = - 1;
	
	} 
	
	cur = new FormIndex(cur, ix, mult, getChildInstanceRef(curElements, curMultiplicities));
	
	curMultiplicities.removeElementAt(curMultiplicities.size() - 1);
	
	curElements.removeElementAt(curElements.size() - 1);
	
	} 
	return cur;
	
	}
	
	
	
	
	public int getNumRepetitions(FormIndex index)
	
	{ 
		Vector indexes = new Vector();
	
	Vector multiplicities = new Vector();
	
	Vector elements = new Vector();
	
	
	if(!index.isInForm())
	
	{ 
		throw new RuntimeException(not an in-form index);
	
	} 
	
	collapseIndex(index, indexes, multiplicities, elements);
	
	
	if(!(elements.lastElement() instanceof GroupDef) || !((GroupDef) elements.lastElement()).getRepeat())
	
	{ 
		throw new RuntimeException(current element not a repeat);
	
	}
	
	//so painful
	
	TreeElement templNode = mainInstance.getTemplate(index.getReference());
	
	TreeReference parentPath = templNode.getParent().getRef().genericize();
	
	TreeElement parentNode = mainInstance.resolveReference(parentPath.contextualize(index.getReference()));
	
	return parentNode.getChildMultiplicity(templNode.getName());
	
	}
	
	//repIndex == -1 => next repetition about to be created
	
	public FormIndex descendIntoRepeat(FormIndex index, int repIndex)
	
	{ 
		int numRepetitions = getNumRepetitions(index);
	
	
	Vector indexes = new Vector();
	
	Vector multiplicities = new Vector();
	
	Vector elements = new Vector();
	
	collapseIndex(index, indexes, multiplicities, elements);
	
	
	if(repIndex == - 1)
	
	{ 
		repIndex = numRepetitions;
	
	} else
	
	{ 
		if(repIndex < 0 || repIndex >= numRepetitions)
	
	{ 
		throw new RuntimeException(selection exceeds current number of repetitions);
	
	}
	
	} 
	
	multiplicities.setElementAt(new Integer(repIndex), multiplicities.size() - 1);
	
	
	return buildIndex(indexes, multiplicities, elements);
	
	}
	
	/*
	* (non-Javadoc)
	*
	* @see org.javarosa.core.model.IFormElement#getDeepChildCount()
	*/
	
	public int getDeepChildCount()
	
	{ 
		int total = 0;
	
	Enumeration e = children.elements();
	
	while(e.hasMoreElements())
	
	{ 
		total +=((IFormElement) e.nextElement()).getDeepChildCount();
	
	} 
	return total;
	
	}
	
	
	public
	
	void registerStateObserver(FormElementStateListener qsl)
	
	{
	// NO. (Or at least not yet).
	
	}
	
	
	public
	
	void unregisterStateObserver(FormElementStateListener qsl)
	
	{
	// NO. (Or at least not yet).
	
	}
	
	
	public Vector getChildren()
	
	{ 
		return children;
	
	}
	
	
	public
	
	void setChildren(List< IFormElement > children)
	
	{ 
		this.children =(children == null ? new Vector(): children);
	
	}
	
	
	public String getTitle()
	
	{ 
		return title;
	
	}
	
	
	public
	
	void setTitle(String title)
	
	{ 
		this.title = title;
	
	}
	
	
	public int getID()
	
	{ 
		return id;
	
	}
	
	
	public
	
	void setID(int id)
	
	{ 
		this.id = id;
	
	}
	
	
	public String getName()
	
	{ 
		return name;
	
	}
	
	
	public
	
	void setName(String name)
	
	{ 
		this.name = name;
	
	}
	
	
	public Localizer getLocalizer()
	
	{ 
		return localizer;
	
	}
	
	
	public Vector getOutputFragments()
	
	{ 
		return outputFragments;
	
	}
	
	
	public
	
	void setOutputFragments(Vector outputFragments)
	
	{ 
		this.outputFragments = outputFragments;
	
	}
	
	
	public HashMap getMetaData()
	
	{ 
		HashMap metadata = new HashMap();
	
	String [] fields = getMetaDataFields();
	
	
	for(int i = 0;
	
	i < fields.length;
	
	i ++)
	
	{
	
	try
	
	{ 
		metadata.put(fields [i], getMetaData(fields [i]));
	
	}
	
	catch(NullPointerException npe)
	
	{ 
		if(getMetaData(fields [i]) == null)
	
	{ 
		System.out.println(ERROR! XFORM MUST HAVE A NAME!);
	
	npe.printStackTrace();
	
	}
	
	}
	
	} 
	
	return metadata;
	
	}
	
	
	public Object getMetaData(String fieldName)
	
	{ 
		if(fieldName.equals(DESCRIPTOR))
	
	{ 
		return name;
	
	} if(fieldName.equals(XMLNS))
	
	{ 
		return ExtUtil.emptyIfNull(mainInstance.schema);
	
	} else
	
	{ 
		throw new IllegalArgumentException();
	
	}
	
	}
	
	
	public String [] getMetaDataFields()
	
	{ 
		return new String []
	
	{ DESCRIPTOR, XMLNS
	
	};
	
	}
	
	/// <summary> Link a deserialized instance back up with its parent FormDef. this allows select/select1 questions to be
	/// internationalizable in chatterbox, and (if using CHOICE_INDEX mode) allows the instance to be serialized
	/// to xml
	/// </summary>
	
	public
	
	void attachControlsToInstanceData()
	
	{ 
		attachControlsToInstanceData(getMainInstance().getRoot());
	
	}
	
	
	private
	
	void attachControlsToInstanceData(TreeElement node)
	
	{ 
		for(int i = 0;
	
	i < node.getNumChildren();
	
	i ++)
	
	{ 
		attachControlsToInstanceData(node.getChildAt(i));
	
	} 
	
	IAnswerData val = node.getValue();
	
	Vector selections = null;
	
	if(val instanceof SelectOneData)
	
	{ 
		selections = new Vector();
	
	selections.addElement(val.getValue());
	
	} else if(val instanceof SelectMultiData)
	
	{ 
		selections =(Vector) val.getValue();
	
	} 
	
	if(selections != null)
	
	{ 
		QuestionDef q = findQuestionByRef(node.getRef(), this);
	
	if(q == null)
	{ 
		throw new RuntimeException("FormDef.attachControlsToInstanceData: can't find question to link");
	
	} 
	
	if(q.getDynamicChoices() != null)
	
	{
	//droos: i think we should do something like initializing the itemset here, so that default answers
	//can be linked to the selectchoices. however, there are complications. for example, the itemset might
	//not be ready to be evaluated at form initialization; it may require certain questions to be answered
	//first. e.g., if we evaluate an itemset and it has no choices, the xform engine will throw an error
	//itemset TODO
	
	} 
	
	for(int i = 0;
	
	i < selections.size();
	
	i ++)
	
	{ 
		Selection s =(Selection) selections.elementAt(i);
	
	s.attachChoice(q);
	
	}
	
	}
	
	}
	
	
	public static QuestionDef findQuestionByRef(TreeReference ref, IFormElement fe)
	
	{ 
		if(fe instanceof FormDef)
	
	{ 
		ref = ref.genericize();
	
	} 
	
	if(fe instanceof QuestionDef)
	
	{ 
		QuestionDef q =(QuestionDef) fe;
	
	TreeReference bind = FormInstance.unpackReference(q.getBind());
	
	return(ref.equals(bind) ? q: null);
	
	} else
	
	{ 
		for(int i = 0;
	
	i < fe.getChildren().size();
	
	i ++)
	
	{ 
		QuestionDef ret = findQuestionByRef(ref, fe.getChild(i));
	
	if(ret != null) 
	return ret;
	
	} 
	return null;
	
	}
	
	}
	
	
	
	/// <summary> Appearance isn't a valid attribute for form, but this method must be included
	/// as a result of conforming to the IFormElement interface.
	/// </summary>
	
	public String getAppearanceAttr()
	
	{ 
		throw new RuntimeException(This method call is not relevant for FormDefs getAppearanceAttr ());
	
	}
	
	/// <summary> Appearance isn't a valid attribute for form, but this method must be included
	/// as a result of conforming to the IFormElement interface.
	/// </summary>
	
	public
	
	void setAppearanceAttr(String appearanceAttr)
	
	{ 
		throw new RuntimeException(This method call is not relevant for FormDefs setAppearanceAttr());
	
	}
	
	/// <summary> Not applicable here.</summary>
	
	public String getLabelInnerText()
	
	{ 
		return null;
	
	}
	
	/// <summary> Not applicable</summary>
	
	public String getTextID()
	
	{ 
		return null;
	
	}
	
	/// <summary> Not applicable</summary>
	
	public
	
	void setTextID(String textID)
	
	{ 
		throw new RuntimeException(This method call is not relevant for FormDefs [setTextID()]);
	
	}
	
	
	
	public
	
	void setDefaultSubmission(SubmissionProfile profile)
	
	{ 
		submissionProfiles.put(DEFAULT_SUBMISSION_PROFILE, profile);
	
	}
	
	
	public
	
	void addSubmissionProfile(String submissionId, SubmissionProfile profile)
	
	{ 
		submissionProfiles.put(submissionId, profile);
	
	}
	
	
	public SubmissionProfile getSubmissionProfile()
	
	{
	//At some point these profiles will be set by the <submit> control in the form.
	//In the mean time, though, we can only promise that the default one will be used.
	
	
	return submissionProfiles.get(DEFAULT_SUBMISSION_PROFILE);
	
	} 
	
	
	@ Override
	
	public
	
	void setAdditionalAttribute(string namespace, string name, 	string value)
	
	{
	// Do nothing. Not supported.
	
	} 
	
	
	@ Override
	
	public String getAdditionalAttribute(String namespace, String name)
	
	{
	// Not supported.
	
	return null;
	
	} 
	
	
	@ Override
	
	public List< TreeElement > getAdditionalAttributes()
	
	{
	// Not supported.
	
	return new List< TreeElement >();
	
	}
	
	public List< Action > getEventListeners(String event)
	
	{ 
		if(this.eventListeners.containsKey(event))
	
	{ 
		return eventListeners.get(event);
	
	} 
	return new List< Action >();
	
	}
	
	
	public
	
	void registerEventListener(String event, Action action)
	
	{ 
		List< Action > actions;
	
	
	if(this.eventListeners.containsKey(event))
	
	{ 
		actions = eventListeners.get(event);
	
	} else
	
	{ 
		actions = new List< Action >();
	
	} 
	actions.addElement(action);
	
	this.eventListeners.put(event, actions);
	
	}
	
	
	public
	
	void dispatchFormEvent(String event)
	
	{ 
		for(Action action: getEventListeners(event))
	
	{ 
		action.processAction(this, null);
	
	}
	
	}
	
	
	public < X extends XFormExtension > X getExtension(Class < X > extension)
	
	{ 
		for(XFormExtension ex: extensions)
	
	{ 
		if(ex.getClass().isAssignableFrom(extension))
	
	{ 
		return(X) ex;
	
	}
	
	} 
	X newEx;
	
	try
	
	{ 
		newEx = extension.newInstance();
	
	}
	
	catch(InstantiationException e)
	
	{ 
		throw new RuntimeException(Illegally Structured XForm Extension  + extension.getName());
	
	}
	
	catch(IllegalAccessException e)
	
	{ 
		throw new RuntimeException(Illegally Structured XForm Extension  + extension.getName());
	
	} 
	extensions.addElement(newEx);
	
	return newEx;
	
	}
	
	
	/// <summary> Frees all of the components of this form which are no longer needed once it is completed.
	/// 
	/// Once this is called, the form is no longer capable of functioning, but all data should be retained.
	/// </summary>
	
	public
	
	void seal()
	
	{ 
		triggerables = null;
	
	triggerIndex = null;
	
	conditionRepeatTargetIndex = null;
	//We may need ths one, actually
	
	exprEvalContext = null;
	
	}
	
	}
}