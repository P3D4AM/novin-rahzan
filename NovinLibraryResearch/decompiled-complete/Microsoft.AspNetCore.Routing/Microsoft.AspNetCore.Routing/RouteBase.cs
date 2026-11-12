using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.AspNetCore.Routing.Logging;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Routing;

public abstract class RouteBase : IRouter, INamedRouter
{
	private TemplateMatcher _matcher;

	private TemplateBinder _binder;

	private ILogger _logger;

	private ILogger _constraintLogger;

	public virtual IDictionary<string, IRouteConstraint> Constraints { get; protected set; }

	protected virtual IInlineConstraintResolver ConstraintResolver { get; set; }

	public virtual RouteValueDictionary DataTokens { get; protected set; }

	public virtual RouteValueDictionary Defaults { get; protected set; }

	public virtual string Name { get; protected set; }

	public virtual RouteTemplate ParsedTemplate { get; protected set; }

	public RouteBase(string template, string name, IInlineConstraintResolver constraintResolver, RouteValueDictionary defaults, IDictionary<string, object> constraints, RouteValueDictionary dataTokens)
	{
		if (constraintResolver == null)
		{
			throw new ArgumentNullException("constraintResolver");
		}
		template = template ?? string.Empty;
		Name = name;
		ConstraintResolver = constraintResolver;
		DataTokens = dataTokens ?? new RouteValueDictionary();
		try
		{
			ParsedTemplate = TemplateParser.Parse(template);
			Constraints = GetConstraints(constraintResolver, ParsedTemplate, constraints);
			Defaults = GetDefaults(ParsedTemplate, defaults);
		}
		catch (Exception innerException)
		{
			throw new RouteCreationException(Resources.FormatTemplateRoute_Exception(name, template), innerException);
		}
	}

	protected abstract Task OnRouteMatched(RouteContext context);

	protected abstract VirtualPathData OnVirtualPathGenerated(VirtualPathContext context);

	public virtual Task RouteAsync(RouteContext context)
	{
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		EnsureMatcher();
		EnsureLoggers(context.HttpContext);
		PathString path = context.HttpContext.Request.Path;
		if (!_matcher.TryMatch(path, context.RouteData.Values))
		{
			return Task.CompletedTask;
		}
		if (DataTokens.Count > 0)
		{
			MergeValues(context.RouteData.DataTokens, DataTokens);
		}
		if (!RouteConstraintMatcher.Match(Constraints, context.RouteData.Values, context.HttpContext, this, RouteDirection.IncomingRequest, _constraintLogger))
		{
			return Task.CompletedTask;
		}
		_logger.MatchedRoute(Name, ParsedTemplate.TemplateText);
		return OnRouteMatched(context);
	}

	public virtual VirtualPathData GetVirtualPath(VirtualPathContext context)
	{
		EnsureBinder(context.HttpContext);
		EnsureLoggers(context.HttpContext);
		TemplateValuesResult values = _binder.GetValues(context.AmbientValues, context.Values);
		if (values == null)
		{
			return null;
		}
		if (!RouteConstraintMatcher.Match(Constraints, values.CombinedValues, context.HttpContext, this, RouteDirection.UrlGeneration, _constraintLogger))
		{
			return null;
		}
		context.Values = values.CombinedValues;
		VirtualPathData virtualPathData = OnVirtualPathGenerated(context);
		if (virtualPathData != null)
		{
			return virtualPathData;
		}
		string text = _binder.BindValues(values.AcceptedValues);
		if (text == null)
		{
			return null;
		}
		virtualPathData = new VirtualPathData(this, text);
		if (DataTokens != null)
		{
			foreach (KeyValuePair<string, object> dataToken in DataTokens)
			{
				virtualPathData.DataTokens.Add(dataToken.Key, dataToken.Value);
			}
		}
		return virtualPathData;
	}

	protected static IDictionary<string, IRouteConstraint> GetConstraints(IInlineConstraintResolver inlineConstraintResolver, RouteTemplate parsedTemplate, IDictionary<string, object> constraints)
	{
		RouteConstraintBuilder routeConstraintBuilder = new RouteConstraintBuilder(inlineConstraintResolver, parsedTemplate.TemplateText);
		if (constraints != null)
		{
			foreach (KeyValuePair<string, object> constraint in constraints)
			{
				routeConstraintBuilder.AddConstraint(constraint.Key, constraint.Value);
			}
		}
		foreach (TemplatePart parameter in parsedTemplate.Parameters)
		{
			if (parameter.IsOptional)
			{
				routeConstraintBuilder.SetOptional(parameter.Name);
			}
			foreach (InlineConstraint inlineConstraint in parameter.InlineConstraints)
			{
				routeConstraintBuilder.AddResolvedConstraint(parameter.Name, inlineConstraint.Constraint);
			}
		}
		return routeConstraintBuilder.Build();
	}

	protected static RouteValueDictionary GetDefaults(RouteTemplate parsedTemplate, RouteValueDictionary defaults)
	{
		RouteValueDictionary routeValueDictionary = ((defaults == null) ? new RouteValueDictionary() : new RouteValueDictionary(defaults));
		foreach (TemplatePart parameter in parsedTemplate.Parameters)
		{
			if (parameter.DefaultValue != null)
			{
				if (routeValueDictionary.ContainsKey(parameter.Name))
				{
					throw new InvalidOperationException(Resources.FormatTemplateRoute_CannotHaveDefaultValueSpecifiedInlineAndExplicitly(parameter.Name));
				}
				routeValueDictionary.Add(parameter.Name, parameter.DefaultValue);
			}
		}
		return routeValueDictionary;
	}

	private static void MergeValues(RouteValueDictionary destination, RouteValueDictionary values)
	{
		foreach (KeyValuePair<string, object> value in values)
		{
			destination[value.Key] = value.Value;
		}
	}

	private void EnsureBinder(HttpContext context)
	{
		if (_binder == null)
		{
			ObjectPool<UriBuildingContext> requiredService = context.RequestServices.GetRequiredService<ObjectPool<UriBuildingContext>>();
			_binder = new TemplateBinder(UrlEncoder.Default, requiredService, ParsedTemplate, Defaults);
		}
	}

	private void EnsureLoggers(HttpContext context)
	{
		if (_logger == null)
		{
			ILoggerFactory requiredService = context.RequestServices.GetRequiredService<ILoggerFactory>();
			_logger = requiredService.CreateLogger(typeof(RouteBase).FullName);
			_constraintLogger = requiredService.CreateLogger(typeof(RouteConstraintMatcher).FullName);
		}
	}

	private void EnsureMatcher()
	{
		if (_matcher == null)
		{
			_matcher = new TemplateMatcher(ParsedTemplate, Defaults);
		}
	}

	public override string ToString()
	{
		return ParsedTemplate.TemplateText;
	}
}
