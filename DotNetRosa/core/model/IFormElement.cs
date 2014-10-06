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
using Localizable = org.javarosa.core.services.locale.Localizable;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
namespace org.javarosa.core.model
{
	
	/// <summary> An IFormDataElement is an element of the physical interaction for
	/// a form, an example of an implementing element would be the definition
	/// of a Question. 
	/// 
	/// </summary>
	/// <author>  Drew Roos
	/// 
	/// </author>
	public interface IFormElement:Localizable, Externalizable
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <returns> The unique ID of this element
		/// </returns>
		/// <param name="id">The new unique ID of this element
		/// </param>
		int ID
		{
			get;
			
			set;
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> get the TextID for this element used for localization purposes</summary>
		/// <returns> the TextID (bare, no ;form appended to it!!)
		/// </returns>
		/// <summary> Set the textID for this element for use with localization.</summary>
		/// <param name="id">the plain TextID WITHOUT any form specification (e.g. ;long)
		/// </param>
		System.String TextID
		{
			get;
			
			set;
			
			
			/// <returns> A vector containing any children that this element
			/// might have. Null if the element is not able to have child
			/// elements.
			/// </returns>
			
		}
	}
	
	List< IFormElement > getChildren();
	
	/// <param name="v">the children of this element, if it is capable of having
	/// child elements.
	/// </param>
	/// <throws>  IllegalStateException if the element is incapable of </throws>
	/// <summary> having children.
	/// </summary>
	
	void setChildren(List< IFormElement > v);
	
	/// <param name="fe">The child element to be added
	/// </param>
	/// <throws>  IllegalStateException if the element is incapable of </throws>
	/// <summary> having children.
	/// </summary>
	
	void addChild(IFormElement fe);
	
	
	IFormElement getChild(int i);
	
	/// <returns> A recursive count of how many elements are ancestors of this element.
	/// </returns>
	
	int getDeepChildCount();
	
	/// <returns> The data reference for this element
	/// </returns>
	
	IDataReference getBind();
	
	/// <summary> Registers a state observer for this element.
	/// 
	/// </summary>
	/// <param name="qsl">
	/// </param>
	
	public
	
	void registerStateObserver(FormElementStateListener qsl);
	
	/// <summary> Unregisters a state observer for this element.
	/// 
	/// </summary>
	/// <param name="qsl">
	/// </param>
	
	public
	
	void unregisterStateObserver(FormElementStateListener qsl);
	
	/// <summary> This method returns the regular
	/// innertext betweem label tags (if present) (&ltlabel&gtinnertext&lt/label&gt).
	/// </summary>
	/// <returns> &ltlabel&gt innertext or null (if innertext is not present).
	/// </returns>
	
	public String getLabelInnerText();
	
	
	/// <returns>
	/// </returns>
	
	public String getAppearanceAttr();
	
	
	public
	
	void setAppearanceAttr(String appearanceAttr);
	
	/// <summary> Capture additional attributes on a Question or Group
	/// 
	/// </summary>
	/// <param name="namespace">
	/// </param>
	/// <param name="name">
	/// </param>
	/// <param name="value">
	/// </param>
	
	public
	
	void setAdditionalAttribute(String namespace, String name, String value);
	
	/// <summary> Retrieve the value of an additional attribute on a Question or Group</summary>
	/// <param name="namespace">
	/// </param>
	/// <param name="name">
	/// </param>
	/// <returns>
	/// </returns>
	
	public String getAdditionalAttribute(String namespace, String name);
	
	/// <summary> Retrieve all additional attributes on a Question or Group
	/// 
	/// </summary>
	/// <returns>
	/// </returns>
	
	public List< TreeElement > getAdditionalAttributes();
	
	
	}
}