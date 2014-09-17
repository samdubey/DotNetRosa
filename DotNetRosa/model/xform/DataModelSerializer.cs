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
//UPGRADE_TODO: The type 'org.javarosa.core.model.instance.AbstractTreeElement' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AbstractTreeElement = org.javarosa.core.model.instance.AbstractTreeElement;
using DataInstance = org.javarosa.core.model.instance.DataInstance;
using InstanceInitializationFactory = org.javarosa.core.model.instance.InstanceInitializationFactory;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
//UPGRADE_TODO: The type 'org.kxml2.io.KXmlParser' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using KXmlParser = org.kxml2.io.KXmlParser;
//UPGRADE_TODO: The type 'org.kxml2.io.KXmlSerializer' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using KXmlSerializer = org.kxml2.io.KXmlSerializer;
namespace org.javarosa.model.xform
{
	
	/// <summary> A quick rewrite of the basics for writing higher level xml documents straight to
	/// output streams.
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public class DataModelSerializer
	{
		
		internal KXmlSerializer serializer;
		internal InstanceInitializationFactory factory;
		
		public DataModelSerializer(System.IO.Stream stream):this(stream, new InstanceInitializationFactory())
		{
		}
		
		public DataModelSerializer(System.IO.Stream stream, InstanceInitializationFactory factory)
		{
			serializer = new KXmlSerializer();
			serializer.setOutput(stream, "UTF-8");
			this.factory = factory;
		}
		
		public DataModelSerializer(KXmlSerializer serializer)
		{
			this.serializer = serializer;
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.model.utils.ITreeVisitor#visit(org.javarosa.core.model.DataModelTree)
		*/
		public virtual void  serialize(DataInstance instance, TreeReference base_Renamed)
		{
			//TODO: Namespaces?
			AbstractTreeElement root;
			if (base_Renamed == null)
			{
				root = instance.getRoot();
			}
			else
			{
				root = instance.resolveReference(base_Renamed);
			}
			
			//write root
			serializer.startTag(root.getNamespace(), root.getName());
			
			for (int i = 0; i < root.getNumChildren(); i++)
			{
				//write children
				AbstractTreeElement childAt = root.getChildAt(i);
				serializeNode(childAt);
			}
			
			//end root
			serializer.endTag(root.getNamespace(), root.getName());
			serializer.flush();
		}
		
		public virtual void  serializeNode(AbstractTreeElement instanceNode)
		{
			//don't serialize template nodes or non-relevant nodes
			if (!instanceNode.isRelevant() || instanceNode.getMult() == TreeReference.INDEX_TEMPLATE)
			{
				return ;
			}
			
			serializer.startTag(instanceNode.getNamespace(), instanceNode.getName());
			for (int i = 0; i < instanceNode.getAttributeCount(); ++i)
			{
				System.String val = instanceNode.getAttributeValue(i);
				val = val == null?"":val;
				serializer.attribute(instanceNode.getAttributeNamespace(i), instanceNode.getAttributeName(i), val);
			}
			
			if (instanceNode.getValue() != null)
			{
				serializer.text(instanceNode.getValue().uncast().getString());
			}
			else
			{
				for (int i = 0; i < instanceNode.getNumChildren(); ++i)
				{
					serializeNode(instanceNode.getChildAt(i));
				}
			}
			
			serializer.endTag(instanceNode.getNamespace(), instanceNode.getName());
		}
	}
}