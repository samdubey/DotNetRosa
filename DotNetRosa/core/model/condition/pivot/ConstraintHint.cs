/// <summary> </summary>
using System;
using EvaluationContext = org.javarosa.core.model.condition.EvaluationContext;
using IConditionExpr = org.javarosa.core.model.condition.IConditionExpr;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
namespace org.javarosa.core.model.condition.pivot
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public interface ConstraintHint
	{
		
		void  init(EvaluationContext c, IConditionExpr conditional, FormInstance instance);
	}
}