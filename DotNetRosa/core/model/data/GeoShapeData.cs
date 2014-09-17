/*
* Copyright (C) 2014 JavaRosa
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
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using IExprDataType = org.javarosa.xpath.IExprDataType;
namespace org.javarosa.core.model.data
{
	
	
	/// <summary> A response to a question requesting an GeoShape Value.
	/// Consisting of a comma-separated ordered list of GeoPoint values.
	/// 
	/// GeoTrace data is an open sequence of geo-locations.
	/// GeoShape data is a closed sequence of geo-locations.
	/// 
	/// </summary>
	/// <author>  mitchellsundt@gmail.com
	/// 
	/// </author>
	public class GeoShapeData : IAnswerData, IExprDataType, System.ICloneable
	{
		virtual public System.String DisplayText
		{
			get
			{
				StringBuilder b = new StringBuilder();
				bool first = true;
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				for(GeoPointData p: points)
				{
					if (!first)
					{
						b.append("; ");
					}
					first = false;
					b.append(p.DisplayText);
				}
				return b.toString();
			}
			
		}
		virtual public System.Object Value
		{
			get
			{
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				ArrayList < double [] > pts = new ArrayList < double [] >();
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				for(GeoPointData p: points)
				{
					pts.add((double[]) p.Value);
				}
				GeoShape gs = new GeoShape(pts);
				return gs;
			}
			
			set
			{
				if (value == null)
				{
					throw new System.NullReferenceException("Attempt to set an IAnswerData class to null.");
				}
				if (!(value is GeoShape))
				{
					GeoShapeData t = new GeoShapeData();
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					GeoShapeData v = t.cast(new UncastData(value.ToString()));
					value = v.Value;
				}
				GeoShape gs = (GeoShape) value;
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				ArrayList < GeoPointData > temp = new ArrayList < GeoPointData >();
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				for(double [] da: gs.points)
				{
					temp.add(new GeoPointData(da));
				}
				points.clear();
				points.addAll(temp);
			}
			
		}
		
		/// <summary> The data value contained in a GeoShapeData object is a GeoShape
		/// 
		/// </summary>
		/// <author>  mitchellsundt@gmail.com
		/// 
		/// </author>
		public class GeoShape
		{
			private void  InitBlock()
			{
				this.points = points;
			}
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			public ArrayList < double [] > points;
			
			public GeoShape()
			{
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				points = new ArrayList < double [] >();
			}
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			public GeoShape(ArrayList < double [] > points)
		}
		
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public final ArrayList < GeoPointData > points = new ArrayList < GeoPointData >();
		
		
		/// <summary> Empty Constructor, necessary for dynamic construction during
		/// deserialization. Shouldn't be used otherwise.
		/// </summary>
		public GeoShapeData()
		{
		}
		
		/// <summary> Copy constructor (deep)
		/// 
		/// </summary>
		/// <param name="data">
		/// </param>
		public GeoShapeData(GeoShapeData data)
		{
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(GeoPointData p: data.points)
			{
				points.add(new GeoPointData(p));
			}
		}
		
		public GeoShapeData(GeoShape ashape)
		{
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(double [] da: ashape.points)
			{
				points.add(new GeoPointData(da));
			}
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.lang.Object.clone' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		public virtual System.Object Clone()
		{
			return new GeoShapeData(this);
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
			points.clear();
			int len = (int) ExtUtil.readNumeric(in_Renamed);
			for (int i = 0; i < len; ++i)
			{
				GeoPointData t = new GeoPointData();
				t.readExternal(in_Renamed, pf);
				points.add(t);
			}
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeNumeric(out_Renamed, points.size());
			for (int i = 0; i < points.size(); ++i)
			{
				GeoPointData t = points.get_Renamed(i);
				t.writeExternal(out_Renamed);
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
		public virtual GeoShapeData cast(UncastData data)
		{
			System.String[] parts = data.value_Renamed.split(";");
			
			// silly...
			GeoPointData t = new GeoPointData();
			
			GeoShapeData d = new GeoShapeData();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(String part: parts)
			{
				// allow for arbitrary surrounding whitespace
				d.points.add(t.cast(new UncastData(part.trim())));
			}
			return d;
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		public virtual System.Boolean toBoolean()
		{
			// return whether or not any Geopoints have been set
			if (points.size() == 0)
			{
				return false;
			}
			return true;
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		public virtual System.Double toNumeric()
		{
			if (points.size() == 0)
			{
				// we have no shape, so no accuracy...
				return GeoPointData.NO_ACCURACY_VALUE;
			}
			// return the worst accuracy...
			double maxValue = 0.0;
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(GeoPointData p: points)
			{
				maxValue = Math.max(maxValue, p.toNumeric());
			}
			return maxValue;
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Override
		public override System.String ToString()
		{
			return DisplayText;
		}
	}
}