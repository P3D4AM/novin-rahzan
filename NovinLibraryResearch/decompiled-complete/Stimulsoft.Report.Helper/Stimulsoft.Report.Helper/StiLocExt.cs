using System;
using System.Drawing;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Helper;

public sealed class StiLocExt
{
	public static void Init()
	{
	}

	private static string GetCategory(Type type)
	{
		string text = type.ToString();
		int num = text.LastIndexOf(".", StringComparison.Ordinal);
		if (num != -1)
		{
			return text.Substring(num + 1);
		}
		return type.ToString();
	}

	private static void ThisProcessDescription(object sender, StiProcessDescriptionEventArgs e)
	{
		Type type = e.ComponentType;
		if (type == typeof(Font))
		{
			switch (e.Name)
			{
			case "Name":
				e.Description = StiLocalizationExt.Get("Font", "Name", throwError: false);
				return;
			case "Size":
				e.Description = StiLocalizationExt.Get("Font", "Size", throwError: false);
				return;
			case "Bold":
				e.Description = StiLocalizationExt.Get("Font", "Bold", throwError: false);
				return;
			case "Italic":
				e.Description = StiLocalizationExt.Get("Font", "Italic", throwError: false);
				return;
			case "Strikeout":
				e.Description = StiLocalizationExt.Get("Font", "Strikeout", throwError: false);
				return;
			case "Underline":
				e.Description = StiLocalizationExt.Get("Font", "Underline", throwError: false);
				return;
			}
		}
		if (type == typeof(SizeD) || type == typeof(Size))
		{
			string name = e.Name;
			if (name == "Width")
			{
				e.Description = StiLocalizationExt.Get("StiComponent", "Width", throwError: false);
				return;
			}
			if (name == "Height")
			{
				e.Description = StiLocalizationExt.Get("StiComponent", "Height", throwError: false);
				return;
			}
		}
		string text;
		while (true)
		{
			if (type == typeof(object))
			{
				return;
			}
			string category = GetCategory(type);
			text = StiLocalizationExt.Get(category, e.Name, throwError: false);
			if (text != null)
			{
				break;
			}
			type = type.BaseType;
		}
		e.Description = text;
	}

	static StiLocExt()
	{
		StiPropertyDescriptor.ProcessDescription += ThisProcessDescription;
	}
}
