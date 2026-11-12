using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Routing.Template;

public class TemplateBinder
{
	[DebuggerDisplay("{DebuggerToString(),nq}")]
	private struct TemplateBindingContext
	{
		private readonly RouteValueDictionary _defaults;

		private readonly RouteValueDictionary _acceptedValues;

		public RouteValueDictionary AcceptedValues => _acceptedValues;

		public TemplateBindingContext(RouteValueDictionary defaults)
		{
			_defaults = defaults;
			_acceptedValues = new RouteValueDictionary();
		}

		public void Accept(string key, object value)
		{
			if (!_acceptedValues.ContainsKey(key))
			{
				_acceptedValues.Add(key, value);
			}
		}

		public void AcceptDefault(string key)
		{
			if (_defaults != null && _defaults.TryGetValue(key, out var value))
			{
				_acceptedValues.Add(key, value);
			}
		}

		public bool NeedsValue(string key)
		{
			return !_acceptedValues.ContainsKey(key);
		}

		private string DebuggerToString()
		{
			return string.Format("{{Accepted: '{0}'}}", string.Join(", ", _acceptedValues.Keys));
		}
	}

	private readonly UrlEncoder _urlEncoder;

	private readonly ObjectPool<UriBuildingContext> _pool;

	private readonly RouteValueDictionary _defaults;

	private readonly RouteValueDictionary _filters;

	private readonly RouteTemplate _template;

	public TemplateBinder(UrlEncoder urlEncoder, ObjectPool<UriBuildingContext> pool, RouteTemplate template, RouteValueDictionary defaults)
	{
		if (urlEncoder == null)
		{
			throw new ArgumentNullException("urlEncoder");
		}
		if (pool == null)
		{
			throw new ArgumentNullException("pool");
		}
		if (template == null)
		{
			throw new ArgumentNullException("template");
		}
		_urlEncoder = urlEncoder;
		_pool = pool;
		_template = template;
		_defaults = defaults;
		_filters = new RouteValueDictionary(_defaults);
		foreach (TemplatePart parameter in _template.Parameters)
		{
			_filters.Remove(parameter.Name);
		}
	}

	public TemplateValuesResult GetValues(RouteValueDictionary ambientValues, RouteValueDictionary values)
	{
		TemplateBindingContext templateBindingContext = new TemplateBindingContext(_defaults);
		for (int i = 0; i < _template.Parameters.Count; i++)
		{
			TemplatePart templatePart = _template.Parameters[i];
			string name = templatePart.Name;
			object value;
			bool flag = values.TryGetValue(name, out value);
			object value2 = null;
			bool flag2 = ambientValues?.TryGetValue(name, out value2) ?? false;
			if (flag && flag2 && !RoutePartsEqual(value2, value))
			{
				break;
			}
			if (!flag && !flag2)
			{
				RouteValueDictionary defaults = _defaults;
				if (defaults == null || !defaults.ContainsKey(templatePart.Name))
				{
					break;
				}
			}
			if (flag)
			{
				if (IsRoutePartNonEmpty(value))
				{
					templateBindingContext.Accept(name, value);
				}
			}
			else if (flag2)
			{
				templateBindingContext.Accept(name, value2);
			}
		}
		foreach (KeyValuePair<string, object> value4 in values)
		{
			if (IsRoutePartNonEmpty(value4.Value))
			{
				templateBindingContext.Accept(value4.Key, value4.Value);
			}
		}
		for (int j = 0; j < _template.Parameters.Count; j++)
		{
			TemplatePart templatePart2 = _template.Parameters[j];
			if (!templatePart2.IsOptional && !templatePart2.IsCatchAll && templateBindingContext.NeedsValue(templatePart2.Name))
			{
				templateBindingContext.AcceptDefault(templatePart2.Name);
			}
		}
		for (int k = 0; k < _template.Parameters.Count; k++)
		{
			TemplatePart templatePart3 = _template.Parameters[k];
			if (!templatePart3.IsOptional && !templatePart3.IsCatchAll && !templateBindingContext.AcceptedValues.ContainsKey(templatePart3.Name))
			{
				return null;
			}
		}
		foreach (KeyValuePair<string, object> filter in _filters)
		{
			if (GetParameter(filter.Key) == null && values.TryGetValue(filter.Key, out var value3) && !RoutePartsEqual(value3, filter.Value))
			{
				return null;
			}
		}
		RouteValueDictionary routeValueDictionary = new RouteValueDictionary(templateBindingContext.AcceptedValues);
		if (ambientValues != null)
		{
			foreach (KeyValuePair<string, object> ambientValue in ambientValues)
			{
				if (IsRoutePartNonEmpty(ambientValue.Value) && GetParameter(ambientValue.Key) == null && !templateBindingContext.AcceptedValues.ContainsKey(ambientValue.Key))
				{
					routeValueDictionary.Add(ambientValue.Key, ambientValue.Value);
				}
			}
		}
		return new TemplateValuesResult
		{
			AcceptedValues = templateBindingContext.AcceptedValues,
			CombinedValues = routeValueDictionary
		};
	}

	public string BindValues(RouteValueDictionary acceptedValues)
	{
		UriBuildingContext uriBuildingContext = _pool.Get();
		string result = BindValues(uriBuildingContext, acceptedValues);
		_pool.Return(uriBuildingContext);
		return result;
	}

	private string BindValues(UriBuildingContext context, RouteValueDictionary acceptedValues)
	{
		for (int i = 0; i < _template.Segments.Count; i++)
		{
			TemplateSegment templateSegment = _template.Segments[i];
			for (int j = 0; j < templateSegment.Parts.Count; j++)
			{
				TemplatePart templatePart = templateSegment.Parts[j];
				if (templatePart.IsLiteral)
				{
					if (!context.Accept(templatePart.Text))
					{
						return null;
					}
				}
				else
				{
					if (!templatePart.IsParameter)
					{
						continue;
					}
					if (acceptedValues.TryGetValue(templatePart.Name, out var value))
					{
						acceptedValues.Remove(templatePart.Name);
					}
					bool flag = false;
					if (_defaults != null && _defaults.TryGetValue(templatePart.Name, out var value2) && RoutePartsEqual(value, value2))
					{
						flag = true;
					}
					string value3 = Convert.ToString(value, CultureInfo.InvariantCulture);
					if (flag)
					{
						if (!context.Buffer(value3))
						{
							return null;
						}
					}
					else if (!context.Accept(value3))
					{
						if (j == 0 || !templatePart.IsOptional || !templateSegment.Parts[j - 1].IsOptionalSeperator)
						{
							return null;
						}
						context.Remove(templateSegment.Parts[j - 1].Text);
					}
				}
			}
			context.EndSegment();
		}
		bool flag2 = false;
		foreach (KeyValuePair<string, object> acceptedValue in acceptedValues)
		{
			if (_defaults != null && _defaults.ContainsKey(acceptedValue.Key))
			{
				continue;
			}
			if (acceptedValue.Value is IEnumerable enumerable && !(enumerable is string))
			{
				foreach (object item in enumerable)
				{
					flag2 |= AddParameterToContext(context, acceptedValue.Key, item, flag2);
				}
			}
			else
			{
				flag2 |= AddParameterToContext(context, acceptedValue.Key, acceptedValue.Value, flag2);
			}
		}
		return context.ToString();
	}

	private bool AddParameterToContext(UriBuildingContext context, string key, object value, bool wroteFirst)
	{
		string value2 = Convert.ToString(value, CultureInfo.InvariantCulture);
		if (!string.IsNullOrEmpty(value2))
		{
			context.Writer.Write(wroteFirst ? '&' : '?');
			_urlEncoder.Encode(context.Writer, key);
			context.Writer.Write('=');
			_urlEncoder.Encode(context.Writer, value2);
			return true;
		}
		return false;
	}

	private TemplatePart GetParameter(string name)
	{
		for (int i = 0; i < _template.Parameters.Count; i++)
		{
			TemplatePart templatePart = _template.Parameters[i];
			if (string.Equals(templatePart.Name, name, StringComparison.OrdinalIgnoreCase))
			{
				return templatePart;
			}
		}
		return null;
	}

	public static bool RoutePartsEqual(object a, object b)
	{
		string text = a as string;
		string text2 = b as string;
		if (text != null && text2 != null)
		{
			return string.Equals(text, text2, StringComparison.OrdinalIgnoreCase);
		}
		if (a != null && b != null)
		{
			return a.Equals(b);
		}
		return a == b;
	}

	private static bool IsRoutePartNonEmpty(object routePart)
	{
		if (!(routePart is string text))
		{
			return routePart != null;
		}
		return text.Length > 0;
	}
}
