using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[Designer("DevComponents.DotNetBar.Design.TabControlPanelDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class TabControlPanel : PanelEx
{
	private const string ᑙ = "Drop controls on this panel to associate them with currently selected tab.";

	private TabItem ᖓ;

	private bool ᖔ;

	[DefaultValue(null)]
	[Browsable(false)]
	public TabItem TabItem
	{
		get
		{
			return ᖓ;
		}
		set
		{
			ᖓ = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Description("Indicates whether style of the panel is managed by tab control automatically. Set this to true if you would like to control style of the panel.")]
	[Category("Appearance")]
	public bool UseCustomStyle
	{
		get
		{
			return ᖔ;
		}
		set
		{
			ᖔ = value;
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

	private Rectangle ᑢ(Rectangle ल)
	{
		switch (ᖓ.Parent.TabAlignment)
		{
		case eTabStripAlignment.Top:
			ल.Y -= 6;
			ल.Height += 6;
			break;
		case eTabStripAlignment.Left:
			ल.X -= 6;
			ल.Width += 6;
			break;
		case eTabStripAlignment.Right:
			ल.Width += 6;
			break;
		case eTabStripAlignment.Bottom:
			ल.Height += 6;
			break;
		}
		return ल;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Expected O, but got Unknown
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		if (((Control)this).ClientRectangle.Width == 0 || ((Control)this).ClientRectangle.Height == 0 || (((Control)this).Parent is TabControl && ((TabControl)(object)((Control)this).Parent).SelectedPanel != this))
		{
			return;
		}
		bool flag = true;
		if (ᑲ && BarFunctions.ThemedOS && ᖓ != null && ᖓ.Parent != null)
		{
			Rectangle rectangle = ᑢ(((Control)this).ClientRectangle);
			Rectangle ल = new Rectangle(0, 0, rectangle.Width, rectangle.Height);
			if (ᖓ.Parent.TabAlignment == eTabStripAlignment.Right || ᖓ.Parent.TabAlignment == eTabStripAlignment.Left)
			{
				ल = new Rectangle(0, 0, ल.Height, ल.Width);
			}
			if (m_ThemeCachedBitmap == null || ((Image)m_ThemeCachedBitmap).Size != ल.Size)
			{
				DisposeThemeCachedBitmap();
				Bitmap val = new Bitmap(ल.Width, ल.Height, e.Graphics);
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
						base.ᑧ._0EE5(val2, ᑷ.ᒀ, ᒂ.ཊ, ल);
					}
					finally
					{
						val2.Dispose();
					}
				}
				finally
				{
					if (ᖓ.Parent.TabAlignment == eTabStripAlignment.Left)
					{
						((Image)val).RotateFlip((RotateFlipType)3);
					}
					else if (ᖓ.Parent.TabAlignment == eTabStripAlignment.Right)
					{
						((Image)val).RotateFlip((RotateFlipType)1);
					}
					else if (ᖓ.Parent.TabAlignment == eTabStripAlignment.Bottom)
					{
						((Image)val).RotateFlip((RotateFlipType)2);
					}
					e.Graphics.DrawImageUnscaled((Image)(object)val, rectangle.X, rectangle.Y);
					m_ThemeCachedBitmap = val;
				}
			}
			else
			{
				e.Graphics.DrawImageUnscaled((Image)(object)m_ThemeCachedBitmap, rectangle.X, rectangle.Y);
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
