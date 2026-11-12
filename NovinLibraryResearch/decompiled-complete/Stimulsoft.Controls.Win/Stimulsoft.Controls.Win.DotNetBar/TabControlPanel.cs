using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class TabControlPanel : PanelEx
{
	private const string INFO_TEXT = "Drop controls on this panel to associate them with currently selected tab.";

	private TabItem m_Tab;

	private bool m_UseCustomStyle;

	[Browsable(false)]
	[DefaultValue(null)]
	public TabItem TabItem
	{
		get
		{
			return m_Tab;
		}
		set
		{
			m_Tab = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether style of the panel is managed by tab control automatically. Set this to true if you would like to control style of the panel.")]
	public bool UseCustomStyle
	{
		get
		{
			return m_UseCustomStyle;
		}
		set
		{
			m_UseCustomStyle = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public override DockStyle Dock
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Dock;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Dock = value;
		}
	}

	public TabControlPanel()
	{
		((ScrollableControl)this).DockPadding.All = 1;
		((Control)this).BackColor = SystemColors.Control;
	}

	private Rectangle GetThemedRect(Rectangle r)
	{
		switch (m_Tab.Parent.TabAlignment)
		{
		case eTabStripAlignment.Top:
			r.Y -= 6;
			r.Height += 6;
			break;
		case eTabStripAlignment.Left:
			r.X -= 6;
			r.Width += 6;
			break;
		case eTabStripAlignment.Right:
			r.Width += 6;
			break;
		case eTabStripAlignment.Bottom:
			r.Height += 6;
			break;
		}
		return r;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		if (((Control)this).ClientRectangle.Width == 0 || ((Control)this).ClientRectangle.Height == 0 || (((Control)this).Parent is TabControl && ((TabControl)(object)((Control)this).Parent).SelectedPanel != this))
		{
			return;
		}
		bool flag = true;
		if (DrawThemedPane && BarFunctions.ThemedOS && m_Tab != null && m_Tab.Parent != null)
		{
			Rectangle themedRect = GetThemedRect(((Control)this).ClientRectangle);
			Rectangle r = new Rectangle(0, 0, themedRect.Width, themedRect.Height);
			if (m_Tab.Parent.TabAlignment == eTabStripAlignment.Right || m_Tab.Parent.TabAlignment == eTabStripAlignment.Left)
			{
				r = new Rectangle(0, 0, r.Height, r.Width);
			}
			if (m_ThemeCachedBitmap == null || ((Image)m_ThemeCachedBitmap).Size != r.Size)
			{
				DisposeThemeCachedBitmap();
				Bitmap val = new Bitmap(r.Width, r.Height, e.Graphics);
				try
				{
					Graphics val2 = Graphics.FromImage((Image)(object)val);
					try
					{
						SolidBrush val3 = new SolidBrush(Color.Transparent);
						try
						{
							val2.FillRectangle((Brush)(object)val3, 0, 0, ((Image)val).Width, ((Image)val).Height);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
						base.ThemeTab.DrawBackground(val2, ThemeTabParts.Pane, ThemeTabStates.Normal, r);
					}
					finally
					{
						val2.Dispose();
					}
				}
				finally
				{
					if (m_Tab.Parent.TabAlignment == eTabStripAlignment.Left)
					{
						((Image)val).RotateFlip((RotateFlipType)3);
					}
					else if (m_Tab.Parent.TabAlignment == eTabStripAlignment.Right)
					{
						((Image)val).RotateFlip((RotateFlipType)1);
					}
					else if (m_Tab.Parent.TabAlignment == eTabStripAlignment.Bottom)
					{
						((Image)val).RotateFlip((RotateFlipType)2);
					}
					e.Graphics.DrawImageUnscaled((Image)(object)val, themedRect.X, themedRect.Y);
					m_ThemeCachedBitmap = val;
				}
			}
			else
			{
				e.Graphics.DrawImageUnscaled((Image)(object)m_ThemeCachedBitmap, themedRect.X, themedRect.Y);
			}
			flag = false;
		}
		if (flag)
		{
			base.OnPaint(e);
		}
		if (((Component)(object)this).DesignMode && ((ArrangedElementCollection)((Control)this).Controls).Count == 0 && ((Control)this).Text == "")
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Inflate(-2, -2);
			eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak;
			Font val4 = new Font(((Control)this).Font, (FontStyle)1);
			TextDrawing.DrawString(e.Graphics, "Drop controls on this panel to associate them with currently selected tab.", val4, ControlPaint.Dark(base.Style.BackColor1.Color), clientRectangle, format);
			val4.Dispose();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		DisposeThemeCachedBitmap();
		base.OnResize(e);
	}
}
