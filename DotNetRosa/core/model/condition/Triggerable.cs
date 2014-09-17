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
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapList = org.javarosa.core.util.externalizable.ExtWrapList;
using ExtWrapTagged = org.javarosa.core.util.externalizable.ExtWrapTagged;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using XPathException = org.javarosa.xpath.XPathException;
namespace org.javarosa.core.model.condition
{
	
	/// <summary> A triggerable represents an action that should be processed based
	/// on a value updating in a model. Trigerrables are comprised of two
	/// basic components: An expression to be evaluated, and a reference
	/// which represents where the resultant value will be stored.
	/// 
	/// A triggerable will dispatch the action it's performing out to
	/// all relevant nodes referenced by the context against thes current
	/// models.
	/// 
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public abstract class Triggerable
	{
		private void  InitBlock()
		{
			return targets;
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'" /// should this be originalContextRef???
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Set < TreeReference > absTriggers = new HashSet < TreeReference >();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			for(TreeReference r: relTriggers)
			{
				absTriggers.add(r.anchor(originalContextRef));
			}
			return absTriggers;
		}
		/// <summary> This should return true if this triggerable's targets will implicity modify the
		/// value of their children. IE: if this triggerable makes a node relevant/irrelevant,
		/// expressions which care about the value of this node's children should be triggered.
		/// 
		/// </summary>
		/// <returns> True if this condition should trigger expressions whose targets include
		/// nodes which are the children of this node's targets.
		/// </returns>
		virtual public bool CascadingToChildren
		{
			get
			{
				return false;
			}
			
		}
		/// <summary> The expression which will be evaluated to produce a result</summary>
		public IConditionExpr expr;
		
		/// <summary> References to all of the (non-contextualized) nodes which should be
		/// updated by the result of this triggerable
		/// 
		/// </summary>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public Vector < TreeReference > targets;
		
		/// <summary> Current reference which is the "Basis" of the trigerrables being evaluated. This is the highest
		/// common root of all of the targets being evaluated.
		/// </summary>
		public TreeReference contextRef; //generic ref used to turn triggers into absolute references
		
		/// <summary> The first context provided to this triggerable before reducing to the common root.</summary>
		public TreeReference originalContextRef;
		
		public Triggerable()
		{
			InitBlock();
		}
		
		public Triggerable(IConditionExpr expr, TreeReference contextRef)
		{
			InitBlock();
			this.expr = expr;
			this.contextRef = contextRef;
			this.originalContextRef = contextRef;
			this.targets = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
		}
		
		//UPGRADE_NOTE: Access modifiers of method 'eval' were changed to 'public'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1204'"
		public abstract System.Object eval(FormInstance instance, EvaluationContext ec);
		
		//UPGRADE_NOTE: Access modifiers of method 'apply' were changed to 'public'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1204'"
		public abstract void  apply(TreeReference ref_Renamed, System.Object result, FormInstance instance, FormDef f);
		
		public abstract bool canCascade();
		
		/// <summary> Not for re-implementation, dispatches all of the evaluation</summary>
		/// <param name="instance">
		/// </param>
		/// <param name="evalContext">
		/// </param>
		/// <param name="f">
		/// </param>
		public void  apply(FormInstance instance, EvaluationContext parentContext, TreeReference context, FormDef f)
		{
			//The triggeringRoot is the highest level of actual data we can inquire about, but it _isn't_ necessarily the basis
			//for the actual expressions, so we need genericize that ref against the current context
			TreeReference ungenericised = originalContextRef.contextualize(context);
			EvaluationContext ec = new EvaluationContext(parentContext, ungenericised);
			
			System.Object result = eval(instance, ec);
			
			for (int i = 0; i < targets.size(); i++)
			{
				TreeReference targetRef = ((TreeReference) targets.elementAt(i)).contextualize(ec.ContextRef);
				System.Collections.ArrayList v = ec.expandReference(targetRef);
				for (int j = 0; j < v.Count; j++)
				{
					TreeReference affectedRef = (TreeReference) v[j];
					apply(affectedRef, result, instance, f);
				}
			}
		}
		
		public virtual void  addTarget(TreeReference target)
		{
			if (targets.indexOf(target) == - 1)
			{
				targets.addElement(target);
			}
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public Vector < TreeReference > getTargets()
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public Set < TreeReference > getTriggers()
		
		public  override bool Equals(System.Object o)
		{
			if (o is Triggerable)
			{
				Triggerable t = (Triggerable) o;
				if (this == t)
					return true;
				
				if (this.expr.Equals(t.expr))
				{
					
					// The original logic did not make any sense --
					// the
					try
					{
						// resolved triggers should match...
						//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
						//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
						
						return (Atriggers.size() == Btriggers.size()) && Atriggers.containsAll(Btriggers);
					}
					catch (XPathException e)
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			expr = (IConditionExpr) ExtUtil.read(in_Renamed, new ExtWrapTagged(), pf);
			contextRef = (TreeReference) ExtUtil.read(in_Renamed, typeof(TreeReference), pf);
			originalContextRef = (TreeReference) ExtUtil.read(in_Renamed, typeof(TreeReference), pf);
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			targets =(Vector < TreeReference >) ExtUtil.read(in, new ExtWrapList(TreeReference.
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		class), pf);
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public abstract void  writeExternal(System.IO.BinaryWriter param1);
		//UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	void writeExternal(DataOutputStream out) throws IOException
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		ExtUtil.write(out, new ExtWrapTagged(expr));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(out, contextRef);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(out, originalContextRef);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	ExtUtil.write(out, new ExtWrapList(targets));
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	public String toString()
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		StringBuilder sb = new StringBuilder();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	for(int i = 0;
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	i < targets.size();
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	i ++)
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	{ 
		sb.append(((TreeReference) targets.elementAt(i)).toString());
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	if(i < targets.size() - 1) 
	sb.append(,);
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	} 
	return trig[expr: + expr.toString() + ;targets[ + sb.toString() + ]];
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
	//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
	}
}