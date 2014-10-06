using System;
using EvaluationContext = org.javarosa.core.model.condition.EvaluationContext;
using IAnswerData = org.javarosa.core.model.data.IAnswerData;
using ITreeVisitor = org.javarosa.core.model.instance.utils.ITreeVisitor;
using XPathExpression = org.javarosa.xpath.expr.XPathExpression;
namespace org.javarosa.core.model.instance
{
	
	
	public
	
	interface AbstractTreeElement < T extends AbstractTreeElement >
	
	{
	
	
	public
	
	abstract boolean isLeaf();
	
	
	public
	
	abstract boolean isChildable();
	
	
	public
	
	abstract String getInstanceName();
	
	
	public
	
	abstract T getChild(String name, int multiplicity);
	
	/// <summary> 
	/// Get all the child nodes of this element, with specific name
	/// 
	/// </summary>
	/// <param name="name">
	/// </param>
	/// <returns>
	/// </returns>
	
	public
	
	abstract List< T > getChildrenWithName(String name);
	
	
	public
	
	abstract boolean hasChildren();
	
	
	public
	
	abstract int getNumChildren();
	
	
	public
	
	abstract T getChildAt(int i);
	
	
	public
	
	abstract boolean isRepeatable();
	
	
	public
	
	abstract boolean isAttribute();
	
	
	public
	
	abstract int getChildMultiplicity(String name);
	
	/// <summary> Visitor pattern acceptance method.
	/// 
	/// </summary>
	/// <param name="visitor">The visitor traveling this tree
	/// </param>
	
	public
	
	abstract
	
	void accept(ITreeVisitor visitor);
	
	/// <summary> Returns the number of attributes of this element.</summary>
	
	public
	
	abstract int getAttributeCount();
	
	/// <summary> get namespace of attribute at 'index' in the vector
	/// 
	/// </summary>
	/// <param name="index">
	/// </param>
	/// <returns> String
	/// </returns>
	
	public
	
	abstract String getAttributeNamespace(int index);
	
	/// <summary> get name of attribute at 'index' in the vector
	/// 
	/// </summary>
	/// <param name="index">
	/// </param>
	/// <returns> String
	/// </returns>
	
	public
	
	abstract String getAttributeName(int index);
	
	/// <summary> get value of attribute at 'index' in the vector
	/// 
	/// </summary>
	/// <param name="index">
	/// </param>
	/// <returns> String
	/// </returns>
	
	public
	
	abstract String getAttributeValue(int index);
	
	/// <summary> Retrieves the TreeElement representing the attribute at
	/// the provided namespace and name, or null if none exists.
	/// 
	/// If 'null' is provided for the namespace, it will match the first
	/// attribute with the matching name.
	/// 
	/// </summary>
	/// <param name="index">
	/// </param>
	/// <returns> TreeElement
	/// </returns>
	
	public
	
	abstract T getAttribute(String namespace, String name);
	
	/// <summary> get value of attribute with namespace:name' in the vector
	/// 
	/// </summary>
	/// <param name="index">
	/// </param>
	/// <returns> String
	/// </returns>
	
	public
	
	abstract String getAttributeValue(String namespace, String name);
	
	//return the tree reference that corresponds to this tree element
	
	public
	
	abstract TreeReference getRef();
	
	
	public
	
	abstract int getDepth();
	
	
	public
	
	abstract String getName();
	
	
	public
	
	abstract int getMult();
	
	//Support? 
	
	public
	
	abstract AbstractTreeElement getParent();
	
	
	public
	
	abstract IAnswerData getValue();
	
	
	public
	
	abstract int getDataType();
	
	
	public
	
	abstract
	
	void clearCaches();
	
	
	public
	
	abstract boolean isRelevant();
	
	
	public
	
	abstract String getNamespace();
	
	/// <summary> TODO: Worst method name ever. Don't use this unless you know what's up.
	/// 
	/// </summary>
	/// <param name="name">
	/// </param>
	/// <param name="mult">
	/// </param>
	/// <param name="predicates">possibly list of predicates to be evaluated. predicates will be removed from list if they are 
	/// able to be evaluated
	/// </param>
	/// <param name="evalContext">
	/// </param>
	/// <returns>
	/// </returns>
	
	public
	
	abstract List< TreeReference > tryBatchChildFetch(String name, int mult, List< XPathExpression > predicates, EvaluationContext evalContext);
	
	}
}