using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Web;

internal class StiVariablesHelper
{
	private class StiVariableItemsComparer : IComparer
	{
		int IComparer.Compare(object x, object y)
		{
			Hashtable hashtable = x as Hashtable;
			Hashtable hashtable2 = y as Hashtable;
			if (hashtable != null && hashtable2 != null)
			{
				string text = ((!string.IsNullOrEmpty(hashtable["value"] as string)) ? (hashtable["value"] as string) : (hashtable["key"] as string));
				string text2 = ((!string.IsNullOrEmpty(hashtable2["value"] as string)) ? (hashtable2["value"] as string) : (hashtable2["key"] as string));
				if (text != null && text2 != null)
				{
					return text.CompareTo(text2);
				}
				return 0;
			}
			return 0;
		}
	}

	private static CultureInfo en_us_culture = CultureInfo.CreateSpecificCulture("en-US");

	public static void FillDialogInfoItems(StiReport report)
	{
		bool flag = false;
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			if (variable.RequestFromUser && variable.DialogInfo.ItemsInitializationType == StiItemsInitializationType.Columns && (variable.DialogInfo.Keys == null || variable.DialogInfo.Keys.Length == 0 || variable.DialogInfo.Values == null || variable.DialogInfo.Values.Length == 0))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			report.Dictionary.Connect();
			StiVariableHelper.FillItemsOfVariables(report);
			report.Dictionary.Disconnect();
		}
	}

	private static string GetVariableAlias(StiVariable variable)
	{
		if (string.IsNullOrEmpty(variable.Alias))
		{
			return variable.Name;
		}
		return variable.Alias;
	}

	private static ArrayList GetItems(StiVariable variable, Hashtable values = null)
	{
		ArrayList arrayList = new ArrayList();
		int num = 0;
		bool flag = variable.DialogInfo.BindingValue && variable.DialogInfo.BindingVariable != null;
		string text = (flag ? variable.DialogInfo.BindingVariable.Value : null);
		if (variable.DialogInfo.Keys != null && variable.DialogInfo.Keys.Length != 0 && (variable.DialogInfo.ItemsInitializationType == StiItemsInitializationType.Items || (variable.DialogInfo.ItemsInitializationType == StiItemsInitializationType.Columns && (!string.IsNullOrEmpty(variable.DialogInfo.KeysColumn) || !string.IsNullOrEmpty(variable.DialogInfo.ValuesColumn)))))
		{
			List<StiDialogInfoItem> dialogInfoItems = variable.DialogInfo.GetDialogInfoItems(variable.Type);
			Hashtable hashtable = new Hashtable();
			HashSet<object> hashSet = new HashSet<object>();
			HashSet<object> hashSet2 = new HashSet<object>();
			foreach (StiDialogInfoItem item2 in dialogInfoItems)
			{
				List<object> list = item2.ValueBinding;
				if (list == null || list.Count == 0)
				{
					list = new List<object> { item2.ValueBinding };
				}
				foreach (object item3 in list)
				{
					if (values != null && flag && (text == null || !(text == Convert.ToString(item3))) && !ContainsBindingVariableValue(variable, Convert.ToString(item3), values))
					{
						continue;
					}
					Hashtable hashtable2 = new Hashtable();
					hashtable2["value"] = item2.Value;
					hashtable2["key"] = item2.KeyObject;
					hashtable2["keyTo"] = item2.KeyObjectTo;
					string item = item2.KeyObject?.ToString();
					if (variable.Type == typeof(DateTime) || variable.Type == typeof(DateTime?) || variable.Type == typeof(DateTimeRange) || variable.Type == typeof(DateTimeList))
					{
						if (item2.KeyObject != null)
						{
							hashtable2["key"] = GetDateTimeObject(item2.KeyObject);
						}
						if (item2.KeyObjectTo != null)
						{
							hashtable2["keyTo"] = GetDateTimeObject(item2.KeyObjectTo);
						}
					}
					if ((string.IsNullOrEmpty(text) || hashtable2["key"] == null || hashtable[hashtable2["key"]] == null) && (!hashSet.Contains(item) || (!string.IsNullOrEmpty(hashtable2["value"] as string) && !hashSet2.Contains(hashtable2["value"]))))
					{
						arrayList.Add(hashtable2);
						hashSet.Add(item);
						hashSet2.Add(hashtable2["value"]);
					}
					if (!string.IsNullOrEmpty(text))
					{
						hashtable[hashtable2["key"]] = true;
					}
				}
				num++;
			}
		}
		if (flag && values == null && StiTypeFinder.FindInterface(variable.Type, typeof(IStiList)))
		{
			StiVariable bindingVariable = variable.DialogInfo.BindingVariable;
			bool[] checkedStates = bindingVariable.DialogInfo.CheckedStates;
			if (!bindingVariable.DialogInfo.AllowUserValues && checkedStates != null && checkedStates.Length != 0 && checkedStates.All((bool s) => !s))
			{
				return new ArrayList();
			}
		}
		if (num <= 0)
		{
			return null;
		}
		return arrayList;
	}

	private static bool ContainsBindingVariableValue(StiVariable variable, string value, Hashtable values)
	{
		if (variable.DialogInfo.BindingValue && variable.DialogInfo.BindingVariable != null && StiTypeFinder.FindInterface(variable.DialogInfo.BindingVariable.Type, typeof(IStiList)))
		{
			try
			{
				if (!string.IsNullOrEmpty(variable.DialogInfo.BindingVariable.Value))
				{
					List<object> list = JsonConvert.DeserializeObject<List<object>>(variable.DialogInfo.BindingVariable.Value);
					if (list != null && value != null && list.Any((object v) => v != null && Convert.ToString(v) == Convert.ToString(value)))
					{
						return true;
					}
				}
				else if (variable.DialogInfo.BindingVariable.DialogInfo.ValuesBindingList != null)
				{
					ArrayList arrayList = ((values != null) ? (values[variable.DialogInfo.BindingVariable.Name] as ArrayList) : null);
					if (arrayList != null && arrayList.Count > 0)
					{
						return arrayList.Cast<object>().ToList().Any((object v) => v != null && Convert.ToString(v) == Convert.ToString(value));
					}
					List<object>[] valuesBindingList = variable.DialogInfo.BindingVariable.DialogInfo.ValuesBindingList;
					foreach (List<object> list2 in valuesBindingList)
					{
						if (list2 == null || value == null || !list2.Any((object v) => v != null && Convert.ToString(v) == Convert.ToString(value)))
						{
							continue;
						}
						ArrayList arrayList2 = ((values != null) ? (values[variable.Name] as ArrayList) : null);
						if (arrayList2 != null && arrayList2.Count > 0)
						{
							return arrayList2.Cast<object>().ToList().Any((object v) => v != null && Convert.ToString(v) == Convert.ToString(value));
						}
						return true;
					}
				}
			}
			catch
			{
			}
		}
		return false;
	}

	private static TimeSpan ParseTimeSpanValue(string value)
	{
		TimeSpan result = TimeSpan.Zero;
		if (!string.IsNullOrEmpty(value) && !TimeSpan.TryParse(value, out result))
		{
			int num = 0;
			int num2 = 0;
			int minutes = 0;
			int seconds = 0;
			int milliseconds = 0;
			try
			{
				string[] array = value.Split(new char[1] { ':' });
				if (array[0].Contains("."))
				{
					string[] array2 = array[0].Split(new char[1] { '.' });
					num = Convert.ToInt32(array2[0]);
					num2 = Convert.ToInt32(array2[1]);
				}
				else
				{
					num2 = Convert.ToInt32(array[0]);
				}
				if (num2 > 23)
				{
					num += (int)Math.Truncate((double)num2 / 24.0);
					num2 %= 24;
				}
				if (array.Length > 1)
				{
					minutes = Math.Min(59, Convert.ToInt32(array[1]));
				}
				if (array.Length > 2)
				{
					if (array[2].Contains("."))
					{
						string[] array3 = array[2].Split(new char[1] { '.' });
						seconds = Math.Min(59, Convert.ToInt32(array3[0]));
						milliseconds = Convert.ToInt32(array3[1]);
					}
					else
					{
						seconds = Math.Min(59, Convert.ToInt32(array[2]));
					}
				}
			}
			catch
			{
			}
			return new TimeSpan(num, num2, minutes, seconds, milliseconds);
		}
		return result;
	}

	private static Hashtable GetDateTimeObject(object value)
	{
		if (value is Hashtable)
		{
			return (Hashtable)value;
		}
		DateTime result = DateTime.Now;
		if (value != null && value is DateTime)
		{
			result = (DateTime)value;
		}
		if (value != null && value is string)
		{
			DateTime.TryParse(Convert.ToString(value), en_us_culture, DateTimeStyles.None, out result);
		}
		Hashtable hashtable = new Hashtable();
		hashtable["year"] = result.Year;
		hashtable["month"] = result.Month;
		hashtable["day"] = result.Day;
		hashtable["hours"] = result.Hour;
		hashtable["minutes"] = result.Minute;
		hashtable["seconds"] = result.Second;
		if (value == null)
		{
			hashtable["isNull"] = true;
		}
		return hashtable;
	}

	private static string GetTimeSpanStringValue(object value)
	{
		if (value != null && value is TimeSpan timeSpan)
		{
			return new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds).ToString();
		}
		return string.Empty;
	}

	private static string GetBasicType(StiVariable variable)
	{
		if (StiTypeFinder.FindType(variable.Type, typeof(Range)))
		{
			return "Range";
		}
		if (StiTypeFinder.FindInterface(variable.Type, typeof(IStiList)))
		{
			return "List";
		}
		if (variable.Type.Name.StartsWith("Nullable"))
		{
			return "NullableValue";
		}
		return "Value";
	}

	private static string GetType(StiVariable variable)
	{
		if (variable.Type == typeof(string) || variable.Type == typeof(StringList) || variable.Type == typeof(StringRange))
		{
			return "String";
		}
		if (variable.Type == typeof(char) || variable.Type == typeof(char?) || variable.Type == typeof(CharRange) || variable.Type == typeof(CharList))
		{
			return "Char";
		}
		if (variable.Type == typeof(bool) || variable.Type == typeof(bool?) || variable.Type == typeof(BoolList))
		{
			return "Bool";
		}
		if (variable.Type == typeof(DateTime) || variable.Type == typeof(DateTime?) || variable.Type == typeof(DateTimeList) || variable.Type == typeof(DateTimeRange))
		{
			return "DateTime";
		}
		if (variable.Type == typeof(TimeSpan) || variable.Type == typeof(TimeSpan?) || variable.Type == typeof(TimeSpanList) || variable.Type == typeof(TimeSpanRange))
		{
			return "TimeSpan";
		}
		if (variable.Type == typeof(Guid) || variable.Type == typeof(Guid?) || variable.Type == typeof(GuidList) || variable.Type == typeof(GuidRange))
		{
			return "Guid";
		}
		if (variable.Type == typeof(Image) || variable.Type == typeof(Bitmap))
		{
			return "Image";
		}
		if (variable.Type == typeof(float) || variable.Type == typeof(float?) || variable.Type == typeof(FloatList) || variable.Type == typeof(FloatRange))
		{
			return "Float";
		}
		if (variable.Type == typeof(double) || variable.Type == typeof(double?) || variable.Type == typeof(DoubleList) || variable.Type == typeof(DoubleRange))
		{
			return "Double";
		}
		if (variable.Type == typeof(decimal) || variable.Type == typeof(decimal?) || variable.Type == typeof(DecimalList) || variable.Type == typeof(DecimalRange))
		{
			return "Decimal";
		}
		if (variable.Type == typeof(int) || variable.Type == typeof(int?) || variable.Type == typeof(IntList) || variable.Type == typeof(IntRange))
		{
			return "Int";
		}
		if (variable.Type == typeof(uint) || variable.Type == typeof(uint?))
		{
			return "Uint";
		}
		if (variable.Type == typeof(short) || variable.Type == typeof(short?) || variable.Type == typeof(ShortList) || variable.Type == typeof(ShortRange))
		{
			return "Short";
		}
		if (variable.Type == typeof(ushort) || variable.Type == typeof(ushort?))
		{
			return "Ushort";
		}
		if (variable.Type == typeof(long) || variable.Type == typeof(long?) || variable.Type == typeof(LongList) || variable.Type == typeof(LongRange))
		{
			return "Long";
		}
		if (variable.Type == typeof(ulong) || variable.Type == typeof(ulong?))
		{
			return "Ulong";
		}
		if (variable.Type == typeof(byte) || variable.Type == typeof(byte?) || variable.Type == typeof(ByteList) || variable.Type == typeof(ByteRange))
		{
			return "Byte";
		}
		if (variable.Type == typeof(sbyte) || variable.Type == typeof(sbyte?))
		{
			return "Sbyte";
		}
		return string.Empty;
	}

	public static void ApplyReportParameters(StiReport report, Hashtable values)
	{
		if (values == null || values.Count <= 0)
		{
			return;
		}
		if (report.CalculationMode == StiCalculationMode.Interpretation)
		{
			foreach (StiVariable variable in report.Dictionary.Variables)
			{
				report[variable.Name] = variable.ValueObject;
			}
		}
		StiParser.PrepareReportVariables(report);
		if (values != null)
		{
			foreach (string key in values.Keys)
			{
				StiVariable stiVariable2 = report.Dictionary.Variables[key];
				if (stiVariable2 != null)
				{
					SetVariableValue(report, key, values[key], stiVariable2);
					SetVariableLabel(report, stiVariable2, values[key]);
				}
			}
		}
		report.IsRendered = false;
	}

	public static void ApplyReportBindingVariables(StiReport report, Hashtable values)
	{
		if (values == null)
		{
			return;
		}
		foreach (string key in values.Keys)
		{
			foreach (StiVariable variable in report.Dictionary.Variables)
			{
				if (variable.Name == key)
				{
					variable.Value = ((values[key] is ArrayList) ? "" : (StiTypeFinder.FindType(variable.Type, typeof(Range)) ? GetRangeStringValue(variable, values[key] as Hashtable) : Convert.ToString(values[key])));
				}
				if (variable.DialogInfo.BindingVariable != null && variable.DialogInfo.BindingVariable.Name == key)
				{
					variable.DialogInfo.BindingVariable.Value = ((values[key] is ArrayList) ? "" : Convert.ToString(values[key]));
				}
			}
		}
	}

	private static void SetVariableLabel(StiReport report, StiVariable variable, object value)
	{
		if (variable == null)
		{
			return;
		}
		string[] keys = variable.DialogInfo.Keys;
		if (keys == null || keys.Length == 0)
		{
			return;
		}
		string[] values = variable.DialogInfo.Values;
		if (values != null && values.Length != 0)
		{
			int num = variable.DialogInfo.Keys.ToList().IndexOf(value?.ToString());
			if (num >= 0 && num < variable.DialogInfo.Values.Length)
			{
				string label = variable.DialogInfo.Values[num];
				StiVariableHelper.SetVariableLabel(report, variable, label);
			}
		}
	}

	private static string GetRangeStringValue(StiVariable variable, Hashtable value)
	{
		string numberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		object obj = null;
		if (variable.Type == typeof(StringRange))
		{
			obj = new StringRange(Convert.ToString(value["from"]), Convert.ToString(value["to"]));
		}
		else if (variable.Type == typeof(FloatRange))
		{
			float result = 0f;
			float result2 = 0f;
			float.TryParse(Convert.ToString(value["from"]).Replace(",", numberDecimalSeparator), out result);
			float.TryParse(Convert.ToString(value["to"]).Replace(".", ",").Replace(",", numberDecimalSeparator), out result2);
			obj = new FloatRange(result, result2);
		}
		else if (variable.Type == typeof(DoubleRange))
		{
			double result3 = 0.0;
			double result4 = 0.0;
			double.TryParse(Convert.ToString(value["from"]).Replace(".", ",").Replace(",", numberDecimalSeparator), out result3);
			double.TryParse(Convert.ToString(value["to"]).Replace(".", ",").Replace(",", numberDecimalSeparator), out result4);
			obj = new DoubleRange(result3, result4);
		}
		else if (variable.Type == typeof(DecimalRange))
		{
			decimal result5 = default(decimal);
			decimal result6 = default(decimal);
			decimal.TryParse(Convert.ToString(value["from"]).Replace(".", ",").Replace(",", numberDecimalSeparator), out result5);
			decimal.TryParse(Convert.ToString(value["to"]).Replace(".", ",").Replace(",", numberDecimalSeparator), out result6);
			obj = new DecimalRange(result5, result6);
		}
		else if (variable.Type == typeof(IntRange))
		{
			int result7 = 0;
			int result8 = 0;
			int.TryParse(Convert.ToString(value["from"]), out result7);
			int.TryParse(Convert.ToString(value["to"]), out result8);
			obj = new IntRange(result7, result8);
		}
		else if (variable.Type == typeof(ShortRange))
		{
			short result9 = 0;
			short result10 = 0;
			short.TryParse(Convert.ToString(value["from"]), out result9);
			short.TryParse(Convert.ToString(value["to"]), out result10);
			obj = new ShortRange(result9, result10);
		}
		else if (variable.Type == typeof(LongRange))
		{
			long result11 = 0L;
			long result12 = 0L;
			long.TryParse(Convert.ToString(value["from"]), out result11);
			long.TryParse(Convert.ToString(value["to"]), out result12);
			obj = new LongRange(result11, result12);
		}
		else if (variable.Type == typeof(ByteRange))
		{
			byte result13 = 0;
			byte result14 = 0;
			byte.TryParse(Convert.ToString(value["from"]), out result13);
			byte.TryParse(Convert.ToString(value["to"]), out result14);
			obj = new ByteRange(result13, result14);
		}
		else if (variable.Type == typeof(CharRange))
		{
			char result15 = ' ';
			char result16 = ' ';
			char.TryParse(Convert.ToString(value["from"]), out result15);
			char.TryParse(Convert.ToString(value["to"]), out result16);
			obj = new CharRange(result15, result16);
		}
		else if (variable.Type == typeof(DateTimeRange))
		{
			DateTime result17 = DateTime.Now;
			DateTime result18 = DateTime.Now;
			DateTime.TryParse(Convert.ToString(value["from"]), en_us_culture, DateTimeStyles.None, out result17);
			DateTime.TryParse(Convert.ToString(value["to"]), en_us_culture, DateTimeStyles.None, out result18);
			obj = new DateTimeRange(result17, result18);
		}
		else if (variable.Type == typeof(TimeSpanRange))
		{
			TimeSpan result19 = TimeSpan.Zero;
			TimeSpan result20 = TimeSpan.Zero;
			TimeSpan.TryParse(Convert.ToString(value["from"]), out result19);
			TimeSpan.TryParse(Convert.ToString(value["to"]), out result20);
			obj = new TimeSpanRange(result19, result20);
		}
		else if (variable.Type == typeof(GuidRange))
		{
			Guid from = Guid.Empty;
			Guid to = Guid.Empty;
			try
			{
				from = new Guid(Convert.ToString(value["from"]));
				to = new Guid(Convert.ToString(value["to"]));
			}
			catch
			{
			}
			return new GuidRange(from, to).ToString();
		}
		return RangeConverter.RangeToString(obj as Range);
	}

	internal static void SetVariableValue(StiReport report, string paramName, object paramValue, StiVariable variable)
	{
		string numberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		string text = null;
		Hashtable hashtable = null;
		ArrayList arrayList = null;
		if (paramValue != null)
		{
			if (paramValue is Hashtable)
			{
				hashtable = (Hashtable)paramValue;
			}
			else if (paramValue is ArrayList)
			{
				arrayList = (ArrayList)paramValue;
			}
			else
			{
				text = Convert.ToString(paramValue);
			}
		}
		if (text == null && (variable.Type == typeof(float?) || variable.Type == typeof(double?) || variable.Type == typeof(decimal?) || variable.Type == typeof(int?) || variable.Type == typeof(uint?) || variable.Type == typeof(short?) || variable.Type == typeof(ushort?) || variable.Type == typeof(long?) || variable.Type == typeof(ulong?) || variable.Type == typeof(byte?) || variable.Type == typeof(sbyte?) || variable.Type == typeof(char?) || variable.Type == typeof(bool?) || variable.Type == typeof(DateTime?) || variable.Type == typeof(TimeSpan?) || variable.Type == typeof(Guid?)))
		{
			report[paramName] = null;
		}
		else if (variable.Type == typeof(string))
		{
			report[paramName] = paramValue;
		}
		else if (variable.Type == typeof(float) || variable.Type == typeof(float?))
		{
			float result = 0f;
			float.TryParse(text.Replace(".", ",").Replace(",", numberDecimalSeparator), out result);
			report[paramName] = result;
		}
		else if (variable.Type == typeof(double) || variable.Type == typeof(double?))
		{
			double result2 = 0.0;
			double.TryParse(text.Replace(".", ",").Replace(",", numberDecimalSeparator), out result2);
			report[paramName] = result2;
		}
		else if (variable.Type == typeof(decimal) || variable.Type == typeof(decimal?))
		{
			decimal result3 = default(decimal);
			decimal.TryParse(text.Replace(".", ",").Replace(",", numberDecimalSeparator), out result3);
			report[paramName] = result3;
		}
		else if (variable.Type == typeof(int) || variable.Type == typeof(int?))
		{
			int result4 = 0;
			int.TryParse(text, out result4);
			report[paramName] = result4;
		}
		else if (variable.Type == typeof(uint) || variable.Type == typeof(uint?))
		{
			uint result5 = 0u;
			uint.TryParse(text, out result5);
			report[paramName] = result5;
		}
		else if (variable.Type == typeof(short) || variable.Type == typeof(short?))
		{
			short result6 = 0;
			short.TryParse(text, out result6);
			report[paramName] = result6;
		}
		else if (variable.Type == typeof(ushort) || variable.Type == typeof(ushort?))
		{
			ushort result7 = 0;
			ushort.TryParse(text, out result7);
			report[paramName] = result7;
		}
		else if (variable.Type == typeof(long) || variable.Type == typeof(long?))
		{
			long result8 = 0L;
			long.TryParse(text, out result8);
			report[paramName] = result8;
		}
		else if (variable.Type == typeof(ulong) || variable.Type == typeof(ulong?))
		{
			ulong result9 = 0uL;
			ulong.TryParse(text, out result9);
			report[paramName] = result9;
		}
		else if (variable.Type == typeof(byte) || variable.Type == typeof(byte?))
		{
			byte result10 = 0;
			byte.TryParse(text, out result10);
			report[paramName] = result10;
		}
		else if (variable.Type == typeof(sbyte) || variable.Type == typeof(sbyte?))
		{
			sbyte result11 = 0;
			sbyte.TryParse(text, out result11);
			report[paramName] = result11;
		}
		else if (variable.Type == typeof(char) || variable.Type == typeof(char?))
		{
			char result12 = ' ';
			char.TryParse(text, out result12);
			report[paramName] = result12;
		}
		else if (variable.Type == typeof(bool) || variable.Type == typeof(bool?))
		{
			bool result13 = false;
			bool.TryParse(text, out result13);
			report[paramName] = result13;
		}
		else if (variable.Type == typeof(DateTime) || variable.Type == typeof(DateTime?))
		{
			DateTime result14 = DateTime.Now;
			DateTime.TryParse(text, en_us_culture, DateTimeStyles.None, out result14);
			report[paramName] = result14;
		}
		else if (variable.Type == typeof(TimeSpan) || variable.Type == typeof(TimeSpan?))
		{
			report[paramName] = ParseTimeSpanValue(text);
		}
		else if (variable.Type == typeof(Guid) || variable.Type == typeof(Guid?))
		{
			Guid guid;
			try
			{
				guid = new Guid(text);
			}
			catch
			{
				guid = Guid.Empty;
			}
			report[paramName] = guid;
		}
		else if (variable.Type == typeof(StringRange))
		{
			report[paramName] = new StringRange(Convert.ToString(hashtable["from"]), Convert.ToString(hashtable["to"]));
		}
		else if (variable.Type == typeof(FloatRange))
		{
			float result15 = 0f;
			float result16 = 0f;
			float.TryParse(Convert.ToString(hashtable["from"]).Replace(",", numberDecimalSeparator), out result15);
			float.TryParse(Convert.ToString(hashtable["to"]).Replace(".", ",").Replace(",", numberDecimalSeparator), out result16);
			report[paramName] = new FloatRange(result15, result16);
		}
		else if (variable.Type == typeof(DoubleRange))
		{
			double result17 = 0.0;
			double result18 = 0.0;
			double.TryParse(Convert.ToString(hashtable["from"]).Replace(".", ",").Replace(",", numberDecimalSeparator), out result17);
			double.TryParse(Convert.ToString(hashtable["to"]).Replace(".", ",").Replace(",", numberDecimalSeparator), out result18);
			report[paramName] = new DoubleRange(result17, result18);
		}
		else if (variable.Type == typeof(DecimalRange))
		{
			decimal result19 = default(decimal);
			decimal result20 = default(decimal);
			decimal.TryParse(Convert.ToString(hashtable["from"]).Replace(".", ",").Replace(",", numberDecimalSeparator), out result19);
			decimal.TryParse(Convert.ToString(hashtable["to"]).Replace(".", ",").Replace(",", numberDecimalSeparator), out result20);
			report[paramName] = new DecimalRange(result19, result20);
		}
		else if (variable.Type == typeof(IntRange))
		{
			int result21 = 0;
			int result22 = 0;
			int.TryParse(Convert.ToString(hashtable["from"]), out result21);
			int.TryParse(Convert.ToString(hashtable["to"]), out result22);
			report[paramName] = new IntRange(result21, result22);
		}
		else if (variable.Type == typeof(ShortRange))
		{
			short result23 = 0;
			short result24 = 0;
			short.TryParse(Convert.ToString(hashtable["from"]), out result23);
			short.TryParse(Convert.ToString(hashtable["to"]), out result24);
			report[paramName] = new ShortRange(result23, result24);
		}
		else if (variable.Type == typeof(LongRange))
		{
			long result25 = 0L;
			long result26 = 0L;
			long.TryParse(Convert.ToString(hashtable["from"]), out result25);
			long.TryParse(Convert.ToString(hashtable["to"]), out result26);
			report[paramName] = new LongRange(result25, result26);
		}
		else if (variable.Type == typeof(ByteRange))
		{
			byte result27 = 0;
			byte result28 = 0;
			byte.TryParse(Convert.ToString(hashtable["from"]), out result27);
			byte.TryParse(Convert.ToString(hashtable["to"]), out result28);
			report[paramName] = new ByteRange(result27, result28);
		}
		else if (variable.Type == typeof(CharRange))
		{
			char result29 = ' ';
			char result30 = ' ';
			char.TryParse(Convert.ToString(hashtable["from"]), out result29);
			char.TryParse(Convert.ToString(hashtable["to"]), out result30);
			report[paramName] = new CharRange(result29, result30);
		}
		else if (variable.Type == typeof(DateTimeRange))
		{
			DateTime result31 = DateTime.Now;
			DateTime result32 = DateTime.Now;
			DateTime.TryParse(Convert.ToString(hashtable["from"]), en_us_culture, DateTimeStyles.None, out result31);
			DateTime.TryParse(Convert.ToString(hashtable["to"]), en_us_culture, DateTimeStyles.None, out result32);
			report[paramName] = new DateTimeRange(result31, result32);
		}
		else if (variable.Type == typeof(TimeSpanRange))
		{
			TimeSpan result33 = TimeSpan.Zero;
			TimeSpan result34 = TimeSpan.Zero;
			TimeSpan.TryParse(Convert.ToString(hashtable["from"]), out result33);
			TimeSpan.TryParse(Convert.ToString(hashtable["to"]), out result34);
			report[paramName] = new TimeSpanRange(result33, result34);
		}
		else if (variable.Type == typeof(GuidRange))
		{
			Guid from = Guid.Empty;
			Guid to = Guid.Empty;
			try
			{
				from = new Guid(Convert.ToString(hashtable["from"]));
				to = new Guid(Convert.ToString(hashtable["to"]));
			}
			catch
			{
			}
			report[paramName] = new GuidRange(from, to);
		}
		else if (variable.Type == typeof(StringList))
		{
			StringList stringList = new StringList();
			foreach (object item2 in arrayList)
			{
				stringList.Add(Convert.ToString(item2));
			}
			report[paramName] = stringList;
			if (variable.DialogInfo.Keys == null || variable.DialogInfo.Keys.Length == 0)
			{
				variable.DialogInfo.Keys = stringList.ToArray();
			}
		}
		else if (variable.Type == typeof(FloatList))
		{
			FloatList floatList = new FloatList();
			foreach (object item3 in arrayList)
			{
				float result35 = 0f;
				float.TryParse(Convert.ToString(item3).Replace(".", ",").Replace(",", numberDecimalSeparator), out result35);
				floatList.Add(result35);
			}
			report[paramName] = floatList;
			if (variable.DialogInfo.Keys == null || variable.DialogInfo.Keys.Length == 0)
			{
				variable.DialogInfo.Keys = (from i in floatList.ToArray()
					select Convert.ToString(i)).ToArray();
			}
		}
		else if (variable.Type == typeof(DoubleList))
		{
			DoubleList doubleList = new DoubleList();
			foreach (object item4 in arrayList)
			{
				double result36 = 0.0;
				double.TryParse(Convert.ToString(item4).Replace(".", ",").Replace(",", numberDecimalSeparator), out result36);
				doubleList.Add(result36);
			}
			report[paramName] = doubleList;
			if (variable.DialogInfo.Keys == null || variable.DialogInfo.Keys.Length == 0)
			{
				variable.DialogInfo.Keys = (from i in doubleList.ToArray()
					select Convert.ToString(i)).ToArray();
			}
		}
		else if (variable.Type == typeof(DecimalList))
		{
			DecimalList decimalList = new DecimalList();
			foreach (object item5 in arrayList)
			{
				decimal result37 = default(decimal);
				decimal.TryParse(Convert.ToString(item5).Replace(".", ",").Replace(",", numberDecimalSeparator), out result37);
				decimalList.Add(result37);
			}
			report[paramName] = decimalList;
			if (variable.DialogInfo.Keys == null || variable.DialogInfo.Keys.Length == 0)
			{
				variable.DialogInfo.Keys = (from i in decimalList.ToArray()
					select Convert.ToString(i)).ToArray();
			}
		}
		else if (variable.Type == typeof(ByteList))
		{
			ByteList byteList = new ByteList();
			foreach (object item6 in arrayList)
			{
				byte result38 = 0;
				byte.TryParse(Convert.ToString(item6), out result38);
				byteList.Add(result38);
			}
			report[paramName] = byteList;
			if (variable.DialogInfo.Keys == null || variable.DialogInfo.Keys.Length == 0)
			{
				variable.DialogInfo.Keys = (from i in byteList.ToArray()
					select Convert.ToString(i)).ToArray();
			}
		}
		else if (variable.Type == typeof(ShortList))
		{
			ShortList shortList = new ShortList();
			foreach (object item7 in arrayList)
			{
				short result39 = 0;
				short.TryParse(Convert.ToString(item7), out result39);
				shortList.Add(result39);
			}
			report[paramName] = shortList;
			if (variable.DialogInfo.Keys == null || variable.DialogInfo.Keys.Length == 0)
			{
				variable.DialogInfo.Keys = (from i in shortList.ToArray()
					select Convert.ToString(i)).ToArray();
			}
		}
		else if (variable.Type == typeof(IntList))
		{
			IntList intList = new IntList();
			foreach (object item8 in arrayList)
			{
				int result40 = 0;
				int.TryParse(Convert.ToString(item8), out result40);
				intList.Add(result40);
			}
			report[paramName] = intList;
			if (variable.DialogInfo.Keys == null || variable.DialogInfo.Keys.Length == 0)
			{
				variable.DialogInfo.Keys = (from i in intList.ToArray()
					select Convert.ToString(i)).ToArray();
			}
		}
		else if (variable.Type == typeof(LongList))
		{
			LongList longList = new LongList();
			foreach (object item9 in arrayList)
			{
				long result41 = 0L;
				long.TryParse(Convert.ToString(item9), out result41);
				longList.Add(result41);
			}
			report[paramName] = longList;
			if (variable.DialogInfo.Keys == null || variable.DialogInfo.Keys.Length == 0)
			{
				variable.DialogInfo.Keys = (from i in longList.ToArray()
					select Convert.ToString(i)).ToArray();
			}
		}
		else if (variable.Type == typeof(CharList))
		{
			CharList charList = new CharList();
			foreach (object item10 in arrayList)
			{
				char result42 = ' ';
				char.TryParse(Convert.ToString(item10), out result42);
				charList.Add(result42);
			}
			report[paramName] = charList;
			if (variable.DialogInfo.Keys == null || variable.DialogInfo.Keys.Length == 0)
			{
				variable.DialogInfo.Keys = (from i in charList.ToArray()
					select Convert.ToString(i)).ToArray();
			}
		}
		else if (variable.Type == typeof(DateTimeList))
		{
			DateTimeList dateTimeList = new DateTimeList();
			foreach (object item11 in arrayList)
			{
				DateTime.TryParse(Convert.ToString(item11), en_us_culture, DateTimeStyles.None, out var result43);
				dateTimeList.Add(result43);
			}
			report[paramName] = dateTimeList;
			if (variable.DialogInfo.Keys == null || variable.DialogInfo.Keys.Length == 0)
			{
				variable.DialogInfo.Keys = (from i in dateTimeList.ToArray()
					select Convert.ToString(i)).ToArray();
			}
		}
		else if (variable.Type == typeof(TimeSpanList))
		{
			TimeSpanList timeSpanList = new TimeSpanList();
			foreach (object item12 in arrayList)
			{
				TimeSpan result44 = TimeSpan.Zero;
				TimeSpan.TryParse(Convert.ToString(item12), out result44);
				timeSpanList.Add(result44);
			}
			report[paramName] = timeSpanList;
			if (variable.DialogInfo.Keys == null || variable.DialogInfo.Keys.Length == 0)
			{
				variable.DialogInfo.Keys = (from i in timeSpanList.ToArray()
					select Convert.ToString(i)).ToArray();
			}
		}
		else if (variable.Type == typeof(BoolList))
		{
			BoolList boolList = new BoolList();
			foreach (object item13 in arrayList)
			{
				bool.TryParse(Convert.ToString(item13), out var result45);
				boolList.Add(result45);
			}
			report[paramName] = boolList;
			if (variable.DialogInfo.Keys == null || variable.DialogInfo.Keys.Length == 0)
			{
				variable.DialogInfo.Keys = (from i in boolList.ToArray()
					select Convert.ToString(i)).ToArray();
			}
		}
		else
		{
			if (!(variable.Type == typeof(GuidList)))
			{
				return;
			}
			GuidList guidList = new GuidList();
			foreach (object item14 in arrayList)
			{
				Guid item;
				try
				{
					item = new Guid(Convert.ToString(item14));
				}
				catch
				{
					item = Guid.Empty;
				}
				guidList.Add(item);
			}
			report[paramName] = guidList;
		}
	}

	public static Hashtable GetVariables(StiReport report, Hashtable values, bool sortDataItems)
	{
		FillDialogInfoItems(report);
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		int num = 0;
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			if (!variable.RequestFromUser)
			{
				continue;
			}
			if (variable.DialogInfo.BindingValue && variable.DialogInfo.BindingVariable != null)
			{
				hashtable2[variable.DialogInfo.BindingVariable.Name] = true;
			}
			string name = StiNameValidator.CorrectName(variable.Name);
			Hashtable hashtable3 = new Hashtable();
			hashtable3["name"] = variable.Name;
			hashtable3["alias"] = GetVariableAlias(variable);
			hashtable3["description"] = variable.Description;
			hashtable3["basicType"] = GetBasicType(variable);
			hashtable3["type"] = GetType(variable);
			hashtable3["allowUserValues"] = variable.DialogInfo.AllowUserValues;
			hashtable3["dateTimeType"] = variable.DialogInfo.DateTimeType.ToString();
			hashtable3["sortDirection"] = variable.DialogInfo.SortDirection.ToString();
			hashtable3["sortField"] = variable.DialogInfo.SortField.ToString();
			hashtable3["formatMask"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(variable.DialogInfo.Mask));
			hashtable3["checkedStates"] = ((variable.DialogInfo.CheckedStates != null && !string.IsNullOrEmpty(variable.DialogInfo.CheckedColumn)) ? new ArrayList(variable.DialogInfo.CheckedStates) : null);
			hashtable3["isFirstInitialization"] = values == null;
			ArrayList items = GetItems(variable, values);
			if (values != null && values.Contains(variable.Name))
			{
				hashtable3["value"] = values[variable.Name];
			}
			else if (variable.Selection == StiSelectionMode.Nothing)
			{
				hashtable3["value"] = string.Empty;
			}
			else if (variable.Selection == StiSelectionMode.First)
			{
				hashtable3["value"] = ((items != null && items.Count > 0) ? ((Hashtable)items[0])["key"] : string.Empty);
			}
			else
			{
				hashtable3["value"] = ((variable.InitBy == StiVariableInitBy.Value) ? variable.Value : report[name]);
			}
			hashtable3["key"] = ((variable.InitBy == StiVariableInitBy.Value) ? variable.ValueObject : ((values != null && values[variable.Name] != null && variable.Type == typeof(DateTime)) ? GetDateTimeObject(values[variable.Name]) : report[name]));
			hashtable3["keyTo"] = string.Empty;
			Hashtable hashtable4 = null;
			if (items != null && items.Count > 0)
			{
				if (variable.Selection == StiSelectionMode.First)
				{
					hashtable4 = (Hashtable)items[0];
				}
				else if (variable.Selection == StiSelectionMode.Nothing)
				{
					hashtable4 = new Hashtable();
					hashtable4["value"] = "";
					hashtable4["key"] = ((variable.Type == typeof(DateTime)) ? GetDateTimeObject(DateTime.Now) : null);
					hashtable4["keyTo"] = ((variable.Type == typeof(DateTime)) ? GetDateTimeObject(DateTime.Now) : null);
				}
				string text = Convert.ToString(hashtable3["value"]);
				foreach (Hashtable item in items)
				{
					if (Convert.ToString(item["key"]) == text)
					{
						hashtable4 = item;
						break;
					}
				}
				if (hashtable4 == null && variable.DialogInfo.BindingValue && variable.DialogInfo.BindingVariable != null && !Convert.ToBoolean(hashtable2[variable.Name]))
				{
					hashtable4 = (Hashtable)items[0];
				}
				if (variable.DialogInfo.ItemsInitializationType == StiItemsInitializationType.Columns && sortDataItems)
				{
					items.Sort(new StiVariableItemsComparer());
				}
			}
			else if (variable.DialogInfo.BindingValue && variable.DialogInfo.BindingVariable != null && !Convert.ToBoolean(hashtable2[variable.Name]) && (variable.InitBy != 0 || string.IsNullOrEmpty(variable.Value)))
			{
				hashtable3["value"] = "";
				hashtable3["key"] = null;
				hashtable3["keyTo"] = null;
			}
			hashtable3["items"] = items;
			if ((string)hashtable3["basicType"] == "Value" || (string)hashtable3["basicType"] == "NullableValue")
			{
				if (hashtable4 != null)
				{
					hashtable3["key"] = hashtable4["key"];
					hashtable3["value"] = hashtable4["value"];
					if (variable.DialogInfo.AllowUserValues || hashtable3["value"] == null || (hashtable3["value"] is string && (string)hashtable3["value"] == ""))
					{
						hashtable3["value"] = hashtable3["key"];
					}
					foreach (StiVariable variable2 in report.Dictionary.Variables)
					{
						if (variable2.DialogInfo.BindingVariable != null && variable2.DialogInfo.BindingVariable.Name == variable.Name)
						{
							variable2.DialogInfo.BindingVariable.ValueObject = hashtable3["key"];
						}
					}
				}
				if ((string)hashtable3["type"] == "DateTime")
				{
					hashtable3["key"] = GetDateTimeObject(hashtable3["key"]);
				}
				if ((string)hashtable3["type"] == "TimeSpan")
				{
					hashtable3["key"] = GetTimeSpanStringValue(hashtable3["key"]);
				}
			}
			if ((string)hashtable3["basicType"] == "Range")
			{
				object obj = ((variable.InitBy != 0) ? ((Range)report[name]).FromObject : ((Range)variable.ValueObject)?.FromObject);
				object obj2 = ((variable.InitBy != 0) ? ((Range)report[name])?.ToObject : ((Range)variable.ValueObject)?.ToObject);
				if ((string)hashtable3["type"] == "DateTime")
				{
					hashtable3["key"] = GetDateTimeObject(obj);
					hashtable3["keyTo"] = GetDateTimeObject(obj2);
				}
				else if ((string)hashtable3["type"] == "TimeSpan")
				{
					hashtable3["key"] = GetTimeSpanStringValue(obj);
					hashtable3["keyTo"] = GetTimeSpanStringValue(obj2);
				}
				else
				{
					hashtable3["key"] = obj?.ToString();
					hashtable3["keyTo"] = obj2?.ToString();
				}
			}
			hashtable[num.ToString()] = hashtable3;
			num++;
		}
		if (hashtable.Count > 0)
		{
			foreach (string key in hashtable2.Keys)
			{
				foreach (Hashtable value in hashtable.Values)
				{
					if ((string)value["name"] == key)
					{
						value["binding"] = true;
					}
				}
			}
			return hashtable;
		}
		return null;
	}

	public static Hashtable GetVariablesValues(StiReport report)
	{
		Hashtable hashtable = new Hashtable();
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			if (!variable.ReadOnly)
			{
				ArrayList items = GetItems(variable);
				string name = StiNameValidator.CorrectName(variable.Name);
				object obj = null;
				obj = ((variable.Selection != StiSelectionMode.First) ? ((variable.InitBy == StiVariableInitBy.Value) ? variable.Value : report[name]) : ((items != null && items.Count > 0) ? ((Hashtable)items[0])["key"] : string.Empty));
				hashtable[variable.Name] = ((obj != null) ? obj.ToString() : string.Empty);
			}
		}
		return hashtable;
	}
}
