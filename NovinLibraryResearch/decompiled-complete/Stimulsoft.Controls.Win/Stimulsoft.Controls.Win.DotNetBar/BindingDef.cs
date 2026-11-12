using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[TypeConverter(typeof(BindingDefConverer))]
[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class BindingDef : INotifyPropertyChanged
{
	private static TypeConverter stringTypeConverter;

	private string[] _PropertyPath = new string[0];

	private string _PropertyName = "";

	private BindingMemberInfo _BindingMemberInfo;

	private string _DataMember = "";

	private bool _FormattingEnabled;

	private string _FormatString = "";

	private IFormatProvider _FormatInfo;

	private object _NullValue;

	[DefaultValue("")]
	[Category("Data")]
	[Description("Indicates property name binding is attached to.")]
	public string PropertyName
	{
		get
		{
			return _PropertyName;
		}
		set
		{
			if (value != _PropertyName)
			{
				string propertyName = _PropertyName;
				_PropertyName = value;
				OnPropertyNameChanged(propertyName, value);
			}
		}
	}

	[Browsable(false)]
	public BindingMemberInfo BindingMemberInfo => _BindingMemberInfo;

	[DefaultValue("")]
	[Category("Data")]
	[Description("Indicates data member name which holds data that PropertyName is populated with.")]
	public string DataMember
	{
		get
		{
			return _DataMember;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (value != _DataMember)
			{
				string dataMember = _DataMember;
				_DataMember = value;
				OnDataMemberChanged(dataMember, value);
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
			return _FormattingEnabled;
		}
		set
		{
			if (value != _FormattingEnabled)
			{
				bool formattingEnabled = _FormattingEnabled;
				_FormattingEnabled = value;
				OnFormattingEnabledChanged(formattingEnabled, value);
			}
		}
	}

	[DefaultValue("")]
	[Category("Data")]
	[Description("Indicates format specifier characters that indicate how a value is to be displayed.")]
	public string FormatString
	{
		get
		{
			return _FormatString;
		}
		set
		{
			if (value != _FormatString)
			{
				string formatString = _FormatString;
				_FormatString = value;
				OnFormatStringChanged(formatString, value);
			}
		}
	}

	[DefaultValue(null)]
	[Category("Data")]
	[Description("Indicates IFormatProvider that provides custom formatting behavior.")]
	public IFormatProvider FormatInfo
	{
		get
		{
			return _FormatInfo;
		}
		set
		{
			if (value != _FormatInfo)
			{
				IFormatProvider formatInfo = _FormatInfo;
				_FormatInfo = value;
				OnFormatInfoChanged(formatInfo, value);
			}
		}
	}

	[DefaultValue(null)]
	[Category("Data")]
	[Description("Indicates Object to be set as the control property when the data source contains a DBNull value. ")]
	[TypeConverter(typeof(StringConverter))]
	public object NullValue
	{
		get
		{
			return _NullValue;
		}
		set
		{
			if (value != _NullValue)
			{
				object nullValue = _NullValue;
				_NullValue = value;
				OnNullValueChanged(nullValue, value);
			}
		}
	}

	public event DataConvertEventHandler Format;

	[Description("Occurs when property on BindingDef object has changed.Occurs when property on BindingDef object has changed.")]
	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual void OnFormat(DataConvertEventArgs e)
	{
		this.Format?.Invoke(this, e);
	}

	public BindingDef()
	{
	}

	public BindingDef(string propertyName, string dataMember)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		_PropertyName = propertyName;
		_PropertyPath = propertyName.Split(new char[1] { '.' });
		_DataMember = dataMember;
		_BindingMemberInfo = new BindingMemberInfo(dataMember);
	}

	public void Update(object item, object data)
	{
		Update(null, item, data);
	}

	public void Update(ItemVisualGenerator generator, object item, object data)
	{
		string name = _PropertyName;
		if (item is BaseItem && _PropertyPath.Length > 1)
		{
			BaseItem baseItem = (BaseItem)item;
			for (int i = 0; i < _PropertyPath.Length - 1; i++)
			{
				baseItem = baseItem.SubItems[_PropertyPath[i]];
			}
			item = baseItem;
			name = _PropertyPath[_PropertyPath.Length - 1];
		}
		PropertyInfo property = item.GetType().GetProperty(name);
		if (property.PropertyType == typeof(string))
		{
			property.SetValue(item, GetDataText(generator, data, _DataMember), null);
		}
		else
		{
			property.SetValue(item, GetPropertyValue(generator, data, _DataMember), null);
		}
	}

	private string GetDataText(ItemVisualGenerator generator, object data, string fieldName)
	{
		object propertyValue = GetPropertyValue(generator, data, fieldName);
		return GetDataText(generator, data, fieldName, propertyValue);
	}

	private string GetDataText(ItemVisualGenerator generator, object data, string fieldName, object propertyValue)
	{
		if (!_FormattingEnabled)
		{
			if (data == null)
			{
				return string.Empty;
			}
			if (propertyValue == null)
			{
				return "";
			}
			return Convert.ToString(propertyValue, CultureInfo.CurrentCulture);
		}
		DataConvertEventArgs dataConvertEventArgs = new DataConvertEventArgs(propertyValue, typeof(string), data, fieldName);
		OnFormat(dataConvertEventArgs);
		if (((ConvertEventArgs)dataConvertEventArgs).Value != data && ((ConvertEventArgs)dataConvertEventArgs).Value is string)
		{
			return (string)((ConvertEventArgs)dataConvertEventArgs).Value;
		}
		if (stringTypeConverter == null)
		{
			stringTypeConverter = TypeDescriptor.GetConverter(typeof(string));
		}
		try
		{
			return (string)FormatHelper.FormatObject(propertyValue, typeof(string), generator.GetFieldConverter(fieldName), stringTypeConverter, _FormatString, _FormatInfo, null, DBNull.Value);
		}
		catch (Exception ex)
		{
			if (ex is SecurityException || IsCriticalException(ex))
			{
				throw;
			}
			return (propertyValue != null) ? Convert.ToString(data, CultureInfo.CurrentCulture) : "";
		}
	}

	private static bool IsCriticalException(Exception ex)
	{
		if (!(ex is NullReferenceException) && !(ex is StackOverflowException) && !(ex is OutOfMemoryException) && !(ex is ThreadAbortException) && !(ex is IndexOutOfRangeException))
		{
			return ex is AccessViolationException;
		}
		return true;
	}

	private object GetPropertyValue(ItemVisualGenerator generator, object data, string fieldName)
	{
		if (data != null && fieldName.Length > 0)
		{
			try
			{
				PropertyDescriptor propertyDescriptor = ((generator.DataManager == null) ? TypeDescriptor.GetProperties(data).Find(fieldName, ignoreCase: true) : ((BindingManagerBase)generator.DataManager).GetItemProperties().Find(fieldName, ignoreCase: true));
				if (propertyDescriptor != null)
				{
					data = propertyDescriptor.GetValue(data);
				}
			}
			catch
			{
			}
		}
		if (data == DBNull.Value && _NullValue != null)
		{
			return _NullValue;
		}
		return data;
	}

	protected virtual void OnPropertyNameChanged(string oldValue, string newValue)
	{
		_PropertyPath = newValue.Split(new char[1] { '.' });
		OnPropertyChanged(new PropertyChangedEventArgs("PropertyName"));
	}

	protected virtual void OnDataMemberChanged(string oldValue, string newValue)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (!string.IsNullOrEmpty(newValue))
		{
			_BindingMemberInfo = new BindingMemberInfo(newValue);
		}
		else
		{
			_BindingMemberInfo = default(BindingMemberInfo);
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
		this.PropertyChanged?.Invoke(this, e);
	}
}
