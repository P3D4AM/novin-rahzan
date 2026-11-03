using System.Drawing;

namespace Stimulsoft.Report.Components.Table;

public class StiTable24StyleFX : StiTableStyleFX
{
	public override StiTableStyleIdent StyleId => StiTableStyleIdent.Style24;

	public override Color HeaderColor
	{
		get
		{
			return ColorTranslator.FromHtml("#b4ed7d31");
		}
		set
		{
		}
	}

	public override Color FooterColor
	{
		get
		{
			return ColorTranslator.FromHtml("#b470ad47");
		}
		set
		{
		}
	}

	public override Color DataColor
	{
		get
		{
			return ColorTranslator.FromHtml("#b4997300");
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
			return ColorTranslator.FromHtml("#000000");
		}
		set
		{
		}
	}

	public override Color FooterForeground
	{
		get
		{
			return ColorTranslator.FromHtml("#000000");
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
