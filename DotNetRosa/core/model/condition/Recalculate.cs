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
using FormDef = org.javarosa.core.model.FormDef;
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
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using XPathException = org.javarosa.xpath.XPathException;
namespace org.javarosa.core.model.condition
{
	
	public class Recalculate:Triggerable
	{
		public Recalculate()
		{
		}
		
		public Recalculate(IConditionExpr expr, TreeReference contextRef):base(expr, contextRef)
		{
		}
		
		public Recalculate(IConditionExpr expr, TreeReference target, TreeReference contextRef):base(expr, contextRef)
		{
			addTarget(target);
		}
		
		public override System.Object eval(FormInstance model, EvaluationContext ec)
		{
			try
			{
				return expr.evalRaw(model, ec);
			}
			catch (XPathException e)
			{
				e.setSource("calculate expression for " + contextRef.toString(true));
				throw e;
			}
		}
		
		public override void  apply(TreeReference ref_Renamed, System.Object result, FormInstance model, FormDef f)
		{
			int dataType = f.MainInstance.resolveReference(ref_Renamed).getDataType();
			f.setAnswer(wrapData(result, dataType), ref_Renamed);
		}
		
		public override bool canCascade()
		{
			return true;
		}
		
		public  override bool Equals(System.Object o)
		{
			if (o is Recalculate)
			{
				Recalculate r = (Recalculate) o;
				if (this == r)
					return true;
				
				return base.Equals(r);
			}
			else
			{
				return false;
			}
		}
		
		//droos 1/29/10: we need to come up with a consistent rule for whether the resulting data is determined
		//by the type of the instance node, or the type of the expression result. right now it's a mix and a mess
		//note a caveat with going solely by instance node type is that untyped nodes default to string!
		
		//for now, these are the rules:
		// if node type == bool, convert to boolean (for numbers, zero = f, non-zero = t; empty string = f, all other datatypes -> error)
		// if numeric data, convert to int if node type is int OR data is an integer; else convert to double
		// if string data or date data, keep as is
		// if NaN or empty string, null
		/// <summary> convert the data object returned by the xpath expression into an IAnswerData suitable for
		/// storage in the FormInstance
		/// 
		/// </summary>
		public static IAnswerData wrapData(System.Object val, int dataType)
		{
			if ((val is System.String && ((System.String) val).Length == 0) || (val is System.Double && System.Double.IsNaN(((System.Double) val))))
			{
				return null;
			}
			
			if (Constants.DATATYPE_BOOLEAN == dataType || val is System.Boolean)
			{
				//ctsims: We should really be using the boolean datatype for real, it's
				//necessary for backend calculations and XSD compliance
				
				bool b;
				
				if (val is System.Boolean)
				{
					b = ((System.Boolean) val);
				}
				else if (val is System.Double)
				{
					System.Double d = (System.Double) val;
					b = System.Math.Abs(d) > 1.0e-12 && !Double.isNaN(d);
				}
				else if (val is System.String)
				{
					System.String s = (System.String) val;
					b = s.Length > 0;
				}
				else
				{
					throw new System.SystemException("unrecognized data representation while trying to convert to BOOLEAN");
				}
				
				return new BooleanData(b);
			}
			else if (val is System.Double)
			{
				double d = ((System.Double) val);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				long l = (long) d;
				bool isIntegral = System.Math.Abs(d - l) < 1.0e-9;
				if (Constants.DATATYPE_INTEGER == dataType || (isIntegral && (System.Int32.MaxValue >= l) && (System.Int32.MinValue <= l)))
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					return new IntegerData((int) d);
				}
				else if (Constants.DATATYPE_LONG == dataType || isIntegral)
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					return new LongData((long) d);
				}
				else
				{
					return new DecimalData(d);
				}
			}
			else if (dataType == Constants.DATATYPE_GEOPOINT)
			{
				return new GeoPointData().cast(new UncastData(System.Convert.ToString(val)));
			}
			else if (dataType == Constants.DATATYPE_GEOSHAPE)
			{
				return new GeoShapeData().cast(new UncastData(System.Convert.ToString(val)));
			}
			else if (dataType == Constants.DATATYPE_GEOTRACE)
			{
				return new GeoTraceData().cast(new UncastData(System.Convert.ToString(val)));
			}
			else if (dataType == Constants.DATATYPE_CHOICE)
			{
				return new SelectOneData().cast(new UncastData(System.Convert.ToString(val)));
			}
			else if (dataType == Constants.DATATYPE_CHOICE_LIST)
			{
				return new SelectMultiData().cast(new UncastData(System.Convert.ToString(val)));
			}
			else if (val is System.String)
			{
				return new StringData((System.String) val);
			}
			else if (val is System.DateTime)
			{
				if (dataType == Constants.DATATYPE_DATE_TIME)
				{
					//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					return new DateTimeData(ref new System.DateTime[]{(System.DateTime) val}[0]);
				}
				else if (dataType == Constants.DATATYPE_TIME)
				{
					//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					return new TimeData(ref new System.DateTime[]{(System.DateTime) val}[0]);
				}
				else
				{
					//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					return new DateData(ref new System.DateTime[]{(System.DateTime) val}[0]);
				}
			}
			else
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new System.SystemException("unrecognized data type in 'calculate' expression: " + val.GetType().FullName);
			}
		}
		//UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}