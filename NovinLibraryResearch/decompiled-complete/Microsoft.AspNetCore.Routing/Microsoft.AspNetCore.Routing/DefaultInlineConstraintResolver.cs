using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Routing;

public class DefaultInlineConstraintResolver : IInlineConstraintResolver
{
	private readonly IDictionary<string, Type> _inlineConstraintMap;

	public DefaultInlineConstraintResolver(IOptions<RouteOptions> routeOptions)
	{
		_inlineConstraintMap = routeOptions.Value.ConstraintMap;
	}

	public virtual IRouteConstraint ResolveConstraint(string inlineConstraint)
	{
		if (inlineConstraint == null)
		{
			throw new ArgumentNullException("inlineConstraint");
		}
		int num = inlineConstraint.IndexOf('(');
		string text;
		string argumentString;
		if (num >= 0 && inlineConstraint.EndsWith(")", StringComparison.Ordinal))
		{
			text = inlineConstraint.Substring(0, num);
			argumentString = inlineConstraint.Substring(num + 1, inlineConstraint.Length - num - 2);
		}
		else
		{
			text = inlineConstraint;
			argumentString = null;
		}
		if (!_inlineConstraintMap.TryGetValue(text, out var value))
		{
			return null;
		}
		if (!typeof(IRouteConstraint).GetTypeInfo().IsAssignableFrom(value.GetTypeInfo()))
		{
			throw new RouteCreationException(Resources.FormatDefaultInlineConstraintResolver_TypeNotConstraint(value, text, typeof(IRouteConstraint).Name));
		}
		try
		{
			return CreateConstraint(value, argumentString);
		}
		catch (RouteCreationException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw new RouteCreationException($"An error occurred while trying to create an instance of route constraint '{value.FullName}'.", innerException);
		}
	}

	private static IRouteConstraint CreateConstraint(Type constraintType, string argumentString)
	{
		if (argumentString == null)
		{
			return (IRouteConstraint)Activator.CreateInstance(constraintType);
		}
		TypeInfo typeInfo = constraintType.GetTypeInfo();
		ConstructorInfo constructorInfo = null;
		object[] array = null;
		ConstructorInfo[] array2 = typeInfo.DeclaredConstructors.ToArray();
		if (array2.Length == 1 && array2[0].GetParameters().Length == 1)
		{
			constructorInfo = array2[0];
			array = ConvertArguments(constructorInfo.GetParameters(), new string[1] { argumentString });
		}
		else
		{
			string[] arguments = (from argument in argumentString.Split(new char[1] { ',' })
				select argument.Trim()).ToArray();
			ConstructorInfo[] array3 = array2.Where((ConstructorInfo ci) => ci.GetParameters().Length == arguments.Length).ToArray();
			switch (array3.Length)
			{
			case 0:
				throw new RouteCreationException(Resources.FormatDefaultInlineConstraintResolver_CouldNotFindCtor(typeInfo.Name, arguments.Length));
			case 1:
				break;
			default:
				throw new RouteCreationException(Resources.FormatDefaultInlineConstraintResolver_AmbiguousCtors(typeInfo.Name, arguments.Length));
			}
			constructorInfo = array3[0];
			array = ConvertArguments(constructorInfo.GetParameters(), arguments);
		}
		return (IRouteConstraint)constructorInfo.Invoke(array);
	}

	private static object[] ConvertArguments(ParameterInfo[] parameterInfos, string[] arguments)
	{
		object[] array = new object[parameterInfos.Length];
		for (int i = 0; i < parameterInfos.Length; i++)
		{
			Type parameterType = parameterInfos[i].ParameterType;
			array[i] = Convert.ChangeType(arguments[i], parameterType, CultureInfo.InvariantCulture);
		}
		return array;
	}
}
