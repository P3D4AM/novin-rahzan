using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Microsoft.AspNetCore.Routing;

public class RouteConstraintBuilder
{
	private readonly IInlineConstraintResolver _inlineConstraintResolver;

	private readonly string _displayName;

	private readonly Dictionary<string, List<IRouteConstraint>> _constraints;

	private readonly HashSet<string> _optionalParameters;

	public RouteConstraintBuilder(IInlineConstraintResolver inlineConstraintResolver, string displayName)
	{
		if (inlineConstraintResolver == null)
		{
			throw new ArgumentNullException("inlineConstraintResolver");
		}
		if (displayName == null)
		{
			throw new ArgumentNullException("displayName");
		}
		_inlineConstraintResolver = inlineConstraintResolver;
		_displayName = displayName;
		_constraints = new Dictionary<string, List<IRouteConstraint>>(StringComparer.OrdinalIgnoreCase);
		_optionalParameters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
	}

	public IDictionary<string, IRouteConstraint> Build()
	{
		Dictionary<string, IRouteConstraint> dictionary = new Dictionary<string, IRouteConstraint>(StringComparer.OrdinalIgnoreCase);
		foreach (KeyValuePair<string, List<IRouteConstraint>> constraint in _constraints)
		{
			IRouteConstraint routeConstraint = ((constraint.Value.Count != 1) ? new CompositeRouteConstraint(constraint.Value.ToArray()) : constraint.Value[0]);
			if (_optionalParameters.Contains(constraint.Key))
			{
				OptionalRouteConstraint value = new OptionalRouteConstraint(routeConstraint);
				dictionary.Add(constraint.Key, value);
			}
			else
			{
				dictionary.Add(constraint.Key, routeConstraint);
			}
		}
		return dictionary;
	}

	public void AddConstraint(string key, object value)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		IRouteConstraint routeConstraint = value as IRouteConstraint;
		if (routeConstraint == null)
		{
			if (!(value is string text))
			{
				throw new RouteCreationException(Resources.FormatRouteConstraintBuilder_ValidationMustBeStringOrCustomConstraint(key, value, _displayName, typeof(IRouteConstraint)));
			}
			routeConstraint = new RegexRouteConstraint("^(" + text + ")$");
		}
		Add(key, routeConstraint);
	}

	public void AddResolvedConstraint(string key, string constraintText)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (constraintText == null)
		{
			throw new ArgumentNullException("constraintText");
		}
		IRouteConstraint routeConstraint = _inlineConstraintResolver.ResolveConstraint(constraintText);
		if (routeConstraint == null)
		{
			throw new InvalidOperationException(Resources.FormatRouteConstraintBuilder_CouldNotResolveConstraint(key, constraintText, _displayName, _inlineConstraintResolver.GetType().Name));
		}
		Add(key, routeConstraint);
	}

	public void SetOptional(string key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		_optionalParameters.Add(key);
	}

	private void Add(string key, IRouteConstraint constraint)
	{
		if (!_constraints.TryGetValue(key, out var value))
		{
			value = new List<IRouteConstraint>();
			_constraints.Add(key, value);
		}
		value.Add(constraint);
	}
}
