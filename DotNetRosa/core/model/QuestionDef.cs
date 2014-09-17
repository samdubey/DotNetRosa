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
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using DateUtils = org.javarosa.core.model.utils.DateUtils;
using Localizable = org.javarosa.core.services.locale.Localizable;
using Localizer = org.javarosa.core.services.locale.Localizer;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapList = org.javarosa.core.util.externalizable.ExtWrapList;
using ExtWrapNullable = org.javarosa.core.util.externalizable.ExtWrapNullable;
using ExtWrapTagged = org.javarosa.core.util.externalizable.ExtWrapTagged;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.model
{
	
	/// <summary> The definition of a Question to be presented to users when
	/// filling out a form.
	/// 
	/// QuestionDef requires that any IDataReferences that are used
	/// are contained in the FormDefRMS's PrototypeFactoryDeprecated in order
	/// to be properly deserialized. If they aren't, an exception
	/// will be thrown at the time of deserialization.
	/// 
	/// </summary>
	/// <author>  Daniel Kayiwa/Drew Roos
	/// 
	/// </author>
	public class QuestionDef : IFormElement, Localizable
	{
		private void  InitBlock()
		{
			return additionalAttributes;
			return choices;
		}
		virtual public int ID
		{
			get
			{
				return id;
			}
			
			set
			{
				this.id = value;
			}
			
		}
		virtual public IDataReference Bind
		{
			get
			{
				return binding;
			}
			
			set
			{
				this.binding = value;
			}
			
		}
		virtual public int ControlType
		{
			get
			{
				return controlType;
			}
			
			set
			{
				this.controlType = value;
			}
			
		}
		virtual public System.String AppearanceAttr
		{
			get
			{
				return appearanceAttr;
			}
			
			set
			{
				this.appearanceAttr = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Only if there is no localizable version of the &lthint&gt available should this method be used</summary>
		/// <summary> Only if there is no localizable version of the &lthint&gtavailable should this method be used</summary>
		virtual public System.String HelpText
		{
			get
			{
				return helpText;
			}
			
			set
			{
				this.helpText = value;
			}
			
		}
		virtual public System.String HelpTextID
		{
			get
			{
				return helpTextID;
			}
			
			set
			{
				this.helpTextID = value;
			}
			
		}
		virtual public int NumChoices
		{
			get
			{
				return (choices != null?choices.size():0);
			}
			
		}
		virtual public ItemsetBinding DynamicChoices
		{
			get
			{
				return dynamicChoices;
			}
			
			set
			{
				if (value != null)
				{
					value.setDestRef(this);
				}
				this.dynamicChoices = value;
			}
			
		}
		/// <summary> true if the answer to this question yields xml tree data, not a simple string value</summary>
		virtual public bool Complex
		{
			get
			{
				return (dynamicChoices != null && dynamicChoices.copyMode);
			}
			
		}
		virtual public System.Collections.ArrayList Children
		{
			get
			{
				return null;
			}
			
			set
			{
				throw new System.SystemException("Can't set children on question def");
			}
			
		}
		virtual public int DeepChildCount
		{
			/*
			* (non-Javadoc)
			* @see org.javarosa.core.model.IFormElement#getDeepChildCount()
			*/
			
			get
			{
				return 1;
			}
			
		}
		virtual public System.String LabelInnerText
		{
			get
			{
				return labelInnerText;
			}
			
			set
			{
				this.labelInnerText = value;
			}
			
		}
		virtual public System.String HelpInnerText
		{
			get
			{
				return helpInnerText;
			}
			
			set
			{
				this.helpInnerText = value;
			}
			
		}
		virtual public System.String TextID
		{
			get
			{
				return textID;
			}
			
			set
			{
				if (DateUtils.stringContains(value, ";"))
				{
					System.Console.Error.WriteLine("Warning: TextID contains ;form modifier:: \"" + value.Substring(value.IndexOf(";")) + "\"... will be stripped.");
					value = value.Substring(0, (value.IndexOf(";")) - (0)); //trim away the form specifier
				}
				this.textID = value;
			}
			
		}
		private int id;
		private IDataReference binding; /// <summary>reference to a location in the model to store data in </summary>
		
		private int controlType; /* The type of widget. eg TextInput,Slider,List etc. */
		private System.String appearanceAttr;
		private System.String helpTextID;
		private System.String labelInnerText;
		private System.String helpText;
		private System.String textID; /* The id (ref) pointing to the localized values of (pic-URIs,audio-URIs,text) */
		private System.String helpInnerText;
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private Vector < TreeElement > additionalAttributes = new Vector < TreeElement >();
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private Vector < SelectChoice > choices;
		private ItemsetBinding dynamicChoices;
		
		internal System.Collections.ArrayList observers;
		
		public QuestionDef():this(Constants.NULL_ID, Constants.DATATYPE_TEXT)
		{
		}
		
		public QuestionDef(int id, int controlType)
		{
			InitBlock();
			ID = id;
			ControlType = controlType;
			observers = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
		}
		
		
		public virtual void  setAdditionalAttribute(System.String namespace_Renamed, System.String name, System.String value_Renamed)
		{
			TreeElement.setAttribute(null, additionalAttributes, namespace_Renamed, name, value_Renamed);
		}
		
		public virtual System.String getAdditionalAttribute(System.String namespace_Renamed, System.String name)
		{
			TreeElement e = TreeElement.getAttribute(additionalAttributes, namespace_Renamed, name);
			if (e != null)
			{
				return e.getAttributeValue();
			}
			return null;
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public Vector < TreeElement > getAdditionalAttributes()
		
		
		public virtual void  addSelectChoice(SelectChoice choice)
		{
			if (choices == null)
			{
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				choices = new Vector < SelectChoice >();
			}
			choice.setIndex(choices.size());
			choices.addElement(choice);
		}
		
		public virtual void  removeSelectChoice(SelectChoice choice)
		{
			if (choices == null)
			{
				choice.Index = 0;
				return ;
			}
			
			if (choices.contains(choice))
			{
				choices.removeElement(choice);
			}
		}
		
		public virtual void  removeAllSelectChoices()
		{
			if (choices != null)
			{
				choices.removeAllElements();
			}
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public Vector < SelectChoice > getChoices()
		
		public virtual SelectChoice getChoice(int i)
		{
			return choices.elementAt(i);
		}
		
		public virtual SelectChoice getChoiceForValue(System.String value_Renamed)
		{
			for (int i = 0; i < NumChoices; i++)
			{
				if (getChoice(i).Value.Equals(value_Renamed))
				{
					return getChoice(i);
				}
			}
			return null;
		}
		
		//Deprecated
		public virtual void  localeChanged(System.String locale, Localizer localizer)
		{
			if (choices != null)
			{
				for (int i = 0; i < choices.size(); i++)
				{
					choices.elementAt(i).localeChanged(null, localizer);
				}
			}
			
			if (dynamicChoices != null)
			{
				dynamicChoices.localeChanged(locale, localizer);
			}
			
			alertStateObservers(org.javarosa.core.model.FormElementStateListener_Fields.CHANGE_LOCALE);
		}
		
		public virtual void  addChild(IFormElement fe)
		{
			throw new System.SystemException("Can't add children to question def");
		}
		
		public virtual IFormElement getChild(int i)
		{
			return null;
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.util.Externalizable#readExternal(java.io.DataInputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader dis, PrototypeFactory pf)
		{
			ID = ExtUtil.readInt(dis);
			binding = (IDataReference) ExtUtil.read(dis, new ExtWrapNullable(new ExtWrapTagged()), pf);
			AppearanceAttr = ((System.String) ExtUtil.read(dis, new ExtWrapNullable(typeof(System.String)), pf));
			TextID = ((System.String) ExtUtil.read(dis, new ExtWrapNullable(typeof(System.String)), pf));
			LabelInnerText = ((System.String) ExtUtil.read(dis, new ExtWrapNullable(typeof(System.String)), pf));
			HelpText = ((System.String) ExtUtil.read(dis, new ExtWrapNullable(typeof(System.String)), pf));
			HelpTextID = ((System.String) ExtUtil.read(dis, new ExtWrapNullable(typeof(System.String)), pf));
			HelpInnerText = ((System.String) ExtUtil.read(dis, new ExtWrapNullable(typeof(System.String)), pf));
			
			ControlType = ExtUtil.readInt(dis);
			
			additionalAttributes = ExtUtil.readAttributes(dis, null);
			
			choices = ExtUtil.nullIfEmpty((System.Collections.ArrayList) ExtUtil.read(dis, new ExtWrapList(typeof(SelectChoice)), pf));
			for (int i = 0; i < NumChoices; i++)
			{
				choices.elementAt(i).setIndex(i);
			}
			DynamicChoices = (ItemsetBinding) ExtUtil.read(dis, new ExtWrapNullable(typeof(ItemsetBinding)));
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.util.Externalizable#writeExternal(java.io.DataOutputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter dos)
		{
			ExtUtil.writeNumeric(dos, ID);
			ExtUtil.write(dos, new ExtWrapNullable(binding == null?null:new ExtWrapTagged(binding)));
			ExtUtil.write(dos, new ExtWrapNullable(AppearanceAttr));
			ExtUtil.write(dos, new ExtWrapNullable(TextID));
			ExtUtil.write(dos, new ExtWrapNullable(LabelInnerText));
			ExtUtil.write(dos, new ExtWrapNullable(HelpText));
			ExtUtil.write(dos, new ExtWrapNullable(HelpTextID));
			ExtUtil.write(dos, new ExtWrapNullable(HelpInnerText));
			
			ExtUtil.writeNumeric(dos, ControlType);
			
			ExtUtil.writeAttributes(dos, additionalAttributes);
			
			ExtUtil.write(dos, new ExtWrapList(ExtUtil.emptyIfNull(choices)));
			ExtUtil.write(dos, new ExtWrapNullable(dynamicChoices));
		}
		
		/* === MANAGING OBSERVERS === */
		
		public virtual void  registerStateObserver(FormElementStateListener qsl)
		{
			if (!observers.Contains(qsl))
			{
				observers.Add(qsl);
			}
		}
		
		public virtual void  unregisterStateObserver(FormElementStateListener qsl)
		{
			observers.Remove(qsl);
		}
		
		public virtual void  unregisterAll()
		{
			observers.Clear();
		}
		
		public virtual void  alertStateObservers(int changeFlags)
		{
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator e = observers.GetEnumerator(); e.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				((FormElementStateListener) e.Current).formElementStateChanged(this, changeFlags);
			}
		}
	}
}