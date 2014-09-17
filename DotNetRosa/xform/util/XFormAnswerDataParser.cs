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
using Constants = org.javarosa.core.model.Constants;
using QuestionDef = org.javarosa.core.model.QuestionDef;
using SelectChoice = org.javarosa.core.model.SelectChoice;
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
using SelectMultiData = org.javarosa.core.model.data.SelectMultiData;
using SelectOneData = org.javarosa.core.model.data.SelectOneData;
using StringData = org.javarosa.core.model.data.StringData;
using TimeData = org.javarosa.core.model.data.TimeData;
using UncastData = org.javarosa.core.model.data.UncastData;
using Selection = org.javarosa.core.model.data.helper.Selection;
using DateUtils = org.javarosa.core.model.utils.DateUtils;
namespace org.javarosa.xform.util
{
	
	/// <summary> The XFormAnswerDataParser is responsible for taking XForms elements and
	/// parsing them into a specific type of IAnswerData.
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	
	/*
	int
	text
	float
	datetime
	date
	time
	choice
	choice list*/
	
	public class XFormAnswerDataParser
	{
		//FIXME: the QuestionDef parameter is a hack until we find a better way to represent AnswerDatas for select questions
		
		public static IAnswerData getAnswerData(System.String text, int dataType)
		{
			return getAnswerData(text, dataType, null);
		}
		public static IAnswerData getAnswerData(System.String text, int dataType, QuestionDef q)
		{
			System.String trimmedText = text.Trim();
			if (trimmedText.Length == 0)
				trimmedText = null;
			
			switch (dataType)
			{
				
				case Constants.DATATYPE_NULL: 
				case Constants.DATATYPE_UNSUPPORTED: 
				case Constants.DATATYPE_TEXT: 
				case Constants.DATATYPE_BARCODE: 
				case Constants.DATATYPE_BINARY: 
					
					return new StringData(text);
				
				
				case Constants.DATATYPE_INTEGER: 
					
					try
					{
						return (trimmedText == null?null:new IntegerData(System.Int32.Parse(trimmedText)));
					}
					catch (System.FormatException nfe)
					{
						return null;
					}
					goto case Constants.DATATYPE_LONG;
				
				
				case Constants.DATATYPE_LONG: 
					
					try
					{
						return (trimmedText == null?null:new LongData(System.Int64.Parse(trimmedText)));
					}
					catch (System.FormatException nfe)
					{
						return null;
					}
					goto case Constants.DATATYPE_DECIMAL;
				
				
				case Constants.DATATYPE_DECIMAL: 
					
					try
					{
						return (trimmedText == null?null:new DecimalData(System.Double.Parse(trimmedText)));
					}
					catch (System.FormatException nfe)
					{
						return null;
					}
					goto case Constants.DATATYPE_CHOICE;
				
				
				case Constants.DATATYPE_CHOICE: 
					
					System.Collections.ArrayList selections = getSelections(text, q);
					return (selections.Count == 0?null:new SelectOneData((Selection) selections[0]));
				
				
				case Constants.DATATYPE_CHOICE_LIST: 
					
					return new SelectMultiData(getSelections(text, q));
				
				
				case Constants.DATATYPE_DATE_TIME: 
					
					//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
					System.DateTime dt = (trimmedText == null?null:DateUtils.parseDateTime(trimmedText));
					//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
					//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					return (dt == null?null:new DateTimeData(ref dt));
				
				
				case Constants.DATATYPE_DATE: 
					
					//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
					System.DateTime d = (trimmedText == null?null:DateUtils.parseDate(trimmedText));
					//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
					//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					return (d == null?null:new DateData(ref d));
				
				
				case Constants.DATATYPE_TIME: 
					
					//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
					System.DateTime t = (trimmedText == null?null:DateUtils.parseTime(trimmedText));
					//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
					//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					return (t == null?null:new TimeData(ref t));
				
				
				case Constants.DATATYPE_BOOLEAN: 
					
					if (trimmedText == null)
					{
						return null;
					}
					else
					{
						if (trimmedText.Equals("1"))
						{
							return new BooleanData(true);
						}
						if (trimmedText.Equals("0"))
						{
							return new BooleanData(false);
						}
						return trimmedText.Equals("t")?new BooleanData(true):new BooleanData(false);
					}
					goto case Constants.DATATYPE_GEOPOINT;
				
				
				case Constants.DATATYPE_GEOPOINT: 
					if (trimmedText == null)
					{
						return new GeoPointData();
					}
					
					try
					{
						UncastData uncast = new UncastData(trimmedText);
						// silly...
						GeoPointData gp = new GeoPointData();
						return gp.cast(uncast);
					}
					catch (System.Exception e)
					{
						return null;
					}
					goto case Constants.DATATYPE_GEOSHAPE;
				
				
				case Constants.DATATYPE_GEOSHAPE: 
					if (trimmedText == null)
					{
						return new GeoShapeData();
					}
					
					try
					{
						UncastData uncast = new UncastData(trimmedText);
						// silly...
						GeoShapeData gs = new GeoShapeData();
						return gs.cast(uncast);
					}
					catch (System.Exception e)
					{
						return null;
					}
					goto case Constants.DATATYPE_GEOTRACE;
				
				
				case Constants.DATATYPE_GEOTRACE: 
					if (trimmedText == null)
					{
						return new GeoTraceData();
					}
					
					try
					{
						UncastData uncast = new UncastData(trimmedText);
						// silly...
						GeoTraceData gl = new GeoTraceData();
						return gl.cast(uncast);
					}
					catch (System.Exception e)
					{
						return null;
					}
					goto default;
				
				
				default: 
					return new UncastData(trimmedText);
				
			}
		}
		
		private static System.Collections.ArrayList getSelections(System.String text, QuestionDef q)
		{
			System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			
			System.Collections.ArrayList choices = DateUtils.split(text, XFormAnswerDataSerializer.DELIMITER, true);
			for (int i = 0; i < choices.Count; i++)
			{
				Selection s = getSelection((System.String) choices[i], q);
				if (s != null)
					v.Add(s);
			}
			
			return v;
		}
		
		private static Selection getSelection(System.String choiceValue, QuestionDef q)
		{
			Selection s;
			
			if (q == null || q.DynamicChoices != null)
			{
				s = new Selection(choiceValue);
			}
			else
			{
				SelectChoice choice = q.getChoiceForValue(choiceValue);
				s = (choice != null?choice.selection():null);
			}
			
			return s;
		}
	}
}