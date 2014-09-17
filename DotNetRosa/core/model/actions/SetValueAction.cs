/// <summary> </summary>
using System;
using Action = org.javarosa.core.model.Action;
using FormDef = org.javarosa.core.model.FormDef;
using EvaluationContext = org.javarosa.core.model.condition.EvaluationContext;
using Recalculate = org.javarosa.core.model.condition.Recalculate;
using AnswerDataFactory = org.javarosa.core.model.data.AnswerDataFactory;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
//UPGRADE_TODO: The type 'org.javarosa.core.model.instance.AbstractTreeElement' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AbstractTreeElement = org.javarosa.core.model.instance.AbstractTreeElement;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapTagged = org.javarosa.core.util.externalizable.ExtWrapTagged;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using XPathExpression = org.javarosa.xpath.expr.XPathExpression;
using XPathFuncExpr = org.javarosa.xpath.expr.XPathFuncExpr;
namespace org.javarosa.core.model.actions
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class SetValueAction:Action
	{
		private TreeReference target;
		private XPathExpression value_Renamed;
		private System.String explicitValue;
		
		public SetValueAction()
		{
		}
		
		public SetValueAction(TreeReference target, XPathExpression value_Renamed):base("setvalue")
		{
			this.target = target;
			this.value_Renamed = value_Renamed;
		}
		
		public SetValueAction(TreeReference target, System.String explicitValue):base("setvalue")
		{
			this.target = target;
			this.explicitValue = explicitValue;
		}
		
		public override void  processAction(FormDef model, TreeReference contextRef)
		{
			
			//Qualify the reference if necessary
			TreeReference qualifiedReference = contextRef == null?target:target.contextualize(contextRef);
			
			//For now we only process setValue actions which are within the
			//context if a context is provided. This happens for repeats where
			//insert events should only trigger on the right nodes
			if (contextRef != null)
			{
				
				//Note: right now we're qualifying then testing parentage to see wheter
				//there was a conflict, but it's not super clear whether this is a perfect
				//strategy
				if (!contextRef.isParentOf(qualifiedReference, false))
				{
					return ;
				}
			}
			
			//TODO: either the target or the value's node might not exist here, catch and throw
			//reasonably
			EvaluationContext context = new EvaluationContext(model.EvaluationContext, qualifiedReference);
			
			System.Object result;
			
			if (explicitValue != null)
			{
				result = explicitValue;
			}
			else
			{
				result = XPathFuncExpr.unpack(value_Renamed.eval(model.MainInstance, context));
			}
			
			AbstractTreeElement node = context.resolveReference(qualifiedReference);
			if (node == null)
			{
				throw new System.NullReferenceException("Target of TreeReference " + qualifiedReference.toString(true) + " could not be resolved!");
			}
			int dataType = node.getDataType();
			IAnswerData val = Recalculate.wrapData(result, dataType);
			
			model.setValue(val == null?null:AnswerDataFactory.templateByDataType(dataType).cast(val.uncast()), qualifiedReference);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			target = (TreeReference) ExtUtil.read(in_Renamed, typeof(TreeReference), pf);
			explicitValue = ExtUtil.nullIfEmpty(ExtUtil.readString(in_Renamed));
			if (explicitValue == null)
			{
				value_Renamed = (XPathExpression) ExtUtil.read(in_Renamed, new ExtWrapTagged(), pf);
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.write(out_Renamed, target);
			
			ExtUtil.write(out_Renamed, ExtUtil.emptyIfNull(explicitValue));
			if (explicitValue == null)
			{
				ExtUtil.write(out_Renamed, new ExtWrapTagged(value_Renamed));
			}
		}
	}
}