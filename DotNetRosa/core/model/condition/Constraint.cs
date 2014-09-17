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
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using Logger = org.javarosa.core.services.Logger;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapTagged = org.javarosa.core.util.externalizable.ExtWrapTagged;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using XPathParseTool = org.javarosa.xpath.XPathParseTool;
using XPathExpression = org.javarosa.xpath.expr.XPathExpression;
namespace org.javarosa.core.model.condition
{
	
	public class Constraint
	{
		public IConditionExpr constraint;
		private System.String constraintMsg;
		private XPathExpression xPathConstraintMsg;
		
		public Constraint()
		{
		}
		
		public Constraint(IConditionExpr constraint, System.String constraintMsg)
		{
			this.constraint = constraint;
			this.constraintMsg = constraintMsg;
			attemptConstraintCompile();
		}
		
		public virtual System.String getConstraintMessage(EvaluationContext ec, FormInstance instance, System.String textForm)
		{
			if (xPathConstraintMsg == null)
			{
				//If the request is for getting a constraint message in a specific format (like audio) from 
				//itext, and there's no xpath, we couldn't possibly fulfill it
				return textForm == null?constraintMsg:null;
			}
			else
			{
				if (textForm != null)
				{
					ec.OutputTextForm = textForm;
				}
				try
				{
					System.Object value_Renamed = xPathConstraintMsg.eval(instance, ec);
					if (value_Renamed != (System.Object) "")
					{
						return (System.String) value_Renamed;
					}
					return null;
				}
				catch (System.Exception e)
				{
					Logger.exception("Error evaluating a valid-looking constraint xpath ", e);
					return constraintMsg;
				}
			}
		}
		
		private void  attemptConstraintCompile()
		{
			xPathConstraintMsg = null;
			try
			{
				if (constraintMsg != null)
				{
					xPathConstraintMsg = XPathParseTool.parseXPath("string(" + constraintMsg + ")");
				}
			}
			catch (System.Exception e)
			{
				//Expected in probably most cases.
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			constraint = (IConditionExpr) ExtUtil.read(in_Renamed, new ExtWrapTagged(), pf);
			constraintMsg = ExtUtil.nullIfEmpty(ExtUtil.readString(in_Renamed));
			attemptConstraintCompile();
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.write(out_Renamed, new ExtWrapTagged(constraint));
			ExtUtil.writeString(out_Renamed, ExtUtil.emptyIfNull(constraintMsg));
		}
	}
}