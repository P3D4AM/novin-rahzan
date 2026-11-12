using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

public static class MarkupSettings
{
	public delegate void ResolveImageEventHandler(object sender, ResolveImageEventArgs e);

	private static HyperlinkStyle _NormalHyperlink = new HyperlinkStyle(Color.Blue, eHyperlinkUnderlineStyle.SolidLine);

	private static HyperlinkStyle _MouseOverHyperlink = new HyperlinkStyle();

	private static HyperlinkStyle _VisitedHyperlink = new HyperlinkStyle();

	public static HyperlinkStyle NormalHyperlink => _NormalHyperlink;

	public static HyperlinkStyle MouseOverHyperlink => _MouseOverHyperlink;

	public static HyperlinkStyle VisitedHyperlink => _VisitedHyperlink;

	public static event ResolveImageEventHandler ResolveImage;

	internal static void InvokeResolveImage(ResolveImageEventArgs e)
	{
		if (MarkupSettings.ResolveImage != null)
		{
			MarkupSettings.ResolveImage(null, e);
		}
	}
}
