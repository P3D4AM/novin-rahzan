using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

public class TabItemLayoutManager : BlockLayoutManager
{
	private int ᵍ = 4;

	private int ᵎ = 2;

	private int ᵏ = 4;

	private int ᵐ;

	private bool ᵑ;

	private int ᵒ;

	private Size ᔜ = Size.Empty;

	private bool ᔝ;

	private Size ᔠ = new Size(11, 11);

	private int ᵓ = 3;

	private TabStrip ᵔ;

	public int TextPadding
	{
		get
		{
			return ᵍ;
		}
		set
		{
			ᵍ = value;
		}
	}

	public int ImagePadding
	{
		get
		{
			return ᵎ;
		}
		set
		{
			ᵎ = value;
		}
	}

	public int PaddingHeight
	{
		get
		{
			return ᵏ;
		}
		set
		{
			ᵏ = value;
		}
	}

	public int PaddingWidth
	{
		get
		{
			return ᵐ;
		}
		set
		{
			ᵐ = value;
		}
	}

	public bool HorizontalText
	{
		get
		{
			return ᵑ;
		}
		set
		{
			ᵑ = value;
		}
	}

	public int SelectedPaddingWidth
	{
		get
		{
			return ᵒ;
		}
		set
		{
			ᵒ = value;
		}
	}

	public Size FixedTabSize
	{
		get
		{
			return ᔜ;
		}
		set
		{
			ᔜ = value;
		}
	}

	public bool CloseButtonOnTabs
	{
		get
		{
			return ᔝ;
		}
		set
		{
			ᔝ = value;
		}
	}

	public Size CloseButtonSize
	{
		get
		{
			return ᔠ;
		}
		set
		{
			ᔠ = value;
		}
	}

	public TabItemLayoutManager()
	{
	}

	public TabItemLayoutManager(TabStrip tabStrip)
	{
		ᵔ = tabStrip;
	}

	public override void Layout(IBlock block, Size availableSize)
	{
		if (base.Graphics == null)
		{
			throw new InvalidOperationException("Graphics property must be set to valid instance of Graphics object.");
		}
		TabItem tabItem = block as TabItem;
		if (!tabItem.Visible)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		bool flag = tabItem.Parent != null && (tabItem.Parent.TabAlignment == eTabStripAlignment.Left || tabItem.Parent.TabAlignment == eTabStripAlignment.Right) && !ᵑ;
		eTabStripStyle style = tabItem.Parent.Style;
		eTextFormat format = eTextFormat.Default;
		Image image = tabItem.GetImage();
		if (tabItem.Icon != null)
		{
			num += tabItem.IconSize.Width;
			if (style != eTabStripStyle.OneNote && style != eTabStripStyle.Office2007Document)
			{
				num += ᵎ;
			}
			num2 = tabItem.IconSize.Height + ᵏ;
		}
		else if (image != null)
		{
			num += Dpi.Width(image.Width);
			if (style != eTabStripStyle.OneNote && style != eTabStripStyle.Office2007Document)
			{
				num += Dpi.Width(ᵎ);
			}
			num2 = Dpi.Height(image.Height) + Dpi.Height(ᵏ);
		}
		if (!tabItem.Parent.DisplaySelectedTextOnly || tabItem == tabItem.Parent.SelectedTab)
		{
			string text = tabItem.Text;
			if (text == "")
			{
				text = "M";
			}
			Font font = ((Control)tabItem.Parent).Font;
			if (tabItem.Parent.SelectedTabFont != null && (tabItem == tabItem.Parent.SelectedTab || (tabItem.Parent.CloseButtonOnTabsAlwaysDisplayed && tabItem.Parent.CloseButtonPosition == eTabCloseButtonPosition.Right) || ((tabItem.Parent.TabAlignment == eTabStripAlignment.Left || tabItem.Parent.TabAlignment == eTabStripAlignment.Right) && ᵑ)))
			{
				font = tabItem.Parent.SelectedTabFont;
			}
			Size empty = Size.Empty;
			empty = ((!flag) ? TextDrawing.MeasureString(base.Graphics, text, font, 0, format) : TextDrawing.MeasureStringLegacy(base.Graphics, text, font, Size.Empty, format));
			num += empty.Width;
			if (style != eTabStripStyle.OneNote)
			{
				num += Dpi.Width(ᵍ);
			}
			if (empty.Height > num2)
			{
				num2 = empty.Height + Dpi.Height(ᵏ);
			}
		}
		if (ᔝ && tabItem.CloseButtonVisible)
		{
			num += ᔠ.Width + ᵓ * 2;
		}
		num += ᵐ;
		if (tabItem.IsSelected)
		{
			num += ᵒ;
		}
		if (ᔜ.Width > 0)
		{
			num = ᔜ.Width;
		}
		if (ᔜ.Height > 0)
		{
			num2 = ᔜ.Height;
		}
		if (style == eTabStripStyle.OneNote || style == eTabStripStyle.Office2007Document)
		{
			num += (int)((double)num2 * 0.5) - 4;
		}
		Rectangle bounds = new Rectangle(0, 0, num, num2);
		if (flag)
		{
			bounds = new Rectangle(0, 0, num2, num);
		}
		if (ᵔ != null && ᵔ.ᓝ)
		{
			MeasureTabItemEventArgs measureTabItemEventArgs = new MeasureTabItemEventArgs(tabItem, bounds.Size);
			ᵔ.ᕈ(measureTabItemEventArgs);
			bounds.Size = measureTabItemEventArgs.Size;
		}
		block.Bounds = bounds;
	}

	public override Rectangle FinalizeLayout(Rectangle containerBounds, Rectangle blocksBounds, ArrayList lines)
	{
		if (lines.Count > 1 && ᵔ.MultiLinePanelAlignSelectedTab)
		{
			int num = ᵘ(lines);
			if (num >= 0)
			{
				if (ᵔ.TabAlignment == eTabStripAlignment.Bottom && num > 0)
				{
					ᵕ(lines, num, 0);
				}
				else if (ᵔ.TabAlignment == eTabStripAlignment.Top && num < lines.Count - 1)
				{
					ᵕ(lines, num, lines.Count - 1);
				}
				else if (ᵔ.TabAlignment == eTabStripAlignment.Left && num < lines.Count - 1)
				{
					ᵕ(lines, num, lines.Count - 1);
				}
				else if (ᵔ.TabAlignment == eTabStripAlignment.Right && num > 0)
				{
					ᵕ(lines, num, 0);
				}
			}
		}
		return blocksBounds;
	}

	private void ᵕ(ArrayList _೨, int ᵖ, int ᵗ)
	{
		SerialContentLayoutManager._0CF0 obj = (SerialContentLayoutManager._0CF0)_೨[ᵖ];
		SerialContentLayoutManager._0CF0 obj2 = (SerialContentLayoutManager._0CF0)_೨[ᵗ];
		bool flag = ᵔ.TabAlignment == eTabStripAlignment.Top || ᵔ.TabAlignment == eTabStripAlignment.Bottom;
		int num = (flag ? ((IBlock)obj.ೱ[0]).Bounds.Y : ((IBlock)obj.ೱ[0]).Bounds.X);
		int num2 = (flag ? ((IBlock)obj2.ೱ[0]).Bounds.Y : ((IBlock)obj2.ೱ[0]).Bounds.X);
		_೨[ᵖ] = obj2;
		_೨[ᵗ] = obj;
		foreach (IBlock item in obj2.ೱ)
		{
			if (flag)
			{
				item.Bounds = new Rectangle(item.Bounds.X, num, item.Bounds.Width, item.Bounds.Height);
			}
			else
			{
				item.Bounds = new Rectangle(num, item.Bounds.Y, item.Bounds.Width, item.Bounds.Height);
			}
		}
		foreach (IBlock item2 in obj.ೱ)
		{
			if (flag)
			{
				item2.Bounds = new Rectangle(item2.Bounds.X, num2, item2.Bounds.Width, item2.Bounds.Height);
			}
			else
			{
				item2.Bounds = new Rectangle(num2, item2.Bounds.Y, item2.Bounds.Width, item2.Bounds.Height);
			}
		}
	}

	private int ᵘ(ArrayList _೨)
	{
		foreach (SerialContentLayoutManager._0CF0 item in _೨)
		{
			foreach (TabItem item2 in item.ೱ)
			{
				if (item2.IsSelected)
				{
					return item._0CF3;
				}
			}
		}
		return -1;
	}
}
