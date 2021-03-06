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
using UnpivotableExpressionException = org.javarosa.core.model.condition.pivot.UnpivotableExpressionException;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using System.Collections.Generic;
namespace org.javarosa.core.model.condition
{

    /// <summary> A condition expression is an expression which is evaluated against the current
    /// model and produces a value. These objects should keep track of the expression that
    /// they evaluate, as well as being able to identify what references will require the
    /// condition to be triggered.
    /// 
    /// As a metaphor into an XForm, a Condition expression represents an XPath expression
    /// which you can query for a value (in a calculate or relevancy condition, for instance),
    /// can tell you what nodes its value will depend on, and optionally what values it "Pivots"
    /// around. (IE: if an expression's value is true if a node is > 25, and false otherwise, it
    /// has a "Comparison Pivot" around 25).
    /// 
    /// </summary>
    /// <author>  ctsims
    /// 
    /// </author>
    public interface IConditionExpr : Externalizable
    {

        /// <summary> Evaluate this expression against the current models and
        /// context and provide a true or false value.
        /// 
        /// </summary>
        /// <param name="model">
        /// </param>
        /// <param name="evalContext">
        /// </param>
        /// <returns>
        /// </returns>
        bool eval(FormInstance model, EvaluationContext evalContext);

        /// <summary> Evaluate this expression against the current models and
        /// context and provide the final value of the expression, without
        /// forcing a cast to a boolean value.
        /// 
        /// </summary>
        /// <param name="model">
        /// </param>
        /// <param name="evalContext">
        /// </param>
        /// <returns>
        /// </returns>
        System.Object evalRaw(FormInstance model, EvaluationContext evalContext);

        /// <summary> Used for itemsets. Fill this documentation in.
        /// 
        /// </summary>
        /// <param name="model">
        /// </param>
        /// <param name="evalContext">
        /// </param>
        /// <returns>
        /// </returns>
        System.String evalReadable(FormInstance model, EvaluationContext evalContext);

        /// <summary> Used for itemsets. Fill this documentation in.
        /// 
        /// </summary>
        /// <param name="model">
        /// </param>
        /// <param name="evalContext">
        /// </param>
        /// <returns>
        /// </returns>
        List<TreeReference> evalNodeset(FormInstance model, EvaluationContext evalContext);

        /// <summary> Provides a list of all of the references that this expression's value depends upon
        /// directly. These values can't be contextualized fully (since these triggers are necessary
        /// before runtime), but should only need to be contextualized to be a complete set.
        /// 
        /// </summary>
        /// <returns>
        /// </returns>
        List<TreeReference> getTriggers(TreeReference contextRef); /* unordered set of TreeReference */

        /// <summary> Provide a list of Pivots around which this Condition Expression depends.
        /// 
        /// Optional to implement. If not implemented, throw an Unpivotable Expression exception
        /// to signal that the expression cannot be statically evaluated.
        /// 
        /// </summary>
        /// <param name="model">
        /// </param>
        /// <param name="evalContext">
        /// </param>
        /// <returns>
        /// </returns>
        /// <throws>  UnpivotableExpressionException </throws>
        List<Object> pivot(FormInstance model, EvaluationContext evalContext);
    }
}