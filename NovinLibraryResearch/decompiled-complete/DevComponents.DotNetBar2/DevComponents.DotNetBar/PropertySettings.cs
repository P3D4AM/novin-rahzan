using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using DevComponents.AdvTree;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
public class PropertySettings : Component, INotifyPropertyChanged
{
	private ConvertValueEventHandler _2D6E;

	private ConvertValueEventHandler ⵯ;

	private PropertyValueListEventHandler _2D70;

	private ElementStyle _ⵢ;

	private bool __2BB9;

	private SuperTooltipInfo __2E85;

	private SuperTooltipInfo _ݩ;

	private Image _ነ;

	private eCellPartAlignment __1712;

	private string __2E86;

	private string __2E87;

	private string _ᬆ = "";

	private PropertyDescriptor __2E88;

	private ElementStyle _ᎌ;

	private UITypeEditor __2E89;

	private PropertyValueEditor __2E8A;

	private bool _ᚉ = true;

	private ePropertyNodeType __2E8B;

	private string __2E45;

	private TypeConverter __2E3F;

	internal static string _2E8C = "PropertyName";

	internal static string _2E8D = "PropertyDescriptor";

	internal static string _2E8E = "Visible";

	private PropertyChangedEventHandler চ;

	[DefaultValue(null)]
	[Description("Indicates read-only property style.")]
	public ElementStyle ReadOnlyStyle
	{
		get
		{
			return _ⵢ;
		}
		set
		{
			if (_ⵢ != value)
			{
				if (_ⵢ != null)
				{
					_ⵢ.StyleChanged -= ReadOnlyStyleChanged;
				}
				if (value != null)
				{
					value.StyleChanged += ReadOnlyStyleChanged;
				}
				_ⵢ = value;
				OnPropertyChanged(new PropertyChangedEventArgs("ReadOnlyStyle"));
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates whether property is in read-only state.")]
	public bool ReadOnly
	{
		get
		{
			return __2BB9;
		}
		set
		{
			if (value != __2BB9)
			{
				bool oldValue = __2BB9;
				__2BB9 = value;
				OnReadOnlyChanged(oldValue, value);
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates SuperTooltip that is displayed when property is in read-only state (ReadOnly=true)")]
	[Category("Behavior")]
	public SuperTooltipInfo ReadOnlyTooltip
	{
		get
		{
			return __2E85;
		}
		set
		{
			if (value != __2E85)
			{
				SuperTooltipInfo oldValue = __2E85;
				__2E85 = value;
				OnReadOnlyTooltipChanged(oldValue, value);
			}
		}
	}

	internal bool HasConvertPropertyValueToStringHandler => _2D6E != null;

	internal bool HasConvertFromStringToPropertyValueHandler => ⵯ != null;

	internal bool HasProvidePropertyValueListHandler => _2D70 != null;

	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates SuperTooltip that is assigned to the property.")]
	[DefaultValue(null)]
	public SuperTooltipInfo Tooltip
	{
		get
		{
			return _ݩ;
		}
		set
		{
			if (value != _ݩ)
			{
				SuperTooltipInfo ݩ = _ݩ;
				_ݩ = value;
				OnTooltipChanged(ݩ, value);
			}
		}
	}

	[Category("Appearance")]
	[Localizable(true)]
	[Description("Indicates image that is displayed next to the property name in the grid.")]
	[DefaultValue(null)]
	public Image Image
	{
		get
		{
			return _ነ;
		}
		set
		{
			if (value != _ነ)
			{
				Image ነ = _ነ;
				_ነ = value;
				OnImageChanged(ነ, value);
			}
		}
	}

	[DefaultValue(eCellPartAlignment.NearCenter)]
	[Description("Indicates image alignment in relation to the property name inside of the grid cell.")]
	[Category("Appearance")]
	public eCellPartAlignment ImageAlignment
	{
		get
		{
			return __1712;
		}
		set
		{
			if (value != __1712)
			{
				eCellPartAlignment oldValue = __1712;
				__1712 = value;
				OnImageAlignmentChanged(oldValue, value);
			}
		}
	}

	[Localizable(true)]
	[Description("Indicates custom category node will be placed in instead of category specified by Category attribute.")]
	[DefaultValue(null)]
	[Category("Appearance")]
	public string Category
	{
		get
		{
			return __2E86;
		}
		set
		{
			if (value != __2E86)
			{
				string oldValue = __2E86;
				__2E86 = value;
				OnCategoryChanged(oldValue, value);
			}
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates custom property name that is displayed in the grid instead of the real property name.")]
	public string DisplayName
	{
		get
		{
			return __2E87;
		}
		set
		{
			if (value != __2E87)
			{
				string oldValue = __2E87;
				__2E87 = value;
				OnDisplayNameChanged(oldValue, value);
			}
		}
	}

	[DefaultValue("Indicates the name of the properties that settings are applied to.")]
	public string PropertyName
	{
		get
		{
			if (__2E88 != null)
			{
				return __2E88.Name;
			}
			return _ᬆ;
		}
		set
		{
			if (!(_ᬆ == value))
			{
				_ᬆ = value;
				OnPropertyNameChanged();
			}
		}
	}

	[Description("Indicates property descriptor that identifies the property that uses these settings.")]
	[DefaultValue(null)]
	public PropertyDescriptor PropertyDescriptor
	{
		get
		{
			return __2E88;
		}
		set
		{
			if (__2E88 != value)
			{
				__2E88 = value;
				OnPropertyDescriptorChanged();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates default property style.")]
	public ElementStyle Style
	{
		get
		{
			return _ᎌ;
		}
		set
		{
			if (_ᎌ != value)
			{
				if (_ᎌ != null)
				{
					_ᎌ.StyleChanged -= StyleChanged;
				}
				if (value != null)
				{
					value.StyleChanged += StyleChanged;
				}
				_ᎌ = value;
				OnPropertyChanged(new PropertyChangedEventArgs("Style"));
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	[Category("Behavior")]
	public UITypeEditor UITypeEditor
	{
		get
		{
			return __2E89;
		}
		set
		{
			if (value != __2E89)
			{
				UITypeEditor oldValue = __2E89;
				__2E89 = value;
				OnUITypeEditorChanged(oldValue, value);
			}
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	public PropertyValueEditor ValueEditor
	{
		get
		{
			return __2E8A;
		}
		set
		{
			if (value != __2E8A)
			{
				PropertyValueEditor oldValue = __2E8A;
				__2E8A = value;
				OnValueEditorChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether node is visible in property grid.")]
	public bool Visible
	{
		get
		{
			return _ᚉ;
		}
		set
		{
			if (value != _ᚉ)
			{
				bool ᚉ = _ᚉ;
				_ᚉ = value;
				OnVisibleChanged(ᚉ, value);
			}
		}
	}

	[DefaultValue(ePropertyNodeType.Default)]
	[Description("Indicates property node type that is used to edit this property value.")]
	public ePropertyNodeType PropertyNodeType
	{
		get
		{
			return __2E8B;
		}
		set
		{
			if (value != __2E8B)
			{
				ePropertyNodeType oldValue = __2E8B;
				__2E8B = value;
				OnPropertyNodeTypeChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates property description that overrides description specified on property through DescriptionAttribute. ")]
	[Localizable(true)]
	[DefaultValue(null)]
	public string Description
	{
		get
		{
			return __2E45;
		}
		set
		{
			if (value != __2E45)
			{
				string oldValue = __2E45;
				__2E45 = value;
				OnDescriptionChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public TypeConverter TypeConverter
	{
		get
		{
			return __2E3F;
		}
		set
		{
			if (value != __2E3F)
			{
				TypeConverter oldValue = __2E3F;
				__2E3F = value;
				OnTypeConverterChanged(oldValue, value);
			}
		}
	}

	[Description("Occurs when property value is being converted to text representation for display.")]
	public event ConvertValueEventHandler ConvertPropertyValueToString
	{
		add
		{
			ConvertValueEventHandler convertValueEventHandler = _2D6E;
			ConvertValueEventHandler convertValueEventHandler2;
			do
			{
				convertValueEventHandler2 = convertValueEventHandler;
				ConvertValueEventHandler value2 = (ConvertValueEventHandler)Delegate.Combine(convertValueEventHandler2, value);
				convertValueEventHandler = Interlocked.CompareExchange(ref _2D6E, value2, convertValueEventHandler2);
			}
			while ((object)convertValueEventHandler != convertValueEventHandler2);
		}
		remove
		{
			ConvertValueEventHandler convertValueEventHandler = _2D6E;
			ConvertValueEventHandler convertValueEventHandler2;
			do
			{
				convertValueEventHandler2 = convertValueEventHandler;
				ConvertValueEventHandler value2 = (ConvertValueEventHandler)Delegate.Remove(convertValueEventHandler2, value);
				convertValueEventHandler = Interlocked.CompareExchange(ref _2D6E, value2, convertValueEventHandler2);
			}
			while ((object)convertValueEventHandler != convertValueEventHandler2);
		}
	}

	[Description("Occurs when text entered by user is being converted to typed property value to be assigned to the property.")]
	public event ConvertValueEventHandler ConvertFromStringToPropertyValue
	{
		add
		{
			ConvertValueEventHandler convertValueEventHandler = ⵯ;
			ConvertValueEventHandler convertValueEventHandler2;
			do
			{
				convertValueEventHandler2 = convertValueEventHandler;
				ConvertValueEventHandler value2 = (ConvertValueEventHandler)Delegate.Combine(convertValueEventHandler2, value);
				convertValueEventHandler = Interlocked.CompareExchange(ref ⵯ, value2, convertValueEventHandler2);
			}
			while ((object)convertValueEventHandler != convertValueEventHandler2);
		}
		remove
		{
			ConvertValueEventHandler convertValueEventHandler = ⵯ;
			ConvertValueEventHandler convertValueEventHandler2;
			do
			{
				convertValueEventHandler2 = convertValueEventHandler;
				ConvertValueEventHandler value2 = (ConvertValueEventHandler)Delegate.Remove(convertValueEventHandler2, value);
				convertValueEventHandler = Interlocked.CompareExchange(ref ⵯ, value2, convertValueEventHandler2);
			}
			while ((object)convertValueEventHandler != convertValueEventHandler2);
		}
	}

	[Description("Occurs when property looks for the list of valid values for the property to show on either drop-down or use in auto-complete list.")]
	public event PropertyValueListEventHandler ProvidePropertyValueList
	{
		add
		{
			PropertyValueListEventHandler propertyValueListEventHandler = _2D70;
			PropertyValueListEventHandler propertyValueListEventHandler2;
			do
			{
				propertyValueListEventHandler2 = propertyValueListEventHandler;
				PropertyValueListEventHandler value2 = (PropertyValueListEventHandler)Delegate.Combine(propertyValueListEventHandler2, value);
				propertyValueListEventHandler = Interlocked.CompareExchange(ref _2D70, value2, propertyValueListEventHandler2);
			}
			while ((object)propertyValueListEventHandler != propertyValueListEventHandler2);
		}
		remove
		{
			PropertyValueListEventHandler propertyValueListEventHandler = _2D70;
			PropertyValueListEventHandler propertyValueListEventHandler2;
			do
			{
				propertyValueListEventHandler2 = propertyValueListEventHandler;
				PropertyValueListEventHandler value2 = (PropertyValueListEventHandler)Delegate.Remove(propertyValueListEventHandler2, value);
				propertyValueListEventHandler = Interlocked.CompareExchange(ref _2D70, value2, propertyValueListEventHandler2);
			}
			while ((object)propertyValueListEventHandler != propertyValueListEventHandler2);
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

	public PropertySettings()
	{
	}

	public PropertySettings(string propertyName)
	{
		_ᬆ = propertyName;
	}

	public PropertySettings(PropertyDescriptor propertyDescriptor)
	{
		__2E88 = propertyDescriptor;
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !base.DesignMode)
		{
			BarUtilities.ᆟ(ref _ነ);
		}
		base.Dispose(disposing);
	}

	private void ReadOnlyStyleChanged(object sender, EventArgs e)
	{
	}

	private void OnReadOnlyChanged(bool oldValue, bool newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("ReadOnly"));
	}

	private void OnReadOnlyTooltipChanged(SuperTooltipInfo oldValue, SuperTooltipInfo newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("ReadOnlyTooltip"));
	}

	protected virtual void OnConvertPropertyValueToString(ConvertValueEventArgs e)
	{
		_2D6E?.Invoke(this, e);
	}

	internal void InvokeConvertPropertyValueToString(ConvertValueEventArgs e)
	{
		OnConvertPropertyValueToString(e);
	}

	protected virtual void OnConvertFromStringToPropertyValue(ConvertValueEventArgs e)
	{
		ⵯ?.Invoke(this, e);
	}

	internal void InvokeConvertFromStringToPropertyValue(ConvertValueEventArgs e)
	{
		OnConvertFromStringToPropertyValue(e);
	}

	protected virtual void OnProvidePropertyValueList(PropertyValueListEventArgs e)
	{
		_2D70?.Invoke(this, e);
	}

	internal void InvokeProvidePropertyValueList(PropertyValueListEventArgs e)
	{
		OnProvidePropertyValueList(e);
	}

	private void OnTooltipChanged(SuperTooltipInfo oldValue, SuperTooltipInfo newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("Tooltip"));
	}

	private void OnImageChanged(Image oldValue, Image newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("Image"));
	}

	private void OnImageAlignmentChanged(eCellPartAlignment oldValue, eCellPartAlignment newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("ImageAlignment"));
	}

	private void OnCategoryChanged(string oldValue, string newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("Category"));
	}

	private void OnDisplayNameChanged(string oldValue, string newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("DisplayName"));
	}

	private void OnPropertyNameChanged()
	{
		OnPropertyChanged(new PropertyChangedEventArgs("PropertyName"));
	}

	private void OnPropertyDescriptorChanged()
	{
		OnPropertyChanged(new PropertyChangedEventArgs("PropertyDescriptor"));
	}

	private void StyleChanged(object sender, EventArgs e)
	{
	}

	private void OnUITypeEditorChanged(UITypeEditor oldValue, UITypeEditor newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("UITypeEditor"));
	}

	private void OnValueEditorChanged(PropertyValueEditor oldValue, PropertyValueEditor newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("ValueEditor"));
	}

	private void OnVisibleChanged(bool oldValue, bool newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
	}

	private void OnPropertyNodeTypeChanged(ePropertyNodeType oldValue, ePropertyNodeType newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("PropertyNodeType"));
	}

	private void OnDescriptionChanged(string oldValue, string newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("Description"));
	}

	protected virtual void OnTypeConverterChanged(TypeConverter oldValue, TypeConverter newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("TypeConverter"));
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		চ?.Invoke(this, e);
	}
}
