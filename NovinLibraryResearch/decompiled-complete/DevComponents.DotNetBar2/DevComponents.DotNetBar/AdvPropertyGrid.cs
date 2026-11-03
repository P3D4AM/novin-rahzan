using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.AdvTree;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(AdvPropertyGrid), "AdvPropertyGrid.ico")]
public class AdvPropertyGrid : ContainerControl, ISupportInitialize, IPropertyGridLocalizer, IPropertyElementStyleProvider, INotifyPropertyChanged
{
	private const string ⵛ = "DefaultNodeStyle";

	private Bar _ⵜ;

	private DevComponents.AdvTree.AdvTree _ⵝ;

	private ExpandableSplitter _ⵞ;

	private PanelControl _ⵟ;

	private SuperTooltip _ⵠ;

	private ElementStyle _ⵡ;

	private ElementStyle _ⵢ;

	private ElementStyle _ⵣ;

	private ElementStyle _ⵤ;

	private AdvPropertyGridLocalization _ⵥ;

	private TextBoxItem _ⵦ;

	private ButtonItem _ⵧ;

	private ButtonItem __2D68;

	private bool __2D69;

	private List<Node> __2D6A = new List<Node>();

	private PropertyNode __2D6B;

	private LocalizeEventHandler _2D6C;

	private PropertyErrorTooltipEventHandler _2D6D;

	private ConvertValueEventHandler _2D6E;

	private ConvertValueEventHandler ⵯ;

	private PropertyValueListEventHandler _2D70;

	private ProvideUITypeEditorEventHandler _2D71;

	private PropertyChangedEventHandler _2D72;

	private PropertyValueChangingEventHandler _2D73;

	private ValidatePropertyValueEventHandler _2D74;

	private EventHandler _2D75;

	private object[] __2D76;

	private AttributeCollection __2D77;

	private bool __2D78;

	private static bool __2D79 = true;

	private string[] __2D7A = new string[0];

	private string[] __2D7B = new string[0];

	private AdvPropertyGridAppearance _ᗹ;

	private PropertySettingsCollection __2D7C;

	private ePropertySort __2D7D = ePropertySort.CategorizedAlphabetical;

	private ePropertyGridHelpType __2D7E = ePropertyGridHelpType.SuperTooltip;

	private bool __2D7F = true;

	private bool _ⱻ = true;

	private bool _ⶀ = true;

	private static char _ⶁ = '*';

	private ePropertySort _ⶂ = ePropertySort.Alphabetical;

	private PropertyChangedEventHandler চ;

	private bool __2054;

	internal bool ⵒ => _2D74 != null;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public object[] SelectedObjects
	{
		get
		{
			return __2D76;
		}
		set
		{
			if (value != __2D76)
			{
				object[] ك = __2D76;
				__2D76 = value;
				ⶖ(ك, value);
			}
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	public object SelectedObject
	{
		get
		{
			if (__2D76 == null)
			{
				return null;
			}
			return __2D76[0];
		}
		set
		{
			if (value != null)
			{
				SelectedObjects = new object[1] { value };
			}
			else
			{
				SelectedObjects = null;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public AttributeCollection BrowsableAttributes
	{
		get
		{
			if (__2D77 == null)
			{
				__2D77 = new AttributeCollection(new BrowsableAttribute(browsable: true));
			}
			return __2D77;
		}
		set
		{
			if (value == null || value == AttributeCollection.Empty)
			{
				__2D77 = new AttributeCollection(BrowsableAttribute.Yes);
			}
			else
			{
				Attribute[] array = new Attribute[value.Count];
				value.CopyTo(array, 0);
				__2D77 = new AttributeCollection(array);
			}
			if (__2D76 != null && __2D76.Length > 0)
			{
				ⶢ(ⶣ: false);
			}
		}
	}

	public static bool HighlightPropertyOnUpdate
	{
		get
		{
			return __2D79;
		}
		set
		{
			__2D79 = value;
		}
	}

	[Description("List of property names that are not loaded into property grid regardless of their Browsable attribute setting.")]
	[Category("Behavior")]
	public string[] IgnoredProperties
	{
		get
		{
			return __2D7A;
		}
		set
		{
			if (value == null)
			{
				value = new string[0];
			}
			__2D7A = value;
			ⶢ(ⶣ: false);
		}
	}

	[Description("List of category names properties below to that are not loaded into property grid regardless of the property Browsable attribute setting.")]
	[Category("Behavior")]
	public string[] IgnoredCategories
	{
		get
		{
			return __2D7B;
		}
		set
		{
			if (value == null)
			{
				value = new string[0];
			}
			__2D7B = value;
			ⶢ(ⶣ: false);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public SuperTooltip SuperTooltip
	{
		get
		{
			return _ⵠ;
		}
		internal set
		{
			_ⵠ = value;
		}
	}

	[Description("Defines appearance of the control.")]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public AdvPropertyGridAppearance Appearance => _ᗹ;

	internal bool ⵓ => _2D6E != null;

	internal bool ⵔ => ⵯ != null;

	internal bool ⵕ => _2D70 != null;

	internal bool ⵖ => _2D71 != null;

	public PropertySettingsCollection PropertySettings => __2D7C;

	[Description("Indicates property sorting inside of the grid.")]
	[Category("Appearance")]
	[DefaultValue(ePropertySort.CategorizedAlphabetical)]
	public ePropertySort PropertySort
	{
		get
		{
			return __2D7D;
		}
		set
		{
			if (value != __2D7D)
			{
				ePropertySort ك = __2D7D;
				__2D7D = value;
				ⶳ(ك, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates help type that is provided by the control.")]
	[DefaultValue(ePropertyGridHelpType.SuperTooltip)]
	public ePropertyGridHelpType HelpType
	{
		get
		{
			return __2D7E;
		}
		set
		{
			if (value != __2D7E)
			{
				ePropertyGridHelpType ك = __2D7E;
				__2D7E = value;
				ⶴ(ك, value);
			}
		}
	}

	[Browsable(false)]
	public PanelControl HelpPanel => _ⵟ;

	[Browsable(false)]
	public ExpandableSplitter HelpExpandableSplitter => _ⵞ;

	[Browsable(false)]
	public DevComponents.AdvTree.AdvTree PropertyTree => _ⵝ;

	[Category("Appearance")]
	[Description("Indicates grid lines color.")]
	public Color GridLinesColor
	{
		get
		{
			return _ⵝ.GridLinesColor;
		}
		set
		{
			_ⵝ.GridLinesColor = value;
		}
	}

	[Browsable(false)]
	public Bar Toolbar => _ⵜ;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether toolbar is visible.")]
	public bool ToolbarVisible
	{
		get
		{
			return __2D7F;
		}
		set
		{
			if (value != __2D7F)
			{
				bool ك = __2D7F;
				__2D7F = value;
				ⶵ(ك, value);
			}
		}
	}

	[Description("Indicates whether search text box that allows property filtering is visible")]
	[Category("Appearance")]
	[DefaultValue(true)]
	public bool SearchBoxVisible
	{
		get
		{
			return _ⱻ;
		}
		set
		{
			if (value != _ⱻ)
			{
				bool ⱻ = _ⱻ;
				_ⱻ = value;
				ⲕ(ⱻ, value);
			}
		}
	}

	[Browsable(false)]
	public TextBoxItem SearchTextBoxItem => _ⵦ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[NotifyParentProperty(true)]
	[Browsable(true)]
	[Category("Localization")]
	[Description("Gets system text used by the component.")]
	public AdvPropertyGridLocalization SystemText => _ⵥ;

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether Tab key navigates between property nodes.")]
	public bool TabKeyNavigation
	{
		get
		{
			return _ⶀ;
		}
		set
		{
			_ⶀ = value;
		}
	}

	public static char PasswordChar
	{
		get
		{
			return _ⶁ;
		}
		set
		{
			_ⶁ = value;
		}
	}

	[DefaultValue(ePropertySort.Alphabetical)]
	[Description("Indicates sub property default sorting.")]
	[Category("Behavior")]
	public ePropertySort SubPropertiesDefaultSort
	{
		get
		{
			return _ⶂ;
		}
		set
		{
			if (value != _ⶂ)
			{
				ePropertySort ⶂ = _ⶂ;
				_ⶂ = value;
				OnSubPropertiesDefaultSortChanged(ⶂ, value);
			}
		}
	}

	ElementStyle IPropertyElementStyleProvider.ⵗ
	{
		get
		{
			if (_ᗹ.ReadOnlyPropertyStyle != null && _ᗹ.ReadOnlyPropertyStyle.Custom)
			{
				return _ᗹ.ReadOnlyPropertyStyle;
			}
			return _ⵢ;
		}
	}

	ElementStyle IPropertyElementStyleProvider.ⵘ
	{
		get
		{
			if (_ᗹ.ValueChangedPropertyStyle != null && _ᗹ.ValueChangedPropertyStyle.Custom)
			{
				return _ᗹ.ValueChangedPropertyStyle;
			}
			return _ⵣ;
		}
	}

	ElementStyle IPropertyElementStyleProvider.ⵙ
	{
		get
		{
			if (_ᗹ.DefaultEditCellStyle != null && _ᗹ.DefaultEditCellStyle.Custom)
			{
				return _ᗹ.DefaultEditCellStyle;
			}
			return _ⵤ;
		}
	}

	ElementStyle IPropertyElementStyleProvider.ⵚ
	{
		get
		{
			if (_ᗹ.CategoryStyle != null && _ᗹ.CategoryStyle.Custom)
			{
				return _ᗹ.CategoryStyle;
			}
			return _ⵡ;
		}
	}

	[Description("Occurs when property grid needs property names translated for UI.")]
	public event LocalizeEventHandler Localize
	{
		add
		{
			LocalizeEventHandler localizeEventHandler = _2D6C;
			LocalizeEventHandler localizeEventHandler2;
			do
			{
				localizeEventHandler2 = localizeEventHandler;
				LocalizeEventHandler value2 = (LocalizeEventHandler)Delegate.Combine(localizeEventHandler2, value);
				localizeEventHandler = Interlocked.CompareExchange(ref _2D6C, value2, localizeEventHandler2);
			}
			while ((object)localizeEventHandler != localizeEventHandler2);
		}
		remove
		{
			LocalizeEventHandler localizeEventHandler = _2D6C;
			LocalizeEventHandler localizeEventHandler2;
			do
			{
				localizeEventHandler2 = localizeEventHandler;
				LocalizeEventHandler value2 = (LocalizeEventHandler)Delegate.Remove(localizeEventHandler2, value);
				localizeEventHandler = Interlocked.CompareExchange(ref _2D6C, value2, localizeEventHandler2);
			}
			while ((object)localizeEventHandler != localizeEventHandler2);
		}
	}

	[Description("Occurs when Super Tooltip that is displayed for the error that occurred when property is being set is being assigned to a property node.")]
	public event PropertyErrorTooltipEventHandler PrepareErrorSuperTooltip
	{
		add
		{
			PropertyErrorTooltipEventHandler propertyErrorTooltipEventHandler = _2D6D;
			PropertyErrorTooltipEventHandler propertyErrorTooltipEventHandler2;
			do
			{
				propertyErrorTooltipEventHandler2 = propertyErrorTooltipEventHandler;
				PropertyErrorTooltipEventHandler value2 = (PropertyErrorTooltipEventHandler)Delegate.Combine(propertyErrorTooltipEventHandler2, value);
				propertyErrorTooltipEventHandler = Interlocked.CompareExchange(ref _2D6D, value2, propertyErrorTooltipEventHandler2);
			}
			while ((object)propertyErrorTooltipEventHandler != propertyErrorTooltipEventHandler2);
		}
		remove
		{
			PropertyErrorTooltipEventHandler propertyErrorTooltipEventHandler = _2D6D;
			PropertyErrorTooltipEventHandler propertyErrorTooltipEventHandler2;
			do
			{
				propertyErrorTooltipEventHandler2 = propertyErrorTooltipEventHandler;
				PropertyErrorTooltipEventHandler value2 = (PropertyErrorTooltipEventHandler)Delegate.Remove(propertyErrorTooltipEventHandler2, value);
				propertyErrorTooltipEventHandler = Interlocked.CompareExchange(ref _2D6D, value2, propertyErrorTooltipEventHandler2);
			}
			while ((object)propertyErrorTooltipEventHandler != propertyErrorTooltipEventHandler2);
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

	[Description("Occurs when property node needs the UITypeEditor for the property. You can handle this event and provide your own UITypeEditor to be used instead of the one specified on the property.")]
	public event ProvideUITypeEditorEventHandler ProvideUITypeEditor
	{
		add
		{
			ProvideUITypeEditorEventHandler provideUITypeEditorEventHandler = _2D71;
			ProvideUITypeEditorEventHandler provideUITypeEditorEventHandler2;
			do
			{
				provideUITypeEditorEventHandler2 = provideUITypeEditorEventHandler;
				ProvideUITypeEditorEventHandler value2 = (ProvideUITypeEditorEventHandler)Delegate.Combine(provideUITypeEditorEventHandler2, value);
				provideUITypeEditorEventHandler = Interlocked.CompareExchange(ref _2D71, value2, provideUITypeEditorEventHandler2);
			}
			while ((object)provideUITypeEditorEventHandler != provideUITypeEditorEventHandler2);
		}
		remove
		{
			ProvideUITypeEditorEventHandler provideUITypeEditorEventHandler = _2D71;
			ProvideUITypeEditorEventHandler provideUITypeEditorEventHandler2;
			do
			{
				provideUITypeEditorEventHandler2 = provideUITypeEditorEventHandler;
				ProvideUITypeEditorEventHandler value2 = (ProvideUITypeEditorEventHandler)Delegate.Remove(provideUITypeEditorEventHandler2, value);
				provideUITypeEditorEventHandler = Interlocked.CompareExchange(ref _2D71, value2, provideUITypeEditorEventHandler2);
			}
			while ((object)provideUITypeEditorEventHandler != provideUITypeEditorEventHandler2);
		}
	}

	[Description("Occurs when user changes the value of the property in property grid.")]
	public event PropertyChangedEventHandler PropertyValueChanged
	{
		add
		{
			PropertyChangedEventHandler propertyChangedEventHandler = _2D72;
			PropertyChangedEventHandler propertyChangedEventHandler2;
			do
			{
				propertyChangedEventHandler2 = propertyChangedEventHandler;
				PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
				propertyChangedEventHandler = Interlocked.CompareExchange(ref _2D72, value2, propertyChangedEventHandler2);
			}
			while ((object)propertyChangedEventHandler != propertyChangedEventHandler2);
		}
		remove
		{
			PropertyChangedEventHandler propertyChangedEventHandler = _2D72;
			PropertyChangedEventHandler propertyChangedEventHandler2;
			do
			{
				propertyChangedEventHandler2 = propertyChangedEventHandler;
				PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
				propertyChangedEventHandler = Interlocked.CompareExchange(ref _2D72, value2, propertyChangedEventHandler2);
			}
			while ((object)propertyChangedEventHandler != propertyChangedEventHandler2);
		}
	}

	[Description("Occurs before property value entered by the user is set on the property. You can cancel internal assignment by property grid by setting Handled=true on event arguments.")]
	public event PropertyValueChangingEventHandler PropertyValueChanging
	{
		add
		{
			PropertyValueChangingEventHandler propertyValueChangingEventHandler = _2D73;
			PropertyValueChangingEventHandler propertyValueChangingEventHandler2;
			do
			{
				propertyValueChangingEventHandler2 = propertyValueChangingEventHandler;
				PropertyValueChangingEventHandler value2 = (PropertyValueChangingEventHandler)Delegate.Combine(propertyValueChangingEventHandler2, value);
				propertyValueChangingEventHandler = Interlocked.CompareExchange(ref _2D73, value2, propertyValueChangingEventHandler2);
			}
			while ((object)propertyValueChangingEventHandler != propertyValueChangingEventHandler2);
		}
		remove
		{
			PropertyValueChangingEventHandler propertyValueChangingEventHandler = _2D73;
			PropertyValueChangingEventHandler propertyValueChangingEventHandler2;
			do
			{
				propertyValueChangingEventHandler2 = propertyValueChangingEventHandler;
				PropertyValueChangingEventHandler value2 = (PropertyValueChangingEventHandler)Delegate.Remove(propertyValueChangingEventHandler2, value);
				propertyValueChangingEventHandler = Interlocked.CompareExchange(ref _2D73, value2, propertyValueChangingEventHandler2);
			}
			while ((object)propertyValueChangingEventHandler != propertyValueChangingEventHandler2);
		}
	}

	[Description("Occurs when users changes the property value and attempts to commit the changes. This even allows you to validate the value and show error message if value is invalid and cancel its application.")]
	public event ValidatePropertyValueEventHandler ValidatePropertyValue
	{
		add
		{
			ValidatePropertyValueEventHandler validatePropertyValueEventHandler = _2D74;
			ValidatePropertyValueEventHandler validatePropertyValueEventHandler2;
			do
			{
				validatePropertyValueEventHandler2 = validatePropertyValueEventHandler;
				ValidatePropertyValueEventHandler value2 = (ValidatePropertyValueEventHandler)Delegate.Combine(validatePropertyValueEventHandler2, value);
				validatePropertyValueEventHandler = Interlocked.CompareExchange(ref _2D74, value2, validatePropertyValueEventHandler2);
			}
			while ((object)validatePropertyValueEventHandler != validatePropertyValueEventHandler2);
		}
		remove
		{
			ValidatePropertyValueEventHandler validatePropertyValueEventHandler = _2D74;
			ValidatePropertyValueEventHandler validatePropertyValueEventHandler2;
			do
			{
				validatePropertyValueEventHandler2 = validatePropertyValueEventHandler;
				ValidatePropertyValueEventHandler value2 = (ValidatePropertyValueEventHandler)Delegate.Remove(validatePropertyValueEventHandler2, value);
				validatePropertyValueEventHandler = Interlocked.CompareExchange(ref _2D74, value2, validatePropertyValueEventHandler2);
			}
			while ((object)validatePropertyValueEventHandler != validatePropertyValueEventHandler2);
		}
	}

	[Description("Occurs after properties have been loaded into the property grid.")]
	public event EventHandler PropertiesLoaded
	{
		add
		{
			EventHandler eventHandler = _2D75;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2D75, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2D75;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2D75, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
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

	public AdvPropertyGrid()
	{
		_ⵥ = new AdvPropertyGridLocalization();
		_ⵥ.PropertyChanged += ⶇ;
		_ᗹ = new AdvPropertyGridAppearance();
		_ᗹ.PropertyChanged += ⶫ;
		__2D7C = new PropertySettingsCollection(this);
		_ⵜ = ⶋ();
		_ⵝ = ⶒ();
		_ⵠ = ⶊ();
		_ⵞ = ⶈ();
		_ⵟ = ⶉ();
		_ⵞ.ExpandableControl = (Control)(object)_ⵟ;
		((Control)this).Controls.Add((Control)(object)_ⵝ);
		((Control)this).Controls.Add((Control)(object)_ⵜ);
		((Control)this).Controls.Add((Control)(object)_ⵞ);
		((Control)this).Controls.Add((Control)(object)_ⵟ);
		((Control)_ⵞ).Visible = false;
		((Control)_ⵟ).Visible = false;
	}

	internal void ⶃ(ValidatePropertyValueEventArgs ھ)
	{
		_2D74?.Invoke(this, ھ);
	}

	internal bool ⶄ(string फ़, object ل, PropertyDescriptor ⶅ, string ⶆ)
	{
		PropertyValueChangingEventArgs propertyValueChangingEventArgs = new PropertyValueChangingEventArgs(फ़, ل, ⶅ, ⶆ);
		OnPropertyValueChanging(propertyValueChangingEventArgs);
		return propertyValueChangingEventArgs.Handled;
	}

	protected virtual void OnPropertyValueChanging(PropertyValueChangingEventArgs e)
	{
		_2D73?.Invoke(this, e);
	}

	public void InvokePropertyValueChanged(string propertyName, object newValue, object oldValue)
	{
		OnPropertyValueChanged(propertyName);
	}

	protected virtual void OnPropertyValueChanged(string propertyName)
	{
		_2D72?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void ⶇ(object _٠, PropertyChangedEventArgs ؿ)
	{
		if (ؿ.PropertyName == "CategorizeToolbarTooltip")
		{
			_ⵧ.Tooltip = _ⵥ.CategorizeToolbarTooltip;
		}
		else if (ؿ.PropertyName == "AlphabeticalToolbarTooltip")
		{
			__2D68.Tooltip = _ⵥ.AlphabeticalToolbarTooltip;
		}
		else if (ؿ.PropertyName == "SearchBoxWatermarkText")
		{
			_ⵦ.WatermarkText = _ⵥ.SearchBoxWatermarkText;
		}
	}

	private ExpandableSplitter ⶈ()
	{
		ExpandableSplitter expandableSplitter = new ExpandableSplitter();
		expandableSplitter.Style = eSplitterStyle.Office2007;
		((Control)expandableSplitter).Dock = (DockStyle)2;
		((Control)expandableSplitter).Height = 6;
		return expandableSplitter;
	}

	private PanelControl ⶉ()
	{
		PanelControl panelControl = new PanelControl();
		((Control)panelControl).Dock = (DockStyle)2;
		((Control)panelControl).Height = 42;
		panelControl.ApplyPanelStyle(eDotNetBarStyle.Office2007);
		panelControl.Style.BackColor = Color.White;
		panelControl.Style.BackColor2 = Color.Empty;
		panelControl.Style.TextColor = Color.Black;
		panelControl.Style.WordWrap = true;
		panelControl.Style.TextAlignment = eStyleTextAlignment.Near;
		return panelControl;
	}

	private SuperTooltip ⶊ()
	{
		SuperTooltip superTooltip = new SuperTooltip();
		superTooltip.PositionBelowControl = false;
		superTooltip.ShowTooltipImmediately = false;
		return superTooltip;
	}

	private Bar ⶋ()
	{
		Bar bar = new Bar();
		((Control)bar).Dock = (DockStyle)1;
		bar.RoundCorners = false;
		ButtonItem buttonItem = new ButtonItem("sys_Category");
		buttonItem.Image = (Image)(object)BarFunctions.ই("SystemImages.Category.png");
		buttonItem.OptionGroup = "sorting";
		buttonItem.Checked = true;
		buttonItem.OptionGroupChanging += ⶑ;
		buttonItem.Tooltip = _ⵥ.CategorizeToolbarTooltip;
		bar.Items.Add(buttonItem);
		_ⵧ = buttonItem;
		buttonItem = new ButtonItem("sys_Sort");
		buttonItem.Image = (Image)(object)BarFunctions.ই("SystemImages.Sort.png");
		buttonItem.OptionGroup = "sorting";
		buttonItem.OptionGroupChanging += ⶑ;
		buttonItem.Tooltip = _ⵥ.AlphabeticalToolbarTooltip;
		bar.Items.Add(buttonItem);
		__2D68 = buttonItem;
		TextBoxItem textBoxItem = new TextBoxItem();
		textBoxItem.WatermarkEnabled = true;
		textBoxItem.WatermarkText = _ⵥ.SearchBoxWatermarkText;
		textBoxItem.WatermarkBehavior = eWatermarkBehavior.HideNonEmpty;
		textBoxItem.TextBoxWidth = 90;
		textBoxItem.BeginGroup = true;
		textBoxItem.InputTextChanged += ⶍ;
		((Control)textBoxItem.TextBox).GotFocus += ⶌ;
		textBoxItem.Enabled = false;
		_ⵦ = textBoxItem;
		bar.Items.Add(textBoxItem);
		bar.Style = eDotNetBarStyle.Office2007;
		return bar;
	}

	private void ⶌ(object _٠, EventArgs ؿ)
	{
		if (!string.IsNullOrEmpty(_ⵦ.ControlText))
		{
			((TextBoxBase)_ⵦ.TextBox).SelectAll();
		}
	}

	private void ⶍ(object _٠)
	{
		string text = _ⵦ.ControlText.ToLower();
		if (string.IsNullOrEmpty(text) && !__2D69)
		{
			return;
		}
		if (string.IsNullOrEmpty(text))
		{
			_ⵝ.BeginUpdate();
			foreach (Node item in __2D6A)
			{
				item.Visible = true;
			}
			_ⵝ.EndUpdate();
			__2D69 = false;
			__2D6A.Clear();
			return;
		}
		_ⵝ.BeginUpdate();
		foreach (Node item2 in __2D6A)
		{
			item2.Visible = true;
		}
		ⶎ(_ⵝ.Nodes, __2D6A, text);
		__2D69 = __2D6A.Count > 0;
		_ⵝ.EndUpdate();
	}

	private void ⶎ(NodeCollection _2737, List<Node> ⶏ, string ⶐ)
	{
		ⶐ = ⶐ.ToUpper();
		foreach (Node item in _2737)
		{
			if (item is PropertyNode propertyNode && !propertyNode.Text.ToUpper().Contains(ⶐ))
			{
				propertyNode.Visible = false;
				ⶏ.Add(propertyNode);
			}
			if (item.HasChildNodes)
			{
				ⶎ(item.Nodes, ⶏ, ⶐ);
			}
		}
	}

	private void ⶑ(object _٠, OptionGroupChangingEventArgs ؿ)
	{
		if (ؿ.NewChecked == _ⵧ)
		{
			if (PropertySort != ePropertySort.CategorizedAlphabetical)
			{
				PropertySort = ePropertySort.CategorizedAlphabetical;
			}
		}
		else if (ؿ.NewChecked == __2D68 && PropertySort != ePropertySort.Alphabetical)
		{
			PropertySort = ePropertySort.Alphabetical;
		}
	}

	private DevComponents.AdvTree.AdvTree ⶒ()
	{
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		DevComponents.AdvTree.AdvTree advTree = new DevComponents.AdvTree.AdvTree();
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Name = "DefaultNodeStyle";
		elementStyle.TextColor = SystemColors.ControlText;
		elementStyle.TextTrimming = eStyleTextTrimming.None;
		elementStyle.TextLineAlignment = eStyleTextAlignment.Center;
		advTree.Styles.Add(elementStyle);
		advTree.NodeStyle = elementStyle;
		_ⵢ = new ElementStyle();
		_ⵢ.Name = "ReadOnlyStyle";
		_ⵢ.TextColor = SystemColors.ControlDark;
		_ⵢ.TextTrimming = eStyleTextTrimming.None;
		advTree.Styles.Add(_ⵢ);
		_ⵡ = new ElementStyle();
		_ⵡ.Name = "CategoryStyle";
		_ⵡ.TextColor = SystemColors.ControlText;
		_ⵡ.BackColorSchemePart = eColorSchemePart.PanelBackground;
		_ⵡ.Font = new Font(((Control)this).Font, (FontStyle)1);
		advTree.Styles.Add(_ⵡ);
		_ⵣ = new ElementStyle();
		_ⵣ.Name = "ValueChangedStyle";
		_ⵣ.Font = new Font(((Control)this).Font, (FontStyle)1);
		advTree.Styles.Add(_ⵣ);
		((Control)advTree).AccessibleRole = (AccessibleRole)35;
		((Control)advTree).AllowDrop = false;
		advTree.DragDropEnabled = false;
		((Control)advTree).BackColor = SystemColors.Window;
		advTree.BackgroundStyle.Class = "TreeBorderKey";
		((Control)advTree).Name = "propertyTree";
		advTree.PathSeparator = ";";
		((Control)advTree).Size = new Size(150, 100);
		((Control)advTree).TabIndex = 0;
		advTree.ExpandWidth = 12;
		advTree.ExpandButtonType = eExpandButtonType.Triangle;
		advTree.Indent = 0;
		advTree.GridRowLines = true;
		advTree.GridColumnLines = true;
		advTree.GridColumnLineResizeEnabled = true;
		advTree.GridLinesColor = Color.WhiteSmoke;
		advTree.HScrollBarVisible = false;
		advTree.SelectionPerCell = true;
		advTree.SelectionBoxStyle = eSelectionStyle.FullRowSelect;
		advTree.SelectionFocusAware = false;
		advTree.CellHorizontalSpacing = 2;
		advTree.BeforeNodeSelect += ⶤ;
		advTree.KeyboardSearchEnabled = false;
		((Control)advTree).KeyPress += new KeyPressEventHandler(_2DA7);
		advTree.ColumnResizing += ⶩ;
		((Control)advTree).DoubleClick += ⶪ;
		((Control)advTree).Leave += ⶓ;
		ColumnHeader columnHeader = new ColumnHeader();
		columnHeader.Name = "propertyName";
		columnHeader.Width.Relative = 50;
		advTree.Columns.Add(columnHeader);
		columnHeader = new ColumnHeader();
		columnHeader.Name = "propertyValue";
		columnHeader.Width.Relative = 50;
		advTree.Columns.Add(columnHeader);
		advTree.ColumnsVisible = false;
		((Control)advTree).Dock = (DockStyle)5;
		return advTree;
	}

	private void ⶓ(object _٠, EventArgs ؿ)
	{
		if (__2D6B != null)
		{
			__2D6B.ApplyEdit();
		}
	}

	internal void ⶔ(PropertyNode _0653)
	{
		if (_0653.IsEditing)
		{
			__2D6B = _0653;
		}
		else
		{
			__2D6B = null;
		}
	}

	protected override void Dispose(bool disposing)
	{
		((ContainerControl)this).Dispose(disposing);
		_2D9A();
		ⶕ(_ⵡ);
		ⶕ(_ⵣ);
	}

	private void ⶕ(ElementStyle _0657)
	{
		if (_0657 != null && _0657.Font != null)
		{
			_0657.Font.Dispose();
			_0657.Font = null;
		}
	}

	protected virtual void OnValidatePropertyValue(ValidatePropertyValueEventArgs e)
	{
		_2D74?.Invoke(this, e);
	}

	protected virtual void OnPropertiesLoaded(EventArgs e)
	{
		_2D75?.Invoke(this, e);
	}

	public bool CommitEdit()
	{
		if (_ⵝ.SelectedNode is PropertyNode { IsEditing: not false } propertyNode)
		{
			return propertyNode.ApplyEdit();
		}
		return false;
	}

	private void ⶖ(object[] ك, object[] ل)
	{
		if (ك != null && ك.Length == 1 && ك[0] is INotifyPropertyChanged)
		{
			((INotifyPropertyChanged)ك[0]).PropertyChanged -= _2D97;
		}
		OnPropertyChanged(new PropertyChangedEventArgs("SelectedObjects"));
		ⶢ(ⶣ: true);
		if (ل != null && ل.Length > 0 && ل.Length == 1 && ل[0] is INotifyPropertyChanged)
		{
			((INotifyPropertyChanged)__2D76[0]).PropertyChanged += _2D97;
		}
	}

	private void _2D97(object _٠, PropertyChangedEventArgs ؿ)
	{
		UpdatePropertyValue(ؿ.PropertyName);
	}

	private void _2D98(Node _06EC, string फ़)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		if (((Control)this).InvokeRequired)
		{
			((Control)this).BeginInvoke((Delegate)(MethodInvoker)delegate
			{
				_2D99(_06EC, फ़);
			});
		}
		else
		{
			_2D99(_06EC, फ़);
		}
	}

	private void _2D99(Node _06EC, string फ़)
	{
		if (_06EC is PropertyNode { PropertyDescriptor: not null } propertyNode && propertyNode.PropertyDescriptor.Name == फ़)
		{
			propertyNode.UpdateDisplayedValue();
		}
		foreach (Node node in _06EC.Nodes)
		{
			_2D98(node, फ़);
		}
	}

	public void UpdatePropertyValue(string propertyName)
	{
		foreach (Node node in _ⵝ.Nodes)
		{
			_2D98(node, propertyName);
		}
	}

	private void _2D9A()
	{
		_ⵝ.BeginUpdate();
		foreach (Node node in _ⵝ.Nodes)
		{
			_2D9B(node, this);
		}
		_ⵝ.Nodes.Clear();
		_ⵝ.EndUpdate(performLayoutAndRefresh: false);
	}

	internal static void _2D9B(Node _0653, AdvPropertyGrid _2D9C)
	{
		_0653.Style = null;
		PropertyNode propertyNode = _0653 as PropertyNode;
		if (propertyNode != null)
		{
			propertyNode.ம = true;
			if (propertyNode.IsEditing)
			{
				propertyNode.CancelEdit();
				propertyNode.ExitEditorMode(eTreeAction.Code);
			}
			propertyNode.PropertySettings = null;
			propertyNode._2E31 = null;
			propertyNode.PropertyDescriptor = null;
			_2D9C.SuperTooltip.SetSuperTooltip(_0653, null);
		}
		if (_0653.HasChildNodes)
		{
			foreach (Node node in _0653.Nodes)
			{
				_2D9B(node, _2D9C);
			}
		}
		_0653.Dispose();
		if (propertyNode != null)
		{
			propertyNode.ம = false;
		}
	}

	private _2E8F _2D9D(object _2D9E)
	{
		if (_2D9E == null)
		{
			throw new ArgumentNullException("selectedObject argument cannot be null");
		}
		Attribute[] array = new Attribute[BrowsableAttributes.Count];
		BrowsableAttributes.CopyTo(array, 0);
		_2E8F _2E8F2 = new _2E8F(_2D9E, array, ⶠ(), new List<string>(__2D7A), new List<string>(__2D7B), this, __2D7C);
		_2E8F2._2E90 = __2D7E;
		return _2E8F2;
	}

	private _2E8F _2D9D(object[] _2D9F)
	{
		if (_2D9F == null)
		{
			throw new ArgumentNullException("selectedObject argument cannot be null");
		}
		Attribute[] array = new Attribute[BrowsableAttributes.Count];
		BrowsableAttributes.CopyTo(array, 0);
		_2E8F _2E8F2 = new _2E8F(_2D9F, array, ⶠ(), new List<string>(__2D7A), new List<string>(__2D7B), this, __2D7C);
		_2E8F2._2E90 = __2D7E;
		return _2E8F2;
	}

	internal _2EA5 ⶠ()
	{
		return new _2EA5(this);
	}

	public void RefreshPropertyValues()
	{
		foreach (Node node in _ⵝ.Nodes)
		{
			ⶡ(node);
		}
	}

	internal void ⶡ(Node ڙ)
	{
		if (ڙ is PropertyNode)
		{
			((PropertyNode)ڙ).UpdateDisplayedValue();
		}
		foreach (Node node in ڙ.Nodes)
		{
			ⶡ(node);
		}
	}

	public void RefreshProperties()
	{
		ⶢ(ⶣ: false);
	}

	public void CollapseAllGridItems()
	{
		if (_ⵝ != null && _ⵝ.Nodes.Count != 0)
		{
			_ⵝ.CollapseAll();
		}
	}

	public void ExpandAllGridItems()
	{
		if (_ⵝ != null && _ⵝ.Nodes.Count != 0)
		{
			_ⵝ.ExpandAll();
		}
	}

	private void ⶢ(bool ⶣ)
	{
		if (__2054)
		{
			__2D78 = true;
			return;
		}
		__2D78 = false;
		string text = null;
		if (!ⶣ && _ⵝ.SelectedNode is PropertyNode)
		{
			PropertyNode propertyNode = (PropertyNode)_ⵝ.SelectedNode;
			text = propertyNode.Text;
		}
		_2D9A();
		((Control)_ⵟ).Text = "";
		if (__2D76 != null && __2D76.Length > 0)
		{
			_2E8F _2E8F2 = null;
			_2E8F2 = ((__2D76.Length != 1) ? _2D9D(__2D76) : _2D9D(__2D76[0]));
			_ⵝ.BeginUpdate();
			try
			{
				_2E8F2.ሕ(_ⵝ.Nodes, __2D7D, _ⵠ);
				if (!string.IsNullOrEmpty(text))
				{
					Node node = _ⵝ.FindNodeByText(text);
					if (node != null)
					{
						_ⵝ.SelectedNode = node;
					}
				}
			}
			finally
			{
				_ⵝ.EndUpdate();
			}
			_ⵦ.Enabled = true;
		}
		else
		{
			_ⵦ.Enabled = false;
		}
		OnPropertiesLoaded(EventArgs.Empty);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		if (_ⵡ != null)
		{
			ⶕ(_ⵡ);
			_ⵡ.Font = new Font(((Control)this).Font, (FontStyle)1);
			ⶕ(_ⵣ);
			_ⵣ.Font = new Font(((Control)this).Font, (FontStyle)1);
		}
		((ContainerControl)this).OnFontChanged(e);
	}

	private void ⶤ(object _٠, AdvTreeNodeCancelEventArgs ؿ)
	{
		if (_ⵝ.SelectedNode is PropertyNode { IsEditing: not false } propertyNode && !propertyNode.ApplyEdit())
		{
			ؿ.Cancel = true;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeIgnoredProperties()
	{
		if (__2D7A != null)
		{
			return __2D7A.Length > 0;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetIgnoredProperties()
	{
		IgnoredProperties = new string[0];
		new List<string>();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeIgnoredCategories()
	{
		if (__2D7B != null)
		{
			return __2D7B.Length > 0;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetIgnoredCategories()
	{
		IgnoredCategories = new string[0];
		new List<string>();
	}

	internal void ⶥ(SuperTooltipInfo _108F, string फ़, Exception ⶦ, object ԝ)
	{
		OnPrepareErrorSuperTooltip(new PropertyErrorTooltipEventArgs(_108F, फ़, ⶦ, ԝ));
	}

	protected virtual void OnPrepareErrorSuperTooltip(PropertyErrorTooltipEventArgs e)
	{
		_2D6D?.Invoke(this, e);
	}

	private void _2DA7(object _٠, KeyPressEventArgs ؿ)
	{
		if ((char.IsLetterOrDigit(ؿ.KeyChar) || char.IsPunctuation(ؿ.KeyChar) || char.IsSeparator(ؿ.KeyChar)) && _ⵝ.SelectedNode is PropertyNode)
		{
			PropertyNode propertyNode = (PropertyNode)_ⵝ.SelectedNode;
			TextBoxDropDown textBoxDropDown = propertyNode._2E57();
			if (textBoxDropDown != null && !((Control)textBoxDropDown.TextBox).Focused && !((TextBoxBase)textBoxDropDown.TextBox).ReadOnly && ((Control)textBoxDropDown.TextBox).Focus())
			{
				((Control)textBoxDropDown).Text = ؿ.KeyChar.ToString();
				((TextBoxBase)textBoxDropDown.TextBox).SelectionStart = 1;
			}
		}
		else if (char.IsWhiteSpace(ؿ.KeyChar) && _ⵝ.SelectedNode is PropertyCheckBoxNode)
		{
			PropertyCheckBoxNode propertyCheckBoxNode = (PropertyCheckBoxNode)_ⵝ.SelectedNode;
			if (!propertyCheckBoxNode.IsReadOnly)
			{
				propertyCheckBoxNode.SelectNextValue();
			}
		}
	}

	public void SetPropertyColumnWidth(int columnIndex, int width)
	{
		_ⵝ.Columns[columnIndex].Width.Absolute = width;
		ⶨ();
	}

	private void ⶨ()
	{
		if (_ⵝ.Columns[0].Width.Absolute != 0 || _ⵝ.Columns[1].Width.Absolute != 0)
		{
			int num = 0;
			num = ((_ⵝ.Columns[0].Width.Absolute <= 0) ? _ⵝ.Columns[0].Width.Relative : Math.Min(95, Math.Max(5, (int)Math.Floor(100f * ((float)_ⵝ.Columns[0].Width.Absolute / (float)_ⵝ.Ԫ.ܒ.Width)))));
			int relative = 100 - num;
			_ⵝ.Columns[0].Width.Absolute = 0;
			_ⵝ.Columns[1].Width.Absolute = 0;
			_ⵝ.Columns[0].Width.Relative = num;
			_ⵝ.Columns[1].Width.Relative = relative;
		}
	}

	private void ⶩ(object _٠, EventArgs ؿ)
	{
		ⶨ();
	}

	private void ⶪ(object _٠, EventArgs ؿ)
	{
		MouseEventArgs val = (MouseEventArgs)(object)((ؿ is MouseEventArgs) ? ؿ : null);
		if (val != null && (_ⵝ.SelectedNode == null || !_ⵝ.SelectedNode.Bounds.Contains(val.X, val.Y)))
		{
			Node nodeAt = _ⵝ.GetNodeAt(val.Y);
			if (nodeAt is PropertyCategoryNode)
			{
				nodeAt.Toggle();
			}
		}
	}

	private void ⶫ(object _٠, PropertyChangedEventArgs ؿ)
	{
		if (ؿ.PropertyName == "DefaultPropertyStyle")
		{
			if (_ᗹ.DefaultPropertyStyle != null)
			{
				_ⵝ.NodeStyle = _ᗹ.DefaultPropertyStyle;
			}
			else
			{
				_ⵝ.NodeStyle = _ⵝ.Styles["DefaultNodeStyle"];
			}
		}
	}

	protected virtual void OnConvertPropertyValueToString(ConvertValueEventArgs e)
	{
		_2D6E?.Invoke(this, e);
	}

	internal void ⶬ(ConvertValueEventArgs ؿ)
	{
		OnConvertPropertyValueToString(ؿ);
	}

	protected virtual void OnConvertFromStringToPropertyValue(ConvertValueEventArgs e)
	{
		ⵯ?.Invoke(this, e);
	}

	internal void ⶭ(ConvertValueEventArgs ؿ)
	{
		OnConvertFromStringToPropertyValue(ؿ);
	}

	protected virtual void OnProvidePropertyValueList(PropertyValueListEventArgs e)
	{
		_2D70?.Invoke(this, e);
	}

	internal void ⶮ(PropertyValueListEventArgs ؿ)
	{
		OnProvidePropertyValueList(ؿ);
	}

	protected virtual void OnProvideUITypeEditor(ProvideUITypeEditorEventArgs e)
	{
		_2D71?.Invoke(this, e);
	}

	internal void _2DAF(ProvideUITypeEditorEventArgs ؿ)
	{
		OnProvideUITypeEditor(ؿ);
	}

	internal void ⶰ(PropertySettings _06EC)
	{
		_06EC.PropertyChanged += ⶱ;
	}

	private void ⶱ(object _٠, PropertyChangedEventArgs ؿ)
	{
	}

	internal void ⶲ(PropertySettings _06EC)
	{
		_06EC.PropertyChanged -= ⶱ;
	}

	private void ⶳ(ePropertySort ك, ePropertySort ل)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("PropertySort"));
		switch (ل)
		{
		case ePropertySort.Alphabetical:
			__2D68.Checked = true;
			break;
		default:
			_ⵧ.Checked = true;
			break;
		case ePropertySort.NoSort:
			__2D68.Checked = false;
			_ⵧ.Checked = false;
			break;
		}
		ⶢ(ⶣ: false);
	}

	private void ⶴ(ePropertyGridHelpType ك, ePropertyGridHelpType ل)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("HelpType"));
		if (ل != ePropertyGridHelpType.Panel)
		{
			((Control)_ⵞ).Visible = false;
			((Control)_ⵟ).Visible = false;
		}
		else
		{
			((Control)_ⵞ).Visible = true;
			((Control)_ⵟ).Visible = true;
		}
		if (ل == ePropertyGridHelpType.SuperTooltip || ك == ePropertyGridHelpType.SuperTooltip)
		{
			ⶢ(ⶣ: false);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeGridLinesColor()
	{
		return _ⵝ.ShouldSerializeGridLinesColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetGridLinesColor()
	{
		_ⵝ.ResetGridLinesColor();
	}

	private void ⶵ(bool ك, bool ل)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("ToolbarVisible"));
		if (_ⵜ != null)
		{
			_ⵜ.Visible = ل;
		}
	}

	private void ⲕ(bool ك, bool ل)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("SearchBoxVisible"));
		_ⵦ.Visible = _ⱻ;
	}

	public PropertyNode GetPropertyNode(string propertyName)
	{
		return ⶶ(_ⵝ, propertyName, null, ទ: true) as PropertyNode;
	}

	private static Node ⶶ(DevComponents.AdvTree.AdvTree ង, string _06EA, Node ថ, bool ទ)
	{
		ថ = ((ថ != null) ? _175C.ᝪ(ថ) : _175C.ᝤ(ង));
		if (ថ == null)
		{
			return null;
		}
		Node node = ថ;
		StringComparison comparisonType = (ទ ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
		while (node != null)
		{
			if (node is PropertyNode && ((PropertyNode)node)._2E34.StartsWith(_06EA, comparisonType))
			{
				return node;
			}
			if (node.Text.StartsWith(_06EA, comparisonType))
			{
				return node;
			}
			node = _175C.ᝪ(node);
		}
		return null;
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		if (_ⶀ)
		{
			Keys val = (Keys)(keyData & 0xFFFF);
			if ((int)val == 9 || ((int)val == 9 && (keyData & 0x10000) == 65536))
			{
				Node node = null;
				node = (((keyData & 0x10000) != 65536) ? _2DB7() : ⶸ());
				if (node != null)
				{
					bool flag = true;
					if (_ⵝ.SelectedNode is PropertyNode)
					{
						PropertyNode propertyNode = (PropertyNode)_ⵝ.SelectedNode;
						if (propertyNode.IsEditing && !propertyNode.ApplyEdit())
						{
							flag = false;
						}
					}
					if (flag)
					{
						_ⵝ.SelectNode(node, eTreeAction.Keyboard);
					}
					return true;
				}
			}
		}
		return ((ContainerControl)this).ProcessCmdKey(ref msg, keyData);
	}

	private Node _2DB7()
	{
		if (_ⵝ.Nodes.Count == 0)
		{
			return null;
		}
		Node node = _ⵝ.SelectedNode;
		if (node == null)
		{
			node = _ⵝ.Nodes[0];
			if (node is PropertyNode && !((PropertyNode)node).IsReadOnly)
			{
				return node;
			}
		}
		while (node != null)
		{
			node = _175C.ᝩ(node);
			if (node is PropertyNode && !((PropertyNode)node).IsReadOnly)
			{
				return node;
			}
		}
		return null;
	}

	private Node ⶸ()
	{
		if (_ⵝ.Nodes.Count == 0)
		{
			return null;
		}
		Node node = _ⵝ.SelectedNode;
		if (node == null)
		{
			node = _ⵝ.Nodes[_ⵝ.Nodes.Count - 1];
			if (node is PropertyNode && !((PropertyNode)node).IsReadOnly)
			{
				return node;
			}
		}
		while (node != null)
		{
			node = _175C.ᝯ(node);
			if (node is PropertyNode && !((PropertyNode)node).IsReadOnly)
			{
				return node;
			}
		}
		return null;
	}

	protected virtual void OnSubPropertiesDefaultSortChanged(ePropertySort oldValue, ePropertySort newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("SubPropertiesDefaultSort"));
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		চ?.Invoke(this, e);
	}

	private string ⶹ(string फ़)
	{
		LocalizeEventHandler localizeEventHandler = _2D6C;
		if (localizeEventHandler != null)
		{
			AdvPropertyGridLocalizeEventArgs advPropertyGridLocalizeEventArgs = new AdvPropertyGridLocalizeEventArgs(फ़, ePropertyGridLocalizationType.PropertyName);
			localizeEventHandler(this, advPropertyGridLocalizeEventArgs);
			return advPropertyGridLocalizeEventArgs.LocalizedValue;
		}
		return null;
	}

	string IPropertyGridLocalizer.GetPropertyName(string फ़)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ⶹ
		return this.ⶹ(फ़);
	}

	private string ⶺ(string ⶻ)
	{
		LocalizeEventHandler localizeEventHandler = _2D6C;
		if (localizeEventHandler != null)
		{
			AdvPropertyGridLocalizeEventArgs advPropertyGridLocalizeEventArgs = new AdvPropertyGridLocalizeEventArgs(ⶻ, ePropertyGridLocalizationType.Category);
			localizeEventHandler(this, advPropertyGridLocalizeEventArgs);
			return advPropertyGridLocalizeEventArgs.LocalizedValue;
		}
		return null;
	}

	string IPropertyGridLocalizer.GetCategoryName(string ⶻ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ⶺ
		return this.ⶺ(ⶻ);
	}

	private string ⶼ(string ⶽ)
	{
		LocalizeEventHandler localizeEventHandler = _2D6C;
		if (localizeEventHandler != null)
		{
			AdvPropertyGridLocalizeEventArgs advPropertyGridLocalizeEventArgs = new AdvPropertyGridLocalizeEventArgs("ErrorBody", ePropertyGridLocalizationType.ErrorTooltip);
			advPropertyGridLocalizeEventArgs.LocalizedValue = ⶽ;
			localizeEventHandler(this, advPropertyGridLocalizeEventArgs);
			return advPropertyGridLocalizeEventArgs.LocalizedValue;
		}
		return ⶽ;
	}

	string IPropertyGridLocalizer.GetErrorTooltipMessage(string ⶽ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ⶼ
		return this.ⶼ(ⶽ);
	}

	public void BeginInit()
	{
		_ⵝ.BeginUpdate();
		__2054 = true;
	}

	public void EndInit()
	{
		_ⵝ.EndUpdate();
		__2054 = false;
		if (__2D78)
		{
			ⶢ(ⶣ: false);
		}
	}
}
