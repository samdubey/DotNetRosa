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
using IAnswerDataSerializer = org.javarosa.core.model.IAnswerDataSerializer;
using BooleanData = org.javarosa.core.model.data.BooleanData;
using DateData = org.javarosa.core.model.data.DateData;
using DateTimeData = org.javarosa.core.model.data.DateTimeData;
using DecimalData = org.javarosa.core.model.data.DecimalData;
using GeoTraceData = org.javarosa.core.model.data.GeoTraceData;
using GeoPointData = org.javarosa.core.model.data.GeoPointData;
using GeoShapeData = org.javarosa.core.model.data.GeoShapeData;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using IntegerData = org.javarosa.core.model.data.IntegerData;
using LongData = org.javarosa.core.model.data.LongData;
using MultiPointerAnswerData = org.javarosa.core.model.data.MultiPointerAnswerData;
using PointerAnswerData = org.javarosa.core.model.data.PointerAnswerData;
using SelectMultiData = org.javarosa.core.model.data.SelectMultiData;
using SelectOneData = org.javarosa.core.model.data.SelectOneData;
using StringData = org.javarosa.core.model.data.StringData;
using TimeData = org.javarosa.core.model.data.TimeData;
using UncastData = org.javarosa.core.model.data.UncastData;
using Selection = org.javarosa.core.model.data.helper.Selection;
using DateUtils = org.javarosa.core.model.utils.DateUtils;
//UPGRADE_TODO: The type 'org.kxml2.kdom.Element' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Element = org.kxml2.kdom.Element;
namespace org.javarosa.xform.util
{
	
	/// <summary> The XFormAnswerDataSerializer takes in AnswerData objects, and provides
	/// an XForms compliant (String or Element) representation of that AnswerData.
	/// 
	/// By default, this serializer can properly operate on StringData, DateData
	/// SelectMultiData, and SelectOneData AnswerData objects. This list can be
	/// extended by registering appropriate XForm serializing AnswerDataSerializers
	/// with this class.
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	public class XFormAnswerDataSerializer : IAnswerDataSerializer
	{
		
		public const System.String DELIMITER = " ";
		
		internal System.Collections.ArrayList additionalSerializers = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
		
		public virtual void  registerAnswerSerializer(IAnswerDataSerializer ads)
		{
			additionalSerializers.Add(ads);
		}
		
		public virtual bool canSerialize(IAnswerData data)
		{
			if (data is StringData || data is DateData || data is TimeData || data is SelectMultiData || data is SelectOneData || data is IntegerData || data is DecimalData || data is PointerAnswerData || data is MultiPointerAnswerData || data is GeoPointData || data is GeoTraceData || data is GeoShapeData || data is LongData || data is DateTimeData || data is UncastData)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		/// <param name="data">The AnswerDataObject to be serialized
		/// </param>
		/// <returns> A String which contains the given answer
		/// </returns>
		public virtual System.Object serializeAnswerData(UncastData data)
		{
			return data.String;
		}
		
		
		/// <param name="data">The AnswerDataObject to be serialized
		/// </param>
		/// <returns> A String which contains the given answer
		/// </returns>
		public virtual System.Object serializeAnswerData(StringData data)
		{
			return (System.String) data.Value;
		}
		
		/// <param name="data">The AnswerDataObject to be serialized
		/// </param>
		/// <returns> A String which contains a date in xsd:date
		/// formatting
		/// </returns>
		public virtual System.Object serializeAnswerData(DateData data)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return DateUtils.formatDate(ref new System.DateTime[]{(System.DateTime) data.Value}[0], DateUtils.FORMAT_ISO8601);
		}
		
		/// <param name="data">The AnswerDataObject to be serialized
		/// </param>
		/// <returns> A String which contains a date in xsd:date
		/// formatting
		/// </returns>
		public virtual System.Object serializeAnswerData(DateTimeData data)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return DateUtils.formatDateTime(ref new System.DateTime[]{(System.DateTime) data.Value}[0], DateUtils.FORMAT_ISO8601);
		}
		
		/// <param name="data">The AnswerDataObject to be serialized
		/// </param>
		/// <returns> A String which contains a date in xsd:time
		/// formatting
		/// </returns>
		public virtual System.Object serializeAnswerData(TimeData data)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return DateUtils.formatTime(ref new System.DateTime[]{(System.DateTime) data.Value}[0], DateUtils.FORMAT_ISO8601);
		}
		
		/// <param name="data">The AnswerDataObject to be serialized
		/// </param>
		/// <returns> A String which contains a reference to the
		/// data
		/// </returns>
		public virtual System.Object serializeAnswerData(PointerAnswerData data)
		{
			//Note: In order to override this default behavior, a
			//new serializer should be used, and then registered
			//with this serializer
			IDataPointer pointer = (IDataPointer) data.Value;
			return pointer.DisplayText;
		}
		
		/// <param name="data">The AnswerDataObject to be serialized
		/// </param>
		/// <returns> A String which contains a reference to the
		/// data
		/// </returns>
		public virtual System.Object serializeAnswerData(MultiPointerAnswerData data)
		{
			//Note: In order to override this default behavior, a
			//new serializer should be used, and then registered
			//with this serializer
			IDataPointer[] pointers = (IDataPointer[]) data.Value;
			if (pointers.Length == 1)
			{
				return pointers[0].DisplayText;
			}
			Element parent = new Element();
			for (int i = 0; i < pointers.Length; ++i)
			{
				Element datael = new Element();
				datael.setName("data");
				
				datael.addChild(Element.TEXT, pointers[i].DisplayText);
				parent.addChild(Element.ELEMENT, datael);
			}
			return parent;
		}
		
		/// <param name="data">The AnswerDataObject to be serialized
		/// </param>
		/// <returns> A string containing the xforms compliant format
		/// for a <select> tag, a string containing a list of answers
		/// separated by space characters.
		/// </returns>
		public virtual System.Object serializeAnswerData(SelectMultiData data)
		{
			System.Collections.ArrayList selections = (System.Collections.ArrayList) data.Value;
			System.Collections.IEnumerator en = selections.GetEnumerator();
			StringBuilder selectString = new StringBuilder();
			
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			while (en.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				Selection selection = (Selection) en.Current;
				if (selectString.length() > 0)
					selectString.append(DELIMITER);
				selectString.append(selection.Value);
			}
			//As Crazy, and stupid, as it sounds, this is the XForms specification
			//for storing multiple selections.
			return selectString.toString();
		}
		
		/// <param name="data">The AnswerDataObject to be serialized
		/// </param>
		/// <returns> A String which contains the value of a selection
		/// </returns>
		public virtual System.Object serializeAnswerData(SelectOneData data)
		{
			return ((Selection) data.Value).Value;
		}
		
		public virtual System.Object serializeAnswerData(IntegerData data)
		{
			return ((System.Int32) data.Value).ToString();
		}
		
		public virtual System.Object serializeAnswerData(LongData data)
		{
			return ((System.Int64) data.Value).ToString();
		}
		
		public virtual System.Object serializeAnswerData(DecimalData data)
		{
			return ((System.Double) data.Value).ToString();
		}
		
		public virtual System.Object serializeAnswerData(GeoPointData data)
		{
			return data.DisplayText;
		}
		
		public virtual System.Object serializeAnswerData(GeoTraceData data)
		{
			return data.DisplayText;
		}
		
		public virtual System.Object serializeAnswerData(GeoShapeData data)
		{
			return data.DisplayText;
		}
		
		public virtual System.Object serializeAnswerData(BooleanData data)
		{
			if (((System.Boolean) data.Value))
			{
				return "1";
			}
			else
			{
				return "0";
			}
		}
		
		public virtual System.Object serializeAnswerData(IAnswerData data, int dataType)
		{
			// First, we want to go through the additional serializers, as they should
			// take priority to the default serializations
			System.Collections.IEnumerator en = additionalSerializers.GetEnumerator();
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			while (en.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				IAnswerDataSerializer serializer = (IAnswerDataSerializer) en.Current;
				if (serializer.canSerialize(data))
				{
					return serializer.serializeAnswerData(data, dataType);
				}
			}
			//Defaults
			System.Object result = serializeAnswerData(data);
			return result;
		}
		
		public virtual System.Object serializeAnswerData(IAnswerData data)
		{
			if (data is StringData)
			{
				return serializeAnswerData((StringData) data);
			}
			else if (data is SelectMultiData)
			{
				return serializeAnswerData((SelectMultiData) data);
			}
			else if (data is SelectOneData)
			{
				return serializeAnswerData((SelectOneData) data);
			}
			else if (data is IntegerData)
			{
				return serializeAnswerData((IntegerData) data);
			}
			else if (data is LongData)
			{
				return serializeAnswerData((LongData) data);
			}
			else if (data is DecimalData)
			{
				return serializeAnswerData((DecimalData) data);
			}
			else if (data is DateData)
			{
				return serializeAnswerData((DateData) data);
			}
			else if (data is TimeData)
			{
				return serializeAnswerData((TimeData) data);
			}
			else if (data is PointerAnswerData)
			{
				return serializeAnswerData((PointerAnswerData) data);
			}
			else if (data is MultiPointerAnswerData)
			{
				return serializeAnswerData((MultiPointerAnswerData) data);
			}
			else if (data is GeoShapeData)
			{
				return serializeAnswerData((GeoShapeData) data);
			}
			else if (data is GeoTraceData)
			{
				return serializeAnswerData((GeoTraceData) data);
			}
			else if (data is GeoPointData)
			{
				return serializeAnswerData((GeoPointData) data);
			}
			else if (data is DateTimeData)
			{
				return serializeAnswerData((DateTimeData) data);
			}
			else if (data is BooleanData)
			{
				return serializeAnswerData((BooleanData) data);
			}
			else if (data is UncastData)
			{
				return serializeAnswerData((UncastData) data);
			}
			
			return null;
		}
		
		/*
		* (non-Javadoc)
		* @see org.javarosa.core.model.IAnswerDataSerializer#containsExternalData(org.javarosa.core.model.data.IAnswerData)
		*/
		public virtual System.Boolean containsExternalData(IAnswerData data)
		{
			//First check for registered serializers to identify whether
			//they override this one.
			System.Collections.IEnumerator en = additionalSerializers.GetEnumerator();
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			while (en.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				IAnswerDataSerializer serializer = (IAnswerDataSerializer) en.Current;
				System.Boolean contains = serializer.containsExternalData(data);
				//UPGRADE_TODO: The 'System.Boolean' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
				if (contains != null)
				{
					return contains;
				}
			}
			if (data is PointerAnswerData || data is MultiPointerAnswerData)
			{
				return true;
			}
			return false;
		}
		
		public virtual IDataPointer[] retrieveExternalDataPointer(IAnswerData data)
		{
			System.Collections.IEnumerator en = additionalSerializers.GetEnumerator();
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			while (en.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				IAnswerDataSerializer serializer = (IAnswerDataSerializer) en.Current;
				System.Boolean contains = serializer.containsExternalData(data);
				//UPGRADE_TODO: The 'System.Boolean' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
				if (contains != null)
				{
					return serializer.retrieveExternalDataPointer(data);
				}
			}
			if (data is PointerAnswerData)
			{
				IDataPointer[] pointer = new IDataPointer[1];
				pointer[0] = (IDataPointer) ((PointerAnswerData) data).Value;
				return pointer;
			}
			else if (data is MultiPointerAnswerData)
			{
				return (IDataPointer[]) ((MultiPointerAnswerData) data).Value;
			}
			//This shouldn't have been called.
			return null;
		}
	}
}