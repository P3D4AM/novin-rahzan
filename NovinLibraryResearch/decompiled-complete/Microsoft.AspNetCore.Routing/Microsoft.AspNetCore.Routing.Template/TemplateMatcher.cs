using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.Extensions.Primitives;

namespace Microsoft.AspNetCore.Routing.Template;

public class TemplateMatcher
{
	private const string SeparatorString = "/";

	private const char SeparatorChar = '/';

	private readonly bool[] _hasDefaultValue;

	private readonly object[] _defaultValues;

	private static readonly char[] Delimiters = new char[1] { '/' };

	public RouteValueDictionary Defaults { get; }

	public RouteTemplate Template { get; }

	public TemplateMatcher(RouteTemplate template, RouteValueDictionary defaults)
	{
		if (template == null)
		{
			throw new ArgumentNullException("template");
		}
		Template = template;
		Defaults = defaults ?? new RouteValueDictionary();
		_hasDefaultValue = new bool[Template.Segments.Count];
		_defaultValues = new object[Template.Segments.Count];
		for (int i = 0; i < Template.Segments.Count; i++)
		{
			TemplateSegment templateSegment = Template.Segments[i];
			if (templateSegment.IsSimple)
			{
				TemplatePart templatePart = templateSegment.Parts[0];
				if (templatePart.IsParameter && Defaults.TryGetValue(templatePart.Name, out var value))
				{
					_hasDefaultValue[i] = true;
					_defaultValues[i] = value;
				}
			}
		}
	}

	public bool TryMatch(PathString path, RouteValueDictionary values)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		int i = 0;
		PathTokenizer pathTokenizer = new PathTokenizer(path);
		foreach (StringSegment item in pathTokenizer)
		{
			if (item.Length == 0)
			{
				return false;
			}
			TemplateSegment segment = Template.GetSegment(i++);
			if (segment == null && item.Length > 0)
			{
				return false;
			}
			if (segment.IsSimple && segment.Parts[0].IsLiteral)
			{
				TemplatePart templatePart = segment.Parts[0];
				if (!item.Equals(templatePart.Text, StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
				continue;
			}
			if (segment.IsSimple && segment.Parts[0].IsCatchAll)
			{
				break;
			}
			if (segment.IsSimple && segment.Parts[0].IsParameter)
			{
				TemplatePart templatePart2 = segment.Parts[0];
				if (item.Length == 0 && !_hasDefaultValue[i] && !templatePart2.IsOptional)
				{
					return false;
				}
			}
		}
		for (; i < Template.Segments.Count; i++)
		{
			TemplateSegment segment2 = Template.GetSegment(i);
			if (!segment2.IsSimple)
			{
				return false;
			}
			TemplatePart templatePart3 = segment2.Parts[0];
			if (templatePart3.IsLiteral)
			{
				return false;
			}
			if (templatePart3.IsCatchAll)
			{
				break;
			}
			if (!_hasDefaultValue[i] && !templatePart3.IsOptional)
			{
				return false;
			}
		}
		i = 0;
		foreach (StringSegment item2 in pathTokenizer)
		{
			TemplateSegment segment3 = Template.GetSegment(i++);
			if (segment3.IsSimple && segment3.Parts[0].IsCatchAll)
			{
				TemplatePart templatePart4 = segment3.Parts[0];
				string text = item2.Buffer.Substring(item2.Offset);
				if (text.Length > 0)
				{
					values[templatePart4.Name] = text;
				}
				else
				{
					values[templatePart4.Name] = _defaultValues[i];
				}
				break;
			}
			if (segment3.IsSimple && segment3.Parts[0].IsParameter)
			{
				TemplatePart templatePart5 = segment3.Parts[0];
				if (item2.Length > 0)
				{
					values[templatePart5.Name] = item2.ToString();
				}
				else if (_hasDefaultValue[i])
				{
					values[templatePart5.Name] = _defaultValues[i];
				}
			}
			else if (!segment3.IsSimple && !MatchComplexSegment(segment3, item2.ToString(), Defaults, values))
			{
				return false;
			}
		}
		for (; i < Template.Segments.Count; i++)
		{
			TemplatePart templatePart6 = Template.GetSegment(i).Parts[0];
			if (_hasDefaultValue[i] || templatePart6.IsCatchAll)
			{
				object obj = _defaultValues[i];
				if (obj != null || !values.ContainsKey(templatePart6.Name))
				{
					values[templatePart6.Name] = obj;
				}
			}
		}
		foreach (KeyValuePair<string, object> @default in Defaults)
		{
			if (!values.ContainsKey(@default.Key))
			{
				values.Add(@default.Key, @default.Value);
			}
		}
		return true;
	}

	private bool MatchComplexSegment(TemplateSegment routeSegment, string requestSegment, IReadOnlyDictionary<string, object> defaults, RouteValueDictionary values)
	{
		int num = routeSegment.Parts.Count - 1;
		if (routeSegment.Parts[num].IsOptional && routeSegment.Parts[num - 1].IsOptionalSeperator)
		{
			if (MatchComplexSegmentCore(routeSegment, requestSegment, Defaults, values, num))
			{
				return true;
			}
			if (requestSegment.EndsWith(routeSegment.Parts[num - 1].Text, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			return MatchComplexSegmentCore(routeSegment, requestSegment, Defaults, values, num - 2);
		}
		return MatchComplexSegmentCore(routeSegment, requestSegment, Defaults, values, num);
	}

	private bool MatchComplexSegmentCore(TemplateSegment routeSegment, string requestSegment, IReadOnlyDictionary<string, object> defaults, RouteValueDictionary values, int indexOfLastSegmentUsed)
	{
		int num = requestSegment.Length;
		TemplatePart templatePart = null;
		TemplatePart templatePart2 = null;
		RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
		while (indexOfLastSegmentUsed >= 0)
		{
			int num2 = num;
			TemplatePart templatePart3 = routeSegment.Parts[indexOfLastSegmentUsed];
			if (templatePart3.IsParameter)
			{
				templatePart = templatePart3;
			}
			else
			{
				templatePart2 = templatePart3;
				int num3 = num - 1;
				if (templatePart != null)
				{
					num3--;
				}
				if (num3 < 0)
				{
					return false;
				}
				int num4 = requestSegment.LastIndexOf(templatePart3.Text, num3, StringComparison.OrdinalIgnoreCase);
				if (num4 == -1)
				{
					return false;
				}
				if (indexOfLastSegmentUsed == routeSegment.Parts.Count - 1 && num4 + templatePart3.Text.Length != requestSegment.Length)
				{
					return false;
				}
				num2 = num4;
			}
			if (templatePart != null && ((templatePart2 != null && templatePart3.IsLiteral) || indexOfLastSegmentUsed == 0))
			{
				int num5;
				int length;
				if (templatePart2 == null)
				{
					num5 = ((indexOfLastSegmentUsed != 0) ? num2 : 0);
					length = num;
				}
				else if (indexOfLastSegmentUsed == 0 && templatePart3.IsParameter)
				{
					num5 = 0;
					length = num;
				}
				else
				{
					num5 = num2 + templatePart2.Text.Length;
					length = num - num5;
				}
				string value = requestSegment.Substring(num5, length);
				if (string.IsNullOrEmpty(value))
				{
					return false;
				}
				routeValueDictionary.Add(templatePart.Name, value);
				templatePart = null;
				templatePart2 = null;
			}
			num = num2;
			indexOfLastSegmentUsed--;
		}
		if (num == 0 || routeSegment.Parts[0].IsParameter)
		{
			foreach (KeyValuePair<string, object> item in routeValueDictionary)
			{
				values.Add(item.Key, item.Value);
			}
			return true;
		}
		return false;
	}
}
