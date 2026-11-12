using System.ComponentModel;

namespace Stimulsoft.Base;

public static class StiAppExpressionHelper
{
	public static bool IsExpressionSpecified(object component, string propName)
	{
		return !string.IsNullOrWhiteSpace(GetExpression(component, propName)?.Expression);
	}

	public static StiAppExpression GetExpression(object component, string propName)
	{
		IStiAppExpressionCollection stiAppExpressionCollection = component as IStiAppExpressionCollection;
		if (stiAppExpressionCollection?.Expressions == null)
		{
			return null;
		}
		return stiAppExpressionCollection?.Expressions[propName];
	}

	public static string GetExpressionValue(object component, string propName)
	{
		return GetExpression(component, propName)?.Expression;
	}

	public static void SetExpression(object component, string propName, string expression)
	{
		if (component is IStiAppExpressionCollection stiAppExpressionCollection)
		{
			if (stiAppExpressionCollection.Expressions == null)
			{
				stiAppExpressionCollection.Expressions = new StiAppExpressionCollection();
			}
			stiAppExpressionCollection.Expressions[propName] = new StiAppExpression(propName, expression);
		}
	}

	public static void RemoveExpression(object component, string propName)
	{
		if (component is IStiAppExpressionCollection stiAppExpressionCollection)
		{
			stiAppExpressionCollection.Expressions?.Remove(propName);
		}
	}

	public static string GetExpressionFromInstance(ITypeDescriptorContext context)
	{
		return GetExpressionFromInstance(context?.Instance, context?.PropertyDescriptor?.Name);
	}

	public static string GetExpressionFromInstance(object instance, string propertyName)
	{
		if (instance == null || string.IsNullOrEmpty(propertyName))
		{
			return null;
		}
		IStiAppExpressionCollection stiAppExpressionCollection = instance as IStiAppExpressionCollection;
		if (stiAppExpressionCollection?.Expressions == null || stiAppExpressionCollection.Expressions[propertyName] == null)
		{
			return null;
		}
		StiAppExpression stiAppExpression = stiAppExpressionCollection.Expressions[propertyName];
		if (string.IsNullOrWhiteSpace(stiAppExpression?.Expression))
		{
			return null;
		}
		return stiAppExpression.Expression;
	}
}
