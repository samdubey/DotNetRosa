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
//UPGRADE_TODO: The type 'org.kxml2.kdom.Element' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Element = org.kxml2.kdom.Element;
//UPGRADE_TODO: The type 'org.kxml2.kdom.Node' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Node = org.kxml2.kdom.Node;
namespace org.javarosa.model.xform
{
	
	/// <summary> A modified version of Clayton's XFormSerializingVisitor that constructs
	/// SMS's.
	/// 
	/// </summary>
	/// <author>  Munaf Sheikh, Cell-Life
	/// 
	/// </author>
	public class SMSSerializingVisitor : IInstanceSerializingVisitor
	{
		virtual public IAnswerDataSerializer AnswerDataSerializer
		{
			/*
			* (non-Javadoc)
			*
			* @seeorg.javarosa.core.model.utils.IInstanceSerializingVisitor#
			* setAnswerDataSerializer(org.javarosa.core.model.IAnswerDataSerializer)
			*/
			
			set
			{
				this.serializer = value;
			}
			
		}
		
		private System.String theSmsStr = null; // sms string to be returned
		private System.String nodeSet = null; // which nodeset the sms contents are in
		private System.String xmlns = null;
		private System.String delimiter = null;
		private System.String prefix = null;
		private System.String method = null;
		private TreeReference rootRef;
		
		/// <summary>The serializer to be used in constructing XML for AnswerData elements </summary>
		internal IAnswerDataSerializer serializer;
		
		/// <summary>The schema to be used to serialize answer data </summary>
		internal FormDef schema; // not used
		
		internal System.Collections.ArrayList dataPointers;
		
		private void  init()
		{
			theSmsStr = null;
			schema = null;
			dataPointers = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			theSmsStr = "";
		}
		
		public virtual sbyte[] serializeInstance(FormInstance model, FormDef formDef)
		{
			init();
			this.schema = formDef;
			return serializeInstance(model);
		}
		
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.model.utils.IInstanceSerializingVisitor#serializeInstance(org.javarosa.core.model.instance.FormInstance)
		*/
		public virtual sbyte[] serializeInstance(FormInstance model)
		{
			return this.serializeInstance(model, new XPathReference("/"));
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.model.utils.IInstanceSerializingVisitor#serializeInstance(org.javarosa.core.model.instance.FormInstance, org.javarosa.core.model.IDataReference)
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
			if (theSmsStr != null)
			{
				//Encode in UTF-16 by default, since it's the default for complex messages
				//UPGRADE_TODO: Method 'java.lang.String.getBytes' was converted to 'System.Text.Encoding.GetEncoding(string).GetBytes(string)' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangStringgetBytes_javalangString'"
				return SupportClass.ToSByteArray(System.Text.Encoding.GetEncoding("UTF-16BE").GetBytes(theSmsStr));
			}
			else
			{
				return null;
			}
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.model.utils.IInstanceSerializingVisitor#createSerializedPayload(org.javarosa.core.model.instance.FormInstance)
		*/
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
			if (theSmsStr != null)
			{
				//UPGRADE_TODO: Method 'java.lang.String.getBytes' was converted to 'System.Text.Encoding.GetEncoding(string).GetBytes(string)' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangStringgetBytes_javalangString'"
				sbyte[] form = SupportClass.ToSByteArray(System.Text.Encoding.GetEncoding("UTF-16").GetBytes(theSmsStr));
				return new ByteArrayPayload(form, null, org.javarosa.core.services.transport.payload.IDataPayload_Fields.PAYLOAD_TYPE_SMS);
			}
			else
			{
				return null;
			}
		}
		
		/*
		* (non-Javadoc)
		*
		* @see
		* org.javarosa.core.model.utils.ITreeVisitor#visit(org.javarosa.core.model
		* .DataModelTree)
		*/
		public virtual void  visit(FormInstance tree)
		{
			nodeSet = new System.Text.StringBuilder().ToString();
			
			//TreeElement root = tree.getRoot();
			TreeElement root = tree.resolveReference(rootRef);
			
			xmlns = root.getAttributeValue("", "xmlns");
			delimiter = root.getAttributeValue("", "delimiter");
			if (delimiter == null)
			{
				// for the spelling-impaired...
				delimiter = root.getAttributeValue("", "delimeter");
			}
			prefix = root.getAttributeValue("", "prefix");
			
			xmlns = (xmlns != null)?xmlns:" ";
			delimiter = (delimiter != null)?delimiter:" ";
			prefix = (prefix != null)?prefix:" ";
			
			//Don't bother adding any delimiters, yet. Delimiters are
			//added before tags/data
			theSmsStr = prefix;
			
			// serialize each node to get it's answers
			for (int j = 0; j < root.NumChildren; j++)
			{
				TreeElement tee = root.getChildAt(j);
				System.String e = serializeNode(tee);
				if (e != null)
				{
					theSmsStr += e;
				}
			}
			theSmsStr = theSmsStr.Trim();
		}
		
		public virtual System.String serializeNode(TreeElement instanceNode)
		{
			StringBuilder b = new StringBuilder();
			// don't serialize template nodes or non-relevant nodes
			if (!instanceNode.isRelevant() || instanceNode.Mult == TreeReference.INDEX_TEMPLATE)
				return null;
			
			if (instanceNode.Value != null)
			{
				System.Object serializedAnswer = serializer.serializeAnswerData(instanceNode.Value, instanceNode.DataType);
				
				if (serializedAnswer is Element)
				{
					// DON"T handle this.
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					throw new System.SystemException("Can't handle serialized output for" + instanceNode.Value.ToString() + ", " + serializedAnswer);
				}
				else if (serializedAnswer is System.String)
				{
					Element e = new Element();
					e.addChild(Node.TEXT, (System.String) serializedAnswer);
					
					System.String tag = instanceNode.getAttributeValue("", "tag");
					if (tag != null)
					{
						b.append(tag);
					}
					b.append(delimiter);
					
					for (int k = 0; k < e.getChildCount(); k++)
					{
						b.append(e.getChild(k).toString());
						b.append(delimiter);
					}
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
			return b.toString();
		}
		
		public virtual IInstanceSerializingVisitor newInstance()
		{
			XFormSerializingVisitor modelSerializer = new XFormSerializingVisitor();
			modelSerializer.AnswerDataSerializer = this.serializer;
			return modelSerializer;
		}
	}
}