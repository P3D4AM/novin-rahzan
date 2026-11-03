using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.PanelDockContainerDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
public class PanelDockContainer : PanelEx
{
	private const string ᑙ = "Drop controls here. Drag and Drop tabs to perform docking.";

	private DockContainerItem _290A;

	private bool ᖔ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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

	private eTabStripAlignment _2909
	{
		get
		{
			if (_290A != null && _290A.ContainerControl is Bar)
			{
				return ((Bar)_290A.ContainerControl).DockTabAlignment;
			}
			return eTabStripAlignment.Top;
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether style of the panel is managed by tab control automatically. Set this to true if you would like to control style of the panel.")]
	[DefaultValue(false)]
	[Browsable(true)]
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

	[DefaultValue(null)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public DockContainerItem DockContainerItem
	{
		get
		{
			return _290A;
		}
		set
		{
			_290A = value;
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

	private Rectangle ᑢ(Rectangle ल)
	{
		switch (_2909)
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
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		bool flag = true;
		if (ᑲ && BarFunctions.ThemedOS && _290A != null)
		{
			Rectangle rectangle = ᑢ(((Control)this).ClientRectangle);
			eTabStripAlignment eTabStripAlignment2 = _2909;
			Rectangle ल = new Rectangle(0, 0, rectangle.Width, rectangle.Height);
			if (eTabStripAlignment2 == eTabStripAlignment.Right || eTabStripAlignment2 == eTabStripAlignment.Left)
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
					switch (eTabStripAlignment2)
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
			TextDrawing.DrawString(e.Graphics, "Drop controls here. Drag and Drop tabs to perform docking.", val4, ControlPaint.Dark(base.Style.BackColor1.Color), clientRectangle, format);
			val4.Dispose();
		}
	}

	protected override void OnColorSchemeChanged()
	{
		base.OnColorSchemeChanged();
		if (!UseCustomStyle)
		{
			if (StyleManager.IsVisualStudio2012(StyleManager.Style))
			{
				base.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
			}
			else
			{
				base.Style.BackColor1.ColorSchemePart = eColorSchemePart.BarBackground;
			}
		}
	}

	protected override void OnResize(EventArgs e)
	{
		DisposeThemeCachedBitmap();
		base.OnResize(e);
	}

	private TabStrip _290B()
	{
		TabStrip result = null;
		if (_290A != null && _290A.ContainerControl is Bar)
		{
			Bar bar = _290A.ContainerControl as Bar;
			if (bar.DockSide == eDockSide.Document)
			{
				result = bar.DockTabControl;
			}
		}
		return result;
	}

	protected override void OnEnter(EventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		_290C()?.InternalDockContainerActivated(_290A);
		TabStrip tabStrip = _290B();
		if (tabStrip == null)
		{
			return;
		}
		try
		{
			if (tabStrip.SelectedTabFont == null)
			{
				tabStrip.SelectedTabFont = new Font(((Control)tabStrip).Font, (FontStyle)1);
			}
		}
		catch
		{
		}
		if (((ArrangedElementCollection)((Control)this).Controls).Count > 0)
		{
			Form val = ((Control)this).FindForm();
			if (val != null && (((ContainerControl)val).ActiveControl == this || (((ContainerControl)val).ActiveControl == tabStrip && tabStrip != null)))
			{
				((Control)this).SelectNextControl((Control)null, true, true, true, true);
			}
		}
		((Control)this).OnEnter(e);
	}

	private DotNetBarManager _290C()
	{
		if (_290A != null && _290A.ContainerControl is Bar)
		{
			return ((Bar)_290A.ContainerControl).Owner as DotNetBarManager;
		}
		return null;
	}

	protected override void OnLeave(EventArgs e)
	{
		_290C()?.InternalDockContainerDeactivated(_290A);
		TabStrip tabStrip = _290B();
		if (tabStrip != null)
		{
			if (tabStrip.SelectedTabFont != null)
			{
				tabStrip.SelectedTabFont = null;
			}
			((Control)this).OnLeave(e);
		}
	}
}
