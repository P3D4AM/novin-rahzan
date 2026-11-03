using System;
using System.Drawing;
using System.Threading;

namespace DevComponents.DotNetBar.TextMarkup;

public static class MarkupSettings
{
	public delegate void ResolveImageEventHandler(object sender, ResolveImageEventArgs e);

	private static HyperlinkStyle _ഋ = new HyperlinkStyle(Color.Blue, eHyperlinkUnderlineStyle.SolidLine);

	private static HyperlinkStyle _ഌ = new HyperlinkStyle();

	private static HyperlinkStyle __0D0D = new HyperlinkStyle();

	private static ResolveImageEventHandler എ;

	public static HyperlinkStyle NormalHyperlink => _ഋ;

	public static HyperlinkStyle MouseOverHyperlink => _ഌ;

	public static HyperlinkStyle VisitedHyperlink => __0D0D;

	public static event ResolveImageEventHandler ResolveImage
	{
		add
		{
			ResolveImageEventHandler resolveImageEventHandler = എ;
			ResolveImageEventHandler resolveImageEventHandler2;
			do
			{
				resolveImageEventHandler2 = resolveImageEventHandler;
				ResolveImageEventHandler value2 = (ResolveImageEventHandler)Delegate.Combine(resolveImageEventHandler2, value);
				resolveImageEventHandler = Interlocked.CompareExchange(ref എ, value2, resolveImageEventHandler2);
			}
			while ((object)resolveImageEventHandler != resolveImageEventHandler2);
		}
		remove
		{
			ResolveImageEventHandler resolveImageEventHandler = എ;
			ResolveImageEventHandler resolveImageEventHandler2;
			do
			{
				resolveImageEventHandler2 = resolveImageEventHandler;
				ResolveImageEventHandler value2 = (ResolveImageEventHandler)Delegate.Remove(resolveImageEventHandler2, value);
				resolveImageEventHandler = Interlocked.CompareExchange(ref എ, value2, resolveImageEventHandler2);
			}
			while ((object)resolveImageEventHandler != resolveImageEventHandler2);
		}
	}

	internal static void ഏ(ResolveImageEventArgs ؿ)
	{
		if (എ != null)
		{
			എ(null, ؿ);
		}
	}
}
