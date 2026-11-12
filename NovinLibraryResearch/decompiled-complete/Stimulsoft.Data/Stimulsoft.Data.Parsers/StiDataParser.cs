using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Data.Expressions.NCalc;
using Stimulsoft.Data.Functions;

namespace Stimulsoft.Data.Parsers;

public abstract class StiDataParser
{
	private Dictionary<string, int> nameToIndex = new Dictionary<string, int>();

	private Dictionary<string, object> nameToValue = new Dictionary<string, object>();

	private Dictionary<string, IStiAppVariable> nameToVariable = new Dictionary<string, IStiAppVariable>();

	protected IStiAppDictionary Dictionary { get; }

	protected DataTable Table { get; }

	protected List<IStiMeter> Meters { get; }

	protected bool IsGrandTotal { get; set; }

	protected object RunFunction(string funcName, FunctionArgs args)
	{
		switch (Funcs.ToLowerCase(funcName))
		{
		case "all":
			if (args.Parameters.Length != 0)
			{
				return Funcs.All(GetDataColumnFromArg0(funcName, args));
			}
			return null;
		case "avg":
			return Funcs.Avg(GetDataColumnFromArg0(funcName, args));
		case "avgnulls":
			return Funcs.AvgNulls(GetDataColumnFromArg0(funcName, args));
		case "avgd":
			return Funcs.AvgD(GetDataColumnFromArg0(funcName, args));
		case "avgi":
			return Funcs.AvgI(GetDataColumnFromArg0(funcName, args));
		case "avgdate":
			return Funcs.AvgDate(GetDataColumnFromArg0(funcName, args));
		case "avgtime":
			return Funcs.AvgTime(GetDataColumnFromArg0(funcName, args));
		case "count":
			if (args.Parameters.Length != 0)
			{
				return Funcs.Count(GetDataColumnFromArg0(funcName, args));
			}
			return 0;
		case "countif":
			if (args.Parameters.Length != 0)
			{
				return Funcs.CountIf(GetDataColumnFromArg0(funcName, args), GetObjectFromArg1("condition", funcName, args));
			}
			return 0;
		case "distinct":
			if (args.Parameters.Length != 0)
			{
				return Funcs.Distinct(GetDataColumnFromArg0(funcName, args));
			}
			return 0;
		case "distinctcount":
			if (args.Parameters.Length != 0)
			{
				return Funcs.DistinctCount(GetDataColumnFromArg0(funcName, args));
			}
			return 0;
		case "distinctcountif":
			if (args.Parameters.Length != 0)
			{
				return Funcs.DistinctCountIf(GetDataColumnFromArg0(funcName, args), GetObjectFromArg1("condition", funcName, args));
			}
			return 0;
		case "grandcount":
			if (args.Parameters.Length != 0)
			{
				IsGrandTotal = true;
				long num3 = Funcs.Count(GetDataColumnFromArg0(funcName, args));
				IsGrandTotal = false;
				return num3;
			}
			return 0;
		case "grandtotal":
			if (args.Parameters.Length != 0)
			{
				IsGrandTotal = true;
				decimal num2 = Funcs.Sum(GetDataColumnFromArg0(funcName, args));
				IsGrandTotal = false;
				return num2;
			}
			return 0;
		case "getparam":
			if (args.Parameters.Length != 0)
			{
				return Dictionary.GetVariableValueByName(GetDataColumnFromArg0(funcName, args)?.ToString());
			}
			return null;
		case "percentofgrandtotal":
			if (args.Parameters.Length != 0)
			{
				decimal num4 = Funcs.Sum(GetDataColumnFromArg0(funcName, args));
				IsGrandTotal = true;
				decimal num5 = Funcs.Sum(GetDataColumnFromArg0(funcName, args));
				IsGrandTotal = false;
				if (num5 == 0m)
				{
					return 0;
				}
				return num4 / num5;
			}
			return 0;
		case "first":
			if (args.Parameters.Length != 0)
			{
				return Funcs.First(GetDataColumnFromArg0(funcName, args));
			}
			return null;
		case "last":
			if (args.Parameters.Length != 0)
			{
				return Funcs.Last(GetDataColumnFromArg0(funcName, args));
			}
			return null;
		case "median":
			return Funcs.Median(GetDataColumnFromArg0(funcName, args));
		case "max":
			if (args.Parameters.Length != 0)
			{
				return Funcs.Max(GetDataColumnFromArg0(funcName, args));
			}
			return 0;
		case "maxnulls":
			if (args.Parameters.Length != 0)
			{
				return Funcs.MaxNulls(GetDataColumnFromArg0(funcName, args));
			}
			return null;
		case "maxd":
			if (args.Parameters.Length != 0)
			{
				return Funcs.MaxD(GetDataColumnFromArg0(funcName, args));
			}
			return 0;
		case "maxi":
			if (args.Parameters.Length != 0)
			{
				return Funcs.MaxI(GetDataColumnFromArg0(funcName, args));
			}
			return 0;
		case "maxdate":
			if (args.Parameters.Length != 0)
			{
				return Funcs.MaxDate(GetDataColumnFromArg0(funcName, args));
			}
			return null;
		case "maxtime":
			if (args.Parameters.Length != 0)
			{
				return Funcs.MaxTime(GetDataColumnFromArg0(funcName, args));
			}
			return null;
		case "maxstr":
			if (args.Parameters.Length != 0)
			{
				return Funcs.MaxStr(GetDataColumnFromArg0(funcName, args));
			}
			return null;
		case "min":
			if (args.Parameters.Length != 0)
			{
				return Funcs.Min(GetDataColumnFromArg0(funcName, args));
			}
			return 0;
		case "minnulls":
			if (args.Parameters.Length != 0)
			{
				return Funcs.MinNulls(GetDataColumnFromArg0(funcName, args));
			}
			return null;
		case "mind":
			if (args.Parameters.Length != 0)
			{
				return Funcs.MinD(GetDataColumnFromArg0(funcName, args));
			}
			return 0;
		case "mini":
			if (args.Parameters.Length != 0)
			{
				return Funcs.MinI(GetDataColumnFromArg0(funcName, args));
			}
			return 0;
		case "mindate":
			if (args.Parameters.Length != 0)
			{
				return Funcs.MinDate(GetDataColumnFromArg0(funcName, args));
			}
			return null;
		case "minmaxdatestring":
			if (args.Parameters.Length != 0)
			{
				return Funcs.MinMaxDateString(GetDataColumnFromArg0(funcName, args));
			}
			return null;
		case "mintime":
			if (args.Parameters.Length != 0)
			{
				return Funcs.MinTime(GetDataColumnFromArg0(funcName, args));
			}
			return null;
		case "minstr":
			if (args.Parameters.Length != 0)
			{
				return Funcs.MinStr(GetDataColumnFromArg0(funcName, args));
			}
			return null;
		case "sum":
			if (args.Parameters.Length != 0)
			{
				return Funcs.Sum(GetDataColumnFromArg0(funcName, args));
			}
			return 0;
		case "sumnulls":
			if (args.Parameters.Length != 0)
			{
				return Funcs.SumNulls(GetDataColumnFromArg0(funcName, args));
			}
			return 0;
		case "sumd":
			if (args.Parameters.Length != 0)
			{
				return Funcs.SumD(GetDataColumnFromArg0(funcName, args));
			}
			return 0;
		case "sumi":
			if (args.Parameters.Length != 0)
			{
				return Funcs.SumI(GetDataColumnFromArg0(funcName, args));
			}
			return 0;
		case "sumdistinct":
			if (args.Parameters.Length != 0)
			{
				return Funcs.SumDistinct(GetDataColumnFromArg0(funcName, args));
			}
			return 0;
		case "sumtime":
			if (args.Parameters.Length != 0)
			{
				return Funcs.SumTime(GetDataColumnFromArg0(funcName, args));
			}
			return default(TimeSpan);
		case "sumif":
			if (args.Parameters.Length != 0)
			{
				return Funcs.SumIf(GetDataColumnFromArg0(funcName, args), GetObjectFromArg1("condition", funcName, args));
			}
			return 0;
		case "sumdif":
			if (args.Parameters.Length != 0)
			{
				return Funcs.SumDIf(GetDataColumnFromArg0(funcName, args), GetObjectFromArg1("condition", funcName, args));
			}
			return 0;
		case "sumiif":
			if (args.Parameters.Length != 0)
			{
				return Funcs.SumIIf(GetDataColumnFromArg0(funcName, args), GetObjectFromArg1("condition", funcName, args));
			}
			return 0;
		case "sumdistinctif":
			if (args.Parameters.Length != 0)
			{
				return Funcs.SumDistinctIf(GetDataColumnFromArg0(funcName, args), GetObjectFromArg1("condition", funcName, args));
			}
			return 0;
		case "sumtimeif":
			if (args.Parameters.Length != 0)
			{
				return Funcs.SumTimeIf(GetDataColumnFromArg0(funcName, args), GetObjectFromArg1("condition", funcName, args));
			}
			return 0;
		case "addmonths":
		{
			object objectFromArg29 = GetObjectFromArg(0, "date", funcName, args);
			int months = StiValueHelper.TryToInt(GetObjectFromArg(1, "months", funcName, args));
			return Funcs.AddMonthsObject(objectFromArg29, months);
		}
		case "addyear":
		{
			object objectFromArg28 = GetObjectFromArg(0, "date", funcName, args);
			int years = StiValueHelper.TryToInt(GetObjectFromArg(1, "years", funcName, args));
			return Funcs.AddYearsObject(objectFromArg28, years);
		}
		case "datediff":
			return Funcs.DateDiffObject(GetObjectFromArg0("date1", funcName, args), GetObjectFromArg1("date2", funcName, args));
		case "datetime":
			if (args.Parameters.Length != 0)
			{
				return Funcs.DateTime(GetDataColumnFromArg0(funcName, args));
			}
			return null;
		case "day":
			return Funcs.DayObject(GetObjectFromArg0("date", funcName, args));
		case "daysinmonth":
			if (args.Parameters.Length == 1)
			{
				return Funcs.DaysInMonthObject(GetObjectFromArg0("date", funcName, args));
			}
			return Funcs.DaysInMonthObject(GetObjectFromArg0("year", funcName, args), GetObjectFromArg1("month", funcName, args));
		case "daysinyear":
		{
			object objectFromArg20 = GetObjectFromArg0("value", funcName, args);
			if (objectFromArg20 is DateTime)
			{
				return Funcs.DaysInYearObject((DateTime)objectFromArg20);
			}
			return Funcs.DaysInYearObject(StiValueHelper.TryToInt(objectFromArg20));
		}
		case "dayofweek":
		{
			object objectFromArg15 = GetObjectFromArg0("date", funcName, args);
			if (args.Parameters.Length == 1)
			{
				return Funcs.DayOfWeekObject(objectFromArg15);
			}
			if (args.Parameters.Length == 2)
			{
				object objectFromArg16 = GetObjectFromArg1("localized", funcName, args);
				if (objectFromArg16 is bool)
				{
					return Funcs.DayOfWeekObject(objectFromArg15, (bool)objectFromArg16);
				}
				return Funcs.DayOfWeekObject(objectFromArg15, objectFromArg16.ToString());
			}
			if (args.Parameters.Length == 3)
			{
				object objectFromArg17 = GetObjectFromArg1("culture", funcName, args);
				object objectFromArg18 = GetObjectFromArg2("upperCase", funcName, args);
				return Funcs.DayOfWeekObject(objectFromArg15, objectFromArg17.ToString(), objectFromArg18 is bool && (bool)objectFromArg18);
			}
			throw new StiArgumentCountException(funcName);
		}
		case "dayofweekident":
			return Funcs.DayOfWeekIdentObject(GetObjectFromArg0("date", funcName, args));
		case "dayofweekindex":
			return Funcs.DayOfWeekIndexObject(GetObjectFromArg0("date", funcName, args));
		case "dayofyear":
			return Funcs.DayOfYearObject(GetObjectFromArg0("date", funcName, args));
		case "financialquarter":
			return Funcs.FinancialQuarterObject(GetObjectFromArg0("date", funcName, args));
		case "financialquarterindex":
			return Funcs.FinancialQuarterIndexObject(GetObjectFromArg0("date", funcName, args));
		case "hour":
			return Funcs.HourObject(GetObjectFromArg0("date", funcName, args));
		case "makedate":
		case "dateserial":
		{
			object objectFromArg11 = GetObjectFromArg(0, "year", funcName, args);
			object objectFromArg12 = GetObjectFromArg(1, "months", funcName, args);
			object objectFromArg13 = GetObjectFromArg(2, "day", funcName, args);
			return Funcs.MakeDateObject(objectFromArg11, objectFromArg12, objectFromArg13);
		}
		case "makedatetime":
		{
			object objectFromArg2 = GetObjectFromArg(0, "year", funcName, args);
			object objectFromArg3 = GetObjectFromArg(1, "months", funcName, args);
			object objectFromArg4 = GetObjectFromArg(2, "day", funcName, args);
			object objectFromArg5 = GetObjectFromArg(3, "hour", funcName, args);
			object objectFromArg6 = GetObjectFromArg(4, "minute", funcName, args);
			object objectFromArg7 = GetObjectFromArg(5, "second", funcName, args);
			return Funcs.MakeDateTimeObject(objectFromArg2, objectFromArg3, objectFromArg4, objectFromArg5, objectFromArg6, objectFromArg7);
		}
		case "maketime":
		case "timeserial":
		{
			object objectFromArg8 = GetObjectFromArg(0, "hour", funcName, args);
			object objectFromArg9 = GetObjectFromArg(1, "minute", funcName, args);
			object objectFromArg10 = GetObjectFromArg(2, "second", funcName, args);
			return Funcs.MakeTimeObject(objectFromArg8, objectFromArg9, objectFromArg10);
		}
		case "minute":
			return Funcs.MinuteObject(GetObjectFromArg0("date", funcName, args));
		case "month":
			return Funcs.MonthObject(GetObjectFromArg0("date", funcName, args));
		case "monthident":
			return Funcs.MonthIdentObject(GetObjectFromArg0("date", funcName, args));
		case "monthname":
		{
			object objectFromArg38 = GetObjectFromArg0("date", funcName, args);
			if (args.Parameters.Length == 1)
			{
				return Funcs.MonthNameObject(objectFromArg38);
			}
			if (args.Parameters.Length == 2)
			{
				object objectFromArg39 = GetObjectFromArg1("localized", funcName, args);
				if (objectFromArg39 is bool)
				{
					return Funcs.MonthNameObject(objectFromArg38, (bool)objectFromArg39);
				}
				return Funcs.MonthNameObject(objectFromArg38, objectFromArg39.ToString());
			}
			if (args.Parameters.Length == 3)
			{
				object objectFromArg40 = GetObjectFromArg1("culture", funcName, args);
				object objectFromArg41 = GetObjectFromArg2("upperCase", funcName, args);
				return Funcs.MonthNameObject(objectFromArg38, objectFromArg40.ToString(), objectFromArg41 is bool && (bool)objectFromArg41);
			}
			throw new StiArgumentCountException(funcName);
		}
		case "now":
			return Funcs.Now();
		case "fromoadate":
		{
			object objectFromArg37 = GetObjectFromArg(0, "value", funcName, args);
			return Funcs.FromOADateObject(objectFromArg37);
		}
		case "tooadate":
		{
			object objectFromArg36 = GetObjectFromArg(0, "value", funcName, args);
			return Funcs.ToOADateObject(objectFromArg36);
		}
		case "quarter":
			return Funcs.QuarterObject(GetObjectFromArg0("date", funcName, args));
		case "quarterindex":
			return Funcs.QuarterIndexObject(GetObjectFromArg0("date", funcName, args));
		case "quartername":
		{
			object objectFromArg34 = GetObjectFromArg0("date", funcName, args);
			if (args.Parameters.Length == 1)
			{
				return Funcs.QuarterNameObject(objectFromArg34, localized: false);
			}
			if (args.Parameters.Length == 2)
			{
				object objectFromArg35 = GetObjectFromArg1("localized", funcName, args);
				if (objectFromArg35 is bool)
				{
					return Funcs.QuarterNameObject(objectFromArg34, (bool)objectFromArg35);
				}
				return Funcs.QuarterNameObject(objectFromArg34);
			}
			throw new StiArgumentCountException(funcName);
		}
		case "second":
			return Funcs.SecondObject(GetObjectFromArg0("date", funcName, args));
		case "time":
			if (args.Parameters.Length != 0)
			{
				return Funcs.Time(GetDataColumnFromArg0(funcName, args));
			}
			return null;
		case "year":
			return Funcs.YearObject(GetObjectFromArg0("date", funcName, args));
		case "yearmonth":
			return Funcs.YearMonthObject(GetObjectFromArg0("date", funcName, args));
		case "insert":
		{
			object objectFromArg33 = GetObjectFromArg0("str", funcName, args);
			int startIndex3 = StiValueHelper.TryToInt(GetObjectFromArg(1, "startIndex", funcName, args));
			string subStr = StiValueHelper.TryToString(GetObjectFromArg(2, "value", funcName, args));
			return Funcs.InsertObject(objectFromArg33, startIndex3, subStr);
		}
		case "iso2":
		{
			object objectFromArg32 = GetObjectFromArg0("name", funcName, args);
			string mapId5 = ((args.Parameters.Length > 1) ? StiValueHelper.TryToString(GetObjectFromArg(1, "mapId", funcName, args)) : null);
			return Funcs.Iso2Object(objectFromArg32, mapId5, null);
		}
		case "iso2toname":
		{
			object objectFromArg31 = GetObjectFromArg0("alpha2", funcName, args);
			string mapId4 = ((args.Parameters.Length > 1) ? StiValueHelper.TryToString(GetObjectFromArg(1, "mapId", funcName, args)) : null);
			return Funcs.Iso2ToNameObject(objectFromArg31, mapId4, null);
		}
		case "iso3":
		{
			object objectFromArg30 = GetObjectFromArg0("name", funcName, args);
			string mapId3 = ((args.Parameters.Length > 1) ? StiValueHelper.TryToString(GetObjectFromArg(1, "mapId", funcName, args)) : null);
			return Funcs.Iso3Object(objectFromArg30, mapId3, null);
		}
		case "iso3toname":
		{
			object objectFromArg27 = GetObjectFromArg0("alpha2", funcName, args);
			string mapId2 = ((args.Parameters.Length > 1) ? StiValueHelper.TryToString(GetObjectFromArg(1, "mapId", funcName, args)) : null);
			return Funcs.Iso3ToNameObject(objectFromArg27, mapId2, null);
		}
		case "left":
		{
			object objectFromArg26 = GetObjectFromArg0("str", funcName, args);
			int length3 = StiValueHelper.TryToInt(GetObjectFromArg(1, "length", funcName, args));
			return Funcs.LeftObject(objectFromArg26, length3);
		}
		case "length":
			return Funcs.LengthObject(GetObjectFromArg0("str", funcName, args));
		case "normalizename":
		{
			object objectFromArg25 = GetObjectFromArg0("name", funcName, args);
			string mapId = ((args.Parameters.Length > 1) ? StiValueHelper.TryToString(GetObjectFromArg(1, "mapId", funcName, args)) : null);
			return Funcs.NormalizeNameObject(objectFromArg25, mapId, null);
		}
		case "remove":
		{
			object objectFromArg24 = GetObjectFromArg0("str", funcName, args);
			int startIndex2 = StiValueHelper.TryToInt(GetObjectFromArg(1, "startIndex", funcName, args));
			int count = StiValueHelper.TryToInt(GetObjectFromArg(2, "count", funcName, args));
			return Funcs.RemoveObject(objectFromArg24, startIndex2, count);
		}
		case "replace":
		{
			object objectFromArg23 = GetObjectFromArg0("str", funcName, args);
			string oldValue = StiValueHelper.TryToString(GetObjectFromArg(1, "oldValue", funcName, args));
			string newValue = StiValueHelper.TryToString(GetObjectFromArg(2, "newValue", funcName, args));
			return Funcs.ReplaceObject(objectFromArg23, oldValue, newValue);
		}
		case "right":
		{
			object objectFromArg22 = GetObjectFromArg0("str", funcName, args);
			int length2 = StiValueHelper.TryToInt(GetObjectFromArg(1, "length", funcName, args));
			return Funcs.RightObject(objectFromArg22, length2);
		}
		case "topropercase":
			return Funcs.ToProperCaseObject(GetObjectFromArg0("str", funcName, args));
		case "tolowercase":
			return Funcs.ToLowerCaseObject(GetObjectFromArg0("str", funcName, args));
		case "tostring":
			return Funcs.ToStringObject(GetObjectFromArg0("value", funcName, args));
		case "touppercase":
			return Funcs.ToUpperCaseObject(GetObjectFromArg0("str", funcName, args));
		case "substring":
		{
			object objectFromArg21 = GetObjectFromArg0("str", funcName, args);
			int startIndex = StiValueHelper.TryToInt(GetObjectFromArg(1, "index", funcName, args));
			int length = ((args.Parameters.Length > 2) ? StiValueHelper.TryToInt(GetObjectFromArg(2, "length", funcName, args)) : (-1));
			return Funcs.SubstringObject(objectFromArg21, startIndex, length);
		}
		case "trim":
			return Funcs.TrimObject(GetObjectFromArg0("str", funcName, args));
		case "trimstart":
			return Funcs.TrimStartObject(GetObjectFromArg0("str", funcName, args));
		case "trimend":
			return Funcs.TrimEndObject(GetObjectFromArg0("str", funcName, args));
		case "image":
		{
			object value = ((args.Parameters.Length > 1) ? GetObjectFromArg1("width", funcName, args) : null);
			object value2 = ((args.Parameters.Length > 2) ? GetObjectFromArg2("height", funcName, args) : null);
			int valueOrDefault = StiValueHelper.TryToNullableInt(value).GetValueOrDefault(200);
			int valueOrDefault2 = StiValueHelper.TryToNullableInt(value2).GetValueOrDefault(200);
			if (args.Parameters.Length != 0)
			{
				return Funcs.Image(GetDataColumnFromArg0(funcName, args), valueOrDefault, valueOrDefault2);
			}
			return null;
		}
		case "array":
			return args.Parameters.Select((Expression p) => p.Evaluate()).ToArray();
		case "list":
			return args.Parameters.Select((Expression p) => p.Evaluate()).ToList();
		case "iif":
		{
			object objectFromArg19 = GetObjectFromArg0("condition", funcName, args);
			if (StiValueHelper.TryToBool(objectFromArg19))
			{
				return GetObjectFromArg1("truePart", funcName, args);
			}
			return GetObjectFromArg2("falsePart", funcName, args);
		}
		case "choose":
		{
			object objectFromArg14 = GetObjectFromArg0("index", funcName, args);
			int num = StiValueHelper.TryToInt(objectFromArg14);
			List<object> list2 = (from p in args.Parameters.Skip(1)
				select p.Evaluate()).ToList();
			if (list2.Count == 0)
			{
				return null;
			}
			if (num < 1 || num > list2.Count)
			{
				return null;
			}
			return list2[num - 1];
		}
		case "switch":
		{
			List<object> list = args.Parameters.Select((Expression p) => p.Evaluate()).ToList();
			for (int i = 0; i < list.Count; i += 2)
			{
				object obj = list[i];
				object result = list[i + 1];
				if (obj is bool && (bool)obj)
				{
					return result;
				}
				if (obj is bool? flag && flag == true)
				{
					return result;
				}
			}
			return null;
		}
		case "abs":
			return Funcs.AbsObject(GetObjectFromArg0("value", funcName, args));
		case "acos":
			return Funcs.AcosObject(GetObjectFromArg0("value", funcName, args));
		case "asin":
			return Funcs.AsinObject(GetObjectFromArg0("value", funcName, args));
		case "atan":
			return Funcs.AtanObject(GetObjectFromArg0("value", funcName, args));
		case "ceiling":
			return Funcs.CeilingObject(GetObjectFromArg0("value", funcName, args));
		case "cos":
			return Funcs.CosObject(GetObjectFromArg0("value", funcName, args));
		case "div":
			return Funcs.DivObject(GetObjectFromArg(0, "value1", funcName, args), GetObjectFromArg(1, "value2", funcName, args), GetObjectFromArg(2, "zeroResult", funcName, args));
		case "exp":
			return Funcs.ExpObject(GetObjectFromArg0("value", funcName, args));
		case "floor":
			return Funcs.FloorObject(GetObjectFromArg0("value", funcName, args));
		case "log":
			return Funcs.LogObject(GetObjectFromArg0("value", funcName, args));
		case "round":
		{
			object objectFromArg = GetObjectFromArg0("value", funcName, args);
			int decimals = 0;
			if (args.Parameters.Length > 1)
			{
				decimals = StiValueHelper.TryToInt(GetObjectFromArg(1, "decimals", funcName, args));
			}
			return Funcs.RoundObject(objectFromArg, decimals);
		}
		case "sign":
			return Funcs.SignObject(GetObjectFromArg0("value", funcName, args));
		case "sin":
			return Funcs.SinObject(GetObjectFromArg0("value", funcName, args));
		case "sqrt":
			return Funcs.SqrtObject(GetObjectFromArg0("value", funcName, args));
		case "tan":
			return Funcs.TanObject(GetObjectFromArg0("value", funcName, args));
		case "truncate":
			return Funcs.TruncateObject(GetObjectFromArg0("value", funcName, args));
		default:
			if (Funcs.ExistsCustomFunction(funcName))
			{
				return Funcs.InvokeCustomFunction(funcName, EvaluateArgs(args));
			}
			throw new StiFunctionNotFoundException(funcName);
		}
	}

	protected object GetVariableValue(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return null;
		}
		bool flag = Dictionary != null && Dictionary.IsReadOnlyVariable(name);
		if (!flag && nameToValue.ContainsKey(name))
		{
			return nameToValue[name];
		}
		object obj = Dictionary?.GetVariableValueByName(name);
		if (!flag)
		{
			nameToValue[name] = obj;
		}
		return obj;
	}

	protected bool IsVariable(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return false;
		}
		if (nameToVariable.ContainsKey(name))
		{
			return true;
		}
		object obj = Dictionary?.GetVariableValueByName(name);
		if (obj != null)
		{
			nameToValue[name] = obj;
			return true;
		}
		return false;
	}

	protected bool IsSystemVariable(string name)
	{
		return name?.ToLowerInvariant() switch
		{
			"line" => true, 
			"lineabc" => true, 
			"lineroman" => true, 
			_ => Dictionary.IsSystemVariable(name), 
		};
	}

	protected object GetSystemVariableValue(string name)
	{
		return name?.ToLowerInvariant() switch
		{
			"line" => StiSystemVariableObject.Line, 
			"lineabc" => StiSystemVariableObject.LineABC, 
			"lineroman" => StiSystemVariableObject.LineRoman, 
			_ => Dictionary.GetSystemVariableValue(name), 
		};
	}

	private static object GetObjectFromArg(int argIndex, string argName, string funcName, FunctionArgs args)
	{
		if (argIndex >= args.Parameters.Length)
		{
			throw new StiArgumentNotFoundException(funcName, argName);
		}
		return args.Parameters[argIndex].Evaluate();
	}

	private static IEnumerable<object> EvaluateArgs(FunctionArgs args)
	{
		return args.Parameters.Select((Expression p) => p.Evaluate());
	}

	private static object GetObjectFromArg0(string argName, string funcName, FunctionArgs args)
	{
		return GetObjectFromArg(0, argName, funcName, args);
	}

	private static object GetObjectFromArg1(string argName, string funcName, FunctionArgs args)
	{
		return GetObjectFromArg(1, argName, funcName, args);
	}

	private static object GetObjectFromArg2(string argName, string funcName, FunctionArgs args)
	{
		return GetObjectFromArg(2, argName, funcName, args);
	}

	private static object GetDataColumnFromArg0(string funcName, FunctionArgs args)
	{
		return GetObjectFromArg0("dataColumn", funcName, args);
	}

	protected int GetDataColumnIndex(string columnName)
	{
		if (Table == null)
		{
			return -1;
		}
		if (nameToIndex.ContainsKey(columnName))
		{
			return nameToIndex[columnName];
		}
		string dataName = Funcs.ToDataName(columnName);
		DataColumn dataColumn = Table.Columns.Cast<DataColumn>().FirstOrDefault((DataColumn c) => DataEqual(c, dataName));
		if (dataColumn == null)
		{
			return -1;
		}
		int num = Table.Columns.IndexOf(dataColumn);
		if (num == -1)
		{
			return -1;
		}
		nameToIndex.Add(columnName, num);
		return num;
	}

	private bool DataEqual(DataColumn dataColumn, string searchColumnName)
	{
		string text = Funcs.ToDataName(dataColumn.ColumnName);
		if (!searchColumnName.Contains("."))
		{
			return false;
		}
		return text == searchColumnName;
	}

	protected int GetDimensionIndex(IStiDimensionMeter dimension)
	{
		return Meters.Where((IStiMeter m) => m is IStiDimensionMeter).ToList().IndexOf(dimension);
	}

	protected StiDataParser(IStiAppDictionary dictionary, DataTable table, List<IStiMeter> meters)
	{
		Dictionary = dictionary;
		Table = table;
		Meters = meters;
	}
}
