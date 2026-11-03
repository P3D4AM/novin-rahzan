using System.Drawing;

namespace Stimulsoft.Report.Components.Table;

public class StiTable27StyleFX : StiTableStyleFX
{
	public override StiTableStyleIdent StyleId => StiTableStyleIdent.Style27;

	public override Color HeaderColor
	{
		get
		{
			return ColorTranslator.FromHtml("#b40bac45");
		}
		set
		{
		}
	}

	public override Color FooterColor
	{
		get
		{
			return ColorTranslator.FromHtml("#b4ec334d");
		}
		set
		{
		}
	}

	public override Color DataColor
	{
		get
		{
			return ColorTranslator.FromHtml("#b433475B");
		}
		set
		{
		}
	}

	public override Color DataForeground
	{
		get
		{
			return ColorTranslator.FromHtml("#ffffff");
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
