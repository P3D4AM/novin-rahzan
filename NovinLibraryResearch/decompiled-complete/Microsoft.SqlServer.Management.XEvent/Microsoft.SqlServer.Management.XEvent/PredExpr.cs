using System.Xml;

namespace Microsoft.SqlServer.Management.XEvent;

public abstract class PredExpr : Predicate
{
	private static BaseXEStore xeStore;

	internal static PredExpr ParsePredicateXml(BaseXEStore store, string predicateXml)
	{
		if (predicateXml == null || predicateXml.Trim() == string.Empty)
		{
			return null;
		}
		xeStore = store;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(predicateXml);
		return RecursiveTraverse((XmlElement)xmlDocument.FirstChild);
	}

	private static PredExpr RecursiveTraverse(XmlElement element)
	{
		if (element.Name == "leaf")
		{
			return ParseLeaf(element);
		}
		PredExpr predExpr = RecursiveTraverse((XmlElement)element.FirstChild);
		if (element.Name == "not")
		{
			return new PredLogicalExpr(PredLogicalExpr.LogicalOperatorType.Not, predExpr, null);
		}
		PredExpr predExpr2 = RecursiveTraverse((XmlElement)element.ChildNodes[1]);
		if (element.Name == "and")
		{
			return new PredLogicalExpr(PredLogicalExpr.LogicalOperatorType.And, predExpr, predExpr2);
		}
		return new PredLogicalExpr(PredLogicalExpr.LogicalOperatorType.Or, predExpr, predExpr2);
	}

	private static PredExpr ParseLeaf(XmlElement leaf)
	{
		XmlElement obj = (XmlElement)leaf.FirstChild;
		string value = obj.Attributes["package"].Value;
		string value2 = obj.Attributes["name"].Value;
		PredCompareInfo func = xeStore.ObjectInfoSet.Get<PredCompareInfo>(value, value2);
		XmlElement xmlElement = (XmlElement)leaf.ChildNodes[1];
		value = xmlElement.Attributes["package"].Value;
		value2 = xmlElement.Attributes["name"].Value;
		PredOperand predOperand = null;
		if (xmlElement.Name == "event")
		{
			string value3 = xmlElement.Attributes["field"].Value;
			predOperand = new PredOperand(xeStore.ObjectInfoSet.Get<EventInfo>(value, value2).DataEventColumnInfoSet[value3]);
		}
		else
		{
			predOperand = new PredOperand(xeStore.ObjectInfoSet.Get<PredSourceInfo>(value, value2));
		}
		PredValue predValue = null;
		predValue = new PredValue(((XmlElement)leaf.ChildNodes[2]).InnerText);
		return new PredFunctionExpr(func, predOperand, predValue);
	}
}
