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
using IModule = org.javarosa.core.api.IModule;
using FormDef = org.javarosa.core.model.FormDef;
using IConditionExpr = org.javarosa.core.model.condition.IConditionExpr;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using IXFormyFactory = org.javarosa.core.model.util.restorable.IXFormyFactory;
using RestoreUtils = org.javarosa.core.model.util.restorable.RestoreUtils;
using PrototypeManager = org.javarosa.core.services.PrototypeManager;
using IDataPayload = org.javarosa.core.services.transport.payload.IDataPayload;
using XFormParser = org.javarosa.xform.parse.XFormParser;
using XFormAnswerDataParser = org.javarosa.xform.util.XFormAnswerDataParser;
using XFormAnswerDataSerializer = org.javarosa.xform.util.XFormAnswerDataSerializer;
using XPathConditional = org.javarosa.xpath.XPathConditional;
using XPathParseTool = org.javarosa.xpath.XPathParseTool;
using XPathPathExpr = org.javarosa.xpath.expr.XPathPathExpr;
namespace org.javarosa.model.xform
{
	
	public class XFormsModule : IModule
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassIXFormyFactory' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassIXFormyFactory : IXFormyFactory
		{
			public AnonymousClassIXFormyFactory(XFormsModule enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(XFormsModule enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private XFormsModule enclosingInstance;
			public XFormsModule Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual TreeReference ref_Renamed(System.String refStr)
			{
				return FormInstance.unpackReference(new XPathReference(refStr));
			}
			
			public virtual IDataPayload serializeInstance(FormInstance dm)
			{
				try
				{
					return (new XFormSerializingVisitor()).createSerializedPayload(dm);
				}
				catch (System.IO.IOException e)
				{
					return null;
				}
			}
			
			public virtual FormInstance parseRestore(sbyte[] data, System.Type restorableType)
			{
				return XFormParser.restoreDataModel(data, restorableType);
			}
			
			public virtual IAnswerData parseData(System.String textVal, int dataType, TreeReference ref_Renamed, FormDef f)
			{
				return XFormAnswerDataParser.getAnswerData(textVal, dataType, XFormParser.ghettoGetQuestionDef(dataType, f, ref_Renamed));
			}
			
			public virtual System.String serializeData(IAnswerData data)
			{
				return (System.String) (new XFormAnswerDataSerializer().serializeAnswerData(data));
			}
			
			public virtual IConditionExpr refToPathExpr(TreeReference ref_Renamed)
			{
				return new XPathConditional(XPathPathExpr.fromRef(ref_Renamed));
			}
		}
		
		public virtual void  registerModule()
		{
			System.String[] classes = new System.String[]{"org.javarosa.model.xform.XPathReference", "org.javarosa.xpath.XPathConditional"};
			
			PrototypeManager.registerPrototypes(classes);
			PrototypeManager.registerPrototypes(XPathParseTool.xpathClasses);
			RestoreUtils.xfFact = new AnonymousClassIXFormyFactory(this);
		}
	}
}