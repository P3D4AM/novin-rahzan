using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.AspNetCore.Routing.Template;

public static class TemplateParser
{
	private class TemplateParserContext
	{
		private readonly string _template;

		private int _index;

		private int? _mark;

		private HashSet<string> _parameterNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		public char Current
		{
			get
			{
				if (_index >= _template.Length || _index < 0)
				{
					return '\0';
				}
				return _template[_index];
			}
		}

		public string Error { get; set; }

		public HashSet<string> ParameterNames => _parameterNames;

		public TemplateParserContext(string template)
		{
			_template = template;
			_index = -1;
		}

		public bool Back()
		{
			return --_index >= 0;
		}

		public bool Next()
		{
			return ++_index < _template.Length;
		}

		public void Mark()
		{
			_mark = _index;
		}

		public string Capture()
		{
			if (_mark.HasValue)
			{
				string result = _template.Substring(_mark.Value, _index - _mark.Value);
				_mark = null;
				return result;
			}
			return null;
		}
	}

	private const char Separator = '/';

	private const char OpenBrace = '{';

	private const char CloseBrace = '}';

	private const char EqualsSign = '=';

	private const char QuestionMark = '?';

	private const char Asterisk = '*';

	private const string PeriodString = ".";

	public static RouteTemplate Parse(string routeTemplate)
	{
		if (routeTemplate == null)
		{
			routeTemplate = string.Empty;
		}
		TemplateParserContext templateParserContext = new TemplateParserContext(TrimPrefix(routeTemplate));
		List<TemplateSegment> segments = new List<TemplateSegment>();
		while (templateParserContext.Next())
		{
			if (templateParserContext.Current == '/')
			{
				throw new ArgumentException(Resources.TemplateRoute_CannotHaveConsecutiveSeparators, "routeTemplate");
			}
			if (!ParseSegment(templateParserContext, segments))
			{
				throw new ArgumentException(templateParserContext.Error, "routeTemplate");
			}
		}
		if (IsAllValid(templateParserContext, segments))
		{
			return new RouteTemplate(routeTemplate, segments);
		}
		throw new ArgumentException(templateParserContext.Error, "routeTemplate");
	}

	private static string TrimPrefix(string routeTemplate)
	{
		if (routeTemplate.StartsWith("~/", StringComparison.Ordinal))
		{
			return routeTemplate.Substring(2);
		}
		if (routeTemplate.StartsWith("/", StringComparison.Ordinal))
		{
			return routeTemplate.Substring(1);
		}
		if (routeTemplate.StartsWith("~", StringComparison.Ordinal))
		{
			throw new ArgumentException(Resources.TemplateRoute_InvalidRouteTemplate, "routeTemplate");
		}
		return routeTemplate;
	}

	private static bool ParseSegment(TemplateParserContext context, List<TemplateSegment> segments)
	{
		TemplateSegment templateSegment = new TemplateSegment();
		do
		{
			if (context.Current == '{')
			{
				if (!context.Next())
				{
					context.Error = Resources.TemplateRoute_MismatchedParameter;
					return false;
				}
				if (context.Current == '{')
				{
					context.Back();
					if (!ParseLiteral(context, templateSegment))
					{
						return false;
					}
				}
				else if (!ParseParameter(context, templateSegment))
				{
					return false;
				}
			}
			else
			{
				if (context.Current == '/')
				{
					break;
				}
				if (!ParseLiteral(context, templateSegment))
				{
					return false;
				}
			}
		}
		while (context.Next());
		if (IsSegmentValid(context, templateSegment))
		{
			segments.Add(templateSegment);
			return true;
		}
		return false;
	}

	private static bool ParseParameter(TemplateParserContext context, TemplateSegment segment)
	{
		context.Mark();
		while (true)
		{
			if (context.Current == '{')
			{
				if (!context.Next())
				{
					context.Error = Resources.TemplateRoute_MismatchedParameter;
					return false;
				}
				if (context.Current != '{')
				{
					context.Error = Resources.TemplateRoute_UnescapedBrace;
					return false;
				}
			}
			else if (context.Current == '}')
			{
				if (!context.Next())
				{
					context.Back();
					break;
				}
				if (context.Current != '}')
				{
					context.Back();
					break;
				}
			}
			if (!context.Next())
			{
				context.Error = Resources.TemplateRoute_MismatchedParameter;
				return false;
			}
		}
		TemplatePart templatePart = InlineRouteParameterParser.ParseRouteParameter(context.Capture().Replace("}}", "}").Replace("{{", "{"));
		if (templatePart.IsCatchAll && templatePart.IsOptional)
		{
			context.Error = Resources.TemplateRoute_CatchAllCannotBeOptional;
			return false;
		}
		if (templatePart.IsOptional && templatePart.DefaultValue != null)
		{
			context.Error = Resources.TemplateRoute_OptionalCannotHaveDefaultValue;
			return false;
		}
		string name = templatePart.Name;
		if (IsValidParameterName(context, name))
		{
			segment.Parts.Add(templatePart);
			return true;
		}
		return false;
	}

	private static bool ParseLiteral(TemplateParserContext context, TemplateSegment segment)
	{
		context.Mark();
		string text;
		while (true)
		{
			if (context.Current == '/')
			{
				text = context.Capture();
				context.Back();
				break;
			}
			if (context.Current == '{')
			{
				if (!context.Next())
				{
					context.Error = Resources.TemplateRoute_MismatchedParameter;
					return false;
				}
				if (context.Current != '{')
				{
					context.Back();
					text = context.Capture();
					context.Back();
					break;
				}
			}
			else if (context.Current == '}')
			{
				if (!context.Next())
				{
					context.Error = Resources.TemplateRoute_MismatchedParameter;
					return false;
				}
				if (context.Current != '}')
				{
					context.Error = Resources.TemplateRoute_MismatchedParameter;
					return false;
				}
			}
			if (!context.Next())
			{
				text = context.Capture();
				break;
			}
		}
		string text2 = text.Replace("}}", "}").Replace("{{", "{");
		if (IsValidLiteral(context, text2))
		{
			segment.Parts.Add(TemplatePart.CreateLiteral(text2));
			return true;
		}
		return false;
	}

	private static bool IsAllValid(TemplateParserContext context, List<TemplateSegment> segments)
	{
		for (int i = 0; i < segments.Count; i++)
		{
			TemplateSegment templateSegment = segments[i];
			for (int j = 0; j < templateSegment.Parts.Count; j++)
			{
				TemplatePart templatePart = templateSegment.Parts[j];
				if (templatePart.IsParameter && templatePart.IsCatchAll && (i != segments.Count - 1 || j != templateSegment.Parts.Count - 1))
				{
					context.Error = Resources.TemplateRoute_CatchAllMustBeLast;
					return false;
				}
			}
		}
		return true;
	}

	private static bool IsSegmentValid(TemplateParserContext context, TemplateSegment segment)
	{
		for (int i = 0; i < segment.Parts.Count; i++)
		{
			TemplatePart templatePart = segment.Parts[i];
			if (templatePart.IsParameter && templatePart.IsCatchAll && segment.Parts.Count > 1)
			{
				context.Error = Resources.TemplateRoute_CannotHaveCatchAllInMultiSegment;
				return false;
			}
		}
		for (int j = 0; j < segment.Parts.Count; j++)
		{
			TemplatePart templatePart2 = segment.Parts[j];
			if (templatePart2.IsParameter && templatePart2.IsOptional && segment.Parts.Count > 1)
			{
				if (j != segment.Parts.Count - 1)
				{
					TemplatePart templatePart3 = segment.Parts[j + 1];
					string arg = (templatePart3.IsParameter ? templatePart3.Name : templatePart3.Text);
					context.Error = string.Format(Resources.TemplateRoute_OptionalParameterHasTobeTheLast, segment.DebuggerToString(), segment.Parts[j].Name, arg);
					return false;
				}
				if (!segment.Parts[j - 1].IsLiteral)
				{
					context.Error = string.Format(Resources.TemplateRoute_OptionalParameterCanbBePrecededByPeriod, segment.DebuggerToString(), templatePart2.Name, segment.Parts[j - 1].DebuggerToString());
					return false;
				}
				if (segment.Parts[j - 1].Text != ".")
				{
					context.Error = string.Format(Resources.TemplateRoute_OptionalParameterCanbBePrecededByPeriod, segment.DebuggerToString(), templatePart2.Name, segment.Parts[j - 1].Text);
					return false;
				}
				segment.Parts[j - 1].IsOptionalSeperator = true;
			}
		}
		bool flag = false;
		for (int k = 0; k < segment.Parts.Count; k++)
		{
			TemplatePart templatePart4 = segment.Parts[k];
			if (templatePart4.IsParameter && flag)
			{
				context.Error = Resources.TemplateRoute_CannotHaveConsecutiveParameters;
				return false;
			}
			flag = templatePart4.IsParameter;
		}
		return true;
	}

	private static bool IsValidParameterName(TemplateParserContext context, string parameterName)
	{
		if (parameterName.Length == 0)
		{
			context.Error = string.Format(CultureInfo.CurrentCulture, Resources.TemplateRoute_InvalidParameterName, parameterName);
			return false;
		}
		foreach (char c in parameterName)
		{
			if (c == '/' || c == '{' || c == '}' || c == '?' || c == '*')
			{
				context.Error = string.Format(CultureInfo.CurrentCulture, Resources.TemplateRoute_InvalidParameterName, parameterName);
				return false;
			}
		}
		if (!context.ParameterNames.Add(parameterName))
		{
			context.Error = string.Format(CultureInfo.CurrentCulture, Resources.TemplateRoute_RepeatedParameter, parameterName);
			return false;
		}
		return true;
	}

	private static bool IsValidLiteral(TemplateParserContext context, string literal)
	{
		if (literal.IndexOf('?') != -1)
		{
			context.Error = string.Format(CultureInfo.CurrentCulture, Resources.TemplateRoute_InvalidLiteral, literal);
			return false;
		}
		return true;
	}

	private static bool IsInvalidRouteTemplate(string routeTemplate)
	{
		if (!routeTemplate.StartsWith("~", StringComparison.Ordinal))
		{
			return routeTemplate.StartsWith("/", StringComparison.Ordinal);
		}
		return true;
	}
}
