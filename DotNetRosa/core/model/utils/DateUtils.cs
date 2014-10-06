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
using Localization = org.javarosa.core.services.locale.Localization;
using MathUtils = org.javarosa.core.util.MathUtils;
namespace org.javarosa.core.model.utils
{
	
	/// <summary> Static utility methods for Dates in j2me
	/// 
	/// </summary>
	/// <author>  Clayton Sims
	/// 
	/// </author>
	
	public class DateUtils
	{
		private void  InitBlock()
		{
			
			List< String > pieces = new List< String >();
			
			int index = str.indexOf(delimiter);
			while (index >= 0)
			{
				pieces.addElement(str.substring(0, index));
				str = str.substring(index + delimiter.length());
				index = str.indexOf(delimiter);
			}
			pieces.addElement(str);
			
			if (combineMultipleDelimiters)
			{
				for (int i = 0; i < pieces.size(); i++)
				{
					if (((System.String) pieces.elementAt(i)).Length == 0)
					{
						pieces.removeElementAt(i);
						i--;
					}
				}
			}
			
			return pieces;
		}
		//UPGRADE_NOTE: Final was removed from the declaration of 'MONTH_OFFSET '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_ISSUE: Field 'java.util.Calendar.JANUARY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilCalendarJANUARY_f'"
		private static readonly int MONTH_OFFSET = (1 - Calendar.JANUARY);
		
		public const int FORMAT_ISO8601 = 1;
		public const int FORMAT_HUMAN_READABLE_SHORT = 2;
		public const int FORMAT_HUMAN_READABLE_DAYS_FROM_TODAY = 5;
		//public static final int FORMAT_HUMAN_READABLE_LONG = 3;
		public const int FORMAT_TIMESTAMP_SUFFIX = 7;
		
		/// <summary>RFC 822 *</summary>
		public const int FORMAT_TIMESTAMP_HTTP = 9;
		
		public const long DAY_IN_MS = 86400000L;
		
		public DateUtils():base()
		{
			InitBlock();
		}
		
		public class DateFields
		{
			public DateFields()
			{
				year = 1970;
				month = 1;
				day = 1;
				hour = 0;
				minute = 0;
				second = 0;
				secTicks = 0;
				dow = 0;
				
				//			tzStr = "Z";
				//			tzOffset = 0;
			}
			
			public int year;
			public int month; //1-12
			public int day; //1-31
			public int hour; //0-23
			public int minute; //0-59
			public int second; //0-59
			public int secTicks; //0-999 (ms)
			
			/// <summary>NOTE: CANNOT BE USED TO SPECIFY A DATE *</summary>
			public int dow; //1-7;
			
			//		public String tzStr;
			//		public int tzOffset; //s ahead of UTC
			
			public virtual bool check()
			{
				return (org.javarosa.core.model.utils.DateUtils.inRange(month, 1, 12) && org.javarosa.core.model.utils.DateUtils.inRange(day, 1, org.javarosa.core.model.utils.DateUtils.daysInMonth(month - org.javarosa.core.model.utils.DateUtils.MONTH_OFFSET, year)) && org.javarosa.core.model.utils.DateUtils.inRange(hour, 0, 23) && org.javarosa.core.model.utils.DateUtils.inRange(minute, 0, 59) && org.javarosa.core.model.utils.DateUtils.inRange(second, 0, 59) && org.javarosa.core.model.utils.DateUtils.inRange(secTicks, 0, 999));
			}
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static DateFields getFields(ref System.DateTime d)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return getFields(ref d, null);
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static DateFields getFields(ref System.DateTime d, System.String timezone)
		{
			System.Globalization.Calendar cd = new System.Globalization.GregorianCalendar();
			//UPGRADE_TODO: The differences in the format  of parameters for method 'java.util.Calendar.setTime'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			SupportClass.CalendarManager.manager.SetDateTime(cd, d);
			if (timezone != null)
			{
				//UPGRADE_ISSUE: Method 'java.util.Calendar.setTimeZone' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilCalendarsetTimeZone_javautilTimeZone'"
				//UPGRADE_ISSUE: Method 'java.util.TimeZone.getTimeZone' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilTimeZonegetTimeZone_javalangString'"
				cd.setTimeZone(TimeZone.getTimeZone(timezone));
			}
			
			DateFields fields = new DateFields();
			//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
			fields.year = SupportClass.CalendarManager.manager.Get(cd, SupportClass.CalendarManager.YEAR);
			//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
			fields.month = SupportClass.CalendarManager.manager.Get(cd, SupportClass.CalendarManager.MONTH) + MONTH_OFFSET;
			//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
			fields.day = SupportClass.CalendarManager.manager.Get(cd, SupportClass.CalendarManager.DAY_OF_MONTH);
			//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
			fields.hour = SupportClass.CalendarManager.manager.Get(cd, SupportClass.CalendarManager.HOUR_OF_DAY);
			//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
			fields.minute = SupportClass.CalendarManager.manager.Get(cd, SupportClass.CalendarManager.MINUTE);
			//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
			fields.second = SupportClass.CalendarManager.manager.Get(cd, SupportClass.CalendarManager.SECOND);
			//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
			fields.secTicks = SupportClass.CalendarManager.manager.Get(cd, SupportClass.CalendarManager.MILLISECOND);
			//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
			fields.dow = SupportClass.CalendarManager.manager.Get(cd, SupportClass.CalendarManager.DAY_OF_WEEK);
			
			return fields;
		}
		
		public static System.DateTime getDate(DateFields f)
		{
			return getDate(f, null);
		}
		
		public static System.DateTime getDate(DateFields f, System.String timezone)
		{
			System.Globalization.Calendar cd = new System.Globalization.GregorianCalendar();
			if (timezone != null)
			{
				//UPGRADE_ISSUE: Method 'java.util.Calendar.setTimeZone' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilCalendarsetTimeZone_javautilTimeZone'"
				//UPGRADE_ISSUE: Method 'java.util.TimeZone.getTimeZone' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilTimeZonegetTimeZone_javalangString'"
				cd.setTimeZone(TimeZone.getTimeZone(timezone));
			}
			SupportClass.CalendarManager.manager.Set(cd, SupportClass.CalendarManager.YEAR, f.year);
			SupportClass.CalendarManager.manager.Set(cd, SupportClass.CalendarManager.MONTH, f.month - MONTH_OFFSET);
			SupportClass.CalendarManager.manager.Set(cd, SupportClass.CalendarManager.DAY_OF_MONTH, f.day);
			SupportClass.CalendarManager.manager.Set(cd, SupportClass.CalendarManager.HOUR_OF_DAY, f.hour);
			SupportClass.CalendarManager.manager.Set(cd, SupportClass.CalendarManager.MINUTE, f.minute);
			SupportClass.CalendarManager.manager.Set(cd, SupportClass.CalendarManager.SECOND, f.second);
			SupportClass.CalendarManager.manager.Set(cd, SupportClass.CalendarManager.MILLISECOND, f.secTicks);
			
			//UPGRADE_TODO: The equivalent in .NET for method 'java.util.Calendar.getTime' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			return SupportClass.CalendarManager.manager.GetDateTime(cd);
		}
		
		/* ==== FORMATTING DATES/TIMES TO STANDARD STRINGS ==== */
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static System.String formatDateTime(ref System.DateTime d, int format)
		{
			//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
			if (d == null)
			{
				return "";
			}
			
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			DateFields fields = getFields(ref d, format == FORMAT_TIMESTAMP_HTTP?"UTC":null);
			
			System.String delim;
			switch (format)
			{
				
				case FORMAT_ISO8601:  delim = "T"; break;
				
				case FORMAT_TIMESTAMP_SUFFIX:  delim = ""; break;
				
				case FORMAT_TIMESTAMP_HTTP:  delim = " "; break;
				
				default:  delim = " "; break;
				
			}
			
			return formatDate(fields, format) + delim + formatTime(fields, format);
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static System.String formatDate(ref System.DateTime d, int format)
		{
			//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return (d == null?"":formatDate(getFields(ref d, format == FORMAT_TIMESTAMP_HTTP?"UTC":null), format));
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static System.String formatTime(ref System.DateTime d, int format)
		{
			//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return (d == null?"":formatTime(getFields(ref d, format == FORMAT_TIMESTAMP_HTTP?"UTC":null), format));
		}
		
		private static System.String formatDate(DateFields f, int format)
		{
			switch (format)
			{
				
				case FORMAT_ISO8601:  return formatDateISO8601(f);
				
				case FORMAT_HUMAN_READABLE_SHORT:  return formatDateColloquial(f);
				
				case FORMAT_HUMAN_READABLE_DAYS_FROM_TODAY:  return formatDaysFromToday(f);
				
				case FORMAT_TIMESTAMP_SUFFIX:  return formatDateSuffix(f);
				
				case FORMAT_TIMESTAMP_HTTP:  return formatDateHttp(f);
				
				default:  return null;
				
			}
		}
		
		private static System.String formatTime(DateFields f, int format)
		{
			switch (format)
			{
				
				case FORMAT_ISO8601:  return formatTimeISO8601(f);
				
				case FORMAT_HUMAN_READABLE_SHORT:  return formatTimeColloquial(f);
				
				case FORMAT_TIMESTAMP_SUFFIX:  return formatTimeSuffix(f);
				
				case FORMAT_TIMESTAMP_HTTP:  return formatTimeHttp(f);
				
				default:  return null;
				
			}
		}
		
		/// <summary>RFC 822 *</summary>
		private static System.String formatDateHttp(DateFields f)
		{
			return format(f, "%a, %d %b %Y");
		}
		
		/// <summary>RFC 822 *</summary>
		private static System.String formatTimeHttp(DateFields f)
		{
			return format(f, "%H:%M:%S GMT");
		}
		
		private static System.String formatDateISO8601(DateFields f)
		{
			return f.year + "-" + intPad(f.month, 2) + "-" + intPad(f.day, 2);
		}
		
		private static System.String formatDateColloquial(DateFields f)
		{
			System.String year = ((System.Int32) f.year).ToString();
			
			//Normal Date
			if (year.Length == 4)
			{
				year = year.Substring(2, (4) - (2));
			}
			//Otherwise we have an old or bizzarre date, don't try to do anything
			
			return intPad(f.day, 2) + "/" + intPad(f.month, 2) + "/" + year;
		}
		
		private static System.String formatDateSuffix(DateFields f)
		{
			return f.year + intPad(f.month, 2) + intPad(f.day, 2);
		}
		
		private static System.String formatTimeISO8601(DateFields f)
		{
			System.String time = intPad(f.hour, 2) + ":" + intPad(f.minute, 2) + ":" + intPad(f.second, 2) + "." + intPad(f.secTicks, 3);
			
			//Time Zone ops (1 in the first field corresponds to 'CE' ERA)
			//UPGRADE_TODO: Method 'java.util.TimeZone.getOffset' was converted to 'System.TimeZone.GetUtcOffset' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			int offset = ((System.TimeZone.CurrentTimeZone.GetUtcOffset(new System.DateTime(f.year, f.month - 1, f.day)).Ticks) / 10000);
			
			//NOTE: offset is in millis
			if (offset == 0)
			{
				time += "Z";
			}
			else
			{
				
				//Start with sign
				System.String offsetSign = offset > 0?"+":"-";
				
				int value_Renamed = System.Math.Abs(offset) / 1000 / 60;
				
				System.String hrs = intPad(value_Renamed / 60, 2);
				System.String mins = value_Renamed % 60 != 0?":" + intPad(value_Renamed % 60, 2):"";
				
				time += (offsetSign + hrs + mins);
			}
			return time;
		}
		
		private static System.String formatTimeColloquial(DateFields f)
		{
			return intPad(f.hour, 2) + ":" + intPad(f.minute, 2);
		}
		
		private static System.String formatTimeSuffix(DateFields f)
		{
			return intPad(f.hour, 2) + intPad(f.minute, 2) + intPad(f.second, 2);
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static System.String format(ref System.DateTime d, System.String format)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return org.javarosa.core.model.utils.DateUtils.format(getFields(ref d), format);
		}
		
		public static System.String format(DateFields f, System.String format)
		{
			StringBuilder sb = new StringBuilder();
			
			for (int i = 0; i < format.Length; i++)
			{
				char c = format[i];
				
				if (c == '%')
				{
					i++;
					if (i >= format.Length)
					{
						throw new System.SystemException("date format string ends with %");
					}
					else
					{
						c = format[i];
					}
					
					if (c == '%')
					{
						//literal '%'
						sb.append("%");
					}
					else if (c == 'Y')
					{
						//4-digit year
						sb.append(intPad(f.year, 4));
					}
					else if (c == 'y')
					{
						//2-digit year
						sb.append(intPad(f.year, 4).Substring(2));
					}
					else if (c == 'm')
					{
						//0-padded month
						sb.append(intPad(f.month, 2));
					}
					else if (c == 'n')
					{
						//numeric month
						sb.append(f.month);
					}
					else if (c == 'b')
					{
						//short text month
						System.String[] months = new System.String[]{"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
						sb.append(months[f.month - 1]);
					}
					else if (c == 'd')
					{
						//0-padded day of month
						sb.append(intPad(f.day, 2));
					}
					else if (c == 'e')
					{
						//day of month
						sb.append(f.day);
					}
					else if (c == 'H')
					{
						//0-padded hour (24-hr time)
						sb.append(intPad(f.hour, 2));
					}
					else if (c == 'h')
					{
						//hour (24-hr time)
						sb.append(f.hour);
					}
					else if (c == 'M')
					{
						//0-padded minute
						sb.append(intPad(f.minute, 2));
					}
					else if (c == 'S')
					{
						//0-padded second
						sb.append(intPad(f.second, 2));
					}
					else if (c == '3')
					{
						//0-padded millisecond ticks (000-999)
						sb.append(intPad(f.secTicks, 3));
					}
					else if (c == 'a')
					{
						//Three letter short text day
						System.String[] dayNames = new System.String[]{"Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"};
						sb.append(dayNames[f.dow - 1]);
					}
					else if (c == 'Z' || c == 'A' || c == 'B')
					{
						throw new System.SystemException("unsupported escape in date format string [%" + c + "]");
					}
					else
					{
						throw new System.SystemException("unrecognized escape in date format string [%" + c + "]");
					}
				}
				else
				{
					sb.append(c);
				}
			}
			
			return sb.toString();
		}
		
		/* ==== PARSING DATES/TIMES FROM STANDARD STRINGS ==== */
		
		public static System.DateTime parseDateTime(System.String str)
		{
			DateFields fields = new DateFields();
			int i = str.IndexOf("T");
			if (i != - 1)
			{
				if (!parseDate(str.Substring(0, (i) - (0)), fields) || !parseTime(str.Substring(i + 1), fields))
				{
					//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
					return null;
				}
			}
			else
			{
				if (!parseDate(str, fields))
				{
					//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
					return null;
				}
			}
			return getDate(fields);
		}
		
		public static System.DateTime parseDate(System.String str)
		{
			DateFields fields = new DateFields();
			if (!parseDate(str, fields))
			{
				//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
				return null;
			}
			return getDate(fields);
		}
		
		public static System.DateTime parseTime(System.String str)
		{
			DateFields fields = new DateFields();
			if (!parseTime(str, fields))
			{
				//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
				return null;
			}
			return getDate(fields);
		}
		
		private static bool parseDate(System.String dateStr, DateFields f)
		{
			System.Collections.ArrayList pieces = split(dateStr, "-", false);
			if (pieces.Count != 3)
				return false;
			
			try
			{
				f.year = System.Int32.Parse((System.String) pieces[0]);
				f.month = System.Int32.Parse((System.String) pieces[1]);
				f.day = System.Int32.Parse((System.String) pieces[2]);
			}
			catch (System.FormatException nfe)
			{
				return false;
			}
			
			return f.check();
		}
		
		private static bool parseTime(System.String timeStr, DateFields f)
		{
			//get timezone information first. Make a Datefields set for the possible offset
			//NOTE: DO NOT DO DIRECT COMPUTATIONS AGAINST THIS. It's a holder for hour/minute
			//data only, but has data in other fields
			DateFields timeOffset = null;
			
			if (timeStr[timeStr.Length - 1] == 'Z')
			{
				//UTC!
				
				//Clean up string for later processing
				timeStr = timeStr.Substring(0, (timeStr.Length - 1) - (0));
				timeOffset = new DateFields();
			}
			else if (timeStr.IndexOf("+") != - 1 || timeStr.IndexOf("-") != - 1)
			{
				timeOffset = new DateFields();
				
				
				
				//We're going to add the Offset straight up to get UTC
				//so we need to invert the sign on the offset string
				int offsetSign = - 1;
				
				if (pieces.size() > 1)
				{
					//offsetSign is already correct
				}
				else
				{
					pieces = split(timeStr, "-", false);
					offsetSign = 1;
				}
				
				timeStr = pieces.elementAt(0);
				
				System.String offset = pieces.elementAt(1);
				System.String hours = offset;
				if (offset.IndexOf(":") != - 1)
				{
					
					hours = tzPieces.elementAt(0);
					int mins = Integer.parseInt(tzPieces.elementAt(1));
					timeOffset.minute = mins * offsetSign;
				}
				timeOffset.hour = System.Int32.Parse(hours) * offsetSign;
			}
			
			//Do the actual parse for the real time values;
			if (!parseRawTime(timeStr, f))
			{
				return false;
			}
			
			if (!(f.check()))
			{
				return false;
			}
			
			//Time is good, if there was no timezone info, just return that;
			if (timeOffset == null)
			{
				return true;
			}
			
			//Now apply any relevant offsets from the timezone.
			//UPGRADE_ISSUE: Method 'java.util.TimeZone.getTimeZone' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilTimeZonegetTimeZone_javalangString'"
			TimeZone.getTimeZone("UTC");
			System.Globalization.Calendar c = new System.Globalization.GregorianCalendar();
			
			//UPGRADE_TODO: The differences in the format  of parameters for method 'java.util.Calendar.setTime'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			//UPGRADE_TODO: Constructor 'java.util.Date.Date' was converted to 'System.DateTime.DateTime' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDateDate_long'"
			//UPGRADE_TODO: Method 'java.util.Date.getTime' was converted to 'System.DateTime.Ticks' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDategetTime'"
			SupportClass.CalendarManager.manager.SetDateTime(c, new System.DateTime(DateUtils.getDate(f, "UTC").Ticks + (((60 * timeOffset.hour) + timeOffset.minute) * 60 * 1000)));
			
			//c is now in the timezone of the parsed value, so put
			//it in the local timezone.
			
			//UPGRADE_ISSUE: Method 'java.util.Calendar.setTimeZone' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilCalendarsetTimeZone_javautilTimeZone'"
			c.setTimeZone(System.TimeZone.CurrentTimeZone);
			//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
			//UPGRADE_TODO: Field 'java.util.Calendar.HOUR' was converted to 'SupportClass.CalendarManager.HOUR' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarHOUR_f'"
			long four = SupportClass.CalendarManager.manager.Get(c, SupportClass.CalendarManager.HOUR);
			
			//UPGRADE_TODO: The equivalent in .NET for method 'java.util.Calendar.getTime' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			System.DateTime tempAux = SupportClass.CalendarManager.manager.GetDateTime(c);
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			DateFields adjusted = getFields(ref tempAux);
			
			f.hour = adjusted.hour;
			f.minute = adjusted.minute;
			f.second = adjusted.second;
			f.secTicks = adjusted.secTicks;
			
			return f.check();
		}
		
		/// <summary> Parse the raw components of time (hh:mm:ss) with no timezone information
		/// 
		/// </summary>
		/// <param name="timeStr">
		/// </param>
		/// <param name="f">
		/// </param>
		/// <returns>
		/// </returns>
		private static bool parseRawTime(System.String timeStr, DateFields f)
		{
			System.Collections.ArrayList pieces = split(timeStr, ":", false);
			if (pieces.Count != 2 && pieces.Count != 3)
				return false;
			
			try
			{
				f.hour = System.Int32.Parse((System.String) pieces[0]);
				f.minute = System.Int32.Parse((System.String) pieces[1]);
				
				if (pieces.Count == 3)
				{
					System.String secStr = (System.String) pieces[2];
					int i;
					for (i = 0; i < secStr.Length; i++)
					{
						char c = secStr[i];
						if (!System.Char.IsDigit(c) && c != '.')
							break;
					}
					secStr = secStr.Substring(0, (i) - (0));
					
					double fsec = System.Double.Parse(secStr);
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					f.second = (int) fsec;
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					f.secTicks = (int) (1000.0 * (fsec - f.second));
				}
			}
			catch (System.FormatException nfe)
			{
				return false;
			}
			
			return f.check();
		}
		
		
		/* ==== DATE UTILITY FUNCTIONS ==== */
		
		public static System.DateTime getDate(int year, int month, int day)
		{
			DateFields f = new DateFields();
			f.year = year;
			f.month = month;
			f.day = day;
			//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
			return (f.check()?getDate(f):null);
		}
		
		/// <summary> </summary>
		/// <returns> new Date object with same date but time set to midnight (in current timezone)
		/// </returns>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static System.DateTime roundDate(ref System.DateTime d)
		{
			//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
			if (d == null)
			{
				//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
				return null;
			}
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			DateFields f = getFields(ref d);
			return getDate(f.year, f.month, f.day);
		}
		
		public static System.DateTime today()
		{
			System.DateTime tempAux = System.DateTime.Now;
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return roundDate(ref tempAux);
		}
		
		/* ==== CALENDAR FUNCTIONS ==== */
		
		/// <summary> Returns the fractional time within the local day.
		/// 
		/// </summary>
		/// <param name="d">
		/// </param>
		/// <returns>
		/// </returns>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static double decimalTimeOfLocalDay(ref System.DateTime d)
		{
			//UPGRADE_TODO: Method 'java.util.Date.getTime' was converted to 'System.DateTime.Ticks' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDategetTime'"
			long milli = d.Ticks;
			// time is local time.
			// We want to obtain milliseconds from start of local day.
			// the Math.floor() function below will do milliseconds from
			// start of UTC day. Adjust back to UTC time-of-day.
			System.TimeZone generatedAux2 = System.TimeZone.CurrentTimeZone;
			System.Globalization.Calendar c = new System.Globalization.GregorianCalendar();
			//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
			//UPGRADE_ISSUE: Field 'java.util.Calendar.ZONE_OFFSET' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilCalendarZONE_OFFSET_f'"
			//UPGRADE_ISSUE: Field 'java.util.Calendar.DST_OFFSET' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilCalendarDST_OFFSET_f'"
			long milliOff = (SupportClass.CalendarManager.manager.Get(c, Calendar.ZONE_OFFSET) + SupportClass.CalendarManager.manager.Get(c, Calendar.DST_OFFSET));
			milli += milliOff;
			// and now convert to fractional day.
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			System.Double v = ((double) milli) / DAY_IN_MS;
			return v - Math.floor(v);
		}
		
		/// <summary> Returns the number of days in the month given for
		/// a given year.
		/// </summary>
		/// <param name="month">The month to be tested
		/// </param>
		/// <param name="year">The year in which the month is to be tested
		/// </param>
		/// <returns> the number of days in the given month on the given
		/// year.
		/// </returns>
		public static int daysInMonth(int month, int year)
		{
			//UPGRADE_ISSUE: Field 'java.util.Calendar.APRIL' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilCalendarAPRIL_f'"
			//UPGRADE_ISSUE: Field 'java.util.Calendar.JUNE' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilCalendarJUNE_f'"
			//UPGRADE_ISSUE: Field 'java.util.Calendar.SEPTEMBER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilCalendarSEPTEMBER_f'"
			//UPGRADE_ISSUE: Field 'java.util.Calendar.NOVEMBER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilCalendarNOVEMBER_f'"
			if (month == Calendar.APRIL || month == Calendar.JUNE || month == Calendar.SEPTEMBER || month == Calendar.NOVEMBER)
			{
				return 30;
			}
			else
			{
				//UPGRADE_ISSUE: Field 'java.util.Calendar.FEBRUARY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilCalendarFEBRUARY_f'"
				if (month == Calendar.FEBRUARY)
				{
					return 28 + (isLeap(year)?1:0);
				}
				else
				{
					return 31;
				}
			}
		}
		
		/// <summary> Determines whether a year is a leap year in the
		/// proleptic Gregorian calendar.
		/// 
		/// </summary>
		/// <param name="year">The year to be tested
		/// </param>
		/// <returns> True, if the year given is a leap year,
		/// false otherwise.
		/// </returns>
		public static bool isLeap(int year)
		{
			return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
		}
		
		
		/* ==== Parsing to Human Text ==== */
		
		/// <summary> Provides text representing a span of time.
		/// 
		/// </summary>
		/// <param name="f">The fields for the date to be compared against the current date.
		/// </param>
		/// <returns> a string which is a human readable representation of the difference between
		/// the provided date and the current date.
		/// </returns>
		private static System.String formatDaysFromToday(DateFields f)
		{
			System.String daysAgoStr = "";
			System.DateTime d = DateUtils.getDate(f);
			System.DateTime tempAux = System.DateTime.Now;
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			int daysAgo = DateUtils.daysSinceEpoch(ref tempAux) - DateUtils.daysSinceEpoch(ref d);
			
			if (daysAgo == 0)
			{
				return Localization.get_Renamed("date.today");
			}
			else if (daysAgo == 1)
			{
				return Localization.get_Renamed("date.yesterday");
			}
			else if (daysAgo == 2)
			{
				return Localization.get_Renamed("date.twoago", new System.String[]{System.Convert.ToString(daysAgo)});
			}
			else if (daysAgo > 2 && daysAgo <= 6)
			{
				return Localization.get_Renamed("date.nago", new System.String[]{System.Convert.ToString(daysAgo)});
			}
			else if (daysAgo == - 1)
			{
				return Localization.get_Renamed("date.tomorrow");
			}
			else if (daysAgo < - 1 && daysAgo >= - 6)
			{
				return Localization.get_Renamed("date.nfromnow", new System.String[]{System.Convert.ToString(- daysAgo)});
			}
			else
			{
				return DateUtils.formatDate(f, DateUtils.FORMAT_HUMAN_READABLE_SHORT);
			}
		}
		
		/* ==== DATE OPERATIONS ==== */
		
		/// <summary> Creates a Date object representing the amount of time between the
		/// reference date, and the given parameters.
		/// </summary>
		/// <param name="ref">The starting reference date
		/// </param>
		/// <param name="type">"week", or "month", representing the time period which is to be returned.
		/// </param>
		/// <param name="start">"sun", "mon", ... etc. representing the start of the time period.
		/// </param>
		/// <param name="beginning">true=return first day of period, false=return last day of period
		/// </param>
		/// <param name="includeToday">Whether to include the current date in the returned calculation
		/// </param>
		/// <param name="nAgo">How many periods ago. 1=most recent period, 0=period in progress
		/// </param>
		/// <returns> a Date object representing the amount of time between the
		/// reference date, and the given parameters.
		/// </returns>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static System.DateTime getPastPeriodDate(ref System.DateTime ref_Renamed, System.String type, System.String start, bool beginning, bool includeToday, int nAgo)
		{
			//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
			System.DateTime d = null;
			
			if (type.Equals("week"))
			{
				//1 week period
				//start: day of week that starts period
				//beginning: true=return first day of period, false=return last day of period
				//includeToday: whether today's date can count as the last day of the period
				//nAgo: how many periods ago; 1=most recent period, 0=period in progress
				
				int target_dow = - 1, current_dow = - 1, diff;
				int offset = (includeToday?1:0);
				
				if (start.Equals("sun"))
					target_dow = 0;
				else if (start.Equals("mon"))
					target_dow = 1;
				else if (start.Equals("tue"))
					target_dow = 2;
				else if (start.Equals("wed"))
					target_dow = 3;
				else if (start.Equals("thu"))
					target_dow = 4;
				else if (start.Equals("fri"))
					target_dow = 5;
				else if (start.Equals("sat"))
					target_dow = 6;
				
				if (target_dow == - 1)
					throw new System.SystemException();
				
				System.Globalization.Calendar cd = new System.Globalization.GregorianCalendar();
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.util.Calendar.setTime'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				SupportClass.CalendarManager.manager.SetDateTime(cd, ref_Renamed);
				
				//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
				switch (SupportClass.CalendarManager.manager.Get(cd, SupportClass.CalendarManager.DAY_OF_WEEK))
				{
					
					case (int) System.DayOfWeek.Sunday:  current_dow = 0; break;
					
					case (int) System.DayOfWeek.Monday:  current_dow = 1; break;
					
					case (int) System.DayOfWeek.Tuesday:  current_dow = 2; break;
					
					case (int) System.DayOfWeek.Wednesday:  current_dow = 3; break;
					
					case (int) System.DayOfWeek.Thursday:  current_dow = 4; break;
					
					case (int) System.DayOfWeek.Friday:  current_dow = 5; break;
					
					case (int) System.DayOfWeek.Saturday:  current_dow = 6; break;
					
					default:  throw new System.SystemException(); //something is wrong
					
				}
				
				diff = (((current_dow - target_dow) + (7 + offset)) % 7 - offset) + (7 * nAgo) - (beginning?0:6); //booyah
				//UPGRADE_TODO: Constructor 'java.util.Date.Date' was converted to 'System.DateTime.DateTime' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDateDate_long'"
				//UPGRADE_TODO: Method 'java.util.Date.getTime' was converted to 'System.DateTime.Ticks' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDategetTime'"
				d = new System.DateTime(ref_Renamed.Ticks - diff * DAY_IN_MS);
			}
			else if (type.Equals("month"))
			{
				//not supported
			}
			else
			{
				throw new System.ArgumentException();
			}
			
			return d;
		}
		
		/// <summary> Gets the number of months separating the two dates.</summary>
		/// <param name="earlierDate">The earlier date, chronologically
		/// </param>
		/// <param name="laterDate">The later date, chronologically
		/// </param>
		/// <returns> the number of months separating the two dates.
		/// </returns>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static int getMonthsDifference(ref System.DateTime earlierDate, ref System.DateTime laterDate)
		{
			//UPGRADE_TODO: Constructor 'java.util.Date.Date' was converted to 'System.DateTime.DateTime' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDateDate_long'"
			//UPGRADE_TODO: Method 'java.util.Date.getTime' was converted to 'System.DateTime.Ticks' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDategetTime'"
			System.DateTime span = new System.DateTime(laterDate.Ticks - earlierDate.Ticks);
			//UPGRADE_TODO: Constructor 'java.util.Date.Date' was converted to 'System.DateTime.DateTime' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDateDate_long'"
			System.DateTime firstDate = new System.DateTime(0);
			System.Globalization.Calendar calendar = new System.Globalization.GregorianCalendar();
			//UPGRADE_TODO: The differences in the format  of parameters for method 'java.util.Calendar.setTime'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			SupportClass.CalendarManager.manager.SetDateTime(calendar, firstDate);
			//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
			int firstYear = SupportClass.CalendarManager.manager.Get(calendar, SupportClass.CalendarManager.YEAR);
			//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
			int firstMonth = SupportClass.CalendarManager.manager.Get(calendar, SupportClass.CalendarManager.MONTH);
			
			//UPGRADE_TODO: The differences in the format  of parameters for method 'java.util.Calendar.setTime'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			SupportClass.CalendarManager.manager.SetDateTime(calendar, span);
			//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
			int spanYear = SupportClass.CalendarManager.manager.Get(calendar, SupportClass.CalendarManager.YEAR);
			//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
			int spanMonth = SupportClass.CalendarManager.manager.Get(calendar, SupportClass.CalendarManager.MONTH);
			int months = (spanYear - firstYear) * 12 + (spanMonth - firstMonth);
			return months;
		}
		
		/// <param name="date">the date object to be analyzed
		/// </param>
		/// <returns> The number of days (as a double precision floating point) since the Epoch
		/// </returns>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static int daysSinceEpoch(ref System.DateTime date)
		{
			System.DateTime tempAux = getDate(1970, 1, 1);
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return dateDiff(ref tempAux, ref date);
		}
		
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static System.Double fractionalDaysSinceEpoch(ref System.DateTime a)
		{
			//UPGRADE_TODO: Method 'java.util.Date.getTime' was converted to 'System.DateTime.Ticks' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDategetTime'"
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (double) ((a.Ticks - getDate(1970, 1, 1).Ticks) / (double) DAY_IN_MS);
		}
		
		/// <summary> add n days to date d
		/// 
		/// </summary>
		/// <param name="d">
		/// </param>
		/// <param name="n">
		/// </param>
		/// <returns>
		/// </returns>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static System.DateTime dateAdd(ref System.DateTime d, int n)
		{
			//UPGRADE_TODO: Constructor 'java.util.Date.Date' was converted to 'System.DateTime.DateTime' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDateDate_long'"
			//UPGRADE_TODO: Method 'java.util.Date.getTime' was converted to 'System.DateTime.Ticks' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDategetTime'"
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			System.DateTime tempAux = new System.DateTime(roundDate(ref d).Ticks + DAY_IN_MS * n + DAY_IN_MS / 2);
			return roundDate(ref tempAux);
			//half-day offset is needed to handle differing DST offsets!
		}
		
		/// <summary> return the number of days between a and b, positive if b is later than a
		/// 
		/// </summary>
		/// <param name="a">
		/// </param>
		/// <param name="b">
		/// </param>
		/// <returns> # days difference
		/// </returns>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static int dateDiff(ref System.DateTime a, ref System.DateTime b)
		{
			//UPGRADE_TODO: Method 'java.util.Date.getTime' was converted to 'System.DateTime.Ticks' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDategetTime'"
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return (int) MathUtils.divLongNotSuck(roundDate(ref b).Ticks - roundDate(ref a).Ticks + DAY_IN_MS / 2, DAY_IN_MS);
			//half-day offset is needed to handle differing DST offsets!
		}
		
		/* ==== UTILITY ==== */
		
		/// <summary> Tokenizes a string based on the given delimiter string</summary>
		/// <param name="original">The string to be split
		/// </param>
		/// <param name="delimiter">The delimeter to be used
		/// </param>
		/// <returns> An array of strings contained in original which were
		/// seperated by the delimeter
		/// </returns>
		
		public static List< String > split(String str, String delimiter, boolean combineMultipleDelimiters)
		
		/// <summary> Converts an integer to a string, ensuring that the string
		/// contains a certain number of digits
		/// </summary>
		/// <param name="n">The integer to be converted
		/// </param>
		/// <param name="pad">The length of the string to be returned
		/// </param>
		/// <returns> A string representing n, which has pad - #digits(n)
		/// 0's preceding the number.
		/// </returns>
		public static System.String intPad(int n, int pad)
		{
			System.String s = System.Convert.ToString(n);
			while (s.Length < pad)
				s = "0" + s;
			return s;
		}
		
		private static bool inRange(int x, int min, int max)
		{
			return (x >= min && x <= max);
		}
		
		/* ==== GARBAGE (backward compatibility; too lazy to remove them now) ==== */
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static System.String formatDateToTimeStamp(ref System.DateTime date)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return formatDateTime(ref date, FORMAT_ISO8601);
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static System.String getShortStringValue(ref System.DateTime val)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return formatDate(ref val, FORMAT_HUMAN_READABLE_SHORT);
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static System.String getXMLStringValue(ref System.DateTime val)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return formatDate(ref val, FORMAT_ISO8601);
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public static System.String get24HourTimeFromDate(ref System.DateTime d)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return formatTime(ref d, FORMAT_HUMAN_READABLE_SHORT);
		}
		
		public static System.DateTime getDateFromString(System.String value_Renamed)
		{
			return parseDate(value_Renamed);
		}
		
		public static System.DateTime getDateTimeFromString(System.String value_Renamed)
		{
			return parseDateTime(value_Renamed);
		}
		
		public static bool stringContains(System.String string_Renamed, System.String substring)
		{
			if (string_Renamed == null || substring == null)
			{
				return false;
			}
			if (string_Renamed.IndexOf(substring) == - 1)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}