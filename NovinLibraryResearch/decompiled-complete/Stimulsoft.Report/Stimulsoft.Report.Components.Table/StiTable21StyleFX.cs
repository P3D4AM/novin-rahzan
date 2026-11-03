using System.Drawing;

namespace Stimulsoft.Report.Components.Table;

public class StiTable21StyleFX : StiTableStyleFX
{
	public override StiTableStyleIdent StyleId => StiTableStyleIdent.Style21;

	public override Color HeaderColor
	{
		get
		{
			return ColorTranslator.FromHtml("#b4239fd9");
		}
		set
		{
		}
	}

	public override Color FooterColor
	{
		get
		{
			return ColorTranslator.FromHtml("#b4239fd9");
		}
		set
		{
		}
	}

	public override Color DataColor
	{
		get
		{
			return ColorTranslator.FromHtml("#b4b2b2b2");
		}
		set
		{
		}
	}

	public override Color DataForeground
	{
		get
		{
			return ColorTranslator.FromHtml("#b4264478");
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
			return ColorTranslator.FromHtml("#ffffff");
		}
		set
		{
		}
	}

	public override Color GridColor
	{
		get
		{
			return ColorTranslator.FromHtml("#ffffff");
		}
		set
		{
		}
	}
}
