using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[TypeConverter(typeof(BindingDefConverer))]
[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class BindingDef : INotifyPropertyChanged
{
	private DataConvertEventHandler _05C3;

	private static TypeConverter ش;

	private string[] _ᬅ = new string[0];

	private string _ᬆ = "";

	private BindingMemberInfo _ᬇ;

	private string _ᬈ = "";

	private bool _ب;

	private string _ة = "";

	private IFormatProvider _ت;

	private object _ᬉ;

	private PropertyChangedEventHandler চ;

	[DefaultValue("")]
	[Category("Data")]
	[Description("Indicates property name binding is attached to.")]
	public string PropertyName
	{
		get
		{
			return _ᬆ;
		}
		set
		{
			if (value != _ᬆ)
			{
				string ᬆ = _ᬆ;
				_ᬆ = value;
				OnPropertyNameChanged(ᬆ, value);
			}
		}
	}

	[Browsable(false)]
	public BindingMemberInfo BindingMemberInfo => _ᬇ;

	[Category("Data")]
	[Description("Indicates data member name which holds data that PropertyName is populated with.")]
	[DefaultValue("")]
	public string DataMember
	{
		get
		{
			return _ᬈ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (value != _ᬈ)
			{
				string ᬈ = _ᬈ;
				_ᬈ = value;
				OnDataMemberChanged(ᬈ, value);
			}
		}
	}

	[DefaultValue(false)]
	[Category("Data")]
	[Description("Indicates whether type conversion and formatting is applied to the property data.")]
	public bool FormattingEnabled
	{
		get
		{
			return _ب;
		}
		set
		{
			if (value != _ب)
			{
				bool ب = _ب;
				_ب = value;
				OnFormattingEnabledChanged(ب, value);
			}
		}
	}

	[Category("Data")]
	[DefaultValue("")]
	[Description("Indicates format specifier characters that indicate how a value is to be displayed.")]
	public string FormatString
	{
		get
		{
			return _ة;
		}
		set
		{
			if (value != _ة)
			{
				string ة = _ة;
				_ة = value;
				OnFormatStringChanged(ة, value);
			}
		}
	}

	[Description("Indicates IFormatProvider that provides custom formatting behavior.")]
	[Category("Data")]
	[DefaultValue(null)]
	public IFormatProvider FormatInfo
	{
		get
		{
			return _ت;
		}
		set
		{
			if (value != _ت)
			{
				IFormatProvider ت = _ت;
				_ت = value;
				OnFormatInfoChanged(ت, value);
			}
		}
	}

	[Category("Data")]
	[Description("Indicates Object to be set as the control property when the data source contains a DBNull value. ")]
	[TypeConverter(typeof(StringConverter))]
	[DefaultValue(null)]
	public object NullValue
	{
		get
		{
			return _ᬉ;
		}
		set
		{
			if (value != _ᬉ)
			{
				object ᬉ = _ᬉ;
				_ᬉ = value;
				OnNullValueChanged(ᬉ, value);
			}
		}
	}

	public event DataConvertEventHandler Format
	{
		add
		{
			DataConvertEventHandler dataConvertEventHandler = _05C3;
			DataConvertEventHandler dataConvertEventHandler2;
			do
			{
				dataConvertEventHandler2 = dataConvertEventHandler;
				DataConvertEventHandler value2 = (DataConvertEventHandler)Delegate.Combine(dataConvertEventHandler2, value);
				dataConvertEventHandler = Interlocked.CompareExchange(ref _05C3, value2, dataConvertEventHandler2);
			}
			while ((object)dataConvertEventHandler != dataConvertEventHandler2);
		}
		remove
		{
			DataConvertEventHandler dataConvertEventHandler = _05C3;
			DataConvertEventHandler dataConvertEventHandler2;
			do
			{
				dataConvertEventHandler2 = dataConvertEventHandler;
				DataConvertEventHandler value2 = (DataConvertEventHandler)Delegate.Remove(dataConvertEventHandler2, value);
				dataConvertEventHandler = Interlocked.CompareExchange(ref _05C3, value2, dataConvertEventHandler2);
			}
			while ((object)dataConvertEventHandler != dataConvertEventHandler2);
		}
	}

	[Description("Occurs when property on BindingDef object has changed.Occurs when property on BindingDef object has changed.")]
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

	protected virtual void OnFormat(DataConvertEventArgs e)
	{
		_05C3?.Invoke(this, e);
	}

	public BindingDef()
	{
	}

	public BindingDef(string propertyName, string dataMember)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		_ᬆ = propertyName;
		_ᬅ = propertyName.Split(new char[1] { '.' });
		_ᬈ = dataMember;
		_ᬇ = new BindingMemberInfo(dataMember);
	}

	public void Update(object item, object data)
	{
		Update(null, item, data);
	}

	public void Update(ItemVisualGenerator generator, object item, object data)
	{
		string name = _ᬆ;
		if (item is BaseItem && _ᬅ.Length > 1)
		{
			BaseItem baseItem = (BaseItem)item;
			for (int i = 0; i < _ᬅ.Length - 1; i++)
			{
				baseItem = baseItem.SubItems[_ᬅ[i]];
			}
			item = baseItem;
			name = _ᬅ[_ᬅ.Length - 1];
		}
		PropertyInfo property = item.GetType().GetProperty(name);
		if ((object)property.PropertyType == typeof(string))
		{
			property.SetValue(item, ᬊ(generator, data, _ᬈ), null);
		}
		else
		{
			property.SetValue(item, ᬎ(generator, data, _ᬈ, ᬏ(generator, data, _ᬈ)), null);
		}
	}

	private string ᬊ(ItemVisualGenerator ᬋ, object ᬌ, string _06E3)
	{
		object ᬍ = ᬏ(ᬋ, ᬌ, _06E3);
		return ᬊ(ᬋ, ᬌ, _06E3, ᬍ);
	}

	private string ᬊ(ItemVisualGenerator ᬋ, object ᬌ, string _06E3, object ᬍ)
	{
		if (!_ب)
		{
			if (ᬌ == null)
			{
				return string.Empty;
			}
			if (ᬍ == null)
			{
				return "";
			}
			return Convert.ToString(ᬍ, CultureInfo.CurrentCulture);
		}
		DataConvertEventArgs dataConvertEventArgs = new DataConvertEventArgs(ᬍ, typeof(string), ᬌ, _06E3);
		OnFormat(dataConvertEventArgs);
		if (((ConvertEventArgs)dataConvertEventArgs).Value != ᬌ && ((ConvertEventArgs)dataConvertEventArgs).Value is string)
		{
			return (string)((ConvertEventArgs)dataConvertEventArgs).Value;
		}
		if (ش == null)
		{
			ش = TypeDescriptor.GetConverter(typeof(string));
		}
		try
		{
			return (string)ᬒ.ᬖ(ᬍ, typeof(string), ᬋ._06E2(_06E3), ش, _ة, _ت, null, DBNull.Value);
		}
		catch (Exception ex)
		{
			if (ex is SecurityException || _۵(ex))
			{
				throw;
			}
			return (ᬍ != null) ? Convert.ToString(ᬌ, CultureInfo.CurrentCulture) : "";
		}
	}

	private object ᬎ(ItemVisualGenerator ᬋ, object ᬌ, string _06E3, object ᬍ)
	{
		if (!_ب)
		{
			return ᬍ;
		}
		DataConvertEventArgs dataConvertEventArgs = new DataConvertEventArgs(ᬍ, typeof(object), ᬌ, _06E3);
		OnFormat(dataConvertEventArgs);
		return ((ConvertEventArgs)dataConvertEventArgs).Value;
	}

	private static bool _۵(Exception _۶)
	{
		if (!(_۶ is NullReferenceException) && !(_۶ is StackOverflowException) && !(_۶ is OutOfMemoryException) && !(_۶ is ThreadAbortException) && !(_۶ is ExecutionEngineException) && !(_۶ is IndexOutOfRangeException))
		{
			return _۶ is AccessViolationException;
		}
		return true;
	}

	private object ᬏ(ItemVisualGenerator ᬋ, object ᬌ, string _06E3)
	{
		if (ᬌ != null && _06E3.Length > 0)
		{
			try
			{
				PropertyDescriptor propertyDescriptor = null;
				if (ᬋ._1AFD != null)
				{
					propertyDescriptor = ((BindingManagerBase)ᬋ._1AFD).GetItemProperties().Find(_06E3, ignoreCase: true);
				}
				if (propertyDescriptor == null)
				{
					propertyDescriptor = TypeDescriptor.GetProperties(ᬌ).Find(_06E3, ignoreCase: true);
				}
				if (propertyDescriptor != null)
				{
					ᬌ = propertyDescriptor.GetValue(ᬌ);
				}
			}
			catch
			{
			}
		}
		if (ᬌ == DBNull.Value && _ᬉ != null)
		{
			return _ᬉ;
		}
		return ᬌ;
	}

	protected virtual void OnPropertyNameChanged(string oldValue, string newValue)
	{
		_ᬅ = newValue.Split(new char[1] { '.' });
		OnPropertyChanged(new PropertyChangedEventArgs("PropertyName"));
	}

	protected virtual void OnDataMemberChanged(string oldValue, string newValue)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (!string.IsNullOrEmpty(newValue))
		{
			_ᬇ = new BindingMemberInfo(newValue);
		}
		else
		{
			_ᬇ = default(BindingMemberInfo);
		}
		OnPropertyChanged(new PropertyChangedEventArgs("DataMember"));
	}

	protected virtual void OnFormattingEnabledChanged(bool oldValue, bool newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("FormattingEnabled"));
	}

	protected virtual void OnFormatStringChanged(string oldValue, string newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("FormatString"));
	}

	protected virtual void OnFormatInfoChanged(IFormatProvider oldValue, IFormatProvider newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("FormatInfo"));
	}

	protected virtual void OnNullValueChanged(object oldValue, object newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("NullValue"));
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		চ?.Invoke(this, e);
	}
}
