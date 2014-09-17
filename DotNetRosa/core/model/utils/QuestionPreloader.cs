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
using DateData = org.javarosa.core.model.data.DateData;
using DateTimeData = org.javarosa.core.model.data.DateTimeData;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using StringData = org.javarosa.core.model.data.StringData;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using PropertyManager = org.javarosa.core.services.PropertyManager;
using Map = org.javarosa.core.util.Map;
using PropertyUtils = org.javarosa.core.util.PropertyUtils;
namespace org.javarosa.core.model.utils
{
	
	/// <summary> The Question Preloader is responsible for maintaining a set of handlers which are capable 
	/// of parsing 'preload' elements, and their parameters, and returning IAnswerData objects.
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public class QuestionPreloader
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassIPreloadHandler' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassIPreloadHandler : IPreloadHandler
		{
			public AnonymousClassIPreloadHandler(QuestionPreloader enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(QuestionPreloader enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private QuestionPreloader enclosingInstance;
			public QuestionPreloader Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual System.String preloadHandled()
			{
				return "date";
			}
			
			public virtual IAnswerData handlePreload(System.String preloadParams)
			{
				return Enclosing_Instance.preloadDate(preloadParams);
			}
			
			public virtual bool handlePostProcess(TreeElement node, System.String params_Renamed)
			{
				//do nothing
				return false;
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassIPreloadHandler1' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassIPreloadHandler1 : IPreloadHandler
		{
			public AnonymousClassIPreloadHandler1(QuestionPreloader enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(QuestionPreloader enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private QuestionPreloader enclosingInstance;
			public QuestionPreloader Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual System.String preloadHandled()
			{
				return "property";
			}
			
			public virtual IAnswerData handlePreload(System.String preloadParams)
			{
				return Enclosing_Instance.preloadProperty(preloadParams);
			}
			
			public virtual bool handlePostProcess(TreeElement node, System.String params_Renamed)
			{
				Enclosing_Instance.saveProperty(params_Renamed, node);
				return false;
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassIPreloadHandler2' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassIPreloadHandler2 : IPreloadHandler
		{
			public AnonymousClassIPreloadHandler2(QuestionPreloader enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(QuestionPreloader enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private QuestionPreloader enclosingInstance;
			public QuestionPreloader Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual System.String preloadHandled()
			{
				return "timestamp";
			}
			
			public virtual IAnswerData handlePreload(System.String preloadParams)
			{
				return ("start".Equals(preloadParams)?Enclosing_Instance.Timestamp:null);
			}
			
			public virtual bool handlePostProcess(TreeElement node, System.String params_Renamed)
			{
				if ("end".Equals(params_Renamed))
				{
					node.setAnswer(Enclosing_Instance.Timestamp);
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassIPreloadHandler3' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassIPreloadHandler3 : IPreloadHandler
		{
			public AnonymousClassIPreloadHandler3(QuestionPreloader enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(QuestionPreloader enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private QuestionPreloader enclosingInstance;
			public QuestionPreloader Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual System.String preloadHandled()
			{
				return "uid";
			}
			public virtual IAnswerData handlePreload(System.String preloadParams)
			{
				return new StringData(PropertyUtils.genGUID(25));
			}
			
			public virtual bool handlePostProcess(TreeElement node, System.String params_Renamed)
			{
				return false;
			}
		}
		private DateTimeData Timestamp
		{
			get
			{
				System.DateTime tempAux = System.DateTime.Now;
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
				return new DateTimeData(ref tempAux);
			}
			
		}
		/* String -> IPreloadHandler */
		private Map preloadHandlers;
		
		/// <summary> Creates a new Preloader with default handlers</summary>
		public QuestionPreloader()
		{
			preloadHandlers = new Map();
			initPreloadHandlers();
		}
		
		/// <summary> Initializes the default preload handlers</summary>
		private void  initPreloadHandlers()
		{
			IPreloadHandler date = new AnonymousClassIPreloadHandler(this);
			
			IPreloadHandler property = new AnonymousClassIPreloadHandler1(this);
			
			IPreloadHandler timestamp = new AnonymousClassIPreloadHandler2(this);
			
			IPreloadHandler uid = new AnonymousClassIPreloadHandler3(this);
			
			/*
			//TODO: Finish this up.
			IPreloadHandler meta = new IPreloadHandler() {
			public String preloadHandled() {
			return "meta";
			}
			public IAnswerData handlePreload(String preloadParams) {
			//TODO: Ideally, we want to handle this preloader by taking in the
			//existing structure. Resultantly, we don't want to mess with this.
			//We should be enforcing that we don't.
			return null;
			}
			
			public boolean handlePostProcess(TreeElement node, String params) {
			Vector kids = node.getChildren();
			Enumeration en = kids.elements();
			while(en.hasMoreElements()) {
			TreeElement kid = (TreeElement)en.nextElement();
			if(kid.getName().equals("uid")) {
			kid.setValue(new StringData(PropertyUtils.genGUID(25)));
			}
			}
			return true;
			}
			};
			*/
			addPreloadHandler(date);
			addPreloadHandler(property);
			addPreloadHandler(timestamp);
			addPreloadHandler(uid);
		}
		
		/// <summary> Adds a new preload handler to this preloader. 
		/// 
		/// </summary>
		/// <param name="handler">an IPreloadHandler that can handle a preload type
		/// </param>
		public virtual void  addPreloadHandler(IPreloadHandler handler)
		{
			preloadHandlers.put(handler.preloadHandled(), handler);
		}
		
		/// <summary> Returns the IAnswerData preload value for the given preload type and parameters
		/// 
		/// </summary>
		/// <param name="preloadType">The type of the preload to be returned
		/// </param>
		/// <param name="preloadParams">Parameters for the preload handler
		/// </param>
		/// <returns> An IAnswerData corresponding to a pre-loaded value for the given
		/// Arguments. null if no preload could successfully be derived due to either
		/// the lack of a handler, or to invalid parameters
		/// </returns>
		public virtual IAnswerData getQuestionPreload(System.String preloadType, System.String preloadParams)
		{
			IPreloadHandler handler = (IPreloadHandler) preloadHandlers.get_Renamed(preloadType);
			if (handler != null)
			{
				return handler.handlePreload(preloadParams);
			}
			else
			{
				System.Console.Error.WriteLine("Do not know how to handle preloader [" + preloadType + "]");
				return null;
			}
		}
		
		public virtual bool questionPostProcess(TreeElement node, System.String preloadType, System.String params_Renamed)
		{
			IPreloadHandler handler = (IPreloadHandler) preloadHandlers.get_Renamed(preloadType);
			if (handler != null)
			{
				return handler.handlePostProcess(node, params_Renamed);
			}
			else
			{
				System.Console.Error.WriteLine("Do not know how to handle preloader [" + preloadType + "]");
				return false;
			}
		}
		
		/// <summary> Preloads a DateData object for the preload type 'date'
		/// 
		/// </summary>
		/// <param name="preloadParams">The parameters determining the date
		/// </param>
		/// <returns> A preload date value if the parameters can be parsed,
		/// null otherwise
		/// </returns>
		private IAnswerData preloadDate(System.String preloadParams)
		{
			//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
			System.DateTime d = null;
			if (preloadParams.Equals("today"))
			{
				d = System.DateTime.Now;
			}
			else if (preloadParams.Substring(0, (11) - (0)).Equals("prevperiod-"))
			{
				System.Collections.ArrayList v = DateUtils.split(preloadParams.Substring(11), "-", false);
				System.String[] params_Renamed = new System.String[v.Count];
				for (int i = 0; i < params_Renamed.Length; i++)
					params_Renamed[i] = ((System.String) v[i]);
				
				try
				{
					System.String type = params_Renamed[0];
					System.String start = params_Renamed[1];
					
					bool beginning;
					if (params_Renamed[2].Equals("head"))
						beginning = true;
					else if (params_Renamed[2].Equals("tail"))
						beginning = false;
					else
						throw new System.SystemException();
					
					bool includeToday;
					if (params_Renamed.Length >= 4)
					{
						if (params_Renamed[3].Equals("x"))
							includeToday = true;
						else if (params_Renamed[3].Equals(""))
							includeToday = false;
						else
							throw new System.SystemException();
					}
					else
					{
						includeToday = false;
					}
					
					int nAgo;
					if (params_Renamed.Length >= 5)
					{
						nAgo = System.Int32.Parse(params_Renamed[4]);
					}
					else
					{
						nAgo = 1;
					}
					
					System.DateTime tempAux = System.DateTime.Now;
					//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					d = DateUtils.getPastPeriodDate(ref tempAux, type, start, beginning, includeToday, nAgo);
				}
				catch (System.Exception e)
				{
					throw new System.ArgumentException("invalid preload params for preload mode 'date'");
				}
			}
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			DateData data = new DateData(ref d);
			return data;
		}
		
		/// <summary> Preloads a StringData object for the preload type 'property'
		/// 
		/// </summary>
		/// <param name="preloadParams">The parameters determining the property to be retrieved
		/// </param>
		/// <returns> A preload property value if the parameters can be parsed,
		/// null otherwise
		/// </returns>
		private IAnswerData preloadProperty(System.String preloadParams)
		{
			System.String propname = preloadParams;
			System.String propval = PropertyManager._().getSingularProperty(propname);
			StringData data = null;
			if (propval != null && propval.Length > 0)
			{
				data = new StringData(propval);
			}
			return data;
		}
		
		private void  saveProperty(System.String propName, TreeElement node)
		{
			IAnswerData answer = node.Value;
			System.String value_Renamed = (answer == null?null:answer.DisplayText);
			if (propName != null && propName.Length > 0 && value_Renamed != null && value_Renamed.Length > 0)
				PropertyManager._().setProperty(propName, value_Renamed);
		}
	}
}