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
using FormDef = org.javarosa.core.model.FormDef;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using XPathException = org.javarosa.xpath.XPathException;
namespace org.javarosa.core.model.condition
{
	
	public class Condition:Triggerable
	{
		override public bool CascadingToChildren
		{
			get
			{
				return (trueAction == ACTION_SHOW || trueAction == ACTION_HIDE);
			}
			
		}
		public const int ACTION_NULL = 0;
		public const int ACTION_SHOW = 1;
		public const int ACTION_HIDE = 2;
		public const int ACTION_ENABLE = 3;
		public const int ACTION_DISABLE = 4;
		public const int ACTION_LOCK = 5;
		public const int ACTION_UNLOCK = 6;
		public const int ACTION_REQUIRE = 7;
		public const int ACTION_DONT_REQUIRE = 8;
		
		public int trueAction;
		public int falseAction;
		
		public Condition()
		{
		}
		
		public Condition(IConditionExpr expr, int trueAction, int falseAction, TreeReference contextRef):this(expr, trueAction, falseAction, contextRef, System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10)))
		{
		}
		
		public Condition(IConditionExpr expr, int trueAction, int falseAction, TreeReference contextRef, System.Collections.ArrayList targets):base(expr, contextRef)
		{
			this.trueAction = trueAction;
			this.falseAction = falseAction;
			this.targets = targets;
		}
		
		public override System.Object eval(FormInstance model, EvaluationContext evalContext)
		{
			try
			{
				return expr.eval(model, evalContext);
			}
			catch (XPathException e)
			{
				e.setSource("Relevant expression for " + contextRef.toString(true));
				throw e;
			}
		}
		
		public virtual bool evalBool(FormInstance model, EvaluationContext evalContext)
		{
			return ((System.Boolean) eval(model, evalContext));
		}
		
		public override void  apply(TreeReference ref_Renamed, System.Object rawResult, FormInstance model, FormDef f)
		{
			bool result = ((System.Boolean) rawResult);
			performAction(model.resolveReference(ref_Renamed), result?trueAction:falseAction);
		}
		
		public override bool canCascade()
		{
			return (trueAction == ACTION_SHOW || trueAction == ACTION_HIDE);
		}
		
		
		private void  performAction(TreeElement node, int action)
		{
			switch (action)
			{
				
				case ACTION_NULL:  break;
				
				case ACTION_SHOW:  node.setRelevant(true); break;
				
				case ACTION_HIDE:  node.setRelevant(false); break;
				
				case ACTION_ENABLE:  node.setEnabled(true); break;
				
				case ACTION_DISABLE:  node.setEnabled(false); break;
				
				case ACTION_LOCK:  /* not supported */ ; break;
				
				case ACTION_UNLOCK:  /* not supported */ ; break;
				
				case ACTION_REQUIRE:  node.Required = true; break;
				
				case ACTION_DONT_REQUIRE:  node.Required = false; break;
				}
		}
		
		//conditions are equal if they have the same actions, expression, and triggers, but NOT targets or context ref
		public  override bool Equals(System.Object o)
		{
			if (o is Condition)
			{
				Condition c = (Condition) o;
				if (this == c)
					return true;
				
				return (this.trueAction == c.trueAction && this.falseAction == c.falseAction && base.Equals(c));
			}
			else
			{
				return false;
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			base.readExternal(in_Renamed, pf);
			trueAction = ExtUtil.readInt(in_Renamed);
			falseAction = ExtUtil.readInt(in_Renamed);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			base.writeExternal(out_Renamed);
			ExtUtil.writeNumeric(out_Renamed, trueAction);
			ExtUtil.writeNumeric(out_Renamed, falseAction);
		}
		//UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}