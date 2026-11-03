using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiDialogInfoConverter))]
public class StiDialogInfo
{
	internal string jsonLoadedBindingVariableName;

	[StiSerializable]
	[DefaultValue(StiDateTimeType.Date)]
	public StiDateTimeType DateTimeType { get; set; }

	[StiSerializable]
	[DefaultValue(StiVariableSortDirection.Asc)]
	public StiVariableSortDirection SortDirection { get; set; } = StiVariableSortDirection.Asc;


	[StiSerializable]
	[DefaultValue(StiVariableSortField.Label)]
	public StiVariableSortField SortField { get; set; } = StiVariableSortField.Label;


	[StiSerializable]
	[DefaultValue(StiItemsInitializationType.Items)]
	public StiItemsInitializationType ItemsInitializationType { get; set; }

	[StiSerializable]
	[DefaultValue("")]
	public string KeysColumn { get; set; } = string.Empty;


	[StiSerializable]
	[DefaultValue("")]
	public string ValuesColumn { get; set; } = string.Empty;


	[StiSerializable]
	[DefaultValue("")]
	public string CheckedColumn { get; set; } = string.Empty;


	[StiSerializable]
	[DefaultValue("")]
	public StiVariable BindingVariable { get; set; }

	[StiSerializable]
	[DefaultValue("")]
	public string BindingValuesColumn { get; set; } = string.Empty;


	[StiSerializable]
	[DefaultValue("")]
	public string Mask { get; set; } = string.Empty;


	[StiSerializable]
	[DefaultValue(true)]
	public bool AllowUserValues { get; set; } = true;


	[StiSerializable]
	[DefaultValue(false)]
	public bool BindingValue { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public string[] Keys { get; set; } = new string[0];


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public string[] Values { get; set; } = new string[0];


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public bool[] CheckedStates { get; set; } = new bool[0];


	public List<object>[] ValuesBindingList { get; set; } = new List<object>[0];


	public bool IsDefault
	{
		get
		{
			if (AllowUserValues && DateTimeType == StiDateTimeType.Date && SortDirection == StiVariableSortDirection.Asc && SortField == StiVariableSortField.Label && BindingVariable == null && (Keys == null || Keys.Length == 0) && (Values == null || Values.Length == 0) && (CheckedStates == null || CheckedStates.Length == 0) && string.IsNullOrEmpty(Mask) && string.IsNullOrEmpty(KeysColumn) && string.IsNullOrEmpty(ValuesColumn) && string.IsNullOrEmpty(CheckedColumn))
			{
				return string.IsNullOrEmpty(BindingValuesColumn);
			}
			return false;
		}
	}

	public JObject SaveToJsonObject()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyEnum(val, "DateTimeType", (Enum)DateTimeType, (Enum)StiDateTimeType.Date);
		JsonExtensions.AddPropertyEnum(val, "SortDirection", (Enum)SortDirection, (Enum)StiVariableSortDirection.Asc);
		JsonExtensions.AddPropertyEnum(val, "SortField", (Enum)SortField, (Enum)StiVariableSortField.Label);
		JsonExtensions.AddPropertyEnum(val, "ItemsInitializationType", (Enum)ItemsInitializationType, (Enum)StiItemsInitializationType.Items);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "KeysColumn", KeysColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValuesColumn", ValuesColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "CheckedColumn", CheckedColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "BindingValuesColumn", BindingValuesColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Mask", Mask);
		JsonExtensions.AddPropertyBool(val, "AllowUserValues", AllowUserValues, true, false);
		JsonExtensions.AddPropertyBool(val, "BindingValue", BindingValue, false, false);
		if (Keys != null)
		{
			JsonExtensions.AddPropertyStringArray(val, "Keys", Keys);
		}
		if (Values != null)
		{
			JsonExtensions.AddPropertyStringArray(val, "Values", Values);
		}
		if (CheckedStates != null)
		{
			JsonExtensions.AddPropertyBoolArray(val, "CheckedStates", CheckedStates, true);
		}
		if (BindingVariable != null)
		{
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "BindingVariable", BindingVariable.Name);
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject, StiReport report)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "DateTimeType":
				DateTimeType = StiJsonReportObjectHelper.DeserializeEnum<StiDateTimeType>(item);
				break;
			case "SortDirection":
				SortDirection = StiJsonReportObjectHelper.DeserializeEnum<StiVariableSortDirection>(item);
				break;
			case "SortField":
				SortField = StiJsonReportObjectHelper.DeserializeEnum<StiVariableSortField>(item);
				break;
			case "ItemsInitializationType":
				ItemsInitializationType = StiJsonReportObjectHelper.DeserializeEnum<StiItemsInitializationType>(item);
				break;
			case "KeysColumn":
				KeysColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ValuesColumn":
				ValuesColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "CheckedColumn":
				CheckedColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "BindingValuesColumn":
				BindingValuesColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Mask":
				Mask = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "AllowUserValues":
				AllowUserValues = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "BindingValue":
				BindingValue = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Keys":
				Keys = StiJsonReportObjectHelper.DeserializeStringArray(item);
				break;
			case "Values":
				Values = StiJsonReportObjectHelper.DeserializeStringArray(item);
				break;
			case "CheckedStates":
				CheckedStates = StiJsonReportObjectHelper.DeserializeBoolArray(item);
				break;
			case "BindingVariable":
				jsonLoadedBindingVariableName = StiJsonReportObjectHelper.DeserializeString(item);
				report.jsonLoaderHelper.DialogInfo.Add(this);
				break;
			}
		}
	}

	public static string Convert(object value)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			return (value == null) ? string.Empty : value.ToString();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public List<StiDialogInfoItem> GetDialogInfoItems(Type type, CultureInfo culture = null, string[] keys = null)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = culture ?? new CultureInfo("en-US", useUserOverride: false);
			List<StiDialogInfoItem> list = new List<StiDialogInfoItem>();
			if (keys == null)
			{
				keys = Keys;
			}
			if (keys == null)
			{
				return list;
			}
			int num = 0;
			string[] array = keys;
			foreach (string text in array)
			{
				StiDialogInfoItem stiDialogInfoItem = null;
				object keyObject = null;
				object keyObjectTo = null;
				try
				{
					if (text.StartsWith("{") && text.EndsWith("}"))
					{
						if (StiTypeFinder.FindType(type, typeof(Range)))
						{
							if (text.Contains("<<|>>"))
							{
								string text2 = text.Substring(1, text.Length - 2);
								string[] array2 = text2.Split(new string[1] { "<<|>>" }, StringSplitOptions.None);
								if (array2.Length == 2)
								{
									keyObject = array2[0];
									keyObjectTo = array2[0];
								}
								else
								{
									keyObject = text.Substring(1, text.Length - 2);
								}
							}
							else
							{
								keyObject = text.Substring(1, text.Length - 2);
							}
							stiDialogInfoItem = new StiExpressionRangeDialogInfoItem();
						}
						else
						{
							keyObject = text.Substring(1, text.Length - 2);
							stiDialogInfoItem = new StiExpressionDialogInfoItem();
						}
					}
					else if (type == typeof(sbyte) || type == typeof(byte) || type == typeof(short) || type == typeof(ushort) || type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) || type == typeof(sbyte?) || type == typeof(byte?) || type == typeof(short?) || type == typeof(ushort?) || type == typeof(int?) || type == typeof(uint?) || type == typeof(long?) || type == typeof(ulong?) || type == typeof(ByteList) || type == typeof(ShortList) || type == typeof(IntList) || type == typeof(LongList))
					{
						keyObject = StiValueHelper.TryToLong((object)text);
						stiDialogInfoItem = new StiLongDialogInfoItem();
					}
					else if (type == typeof(string) || type == typeof(StringList))
					{
						keyObject = text;
						stiDialogInfoItem = new StiStringDialogInfoItem();
					}
					else if (type == typeof(double) || type == typeof(float) || type == typeof(double?) || type == typeof(float?) || type == typeof(DoubleList) || type == typeof(FloatList))
					{
						keyObject = StiValueHelper.TryToDouble((object)text);
						stiDialogInfoItem = new StiDoubleDialogInfoItem();
					}
					else if (type == typeof(decimal) || type == typeof(decimal?) || type == typeof(DecimalList))
					{
						keyObject = StiValueHelper.TryToDecimal((object)text);
						stiDialogInfoItem = new StiDecimalDialogInfoItem();
					}
					else if (type == typeof(DateTime) || type == typeof(DateTime?) || type == typeof(DateTimeList))
					{
						keyObject = StiValueHelper.TryToDateTime((object)text);
						stiDialogInfoItem = new StiDateTimeDialogInfoItem();
					}
					else if (type == typeof(TimeSpan) || type == typeof(TimeSpan?) || type == typeof(TimeSpanList))
					{
						keyObject = StiValueHelper.TryToTimeSpan((object)text);
						stiDialogInfoItem = new StiTimeSpanDialogInfoItem();
					}
					else if (type == typeof(bool) || type == typeof(bool?) || type == typeof(BoolList))
					{
						keyObject = StiValueHelper.TryToBool((object)text);
						stiDialogInfoItem = new StiBoolDialogInfoItem();
					}
					else if (type == typeof(char) || type == typeof(char?) || type == typeof(CharList))
					{
						keyObject = (string.IsNullOrEmpty(text) ? ((object)' ') : ((object)StiValueHelper.TryToChar((object)text)));
						stiDialogInfoItem = new StiCharDialogInfoItem();
					}
					else if (type == typeof(Guid) || type == typeof(Guid?) || type == typeof(GuidList))
					{
						keyObject = StiValueHelper.TryToGuid((object)text);
						stiDialogInfoItem = new StiGuidDialogInfoItem();
					}
					else if (type == typeof(Image) || type == typeof(ImageList) || type == typeof(Bitmap))
					{
						keyObject = StiImageConverter.StringToImage(text);
						stiDialogInfoItem = new StiImageDialogInfoItem();
					}
					else if (type == typeof(ByteRange) || type == typeof(ShortRange) || type == typeof(IntRange) || type == typeof(LongRange))
					{
						Range range = RangeConverter.StringToRange(text);
						stiDialogInfoItem = new StiLongRangeDialogInfoItem();
						keyObject = StiConvert.ChangeType(range.FromObject, typeof(long));
						keyObjectTo = StiConvert.ChangeType(range.ToObject, typeof(long));
					}
					else if (type == typeof(StringRange))
					{
						Range range2 = RangeConverter.StringToRange(text);
						stiDialogInfoItem = new StiStringRangeDialogInfoItem();
						keyObject = range2.FromObject.ToString();
						keyObjectTo = range2.ToObject.ToString();
					}
					else if (type == typeof(DoubleRange) || type == typeof(FloatRange))
					{
						Range range3 = RangeConverter.StringToRange(text);
						stiDialogInfoItem = new StiDoubleRangeDialogInfoItem();
						keyObject = StiConvert.ChangeType(range3.FromObject, typeof(double));
						keyObjectTo = StiConvert.ChangeType(range3.ToObject, typeof(double));
					}
					else if (type == typeof(DecimalRange))
					{
						Range range4 = RangeConverter.StringToRange(text);
						stiDialogInfoItem = new StiDecimalRangeDialogInfoItem();
						keyObject = StiConvert.ChangeType(range4.FromObject, typeof(decimal));
						keyObjectTo = StiConvert.ChangeType(range4.ToObject, typeof(decimal));
					}
					else if (type == typeof(DateTimeRange))
					{
						Range range5 = RangeConverter.StringToRange(text);
						stiDialogInfoItem = new StiDateTimeRangeDialogInfoItem();
						keyObject = range5.FromObject;
						keyObjectTo = range5.ToObject;
					}
					else if (type == typeof(TimeSpanRange))
					{
						Range range6 = RangeConverter.StringToRange(text);
						stiDialogInfoItem = new StiTimeSpanRangeDialogInfoItem();
						keyObject = range6.FromObject;
						keyObjectTo = range6.ToObject;
					}
					else if (type == typeof(CharRange))
					{
						Range range7 = RangeConverter.StringToRange(text);
						stiDialogInfoItem = new StiCharRangeDialogInfoItem();
						keyObject = range7.FromObject;
						keyObjectTo = range7.ToObject;
					}
					else if (type == typeof(GuidRange))
					{
						Range range8 = RangeConverter.StringToRange(text);
						stiDialogInfoItem = new StiGuidRangeDialogInfoItem();
						keyObject = range8.FromObject;
						keyObjectTo = range8.ToObject;
					}
					stiDialogInfoItem.KeyObject = keyObject;
					stiDialogInfoItem.KeyObjectTo = keyObjectTo;
				}
				catch
				{
				}
				if (stiDialogInfoItem != null)
				{
					stiDialogInfoItem.Value = ((Values.Length > num && !string.IsNullOrEmpty(Values[num])) ? Values[num] : string.Empty);
					stiDialogInfoItem.Checked = CheckedStates == null || CheckedStates.Length <= num || CheckedStates[num];
					stiDialogInfoItem.ValueBinding = ((ValuesBindingList.Length > num) ? ValuesBindingList[num] : null);
					list.Add(stiDialogInfoItem);
				}
				num++;
			}
			return OrderBy(list);
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public List<StiDialogInfoItem> GetAndParseDialogInfoItems(Type type, CultureInfo culture = null, string[] keys = null, StiReport report = null)
	{
		List<StiDialogInfoItem> dialogInfoItems = GetDialogInfoItems(type, culture, keys);
		List<StiDialogInfoItem> list = new List<StiDialogInfoItem>();
		foreach (StiDialogInfoItem item in dialogInfoItems)
		{
			if (item is StiExpressionDialogInfoItem || item is StiExpressionRangeDialogInfoItem)
			{
				StiDialogInfoItem stiDialogInfoItem = GetDialogInfoItems(type, culture, new string[1] { "" }).FirstOrDefault();
				stiDialogInfoItem.Checked = item.Checked;
				stiDialogInfoItem.Label = item.Label;
				stiDialogInfoItem.KeyObject = StiConvert.ChangeType((object)StiReportParser.Parse(item.KeyObject as string, (IStiReport)(object)report, allowCache: false), type);
				if (item is StiExpressionRangeDialogInfoItem)
				{
					stiDialogInfoItem.KeyObjectTo = StiReportParser.Parse(item.KeyObjectTo as string, (IStiReport)(object)report, allowCache: false);
				}
				list.Add(stiDialogInfoItem);
			}
			else
			{
				list.Add(item);
			}
		}
		return list;
	}

	public List<StiDialogInfoItem> OrderBy(List<StiDialogInfoItem> items)
	{
		switch (SortField)
		{
		case StiVariableSortField.Key:
			if (SortDirection == StiVariableSortDirection.None)
			{
				return items;
			}
			if (SortDirection == StiVariableSortDirection.Asc)
			{
				return items.OrderBy((StiDialogInfoItem i) => i.KeyObject).ToList();
			}
			return items.OrderByDescending((StiDialogInfoItem i) => i.KeyObject).ToList();
		case StiVariableSortField.Label:
			if (SortDirection == StiVariableSortDirection.None)
			{
				return items;
			}
			if (SortDirection == StiVariableSortDirection.Asc)
			{
				return items.OrderBy((StiDialogInfoItem i) => i.Value).ToList();
			}
			return items.OrderByDescending((StiDialogInfoItem i) => i.Value).ToList();
		default:
			return items;
		}
	}

	public void SetDialogInfoItems(List<StiDialogInfoItem> items, Type type)
	{
		if (items == null || items.Count == 0)
		{
			Keys = null;
			Values = null;
			CheckedStates = null;
			return;
		}
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			Keys = new string[items.Count];
			Values = new string[items.Count];
			CheckedStates = new bool[items.Count];
			ValuesBindingList = new List<object>[items.Count];
			int num = 0;
			foreach (StiDialogInfoItem item in items)
			{
				object obj = null;
				try
				{
					if (item is StiExpressionDialogInfoItem)
					{
						obj = $"{{{item.KeyObject}}}";
					}
					else if (item is StiExpressionRangeDialogInfoItem)
					{
						obj = $"{{{item.KeyObject}<<|>>{item.KeyObjectTo}}}";
					}
					else if (item is StiLongDialogInfoItem || item is StiDoubleDialogInfoItem || item is StiDecimalDialogInfoItem || item is StiDateTimeDialogInfoItem || item is StiTimeSpanDialogInfoItem || item is StiBoolDialogInfoItem || item is StiCharDialogInfoItem || item is StiGuidDialogInfoItem || item is StiStringDialogInfoItem)
					{
						obj = item.KeyObject.ToString();
					}
					else if (item is StiImageDialogInfoItem)
					{
						object keyObject = item.KeyObject;
						obj = StiImageConverter.ImageToString((Image)((keyObject is Image) ? keyObject : null), false);
					}
					else if (item is StiLongRangeDialogInfoItem || item is StiDoubleRangeDialogInfoItem || item is StiDecimalRangeDialogInfoItem || item is StiDateTimeRangeDialogInfoItem || item is StiTimeSpanRangeDialogInfoItem || item is StiCharRangeDialogInfoItem || item is StiGuidRangeDialogInfoItem || item is StiStringRangeDialogInfoItem)
					{
						Range range = (Range)StiActivator.CreateObject(type);
						range.Parse(item.KeyObject.ToString(), item.KeyObjectTo.ToString());
						obj = RangeConverter.RangeToString(range);
					}
				}
				catch
				{
				}
				Keys[num] = ((obj == null) ? string.Empty : obj.ToString());
				Values[num] = item.Value;
				CheckedStates[num] = item.Checked;
				ValuesBindingList[num] = item.ValueBinding;
				num++;
			}
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public StiDialogInfo()
	{
	}

	public StiDialogInfo(StiDateTimeType type, string mask, bool allowUserValues, string[] keys, string[] values)
	{
		DateTimeType = type;
		Mask = mask;
		AllowUserValues = allowUserValues;
		Keys = keys;
		Values = values;
		ItemsInitializationType = StiItemsInitializationType.Items;
	}

	public StiDialogInfo(StiDateTimeType type, string mask, bool allowUserValues, string[] keys, string[] values, bool[] checkedStates)
	{
		DateTimeType = type;
		Mask = mask;
		AllowUserValues = allowUserValues;
		Keys = keys;
		Values = values;
		CheckedStates = checkedStates;
		ItemsInitializationType = StiItemsInitializationType.Items;
	}

	public StiDialogInfo(StiDateTimeType type, string mask, bool allowUserValues, StiVariableSortDirection sortDirection, string[] keys, string[] values, bool[] checkedStates)
	{
		DateTimeType = type;
		Mask = mask;
		AllowUserValues = allowUserValues;
		Keys = keys;
		Values = values;
		CheckedStates = checkedStates;
		ItemsInitializationType = StiItemsInitializationType.Items;
		SortDirection = sortDirection;
	}

	public StiDialogInfo(StiDateTimeType type, string mask, bool allowUserValues, string keysColumn, string valuesColumn)
	{
		DateTimeType = type;
		Mask = mask;
		AllowUserValues = allowUserValues;
		ItemsInitializationType = StiItemsInitializationType.Columns;
		KeysColumn = keysColumn;
		ValuesColumn = valuesColumn;
	}

	public StiDialogInfo(StiDateTimeType type, string mask, bool allowUserValues, string keysColumn, string valuesColumn, StiVariableSortDirection sortDirection, StiVariableSortField sortField)
	{
		DateTimeType = type;
		Mask = mask;
		AllowUserValues = allowUserValues;
		ItemsInitializationType = StiItemsInitializationType.Columns;
		KeysColumn = keysColumn;
		ValuesColumn = valuesColumn;
		SortDirection = sortDirection;
		SortField = sortField;
	}

	public StiDialogInfo(StiDateTimeType type, string mask, bool allowUserValues, string keysColumn, string valuesColumn, string checkedColumn, StiVariableSortDirection sortDirection, StiVariableSortField sortField)
	{
		DateTimeType = type;
		Mask = mask;
		AllowUserValues = allowUserValues;
		ItemsInitializationType = StiItemsInitializationType.Columns;
		KeysColumn = keysColumn;
		ValuesColumn = valuesColumn;
		CheckedColumn = checkedColumn;
		SortDirection = sortDirection;
		SortField = sortField;
	}

	public StiDialogInfo(StiDateTimeType type, string mask, bool allowUserValues, string keysColumn, string valuesColumn, bool bindingValue, StiVariable bindingVariable, string bindingValuesColumn)
	{
		DateTimeType = type;
		Mask = mask;
		AllowUserValues = allowUserValues;
		ItemsInitializationType = StiItemsInitializationType.Columns;
		KeysColumn = keysColumn;
		ValuesColumn = valuesColumn;
		BindingValue = bindingValue;
		BindingVariable = bindingVariable;
		BindingValuesColumn = bindingValuesColumn;
	}

	public StiDialogInfo(StiDateTimeType type, string mask, bool allowUserValues, string keysColumn, string valuesColumn, string checkedColumn, bool bindingValue, StiVariable bindingVariable, string bindingValuesColumn)
	{
		DateTimeType = type;
		Mask = mask;
		AllowUserValues = allowUserValues;
		ItemsInitializationType = StiItemsInitializationType.Columns;
		KeysColumn = keysColumn;
		ValuesColumn = valuesColumn;
		CheckedColumn = checkedColumn;
		BindingValue = bindingValue;
		BindingVariable = bindingVariable;
		BindingValuesColumn = bindingValuesColumn;
	}

	public StiDialogInfo(StiDateTimeType type, string mask, bool allowUserValues, string keysColumn, string valuesColumn, bool bindingValue, StiVariable bindingVariable, string bindingValuesColumn, StiVariableSortDirection sortDirection, StiVariableSortField sortField)
	{
		DateTimeType = type;
		Mask = mask;
		AllowUserValues = allowUserValues;
		ItemsInitializationType = StiItemsInitializationType.Columns;
		KeysColumn = keysColumn;
		ValuesColumn = valuesColumn;
		BindingValue = bindingValue;
		BindingVariable = bindingVariable;
		BindingValuesColumn = bindingValuesColumn;
		SortDirection = sortDirection;
		SortField = sortField;
	}

	public StiDialogInfo(StiDateTimeType type, string mask, bool allowUserValues, string keysColumn, string valuesColumn, string checkedColumn, bool bindingValue, StiVariable bindingVariable, string bindingValuesColumn, StiVariableSortDirection sortDirection, StiVariableSortField sortField)
	{
		DateTimeType = type;
		Mask = mask;
		AllowUserValues = allowUserValues;
		ItemsInitializationType = StiItemsInitializationType.Columns;
		KeysColumn = keysColumn;
		ValuesColumn = valuesColumn;
		CheckedColumn = checkedColumn;
		BindingValue = bindingValue;
		BindingVariable = bindingVariable;
		BindingValuesColumn = bindingValuesColumn;
		SortDirection = sortDirection;
		SortField = sortField;
	}
}
