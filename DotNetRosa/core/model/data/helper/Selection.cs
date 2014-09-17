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
using QuestionDef = org.javarosa.core.model.QuestionDef;
using SelectChoice = org.javarosa.core.model.SelectChoice;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using XPathTypeMismatchException = org.javarosa.xpath.XPathTypeMismatchException;
namespace org.javarosa.core.model.data.helper
{
	
	/// <summary> A response to a question requesting a selection
	/// from a list.
	/// 
	/// This class may exist in 3 states:
	/// 
	/// 1) only index has a value
	/// 2) only xmlValue has a value
	/// 3) index, xmlValue, and choice have values, where index and xmlValue are simply cached copies of the values in 'choice'
	/// 
	/// the 3rd form is the most full-featured, and is required for situations where you want to recover the captions for the
	/// choices, such as form entry. the choice objects used in the form entry model will receive localization updates,
	/// allowing you to retrieve the appropriate caption.
	/// 
	/// the 2nd form is useful when dealing with FormInstances without having to worry about the FormDef or the captions
	/// from the <select> or <select1> controls. this form contains enough information to convert to an XML instance
	/// 
	/// the 1st form is used when serializing instances in an ultra-compact manner, but requires linking to a FormDef before
	/// you can do anything useful with the instance (insufficient info to convert to XML instance).
	/// 
	/// </summary>
	/// <author>  Drew Roos
	/// 
	/// </author>
	public class Selection : System.ICloneable
	{
		virtual public System.String Value
		{
			get
			{
				if (xmlValue != null && xmlValue.Length > 0)
				{
					return xmlValue;
				}
				else
				{
					throw new System.SystemException("don't know xml value! perhaps selection was stored as index only and has not yet been linked up to a formdef?");
				}
			}
			
		}
		public System.String xmlValue = null;
		public int index = - 1;
		
		/* in order to get localizable captions for this selection, the choice object must be the
		* same object in the form model, or else it won't receive localization updates from form
		* entry session
		*/
		public SelectChoice choice;
		
		/// <summary> for deserialization only</summary>
		public Selection()
		{
		}
		
		public Selection(SelectChoice choice)
		{
			attachChoice(choice);
		}
		
		public Selection(System.String xmlValue)
		{
			this.xmlValue = xmlValue;
		}
		
		public Selection(int index)
		{
			this.index = index;
		}
		
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.lang.Object.clone' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		public virtual System.Object Clone()
		{
			Selection s = new Selection();
			s.choice = choice;
			s.xmlValue = xmlValue;
			s.index = index;
			
			return s;
		}
		
		public virtual void  attachChoice(SelectChoice choice)
		{
			this.choice = choice;
			this.xmlValue = choice.Value;
			this.index = choice.Index;
		}
		
		public virtual void  attachChoice(QuestionDef q)
		{
			if (q.DynamicChoices != null)
			//can't attach dynamic choices because they aren't guaranteed to exist yet
				return ;
			
			SelectChoice choice = null;
			
			if (index != - 1 && index < q.NumChoices)
			{
				choice = q.getChoice(index);
			}
			else if (xmlValue != null && xmlValue.Length > 0)
			{
				choice = q.getChoiceForValue(xmlValue);
			}
			
			if (choice != null)
			{
				attachChoice(choice);
			}
			else
			{
				throw new XPathTypeMismatchException("value " + xmlValue + " could not be loaded into question " + q.TextID + ".  Check to see if value " + xmlValue + " is a valid option for question " + q.TextID + ".");
			}
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.utilities.Externalizable#readExternal(java.io.DataInputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			xmlValue = ExtUtil.readString(in_Renamed);
			index = ExtUtil.readInt(in_Renamed);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.utilities.Externalizable#writeExternal(java.io.DataOutputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeString(out_Renamed, Value);
			ExtUtil.writeNumeric(out_Renamed, index);
		}
	}
}