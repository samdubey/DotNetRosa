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
//UPGRADE_TODO: The type 'java.util.regex.Pattern' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Pattern = java.util.regex.Pattern;
using EvaluationContext = org.javarosa.core.model.condition.EvaluationContext;
using IFunctionHandler = org.javarosa.core.model.condition.IFunctionHandler;
using UnpivotableExpressionException = org.javarosa.core.model.condition.pivot.UnpivotableExpressionException;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using DateUtils = org.javarosa.core.model.utils.DateUtils;
using PropertyManager = org.javarosa.core.services.PropertyManager;
using MathUtils = org.javarosa.core.util.MathUtils;
using PropertyUtils = org.javarosa.core.util.PropertyUtils;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapListPoly = org.javarosa.core.util.externalizable.ExtWrapListPoly;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using IExprDataType = org.javarosa.xpath.IExprDataType;
using XPathNodeset = org.javarosa.xpath.XPathNodeset;
using XPathTypeMismatchException = org.javarosa.xpath.XPathTypeMismatchException;
using XPathUnhandledException = org.javarosa.xpath.XPathUnhandledException;
namespace org.javarosa.xpath.expr
{
	
	/// <summary> Representation of an xpath function expression.
	/// 
	/// All of the built-in xpath functions are included here, as well as the xpath type conversion logic
	/// 
	/// Evaluation of functions can delegate out to custom function handlers that must be registered at
	/// runtime.
	/// 
	/// </summary>
	/// <author>  Drew Roos
	/// 
	/// </author>
	public class XPathFuncExpr:XPathExpression
	{
		private void  InitBlock()
		{
			System.String name = id.ToString();
			
			//for now we'll assume that all that functions do is return the composition of their components
			System.Object[] argVals = new System.Object[args.Length];
			
			
			//Identify whether this function is an identity: IE: can reflect back the pivot sentinal with no modification
			System.String[] identities = new System.String[]{"string-length"};
			bool id = false;
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(String identity: identities)
			{
				if (identity.equals(name))
				{
					id = true;
				}
			}
			
			//get each argument's pivot
			for (int i = 0; i < args.Length; i++)
			{
				argVals[i] = args[i].pivot(model, evalContext, pivots, sentinal);
			}
			
			bool pivoted = false;
			//evaluate the pivots
			for (int i = 0; i < argVals.Length; ++i)
			{
				if (argVals[i] == null)
				{
					//one of our arguments contained pivots,
					pivoted = true;
				}
				else if (sentinal.equals(argVals[i]))
				{
					//one of our arguments is the sentinal, return the sentinal if possible
					if (id)
					{
						return sentinal;
					}
					else
					{
						//This function modifies the sentinal in a way that makes it impossible to capture
						//the pivot.
						throw new UnpivotableExpressionException();
					}
				}
			}
			
			if (pivoted)
			{
				if (id)
				{
					return null;
				}
				else
				{
					//This function modifies the sentinal in a way that makes it impossible to capture
					//the pivot.
					throw new UnpivotableExpressionException();
				}
			}
			
			//TODO: Inner eval here with eval'd args to improve speed
			return eval(model, evalContext);
		}
		public XPathQName id; //name of the function
		public XPathExpression[] args; //argument list
		
		public XPathFuncExpr()
		{
			InitBlock();
		} //for deserialization
		
		public XPathFuncExpr(XPathQName id, XPathExpression[] args)
		{
			InitBlock();
			this.id = id;
			this.args = args;
		}
		
		public override System.String ToString()
		{
			StringBuilder sb = new StringBuilder();
			
			sb.append("{func-expr:");
			sb.append(id.ToString());
			sb.append(",{");
			for (int i = 0; i < args.Length; i++)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				sb.append(args[i].ToString());
				if (i < args.Length - 1)
					sb.append(",");
			}
			sb.append("}}");
			
			return sb.toString();
		}
		
		public  override bool Equals(System.Object o)
		{
			if (o is XPathFuncExpr)
			{
				XPathFuncExpr x = (XPathFuncExpr) o;
				
				//Shortcuts for very easily comprable values
				//We also only return "True" for methods we expect to return the same thing. This is not good
				//practice in Java, since o.equals(o) will return false. We should evaluate that differently.
				//Dec 8, 2011 - Added "uuid", since we should never assume one uuid equals another
				//May 6, 2013 - Added "random", since two calls asking for a random
				//Jun 4, 2013 - Added "now" and "today", since these could change during the course of a survey
				if (!id.Equals(x.id) || args.Length != x.args.Length || id.ToString().Equals("uuid") || id.ToString().Equals("random") || id.ToString().Equals("once") || id.ToString().Equals("now") || id.ToString().Equals("today"))
				{
					return false;
				}
				
				return ExtUtil.arrayEquals(args, x.args);
			}
			else
			{
				return false;
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			id = (XPathQName) ExtUtil.read(in_Renamed, typeof(XPathQName));
			System.Collections.ArrayList v = (System.Collections.ArrayList) ExtUtil.read(in_Renamed, new ExtWrapListPoly(), pf);
			
			args = new XPathExpression[v.Count];
			for (int i = 0; i < args.Length; i++)
				args[i] = (XPathExpression) v[i];
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			for (int i = 0; i < args.Length; i++)
				v.Add(args[i]);
			
			ExtUtil.write(out_Renamed, id);
			ExtUtil.write(out_Renamed, new ExtWrapListPoly(v));
		}
		
		/// <summary> Evaluate the function call.
		/// 
		/// First check if the function is a member of the built-in function suite. If not, then check
		/// for any custom handlers registered to handler the function. If not, throw and exception.
		/// 
		/// Both function name and appropriate arguments are taken into account when finding a suitable
		/// handler. For built-in functions, the number of arguments must match; for custom functions,
		/// the supplied arguments must match one of the function prototypes defined by the handler.
		/// 
		/// </summary>
		public override System.Object eval(FormInstance model, EvaluationContext evalContext)
		{
			System.String name = id.ToString();
			System.Object[] argVals = new System.Object[args.Length];
			
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			System.Collections.Hashtable funcHandlers = evalContext.FunctionHandlers;
			
			//TODO: Func handlers should be able to declare the desire for short circuiting as well
			if (name.Equals("if"))
			{
				assertArgsCount(name, args, 3);
				return ifThenElse(model, evalContext, args, argVals);
			}
			else if (name.Equals("coalesce"))
			{
				assertArgsCount(name, args, 2);
				argVals[0] = args[0].eval(model, evalContext);
				if (!isNull(argVals[0]))
				{
					return argVals[0];
				}
				else
				{
					// that was null, so try the other one...
					argVals[1] = args[1].eval(model, evalContext);
					return argVals[1];
				}
			}
			else if (name.Equals("indexed-repeat"))
			{
				if ((args.Length == 3 || args.Length == 5 || args.Length == 7 || args.Length == 9 || args.Length == 11))
				{
					return indexedRepeat(model, evalContext, args, argVals);
				}
				else
				{
					throw new XPathUnhandledException("function \'" + name + "\' requires " + "3, 5, 7, 9 or 11 arguments. Only " + args.Length + " provided.");
				}
			}
			
			for (int i = 0; i < args.Length; i++)
			{
				argVals[i] = args[i].eval(model, evalContext);
			}
			
			//check built-in functions
			if (name.Equals("true"))
			{
				assertArgsCount(name, args, 0);
				return true;
			}
			else if (name.Equals("false"))
			{
				assertArgsCount(name, args, 0);
				return false;
			}
			else if (name.Equals("boolean"))
			{
				assertArgsCount(name, args, 1);
				return toBoolean(argVals[0]);
			}
			else if (name.Equals("number"))
			{
				assertArgsCount(name, args, 1);
				return toNumeric(argVals[0]);
			}
			else if (name.Equals("int"))
			{
				//non-standard
				assertArgsCount(name, args, 1);
				return toInt(argVals[0]);
			}
			else if (name.Equals("round"))
			{
				// non-standard Excel-style round(value,decimal place)
				assertArgsCount(name, args, 2);
				System.Double aval = toNumeric(argVals[0]);
				System.Double bval = toInt(argVals[1]);
				return round(aval, bval);
			}
			else if (name.Equals("string"))
			{
				assertArgsCount(name, args, 1);
				return toString(argVals[0]);
			}
			else if (name.Equals("date"))
			{
				//non-standard
				assertArgsCount(name, args, 1);
				return toDate(argVals[0], false);
			}
			else if (name.Equals("date-time"))
			{
				//non-standard -- convert double/int/string to Date object
				assertArgsCount(name, args, 1);
				return toDate(argVals[0], true);
			}
			else if (name.Equals("decimal-date-time"))
			{
				//non-standard -- convert string/date to decimal days off 1970-01-01T00:00:00.000-000
				assertArgsCount(name, args, 1);
				return toDecimalDateTime(argVals[0], true);
			}
			else if (name.Equals("decimal-time"))
			{
				//non-standard -- convert string/date to decimal days off 1970-01-01T00:00:00.000-000
				assertArgsCount(name, args, 1);
				return toDecimalDateTime(argVals[0], false);
			}
			else if (name.Equals("not"))
			{
				assertArgsCount(name, args, 1);
				return boolNot(argVals[0]);
			}
			else if (name.Equals("boolean-from-string"))
			{
				assertArgsCount(name, args, 1);
				return boolStr(argVals[0]);
			}
			else if (name.Equals("format-date"))
			{
				assertArgsCount(name, args, 2);
				return dateStr(argVals[0], argVals[1], false);
			}
			else if (name.Equals("format-date-time"))
			{
				// non-standard
				assertArgsCount(name, args, 2);
				return dateStr(argVals[0], argVals[1], true);
			}
			else if ((name.Equals("selected") || name.Equals("is-selected")))
			{
				//non-standard
				assertArgsCount(name, args, 2);
				return multiSelected(argVals[0], argVals[1], name);
			}
			else if (name.Equals("count-selected"))
			{
				//non-standard
				assertArgsCount(name, args, 1);
				return countSelected(argVals[0]);
			}
			else if (name.Equals("selected-at"))
			{
				//non-standard
				assertArgsCount(name, args, 2);
				return selectedAt(argVals[0], argVals[1]);
			}
			else if (name.Equals("position"))
			{
				//TODO: Technically, only the 0 length argument is valid here.
				if (args.Length == 1)
				{
					XPathNodeset nodes = (XPathNodeset) argVals[0];
					if (nodes.size() == 0)
					{
						// Added to prevent an exception within ODK Validate.
						// Will likely cause an error downstream when used in an XPath.
						return (double) (1 + TreeReference.INDEX_UNBOUND);
					}
					else
					{
						// This is weird -- we are returning the position of the first
						// Nodeset element but there may be a list of elements. Unclear
						// if or how this might manifest into a bug... .
						return position(nodes.getRefAt(0));
					}
				}
				else if (args.Length == 0)
				{
					if (evalContext.ContextPosition != - 1)
					{
						return (double) (1 + evalContext.ContextPosition);
					}
					return position(evalContext.ContextRef);
				}
				else
				{
					throw new XPathUnhandledException("function \'" + name + "\' requires either exactly one argument or no arguments. Only " + args.Length + " provided.");
				}
			}
			else if (name.Equals("count"))
			{
				assertArgsCount(name, args, 1);
				return count(argVals[0]);
			}
			else if (name.Equals("sum"))
			{
				assertArgsCount(name, args, 1);
				if (argVals[0] is XPathNodeset)
				{
					return sum(((XPathNodeset) argVals[0]).toArgList());
				}
				else
				{
					throw new XPathTypeMismatchException("not a nodeset");
				}
			}
			else if (name.Equals("max"))
			{
				if (args.Length == 1 && argVals[0] is XPathNodeset)
				{
					return max(((XPathNodeset) argVals[0]).toArgList());
				}
				else
				{
					return max(argVals);
				}
			}
			else if (name.Equals("min"))
			{
				if (args.Length == 1 && argVals[0] is XPathNodeset)
				{
					return min(((XPathNodeset) argVals[0]).toArgList());
				}
				else
				{
					return min(argVals);
				}
			}
			else if (name.Equals("today"))
			{
				assertArgsCount(name, args, 0);
				System.DateTime tempAux = System.DateTime.Now;
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
				return DateUtils.roundDate(ref tempAux);
			}
			else if (name.Equals("now"))
			{
				assertArgsCount(name, args, 0);
				return System.DateTime.Now;
			}
			else if (name.Equals("concat"))
			{
				if (args.Length == 1 && argVals[0] is XPathNodeset)
				{
					return join("", ((XPathNodeset) argVals[0]).toArgList());
				}
				else
				{
					return join("", argVals);
				}
			}
			else if (name.Equals("join") && args.Length >= 1)
			{
				if (args.Length == 2 && argVals[1] is XPathNodeset)
				{
					return join(argVals[0], ((XPathNodeset) argVals[1]).toArgList());
				}
				else
				{
					return join(argVals[0], subsetArgList(argVals, 1));
				}
			}
			else if (name.Equals("substr") && (args.Length == 2 || args.Length == 3))
			{
				return substring(argVals[0], argVals[1], (System.Object) (args.Length == 3?argVals[2]:null));
			}
			else if (name.Equals("string-length"))
			{
				assertArgsCount(name, args, 1);
				return stringLength(argVals[0]);
			}
			else if (name.Equals("checklist") && args.Length >= 2)
			{
				//non-standard
				if (args.Length == 3 && argVals[2] is XPathNodeset)
				{
					return checklist(argVals[0], argVals[1], ((XPathNodeset) argVals[2]).toArgList());
				}
				else
				{
					return checklist(argVals[0], argVals[1], subsetArgList(argVals, 2));
				}
			}
			else if (name.Equals("weighted-checklist") && args.Length >= 2 && args.Length % 2 == 0)
			{
				//non-standard
				if (args.Length == 4 && argVals[2] is XPathNodeset && argVals[3] is XPathNodeset)
				{
					System.Object[] factors = ((XPathNodeset) argVals[2]).toArgList();
					System.Object[] weights = ((XPathNodeset) argVals[3]).toArgList();
					if (factors.Length != weights.Length)
					{
						throw new XPathTypeMismatchException("weighted-checklist: nodesets not same length");
					}
					return checklistWeighted(argVals[0], argVals[1], factors, weights);
				}
				else
				{
					return checklistWeighted(argVals[0], argVals[1], subsetArgList(argVals, 2, 2), subsetArgList(argVals, 3, 2));
				}
			}
			else if (name.Equals("regex"))
			{
				//non-standard
				assertArgsCount(name, args, 2);
				return regex(argVals[0], argVals[1]);
			}
			else if (name.Equals("depend") && args.Length >= 1)
			{
				//non-standard
				return argVals[0];
			}
			else if (name.Equals("random"))
			{
				//non-standard
				assertArgsCount(name, args, 0);
				//calculated expressions may be recomputed w/o warning! use with caution!!
				return (double) MathUtils.Rand.NextDouble();
			}
			else if (name.Equals("once"))
			{
				assertArgsCount(name, args, 1);
				XPathPathExpr currentFieldPathExpr = XPathPathExpr.fromRef(evalContext.ContextRef);
				System.Object currValue = currentFieldPathExpr.eval(model, evalContext).unpack();
				if (currValue == null || toString(currValue).Length == 0)
				{
					// this is the "once" case
					return argVals[0];
				}
				else
				{
					return currValue;
				}
			}
			else if (name.Equals("uuid") && (args.Length == 0 || args.Length == 1))
			{
				//non-standard
				//calculated expressions may be recomputed w/o warning! use with caution!!
				if (args.Length == 0)
				{
					return PropertyUtils.genUUID();
				}
				
				int len = (int) toInt(argVals[0]);
				return PropertyUtils.genGUID(len);
			}
			else if (name.Equals("version"))
			{
				//non-standard
				assertArgsCount(name, args, 0);
				return model.formVersion == null?"":model.formVersion;
			}
			else if (name.Equals("property"))
			{
				// non-standard
				// return a property defined by the property manager.
				// NOTE: Property should be immutable.
				// i.e., does not work with 'start' or 'end' property.
				assertArgsCount(name, args, 1);
				System.String s = toString(argVals[0]);
				return PropertyManager._().getSingularProperty(s);
			}
			else if (name.Equals("pow") && (args.Length == 2))
			{
				//XPath 3.0
				double a = toDouble(argVals[0]);
				double b = toDouble(argVals[1]);
				return System.Math.Pow(a, b);
			}
			else
			{
				//check for custom handler
				//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
				IFunctionHandler handler = (IFunctionHandler) funcHandlers[name];
				if (handler != null)
				{
					return evalCustomFunction(handler, argVals, evalContext);
				}
				else
				{
					throw new XPathUnhandledException("function \'" + name + "\'");
				}
			}
		}
		
		private static void  assertArgsCount(System.String name, System.Object[] args, int count)
		{
			if (args.Length != count)
			{
				throw new XPathUnhandledException("function \'" + name + "\' requires " + count + " arguments. Only " + args.Length + " provided.");
			}
		}
		
		/// <summary> Given a handler registered to handle the function, try to coerce the function arguments into
		/// one of the prototypes defined by the handler. If no suitable prototype found, throw an eval
		/// exception. Otherwise, evaluate.
		/// 
		/// Note that if the handler supports 'raw args', it will receive the full, unaltered argument
		/// list if no prototype matches. (this lets functions support variable-length argument lists)
		/// 
		/// </summary>
		/// <param name="handler">
		/// </param>
		/// <param name="args">
		/// </param>
		/// <returns>
		/// </returns>
		private static System.Object evalCustomFunction(IFunctionHandler handler, System.Object[] args, EvaluationContext ec)
		{
			System.Collections.ArrayList prototypes = handler.Prototypes;
			System.Collections.IEnumerator e = prototypes.GetEnumerator();
			System.Object[] typedArgs = null;
			
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			while (typedArgs == null && e.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				typedArgs = matchPrototype(args, (System.Type[]) e.Current);
			}
			
			if (typedArgs != null)
			{
				return handler.eval(typedArgs, ec);
			}
			else if (handler.rawArgs())
			{
				return handler.eval(args, ec); //should we have support for expanding nodesets here?
			}
			else
			{
				throw new XPathTypeMismatchException("for function \'" + handler.Name + "\'");
			}
		}
		
		/// <summary> Given a prototype defined by the function handler, attempt to coerce the function arguments
		/// to match that prototype (checking # args, type conversion, etc.). If it is coercible, return
		/// the type-converted argument list -- these will be the arguments used to evaluate the function.
		/// If not coercible, return null.
		/// 
		/// </summary>
		/// <param name="args">
		/// </param>
		/// <param name="prototype">
		/// </param>
		/// <returns>
		/// </returns>
		private static System.Object[] matchPrototype(System.Object[] args, System.Type[] prototype)
		{
			System.Object[] typed = null;
			
			if (prototype.Length == args.Length)
			{
				typed = new System.Object[args.Length];
				
				for (int i = 0; i < prototype.Length; i++)
				{
					typed[i] = null;
					
					//how to handle type conversions of custom types?
					if (prototype[i].IsAssignableFrom(args[i].GetType()))
					{
						typed[i] = args[i];
					}
					else
					{
						try
						{
							if (prototype[i] == typeof(System.Boolean))
							{
								typed[i] = toBoolean(args[i]);
							}
							else if (prototype[i] == typeof(System.Double))
							{
								typed[i] = toNumeric(args[i]);
							}
							else if (prototype[i] == typeof(System.String))
							{
								typed[i] = toString(args[i]);
							}
							else if (prototype[i] == typeof(System.DateTime))
							{
								typed[i] = toDate(args[i], false);
							}
						}
						catch (XPathTypeMismatchException xptme)
						{
							/* swallow type mismatch exception */
						}
					}
					
					if (typed[i] == null)
						return null;
				}
			}
			
			return typed;
		}
		
		/// <summary>***** HANDLERS FOR BUILT-IN FUNCTIONS ********
		/// 
		/// the functions below are the handlers for the built-in xpath function suite
		/// 
		/// if you add a function to the suite, it should adhere to the following pattern:
		/// 
		/// * the function takes in its arguments as objects (DO NOT cast the arguments when calling
		/// the handler up in eval() (i.e., return stringLength((String)argVals[0])  <--- NO!)
		/// 
		/// * the function converts the generic argument(s) to the desired type using the built-in
		/// xpath type conversion functions (toBoolean(), toNumeric(), toString(), toDate())
		/// 
		/// * the function MUST return an object of type Boolean, Double, String, or Date; it may
		/// never return null (instead return the empty string or NaN)
		/// 
		/// * the function may throw exceptions, but should try as hard as possible not to, and if
		/// it must, strive to make it an XPathException
		/// 
		/// </summary>
		
		public static bool isNull(System.Object o)
		{
			if (o == null)
			{
				return true; //true 'null' values aren't allowed in the xpath engine, but whatever
			}
			
			o = unpack(o);
			if (o is System.String && ((System.String) o).Length == 0)
			{
				return true;
			}
			else if (o is System.Double && System.Double.IsNaN(((System.Double) o)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		public static System.Double stringLength(System.Object o)
		{
			System.String s = toString(o);
			if (s == null)
			{
				return (double) 0.0;
			}
			return (double) s.Length;
		}
		
		/// <summary> convert a value to a boolean using xpath's type conversion rules
		/// 
		/// </summary>
		/// <param name="o">
		/// </param>
		/// <returns>
		/// </returns>
		public static System.Boolean toBoolean(System.Object o)
		{
			//UPGRADE_TODO: The 'System.Boolean' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
			System.Boolean val = null;
			
			o = unpack(o);
			
			if (o is System.Boolean)
			{
				val = (System.Boolean) o;
			}
			else if (o is System.Double)
			{
				double d = ((System.Double) o);
				val = System.Math.Abs(d) > 1.0e-12 && !System.Double.IsNaN(d);
			}
			else if (o is System.String)
			{
				System.String s = (System.String) o;
				val = s.Length > 0;
			}
			else if (o is System.DateTime)
			{
				val = true;
			}
			else if (o is IExprDataType)
			{
				val = ((IExprDataType) o).toBoolean();
			}
			
			//UPGRADE_TODO: The 'System.Boolean' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
			if (val != null)
			{
				return val;
			}
			else
			{
				throw new XPathTypeMismatchException("converting to boolean");
			}
		}
		
		public static System.Double toDouble(System.Object o)
		{
			if (o is System.DateTime)
			{
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
				return DateUtils.fractionalDaysSinceEpoch(ref new System.DateTime[]{(System.DateTime) o}[0]);
			}
			else
			{
				return toNumeric(o);
			}
		}
		
		/// <summary> convert a value to a number using xpath's type conversion rules (note that xpath itself makes
		/// no distinction between integer and floating point numbers)
		/// 
		/// </summary>
		/// <param name="o">
		/// </param>
		/// <returns>
		/// </returns>
		public static System.Double toNumeric(System.Object o)
		{
			//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
			System.Double val = null;
			
			o = unpack(o);
			
			if (o is System.Boolean)
			{
				val = (double) (((System.Boolean) o)?1:0);
			}
			else if (o is System.Double)
			{
				val = (System.Double) o;
			}
			else if (o is System.String)
			{
				/* annoying, but the xpath spec doesn't recognize scientific notation, or +/-Infinity
				* when converting a string to a number
				*/
				
				System.String s = (System.String) o;
				double d;
				try
				{
					s = s.Trim();
					for (int i = 0; i < s.Length; i++)
					{
						char c = s[i];
						if (c != '-' && c != '.' && (c < '0' || c > '9'))
							throw new System.FormatException();
					}
					
					d = System.Double.Parse(s);
					val = (double) d;
				}
				catch (System.FormatException nfe)
				{
					val = (double) System.Double.NaN;
				}
			}
			else if (o is System.DateTime)
			{
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
				val = (double) DateUtils.daysSinceEpoch(ref new System.DateTime[]{(System.DateTime) o}[0]);
			}
			else if (o is IExprDataType)
			{
				val = ((IExprDataType) o).toNumeric();
			}
			
			//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
			if (val != null)
			{
				return val;
			}
			else
			{
				throw new XPathTypeMismatchException("converting to numeric");
			}
		}
		
		/// <summary> convert a number to an integer by truncating the fractional part. if non-numeric, coerce the
		/// value to a number first. note that the resulting return value is still a Double, as required
		/// by the xpath engine
		/// 
		/// </summary>
		/// <param name="o">
		/// </param>
		/// <returns>
		/// </returns>
		public static System.Double toInt(System.Object o)
		{
			System.Double val = toNumeric(o);
			
			if (System.Double.IsInfinity(val) || System.Double.IsNaN(val))
			{
				return val;
			}
			else if (val >= System.Int64.MaxValue || val <= System.Int64.MinValue)
			{
				return val;
			}
			else
			{
				long l = (long) val;
				System.Double dbl = (double) l;
				if (l == 0 && (val < 0.0 || val.Equals((double) (- 0.0))))
				{
					dbl = (double) (- 0.0);
				}
				return dbl;
			}
		}
		
		/// <summary> convert a value to a string using xpath's type conversion rules
		/// 
		/// </summary>
		/// <param name="o">
		/// </param>
		/// <returns>
		/// </returns>
		public static System.String toString(System.Object o)
		{
			System.String val = null;
			
			o = unpack(o);
			
			if (o is System.Boolean)
			{
				val = (((System.Boolean) o)?"true":"false");
			}
			else if (o is System.Double)
			{
				double d = ((System.Double) o);
				if (System.Double.IsNaN(d))
				{
					val = "NaN";
				}
				else if (System.Math.Abs(d) < 1.0e-12)
				{
					val = "0";
				}
				else if (System.Double.IsInfinity(d))
				{
					val = (d < 0?"-":"") + "Infinity";
				}
				else
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					if (System.Math.Abs(d - (int) d) < 1.0e-12)
					{
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						val = System.Convert.ToString((int) d);
					}
					else
					{
						val = System.Convert.ToString(d);
					}
				}
			}
			else if (o is System.String)
			{
				val = ((System.String) o);
			}
			else if (o is System.DateTime)
			{
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
				val = DateUtils.formatDate(ref new System.DateTime[]{(System.DateTime) o}[0], DateUtils.FORMAT_ISO8601);
			}
			else if (o is IExprDataType)
			{
				val = ((IExprDataType) o).ToString();
			}
			
			if (val != null)
			{
				return val;
			}
			else
			{
				throw new XPathTypeMismatchException("converting to string");
			}
		}
		
		/// <summary> convert a value to a date. note that xpath has no intrinsic representation of dates, so this
		/// is off-spec. dates convert to strings as 'yyyy-mm-dd', convert to numbers as # of days since
		/// the unix epoch, and convert to booleans always as 'true'
		/// 
		/// string and int conversions are reversable, however:
		/// * cannot convert bool to date
		/// * empty string and NaN (xpath's 'null values') go unchanged, instead of being converted
		/// into a date (which would cause an error, since Date has no null value (other than java
		/// null, which the xpath engine can't handle))
		/// * note, however, than non-empty strings that aren't valid dates _will_ cause an error
		/// during conversion
		/// 
		/// </summary>
		/// <param name="o">
		/// </param>
		/// <returns>
		/// </returns>
		public static System.Object toDate(System.Object o, bool preserveTime)
		{
			o = unpack(o);
			
			if (o is System.Double)
			{
				if (preserveTime)
				{
					System.Double n = (System.Double) o;
					
					if (System.Double.IsNaN(n))
					{
						return n;
					}
					
					if (System.Double.IsInfinity(n) || n > System.Int32.MaxValue || n < System.Int32.MinValue)
					{
						throw new XPathTypeMismatchException("converting out-of-range value to date");
					}
					
					long timeMillis = (long) (n * DateUtils.DAY_IN_MS);
					
					//UPGRADE_TODO: Constructor 'java.util.Date.Date' was converted to 'System.DateTime.DateTime' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDateDate_long'"
					System.DateTime d = new System.DateTime(timeMillis);
					return d;
				}
				else
				{
					System.Double n = toInt(o);
					
					if (System.Double.IsNaN(n))
					{
						return n;
					}
					
					if (System.Double.IsInfinity(n) || n > System.Int32.MaxValue || n < System.Int32.MinValue)
					{
						throw new XPathTypeMismatchException("converting out-of-range value to date");
					}
					
					System.DateTime tempAux = DateUtils.getDate(1970, 1, 1);
					//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					return DateUtils.dateAdd(ref tempAux, (int) n);
				}
			}
			else if (o is System.String)
			{
				System.String s = (System.String) o;
				
				if (s.Length == 0)
				{
					return s;
				}
				
				System.DateTime d = DateUtils.parseDateTime(s);
				//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
				if (d == null)
				{
					throw new XPathTypeMismatchException("converting to date");
				}
				else
				{
					return d;
				}
			}
			else if (o is System.DateTime)
			{
				if (preserveTime)
				{
					return (System.DateTime) o;
				}
				else
				{
					//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					return DateUtils.roundDate(ref new System.DateTime[]{(System.DateTime) o}[0]);
				}
			}
			else
			{
				throw new XPathTypeMismatchException("converting to date");
			}
		}
		
		public static System.Object toDecimalDateTime(System.Object o, bool keepDate)
		{
			o = unpack(o);
			
			if (o is System.Double)
			{
				System.Double n = (System.Double) o;
				
				if (System.Double.IsNaN(n))
				{
					return n;
				}
				
				if (System.Double.IsInfinity(n) || n > System.Int32.MaxValue || n < System.Int32.MinValue)
				{
					throw new XPathTypeMismatchException("converting out-of-range value to date");
				}
				
				if (keepDate)
				{
					return n;
				}
				else
				{
					return n - Math.floor(n);
				}
			}
			else if (o is System.String)
			{
				System.String s = (System.String) o;
				
				if (s.Length == 0)
				{
					return s;
				}
				
				System.DateTime d = DateUtils.parseDateTime(s);
				//UPGRADE_TODO: The 'System.DateTime' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
				if (d == null)
				{
					throw new XPathTypeMismatchException("converting to date");
				}
				else
				{
					if (keepDate)
					{
						//UPGRADE_TODO: Method 'java.util.Date.getTime' was converted to 'System.DateTime.Ticks' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDategetTime'"
						long milli = d.Ticks;
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						System.Double v = ((double) milli) / DateUtils.DAY_IN_MS;
						return v;
					}
					else
					{
						//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
						return DateUtils.decimalTimeOfLocalDay(ref d);
					}
				}
			}
			else if (o is System.DateTime)
			{
				System.DateTime d = (System.DateTime) o;
				if (keepDate)
				{
					//UPGRADE_TODO: Method 'java.util.Date.getTime' was converted to 'System.DateTime.Ticks' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDategetTime'"
					long milli = d.Ticks;
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					System.Double v = ((double) milli) / DateUtils.DAY_IN_MS;
					return v;
				}
				else
				{
					//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					return DateUtils.decimalTimeOfLocalDay(ref d);
				}
			}
			else
			{
				throw new XPathTypeMismatchException("converting to date");
			}
		}
		
		public static System.Boolean boolNot(System.Object o)
		{
			bool b = toBoolean(o);
			return !b;
		}
		
		public static System.Boolean boolStr(System.Object o)
		{
			System.String s = toString(o);
			if (s.ToUpper().Equals("true".ToUpper()) || s.Equals("1"))
				return true;
			else
				return false;
		}
		
		public static System.String dateStr(System.Object od, System.Object of, bool preserveTime)
		{
			od = toDate(od, preserveTime);
			if (od is System.DateTime)
			{
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
				return DateUtils.format(ref new System.DateTime[]{(System.DateTime) od}[0], toString(of));
			}
			else
			{
				return "";
			}
		}
		
		private System.Double position(TreeReference refAt)
		{
			return (double) (1 + refAt.MultLast);
		}
		
		public static System.Object ifThenElse(FormInstance model, EvaluationContext ec, XPathExpression[] args, System.Object[] argVals)
		{
			argVals[0] = args[0].eval(model, ec);
			bool b = toBoolean(argVals[0]);
			return (b?args[1].eval(model, ec):args[2].eval(model, ec));
		}
		
		/// <summary> This provides a method of indexing fields stored in prior repeat groups.
		/// 
		/// args[0] = generic XPath expression to index
		/// args[1] = generic XPath expression for group to index
		/// args[2] = index number for group
		/// args[3] = generic XPath expression for add'l group to index (if 5 or 7 parameters passed)
		/// args[4] = index number for group (if 5 or 7 parameters passed)
		/// args[5] = generic XPath expression for add'l group to index (if 7 parameters passed)
		/// args[6] = index number for group (if 7 parameters passed)
		/// 
		/// </summary>
		/// <param name="model">
		/// </param>
		/// <param name="ec">
		/// </param>
		/// <param name="args">
		/// </param>
		/// <param name="argVals">
		/// </param>
		/// <returns>
		/// </returns>
		public static System.Object indexedRepeat(FormInstance model, EvaluationContext ec, XPathExpression[] args, System.Object[] argVals)
		{
			// initialize target and context references
			if (!(args[0] is XPathPathExpr))
			{
				throw new XPathTypeMismatchException("indexed-repeat(): first parameter must be XPath field reference");
			}
			XPathPathExpr targetPath = (XPathPathExpr) args[0];
			TreeReference targetRef = targetPath.getReference();
			TreeReference contextRef = targetRef.Clone();
			
			// process passed index(es)
			for (int pathargi = 1, idxargi = 2; idxargi < args.Length; pathargi += 2, idxargi += 2)
			{
				// confirm that we were passed an XPath
				if (!(args[pathargi] is XPathPathExpr))
				{
					throw new XPathTypeMismatchException("indexed-repeat(): parameter " + (pathargi + 1) + " must be XPath repeat-group reference");
				}
				// confirm that the passed XPath is a parent of our overall target path
				TreeReference groupRef = ((XPathPathExpr) args[pathargi]).getReference();
				if (!groupRef.isParentOf(targetRef, true))
				{
					throw new XPathTypeMismatchException("indexed-repeat(): parameter " + (pathargi + 1) + " must be a parent of the field in parameter 1");
				}
				
				// process index (if valid)
				int groupIdx = (int) toInt(args[idxargi].eval(model, ec));
				if (groupIdx <= 0)
				{
					// ignore invalid indexes (primarily happens during validation)
				}
				else
				{
					// otherwise, add the index to the context reference
					contextRef.setMultiplicity(groupRef.size() - 1, groupIdx - 1);
				}
			}
			
			// evaluate and return the XPath expression, in context
			EvaluationContext revisedec = new EvaluationContext(ec, contextRef);
			return (targetPath.eval(model, revisedec));
		}
		
		/// <summary> return whether a particular choice of a multi-select is selected
		/// 
		/// </summary>
		/// <param name="o1">XML-serialized answer to multi-select question (i.e, space-delimited choice values)
		/// </param>
		/// <param name="o2">choice to look for
		/// </param>
		/// <returns>
		/// </returns>
		public static System.Boolean multiSelected(System.Object o1, System.Object o2, System.String functionName)
		{
			System.Object indexObject = unpack(o2);
			if (!(indexObject is System.String))
			{
				throw new XPathTypeMismatchException("The second parameter to the " + functionName + "() function must be in quotes (like '1').");
			}
			System.String s1 = (System.String) unpack(o1);
			System.String s2 = ((System.String) indexObject).Trim();
			
			return (" " + s1 + " ").IndexOf(" " + s2 + " ") != - 1;
		}
		
		/// <summary> return the number of choices in a multi-select answer
		/// 
		/// </summary>
		/// <param name="o">XML-serialized answer to multi-select question (i.e, space-delimited choice values)
		/// </param>
		/// <returns>
		/// </returns>
		public static System.Double countSelected(System.Object o)
		{
			System.String s = (System.String) unpack(o);
			
			return new Double(DateUtils.split(s, " ", true).size());
		}
		
		/// <summary> Get the Nth item in a selected list
		/// 
		/// </summary>
		/// <param name="o1">XML-serialized answer to multi-select question (i.e, space-delimited choice values)
		/// </param>
		/// <param name="o2">the integer index into the list to return
		/// </param>
		/// <returns>
		/// </returns>
		public static System.String selectedAt(System.Object o1, System.Object o2)
		{
			System.String selection = (System.String) unpack(o1);
			int index = (int) toInt(o2);
			System.Collections.ArrayList stringVector = DateUtils.split(selection, " ", true);
			if (stringVector.Count > index && index >= 0)
			{
				return (System.String) stringVector[index];
			}
			else
			{
				return ""; // empty string if outside of array
			}
		}
		
		/// <summary> count the number of nodes in a nodeset
		/// 
		/// </summary>
		/// <param name="o">
		/// </param>
		/// <returns>
		/// </returns>
		public static System.Double count(System.Object o)
		{
			if (o is XPathNodeset)
			{
				return (double) ((XPathNodeset) o).size();
			}
			else
			{
				throw new XPathTypeMismatchException("not a nodeset");
			}
		}
		
		/// <summary> sum the values in a nodeset; each element is coerced to a numeric value
		/// 
		/// </summary>
		/// <param name="model">
		/// </param>
		/// <param name="o">
		/// </param>
		/// <returns>
		/// </returns>
		public static System.Double sum(System.Object[] argVals)
		{
			double sum = 0.0;
			for (int i = 0; i < argVals.Length; i++)
			{
				System.Double dargVal = toNumeric(argVals[i]);
				if (!System.Double.IsNaN(dargVal))
				{
					sum += dargVal;
				}
			}
			return (double) sum;
		}
		
		/// <summary> round function like in Excel.
		/// 
		/// </summary>
		/// <param name="num">
		/// </param>
		/// <param name="dblDecim">
		/// </param>
		/// <returns>
		/// </returns>
		private static System.Double round(double num, double numDecim)
		{
			long p = 0;
			
			// rounding doesn't affect special values...
			if (num == System.Double.NaN || num == System.Double.NegativeInfinity || num == System.Double.PositiveInfinity)
			{
				return (double) num;
			}
			
			// absurdly large rounding requests yield NaN...
			if (numDecim > 30.0 || numDecim < - 30.0)
			{
				return (double) System.Double.NaN;
			}
			
			// ignore fractional numDecim rounding values.
			// any numDecim that is more than 0.5 is considered "1"
			
			if (numDecim < 0.0)
			{
				// we want to retain 100's or higher value
				// round( 33.33, 1) = 30.0
				// divide everything by 10's.
				while (numDecim <= - 0.5)
				{
					num /= 10.0;
					++p;
					++numDecim;
				}
			}
			else
			{
				// we want to retain a fractional value
				// round( 33.33, -1) = 3.3
				// multiply everything by 10's.
				while (numDecim >= 0.5)
				{
					num *= 10.0;
					--p;
					--numDecim;
				}
			}
			// truncate with a 0.5 offset to round to the nearest long...
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			num = (double) ((long) (num + 0.5));
			
			// and now restore the number to the appropriate scale.
			if (p < 0)
			{
				while (p < 0)
				{
					num /= 10.0;
					++p;
				}
			}
			else
			{
				while (p > 0)
				{
					num *= 10.0;
					--p;
				}
			}
			return (double) num;
		}
		
		/// <summary> Identify the largest value from the list of provided values.
		/// 
		/// </summary>
		/// <param name="argVals">
		/// </param>
		/// <returns>
		/// </returns>
		private static System.Object max(System.Object[] argVals)
		{
			//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			double max = System.Double.MinValue;
			bool returnNaN = true;
			for (int i = 0; i < argVals.Length; i++)
			{
				System.Double dargVal = toNumeric(argVals[i]);
				if (!System.Double.IsNaN(dargVal))
				{
					max = System.Math.Max(max, dargVal);
					returnNaN = false;
				}
			}
			return (double) (returnNaN?System.Double.NaN:max);
		}
		
		private static System.Object min(System.Object[] argVals)
		{
			//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			double min = System.Double.MaxValue;
			bool returnNaN = true;
			for (int i = 0; i < argVals.Length; i++)
			{
				System.Double dargVal = toNumeric(argVals[i]);
				if (!System.Double.IsNaN(dargVal))
				{
					min = System.Math.Min(min, dargVal);
					returnNaN = false;
				}
			}
			return (double) (returnNaN?System.Double.NaN:min);
		}
		
		/// <summary> concatenate an abritrary-length argument list of string values together
		/// 
		/// </summary>
		/// <param name="argVals">
		/// </param>
		/// <returns>
		/// </returns>
		public static System.String join(System.Object oSep, System.Object[] argVals)
		{
			System.String sep = toString(oSep);
			StringBuilder sb = new StringBuilder();
			
			for (int i = 0; i < argVals.Length; i++)
			{
				sb.append(toString(argVals[i]));
				if (i < argVals.Length - 1)
					sb.append(sep);
			}
			
			return sb.toString();
		}
		
		public static System.String substring(System.Object o1, System.Object o2, System.Object o3)
		{
			System.String s = toString(o1);
			int start = (int) toInt(o2);
			
			int len = s.Length;
			
			int end = (o3 != null?(int) toInt(o3):len);
			if (start < 0)
			{
				start = len + start;
			}
			if (end < 0)
			{
				end = len + end;
			}
			end = System.Math.Min(System.Math.Max(0, end), len);
			start = System.Math.Min(System.Math.Max(0, start), len);
			
			return (start <= end?s.Substring(start, (end) - (start)):"");
		}
		
		/// <summary> perform a 'checklist' computation, enabling expressions like 'if there are at least 3 risk
		/// factors active'
		/// 
		/// </summary>
		/// <param name="argVals">the first argument is a numeric value expressing the minimum number of factors required.
		/// if -1, no minimum is applicable
		/// the second argument is a numeric value expressing the maximum number of allowed factors.
		/// if -1, no maximum is applicalbe
		/// arguments 3 through the end are the individual factors, each coerced to a boolean value
		/// </param>
		/// <returns> true if the count of 'true' factors is between the applicable minimum and maximum,
		/// inclusive
		/// </returns>
		public static System.Boolean checklist(System.Object oMin, System.Object oMax, System.Object[] factors)
		{
			int min = (int) toNumeric(oMin);
			int max = (int) toNumeric(oMax);
			
			int count = 0;
			for (int i = 0; i < factors.Length; i++)
			{
				if (toBoolean(factors[i]))
					count++;
			}
			
			return (min < 0 || count >= min) && (max < 0 || count <= max);
		}
		
		/// <summary> very similar to checklist, only each factor is assigned a real-number 'weight'.
		/// 
		/// the first and second args are again the minimum and maximum, but -1 no longer means
		/// 'not applicable'.
		/// 
		/// subsequent arguments come in pairs: first the boolean value, then the floating-point
		/// weight for that value
		/// 
		/// the weights of all the 'true' factors are summed, and the function returns whether
		/// this sum is between the min and max
		/// 
		/// </summary>
		/// <param name="argVals">
		/// </param>
		/// <returns>
		/// </returns>
		public static System.Boolean checklistWeighted(System.Object oMin, System.Object oMax, System.Object[] flags, System.Object[] weights)
		{
			double min = toNumeric(oMin);
			double max = toNumeric(oMax);
			
			double sum = 0.0;
			for (int i = 0; i < flags.Length; i++)
			{
				bool flag = toBoolean(flags[i]);
				double weight = toNumeric(weights[i]);
				
				if (flag)
					sum += weight;
			}
			
			return sum >= min && sum <= max;
		}
		
		/// <summary> determine if a string matches a regular expression.
		/// 
		/// </summary>
		/// <param name="o1">string being matched
		/// </param>
		/// <param name="o2">regular expression
		/// </param>
		/// <returns>
		/// </returns>
		public static System.Boolean regex(System.Object o1, System.Object o2)
		{
			System.String str = toString(o1);
			System.String re = toString(o2);
			
			bool result = Pattern.matches(re, str);
			return result;
		}
		
		private static System.Object[] subsetArgList(System.Object[] args, int start)
		{
			return subsetArgList(args, start, 1);
		}
		
		/// <summary> return a subset of an argument list as a new arguments list
		/// 
		/// </summary>
		/// <param name="args">
		/// </param>
		/// <param name="start">index to start at
		/// </param>
		/// <param name="skip">sub-list will contain every nth argument, where n == skip (default: 1)
		/// </param>
		/// <returns>
		/// </returns>
		private static System.Object[] subsetArgList(System.Object[] args, int start, int skip)
		{
			if (start > args.Length || skip < 1)
			{
				throw new System.SystemException("error in subsetting arglist");
			}
			
			System.Object[] subargs = new System.Object[(int) MathUtils.divLongNotSuck(args.Length - start - 1, skip) + 1];
			for (int i = start, j = 0; i < args.Length; i += skip, j++)
			{
				subargs[j] = args[i];
			}
			
			return subargs;
		}
		
		public static System.Object unpack(System.Object o)
		{
			if (o is XPathNodeset)
			{
				return ((XPathNodeset) o).unpack();
			}
			else
			{
				return o;
			}
		}
		
		/// <summary> </summary>
		new public System.Object pivot;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		(FormInstance model, EvaluationContext evalContext, Vector < Object > pivots, Object sentinal) throws UnpivotableExpressionException
		//UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}