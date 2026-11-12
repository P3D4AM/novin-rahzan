namespace Stimulsoft.Controls.Win.DotNetBar;

public class SystemCaptionItemPainter
{
	public virtual void Paint(SystemCaptionItemRendererEventArgs e)
	{
		if (e.SystemCaptionItem.IsSystemIcon)
		{
			PaintSystemIcon(e, e.SystemCaptionItem.GetEnabled());
		}
		else
		{
			PaintFormButtons(e);
		}
	}

	public virtual void PaintSystemIcon(SystemCaptionItemRendererEventArgs e, bool isEnabled)
	{
	}

	public virtual void PaintFormButtons(SystemCaptionItemRendererEventArgs e)
	{
	}
}
