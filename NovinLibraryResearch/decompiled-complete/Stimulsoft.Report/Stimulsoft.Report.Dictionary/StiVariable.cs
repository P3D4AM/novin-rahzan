using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Globalization;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiVariableConverter))]
[StiSerializable]
public class StiVariable : StiExpression, IStiName, IStiAlias, IStiInherited, IStiPropertyGridObject, IStiAppVariable, IStiAppDataCell, IStiAppCell, IStiAppAlias
{
	public enum Order
	{
		Name = 100,
		Alias = 200,
		Category = 300
	}

	private bool readOnly;

	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiVariable;

	[Browsable(false)]
	public string PropName => Name;

	[Browsable(false)]
	[DefaultValue(false)]
	public bool Inherited { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[ParenthesizePropertyName(true)]
	[StiOrder(100)]
	[Description("Gets or sets a name of the variable.")]
	public string Name { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[ParenthesizePropertyName(true)]
	[StiOrder(200)]
	[Description("Gets or sets an alias of the variable.")]
	public string Alias { get; set; }

	[Browsable(false)]
	public override bool ApplyFormat => false;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a dialog info of the variable.")]
	public StiDialogInfo DialogInfo { get; set; } = new StiDialogInfo();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiTypeConverter))]
	[Editor("Stimulsoft.Base.Design.StiTypeEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Browsable(false)]
	[Description("Gets or sets a type of the variable.")]
	public Type Type { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Browsable(false)]
	[Description("Gets or sets a value which indicates that this variable is read only.")]
	public bool ReadOnly
	{
		get
		{
			return readOnly;
		}
		set
		{
			readOnly = value;
			if (value)
			{
				RequestFromUser = false;
			}
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Browsable(false)]
	[Description("Gets or sets a request from user value which indicates that this variable will be requested from user.")]
	public bool RequestFromUser { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Browsable(false)]
	[Description("Gets or sets a value which allows using this variable as a SQL parameter in the query.")]
	public bool AllowUseAsSqlParameter { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a category of the variable.")]
	public string Category { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a description of the variable.")]
	public string Description { get; set; }

	[Browsable(false)]
	public bool IsCategory => string.IsNullOrEmpty(Name);

	[Browsable(false)]
	public object ValueObject
	{
		get
		{
			if (InitBy != StiVariableInitBy.Expression)
			{
				return GetValue(Value, Type);
			}
			return Value;
		}
		set
		{
			if (InitBy == StiVariableInitBy.Expression)
			{
				Value = value as string;
			}
			if (value == null)
			{
				Value = string.Empty;
			}
			else
			{
				SetValue(value);
			}
		}
	}

	[Browsable(false)]
	public string InitByExpressionFrom
	{
		get
		{
			if (!StiTypeFinder.FindType(Type, typeof(Range)))
			{
				return null;
			}
			string[] rangeValues = GetRangeValues();
			if (rangeValues != null)
			{
				return rangeValues[0];
			}
			return string.Empty;
		}
		set
		{
			if (StiTypeFinder.FindType(Type, typeof(Range)))
			{
				string[] rangeValues = GetRangeValues();
				string text = string.Empty;
				if (rangeValues != null)
				{
					text = rangeValues[1];
				}
				Value = value + "<<|>>" + text;
			}
		}
	}

	[Browsable(false)]
	public string InitByExpressionTo
	{
		get
		{
			if (!StiTypeFinder.FindType(Type, typeof(Range)))
			{
				return null;
			}
			string[] rangeValues = GetRangeValues();
			if (rangeValues != null)
			{
				return rangeValues[1];
			}
			return string.Empty;
		}
		set
		{
			if (StiTypeFinder.FindType(Type, typeof(Range)))
			{
				string[] rangeValues = GetRangeValues();
				string text = string.Empty;
				if (rangeValues != null)
				{
					text = rangeValues[0];
				}
				Value = text + "<<|>>" + value;
			}
		}
	}

	[Browsable(false)]
	public override string Value
	{
		get
		{
			if (InitBy == StiVariableInitBy.Expression)
			{
				return base.Value;
			}
			if (!string.IsNullOrEmpty(base.Value))
			{
				if (Type == typeof(DateTime))
				{
					DateTime dateTimeFromValue = GetDateTimeFromValue(base.Value);
					CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
					try
					{
						Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
						return dateTimeFromValue.ToString();
					}
					finally
					{
						Thread.CurrentThread.CurrentCulture = currentCulture;
					}
				}
				if (Type == typeof(DateTime?))
				{
					DateTime dateTimeFromValue2 = GetDateTimeFromValue(base.Value);
					CultureInfo currentCulture2 = Thread.CurrentThread.CurrentCulture;
					try
					{
						Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
						return dateTimeFromValue2.ToString();
					}
					finally
					{
						Thread.CurrentThread.CurrentCulture = currentCulture2;
					}
				}
			}
			if (Type == typeof(DateTime))
			{
				return null;
			}
			if (Type == typeof(DateTime?))
			{
				return null;
			}
			return base.Value;
		}
		set
		{
			if (InitBy == StiVariableInitBy.Expression)
			{
				base.Value = value;
				return;
			}
			if (value != null && value != "null" && value.Length > 0)
			{
				if (Type == typeof(DateTime))
				{
					try
					{
						CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
						try
						{
							Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
							DateTime value2 = DateTime.Parse(value);
							base.Value = GetValueFromDateTime(value2);
						}
						finally
						{
							Thread.CurrentThread.CurrentCulture = currentCulture;
						}
					}
					catch
					{
						throw new ArgumentException("Variable value '" + value + "' is wrong DateTime value");
					}
				}
				else if (Type == typeof(DateTime?))
				{
					try
					{
						CultureInfo currentCulture2 = Thread.CurrentThread.CurrentCulture;
						try
						{
							Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
							DateTime value3 = DateTime.Parse(value);
							base.Value = GetValueFromDateTime(value3);
						}
						finally
						{
							Thread.CurrentThread.CurrentCulture = currentCulture2;
						}
					}
					catch
					{
						throw new ArgumentException("Variable value '" + value + "' is wrong DateTime value");
					}
				}
			}
			base.Value = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[ParenthesizePropertyName(true)]
	[DefaultValue(false)]
	[Description("Gets or sets a value which indicates that this variable will be have defalt value based on calculation of expression which specified in Value property.")]
	[Obsolete("'Function' property is obsolete. Please use 'InitBy' property instead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool Function
	{
		get
		{
			return InitBy == StiVariableInitBy.Expression;
		}
		set
		{
			InitBy = (value ? StiVariableInitBy.Expression : StiVariableInitBy.Value);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[DefaultValue(StiVariableInitBy.Value)]
	[Description("Gets or sets a value which indicates that this variable will be have defalt value based on calculation of expression which specified in Value property or based on default value.")]
	[Browsable(false)]
	public StiVariableInitBy InitBy { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[DefaultValue(StiSelectionMode.FromVariable)]
	[Browsable(false)]
	public StiSelectionMode Selection { get; set; }

	[DefaultValue(null)]
	[StiSerializable]
	[Browsable(false)]
	public string Key { get; set; }

	private string ConvertTypeToJsonString(Type type)
	{
		string text = type.FullName;
		if (text.StartsWith("System.Nullable`1"))
		{
			text = text[..text.IndexOf(",")].Replace("[[", "[") + "]";
		}
		return text;
	}

	private Type ConvertJsonStringToType(string text)
	{
		if (text.StartsWith("System.Nullable`1") && !text.Contains(", mscorlib,"))
		{
			text = text.Substring(0, text.Length - 1) + ", mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]";
			text = text.Replace("[", "[[");
		}
		return StiTypeFinder.GetType(text);
	}

	public JObject SaveToJsonObjectEx()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Value", Value);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Name", Name);
		if (DialogInfo != null)
		{
			JsonExtensions.AddPropertyJObject(val, "DialogInfo", DialogInfo.SaveToJsonObject());
		}
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Alias", Alias);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Type", ConvertTypeToJsonString(Type));
		JsonExtensions.AddPropertyBool(val, "ReadOnly", ReadOnly, false, false);
		JsonExtensions.AddPropertyBool(val, "RequestFromUser", RequestFromUser, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Category", Category);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Description", Description);
		JsonExtensions.AddPropertyEnum(val, "InitBy", (Enum)InitBy, (Enum)StiVariableInitBy.Value);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Key", Key);
		JsonExtensions.AddPropertyBool(val, "AllowUseAsSqlParameter", AllowUseAsSqlParameter, false, false);
		JsonExtensions.AddPropertyEnum(val, "SelectionMode", (Enum)Selection, (Enum)StiSelectionMode.FromVariable);
		return val;
	}

	public void LoadFromJsonObjectEx(JObject jObject, StiReport report)
	{
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Name":
				Name = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "DialogInfo":
				DialogInfo.LoadFromJsonObject((JObject)item.Value, report);
				break;
			case "Alias":
				Alias = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Type":
				Type = ConvertJsonStringToType(StiJsonReportObjectHelper.DeserializeString(item));
				break;
			case "ReadOnly":
				readOnly = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "RequestFromUser":
				RequestFromUser = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Category":
				Category = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Description":
				Description = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "InitBy":
				InitBy = StiJsonReportObjectHelper.DeserializeEnum<StiVariableInitBy>(item);
				break;
			case "Key":
				Key = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "AllowUseAsSqlParameter":
				AllowUseAsSqlParameter = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "SelectionMode":
				Selection = StiJsonReportObjectHelper.DeserializeEnum<StiSelectionMode>(item);
				break;
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[4]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias(),
			propertiesHelper.Category(),
			propertiesHelper.Description()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	object IStiAppVariable.GetValue()
	{
		return ValueObject;
	}

	string IStiAppDataCell.GetName()
	{
		return Name;
	}

	string IStiAppCell.GetKey()
	{
		Key = StiKeyHelper.GetOrGeneratedKey(Key);
		return Key;
	}

	void IStiAppCell.SetKey(string key)
	{
		Key = key;
	}

	string IStiAppAlias.GetAlias()
	{
		return Alias;
	}

	private string[] GetRangeValues()
	{
		string value = Value;
		if (string.IsNullOrEmpty(value))
		{
			return null;
		}
		if (!value.Contains("<<|>>"))
		{
			return null;
		}
		string[] array = value.Split(new string[1] { "<<|>>" }, StringSplitOptions.None);
		if (array.Length != 2)
		{
			return null;
		}
		return array;
	}

	public static object GetValue(string str, Type type)
	{
		if (string.IsNullOrEmpty(str))
		{
			if (type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) || type == typeof(byte) || type == typeof(sbyte) || type == typeof(short) || type == typeof(ushort))
			{
				return 0;
			}
			if (type == typeof(string))
			{
				return string.Empty;
			}
			if (type == typeof(bool))
			{
				return false;
			}
			if (type == typeof(char))
			{
				return ' ';
			}
			if (type == typeof(double))
			{
				return 0.0;
			}
			if (type == typeof(float))
			{
				return 0f;
			}
			if (type == typeof(decimal))
			{
				return 0m;
			}
			if (type == typeof(DateTime))
			{
				return null;
			}
			if (type == typeof(TimeSpan))
			{
				return null;
			}
			if (StiTypeFinder.FindType(type, typeof(Range)))
			{
				return Activator.CreateInstance(type) as Range;
			}
			if (StiTypeFinder.FindType(type, typeof(Image)))
			{
				return null;
			}
			if (type == typeof(int?) || type == typeof(uint?) || type == typeof(long?) || type == typeof(ulong?) || type == typeof(byte?) || type == typeof(sbyte?) || type == typeof(short?) || type == typeof(ushort?))
			{
				return 0;
			}
			if (type == typeof(bool?))
			{
				return false;
			}
			if (type == typeof(char?))
			{
				return ' ';
			}
			if (type == typeof(double?))
			{
				return 0.0;
			}
			if (type == typeof(float?))
			{
				return 0f;
			}
			if (type == typeof(decimal?))
			{
				return 0m;
			}
			if (type == typeof(DateTime?))
			{
				return null;
			}
			if (type == typeof(TimeSpan?))
			{
				return null;
			}
		}
		if (type == typeof(int))
		{
			return StiValueHelper.TryToInt((object)str);
		}
		if (type == typeof(uint))
		{
			return StiValueHelper.TryToUInt((object)str);
		}
		if (type == typeof(long))
		{
			return StiValueHelper.TryToLong((object)str);
		}
		if (type == typeof(ulong))
		{
			return StiValueHelper.TryToULong((object)str);
		}
		if (type == typeof(byte))
		{
			return (byte)StiValueHelper.TryToInt((object)str);
		}
		if (type == typeof(sbyte))
		{
			return (sbyte)StiValueHelper.TryToInt((object)str);
		}
		if (type == typeof(short))
		{
			return (short)StiValueHelper.TryToInt((object)str);
		}
		if (type == typeof(ushort))
		{
			return (ushort)StiValueHelper.TryToInt((object)str);
		}
		if (type == typeof(string))
		{
			return str;
		}
		if (type == typeof(bool))
		{
			return str.ToLower(CultureInfo.InvariantCulture) == "true";
		}
		if (type == typeof(char))
		{
			return str[0];
		}
		if (type == typeof(double))
		{
			return StiValueHelper.TryToDouble((object)str);
		}
		if (type == typeof(float))
		{
			return StiValueHelper.TryToFloat((object)str);
		}
		if (type == typeof(decimal))
		{
			return StiValueHelper.TryToDecimal((object)str);
		}
		if (type == typeof(Guid))
		{
			return string.IsNullOrEmpty(str) ? default(Guid) : new Guid(str);
		}
		if (type == typeof(DateTime))
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				return DateTime.Parse(str);
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}
		if (type == typeof(TimeSpan))
		{
			CultureInfo currentCulture2 = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				return TimeSpan.Parse(str);
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = currentCulture2;
			}
		}
		if (type == typeof(int?))
		{
			return (str.ToLowerInvariant() == "null") ? null : StiValueHelper.TryToNullableInt((object)str);
		}
		if (type == typeof(uint?))
		{
			return (str.ToLowerInvariant() == "null") ? null : StiValueHelper.TryToNullableUInt((object)str);
		}
		if (type == typeof(long?))
		{
			return (str.ToLowerInvariant() == "null") ? null : StiValueHelper.TryToNullableLong((object)str);
		}
		if (type == typeof(ulong?))
		{
			return (str.ToLowerInvariant() == "null") ? null : StiValueHelper.TryToNullableULong((object)str);
		}
		if (type == typeof(byte?))
		{
			return (str.ToLowerInvariant() == "null") ? null : ((byte?)StiValueHelper.TryToNullableInt((object)str));
		}
		if (type == typeof(sbyte?))
		{
			return (str.ToLowerInvariant() == "null") ? null : ((sbyte?)StiValueHelper.TryToNullableInt((object)str));
		}
		if (type == typeof(short?))
		{
			return (str.ToLowerInvariant() == "null") ? null : ((short?)StiValueHelper.TryToNullableInt((object)str));
		}
		if (type == typeof(ushort?))
		{
			return (str.ToLowerInvariant() == "null") ? null : ((ushort?)StiValueHelper.TryToNullableInt((object)str));
		}
		if (type == typeof(bool?))
		{
			return (str.ToLowerInvariant() == "null") ? null : new bool?(str.ToLower(CultureInfo.InvariantCulture) == "true");
		}
		if (type == typeof(char?))
		{
			return (str.ToLowerInvariant() == "null") ? null : new char?(str[0]);
		}
		if (type == typeof(double?))
		{
			return (str.ToLowerInvariant() == "null") ? null : StiValueHelper.TryToNullableDouble((object)str);
		}
		if (type == typeof(float?))
		{
			return (str.ToLowerInvariant() == "null") ? null : StiValueHelper.TryToNullableFloat((object)str);
		}
		if (type == typeof(decimal?))
		{
			return (str.ToLowerInvariant() == "null") ? null : StiValueHelper.TryToNullableDecimal((object)str);
		}
		if (type == typeof(DateTime?))
		{
			if (str.ToLowerInvariant() == "null")
			{
				return null;
			}
			CultureInfo currentCulture3 = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				return DateTime.Parse(str);
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = currentCulture3;
			}
		}
		if (type == typeof(TimeSpan?))
		{
			if (str.ToLowerInvariant() == "null")
			{
				return null;
			}
			CultureInfo currentCulture4 = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				return TimeSpan.Parse(str);
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = currentCulture4;
			}
		}
		if (type == typeof(Metafile))
		{
			return StiMetafileConverter.StringToMetafile(str);
		}
		if (StiTypeFinder.FindType(type, typeof(Image)))
		{
			return StiImageConverter.StringToImage(str);
		}
		if (StiTypeFinder.FindType(type, typeof(Range)))
		{
			return RangeConverter.StringToRange(str);
		}
		return null;
	}

	private void SetValue(object value)
	{
		if (value is string)
		{
			Value = value as string;
			return;
		}
		if (value is char)
		{
			Value = value.ToString();
			return;
		}
		if (value is Guid)
		{
			Value = value.ToString();
			return;
		}
		if (value is bool)
		{
			Value = ((bool)value).ToString().ToLower(CultureInfo.InvariantCulture);
			return;
		}
		if (value is Metafile)
		{
			Value = StiMetafileConverter.MetafileToString((Metafile)((value is Metafile) ? value : null));
			return;
		}
		if (value is Image)
		{
			Value = StiImageConverter.ImageToString((Image)((value is Image) ? value : null), false);
			return;
		}
		if (value is byte[])
		{
			Value = Convert.ToBase64String(value as byte[]);
			return;
		}
		if (value is Range)
		{
			Value = RangeConverter.RangeToString(value as Range);
			return;
		}
		if (value is TimeSpan)
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				Value = ((TimeSpan)value).ToString();
				return;
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}
		if (value is DateTime)
		{
			CultureInfo currentCulture2 = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				Value = ((DateTime)value).ToString();
				return;
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = currentCulture2;
			}
		}
		if (!(Type == typeof(int)) && !(Type == typeof(uint)) && !(Type == typeof(long)) && !(Type == typeof(ulong)) && !(Type == typeof(byte)) && !(Type == typeof(sbyte)) && !(Type == typeof(short)) && !(Type == typeof(ushort)) && !(Type == typeof(double)) && !(Type == typeof(float)) && !(Type == typeof(decimal)) && !(Type == typeof(int?)) && !(Type == typeof(uint?)) && !(Type == typeof(long?)) && !(Type == typeof(ulong?)) && !(Type == typeof(byte?)) && !(Type == typeof(sbyte?)) && !(Type == typeof(short?)) && !(Type == typeof(ushort?)) && !(Type == typeof(double?)) && !(Type == typeof(float?)) && !(Type == typeof(decimal?)))
		{
			return;
		}
		CultureInfo currentCulture3 = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			Value = value.ToString();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture3;
		}
	}

	public string GetNativeValue()
	{
		return base.Value;
	}

	public static DateTime GetDateTimeFromValue(string value)
	{
		if (value == "null")
		{
			return DateTime.Now;
		}
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			return DateTime.Parse(value);
		}
		catch
		{
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
		return DateTime.Now;
	}

	internal static string GetValueFromDateTime(DateTime value)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			return value.ToString();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	private static string ConvertValue(object value)
	{
		if (value == null)
		{
			return string.Empty;
		}
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			if (value is DateTime dateTime)
			{
				return dateTime.ToString();
			}
			if (value is Metafile)
			{
				return StiMetafileConverter.MetafileToString((Metafile)((value is Metafile) ? value : null));
			}
			if (value is Image)
			{
				return StiImageConverter.ImageToString((Image)((value is Image) ? value : null), false);
			}
			if (value is byte[])
			{
				return Convert.ToBase64String(value as byte[]);
			}
			if (value is Range)
			{
				return RangeConverter.RangeToString(value as Range);
			}
			if (value is bool)
			{
				return ((bool)value) ? "true" : "false";
			}
			return value.ToString();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public static string GetExpressionString(StiVariable variable)
	{
		if (variable == null)
		{
			return "";
		}
		return variable.Name;
	}

	public override string ToString()
	{
		return ToString(onlyAlias: false);
	}

	public string ToString(bool onlyAlias)
	{
		if (IsCategory)
		{
			return Category;
		}
		if (onlyAlias && !string.IsNullOrWhiteSpace(Alias))
		{
			return Alias;
		}
		if (Name == Alias || string.IsNullOrWhiteSpace(Alias))
		{
			return Name;
		}
		return Name + " [" + Alias + "]";
	}

	public object Eval(StiReport report)
	{
		return StiParser.PrepareVariableValue(this, report, null, fillItems: true);
	}

	public StiVariable()
		: this(string.Empty, string.Empty, typeof(string), string.Empty, readOnly: false)
	{
	}

	public StiVariable(string category)
		: this(category, string.Empty, typeof(string), string.Empty, readOnly: false)
	{
	}

	public StiVariable(string name, Type type)
		: this(string.Empty, name, name, type, string.Empty, readOnly: false)
	{
	}

	public StiVariable(string category, string name, Type type)
		: this(category, name, name, type, string.Empty, readOnly: false)
	{
	}

	public StiVariable(string name, object value)
		: this(string.Empty, name, value)
	{
	}

	public StiVariable(string category, string name, object value)
		: this(category, name, name, value, readOnly: false)
	{
	}

	public StiVariable(string category, string name, string alias, object value)
		: this(category, name, alias, value, readOnly: false)
	{
	}

	public StiVariable(string category, string name, string alias, object value, bool readOnly)
		: this(category, name, alias, (value == null) ? typeof(object) : value.GetType(), null, readOnly)
	{
		if (Type == typeof(DateTime) && value is DateTime)
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				Value = ((DateTime)value).ToString();
				return;
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}
		Value = ConvertValue(value);
	}

	public StiVariable(string category, string name, Type type, string value, bool readOnly)
		: this(category, name, name, type, value, readOnly)
	{
	}

	public StiVariable(string category, string name, string alias, Type type, string value, bool readOnly)
		: this(category, name, alias, type, value, readOnly, StiVariableInitBy.Value)
	{
	}

	public StiVariable(string category, string name, string alias, string description, Type type, string value, bool readOnly)
		: this(category, name, alias, description, type, value, readOnly, StiVariableInitBy.Value, requestFromUser: false)
	{
	}

	[Obsolete("This constructor is obsolete! It use 'function' argument. Please use instead it constructor with 'initBy' argument!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiVariable(string category, string name, string alias, Type type, string value, bool readOnly, bool function)
		: this(category, name, alias, string.Empty, type, value, readOnly, function, requestFromUser: false)
	{
	}

	[Obsolete("This constructor is obsolete! It use 'function' argument. Please use instead it constructor with 'initBy' argument!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiVariable(string category, string name, string alias, string description, Type type, string value, bool readOnly, bool function)
		: this(category, name, alias, description, type, value, readOnly, function, requestFromUser: false)
	{
	}

	[Obsolete("This constructor is obsolete! It use 'function' argument. Please use instead it constructor with 'initBy' argument!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiVariable(string category, string name, string alias, string description, Type type, string value, bool readOnly, bool function, bool requestFromUser)
	{
		Category = category;
		Name = name;
		Alias = alias;
		Description = description;
		Type = type;
		this.readOnly = readOnly;
		Function = function;
		Value = value;
		RequestFromUser = requestFromUser;
	}

	public StiVariable(string category, string name, string alias, Type type, string value, bool readOnly, StiVariableInitBy initBy)
		: this(category, name, alias, string.Empty, type, value, readOnly, initBy, requestFromUser: false)
	{
	}

	public StiVariable(string category, string name, string alias, string description, Type type, string value, bool readOnly, StiVariableInitBy initBy)
		: this(category, name, alias, description, type, value, readOnly, initBy, requestFromUser: false)
	{
	}

	public StiVariable(string category, string name, string alias, string description, Type type, string value, bool readOnly, StiVariableInitBy initBy, bool requestFromUser)
	{
		Category = category;
		Name = name;
		Alias = alias;
		Description = description;
		Type = type;
		this.readOnly = readOnly;
		InitBy = initBy;
		Value = value;
		RequestFromUser = requestFromUser;
	}

	public StiVariable(string category, string name, string alias, string description, Type type, string value, bool readOnly, StiVariableInitBy initBy, bool requestFromUser, StiDialogInfo dialogInfo)
	{
		Category = category;
		Name = name;
		Alias = alias;
		Description = description;
		Type = type;
		this.readOnly = readOnly;
		InitBy = initBy;
		Value = value;
		RequestFromUser = requestFromUser;
		DialogInfo = dialogInfo;
	}

	public StiVariable(string category, string name, string alias, string description, Type type, string value, bool readOnly, StiVariableInitBy initBy, bool requestFromUser, StiDialogInfo dialogInfo, string key)
	{
		Category = category;
		Name = name;
		Alias = alias;
		Description = description;
		Type = type;
		this.readOnly = readOnly;
		InitBy = initBy;
		Value = value;
		RequestFromUser = requestFromUser;
		DialogInfo = dialogInfo;
		Key = key;
	}

	public StiVariable(string category, string name, string alias, string description, Type type, string value, bool readOnly, StiVariableInitBy initBy, bool requestFromUser, StiDialogInfo dialogInfo, string key, bool allowUseAsSqlParameter)
	{
		Category = category;
		Name = name;
		Alias = alias;
		Description = description;
		Type = type;
		this.readOnly = readOnly;
		InitBy = initBy;
		Value = value;
		RequestFromUser = requestFromUser;
		DialogInfo = dialogInfo;
		Key = key;
		AllowUseAsSqlParameter = allowUseAsSqlParameter;
	}

	public StiVariable(string category, string name, string alias, string description, Type type, string value, bool readOnly, StiVariableInitBy initBy, bool requestFromUser, StiDialogInfo dialogInfo, string key, bool allowUseAsSqlParameter, StiSelectionMode selection)
	{
		Category = category;
		Name = name;
		Alias = alias;
		Description = description;
		Type = type;
		this.readOnly = readOnly;
		InitBy = initBy;
		Value = value;
		RequestFromUser = requestFromUser;
		DialogInfo = dialogInfo;
		Key = key;
		AllowUseAsSqlParameter = allowUseAsSqlParameter;
		Selection = selection;
	}
}
