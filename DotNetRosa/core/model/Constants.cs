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
namespace org.javarosa.core.model
{
	
	
	/// <summary> Constants shared throught classes in the containing package.
	/// 
	/// </summary>
	/// <version>  ,
	/// </version>
	public class Constants
	{
		
		/// <summary>Empty strig representation </summary>
		public const System.String EMPTY_STRING = "";
		
		/// <summary>Index for no selection </summary>
		public const int NO_SELECTION = - 1;
		
		/// <summary>ID not set to a value </summary>
		public const int NULL_ID = - 1;
		
		/// <summary>Connection type not specified </summary>
		public const int CONNECTION_NONE = 0;
		
		/// <summary>Infrared connection </summary>
		public const int CONNECTION_INFRARED = 1;
		
		/// <summary>Bluetooth connection </summary>
		public const int CONNECTION_BLUETOOTH = 2;
		
		/// <summary>Data cable connection. Can be USB or Serial </summary>
		public const int CONNECTION_CABLE = 3;
		
		/// <summary>Over The Air or HTTP Connection </summary>
		public const int CONNECTION_OTA = 4;
		
		public const int DATATYPE_UNSUPPORTED = - 1;
		public const int DATATYPE_NULL = 0; /* for nodes that have no data, or data type otherwise unknown */
		public const int DATATYPE_TEXT = 1; /// <summary>Text question type. </summary>
		public const int DATATYPE_INTEGER = 2; /// <summary>Numeric question type. These are numbers without decimal points</summary>
		public const int DATATYPE_DECIMAL = 3; /// <summary>Decimal question type. These are numbers with decimals </summary>
		public const int DATATYPE_DATE = 4; /// <summary>Date question type. This has only date component without time. </summary>
		public const int DATATYPE_TIME = 5; /// <summary>Time question type. This has only time element without date</summary>
		public const int DATATYPE_DATE_TIME = 6; /// <summary>Date and Time question type. This has both the date and time components</summary>
		public const int DATATYPE_CHOICE = 7; /// <summary>This is a question with alist of options where not more than one option can be selected at a time. </summary>
		public const int DATATYPE_CHOICE_LIST = 8; /// <summary>This is a question with alist of options where more than one option can be selected at a time. </summary>
		public const int DATATYPE_BOOLEAN = 9; /// <summary>Question with true and false answers. </summary>
		public const int DATATYPE_GEOPOINT = 10; /// <summary>Question with location answer. </summary>
		public const int DATATYPE_BARCODE = 11; /// <summary>Question with barcode string answer. </summary>
		public const int DATATYPE_BINARY = 12; /// <summary>Question with external binary answer. </summary>
		public const int DATATYPE_LONG = 13; /// <summary>Question with external binary answer. </summary>
		public const int DATATYPE_GEOSHAPE = 14; /// <summary>Question with GeoShape answer. </summary>
		public const int DATATYPE_GEOTRACE = 15; /// <summary>Question with GeoTrace answer. </summary>
		
		public const int CONTROL_UNTYPED = - 1;
		public const int CONTROL_INPUT = 1;
		public const int CONTROL_SELECT_ONE = 2;
		public const int CONTROL_SELECT_MULTI = 3;
		public const int CONTROL_TEXTAREA = 4;
		public const int CONTROL_SECRET = 5;
		public const int CONTROL_RANGE = 6;
		public const int CONTROL_UPLOAD = 7;
		public const int CONTROL_SUBMIT = 8;
		public const int CONTROL_TRIGGER = 9;
		public const int CONTROL_IMAGE_CHOOSE = 10;
		public const int CONTROL_LABEL = 11;
		public const int CONTROL_AUDIO_CAPTURE = 12;
		public const int CONTROL_VIDEO_CAPTURE = 13;
		
		/// <summary>constants for xform tags </summary>
		public const System.String XFTAG_UPLOAD = "upload";
	}
}