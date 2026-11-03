using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace DevComponents.DotNetBar.Rendering;

[ToolboxItem(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
[DesignTimeVisible(false)]
public class RadialMenuColorTable : Component, INotifyPropertyChanged
{
	private Color _ኞ = Color.Empty;

	private Color _ኟ = Color.Empty;

	private Color _አ = Color.Empty;

	private Color _ኡ = Color.Empty;

	private Color _ኢ = Color.Empty;

	private Color _ኣ = Color.Empty;

	private Color _ኤ = Color.Empty;

	private Color _እ = Color.Empty;

	private Color _ኦ = Color.Empty;

	private Color _ኧ = Color.Empty;

	private Color _ከ = Color.Empty;

	private Color _ኩ = Color.Empty;

	private Color _ኪ = Color.Empty;

	private Color _ካ = Color.Empty;

	private PropertyChangedEventHandler চ;

	[Category("Appearance")]
	[Description("Indicates color of Radial Menu button background.")]
	public Color RadialMenuButtonBackground
	{
		get
		{
			return _ኞ;
		}
		set
		{
			_ኞ = value;
			OnPropertyChanged("RadialMenuButtonBorder");
		}
	}

	[Description("Indicates color of Radial Menu Button border.")]
	[Category("Appearance")]
	public Color RadialMenuButtonBorder
	{
		get
		{
			return _ኟ;
		}
		set
		{
			_ኟ = value;
			OnPropertyChanged("RadialMenuButtonBorder");
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of Radial Menu background.")]
	public Color RadialMenuBackground
	{
		get
		{
			return _አ;
		}
		set
		{
			_አ = value;
			OnPropertyChanged("RadialMenuBackground");
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of Radial Menu border.")]
	public Color RadialMenuBorder
	{
		get
		{
			return _ኡ;
		}
		set
		{
			_ኡ = value;
			OnPropertyChanged("RadialMenuBorder");
		}
	}

	[Description("Indicates color of border for mouse over state for the radial menu items expand part.")]
	[Category("Appearance")]
	public Color RadialMenuMouseOverBorder
	{
		get
		{
			return _ኢ;
		}
		set
		{
			_ኢ = value;
			OnPropertyChanged("RadialMenuMouseOverBorder");
		}
	}

	[Description("Indicates color of radial menu border for parts where items do not have sub-items, i.e. non-active area of the border..")]
	[Category("Appearance")]
	public Color RadialMenuInactiveBorder
	{
		get
		{
			return _ኣ;
		}
		set
		{
			_ኣ = value;
			OnPropertyChanged("RadialMenuInactiveBorder");
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of expand sign which shows menu item sub-items..")]
	public Color RadialMenuExpandForeground
	{
		get
		{
			return _ኤ;
		}
		set
		{
			_ኤ = value;
			OnPropertyChanged("RadialMenuExpandForeground");
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of radial menu item text.")]
	public Color RadialMenuItemForeground
	{
		get
		{
			return _እ;
		}
		set
		{
			_እ = value;
			OnPropertyChanged("RadialMenuItemForeground");
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of radial menu item mouse over background.")]
	public Color RadialMenuItemMouseOverBackground
	{
		get
		{
			return _ኦ;
		}
		set
		{
			_ኦ = value;
			OnPropertyChanged("RadialMenuItemMouseOverBackground");
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of radial menu item mouse over foreground.")]
	public Color RadialMenuItemMouseOverForeground
	{
		get
		{
			return _ኧ;
		}
		set
		{
			_ኧ = value;
			OnPropertyChanged("RadialMenuItemMouseOverForeground");
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of radial menu item foreground when disabled.")]
	public Color RadialMenuItemDisabledForeground
	{
		get
		{
			return _ከ;
		}
		set
		{
			_ከ = value;
			OnPropertyChanged("_RadialMenuItemDisabledForeground");
		}
	}

	[Description("Indicates background color of the circular menu item type.")]
	[Category("Appearance")]
	public Color CircularBackColor
	{
		get
		{
			return _ኩ;
		}
		set
		{
			_ኩ = value;
			OnPropertyChanged("CircularBackColor");
		}
	}

	[Category("Appearance")]
	[Description("Indicates text color of the circular menu item type.")]
	public Color CircularForeColor
	{
		get
		{
			return _ኪ;
		}
		set
		{
			_ኪ = value;
			OnPropertyChanged("CircularForeColor");
		}
	}

	[Description("Indicates border color of the circular menu item type.")]
	[Category("Appearance")]
	public Color CircularBorderColor
	{
		get
		{
			return _ካ;
		}
		set
		{
			_ካ = value;
			OnPropertyChanged("CircularBorderColor");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged
	{
		add
		{
			PropertyChangedEventHandler propertyChangedEventHandler = চ;
			PropertyChangedEventHandler propertyChangedEventHandler2;
			do
			{
				propertyChangedEventHandler2 = propertyChangedEventHandler;
				PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
				propertyChangedEventHandler = Interlocked.CompareExchange(ref চ, value2, propertyChangedEventHandler2);
			}
			while ((object)propertyChangedEventHandler != propertyChangedEventHandler2);
		}
		remove
		{
			PropertyChangedEventHandler propertyChangedEventHandler = চ;
			PropertyChangedEventHandler propertyChangedEventHandler2;
			do
			{
				propertyChangedEventHandler2 = propertyChangedEventHandler;
				PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
				propertyChangedEventHandler = Interlocked.CompareExchange(ref চ, value2, propertyChangedEventHandler2);
			}
			while ((object)propertyChangedEventHandler != propertyChangedEventHandler2);
		}
	}

	public void Apply(RadialMenuColorTable table)
	{
		table.CircularBackColor = CircularBackColor;
		table.CircularBorderColor = CircularBorderColor;
		table.CircularForeColor = CircularForeColor;
		table.RadialMenuBackground = RadialMenuBackground;
		table.RadialMenuBorder = RadialMenuBorder;
		table.RadialMenuButtonBackground = RadialMenuButtonBackground;
		table.RadialMenuButtonBorder = RadialMenuButtonBorder;
		table.RadialMenuExpandForeground = RadialMenuExpandForeground;
		table.RadialMenuInactiveBorder = RadialMenuInactiveBorder;
		table.RadialMenuItemForeground = RadialMenuItemForeground;
		table.RadialMenuItemDisabledForeground = RadialMenuItemDisabledForeground;
		table.RadialMenuItemMouseOverBackground = RadialMenuItemMouseOverBackground;
		table.RadialMenuItemMouseOverForeground = RadialMenuItemMouseOverForeground;
		table.RadialMenuMouseOverBorder = RadialMenuMouseOverBorder;
	}

	private void OnPropertyChanged(string propertyName)
	{
		OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRadialMenuButtonBackground()
	{
		return !_ኞ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRadialMenuButtonBackground()
	{
		RadialMenuButtonBackground = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRadialMenuButtonBorder()
	{
		return !_ኟ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRadialMenuButtonBorder()
	{
		RadialMenuButtonBorder = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRadialMenuBackground()
	{
		return !_አ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRadialMenuBackground()
	{
		RadialMenuBackground = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRadialMenuBorder()
	{
		return !_ኡ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRadialMenuBorder()
	{
		RadialMenuBorder = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRadialMenuMouseOverBorder()
	{
		return !_ኢ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRadialMenuMouseOverBorder()
	{
		RadialMenuMouseOverBorder = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRadialMenuInactiveBorder()
	{
		return !_ኣ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRadialMenuInactiveBorder()
	{
		RadialMenuInactiveBorder = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRadialMenuExpandForeground()
	{
		return !_ኤ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRadialMenuExpandForeground()
	{
		RadialMenuExpandForeground = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRadialMenuItemForeground()
	{
		return !_እ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRadialMenuItemForeground()
	{
		RadialMenuItemForeground = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRadialMenuItemMouseOverBackground()
	{
		return !_ኦ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRadialMenuItemMouseOverBackground()
	{
		RadialMenuItemMouseOverBackground = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRadialMenuItemMouseOverForeground()
	{
		return !_ኧ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRadialMenuItemMouseOverForeground()
	{
		RadialMenuItemMouseOverForeground = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerialize_RadialMenuItemDisabledForeground()
	{
		return !_ከ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Reset_RadialMenuItemDisabledForeground()
	{
		RadialMenuItemDisabledForeground = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCircularBackColor()
	{
		return !_ኩ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCircularBackColor()
	{
		CircularBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCircularForeColor()
	{
		return !_ኪ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCircularForeColor()
	{
		CircularForeColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCircularBorderColor()
	{
		return !_ካ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCircularBorderColor()
	{
		CircularBorderColor = Color.Empty;
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		চ?.Invoke(this, e);
	}
}
