using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.AdvTree;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar;

public class PropertyNode : Node, ITypeDescriptorContext, IWindowsFormsEditorService, IServiceProvider
{
	public delegate void AfterEditorCreatedDelegate(object sender, object editor);

	private delegate void _2EBB(int alpha);

	private const string _2E3C = "UITypeEditorImage";

	private PropertyDescriptor __2E3D;

	private Color __2E3E = Color.Black;

	private TypeConverter __2E3F;

	private object __2E40;

	private bool __2E41;

	private EventHandler __2E42;

	private bool __2E43;

	private ElementStyle _ⵣ;

	private ElementStyle __2E44;

	private object __17C0;

	private string __2E45;

	private bool _ᣇ;

	private bool __2E46;

	public AfterEditorCreatedDelegate AfterEditorCreated;

	private Size __2E47 = Size.Empty;

	private int __2E48 = -1;

	private bool __2E49;

	private bool __2E4A;

	private Image __2E4B;

	private BackgroundWorker __2E4C;

	private ElementStyle __2E4D;

	private bool __237D;

	private bool _ᗞ;

	private PropertySettings __2D7C;

	private bool __2E4E;

	private bool __2E4F;

	private bool __2E50;

	private Color ⸯ => __2E3E;

	protected virtual bool IsTypeEditorPaintValueSupported => true;

	internal virtual TypeConverter _2E30
	{
		get
		{
			if (__2D7C != null && __2D7C.TypeConverter != null)
			{
				return __2D7C.TypeConverter;
			}
			if (__2E3F == null)
			{
				__2E3F = GetPropertyDescriptor().Converter;
			}
			return __2E3F;
		}
	}

	public virtual Type PropertyType
	{
		get
		{
			object propertyValue = PropertyValue;
			if (propertyValue != null)
			{
				return propertyValue.GetType();
			}
			return __2E3D.PropertyType;
		}
	}

	public virtual object PropertyValue
	{
		get
		{
			try
			{
				return GetPropertyValue(GetTargetComponent());
			}
			catch (Exception result)
			{
				return result;
			}
		}
		set
		{
			ApplyValue(value, null);
		}
	}

	internal object _2E31
	{
		get
		{
			return __2E40;
		}
		set
		{
			object oldValue = __2E40;
			__2E40 = value;
			OnTargetComponentChanged(oldValue, value);
		}
	}

	private EventHandler _2E32
	{
		get
		{
			if (__2E42 == null)
			{
				__2E42 = _2D72;
			}
			return __2E42;
		}
	}

	public PropertyDescriptor PropertyDescriptor
	{
		get
		{
			return __2E3D;
		}
		internal set
		{
			__2E3D = value;
			_2E55();
		}
	}

	private bool _2E33 => PropertyDescriptor?.IsReadOnly ?? false;

	public ElementStyle ValueChangedStyle
	{
		get
		{
			if (_ⵣ == null)
			{
				IPropertyElementStyleProvider styleProvider = StyleProvider;
				if (styleProvider != null)
				{
					return styleProvider.ValueChangedStyle;
				}
			}
			return _ⵣ;
		}
		set
		{
			_ⵣ = value;
		}
	}

	public ElementStyle EditCellStyle
	{
		get
		{
			if (__2E44 == null)
			{
				IPropertyElementStyleProvider styleProvider = StyleProvider;
				if (styleProvider != null)
				{
					return styleProvider.DefaultEditCellStyle;
				}
			}
			return __2E44;
		}
		set
		{
			if (__2E44 != value)
			{
				__2E44 = value;
				UpdateDisplayedValue();
			}
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return _ᣇ;
		}
		set
		{
			if (_ᣇ != value)
			{
				_ᣇ = value;
				if (_ᣇ)
				{
					base.Style = _2E5A();
				}
				else
				{
					base.Style = ᑡ();
				}
				OnIsReadOnlyChanged();
			}
		}
	}

	public IPropertyElementStyleProvider StyleProvider => AdvPropertyGrid;

	protected virtual Cell EditCell => base.Cells[1];

	public new bool IsEditing
	{
		get
		{
			return __2E46;
		}
		internal set
		{
			if (__2E46 != value)
			{
				__2E46 = value;
				AdvPropertyGrid?.ⶔ(this);
			}
		}
	}

	internal string _2E34 => GetPropertyName();

	protected virtual bool IsPropertyValueNullable => !__2E3D.PropertyType.IsValueType;

	public bool EditValueChanged
	{
		get
		{
			return __2E49;
		}
		set
		{
			__2E49 = value;
		}
	}

	private Color _2E35 => AdvPropertyGrid?.Appearance.ErrorHighlightColor ?? ColorScheme.GetColor(14259860);

	private Color _2E36 => AdvPropertyGrid?.Appearance.SuccessHighlightColor ?? ColorScheme.GetColor(10206041);

	internal bool ம
	{
		get
		{
			return __237D;
		}
		set
		{
			__237D = value;
		}
	}

	internal bool ᗘ => _ᗞ;

	protected virtual AdvPropertyGrid AdvPropertyGrid
	{
		get
		{
			DevComponents.AdvTree.AdvTree treeControl = base.TreeControl;
			if (treeControl == null)
			{
				return null;
			}
			for (Control parent = ((Control)treeControl).Parent; parent != null; parent = parent.Parent)
			{
				if (parent is AdvPropertyGrid)
				{
					return (AdvPropertyGrid)(object)parent;
				}
			}
			return null;
		}
	}

	public bool HasChildProperties
	{
		get
		{
			TypeConverter typeConverter = _2E30;
			if (typeConverter != null && typeConverter.GetPropertiesSupported())
			{
				PropertyDescriptorCollection propertyDescriptorCollection = _2E8F._2E9E(this, AttributeFilter, PropertyValue, typeConverter.GetPropertiesSupported(this) ? typeConverter : null);
				if (propertyDescriptorCollection != null && propertyDescriptorCollection.Count > 0)
				{
					return true;
				}
			}
			return false;
		}
	}

	protected virtual Attribute[] AttributeFilter
	{
		get
		{
			AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
			if (advPropertyGrid != null)
			{
				Attribute[] array = new Attribute[advPropertyGrid.BrowsableAttributes.Count];
				advPropertyGrid.BrowsableAttributes.CopyTo(array, 0);
				return array;
			}
			return null;
		}
	}

	internal _2EA5 _2E37
	{
		get
		{
			AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
			if (advPropertyGrid != null)
			{
				return advPropertyGrid.ⶠ();
			}
			return new _2EA5(null);
		}
	}

	public IPropertyGridLocalizer PropertyGridLocalizer => AdvPropertyGrid;

	public PropertySettingsCollection PropertySettingsCollection => AdvPropertyGrid?.PropertySettings;

	public PropertySettings PropertySettings
	{
		get
		{
			return __2D7C;
		}
		set
		{
			if (__2D7C != value)
			{
				if (__2D7C != null)
				{
					__2D7C.PropertyChanged -= _2E77;
				}
				__2D7C = value;
				if (__2D7C != null)
				{
					__2D7C.PropertyChanged += _2E77;
				}
				_2E55();
			}
		}
	}

	private bool _2E38
	{
		get
		{
			if (__2E3D == null)
			{
				return false;
			}
			if ((object)__2E3D.PropertyType.GetInterface("IList") != null && !__2E3D.PropertyType.IsArray && __2E3D.IsReadOnly)
			{
				return true;
			}
			return false;
		}
	}

	public List<string> IgnoredCategories
	{
		get
		{
			List<string> list = new List<string>();
			AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
			if (advPropertyGrid != null)
			{
				list.AddRange(advPropertyGrid.IgnoredCategories);
			}
			return list;
		}
	}

	public List<string> IgnoredProperties
	{
		get
		{
			List<string> list = new List<string>();
			AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
			if (advPropertyGrid != null)
			{
				list.AddRange(advPropertyGrid.IgnoredProperties);
			}
			return list;
		}
	}

	public bool IsMultiObject
	{
		get
		{
			return __2E4E;
		}
		internal set
		{
			__2E4E = value;
		}
	}

	public bool IsPassword
	{
		get
		{
			return __2E4F;
		}
		set
		{
			__2E4F = value;
		}
	}

	public virtual IComponent Component
	{
		get
		{
			object targetComponent = GetTargetComponent();
			if (targetComponent is IComponent)
			{
				return (IComponent)targetComponent;
			}
			return null;
		}
	}

	IContainer ITypeDescriptorContext._2E39
	{
		get
		{
			IComponent component = Component;
			if (component != null)
			{
				ISite site = component.Site;
				if (site != null)
				{
					return site.Container;
				}
			}
			return null;
		}
	}

	object ITypeDescriptorContext._2E3A => GetTargetComponent();

	PropertyDescriptor ITypeDescriptorContext._2E3B => GetPropertyDescriptor();

	public PropertyNode(PropertyDescriptor property)
	{
		__2E3D = property;
		CellNavigationEnabled = false;
		if (__2E3D.Attributes[typeof(PasswordPropertyTextAttribute)] is PasswordPropertyTextAttribute { Password: not false })
		{
			IsPassword = true;
		}
	}

	public virtual void UpdateDisplayedValue()
	{
		UpdateDisplayedValue(refreshSubProperties: true);
	}

	public virtual void UpdateDisplayedValue(bool refreshSubProperties)
	{
		if (!_ᗞ && !__237D)
		{
			object propertyValue = PropertyValue;
			UpdateDisplayedValue(propertyValue, refreshSubProperties);
		}
	}

	protected virtual void UpdateDisplayedValue(object propertyValue)
	{
		UpdateDisplayedValue(propertyValue, refreshSubProperties: true);
	}

	protected virtual void UpdateDisplayedValue(object propertyValue, bool refreshSubProperties)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		if (_ᗞ || __237D)
		{
			return;
		}
		Cell editCell = EditCell;
		editCell.Text = GetPropertyTextValue(propertyValue);
		if (_2E52(propertyValue))
		{
			editCell.StyleNormal = EditCellStyle;
		}
		else
		{
			editCell.StyleNormal = ValueChangedStyle;
		}
		RevertEditorValue();
		if (IsTypeEditorPaintValueSupported)
		{
			UITypeEditor val = _2E5F();
			if (val != null && val.GetPaintValueSupported((ITypeDescriptorContext)this))
			{
				Rectangle rectangle = new Rectangle(0, 0, 20, 13);
				Bitmap val2 = new Bitmap(rectangle.Width, rectangle.Height, (PixelFormat)2498570);
				((Image)val2).Tag = "UITypeEditorImage";
				Graphics val3 = Graphics.FromImage((Image)(object)val2);
				try
				{
					bool flag = false;
					try
					{
						val.PaintValue(new PaintValueEventArgs((ITypeDescriptorContext)this, propertyValue, val3, rectangle));
					}
					catch
					{
						flag = true;
					}
					if (flag)
					{
						DisplayHelp.FillRectangle(val3, rectangle, Color.White);
					}
					DisplayHelp.DrawRectangle(val3, ⸯ, rectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				editCell.Images.Image = (Image)(object)val2;
			}
			else if (editCell.Images.Image != null && editCell.Images.Image.Tag is string && (string)editCell.Images.Image.Tag == "UITypeEditorImage")
			{
				Image image = editCell.Images.Image;
				editCell.Images.Image = null;
				image.Dispose();
			}
		}
		if (refreshSubProperties)
		{
			_2E75();
		}
	}

	private string _2E51()
	{
		return GetPropertyTextValue(PropertyValue);
	}

	protected string GetPropertyTextValue(object propertyValue)
	{
		if (__2D7C != null && __2D7C.HasConvertPropertyValueToStringHandler)
		{
			ConvertValueEventArgs convertValueEventArgs = new ConvertValueEventArgs(null, propertyValue, GetPropertyName(), GetTargetComponent(), PropertyDescriptor);
			__2D7C.InvokeConvertPropertyValueToString(convertValueEventArgs);
			if (convertValueEventArgs.IsConverted && convertValueEventArgs.StringValue != null)
			{
				return convertValueEventArgs.StringValue;
			}
		}
		AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
		if (advPropertyGrid != null && advPropertyGrid.ⵓ)
		{
			ConvertValueEventArgs convertValueEventArgs2 = new ConvertValueEventArgs(null, propertyValue, GetPropertyName(), GetTargetComponent(), PropertyDescriptor);
			advPropertyGrid.ⶬ(convertValueEventArgs2);
			if (convertValueEventArgs2.IsConverted && convertValueEventArgs2.StringValue != null)
			{
				return convertValueEventArgs2.StringValue;
			}
		}
		string text = null;
		TypeConverter typeConverter = _2E30;
		try
		{
			text = typeConverter.ConvertToString(this, propertyValue);
		}
		catch (Exception)
		{
		}
		if (text == null)
		{
			text = string.Empty;
		}
		if (IsPassword)
		{
			text = _26E6._26EA(AdvPropertyGrid.PasswordChar, text.Length);
		}
		return text;
	}

	protected virtual bool GetIsDefaultValue()
	{
		return _2E52(PropertyValue);
	}

	private bool _2E52(object ᬍ)
	{
		if (GetPropertyDescriptor().Attributes[typeof(DefaultValueAttribute)] is DefaultValueAttribute defaultValueAttribute)
		{
			if (defaultValueAttribute.Value != null)
			{
				return defaultValueAttribute.Value.Equals(ᬍ);
			}
			return defaultValueAttribute.Value == ᬍ;
		}
		if (ᬍ != null)
		{
			string name = "ShouldSerialize" + GetPropertyName();
			PropertyDescriptor propertyDescriptor = GetPropertyDescriptor();
			if (propertyDescriptor != null && (object)propertyDescriptor.ComponentType != null && (object)propertyDescriptor.ComponentType.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null)
			{
				MethodInfo method = propertyDescriptor.ComponentType.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				object targetComponent = GetTargetComponent();
				if (method.DeclaringType.IsInstanceOfType(targetComponent))
				{
					object obj = method.Invoke(GetTargetComponent(), null);
					if (obj is bool && (bool)obj)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	protected virtual object GetTargetComponent()
	{
		if (__2E40 != null)
		{
			return __2E40;
		}
		for (Node parent = base.Parent; parent != null; parent = parent.Parent)
		{
			if (parent is PropertyNode)
			{
				return ((PropertyNode)parent).GetTargetComponent();
			}
		}
		return __2E40;
	}

	protected virtual void OnTargetComponentChanged(object oldValue, object newValue)
	{
		string text = GetPropertyName() + "Changed";
		if (oldValue != null && __2E41)
		{
			try
			{
				if (oldValue is ICustomTypeDescriptor)
				{
					_2E53(oldValue, text)?.RemoveEventHandler(oldValue, _2E32);
				}
				else
				{
					oldValue.GetType().GetEvent(text)?.RemoveEventHandler(oldValue, _2E32);
				}
			}
			finally
			{
				__2E41 = false;
			}
		}
		if (newValue == null || GetPropertyDescriptor() == null)
		{
			return;
		}
		if (newValue is ICustomTypeDescriptor)
		{
			EventDescriptor eventDescriptor = _2E53(newValue, text);
			if (eventDescriptor != null)
			{
				try
				{
					eventDescriptor.AddEventHandler(newValue, _2E32);
					__2E41 = true;
					return;
				}
				catch
				{
					__2E41 = false;
					return;
				}
			}
			return;
		}
		EventInfo @event = newValue.GetType().GetEvent(text);
		if ((object)@event != null && (object)@event.EventHandlerType == typeof(EventHandler))
		{
			try
			{
				@event.AddEventHandler(newValue, _2E32);
				__2E41 = true;
			}
			catch
			{
				__2E41 = false;
			}
		}
	}

	private EventDescriptor _2E53(object ԝ, string _2E54)
	{
		EventDescriptorCollection events = ((ICustomTypeDescriptor)ԝ).GetEvents();
		foreach (EventDescriptor item in events)
		{
			if (item.Name == _2E54)
			{
				if ((object)item.EventType == typeof(EventHandler))
				{
					return item;
				}
				break;
			}
		}
		return null;
	}

	private void _2D72(object _٠, EventArgs ؿ)
	{
		UpdateDisplayedValue();
	}

	protected virtual object GetPropertyValue(object target)
	{
		PropertyDescriptor propertyDescriptor = GetPropertyDescriptor();
		if (propertyDescriptor == null)
		{
			return null;
		}
		if (__2E4E)
		{
			object[] array = (object[])target;
			object obj = GetPropertyValueCore(array[0], propertyDescriptor);
			_ = _2E30;
			for (int i = 1; i < array.Length; i++)
			{
				object obj2 = null;
				if (propertyDescriptor.ComponentType.IsAssignableFrom(array[i].GetType()))
				{
					obj2 = GetPropertyValueCore(array[i], propertyDescriptor);
				}
				else
				{
					PropertyDescriptor propertyDescriptor2 = _2E8F._2EA2(propertyDescriptor.Name, array[i], null);
					obj2 = GetPropertyValueCore(array[i], propertyDescriptor2);
				}
				if ((obj != null && !obj.Equals(obj2)) || (obj == null && obj2 != null))
				{
					obj = null;
					break;
				}
			}
			return obj;
		}
		return GetPropertyValueCore(target, propertyDescriptor);
	}

	protected virtual object GetPropertyValueCore(object target, PropertyDescriptor propertyDescriptor)
	{
		if (target is ICustomTypeDescriptor)
		{
			target = ((ICustomTypeDescriptor)target).GetPropertyOwner(propertyDescriptor);
		}
		try
		{
			return propertyDescriptor.GetValue(target);
		}
		catch
		{
			throw;
		}
	}

	protected override void OnParentChanged()
	{
		_2E55();
		base.OnParentChanged();
	}

	public virtual void ReleaseInlineEditor()
	{
		_2E55();
	}

	private void _2E55()
	{
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		if (__2E43)
		{
			Cell editCell = EditCell;
			Control hostedControl = editCell.HostedControl;
			if (hostedControl != null)
			{
				editCell.HostedControl = null;
				if (hostedControl is IPropertyValueEditor)
				{
					((IPropertyValueEditor)hostedControl).EditValueChanged -= _2E5B;
				}
				((Component)(object)hostedControl).Dispose();
			}
			else if (editCell.HostedItem != null)
			{
				BaseItem hostedItem = editCell.HostedItem;
				editCell.HostedItem = null;
				if (hostedItem is IPropertyValueEditor)
				{
					((IPropertyValueEditor)hostedItem).EditValueChanged -= _2E5B;
				}
				hostedItem.Dispose();
			}
		}
		__2E43 = false;
		if (__2E3D == null || ம)
		{
			return;
		}
		PropertyValueEditor propertyValueEditor = ((__2D7C == null || __2D7C.ValueEditor == null) ? (__2E3D.Attributes[typeof(PropertyValueEditor)] as PropertyValueEditor) : __2D7C.ValueEditor);
		if (propertyValueEditor == null)
		{
			return;
		}
		IPropertyValueEditor propertyValueEditor2 = propertyValueEditor.CreateEditor(__2E3D, __2E40);
		propertyValueEditor2.EditValueChanged += _2E5B;
		__17C0 = propertyValueEditor2;
		if (propertyValueEditor2 is Control)
		{
			EditCell.HostedControl = (Control)__17C0;
			if (IsReadOnly || _2E33)
			{
				EditCell.HostedControl.Enabled = false;
			}
		}
		else
		{
			if (!(propertyValueEditor2 is BaseItem))
			{
				throw new ArgumentException("PropertyValueEditor must inherit from Control or BaseItem.");
			}
			EditCell.HostedItem = (BaseItem)__17C0;
			if (IsReadOnly || _2E33)
			{
				EditCell.Enabled = false;
			}
		}
		__2E43 = true;
	}

	protected override void OnSelected(eTreeAction action)
	{
		if (_ᗞ || __237D)
		{
			return;
		}
		UpdateDisplayedValue();
		DevComponents.AdvTree.AdvTree treeControl = base.TreeControl;
		bool flag = false;
		Point pt = Point.Empty;
		if (treeControl != null)
		{
			pt = ((Control)treeControl).PointToClient(Control.MousePosition);
			if (EditCell.Bounds.Contains(pt))
			{
				flag = true;
			}
		}
		if (action == eTreeAction.Keyboard && _0EFE._0F0D(_0EFE.GetKeyState(9)) != 0)
		{
			flag = true;
		}
		if (base.Enabled && !IsReadOnly && _2E59())
		{
			EnterEditorMode(action, flag);
			if (flag && !pt.IsEmpty && __17C0 is TextBoxDropDown)
			{
				((TextBoxBase)((TextBoxDropDown)__17C0).TextBox).SelectAll();
			}
		}
		AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
		if (advPropertyGrid != null)
		{
			string _06EA = _2E56();
			if (!string.IsNullOrEmpty(_06EA))
			{
				bool flag2 = false;
				if (!ሒ.መ(ref _06EA))
				{
					flag2 = true;
				}
				_06EA = _06EA.Replace("<", "&lt;");
				_06EA = _06EA.Replace(">", "&gt;");
				_06EA = "<b>" + base.Text + "</b><br/>" + _06EA;
				if (flag2)
				{
					_06EA = _06EA.Replace("\n", "<br/>");
				}
			}
			((Control)advPropertyGrid.HelpPanel).Text = _06EA;
		}
		base.OnSelected(action);
	}

	private string _2E56()
	{
		if (__2D7C != null && __2D7C.Description != null)
		{
			return __2D7C.Description;
		}
		if (__2E45 == null)
		{
			__2E45 = __2E3D.Description;
		}
		return __2E45;
	}

	internal TextBoxDropDown _2E57()
	{
		return __17C0 as TextBoxDropDown;
	}

	private bool _2E58()
	{
		if (__2E3D != null && !__2E3D.PropertyType.IsPrimitive && !__2E3D.PropertyType.IsEnum && (object)__2E3D.PropertyType != typeof(string))
		{
			UITypeEditor val = _2E5F();
			if (val != null)
			{
				return true;
			}
		}
		return false;
	}

	private bool _2E59()
	{
		if (__2E3D != null && !__2E3D.PropertyType.IsPrimitive && !__2E3D.PropertyType.IsEnum && (object)__2E3D.PropertyType != typeof(string))
		{
			UITypeEditor val = _2E5F();
			if (val != null)
			{
				return true;
			}
			if (_2E30 != null && _2E30.CanConvertFrom(typeof(string)))
			{
				return true;
			}
			return false;
		}
		return true;
	}

	protected virtual void OnIsReadOnlyChanged()
	{
		Cell editCell = EditCell;
		if (editCell != null)
		{
			if (editCell.HostedControl is IPropertyValueEditor)
			{
				editCell.HostedControl.Enabled = !_ᣇ;
			}
			if (editCell.HostedItem is IPropertyValueEditor)
			{
				editCell.HostedItem.Enabled = !_ᣇ;
			}
		}
	}

	private ElementStyle _2E5A()
	{
		if (__2D7C != null && __2D7C.ReadOnlyStyle != null)
		{
			return __2D7C.ReadOnlyStyle;
		}
		return StyleProvider?.ReadOnlyStyle;
	}

	private ElementStyle ᑡ()
	{
		if (__2D7C != null && __2D7C.Style != null)
		{
			return __2D7C.Style;
		}
		return null;
	}

	protected override bool CanKeyboardNavigate(KeyEventArgs e)
	{
		if (IsEditing)
		{
			if (IsEditing)
			{
				if (!(__17C0 is TextBoxDropDown) || ((Control)((TextBoxDropDown)__17C0).TextBox).Focused)
				{
					if (__17C0 is IPropertyValueEditor)
					{
						return ((IPropertyValueEditor)__17C0).IsEditorFocused;
					}
					return false;
				}
				return true;
			}
			return false;
		}
		return true;
	}

	public virtual void EnterEditorMode(eTreeAction action, bool focusEditor)
	{
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		if (IsEditing)
		{
			throw new InvalidOperationException("Node is already in edit mode.");
		}
		if (!base.Enabled)
		{
			throw new InvalidOperationException("Node is disabled. Cannot enter edit mode.");
		}
		if (__2E43)
		{
			if (__17C0 is IPropertyValueEditor)
			{
				((IPropertyValueEditor)__17C0).EditValue = PropertyValue;
			}
			IsEditing = true;
			if (focusEditor && __17C0 is IPropertyValueEditor)
			{
				((IPropertyValueEditor)__17C0).FocusEditor();
			}
			EditValueChanged = false;
		}
		else
		{
			if (__17C0 != null)
			{
				return;
			}
			__17C0 = CreateNodeEditor();
			TextBoxDropDown textBoxDropDown = __17C0 as TextBoxDropDown;
			if (textBoxDropDown != null)
			{
				if (IsPassword)
				{
					textBoxDropDown.PasswordChar = AdvPropertyGrid.PasswordChar;
					((Control)textBoxDropDown).Text = _2E51();
				}
				else
				{
					textBoxDropDown.PasswordChar = '\0';
					((Control)textBoxDropDown).Text = EditCell.Text;
				}
			}
			else if (__17C0 is IPropertyValueEditor)
			{
				((IPropertyValueEditor)__17C0).EditValue = PropertyValue;
				if (!IsReadOnly)
				{
					((Control)__17C0).Enabled = false;
				}
			}
			ElementStyle valueChangedStyle = ValueChangedStyle;
			if (EditCell.StyleNormal == valueChangedStyle && valueChangedStyle.Font != null)
			{
				if (textBoxDropDown != null)
				{
					((Control)textBoxDropDown.TextBox).Font = valueChangedStyle.Font;
				}
				else if (__17C0 is IPropertyValueEditor)
				{
					((IPropertyValueEditor)__17C0).EditorFont = valueChangedStyle.Font;
				}
			}
			EditCell.HostedControl = (Control)__17C0;
			IsEditing = true;
			if (focusEditor)
			{
				if (textBoxDropDown != null)
				{
					((Control)textBoxDropDown).Focus();
				}
				else if (__17C0 is IPropertyValueEditor)
				{
					((IPropertyValueEditor)__17C0).FocusEditor();
				}
			}
			EditValueChanged = false;
		}
	}

	protected virtual Control CreateNodeEditor()
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Invalid comparison between Unknown and I4
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Invalid comparison between Unknown and I4
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Invalid comparison between Unknown and I4
		bool flag = _2E5E();
		TextBoxDropDown textBoxDropDown = new TextBoxDropDown();
		if (BarUtilities.UseTextRenderer)
		{
			textBoxDropDown.BackgroundStyle.MarginLeft = 3;
		}
		AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
		if (advPropertyGrid != null)
		{
			((Control)textBoxDropDown).Font = ((Control)advPropertyGrid).Font;
		}
		((Control)textBoxDropDown).Size = new Size(32, textBoxDropDown.PreferredHeight);
		((Control)textBoxDropDown).Visible = true;
		((Control)textBoxDropDown.TextBox).KeyDown += new KeyEventHandler(EditorKeyDown);
		((Control)textBoxDropDown.TextBox).TextChanged += _2E62;
		((TextBoxBase)textBoxDropDown.TextBox).ReadOnly = flag;
		textBoxDropDown.TextBox.PreventEnterBeep = true;
		UITypeEditor val = _2E5F();
		UITypeEditorEditStyle val2 = (UITypeEditorEditStyle)1;
		if (val != null)
		{
			val2 = val.GetEditStyle((ITypeDescriptorContext)this);
			if ((int)val2 == 2)
			{
				textBoxDropDown.ButtonCustom.Visible = true;
				textBoxDropDown.ButtonCustomClick += _2E60;
			}
			else if ((int)val2 == 3)
			{
				textBoxDropDown.ButtonDropDown.Visible = true;
				textBoxDropDown.ButtonDropDownClick += _2E5D;
			}
			if (!IsReadOnly)
			{
				PropertyDescriptor propertyDescriptor = GetPropertyDescriptor();
				if (propertyDescriptor != null && !propertyDescriptor.PropertyType.IsAssignableFrom(typeof(string)) && propertyDescriptor.Converter != null && !propertyDescriptor.Converter.CanConvertFrom(typeof(string)))
				{
					((TextBoxBase)textBoxDropDown.TextBox).ReadOnly = true;
				}
			}
		}
		if ((val == null || (int)val2 == 1) && !flag)
		{
			SetupEditorAutoCompleteList(textBoxDropDown);
			if (!SetupEditorPopup(textBoxDropDown))
			{
				textBoxDropDown.ButtonDropDown.Visible = false;
			}
			else
			{
				textBoxDropDown.ButtonDropDown.Visible = true;
			}
		}
		if (AfterEditorCreated != null)
		{
			AfterEditorCreated(this, textBoxDropDown);
		}
		return (Control)(object)textBoxDropDown;
	}

	private void _2E5B(object _٠, EventArgs ؿ)
	{
		EditValueChanged = true;
		if (IsEditing)
		{
			ApplyEdit();
		}
	}

	private bool _2E5C()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		UITypeEditor val = _2E5F();
		if (val != null && (int)val.GetEditStyle((ITypeDescriptorContext)this) == 3)
		{
			object value = val.EditValue((ITypeDescriptorContext)this, (IServiceProvider)this, PropertyValue);
			ApplyValue(value, null);
			return true;
		}
		return false;
	}

	private void _2E5D(object _٠, CancelEventArgs ؿ)
	{
		_2E5C();
		ؿ.Cancel = true;
	}

	private bool _2E5E()
	{
		return IsReadOnly;
	}

	private UITypeEditor _2E5F()
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		PropertyDescriptor propertyDescriptor = GetPropertyDescriptor();
		if (__2D7C != null && __2D7C.UITypeEditor != null)
		{
			return __2D7C.UITypeEditor;
		}
		AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
		if (advPropertyGrid != null && advPropertyGrid.ⵖ)
		{
			ProvideUITypeEditorEventArgs provideUITypeEditorEventArgs = new ProvideUITypeEditorEventArgs(GetPropertyName(), propertyDescriptor);
			advPropertyGrid._2DAF(provideUITypeEditorEventArgs);
			if (provideUITypeEditorEventArgs.EditorSpecified)
			{
				return provideUITypeEditorEventArgs.UITypeEditor;
			}
		}
		object propertyValue = PropertyValue;
		if (propertyValue is ICustomTypeDescriptor)
		{
			ICustomTypeDescriptor customTypeDescriptor = (ICustomTypeDescriptor)propertyValue;
			UITypeEditor val = (UITypeEditor)customTypeDescriptor.GetEditor(typeof(UITypeEditor));
			if (val != null)
			{
				return val;
			}
		}
		return (UITypeEditor)propertyDescriptor.GetEditor(typeof(UITypeEditor));
	}

	protected virtual string GetPropertyName()
	{
		return GetPropertyDescriptor().Name;
	}

	protected virtual PropertyDescriptor GetPropertyDescriptor()
	{
		return __2E3D;
	}

	private void _2E60(object _٠, EventArgs ؿ)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		BarUtilities.InvokeDelayed((MethodInvoker)delegate
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			UITypeEditor val = _2E5F();
			if (val != null)
			{
				object obj = null;
				try
				{
					obj = val.EditValue((ITypeDescriptorContext)this, (IServiceProvider)this, PropertyValue);
				}
				catch (Exception ex)
				{
					MessageBoxEx.Show(ex.Message);
					return;
				}
				ApplyValue(obj, null);
			}
		});
	}

	protected virtual bool SetupEditorPopup(TextBoxDropDown editor)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		if ((object)PropertyType == null)
		{
			return false;
		}
		string[] items = GetPopupListContent();
		if (items == null || items.Length == 0)
		{
			return false;
		}
		ListBox val = new ListBox();
		val.SelectedIndexChanged += _2E61;
		val.BorderStyle = (BorderStyle)0;
		val.Items.AddRange((object[])items);
		if (!string.IsNullOrEmpty(((Control)editor).Text))
		{
			val.SelectedItem = ((Control)editor).Text;
		}
		editor.DropDownControl = (Control)(object)val;
		((Control)val).Size = GetPreferredListBoxSize(ref items);
		return true;
	}

	private void _2E61(object _٠, EventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		ListBox val = (ListBox)_٠;
		if (!((Control)val).Visible || val.SelectedItem == null)
		{
			return;
		}
		TextBoxDropDown textBoxDropDown = __17C0 as TextBoxDropDown;
		((Control)textBoxDropDown).Text = (string)val.SelectedItem;
		textBoxDropDown.CloseDropDown();
		EditValueChanged = true;
		if (ApplyEdit())
		{
			DevComponents.AdvTree.AdvTree treeControl = base.TreeControl;
			if (treeControl != null)
			{
				((Control)treeControl).Focus();
			}
			base.SelectedCell = base.Cells[0];
		}
	}

	protected virtual Size GetPreferredListBoxSize(ref string[] items)
	{
		if (items == null || items.Length == 0)
		{
			return new Size(96, 32);
		}
		DevComponents.AdvTree.AdvTree treeControl = base.TreeControl;
		if (treeControl == null)
		{
			return new Size(96, 32);
		}
		if (!__2E47.IsEmpty && __2E48 == items.Length)
		{
			return __2E47;
		}
		Size empty = Size.Empty;
		Graphics val = treeControl.CreateGraphics();
		try
		{
			Font font = ((Control)treeControl).Font;
			for (int i = 0; i < items.Length; i++)
			{
				Size size = TextDrawing.MeasureString(val, items[i], font);
				if (size.Width == 0)
				{
					size.Width = EditCell.Bounds.Width - 8;
				}
				if (size.Height == 0)
				{
					size.Height = font.Height;
				}
				empty.Width = Math.Max(size.Width, empty.Width);
				if (empty.Height < 320)
				{
					empty.Height += size.Height;
					continue;
				}
				if (empty.Width < 120)
				{
					empty.Width = 128;
				}
				break;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (empty.Width < EditCell.Bounds.Width - 8)
		{
			empty.Width = EditCell.Bounds.Width - 8;
		}
		__2E47 = empty;
		__2E48 = items.Length;
		return empty;
	}

	protected virtual string[] GetPopupListContent()
	{
		if (__2D7C != null && __2D7C.HasProvidePropertyValueListHandler)
		{
			PropertyValueListEventArgs propertyValueListEventArgs = new PropertyValueListEventArgs(GetPropertyName(), GetTargetComponent(), PropertyDescriptor);
			__2D7C.InvokeProvidePropertyValueList(propertyValueListEventArgs);
			if (propertyValueListEventArgs.IsListValid)
			{
				if (propertyValueListEventArgs.ValueList == null)
				{
					return null;
				}
				return propertyValueListEventArgs.ValueList.ToArray();
			}
		}
		AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
		if (advPropertyGrid != null && advPropertyGrid.ⵕ)
		{
			PropertyValueListEventArgs propertyValueListEventArgs2 = new PropertyValueListEventArgs(GetPropertyName(), GetTargetComponent(), PropertyDescriptor);
			advPropertyGrid.ⶮ(propertyValueListEventArgs2);
			if (propertyValueListEventArgs2.IsListValid)
			{
				if (propertyValueListEventArgs2.ValueList == null)
				{
					return null;
				}
				return propertyValueListEventArgs2.ValueList.ToArray();
			}
		}
		TypeConverter typeConverter = _2E30;
		if (typeConverter != null && typeConverter.GetStandardValuesSupported(this))
		{
			TypeConverter.StandardValuesCollection standardValues = typeConverter.GetStandardValues(this);
			if (standardValues != null && standardValues.Count > 0)
			{
				string[] array = new string[standardValues.Count];
				for (int i = 0; i < standardValues.Count; i++)
				{
					object value = standardValues[i];
					array[i] = typeConverter.ConvertToString(value);
				}
				return array;
			}
		}
		if (PropertyType.IsEnum)
		{
			return Enum.GetNames(PropertyType);
		}
		if ((object)PropertyType == typeof(bool))
		{
			string[] array2 = new string[2];
			if (typeConverter != null && typeConverter.CanConvertTo(typeof(string)))
			{
				array2[0] = (string)typeConverter.ConvertTo(false, typeof(string));
				array2[1] = (string)typeConverter.ConvertTo(true, typeof(string));
			}
			else
			{
				array2[0] = bool.FalseString;
				array2[1] = bool.TrueString;
			}
			return array2;
		}
		return null;
	}

	protected virtual void SetupEditorAutoCompleteList(TextBoxDropDown editor)
	{
		if ((object)PropertyType == null)
		{
			return;
		}
		TextBox textBox = (TextBox)(object)editor.TextBox;
		TypeConverter typeConverter = _2E30;
		string[] popupListContent = GetPopupListContent();
		if (popupListContent != null && popupListContent.Length > 0)
		{
			textBox.AutoCompleteMode = (AutoCompleteMode)3;
			textBox.AutoCompleteSource = (AutoCompleteSource)64;
			textBox.AutoCompleteCustomSource.Clear();
			textBox.AutoCompleteCustomSource.AddRange(popupListContent);
			if (typeConverter != null && typeConverter.GetStandardValuesSupported() && typeConverter.GetStandardValuesExclusive())
			{
				((TextBoxBase)textBox).ReadOnly = true;
			}
			else
			{
				((TextBoxBase)textBox).ReadOnly = false;
			}
		}
	}

	protected override void OnNodeDoubleClick(EventArgs e)
	{
		base.OnNodeDoubleClick(e);
		if (!_2E5E() && !SelectNextValue())
		{
			if (__17C0 is TextBoxDropDown)
			{
				((TextBoxBase)((TextBoxDropDown)__17C0).TextBox).SelectAll();
				((Control)(TextBoxDropDown)__17C0).Focus();
			}
			else if (__17C0 is IPropertyValueEditor)
			{
				((IPropertyValueEditor)__17C0).FocusEditor();
			}
		}
	}

	public virtual bool SelectNextValue()
	{
		string[] popupListContent = GetPopupListContent();
		if (popupListContent == null || popupListContent.Length == 0)
		{
			return false;
		}
		List<string> list = new List<string>(popupListContent);
		string text = EditCell.Text;
		if (string.IsNullOrEmpty(text) || !list.Contains(text))
		{
			text = list[0];
		}
		else
		{
			int num = list.IndexOf(text) + 1;
			if (num >= list.Count)
			{
				num = 0;
			}
			text = list[num];
		}
		Exception exception = null;
		object valueFromString = GetValueFromString(text, out exception);
		if (exception != null)
		{
			return false;
		}
		SetPropertyValue(GetTargetComponent(), valueFromString);
		_2E63();
		UpdateDisplayedValue();
		if (AdvPropertyGrid.HighlightPropertyOnUpdate && !__2E43)
		{
			base.IsSelectionVisible = false;
			StartHighlight(_2E36);
		}
		return true;
	}

	private void _2E62(object _٠, EventArgs ؿ)
	{
		EditValueChanged = true;
	}

	protected virtual void EditorKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			CancelEdit();
			DevComponents.AdvTree.AdvTree treeControl = base.TreeControl;
			if (treeControl != null)
			{
				((Control)treeControl).Focus();
			}
			SetSelectedCell(base.Cells[0], eTreeAction.Keyboard);
		}
		else if ((int)e.KeyCode == 13)
		{
			if (ApplyEdit() && !_ᗞ && !__237D)
			{
				SetSelectedCell(base.Cells[0], eTreeAction.Keyboard);
				DevComponents.AdvTree.AdvTree treeControl2 = base.TreeControl;
				if (treeControl2 != null)
				{
					((Control)treeControl2).Focus();
				}
			}
		}
		else if ((int)e.KeyCode == 46 && !IsReadOnly && IsPropertyValueNullable && PropertyValue != null)
		{
			bool flag = true;
			TextBox val = (TextBox)((sender is TextBox) ? sender : null);
			if (val != null && ((TextBoxBase)val).SelectionLength != ((TextBoxBase)val).TextLength)
			{
				flag = false;
			}
			if (flag)
			{
				ApplyValue(null, null);
			}
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 40 && (int)e.Modifiers == 262144 && !_2E5C() && __17C0 is TextBoxDropDown textBoxDropDown && textBoxDropDown.ButtonDropDown.Visible && textBoxDropDown.DropDownControl != null && !textBoxDropDown.IsPopupOpen)
		{
			textBoxDropDown.IsPopupOpen = true;
		}
		base.OnKeyDown(e);
	}

	protected override void OnDeselected(eTreeAction action)
	{
		if (IsEditing)
		{
			if (EditValueChanged && !IsReadOnly)
			{
				ApplyEdit();
			}
			ExitEditorMode(action);
		}
		base.OnDeselected(action);
	}

	public virtual void ExitEditorMode(eTreeAction action)
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		if (!IsEditing)
		{
			return;
		}
		object obj = __17C0;
		Control val = (Control)((obj is Control) ? obj : null);
		if (!__2E43)
		{
			__17C0 = null;
			EditCell.HostedControl = null;
		}
		if (val is TextBoxDropDown && !__2E43)
		{
			TextBoxDropDown textBoxDropDown = (TextBoxDropDown)(object)val;
			if (textBoxDropDown.IsPopupOpen)
			{
				textBoxDropDown.CloseDropDown();
			}
			if (textBoxDropDown.DropDownControl != null && !__2E50)
			{
				Control dropDownControl = textBoxDropDown.DropDownControl;
				textBoxDropDown.DropDownControl = null;
				((Component)(object)dropDownControl).Dispose();
			}
			textBoxDropDown.ButtonCustomClick -= _2E60;
			textBoxDropDown.ButtonDropDownClick -= _2E5D;
			((Control)textBoxDropDown.TextBox).KeyDown -= new KeyEventHandler(EditorKeyDown);
			((Control)textBoxDropDown.TextBox).TextChanged -= _2E62;
			((Component)(object)textBoxDropDown).Dispose();
		}
		else if (val != null && !__2E43)
		{
			if (val is IPropertyValueEditor)
			{
				((IPropertyValueEditor)val).EditValueChanged -= _2E5B;
			}
			((Component)(object)val).Dispose();
		}
		if (__2E4C != null && __2E4C.IsBusy)
		{
			__2E4C.CancelAsync();
		}
		IsEditing = false;
	}

	internal override void OnDisplayChanged()
	{
		if (!ம && !ᗘ)
		{
			base.OnDisplayChanged();
		}
	}

	public virtual void CancelEdit()
	{
		if (IsEditing)
		{
			UpdateDisplayedValue();
			_2E65();
			__2E49 = false;
			if (__2E4C != null && __2E4C.IsBusy)
			{
				__2E4C.CancelAsync();
			}
		}
	}

	protected virtual void RevertEditorValue()
	{
		Cell editCell = EditCell;
		if (__17C0 is TextBoxDropDown)
		{
			TextBoxDropDown textBoxDropDown = (TextBoxDropDown)__17C0;
			((Control)textBoxDropDown).Text = editCell.Text;
			EditValueChanged = false;
			ElementStyle valueChangedStyle = ValueChangedStyle;
			if (valueChangedStyle != null && editCell.StyleNormal == valueChangedStyle && valueChangedStyle.Font != null)
			{
				((Control)textBoxDropDown.TextBox).Font = valueChangedStyle.Font;
			}
			else
			{
				((Control)textBoxDropDown.TextBox).Font = null;
			}
		}
		else if (__17C0 is IPropertyValueEditor)
		{
			IPropertyValueEditor propertyValueEditor = (IPropertyValueEditor)__17C0;
			if (!object.Equals(propertyValueEditor.EditValue, PropertyValue))
			{
				propertyValueEditor.EditValue = PropertyValue;
			}
			EditValueChanged = false;
			ElementStyle valueChangedStyle2 = ValueChangedStyle;
			if (valueChangedStyle2 != null && editCell.StyleNormal == valueChangedStyle2 && valueChangedStyle2.Font != null)
			{
				propertyValueEditor.EditorFont = valueChangedStyle2.Font;
			}
			else
			{
				propertyValueEditor.EditorFont = null;
			}
		}
	}

	public virtual bool ApplyEdit()
	{
		if (!IsEditing)
		{
			throw new InvalidOperationException("Node is not in edit mode.");
		}
		if (!EditValueChanged)
		{
			return true;
		}
		if (__17C0 is TextBoxDropDown)
		{
			return ApplyEdit(((Control)(TextBoxDropDown)__17C0).Text);
		}
		return ApplyEdit(((IPropertyValueEditor)__17C0).EditValue);
	}

	protected virtual bool ApplyEdit(object text)
	{
		Exception exception = null;
		object obj = null;
		obj = ((!(text is string)) ? text : GetValueFromString((string)text, out exception));
		return ApplyValue(obj, exception);
	}

	public string GetPropertyPath()
	{
		string text = GetPropertyName();
		for (Node parent = base.Parent; parent != null; parent = parent.Parent)
		{
			if (parent is PropertyNode { PropertyDescriptor: not null } propertyNode)
			{
				text = propertyNode.GetPropertyName() + "." + text;
			}
		}
		return text;
	}

	protected virtual bool ApplyValue(object value, Exception valueException)
	{
		bool flag = false;
		if (GetPropertyValue(GetTargetComponent()) == value && valueException == null)
		{
			return true;
		}
		if (valueException == null)
		{
			AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
			if (advPropertyGrid != null)
			{
				flag = advPropertyGrid.ⶄ(GetPropertyName(), value, __2E3D, GetPropertyPath());
			}
			if (!flag)
			{
				try
				{
					SetPropertyValue(GetTargetComponent(), value);
					flag = true;
					if (_ᗞ || __237D)
					{
						return true;
					}
				}
				catch (Exception ex)
				{
					valueException = ex;
				}
			}
		}
		if (!flag)
		{
			if (__2D7C == null || __2D7C.Image == null)
			{
				base.Image = _2E6B();
			}
			base.IsSelectionVisible = false;
			SuperTooltip superTooltip = _2E6C();
			if (superTooltip != null)
			{
				_2E66(superTooltip, valueException, value);
			}
			StartHighlight(_2E35);
		}
		else
		{
			if (value != null && !value.Equals(PropertyValue) && PropertyType.IsPrimitive)
			{
				flag = false;
			}
			bool flag2 = AdvPropertyGrid.HighlightPropertyOnUpdate && !__2E43;
			if (__2E4A)
			{
				flag2 = true;
			}
			_2E65();
			UpdateDisplayedValue();
			if (flag2)
			{
				base.IsSelectionVisible = false;
			}
			if (flag2 && flag)
			{
				StartHighlight(_2E36);
			}
			__2E49 = false;
			if (__2E3D != null && flag)
			{
				_2E63();
			}
		}
		return flag;
	}

	private void _2E63()
	{
		if (__2E3D == null)
		{
			return;
		}
		RefreshPropertiesAttribute refreshPropertiesAttribute = _2E64();
		RefreshPropertiesAttribute refreshPropertiesAttribute2 = null;
		if (base.Parent is PropertyNode)
		{
			refreshPropertiesAttribute2 = ((PropertyNode)base.Parent)._2E64();
		}
		if (refreshPropertiesAttribute2 != null && refreshPropertiesAttribute2.RefreshProperties == RefreshProperties.All)
		{
			AdvPropertyGrid.ⶡ(base.Parent);
		}
		else if (refreshPropertiesAttribute != null)
		{
			if (refreshPropertiesAttribute.RefreshProperties == RefreshProperties.Repaint)
			{
				AdvPropertyGrid.RefreshPropertyValues();
			}
			else if (refreshPropertiesAttribute.RefreshProperties == RefreshProperties.All)
			{
				AdvPropertyGrid.RefreshProperties();
			}
		}
		if (base.Parent is PropertyNode)
		{
			((PropertyNode)base.Parent).UpdateDisplayedValue(refreshSubProperties: false);
		}
	}

	internal RefreshPropertiesAttribute _2E64()
	{
		return __2E3D.Attributes[typeof(RefreshPropertiesAttribute)] as RefreshPropertiesAttribute;
	}

	private void _2E65()
	{
		SuperTooltip superTooltip = _2E6C();
		if (superTooltip != null)
		{
			_2E6A(superTooltip);
		}
		if (__2D7C == null || __2D7C.Image == null)
		{
			base.Image = null;
		}
		if (__2D7C != null)
		{
			UpdatePropertySettings();
		}
	}

	private void _2E66(SuperTooltip _2E67, Exception _2E68, object ԝ)
	{
		SuperTooltipInfo superTooltipInfo = new SuperTooltipInfo();
		superTooltipInfo.HeaderText = base.Text;
		string text = _2E69();
		bool flag = false;
		if (_2E68 is InvalidPropertyValueException && !string.IsNullOrEmpty(_2E68.Message))
		{
			text = _2E68.Message;
			flag = true;
		}
		else if (_2E68 != null)
		{
			if (!text.EndsWith(" "))
			{
				text += " ";
			}
			text += _2E68.Message;
		}
		superTooltipInfo.BodyText = text;
		AdvPropertyGrid?.ⶥ(superTooltipInfo, GetPropertyName(), _2E68, ԝ);
		_2E67.SetSuperTooltip(this, superTooltipInfo);
		if (flag)
		{
			Rectangle bounds = base.Cells[0].Bounds;
			DevComponents.AdvTree.AdvTree treeControl = base.TreeControl;
			if (treeControl != null)
			{
				bounds.Location = ((Control)treeControl).PointToScreen(bounds.Location);
			}
			_2E67.ShowTooltip(this, new Point(bounds.Right - 18, bounds.Bottom));
		}
		__2E4A = true;
	}

	private string _2E69()
	{
		string text = "Error setting the value. ";
		AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
		if (advPropertyGrid != null)
		{
			text = advPropertyGrid.SystemText.ErrorSettingPropertyValueTooltip;
		}
		IPropertyGridLocalizer propertyGridLocalizer = PropertyGridLocalizer;
		if (propertyGridLocalizer != null)
		{
			return propertyGridLocalizer.GetErrorTooltipMessage(text) ?? text;
		}
		return text ?? "";
	}

	private void _2E6A(SuperTooltip _2E67)
	{
		_2E67.HideTooltip();
		_2E67.SetSuperTooltip(this, null);
		__2E4A = false;
	}

	private Image _2E6B()
	{
		AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
		if (advPropertyGrid != null && advPropertyGrid.Appearance.PropertyValueErrorImage != null)
		{
			return advPropertyGrid.Appearance.PropertyValueErrorImage;
		}
		if (__2E4B == null)
		{
			__2E4B = (Image)(object)BarFunctions.ই("SystemImages.ErrorIcon.png");
		}
		return __2E4B;
	}

	public void StartHighlight(Color color)
	{
		if (__2E4C == null)
		{
			__2E4C = new BackgroundWorker();
			__2E4C.WorkerSupportsCancellation = true;
			__2E4C.DoWork += _2E70;
			__2E4C.RunWorkerCompleted += _2E6F;
			if (base.Style != null)
			{
				__2E4D = base.Style.Copy();
			}
			else
			{
				DevComponents.AdvTree.AdvTree treeControl = base.TreeControl;
				if (treeControl != null && treeControl.NodeStyle != null)
				{
					__2E4D = treeControl.NodeStyle.Copy();
				}
				else
				{
					__2E4D = new ElementStyle();
				}
			}
		}
		else if (__2E4C.IsBusy)
		{
			__2E4C.CancelAsync();
			return;
		}
		__2E4D.BackColor = color;
		base.Style = __2E4D;
		__2E4C.RunWorkerAsync(base.TreeControl);
	}

	private SuperTooltip _2E6C()
	{
		return AdvPropertyGrid?.SuperTooltip;
	}

	internal void _2E6D(SuperTooltip _2E6E)
	{
		string text = _2E56();
		if (string.IsNullOrEmpty(text))
		{
			_2E6E.SetSuperTooltip(this, null);
			return;
		}
		SuperTooltipInfo info = new SuperTooltipInfo(base.Text, "", text, null, null, eTooltipColor.System, headerVisible: true, footerVisible: false, Size.Empty);
		_2E6E.SetSuperTooltip(this, info);
	}

	private void _2E6F(object _٠, RunWorkerCompletedEventArgs ؿ)
	{
		base.Style = ᑡ();
		base.IsSelectionVisible = true;
	}

	private void _2E70(object _٠, DoWorkEventArgs ؿ)
	{
		int num = 255;
		while (num > 0 && !ؿ.Cancel)
		{
			object? argument = ؿ.Argument;
			Control val = (Control)((argument is Control) ? argument : null);
			if (val.IsHandleCreated && !val.IsDisposed)
			{
				val.BeginInvoke((Delegate)new _2EBB(_2E71), new object[1] { num });
			}
			try
			{
				using ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
				manualResetEvent.WaitOne(80);
			}
			catch
			{
				break;
			}
			num -= 25;
		}
	}

	private void _2E71(int _2E72)
	{
		__2E4D.BackColor = Color.FromArgb(_2E72, __2E4D.BackColor.R, __2E4D.BackColor.G, __2E4D.BackColor.B);
	}

	protected virtual object GetValueFromString(string text, out Exception exception)
	{
		object result = null;
		exception = null;
		if (__2D7C != null && __2D7C.HasConvertFromStringToPropertyValueHandler)
		{
			ConvertValueEventArgs convertValueEventArgs = new ConvertValueEventArgs(text, null, GetPropertyName(), GetTargetComponent(), PropertyDescriptor);
			try
			{
				__2D7C.InvokeConvertFromStringToPropertyValue(convertValueEventArgs);
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			if (convertValueEventArgs.IsConverted)
			{
				return convertValueEventArgs.TypedValue;
			}
		}
		AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
		if (advPropertyGrid != null && advPropertyGrid.ⵔ)
		{
			ConvertValueEventArgs convertValueEventArgs2 = new ConvertValueEventArgs(text, null, GetPropertyName(), GetTargetComponent(), PropertyDescriptor);
			try
			{
				advPropertyGrid.ⶭ(convertValueEventArgs2);
			}
			catch (Exception ex2)
			{
				exception = ex2;
			}
			if (convertValueEventArgs2.IsConverted)
			{
				return convertValueEventArgs2.TypedValue;
			}
		}
		TypeConverter typeConverter = _2E30;
		try
		{
			result = typeConverter.ConvertFromString(this, text);
		}
		catch (Exception ex3)
		{
			exception = ex3;
		}
		return result;
	}

	protected virtual void SetPropertyValue(object target, object value)
	{
		PropertyDescriptor propertyDescriptor = GetPropertyDescriptor();
		if (propertyDescriptor == null)
		{
			return;
		}
		if (__2E4E)
		{
			TypeConverter typeConverter = _2E30;
			object[] array = (object[])target;
			object[] array2 = array;
			foreach (object obj in array2)
			{
				if (propertyDescriptor.PropertyType.IsAssignableFrom(obj.GetType()))
				{
					SetPropertyValueCore(obj, value, propertyDescriptor);
					continue;
				}
				PropertyDescriptor propertyDescriptor2 = _2E8F._2EA2(propertyDescriptor.Name, obj, typeConverter.GetPropertiesSupported(this) ? typeConverter : null);
				if (propertyDescriptor2 == null)
				{
					propertyDescriptor2 = _2E8F._2EA2(propertyDescriptor.Name, obj, null);
				}
				SetPropertyValueCore(obj, value, propertyDescriptor2);
			}
		}
		else
		{
			SetPropertyValueCore(target, value, propertyDescriptor);
		}
	}

	private void _2D74(object _᭖, object ԝ, PropertyDescriptor _2E73)
	{
		AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
		if (advPropertyGrid != null && advPropertyGrid.ⵒ)
		{
			ValidatePropertyValueEventArgs validatePropertyValueEventArgs = new ValidatePropertyValueEventArgs(_2E73.Name, ԝ, _᭖);
			advPropertyGrid.ⶃ(validatePropertyValueEventArgs);
			if (validatePropertyValueEventArgs.Cancel)
			{
				throw new InvalidPropertyValueException(validatePropertyValueEventArgs.Message);
			}
		}
	}

	protected virtual void SetPropertyValueCore(object target, object value, PropertyDescriptor propertyDescriptor)
	{
		_2D74(target, value, propertyDescriptor);
		bool flag = false;
		PropertyNode propertyNode = base.Parent as PropertyNode;
		if (propertyNode != null)
		{
			TypeConverter typeConverter = propertyNode._2E30;
			if (typeConverter != null && typeConverter.GetCreateInstanceSupported(this))
			{
				flag = true;
			}
		}
		if (flag)
		{
			if (target is ICustomTypeDescriptor)
			{
				target = ((ICustomTypeDescriptor)target).GetPropertyOwner(propertyDescriptor);
			}
			object targetComponent = propertyNode.GetTargetComponent();
			TypeConverter typeConverter2 = propertyNode._2E30;
			PropertyDescriptorCollection properties = typeConverter2.GetProperties(propertyNode, targetComponent);
			Dictionary<string, object> dictionary = new Dictionary<string, object>(properties.Count);
			object obj = null;
			for (int i = 0; i < properties.Count; i++)
			{
				string propertyName = GetPropertyName();
				if (propertyName != null && propertyName.Equals(properties[i].Name))
				{
					dictionary[properties[i].Name] = value;
				}
				else
				{
					dictionary[properties[i].Name] = properties[i].GetValue(target);
				}
			}
			try
			{
				obj = typeConverter2.CreateInstance(propertyNode, dictionary);
			}
			catch (Exception ex)
			{
				if (string.IsNullOrEmpty(ex.Message))
				{
					throw new TargetInvocationException("Exception Creating New Instance Of Object For Property " + base.Text + " " + propertyNode.PropertyType.FullName + " " + ex.ToString(), ex);
				}
				throw;
			}
			if (obj != null)
			{
				propertyNode.PropertyValue = obj;
			}
		}
		else
		{
			if (target is ICustomTypeDescriptor)
			{
				target = ((ICustomTypeDescriptor)target).GetPropertyOwner(propertyDescriptor);
			}
			try
			{
				propertyDescriptor.SetValue(target, value);
			}
			catch
			{
				throw;
			}
		}
		AdvPropertyGrid?.InvokePropertyValueChanged(GetPropertyName(), value, null);
	}

	protected override void Dispose(bool disposing)
	{
		_ᗞ = true;
		if (IsEditing)
		{
			CancelEdit();
		}
		BackgroundWorker backgroundWorker = __2E4C;
		__2E4C = null;
		backgroundWorker?.Dispose();
		if (__2E4B != null)
		{
			__2E4B.Dispose();
		}
		base.Dispose(disposing);
	}

	internal virtual void _2E74()
	{
		_2E75();
		UpdatePropertySettings();
	}

	private void _2E75()
	{
		if (HasChildProperties)
		{
			base.ExpandVisibility = eNodeExpandVisibility.Visible;
			if (base.Expanded)
			{
				LoadChildProperties();
			}
			return;
		}
		if (base.HasChildNodes)
		{
			AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
			foreach (Node node in base.Nodes)
			{
				AdvPropertyGrid._2D9B(node, advPropertyGrid);
			}
			base.Nodes.Clear();
		}
		base.Expanded = false;
		base.ExpandVisibility = eNodeExpandVisibility.Auto;
	}

	protected virtual void LoadChildProperties()
	{
		string text = null;
		DevComponents.AdvTree.AdvTree treeControl = base.TreeControl;
		ePropertySort _2E99 = ePropertySort.Alphabetical;
		if (treeControl != null)
		{
			treeControl.BeginUpdate();
			if (((Control)treeControl).Parent is AdvPropertyGrid)
			{
				_2E99 = ((AdvPropertyGrid)(object)((Control)treeControl).Parent).SubPropertiesDefaultSort;
			}
		}
		try
		{
			if (base.HasChildNodes && treeControl != null && treeControl.SelectedNode != null && base.Nodes.Contains(treeControl.SelectedNode))
			{
				text = treeControl.SelectedNode.Text;
			}
			_2E76();
			TypeConverter typeConverter = _2E30;
			if (typeConverter != null && typeConverter.GetPropertiesSupported())
			{
				Attribute[] attributeFilter = AttributeFilter;
				List<string> ignoredProperties = IgnoredProperties;
				List<string> ignoredCategories = IgnoredCategories;
				IPropertyGridLocalizer propertyGridLocalizer = PropertyGridLocalizer;
				PropertyDescriptorCollection propertyDescriptorCollection = _2E8F._2E9E(this, attributeFilter, PropertyValue, typeConverter.GetPropertiesSupported(this) ? typeConverter : null);
				_2EA5 nodeFactory = _2E37;
				if (propertyDescriptorCollection != null && propertyDescriptorCollection.Count > 0)
				{
					_2E8F _2E8F2 = new _2E8F(this, PropertyValue, attributeFilter, nodeFactory, ignoredProperties, ignoredCategories, propertyGridLocalizer, PropertySettingsCollection);
					_2E8F2._2E90 = AdvPropertyGrid.HelpType;
					_2E8F2._2E30 = (typeConverter.GetPropertiesSupported(this) ? typeConverter : null);
					_2E8F2.ሕ(base.Nodes, _2E99, _2E6C());
				}
			}
			if (string.IsNullOrEmpty(text) || !base.HasChildNodes || treeControl == null)
			{
				return;
			}
			foreach (Node node in base.Nodes)
			{
				if (node.Text == text)
				{
					treeControl.SelectedNode = node;
					break;
				}
			}
		}
		finally
		{
			treeControl?.EndUpdate();
		}
	}

	private void _2E76()
	{
		if (!base.HasChildNodes)
		{
			return;
		}
		AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
		foreach (Node node in base.Nodes)
		{
			AdvPropertyGrid._2D9B(node, advPropertyGrid);
			if (node.IsSelected && advPropertyGrid.PropertyTree != null)
			{
				advPropertyGrid.PropertyTree.SelectedNode = null;
			}
		}
		base.Nodes.Clear();
	}

	private void _2E77(object _٠, PropertyChangedEventArgs ؿ)
	{
		if (ؿ.PropertyName != PropertySettings._2E8C && ؿ.PropertyName != PropertySettings._2E8D)
		{
			UpdatePropertySettings();
		}
		if (ؿ.PropertyName == "ValueEditor")
		{
			_2E55();
		}
	}

	public virtual void UpdatePropertySettings()
	{
		bool flag = false;
		TypeConverter typeConverter = null;
		if (base.Parent is PropertyNode)
		{
			PropertyNode propertyNode = (PropertyNode)base.Parent;
			typeConverter = propertyNode._2E30;
			if (typeConverter != null && typeConverter.GetCreateInstanceSupported(this))
			{
				flag = true;
			}
		}
		if (__2D7C == null)
		{
			if (!base.Visible)
			{
				base.Visible = true;
			}
			bool flag2 = false;
			if (typeConverter == null || !flag)
			{
				flag2 = _2E8F._2E9B(__2E3D, GetTargetComponent());
			}
			if (flag2 && !_2E38)
			{
				IsReadOnly = flag2;
			}
			else if (flag || _2E58())
			{
				IsReadOnly = false;
			}
			base.Text = _2E78();
			base.ImageAlignment = eCellPartAlignment.FarCenter;
			base.Image = null;
			if (!__2E4A)
			{
				_2E6C()?.SetSuperTooltip(this, null);
			}
			return;
		}
		if (base.Visible != __2D7C.Visible)
		{
			base.Visible = __2D7C.Visible;
		}
		if (IsReadOnly != __2D7C.ReadOnly)
		{
			IsReadOnly = __2D7C.ReadOnly;
		}
		else if (!__2E4A && !IsReadOnly)
		{
			base.Style = ᑡ();
		}
		if (!__2E4A && IsReadOnly && __2D7C.ReadOnlyTooltip != null)
		{
			_2E6C()?.SetSuperTooltip(this, __2D7C.ReadOnlyTooltip);
		}
		if (__2D7C.Image != null)
		{
			base.Image = __2D7C.Image;
			base.ImageAlignment = __2D7C.ImageAlignment;
		}
		else
		{
			base.ImageAlignment = eCellPartAlignment.FarCenter;
			base.Image = null;
		}
		if (!string.IsNullOrEmpty(__2D7C.DisplayName))
		{
			base.Text = __2D7C.DisplayName;
		}
		else
		{
			base.Text = _2E78();
		}
		if (!__2E4A && __2D7C.Tooltip != null && (!__2D7C.ReadOnly || __2D7C.ReadOnlyTooltip == null))
		{
			_2E6C()?.SetSuperTooltip(this, __2D7C.Tooltip);
		}
	}

	private string _2E78()
	{
		string result = __2E3D.DisplayName;
		IPropertyGridLocalizer propertyGridLocalizer = PropertyGridLocalizer;
		if (propertyGridLocalizer != null)
		{
			string propertyName = propertyGridLocalizer.GetPropertyName(__2E3D.Name);
			if (!string.IsNullOrEmpty(propertyName))
			{
				result = propertyName;
			}
		}
		return result;
	}

	protected override void OnExpandChanging(bool expanded, eTreeAction action)
	{
		if (expanded)
		{
			LoadChildProperties();
		}
		base.OnExpandChanging(expanded, action);
	}

	protected override void OnKeyboardCopy(KeyEventArgs args)
	{
		Clipboard.SetText(EditCell.Text);
		base.OnKeyboardCopy(args);
	}

	protected override void OnKeyboardPaste(KeyEventArgs args)
	{
		if (Clipboard.ContainsText() && !IsReadOnly)
		{
			ApplyEdit(Clipboard.GetText());
		}
		base.OnKeyboardPaste(args);
	}

	private void _2E7B()
	{
	}

	void ITypeDescriptorContext.OnComponentChanged()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ⹻
		this._2E7B();
	}

	private bool _2E7C()
	{
		return true;
	}

	bool ITypeDescriptorContext.OnComponentChanging()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ⹼
		return this._2E7C();
	}

	private object _2E7E(Type _2E7F)
	{
		if ((object)_2E7F == typeof(PropertyNode) || (object)_2E7F == typeof(IWindowsFormsEditorService))
		{
			return this;
		}
		AdvPropertyGrid advPropertyGrid = AdvPropertyGrid;
		if (advPropertyGrid != null)
		{
			if (((Component)(object)advPropertyGrid).Site != null)
			{
				return ((Component)(object)advPropertyGrid).Site.GetService(_2E7F);
			}
			if (((Control)advPropertyGrid).Parent != null && ((Component)(object)((Control)advPropertyGrid).Parent).Site != null)
			{
				return ((Component)(object)((Control)advPropertyGrid).Parent).Site.GetService(_2E7F);
			}
		}
		return null;
	}

	object IServiceProvider.GetService(Type _2E7F)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ⹾
		return this._2E7E(_2E7F);
	}

	private void _2E80()
	{
		if (__17C0 is TextBoxDropDown { IsPopupOpen: not false } textBoxDropDown)
		{
			textBoxDropDown.CloseDropDown();
		}
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	private static extern int MsgWaitForMultipleObjectsEx(int nCount, IntPtr pHandles, int dwMilliseconds, int dwWakeMask, int dwFlags);

	private void _2E81(Control ॼ)
	{
		Type typeFromHandle = typeof(Application);
		TextBoxDropDown textBoxDropDown = __17C0 as TextBoxDropDown;
		MethodInfo method = typeFromHandle.GetMethod("DoEventsModal", BindingFlags.Static | BindingFlags.NonPublic);
		if ((object)method != null && textBoxDropDown != null)
		{
			textBoxDropDown.DropDownControl = ॼ;
			__2E50 = true;
			textBoxDropDown.ShowDropDown();
			while (textBoxDropDown != null && textBoxDropDown.IsPopupOpen)
			{
				method.Invoke(null, null);
				MsgWaitForMultipleObjectsEx(0, IntPtr.Zero, 250, 255, 4);
			}
			if (textBoxDropDown != null)
			{
				textBoxDropDown._28DE();
				textBoxDropDown.DropDownControl = null;
			}
			__2E50 = false;
		}
	}

	private DialogResult _2E82(Form _2E83)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return _2E83.ShowDialog();
	}

	[CompilerGenerated]
	private void _2E84()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		UITypeEditor val = _2E5F();
		if (val != null)
		{
			object obj = null;
			try
			{
				obj = val.EditValue((ITypeDescriptorContext)this, (IServiceProvider)this, PropertyValue);
			}
			catch (Exception ex)
			{
				MessageBoxEx.Show(ex.Message);
				return;
			}
			ApplyValue(obj, null);
		}
	}
}
