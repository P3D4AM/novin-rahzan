using System.Drawing;

namespace Stimulsoft.Report.Components.Table;

public class StiTable26StyleFX : StiTableStyleFX
{
	public override StiTableStyleIdent StyleId => StiTableStyleIdent.Style26;

	public override Color HeaderColor
	{
		get
		{
			return ColorTranslator.FromHtml("#b42ec6c8");
		}
		set
		{
		}
	}

	public override Color FooterColor
	{
		get
		{
			return ColorTranslator.FromHtml("#b4d04456");
		}
		set
		{
		}
	}

	public override Color DataColor
	{
		get
		{
			return ColorTranslator.FromHtml("#ffffff");
		}
		set
		{
		}
	}

	public override Color DataForeground
	{
		get
		{
			return ColorTranslator.FromHtml("#000000");
		}
		set
		{
		}
	}

	public override Color HeaderForeground
	{
		get
		{
			return ColorTranslator.FromHtml("#ffffff");
		}
		set
		{
		}
	}

	public override Color FooterForeground
	{
		get
		{
			return ColorTranslator.FromHtml("#b45ab0ee");
		}
		set
		{
		}
	}

	public override Color GridColor
	{
		get
		{
			return ColorTranslator.FromHtml("#b4264478");
		}
		set
		{
		}
	}
}
