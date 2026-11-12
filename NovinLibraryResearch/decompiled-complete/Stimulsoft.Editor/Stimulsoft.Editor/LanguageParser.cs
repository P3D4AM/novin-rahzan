using System.ComponentModel;
using System.Resources;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
public class LanguageParser : Lexer
{
	private Languages language;

	private int schemeUpdateCount;

	[DefaultValue(Languages.Cs)]
	[Description("Specifies language to analize.")]
	public Languages Language
	{
		get
		{
			return language;
		}
		set
		{
			if (language != value)
			{
				language = value;
				UpdateScheme();
			}
		}
	}

	public override string XmlScheme
	{
		get
		{
			return base.XmlScheme;
		}
		set
		{
			base.XmlScheme = value;
		}
	}

	public override ILexScheme Scheme
	{
		get
		{
			return base.Scheme;
		}
		set
		{
			base.Scheme = value;
			if (schemeUpdateCount == 0)
			{
				language = Languages.Custom;
			}
		}
	}

	public LanguageParser()
	{
		language = Languages.Custom;
	}

	private void UpdateScheme()
	{
		schemeUpdateCount++;
		try
		{
			ResourceManager resourceManager = new ResourceManager(typeof(LanguageParser));
			string name = $"language {language}";
			object @object = resourceManager.GetObject(name);
			if (@object != null && @object is string)
			{
				XmlScheme = (string)@object;
			}
			else
			{
				Scheme.ClearScheme();
			}
		}
		finally
		{
			schemeUpdateCount--;
		}
	}

	public bool ShouldSerializeXmlScheme()
	{
		return language == Languages.Custom;
	}
}
