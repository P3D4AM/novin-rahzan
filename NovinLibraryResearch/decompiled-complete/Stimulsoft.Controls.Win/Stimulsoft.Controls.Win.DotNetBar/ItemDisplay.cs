using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ItemDisplay
{
	public void Paint(ItemContainer container, ItemPaintArgs p)
	{
		foreach (BaseItem subItem in container.SubItems)
		{
			if (!subItem.Visible || !subItem.Displayed)
			{
				continue;
			}
			if (subItem.BeginGroup && p.Renderer != null)
			{
				p.Renderer.DrawItemContainerSeparator(new ItemContainerSeparatorRendererEventArgs(p.Graphics, container, subItem));
			}
			if (p.ClipRectangle.IsEmpty || p.ClipRectangle.IntersectsWith(subItem.DisplayRectangle))
			{
				Region clip = p.Graphics.Clip;
				p.Graphics.SetClip(subItem.DisplayRectangle, (CombineMode)1);
				if (!p.Graphics.IsClipEmpty)
				{
					subItem.Paint(p);
				}
				p.Graphics.Clip = clip;
				if (clip != null)
				{
					clip.Dispose();
				}
				if (subItem.ShowArrowIfNoSubItems)
				{
					Rectangle displayRectangle = subItem.DisplayRectangle;
					int num = SystemInformation.HorizontalScrollBarHeight - StiScale2.I4;
					StiButtonHelper.DrawDownArrow(p.Graphics, new Rectangle(displayRectangle.X, displayRectangle.Bottom - num, displayRectangle.Width, num), enabled: true);
				}
			}
		}
	}

	public void Paint(BaseItem container, ItemPaintArgs p)
	{
		foreach (BaseItem subItem in container.SubItems)
		{
			if (subItem.Visible && subItem.Displayed && (p.ClipRectangle.IsEmpty || p.ClipRectangle.IntersectsWith(subItem.DisplayRectangle)))
			{
				Region clip = p.Graphics.Clip;
				p.Graphics.SetClip(subItem.DisplayRectangle, (CombineMode)1);
				subItem.Paint(p);
				p.Graphics.Clip = clip;
				if (clip != null)
				{
					clip.Dispose();
				}
			}
		}
	}
}
