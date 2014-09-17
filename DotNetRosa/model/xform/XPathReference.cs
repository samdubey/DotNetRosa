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

/// <summary> </summary>
using System;
using IDataReference = org.javarosa.core.model.IDataReference;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using XPathException = org.javarosa.xpath.XPathException;
using XPathParseTool = org.javarosa.xpath.XPathParseTool;
using XPathTypeMismatchException = org.javarosa.xpath.XPathTypeMismatchException;
using XPathExpression = org.javarosa.xpath.expr.XPathExpression;
using XPathPathExpr = org.javarosa.xpath.expr.XPathPathExpr;
using XPathSyntaxException = org.javarosa.xpath.parser.XPathSyntaxException;
namespace org.javarosa.model.xform
{
	
	/// <summary> </summary>
	public class XPathReference : IDataReference
	{
		virtual public System.Object Reference
		{
			get
			{
				return ref_Renamed;
			}
			
			set
			{
				//do nothing
			}
			
		}
		private TreeReference ref_Renamed;
		private System.String nodeset;
		
		public XPathReference()
		{
		}
		
		public XPathReference(System.String nodeset)
		{
			ref_Renamed = getPathExpr(nodeset).getReference();
			this.nodeset = nodeset;
		}
		
		public static XPathPathExpr getPathExpr(System.String nodeset)
		{
			XPathExpression path;
			bool validNonPathExpr = false;
			
			try
			{
				path = XPathParseTool.parseXPath(nodeset);
				if (!(path is XPathPathExpr))
				{
					validNonPathExpr = true;
					throw new XPathSyntaxException();
				}
			}
			catch (XPathSyntaxException xse)
			{
				//make these checked exceptions?
				if (validNonPathExpr)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					throw new XPathTypeMismatchException("Expected XPath path, got XPath expression: [" + nodeset + "]," + xse.Message);
				}
				else
				{
					SupportClass.WriteStackTrace(xse, Console.Error);
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					throw new XPathException("Parse error in XPath path: [" + nodeset + "]." + (xse.Message == null?"":"\n" + xse.Message));
				}
			}
			
			return (XPathPathExpr) path;
		}
		
		public XPathReference(XPathPathExpr path)
		{
			ref_Renamed = path.getReference();
		}
		
		public XPathReference(TreeReference ref_Renamed)
		{
			this.ref_Renamed = ref_Renamed;
		}
		
		public  override bool Equals(System.Object o)
		{
			if (o is XPathReference)
			{
				return ref_Renamed.Equals(((XPathReference) o).ref_Renamed);
			}
			else
			{
				return false;
			}
		}
		
		public override int GetHashCode()
		{
			return ref_Renamed.GetHashCode();
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.utilities.Externalizable#readExternal(java.io.DataInputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			nodeset = ExtUtil.nullIfEmpty(ExtUtil.readString(in_Renamed));
			ref_Renamed = (TreeReference) ExtUtil.read(in_Renamed, typeof(TreeReference), pf);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.services.storage.utilities.Externalizable#writeExternal(java.io.DataOutputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeString(out_Renamed, ExtUtil.emptyIfNull(nodeset));
			ExtUtil.write(out_Renamed, ref_Renamed);
		}
	}
}