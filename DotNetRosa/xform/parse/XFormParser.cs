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
using SetValueAction = org.javarosa.core.model.actions.SetValueAction;
using org.javarosa.core.model.condition;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using InvalidReferenceException = org.javarosa.core.model.instance.InvalidReferenceException;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using IAnswerResolver = org.javarosa.core.model.instance.utils.IAnswerResolver;
using Restorable = org.javarosa.core.model.util.restorable.Restorable;
using RestoreUtils = org.javarosa.core.model.util.restorable.RestoreUtils;
using Logger = org.javarosa.core.services.Logger;
using Localizer = org.javarosa.core.services.locale.Localizer;
using TableLocaleSource = org.javarosa.core.services.locale.TableLocaleSource;
using CacheTable = org.javarosa.core.util.CacheTable;
using OrderedMap = org.javarosa.core.util.OrderedMap;
using PrefixTreeNode = org.javarosa.core.util.PrefixTreeNode;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using PrototypeFactoryDeprecated = org.javarosa.core.util.externalizable.PrototypeFactoryDeprecated;
using XPathReference = org.javarosa.model.xform.XPathReference;
using InterningKXmlParser = org.javarosa.xform.util.InterningKXmlParser;
using XFormAnswerDataParser = org.javarosa.xform.util.XFormAnswerDataParser;
using XFormSerializer = org.javarosa.xform.util.XFormSerializer;
using XFormUtils = org.javarosa.xform.util.XFormUtils;
using XPathConditional = org.javarosa.xpath.XPathConditional;
using XPathException = org.javarosa.xpath.XPathException;
using XPathParseTool = org.javarosa.xpath.XPathParseTool;
using XPathPathExpr = org.javarosa.xpath.expr.XPathPathExpr;
using XPathSyntaxException = org.javarosa.xpath.parser.XPathSyntaxException;
//UPGRADE_TODO: The type 'org.kxml2.io.KXmlParser' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using KXmlParser = org.kxml2.io.KXmlParser;
//UPGRADE_TODO: The type 'org.kxml2.kdom.Document' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Document = org.kxml2.kdom.Document;
//UPGRADE_TODO: The type 'org.kxml2.kdom.Element' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Element = org.kxml2.kdom.Element;
//UPGRADE_TODO: The type 'org.kxml2.kdom.Node' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Node = org.kxml2.kdom.Node;
//UPGRADE_TODO: The type 'org.xmlpull.v1.XmlPullParser' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using XmlPullParser = org.xmlpull.v1.XmlPullParser;
//UPGRADE_TODO: The type 'org.xmlpull.v1.XmlPullParserException' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using XmlPullParserException = org.xmlpull.v1.XmlPullParserException;
namespace org.javarosa.xform.parse
{
	
	/* droos: i think we need to start storing the contents of the <bind>s in the formdef again */
	
	/// <summary> Provides conversion from xform to epihandy object model and vice vasa.
	/// 
	/// </summary>
	/// <author>  Daniel Kayiwa
	/// </author>
	/// <author>  Drew Roos
	/// 
	/// </author>
	public class XFormParser
	{
		private class AnonymousClassIElementHandler : IElementHandler
		{
			public virtual void  handle(XFormParser p, Element e, System.Object parent)
			{
				p.parseTitle(e);
			}
		}
		private class AnonymousClassIElementHandler1 : IElementHandler
		{
			public virtual void  handle(XFormParser p, Element e, System.Object parent)
			{
				p.parseMeta(e);
			}
		}
		private class AnonymousClassIElementHandler2 : IElementHandler
		{
			public virtual void  handle(XFormParser p, Element e, System.Object parent)
			{
				p.parseModel(e);
			}
		}
		private class AnonymousClassIElementHandler3 : IElementHandler
		{
			public virtual void  handle(XFormParser p, Element e, System.Object parent)
			{
				p.parseControl((IFormElement) parent, e, Constants.CONTROL_INPUT);
			}
		}
		private class AnonymousClassIElementHandler4 : IElementHandler
		{
			public virtual void  handle(XFormParser p, Element e, System.Object parent)
			{
				p.parseControl((IFormElement) parent, e, Constants.CONTROL_SECRET);
			}
		}
		private class AnonymousClassIElementHandler5 : IElementHandler
		{
			public virtual void  handle(XFormParser p, Element e, System.Object parent)
			{
				p.parseControl((IFormElement) parent, e, Constants.CONTROL_SELECT_MULTI);
			}
		}
		private class AnonymousClassIElementHandler6 : IElementHandler
		{
			public virtual void  handle(XFormParser p, Element e, System.Object parent)
			{
				p.parseControl((IFormElement) parent, e, Constants.CONTROL_SELECT_ONE);
			}
		}
		private class AnonymousClassIElementHandler7 : IElementHandler
		{
			public virtual void  handle(XFormParser p, Element e, System.Object parent)
			{
				p.parseGroup((IFormElement) parent, e, org.javarosa.xform.parse.XFormParser.CONTAINER_GROUP);
			}
		}
		private class AnonymousClassIElementHandler8 : IElementHandler
		{
			public virtual void  handle(XFormParser p, Element e, System.Object parent)
			{
				p.parseGroup((IFormElement) parent, e, org.javarosa.xform.parse.XFormParser.CONTAINER_REPEAT);
			}
		}
		private class AnonymousClassIElementHandler9 : IElementHandler
		{
			public virtual void  handle(XFormParser p, Element e, System.Object parent)
			{
				p.parseGroupLabel((GroupDef) parent, e);
			}
		}
		private class AnonymousClassIElementHandler10 : IElementHandler
		{
			public virtual void  handle(XFormParser p, Element e, System.Object parent)
			{
				p.parseControl((IFormElement) parent, e, Constants.CONTROL_TRIGGER);
			}
		}
		private class AnonymousClassIElementHandler11 : IElementHandler
		{
			public virtual void  handle(XFormParser p, Element e, System.Object parent)
			{
				p.parseUpload((IFormElement) parent, e, Constants.CONTROL_UPLOAD);
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassIElementHandler12' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassIElementHandler12 : IElementHandler
		{
			public AnonymousClassIElementHandler12(XFormParser enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(XFormParser enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private XFormParser enclosingInstance;
			public XFormParser Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  handle(XFormParser p, Element e, System.Object parent)
			{
				p.parseSetValueAction((FormDef) parent, e);
			}
		}
		private class AnonymousClassIElementHandler13 : IElementHandler
		{
			public AnonymousClassIElementHandler13(int typeId)
			{
				InitBlock(typeId);
			}
			private void  InitBlock(int typeId)
			{
				this.typeId = typeId;
			}
			//UPGRADE_NOTE: Final variable typeId was copied into class AnonymousClassIElementHandler13. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1023'"
			private int typeId;
			public virtual void  handle(XFormParser p, Element e, System.Object parent)
			{
				p.parseControl((IFormElement) parent, e, typeId);
			}
		}
		private void  InitBlock()
		{
			Document doc = new Document();
			
			try
			{
				KXmlParser parser;
				
				if (stringCache != null)
				{
					parser = new InterningKXmlParser(stringCache);
				}
				else
				{
					parser = new KXmlParser();
				}
				
				parser.setInput(reader);
				parser.setFeature(XmlPullParser.FEATURE_PROCESS_NAMESPACES, true);
				doc.parse(parser);
			}
			catch (XmlPullParserException e)
			{
				System.String errorMsg = "XML Syntax Error at Line: " + e.getLineNumber() + ", Column: " + e.getColumnNumber() + "!";
				System.Console.Error.WriteLine(errorMsg);
				e.printStackTrace();
				throw new XFormParseException(errorMsg);
			}
			catch (System.IO.IOException e)
			{
				//CTS - 12/09/2012 - Stop swallowing IO Exceptions
				throw e;
			}
			catch (System.Exception e)
			{
				//#if debug.output==verbose || debug.output==exception
				System.String errorMsg = "Unhandled Exception while Parsing XForm";
				System.Console.Error.WriteLine(errorMsg);
				if (e is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
					((org.javarosa.core.io.StreamsUtil.DirectionalIOException) e).printStackTrace();
				else
					SupportClass.WriteStackTrace(e, Console.Error);
				throw new XFormParseException(errorMsg);
				//#endif
			}
			
			try
			{
				reader.close();
			}
			catch (System.IO.IOException e)
			{
				System.Console.Out.WriteLine("Error closing reader");
				if (e is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
					((org.javarosa.core.io.StreamsUtil.DirectionalIOException) e).printStackTrace();
				else
					SupportClass.WriteStackTrace(e, Console.Error);
			}
			
			//For escaped unicode strings we end up with a looooot of cruft,
			//so we really want to go through and convert the kxml parsed
			//text (which have lots of characters each as their own string)
			//into one single string
			
			Stack < Element > q = new Stack < Element >();
			
			q.push(doc.getRootElement());
			while (!q.isEmpty())
			{
				Element e = q.pop();
				bool[] toRemove = new bool[e.getChildCount() * 2];
				System.String accumulate = "";
				for (int i = 0; i < e.getChildCount(); ++i)
				{
					int type = e.getType(i);
					if (type == Element.TEXT)
					{
						System.String text = e.getText(i);
						accumulate += text;
						toRemove[i] = true;
					}
					else
					{
						if (type == Element.ELEMENT)
						{
							q.addElement(e.getElement(i));
						}
						System.String accumulatedString = accumulate.Trim();
						if (accumulatedString.Length != 0)
						{
							if (stringCache == null)
							{
								e.addChild(i, Element.TEXT, accumulate);
							}
							else
							{
								e.addChild(i, Element.TEXT, stringCache.intern(accumulate));
							}
							accumulate = "";
							++i;
						}
						else
						{
							accumulate = "";
						}
					}
				}
				if (accumulate.Trim().Length != 0)
				{
					if (stringCache == null)
					{
						e.addChild(Element.TEXT, accumulate);
					}
					else
					{
						e.addChild(Element.TEXT, stringCache.intern(accumulate));
					}
				}
				for (int i = e.getChildCount() - 1; i >= 0; i--)
				{
					if (toRemove[i])
					{
						e.removeChild(i);
					}
				}
			}
			
			return doc;
			//,
			//			boolean allowUnknownElements, boolean allowText, boolean recurseUnknown) {
			System.String name = e.getName();
			
			System.String[] suppressWarningArr = new System.String[]{"html", "head", "body", "xform", "chooseCaption", "addCaption", "addEmptyCaption", "delCaption", "doneCaption", "doneEmptyCaption", "mainHeader", "entryHeader", "delHeader"};
			
			List< String > suppressWarning = new List< String >();
			for (int i = 0; i < suppressWarningArr.Length; i++)
			{
				suppressWarning.addElement(suppressWarningArr[i]);
			}
			
			IElementHandler eh = handlers.get_Renamed(name);
			if (eh != null)
			{
				eh.handle(this, e, parent);
			}
			else
			{
				if (!suppressWarning.contains(name))
				{
					reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, "Unrecognized element [" + name + "]. Ignoring and processing children...", getVagueLocation(e));
				}
				for (int i = 0; i < e.getChildCount(); i++)
				{
					if (e.getType(i) == Element.ELEMENT)
					{
						parseElement(e.getElement(i), parent, handlers);
					}
				}
			}
			
			HashMap < String, String > prefixes = new HashMap < String, String >();
			for (int i = 0; i < e.getNamespaceCount(); ++i)
			{
				System.String uri = e.getNamespaceUri(i);
				System.String prefix = e.getNamespacePrefix(i);
				if (uri != null && prefix != null)
				{
					tree.addNamespace(prefix, uri);
				}
			}
			return prefixes;
			
			List< TreeReference > refs = new List< TreeReference >();
			
			for (int i = 0; i < repeats.size(); i++)
			{
				refs.addElement((TreeReference) repeats.elementAt(i));
			}
			
			for (int i = 0; i < itemsets.size(); i++)
			{
				ItemsetBinding itemset = (ItemsetBinding) itemsets.elementAt(i);
				TreeReference srcRef = itemset.nodesetRef;
				if (!refs.contains(srcRef))
				{
					//CTS: Being an itemset root is not sufficient to mark
					//a node as repeatable. It has to be nonstatic (which it
					//must be inherently unless there's a wildcard).
					bool nonstatic = true;
					for (int j = 0; j < srcRef.size(); ++j)
					{
						if (TreeReference.NAME_WILDCARD.Equals(srcRef.getName(j)))
						{
							nonstatic = false;
						}
					}
					
					//CTS: we're also going to go ahead and assume that all external
					//instance are static (we can't modify them TODO: This may only be
					//the case if the instances are of specific types (non Tree-Element
					//style). Revisit if needed.
					if (srcRef.InstanceName != null)
					{
						nonstatic = false;
					}
					if (nonstatic)
					{
						refs.addElement(srcRef);
					}
				}
				
				if (itemset.copyMode)
				{
					TreeReference destRef = itemset.getDestRef();
					if (!refs.contains(destRef))
					{
						refs.addElement(destRef);
					}
				}
			}
			
			return refs;
			TreeElement root = new TreeElement(null, 0);
			
			for (int i = 0; i < repeatRefs.size(); i++)
			{
				TreeReference repeatRef = repeatRefs.elementAt(i);
				//check and see if this references a repeat from a non-main instance, if so, skip it
				if (repeatRef.InstanceName != null)
				{
					continue;
				}
				if (repeatRef.size() <= 1)
				{
					//invalid repeat: binds too high. ignore for now and error will be raised in verifyBindings
					continue;
				}
				
				TreeElement cur = root;
				for (int j = 0; j < repeatRef.size(); j++)
				{
					System.String name = repeatRef.getName(j);
					TreeElement child = cur.getChild(name, 0);
					if (child == null)
					{
						child = new TreeElement(name, 0);
						cur.addChild(child);
					}
					
					cur = child;
				}
				cur.Repeatable = true;
			}
			
			if (root.NumChildren == 0)
				return null;
			else
				return new FormInstance(root.getChild(topLevelName, TreeReference.DEFAULT_MUTLIPLICITY));
			if (repeatTree != null)
				checkRepeatsForTemplate(repeatTree.getRoot(), TreeReference.rootRef(), instance, missingTemplates);
			System.String name = repeatTreeNode.getName();
			int mult = (repeatTreeNode.isRepeatable()?TreeReference.INDEX_TEMPLATE:0);
			ref_Renamed = ref_Renamed.extendRef(name, mult);
			
			if (repeatTreeNode.isRepeatable())
			{
				TreeElement template = instance.resolveReference(ref_Renamed);
				if (template == null)
				{
					missing.addElement(ref_Renamed);
				}
			}
			
			for (int i = 0; i < repeatTreeNode.getNumChildren(); i++)
			{
				checkRepeatsForTemplate(repeatTreeNode.getChildAt(i), ref_Renamed, instance, missing);
			}
			//it is VERY important that the missing template refs are listed in depth-first or breadth-first order... namely, that
			//every ref is listed after a ref that could be its parent. checkRepeatsForTemplate currently behaves this way
			for (int i = 0; i < missingTemplates.size(); i++)
			{
				TreeReference templRef = missingTemplates.elementAt(i);
				TreeReference firstMatch;
				
				//make template ref generic and choose first matching node
				TreeReference ref_Renamed = templRef.Clone();
				for (int j = 0; j < ref_Renamed.size(); j++)
				{
					ref_Renamed.setMultiplicity(j, TreeReference.INDEX_UNBOUND);
				}
				
				if (nodes.size() == 0)
				{
					//binding error; not a single node matches the repeat binding; will be reported later
					continue;
				}
				else
				{
					firstMatch = nodes.elementAt(0);
				}
				
				try
				{
					instance.copyNode(firstMatch, templRef);
				}
				catch (InvalidReferenceException e)
				{
					reporter.warning(XFormParserReporter.TYPE_INVALID_STRUCTURE, "Could not create a default repeat template; this is almost certainly a homogeneity error! Your form will not work! (Failed on " + templRef.ToString() + ")", null);
				}
				trimRepeatChildren(instance.resolveReference(templRef));
			}
			//throws XmlPullParserException {
			if (fe.getChildren() == null)
				return ;
			
			for (int i = 0; i < fe.getChildren().size(); i++)
			{
				IFormElement child = fe.getChildren().elementAt(i);
				IDataReference ref_Renamed = null;
				System.String type = null;
				
				if (child is GroupDef)
				{
					ref_Renamed = ((GroupDef) child).Bind;
					type = (((GroupDef) child).Repeat?"Repeat":"Group");
				}
				else if (child is QuestionDef)
				{
					ref_Renamed = ((QuestionDef) child).Bind;
					type = "Question";
				}
				TreeReference tref = FormInstance.unpackReference(ref_Renamed);
				
				if (child is QuestionDef && tref.size() == 0)
				{
					//group can bind to '/'; repeat can't, but that's checked above
					reporter.warning(XFormParserReporter.TYPE_INVALID_STRUCTURE, "Cannot bind control to '/'", null);
				}
				else
				{
					
					if (nodes.size() == 0)
					{
						System.String error = type + " bound to non-existent node: [" + tref.ToString() + "]";
						reporter.error(error);
						errors.addElement(error);
					}
					//we can't check whether questions map to the right kind of node ('data' node vs. 'sub-tree' node) as that depends
					//on the question's data type, which we don't know yet
				}
				
				verifyControlBindings(child, instance, errors);
			}
			this.stringCache = stringCache;
		}
		public static IAnswerResolver AnswerResolver
		{
			get
			{
				return answerResolver;
			}
			
			set
			{
				XFormParser.answerResolver = value;
			}
			
		}
		
		//Constants to clean up code and prevent user error
		private const System.String ID_ATTR = "id";
		private const System.String FORM_ATTR = "form";
		private const System.String APPEARANCE_ATTR = "appearance";
		private const System.String NODESET_ATTR = "nodeset";
		private const System.String LABEL_ELEMENT = "label";
		private const System.String VALUE = "value";
		private const System.String ITEXT_CLOSE = "')";
		private const System.String ITEXT_OPEN = "jr:itext('";
		private const System.String BIND_ATTR = "bind";
		private const System.String REF_ATTR = "ref";
		private const System.String SELECTONE = "select1";
		private const System.String SELECT = "select";
		
		public const System.String NAMESPACE_JAVAROSA = "http://openrosa.org/javarosa";
		public const System.String NAMESPACE_HTML = "http://www.w3.org/1999/xhtml";
		
		private const int CONTAINER_GROUP = 1;
		private const int CONTAINER_REPEAT = 2;
		
		
		private static HashMap < String, IElementHandler > topLevelHandlers;
		
		private static HashMap < String, IElementHandler > groupLevelHandlers;
		
		private static HashMap < String, Integer > typeMappings;
		private static PrototypeFactoryDeprecated modelPrototypes;
		
		private static List< SubmissionParser > submissionParsers;
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.IO.StreamReader _reader;
		private Document _xmldoc;
		private FormDef _f;
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.IO.StreamReader _instReader;
		private Document _instDoc;
		
		private bool modelFound;
		
		private HashMap < String, DataBinding > bindingsByID;
		
		private List< DataBinding > bindings;
		
		private List< TreeReference > actionTargets;
		
		private List< TreeReference > repeats;
		
		private List< ItemsetBinding > itemsets;
		
		private List< TreeReference > selectOnes;
		
		private List< TreeReference > selectMultis;
		private Element mainInstanceNode; //top-level data node of the instance; saved off so it can be processed after the <bind>s
		
		private List< Element > instanceNodes;
		
		private List< String > instanceNodeIdStrs;
		private System.String defaultNamespace;
		
		private List< String > itextKnownForms;
		
		private List< String > namedActions;
		
		private HashMap < String, IElementHandler > structuredActions;
		
		
		private FormInstance repeatTree; //pseudo-data model tree that describes the repeat structure of the instance;
		//useful during instance processing and validation
		
		//incremented to provide unique question ID for each question
		private int serialQuestionID = 1;
		
		private static IAnswerResolver answerResolver;
		
		private static void  staticInit()
		{
			initProcessingRules();
			initTypeMappings();
			modelPrototypes = new PrototypeFactoryDeprecated();
			
			submissionParsers = new List< SubmissionParser >();
		}
		
		private static void  initProcessingRules()
		{
			IElementHandler title = new AnonymousClassIElementHandler();
			IElementHandler meta = new AnonymousClassIElementHandler1();
			IElementHandler model = new AnonymousClassIElementHandler2();
			IElementHandler input = new AnonymousClassIElementHandler3();
			IElementHandler secret = new AnonymousClassIElementHandler4();
			IElementHandler select = new AnonymousClassIElementHandler5();
			IElementHandler select1 = new AnonymousClassIElementHandler6();
			IElementHandler group = new AnonymousClassIElementHandler7();
			IElementHandler repeat = new AnonymousClassIElementHandler8();
			IElementHandler groupLabel = new AnonymousClassIElementHandler9();
			IElementHandler trigger = new AnonymousClassIElementHandler10();
			IElementHandler upload = new AnonymousClassIElementHandler11();
			
			
			groupLevelHandlers = new HashMap < String, IElementHandler >();
			groupLevelHandlers.put("input", input);
			groupLevelHandlers.put("secret", secret);
			groupLevelHandlers.put(SELECT, select);
			groupLevelHandlers.put(SELECTONE, select1);
			groupLevelHandlers.put("group", group);
			groupLevelHandlers.put("repeat", repeat);
			groupLevelHandlers.put("trigger", trigger); //multi-purpose now; need to dig deeper
			groupLevelHandlers.put(Constants.XFTAG_UPLOAD, upload);
			
			
			topLevelHandlers = new HashMap < String, IElementHandler >();
			
			for(String key: groupLevelHandlers.keySet())
			{
				topLevelHandlers.put(key, groupLevelHandlers.get_Renamed(key));
			}
			topLevelHandlers.put("model", model);
			topLevelHandlers.put("title", title);
			topLevelHandlers.put("meta", meta);
			
			groupLevelHandlers.put(LABEL_ELEMENT, groupLabel);
		}
		
		private static void  initTypeMappings()
		{
			
			typeMappings = new HashMap < String, Integer >();
			typeMappings.put("string", (System.Int32) Constants.DATATYPE_TEXT); //xsd:
			typeMappings.put("integer", (System.Int32) Constants.DATATYPE_INTEGER); //xsd:
			typeMappings.put("long", (System.Int32) Constants.DATATYPE_LONG); //xsd:
			typeMappings.put("int", (System.Int32) Constants.DATATYPE_INTEGER); //xsd:
			typeMappings.put("decimal", (System.Int32) Constants.DATATYPE_DECIMAL); //xsd:
			typeMappings.put("double", (System.Int32) Constants.DATATYPE_DECIMAL); //xsd:
			typeMappings.put("float", (System.Int32) Constants.DATATYPE_DECIMAL); //xsd:
			typeMappings.put("dateTime", (System.Int32) Constants.DATATYPE_DATE_TIME); //xsd:
			typeMappings.put("date", (System.Int32) Constants.DATATYPE_DATE); //xsd:
			typeMappings.put("time", (System.Int32) Constants.DATATYPE_TIME); //xsd:
			typeMappings.put("gYear", (System.Int32) Constants.DATATYPE_UNSUPPORTED); //xsd:
			typeMappings.put("gMonth", (System.Int32) Constants.DATATYPE_UNSUPPORTED); //xsd:
			typeMappings.put("gDay", (System.Int32) Constants.DATATYPE_UNSUPPORTED); //xsd:
			typeMappings.put("gYearMonth", (System.Int32) Constants.DATATYPE_UNSUPPORTED); //xsd:
			typeMappings.put("gMonthDay", (System.Int32) Constants.DATATYPE_UNSUPPORTED); //xsd:
			typeMappings.put("boolean", (System.Int32) Constants.DATATYPE_BOOLEAN); //xsd:
			typeMappings.put("base64Binary", (System.Int32) Constants.DATATYPE_UNSUPPORTED); //xsd:
			typeMappings.put("hexBinary", (System.Int32) Constants.DATATYPE_UNSUPPORTED); //xsd:
			typeMappings.put("anyURI", (System.Int32) Constants.DATATYPE_UNSUPPORTED); //xsd:
			typeMappings.put("listItem", (System.Int32) Constants.DATATYPE_CHOICE); //xforms:
			typeMappings.put("listItems", (System.Int32) Constants.DATATYPE_CHOICE_LIST); //xforms:
			typeMappings.put(SELECTONE, (System.Int32) Constants.DATATYPE_CHOICE); //non-standard
			typeMappings.put(SELECT, (System.Int32) Constants.DATATYPE_CHOICE_LIST); //non-standard
			typeMappings.put("geopoint", (System.Int32) Constants.DATATYPE_GEOPOINT); //non-standard
			typeMappings.put("geoshape", (System.Int32) Constants.DATATYPE_GEOSHAPE); //non-standard
			typeMappings.put("geotrace", (System.Int32) Constants.DATATYPE_GEOTRACE); //non-standard
			typeMappings.put("barcode", (System.Int32) Constants.DATATYPE_BARCODE); //non-standard
			typeMappings.put("binary", (System.Int32) Constants.DATATYPE_BINARY); //non-standard
		}
		
		private void  initState()
		{
			modelFound = false;
			
			bindingsByID = new HashMap < String, DataBinding >();
			
			bindings = new List< DataBinding >();
			
			actionTargets = new List< TreeReference >();
			
			repeats = new List< TreeReference >();
			
			itemsets = new List< ItemsetBinding >();
			
			selectOnes = new List< TreeReference >();
			
			selectMultis = new List< TreeReference >();
			mainInstanceNode = null;
			
			instanceNodes = new List< Element >();
			
			instanceNodeIdStrs = new List< String >();
			repeatTree = null;
			defaultNamespace = null;
			
			
			itextKnownForms = new List< String >();
			itextKnownForms.addElement("long");
			itextKnownForms.addElement("short");
			itextKnownForms.addElement("image");
			itextKnownForms.addElement("audio");
			
			
			namedActions = new List< String >();
			namedActions.addElement("rebuild");
			namedActions.addElement("recalculate");
			namedActions.addElement("revalidate");
			namedActions.addElement("refresh");
			namedActions.addElement("setfocus");
			namedActions.addElement("reset");
			
			
			
			structuredActions = new HashMap < String, IElementHandler >();
			structuredActions.put("setvalue", new AnonymousClassIElementHandler12(this));
		}
		
		internal XFormParserReporter reporter = new XFormParserReporter();
		
		
		CacheTable < String > stringCache;
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public XFormParser(System.IO.StreamReader reader)
		{
			InitBlock();
			_reader = reader;
		}
		
		public XFormParser(Document doc)
		{
			InitBlock();
			_xmldoc = doc;
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public XFormParser(System.IO.StreamReader form, System.IO.StreamReader instance)
		{
			InitBlock();
			_reader = form;
			_instReader = instance;
		}
		
		public XFormParser(Document form, Document instance)
		{
			InitBlock();
			_xmldoc = form;
			_instDoc = instance;
		}
		
		public virtual void  attachReporter(XFormParserReporter reporter)
		{
			this.reporter = reporter;
		}
		
		public virtual FormDef parse()
		{
			if (_f == null)
			{
				System.Console.Out.WriteLine("Parsing form...");
				
				if (_xmldoc == null)
				{
					_xmldoc = getXMLDocument(_reader, stringCache);
				}
				
				parseDoc();
				
				//load in a custom xml instance, if applicable
				if (_instReader != null)
				{
					loadXmlInstance(_f, _instReader);
				}
				else if (_instDoc != null)
				{
					loadXmlInstance(_f, _instDoc);
				}
			}
			return _f;
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public static Document getXMLDocument(System.IO.StreamReader reader)
		{
			return getXMLDocument(reader, null);
		}
		
		public static Document getXMLDocument_Renamed_Field;
		
		(Reader reader, CacheTable < String > stringCache) throws IOException
		
		private void  parseDoc()
		{
			_f = new FormDef();
			
			initState();
			defaultNamespace = _xmldoc.getRootElement().getNamespaceUri(null);
			parseElement(_xmldoc.getRootElement(), _f, topLevelHandlers);
			collapseRepeatGroups(_f);
			
			//parse the non-main instance nodes first
			//we assume that the non-main instances won't
			//reference the main node, so we do them first.
			//if this assumption is wrong, well, then we're screwed.
			if (instanceNodes.size() > 1)
			{
				for (int i = 1; i < instanceNodes.size(); i++)
				{
					Element e = instanceNodes.elementAt(i);
					FormInstance fi = parseInstance(e, false);
					loadInstanceData(e, fi.getRoot(), _f);
					_f.addNonMainInstance(fi);
				}
			}
			//now parse the main instance
			if (mainInstanceNode != null)
			{
				FormInstance fi = parseInstance(mainInstanceNode, true);
				addMainInstanceToFormDef(mainInstanceNode, fi);
				
				//set the main instance
				_f.Instance = fi;
			}
		}
		
		
		private
		
		void parseElement(Element e, Object parent, HashMap < String, IElementHandler > handlers)
		
		private void  parseTitle(Element e)
		{
			System.Collections.ArrayList usedAtts = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10)); //no attributes parsed in title.
			System.String title = getXMLText(e, true);
			System.Console.Out.WriteLine("Title: \"" + title + "\"");
			_f.setTitle(title);
			if (_f.getName() == null)
			{
				//Jan 9, 2009 - ctsims
				//We don't really want to allow for forms without
				//some unique ID, so if a title is available, use
				//that.
				_f.setName(title);
			}
			
			
			if (XFormUtils.showUnusedAttributeWarning(e, usedAtts))
			{
				reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(e, usedAtts), getVagueLocation(e));
			}
		}
		
		private void  parseMeta(Element e)
		{
			
			List< String > usedAtts = new List< String >();
			int attributes = e.getAttributeCount();
			for (int i = 0; i < attributes; ++i)
			{
				System.String name = e.getAttributeName(i);
				System.String value_Renamed = e.getAttributeValue(i);
				if ("name".Equals(name))
				{
					_f.setName(value_Renamed);
				}
			}
			
			
			usedAtts.addElement("name");
			if (XFormUtils.showUnusedAttributeWarning(e, usedAtts))
			{
				reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(e, usedAtts), getVagueLocation(e));
			}
		}
		
		//for ease of parsing, we assume a model comes before the controls, which isn't necessarily mandated by the xforms spec
		private void  parseModel(Element e)
		{
			
			List< String > usedAtts = new List< String >(); //no attributes parsed in title.
			
			List< Element > delayedParseElements = new List< Element >();
			
			if (modelFound)
			{
				reporter.warning(XFormParserReporter.TYPE_INVALID_STRUCTURE, "Multiple models not supported. Ignoring subsequent models.", getVagueLocation(e));
				return ;
			}
			modelFound = true;
			
			if (XFormUtils.showUnusedAttributeWarning(e, usedAtts))
			{
				reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(e, usedAtts), getVagueLocation(e));
			}
			
			for (int i = 0; i < e.getChildCount(); i++)
			{
				
				int type = e.getType(i);
				Element child = (type == Node.ELEMENT?e.getElement(i):null);
				System.String childName = (child != null?child.getName():null);
				
				if ("itext".Equals(childName))
				{
					parseIText(child);
				}
				else if ("instance".Equals(childName))
				{
					//we save parsing the instance node until the end, giving us the information we need about
					//binds and data types and such
					saveInstanceNode(child);
				}
				else if (BIND_ATTR.Equals(childName))
				{
					//<instance> must come before <bind>s
					parseBind(child);
				}
				else if ("submission".Equals(childName))
				{
					delayedParseElements.addElement(child);
				}
				else if (namedActions.contains(childName) || (childName != null && structuredActions.containsKey(childName)))
				{
					delayedParseElements.addElement(child);
				}
				else
				{
					//invalid model content
					if (type == Node.ELEMENT)
					{
						throw new XFormParseException("Unrecognized top-level tag [" + childName + "] found within <model>", child);
					}
					else if (type == Node.TEXT && getXMLText(e, i, true).length() != 0)
					{
						throw new XFormParseException("Unrecognized text content found within <model>: \"" + getXMLText(e, i, true) + "\"", child == null?e:child);
					}
				}
				
				if (child == null || BIND_ATTR.Equals(childName) || "itext".Equals(childName))
				{
					//Clayton Sims - Jun 17, 2009 - This code is used when the stinginess flag
					//is set for the build. It dynamically wipes out old model nodes once they're
					//used. This is sketchy if anything else plans on touching the nodes.
					//This code can be removed once we're pull-parsing
					//#if org.javarosa.xform.stingy
					e.removeChild(i);
					--i;
					//#endif
				}
			}
			
			//Now parse out the submission/action blocks (we needed the binds to all be set before we could)
			
			for(Element child: delayedParseElements)
			{
				System.String name = child.Name;
				if (name.Equals("submission"))
				{
					parseSubmission(child);
				}
				else
				{
					//For now, anything that isn't a submission is an action
					if (namedActions.contains(name))
					{
						parseNamedAction(child);
					}
					else
					{
						structuredActions.get_Renamed(name).handle(this, child, _f);
					}
				}
			}
		}
		
		private void  parseNamedAction(Element action)
		{
			//TODO: Anything useful
		}
		
		private void  parseSetValueAction(FormDef form, Element e)
		{
			System.String ref_Renamed = e.getAttributeValue(null, REF_ATTR);
			System.String bind = e.getAttributeValue(null, BIND_ATTR);
			
			System.String event_Renamed = e.getAttributeValue(null, "event");
			
			IDataReference dataRef = null;
			bool refFromBind = false;
			
			
			//TODO: There is a _lot_ of duplication of this code, fix that!
			if (bind != null)
			{
				DataBinding binding = bindingsByID.get_Renamed(bind);
				if (binding == null)
				{
					throw new XFormParseException("XForm Parse: invalid binding ID in submit'" + bind + "'", e);
				}
				dataRef = binding.Reference;
				refFromBind = true;
			}
			else if (ref_Renamed != null)
			{
				dataRef = new XPathReference(ref_Renamed);
			}
			else
			{
				throw new XFormParseException("setvalue action with no target!", e);
			}
			
			if (dataRef != null)
			{
				if (!refFromBind)
				{
					dataRef = getAbsRef(dataRef, TreeReference.rootRef());
				}
			}
			
			System.String valueRef = e.getAttributeValue(null, "value");
			Action action;
			TreeReference treeref = FormInstance.unpackReference(dataRef);
			
			actionTargets.addElement(treeref);
			if (valueRef == null)
			{
				if (e.getChildCount() == 0 || !e.isText(0))
				{
					throw new XFormParseException("No 'value' attribute and no inner value set in <setvalue> associated with: " + treeref, e);
				}
				//Set expression
				action = new SetValueAction(treeref, e.getText(0));
			}
			else
			{
				try
				{
					action = new SetValueAction(treeref, XPathParseTool.parseXPath(valueRef));
				}
				catch (XPathSyntaxException e1)
				{
					SupportClass.WriteStackTrace(e1, Console.Error);
					throw new XFormParseException("Invalid XPath in value set action declaration: '" + valueRef + "'", e);
				}
			}
			form.registerEventListener(event_Renamed, action);
		}
		
		private void  parseSubmission(Element submission)
		{
			System.String id = submission.getAttributeValue(null, ID_ATTR);
			
			//These two are always required
			System.String method = submission.getAttributeValue(null, "method");
			System.String action = submission.getAttributeValue(null, "action");
			
			SubmissionParser parser = new SubmissionParser();
			
			for(SubmissionParser p: submissionParsers)
			{
				if (p.matchesCustomMethod(method))
				{
					parser = p;
				}
			}
			
			//These two might exist, but if neither do, we just assume you want the entire instance.
			System.String ref_Renamed = submission.getAttributeValue(null, REF_ATTR);
			System.String bind = submission.getAttributeValue(null, BIND_ATTR);
			
			IDataReference dataRef = null;
			bool refFromBind = false;
			
			if (bind != null)
			{
				DataBinding binding = bindingsByID.get_Renamed(bind);
				if (binding == null)
				{
					throw new XFormParseException("XForm Parse: invalid binding ID in submit'" + bind + "'", submission);
				}
				dataRef = binding.Reference;
				refFromBind = true;
			}
			else if (ref_Renamed != null)
			{
				dataRef = new XPathReference(ref_Renamed);
			}
			else
			{
				//no reference! No big deal, assume we want the root reference
				dataRef = new XPathReference("/");
			}
			
			if (dataRef != null)
			{
				if (!refFromBind)
				{
					dataRef = getAbsRef(dataRef, TreeReference.rootRef());
				}
			}
			
			SubmissionProfile profile = parser.parseSubmission(method, action, dataRef, submission);
			
			if (id == null)
			{
				//default submission profile
				_f.setDefaultSubmission(profile);
			}
			else
			{
				//typed submission profile
				_f.addSubmissionProfile(id, profile);
			}
		}
		
		private void  saveInstanceNode(Element instance)
		{
			Element instanceNode = null;
			System.String instanceId = instance.getAttributeValue("", "id");
			
			for (int i = 0; i < instance.getChildCount(); i++)
			{
				if (instance.getType(i) == Node.ELEMENT)
				{
					if (instanceNode != null)
					{
						throw new XFormParseException("XForm Parse: <instance> has more than one child element", instance);
					}
					else
					{
						instanceNode = instance.getElement(i);
					}
				}
			}
			
			if (instanceNode == null)
			{
				//no kids
				instanceNode = instance;
			}
			
			if (mainInstanceNode == null)
			{
				mainInstanceNode = instanceNode;
			}
			
			instanceNodes.addElement(instanceNode);
			instanceNodeIdStrs.addElement(instanceId);
		}
		
		protected internal virtual void  processAdditionalAttributes(QuestionDef question, Element e, System.Collections.ArrayList usedAtts)
		{
			// save all the unused attributes verbatim...
			for (int i = 0; i < e.getAttributeCount(); i++)
			{
				System.String name = e.getAttributeName(i);
				if (usedAtts.Contains(name))
					continue;
				question.setAdditionalAttribute(e.getAttributeNamespace(i), name, e.getAttributeValue(i));
			}
			
			if (XFormUtils.showUnusedAttributeWarning(e, usedAtts))
			{
				reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(e, usedAtts), getVagueLocation(e));
			}
		}
		
		protected internal virtual QuestionDef parseUpload(IFormElement parent, Element e, int controlUpload)
		{
			System.Collections.ArrayList usedAtts = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			usedAtts.Add("mediatype");
			// get media type value
			System.String mediaType = e.getAttributeValue(null, "mediatype");
			// parse the control
			QuestionDef question = parseControl(parent, e, controlUpload, usedAtts);
			
			// apply the media type value to the returned question def.
			if ("image/*".Equals(mediaType))
			{
				// NOTE: this could be further expanded.
				question.ControlType = Constants.CONTROL_IMAGE_CHOOSE;
			}
			else if ("audio/*".Equals(mediaType))
			{
				question.ControlType = Constants.CONTROL_AUDIO_CAPTURE;
			}
			else if ("video/*".Equals(mediaType))
			{
				question.ControlType = Constants.CONTROL_VIDEO_CAPTURE;
			}
			return question;
		}
		
		protected internal virtual QuestionDef parseControl(IFormElement parent, Element e, int controlType)
		{
			
			return parseControl(parent, e, controlType, null);
		}
		
		protected internal virtual QuestionDef parseControl(IFormElement parent, Element e, int controlType, System.Collections.ArrayList additionalUsedAtts)
		{
			QuestionDef question = new QuestionDef();
			question.ID = serialQuestionID++; //until we come up with a better scheme
			
			System.Collections.ArrayList usedAtts = (additionalUsedAtts != null)?additionalUsedAtts:System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			usedAtts.Add(REF_ATTR);
			usedAtts.Add(BIND_ATTR);
			usedAtts.Add(APPEARANCE_ATTR);
			
			IDataReference dataRef = null;
			bool refFromBind = false;
			
			System.String ref_Renamed = e.getAttributeValue(null, REF_ATTR);
			System.String bind = e.getAttributeValue(null, BIND_ATTR);
			
			if (bind != null)
			{
				DataBinding binding = bindingsByID.get_Renamed(bind);
				if (binding == null)
				{
					throw new XFormParseException("XForm Parse: invalid binding ID '" + bind + "'", e);
				}
				dataRef = binding.Reference;
				refFromBind = true;
			}
			else if (ref_Renamed != null)
			{
				try
				{
					dataRef = new XPathReference(ref_Renamed);
				}
				catch (System.SystemException el)
				{
					//UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.Out.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintStreamprintln_javalangObject'"
					System.Console.Out.WriteLine(this.getVagueLocation(e));
					throw el;
				}
			}
			else
			{
				if (controlType == Constants.CONTROL_TRIGGER)
				{
					//TODO: special handling for triggers? also, not all triggers created equal
				}
				else
				{
					throw new XFormParseException("XForm Parse: input control with neither 'ref' nor 'bind'", e);
				}
			}
			
			if (dataRef != null)
			{
				if (!refFromBind)
				{
					dataRef = getAbsRef(dataRef, parent);
				}
				question.Bind = dataRef;
				
				if (controlType == Constants.CONTROL_SELECT_ONE)
				{
					selectOnes.addElement((TreeReference) dataRef.Reference);
				}
				else if (controlType == Constants.CONTROL_SELECT_MULTI)
				{
					selectMultis.addElement((TreeReference) dataRef.Reference);
				}
			}
			
			bool isSelect = (controlType == Constants.CONTROL_SELECT_MULTI || controlType == Constants.CONTROL_SELECT_ONE);
			question.ControlType = controlType;
			question.setAppearanceAttr(e.getAttributeValue(null, APPEARANCE_ATTR));
			
			for (int i = 0; i < e.getChildCount(); i++)
			{
				int type = e.getType(i);
				Element child = (type == Node.ELEMENT?e.getElement(i):null);
				System.String childName = (child != null?child.getName():null);
				
				if (LABEL_ELEMENT.Equals(childName))
				{
					parseQuestionLabel(question, child);
				}
				else if ("hint".Equals(childName))
				{
					parseHint(question, child);
				}
				else if (isSelect && "item".Equals(childName))
				{
					parseItem(question, child);
				}
				else if (isSelect && "itemset".Equals(childName))
				{
					parseItemset(question, child, parent);
				}
			}
			if (isSelect)
			{
				if (question.NumChoices > 0 && question.DynamicChoices != null)
				{
					throw new XFormParseException("Select question contains both literal choices and <itemset>");
				}
				else if (question.NumChoices == 0 && question.DynamicChoices == null)
				{
					throw new XFormParseException("Select question has no choices");
				}
			}
			
			parent.addChild(question);
			
			processAdditionalAttributes(question, e, usedAtts);
			
			return question;
		}
		
		private void  parseQuestionLabel(QuestionDef q, Element e)
		{
			System.String label = getLabel(e);
			System.String ref_Renamed = e.getAttributeValue("", REF_ATTR);
			
			System.Collections.ArrayList usedAtts = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			usedAtts.Add(REF_ATTR);
			
			if (ref_Renamed != null)
			{
				if (ref_Renamed.StartsWith(ITEXT_OPEN) && ref_Renamed.EndsWith(ITEXT_CLOSE))
				{
					System.String textRef = ref_Renamed.Substring(ITEXT_OPEN.Length, (ref_Renamed.IndexOf(ITEXT_CLOSE)) - (ITEXT_OPEN.Length));
					
					verifyTextMappings(textRef, "Question <label>", true);
					q.TextID = textRef;
				}
				else
				{
					throw new System.SystemException("malformed ref [" + ref_Renamed + "] for <label>");
				}
			}
			else
			{
				q.LabelInnerText = label;
			}
			
			
			if (XFormUtils.showUnusedAttributeWarning(e, usedAtts))
			{
				reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(e, usedAtts), getVagueLocation(e));
			}
		}
		
		private void  parseGroupLabel(GroupDef g, Element e)
		{
			if (g.Repeat)
				return ; //ignore child <label>s for <repeat>; the appropriate <label> must be in the wrapping <group>
			
			System.Collections.ArrayList usedAtts = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			usedAtts.Add(REF_ATTR);
			
			
			System.String label = getLabel(e);
			System.String ref_Renamed = e.getAttributeValue("", REF_ATTR);
			
			if (ref_Renamed != null)
			{
				if (ref_Renamed.StartsWith(ITEXT_OPEN) && ref_Renamed.EndsWith(ITEXT_CLOSE))
				{
					System.String textRef = ref_Renamed.Substring(ITEXT_OPEN.Length, (ref_Renamed.IndexOf(ITEXT_CLOSE)) - (ITEXT_OPEN.Length));
					
					verifyTextMappings(textRef, "Group <label>", true);
					g.TextID = textRef;
				}
				else
				{
					throw new System.SystemException("malformed ref [" + ref_Renamed + "] for <label>");
				}
			}
			else
			{
				g.LabelInnerText = label;
			}
			
			
			if (XFormUtils.showUnusedAttributeWarning(e, usedAtts))
			{
				reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(e, usedAtts), getVagueLocation(e));
			}
		}
		
		private System.String getLabel(Element e)
		{
			if (e.getChildCount() == 0)
				return null;
			
			recurseForOutput(e);
			
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < e.getChildCount(); i++)
			{
				if (e.getType(i) != Node.TEXT && !(e.getChild(i) is System.String))
				{
					System.Object b = e.getChild(i);
					Element child = (Element) b;
					
					//If the child is in the HTML namespace, retain it.
					if (NAMESPACE_HTML.Equals(child.getNamespace()))
					{
						sb.append(XFormSerializer.elementToString(child));
					}
					else
					{
						//Otherwise, ignore it.
						System.Console.Out.WriteLine("Unrecognized tag inside of text: <" + child.getName() + ">. " + "Did you intend to use HTML markup? If so, ensure that the element is defined in " + "the HTML namespace.");
					}
				}
				else
				{
					sb.append(e.getText(i));
				}
			}
			
			System.String s = sb.toString().trim();
			
			return s;
		}
		
		private void  recurseForOutput(Element e)
		{
			if (e.getChildCount() == 0)
				return ;
			
			for (int i = 0; i < e.getChildCount(); i++)
			{
				int kidType = e.getType(i);
				if (kidType == Node.TEXT)
				{
					continue;
				}
				if (e.getChild(i) is System.String)
				{
					continue;
				}
				Element kid = (Element) e.getChild(i);
				
				//is just text
				if (kidType == Node.ELEMENT && XFormUtils.isOutput(kid))
				{
					System.String s = "${" + parseOutput(kid) + "}";
					e.removeChild(i);
					e.addChild(i, Node.TEXT, s);
					
					//has kids? Recurse through them and swap output tag for parsed version
				}
				else if (kid.getChildCount() != 0)
				{
					recurseForOutput(kid);
					//is something else
				}
				else
				{
					continue;
				}
			}
		}
		
		private System.String parseOutput(Element e)
		{
			
			List< String > usedAtts = new List< String >();
			usedAtts.addElement(REF_ATTR);
			usedAtts.addElement(VALUE);
			
			System.String xpath = e.getAttributeValue(null, REF_ATTR);
			if (xpath == null)
			{
				xpath = e.getAttributeValue(null, VALUE);
			}
			if (xpath == null)
			{
				throw new XFormParseException("XForm Parse: <output> without 'ref' or 'value'", e);
			}
			
			XPathConditional expr = null;
			try
			{
				expr = new XPathConditional(xpath);
			}
			catch (XPathSyntaxException xse)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				reporter.error("Invalid XPath expression in <output> [" + xpath + "]! " + xse.Message);
				return "";
			}
			
			int index = - 1;
			if (_f.getOutputFragments().contains(expr))
			{
				index = _f.getOutputFragments().indexOf(expr);
			}
			else
			{
				index = _f.getOutputFragments().size();
				_f.getOutputFragments().addElement(expr);
			}
			
			if (XFormUtils.showUnusedAttributeWarning(e, usedAtts))
			{
				reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(e, usedAtts), getVagueLocation(e));
			}
			
			return System.Convert.ToString(index);
		}
		
		private void  parseHint(QuestionDef q, Element e)
		{
			System.Collections.ArrayList usedAtts = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			usedAtts.Add(REF_ATTR);
			System.String hint = getXMLText(e, true);
			System.String hintInnerText = getLabel(e);
			System.String ref_Renamed = e.getAttributeValue("", REF_ATTR);
			
			if (ref_Renamed != null)
			{
				if (ref_Renamed.StartsWith(ITEXT_OPEN) && ref_Renamed.EndsWith(ITEXT_CLOSE))
				{
					System.String textRef = ref_Renamed.Substring(ITEXT_OPEN.Length, (ref_Renamed.IndexOf(ITEXT_CLOSE)) - (ITEXT_OPEN.Length));
					
					verifyTextMappings(textRef, "<hint>", false);
					q.HelpTextID = textRef;
				}
				else
				{
					throw new System.SystemException("malformed ref [" + ref_Renamed + "] for <hint>");
				}
			}
			else
			{
				q.HelpInnerText = hintInnerText;
				q.HelpText = hint;
			}
			
			if (XFormUtils.showUnusedAttributeWarning(e, usedAtts))
			{
				reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(e, usedAtts), getVagueLocation(e));
			}
		}
		
		private void  parseItem(QuestionDef q, Element e)
		{
			//UPGRADE_NOTE: Final was removed from the declaration of 'MAX_VALUE_LEN '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			int MAX_VALUE_LEN = 32;
			
			//catalogue of used attributes in this method/element
			System.Collections.ArrayList usedAtts = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList labelUA = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList valueUA = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			labelUA.Add(REF_ATTR);
			valueUA.Add(FORM_ATTR);
			
			System.String labelInnerText = null;
			System.String textRef = null;
			System.String value_Renamed = null;
			
			for (int i = 0; i < e.getChildCount(); i++)
			{
				int type = e.getType(i);
				Element child = (type == Node.ELEMENT?e.getElement(i):null);
				System.String childName = (child != null?child.getName():null);
				
				if (LABEL_ELEMENT.Equals(childName))
				{
					
					//print attribute warning for child element
					if (XFormUtils.showUnusedAttributeWarning(child, labelUA))
					{
						reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(child, labelUA), getVagueLocation(child));
					}
					labelInnerText = getLabel(child);
					System.String ref_Renamed = child.getAttributeValue("", REF_ATTR);
					
					if (ref_Renamed != null)
					{
						if (ref_Renamed.StartsWith(ITEXT_OPEN) && ref_Renamed.EndsWith(ITEXT_CLOSE))
						{
							textRef = ref_Renamed.Substring(ITEXT_OPEN.Length, (ref_Renamed.IndexOf(ITEXT_CLOSE)) - (ITEXT_OPEN.Length));
							
							verifyTextMappings(textRef, "Item <label>", true);
						}
						else
						{
							throw new XFormParseException("malformed ref [" + ref_Renamed + "] for <item>", child);
						}
					}
				}
				else if (VALUE.Equals(childName))
				{
					value_Renamed = getXMLText(child, true);
					
					//print attribute warning for child element
					if (XFormUtils.showUnusedAttributeWarning(child, valueUA))
					{
						reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(child, valueUA), getVagueLocation(child));
					}
					
					if (value_Renamed != null)
					{
						if (value_Renamed.Length > MAX_VALUE_LEN)
						{
							reporter.warning(XFormParserReporter.TYPE_ERROR_PRONE, "choice value [" + value_Renamed + "] is too long; max. suggested length " + MAX_VALUE_LEN + " chars", getVagueLocation(child));
						}
						
						//validate
						for (int k = 0; k < value_Renamed.Length; k++)
						{
							char c = value_Renamed[k];
							
							if (" \n\t\f\r\'\"`".IndexOf((System.Char) c) >= 0)
							{
								bool isMultiSelect = (q.ControlType == Constants.CONTROL_SELECT_MULTI);
								reporter.warning(XFormParserReporter.TYPE_ERROR_PRONE, (isMultiSelect?SELECT:SELECTONE) + " question <value>s [" + value_Renamed + "] " + (isMultiSelect?"cannot":"should not") + " contain spaces, and are recommended not to contain apostraphes/quotation marks", getVagueLocation(child));
								break;
							}
						}
					}
				}
			}
			
			if (textRef == null && labelInnerText == null)
			{
				throw new XFormParseException("<item> without proper <label>", e);
			}
			if (value_Renamed == null)
			{
				throw new XFormParseException("<item> without proper <value>", e);
			}
			
			if (textRef != null)
			{
				q.addSelectChoice(new SelectChoice(textRef, value_Renamed));
			}
			else
			{
				q.addSelectChoice(new SelectChoice(null, labelInnerText, value_Renamed, false));
			}
			
			//print unused attribute warning message for parent element
			if (XFormUtils.showUnusedAttributeWarning(e, usedAtts))
			{
				reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(e, usedAtts), getVagueLocation(e));
			}
		}
		
		private void  parseItemset(QuestionDef q, Element e, IFormElement qparent)
		{
			ItemsetBinding itemset = new ItemsetBinding();
			
			////////////////USED FOR PARSER WARNING OUTPUT ONLY
			//catalogue of used attributes in this method/element
			System.Collections.ArrayList usedAtts = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList labelUA = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10)); //for child with name 'label'
			System.Collections.ArrayList valueUA = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10)); //for child with name 'value'
			System.Collections.ArrayList copyUA = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10)); //for child with name 'copy'
			usedAtts.Add(NODESET_ATTR);
			labelUA.Add(REF_ATTR);
			valueUA.Add(REF_ATTR);
			valueUA.Add(FORM_ATTR);
			copyUA.Add(REF_ATTR);
			////////////////////////////////////////////////////
			
			System.String nodesetStr = e.getAttributeValue("", NODESET_ATTR);
			if (nodesetStr == null)
				throw new System.SystemException("No nodeset attribute in element: [" + e.getName() + "]. This is required. (Element Printout:" + XFormSerializer.elementToString(e) + ")");
			XPathPathExpr path = XPathReference.getPathExpr(nodesetStr);
			itemset.nodesetExpr = new XPathConditional(path);
			itemset.contextRef = getFormElementRef(qparent);
			itemset.nodesetRef = FormInstance.unpackReference(getAbsRef(new XPathReference(path.getReference(true)), itemset.contextRef));
			
			for (int i = 0; i < e.getChildCount(); i++)
			{
				int type = e.getType(i);
				Element child = (type == Node.ELEMENT?e.getElement(i):null);
				System.String childName = (child != null?child.getName():null);
				
				if (LABEL_ELEMENT.Equals(childName))
				{
					System.String labelXpath = child.getAttributeValue("", REF_ATTR);
					bool labelItext = false;
					
					//print unused attribute warning message for child element
					if (XFormUtils.showUnusedAttributeWarning(child, labelUA))
					{
						reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(child, labelUA), getVagueLocation(child));
					}
					/////////////////////////////////////////////////////////////
					
					if (labelXpath != null)
					{
						if (labelXpath.StartsWith("jr:itext(") && labelXpath.EndsWith(")"))
						{
							labelXpath = labelXpath.Substring("jr:itext(".Length, (labelXpath.IndexOf(")")) - ("jr:itext(".Length));
							labelItext = true;
						}
					}
					else
					{
						throw new XFormParseException("<label> in <itemset> requires 'ref'");
					}
					
					XPathPathExpr labelPath = XPathReference.getPathExpr(labelXpath);
					itemset.labelRef = FormInstance.unpackReference(getAbsRef(new XPathReference(labelPath), itemset.nodesetRef));
					itemset.labelExpr = new XPathConditional(labelPath);
					itemset.labelIsItext = labelItext;
				}
				else if ("copy".Equals(childName))
				{
					System.String copyRef = child.getAttributeValue("", REF_ATTR);
					
					//print unused attribute warning message for child element
					if (XFormUtils.showUnusedAttributeWarning(child, copyUA))
					{
						reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(child, copyUA), getVagueLocation(child));
					}
					
					if (copyRef == null)
					{
						throw new XFormParseException("<copy> in <itemset> requires 'ref'");
					}
					
					itemset.copyRef = FormInstance.unpackReference(getAbsRef(new XPathReference(copyRef), itemset.nodesetRef));
					itemset.copyMode = true;
				}
				else if (VALUE.Equals(childName))
				{
					System.String valueXpath = child.getAttributeValue("", REF_ATTR);
					
					//print unused attribute warning message for child element
					if (XFormUtils.showUnusedAttributeWarning(child, valueUA))
					{
						reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(child, valueUA), getVagueLocation(child));
					}
					
					if (valueXpath == null)
					{
						throw new XFormParseException("<value> in <itemset> requires 'ref'");
					}
					
					XPathPathExpr valuePath = XPathReference.getPathExpr(valueXpath);
					itemset.valueRef = FormInstance.unpackReference(getAbsRef(new XPathReference(valuePath), itemset.nodesetRef));
					itemset.valueExpr = new XPathConditional(valuePath);
					itemset.copyMode = false;
				}
			}
			
			if (itemset.labelRef == null)
			{
				throw new XFormParseException("<itemset> requires <label>");
			}
			else if (itemset.copyRef == null && itemset.valueRef == null)
			{
				throw new XFormParseException("<itemset> requires <copy> or <value>");
			}
			
			if (itemset.copyRef != null)
			{
				if (itemset.valueRef == null)
				{
					reporter.warning(XFormParserReporter.TYPE_TECHNICAL, "<itemset>s with <copy> are STRONGLY recommended to have <value> as well; pre-selecting, default answers, and display of answers will not work properly otherwise", getVagueLocation(e));
				}
				else if (!itemset.copyRef.isParentOf(itemset.valueRef, false))
				{
					throw new XFormParseException("<value> is outside <copy>");
				}
			}
			
			q.DynamicChoices = itemset;
			itemsets.addElement(itemset);
			
			//print unused attribute warning message for parent element
			if (XFormUtils.showUnusedAttributeWarning(e, usedAtts))
			{
				reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(e, usedAtts), getVagueLocation(e));
			}
		}
		
		private void  parseGroup(IFormElement parent, Element e, int groupType)
		{
			GroupDef group = new GroupDef();
			group.ID = serialQuestionID++; //until we come up with a better scheme
			IDataReference dataRef = null;
			bool refFromBind = false;
			
			System.Collections.ArrayList usedAtts = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			usedAtts.Add(REF_ATTR);
			usedAtts.Add(NODESET_ATTR);
			usedAtts.Add(BIND_ATTR);
			usedAtts.Add(APPEARANCE_ATTR);
			usedAtts.Add("count");
			usedAtts.Add("noAddRemove");
			
			if (groupType == CONTAINER_REPEAT)
			{
				group.Repeat = true;
			}
			
			System.String ref_Renamed = e.getAttributeValue(null, REF_ATTR);
			System.String nodeset = e.getAttributeValue(null, NODESET_ATTR);
			System.String bind = e.getAttributeValue(null, BIND_ATTR);
			group.setAppearanceAttr(e.getAttributeValue(null, APPEARANCE_ATTR));
			
			if (bind != null)
			{
				DataBinding binding = bindingsByID.get_Renamed(bind);
				if (binding == null)
				{
					throw new XFormParseException("XForm Parse: invalid binding ID [" + bind + "]", e);
				}
				dataRef = binding.Reference;
				refFromBind = true;
			}
			else
			{
				if (group.Repeat)
				{
					if (nodeset != null)
					{
						dataRef = new XPathReference(nodeset);
					}
					else
					{
						throw new XFormParseException("XForm Parse: <repeat> with no binding ('bind' or 'nodeset')", e);
					}
				}
				else
				{
					if (ref_Renamed != null)
					{
						dataRef = new XPathReference(ref_Renamed);
					} //<group> not required to have a binding
				}
			}
			
			if (!refFromBind)
			{
				dataRef = getAbsRef(dataRef, parent);
			}
			group.Bind = dataRef;
			
			if (group.Repeat)
			{
				repeats.addElement((TreeReference) dataRef.Reference);
				
				System.String countRef = e.getAttributeValue(NAMESPACE_JAVAROSA, "count");
				if (countRef != null)
				{
					group.count = getAbsRef(new XPathReference(countRef), parent);
					group.noAddRemove = true;
				}
				else
				{
					group.noAddRemove = (e.getAttributeValue(NAMESPACE_JAVAROSA, "noAddRemove") != null);
				}
			}
			
			for (int i = 0; i < e.getChildCount(); i++)
			{
				int type = e.getType(i);
				Element child = (type == Node.ELEMENT?e.getElement(i):null);
				System.String childName = (child != null?child.getName():null);
				System.String childNamespace = (child != null?child.getNamespace():null);
				
				if (group.Repeat && NAMESPACE_JAVAROSA.Equals(childNamespace))
				{
					if ("chooseCaption".Equals(childName))
					{
						group.chooseCaption = getLabel(child);
					}
					else if ("addCaption".Equals(childName))
					{
						group.addCaption = getLabel(child);
					}
					else if ("delCaption".Equals(childName))
					{
						group.delCaption = getLabel(child);
					}
					else if ("doneCaption".Equals(childName))
					{
						group.doneCaption = getLabel(child);
					}
					else if ("addEmptyCaption".Equals(childName))
					{
						group.addEmptyCaption = getLabel(child);
					}
					else if ("doneEmptyCaption".Equals(childName))
					{
						group.doneEmptyCaption = getLabel(child);
					}
					else if ("entryHeader".Equals(childName))
					{
						group.entryHeader = getLabel(child);
					}
					else if ("delHeader".Equals(childName))
					{
						group.delHeader = getLabel(child);
					}
					else if ("mainHeader".Equals(childName))
					{
						group.mainHeader = getLabel(child);
					}
				}
			}
			
			//the case of a group wrapping a repeat is cleaned up in a post-processing step (collapseRepeatGroups)
			
			for (int i = 0; i < e.getChildCount(); i++)
			{
				if (e.getType(i) == Element.ELEMENT)
				{
					parseElement(e.getElement(i), group, groupLevelHandlers);
				}
			}
			
			// save all the unused attributes verbatim...
			for (int i = 0; i < e.getAttributeCount(); i++)
			{
				System.String name = e.getAttributeName(i);
				if (usedAtts.Contains(name))
					continue;
				group.setAdditionalAttribute(e.getAttributeNamespace(i), name, e.getAttributeValue(i));
			}
			
			//print unused attribute warning message for parent element
			if (XFormUtils.showUnusedAttributeWarning(e, usedAtts))
			{
				reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(e, usedAtts), getVagueLocation(e));
			}
			
			parent.addChild(group);
		}
		
		private TreeReference getFormElementRef(IFormElement fe)
		{
			if (fe is FormDef)
			{
				TreeReference ref_Renamed = TreeReference.rootRef();
				ref_Renamed.add(mainInstanceNode.getName(), 0);
				return ref_Renamed;
			}
			else
			{
				return (TreeReference) fe.getBind().getReference();
			}
		}
		
		private IDataReference getAbsRef(IDataReference ref_Renamed, IFormElement parent)
		{
			return getAbsRef(ref_Renamed, getFormElementRef(parent));
		}
		
		//take a (possibly relative) reference, and make it absolute based on its parent
		private static IDataReference getAbsRef(IDataReference ref_Renamed, TreeReference parentRef)
		{
			TreeReference tref;
			
			if (!parentRef.Absolute)
			{
				throw new System.SystemException("XFormParser.getAbsRef: parentRef must be absolute");
			}
			
			if (ref_Renamed != null)
			{
				tref = (TreeReference) ref_Renamed.Reference;
			}
			else
			{
				tref = TreeReference.selfRef(); //only happens for <group>s with no binding
			}
			
			tref = tref.parent(parentRef);
			if (tref == null)
			{
				throw new XFormParseException("Binding path [" + tref + "] not allowed with parent binding of [" + parentRef + "]");
			}
			
			return new XPathReference(tref);
		}
		
		//collapse groups whose only child is a repeat into a single repeat that uses the label of the wrapping group
		private static void  collapseRepeatGroups(IFormElement fe)
		{
			if (fe.getChildren() == null)
				return ;
			
			for (int i = 0; i < fe.getChildren().size(); i++)
			{
				IFormElement child = fe.getChild(i);
				GroupDef group = null;
				if (child is GroupDef)
					group = (GroupDef) child;
				
				if (group != null)
				{
					if (!group.Repeat && group.Children.Count == 1)
					{
						IFormElement grandchild = (IFormElement) group.Children[0];
						GroupDef repeat = null;
						if (grandchild is GroupDef)
							repeat = (GroupDef) grandchild;
						
						if (repeat != null && repeat.Repeat)
						{
							//collapse the wrapping group
							
							//merge group into repeat
							//id - later
							//name - later
							repeat.LabelInnerText = group.LabelInnerText;
							repeat.TextID = group.TextID;
							//						repeat.setLongText(group.getLongText());
							//						repeat.setShortText(group.getShortText());
							//						repeat.setLongTextID(group.getLongTextID(), null);
							//						repeat.setShortTextID(group.getShortTextID(), null);
							//don't merge binding; repeat will always already have one
							
							//replace group with repeat
							fe.getChildren().setElementAt(repeat, i);
							group = repeat;
						}
					}
					
					collapseRepeatGroups(group);
				}
			}
		}
		
		private void  parseIText(Element itext)
		{
			Localizer l = new Localizer(true, true);
			_f.Localizer = l;
			l.registerLocalizable(_f);
			
			System.Collections.ArrayList usedAtts = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10)); //used for warning message
			
			for (int i = 0; i < itext.getChildCount(); i++)
			{
				Element trans = itext.getElement(i);
				if (trans == null || !trans.getName().equals("translation"))
					continue;
				
				parseTranslation(l, trans);
			}
			
			if (l.AvailableLocales.Length == 0)
				throw new XFormParseException("no <translation>s defined", itext);
			
			if (l.DefaultLocale == null)
				l.DefaultLocale = l.AvailableLocales[0];
			
			//print unused attribute warning message for parent element
			if (XFormUtils.showUnusedAttributeWarning(itext, usedAtts))
			{
				reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(itext, usedAtts), getVagueLocation(itext));
			}
		}
		
		private void  parseTranslation(Localizer l, Element trans)
		{
			/////for warning message
			System.Collections.ArrayList usedAtts = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			usedAtts.Add("lang");
			usedAtts.Add("default");
			/////////////////////////
			
			System.String lang = trans.getAttributeValue("", "lang");
			if (lang == null || lang.Length == 0)
			{
				throw new XFormParseException("no language specified for <translation>", trans);
			}
			System.String isDefault = trans.getAttributeValue("", "default");
			
			if (!l.addAvailableLocale(lang))
			{
				throw new XFormParseException("duplicate <translation> for language '" + lang + "'", trans);
			}
			
			if (isDefault != null)
			{
				if (l.DefaultLocale != null)
					throw new XFormParseException("more than one <translation> set as default", trans);
				l.DefaultLocale = lang;
			}
			
			TableLocaleSource source = new TableLocaleSource();
			
			//source.startEditing();
			for (int j = 0; j < trans.getChildCount(); j++)
			{
				Element text = trans.getElement(j);
				if (text == null || !text.getName().equals("text"))
				{
					continue;
				}
				
				parseTextHandle(source, text);
				//Clayton Sims - Jun 17, 2009 - This code is used when the stinginess flag
				//is set for the build. It dynamically wipes out old model nodes once they're
				//used. This is sketchy if anything else plans on touching the nodes.
				//This code can be removed once we're pull-parsing
				//#if org.javarosa.xform.stingy
				trans.removeChild(j);
				--j;
				//#endif
			}
			
			//print unused attribute warning message for parent element
			if (XFormUtils.showUnusedAttributeWarning(trans, usedAtts))
			{
				reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(trans, usedAtts), getVagueLocation(trans));
			}
			
			//source.stopEditing();
			l.registerLocaleResource(lang, source);
		}
		
		private void  parseTextHandle(TableLocaleSource l, Element text)
		{
			System.String id = text.getAttributeValue("", ID_ATTR);
			
			//used for parser warnings...
			System.Collections.ArrayList usedAtts = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList childUsedAtts = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			usedAtts.Add(ID_ATTR);
			usedAtts.Add(FORM_ATTR);
			childUsedAtts.Add(FORM_ATTR);
			childUsedAtts.Add(ID_ATTR);
			//////////
			
			if (id == null || id.Length == 0)
			{
				throw new XFormParseException("no id defined for <text>", text);
			}
			
			for (int k = 0; k < text.getChildCount(); k++)
			{
				Element value_Renamed = text.getElement(k);
				if (value_Renamed == null)
					continue;
				if (!value_Renamed.getName().equals(VALUE))
				{
					throw new XFormParseException("Unrecognized element [" + value_Renamed.getName() + "] in Itext->translation->text");
				}
				
				System.String form = value_Renamed.getAttributeValue("", FORM_ATTR);
				if (form != null && form.Length == 0)
				{
					form = null;
				}
				System.String data = getLabel(value_Renamed);
				if (data == null)
				{
					data = "";
				}
				
				System.String textID = (form == null?id:id + ";" + form); //kind of a hack
				if (l.hasMapping(textID))
				{
					throw new XFormParseException("duplicate definition for text ID \"" + id + "\" and form \"" + form + "\"" + ". Can only have one definition for each text form.", text);
				}
				l.setLocaleMapping(textID, data);
				
				//print unused attribute warning message for child element
				if (XFormUtils.showUnusedAttributeWarning(value_Renamed, childUsedAtts))
				{
					reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(value_Renamed, childUsedAtts), getVagueLocation(value_Renamed));
				}
			}
			
			//print unused attribute warning message for parent element
			if (XFormUtils.showUnusedAttributeWarning(text, usedAtts))
			{
				reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(text, usedAtts), getVagueLocation(text));
			}
		}
		
		private bool hasITextMapping(System.String textID, System.String locale)
		{
			Localizer l = _f.getLocalizer();
			return l.hasMapping(locale == null?l.DefaultLocale:locale, textID);
		}
		
		private void  verifyTextMappings(System.String textID, System.String type, bool allowSubforms)
		{
			Localizer l = _f.getLocalizer();
			System.String[] locales = l.AvailableLocales;
			
			for (int i = 0; i < locales.Length; i++)
			{
				//Test whether there is a default translation, or whether there is any special form available.
				if (!(hasITextMapping(textID, locales[i]) || (allowSubforms && hasSpecialFormMapping(textID, locales[i]))))
				{
					if (locales[i].Equals(l.DefaultLocale))
					{
						throw new XFormParseException(type + " '" + textID + "': text is not localizable for default locale [" + l.DefaultLocale + "]!");
					}
					else
					{
						reporter.warning(XFormParserReporter.TYPE_TECHNICAL, type + " '" + textID + "': text is not localizable for locale " + locales[i] + ".", null);
					}
				}
			}
		}
		
		/// <summary> Tests whether or not there is any form (default or special) for the provided
		/// text id.
		/// 
		/// </summary>
		/// <returns> True if a translation is present for the given textID in the form. False otherwise
		/// </returns>
		private bool hasSpecialFormMapping(System.String textID, System.String locale)
		{
			//First check our guesses
			
			for(String guess: itextKnownForms)
			{
				if (hasITextMapping(textID + ";" + guess, locale))
				{
					return true;
				}
			}
			//Otherwise this sucks and we have to test the keys
			
			OrderedMap < String, PrefixTreeNode > table = _f.getLocalizer().getLocaleData(locale);
			
			for(String key: table.keySet())
			{
				if (key.startsWith(textID + ";"))
				{
					//A key is found, pull it out, add it to the list of guesses, and return positive
					System.String textForm = key.substring(key.indexOf(";") + 1, key.length());
					//Kind of a long story how we can end up getting here. It involves the default locale loading values
					//for the other locale, but isn't super good.
					//TODO: Clean up being able to get here
					if (!itextKnownForms.contains(textForm))
					{
						System.Console.Out.WriteLine("adding unexpected special itext form: " + textForm + " to list of expected forms");
						itextKnownForms.addElement(textForm);
					}
					return true;
				}
			}
			return false;
		}
		
		protected internal virtual DataBinding processStandardBindAttributes(System.Collections.ArrayList usedAtts, Element e)
		{
			usedAtts.Add(ID_ATTR);
			usedAtts.Add(NODESET_ATTR);
			usedAtts.Add("type");
			usedAtts.Add("relevant");
			usedAtts.Add("required");
			usedAtts.Add("readonly");
			usedAtts.Add("constraint");
			usedAtts.Add("constraintMsg");
			usedAtts.Add("calculate");
			usedAtts.Add("preload");
			usedAtts.Add("preloadParams");
			
			DataBinding binding = new DataBinding();
			
			
			binding.setId(e.getAttributeValue("", ID_ATTR));
			
			System.String nodeset = e.getAttributeValue(null, NODESET_ATTR);
			if (nodeset == null)
			{
				throw new XFormParseException("XForm Parse: <bind> without nodeset", e);
			}
			IDataReference ref_Renamed;
			try
			{
				ref_Renamed = new XPathReference(nodeset);
			}
			catch (XPathException xpe)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new XFormParseException(xpe.Message);
			}
			ref_Renamed = getAbsRef(ref_Renamed, _f);
			binding.Reference = ref_Renamed;
			
			binding.setDataType(getDataType(e.getAttributeValue(null, "type")));
			
			System.String xpathRel = e.getAttributeValue(null, "relevant");
			if (xpathRel != null)
			{
				if ("true()".Equals(xpathRel))
				{
					binding.relevantAbsolute = true;
				}
				else if ("false()".Equals(xpathRel))
				{
					binding.relevantAbsolute = false;
				}
				else
				{
					Condition c = buildCondition(xpathRel, "relevant", ref_Renamed);
					c = (Condition) _f.addTriggerable(c);
					binding.relevancyCondition = c;
				}
			}
			
			System.String xpathReq = e.getAttributeValue(null, "required");
			if (xpathReq != null)
			{
				if ("true()".Equals(xpathReq))
				{
					binding.requiredAbsolute = true;
				}
				else if ("false()".Equals(xpathReq))
				{
					binding.requiredAbsolute = false;
				}
				else
				{
					Condition c = buildCondition(xpathReq, "required", ref_Renamed);
					c = (Condition) _f.addTriggerable(c);
					binding.requiredCondition = c;
				}
			}
			
			System.String xpathRO = e.getAttributeValue(null, "readonly");
			if (xpathRO != null)
			{
				if ("true()".Equals(xpathRO))
				{
					binding.readonlyAbsolute = true;
				}
				else if ("false()".Equals(xpathRO))
				{
					binding.readonlyAbsolute = false;
				}
				else
				{
					Condition c = buildCondition(xpathRO, "readonly", ref_Renamed);
					c = (Condition) _f.addTriggerable(c);
					binding.readonlyCondition = c;
				}
			}
			
			System.String xpathConstr = e.getAttributeValue(null, "constraint");
			if (xpathConstr != null)
			{
				try
				{
					binding.constraint = new XPathConditional(xpathConstr);
				}
				catch (XPathSyntaxException xse)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					throw new XFormParseException("bind for " + nodeset + " contains invalid constraint expression [" + xpathConstr + "] " + xse.Message);
				}
				binding.constraintMessage = e.getAttributeValue(NAMESPACE_JAVAROSA, "constraintMsg");
			}
			
			System.String xpathCalc = e.getAttributeValue(null, "calculate");
			if (xpathCalc != null)
			{
				Recalculate r;
				try
				{
					r = buildCalculate(xpathCalc, ref_Renamed);
				}
				catch (XPathSyntaxException xpse)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					throw new XFormParseException("Invalid calculate for the bind attached to \"" + nodeset + "\" : " + xpse.Message + " in expression " + xpathCalc);
				}
				r = (Recalculate) _f.addTriggerable(r);
				binding.calculate = r;
			}
			
			binding.setPreload(e.getAttributeValue(NAMESPACE_JAVAROSA, "preload"));
			binding.setPreloadParams(e.getAttributeValue(NAMESPACE_JAVAROSA, "preloadParams"));
			
			// save all the unused attributes verbatim...
			for (int i = 0; i < e.getAttributeCount(); i++)
			{
				System.String name = e.getAttributeName(i);
				if (usedAtts.Contains(name))
					continue;
				binding.setAdditionalAttribute(e.getAttributeNamespace(i), name, e.getAttributeValue(i));
			}
			
			return binding;
		}
		
		protected internal virtual void  parseBind(Element e)
		{
			System.Collections.ArrayList usedAtts = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			
			DataBinding binding = processStandardBindAttributes(usedAtts, e);
			
			//print unused attribute warning message for parent element
			if (XFormUtils.showUnusedAttributeWarning(e, usedAtts))
			{
				reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(e, usedAtts), getVagueLocation(e));
			}
			
			addBinding(binding);
		}
		
		private Condition buildCondition(System.String xpath, System.String type, IDataReference contextRef)
		{
			XPathConditional cond;
			int trueAction = - 1, falseAction = - 1;
			
			System.String prettyType;
			
			if ("relevant".Equals(type))
			{
				prettyType = "display condition";
				trueAction = Condition.ACTION_SHOW;
				falseAction = Condition.ACTION_HIDE;
			}
			else if ("required".Equals(type))
			{
				prettyType = "require condition";
				trueAction = Condition.ACTION_REQUIRE;
				falseAction = Condition.ACTION_DONT_REQUIRE;
			}
			else if ("readonly".Equals(type))
			{
				prettyType = "readonly condition";
				trueAction = Condition.ACTION_DISABLE;
				falseAction = Condition.ACTION_ENABLE;
			}
			else
			{
				prettyType = "unknown condition";
			}
			
			try
			{
				cond = new XPathConditional(xpath);
			}
			catch (XPathSyntaxException xse)
			{
				
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.String errorMessage = "Encountered a problem with " + prettyType + " for node [" + contextRef.Reference.ToString() + "] at line: " + xpath + ", " + xse.Message;
				
				reporter.error(errorMessage);
				
				throw new XFormParseException(errorMessage);
			}
			
			Condition c = new Condition(cond, trueAction, falseAction, FormInstance.unpackReference(contextRef));
			return c;
		}
		
		private static Recalculate buildCalculate(System.String xpath, IDataReference contextRef)
		{
			XPathConditional calc = new XPathConditional(xpath);
			
			Recalculate r = new Recalculate(calc, FormInstance.unpackReference(contextRef));
			return r;
		}
		
		protected internal virtual void  addBinding(DataBinding binding)
		{
			bindings.addElement(binding);
			
			if (binding.Id != null)
			{
				if (bindingsByID.put(binding.Id, binding) != null)
				{
					throw new XFormParseException("XForm Parse: <bind>s with duplicate ID: '" + binding.Id + "'");
				}
			}
		}
		
		//e is the top-level _data_ node of the instance (immediate (and only) child of <instance>)
		private void  addMainInstanceToFormDef(Element e, FormInstance instanceModel)
		{
			//TreeElement root = buildInstanceStructure(e, null);
			loadInstanceData(e, instanceModel.getRoot(), _f);
			
			checkDependencyCycles();
			_f.Instance = instanceModel;
			try
			{
				_f.finalizeTriggerables();
			}
			catch (System.SystemException ise)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new XFormParseException(ise.Message == null?"Form has an illegal cycle in its calculate and relevancy expressions!":ise.Message);
			}
			
			//print unused attribute warning message for parent element
			//if(XFormUtils.showUnusedAttributeWarning(e, usedAtts)){
			//	reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(e, usedAtts), getVagueLocation(e));
			//}
		}
		
		private FormInstance parseInstance(Element e, bool isMainInstance)
		{
			System.String name = instanceNodeIdStrs.elementAt(instanceNodes.indexOf(e));
			
			TreeElement root = buildInstanceStructure(e, null, !isMainInstance?name:null, e.getNamespace());
			FormInstance instanceModel = new FormInstance(root);
			if (isMainInstance)
			{
				instanceModel.setName(_f.getTitle());
			}
			else
			{
				instanceModel.Name = name;
			}
			
			System.Collections.ArrayList usedAtts = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			usedAtts.Add("id");
			usedAtts.Add("version");
			usedAtts.Add("uiVersion");
			usedAtts.Add("name");
			
			System.String schema = e.getNamespace();
			if (schema != null && schema.Length > 0 && !schema.Equals(defaultNamespace))
			{
				instanceModel.schema = schema;
			}
			instanceModel.formVersion = e.getAttributeValue(null, "version");
			instanceModel.uiVersion = e.getAttributeValue(null, "uiVersion");
			
			loadNamespaces(e, instanceModel);
			if (isMainInstance)
			{
				processRepeats(instanceModel);
				verifyBindings(instanceModel);
				verifyActions(instanceModel);
			}
			applyInstanceProperties(instanceModel);
			
			//print unused attribute warning message for parent element
			if (XFormUtils.showUnusedAttributeWarning(e, usedAtts))
			{
				reporter.warning(XFormParserReporter.TYPE_UNKNOWN_MARKUP, XFormUtils.unusedAttWarning(e, usedAtts), getVagueLocation(e));
			}
			
			return instanceModel;
		}
		
		
		
		
		private static HashMap < String, String > loadNamespaces(Element e, FormInstance tree)
		
		public static TreeElement buildInstanceStructure(Element node, TreeElement parent)
		{
			return buildInstanceStructure(node, parent, null, node.getNamespace());
		}
		
		//parse instance hierarchy and turn into a skeleton model; ignoring data content, but respecting repeated nodes and 'template' flags
		public static TreeElement buildInstanceStructure(Element node, TreeElement parent, System.String instanceName, System.String docnamespace)
		{
			TreeElement element = null;
			
			//catch when text content is mixed with children
			int numChildren = node.getChildCount();
			bool hasText = false;
			bool hasElements = false;
			for (int i = 0; i < numChildren; i++)
			{
				switch (node.getType(i))
				{
					
					case Node.ELEMENT: 
						hasElements = true; break;
					
					case Node.TEXT: 
						if (node.getText(i).trim().length() > 0)
							hasText = true;
						break;
					}
			}
			if (hasElements && hasText)
			{
				System.Console.Out.WriteLine("Warning: instance node '" + node.getName() + "' contains both elements and text as children; text ignored");
			}
			
			//check for repeat templating
			System.String name = node.getName();
			int multiplicity;
			if (node.getAttributeValue(NAMESPACE_JAVAROSA, "template") != null)
			{
				multiplicity = TreeReference.INDEX_TEMPLATE;
				if (parent != null && parent.getChild(name, TreeReference.INDEX_TEMPLATE) != null)
				{
					throw new XFormParseException("More than one node declared as the template for the same repeated set [" + name + "]", node);
				}
			}
			else
			{
				multiplicity = (parent == null?0:parent.getChildMultiplicity(name));
			}
			
			
			System.String modelType = node.getAttributeValue(NAMESPACE_JAVAROSA, "modeltype");
			//create node; handle children
			if (modelType == null)
			{
				element = new TreeElement(name, multiplicity);
				element.InstanceName = instanceName;
			}
			else
			{
				if (typeMappings.get_Renamed(modelType) == null)
				{
					throw new XFormParseException("ModelType " + modelType + " is not recognized.", node);
				}
				element = (TreeElement) modelPrototypes.getNewInstance(((System.Int32) typeMappings.get_Renamed(modelType)).ToString());
				if (element == null)
				{
					element = new TreeElement(name, multiplicity);
					System.Console.Out.WriteLine("No model type prototype available for " + modelType);
				}
				else
				{
					element.Name = name;
					element.Mult = multiplicity;
				}
			}
			if (node.getNamespace() != null)
			{
				if (!node.getNamespace().equals(docnamespace))
				{
					element.setNamespace(node.getNamespace());
				}
			}
			
			
			if (hasElements)
			{
				for (int i = 0; i < numChildren; i++)
				{
					if (node.getType(i) == Node.ELEMENT)
					{
						element.addChild(buildInstanceStructure(node.getElement(i), element, instanceName, docnamespace));
					}
				}
			}
			
			//handle attributes
			if (node.getAttributeCount() > 0)
			{
				for (int i = 0; i < node.getAttributeCount(); i++)
				{
					System.String attrNamespace = node.getAttributeNamespace(i);
					System.String attrName = node.getAttributeName(i);
					if (attrNamespace.Equals(NAMESPACE_JAVAROSA) && attrName.Equals("template"))
					{
						continue;
					}
					if (attrNamespace.Equals(NAMESPACE_JAVAROSA) && attrName.Equals("recordset"))
					{
						continue;
					}
					
					element.setAttribute(attrNamespace, attrName, node.getAttributeValue(i));
				}
			}
			
			return element;
		}
		
		
		private List< TreeReference > getRepeatableRefs()
		
		//pre-process and clean up instance regarding repeats; in particular:
		// 1) flag all repeat-related nodes as repeatable
		// 2) catalog which repeat template nodes are explicitly defined, and note which repeats bindings lack templates
		// 3) remove template nodes that are not valid for a repeat binding
		// 4) generate template nodes for repeat bindings that do not have one defined explicitly
		// 5) give a stern warning for any repeated instance nodes that do not correspond to a repeat binding
		// 6) verify that all sets of repeated nodes are homogeneous
		private void  processRepeats(FormInstance instance)
		{
			flagRepeatables(instance);
			processTemplates(instance);
			checkDuplicateNodesAreRepeatable(instance.getRoot());
			checkHomogeneity(instance);
		}
		
		//flag all nodes identified by repeat bindings as repeatable
		private void  flagRepeatables(FormInstance instance)
		{
			
			for (int i = 0; i < refs.size(); i++)
			{
				TreeReference ref_Renamed = refs.elementAt(i);
				
				for (int j = 0; j < nodes.size(); j++)
				{
					TreeReference nref = nodes.elementAt(j);
					TreeElement node = instance.resolveReference(nref);
					if (node != null)
					{
						// catch '/'
						node.Repeatable = true;
					}
				}
			}
		}
		
		private void  processTemplates(FormInstance instance)
		{
			repeatTree = buildRepeatTree(getRepeatableRefs(), instance.getRoot().Name);
			
			
			List< TreeReference > missingTemplates = new List< TreeReference >();
			checkRepeatsForTemplate(instance, repeatTree, missingTemplates);
			
			removeInvalidTemplates(instance, repeatTree);
			createMissingTemplates(instance, missingTemplates);
		}
		
		//build a pseudo-data model tree that describes the repeat structure of the instance
		//result is a FormInstance collapsed where all indexes are 0, and repeatable nodes are flagged as such
		//return null if no repeats
		//ignores (invalid) repeats that bind outside the top-level instance data node
		private static FormInstance buildRepeatTree;
		
		(List< TreeReference > repeatRefs, String topLevelName)
		
		//checks which repeat bindings have explicit template nodes; returns a vector of the bindings that do not
		
		private static
		
		void checkRepeatsForTemplate(FormInstance instance, FormInstance repeatTree, List< TreeReference > missingTemplates)
		
		//helper function for checkRepeatsForTemplate
		
		private static
		
		void checkRepeatsForTemplate(TreeElement repeatTreeNode, TreeReference ref, FormInstance instance, List< TreeReference > missing)
		
		//iterates through instance and removes template nodes that are not valid. a template is invalid if:
		//  it is declared for a node that is not repeatable
		//  it is for a repeat that is a child of another repeat and is not located within the parent's template node
		private void  removeInvalidTemplates(FormInstance instance, FormInstance repeatTree)
		{
			removeInvalidTemplates(instance.getRoot(), (repeatTree == null?null:repeatTree.getRoot()), true);
		}
		
		//helper function for removeInvalidTemplates
		private bool removeInvalidTemplates(TreeElement instanceNode, TreeElement repeatTreeNode, bool templateAllowed)
		{
			int mult = instanceNode.Mult;
			bool repeatable = (repeatTreeNode == null?false:repeatTreeNode.Repeatable);
			
			if (mult == TreeReference.INDEX_TEMPLATE)
			{
				if (!templateAllowed)
				{
					reporter.warning(XFormParserReporter.TYPE_INVALID_STRUCTURE, "Template nodes for sub-repeats must be located within the template node of the parent repeat; ignoring template... [" + instanceNode.Name + "]", null);
					return true;
				}
				else if (!repeatable)
				{
					reporter.warning(XFormParserReporter.TYPE_INVALID_STRUCTURE, "Warning: template node found for ref that is not repeatable; ignoring... [" + instanceNode.Name + "]", null);
					return true;
				}
			}
			
			if (repeatable && mult != TreeReference.INDEX_TEMPLATE)
				templateAllowed = false;
			
			for (int i = 0; i < instanceNode.NumChildren; i++)
			{
				TreeElement child = instanceNode.getChildAt(i);
				TreeElement rchild = (repeatTreeNode == null?null:repeatTreeNode.getChild(child.Name, 0));
				
				if (removeInvalidTemplates(child, rchild, templateAllowed))
				{
					instanceNode.removeChildAt(i);
					i--;
				}
			}
			return false;
		}
		
		//if repeatables have no template node, duplicate first as template
		
		private
		
		void createMissingTemplates(FormInstance instance, List< TreeReference > missingTemplates)
		
		//trim repeatable children of newly created template nodes; we trim because the templates are supposed to be devoid of 'data',
		//  and # of repeats for a given repeat node is a kind of data. trust me
		private static void  trimRepeatChildren(TreeElement node)
		{
			for (int i = 0; i < node.NumChildren; i++)
			{
				TreeElement child = node.getChildAt(i);
				if (child.Repeatable)
				{
					node.removeChildAt(i);
					i--;
				}
				else
				{
					trimRepeatChildren(child);
				}
			}
		}
		
		private static void  checkDuplicateNodesAreRepeatable(TreeElement node)
		{
			int mult = node.Mult;
			if (mult > 0)
			{
				//repeated node
				if (!node.Repeatable)
				{
					System.Console.Out.WriteLine("Warning: repeated nodes [" + node.Name + "] detected that have no repeat binding in the form; DO NOT bind questions to these nodes or their children!");
					//we could do a more comprehensive safety check in the future
				}
			}
			
			for (int i = 0; i < node.NumChildren; i++)
			{
				checkDuplicateNodesAreRepeatable(node.getChildAt(i));
			}
		}
		
		//check repeat sets for homogeneity
		private void  checkHomogeneity(FormInstance instance)
		{
			
			for (int i = 0; i < refs.size(); i++)
			{
				TreeReference ref_Renamed = refs.elementAt(i);
				TreeElement template = null;
				
				for (int j = 0; j < nodes.size(); j++)
				{
					TreeReference nref = nodes.elementAt(j);
					TreeElement node = instance.resolveReference(nref);
					if (node == null)
					//don't crash on '/'... invalid repeat binding will be caught later
						continue;
					
					if (template == null)
						template = instance.getTemplate(nref);
					
					if (!FormInstance.isHomogeneous(template, node))
					{
						reporter.warning(XFormParserReporter.TYPE_INVALID_STRUCTURE, "Not all repeated nodes for a given repeat binding [" + nref.ToString() + "] are homogeneous! This will cause serious problems!", null);
					}
				}
			}
		}
		
		private void  verifyBindings(FormInstance instance)
		{
			//check <bind>s (can't bind to '/', bound nodes actually exist)
			for (int i = 0; i < bindings.size(); i++)
			{
				DataBinding bind = bindings.elementAt(i);
				TreeReference ref_Renamed = FormInstance.unpackReference(bind.Reference);
				
				if (ref_Renamed.size() == 0)
				{
					System.Console.Out.WriteLine("Cannot bind to '/'; ignoring bind...");
					bindings.removeElementAt(i);
					i--;
				}
				else
				{
					
					if (nodes.size() == 0)
					{
						reporter.warning(XFormParserReporter.TYPE_ERROR_PRONE, "<bind> defined for a node that doesn't exist [" + ref_Renamed.ToString() + "]. The node's name was probably changed and the bind should be updated. ", null);
					}
				}
			}
			
			//check <repeat>s (can't bind to '/' or '/data')
			
			for (int i = 0; i < refs.size(); i++)
			{
				TreeReference ref_Renamed = refs.elementAt(i);
				
				if (ref_Renamed.size() <= 1)
				{
					throw new XFormParseException("Cannot bind repeat to '/' or '/" + mainInstanceNode.getName() + "'");
				}
			}
			
			//check control/group/repeat bindings (bound nodes exist, question can't bind to '/')
			
			List< String > bindErrors = new List< String >();
			verifyControlBindings(_f, instance, bindErrors);
			if (bindErrors.size() > 0)
			{
				System.String errorMsg = "";
				for (int i = 0; i < bindErrors.size(); i++)
				{
					errorMsg += (bindErrors.elementAt(i) + "\n");
				}
				throw new XFormParseException(errorMsg);
			}
			
			//check that repeat members bind to the proper scope (not above the binding of the parent repeat, and not within any sub-repeat (or outside repeat))
			verifyRepeatMemberBindings(_f, instance, null);
			
			//check that label/copy/value refs are children of nodeset ref, and exist
			verifyItemsetBindings(instance);
			
			verifyItemsetSrcDstCompatibility(instance);
		}
		
		private void  verifyActions(FormInstance instance)
		{
			//check the target of actions which are manipulating real values
			for (int i = 0; i < actionTargets.size(); i++)
			{
				TreeReference target = actionTargets.elementAt(i);
				
				if (nodes.size() == 0)
				{
					throw new XFormParseException("Invalid Action - Targets non-existent node: " + target.toString(true));
				}
			}
		}
		
		
		private
		
		void verifyControlBindings(IFormElement fe, FormInstance instance, List< String > errors)
		
		private void  verifyRepeatMemberBindings(IFormElement fe, FormInstance instance, GroupDef parentRepeat)
		{
			if (fe.getChildren() == null)
				return ;
			
			for (int i = 0; i < fe.getChildren().size(); i++)
			{
				IFormElement child = fe.getChildren().elementAt(i);
				bool isRepeat = (child is GroupDef && ((GroupDef) child).Repeat);
				
				//get bindings of current node and nearest enclosing repeat
				TreeReference repeatBind = (parentRepeat == null?TreeReference.rootRef():FormInstance.unpackReference(parentRepeat.Bind));
				TreeReference childBind = FormInstance.unpackReference(child.getBind());
				
				//check if current binding is within scope of repeat binding
				if (!repeatBind.isParentOf(childBind, false))
				{
					//catch <repeat nodeset="/a/b"><input ref="/a/c" /></repeat>: repeat question is not a child of the repeated node
					throw new XFormParseException("<repeat> member's binding [" + childBind.ToString() + "] is not a descendant of <repeat> binding [" + repeatBind.ToString() + "]!");
				}
				else if (repeatBind.Equals(childBind) && isRepeat)
				{
					//catch <repeat nodeset="/a/b"><repeat nodeset="/a/b">...</repeat></repeat> (<repeat nodeset="/a/b"><input ref="/a/b" /></repeat> is ok)
					throw new XFormParseException("child <repeat>s [" + childBind.ToString() + "] cannot bind to the same node as their parent <repeat>; only questions/groups can");
				}
				
				//check that, in the instance, current node is not within the scope of any closer repeat binding
				//build a list of all the node's instance ancestors
				
				List< TreeElement > repeatAncestry = new List< TreeElement >();
				TreeElement repeatNode = (repeatTree == null?null:repeatTree.getRoot());
				if (repeatNode != null)
				{
					repeatAncestry.addElement(repeatNode);
					for (int j = 1; j < childBind.size(); j++)
					{
						repeatNode = repeatNode.getChild(childBind.getName(j), 0);
						if (repeatNode != null)
						{
							repeatAncestry.addElement(repeatNode);
						}
						else
						{
							break;
						}
					}
				}
				//check that no nodes between the parent repeat and the target are repeatable
				for (int k = repeatBind.size(); k < childBind.size(); k++)
				{
					TreeElement rChild = (k < repeatAncestry.size()?repeatAncestry.elementAt(k):null);
					bool repeatable = (rChild == null?false:rChild.Repeatable);
					if (repeatable && !(k == childBind.size() - 1 && isRepeat))
					{
						//catch <repeat nodeset="/a/b"><input ref="/a/b/c/d" /></repeat>...<repeat nodeset="/a/b/c">...</repeat>:
						//  question's/group's/repeat's most immediate repeat parent in the instance is not its most immediate repeat parent in the form def
						throw new XFormParseException("<repeat> member's binding [" + childBind.ToString() + "] is within the scope of a <repeat> that is not its closest containing <repeat>!");
					}
				}
				
				verifyRepeatMemberBindings(child, instance, (isRepeat?(GroupDef) child:parentRepeat));
			}
		}
		
		private void  verifyItemsetBindings(FormInstance instance)
		{
			for (int i = 0; i < itemsets.size(); i++)
			{
				ItemsetBinding itemset = itemsets.elementAt(i);
				
				//check proper parent/child relationship
				if (!itemset.nodesetRef.isParentOf(itemset.labelRef, false))
				{
					throw new XFormParseException("itemset nodeset ref is not a parent of label ref");
				}
				else if (itemset.copyRef != null && !itemset.nodesetRef.isParentOf(itemset.copyRef, false))
				{
					throw new XFormParseException("itemset nodeset ref is not a parent of copy ref");
				}
				else if (itemset.valueRef != null && !itemset.nodesetRef.isParentOf(itemset.valueRef, false))
				{
					throw new XFormParseException("itemset nodeset ref is not a parent of value ref");
				}
				
				//make sure the labelref is tested against the right instance
				//check if it's not the main instance
				FormInstance fi = null;
				if (itemset.labelRef.InstanceName != null)
				{
					fi = _f.getNonMainInstance(itemset.labelRef.InstanceName);
					if (fi == null)
					{
						throw new XFormParseException("Instance: " + itemset.labelRef.InstanceName + " Does not exists");
					}
				}
				else
				{
					fi = instance;
				}
				
				
				if (fi.getTemplatePath(itemset.labelRef) == null)
				{
					throw new XFormParseException("<label> node for itemset doesn't exist! [" + itemset.labelRef + "]");
				}
				/****  NOT SURE WHAT A COPYREF DOES OR IS, SO I'M NOT CHECKING FOR IT
				else if (itemset.copyRef != null && instance.getTemplatePath(itemset.copyRef) == null) {
				throw new XFormParseException("<copy> node for itemset doesn't exist! [" + itemset.copyRef + "]");
				}
				****/
				//check value nodes exist
				else if (itemset.valueRef != null && fi.getTemplatePath(itemset.valueRef) == null)
				{
					throw new XFormParseException("<value> node for itemset doesn't exist! [" + itemset.valueRef + "]");
				}
			}
		}
		
		private void  verifyItemsetSrcDstCompatibility(FormInstance instance)
		{
			for (int i = 0; i < itemsets.size(); i++)
			{
				ItemsetBinding itemset = itemsets.elementAt(i);
				
				bool destRepeatable = (instance.getTemplate(itemset.getDestRef()) != null);
				if (itemset.copyMode)
				{
					if (!destRepeatable)
					{
						throw new XFormParseException("itemset copies to node(s) which are not repeatable");
					}
					
					//validate homogeneity between src and dst nodes
					TreeElement srcNode = instance.getTemplatePath(itemset.copyRef);
					TreeElement dstNode = instance.getTemplate(itemset.getDestRef());
					
					if (!FormInstance.isHomogeneous(srcNode, dstNode))
					{
						reporter.warning(XFormParserReporter.TYPE_INVALID_STRUCTURE, "Your itemset source [" + srcNode.Ref.ToString() + "] and dest [" + dstNode.Ref.ToString() + "] of appear to be incompatible!", null);
					}
					
					//TODO: i feel like, in theory, i should additionally check that the repeatable children of src and dst
					//match up (Achild is repeatable <--> Bchild is repeatable). isHomogeneous doesn't check this. but i'm
					//hard-pressed to think of scenarios where this would actually cause problems
				}
				else
				{
					if (destRepeatable)
					{
						throw new XFormParseException("itemset sets value on repeatable nodes");
					}
				}
			}
		}
		
		private void  applyInstanceProperties(FormInstance instance)
		{
			for (int i = 0; i < bindings.size(); i++)
			{
				DataBinding bind = bindings.elementAt(i);
				TreeReference ref_Renamed = FormInstance.unpackReference(bind.Reference);
				
				
				if (nodes.size() > 0)
				{
					attachBindGeneral(bind);
				}
				for (int j = 0; j < nodes.size(); j++)
				{
					TreeReference nref = nodes.elementAt(j);
					attachBind(instance.resolveReference(nref), bind);
				}
			}
			
			applyControlProperties(instance);
		}
		
		private static void  attachBindGeneral(DataBinding bind)
		{
			TreeReference ref_Renamed = FormInstance.unpackReference(bind.Reference);
			
			if (bind.relevancyCondition != null)
			{
				bind.relevancyCondition.addTarget(ref_Renamed);
			}
			if (bind.requiredCondition != null)
			{
				bind.requiredCondition.addTarget(ref_Renamed);
			}
			if (bind.readonlyCondition != null)
			{
				bind.readonlyCondition.addTarget(ref_Renamed);
			}
			if (bind.calculate != null)
			{
				bind.calculate.addTarget(ref_Renamed);
			}
		}
		
		private static void  attachBind(TreeElement node, DataBinding bind)
		{
			node.DataType = bind.DataType;
			
			if (bind.relevancyCondition == null)
			{
				node.setRelevant(bind.relevantAbsolute);
			}
			if (bind.requiredCondition == null)
			{
				node.Required = bind.requiredAbsolute;
			}
			if (bind.readonlyCondition == null)
			{
				node.setEnabled(!bind.readonlyAbsolute);
			}
			if (bind.constraint != null)
			{
				node.Constraint = new Constraint(bind.constraint, bind.constraintMessage);
			}
			
			node.PreloadHandler = bind.Preload;
			node.PreloadParams = bind.PreloadParams;
			node.setBindAttributes(bind.getAdditionalAttributes());
		}
		
		//apply properties to instance nodes that are determined by controls bound to those nodes
		//this should make you feel slightly dirty, but it allows us to be somewhat forgiving with the form
		//(e.g., a select question bound to a 'text' type node)
		private void  applyControlProperties(FormInstance instance)
		{
			for (int h = 0; h < 2; h++)
			{
				
				int type = (h == 0?Constants.DATATYPE_CHOICE:Constants.DATATYPE_CHOICE_LIST);
				
				for (int i = 0; i < selectRefs.size(); i++)
				{
					TreeReference ref_Renamed = selectRefs.elementAt(i);
					
					for (int j = 0; j < nodes.size(); j++)
					{
						TreeElement node = instance.resolveReference(nodes.elementAt(j));
						if (node.DataType == Constants.DATATYPE_CHOICE || node.DataType == Constants.DATATYPE_CHOICE_LIST)
						{
							//do nothing
						}
						else if (node.DataType == Constants.DATATYPE_NULL || node.DataType == Constants.DATATYPE_TEXT)
						{
							node.DataType = type;
						}
						else
						{
							reporter.warning(XFormParserReporter.TYPE_INVALID_STRUCTURE, "Select question " + ref_Renamed.ToString() + " appears to have data type that is incompatible with selection", null);
						}
					}
				}
			}
		}
		
		//TODO: hook here for turning sub-trees into complex IAnswerData objects (like for immunizations)
		//FIXME: the 'ref' and FormDef parameters (along with the helper function above that initializes them) are only needed so that we
		//can fetch QuestionDefs bound to the given node, as the QuestionDef reference is needed to properly represent answers
		//to select questions. obviously, we want to fix this.
		private static void  loadInstanceData(Element node, TreeElement cur, FormDef f)
		{
			int numChildren = node.getChildCount();
			bool hasElements = false;
			for (int i = 0; i < numChildren; i++)
			{
				if (node.getType(i) == Node.ELEMENT)
				{
					hasElements = true;
					break;
				}
			}
			
			if (hasElements)
			{
				
				HashMap < String, Integer > multiplicities = new HashMap < String, Integer >(); //stores max multiplicity seen for a given node name thus far
				for (int i = 0; i < numChildren; i++)
				{
					if (node.getType(i) == Node.ELEMENT)
					{
						Element child = node.getElement(i);
						
						System.String name = child.getName();
						int index;
						bool isTemplate = (child.getAttributeValue(NAMESPACE_JAVAROSA, "template") != null);
						
						if (isTemplate)
						{
							index = TreeReference.INDEX_TEMPLATE;
						}
						else
						{
							//update multiplicity counter
							System.Int32 mult = multiplicities.get_Renamed(name);
							//UPGRADE_TODO: The 'System.Int32' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
							index = (mult == null?0:mult + 1);
							multiplicities.put(name, Integer.valueOf(index));
						}
						
						loadInstanceData(child, cur.getChild(name, index), f);
					}
				}
			}
			else
			{
				System.String text = getXMLText(node, true);
				if (text != null && text.Trim().Length > 0)
				{
					//ignore text that is only whitespace
					//TODO: custom data types? modelPrototypes?
					cur.Value = XFormAnswerDataParser.getAnswerData(text, cur.DataType, ghettoGetQuestionDef(cur.DataType, f, cur.Ref));
				}
			}
		}
		
		//find a questiondef that binds to ref, if the data type is a 'select' question type
		public static QuestionDef ghettoGetQuestionDef(int dataType, FormDef f, TreeReference ref_Renamed)
		{
			if (dataType == Constants.DATATYPE_CHOICE || dataType == Constants.DATATYPE_CHOICE_LIST)
			{
				return FormDef.findQuestionByRef(ref_Renamed, f);
			}
			else
			{
				return null;
			}
		}
		
		private void  checkDependencyCycles()
		{
			
			List< TreeReference > vertices = new List< TreeReference >();
			
			List< TreeReference [] > edges = new List< TreeReference [] >();
			
			//build graph
			
			for(TreeReference trigger: _f.triggerIndex.keySet())
			{
				if (!vertices.contains(trigger))
					vertices.addElement(trigger);
				
				
				
				List< TreeReference > targets = new List< TreeReference >();
				for (int i = 0; i < triggered.size(); i++)
				{
					Triggerable t = (Triggerable) triggered.elementAt(i);
					for (int j = 0; j < t.getTargets().size(); j++)
					{
						TreeReference target = t.getTargets().elementAt(j);
						if (!targets.contains(target))
							targets.addElement(target);
					}
				}
				
				for (int i = 0; i < targets.size(); i++)
				{
					TreeReference target = targets.elementAt(i);
					if (!vertices.contains(target))
						vertices.addElement(target);
					
					TreeReference[] edge = new TreeReference[]{trigger, target};
					edges.addElement(edge);
				}
			}
			
			//find cycles
			bool acyclic = true;
			while (vertices.size() > 0)
			{
				//determine leaf nodes
				System.Collections.ArrayList leaves = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				for (int i = 0; i < vertices.size(); i++)
				{
					leaves.Add(vertices.elementAt(i));
				}
				for (int i = 0; i < edges.size(); i++)
				{
					TreeReference[] edge = (TreeReference[]) edges.elementAt(i);
					leaves.Remove(edge[0]);
				}
				
				//if no leaf nodes while graph still has nodes, graph has cycles
				if (leaves.Count == 0)
				{
					acyclic = false;
					break;
				}
				
				//remove leaf nodes and edges pointing to them
				for (int i = 0; i < leaves.Count; i++)
				{
					TreeReference leaf = (TreeReference) leaves[i];
					vertices.removeElement(leaf);
				}
				for (int i = edges.size() - 1; i >= 0; i--)
				{
					TreeReference[] edge = (TreeReference[]) edges.elementAt(i);
					if (leaves.Contains(edge[1]))
						edges.removeElementAt(i);
				}
			}
			
			if (!acyclic)
			{
				StringBuilder b = new StringBuilder();
				b.append("XPath Dependency Cycle:\n");
				for (int i = 0; i < edges.size(); i++)
				{
					TreeReference[] edge = (TreeReference[]) edges.elementAt(i);
					b.append(edge[0].ToString()).append(" => ").append(edge[1].ToString()).append("\n");
				}
				reporter.error(b.toString());
				
				throw new System.SystemException("Dependency cycles amongst the xpath expressions in relevant/calculate");
			}
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public virtual void  loadXmlInstance(FormDef f, System.IO.StreamReader xmlReader)
		{
			loadXmlInstance(f, getXMLDocument(xmlReader));
		}
		
		/// <summary> Load a compatible xml instance into FormDef f
		/// 
		/// call before f.initialize()!
		/// </summary>
		public static void  loadXmlInstance(FormDef f, Document xmlInst)
		{
			TreeElement savedRoot = XFormParser.restoreDataModel(xmlInst, null).getRoot();
			TreeElement templateRoot = f.MainInstance.getRoot().deepCopy(true);
			
			// weak check for matching forms
			// TODO: should check that namespaces match?
			if (!savedRoot.Name.Equals(templateRoot.Name) || savedRoot.Mult != 0)
			{
				throw new System.SystemException("Saved form instance does not match template form definition");
			}
			
			// populate the data model
			TreeReference tr = TreeReference.rootRef();
			tr.add(templateRoot.Name, TreeReference.INDEX_UNBOUND);
			templateRoot.populate(savedRoot, f);
			
			// populated model to current form
			f.MainInstance.setRoot(templateRoot);
			
			// if the new instance is inserted into the formdef before f.initialize() is called, this
			// locale refresh is unnecessary
			//   Localizer loc = f.getLocalizer();
			//   if (loc != null) {
			//       f.localeChanged(loc.getLocale(), loc);
			//	 }
		}
		
		//returns data type corresponding to type string; doesn't handle defaulting to 'text' if type unrecognized/unknown
		private int getDataType(System.String type)
		{
			int dataType = Constants.DATATYPE_NULL;
			
			if (type != null)
			{
				//cheap out and ignore namespace
				if (type.IndexOf(":") != - 1)
				{
					type = type.Substring(type.IndexOf(":") + 1);
				}
				
				if (typeMappings.containsKey(type))
				{
					dataType = ((System.Int32) typeMappings.get_Renamed(type));
				}
				else
				{
					dataType = Constants.DATATYPE_UNSUPPORTED;
					reporter.warning(XFormParserReporter.TYPE_ERROR_PRONE, "unrecognized data type [" + type + "]", null);
				}
			}
			
			return dataType;
		}
		
		public static void  addModelPrototype(int type, TreeElement element)
		{
			modelPrototypes.addNewPrototype(System.Convert.ToString(type), element.GetType());
		}
		
		public static void  addDataType(System.String type, int dataType)
		{
			typeMappings.put(type, Integer.valueOf(dataType));
		}
		
		public static void  registerControlType(System.String type, int typeId)
		{
			IElementHandler newHandler = new AnonymousClassIElementHandler13(typeId);
			topLevelHandlers.put(type, newHandler);
			groupLevelHandlers.put(type, newHandler);
		}
		
		public static void  registerHandler(System.String type, IElementHandler handler)
		{
			topLevelHandlers.put(type, handler);
			groupLevelHandlers.put(type, handler);
		}
		
		public static System.String getXMLText(Node n, bool trim)
		{
			return (n.getChildCount() == 0?null:getXMLText(n, 0, trim));
		}
		
		/// <summary> reads all subsequent text nodes and returns the combined string
		/// needed because escape sequences are parsed into consecutive text nodes
		/// e.g. "abc&amp;123" --> (abc)(&)(123)
		/// 
		/// </summary>
		public static System.String getXMLText(Node node, int i, bool trim)
		{
			StringBuilder strBuff = null;
			
			System.String text = node.getText(i);
			if (text == null)
				return null;
			
			for (i++; i < node.getChildCount() && node.getType(i) == Node.TEXT; i++)
			{
				if (strBuff == null)
					strBuff = new StringBuilder(text);
				
				strBuff.append(node.getText(i));
			}
			if (strBuff != null)
				text = strBuff.toString();
			
			if (trim)
				text = text.Trim();
			
			return text;
		}
		
		public static FormInstance restoreDataModel(System.IO.Stream input, System.Type restorableType)
		{
			Document doc = getXMLDocument(new System.IO.StreamReader(input, System.Text.Encoding.Default));
			if (doc == null)
			{
				throw new System.SystemException("syntax error in XML instance; could not parse");
			}
			return restoreDataModel(doc, restorableType);
		}
		
		public static FormInstance restoreDataModel(Document doc, System.Type restorableType)
		{
			Restorable r = (restorableType != null?(Restorable) PrototypeFactory.getInstance(restorableType):null);
			
			Element e = doc.getRootElement();
			
			TreeElement te = buildInstanceStructure(e, null);
			FormInstance dm = new FormInstance(te);
			loadNamespaces(e, dm);
			if (r != null)
			{
				RestoreUtils.templateData(r, dm, null);
			}
			loadInstanceData(e, te, null);
			
			return dm;
		}
		
		public static FormInstance restoreDataModel(sbyte[] data, System.Type restorableType)
		{
			try
			{
				return restoreDataModel(new System.IO.MemoryStream(SupportClass.ToByteArray(data)), restorableType);
			}
			catch (System.IO.IOException e)
			{
				if (e is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
					((org.javarosa.core.io.StreamsUtil.DirectionalIOException) e).printStackTrace();
				else
					SupportClass.WriteStackTrace(e, Console.Error);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new XFormParseException("Bad parsing from byte array " + e.Message);
			}
		}
		
		public static System.String getVagueLocation(Element e)
		{
			System.String path = e.getName();
			Element walker = e;
			while (walker != null)
			{
				Node n = walker.getParent();
				if (n is Element)
				{
					walker = (Element) n;
					System.String step = walker.getName();
					for (int i = 0; i < walker.getAttributeCount(); ++i)
					{
						step += ("[@" + walker.getAttributeName(i) + "=");
						step += (walker.getAttributeValue(i) + "]");
					}
					path = step + "/" + path;
				}
				else
				{
					walker = null;
					path = "/" + path;
				}
			}
			
			System.String elementString = getVagueElementPrintout(e, 2);
			
			System.String fullmsg = "\n    Problem found at nodeset: " + path;
			fullmsg += ("\n    With element " + elementString + "\n");
			return fullmsg;
		}
		
		public static System.String getVagueElementPrintout(Element e, int maxDepth)
		{
			System.String elementString = "<" + e.getName();
			for (int i = 0; i < e.getAttributeCount(); ++i)
			{
				elementString += (" " + e.getAttributeName(i) + "=\"");
				elementString += (e.getAttributeValue(i) + "\"");
			}
			if (e.getChildCount() > 0)
			{
				elementString += ">";
				if (e.getType(0) == Element.ELEMENT)
				{
					if (maxDepth > 0)
					{
						elementString += getVagueElementPrintout((Element) e.getChild(0), maxDepth - 1);
					}
					else
					{
						elementString += "...";
					}
				}
			}
			else
			{
				elementString += "/>";
			}
			return elementString;
		}
		
		
		public
		
		void setStringCache(CacheTable < String > stringCache)
		static XFormParser()
		{
			{
				try
				{
					staticInit();
				}
				catch (System.Exception e)
				{
					Logger.die("xfparser-static-init", e);
				}
			}
		}
	}
}