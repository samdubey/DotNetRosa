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
using IDataPointer = org.javarosa.core.data.IDataPointer;
using FormDef = org.javarosa.core.model.FormDef;
using IAnswerDataSerializer = org.javarosa.core.model.IAnswerDataSerializer;
using IDataReference = org.javarosa.core.model.IDataReference;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using IInstanceSerializingVisitor = org.javarosa.core.model.utils.IInstanceSerializingVisitor;
using ByteArrayPayload = org.javarosa.core.services.transport.payload.ByteArrayPayload;
using DataPointerPayload = org.javarosa.core.services.transport.payload.DataPointerPayload;
using IDataPayload = org.javarosa.core.services.transport.payload.IDataPayload;
using MultiMessagePayload = org.javarosa.core.services.transport.payload.MultiMessagePayload;
using XFormAnswerDataSerializer = org.javarosa.xform.util.XFormAnswerDataSerializer;
using XFormSerializer = org.javarosa.xform.util.XFormSerializer;
//UPGRADE_TODO: The type 'org.kxml2.kdom.Document' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Document = org.kxml2.kdom.Document;
//UPGRADE_TODO: The type 'org.kxml2.kdom.Element' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Element = org.kxml2.kdom.Element;
//UPGRADE_TODO: The type 'org.kxml2.kdom.Node' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Node = org.kxml2.kdom.Node;
namespace org.javarosa.model.xform
{
	
	/// <summary> A visitor-esque class which walks a FormInstance and constructs an XML document
	/// containing its instance.
	/// 
	/// The XML node elements are constructed in a depth-first manner, consistent with
	/// standard XML document parsing.
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public class XFormSerializingVisitor : IInstanceSerializingVisitor
	{
		virtual public IAnswerDataSerializer AnswerDataSerializer
		{
			/*
			* (non-Javadoc)
			* @see org.javarosa.core.model.utils.IInstanceSerializingVisitor#setAnswerDataSerializer(org.javarosa.core.model.IAnswerDataSerializer)
			*/
			
			set
			{
				this.serializer = value;
			}
			
		}
		
		/// <summary>The XML document containing the instance that is to be returned </summary>
		internal Document theXmlDoc;
		
		/// <summary>The serializer to be used in constructing XML for AnswerData elements </summary>
		internal IAnswerDataSerializer serializer;
		
		/// <summary>The root of the xml document which should be included in the serialization *</summary>
		internal TreeReference rootRef;
		
		/// <summary>The schema to be used to serialize answer data </summary>
		internal FormDef schema; //not used
		
		internal System.Collections.ArrayList dataPointers;
		
		internal bool respectRelevance = true;
		
		public XFormSerializingVisitor():this(true)
		{
		}
		public XFormSerializingVisitor(bool respectRelevance)
		{
			this.respectRelevance = respectRelevance;
		}
		
		private void  init()
		{
			theXmlDoc = null;
			schema = null;
			dataPointers = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
		}
		
		public virtual sbyte[] serializeInstance(FormInstance model, FormDef formDef)
		{
			
			//LEGACY: Should remove
			init();
			this.schema = formDef;
			return serializeInstance(model);
		}
		
		public virtual sbyte[] serializeInstance(FormInstance model)
		{
			return serializeInstance(model, new XPathReference("/"));
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.model.utils.IInstanceSerializingVisitor#serializeDataModel(org.javarosa.core.model.IFormDataModel)
		*/
		public virtual sbyte[] serializeInstance(FormInstance model, IDataReference ref_Renamed)
		{
			init();
			rootRef = org.javarosa.core.model.instance.FormInstance.unpackReference(ref_Renamed);
			if (this.serializer == null)
			{
				this.AnswerDataSerializer = new XFormAnswerDataSerializer();
			}
			
			model.accept(this);
			if (theXmlDoc != null)
			{
				return XFormSerializer.getUtfBytes(theXmlDoc);
			}
			else
			{
				return null;
			}
		}
		
		public virtual IDataPayload createSerializedPayload(FormInstance model)
		{
			return createSerializedPayload(model, new XPathReference("/"));
		}
		
		public virtual IDataPayload createSerializedPayload(FormInstance model, IDataReference ref_Renamed)
		{
			init();
			rootRef = org.javarosa.core.model.instance.FormInstance.unpackReference(ref_Renamed);
			if (this.serializer == null)
			{
				this.AnswerDataSerializer = new XFormAnswerDataSerializer();
			}
			model.accept(this);
			if (theXmlDoc != null)
			{
				//TODO: Did this strip necessary data?
				sbyte[] form = XFormSerializer.getUtfBytes(theXmlDoc);
				if (dataPointers.Count == 0)
				{
					return new ByteArrayPayload(form, null, org.javarosa.core.services.transport.payload.IDataPayload_Fields.PAYLOAD_TYPE_XML);
				}
				MultiMessagePayload payload = new MultiMessagePayload();
				payload.addPayload(new ByteArrayPayload(form, "xml_submission_file", org.javarosa.core.services.transport.payload.IDataPayload_Fields.PAYLOAD_TYPE_XML));
				System.Collections.IEnumerator en = dataPointers.GetEnumerator();
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				while (en.MoveNext())
				{
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					IDataPointer pointer = (IDataPointer) en.Current;
					payload.addPayload(new DataPointerPayload(pointer));
				}
				return payload;
			}
			else
			{
				return null;
			}
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.model.utils.ITreeVisitor#visit(org.javarosa.core.model.DataModelTree)
		*/
		public virtual void  visit(FormInstance tree)
		{
			theXmlDoc = new Document();
			//TreeElement root = tree.getRoot();
			
			TreeElement root = tree.resolveReference(rootRef);
			
			//For some reason resolveReference won't ever return the root, so we'll
			//catch that case and just start at the root.
			if (root == null)
			{
				root = tree.getRoot();
			}
			
			for (int i = 0; i < root.NumChildren; i++)
			{
				TreeElement childAt = root.getChildAt(i);
			}
			
			if (root != null)
			{
				theXmlDoc.addChild(Node.ELEMENT, serializeNode(root));
			}
			
			Element top = theXmlDoc.getElement(0);
			
			System.String[] prefixes = tree.NamespacePrefixes;
			for (int i = 0; i < prefixes.Length; ++i)
			{
				top.setPrefix(prefixes[i], tree.getNamespaceURI(prefixes[i]));
			}
			if (tree.schema != null)
			{
				top.setNamespace(tree.schema);
				top.setPrefix("", tree.schema);
			}
		}
		
		public virtual Element serializeNode(TreeElement instanceNode)
		{
			Element e = new Element(); //don't set anything on this element yet, as it might get overwritten
			
			//don't serialize template nodes or non-relevant nodes
			if ((respectRelevance && !instanceNode.isRelevant()) || instanceNode.Mult == TreeReference.INDEX_TEMPLATE)
			{
				return null;
			}
			
			if (instanceNode.Value != null)
			{
				System.Object serializedAnswer = serializer.serializeAnswerData(instanceNode.Value, instanceNode.DataType);
				
				if (serializedAnswer is Element)
				{
					e = (Element) serializedAnswer;
				}
				else if (serializedAnswer is System.String)
				{
					e = new Element();
					e.addChild(Node.TEXT, (System.String) serializedAnswer);
				}
				else
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					throw new System.SystemException("Can't handle serialized output for" + instanceNode.Value.ToString() + ", " + serializedAnswer);
				}
				
				if (serializer.containsExternalData(instanceNode.Value))
				{
					IDataPointer[] pointer = serializer.retrieveExternalDataPointer(instanceNode.Value);
					for (int i = 0; i < pointer.Length; ++i)
					{
						dataPointers.Add(pointer[i]);
					}
				}
			}
			else
			{
				//make sure all children of the same tag name are written en bloc
				System.Collections.ArrayList childNames = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				for (int i = 0; i < instanceNode.NumChildren; i++)
				{
					System.String childName = instanceNode.getChildAt(i).Name;
					if (!childNames.Contains(childName))
						childNames.Add(childName);
				}
				
				for (int i = 0; i < childNames.Count; i++)
				{
					System.String childName = (System.String) childNames[i];
					int mult = instanceNode.getChildMultiplicity(childName);
					for (int j = 0; j < mult; j++)
					{
						Element child = serializeNode(instanceNode.getChild(childName, j));
						if (child != null)
						{
							e.addChild(Node.ELEMENT, child);
						}
					}
				}
			}
			
			e.setName(instanceNode.Name);
			
			// add hard-coded attributes
			for (int i = 0; i < instanceNode.AttributeCount; i++)
			{
				System.String namespace_Renamed = instanceNode.getAttributeNamespace(i);
				System.String name = instanceNode.getAttributeName(i);
				System.String val = instanceNode.getAttributeValue(i);
				// is it legal for getAttributeValue() to return null? playing it safe for now and assuming yes
				if (val == null)
				{
					val = "";
				}
				e.setAttribute(namespace_Renamed, name, val);
			}
			if (instanceNode.Namespace != null)
			{
				e.setNamespace(instanceNode.Namespace);
			}
			
			return e;
		}
		
		public virtual IInstanceSerializingVisitor newInstance()
		{
			XFormSerializingVisitor modelSerializer = new XFormSerializingVisitor();
			modelSerializer.AnswerDataSerializer = this.serializer;
			return modelSerializer;
		}
	}
}