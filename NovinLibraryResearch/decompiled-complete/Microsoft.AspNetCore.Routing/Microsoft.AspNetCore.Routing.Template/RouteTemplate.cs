using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.AspNetCore.Routing.Template;

[DebuggerDisplay("{DebuggerToString()}")]
public class RouteTemplate
{
	private const string SeparatorString = "/";

	public string TemplateText { get; }

	public IList<TemplatePart> Parameters { get; }

	public IList<TemplateSegment> Segments { get; }

	public RouteTemplate(string template, List<TemplateSegment> segments)
	{
		if (segments == null)
		{
			throw new ArgumentNullException("segments");
		}
		TemplateText = template;
		Segments = segments;
		Parameters = new List<TemplatePart>();
		for (int i = 0; i < segments.Count; i++)
		{
			TemplateSegment templateSegment = Segments[i];
			for (int j = 0; j < templateSegment.Parts.Count; j++)
			{
				TemplatePart templatePart = templateSegment.Parts[j];
				if (templatePart.IsParameter)
				{
					Parameters.Add(templatePart);
				}
			}
		}
	}

	public TemplateSegment GetSegment(int index)
	{
		if (index < 0)
		{
			throw new IndexOutOfRangeException();
		}
		if (index < Segments.Count)
		{
			return Segments[index];
		}
		return null;
	}

	private string DebuggerToString()
	{
		return string.Join("/", Segments.Select((TemplateSegment s) => s.DebuggerToString()));
	}

	public TemplatePart GetParameter(string name)
	{
		for (int i = 0; i < Parameters.Count; i++)
		{
			TemplatePart templatePart = Parameters[i];
			if (string.Equals(templatePart.Name, name, StringComparison.OrdinalIgnoreCase))
			{
				return templatePart;
			}
		}
		return null;
	}
}
