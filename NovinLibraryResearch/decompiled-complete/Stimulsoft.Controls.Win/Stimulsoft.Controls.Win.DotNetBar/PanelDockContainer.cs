using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class PanelDockContainer : PanelEx
{
	private const string INFO_TEXT = "Drop controls here. Drag and Drop tabs to perform docking.";

	private DockContainerItem m_DockContainerItem;

	private bool m_UseCustomStyle;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool AutoSize
	{
		get
		{
			return ((Panel)this).AutoSize;
		}
		set
		{
			((Panel)this).AutoSize = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override AutoSizeMode AutoSizeMode
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Panel)this).AutoSizeMode;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).AutoSizeMode = value;
		}
	}

	private eTabStripAlignment TabAlignment
	{
		get
		{
			if (m_DockContainerItem != null && m_DockContainerItem.ContainerControl is Bar)
			{
				return ((Bar)m_DockContainerItem.ContainerControl).DockTabAlignment;
			}
			return eTabStripAlignment.Top;
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
	[DefaultValue(null)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DockContainerItem DockContainerItem
	{
		get
		{
			return m_DockContainerItem;
		}
		set
		{
			m_DockContainerItem = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[Browsable(false)]
	public Size Size
	{
		get
		{
			return ((Control)this).Size;
		}
		set
		{
			((Control)this).Size = value;
		}
	}

	[Browsable(false)]
	public Point Location
	{
		get
		{
			return ((Control)this).Location;
		}
		set
		{
			((Control)this).Location = value;
		}
	}

	[Browsable(false)]
	public bool Visible
	{
		get
		{
			return ((Control)this).Visible;
		}
		set
		{
			((Control)this).Visible = value;
		}
	}

	[Browsable(false)]
	public override AnchorStyles Anchor
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Anchor;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Anchor = value;
		}
	}

	public PanelDockContainer()
	{
		((Control)this).BackColor = SystemColors.Control;
	}

	private Rectangle GetThemedRect(Rectangle r)
	{
		switch (TabAlignment)
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
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		bool flag = true;
		if (DrawThemedPane && BarFunctions.ThemedOS && m_DockContainerItem != null)
		{
			Rectangle themedRect = GetThemedRect(((Control)this).ClientRectangle);
			eTabStripAlignment tabAlignment = TabAlignment;
			Rectangle r = new Rectangle(0, 0, themedRect.Width, themedRect.Height);
			if (tabAlignment == eTabStripAlignment.Right || tabAlignment == eTabStripAlignment.Left)
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
					switch (tabAlignment)
					{
					case eTabStripAlignment.Left:
						((Image)val).RotateFlip((RotateFlipType)3);
						break;
					case eTabStripAlignment.Right:
						((Image)val).RotateFlip((RotateFlipType)1);
						break;
					case eTabStripAlignment.Bottom:
						((Image)val).RotateFlip((RotateFlipType)2);
						break;
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
			TextDrawing.DrawString(e.Graphics, "Drop controls here. Drag and Drop tabs to perform docking.", val4, ControlPaint.Dark(base.Style.BackColor1.Color), clientRectangle, format);
			val4.Dispose();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		DisposeThemeCachedBitmap();
		base.OnResize(e);
	}

	private TabStrip GetDockTabStrip()
	{
		TabStrip result = null;
		if (m_DockContainerItem != null && m_DockContainerItem.ContainerControl is Bar)
		{
			Bar bar = m_DockContainerItem.ContainerControl as Bar;
			if (bar.DockSide == eDockSide.Document)
			{
				result = bar.DockTabControl;
			}
		}
		return result;
	}

	protected override void OnEnter(EventArgs e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		GetDotNetBarManager()?.InternalDockContainerActivated(m_DockContainerItem);
		TabStrip dockTabStrip = GetDockTabStrip();
		if (dockTabStrip == null)
		{
			return;
		}
		try
		{
			if (dockTabStrip.SelectedTabFont == null)
			{
				dockTabStrip.SelectedTabFont = new Font(((Control)dockTabStrip).Font, (FontStyle)1);
			}
		}
		catch
		{
		}
		if (((ArrangedElementCollection)((Control)this).Controls).Count > 0)
		{
			Form val = ((Control)this).FindForm();
			if (val != null && (((ContainerControl)val).ActiveControl == this || (((ContainerControl)val).ActiveControl == dockTabStrip && dockTabStrip != null)))
			{
				((Control)this).SelectNextControl((Control)null, true, true, true, true);
			}
		}
		((Control)this).OnEnter(e);
	}

	private DotNetBarManager GetDotNetBarManager()
	{
		if (m_DockContainerItem != null && m_DockContainerItem.ContainerControl is Bar)
		{
			return ((Bar)m_DockContainerItem.ContainerControl).Owner as DotNetBarManager;
		}
		return null;
	}

	protected override void OnLeave(EventArgs e)
	{
		GetDotNetBarManager()?.InternalDockContainerDeactivated(m_DockContainerItem);
		TabStrip dockTabStrip = GetDockTabStrip();
		if (dockTabStrip != null)
		{
			if (dockTabStrip.SelectedTabFont != null)
			{
				dockTabStrip.SelectedTabFont = null;
			}
			((Control)this).OnLeave(e);
		}
	}
}
