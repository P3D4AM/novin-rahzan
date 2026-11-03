using System.ComponentModel;
using System.Drawing;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
public class Separator : BaseItem
{
	private Padding _ᬩ = new Padding(2, 2, 2, 2);

	private Size _ᅫ = new Size(3, 16);

	private eDesignMarkerOrientation __2360;

	private Color __2361 = Color.Empty;

	private Color __2362 = Color.Empty;

	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets or sets separator padding.")]
	[Browsable(true)]
	public Padding Padding => _ᬩ;

	public Size FixedSize
	{
		get
		{
			return _ᅫ;
		}
		set
		{
			_ᅫ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[DefaultValue(eDesignMarkerOrientation.NotSet)]
	[Description("Indicates splitter orientation.")]
	[Category("Appearance")]
	public eDesignMarkerOrientation SeparatorOrientation
	{
		get
		{
			return __2360;
		}
		set
		{
			if (__2360 != value)
			{
				eDesignMarkerOrientation oldValue = __2360;
				__2360 = value;
				OnSeparatorOrientationChanged(value, oldValue);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates separator color.")]
	public Color SeparatorColor
	{
		get
		{
			return __2361;
		}
		set
		{
			__2361 = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("Indicates separator shade color.")]
	public Color ShadeColor
	{
		get
		{
			return __2362;
		}
		set
		{
			__2362 = value;
			Refresh();
		}
	}

	public Separator()
		: this("")
	{
	}

	public Separator(string sItemName)
		: base(sItemName)
	{
	}

	public override void Paint(ItemPaintArgs p)
	{
		Rectangle bounds = Bounds;
		if (bounds.Width < 1 || bounds.Height < 1)
		{
			return;
		}
		Color color = p.Colors.ItemSeparator;
		if (p.ContainerControl is SideNavStrip)
		{
			color = p.Colors.BarDockedBorder;
		}
		if (!__2361.IsEmpty)
		{
			color = __2361;
		}
		Color itemSeparatorShade = p.Colors.ItemSeparatorShade;
		if (!__2362.IsEmpty)
		{
			itemSeparatorShade = __2362;
		}
		Graphics graphics = p.Graphics;
		Padding padding = Dpi.Size(_ᬩ);
		Rectangle r;
		if ((Orientation == eOrientation.Vertical && __2360 == eDesignMarkerOrientation.NotSet) || __2360 == eDesignMarkerOrientation.Vertical)
		{
			r = new Rectangle(bounds.X + padding.Left, bounds.Y + padding.Top + (bounds.Height - padding.Vertical) / 2, bounds.Width - padding.Horizontal, 1);
			if (!color.IsEmpty)
			{
				DisplayHelp.DrawLine(graphics, r.X, r.Y, r.Right - 1, r.Y, color, 1);
			}
		}
		else
		{
			r = new Rectangle(bounds.X + padding.Left + (bounds.Width - padding.Horizontal) / 2, bounds.Y + padding.Top, 1, bounds.Height - padding.Vertical);
			if (!color.IsEmpty)
			{
				DisplayHelp.DrawLine(graphics, r.X, r.Y, r.X, r.Bottom - 1, color, 1);
			}
		}
		if (!itemSeparatorShade.IsEmpty && ((_ᅫ.Height > 1 && (__2360 == eDesignMarkerOrientation.Vertical || (Orientation == eOrientation.Vertical && __2360 == eDesignMarkerOrientation.NotSet))) || (_ᅫ.Width > 1 && (__2360 == eDesignMarkerOrientation.Horizontal || (Orientation == eOrientation.Horizontal && __2360 == eDesignMarkerOrientation.NotSet)))))
		{
			r.Inflate(1, 1);
			DisplayHelp.DrawRectangle(graphics, itemSeparatorShade, r);
		}
		if (DesignMode && Focused)
		{
			r = Bounds;
			r.Inflate(-1, -1);
			_1A6B._1A6C(p.Graphics, r, p.Colors.ItemDesignTimeBorder);
		}
		DrawInsertMarker(p.Graphics);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePadding()
	{
		if (_ᬩ.Bottom == 2 && _ᬩ.Top == 2 && _ᬩ.Left == 2)
		{
			return _ᬩ.Right != 2;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private void ResetPadding()
	{
		_ᬩ = new Padding(2, 2, 2, 2);
	}

	public override void RecalcSize()
	{
		if (Orientation == eOrientation.Horizontal)
		{
			m_Rect.Size = new Size(Dpi.Width(_ᅫ.Width + _ᬩ.Horizontal), Dpi.Height(_ᅫ.Height + _ᬩ.Vertical));
		}
		else
		{
			m_Rect.Size = new Size(Dpi.Width(_ᅫ.Height + Padding.Horizontal), Dpi.Height(_ᅫ.Width + _ᬩ.Vertical));
		}
		base.RecalcSize();
	}

	public override BaseItem Copy()
	{
		Separator separator = new Separator(m_Name);
		CopyToItem(separator);
		return separator;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		Separator separator = copy as Separator;
		base.CopyToItem(separator);
		separator.FixedSize = _ᅫ;
		separator.Padding.Left = _ᬩ.Left;
		separator.Padding.Right = _ᬩ.Right;
		separator.Padding.Top = _ᬩ.Top;
		separator.Padding.Bottom = _ᬩ.Bottom;
	}

	protected virtual void OnSeparatorOrientationChanged(eDesignMarkerOrientation newValue, eDesignMarkerOrientation oldValue)
	{
		OnAppearanceChanged();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSeparatorColor()
	{
		return !__2361.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSeparatorColor()
	{
		SeparatorColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeShadeColor()
	{
		return !__2362.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetShadeColor()
	{
		ShadeColor = Color.Empty;
	}
}
