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
using Selection = org.javarosa.core.model.data.helper.Selection;
using DateUtils = org.javarosa.core.model.utils.DateUtils;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapList = org.javarosa.core.util.externalizable.ExtWrapList;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.model.data
{
	
	/// <summary> A response to a question requesting a selection of
	/// any number of items from a list.
	/// 
	/// </summary>
	/// <author>  Drew Roos
	/// 
	/// </author>
	public class SelectMultiData : IAnswerData, System.ICloneable
	{
		private void  InitBlock()
		{
			Value = vs; 
			List< Selection > output = new List< Selection >();
			//validate type
			for (int i = 0; i < input.size(); i++)
			{
				Selection s = (Selection) input.elementAt(i);
				output.addElement(s);
			}
			return output;
		}
		virtual public System.Object Value
		{
			get
			{
				return vectorCopy(vs);
			}
			
			set
			{
				if (value == null)
				{
					throw new System.NullReferenceException("Attempt to set an IAnswerData class to null.");
				}
				
				
				vs = vectorCopy((List< Selection >) o);
			}
			
		}
		virtual public System.String DisplayText
		{
			get
			{
				StringBuilder b = new StringBuilder();
				
				for (int i = 0; i < vs.size(); i++)
				{
					Selection s = (Selection) vs.elementAt(i);
					b.append(s.Value);
					if (i < vs.size() - 1)
						b.append(", ");
				}
				
				return b.toString();
			}
			
		}
		
		List< Selection > vs; //vector of Selection
		
		/// <summary> Empty Constructor, necessary for dynamic construction during deserialization.
		/// Shouldn't be used otherwise.
		/// </summary>
		public SelectMultiData()
		{
			InitBlock();
		}
		
		
		public SelectMultiData(List< Selection > vs)
		
		Override
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.lang.Object.clone' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		public virtual System.Object Clone()
		{
			
			List< Selection > v = new List< Selection >();
			for (int i = 0; i < vs.size(); i++)
			{
				v.addElement(((Selection) vs.elementAt(i)).Clone());
			}
			return new SelectMultiData(v);
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.model.data.IAnswerData#setValue(java.lang.Object)
		*/
		
		Override
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.model.data.IAnswerData#getValue()
		*/
		
		Override
		
		/// <returns> A type checked vector containing all of the elements
		/// contained in the vector input
		/// TODO: move to utility class
		/// </returns>
		
		private List< Selection > vectorCopy(List< Selection > input)
		/// <returns> THE XMLVALUE!!
		/// </returns>
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.model.data.IAnswerData#getDisplayText()
		*/
		
		Override
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.utilities.Externalizable#readExternal(java.io.DataInputStream)
		*/
		
		SuppressWarnings(unchecked) 
		@ Override
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			
			vs =(List< Selection >) ExtUtil.read(in, new ExtWrapList(Selection.
		}
		
		class), pf);
	}
	
	/* (non-Javadoc)
	* @see org.javarosa.core.services.storage.utilities.Externalizable#writeExternal(java.io.DataOutputStream)
	*/
	
	Override
	
	public
	
	void writeExternal(DataOutputStream out) throws IOException
	
	{ 
		ExtUtil.write(out, new ExtWrapList(vs));
	
	} 
	
	@ Override
	
	public UncastData uncast()
	
	{ 
		Enumeration < Selection > en = vs.elements();
	
	StringBuilder selectString = new StringBuilder();
	
	
	while(en.hasMoreElements())
	
	{ 
		Selection selection =(Selection) en.nextElement();
	
	if(selectString.length() > 0) 
	selectString.append( );
	
	selectString.append(selection.getValue());
	
	}
	//As Crazy, and stupid, as it sounds, this is the XForms specification
	//for storing multiple selections.
	
	return new UncastData(selectString.toString());
	
	} 
	
	@ Override
	
	public SelectMultiData cast(UncastData data) throws IllegalArgumentException
	
	{ 
		List< Selection > v = new List< Selection >();
	
	
	List< String > choices = DateUtils.split(data.value,  , true);
	
	for(String s: choices)
	
	{ 
		v.addElement(new Selection(s));
	
	} 
	return new SelectMultiData(v);
	
	}
	
	}
}