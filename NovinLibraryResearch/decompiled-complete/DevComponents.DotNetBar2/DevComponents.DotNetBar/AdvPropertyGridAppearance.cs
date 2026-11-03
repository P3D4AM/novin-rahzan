using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace DevComponents.DotNetBar;

[TypeConverter(typeof(ExpandableObjectConverter))]
[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class AdvPropertyGridAppearance : Component, INotifyPropertyChanged
{
	private Color _ⷂ = ColorScheme.GetColor(14259860);

	private Color _ⷃ = ColorScheme.GetColor(10206041);

	private ElementStyle _ⷄ;

	private ElementStyle _ⷅ;

	private ElementStyle _ⷆ;

	private ElementStyle _ⵤ;

	private ElementStyle _ⵡ;

	private Image __2DC7;

	private PropertyChangedEventHandler চ;

	[Description("Indicates color of the node highlight when error has occurred while setting property value.")]
	[Category("Colors")]
	public Color ErrorHighlightColor
	{
		get
		{
			return _ⷂ;
		}
		set
		{
			_ⷂ = value;
			OnPropertyChanged(new PropertyChangedEventArgs("ErrorHighlightColor"));
		}
	}

	[Description("Indicates color of node highlight when property update was successful.")]
	[Category("Colors")]
	public Color SuccessHighlightColor
	{
		get
		{
			return _ⷃ;
		}
		set
		{
			_ⷃ = value;
			OnPropertyChanged(new PropertyChangedEventArgs("SuccessHighlightColor"));
		}
	}

	[Description("Gets or sets default style for property node.")]
	[DefaultValue(null)]
	public ElementStyle DefaultPropertyStyle
	{
		get
		{
			return _ⷄ;
		}
		set
		{
			if (_ⷄ != value)
			{
				if (_ⷄ != null)
				{
					_ⷄ.StyleChanged -= DefaultPropertyStyleChanged;
				}
				if (value != null)
				{
					value.StyleChanged += DefaultPropertyStyleChanged;
				}
				_ⷄ = value;
				OnPropertyChanged(new PropertyChangedEventArgs("DefaultPropertyStyle"));
			}
		}
	}

	[Description("Gets or sets style for property node when in read-only state.")]
	[DefaultValue(null)]
	public ElementStyle ReadOnlyPropertyStyle
	{
		get
		{
			return _ⷅ;
		}
		set
		{
			if (_ⷅ != value)
			{
				if (_ⷅ != null)
				{
					_ⷅ.StyleChanged -= ReadOnlyPropertyStyleChanged;
				}
				if (value != null)
				{
					value.StyleChanged += ReadOnlyPropertyStyleChanged;
				}
				_ⷅ = value;
				OnPropertyChanged(new PropertyChangedEventArgs("ReadOnlyPropertyStyle"));
			}
		}
	}

	[DefaultValue(null)]
	[Description("Gets or sets style for property node when in read-only state.")]
	public ElementStyle ValueChangedPropertyStyle
	{
		get
		{
			return _ⷆ;
		}
		set
		{
			if (_ⷆ != value)
			{
				if (_ⷆ != null)
				{
					_ⷆ.StyleChanged -= ValueChangedPropertyStyleChanged;
				}
				if (value != null)
				{
					value.StyleChanged += ValueChangedPropertyStyleChanged;
				}
				_ⷆ = value;
				OnPropertyChanged(new PropertyChangedEventArgs("ValueChangedPropertyStyle"));
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates style for property node edit cell when displaying unchanged property value.")]
	public ElementStyle DefaultEditCellStyle
	{
		get
		{
			return _ⵤ;
		}
		set
		{
			if (_ⵤ != value)
			{
				if (_ⵤ != null)
				{
					_ⵤ.StyleChanged -= DefaultEditCellStyleChanged;
				}
				if (value != null)
				{
					value.StyleChanged += ValueChangedPropertyStyleChanged;
				}
				_ⵤ = value;
				OnPropertyChanged(new PropertyChangedEventArgs("DefaultEditCellStyle"));
			}
		}
	}

	[Description("Gets or sets style for property node when in read-only state.")]
	[DefaultValue(null)]
	public ElementStyle CategoryStyle
	{
		get
		{
			return _ⵡ;
		}
		set
		{
			if (_ⵡ != value)
			{
				if (_ⵡ != null)
				{
					_ⵡ.StyleChanged -= CategoryStyleChanged;
				}
				if (value != null)
				{
					value.StyleChanged += CategoryStyleChanged;
				}
				_ⵡ = value;
				OnPropertyChanged(new PropertyChangedEventArgs("CategoryStyle"));
			}
		}
	}

	[Description("Indicates image that is displayed in property name cell when property value has failed the validation.")]
	[DefaultValue(null)]
	[Category("Appearance")]
	public Image PropertyValueErrorImage
	{
		get
		{
			return __2DC7;
		}
		set
		{
			if (__2DC7 != value)
			{
				__2DC7 = value;
				OnPropertyChanged(new PropertyChangedEventArgs("PropertyValueErrorImage"));
			}
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

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !base.DesignMode)
		{
			BarUtilities.ᆟ(ref __2DC7);
		}
		base.Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeErrorHighlightColor()
	{
		return !_ⷂ.Equals((object?)ColorScheme.GetColor(14259860));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetErrorHighlightColor()
	{
		ErrorHighlightColor = ColorScheme.GetColor(14259860);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSuccessHighlightColor()
	{
		return !_ⷃ.Equals((object?)ColorScheme.GetColor(10206041));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSuccessHighlightColor()
	{
		SuccessHighlightColor = ColorScheme.GetColor(10206041);
	}

	private void DefaultPropertyStyleChanged(object sender, EventArgs e)
	{
	}

	private void ReadOnlyPropertyStyleChanged(object sender, EventArgs e)
	{
	}

	private void ValueChangedPropertyStyleChanged(object sender, EventArgs e)
	{
	}

	private void DefaultEditCellStyleChanged(object sender, EventArgs e)
	{
	}

	private void CategoryStyleChanged(object sender, EventArgs e)
	{
	}

	private void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		চ?.Invoke(this, e);
	}
}
