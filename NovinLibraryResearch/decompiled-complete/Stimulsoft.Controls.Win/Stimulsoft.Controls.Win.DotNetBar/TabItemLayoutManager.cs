using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class TabItemLayoutManager : BlockLayoutManager
{
	private int m_TextPadding = 4;

	private int m_ImagePadding = 2;

	private int m_PaddingHeight = 4;

	private int m_PaddingWidth;

	private bool m_HorizontalText;

	private int m_SelectedPaddingWidth;

	private Size m_FixedTabSize = Size.Empty;

	private bool m_CloseButtonOnTabs;

	private Size m_CloseButtonSize = new Size(11, 11);

	private int m_CloseButtonSpacing = 3;

	private TabStrip m_TabStrip;

	public int TextPadding
	{
		get
		{
			return m_TextPadding;
		}
		set
		{
			m_TextPadding = value;
		}
	}

	public int ImagePadding
	{
		get
		{
			return m_ImagePadding;
		}
		set
		{
			m_ImagePadding = value;
		}
	}

	public int PaddingHeight
	{
		get
		{
			return m_PaddingHeight;
		}
		set
		{
			m_PaddingHeight = value;
		}
	}

	public int PaddingWidth
	{
		get
		{
			return m_PaddingWidth;
		}
		set
		{
			m_PaddingWidth = value;
		}
	}

	public bool HorizontalText
	{
		get
		{
			return m_HorizontalText;
		}
		set
		{
			m_HorizontalText = value;
		}
	}

	public int SelectedPaddingWidth
	{
		get
		{
			return m_SelectedPaddingWidth;
		}
		set
		{
			m_SelectedPaddingWidth = value;
		}
	}

	public Size FixedTabSize
	{
		get
		{
			return m_FixedTabSize;
		}
		set
		{
			m_FixedTabSize = value;
		}
	}

	public bool CloseButtonOnTabs
	{
		get
		{
			return m_CloseButtonOnTabs;
		}
		set
		{
			m_CloseButtonOnTabs = value;
		}
	}

	public Size CloseButtonSize
	{
		get
		{
			return m_CloseButtonSize;
		}
		set
		{
			m_CloseButtonSize = value;
		}
	}

	public TabItemLayoutManager()
	{
	}

	public TabItemLayoutManager(TabStrip tabStrip)
	{
		m_TabStrip = tabStrip;
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
		bool flag = tabItem.Parent != null && (tabItem.Parent.TabAlignment == eTabStripAlignment.Left || tabItem.Parent.TabAlignment == eTabStripAlignment.Right) && !m_HorizontalText;
		eTabStripStyle style = tabItem.Parent.Style;
		eTextFormat format = eTextFormat.Default;
		Image image = tabItem.GetImage();
		if (tabItem.Icon != null)
		{
			num += tabItem.IconSize.Width;
			if (style != eTabStripStyle.OneNote && style != eTabStripStyle.Office2007Document)
			{
				num += m_ImagePadding;
			}
			num2 = tabItem.IconSize.Height + m_PaddingHeight;
		}
		else if (image != null)
		{
			num += image.Width;
			if (style != eTabStripStyle.OneNote && style != eTabStripStyle.Office2007Document)
			{
				num += m_ImagePadding;
			}
			num2 = image.Height + m_PaddingHeight;
		}
		if (!tabItem.Parent.DisplaySelectedTextOnly || tabItem == tabItem.Parent.SelectedTab)
		{
			string text = tabItem.Text;
			if (text == "")
			{
				text = "M";
			}
			Font font = ((Control)tabItem.Parent).Font;
			if (tabItem.Parent.SelectedTabFont != null && (tabItem == tabItem.Parent.SelectedTab || (tabItem.Parent.CloseButtonOnTabsAlwaysDisplayed && tabItem.Parent.CloseButtonPosition == eTabCloseButtonPosition.Right) || ((tabItem.Parent.TabAlignment == eTabStripAlignment.Left || tabItem.Parent.TabAlignment == eTabStripAlignment.Right) && m_HorizontalText)))
			{
				font = tabItem.Parent.SelectedTabFont;
			}
			Size empty = Size.Empty;
			empty = ((!flag) ? TextDrawing.MeasureString(base.Graphics, text, font, 0, format) : TextDrawing.MeasureStringLegacy(base.Graphics, text, font, Size.Empty, format));
			num += empty.Width;
			if (style != eTabStripStyle.OneNote)
			{
				num += m_TextPadding;
			}
			if (empty.Height > num2)
			{
				num2 = empty.Height + m_PaddingHeight;
			}
		}
		if (m_CloseButtonOnTabs && tabItem.CloseButtonVisible)
		{
			num += m_CloseButtonSize.Width + m_CloseButtonSpacing * 2;
		}
		num += m_PaddingWidth;
		if (tabItem.IsSelected)
		{
			num += m_SelectedPaddingWidth;
		}
		if (m_FixedTabSize.Width > 0)
		{
			num = m_FixedTabSize.Width;
		}
		if (m_FixedTabSize.Height > 0)
		{
			num2 = m_FixedTabSize.Height;
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
		if (m_TabStrip != null && m_TabStrip.HasMeasureTabItem)
		{
			MeasureTabItemEventArgs measureTabItemEventArgs = new MeasureTabItemEventArgs(tabItem, bounds.Size);
			m_TabStrip.InvokeMeasureTabItem(measureTabItemEventArgs);
			bounds.Size = measureTabItemEventArgs.Size;
		}
		block.Bounds = bounds;
	}

	public override Rectangle FinalizeLayout(Rectangle containerBounds, Rectangle blocksBounds, ArrayList lines)
	{
		return blocksBounds;
	}
}
