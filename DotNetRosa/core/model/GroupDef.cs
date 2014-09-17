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
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using DateUtils = org.javarosa.core.model.utils.DateUtils;
using Localizable = org.javarosa.core.services.locale.Localizable;
using Localizer = org.javarosa.core.services.locale.Localizer;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapListPoly = org.javarosa.core.util.externalizable.ExtWrapListPoly;
using ExtWrapNullable = org.javarosa.core.util.externalizable.ExtWrapNullable;
using ExtWrapTagged = org.javarosa.core.util.externalizable.ExtWrapTagged;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.model
{
	
	/// <summary>The definition of a group in a form or questionaire.
	/// 
	/// </summary>
	/// <author>  Daniel Kayiwa
	/// 
	/// </author>
	public class GroupDef : IFormElement, Localizable
	{
		private void  InitBlock()
		{
			return additionalAttributes;
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
		virtual public System.Collections.ArrayList Children
		{
			get
			{
				return children;
			}
			
			set
			{
				this.children = (value == null?System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10)):value);
			}
			
		}
		/// <returns> true if this represents a <repeat> element
		/// </returns>
		virtual public bool Repeat
		{
			get
			{
				return repeat;
			}
			
			set
			{
				this.repeat = value;
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
				labelInnerText = value;
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
		virtual public IDataReference CountReference
		{
			get
			{
				return count;
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
				int total = 0;
				System.Collections.IEnumerator e = children.GetEnumerator();
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				while (e.MoveNext())
				{
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					total += ((IFormElement) e.Current).DeepChildCount;
				}
				return total;
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
				if (value == null)
				{
					this.textID = null;
					return ;
				}
				if (DateUtils.stringContains(value, ";"))
				{
					System.Console.Error.WriteLine("Warning: TextID contains ;form modifier:: \"" + value.Substring(value.IndexOf(";")) + "\"... will be stripped.");
					value = value.Substring(0, (value.IndexOf(";")) - (0)); //trim away the form specifier
				}
				this.textID = value;
			}
			
		}
		private System.Collections.ArrayList children; /// <summary>A list of questions on a group. </summary>
		private bool repeat; /// <summary>True if this is a "repeat", false if it is a "group" </summary>
		private int id; /// <summary>The group number. </summary>
		private IDataReference binding; /// <summary>reference to a location in the model to store data in </summary>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private Vector < TreeElement > additionalAttributes = new Vector < TreeElement >();
		
		private System.String labelInnerText;
		private System.String appearanceAttr;
		private System.String textID;
		
		//custom phrasings for repeats
		public System.String chooseCaption;
		public System.String addCaption;
		public System.String delCaption;
		public System.String doneCaption;
		public System.String addEmptyCaption;
		public System.String doneEmptyCaption;
		public System.String entryHeader;
		public System.String delHeader;
		public System.String mainHeader;
		
		internal System.Collections.ArrayList observers;
		
		public bool noAddRemove = false;
		public IDataReference count = null;
		
		public GroupDef():this(Constants.NULL_ID, null, false)
		{
		}
		
		public GroupDef(int id, System.Collections.ArrayList children, bool repeat)
		{
			InitBlock();
			ID = id;
			Children = children;
			Repeat = repeat;
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
		
		public virtual void  addChild(IFormElement fe)
		{
			children.Add(fe);
		}
		
		public virtual IFormElement getChild(int i)
		{
			if (children == null || i >= children.Count)
			{
				return null;
			}
			else
			{
				return (IFormElement) children[i];
			}
		}
		
		public virtual void  localeChanged(System.String locale, Localizer localizer)
		{
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator e = children.GetEnumerator(); e.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				((IFormElement) e.Current).localeChanged(locale, localizer);
			}
		}
		
		public virtual TreeReference getConextualizedCountReference(TreeReference context)
		{
			return FormInstance.unpackReference(count).contextualize(context);
		}
		
		public override System.String ToString()
		{
			return "<group>";
		}
		
		/// <summary>Reads a group definition object from the supplied stream. </summary>
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader dis, PrototypeFactory pf)
		{
			ID = ExtUtil.readInt(dis);
			AppearanceAttr = ((System.String) ExtUtil.read(dis, new ExtWrapNullable(typeof(System.String)), pf));
			Bind = (IDataReference) ExtUtil.read(dis, new ExtWrapTagged(), pf);
			TextID = ((System.String) ExtUtil.read(dis, new ExtWrapNullable(typeof(System.String)), pf));
			LabelInnerText = ((System.String) ExtUtil.read(dis, new ExtWrapNullable(typeof(System.String)), pf));
			Repeat = ExtUtil.readBool(dis);
			Children = (System.Collections.ArrayList) ExtUtil.read(dis, new ExtWrapListPoly(), pf);
			
			noAddRemove = ExtUtil.readBool(dis);
			count = (IDataReference) ExtUtil.read(dis, new ExtWrapNullable(new ExtWrapTagged()), pf);
			
			chooseCaption = ExtUtil.nullIfEmpty(ExtUtil.readString(dis));
			addCaption = ExtUtil.nullIfEmpty(ExtUtil.readString(dis));
			delCaption = ExtUtil.nullIfEmpty(ExtUtil.readString(dis));
			doneCaption = ExtUtil.nullIfEmpty(ExtUtil.readString(dis));
			addEmptyCaption = ExtUtil.nullIfEmpty(ExtUtil.readString(dis));
			doneEmptyCaption = ExtUtil.nullIfEmpty(ExtUtil.readString(dis));
			entryHeader = ExtUtil.nullIfEmpty(ExtUtil.readString(dis));
			delHeader = ExtUtil.nullIfEmpty(ExtUtil.readString(dis));
			mainHeader = ExtUtil.nullIfEmpty(ExtUtil.readString(dis));
			
			additionalAttributes = ExtUtil.readAttributes(dis, null);
		}
		
		/// <summary>Write the group definition object to the supplied stream. </summary>
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter dos)
		{
			ExtUtil.writeNumeric(dos, ID);
			ExtUtil.write(dos, new ExtWrapNullable(AppearanceAttr));
			ExtUtil.write(dos, new ExtWrapTagged(Bind));
			ExtUtil.write(dos, new ExtWrapNullable(TextID));
			ExtUtil.write(dos, new ExtWrapNullable(LabelInnerText));
			ExtUtil.writeBool(dos, Repeat);
			ExtUtil.write(dos, new ExtWrapListPoly(Children));
			
			ExtUtil.writeBool(dos, noAddRemove);
			ExtUtil.write(dos, new ExtWrapNullable(count != null?new ExtWrapTagged(count):null));
			
			ExtUtil.writeString(dos, ExtUtil.emptyIfNull(chooseCaption));
			ExtUtil.writeString(dos, ExtUtil.emptyIfNull(addCaption));
			ExtUtil.writeString(dos, ExtUtil.emptyIfNull(delCaption));
			ExtUtil.writeString(dos, ExtUtil.emptyIfNull(doneCaption));
			ExtUtil.writeString(dos, ExtUtil.emptyIfNull(addEmptyCaption));
			ExtUtil.writeString(dos, ExtUtil.emptyIfNull(doneEmptyCaption));
			ExtUtil.writeString(dos, ExtUtil.emptyIfNull(entryHeader));
			ExtUtil.writeString(dos, ExtUtil.emptyIfNull(delHeader));
			ExtUtil.writeString(dos, ExtUtil.emptyIfNull(mainHeader));
			
			ExtUtil.writeAttributes(dos, additionalAttributes);
		}
		
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
	}
}