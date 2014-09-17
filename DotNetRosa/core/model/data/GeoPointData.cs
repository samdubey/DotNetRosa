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
using DateUtils = org.javarosa.core.model.utils.DateUtils;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using IExprDataType = org.javarosa.xpath.IExprDataType;
namespace org.javarosa.core.model.data
{
	
	
	/// <summary> A response to a question requesting an GeoPoint Value.
	/// 
	/// Ensure that any missing values are reset to MISSING_VALUE
	/// This is currently 0.0, but perhaps should be NaN?
	/// 
	/// An uninitialized GeoPoint is:
	/// [0.0, 0.0, MISSING_VALUE, MISSING_VALUE]
	/// 
	/// </summary>
	/// <author>  mitchellsundt@gmail.com
	/// </author>
	/// <author>  Yaw Anokwa
	/// 
	/// </author>
	public class GeoPointData : IAnswerData, IExprDataType, System.ICloneable
	{
		virtual public System.String DisplayText
		{
			get
			{
				if (!toBoolean())
				{
					// it hasn't been set...
					return "";
				}
				StringBuilder b = new StringBuilder();
				for (int i = 0; i < len; i++)
				{
					b.append(gp[i]);
					if (i != len - 1)
					{
						b.append(" ");
					}
				}
				return b.toString();
			}
			
		}
		virtual public System.Object Value
		{
			get
			{
				// clone()'ing to prevent some potential bad direct accesses
				// when these values are returned by GeoLine or GeoShape objects.
				return gp.Clone();
			}
			
			set
			{
				if (value == null)
				{
					throw new System.NullReferenceException("Attempt to set an IAnswerData class to null.");
				}
				this.fillArray((double[]) value);
			}
			
		}
		
		public const int REQUIRED_ARRAY_SIZE = 2;
		public const double MISSING_VALUE = 0.0;
		// value to be reported if we never captured a datapoint
		public const double NO_ACCURACY_VALUE = 9999999.0;
		
		private double[] gp = new double[4];
		private int len = REQUIRED_ARRAY_SIZE;
		
		
		/// <summary> Empty Constructor, necessary for dynamic construction during
		/// deserialization. Shouldn't be used otherwise.
		/// </summary>
		public GeoPointData()
		{
			// reset missing data...
			for (int i = REQUIRED_ARRAY_SIZE; i < gp.Length; ++i)
			{
				this.gp[i] = MISSING_VALUE;
			}
		}
		
		public GeoPointData(GeoPointData gpd)
		{
			this.fillArray(gpd.gp);
		}
		
		public GeoPointData(double[] gp)
		{
			this.fillArray(gp);
		}
		
		
		private void  fillArray(double[] gp)
		{
			len = gp.Length;
			for (int i = 0; i < len; i++)
			{
				this.gp[i] = gp[i];
			}
			// make sure that any old data is removed...
			for (int i = len; i < gp.Length; ++i)
			{
				this.gp[i] = MISSING_VALUE;
			}
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.lang.Object.clone' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		public virtual System.Object Clone()
		{
			return new GeoPointData(gp);
		}
		
		
		/*
		* (non-Javadoc)
		*
		* @see org.javarosa.core.model.data.IAnswerData#getDisplayText()
		*/
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		
		
		/*
		* (non-Javadoc)
		*
		* @see org.javarosa.core.model.data.IAnswerData#getValue()
		*/
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			len = (int) ExtUtil.readNumeric(in_Renamed);
			for (int i = 0; i < len; i++)
			{
				gp[i] = ExtUtil.readDecimal(in_Renamed);
			}
			// make sure that any old data is removed...
			for (int i = len; i < gp.Length; ++i)
			{
				this.gp[i] = MISSING_VALUE;
			}
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeNumeric(out_Renamed, len);
			for (int i = 0; i < len; i++)
			{
				ExtUtil.writeDecimal(out_Renamed, gp[i]);
			}
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		public virtual UncastData uncast()
		{
			return new UncastData(DisplayText);
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		public virtual GeoPointData cast(UncastData data)
		{
			double[] ret = new double[4];
			// make sure that missing data is flagged as absent...
			for (int i = REQUIRED_ARRAY_SIZE; i < ret.Length; ++i)
			{
				ret[i] = MISSING_VALUE;
			}
			
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			int i2 = 0;
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(String s: choices)
			{
				double d = Double.parseDouble(s);
				ret[i2] = d;
				++i2;
			}
			return new GeoPointData(ret);
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		public virtual System.Boolean toBoolean()
		{
			// return whether or not the Geopoint has been set
			return (gp[0] != 0.0 || gp[1] != 0.0 || gp[2] != 0.0 || gp[3] != 0.0);
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		public virtual System.Double toNumeric()
		{
			// return accuracy...
			if (!toBoolean())
			{
				// we have no captured geopoint...
				// bigger than the radius of the earth (meters)...
				return NO_ACCURACY_VALUE;
			}
			return gp[3];
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		public override System.String ToString()
		{
			return DisplayText;
		}
	}
}