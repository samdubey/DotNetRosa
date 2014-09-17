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
using CacheTable = org.javarosa.core.util.CacheTable;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapListPoly = org.javarosa.core.util.externalizable.ExtWrapListPoly;
using ExtWrapNullable = org.javarosa.core.util.externalizable.ExtWrapNullable;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.xpath.expr
{
	
	public class XPathStep
	{
		private void  InitBlock()
		{
			XPathStep.refs = refs;
		}
		public const int AXIS_CHILD = 0;
		public const int AXIS_DESCENDANT = 1;
		public const int AXIS_PARENT = 2;
		public const int AXIS_ANCESTOR = 3;
		public const int AXIS_FOLLOWING_SIBLING = 4;
		public const int AXIS_PRECEDING_SIBLING = 5;
		public const int AXIS_FOLLOWING = 6;
		public const int AXIS_PRECEDING = 7;
		public const int AXIS_ATTRIBUTE = 8;
		public const int AXIS_NAMESPACE = 9;
		public const int AXIS_SELF = 10;
		public const int AXIS_DESCENDANT_OR_SELF = 11;
		public const int AXIS_ANCESTOR_OR_SELF = 12;
		
		public const int TEST_NAME = 0;
		public const int TEST_NAME_WILDCARD = 1;
		public const int TEST_NAMESPACE_WILDCARD = 2;
		public const int TEST_TYPE_NODE = 3;
		public const int TEST_TYPE_TEXT = 4;
		public const int TEST_TYPE_COMMENT = 5;
		public const int TEST_TYPE_PROCESSING_INSTRUCTION = 6;
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private static CacheTable < XPathStep > refs;
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public static
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		void attachCacheTable(CacheTable < XPathStep > refs)
		
		public static XPathStep ABBR_SELF()
		{
			return new XPathStep(AXIS_SELF, TEST_TYPE_NODE);
		}
		
		public static XPathStep ABBR_PARENT()
		{
			return new XPathStep(AXIS_PARENT, TEST_TYPE_NODE);
		}
		
		public static XPathStep ABBR_DESCENDANTS()
		{
			return new XPathStep(AXIS_DESCENDANT_OR_SELF, TEST_TYPE_NODE);
		}
		
		public int axis;
		public int test;
		public XPathExpression[] predicates;
		
		//test-dependent variables
		public XPathQName name; //TEST_NAME only
		public System.String namespace_Renamed; //TEST_NAMESPACE_WILDCARD only
		public System.String literal; //TEST_TYPE_PROCESSING_INSTRUCTION only
		
		public XPathStep()
		{
			InitBlock();
		} //for deserialization
		
		public XPathStep(int axis, int test)
		{
			InitBlock();
			this.axis = axis;
			this.test = test;
			this.predicates = new XPathExpression[0];
		}
		
		public XPathStep(int axis, XPathQName name):this(axis, TEST_NAME)
		{
			this.name = name;
		}
		
		public XPathStep(int axis, System.String namespace_Renamed):this(axis, TEST_NAMESPACE_WILDCARD)
		{
			this.namespace_Renamed = namespace_Renamed;
		}
		
		public override System.String ToString()
		{
			StringBuilder sb = new StringBuilder();
			
			sb.append("{step:");
			sb.append(axisStr(axis));
			sb.append(",");
			sb.append(testStr());
			
			if (predicates.Length > 0)
			{
				sb.append(",{");
				for (int i = 0; i < predicates.Length; i++)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					sb.append(predicates[i].ToString());
					if (i < predicates.Length - 1)
						sb.append(",");
				}
				sb.append("}");
			}
			sb.append("}");
			
			return sb.toString();
		}
		
		public static System.String axisStr(int axis)
		{
			switch (axis)
			{
				
				case AXIS_CHILD:  return "child";
				
				case AXIS_DESCENDANT:  return "descendant";
				
				case AXIS_PARENT:  return "parent";
				
				case AXIS_ANCESTOR:  return "ancestor";
				
				case AXIS_FOLLOWING_SIBLING:  return "following-sibling";
				
				case AXIS_PRECEDING_SIBLING:  return "preceding-sibling";
				
				case AXIS_FOLLOWING:  return "following";
				
				case AXIS_PRECEDING:  return "preceding";
				
				case AXIS_ATTRIBUTE:  return "attribute";
				
				case AXIS_NAMESPACE:  return "namespace";
				
				case AXIS_SELF:  return "self";
				
				case AXIS_DESCENDANT_OR_SELF:  return "descendant-or-self";
				
				case AXIS_ANCESTOR_OR_SELF:  return "ancestor-or-self";
				
				default:  return null;
				
			}
		}
		
		public virtual System.String testStr()
		{
			switch (test)
			{
				
				case TEST_NAME:  return name.ToString();
				
				case TEST_NAME_WILDCARD:  return "*";
				
				case TEST_NAMESPACE_WILDCARD:  return namespace_Renamed + ":*";
				
				case TEST_TYPE_NODE:  return "node()";
				
				case TEST_TYPE_TEXT:  return "text()";
				
				case TEST_TYPE_COMMENT:  return "comment()";
				
				case TEST_TYPE_PROCESSING_INSTRUCTION:  return "proc-instr(" + (literal == null?"":"\'" + literal + "\'") + ")";
				
				default:  return null;
				
			}
		}
		
		public  override bool Equals(System.Object o)
		{
			if (o is XPathStep)
			{
				XPathStep x = (XPathStep) o;
				
				//shortcuts for faster evaluation
				if (axis != x.axis || test != x.test || predicates.Length != x.predicates.Length)
				{
					return false;
				}
				
				switch (test)
				{
					
					case TEST_NAME:  if (!name.Equals(x.name))
						{
							return false;
						} break;
					
					case TEST_NAMESPACE_WILDCARD:  if (!namespace_Renamed.Equals(x.namespace_Renamed))
						{
							return false;
						} break;
					
					case TEST_TYPE_PROCESSING_INSTRUCTION:  if (!ExtUtil.equals(literal, x.literal))
						{
							return false;
						} break;
					
					default:  break;
					
				}
				
				return ExtUtil.arrayEquals(predicates, x.predicates);
			}
			else
			{
				return false;
			}
		}
		
		/// <summary> "matches" follows roughly the same process as equals(), in that it for a step it will
		/// check whether two steps share the same properties (multiplicity, test, axis, etc).
		/// The only difference is that match() will allow for a named step to match a step who's name
		/// is a wildcard.
		/// 
		/// So
		/// \/path\/
		/// will "match"
		/// \/*\/
		/// 
		/// even though they are not equal.
		/// 
		/// Matching is reflexive, consistent, and symmetric, but _not_ transitive.
		/// </summary>
		/// <param name="xPathStep">
		/// </param>
		/// <returns>
		/// </returns>
		protected internal virtual bool matches(XPathStep o)
		{
			if (o is XPathStep)
			{
				XPathStep x = (XPathStep) o;
				
				//shortcuts for faster evaluation
				if (axis != x.axis || (test != x.test && !((x.test == TEST_NAME && this.test == TEST_NAME_WILDCARD) || (this.test == TEST_NAME && x.test == TEST_NAME_WILDCARD))) || predicates.Length != x.predicates.Length)
				{
					return false;
				}
				
				switch (test)
				{
					
					case TEST_NAME:  if (x.test != TEST_NAME_WILDCARD && !name.Equals(x.name))
						{
							return false;
						} break;
					
					case TEST_NAMESPACE_WILDCARD:  if (!namespace_Renamed.Equals(x.namespace_Renamed))
						{
							return false;
						} break;
					
					case TEST_TYPE_PROCESSING_INSTRUCTION:  if (!ExtUtil.equals(literal, x.literal))
						{
							return false;
						} break;
					
					default:  break;
					
				}
				
				return ExtUtil.arrayEquals(predicates, x.predicates);
			}
			else
			{
				return false;
			}
		}
		
		public override int GetHashCode()
		{
			int code = this.axis ^ this.test ^ (this.name == null?0:this.name.GetHashCode()) ^ (this.literal == null?0:this.literal.GetHashCode()) ^ (this.namespace_Renamed == null?0:this.namespace_Renamed.GetHashCode());
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(XPathExpression xpe: predicates)
			{
				code ^= xpe.hashCode();
			}
			return code;
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			axis = ExtUtil.readInt(in_Renamed);
			test = ExtUtil.readInt(in_Renamed);
			
			switch (test)
			{
				
				case TEST_NAME:  name = (XPathQName) ExtUtil.read(in_Renamed, typeof(XPathQName)); break;
				
				case TEST_NAMESPACE_WILDCARD:  namespace_Renamed = ExtUtil.readString(in_Renamed); break;
				
				case TEST_TYPE_PROCESSING_INSTRUCTION:  literal = ((System.String) ExtUtil.read(in_Renamed, new ExtWrapNullable(typeof(System.String)))); break;
				}
			
			System.Collections.ArrayList v = (System.Collections.ArrayList) ExtUtil.read(in_Renamed, new ExtWrapListPoly(), pf);
			predicates = new XPathExpression[v.Count];
			for (int i = 0; i < predicates.Length; i++)
				predicates[i] = (XPathExpression) v[i];
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeNumeric(out_Renamed, axis);
			ExtUtil.writeNumeric(out_Renamed, test);
			
			switch (test)
			{
				
				case TEST_NAME:  ExtUtil.write(out_Renamed, name); break;
				
				case TEST_NAMESPACE_WILDCARD:  ExtUtil.writeString(out_Renamed, namespace_Renamed); break;
				
				case TEST_TYPE_PROCESSING_INSTRUCTION:  ExtUtil.write(out_Renamed, new ExtWrapNullable(literal)); break;
				}
			
			System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			for (int i = 0; i < predicates.Length; i++)
				v.Add(predicates[i]);
			ExtUtil.write(out_Renamed, new ExtWrapListPoly(v));
		}
		
		public static bool XPathStepInterningEnabled = true;
		public virtual XPathStep intern()
		{
			if (!XPathStepInterningEnabled || refs == null)
			{
				return this;
			}
			else
			{
				return refs.intern(this);
			}
		}
	}
}