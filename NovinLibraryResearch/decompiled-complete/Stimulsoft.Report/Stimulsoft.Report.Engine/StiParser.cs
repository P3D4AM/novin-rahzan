using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Engine;

public class StiParser
{
	private enum ParserErrorCode
	{
		SyntaxError,
		IntegralConstantIsTooLarge,
		ExpressionIsEmpty,
		DivisionByZero,
		UnexpectedEndOfExpression,
		NameDoesNotExistInCurrentContext,
		UnprocessedLexemesRemain,
		LeftParenthesisExpected,
		RightParenthesisExpected,
		FieldMethodOrPropertyNotFound,
		OperatorCannotBeAppliedToOperands,
		FunctionNotFound,
		NoOverloadForMethodTakesNArguments,
		FunctionHasInvalidArgument,
		FunctionNotYetImplemented,
		MethodHasInvalidArgument,
		ItemDoesNotContainDefinition,
		NoMatchingOverloadedMethod,
		TheTypeOrNamespaceNotExistInTheNamespace
	}

	public class StiParserException : Exception
	{
		public string BaseMessage;

		public int Position = -1;

		public int Length = -1;

		public StiParserException(string message)
			: base(message)
		{
		}

		public StiParserException()
		{
		}
	}

	private class StiCheckTypeData
	{
		public Type TypeCode;

		public int Position = -1;

		public int Length = -1;

		public StiCheckTypeData(Type typeCode, int position, int length)
		{
			TypeCode = typeCode;
			Position = position;
			Length = length;
		}

		public override string ToString()
		{
			return $"{TypeCode}";
		}
	}

	public class StiParserData
	{
		public object Data;

		public List<StiAsmCommand> AsmList;

		public List<StiAsmCommand> AsmList2;

		public List<StiAsmCommand> ConditionAsmList;

		public StiParser Parser;

		public StiParserData(object data, List<StiAsmCommand> asmList, StiParser parser)
		{
			Data = data;
			AsmList = asmList;
			Parser = parser;
			ConditionAsmList = null;
		}

		public StiParserData(object data, List<StiAsmCommand> asmList, StiParser parser, List<StiAsmCommand> conditionAsmList)
		{
			Data = data;
			AsmList = asmList;
			Parser = parser;
			ConditionAsmList = conditionAsmList;
		}
	}

	public class StiFilterParserData
	{
		public StiComponent Component;

		public string Expression;

		public StiFilterParserData(StiComponent component, string expression)
		{
			Component = component;
			Expression = expression;
		}
	}

	public class StiToken
	{
		public StiTokenType Type;

		public string Value;

		public object ValueObject;

		public int Position = -1;

		public int Length = -1;

		public StiToken(StiTokenType type, int position, int length)
		{
			Type = type;
			Position = position;
			Length = length;
		}

		public StiToken(StiTokenType type, int position)
		{
			Type = type;
			Position = position;
		}

		public StiToken(StiTokenType type)
		{
			Type = type;
		}

		public StiToken()
		{
			Type = StiTokenType.Empty;
		}

		public override string ToString()
		{
			return string.Format("TokenType={0}{1}", Type.ToString(), (Value != null) ? $", value=\"{Value}\"" : "");
		}
	}

	public class StiAsmCommand
	{
		public StiAsmCommandType Type;

		public int ArgsCount;

		public object Parameter1;

		public object Parameter2;

		public int Position = -1;

		public int Length = -1;

		public StiAsmCommand(StiAsmCommandType type)
			: this(type, null, 0)
		{
		}

		public StiAsmCommand(StiAsmCommandType type, object parameter)
			: this(type, parameter, 0)
		{
		}

		public StiAsmCommand(StiAsmCommandType type, object parameter1, int argsCount, object parameter2 = null)
		{
			Type = type;
			Parameter1 = parameter1;
			ArgsCount = argsCount;
			Parameter2 = parameter2;
		}

		public override string ToString()
		{
			return string.Format("{0}({1},{2}{3})", Type.ToString(), (Parameter1 != null) ? Parameter1.ToString() : "null", ArgsCount, (Parameter2 != null) ? ",*" : "");
		}
	}

	public class StiParserMethodInfo
	{
		public StiFunctionType Name;

		public int Number;

		public Type[] Arguments;

		public Type ReturnType;

		public StiParserMethodInfo(StiFunctionType name, int number, Type[] arguments)
		{
			Name = name;
			Number = number;
			Arguments = arguments;
			ReturnType = typeof(string);
		}

		public StiParserMethodInfo(StiFunctionType name, int number, Type[] arguments, Type returnType)
		{
			Name = name;
			Number = number;
			Arguments = arguments;
			ReturnType = returnType;
		}
	}

	internal class StiRefVariableObject
	{
		public string Name;

		public object Value;

		public StiRefVariableObject(string name, object value)
		{
			Name = name;
			Value = value;
		}
	}

	internal class StiParserDataSourceFieldInfo
	{
		public List<string> Path;

		public List<object> Objects;

		public StiParserDataSourceFieldInfo()
		{
			Path = new List<string>();
			Objects = new List<object>();
		}
	}

	internal class StiParserCheckMethodInfo
	{
		public int LastOverload = -1;

		public Type[] LastArgsTypes;

		public StiParserCheckMethodInfo(int lastOverload, Type[] lastArgsTypes)
		{
			LastOverload = lastOverload;
			LastArgsTypes = lastArgsTypes;
		}
	}

	public class StiParserGetDataFieldValueEventArgs : EventArgs
	{
		public virtual string DataSourceName { get; set; }

		public virtual string DataColumnName { get; set; }

		public virtual bool Processed { get; set; }

		public virtual object Value { get; set; }

		public StiParserGetDataFieldValueEventArgs(string dataSourceName, string dataColumnName)
		{
			DataSourceName = dataSourceName;
			DataColumnName = dataColumnName;
		}
	}

	public delegate void StiParserGetDataFieldValueDelegate(object sender, StiParserGetDataFieldValueEventArgs e);

	public enum StiTokenType
	{
		Empty,
		Delimiter,
		Variable,
		SystemVariable,
		DataSourceField,
		BusinessObjectField,
		Number,
		Function,
		Method,
		Property,
		Component,
		Cast,
		String,
		Char,
		RefVariable,
		Dot,
		Comma,
		Colon,
		SemiColon,
		Shl,
		Shr,
		Assign,
		Equal,
		NotEqual,
		LeftEqual,
		Left,
		RightEqual,
		Right,
		Not,
		Or,
		And,
		Xor,
		DoubleOr,
		DoubleAnd,
		Question,
		Plus,
		Minus,
		Mult,
		Div,
		Percent,
		LParenthesis,
		RParenthesis,
		LBracket,
		RBracket,
		Identifier,
		Unknown
	}

	public enum StiAsmCommandType
	{
		PushValue = 2000,
		PushVariable = 2001,
		PushRefVariable = 2002,
		PushSystemVariable = 2003,
		PushDataSourceField = 2004,
		PushBusinessObjectField = 2005,
		PushFunction = 2006,
		PushMethod = 2007,
		PushProperty = 2008,
		PushComponent = 2009,
		PushArrayElement = 2010,
		CopyToVariable = 2011,
		Add = 2020,
		Sub = 2021,
		Mult = 2022,
		Div = 2023,
		Mod = 2024,
		Power = 2025,
		Neg = 2026,
		Cast = 2027,
		Not = 2028,
		CompareLeft = 2029,
		CompareLeftEqual = 2030,
		CompareRight = 2031,
		CompareRightEqual = 2032,
		CompareEqual = 2033,
		CompareNotEqual = 2034,
		Shl = 2035,
		Shr = 2036,
		And = 2037,
		And2 = 2038,
		Or = 2039,
		Or2 = 2040,
		Xor = 2041,
		Jump = 2042,
		JumpTrue = 2043,
		JumpFalse = 2044
	}

	public enum StiSystemVariableType
	{
		Column,
		Line,
		LineThrough,
		LineABC,
		LineRoman,
		GroupLine,
		PageNumber,
		PageNumberThrough,
		PageNofM,
		PageNofMThrough,
		TotalPageCount,
		TotalPageCountThrough,
		IsFirstPage,
		IsFirstPageThrough,
		IsLastPage,
		IsLastPageThrough,
		PageCopyNumber,
		ReportAlias,
		ReportAuthor,
		ReportChanged,
		ReportCreated,
		ReportDescription,
		ReportName,
		Time,
		Today,
		ConditionValue,
		ConditionValue2,
		ConditionTag,
		Sender,
		DateTimeNow,
		DateTimeToday,
		DateTimeUtcNow
	}

	public enum StiPropertyType
	{
		Year,
		Month,
		Day,
		Hour,
		Minute,
		Second,
		Date,
		DayOfWeek,
		Length,
		From,
		To,
		FromDate,
		ToDate,
		FromTime,
		ToTime,
		SelectedLine,
		Name,
		TagValue,
		Days,
		Hours,
		Milliseconds,
		Minutes,
		Seconds,
		Ticks,
		TotalDays,
		TotalHours,
		TotalMinutes,
		TotalSeconds,
		TotalMilliseconds,
		Count,
		BusinessObjectValue,
		Position,
		Line,
		Rows,
		SelectedIndex,
		SelectedValue,
		SelectedItem,
		Text,
		Value,
		Enabled,
		Skip
	}

	public enum StiFunctionType
	{
		NameSpace = 0,
		Count = 1,
		CountDistinct = 2,
		Avg = 3,
		AvgD = 4,
		AvgDate = 5,
		AvgI = 6,
		AvgTime = 7,
		Max = 8,
		MaxD = 9,
		MaxDate = 10,
		MaxI = 11,
		MaxStr = 12,
		MaxTime = 13,
		Median = 14,
		MedianD = 15,
		MedianI = 16,
		Min = 17,
		MinD = 18,
		MinDate = 19,
		MinI = 20,
		MinStr = 21,
		MinTime = 22,
		Mode = 23,
		ModeD = 24,
		ModeI = 25,
		Sum = 26,
		SumD = 27,
		SumDistinct = 28,
		SumI = 29,
		SumTime = 30,
		First = 31,
		Last = 32,
		rCount = 33,
		rCountDistinct = 34,
		rAvg = 35,
		rAvgD = 36,
		rAvgDate = 37,
		rAvgI = 38,
		rAvgTime = 39,
		rMax = 40,
		rMaxD = 41,
		rMaxDate = 42,
		rMaxI = 43,
		rMaxStr = 44,
		rMaxTime = 45,
		rMedian = 46,
		rMedianD = 47,
		rMedianI = 48,
		rMin = 49,
		rMinD = 50,
		rMinDate = 51,
		rMinI = 52,
		rMinStr = 53,
		rMinTime = 54,
		rMode = 55,
		rModeD = 56,
		rModeI = 57,
		rSum = 58,
		rSumD = 59,
		rSumDistinct = 60,
		rSumI = 61,
		rSumTime = 62,
		rFirst = 63,
		rLast = 64,
		iCount = 65,
		iCountDistinct = 66,
		iAvg = 67,
		iAvgD = 68,
		iAvgDate = 69,
		iAvgI = 70,
		iAvgTime = 71,
		iMax = 72,
		iMaxD = 73,
		iMaxDate = 74,
		iMaxI = 75,
		iMaxStr = 76,
		iMaxTime = 77,
		iMedian = 78,
		iMedianD = 79,
		iMedianI = 80,
		iMin = 81,
		iMinD = 82,
		iMinDate = 83,
		iMinI = 84,
		iMinStr = 85,
		iMinTime = 86,
		iMode = 87,
		iModeD = 88,
		iModeI = 89,
		iSum = 90,
		iSumD = 91,
		iSumDistinct = 92,
		iSumI = 93,
		iSumTime = 94,
		iFirst = 95,
		iLast = 96,
		riCount = 97,
		riCountDistinct = 98,
		riAvg = 99,
		riAvgD = 100,
		riAvgDate = 101,
		riAvgI = 102,
		riAvgTime = 103,
		riMax = 104,
		riMaxD = 105,
		riMaxDate = 106,
		riMaxI = 107,
		riMaxStr = 108,
		riMaxTime = 109,
		riMedian = 110,
		riMedianD = 111,
		riMedianI = 112,
		riMin = 113,
		riMinD = 114,
		riMinDate = 115,
		riMinI = 116,
		riMinStr = 117,
		riMinTime = 118,
		riMode = 119,
		riModeD = 120,
		riModeI = 121,
		riSum = 122,
		riSumD = 123,
		riSumDistinct = 124,
		riSumI = 125,
		riSumTime = 126,
		riFirst = 127,
		riLast = 128,
		cCount = 129,
		cCountDistinct = 130,
		cAvg = 131,
		cAvgD = 132,
		cAvgDate = 133,
		cAvgI = 134,
		cAvgTime = 135,
		cMax = 136,
		cMaxD = 137,
		cMaxDate = 138,
		cMaxI = 139,
		cMaxStr = 140,
		cMaxTime = 141,
		cMedian = 142,
		cMedianD = 143,
		cMedianI = 144,
		cMin = 145,
		cMinD = 146,
		cMinDate = 147,
		cMinI = 148,
		cMinStr = 149,
		cMinTime = 150,
		cMode = 151,
		cModeD = 152,
		cModeI = 153,
		cSum = 154,
		cSumD = 155,
		cSumDistinct = 156,
		cSumI = 157,
		cSumTime = 158,
		cFirst = 159,
		cLast = 160,
		crCount = 161,
		crCountDistinct = 162,
		crAvg = 163,
		crAvgD = 164,
		crAvgDate = 165,
		crAvgI = 166,
		crAvgTime = 167,
		crMax = 168,
		crMaxD = 169,
		crMaxDate = 170,
		crMaxI = 171,
		crMaxStr = 172,
		crMaxTime = 173,
		crMedian = 174,
		crMedianD = 175,
		crMedianI = 176,
		crMin = 177,
		crMinD = 178,
		crMinDate = 179,
		crMinI = 180,
		crMinStr = 181,
		crMinTime = 182,
		crMode = 183,
		crModeD = 184,
		crModeI = 185,
		crSum = 186,
		crSumD = 187,
		crSumDistinct = 188,
		crSumI = 189,
		crSumTime = 190,
		crFirst = 191,
		crLast = 192,
		ciCount = 193,
		ciCountDistinct = 194,
		ciAvg = 195,
		ciAvgD = 196,
		ciAvgDate = 197,
		ciAvgI = 198,
		ciAvgTime = 199,
		ciMax = 200,
		ciMaxD = 201,
		ciMaxDate = 202,
		ciMaxI = 203,
		ciMaxStr = 204,
		ciMaxTime = 205,
		ciMedian = 206,
		ciMedianD = 207,
		ciMedianI = 208,
		ciMin = 209,
		ciMinD = 210,
		ciMinDate = 211,
		ciMinI = 212,
		ciMinStr = 213,
		ciMinTime = 214,
		ciMode = 215,
		ciModeD = 216,
		ciModeI = 217,
		ciSum = 218,
		ciSumD = 219,
		ciSumDistinct = 220,
		ciSumI = 221,
		ciSumTime = 222,
		ciFirst = 223,
		ciLast = 224,
		criCount = 225,
		criCountDistinct = 226,
		criAvg = 227,
		criAvgD = 228,
		criAvgDate = 229,
		criAvgI = 230,
		criAvgTime = 231,
		criMax = 232,
		criMaxD = 233,
		criMaxDate = 234,
		criMaxI = 235,
		criMaxStr = 236,
		criMaxTime = 237,
		criMedian = 238,
		criMedianD = 239,
		criMedianI = 240,
		criMin = 241,
		criMinD = 242,
		criMinDate = 243,
		criMinI = 244,
		criMinStr = 245,
		criMinTime = 246,
		criMode = 247,
		criModeD = 248,
		criModeI = 249,
		criSum = 250,
		criSumD = 251,
		criSumDistinct = 252,
		criSumI = 253,
		criSumTime = 254,
		criFirst = 255,
		criLast = 256,
		pCount = 257,
		pCountDistinct = 258,
		pAvg = 259,
		pAvgD = 260,
		pAvgDate = 261,
		pAvgI = 262,
		pAvgTime = 263,
		pMax = 264,
		pMaxD = 265,
		pMaxDate = 266,
		pMaxI = 267,
		pMaxStr = 268,
		pMaxTime = 269,
		pMedian = 270,
		pMedianD = 271,
		pMedianI = 272,
		pMin = 273,
		pMinD = 274,
		pMinDate = 275,
		pMinI = 276,
		pMinStr = 277,
		pMinTime = 278,
		pMode = 279,
		pModeD = 280,
		pModeI = 281,
		pSum = 282,
		pSumD = 283,
		pSumDistinct = 284,
		pSumI = 285,
		pSumTime = 286,
		pFirst = 287,
		pLast = 288,
		prCount = 289,
		prCountDistinct = 290,
		prAvg = 291,
		prAvgD = 292,
		prAvgDate = 293,
		prAvgI = 294,
		prAvgTime = 295,
		prMax = 296,
		prMaxD = 297,
		prMaxDate = 298,
		prMaxI = 299,
		prMaxStr = 300,
		prMaxTime = 301,
		prMedian = 302,
		prMedianD = 303,
		prMedianI = 304,
		prMin = 305,
		prMinD = 306,
		prMinDate = 307,
		prMinI = 308,
		prMinStr = 309,
		prMinTime = 310,
		prMode = 311,
		prModeD = 312,
		prModeI = 313,
		prSum = 314,
		prSumD = 315,
		prSumDistinct = 316,
		prSumI = 317,
		prSumTime = 318,
		prFirst = 319,
		prLast = 320,
		piCount = 321,
		piCountDistinct = 322,
		piAvg = 323,
		piAvgD = 324,
		piAvgDate = 325,
		piAvgI = 326,
		piAvgTime = 327,
		piMax = 328,
		piMaxD = 329,
		piMaxDate = 330,
		piMaxI = 331,
		piMaxStr = 332,
		piMaxTime = 333,
		piMedian = 334,
		piMedianD = 335,
		piMedianI = 336,
		piMin = 337,
		piMinD = 338,
		piMinDate = 339,
		piMinI = 340,
		piMinStr = 341,
		piMinTime = 342,
		piMode = 343,
		piModeD = 344,
		piModeI = 345,
		piSum = 346,
		piSumD = 347,
		piSumDistinct = 348,
		piSumI = 349,
		piSumTime = 350,
		piFirst = 351,
		piLast = 352,
		priCount = 353,
		priCountDistinct = 354,
		priAvg = 355,
		priAvgD = 356,
		priAvgDate = 357,
		priAvgI = 358,
		priAvgTime = 359,
		priMax = 360,
		priMaxD = 361,
		priMaxDate = 362,
		priMaxI = 363,
		priMaxStr = 364,
		priMaxTime = 365,
		priMedian = 366,
		priMedianD = 367,
		priMedianI = 368,
		priMin = 369,
		priMinD = 370,
		priMinDate = 371,
		priMinI = 372,
		priMinStr = 373,
		priMinTime = 374,
		priMode = 375,
		priModeD = 376,
		priModeI = 377,
		priSum = 378,
		priSumD = 379,
		priSumDistinct = 380,
		priSumI = 381,
		priSumTime = 382,
		priFirst = 383,
		priLast = 384,
		CountAllLevels = 385,
		CountAllLevelsOnlyChilds = 386,
		CountOnlyChilds = 387,
		Rank = 388,
		Abs = 389,
		Acos = 390,
		Asin = 391,
		Atan = 392,
		Ceiling = 393,
		Cos = 394,
		Div = 395,
		Exp = 396,
		Floor = 397,
		Log = 398,
		Maximum = 399,
		Minimum = 400,
		Round = 401,
		Sign = 402,
		Sin = 403,
		Sqrt = 404,
		Tan = 405,
		Truncate = 406,
		DateDiff = 407,
		DateSerial = 408,
		Day = 409,
		DayOfWeek = 410,
		DayOfYear = 411,
		DaysInMonth = 412,
		DaysInYear = 413,
		Hour = 414,
		Minute = 415,
		Month = 416,
		Second = 417,
		TimeSerial = 418,
		Year = 419,
		MonthName = 420,
		WeekOfYear = 421,
		WeekOfMonth = 422,
		DateToStr = 423,
		DateToStrPl = 424,
		DateToStrRu = 425,
		DateToStrUa = 426,
		DateToStrPt = 427,
		DateToStrPtBr = 428,
		Insert = 429,
		Length = 430,
		Remove = 431,
		Replace = 432,
		Roman = 433,
		Substring = 434,
		ToCurrencyWords = 435,
		ToCurrencyWordsEnGb = 436,
		ToCurrencyWordsEnIn = 437,
		ToCurrencyWordsEs = 438,
		ToCurrencyWordsFr = 439,
		ToCurrencyWordsNl = 440,
		ToCurrencyWordsPl = 441,
		ToCurrencyWordsPt = 442,
		ToCurrencyWordsPtBr = 443,
		ToCurrencyWordsRu = 444,
		ToCurrencyWordsThai = 445,
		ToCurrencyWordsTr = 446,
		ToCurrencyWordsUa = 447,
		ToLowerCase = 448,
		ToProperCase = 449,
		ToUpperCase = 450,
		ToWords = 451,
		ToWordsEs = 452,
		ToWordsEnIn = 453,
		ToWordsFa = 454,
		ToWordsPl = 455,
		ToWordsPt = 456,
		ToWordsRu = 457,
		ToWordsTr = 458,
		ToWordsUa = 459,
		Trim = 460,
		TryParseDecimal = 461,
		TryParseDouble = 462,
		TryParseLong = 463,
		Arabic = 464,
		Persian = 465,
		ToOrdinal = 466,
		Left = 467,
		Mid = 468,
		Right = 469,
		StrToDateTime = 470,
		StrToNullableDateTime = 471,
		IsNull = 472,
		Next = 473,
		NextIsNull = 474,
		Previous = 475,
		PreviousIsNull = 476,
		IIF = 477,
		Choose = 478,
		Switch = 479,
		ToString = 480,
		Format = 481,
		SystemConvertToBoolean = 482,
		SystemConvertToByte = 483,
		SystemConvertToChar = 484,
		SystemConvertToDateTime = 485,
		SystemConvertToDecimal = 486,
		SystemConvertToDouble = 487,
		SystemConvertToInt16 = 488,
		SystemConvertToInt32 = 489,
		SystemConvertToInt64 = 490,
		SystemConvertToSByte = 491,
		SystemConvertToSingle = 492,
		SystemConvertToString = 493,
		SystemConvertToUInt16 = 494,
		SystemConvertToUInt32 = 495,
		SystemConvertToUInt64 = 496,
		MathRound = 497,
		MathPow = 498,
		AddAnchor = 499,
		GetAnchorPageNumber = 500,
		GetAnchorPageNumberThrough = 501,
		ConvertRtf = 502,
		GetLabel = 503,
		GetParam = 504,
		ParseInt = 505,
		ParseInt64 = 506,
		ParseDouble = 507,
		ParseDecimal = 508,
		ParseDateTime = 509,
		ParseTimeSpan = 510,
		StringIsNullOrEmpty = 511,
		StringIsNullOrWhiteSpace = 512,
		EngineHelperJoinColumnContent = 513,
		EngineHelperToQueryString = 514,
		EngineHelperGetRealPageNumber = 515,
		StiNullValuesHelperIsNull = 516,
		TimeSpanFromDays = 517,
		TimeSpanFromHours = 518,
		TimeSpanFromMilliseconds = 519,
		TimeSpanFromMinutes = 520,
		TimeSpanFromSeconds = 521,
		TimeSpanFromTicks = 522,
		ImageFromFile = 523,
		ConvertToBase64String = 524,
		m_Substring = 1000,
		m_ToString = 1001,
		m_ToLower = 1002,
		m_ToUpper = 1003,
		m_IndexOf = 1004,
		m_StartsWith = 1005,
		m_EndsWith = 1006,
		m_Replace = 1007,
		m_PadLeft = 1008,
		m_PadRight = 1009,
		m_TrimStart = 1010,
		m_TrimEnd = 1011,
		m_Parse = 1012,
		m_Contains = 1013,
		m_GetData = 1014,
		m_ToQueryString = 1015,
		m_AddYears = 1016,
		m_AddMonths = 1017,
		m_AddDays = 1018,
		m_AddHours = 1019,
		m_AddMinutes = 1020,
		m_AddSeconds = 1021,
		m_AddMilliseconds = 1022,
		m_ToShortDateString = 1023,
		m_ToShortTimeString = 1024,
		m_ToLongDateString = 1025,
		m_ToLongTimeString = 1026,
		m_GetCurrentConditionValue = 1027,
		m_Add = 1028,
		m_Subtract = 1029,
		m_MethodNameSpace = 1030,
		op_Add = 2020,
		op_Sub = 2021,
		op_Mult = 2022,
		op_Div = 2023,
		op_Mod = 2024,
		op_Power = 2025,
		op_Neg = 2026,
		op_Cast = 2027,
		op_Not = 2028,
		op_CompareLeft = 2029,
		op_CompareLeftEqual = 2030,
		op_CompareRight = 2031,
		op_CompareRightEqual = 2032,
		op_CompareEqual = 2033,
		op_CompareNotEqual = 2034,
		op_Shl = 2035,
		op_Shr = 2036,
		op_And = 2037,
		op_And2 = 2038,
		op_Or = 2039,
		op_Or2 = 2040,
		op_Xor = 2041,
		UserFunction = 3000
	}

	public enum StiMethodType
	{
		Substring = 1000,
		ToString,
		ToLower,
		ToUpper,
		IndexOf,
		StartsWith,
		EndsWith,
		Replace,
		PadLeft,
		PadRight,
		TrimStart,
		TrimEnd,
		Parse,
		Contains,
		GetData,
		ToQueryString,
		AddYears,
		AddMonths,
		AddDays,
		AddHours,
		AddMinutes,
		AddSeconds,
		AddMilliseconds,
		ToShortDateString,
		ToShortTimeString,
		ToLongDateString,
		ToLongTimeString,
		GetCurrentConditionValue,
		Add,
		Subtract,
		MethodNameSpace
	}

	[Flags]
	public enum StiParameterNumber
	{
		Param1 = 1,
		Param2 = 2,
		Param3 = 4,
		Param4 = 8
	}

	private static string[] errorsList = new string[19]
	{
		"Syntax error", "Integral constant is too large", "The expression is empty", "Division by zero", "Unexpected end of expression", "The name '{0}' does not exist in the current context", "Syntax error - unprocessed lexemes remain", "( expected", ") expected", "Field, method, or property is not found: '{0}'",
		"Operator '{0}' cannot be applied to operands of type '{1}' and type '{2}'", "The function is not found: '{0}'", "No overload for method '{0}' takes '{1}' arguments", "The '{0}' function has invalid argument '{1}': cannot convert from '{2}' to '{3}'", "The '{0}' function is not yet implemented", "The '{0}' method has invalid argument '{1}': cannot convert from '{2}' to '{3}'", "'{0}' does not contain a definition for '{1}'", "There is no matching overloaded method for '{0}({1})'", "The type or namespace name '{0}' does not exist in the namespace '{1}'"
	};

	private static readonly Dictionary<KeyValuePair<Type, Type>, bool> ImplicitCastCache = new Dictionary<KeyValuePair<Type, Type>, bool>
	{
		{
			new KeyValuePair<Type, Type>(typeof(string), typeof(int)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(string), typeof(uint)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(string), typeof(long)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(string), typeof(ulong)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(string), typeof(float)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(string), typeof(double)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(string), typeof(decimal)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(string), typeof(char)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(decimal), typeof(int)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(decimal), typeof(uint)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(decimal), typeof(long)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(decimal), typeof(ulong)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(decimal), typeof(double)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(decimal), typeof(float)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(decimal), typeof(string)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(double), typeof(int)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(double), typeof(uint)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(double), typeof(long)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(double), typeof(ulong)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(double), typeof(float)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(double), typeof(decimal)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(double), typeof(string)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(double), typeof(DateTime)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(double), typeof(TimeSpan)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(double), typeof(char)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(bool), typeof(int)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(bool), typeof(uint)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(bool), typeof(long)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(bool), typeof(ulong)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(bool), typeof(decimal)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(bool), typeof(double)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(bool), typeof(float)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(bool), typeof(char)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(bool), typeof(string)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(bool), typeof(DateTime)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(int), typeof(uint)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(int), typeof(ulong)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(int), typeof(string)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(int), typeof(char)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(int), typeof(DateTime)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(DBNull), typeof(double)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(DBNull), typeof(decimal)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(DBNull), typeof(ulong)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(DBNull), typeof(long)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(DBNull), typeof(uint)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(DBNull), typeof(int)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(StiBusinessObject), typeof(StiDataSource)),
			false
		},
		{
			new KeyValuePair<Type, Type>(typeof(StiDataSource), typeof(StiBusinessObject)),
			false
		}
	};

	private StiReport report;

	private string inputExpression = string.Empty;

	private StiComponent component;

	private object sender;

	private bool syntaxCaseSensitive = true;

	private bool checkSyntaxMode;

	private StiParserGetDataFieldValueDelegate getDataFieldValue;

	private bool useAliases;

	private StiParserParameters parameters;

	private int position;

	private List<StiToken> tokensList;

	private StiToken currentToken;

	private int tokenPos;

	private List<StiAsmCommand> asmList;

	private Hashtable hashAliases;

	private Hashtable runtimeConstants;

	private Hashtable runtimeConstantsHash;

	private int expressionPosition;

	private static Hashtable typesList = null;

	private static Hashtable typesList_low = null;

	private static Hashtable typesList_vb = null;

	private static Hashtable systemVariablesList = null;

	private static Hashtable systemVariablesList_low = null;

	private static Hashtable propertiesList = null;

	private static Hashtable propertiesList_low = null;

	private static Hashtable functionsList = null;

	private static Hashtable functionsList_low = null;

	private static Hashtable functionsList_vb = null;

	private static Hashtable methodsList = null;

	private static Hashtable methodsList_low = null;

	private static Hashtable parametersList = null;

	private Hashtable componentsList;

	private static object lockMethodsHash = new object();

	private static Hashtable methodsHash = null;

	private static Hashtable constantsList = null;

	private static Hashtable constantsList_low = null;

	private static object namespaceObj = new object();

	private static Hashtable namespacesList = null;

	private static Hashtable namespacesList_low = null;

	private object lockUserFunctionsList = new object();

	private Hashtable userFunctionsList;

	private static Hashtable operatorsList = null;

	private static Hashtable operatorsList_low = null;

	private static Hashtable operatorsList_vb = null;

	private static Hashtable keywordsList = null;

	private bool IsVB
	{
		get
		{
			if (report != null)
			{
				return report.ScriptLanguage == StiReportLanguageType.VB;
			}
			return false;
		}
	}

	public static bool ProcessIifAsTernary { get; set; } = false;


	private Hashtable TypesList
	{
		get
		{
			if (typesList == null)
			{
				typesList = new Hashtable();
				typesList["bool"] = TypeCode.Boolean;
				typesList["Boolean"] = TypeCode.Boolean;
				typesList["byte"] = TypeCode.Byte;
				typesList["Byte"] = TypeCode.Byte;
				typesList["sbyte"] = TypeCode.SByte;
				typesList["Sbyte"] = TypeCode.SByte;
				typesList["char"] = TypeCode.Char;
				typesList["Char"] = TypeCode.Char;
				typesList["decimal"] = TypeCode.Decimal;
				typesList["Decimal"] = TypeCode.Decimal;
				typesList["double"] = TypeCode.Double;
				typesList["Double"] = TypeCode.Double;
				typesList["float"] = TypeCode.Single;
				typesList["Single"] = TypeCode.Single;
				typesList["int"] = TypeCode.Int32;
				typesList["uint"] = TypeCode.UInt32;
				typesList["long"] = TypeCode.Int64;
				typesList["ulong"] = TypeCode.UInt64;
				typesList["short"] = TypeCode.Int16;
				typesList["Int16"] = TypeCode.Int16;
				typesList["Int32"] = TypeCode.Int32;
				typesList["Int64"] = TypeCode.Int64;
				typesList["ushort"] = TypeCode.UInt16;
				typesList["UInt16"] = TypeCode.UInt16;
				typesList["UInt32"] = TypeCode.UInt32;
				typesList["UInt64"] = TypeCode.UInt64;
				typesList["object"] = TypeCode.Object;
				typesList["string"] = TypeCode.String;
				typesList["String"] = TypeCode.String;
				typesList["DateTime"] = TypeCode.DateTime;
				typesList["TimeSpan"] = typeof(TimeSpan);
				typesList["List"] = typeof(List<>);
				typesList_low = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
				foreach (DictionaryEntry types in typesList)
				{
					typesList_low[((string)types.Key).ToLowerInvariant()] = types.Value;
				}
				typesList_vb = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
				typesList_vb["Boolean"] = TypeCode.Boolean;
				typesList_vb["Byte"] = TypeCode.Byte;
				typesList_vb["Char"] = TypeCode.Char;
				typesList_vb["Date"] = TypeCode.DateTime;
				typesList_vb["Decimal"] = TypeCode.Decimal;
				typesList_vb["Double"] = TypeCode.Double;
				typesList_vb["Integer"] = TypeCode.Int32;
				typesList_vb["Long"] = TypeCode.Int64;
				typesList_vb["Object"] = TypeCode.Object;
				typesList_vb["SByte"] = TypeCode.SByte;
				typesList_vb["Short"] = TypeCode.Int16;
				typesList_vb["Single"] = TypeCode.Single;
				typesList_vb["String"] = TypeCode.String;
				typesList_vb["UInteger"] = TypeCode.UInt32;
				typesList_vb["ULong"] = TypeCode.UInt64;
				typesList_vb["UShort"] = TypeCode.UInt16;
				typesList_vb["TimeSpan"] = typeof(TimeSpan);
			}
			if (IsVB)
			{
				return typesList_vb;
			}
			if (!syntaxCaseSensitive)
			{
				return typesList_low;
			}
			return typesList;
		}
	}

	private Hashtable SystemVariablesList
	{
		get
		{
			if (systemVariablesList == null)
			{
				systemVariablesList = new Hashtable();
				systemVariablesList["Column"] = StiSystemVariableType.Column;
				systemVariablesList["Line"] = StiSystemVariableType.Line;
				systemVariablesList["LineThrough"] = StiSystemVariableType.LineThrough;
				systemVariablesList["LineABC"] = StiSystemVariableType.LineABC;
				systemVariablesList["LineRoman"] = StiSystemVariableType.LineRoman;
				systemVariablesList["GroupLine"] = StiSystemVariableType.GroupLine;
				systemVariablesList["PageNumber"] = StiSystemVariableType.PageNumber;
				systemVariablesList["PageNumberThrough"] = StiSystemVariableType.PageNumberThrough;
				systemVariablesList["PageNofM"] = StiSystemVariableType.PageNofM;
				systemVariablesList["PageNofMThrough"] = StiSystemVariableType.PageNofMThrough;
				systemVariablesList["TotalPageCount"] = StiSystemVariableType.TotalPageCount;
				systemVariablesList["TotalPageCountThrough"] = StiSystemVariableType.TotalPageCountThrough;
				systemVariablesList["IsFirstPage"] = StiSystemVariableType.IsFirstPage;
				systemVariablesList["IsFirstPageThrough"] = StiSystemVariableType.IsFirstPageThrough;
				systemVariablesList["IsLastPage"] = StiSystemVariableType.IsLastPage;
				systemVariablesList["IsLastPageThrough"] = StiSystemVariableType.IsLastPageThrough;
				systemVariablesList["PageCopyNumber"] = StiSystemVariableType.PageCopyNumber;
				systemVariablesList["ReportAlias"] = StiSystemVariableType.ReportAlias;
				systemVariablesList["ReportAuthor"] = StiSystemVariableType.ReportAuthor;
				systemVariablesList["ReportChanged"] = StiSystemVariableType.ReportChanged;
				systemVariablesList["ReportCreated"] = StiSystemVariableType.ReportCreated;
				systemVariablesList["ReportDescription"] = StiSystemVariableType.ReportDescription;
				systemVariablesList["ReportName"] = StiSystemVariableType.ReportName;
				systemVariablesList["Time"] = StiSystemVariableType.Time;
				systemVariablesList["Today"] = StiSystemVariableType.Today;
				systemVariablesList["Date"] = StiSystemVariableType.Today;
				systemVariablesList["value"] = StiSystemVariableType.ConditionValue;
				systemVariablesList["value2"] = StiSystemVariableType.ConditionValue2;
				systemVariablesList["tag"] = StiSystemVariableType.ConditionTag;
				systemVariablesList["sender"] = StiSystemVariableType.Sender;
				systemVariablesList["DateTime.Now"] = StiSystemVariableType.DateTimeNow;
				systemVariablesList["DateTime.Today"] = StiSystemVariableType.DateTimeToday;
				systemVariablesList["DateTime.UtcNow"] = StiSystemVariableType.DateTimeUtcNow;
				systemVariablesList_low = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
				foreach (DictionaryEntry systemVariables in systemVariablesList)
				{
					systemVariablesList_low[((string)systemVariables.Key).ToLowerInvariant()] = systemVariables.Value;
				}
			}
			if (!syntaxCaseSensitive)
			{
				return systemVariablesList_low;
			}
			return systemVariablesList;
		}
	}

	private Hashtable PropertiesList
	{
		get
		{
			if (propertiesList == null)
			{
				propertiesList = new Hashtable();
				propertiesList["Year"] = StiPropertyType.Year;
				propertiesList["Month"] = StiPropertyType.Month;
				propertiesList["Day"] = StiPropertyType.Day;
				propertiesList["Hour"] = StiPropertyType.Hour;
				propertiesList["Minute"] = StiPropertyType.Minute;
				propertiesList["Second"] = StiPropertyType.Second;
				propertiesList["Date"] = StiPropertyType.Date;
				propertiesList["Length"] = StiPropertyType.Length;
				propertiesList["From"] = StiPropertyType.From;
				propertiesList["To"] = StiPropertyType.To;
				propertiesList["FromDate"] = StiPropertyType.FromDate;
				propertiesList["ToDate"] = StiPropertyType.ToDate;
				propertiesList["FromTime"] = StiPropertyType.FromTime;
				propertiesList["ToTime"] = StiPropertyType.ToTime;
				propertiesList["SelectedLine"] = StiPropertyType.SelectedLine;
				propertiesList["Name"] = StiPropertyType.Name;
				propertiesList["TagValue"] = StiPropertyType.TagValue;
				propertiesList["Days"] = StiPropertyType.Days;
				propertiesList["Hours"] = StiPropertyType.Hours;
				propertiesList["Milliseconds"] = StiPropertyType.Milliseconds;
				propertiesList["Minutes"] = StiPropertyType.Minutes;
				propertiesList["Seconds"] = StiPropertyType.Seconds;
				propertiesList["Ticks"] = StiPropertyType.Ticks;
				propertiesList["TotalDays"] = StiPropertyType.TotalDays;
				propertiesList["TotalHours"] = StiPropertyType.TotalHours;
				propertiesList["TotalMinutes"] = StiPropertyType.TotalMinutes;
				propertiesList["TotalSeconds"] = StiPropertyType.TotalSeconds;
				propertiesList["TotalMilliseconds"] = StiPropertyType.TotalMilliseconds;
				propertiesList["DayOfWeek"] = StiPropertyType.DayOfWeek;
				propertiesList["Count"] = StiPropertyType.Count;
				propertiesList["BusinessObjectValue"] = StiPropertyType.BusinessObjectValue;
				propertiesList["Position"] = StiPropertyType.Position;
				propertiesList["Line"] = StiPropertyType.Line;
				propertiesList["Rows"] = StiPropertyType.Rows;
				propertiesList["SelectedIndex"] = StiPropertyType.SelectedIndex;
				propertiesList["SelectedValue"] = StiPropertyType.SelectedValue;
				propertiesList["SelectedItem"] = StiPropertyType.SelectedItem;
				propertiesList["Text"] = StiPropertyType.Text;
				propertiesList["Value"] = StiPropertyType.Value;
				propertiesList["Enabled"] = StiPropertyType.Enabled;
				propertiesList["Skip"] = StiPropertyType.Skip;
				propertiesList_low = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
				foreach (DictionaryEntry properties in propertiesList)
				{
					propertiesList_low[((string)properties.Key).ToLowerInvariant()] = properties.Value;
				}
			}
			if (!syntaxCaseSensitive)
			{
				return propertiesList_low;
			}
			return propertiesList;
		}
	}

	private Hashtable FunctionsList
	{
		get
		{
			if (functionsList == null)
			{
				functionsList = new Hashtable();
				functionsList["Count"] = StiFunctionType.Count;
				functionsList["CountDistinct"] = StiFunctionType.CountDistinct;
				functionsList["Avg"] = StiFunctionType.Avg;
				functionsList["AvgD"] = StiFunctionType.AvgD;
				functionsList["AvgDate"] = StiFunctionType.AvgDate;
				functionsList["AvgI"] = StiFunctionType.AvgI;
				functionsList["AvgTime"] = StiFunctionType.AvgTime;
				functionsList["Max"] = StiFunctionType.Max;
				functionsList["MaxD"] = StiFunctionType.MaxD;
				functionsList["MaxDate"] = StiFunctionType.MaxDate;
				functionsList["MaxI"] = StiFunctionType.MaxI;
				functionsList["MaxStr"] = StiFunctionType.MaxStr;
				functionsList["MaxTime"] = StiFunctionType.MaxTime;
				functionsList["Median"] = StiFunctionType.Median;
				functionsList["MedianD"] = StiFunctionType.MedianD;
				functionsList["MedianI"] = StiFunctionType.MedianI;
				functionsList["Min"] = StiFunctionType.Min;
				functionsList["MinD"] = StiFunctionType.MinD;
				functionsList["MinDate"] = StiFunctionType.MinDate;
				functionsList["MinI"] = StiFunctionType.MinI;
				functionsList["MinStr"] = StiFunctionType.MinStr;
				functionsList["MinTime"] = StiFunctionType.MinTime;
				functionsList["Mode"] = StiFunctionType.Mode;
				functionsList["ModeD"] = StiFunctionType.ModeD;
				functionsList["ModeI"] = StiFunctionType.ModeI;
				functionsList["Sum"] = StiFunctionType.Sum;
				functionsList["SumD"] = StiFunctionType.SumD;
				functionsList["SumDistinct"] = StiFunctionType.SumDistinct;
				functionsList["SumI"] = StiFunctionType.SumI;
				functionsList["SumTime"] = StiFunctionType.SumTime;
				functionsList["First"] = StiFunctionType.First;
				functionsList["Last"] = StiFunctionType.Last;
				functionsList["CountRunning"] = StiFunctionType.rCount;
				functionsList["CountDistinctRunning"] = StiFunctionType.rCountDistinct;
				functionsList["AvgRunning"] = StiFunctionType.rAvg;
				functionsList["AvgDRunning"] = StiFunctionType.rAvgD;
				functionsList["AvgDateRunning"] = StiFunctionType.rAvgDate;
				functionsList["AvgIRunning"] = StiFunctionType.rAvgI;
				functionsList["AvgTimeRunning"] = StiFunctionType.rAvgTime;
				functionsList["MaxRunning"] = StiFunctionType.rMax;
				functionsList["MaxDRunning"] = StiFunctionType.rMaxD;
				functionsList["MaxDateRunning"] = StiFunctionType.rMaxDate;
				functionsList["MaxIRunning"] = StiFunctionType.rMaxI;
				functionsList["MaxStrRunning"] = StiFunctionType.rMaxStr;
				functionsList["MaxTimeRunning"] = StiFunctionType.rMaxTime;
				functionsList["MedianRunning"] = StiFunctionType.rMedian;
				functionsList["MedianDRunning"] = StiFunctionType.rMedianD;
				functionsList["MedianIRunning"] = StiFunctionType.rMedianI;
				functionsList["MinRunning"] = StiFunctionType.rMin;
				functionsList["MinDRunning"] = StiFunctionType.rMinD;
				functionsList["MinDateRunning"] = StiFunctionType.rMinDate;
				functionsList["MinIRunning"] = StiFunctionType.rMinI;
				functionsList["MinStrRunning"] = StiFunctionType.rMinStr;
				functionsList["MinTimeRunning"] = StiFunctionType.rMinTime;
				functionsList["ModeRunning"] = StiFunctionType.rMode;
				functionsList["ModeDRunning"] = StiFunctionType.rModeD;
				functionsList["ModeIRunning"] = StiFunctionType.rModeI;
				functionsList["SumRunning"] = StiFunctionType.rSum;
				functionsList["SumDRunning"] = StiFunctionType.rSumD;
				functionsList["SumDistinctRunning"] = StiFunctionType.rSumDistinct;
				functionsList["SumIRunning"] = StiFunctionType.rSumI;
				functionsList["SumTimeRunning"] = StiFunctionType.rSumTime;
				functionsList["FirstRunning"] = StiFunctionType.rFirst;
				functionsList["LastRunning"] = StiFunctionType.rLast;
				functionsList["CountIf"] = StiFunctionType.iCount;
				functionsList["CountDistinctIf"] = StiFunctionType.iCountDistinct;
				functionsList["AvgIf"] = StiFunctionType.iAvg;
				functionsList["AvgDIf"] = StiFunctionType.iAvgD;
				functionsList["AvgDateIf"] = StiFunctionType.iAvgDate;
				functionsList["AvgIIf"] = StiFunctionType.iAvgI;
				functionsList["AvgTimeIf"] = StiFunctionType.iAvgTime;
				functionsList["MaxIf"] = StiFunctionType.iMax;
				functionsList["MaxDIf"] = StiFunctionType.iMaxD;
				functionsList["MaxDateIf"] = StiFunctionType.iMaxDate;
				functionsList["MaxIIf"] = StiFunctionType.iMaxI;
				functionsList["MaxStrIf"] = StiFunctionType.iMaxStr;
				functionsList["MaxTimeIf"] = StiFunctionType.iMaxTime;
				functionsList["MedianIf"] = StiFunctionType.iMedian;
				functionsList["MedianDIf"] = StiFunctionType.iMedianD;
				functionsList["MedianIIf"] = StiFunctionType.iMedianI;
				functionsList["MinIf"] = StiFunctionType.iMin;
				functionsList["MinDIf"] = StiFunctionType.iMinD;
				functionsList["MinDateIf"] = StiFunctionType.iMinDate;
				functionsList["MinIIf"] = StiFunctionType.iMinI;
				functionsList["MinStrIf"] = StiFunctionType.iMinStr;
				functionsList["MinTimeIf"] = StiFunctionType.iMinTime;
				functionsList["ModeIf"] = StiFunctionType.iMode;
				functionsList["ModeDIf"] = StiFunctionType.iModeD;
				functionsList["ModeIIf"] = StiFunctionType.iModeI;
				functionsList["SumIf"] = StiFunctionType.iSum;
				functionsList["SumDIf"] = StiFunctionType.iSumD;
				functionsList["SumDistinctIf"] = StiFunctionType.iSumDistinct;
				functionsList["SumIIf"] = StiFunctionType.iSumI;
				functionsList["SumTimeIf"] = StiFunctionType.iSumTime;
				functionsList["FirstIf"] = StiFunctionType.iFirst;
				functionsList["LastIf"] = StiFunctionType.iLast;
				functionsList["CountIfRunning"] = StiFunctionType.riCount;
				functionsList["CountDistinctIfRunning"] = StiFunctionType.riCountDistinct;
				functionsList["AvgIfRunning"] = StiFunctionType.riAvg;
				functionsList["AvgDIfRunning"] = StiFunctionType.riAvgD;
				functionsList["AvgDateIfRunning"] = StiFunctionType.riAvgDate;
				functionsList["AvgIIfRunning"] = StiFunctionType.riAvgI;
				functionsList["AvgTimeIfRunning"] = StiFunctionType.riAvgTime;
				functionsList["MaxIfRunning"] = StiFunctionType.riMax;
				functionsList["MaxDIfRunning"] = StiFunctionType.riMaxD;
				functionsList["MaxDateIfRunning"] = StiFunctionType.riMaxDate;
				functionsList["MaxIIfRunning"] = StiFunctionType.riMaxI;
				functionsList["MaxStrIfRunning"] = StiFunctionType.riMaxStr;
				functionsList["MaxTimeIfRunning"] = StiFunctionType.riMaxTime;
				functionsList["MedianIfRunning"] = StiFunctionType.riMedian;
				functionsList["MedianDIfRunning"] = StiFunctionType.riMedianD;
				functionsList["MedianIIfRunning"] = StiFunctionType.riMedianI;
				functionsList["MinIfRunning"] = StiFunctionType.riMin;
				functionsList["MinDIfRunning"] = StiFunctionType.riMinD;
				functionsList["MinDateIfRunning"] = StiFunctionType.riMinDate;
				functionsList["MinIIfRunning"] = StiFunctionType.riMinI;
				functionsList["MinStrIfRunning"] = StiFunctionType.riMinStr;
				functionsList["MinTimeIfRunning"] = StiFunctionType.riMinTime;
				functionsList["ModeIfRunning"] = StiFunctionType.riMode;
				functionsList["ModeDIfRunning"] = StiFunctionType.riModeD;
				functionsList["ModeIIfRunning"] = StiFunctionType.riModeI;
				functionsList["SumIfRunning"] = StiFunctionType.riSum;
				functionsList["SumDIfRunning"] = StiFunctionType.riSumD;
				functionsList["SumDistinctIfRunning"] = StiFunctionType.riSumDistinct;
				functionsList["SumIIfRunning"] = StiFunctionType.riSumI;
				functionsList["SumTimeIfRunning"] = StiFunctionType.riSumTime;
				functionsList["FirstIfRunning"] = StiFunctionType.riFirst;
				functionsList["LastIfRunning"] = StiFunctionType.riLast;
				functionsList["colCount"] = StiFunctionType.cCount;
				functionsList["colCountDistinct"] = StiFunctionType.cCountDistinct;
				functionsList["colAvg"] = StiFunctionType.cAvg;
				functionsList["colAvgD"] = StiFunctionType.cAvgD;
				functionsList["colAvgDate"] = StiFunctionType.cAvgDate;
				functionsList["colAvgI"] = StiFunctionType.cAvgI;
				functionsList["colAvgTime"] = StiFunctionType.cAvgTime;
				functionsList["colMax"] = StiFunctionType.cMax;
				functionsList["colMaxD"] = StiFunctionType.cMaxD;
				functionsList["colMaxDate"] = StiFunctionType.cMaxDate;
				functionsList["colMaxI"] = StiFunctionType.cMaxI;
				functionsList["colMaxStr"] = StiFunctionType.cMaxStr;
				functionsList["colMaxTime"] = StiFunctionType.cMaxTime;
				functionsList["colMedian"] = StiFunctionType.cMedian;
				functionsList["colMedianD"] = StiFunctionType.cMedianD;
				functionsList["colMedianI"] = StiFunctionType.cMedianI;
				functionsList["colMin"] = StiFunctionType.cMin;
				functionsList["colMinD"] = StiFunctionType.cMinD;
				functionsList["colMinDate"] = StiFunctionType.cMinDate;
				functionsList["colMinI"] = StiFunctionType.cMinI;
				functionsList["colMinStr"] = StiFunctionType.cMinStr;
				functionsList["colMinTime"] = StiFunctionType.cMinTime;
				functionsList["colMode"] = StiFunctionType.cMode;
				functionsList["colModeD"] = StiFunctionType.cModeD;
				functionsList["colModeI"] = StiFunctionType.cModeI;
				functionsList["colSum"] = StiFunctionType.cSum;
				functionsList["colSumD"] = StiFunctionType.cSumD;
				functionsList["colSumDistinct"] = StiFunctionType.cSumDistinct;
				functionsList["colSumI"] = StiFunctionType.cSumI;
				functionsList["colSumTime"] = StiFunctionType.cSumTime;
				functionsList["colFirst"] = StiFunctionType.cFirst;
				functionsList["colLast"] = StiFunctionType.cLast;
				functionsList["colCountRunning"] = StiFunctionType.crCount;
				functionsList["colCountDistinctRunning"] = StiFunctionType.crCountDistinct;
				functionsList["colAvgRunning"] = StiFunctionType.crAvg;
				functionsList["colAvgDRunning"] = StiFunctionType.crAvgD;
				functionsList["colAvgDateRunning"] = StiFunctionType.crAvgDate;
				functionsList["colAvgIRunning"] = StiFunctionType.crAvgI;
				functionsList["colAvgTimeRunning"] = StiFunctionType.crAvgTime;
				functionsList["colMaxRunning"] = StiFunctionType.crMax;
				functionsList["colMaxDRunning"] = StiFunctionType.crMaxD;
				functionsList["colMaxDateRunning"] = StiFunctionType.crMaxDate;
				functionsList["colMaxIRunning"] = StiFunctionType.crMaxI;
				functionsList["colMaxStrRunning"] = StiFunctionType.crMaxStr;
				functionsList["colMaxTimeRunning"] = StiFunctionType.crMaxTime;
				functionsList["colMedianRunning"] = StiFunctionType.crMedian;
				functionsList["colMedianDRunning"] = StiFunctionType.crMedianD;
				functionsList["colMedianIRunning"] = StiFunctionType.crMedianI;
				functionsList["colMinRunning"] = StiFunctionType.crMin;
				functionsList["colMinDRunning"] = StiFunctionType.crMinD;
				functionsList["colMinDateRunning"] = StiFunctionType.crMinDate;
				functionsList["colMinIRunning"] = StiFunctionType.crMinI;
				functionsList["colMinStrRunning"] = StiFunctionType.crMinStr;
				functionsList["colMinTimeRunning"] = StiFunctionType.crMinTime;
				functionsList["colModeRunning"] = StiFunctionType.crMode;
				functionsList["colModeDRunning"] = StiFunctionType.crModeD;
				functionsList["colModeIRunning"] = StiFunctionType.crModeI;
				functionsList["colSumRunning"] = StiFunctionType.crSum;
				functionsList["colSumDRunning"] = StiFunctionType.crSumD;
				functionsList["colSumDistinctRunning"] = StiFunctionType.crSumDistinct;
				functionsList["colSumIRunning"] = StiFunctionType.crSumI;
				functionsList["colSumTimeRunning"] = StiFunctionType.crSumTime;
				functionsList["colFirstRunning"] = StiFunctionType.crFirst;
				functionsList["colLastRunning"] = StiFunctionType.crLast;
				functionsList["colCountIf"] = StiFunctionType.ciCount;
				functionsList["colCountDistinctIf"] = StiFunctionType.ciCountDistinct;
				functionsList["colAvgIf"] = StiFunctionType.ciAvg;
				functionsList["colAvgDIf"] = StiFunctionType.ciAvgD;
				functionsList["colAvgDateIf"] = StiFunctionType.ciAvgDate;
				functionsList["colAvgIIf"] = StiFunctionType.ciAvgI;
				functionsList["colAvgTimeIf"] = StiFunctionType.ciAvgTime;
				functionsList["colMaxIf"] = StiFunctionType.ciMax;
				functionsList["colMaxDIf"] = StiFunctionType.ciMaxD;
				functionsList["colMaxDateIf"] = StiFunctionType.ciMaxDate;
				functionsList["colMaxIIf"] = StiFunctionType.ciMaxI;
				functionsList["colMaxStrIf"] = StiFunctionType.ciMaxStr;
				functionsList["colMaxTimeIf"] = StiFunctionType.ciMaxTime;
				functionsList["colMedianIf"] = StiFunctionType.ciMedian;
				functionsList["colMedianDIf"] = StiFunctionType.ciMedianD;
				functionsList["colMedianIIf"] = StiFunctionType.ciMedianI;
				functionsList["colMinIf"] = StiFunctionType.ciMin;
				functionsList["colMinDIf"] = StiFunctionType.ciMinD;
				functionsList["colMinDateIf"] = StiFunctionType.ciMinDate;
				functionsList["colMinIIf"] = StiFunctionType.ciMinI;
				functionsList["colMinStrIf"] = StiFunctionType.ciMinStr;
				functionsList["colMinTimeIf"] = StiFunctionType.ciMinTime;
				functionsList["colModeIf"] = StiFunctionType.ciMode;
				functionsList["colModeDIf"] = StiFunctionType.ciModeD;
				functionsList["colModeIIf"] = StiFunctionType.ciModeI;
				functionsList["colSumIf"] = StiFunctionType.ciSum;
				functionsList["colSumDIf"] = StiFunctionType.ciSumD;
				functionsList["colSumDistinctIf"] = StiFunctionType.ciSumDistinct;
				functionsList["colSumIIf"] = StiFunctionType.ciSumI;
				functionsList["colSumTimeIf"] = StiFunctionType.ciSumTime;
				functionsList["colFirstIf"] = StiFunctionType.ciFirst;
				functionsList["colLastIf"] = StiFunctionType.ciLast;
				functionsList["colCountIfRunning"] = StiFunctionType.criCount;
				functionsList["colCountDistinctIfRunning"] = StiFunctionType.criCountDistinct;
				functionsList["colAvgIfRunning"] = StiFunctionType.criAvg;
				functionsList["colAvgDIfRunning"] = StiFunctionType.criAvgD;
				functionsList["colAvgDateIfRunning"] = StiFunctionType.criAvgDate;
				functionsList["colAvgIIfRunning"] = StiFunctionType.criAvgI;
				functionsList["colAvgTimeIfRunning"] = StiFunctionType.criAvgTime;
				functionsList["colMaxIfRunning"] = StiFunctionType.criMax;
				functionsList["colMaxDIfRunning"] = StiFunctionType.criMaxD;
				functionsList["colMaxDateIfRunning"] = StiFunctionType.criMaxDate;
				functionsList["colMaxIIfRunning"] = StiFunctionType.criMaxI;
				functionsList["colMaxStrIfRunning"] = StiFunctionType.criMaxStr;
				functionsList["colMaxTimeIfRunning"] = StiFunctionType.criMaxTime;
				functionsList["colMedianIfRunning"] = StiFunctionType.criMedian;
				functionsList["colMedianDIfRunning"] = StiFunctionType.criMedianD;
				functionsList["colMedianIIfRunning"] = StiFunctionType.criMedianI;
				functionsList["colMinIfRunning"] = StiFunctionType.criMin;
				functionsList["colMinDIfRunning"] = StiFunctionType.criMinD;
				functionsList["colMinDateIfRunning"] = StiFunctionType.criMinDate;
				functionsList["colMinIIfRunning"] = StiFunctionType.criMinI;
				functionsList["colMinStrIfRunning"] = StiFunctionType.criMinStr;
				functionsList["colMinTimeIfRunning"] = StiFunctionType.criMinTime;
				functionsList["colModeIfRunning"] = StiFunctionType.criMode;
				functionsList["colModeDIfRunning"] = StiFunctionType.criModeD;
				functionsList["colModeIIfRunning"] = StiFunctionType.criModeI;
				functionsList["colSumIfRunning"] = StiFunctionType.criSum;
				functionsList["colSumDIfRunning"] = StiFunctionType.criSumD;
				functionsList["colSumDistinctIfRunning"] = StiFunctionType.criSumDistinct;
				functionsList["colSumIIfRunning"] = StiFunctionType.criSumI;
				functionsList["colSumTimeIfRunning"] = StiFunctionType.criSumTime;
				functionsList["colFirstIfRunning"] = StiFunctionType.criFirst;
				functionsList["colLastIfRunning"] = StiFunctionType.criLast;
				functionsList["cCount"] = StiFunctionType.pCount;
				functionsList["cCountDistinct"] = StiFunctionType.pCountDistinct;
				functionsList["cAvg"] = StiFunctionType.pAvg;
				functionsList["cAvgD"] = StiFunctionType.pAvgD;
				functionsList["cAvgDate"] = StiFunctionType.pAvgDate;
				functionsList["cAvgI"] = StiFunctionType.pAvgI;
				functionsList["cAvgTime"] = StiFunctionType.pAvgTime;
				functionsList["cMax"] = StiFunctionType.pMax;
				functionsList["cMaxD"] = StiFunctionType.pMaxD;
				functionsList["cMaxDate"] = StiFunctionType.pMaxDate;
				functionsList["cMaxI"] = StiFunctionType.pMaxI;
				functionsList["cMaxStr"] = StiFunctionType.pMaxStr;
				functionsList["cMaxTime"] = StiFunctionType.pMaxTime;
				functionsList["cMedian"] = StiFunctionType.pMedian;
				functionsList["cMedianD"] = StiFunctionType.pMedianD;
				functionsList["cMedianI"] = StiFunctionType.pMedianI;
				functionsList["cMin"] = StiFunctionType.pMin;
				functionsList["cMinD"] = StiFunctionType.pMinD;
				functionsList["cMinDate"] = StiFunctionType.pMinDate;
				functionsList["cMinI"] = StiFunctionType.pMinI;
				functionsList["cMinStr"] = StiFunctionType.pMinStr;
				functionsList["cMinTime"] = StiFunctionType.pMinTime;
				functionsList["cMode"] = StiFunctionType.pMode;
				functionsList["cModeD"] = StiFunctionType.pModeD;
				functionsList["cModeI"] = StiFunctionType.pModeI;
				functionsList["cSum"] = StiFunctionType.pSum;
				functionsList["cSumD"] = StiFunctionType.pSumD;
				functionsList["cSumDistinct"] = StiFunctionType.pSumDistinct;
				functionsList["cSumI"] = StiFunctionType.pSumI;
				functionsList["cSumTime"] = StiFunctionType.pSumTime;
				functionsList["cFirst"] = StiFunctionType.pFirst;
				functionsList["cLast"] = StiFunctionType.pLast;
				functionsList["cCountRunning"] = StiFunctionType.prCount;
				functionsList["cCountDistinctRunning"] = StiFunctionType.prCountDistinct;
				functionsList["cAvgRunning"] = StiFunctionType.prAvg;
				functionsList["cAvgDRunning"] = StiFunctionType.prAvgD;
				functionsList["cAvgDateRunning"] = StiFunctionType.prAvgDate;
				functionsList["cAvgIRunning"] = StiFunctionType.prAvgI;
				functionsList["cAvgTimeRunning"] = StiFunctionType.prAvgTime;
				functionsList["cMaxRunning"] = StiFunctionType.prMax;
				functionsList["cMaxDRunning"] = StiFunctionType.prMaxD;
				functionsList["cMaxDateRunning"] = StiFunctionType.prMaxDate;
				functionsList["cMaxIRunning"] = StiFunctionType.prMaxI;
				functionsList["cMaxStrRunning"] = StiFunctionType.prMaxStr;
				functionsList["cMaxTimeRunning"] = StiFunctionType.prMaxTime;
				functionsList["cMedianRunning"] = StiFunctionType.prMedian;
				functionsList["cMedianDRunning"] = StiFunctionType.prMedianD;
				functionsList["cMedianIRunning"] = StiFunctionType.prMedianI;
				functionsList["cMinRunning"] = StiFunctionType.prMin;
				functionsList["cMinDRunning"] = StiFunctionType.prMinD;
				functionsList["cMinDateRunning"] = StiFunctionType.prMinDate;
				functionsList["cMinIRunning"] = StiFunctionType.prMinI;
				functionsList["cMinStrRunning"] = StiFunctionType.prMinStr;
				functionsList["cMinTimeRunning"] = StiFunctionType.prMinTime;
				functionsList["cModeRunning"] = StiFunctionType.prMode;
				functionsList["cModeDRunning"] = StiFunctionType.prModeD;
				functionsList["cModeIRunning"] = StiFunctionType.prModeI;
				functionsList["cSumRunning"] = StiFunctionType.prSum;
				functionsList["cSumDRunning"] = StiFunctionType.prSumD;
				functionsList["cSumDistinctRunning"] = StiFunctionType.prSumDistinct;
				functionsList["cSumIRunning"] = StiFunctionType.prSumI;
				functionsList["cSumTimeRunning"] = StiFunctionType.prSumTime;
				functionsList["cFirstRunning"] = StiFunctionType.prFirst;
				functionsList["cLastRunning"] = StiFunctionType.prLast;
				functionsList["cCountIf"] = StiFunctionType.piCount;
				functionsList["cCountDistinctIf"] = StiFunctionType.piCountDistinct;
				functionsList["cAvgIf"] = StiFunctionType.piAvg;
				functionsList["cAvgDIf"] = StiFunctionType.piAvgD;
				functionsList["cAvgDateIf"] = StiFunctionType.piAvgDate;
				functionsList["cAvgIIf"] = StiFunctionType.piAvgI;
				functionsList["cAvgTimeIf"] = StiFunctionType.piAvgTime;
				functionsList["cMaxIf"] = StiFunctionType.piMax;
				functionsList["cMaxDIf"] = StiFunctionType.piMaxD;
				functionsList["cMaxDateIf"] = StiFunctionType.piMaxDate;
				functionsList["cMaxIIf"] = StiFunctionType.piMaxI;
				functionsList["cMaxStrIf"] = StiFunctionType.piMaxStr;
				functionsList["cMaxTimeIf"] = StiFunctionType.piMaxTime;
				functionsList["cMedianIf"] = StiFunctionType.piMedian;
				functionsList["cMedianDIf"] = StiFunctionType.piMedianD;
				functionsList["cMedianIIf"] = StiFunctionType.piMedianI;
				functionsList["cMinIf"] = StiFunctionType.piMin;
				functionsList["cMinDIf"] = StiFunctionType.piMinD;
				functionsList["cMinDateIf"] = StiFunctionType.piMinDate;
				functionsList["cMinIIf"] = StiFunctionType.piMinI;
				functionsList["cMinStrIf"] = StiFunctionType.piMinStr;
				functionsList["cMinTimeIf"] = StiFunctionType.piMinTime;
				functionsList["cModeIf"] = StiFunctionType.piMode;
				functionsList["cModeDIf"] = StiFunctionType.piModeD;
				functionsList["cModeIIf"] = StiFunctionType.piModeI;
				functionsList["cSumIf"] = StiFunctionType.piSum;
				functionsList["cSumDIf"] = StiFunctionType.piSumD;
				functionsList["cSumDistinctIf"] = StiFunctionType.piSumDistinct;
				functionsList["cSumIIf"] = StiFunctionType.piSumI;
				functionsList["cSumTimeIf"] = StiFunctionType.piSumTime;
				functionsList["cFirstIf"] = StiFunctionType.piFirst;
				functionsList["cLastIf"] = StiFunctionType.piLast;
				functionsList["cCountIfRunning"] = StiFunctionType.priCount;
				functionsList["cCountDistinctIfRunning"] = StiFunctionType.priCountDistinct;
				functionsList["cAvgIfRunning"] = StiFunctionType.priAvg;
				functionsList["cAvgDIfRunning"] = StiFunctionType.priAvgD;
				functionsList["cAvgDateIfRunning"] = StiFunctionType.priAvgDate;
				functionsList["cAvgIIfRunning"] = StiFunctionType.priAvgI;
				functionsList["cAvgTimeIfRunning"] = StiFunctionType.priAvgTime;
				functionsList["cMaxIfRunning"] = StiFunctionType.priMax;
				functionsList["cMaxDIfRunning"] = StiFunctionType.priMaxD;
				functionsList["cMaxDateIfRunning"] = StiFunctionType.priMaxDate;
				functionsList["cMaxIIfRunning"] = StiFunctionType.priMaxI;
				functionsList["cMaxStrIfRunning"] = StiFunctionType.priMaxStr;
				functionsList["cMaxTimeIfRunning"] = StiFunctionType.priMaxTime;
				functionsList["cMedianIfRunning"] = StiFunctionType.priMedian;
				functionsList["cMedianDIfRunning"] = StiFunctionType.priMedianD;
				functionsList["cMedianIIfRunning"] = StiFunctionType.priMedianI;
				functionsList["cMinIfRunning"] = StiFunctionType.priMin;
				functionsList["cMinDIfRunning"] = StiFunctionType.priMinD;
				functionsList["cMinDateIfRunning"] = StiFunctionType.priMinDate;
				functionsList["cMinIIfRunning"] = StiFunctionType.priMinI;
				functionsList["cMinStrIfRunning"] = StiFunctionType.priMinStr;
				functionsList["cMinTimeIfRunning"] = StiFunctionType.priMinTime;
				functionsList["cModeIfRunning"] = StiFunctionType.priMode;
				functionsList["cModeDIfRunning"] = StiFunctionType.priModeD;
				functionsList["cModeIIfRunning"] = StiFunctionType.priModeI;
				functionsList["cSumIfRunning"] = StiFunctionType.priSum;
				functionsList["cSumDIfRunning"] = StiFunctionType.priSumD;
				functionsList["cSumDistinctIfRunning"] = StiFunctionType.priSumDistinct;
				functionsList["cSumIIfRunning"] = StiFunctionType.priSumI;
				functionsList["cSumTimeIfRunning"] = StiFunctionType.priSumTime;
				functionsList["cFirstIfRunning"] = StiFunctionType.priFirst;
				functionsList["cLastIfRunning"] = StiFunctionType.priLast;
				functionsList["Totals.Count"] = StiFunctionType.Count;
				functionsList["Totals.CountDistinct"] = StiFunctionType.CountDistinct;
				functionsList["Totals.Avg"] = StiFunctionType.Avg;
				functionsList["Totals.AvgD"] = StiFunctionType.AvgD;
				functionsList["Totals.AvgDate"] = StiFunctionType.AvgDate;
				functionsList["Totals.AvgI"] = StiFunctionType.AvgI;
				functionsList["Totals.AvgTime"] = StiFunctionType.AvgTime;
				functionsList["Totals.Max"] = StiFunctionType.Max;
				functionsList["Totals.MaxD"] = StiFunctionType.MaxD;
				functionsList["Totals.MaxDate"] = StiFunctionType.MaxDate;
				functionsList["Totals.MaxI"] = StiFunctionType.MaxI;
				functionsList["Totals.MaxStr"] = StiFunctionType.MaxStr;
				functionsList["Totals.MaxTime"] = StiFunctionType.MaxTime;
				functionsList["Totals.Median"] = StiFunctionType.Median;
				functionsList["Totals.MedianD"] = StiFunctionType.MedianD;
				functionsList["Totals.MedianI"] = StiFunctionType.MedianI;
				functionsList["Totals.Min"] = StiFunctionType.Min;
				functionsList["Totals.MinD"] = StiFunctionType.MinD;
				functionsList["Totals.MinDate"] = StiFunctionType.MinDate;
				functionsList["Totals.MinI"] = StiFunctionType.MinI;
				functionsList["Totals.MinStr"] = StiFunctionType.MinStr;
				functionsList["Totals.MinTime"] = StiFunctionType.MinTime;
				functionsList["Totals.Mode"] = StiFunctionType.Mode;
				functionsList["Totals.ModeD"] = StiFunctionType.ModeD;
				functionsList["Totals.ModeI"] = StiFunctionType.ModeI;
				functionsList["Totals.Sum"] = StiFunctionType.Sum;
				functionsList["Totals.SumD"] = StiFunctionType.SumD;
				functionsList["Totals.SumDistinct"] = StiFunctionType.SumDistinct;
				functionsList["Totals.SumI"] = StiFunctionType.SumI;
				functionsList["Totals.SumTime"] = StiFunctionType.SumTime;
				functionsList["Totals.First"] = StiFunctionType.First;
				functionsList["Totals.Last"] = StiFunctionType.Last;
				functionsList["Totals.cCount"] = StiFunctionType.pCount;
				functionsList["Totals.cCountDistinct"] = StiFunctionType.pCountDistinct;
				functionsList["Totals.cAvg"] = StiFunctionType.pAvg;
				functionsList["Totals.cAvgD"] = StiFunctionType.pAvgD;
				functionsList["Totals.cAvgDate"] = StiFunctionType.pAvgDate;
				functionsList["Totals.cAvgI"] = StiFunctionType.pAvgI;
				functionsList["Totals.cAvgTime"] = StiFunctionType.pAvgTime;
				functionsList["Totals.cMax"] = StiFunctionType.pMax;
				functionsList["Totals.cMaxD"] = StiFunctionType.pMaxD;
				functionsList["Totals.cMaxDate"] = StiFunctionType.pMaxDate;
				functionsList["Totals.cMaxI"] = StiFunctionType.pMaxI;
				functionsList["Totals.cMaxStr"] = StiFunctionType.pMaxStr;
				functionsList["Totals.cMaxTime"] = StiFunctionType.pMaxTime;
				functionsList["Totals.cMedian"] = StiFunctionType.pMedian;
				functionsList["Totals.cMedianD"] = StiFunctionType.pMedianD;
				functionsList["Totals.cMedianI"] = StiFunctionType.pMedianI;
				functionsList["Totals.cMin"] = StiFunctionType.pMin;
				functionsList["Totals.cMinD"] = StiFunctionType.pMinD;
				functionsList["Totals.cMinDate"] = StiFunctionType.pMinDate;
				functionsList["Totals.cMinI"] = StiFunctionType.pMinI;
				functionsList["Totals.cMinStr"] = StiFunctionType.pMinStr;
				functionsList["Totals.cMinTime"] = StiFunctionType.pMinTime;
				functionsList["Totals.cMode"] = StiFunctionType.pMode;
				functionsList["Totals.cModeD"] = StiFunctionType.pModeD;
				functionsList["Totals.cModeI"] = StiFunctionType.pModeI;
				functionsList["Totals.cSum"] = StiFunctionType.pSum;
				functionsList["Totals.cSumD"] = StiFunctionType.pSumD;
				functionsList["Totals.cSumDistinct"] = StiFunctionType.pSumDistinct;
				functionsList["Totals.cSumI"] = StiFunctionType.pSumI;
				functionsList["Totals.cSumTime"] = StiFunctionType.pSumTime;
				functionsList["Totals.cFirst"] = StiFunctionType.pFirst;
				functionsList["Totals.cLast"] = StiFunctionType.pLast;
				functionsList["Totals.cCountRunning"] = StiFunctionType.prCount;
				functionsList["Totals.cCountDistinctRunning"] = StiFunctionType.prCountDistinct;
				functionsList["Totals.cAvgRunning"] = StiFunctionType.prAvg;
				functionsList["Totals.cAvgDRunning"] = StiFunctionType.prAvgD;
				functionsList["Totals.cAvgDateRunning"] = StiFunctionType.prAvgDate;
				functionsList["Totals.cAvgIRunning"] = StiFunctionType.prAvgI;
				functionsList["Totals.cAvgTimeRunning"] = StiFunctionType.prAvgTime;
				functionsList["Totals.cMaxRunning"] = StiFunctionType.prMax;
				functionsList["Totals.cMaxDRunning"] = StiFunctionType.prMaxD;
				functionsList["Totals.cMaxDateRunning"] = StiFunctionType.prMaxDate;
				functionsList["Totals.cMaxIRunning"] = StiFunctionType.prMaxI;
				functionsList["Totals.cMaxStrRunning"] = StiFunctionType.prMaxStr;
				functionsList["Totals.cMaxTimeRunning"] = StiFunctionType.prMaxTime;
				functionsList["Totals.cMedianRunning"] = StiFunctionType.prMedian;
				functionsList["Totals.cMedianDRunning"] = StiFunctionType.prMedianD;
				functionsList["Totals.cMedianIRunning"] = StiFunctionType.prMedianI;
				functionsList["Totals.cMinRunning"] = StiFunctionType.prMin;
				functionsList["Totals.cMinDRunning"] = StiFunctionType.prMinD;
				functionsList["Totals.cMinDateRunning"] = StiFunctionType.prMinDate;
				functionsList["Totals.cMinIRunning"] = StiFunctionType.prMinI;
				functionsList["Totals.cMinStrRunning"] = StiFunctionType.prMinStr;
				functionsList["Totals.cMinTimeRunning"] = StiFunctionType.prMinTime;
				functionsList["Totals.cModeRunning"] = StiFunctionType.prMode;
				functionsList["Totals.cModeDRunning"] = StiFunctionType.prModeD;
				functionsList["Totals.cModeIRunning"] = StiFunctionType.prModeI;
				functionsList["Totals.cSumRunning"] = StiFunctionType.prSum;
				functionsList["Totals.cSumDRunning"] = StiFunctionType.prSumD;
				functionsList["Totals.cSumDistinctRunning"] = StiFunctionType.prSumDistinct;
				functionsList["Totals.cSumIRunning"] = StiFunctionType.prSumI;
				functionsList["Totals.cSumTimeRunning"] = StiFunctionType.prSumTime;
				functionsList["Totals.cFirstRunning"] = StiFunctionType.prFirst;
				functionsList["Totals.cLastRunning"] = StiFunctionType.prLast;
				functionsList["Totals.Rank"] = StiFunctionType.Rank;
				functionsList["Totals.CountAllLevels"] = StiFunctionType.CountAllLevels;
				functionsList["Totals.CountAllLevelsOnlyChilds"] = StiFunctionType.CountAllLevelsOnlyChilds;
				functionsList["Totals.CountOnlyChilds"] = StiFunctionType.CountOnlyChilds;
				functionsList["Abs"] = StiFunctionType.Abs;
				functionsList["Acos"] = StiFunctionType.Acos;
				functionsList["Asin"] = StiFunctionType.Asin;
				functionsList["Atan"] = StiFunctionType.Atan;
				functionsList["Ceiling"] = StiFunctionType.Ceiling;
				functionsList["Cos"] = StiFunctionType.Cos;
				functionsList["Div"] = StiFunctionType.Div;
				functionsList["Exp"] = StiFunctionType.Exp;
				functionsList["Floor"] = StiFunctionType.Floor;
				functionsList["Log"] = StiFunctionType.Log;
				functionsList["Maximum"] = StiFunctionType.Maximum;
				functionsList["Minimum"] = StiFunctionType.Minimum;
				functionsList["Round"] = StiFunctionType.Round;
				functionsList["Sign"] = StiFunctionType.Sign;
				functionsList["Sin"] = StiFunctionType.Sin;
				functionsList["Sqrt"] = StiFunctionType.Sqrt;
				functionsList["Tan"] = StiFunctionType.Tan;
				functionsList["Truncate"] = StiFunctionType.Truncate;
				functionsList["DateDiff"] = StiFunctionType.DateDiff;
				functionsList["DateSerial"] = StiFunctionType.DateSerial;
				functionsList["Day"] = StiFunctionType.Day;
				functionsList["DayOfWeek"] = StiFunctionType.DayOfWeek;
				functionsList["DayOfYear"] = StiFunctionType.DayOfYear;
				functionsList["DaysInMonth"] = StiFunctionType.DaysInMonth;
				functionsList["DaysInYear"] = StiFunctionType.DaysInYear;
				functionsList["Hour"] = StiFunctionType.Hour;
				functionsList["Minute"] = StiFunctionType.Minute;
				functionsList["Month"] = StiFunctionType.Month;
				functionsList["Second"] = StiFunctionType.Second;
				functionsList["TimeSerial"] = StiFunctionType.TimeSerial;
				functionsList["Year"] = StiFunctionType.Year;
				functionsList["MonthName"] = StiFunctionType.MonthName;
				functionsList["WeekOfYear"] = StiFunctionType.WeekOfYear;
				functionsList["WeekOfMonth"] = StiFunctionType.WeekOfMonth;
				functionsList["DateToStr"] = StiFunctionType.DateToStr;
				functionsList["DateToStrPl"] = StiFunctionType.DateToStrPl;
				functionsList["DateToStrRu"] = StiFunctionType.DateToStrRu;
				functionsList["DateToStrUa"] = StiFunctionType.DateToStrUa;
				functionsList["DateToStrPt"] = StiFunctionType.DateToStrPt;
				functionsList["DateToStrPtBr"] = StiFunctionType.DateToStrPtBr;
				functionsList["Insert"] = StiFunctionType.Insert;
				functionsList["Length"] = StiFunctionType.Length;
				functionsList["Remove"] = StiFunctionType.Remove;
				functionsList["Replace"] = StiFunctionType.Replace;
				functionsList["Roman"] = StiFunctionType.Roman;
				functionsList["Substring"] = StiFunctionType.Substring;
				functionsList["ToCurrencyWords"] = StiFunctionType.ToCurrencyWords;
				functionsList["ToCurrencyWordsEnGb"] = StiFunctionType.ToCurrencyWordsEnGb;
				functionsList["ToCurrencyWordsEnIn"] = StiFunctionType.ToCurrencyWordsEnIn;
				functionsList["ToCurrencyWordsEs"] = StiFunctionType.ToCurrencyWordsEs;
				functionsList["ToCurrencyWordsFr"] = StiFunctionType.ToCurrencyWordsFr;
				functionsList["ToCurrencyWordsNl"] = StiFunctionType.ToCurrencyWordsNl;
				functionsList["ToCurrencyWordsPl"] = StiFunctionType.ToCurrencyWordsPl;
				functionsList["ToCurrencyWordsPt"] = StiFunctionType.ToCurrencyWordsPt;
				functionsList["ToCurrencyWordsPtBr"] = StiFunctionType.ToCurrencyWordsPtBr;
				functionsList["ToCurrencyWordsRu"] = StiFunctionType.ToCurrencyWordsRu;
				functionsList["ToCurrencyWordsThai"] = StiFunctionType.ToCurrencyWordsThai;
				functionsList["ToCurrencyWordsTr"] = StiFunctionType.ToCurrencyWordsTr;
				functionsList["ToCurrencyWordsUa"] = StiFunctionType.ToCurrencyWordsUa;
				functionsList["ToLowerCase"] = StiFunctionType.ToLowerCase;
				functionsList["ToProperCase"] = StiFunctionType.ToProperCase;
				functionsList["ToUpperCase"] = StiFunctionType.ToUpperCase;
				functionsList["ToWords"] = StiFunctionType.ToWords;
				functionsList["ToWordsEs"] = StiFunctionType.ToWordsEs;
				functionsList["ToWordsEnIn"] = StiFunctionType.ToWordsEnIn;
				functionsList["ToWordsFa"] = StiFunctionType.ToWordsFa;
				functionsList["ToWordsPl"] = StiFunctionType.ToWordsPl;
				functionsList["ToWordsPt"] = StiFunctionType.ToWordsPt;
				functionsList["ToWordsRu"] = StiFunctionType.ToWordsRu;
				functionsList["ToWordsTr"] = StiFunctionType.ToWordsTr;
				functionsList["ToWordsUa"] = StiFunctionType.ToWordsUa;
				functionsList["Trim"] = StiFunctionType.Trim;
				functionsList["TryParseDecimal"] = StiFunctionType.TryParseDecimal;
				functionsList["TryParseDouble"] = StiFunctionType.TryParseDouble;
				functionsList["TryParseLong"] = StiFunctionType.TryParseLong;
				functionsList["Arabic"] = StiFunctionType.Arabic;
				functionsList["Persian"] = StiFunctionType.Persian;
				functionsList["ToOrdinal"] = StiFunctionType.ToOrdinal;
				functionsList["Left"] = StiFunctionType.Left;
				functionsList["Mid"] = StiFunctionType.Mid;
				functionsList["Right"] = StiFunctionType.Right;
				functionsList["StrToDateTime"] = StiFunctionType.StrToDateTime;
				functionsList["StrToNullableDateTime"] = StiFunctionType.StrToNullableDateTime;
				functionsList["ConvertToBase64String"] = StiFunctionType.ConvertToBase64String;
				functionsList["IsNull"] = StiFunctionType.IsNull;
				functionsList["Next"] = StiFunctionType.Next;
				functionsList["NextIsNull"] = StiFunctionType.NextIsNull;
				functionsList["Previous"] = StiFunctionType.Previous;
				functionsList["PreviousIsNull"] = StiFunctionType.PreviousIsNull;
				functionsList["IIF"] = StiFunctionType.IIF;
				functionsList["Choose"] = StiFunctionType.Choose;
				functionsList["Switch"] = StiFunctionType.Switch;
				functionsList["ToString"] = StiFunctionType.ToString;
				functionsList["Format"] = StiFunctionType.Format;
				functionsList["System.Convert.ToBoolean"] = StiFunctionType.SystemConvertToBoolean;
				functionsList["System.Convert.ToByte"] = StiFunctionType.SystemConvertToByte;
				functionsList["System.Convert.ToChar"] = StiFunctionType.SystemConvertToChar;
				functionsList["System.Convert.ToDateTime"] = StiFunctionType.SystemConvertToDateTime;
				functionsList["System.Convert.ToDecimal"] = StiFunctionType.SystemConvertToDecimal;
				functionsList["System.Convert.ToDouble"] = StiFunctionType.SystemConvertToDouble;
				functionsList["System.Convert.ToInt16"] = StiFunctionType.SystemConvertToInt16;
				functionsList["System.Convert.ToInt32"] = StiFunctionType.SystemConvertToInt32;
				functionsList["System.Convert.ToInt64"] = StiFunctionType.SystemConvertToInt64;
				functionsList["System.Convert.ToSByte"] = StiFunctionType.SystemConvertToSByte;
				functionsList["System.Convert.ToSingle"] = StiFunctionType.SystemConvertToSingle;
				functionsList["System.Convert.ToString"] = StiFunctionType.SystemConvertToString;
				functionsList["System.Convert.ToUInt16"] = StiFunctionType.SystemConvertToUInt16;
				functionsList["System.Convert.ToUInt32"] = StiFunctionType.SystemConvertToUInt32;
				functionsList["System.Convert.ToUInt64"] = StiFunctionType.SystemConvertToUInt64;
				functionsList["Convert.ToBoolean"] = StiFunctionType.SystemConvertToBoolean;
				functionsList["Convert.ToByte"] = StiFunctionType.SystemConvertToByte;
				functionsList["Convert.ToChar"] = StiFunctionType.SystemConvertToChar;
				functionsList["Convert.ToDateTime"] = StiFunctionType.SystemConvertToDateTime;
				functionsList["Convert.ToDecimal"] = StiFunctionType.SystemConvertToDecimal;
				functionsList["Convert.ToDouble"] = StiFunctionType.SystemConvertToDouble;
				functionsList["Convert.ToInt16"] = StiFunctionType.SystemConvertToInt16;
				functionsList["Convert.ToInt32"] = StiFunctionType.SystemConvertToInt32;
				functionsList["Convert.ToInt64"] = StiFunctionType.SystemConvertToInt64;
				functionsList["Convert.ToSByte"] = StiFunctionType.SystemConvertToSByte;
				functionsList["Convert.ToSingle"] = StiFunctionType.SystemConvertToSingle;
				functionsList["Convert.ToString"] = StiFunctionType.SystemConvertToString;
				functionsList["Convert.ToUInt16"] = StiFunctionType.SystemConvertToUInt16;
				functionsList["Convert.ToUInt32"] = StiFunctionType.SystemConvertToUInt32;
				functionsList["Convert.ToUInt64"] = StiFunctionType.SystemConvertToUInt64;
				functionsList["Math.Round"] = StiFunctionType.MathRound;
				functionsList["Math.Pow"] = StiFunctionType.MathPow;
				functionsList["AddAnchor"] = StiFunctionType.AddAnchor;
				functionsList["GetAnchorPageNumber"] = StiFunctionType.GetAnchorPageNumber;
				functionsList["GetAnchorPageNumberThrough"] = StiFunctionType.GetAnchorPageNumberThrough;
				functionsList["ConvertRtf"] = StiFunctionType.ConvertRtf;
				functionsList["GetLabel"] = StiFunctionType.GetLabel;
				functionsList["GetParam"] = StiFunctionType.GetParam;
				functionsList["string.IsNullOrEmpty"] = StiFunctionType.StringIsNullOrEmpty;
				functionsList["String.IsNullOrEmpty"] = StiFunctionType.StringIsNullOrEmpty;
				functionsList["string.IsNullOrWhiteSpace"] = StiFunctionType.StringIsNullOrWhiteSpace;
				functionsList["String.IsNullOrWhiteSpace"] = StiFunctionType.StringIsNullOrWhiteSpace;
				functionsList["string.Format"] = StiFunctionType.Format;
				functionsList["String.Format"] = StiFunctionType.Format;
				functionsList["Func.EngineHelper.JoinColumnContent"] = StiFunctionType.EngineHelperJoinColumnContent;
				functionsList["Func.EngineHelper.ToQueryString"] = StiFunctionType.EngineHelperToQueryString;
				functionsList["Func.EngineHelper.GetRealPageNumber"] = StiFunctionType.EngineHelperGetRealPageNumber;
				functionsList["StiNullValuesHelper.IsNull"] = StiFunctionType.StiNullValuesHelperIsNull;
				functionsList["TimeSpan.FromDays"] = StiFunctionType.TimeSpanFromDays;
				functionsList["TimeSpan.FromHours"] = StiFunctionType.TimeSpanFromHours;
				functionsList["TimeSpan.FromMilliseconds"] = StiFunctionType.TimeSpanFromMilliseconds;
				functionsList["TimeSpan.FromMinutes"] = StiFunctionType.TimeSpanFromMinutes;
				functionsList["TimeSpan.FromSeconds"] = StiFunctionType.TimeSpanFromSeconds;
				functionsList["TimeSpan.FromTicks"] = StiFunctionType.TimeSpanFromTicks;
				functionsList["Image.FromFile"] = StiFunctionType.ImageFromFile;
				functionsList_vb = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
				foreach (DictionaryEntry functions in functionsList)
				{
					functionsList_vb[((string)functions.Key).ToLowerInvariant()] = functions.Value;
				}
				functionsList_vb["Integer.Parse"] = StiFunctionType.ParseInt;
				functionsList_vb["Double.Parse"] = StiFunctionType.ParseDouble;
				functionsList_vb["Decimal.Parse"] = StiFunctionType.ParseDecimal;
				functionsList_vb["Date.Parse"] = StiFunctionType.ParseDateTime;
				functionsList["int.Parse"] = StiFunctionType.ParseInt;
				functionsList["Int16.Parse"] = StiFunctionType.ParseInt;
				functionsList["Int32.Parse"] = StiFunctionType.ParseInt;
				functionsList["Int64.Parse"] = StiFunctionType.ParseInt64;
				functionsList["double.Parse"] = StiFunctionType.ParseDouble;
				functionsList["Double.Parse"] = StiFunctionType.ParseDouble;
				functionsList["decimal.Parse"] = StiFunctionType.ParseDecimal;
				functionsList["Decimal.Parse"] = StiFunctionType.ParseDecimal;
				functionsList["TimeSpan.Parse"] = StiFunctionType.ParseTimeSpan;
				functionsList["DateTime.Parse"] = StiFunctionType.ParseDateTime;
				functionsList["Date.Parse"] = StiFunctionType.ParseDateTime;
				functionsList["DateTime.DaysInMonth"] = StiFunctionType.DaysInMonth;
				functionsList_low = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
				foreach (DictionaryEntry functions2 in functionsList)
				{
					functionsList_low[((string)functions2.Key).ToLowerInvariant()] = functions2.Value;
				}
			}
			if (IsVB)
			{
				return functionsList_vb;
			}
			if (!syntaxCaseSensitive)
			{
				return functionsList_low;
			}
			return functionsList;
		}
	}

	private Hashtable MethodsList
	{
		get
		{
			if (methodsList == null)
			{
				methodsList = new Hashtable();
				methodsList["Substring"] = StiMethodType.Substring;
				methodsList["ToString"] = StiMethodType.ToString;
				methodsList["ToLower"] = StiMethodType.ToLower;
				methodsList["ToUpper"] = StiMethodType.ToUpper;
				methodsList["IndexOf"] = StiMethodType.IndexOf;
				methodsList["StartsWith"] = StiMethodType.StartsWith;
				methodsList["EndsWith"] = StiMethodType.EndsWith;
				methodsList["Replace"] = StiMethodType.Replace;
				methodsList["PadLeft"] = StiMethodType.PadLeft;
				methodsList["PadRight"] = StiMethodType.PadRight;
				methodsList["TrimStart"] = StiMethodType.TrimStart;
				methodsList["TrimEnd"] = StiMethodType.TrimEnd;
				methodsList["Parse"] = StiMethodType.Parse;
				methodsList["Contains"] = StiMethodType.Contains;
				methodsList["GetData"] = StiMethodType.GetData;
				methodsList["ToQueryString"] = StiMethodType.ToQueryString;
				methodsList["AddYears"] = StiMethodType.AddYears;
				methodsList["AddMonths"] = StiMethodType.AddMonths;
				methodsList["AddDays"] = StiMethodType.AddDays;
				methodsList["AddHours"] = StiMethodType.AddHours;
				methodsList["AddMinutes"] = StiMethodType.AddMinutes;
				methodsList["AddSeconds"] = StiMethodType.AddSeconds;
				methodsList["AddMilliseconds"] = StiMethodType.AddMilliseconds;
				methodsList["ToShortDateString"] = StiMethodType.ToShortDateString;
				methodsList["ToShortTimeString"] = StiMethodType.ToShortTimeString;
				methodsList["ToLongDateString"] = StiMethodType.ToLongDateString;
				methodsList["ToLongTimeString"] = StiMethodType.ToLongTimeString;
				methodsList["GetCurrentConditionValue"] = StiMethodType.GetCurrentConditionValue;
				methodsList["Add"] = StiMethodType.Add;
				methodsList["Subtract"] = StiMethodType.Subtract;
				methodsList_low = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
				foreach (DictionaryEntry methods in methodsList)
				{
					methodsList_low[((string)methods.Key).ToLowerInvariant()] = methods.Value;
				}
			}
			if (!syntaxCaseSensitive)
			{
				return methodsList_low;
			}
			return methodsList;
		}
	}

	private static Hashtable ParametersList
	{
		get
		{
			if (parametersList == null)
			{
				parametersList = new Hashtable();
				parametersList[StiFunctionType.CountDistinct] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.Avg] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.AvgD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.AvgDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.AvgI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.AvgTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.Max] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.MaxD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.MaxDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.MaxI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.MaxStr] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.MaxTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.Median] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.MedianD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.MedianI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.Min] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.MinD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.MinDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.MinI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.MinStr] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.MinTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.Mode] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.ModeD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.ModeI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.Sum] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.SumD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.SumDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.SumI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.SumTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.First] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.Last] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rCountDistinct] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rAvg] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rAvgD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rAvgDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rAvgI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rAvgTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rMax] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rMaxD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rMaxDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rMaxI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rMaxStr] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rMaxTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rMedian] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rMedianD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rMedianI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rMin] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rMinD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rMinDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rMinI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rMinStr] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rMinTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rMode] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rModeD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rModeI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rSum] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rSumD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rSumDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.rSumI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rSumTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rFirst] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.rLast] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.iCount] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.iCountDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iAvg] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iAvgD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iAvgDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iAvgI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iAvgTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iMax] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iMaxD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iMaxDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iMaxI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iMaxStr] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iMaxTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iMedian] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iMedianD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iMedianI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iMin] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iMinD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iMinDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iMinI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iMinStr] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iMinTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iMode] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iModeD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iModeI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iSum] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iSumD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iSumDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3 | StiParameterNumber.Param4;
				parametersList[StiFunctionType.iSumI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iSumTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iFirst] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.iLast] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riCount] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.riCountDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riAvg] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riAvgD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riAvgDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riAvgI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riAvgTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riMax] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riMaxD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riMaxDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riMaxI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riMaxStr] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riMaxTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riMedian] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riMedianD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riMedianI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riMin] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riMinD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riMinDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riMinI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riMinStr] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riMinTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riMode] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riModeD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riModeI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riSum] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riSumD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riSumDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3 | StiParameterNumber.Param4;
				parametersList[StiFunctionType.riSumI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riSumTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riFirst] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.riLast] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.cCountDistinct] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cAvg] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cAvgD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cAvgDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cAvgI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cAvgTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cMax] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cMaxD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cMaxDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cMaxI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cMaxStr] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cMaxTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cMedian] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cMedianD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cMedianI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cMin] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cMinD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cMinDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cMinI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cMinStr] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cMinTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cMode] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cModeD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cModeI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cSum] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cSumD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cSumDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.cSumI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cSumTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cFirst] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.cLast] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crCountDistinct] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crAvg] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crAvgD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crAvgDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crAvgI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crAvgTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crMax] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crMaxD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crMaxDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crMaxI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crMaxStr] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crMaxTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crMedian] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crMedianD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crMedianI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crMin] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crMinD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crMinDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crMinI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crMinStr] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crMinTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crMode] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crModeD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crModeI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crSum] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crSumD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crSumDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.crSumI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crSumTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crFirst] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.crLast] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.ciCount] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.ciCountDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciAvg] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciAvgD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciAvgDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciAvgI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciAvgTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciMax] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciMaxD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciMaxDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciMaxI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciMaxStr] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciMaxTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciMedian] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciMedianD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciMedianI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciMin] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciMinD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciMinDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciMinI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciMinStr] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciMinTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciMode] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciModeD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciModeI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciSum] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciSumD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciSumDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3 | StiParameterNumber.Param4;
				parametersList[StiFunctionType.ciSumI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciSumTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciFirst] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.ciLast] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criCount] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.criCountDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criAvg] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criAvgD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criAvgDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criAvgI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criAvgTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criMax] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criMaxD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criMaxDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criMaxI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criMaxStr] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criMaxTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criMedian] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criMedianD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criMedianI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criMin] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criMinD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criMinDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criMinI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criMinStr] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criMinTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criMode] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criModeD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criModeI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criSum] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criSumD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criSumDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3 | StiParameterNumber.Param4;
				parametersList[StiFunctionType.criSumI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criSumTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criFirst] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.criLast] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.pCountDistinct] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pAvg] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pAvgD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pAvgDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pAvgI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pAvgTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pMax] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pMaxD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pMaxDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pMaxI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pMaxStr] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pMaxTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pMedian] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pMedianD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pMedianI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pMin] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pMinD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pMinDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pMinI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pMinStr] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pMinTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pMode] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pModeD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pModeI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pSum] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pSumD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pSumDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.pSumI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pSumTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pFirst] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.pLast] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prCountDistinct] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prAvg] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prAvgD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prAvgDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prAvgI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prAvgTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prMax] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prMaxD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prMaxDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prMaxI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prMaxStr] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prMaxTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prMedian] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prMedianD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prMedianI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prMin] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prMinD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prMinDate] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prMinI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prMinStr] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prMinTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prMode] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prModeD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prModeI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prSum] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prSumD] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prSumDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.prSumI] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prSumTime] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prFirst] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.prLast] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.piCount] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.piCountDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piAvg] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piAvgD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piAvgDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piAvgI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piAvgTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piMax] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piMaxD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piMaxDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piMaxI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piMaxStr] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piMaxTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piMedian] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piMedianD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piMedianI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piMin] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piMinD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piMinDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piMinI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piMinStr] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piMinTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piMode] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piModeD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piModeI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piSum] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piSumD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piSumDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3 | StiParameterNumber.Param4;
				parametersList[StiFunctionType.piSumI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piSumTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piFirst] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.piLast] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priCount] = StiParameterNumber.Param2;
				parametersList[StiFunctionType.priCountDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priAvg] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priAvgD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priAvgDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priAvgI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priAvgTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priMax] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priMaxD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priMaxDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priMaxI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priMaxStr] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priMaxTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priMedian] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priMedianD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priMedianI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priMin] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priMinD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priMinDate] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priMinI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priMinStr] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priMinTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priMode] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priModeD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priModeI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priSum] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priSumD] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priSumDistinct] = StiParameterNumber.Param2 | StiParameterNumber.Param3 | StiParameterNumber.Param4;
				parametersList[StiFunctionType.priSumI] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priSumTime] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priFirst] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.priLast] = StiParameterNumber.Param2 | StiParameterNumber.Param3;
				parametersList[StiFunctionType.Rank] = StiParameterNumber.Param2;
			}
			return parametersList;
		}
	}

	private Hashtable ComponentsList
	{
		get
		{
			if (componentsList == null)
			{
				componentsList = new Hashtable();
				StiComponentsCollection components = report.GetComponents();
				foreach (StiComponent item in components)
				{
					componentsList[item.Name] = item;
				}
				componentsList["this"] = report;
			}
			return componentsList;
		}
	}

	private static Hashtable MethodsHash
	{
		get
		{
			if (methodsHash == null)
			{
				lock (lockMethodsHash)
				{
					StiParserMethodInfo[] array = new StiParserMethodInfo[639]
					{
						new StiParserMethodInfo(StiFunctionType.DateDiff, 1, new Type[2]
						{
							typeof(DateTime),
							typeof(DateTime)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.DateDiff, 2, new Type[2]
						{
							typeof(DateTime?),
							typeof(DateTime?)
						}, typeof(TimeSpan?)),
						new StiParserMethodInfo(StiFunctionType.DateSerial, 1, new Type[1] { typeof(long) }, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.DateSerial, 2, new Type[3]
						{
							typeof(long),
							typeof(long),
							typeof(long)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.TimeSerial, 1, new Type[3]
						{
							typeof(long),
							typeof(long),
							typeof(long)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.Year, 1, new Type[1] { typeof(DateTime) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Year, 2, new Type[1] { typeof(DateTime?) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Month, 1, new Type[1] { typeof(DateTime) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Month, 2, new Type[1] { typeof(DateTime?) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Day, 1, new Type[1] { typeof(DateTime) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Day, 2, new Type[1] { typeof(DateTime?) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Hour, 1, new Type[1] { typeof(DateTime) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Hour, 2, new Type[1] { typeof(DateTime?) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Minute, 1, new Type[1] { typeof(DateTime) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Minute, 2, new Type[1] { typeof(DateTime?) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Second, 1, new Type[1] { typeof(DateTime) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Second, 2, new Type[1] { typeof(DateTime?) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.DayOfWeek, 1, new Type[1] { typeof(DateTime) }),
						new StiParserMethodInfo(StiFunctionType.DayOfWeek, 2, new Type[1] { typeof(DateTime?) }),
						new StiParserMethodInfo(StiFunctionType.DayOfWeek, 3, new Type[2]
						{
							typeof(DateTime),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.DayOfWeek, 4, new Type[2]
						{
							typeof(DateTime?),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.DayOfWeek, 5, new Type[2]
						{
							typeof(DateTime),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.DayOfWeek, 6, new Type[2]
						{
							typeof(DateTime?),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.DayOfWeek, 7, new Type[3]
						{
							typeof(DateTime),
							typeof(string),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.DayOfWeek, 8, new Type[3]
						{
							typeof(DateTime?),
							typeof(string),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.DayOfYear, 1, new Type[1] { typeof(DateTime) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.DayOfYear, 2, new Type[1] { typeof(DateTime?) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.DaysInMonth, 1, new Type[1] { typeof(DateTime) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.DaysInMonth, 2, new Type[1] { typeof(DateTime?) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.DaysInMonth, 3, new Type[2]
						{
							typeof(long),
							typeof(long)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.DaysInYear, 1, new Type[1] { typeof(DateTime) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.DaysInYear, 2, new Type[1] { typeof(DateTime?) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.DaysInYear, 3, new Type[1] { typeof(long) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.MonthName, 1, new Type[1] { typeof(DateTime) }),
						new StiParserMethodInfo(StiFunctionType.MonthName, 2, new Type[1] { typeof(DateTime?) }),
						new StiParserMethodInfo(StiFunctionType.MonthName, 3, new Type[2]
						{
							typeof(DateTime),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.MonthName, 4, new Type[2]
						{
							typeof(DateTime?),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.MonthName, 5, new Type[2]
						{
							typeof(DateTime),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.MonthName, 6, new Type[2]
						{
							typeof(DateTime?),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.MonthName, 7, new Type[3]
						{
							typeof(DateTime),
							typeof(string),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.MonthName, 8, new Type[3]
						{
							typeof(DateTime?),
							typeof(string),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.WeekOfYear, 1, new Type[1] { typeof(DateTime) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.WeekOfYear, 2, new Type[1] { typeof(DateTime?) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.WeekOfYear, 3, new Type[2]
						{
							typeof(DateTime),
							typeof(DayOfWeek)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.WeekOfYear, 4, new Type[2]
						{
							typeof(DateTime?),
							typeof(DayOfWeek)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.WeekOfYear, 5, new Type[3]
						{
							typeof(DateTime),
							typeof(DayOfWeek),
							typeof(CalendarWeekRule)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.WeekOfYear, 6, new Type[3]
						{
							typeof(DateTime?),
							typeof(DayOfWeek),
							typeof(CalendarWeekRule)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.WeekOfMonth, 1, new Type[1] { typeof(DateTime) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.WeekOfMonth, 2, new Type[1] { typeof(DateTime?) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.WeekOfMonth, 3, new Type[2]
						{
							typeof(DateTime),
							typeof(DayOfWeek)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.WeekOfMonth, 4, new Type[2]
						{
							typeof(DateTime?),
							typeof(DayOfWeek)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.WeekOfMonth, 5, new Type[3]
						{
							typeof(DateTime),
							typeof(DayOfWeek),
							typeof(CalendarWeekRule)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.WeekOfMonth, 6, new Type[3]
						{
							typeof(DateTime?),
							typeof(DayOfWeek),
							typeof(CalendarWeekRule)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Abs, 1, new Type[1] { typeof(long) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Abs, 2, new Type[1] { typeof(double) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Abs, 3, new Type[1] { typeof(decimal) }, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.Acos, 1, new Type[1] { typeof(double) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Asin, 1, new Type[1] { typeof(double) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Atan, 1, new Type[1] { typeof(double) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Cos, 1, new Type[1] { typeof(double) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Sin, 1, new Type[1] { typeof(double) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Tan, 1, new Type[1] { typeof(double) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Ceiling, 1, new Type[1] { typeof(double) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Ceiling, 2, new Type[1] { typeof(decimal) }, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.Div, 1, new Type[2]
						{
							typeof(long),
							typeof(long)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Div, 2, new Type[3]
						{
							typeof(long),
							typeof(long),
							typeof(long)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Div, 3, new Type[2]
						{
							typeof(double),
							typeof(double)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Div, 4, new Type[3]
						{
							typeof(double),
							typeof(double),
							typeof(double)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Div, 5, new Type[2]
						{
							typeof(decimal),
							typeof(decimal)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.Div, 6, new Type[3]
						{
							typeof(decimal),
							typeof(decimal),
							typeof(decimal)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.Div, 7, new Type[2]
						{
							typeof(long?),
							typeof(long?)
						}, typeof(long?)),
						new StiParserMethodInfo(StiFunctionType.Div, 8, new Type[3]
						{
							typeof(long?),
							typeof(long?),
							typeof(long?)
						}, typeof(long?)),
						new StiParserMethodInfo(StiFunctionType.Div, 9, new Type[2]
						{
							typeof(double?),
							typeof(double?)
						}, typeof(double?)),
						new StiParserMethodInfo(StiFunctionType.Div, 10, new Type[3]
						{
							typeof(double?),
							typeof(double?),
							typeof(double?)
						}, typeof(double?)),
						new StiParserMethodInfo(StiFunctionType.Div, 11, new Type[2]
						{
							typeof(decimal?),
							typeof(decimal?)
						}, typeof(decimal?)),
						new StiParserMethodInfo(StiFunctionType.Div, 12, new Type[3]
						{
							typeof(decimal?),
							typeof(decimal?),
							typeof(decimal?)
						}, typeof(decimal?)),
						new StiParserMethodInfo(StiFunctionType.Exp, 1, new Type[1] { typeof(long) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Floor, 1, new Type[1] { typeof(double) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Floor, 2, new Type[1] { typeof(decimal) }, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.Log, 1, new Type[1] { typeof(double) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Maximum, 1, new Type[2]
						{
							typeof(long),
							typeof(long)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Maximum, 2, new Type[2]
						{
							typeof(double),
							typeof(double)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Maximum, 3, new Type[2]
						{
							typeof(decimal),
							typeof(decimal)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.Maximum, 4, new Type[2]
						{
							typeof(long?),
							typeof(long?)
						}, typeof(long?)),
						new StiParserMethodInfo(StiFunctionType.Maximum, 5, new Type[2]
						{
							typeof(double?),
							typeof(double?)
						}, typeof(double?)),
						new StiParserMethodInfo(StiFunctionType.Maximum, 6, new Type[2]
						{
							typeof(decimal?),
							typeof(decimal?)
						}, typeof(decimal?)),
						new StiParserMethodInfo(StiFunctionType.Minimum, 1, new Type[2]
						{
							typeof(long),
							typeof(long)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Minimum, 2, new Type[2]
						{
							typeof(double),
							typeof(double)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Minimum, 3, new Type[2]
						{
							typeof(decimal),
							typeof(decimal)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.Minimum, 4, new Type[2]
						{
							typeof(long?),
							typeof(long?)
						}, typeof(long?)),
						new StiParserMethodInfo(StiFunctionType.Minimum, 5, new Type[2]
						{
							typeof(double?),
							typeof(double?)
						}, typeof(double?)),
						new StiParserMethodInfo(StiFunctionType.Minimum, 6, new Type[2]
						{
							typeof(decimal?),
							typeof(decimal?)
						}, typeof(decimal?)),
						new StiParserMethodInfo(StiFunctionType.Round, 1, new Type[1] { typeof(double) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Round, 2, new Type[2]
						{
							typeof(double),
							typeof(int)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Round, 3, new Type[1] { typeof(decimal) }, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.Round, 4, new Type[2]
						{
							typeof(decimal),
							typeof(int)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.Round, 5, new Type[2]
						{
							typeof(double),
							typeof(MidpointRounding)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Round, 6, new Type[3]
						{
							typeof(double),
							typeof(int),
							typeof(MidpointRounding)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Round, 7, new Type[2]
						{
							typeof(decimal),
							typeof(MidpointRounding)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.Round, 8, new Type[3]
						{
							typeof(decimal),
							typeof(int),
							typeof(MidpointRounding)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.Sign, 1, new Type[1] { typeof(long) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Sign, 2, new Type[1] { typeof(double) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Sign, 3, new Type[1] { typeof(decimal) }, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.Sqrt, 1, new Type[1] { typeof(double) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Truncate, 1, new Type[1] { typeof(double) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.Truncate, 2, new Type[1] { typeof(decimal) }, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.IsNull, 1, new Type[2]
						{
							typeof(object),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.Next, 1, new Type[2]
						{
							typeof(object),
							typeof(string)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.NextIsNull, 1, new Type[2]
						{
							typeof(object),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.Previous, 1, new Type[2]
						{
							typeof(object),
							typeof(string)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.PreviousIsNull, 1, new Type[2]
						{
							typeof(object),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.IIF, 1, new Type[3]
						{
							typeof(bool),
							typeof(int),
							typeof(int)
						}, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.IIF, 2, new Type[3]
						{
							typeof(bool),
							typeof(long),
							typeof(long)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.IIF, 3, new Type[3]
						{
							typeof(bool),
							typeof(double),
							typeof(double)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.IIF, 4, new Type[3]
						{
							typeof(bool),
							typeof(decimal),
							typeof(decimal)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.IIF, 5, new Type[3]
						{
							typeof(bool),
							typeof(char),
							typeof(char)
						}, typeof(char)),
						new StiParserMethodInfo(StiFunctionType.IIF, 6, new Type[3]
						{
							typeof(bool),
							typeof(string),
							typeof(string)
						}, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.IIF, 7, new Type[3]
						{
							typeof(bool),
							typeof(DateTime),
							typeof(DateTime)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.IIF, 8, new Type[3]
						{
							typeof(bool),
							typeof(object),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Choose, 1, new Type[2]
						{
							typeof(int),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Choose, 2, new Type[3]
						{
							typeof(int),
							typeof(object),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Choose, 3, new Type[4]
						{
							typeof(int),
							typeof(object),
							typeof(object),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Choose, 4, new Type[5]
						{
							typeof(int),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Choose, 5, new Type[6]
						{
							typeof(int),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Choose, 6, new Type[7]
						{
							typeof(int),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Choose, 7, new Type[8]
						{
							typeof(int),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Choose, 8, new Type[9]
						{
							typeof(int),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Choose, 9, new Type[10]
						{
							typeof(int),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Choose, 10, new Type[11]
						{
							typeof(int),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Choose, 11, new Type[12]
						{
							typeof(int),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Choose, 12, new Type[13]
						{
							typeof(int),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Choose, 13, new Type[14]
						{
							typeof(int),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Choose, 14, new Type[15]
						{
							typeof(int),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 1, new Type[2]
						{
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 2, new Type[4]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 3, new Type[6]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 4, new Type[8]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 5, new Type[10]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 6, new Type[12]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 7, new Type[14]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 8, new Type[16]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 9, new Type[18]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 10, new Type[20]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 11, new Type[22]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 12, new Type[24]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 13, new Type[26]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 14, new Type[28]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 15, new Type[30]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 16, new Type[32]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 17, new Type[34]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 18, new Type[36]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 19, new Type[38]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Switch, 20, new Type[40]
						{
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object),
							typeof(bool),
							typeof(object)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Arabic, 1, new Type[1] { typeof(int) }),
						new StiParserMethodInfo(StiFunctionType.Arabic, 2, new Type[1] { typeof(string) }),
						new StiParserMethodInfo(StiFunctionType.Persian, 1, new Type[1] { typeof(int) }),
						new StiParserMethodInfo(StiFunctionType.Persian, 2, new Type[1] { typeof(string) }),
						new StiParserMethodInfo(StiFunctionType.DateToStr, 1, new Type[1] { typeof(DateTime) }),
						new StiParserMethodInfo(StiFunctionType.DateToStr, 2, new Type[1] { typeof(DateTime?) }),
						new StiParserMethodInfo(StiFunctionType.DateToStr, 3, new Type[2]
						{
							typeof(DateTime),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.DateToStr, 4, new Type[2]
						{
							typeof(DateTime?),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.DateToStrPl, 1, new Type[2]
						{
							typeof(DateTime),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.DateToStrRu, 1, new Type[1] { typeof(DateTime) }),
						new StiParserMethodInfo(StiFunctionType.DateToStrRu, 2, new Type[2]
						{
							typeof(DateTime),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.DateToStrUa, 1, new Type[1] { typeof(DateTime) }),
						new StiParserMethodInfo(StiFunctionType.DateToStrUa, 2, new Type[2]
						{
							typeof(DateTime),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.DateToStrPt, 1, new Type[1] { typeof(DateTime) }),
						new StiParserMethodInfo(StiFunctionType.DateToStrPtBr, 1, new Type[1] { typeof(DateTime) }),
						new StiParserMethodInfo(StiFunctionType.Insert, 1, new Type[3]
						{
							typeof(object),
							typeof(int),
							typeof(object)
						}),
						new StiParserMethodInfo(StiFunctionType.Left, 1, new Type[2]
						{
							typeof(object),
							typeof(int)
						}),
						new StiParserMethodInfo(StiFunctionType.Right, 1, new Type[2]
						{
							typeof(object),
							typeof(int)
						}),
						new StiParserMethodInfo(StiFunctionType.Mid, 1, new Type[3]
						{
							typeof(object),
							typeof(int),
							typeof(int)
						}),
						new StiParserMethodInfo(StiFunctionType.Length, 1, new Type[1] { typeof(object) }, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.Remove, 1, new Type[2]
						{
							typeof(object),
							typeof(int)
						}),
						new StiParserMethodInfo(StiFunctionType.Remove, 2, new Type[3]
						{
							typeof(object),
							typeof(int),
							typeof(int)
						}),
						new StiParserMethodInfo(StiFunctionType.Replace, 1, new Type[3]
						{
							typeof(object),
							typeof(object),
							typeof(object)
						}),
						new StiParserMethodInfo(StiFunctionType.Substring, 1, new Type[3]
						{
							typeof(object),
							typeof(int),
							typeof(int)
						}),
						new StiParserMethodInfo(StiFunctionType.Trim, 1, new Type[1] { typeof(object) }),
						new StiParserMethodInfo(StiFunctionType.Roman, 1, new Type[1] { typeof(int) }),
						new StiParserMethodInfo(StiFunctionType.ToOrdinal, 1, new Type[1] { typeof(long) }),
						new StiParserMethodInfo(StiFunctionType.ToLowerCase, 1, new Type[1] { typeof(object) }),
						new StiParserMethodInfo(StiFunctionType.ToUpperCase, 1, new Type[1] { typeof(object) }),
						new StiParserMethodInfo(StiFunctionType.ToProperCase, 1, new Type[1] { typeof(object) }),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWords, 1, new Type[1] { typeof(long) }),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWords, 2, new Type[1] { typeof(double) }),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWords, 3, new Type[1] { typeof(decimal) }),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWords, 4, new Type[2]
						{
							typeof(long),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWords, 5, new Type[2]
						{
							typeof(double),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWords, 6, new Type[2]
						{
							typeof(decimal),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWords, 7, new Type[3]
						{
							typeof(long),
							typeof(bool),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWords, 8, new Type[3]
						{
							typeof(double),
							typeof(bool),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWords, 9, new Type[3]
						{
							typeof(decimal),
							typeof(bool),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWords, 10, new Type[5]
						{
							typeof(double),
							typeof(bool),
							typeof(bool),
							typeof(string),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsEnGb, 1, new Type[3]
						{
							typeof(decimal),
							typeof(string),
							typeof(int)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsEnIn, 1, new Type[5]
						{
							typeof(string),
							typeof(string),
							typeof(decimal),
							typeof(int),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsEs, 1, new Type[3]
						{
							typeof(decimal),
							typeof(string),
							typeof(int)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsFr, 1, new Type[3]
						{
							typeof(decimal),
							typeof(string),
							typeof(int)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsNl, 1, new Type[3]
						{
							typeof(decimal),
							typeof(string),
							typeof(int)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsPl, 1, new Type[4]
						{
							typeof(decimal),
							typeof(string),
							typeof(bool),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsPt, 1, new Type[3]
						{
							typeof(decimal),
							typeof(bool),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsPt, 2, new Type[5]
						{
							typeof(decimal),
							typeof(bool),
							typeof(bool),
							typeof(string),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsPtBr, 1, new Type[1] { typeof(decimal) }),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsPtBr, 2, new Type[4]
						{
							typeof(decimal),
							typeof(bool),
							typeof(string),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsRu, 1, new Type[1] { typeof(long) }),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsRu, 2, new Type[1] { typeof(double) }),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsRu, 3, new Type[1] { typeof(decimal) }),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsRu, 4, new Type[2]
						{
							typeof(long),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsRu, 5, new Type[2]
						{
							typeof(double),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsRu, 6, new Type[2]
						{
							typeof(decimal),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsRu, 7, new Type[3]
						{
							typeof(long),
							typeof(bool),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsRu, 8, new Type[3]
						{
							typeof(double),
							typeof(bool),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsRu, 9, new Type[3]
						{
							typeof(decimal),
							typeof(bool),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsRu, 10, new Type[3]
						{
							typeof(long),
							typeof(string),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsRu, 11, new Type[3]
						{
							typeof(double),
							typeof(string),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsRu, 12, new Type[3]
						{
							typeof(decimal),
							typeof(string),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsThai, 1, new Type[1] { typeof(long) }),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsThai, 2, new Type[1] { typeof(double) }),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsThai, 3, new Type[1] { typeof(decimal) }),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsTr, 1, new Type[1] { typeof(decimal) }),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsTr, 2, new Type[3]
						{
							typeof(decimal),
							typeof(string),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsUa, 1, new Type[1] { typeof(long) }),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsUa, 2, new Type[1] { typeof(double) }),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsUa, 3, new Type[1] { typeof(decimal) }),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsUa, 4, new Type[2]
						{
							typeof(long),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsUa, 5, new Type[2]
						{
							typeof(double),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsUa, 6, new Type[2]
						{
							typeof(decimal),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsUa, 7, new Type[3]
						{
							typeof(long),
							typeof(bool),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsUa, 8, new Type[3]
						{
							typeof(double),
							typeof(bool),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.ToCurrencyWordsUa, 9, new Type[3]
						{
							typeof(decimal),
							typeof(bool),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.ToWords, 1, new Type[1] { typeof(long) }),
						new StiParserMethodInfo(StiFunctionType.ToWords, 3, new Type[1] { typeof(double) }),
						new StiParserMethodInfo(StiFunctionType.ToWords, 2, new Type[1] { typeof(decimal) }),
						new StiParserMethodInfo(StiFunctionType.ToWords, 4, new Type[2]
						{
							typeof(long),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToWords, 6, new Type[2]
						{
							typeof(double),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToWords, 5, new Type[2]
						{
							typeof(decimal),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToWordsEs, 1, new Type[2]
						{
							typeof(long),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToWordsEs, 2, new Type[3]
						{
							typeof(long),
							typeof(bool),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToWordsEnIn, 1, new Type[2]
						{
							typeof(long),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToWordsFa, 1, new Type[1] { typeof(long) }),
						new StiParserMethodInfo(StiFunctionType.ToWordsPl, 1, new Type[2]
						{
							typeof(long),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToWordsPt, 1, new Type[2]
						{
							typeof(long),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToWordsRu, 1, new Type[1] { typeof(long) }),
						new StiParserMethodInfo(StiFunctionType.ToWordsRu, 3, new Type[1] { typeof(double) }),
						new StiParserMethodInfo(StiFunctionType.ToWordsRu, 2, new Type[1] { typeof(decimal) }),
						new StiParserMethodInfo(StiFunctionType.ToWordsRu, 4, new Type[2]
						{
							typeof(long),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToWordsRu, 6, new Type[2]
						{
							typeof(double),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToWordsRu, 5, new Type[2]
						{
							typeof(decimal),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToWordsTr, 1, new Type[1] { typeof(decimal) }),
						new StiParserMethodInfo(StiFunctionType.ToWordsUa, 1, new Type[1] { typeof(long) }),
						new StiParserMethodInfo(StiFunctionType.ToWordsUa, 3, new Type[1] { typeof(double) }),
						new StiParserMethodInfo(StiFunctionType.ToWordsUa, 2, new Type[1] { typeof(decimal) }),
						new StiParserMethodInfo(StiFunctionType.ToWordsUa, 4, new Type[2]
						{
							typeof(long),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToWordsUa, 6, new Type[2]
						{
							typeof(double),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.ToWordsUa, 5, new Type[2]
						{
							typeof(decimal),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.TryParseDecimal, 1, new Type[1] { typeof(string) }, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.TryParseDouble, 1, new Type[1] { typeof(string) }, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.TryParseLong, 1, new Type[1] { typeof(string) }, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.StrToDateTime, 1, new Type[1] { typeof(string) }, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.StrToNullableDateTime, 1, new Type[1] { typeof(string) }, typeof(DateTime?)),
						new StiParserMethodInfo(StiFunctionType.Format, 1, new Type[2]
						{
							typeof(string),
							typeof(object)
						}),
						new StiParserMethodInfo(StiFunctionType.Format, 2, new Type[3]
						{
							typeof(string),
							typeof(object),
							typeof(object)
						}),
						new StiParserMethodInfo(StiFunctionType.Format, 3, new Type[4]
						{
							typeof(string),
							typeof(object),
							typeof(object),
							typeof(object)
						}),
						new StiParserMethodInfo(StiFunctionType.Format, 4, new Type[5]
						{
							typeof(string),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object)
						}),
						new StiParserMethodInfo(StiFunctionType.Format, 5, new Type[6]
						{
							typeof(string),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object)
						}),
						new StiParserMethodInfo(StiFunctionType.Format, 6, new Type[7]
						{
							typeof(string),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object)
						}),
						new StiParserMethodInfo(StiFunctionType.Format, 7, new Type[8]
						{
							typeof(string),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object),
							typeof(object)
						}),
						new StiParserMethodInfo(StiFunctionType.ConvertToBase64String, 1, new Type[1] { typeof(string) }, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.Count, 1, new Type[1] { typeof(object) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.CountDistinct, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Avg, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.AvgD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.AvgDate, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.AvgI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.AvgTime, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.Max, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.MaxD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.MaxDate, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.MaxI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.MaxStr, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.MaxTime, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.Median, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.MedianD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.MedianI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Min, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.MinD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.MinDate, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.MinI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.MinStr, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.MinTime, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.Mode, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.ModeD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.ModeI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.Sum, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.SumD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.SumDistinct, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.SumI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.SumTime, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.First, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Last, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.pCount, 1, new Type[1] { typeof(object) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.pCountDistinct, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.pAvg, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.pAvgD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.pAvgDate, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.pAvgI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.pAvgTime, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.pMax, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.pMaxD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.pMaxDate, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.pMaxI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.pMaxStr, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.pMaxTime, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.pMedian, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.pMedianD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.pMedianI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.pMin, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.pMinD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.pMinDate, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.pMinI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.pMinStr, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.pMinTime, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.pMode, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.pModeD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.pModeI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.pSum, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.pSumD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.pSumDistinct, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.pSumI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.pSumTime, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.pFirst, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.pLast, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.prCount, 1, new Type[1] { typeof(object) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.prCountDistinct, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.prAvg, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.prAvgD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.prAvgDate, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.prAvgI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.prAvgTime, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.prMax, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.prMaxD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.prMaxDate, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.prMaxI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.prMaxStr, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.prMaxTime, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.prMedian, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.prMedianD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.prMedianI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.prMin, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.prMinD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.prMinDate, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.prMinI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.prMinStr, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.prMinTime, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.prMode, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.prModeD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.prModeI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.prSum, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.prSumD, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.prSumDistinct, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.prSumI, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.prSumTime, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.prFirst, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.prLast, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.iCount, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.iCountDistinct, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.iAvg, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.iAvgD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.iAvgDate, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.iAvgI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.iAvgTime, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.iMax, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.iMaxD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.iMaxDate, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.iMaxI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.iMaxStr, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.iMaxTime, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.iMedian, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.iMedianD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.iMedianI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.iMin, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.iMinD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.iMinDate, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.iMinI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.iMinStr, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.iMinTime, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.iMode, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.iModeD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.iModeI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.iSum, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.iSumD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.iSumDistinct, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.iSumI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.iSumTime, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.iFirst, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.iLast, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.piCount, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.piCountDistinct, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.piAvg, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.piAvgD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.piAvgDate, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.piAvgI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.piAvgTime, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.piMax, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.piMaxD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.piMaxDate, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.piMaxI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.piMaxStr, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.piMaxTime, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.piMedian, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.piMedianD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.piMedianI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.piMin, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.piMinD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.piMinDate, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.piMinI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.piMinStr, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.piMinTime, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.piMode, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.piModeD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.piModeI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.piSum, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.piSumD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.piSumDistinct, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.piSumI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.piSumTime, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.piFirst, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.piLast, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.priCount, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.priCountDistinct, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.priAvg, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.priAvgD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.priAvgDate, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.priAvgI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.priAvgTime, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.priMax, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.priMaxD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.priMaxDate, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.priMaxI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.priMaxStr, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.priMaxTime, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.priMedian, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.priMedianD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.priMedianI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.priMin, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.priMinD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.priMinDate, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.priMinI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.priMinStr, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.priMinTime, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.priMode, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.priModeD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.priModeI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.priSum, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.priSumD, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.priSumDistinct, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.priSumI, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.priSumTime, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.priFirst, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.priLast, 1, new Type[3]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(List<StiAsmCommand>)
						}, typeof(object)),
						new StiParserMethodInfo(StiFunctionType.Rank, 1, new Type[2]
						{
							typeof(object),
							typeof(List<StiAsmCommand>)
						}),
						new StiParserMethodInfo(StiFunctionType.Rank, 2, new Type[4]
						{
							typeof(object),
							typeof(List<StiAsmCommand>),
							typeof(bool),
							typeof(StiRankOrder)
						}),
						new StiParserMethodInfo(StiFunctionType.CountAllLevels, 1, new Type[1] { typeof(object) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.CountAllLevelsOnlyChilds, 1, new Type[1] { typeof(object) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.CountOnlyChilds, 1, new Type[1] { typeof(object) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.MathRound, 1, new Type[1] { typeof(double) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.MathRound, 2, new Type[2]
						{
							typeof(double),
							typeof(MidpointRounding)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.MathRound, 3, new Type[2]
						{
							typeof(double),
							typeof(int)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.MathRound, 4, new Type[3]
						{
							typeof(double),
							typeof(int),
							typeof(MidpointRounding)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.MathRound, 5, new Type[1] { typeof(decimal) }, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.MathRound, 6, new Type[2]
						{
							typeof(decimal),
							typeof(MidpointRounding)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.MathRound, 7, new Type[2]
						{
							typeof(decimal),
							typeof(int)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.MathRound, 8, new Type[3]
						{
							typeof(decimal),
							typeof(int),
							typeof(MidpointRounding)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.MathPow, 1, new Type[2]
						{
							typeof(double),
							typeof(double)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.GetAnchorPageNumber, 1, new Type[1] { typeof(object) }, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.GetAnchorPageNumberThrough, 1, new Type[1] { typeof(object) }, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.ParseTimeSpan, 1, new Type[1] { typeof(string) }, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.ParseDateTime, 1, new Type[1] { typeof(string) }, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.ParseDecimal, 1, new Type[1] { typeof(string) }, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.ParseDouble, 1, new Type[1] { typeof(string) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.ParseInt, 1, new Type[1] { typeof(string) }, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.ParseInt64, 1, new Type[1] { typeof(string) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.ToString, 1, new Type[1] { typeof(object) }),
						new StiParserMethodInfo(StiFunctionType.StringIsNullOrEmpty, 1, new Type[1] { typeof(string) }, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.StringIsNullOrWhiteSpace, 1, new Type[1] { typeof(string) }, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.EngineHelperJoinColumnContent, 1, new Type[3]
						{
							typeof(StiDataSource),
							typeof(string),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.EngineHelperJoinColumnContent, 2, new Type[3]
						{
							typeof(StiBusinessObject),
							typeof(string),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.EngineHelperJoinColumnContent, 3, new Type[4]
						{
							typeof(StiDataSource),
							typeof(string),
							typeof(string),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.EngineHelperJoinColumnContent, 4, new Type[4]
						{
							typeof(StiBusinessObject),
							typeof(string),
							typeof(string),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.EngineHelperToQueryString, 1, new Type[3]
						{
							typeof(IList),
							typeof(string),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.EngineHelperToQueryString, 2, new Type[4]
						{
							typeof(IList),
							typeof(string),
							typeof(string),
							typeof(bool)
						}),
						new StiParserMethodInfo(StiFunctionType.EngineHelperGetRealPageNumber, 1, new Type[1] { typeof(object) }, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.SystemConvertToBoolean, 1, new Type[1] { typeof(object) }, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.SystemConvertToByte, 2, new Type[1] { typeof(object) }, typeof(byte)),
						new StiParserMethodInfo(StiFunctionType.SystemConvertToChar, 3, new Type[1] { typeof(object) }, typeof(char)),
						new StiParserMethodInfo(StiFunctionType.SystemConvertToDateTime, 4, new Type[1] { typeof(object) }, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.SystemConvertToDecimal, 5, new Type[1] { typeof(object) }, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.SystemConvertToDouble, 6, new Type[1] { typeof(object) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.SystemConvertToInt16, 7, new Type[1] { typeof(object) }, typeof(short)),
						new StiParserMethodInfo(StiFunctionType.SystemConvertToInt32, 8, new Type[1] { typeof(object) }, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.SystemConvertToInt64, 9, new Type[1] { typeof(object) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.SystemConvertToSByte, 10, new Type[1] { typeof(object) }, typeof(sbyte)),
						new StiParserMethodInfo(StiFunctionType.SystemConvertToSingle, 11, new Type[1] { typeof(object) }, typeof(float)),
						new StiParserMethodInfo(StiFunctionType.SystemConvertToString, 12, new Type[1] { typeof(object) }, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.SystemConvertToUInt16, 13, new Type[1] { typeof(object) }, typeof(ushort)),
						new StiParserMethodInfo(StiFunctionType.SystemConvertToUInt32, 14, new Type[1] { typeof(object) }, typeof(uint)),
						new StiParserMethodInfo(StiFunctionType.SystemConvertToUInt64, 15, new Type[1] { typeof(object) }, typeof(ulong)),
						new StiParserMethodInfo(StiFunctionType.StiNullValuesHelperIsNull, 1, new Type[2]
						{
							typeof(StiReport),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.TimeSpanFromDays, 1, new Type[1] { typeof(double) }, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.TimeSpanFromHours, 1, new Type[1] { typeof(double) }, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.TimeSpanFromMilliseconds, 1, new Type[1] { typeof(double) }, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.TimeSpanFromMinutes, 1, new Type[1] { typeof(double) }, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.TimeSpanFromSeconds, 1, new Type[1] { typeof(double) }, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.TimeSpanFromTicks, 1, new Type[1] { typeof(long) }, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.ImageFromFile, 1, new Type[1] { typeof(string) }, typeof(Image)),
						new StiParserMethodInfo(StiFunctionType.m_ToShortDateString, 1, new Type[1] { typeof(DateTime) }),
						new StiParserMethodInfo(StiFunctionType.m_ToShortTimeString, 1, new Type[1] { typeof(DateTime) }),
						new StiParserMethodInfo(StiFunctionType.m_ToLongDateString, 1, new Type[1] { typeof(DateTime) }),
						new StiParserMethodInfo(StiFunctionType.m_ToLongTimeString, 1, new Type[1] { typeof(DateTime) }),
						new StiParserMethodInfo(StiFunctionType.m_Substring, 1, new Type[2]
						{
							typeof(string),
							typeof(int)
						}),
						new StiParserMethodInfo(StiFunctionType.m_Substring, 2, new Type[3]
						{
							typeof(string),
							typeof(int),
							typeof(int)
						}),
						new StiParserMethodInfo(StiFunctionType.m_ToLower, 1, new Type[1] { typeof(string) }),
						new StiParserMethodInfo(StiFunctionType.m_ToUpper, 1, new Type[1] { typeof(string) }),
						new StiParserMethodInfo(StiFunctionType.m_IndexOf, 1, new Type[2]
						{
							typeof(string),
							typeof(string)
						}, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.m_StartsWith, 1, new Type[2]
						{
							typeof(string),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_EndsWith, 1, new Type[2]
						{
							typeof(string),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Replace, 1, new Type[3]
						{
							typeof(string),
							typeof(char),
							typeof(char)
						}),
						new StiParserMethodInfo(StiFunctionType.m_Replace, 2, new Type[3]
						{
							typeof(string),
							typeof(string),
							typeof(string)
						}),
						new StiParserMethodInfo(StiFunctionType.m_PadLeft, 1, new Type[2]
						{
							typeof(string),
							typeof(int)
						}),
						new StiParserMethodInfo(StiFunctionType.m_PadLeft, 2, new Type[3]
						{
							typeof(string),
							typeof(int),
							typeof(char)
						}),
						new StiParserMethodInfo(StiFunctionType.m_PadRight, 1, new Type[2]
						{
							typeof(string),
							typeof(int)
						}),
						new StiParserMethodInfo(StiFunctionType.m_PadRight, 2, new Type[3]
						{
							typeof(string),
							typeof(int),
							typeof(char)
						}),
						new StiParserMethodInfo(StiFunctionType.m_TrimStart, 1, new Type[1] { typeof(string) }),
						new StiParserMethodInfo(StiFunctionType.m_TrimEnd, 1, new Type[1] { typeof(string) }),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 1, new Type[2]
						{
							typeof(string),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 210, new Type[2]
						{
							typeof(BoolList),
							typeof(bool)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 220, new Type[2]
						{
							typeof(CharList),
							typeof(char)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 230, new Type[2]
						{
							typeof(DateTimeList),
							typeof(DateTime)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 231, new Type[2]
						{
							typeof(DateTimeList),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 240, new Type[2]
						{
							typeof(TimeSpanList),
							typeof(TimeSpan)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 241, new Type[2]
						{
							typeof(TimeSpanList),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 250, new Type[2]
						{
							typeof(DecimalList),
							typeof(decimal)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 251, new Type[2]
						{
							typeof(DecimalList),
							typeof(long)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 252, new Type[2]
						{
							typeof(DecimalList),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 260, new Type[2]
						{
							typeof(FloatList),
							typeof(float)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 261, new Type[2]
						{
							typeof(FloatList),
							typeof(long)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 262, new Type[2]
						{
							typeof(FloatList),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 270, new Type[2]
						{
							typeof(DoubleList),
							typeof(double)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 271, new Type[2]
						{
							typeof(DoubleList),
							typeof(long)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 272, new Type[2]
						{
							typeof(DoubleList),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 280, new Type[2]
						{
							typeof(ByteList),
							typeof(byte)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 282, new Type[2]
						{
							typeof(ByteList),
							typeof(decimal)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 283, new Type[2]
						{
							typeof(ByteList),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 290, new Type[2]
						{
							typeof(ShortList),
							typeof(short)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 292, new Type[2]
						{
							typeof(ShortList),
							typeof(decimal)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 293, new Type[2]
						{
							typeof(ShortList),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 300, new Type[2]
						{
							typeof(IntList),
							typeof(int)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 302, new Type[2]
						{
							typeof(IntList),
							typeof(decimal)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 303, new Type[2]
						{
							typeof(IntList),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 310, new Type[2]
						{
							typeof(LongList),
							typeof(long)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 312, new Type[2]
						{
							typeof(LongList),
							typeof(decimal)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 313, new Type[2]
						{
							typeof(LongList),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 320, new Type[2]
						{
							typeof(GuidList),
							typeof(Guid)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 330, new Type[2]
						{
							typeof(StringList),
							typeof(string)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 331, new Type[2]
						{
							typeof(StringList),
							typeof(long)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 332, new Type[2]
						{
							typeof(StringList),
							typeof(decimal)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.m_Contains, 333, new Type[2]
						{
							typeof(StringList),
							typeof(double)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.op_Add, 1, new Type[2]
						{
							typeof(int),
							typeof(int)
						}, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.op_Add, 2, new Type[2]
						{
							typeof(uint),
							typeof(uint)
						}, typeof(uint)),
						new StiParserMethodInfo(StiFunctionType.op_Add, 3, new Type[2]
						{
							typeof(long),
							typeof(long)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.op_Add, 4, new Type[2]
						{
							typeof(ulong),
							typeof(ulong)
						}, typeof(ulong)),
						new StiParserMethodInfo(StiFunctionType.op_Add, 5, new Type[2]
						{
							typeof(float),
							typeof(float)
						}, typeof(float)),
						new StiParserMethodInfo(StiFunctionType.op_Add, 6, new Type[2]
						{
							typeof(double),
							typeof(double)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.op_Add, 7, new Type[2]
						{
							typeof(decimal),
							typeof(decimal)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.op_Add, 8, new Type[2]
						{
							typeof(string),
							typeof(string)
						}, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.op_Add, 9, new Type[2]
						{
							typeof(string),
							typeof(object)
						}, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.op_Add, 10, new Type[2]
						{
							typeof(object),
							typeof(string)
						}, typeof(string)),
						new StiParserMethodInfo(StiFunctionType.op_Add, 11, new Type[2]
						{
							typeof(DateTime),
							typeof(TimeSpan)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.op_Add, 12, new Type[2]
						{
							typeof(TimeSpan),
							typeof(DateTime)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.op_Sub, 1, new Type[2]
						{
							typeof(int),
							typeof(int)
						}, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.op_Sub, 2, new Type[2]
						{
							typeof(uint),
							typeof(uint)
						}, typeof(uint)),
						new StiParserMethodInfo(StiFunctionType.op_Sub, 3, new Type[2]
						{
							typeof(long),
							typeof(long)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.op_Sub, 4, new Type[2]
						{
							typeof(ulong),
							typeof(ulong)
						}, typeof(ulong)),
						new StiParserMethodInfo(StiFunctionType.op_Sub, 5, new Type[2]
						{
							typeof(float),
							typeof(float)
						}, typeof(float)),
						new StiParserMethodInfo(StiFunctionType.op_Sub, 6, new Type[2]
						{
							typeof(double),
							typeof(double)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.op_Sub, 7, new Type[2]
						{
							typeof(decimal),
							typeof(decimal)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.op_Sub, 8, new Type[2]
						{
							typeof(DateTime),
							typeof(DateTime)
						}, typeof(TimeSpan)),
						new StiParserMethodInfo(StiFunctionType.op_Sub, 9, new Type[2]
						{
							typeof(DateTime),
							typeof(TimeSpan)
						}, typeof(DateTime)),
						new StiParserMethodInfo(StiFunctionType.op_Mult, 1, new Type[2]
						{
							typeof(int),
							typeof(int)
						}, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.op_Mult, 2, new Type[2]
						{
							typeof(uint),
							typeof(uint)
						}, typeof(uint)),
						new StiParserMethodInfo(StiFunctionType.op_Mult, 3, new Type[2]
						{
							typeof(long),
							typeof(long)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.op_Mult, 4, new Type[2]
						{
							typeof(ulong),
							typeof(ulong)
						}, typeof(ulong)),
						new StiParserMethodInfo(StiFunctionType.op_Mult, 5, new Type[2]
						{
							typeof(float),
							typeof(float)
						}, typeof(float)),
						new StiParserMethodInfo(StiFunctionType.op_Mult, 6, new Type[2]
						{
							typeof(double),
							typeof(double)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.op_Mult, 7, new Type[2]
						{
							typeof(decimal),
							typeof(decimal)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.op_Div, 1, new Type[2]
						{
							typeof(int),
							typeof(int)
						}, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.op_Div, 2, new Type[2]
						{
							typeof(uint),
							typeof(uint)
						}, typeof(uint)),
						new StiParserMethodInfo(StiFunctionType.op_Div, 3, new Type[2]
						{
							typeof(long),
							typeof(long)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.op_Div, 4, new Type[2]
						{
							typeof(ulong),
							typeof(ulong)
						}, typeof(ulong)),
						new StiParserMethodInfo(StiFunctionType.op_Div, 5, new Type[2]
						{
							typeof(float),
							typeof(float)
						}, typeof(float)),
						new StiParserMethodInfo(StiFunctionType.op_Div, 6, new Type[2]
						{
							typeof(double),
							typeof(double)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.op_Div, 7, new Type[2]
						{
							typeof(decimal),
							typeof(decimal)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.op_Mod, 1, new Type[2]
						{
							typeof(int),
							typeof(int)
						}, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.op_Mod, 2, new Type[2]
						{
							typeof(uint),
							typeof(uint)
						}, typeof(uint)),
						new StiParserMethodInfo(StiFunctionType.op_Mod, 3, new Type[2]
						{
							typeof(long),
							typeof(long)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.op_Mod, 4, new Type[2]
						{
							typeof(ulong),
							typeof(ulong)
						}, typeof(ulong)),
						new StiParserMethodInfo(StiFunctionType.op_Mod, 5, new Type[2]
						{
							typeof(float),
							typeof(float)
						}, typeof(float)),
						new StiParserMethodInfo(StiFunctionType.op_Mod, 6, new Type[2]
						{
							typeof(double),
							typeof(double)
						}, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.op_Mod, 7, new Type[2]
						{
							typeof(decimal),
							typeof(decimal)
						}, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.op_Shl, 1, new Type[2]
						{
							typeof(int),
							typeof(int)
						}, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.op_Shl, 2, new Type[2]
						{
							typeof(uint),
							typeof(int)
						}, typeof(uint)),
						new StiParserMethodInfo(StiFunctionType.op_Shl, 3, new Type[2]
						{
							typeof(long),
							typeof(int)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.op_Shl, 4, new Type[2]
						{
							typeof(ulong),
							typeof(int)
						}, typeof(ulong)),
						new StiParserMethodInfo(StiFunctionType.op_Shr, 1, new Type[2]
						{
							typeof(int),
							typeof(int)
						}, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.op_Shr, 2, new Type[2]
						{
							typeof(uint),
							typeof(int)
						}, typeof(uint)),
						new StiParserMethodInfo(StiFunctionType.op_Shr, 3, new Type[2]
						{
							typeof(long),
							typeof(int)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.op_Shr, 4, new Type[2]
						{
							typeof(ulong),
							typeof(int)
						}, typeof(ulong)),
						new StiParserMethodInfo(StiFunctionType.op_And, 1, new Type[2]
						{
							typeof(int),
							typeof(int)
						}, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.op_And, 2, new Type[2]
						{
							typeof(uint),
							typeof(uint)
						}, typeof(uint)),
						new StiParserMethodInfo(StiFunctionType.op_And, 3, new Type[2]
						{
							typeof(long),
							typeof(long)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.op_And, 4, new Type[2]
						{
							typeof(ulong),
							typeof(ulong)
						}, typeof(ulong)),
						new StiParserMethodInfo(StiFunctionType.op_And, 5, new Type[2]
						{
							typeof(bool),
							typeof(bool)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.op_Or, 1, new Type[2]
						{
							typeof(int),
							typeof(int)
						}, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.op_Or, 2, new Type[2]
						{
							typeof(uint),
							typeof(uint)
						}, typeof(uint)),
						new StiParserMethodInfo(StiFunctionType.op_Or, 3, new Type[2]
						{
							typeof(long),
							typeof(long)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.op_Or, 4, new Type[2]
						{
							typeof(ulong),
							typeof(ulong)
						}, typeof(ulong)),
						new StiParserMethodInfo(StiFunctionType.op_Or, 5, new Type[2]
						{
							typeof(bool),
							typeof(bool)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.op_Xor, 1, new Type[2]
						{
							typeof(int),
							typeof(int)
						}, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.op_Xor, 2, new Type[2]
						{
							typeof(uint),
							typeof(uint)
						}, typeof(uint)),
						new StiParserMethodInfo(StiFunctionType.op_Xor, 3, new Type[2]
						{
							typeof(long),
							typeof(long)
						}, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.op_Xor, 4, new Type[2]
						{
							typeof(ulong),
							typeof(ulong)
						}, typeof(ulong)),
						new StiParserMethodInfo(StiFunctionType.op_Xor, 5, new Type[2]
						{
							typeof(bool),
							typeof(bool)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.op_And2, 1, new Type[2]
						{
							typeof(bool),
							typeof(bool)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.op_Or2, 1, new Type[2]
						{
							typeof(bool),
							typeof(bool)
						}, typeof(bool)),
						new StiParserMethodInfo(StiFunctionType.op_Neg, 1, new Type[1] { typeof(int) }, typeof(int)),
						new StiParserMethodInfo(StiFunctionType.op_Neg, 2, new Type[1] { typeof(long) }, typeof(long)),
						new StiParserMethodInfo(StiFunctionType.op_Neg, 3, new Type[1] { typeof(float) }, typeof(float)),
						new StiParserMethodInfo(StiFunctionType.op_Neg, 4, new Type[1] { typeof(double) }, typeof(double)),
						new StiParserMethodInfo(StiFunctionType.op_Neg, 5, new Type[1] { typeof(decimal) }, typeof(decimal)),
						new StiParserMethodInfo(StiFunctionType.op_Not, 1, new Type[1] { typeof(bool) }, typeof(bool))
					};
					methodsHash = new Hashtable();
					StiParserMethodInfo[] array2 = array;
					foreach (StiParserMethodInfo stiParserMethodInfo in array2)
					{
						List<StiParserMethodInfo> list = (List<StiParserMethodInfo>)methodsHash[stiParserMethodInfo.Name];
						if (list == null)
						{
							list = new List<StiParserMethodInfo>();
							methodsHash[stiParserMethodInfo.Name] = list;
						}
						list.Add(stiParserMethodInfo);
					}
				}
			}
			return methodsHash;
		}
	}

	private Hashtable ConstantsList
	{
		get
		{
			if (constantsList == null)
			{
				constantsList = new Hashtable();
				constantsList["true"] = true;
				constantsList["True"] = true;
				constantsList["false"] = false;
				constantsList["False"] = false;
				constantsList["null"] = null;
				constantsList["DBNull"] = namespaceObj;
				constantsList["DBNull.Value"] = DBNull.Value;
				constantsList["string.Empty"] = string.Empty;
				constantsList["MidpointRounding"] = namespaceObj;
				constantsList["MidpointRounding.ToEven"] = MidpointRounding.ToEven;
				constantsList["MidpointRounding.AwayFromZero"] = MidpointRounding.AwayFromZero;
				constantsList["StiRankOrder"] = namespaceObj;
				constantsList["StiRankOrder.Asc"] = StiRankOrder.Asc;
				constantsList["StiRankOrder.Desc"] = StiRankOrder.Desc;
				constantsList["DayOfWeek"] = namespaceObj;
				constantsList["DayOfWeek.Sunday"] = DayOfWeek.Sunday;
				constantsList["DayOfWeek.Monday"] = DayOfWeek.Monday;
				constantsList["DayOfWeek.Tuesday"] = DayOfWeek.Tuesday;
				constantsList["DayOfWeek.Wednesday"] = DayOfWeek.Wednesday;
				constantsList["DayOfWeek.Thursday"] = DayOfWeek.Thursday;
				constantsList["DayOfWeek.Friday"] = DayOfWeek.Friday;
				constantsList["DayOfWeek.Saturday"] = DayOfWeek.Saturday;
				constantsList["CalendarWeekRule"] = namespaceObj;
				constantsList["CalendarWeekRule.FirstDay"] = CalendarWeekRule.FirstDay;
				constantsList["CalendarWeekRule.FirstFullWeek"] = CalendarWeekRule.FirstFullWeek;
				constantsList["CalendarWeekRule.FirstFourDayWeek"] = CalendarWeekRule.FirstFourDayWeek;
				constantsList["System.Globalization.CalendarWeekRule"] = namespaceObj;
				constantsList["System.Globalization.CalendarWeekRule.FirstDay"] = CalendarWeekRule.FirstDay;
				constantsList["System.Globalization.CalendarWeekRule.FirstFullWeek"] = CalendarWeekRule.FirstFullWeek;
				constantsList["System.Globalization.CalendarWeekRule.FirstFourDayWeek"] = CalendarWeekRule.FirstFourDayWeek;
				constantsList["Color.AliceBlue"] = Color.AliceBlue;
				constantsList["Color.AntiqueWhite"] = Color.AntiqueWhite;
				constantsList["Color.Aqua"] = Color.Aqua;
				constantsList["Color.Aquamarine"] = Color.Aquamarine;
				constantsList["Color.Azure"] = Color.Azure;
				constantsList["Color.Beige"] = Color.Beige;
				constantsList["Color.Bisque"] = Color.Bisque;
				constantsList["Color.Black"] = Color.Black;
				constantsList["Color.BlanchedAlmond"] = Color.BlanchedAlmond;
				constantsList["Color.Blue"] = Color.Blue;
				constantsList["Color.BlueViolet"] = Color.BlueViolet;
				constantsList["Color.Brown"] = Color.Brown;
				constantsList["Color.BurlyWood"] = Color.BurlyWood;
				constantsList["Color.CadetBlue"] = Color.CadetBlue;
				constantsList["Color.Chartreuse"] = Color.Chartreuse;
				constantsList["Color.Chocolate"] = Color.Chocolate;
				constantsList["Color.Coral"] = Color.Coral;
				constantsList["Color.CornflowerBlue"] = Color.CornflowerBlue;
				constantsList["Color.Cornsilk"] = Color.Cornsilk;
				constantsList["Color.Crimson"] = Color.Crimson;
				constantsList["Color.Cyan"] = Color.Cyan;
				constantsList["Color.DarkBlue"] = Color.DarkBlue;
				constantsList["Color.DarkCyan"] = Color.DarkCyan;
				constantsList["Color.DarkGoldenrod"] = Color.DarkGoldenrod;
				constantsList["Color.DarkGray"] = Color.DarkGray;
				constantsList["Color.DarkGreen"] = Color.DarkGreen;
				constantsList["Color.DarkKhaki"] = Color.DarkKhaki;
				constantsList["Color.DarkMagenta"] = Color.DarkMagenta;
				constantsList["Color.DarkOliveGreen"] = Color.DarkOliveGreen;
				constantsList["Color.DarkOrange"] = Color.DarkOrange;
				constantsList["Color.DarkOrchid"] = Color.DarkOrchid;
				constantsList["Color.DarkRed"] = Color.DarkRed;
				constantsList["Color.DarkSalmon"] = Color.DarkSalmon;
				constantsList["Color.DarkSeaGreen"] = Color.DarkSeaGreen;
				constantsList["Color.DarkSlateBlue"] = Color.DarkSlateBlue;
				constantsList["Color.DarkSlateGray"] = Color.DarkSlateGray;
				constantsList["Color.DarkTurquoise"] = Color.DarkTurquoise;
				constantsList["Color.DarkViolet"] = Color.DarkViolet;
				constantsList["Color.DeepPink"] = Color.DeepPink;
				constantsList["Color.DeepSkyBlue"] = Color.DeepSkyBlue;
				constantsList["Color.DimGray"] = Color.DimGray;
				constantsList["Color.DodgerBlue"] = Color.DodgerBlue;
				constantsList["Color.Firebrick"] = Color.Firebrick;
				constantsList["Color.FloralWhite"] = Color.FloralWhite;
				constantsList["Color.ForestGreen"] = Color.ForestGreen;
				constantsList["Color.Fuchsia"] = Color.Fuchsia;
				constantsList["Color.Gainsboro"] = Color.Gainsboro;
				constantsList["Color.GhostWhite"] = Color.GhostWhite;
				constantsList["Color.Gold"] = Color.Gold;
				constantsList["Color.Goldenrod"] = Color.Goldenrod;
				constantsList["Color.Gray"] = Color.Gray;
				constantsList["Color.Green"] = Color.Green;
				constantsList["Color.GreenYellow"] = Color.GreenYellow;
				constantsList["Color.Honeydew"] = Color.Honeydew;
				constantsList["Color.HotPink"] = Color.HotPink;
				constantsList["Color.IndianRed"] = Color.IndianRed;
				constantsList["Color.Indigo"] = Color.Indigo;
				constantsList["Color.Ivory"] = Color.Ivory;
				constantsList["Color.Khaki"] = Color.Khaki;
				constantsList["Color.Lavender"] = Color.Lavender;
				constantsList["Color.LavenderBlush"] = Color.LavenderBlush;
				constantsList["Color.LawnGreen"] = Color.LawnGreen;
				constantsList["Color.LemonChiffon"] = Color.LemonChiffon;
				constantsList["Color.LightBlue"] = Color.LightBlue;
				constantsList["Color.LightCoral"] = Color.LightCoral;
				constantsList["Color.LightCyan"] = Color.LightCyan;
				constantsList["Color.LightGoldenrodYellow"] = Color.LightGoldenrodYellow;
				constantsList["Color.LightGray"] = Color.LightGray;
				constantsList["Color.LightGreen"] = Color.LightGreen;
				constantsList["Color.LightPink"] = Color.LightPink;
				constantsList["Color.LightSalmon"] = Color.LightSalmon;
				constantsList["Color.LightSeaGreen"] = Color.LightSeaGreen;
				constantsList["Color.LightSkyBlue"] = Color.LightSkyBlue;
				constantsList["Color.LightSlateGray"] = Color.LightSlateGray;
				constantsList["Color.LightSteelBlue"] = Color.LightSteelBlue;
				constantsList["Color.LightYellow"] = Color.LightYellow;
				constantsList["Color.Lime"] = Color.Lime;
				constantsList["Color.LimeGreen"] = Color.LimeGreen;
				constantsList["Color.Linen"] = Color.Linen;
				constantsList["Color.Magenta"] = Color.Magenta;
				constantsList["Color.Maroon"] = Color.Maroon;
				constantsList["Color.MediumAquamarine"] = Color.MediumAquamarine;
				constantsList["Color.MediumBlue"] = Color.MediumBlue;
				constantsList["Color.MediumOrchid"] = Color.MediumOrchid;
				constantsList["Color.MediumPurple"] = Color.MediumPurple;
				constantsList["Color.MediumSeaGreen"] = Color.MediumSeaGreen;
				constantsList["Color.MediumSlateBlue"] = Color.MediumSlateBlue;
				constantsList["Color.MediumSpringGreen"] = Color.MediumSpringGreen;
				constantsList["Color.MediumTurquoise"] = Color.MediumTurquoise;
				constantsList["Color.MediumVioletRed"] = Color.MediumVioletRed;
				constantsList["Color.MidnightBlue"] = Color.MidnightBlue;
				constantsList["Color.MintCream"] = Color.MintCream;
				constantsList["Color.MistyRose"] = Color.MistyRose;
				constantsList["Color.Moccasin"] = Color.Moccasin;
				constantsList["Color.NavajoWhite"] = Color.NavajoWhite;
				constantsList["Color.Navy"] = Color.Navy;
				constantsList["Color.OldLace"] = Color.OldLace;
				constantsList["Color.Olive"] = Color.Olive;
				constantsList["Color.OliveDrab"] = Color.OliveDrab;
				constantsList["Color.Orange"] = Color.Orange;
				constantsList["Color.OrangeRed"] = Color.OrangeRed;
				constantsList["Color.Orchid"] = Color.Orchid;
				constantsList["Color.PaleGoldenrod"] = Color.PaleGoldenrod;
				constantsList["Color.PaleGreen"] = Color.PaleGreen;
				constantsList["Color.PaleTurquoise"] = Color.PaleTurquoise;
				constantsList["Color.PaleVioletRed"] = Color.PaleVioletRed;
				constantsList["Color.PapayaWhip"] = Color.PapayaWhip;
				constantsList["Color.PeachPuff"] = Color.PeachPuff;
				constantsList["Color.Peru"] = Color.Peru;
				constantsList["Color.Pink"] = Color.Pink;
				constantsList["Color.Plum"] = Color.Plum;
				constantsList["Color.PowderBlue"] = Color.PowderBlue;
				constantsList["Color.Purple"] = Color.Purple;
				constantsList["Color.Red"] = Color.Red;
				constantsList["Color.RosyBrown"] = Color.RosyBrown;
				constantsList["Color.RoyalBlue"] = Color.RoyalBlue;
				constantsList["Color.SaddleBrown"] = Color.SaddleBrown;
				constantsList["Color.Salmon"] = Color.Salmon;
				constantsList["Color.SandyBrown"] = Color.SandyBrown;
				constantsList["Color.SeaGreen"] = Color.SeaGreen;
				constantsList["Color.SeaShell"] = Color.SeaShell;
				constantsList["Color.Sienna"] = Color.Sienna;
				constantsList["Color.Silver"] = Color.Silver;
				constantsList["Color.SkyBlue"] = Color.SkyBlue;
				constantsList["Color.SlateBlue"] = Color.SlateBlue;
				constantsList["Color.SlateGray"] = Color.SlateGray;
				constantsList["Color.Snow"] = Color.Snow;
				constantsList["Color.SpringGreen"] = Color.SpringGreen;
				constantsList["Color.SteelBlue"] = Color.SteelBlue;
				constantsList["Color.Tan"] = Color.Tan;
				constantsList["Color.Teal"] = Color.Teal;
				constantsList["Color.Thistle"] = Color.Thistle;
				constantsList["Color.Tomato"] = Color.Tomato;
				constantsList["Color.Transparent"] = Color.Transparent;
				constantsList["Color.Turquoise"] = Color.Turquoise;
				constantsList["Color.Violet"] = Color.Violet;
				constantsList["Color.Wheat"] = Color.Wheat;
				constantsList["Color.White"] = Color.White;
				constantsList["Color.WhiteSmoke"] = Color.WhiteSmoke;
				constantsList["Color.Yellow"] = Color.Yellow;
				constantsList["Color.YellowGreen"] = Color.YellowGreen;
				constantsList["HatchStyle.BackwardDiagonal"] = (object)(HatchStyle)3;
				constantsList["HatchStyle.Cross"] = (object)(HatchStyle)4;
				constantsList["HatchStyle.DarkDownwardDiagonal"] = (object)(HatchStyle)20;
				constantsList["HatchStyle.DarkHorizontal"] = (object)(HatchStyle)29;
				constantsList["HatchStyle.DarkUpwardDiagonal"] = (object)(HatchStyle)21;
				constantsList["HatchStyle.DarkVertical"] = (object)(HatchStyle)28;
				constantsList["HatchStyle.DashedDownwardDiagonal"] = (object)(HatchStyle)30;
				constantsList["HatchStyle.DashedHorizontal"] = (object)(HatchStyle)32;
				constantsList["HatchStyle.DashedUpwardDiagonal"] = (object)(HatchStyle)31;
				constantsList["HatchStyle.DashedVertical"] = (object)(HatchStyle)33;
				constantsList["HatchStyle.DiagonalBrick"] = (object)(HatchStyle)38;
				constantsList["HatchStyle.DiagonalCross"] = (object)(HatchStyle)5;
				constantsList["HatchStyle.Divot"] = (object)(HatchStyle)42;
				constantsList["HatchStyle.DottedDiamond"] = (object)(HatchStyle)44;
				constantsList["HatchStyle.DottedGrid"] = (object)(HatchStyle)43;
				constantsList["HatchStyle.ForwardDiagonal"] = (object)(HatchStyle)2;
				constantsList["HatchStyle.Horizontal"] = (object)(HatchStyle)0;
				constantsList["HatchStyle.HorizontalBrick"] = (object)(HatchStyle)39;
				constantsList["HatchStyle.LargeCheckerBoard"] = (object)(HatchStyle)50;
				constantsList["HatchStyle.LargeConfetti"] = (object)(HatchStyle)35;
				constantsList["HatchStyle.LargeGrid"] = (object)(HatchStyle)4;
				constantsList["HatchStyle.LightDownwardDiagonal"] = (object)(HatchStyle)18;
				constantsList["HatchStyle.LightHorizontal"] = (object)(HatchStyle)25;
				constantsList["HatchStyle.LightUpwardDiagonal"] = (object)(HatchStyle)19;
				constantsList["HatchStyle.LightVertical"] = (object)(HatchStyle)24;
				constantsList["HatchStyle.Max"] = (object)(HatchStyle)4;
				constantsList["HatchStyle.Min"] = (object)(HatchStyle)0;
				constantsList["HatchStyle.NarrowHorizontal"] = (object)(HatchStyle)27;
				constantsList["HatchStyle.NarrowVertical"] = (object)(HatchStyle)26;
				constantsList["HatchStyle.OutlinedDiamond"] = (object)(HatchStyle)51;
				constantsList["HatchStyle.Percent05"] = (object)(HatchStyle)6;
				constantsList["HatchStyle.Percent10"] = (object)(HatchStyle)7;
				constantsList["HatchStyle.Percent20"] = (object)(HatchStyle)8;
				constantsList["HatchStyle.Percent25"] = (object)(HatchStyle)9;
				constantsList["HatchStyle.Percent30"] = (object)(HatchStyle)10;
				constantsList["HatchStyle.Percent40"] = (object)(HatchStyle)11;
				constantsList["HatchStyle.Percent50"] = (object)(HatchStyle)12;
				constantsList["HatchStyle.Percent60"] = (object)(HatchStyle)13;
				constantsList["HatchStyle.Percent70"] = (object)(HatchStyle)14;
				constantsList["HatchStyle.Percent75"] = (object)(HatchStyle)15;
				constantsList["HatchStyle.Percent80"] = (object)(HatchStyle)16;
				constantsList["HatchStyle.Percent90"] = (object)(HatchStyle)17;
				constantsList["HatchStyle.Plaid"] = (object)(HatchStyle)41;
				constantsList["HatchStyle.Shingle"] = (object)(HatchStyle)45;
				constantsList["HatchStyle.SmallCheckerBoard"] = (object)(HatchStyle)49;
				constantsList["HatchStyle.SmallConfetti"] = (object)(HatchStyle)34;
				constantsList["HatchStyle.SmallGrid"] = (object)(HatchStyle)48;
				constantsList["HatchStyle.SolidDiamond"] = (object)(HatchStyle)52;
				constantsList["HatchStyle.Sphere"] = (object)(HatchStyle)47;
				constantsList["HatchStyle.Trellis"] = (object)(HatchStyle)46;
				constantsList["HatchStyle.Vertical"] = (object)(HatchStyle)1;
				constantsList["HatchStyle.Wave"] = (object)(HatchStyle)37;
				constantsList["HatchStyle.Weave"] = (object)(HatchStyle)40;
				constantsList["HatchStyle.WideDownwardDiagonal"] = (object)(HatchStyle)22;
				constantsList["HatchStyle.WideUpwardDiagonal"] = (object)(HatchStyle)23;
				constantsList["HatchStyle.ZigZag"] = (object)(HatchStyle)36;
				constantsList_low = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
				foreach (DictionaryEntry constants in constantsList)
				{
					constantsList_low[((string)constants.Key).ToLowerInvariant()] = constants.Value;
				}
			}
			if (!syntaxCaseSensitive)
			{
				return constantsList_low;
			}
			return constantsList;
		}
	}

	private Hashtable NamespacesList
	{
		get
		{
			if (namespacesList == null)
			{
				namespacesList = new Hashtable();
				namespacesList["Totals"] = namespaceObj;
				namespacesList["System"] = namespaceObj;
				namespacesList["System.Convert"] = namespaceObj;
				namespacesList["System.Globalization"] = namespaceObj;
				namespacesList["Convert"] = namespaceObj;
				namespacesList["Math"] = namespaceObj;
				namespacesList["Func"] = namespaceObj;
				namespacesList["Func.EngineHelper"] = namespaceObj;
				namespacesList["StiNullValuesHelper"] = namespaceObj;
				namespacesList["Image"] = namespaceObj;
				namespacesList["Color"] = namespaceObj;
				namespacesList["HatchStyle"] = namespaceObj;
				namespacesList_low = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
				foreach (DictionaryEntry namespaces in namespacesList)
				{
					namespacesList_low[((string)namespaces.Key).ToLowerInvariant()] = namespaces.Value;
				}
			}
			if (!syntaxCaseSensitive)
			{
				return namespacesList_low;
			}
			return namespacesList;
		}
	}

	private Hashtable UserFunctionsList
	{
		get
		{
			if (userFunctionsList == null)
			{
				lock (lockUserFunctionsList)
				{
					userFunctionsList = new Hashtable();
					Hashtable hashtable = new Hashtable();
					StiFunction[] functions = StiFunctions.GetFunctions(isCompile: false);
					StiFunction[] array = functions;
					foreach (StiFunction stiFunction in array)
					{
						List<StiFunction> list = hashtable[stiFunction.FunctionName] as List<StiFunction>;
						if (list == null)
						{
							list = new List<StiFunction>();
							hashtable[stiFunction.FunctionName] = list;
							userFunctionsList[stiFunction.FunctionName] = 3000 + userFunctionsList.Count;
						}
						list.Add(stiFunction);
					}
				}
			}
			return userFunctionsList;
		}
	}

	private Hashtable OperatorsList
	{
		get
		{
			if (operatorsList == null)
			{
				operatorsList = new Hashtable();
				operatorsList_vb = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
				foreach (DictionaryEntry operators in operatorsList)
				{
					operatorsList_vb[((string)operators.Key).ToLowerInvariant()] = operators.Value;
				}
				operatorsList_vb["Not"] = StiTokenType.Not;
				operatorsList_vb["And"] = StiTokenType.And;
				operatorsList_vb["Or"] = StiTokenType.Or;
				operatorsList_vb["Xor"] = StiTokenType.Xor;
				operatorsList_vb["AndAlso"] = StiTokenType.DoubleAnd;
				operatorsList_vb["OrElse"] = StiTokenType.DoubleOr;
				operatorsList_vb["Mod"] = StiTokenType.Percent;
				operatorsList_low = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
				foreach (DictionaryEntry operators2 in operatorsList)
				{
					operatorsList_low[((string)operators2.Key).ToLowerInvariant()] = operators2.Value;
				}
			}
			if (IsVB)
			{
				return operatorsList_vb;
			}
			if (!syntaxCaseSensitive)
			{
				return operatorsList_low;
			}
			return operatorsList;
		}
	}

	private Hashtable KeywordsList
	{
		get
		{
			if (keywordsList == null)
			{
				keywordsList = new Hashtable();
				keywordsList["ref"] = StiTokenType.RefVariable;
				keywordsList["out"] = StiTokenType.RefVariable;
			}
			return keywordsList;
		}
	}

	private object call_func(object name, ArrayList argsList, StiAsmCommand asmCommand)
	{
		StiFunctionType stiFunctionType = (StiFunctionType)name;
		int num = CheckParserMethodInfo2(stiFunctionType, argsList, asmCommand);
		switch (stiFunctionType)
		{
		case StiFunctionType.Count:
			return Totals.Count(argsList[0], report, null);
		case StiFunctionType.CountDistinct:
			return Totals.CountDistinct(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.Avg:
			return Totals.Avg(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.AvgD:
			return Totals.AvgD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.AvgDate:
			return Totals.AvgDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.AvgI:
			return Totals.AvgI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.AvgTime:
			return Totals.AvgTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.Max:
			return Totals.Max(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.MaxD:
			return Totals.MaxD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.MaxDate:
			return Totals.MaxDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.MaxI:
			return Totals.MaxI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.MaxStr:
			return Totals.MaxStr(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.MaxTime:
			return Totals.MaxTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.Median:
			return Totals.Median(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.MedianD:
			return Totals.MedianD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.MedianI:
			return Totals.MedianI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.Min:
			return Totals.Min(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.MinD:
			return Totals.MinD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.MinDate:
			return Totals.MinDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.MinI:
			return Totals.MinI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.MinStr:
			return Totals.MinStr(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.MinTime:
			return Totals.MinTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.Mode:
			return Totals.Mode(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.ModeD:
			return Totals.ModeD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.ModeI:
			return Totals.ModeI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.Sum:
			return Totals.Sum(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.SumD:
			return Totals.SumD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.SumDistinct:
		{
			StiParserData stiParserData = new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this);
			stiParserData.AsmList2 = (List<StiAsmCommand>)argsList[2];
			return Totals.SumDistinct(stiParserData, report, null);
		}
		case StiFunctionType.SumI:
			return Totals.SumI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.SumTime:
			return Totals.SumTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.First:
			return Totals.First(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.Last:
			return Totals.Last(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pCount:
			return Totals.cCount(argsList[0], report, null);
		case StiFunctionType.pCountDistinct:
			return Totals.cCountDistinct(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pAvg:
			return Totals.cAvg(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pAvgD:
			return Totals.cAvgD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pAvgDate:
			return Totals.cAvgDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pAvgI:
			return Totals.cAvgI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pAvgTime:
			return Totals.cAvgTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pMax:
			return Totals.cMax(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pMaxD:
			return Totals.cMaxD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pMaxDate:
			return Totals.cMaxDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pMaxI:
			return Totals.cMaxI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pMaxStr:
			return Totals.cMaxStr(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pMaxTime:
			return Totals.cMaxTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pMedian:
			return Totals.cMedian(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pMedianD:
			return Totals.cMedianD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pMedianI:
			return Totals.cMedianI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pMin:
			return Totals.cMin(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pMinD:
			return Totals.cMinD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pMinDate:
			return Totals.cMinDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pMinI:
			return Totals.cMinI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pMinStr:
			return Totals.cMinStr(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pMinTime:
			return Totals.cMinTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pMode:
			return Totals.cMode(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pModeD:
			return Totals.cModeD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pModeI:
			return Totals.cModeI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pSum:
			return Totals.cSum(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pSumD:
			return Totals.cSumD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pSumDistinct:
			return Totals.cSum(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pSumI:
			return Totals.cSumI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pSumTime:
			return Totals.cSumTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pFirst:
			return Totals.cFirst(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.pLast:
			return Totals.cLast(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prCount:
			return Totals.cCountRunning(argsList[0], report, null);
		case StiFunctionType.prCountDistinct:
			return Totals.cCountDistinctRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prAvg:
			return Totals.cAvgRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prAvgD:
			return Totals.cAvgDRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prAvgDate:
			return Totals.cAvgDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prAvgI:
			return Totals.cAvgIRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prAvgTime:
			return Totals.cAvgTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prMax:
			return Totals.cMaxRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prMaxD:
			return Totals.cMaxDRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prMaxDate:
			return Totals.cMaxDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prMaxI:
			return Totals.cMaxIRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prMaxStr:
			return Totals.cMaxStr(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prMaxTime:
			return Totals.cMaxTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prMedian:
			return Totals.cMedianRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prMedianD:
			return Totals.cMedianDRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prMedianI:
			return Totals.cMedianIRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prMin:
			return Totals.cMinRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prMinD:
			return Totals.cMinDRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prMinDate:
			return Totals.cMinDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prMinI:
			return Totals.cMinIRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prMinStr:
			return Totals.cMinStr(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prMinTime:
			return Totals.cMinTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prMode:
			return Totals.cModeRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prModeD:
			return Totals.cModeDRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prModeI:
			return Totals.cModeIRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prSum:
			return Totals.cSumRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prSumD:
			return Totals.cSumDRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prSumDistinct:
			return Totals.cSum(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prSumI:
			return Totals.cSumIRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prSumTime:
			return Totals.cSumTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prFirst:
			return Totals.cFirstRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.prLast:
			return Totals.cLastRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
		case StiFunctionType.iCount:
			return Totals.Count(new StiParserData(argsList[0], null, this, (List<StiAsmCommand>)argsList[1]), report, null);
		case StiFunctionType.iCountDistinct:
			return Totals.CountDistinct(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iAvg:
			return Totals.Avg(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iAvgD:
			return Totals.AvgD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iAvgDate:
			return Totals.AvgDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iAvgI:
			return Totals.AvgI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iAvgTime:
			return Totals.AvgTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iMax:
			return Totals.Max(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iMaxD:
			return Totals.MaxD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iMaxDate:
			return Totals.MaxDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iMaxI:
			return Totals.MaxI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iMaxStr:
			return Totals.MaxStr(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iMaxTime:
			return Totals.MaxTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iMedian:
			return Totals.Median(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iMedianD:
			return Totals.MedianD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iMedianI:
			return Totals.MedianI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iMin:
			return Totals.Min(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iMinD:
			return Totals.MinD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iMinDate:
			return Totals.MinDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iMinI:
			return Totals.MinI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iMinStr:
			return Totals.MinStr(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iMinTime:
			return Totals.MinTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iMode:
			return Totals.Mode(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iModeD:
			return Totals.ModeD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iModeI:
			return Totals.ModeI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iSum:
			return Totals.Sum(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iSumD:
			return Totals.SumD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iSumDistinct:
			return Totals.SumDistinct(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iSumI:
			return Totals.SumI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iSumTime:
			return Totals.SumTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iFirst:
			return Totals.First(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.iLast:
			return Totals.Last(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piCount:
			return Totals.cCount(new StiParserData(argsList[0], null, this, (List<StiAsmCommand>)argsList[1]), report, null);
		case StiFunctionType.piCountDistinct:
			return Totals.cCountDistinct(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piAvg:
			return Totals.cAvg(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piAvgD:
			return Totals.cAvgD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piAvgDate:
			return Totals.cAvgDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piAvgI:
			return Totals.cAvgI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piAvgTime:
			return Totals.cAvgTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piMax:
			return Totals.cMax(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piMaxD:
			return Totals.cMaxD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piMaxDate:
			return Totals.cMaxDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piMaxI:
			return Totals.cMaxI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piMaxStr:
			return Totals.cMaxStr(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piMaxTime:
			return Totals.cMaxTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piMedian:
			return Totals.cMedian(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piMedianD:
			return Totals.cMedianD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piMedianI:
			return Totals.cMedianI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piMin:
			return Totals.cMin(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piMinD:
			return Totals.cMinD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piMinDate:
			return Totals.cMinDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piMinI:
			return Totals.cMinI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piMinStr:
			return Totals.cMinStr(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piMinTime:
			return Totals.cMinTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piMode:
			return Totals.cMode(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piModeD:
			return Totals.cModeD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piModeI:
			return Totals.cModeI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piSum:
			return Totals.cSum(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piSumD:
			return Totals.cSumD(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piSumDistinct:
			return Totals.cSum(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piSumI:
			return Totals.cSumI(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piSumTime:
			return Totals.cSumTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piFirst:
			return Totals.cFirst(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.piLast:
			return Totals.cLast(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priCount:
			return Totals.cCountRunning(new StiParserData(argsList[0], null, this, (List<StiAsmCommand>)argsList[1]), report, null);
		case StiFunctionType.priCountDistinct:
			return Totals.cCountDistinctRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priAvg:
			return Totals.cAvgRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priAvgD:
			return Totals.cAvgDRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priAvgDate:
			return Totals.cAvgDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priAvgI:
			return Totals.cAvgIRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priAvgTime:
			return Totals.cAvgTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priMax:
			return Totals.cMaxRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priMaxD:
			return Totals.cMaxDRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priMaxDate:
			return Totals.cMaxDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priMaxI:
			return Totals.cMaxIRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priMaxStr:
			return Totals.cMaxStr(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priMaxTime:
			return Totals.cMaxTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priMedian:
			return Totals.cMedianRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priMedianD:
			return Totals.cMedianDRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priMedianI:
			return Totals.cMedianIRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priMin:
			return Totals.cMinRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priMinD:
			return Totals.cMinDRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priMinDate:
			return Totals.cMinDate(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priMinI:
			return Totals.cMinIRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priMinStr:
			return Totals.cMinStr(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priMinTime:
			return Totals.cMinTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priMode:
			return Totals.cModeRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priModeD:
			return Totals.cModeDRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priModeI:
			return Totals.cModeIRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priSum:
			return Totals.cSumRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priSumD:
			return Totals.cSumDRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priSumDistinct:
			return Totals.cSum(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priSumI:
			return Totals.cSumIRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priSumTime:
			return Totals.cSumTime(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priFirst:
			return Totals.cFirstRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.priLast:
			return Totals.cLastRunning(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this, (List<StiAsmCommand>)argsList[2]), report, null);
		case StiFunctionType.Rank:
			switch (num)
			{
			case 1:
				return Totals.Rank(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null);
			case 2:
				return Totals.Rank(new StiParserData(argsList[0], (List<StiAsmCommand>)argsList[1], this), report, null, Convert.ToBoolean(argsList[2]), (StiRankOrder)argsList[3]);
			}
			break;
		case StiFunctionType.CountAllLevels:
			return Totals.CountAllLevels(argsList[0]);
		case StiFunctionType.CountAllLevelsOnlyChilds:
			return Totals.CountAllLevelsOnlyChilds(argsList[0]);
		case StiFunctionType.CountOnlyChilds:
			return Totals.CountOnlyChilds(argsList[0]);
		case StiFunctionType.Abs:
			switch (get_category(argsList[0]))
			{
			default:
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Abs", "1", GetTypeName(argsList[0]), "double");
				break;
			case 2:
				return Math.Abs(Convert.ToDecimal(argsList[0]));
			case 3:
				return Math.Abs(Convert.ToDouble(argsList[0]));
			case 4:
			case 5:
			case 6:
			case 7:
				break;
			}
			return Math.Abs(Convert.ToInt32(argsList[0]));
		case StiFunctionType.Acos:
		{
			int num2 = get_category(argsList[0]);
			if (num2 <= 1 || num2 >= 8)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Acos", "1", GetTypeName(argsList[0]), "double");
			}
			return Math.Acos(Convert.ToDouble(argsList[0]));
		}
		case StiFunctionType.Asin:
		{
			int num2 = get_category(argsList[0]);
			if (num2 <= 1 || num2 >= 8)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Asin", "1", GetTypeName(argsList[0]), "double");
			}
			return Math.Asin(Convert.ToDouble(argsList[0]));
		}
		case StiFunctionType.Atan:
		{
			int num2 = get_category(argsList[0]);
			if (num2 <= 1 || num2 >= 8)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Atan", "1", GetTypeName(argsList[0]), "double");
			}
			return Math.Atan(Convert.ToDouble(argsList[0]));
		}
		case StiFunctionType.Ceiling:
			switch (get_category(argsList[0]))
			{
			default:
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Ceiling", "1", GetTypeName(argsList[0]), "double");
				break;
			case 2:
				return Math.Ceiling(Convert.ToDecimal(argsList[0]));
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
				break;
			}
			return Math.Ceiling(Convert.ToDouble(argsList[0]));
		case StiFunctionType.Cos:
		{
			int num2 = get_category(argsList[0]);
			if (num2 <= 1 || num2 >= 8)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Cos", "1", GetTypeName(argsList[0]), "double");
			}
			return Math.Cos(Convert.ToDouble(argsList[0]));
		}
		case StiFunctionType.Div:
		{
			int num2 = get_category(argsList[0]);
			int num3 = get_category(argsList[1]);
			if (num2 <= 1 || num2 >= 8)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Div", "1", GetTypeName(argsList[0]), "double");
			}
			else if (num3 <= 1 || num3 >= 8)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Div", "2", GetTypeName(argsList[1]), "double");
			}
			else
			{
				if (argsList.Count == 3)
				{
					if (num2 == 2)
					{
						if (Convert.ToDecimal(argsList[1]) == 0m)
						{
							return Convert.ToDecimal(argsList[2]);
						}
						return Convert.ToDecimal(argsList[0]) / Convert.ToDecimal(argsList[1]);
					}
					if (Convert.ToDouble(argsList[1]) == 0.0)
					{
						return Convert.ToDouble(argsList[2]);
					}
					return Convert.ToDouble(argsList[0]) / Convert.ToDouble(argsList[1]);
				}
				if (argsList.Count == 2)
				{
					if (num2 == 2)
					{
						return Convert.ToDecimal(argsList[0]) / Convert.ToDecimal(argsList[1]);
					}
					return Convert.ToDouble(argsList[0]) / Convert.ToDouble(argsList[1]);
				}
			}
			ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "Div", argsList.Count.ToString());
			break;
		}
		case StiFunctionType.Exp:
		{
			int num2 = get_category(argsList[0]);
			if (num2 <= 1 || num2 >= 8)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Exp", "1", GetTypeName(argsList[0]), "double");
			}
			return Math.Exp(Convert.ToDouble(argsList[0]));
		}
		case StiFunctionType.Floor:
		{
			int num2 = get_category(argsList[0]);
			if (num2 <= 1 || num2 >= 4)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Floor", "1", GetTypeName(argsList[0]), "double");
			}
			if (num2 == 2)
			{
				return Math.Floor(Convert.ToDecimal(argsList[0]));
			}
			return Math.Floor(Convert.ToDouble(argsList[0]));
		}
		case StiFunctionType.Log:
		{
			int num2 = get_category(argsList[0]);
			if (num2 <= 1 || num2 >= 8)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Log", "1", GetTypeName(argsList[0]), "double");
			}
			return Math.Log(Convert.ToDouble(argsList[0]));
		}
		case StiFunctionType.Maximum:
		{
			if (argsList.Count != 2)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "Maximum", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			int num3 = get_category(argsList[1]);
			if (num2 <= 1 || num2 >= 8)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Maximum", "1", GetTypeName(argsList[0]), "double");
			}
			else if (num3 <= 1 || num3 >= 8)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Maximum", "2", GetTypeName(argsList[1]), "double");
			}
			else
			{
				switch (num2)
				{
				case 2:
					return Math.Max(Convert.ToDecimal(argsList[0]), Convert.ToDecimal(argsList[1]));
				case 3:
					return Math.Max(Convert.ToDouble(argsList[0]), Convert.ToDouble(argsList[1]));
				}
			}
			return Math.Max(Convert.ToInt64(argsList[0]), Convert.ToInt64(argsList[1]));
		}
		case StiFunctionType.Minimum:
		{
			if (argsList.Count != 2)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "Minimum", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			int num3 = get_category(argsList[1]);
			if (num2 <= 1 || num2 >= 8)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Minimum", "1", GetTypeName(argsList[0]), "double");
			}
			else if (num3 <= 1 || num3 >= 8)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Minimum", "2", GetTypeName(argsList[1]), "double");
			}
			else
			{
				switch (num2)
				{
				case 2:
					return Math.Min(Convert.ToDecimal(argsList[0]), Convert.ToDecimal(argsList[1]));
				case 3:
					return Math.Min(Convert.ToDouble(argsList[0]), Convert.ToDouble(argsList[1]));
				}
			}
			return Math.Min(Convert.ToInt64(argsList[0]), Convert.ToInt64(argsList[1]));
		}
		case StiFunctionType.Round:
			switch (num)
			{
			case 1:
				return Math.Round(Convert.ToDouble(argsList[0]));
			case 2:
				return Math.Round(Convert.ToDouble(argsList[0]), Convert.ToInt32(argsList[1]));
			case 3:
				return Math.Round(Convert.ToDecimal(argsList[0]));
			case 4:
				return Math.Round(Convert.ToDecimal(argsList[0]), Convert.ToInt32(argsList[1]));
			case 5:
				return Math.Round(Convert.ToDouble(argsList[0]), (MidpointRounding)argsList[1]);
			case 6:
				return Math.Round(Convert.ToDouble(argsList[0]), Convert.ToInt32(argsList[1]), (MidpointRounding)argsList[2]);
			case 7:
				return Math.Round(Convert.ToDecimal(argsList[0]), (MidpointRounding)argsList[1]);
			case 8:
				return Math.Round(Convert.ToDecimal(argsList[0]), Convert.ToInt32(argsList[1]), (MidpointRounding)argsList[2]);
			}
			break;
		case StiFunctionType.Sign:
			switch (get_category(argsList[0]))
			{
			default:
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Sign", "1", GetTypeName(argsList[0]), "double");
				break;
			case 2:
				return Math.Sign(Convert.ToDecimal(argsList[0]));
			case 3:
				return Math.Sign(Convert.ToDouble(argsList[0]));
			case 4:
			case 5:
			case 6:
			case 7:
				break;
			}
			return Math.Sign(Convert.ToInt32(argsList[0]));
		case StiFunctionType.Sin:
		{
			int num2 = get_category(argsList[0]);
			if (num2 <= 1 || num2 >= 8)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Sin", "1", GetTypeName(argsList[0]), "double");
			}
			return Math.Sin(Convert.ToDouble(argsList[0]));
		}
		case StiFunctionType.Sqrt:
		{
			int num2 = get_category(argsList[0]);
			if (num2 <= 1 || num2 >= 8)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Sqrt", "1", GetTypeName(argsList[0]), "double");
			}
			return Math.Sqrt(Convert.ToDouble(argsList[0]));
		}
		case StiFunctionType.Tan:
		{
			int num2 = get_category(argsList[0]);
			if (num2 <= 1 || num2 >= 8)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Tan", "1", GetTypeName(argsList[0]), "double");
			}
			return Math.Tan(Convert.ToDouble(argsList[0]));
		}
		case StiFunctionType.Truncate:
			switch (get_category(argsList[0]))
			{
			default:
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Truncate", "1", GetTypeName(argsList[0]), "double");
				break;
			case 2:
				return Math.Truncate(Convert.ToDecimal(argsList[0]));
			case 3:
				break;
			}
			return Math.Truncate(Convert.ToDouble(argsList[0]));
		case StiFunctionType.DateDiff:
			switch (num)
			{
			case 1:
				return Convert.ToDateTime(argsList[0]).Subtract(Convert.ToDateTime(argsList[1]));
			case 2:
				return Convert.ToDateTime(argsList[0]).Subtract(Convert.ToDateTime(argsList[1]));
			}
			break;
		case StiFunctionType.DateSerial:
			switch (num)
			{
			case 1:
				return new DateTime(Convert.ToInt64(argsList[0]));
			case 2:
				return new DateTime(Convert.ToInt32(argsList[0]), Convert.ToInt32(argsList[1]), Convert.ToInt32(argsList[2]));
			}
			break;
		case StiFunctionType.TimeSerial:
			if (num == 1)
			{
				return new TimeSpan(Convert.ToInt32(argsList[0]), Convert.ToInt32(argsList[1]), Convert.ToInt32(argsList[2]));
			}
			break;
		case StiFunctionType.Year:
			if (num == 1 || num == 2)
			{
				return Convert.ToDateTime(argsList[0]).Year;
			}
			break;
		case StiFunctionType.Month:
			if (num == 1 || num == 2)
			{
				return Convert.ToDateTime(argsList[0]).Month;
			}
			break;
		case StiFunctionType.Day:
			if (num == 1 || num == 2)
			{
				return Convert.ToDateTime(argsList[0]).Day;
			}
			break;
		case StiFunctionType.Hour:
			if (num == 1 || num == 2)
			{
				return Convert.ToDateTime(argsList[0]).Hour;
			}
			break;
		case StiFunctionType.Minute:
			if (num == 1 || num == 2)
			{
				return Convert.ToDateTime(argsList[0]).Minute;
			}
			break;
		case StiFunctionType.Second:
			if (num == 1 || num == 2)
			{
				return Convert.ToDateTime(argsList[0]).Second;
			}
			break;
		case StiFunctionType.DayOfWeek:
			switch (num)
			{
			case 1:
				return StiFunctionsDate.DayOfWeek(Convert.ToDateTime(argsList[0]));
			case 3:
				return StiFunctionsDate.DayOfWeek(Convert.ToDateTime(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 5:
				return StiFunctionsDate.DayOfWeek(Convert.ToDateTime(argsList[0]), Convert.ToString(argsList[1]));
			case 7:
				return StiFunctionsDate.DayOfWeek(Convert.ToDateTime(argsList[0]), Convert.ToString(argsList[1]), Convert.ToBoolean(argsList[2]));
			case 2:
				return StiFunctionsDate.DayOfWeek((DateTime?)argsList[0]);
			case 4:
				return StiFunctionsDate.DayOfWeek((DateTime?)argsList[0], Convert.ToBoolean(argsList[1]));
			case 6:
				return StiFunctionsDate.DayOfWeek((DateTime?)argsList[0], Convert.ToString(argsList[1]));
			case 8:
				return StiFunctionsDate.DayOfWeek((DateTime?)argsList[0], Convert.ToString(argsList[1]), Convert.ToBoolean(argsList[2]));
			}
			break;
		case StiFunctionType.DayOfYear:
			if (num == 1 || num == 2)
			{
				return Convert.ToDateTime(argsList[0]).DayOfYear.ToString();
			}
			break;
		case StiFunctionType.DaysInMonth:
			switch (num)
			{
			case 1:
				return (long)DateTime.DaysInMonth(Convert.ToDateTime(argsList[0]).Year, Convert.ToDateTime(argsList[0]).Month);
			case 3:
				return (long)DateTime.DaysInMonth(Convert.ToInt32(argsList[0]), Convert.ToInt32(argsList[1]));
			case 2:
				return (long)DateTime.DaysInMonth(((DateTime?)argsList[0]).Value.Year, Convert.ToDateTime(argsList[0]).Month);
			}
			break;
		case StiFunctionType.DaysInYear:
			switch (num)
			{
			case 1:
				return (long)(DateTime.IsLeapYear(Convert.ToDateTime(argsList[0]).Year) ? 366 : 365);
			case 3:
				return (long)(DateTime.IsLeapYear(Convert.ToInt32(argsList[0])) ? 366 : 365);
			case 2:
				return (long)(DateTime.IsLeapYear(((DateTime?)argsList[0]).Value.Year) ? 366 : 365);
			}
			break;
		case StiFunctionType.MonthName:
			switch (num)
			{
			case 1:
				return StiFunctionsDate.MonthName(Convert.ToDateTime(argsList[0]));
			case 3:
				return StiFunctionsDate.MonthName(Convert.ToDateTime(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 5:
				return StiFunctionsDate.MonthName(Convert.ToDateTime(argsList[0]), Convert.ToString(argsList[1]));
			case 7:
				return StiFunctionsDate.MonthName(Convert.ToDateTime(argsList[0]), Convert.ToString(argsList[1]), Convert.ToBoolean(argsList[2]));
			case 2:
				return StiFunctionsDate.MonthName((DateTime?)argsList[0]);
			case 4:
				return StiFunctionsDate.MonthName((DateTime?)argsList[0], Convert.ToBoolean(argsList[1]));
			case 6:
				return StiFunctionsDate.MonthName((DateTime?)argsList[0], Convert.ToString(argsList[1]));
			case 8:
				return StiFunctionsDate.MonthName((DateTime?)argsList[0], Convert.ToString(argsList[1]), Convert.ToBoolean(argsList[2]));
			}
			break;
		case StiFunctionType.WeekOfYear:
			switch (num)
			{
			case 1:
				return StiFunctionsDate.WeekOfYear(Convert.ToDateTime(argsList[0]));
			case 3:
				return StiFunctionsDate.WeekOfYear(Convert.ToDateTime(argsList[0]), (DayOfWeek)argsList[1]);
			case 5:
				return StiFunctionsDate.WeekOfYear(Convert.ToDateTime(argsList[0]), (DayOfWeek)argsList[1], (CalendarWeekRule)argsList[2]);
			case 2:
				return StiFunctionsDate.WeekOfYear((DateTime?)argsList[0]);
			case 4:
				return StiFunctionsDate.WeekOfYear((DateTime?)argsList[0], (DayOfWeek)argsList[1]);
			case 6:
				return StiFunctionsDate.WeekOfYear((DateTime?)argsList[0], (DayOfWeek)argsList[1], (CalendarWeekRule)argsList[2]);
			}
			break;
		case StiFunctionType.WeekOfMonth:
			switch (num)
			{
			case 1:
				return StiFunctionsDate.WeekOfMonth(Convert.ToDateTime(argsList[0]));
			case 3:
				return StiFunctionsDate.WeekOfMonth(Convert.ToDateTime(argsList[0]), (DayOfWeek)argsList[1]);
			case 5:
				return StiFunctionsDate.WeekOfMonth(Convert.ToDateTime(argsList[0]), (DayOfWeek)argsList[1], (CalendarWeekRule)argsList[2]);
			case 2:
				return StiFunctionsDate.WeekOfMonth((DateTime?)argsList[0]);
			case 4:
				return StiFunctionsDate.WeekOfMonth((DateTime?)argsList[0], (DayOfWeek)argsList[1]);
			case 6:
				return StiFunctionsDate.WeekOfMonth((DateTime?)argsList[0], (DayOfWeek)argsList[1], (CalendarWeekRule)argsList[2]);
			}
			break;
		case StiFunctionType.Insert:
		{
			if (argsList.Count != 3)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "Insert", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			if (num2 != 1)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Insert", "1", GetTypeName(argsList[0]), "string");
			}
			num2 = get_category(argsList[1]);
			if (num2 < 4 || num2 > 7)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Insert", "2", GetTypeName(argsList[0]), "int");
			}
			num2 = get_category(argsList[2]);
			if (num2 != 1)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Insert", "3", GetTypeName(argsList[0]), "string");
			}
			return Convert.ToString(argsList[0]).Insert(Convert.ToInt32(argsList[1]), Convert.ToString(argsList[2]));
		}
		case StiFunctionType.Length:
		{
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "Length", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			if (num2 != 1)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Length", "1", GetTypeName(argsList[0]), "string");
			}
			return Convert.ToString(argsList[0]).Length;
		}
		case StiFunctionType.Remove:
		{
			if (argsList.Count != 3)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "Remove", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			if (num2 != 1)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Remove", "1", GetTypeName(argsList[0]), "string");
			}
			num2 = get_category(argsList[1]);
			if (num2 < 4 || num2 > 7)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Remove", "2", GetTypeName(argsList[0]), "int");
			}
			num2 = get_category(argsList[2]);
			if (num2 < 4 || num2 > 7)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Remove", "3", GetTypeName(argsList[0]), "int");
			}
			return Convert.ToString(argsList[0]).Remove(Convert.ToInt32(argsList[1]), Convert.ToInt32(argsList[2]));
		}
		case StiFunctionType.Replace:
		{
			if (argsList.Count != 3)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "Replace", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			if (num2 != 1)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Replace", "1", GetTypeName(argsList[0]), "string");
			}
			num2 = get_category(argsList[1]);
			if (num2 != 1)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Replace", "2", GetTypeName(argsList[0]), "string");
			}
			num2 = get_category(argsList[2]);
			if (num2 != 1)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Replace", "3", GetTypeName(argsList[0]), "string");
			}
			return Convert.ToString(argsList[0]).Replace(Convert.ToString(argsList[1]), Convert.ToString(argsList[2]));
		}
		case StiFunctionType.Roman:
		{
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "Roman", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			if (num2 >= 4 && num2 <= 7)
			{
				return StiFunctionsStrings.Roman(Convert.ToInt32(argsList[0]));
			}
			ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Roman", "1", GetTypeName(argsList[0]), "int");
			break;
		}
		case StiFunctionType.Substring:
		{
			if (argsList.Count != 3)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "Substring", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			if (num2 != 1)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Substring", "1", GetTypeName(argsList[0]), "string");
			}
			num2 = get_category(argsList[1]);
			if (num2 < 4 || num2 > 7)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Substring", "2", GetTypeName(argsList[0]), "int");
			}
			num2 = get_category(argsList[2]);
			if (num2 < 4 || num2 > 7)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Substring", "3", GetTypeName(argsList[0]), "int");
			}
			return StiFunctionsStrings.Substring(Convert.ToString(argsList[0]), Convert.ToInt32(argsList[1]), Convert.ToInt32(argsList[2]));
		}
		case StiFunctionType.ToLowerCase:
		{
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "ToLowerCase", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			if (num2 != 1)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "ToLowerCase", "1", GetTypeName(argsList[0]), "string");
			}
			return Convert.ToString(argsList[0]).ToLowerInvariant();
		}
		case StiFunctionType.ToProperCase:
		{
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "ToProperCase", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			if (num2 != 1)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "ToProperCase", "1", GetTypeName(argsList[0]), "string");
			}
			return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Convert.ToString(argsList[0]).ToLowerInvariant());
		}
		case StiFunctionType.ToUpperCase:
		{
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "ToUpperCase", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			if (num2 != 1)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "ToUpperCase", "1", GetTypeName(argsList[0]), "string");
			}
			return Convert.ToString(argsList[0]).ToUpperInvariant();
		}
		case StiFunctionType.Trim:
		{
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "Trim", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			if (num2 != 1)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Trim", "1", GetTypeName(argsList[0]), "string");
			}
			return Convert.ToString(argsList[0]).Trim();
		}
		case StiFunctionType.TryParseDecimal:
		{
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "TryParseDecimal", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			if (num2 != 1)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "TryParseDecimal", "1", GetTypeName(argsList[0]), "string");
			}
			decimal result3 = default(decimal);
			return decimal.TryParse(Convert.ToString(argsList[0]), out result3);
		}
		case StiFunctionType.TryParseDouble:
		{
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "TryParseDouble", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			if (num2 != 1)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "TryParseDouble", "1", GetTypeName(argsList[0]), "string");
			}
			double result2 = 0.0;
			return double.TryParse(Convert.ToString(argsList[0]), out result2);
		}
		case StiFunctionType.TryParseLong:
		{
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "TryParseLong", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			if (num2 != 1)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "TryParseLong", "1", GetTypeName(argsList[0]), "string");
			}
			long result = 0L;
			return long.TryParse(Convert.ToString(argsList[0]), out result);
		}
		case StiFunctionType.Arabic:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "Arabic", argsList.Count.ToString());
			}
			switch (get_category(argsList[0]))
			{
			case 1:
				return StiFunctionsStrings.Arabic(Convert.ToString(argsList[0]));
			case 4:
			case 5:
			case 6:
			case 7:
				return StiFunctionsStrings.Arabic(Convert.ToInt32(argsList[0]));
			}
			ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Arabic", "1", GetTypeName(argsList[0]), "string");
			break;
		case StiFunctionType.Persian:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "Persian", argsList.Count.ToString());
			}
			switch (get_category(argsList[0]))
			{
			case 1:
				return StiFunctionsStrings.Persian(Convert.ToString(argsList[0]));
			case 4:
			case 5:
			case 6:
			case 7:
				return StiFunctionsStrings.Persian(Convert.ToInt32(argsList[0]));
			}
			ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Persian", "1", GetTypeName(argsList[0]), "string");
			break;
		case StiFunctionType.ToOrdinal:
		{
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "ToOrdinal", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			if (num2 >= 4 && num2 <= 7)
			{
				return StiFunctionsStrings.ToOrdinal(Convert.ToInt32(argsList[0]));
			}
			ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "ToOrdinal", "1", GetTypeName(argsList[0]), "int");
			break;
		}
		case StiFunctionType.Left:
			if (num == 1)
			{
				return StiFunctionsStrings.Left(Convert.ToString(argsList[0]), Convert.ToInt32(argsList[1]));
			}
			break;
		case StiFunctionType.Right:
			if (num == 1)
			{
				return StiFunctionsStrings.Right(Convert.ToString(argsList[0]), Convert.ToInt32(argsList[1]));
			}
			break;
		case StiFunctionType.Mid:
			if (num == 1)
			{
				return StiFunctionsStrings.Mid(Convert.ToString(argsList[0]), Convert.ToInt32(argsList[1]), Convert.ToInt32(argsList[2]));
			}
			break;
		case StiFunctionType.ToWords:
			switch (num)
			{
			case 1:
				return StiFunctionsStrings.ToWords(Convert.ToInt64(argsList[0]));
			case 2:
				return StiFunctionsStrings.ToWords(Convert.ToDecimal(argsList[0]));
			case 3:
				return StiFunctionsStrings.ToWords(Convert.ToDouble(argsList[0]));
			case 4:
				return StiFunctionsStrings.ToWords(Convert.ToInt64(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 5:
				return StiFunctionsStrings.ToWords(Convert.ToDecimal(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 6:
				return StiFunctionsStrings.ToWords(Convert.ToDouble(argsList[0]), Convert.ToBoolean(argsList[1]));
			}
			break;
		case StiFunctionType.ToWordsEs:
			switch (num)
			{
			case 1:
				return StiFunctionsStrings.ToWordsEs(Convert.ToInt64(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 2:
				return StiFunctionsStrings.ToWordsEs(Convert.ToInt64(argsList[0]), Convert.ToBoolean(argsList[1]), Convert.ToBoolean(argsList[2]));
			}
			break;
		case StiFunctionType.ToWordsEnIn:
			return StiFunctionsStrings.ToWordsEnIn(Convert.ToInt64(argsList[0]), Convert.ToBoolean(argsList[1]));
		case StiFunctionType.ToWordsFa:
			return StiFunctionsStrings.ToWordsFa(Convert.ToInt64(argsList[0]));
		case StiFunctionType.ToWordsPl:
			return StiFunctionsStrings.ToWordsPl(Convert.ToInt64(argsList[0]), Convert.ToBoolean(argsList[1]));
		case StiFunctionType.ToWordsPt:
			return StiFunctionsStrings.ToWordsPt(Convert.ToInt64(argsList[0]), Convert.ToBoolean(argsList[1]));
		case StiFunctionType.ToWordsRu:
			switch (num)
			{
			case 1:
				return StiFunctionsStrings.ToWordsRu(Convert.ToInt64(argsList[0]));
			case 2:
				return StiFunctionsStrings.ToWordsRu(Convert.ToDecimal(argsList[0]));
			case 3:
				return StiFunctionsStrings.ToWordsRu(Convert.ToDouble(argsList[0]));
			case 4:
				return StiFunctionsStrings.ToWordsRu(Convert.ToInt64(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 5:
				return StiFunctionsStrings.ToWordsRu(Convert.ToDecimal(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 6:
				return StiFunctionsStrings.ToWordsRu(Convert.ToDouble(argsList[0]), Convert.ToBoolean(argsList[1]));
			}
			break;
		case StiFunctionType.ToWordsTr:
			return StiFunctionsStrings.ToWordsTr(Convert.ToDecimal(argsList[0]));
		case StiFunctionType.ToWordsUa:
			switch (num)
			{
			case 1:
				return StiFunctionsStrings.ToWordsUa(Convert.ToInt64(argsList[0]));
			case 2:
				return StiFunctionsStrings.ToWordsUa(Convert.ToDecimal(argsList[0]));
			case 3:
				return StiFunctionsStrings.ToWordsUa(Convert.ToDouble(argsList[0]));
			case 4:
				return StiFunctionsStrings.ToWordsUa(Convert.ToInt64(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 5:
				return StiFunctionsStrings.ToWordsUa(Convert.ToDecimal(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 6:
				return StiFunctionsStrings.ToWordsUa(Convert.ToDouble(argsList[0]), Convert.ToBoolean(argsList[1]));
			}
			break;
		case StiFunctionType.ToCurrencyWords:
			switch (num)
			{
			case 1:
				return StiFunctionsStrings.ToCurrencyWords(Convert.ToInt64(argsList[0]));
			case 2:
				return StiFunctionsStrings.ToCurrencyWords(Convert.ToDouble(argsList[0]));
			case 3:
				return StiFunctionsStrings.ToCurrencyWords(Convert.ToDecimal(argsList[0]));
			case 4:
				return StiFunctionsStrings.ToCurrencyWords(Convert.ToInt64(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 5:
				return StiFunctionsStrings.ToCurrencyWords(Convert.ToDouble(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 6:
				return StiFunctionsStrings.ToCurrencyWords(Convert.ToDecimal(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 7:
				return StiFunctionsStrings.ToCurrencyWords(Convert.ToInt64(argsList[0]), Convert.ToBoolean(argsList[1]), Convert.ToBoolean(argsList[2]));
			case 8:
				return StiFunctionsStrings.ToCurrencyWords(Convert.ToDouble(argsList[0]), Convert.ToBoolean(argsList[1]), Convert.ToBoolean(argsList[2]));
			case 9:
				return StiFunctionsStrings.ToCurrencyWords(Convert.ToDecimal(argsList[0]), Convert.ToBoolean(argsList[1]), Convert.ToBoolean(argsList[2]));
			case 10:
				return StiFunctionsStrings.ToCurrencyWords(Convert.ToDouble(argsList[0]), Convert.ToBoolean(argsList[1]), Convert.ToBoolean(argsList[2]), Convert.ToString(argsList[3]), Convert.ToString(argsList[4]));
			}
			break;
		case StiFunctionType.ToCurrencyWordsEnGb:
			return StiFunctionsStrings.ToCurrencyWordsEnGb(Convert.ToDecimal(argsList[0]), Convert.ToString(argsList[1]), Convert.ToInt32(argsList[2]));
		case StiFunctionType.ToCurrencyWordsEnIn:
			return StiFunctionsStrings.ToCurrencyWordsEnIn(Convert.ToString(argsList[0]), Convert.ToString(argsList[1]), Convert.ToDecimal(argsList[2]), Convert.ToInt32(argsList[3]), Convert.ToBoolean(argsList[4]));
		case StiFunctionType.ToCurrencyWordsEs:
			return StiFunctionsStrings.ToCurrencyWordsEs(Convert.ToDecimal(argsList[0]), Convert.ToString(argsList[1]), Convert.ToInt32(argsList[2]));
		case StiFunctionType.ToCurrencyWordsFr:
			return StiFunctionsStrings.ToCurrencyWordsFr(Convert.ToDecimal(argsList[0]), Convert.ToString(argsList[1]), Convert.ToInt32(argsList[2]));
		case StiFunctionType.ToCurrencyWordsNl:
			return StiFunctionsStrings.ToCurrencyWordsNl(Convert.ToDecimal(argsList[0]), Convert.ToString(argsList[1]), Convert.ToInt32(argsList[2]));
		case StiFunctionType.ToCurrencyWordsPl:
			return StiFunctionsStrings.ToCurrencyWordsPl(Convert.ToDecimal(argsList[0]), Convert.ToString(argsList[1]), Convert.ToBoolean(argsList[2]), Convert.ToBoolean(argsList[3]));
		case StiFunctionType.ToCurrencyWordsPt:
			switch (num)
			{
			case 1:
				return StiFunctionsStrings.ToCurrencyWordsPt(Convert.ToDecimal(argsList[0]), Convert.ToBoolean(argsList[1]), Convert.ToBoolean(argsList[2]));
			case 2:
				return StiFunctionsStrings.ToCurrencyWordsPt(Convert.ToDecimal(argsList[0]), Convert.ToBoolean(argsList[1]), Convert.ToBoolean(argsList[2]), Convert.ToString(argsList[3]), Convert.ToString(argsList[4]));
			}
			break;
		case StiFunctionType.ToCurrencyWordsPtBr:
			switch (num)
			{
			case 1:
				return StiFunctionsStrings.ToCurrencyWordsPtBr(Convert.ToDecimal(argsList[0]));
			case 2:
				return StiFunctionsStrings.ToCurrencyWordsPtBr(Convert.ToDecimal(argsList[0]), Convert.ToBoolean(argsList[1]), Convert.ToString(argsList[2]), Convert.ToString(argsList[3]));
			}
			break;
		case StiFunctionType.ToCurrencyWordsRu:
			switch (num)
			{
			case 1:
				return StiFunctionsStrings.ToCurrencyWordsRu(Convert.ToInt64(argsList[0]));
			case 2:
				return StiFunctionsStrings.ToCurrencyWordsRu(Convert.ToDouble(argsList[0]));
			case 3:
				return StiFunctionsStrings.ToCurrencyWordsRu(Convert.ToDecimal(argsList[0]));
			case 4:
				return StiFunctionsStrings.ToCurrencyWordsRu(Convert.ToInt64(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 5:
				return StiFunctionsStrings.ToCurrencyWordsRu(Convert.ToDouble(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 6:
				return StiFunctionsStrings.ToCurrencyWordsRu(Convert.ToDecimal(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 7:
				return StiFunctionsStrings.ToCurrencyWordsRu(Convert.ToInt64(argsList[0]), Convert.ToBoolean(argsList[1]), Convert.ToString(argsList[2]));
			case 8:
				return StiFunctionsStrings.ToCurrencyWordsRu(Convert.ToDouble(argsList[0]), Convert.ToBoolean(argsList[1]), Convert.ToString(argsList[2]));
			case 9:
				return StiFunctionsStrings.ToCurrencyWordsRu(Convert.ToDecimal(argsList[0]), Convert.ToBoolean(argsList[1]), Convert.ToString(argsList[2]));
			case 10:
				return StiFunctionsStrings.ToCurrencyWordsRu(Convert.ToInt64(argsList[0]), Convert.ToString(argsList[1]), Convert.ToBoolean(argsList[2]));
			case 11:
				return StiFunctionsStrings.ToCurrencyWordsRu(Convert.ToDouble(argsList[0]), Convert.ToString(argsList[1]), Convert.ToBoolean(argsList[2]));
			case 12:
				return StiFunctionsStrings.ToCurrencyWordsRu(Convert.ToDecimal(argsList[0]), Convert.ToString(argsList[1]), Convert.ToBoolean(argsList[2]));
			}
			break;
		case StiFunctionType.ToCurrencyWordsThai:
			switch (num)
			{
			case 1:
				return StiFunctionsStrings.ToCurrencyWordsThai(Convert.ToInt64(argsList[0]));
			case 2:
				return StiFunctionsStrings.ToCurrencyWordsThai(Convert.ToDouble(argsList[0]));
			case 3:
				return StiFunctionsStrings.ToCurrencyWordsThai(Convert.ToDecimal(argsList[0]));
			}
			break;
		case StiFunctionType.ToCurrencyWordsTr:
			switch (num)
			{
			case 1:
				return StiFunctionsStrings.ToCurrencyWordsTr(Convert.ToDecimal(argsList[0]));
			case 2:
				return StiFunctionsStrings.ToCurrencyWordsTr(Convert.ToDecimal(argsList[0]), Convert.ToString(argsList[1]), Convert.ToBoolean(argsList[2]));
			}
			break;
		case StiFunctionType.ToCurrencyWordsUa:
			switch (num)
			{
			case 1:
				return StiFunctionsStrings.ToCurrencyWordsUa(Convert.ToInt64(argsList[0]));
			case 2:
				return StiFunctionsStrings.ToCurrencyWordsUa(Convert.ToDouble(argsList[0]));
			case 3:
				return StiFunctionsStrings.ToCurrencyWordsUa(Convert.ToDecimal(argsList[0]));
			case 4:
				return StiFunctionsStrings.ToCurrencyWordsUa(Convert.ToInt64(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 5:
				return StiFunctionsStrings.ToCurrencyWordsUa(Convert.ToDouble(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 6:
				return StiFunctionsStrings.ToCurrencyWordsUa(Convert.ToDecimal(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 7:
				return StiFunctionsStrings.ToCurrencyWordsUa(Convert.ToInt64(argsList[0]), Convert.ToBoolean(argsList[1]), Convert.ToString(argsList[2]));
			case 8:
				return StiFunctionsStrings.ToCurrencyWordsUa(Convert.ToDouble(argsList[0]), Convert.ToBoolean(argsList[1]), Convert.ToString(argsList[2]));
			case 9:
				return StiFunctionsStrings.ToCurrencyWordsUa(Convert.ToDecimal(argsList[0]), Convert.ToBoolean(argsList[1]), Convert.ToString(argsList[2]));
			}
			break;
		case StiFunctionType.DateToStr:
			switch (num)
			{
			case 1:
				return StiFunctionsStrings.DateToStr(Convert.ToDateTime(argsList[0]));
			case 3:
				return StiFunctionsStrings.DateToStr(Convert.ToDateTime(argsList[0]), Convert.ToBoolean(argsList[1]));
			case 2:
				return StiFunctionsStrings.DateToStr((DateTime?)argsList[0]);
			case 4:
				return StiFunctionsStrings.DateToStr((DateTime?)argsList[0], Convert.ToBoolean(argsList[1]));
			}
			break;
		case StiFunctionType.DateToStrPl:
			return StiFunctionsStrings.DateToStrPl(Convert.ToDateTime(argsList[0]), Convert.ToBoolean(argsList[1]));
		case StiFunctionType.DateToStrRu:
			switch (num)
			{
			case 1:
				return StiFunctionsStrings.DateToStrRu(Convert.ToDateTime(argsList[0]));
			case 2:
				return StiFunctionsStrings.DateToStrRu(Convert.ToDateTime(argsList[0]), Convert.ToBoolean(argsList[1]));
			}
			break;
		case StiFunctionType.DateToStrUa:
			switch (num)
			{
			case 1:
				return StiFunctionsStrings.DateToStrUa(Convert.ToDateTime(argsList[0]));
			case 2:
				return StiFunctionsStrings.DateToStrUa(Convert.ToDateTime(argsList[0]), Convert.ToBoolean(argsList[1]));
			}
			break;
		case StiFunctionType.DateToStrPt:
			return StiFunctionsStrings.DateToStrPt(Convert.ToDateTime(argsList[0]));
		case StiFunctionType.DateToStrPtBr:
			return StiFunctionsStrings.DateToStrPtBr(Convert.ToDateTime(argsList[0]));
		case StiFunctionType.StringIsNullOrEmpty:
			if (num == 1)
			{
				return string.IsNullOrEmpty(Convert.ToString(argsList[0]));
			}
			break;
		case StiFunctionType.StringIsNullOrWhiteSpace:
			if (num == 1)
			{
				return string.IsNullOrWhiteSpace(Convert.ToString(argsList[0]));
			}
			break;
		case StiFunctionType.StrToDateTime:
			if (num == 1)
			{
				return StiFunctionsStrings.StrToDateTime(Convert.ToString(argsList[0]));
			}
			break;
		case StiFunctionType.StrToNullableDateTime:
			if (num == 1)
			{
				return StiFunctionsStrings.StrToNullableDateTime(Convert.ToString(argsList[0]));
			}
			break;
		case StiFunctionType.ConvertToBase64String:
			if (num == 1)
			{
				return StiFunctionsStrings.ConvertToBase64String(Convert.ToString(argsList[0]));
			}
			break;
		case StiFunctionType.IsNull:
			if (num == 1)
			{
				return StiFunctionsPrintState.IsNull(argsList[0], Convert.ToString(argsList[1]));
			}
			break;
		case StiFunctionType.Next:
			if (num == 1)
			{
				return StiFunctionsPrintState.Next(argsList[0], Convert.ToString(argsList[1]));
			}
			break;
		case StiFunctionType.NextIsNull:
			if (num == 1)
			{
				return StiFunctionsPrintState.NextIsNull(argsList[0], Convert.ToString(argsList[1]));
			}
			break;
		case StiFunctionType.Previous:
			if (num == 1)
			{
				return StiFunctionsPrintState.Previous(argsList[0], Convert.ToString(argsList[1]));
			}
			break;
		case StiFunctionType.PreviousIsNull:
			if (num == 1)
			{
				return StiFunctionsPrintState.PreviousIsNull(argsList[0], Convert.ToString(argsList[1]));
			}
			break;
		case StiFunctionType.IIF:
			switch (num)
			{
			case 1:
				return Convert.ToInt32(Convert.ToBoolean(argsList[0]) ? argsList[1] : argsList[2]);
			case 2:
				return Convert.ToInt64(Convert.ToBoolean(argsList[0]) ? argsList[1] : argsList[2]);
			case 3:
				return Convert.ToDouble(Convert.ToBoolean(argsList[0]) ? argsList[1] : argsList[2]);
			case 4:
				return Convert.ToDecimal(Convert.ToBoolean(argsList[0]) ? argsList[1] : argsList[2]);
			case 5:
				return Convert.ToChar(Convert.ToBoolean(argsList[0]) ? argsList[1] : argsList[2]);
			case 6:
				return Convert.ToString(Convert.ToBoolean(argsList[0]) ? argsList[1] : argsList[2]);
			case 7:
				return Convert.ToDateTime(Convert.ToBoolean(argsList[0]) ? argsList[1] : argsList[2]);
			case 8:
				if (!Convert.ToBoolean(argsList[0]))
				{
					return argsList[2];
				}
				return argsList[1];
			}
			break;
		case StiFunctionType.Choose:
		{
			if (argsList.Count < 2)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "Choose", argsList.Count.ToString());
			}
			int num2 = get_category(argsList[0]);
			if (num2 < 4 || num2 > 7)
			{
				ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Choose", "1", GetTypeName(argsList[0]), "int");
			}
			int num7 = Convert.ToInt32(argsList[0]);
			if (num7 > 0 && num7 < argsList.Count)
			{
				return argsList[num7];
			}
			return null;
		}
		case StiFunctionType.Switch:
		{
			if (argsList.Count < 2)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "Switch", argsList.Count.ToString());
			}
			for (int i = 0; i + 1 < argsList.Count; i += 2)
			{
				if (Convert.ToBoolean(argsList[i]))
				{
					return argsList[i + 1];
				}
			}
			return null;
		}
		case StiFunctionType.ToString:
		{
			if (argsList[0] == null || argsList[0] == DBNull.Value)
			{
				return string.Empty;
			}
			int num2 = get_category(argsList[0]);
			switch (num2)
			{
			case 1:
				return Convert.ToString(argsList[0]);
			case 2:
			case 3:
			{
				decimal num5 = Convert.ToDecimal(argsList[0]);
				if (argsList.Count == 1)
				{
					return num5.ToString();
				}
				return num5.ToString(Convert.ToString(argsList[1]));
			}
			case 4:
			case 6:
			{
				ulong num6 = Convert.ToUInt64(argsList[0]);
				if (argsList.Count == 1)
				{
					return num6.ToString();
				}
				return num6.ToString(Convert.ToString(argsList[1]));
			}
			case 5:
			case 7:
			{
				long num4 = Convert.ToInt64(argsList[0]);
				if (argsList.Count == 1)
				{
					return num4.ToString();
				}
				return num4.ToString(Convert.ToString(argsList[1]));
			}
			case 8:
			{
				DateTime dateTime = Convert.ToDateTime(argsList[0]);
				if (argsList.Count == 1)
				{
					return dateTime.ToString();
				}
				return dateTime.ToString(Convert.ToString(argsList[1]));
			}
			default:
				if (num2 == 8)
				{
					return Convert.ToBoolean(argsList[0]).ToString();
				}
				return argsList[0].ToString();
			}
		}
		case StiFunctionType.Format:
			switch (num)
			{
			case 1:
				return string.Format(Convert.ToString(argsList[0]), argsList[1]);
			case 2:
				return string.Format(Convert.ToString(argsList[0]), argsList[1], argsList[2]);
			case 3:
				return string.Format(Convert.ToString(argsList[0]), argsList[1], argsList[2], argsList[3]);
			case 4:
				return string.Format(Convert.ToString(argsList[0]), argsList[1], argsList[2], argsList[3], argsList[4]);
			case 5:
				return string.Format(Convert.ToString(argsList[0]), argsList[1], argsList[2], argsList[3], argsList[4], argsList[5]);
			case 6:
				return string.Format(Convert.ToString(argsList[0]), argsList[1], argsList[2], argsList[3], argsList[4], argsList[5], argsList[6]);
			case 7:
				return string.Format(Convert.ToString(argsList[0]), argsList[1], argsList[2], argsList[3], argsList[4], argsList[5], argsList[6], argsList[7]);
			}
			break;
		case StiFunctionType.SystemConvertToBoolean:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "System.Convert.ToBoolean", argsList.Count.ToString());
			}
			return Convert.ToBoolean(argsList[0]);
		case StiFunctionType.SystemConvertToByte:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "System.Convert.ToByte", argsList.Count.ToString());
			}
			return Convert.ToByte(argsList[0]);
		case StiFunctionType.SystemConvertToChar:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "System.Convert.ToChar", argsList.Count.ToString());
			}
			return Convert.ToChar(argsList[0]);
		case StiFunctionType.SystemConvertToDateTime:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "System.Convert.ToDateTime", argsList.Count.ToString());
			}
			return Convert.ToDateTime(argsList[0]);
		case StiFunctionType.SystemConvertToDecimal:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "System.Convert.ToDecimal", argsList.Count.ToString());
			}
			return Convert.ToDecimal(argsList[0]);
		case StiFunctionType.SystemConvertToDouble:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "System.Convert.ToDouble", argsList.Count.ToString());
			}
			return Convert.ToDouble(argsList[0]);
		case StiFunctionType.SystemConvertToInt16:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "System.Convert.ToInt16", argsList.Count.ToString());
			}
			return Convert.ToInt16(argsList[0]);
		case StiFunctionType.SystemConvertToInt32:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "System.Convert.ToInt32", argsList.Count.ToString());
			}
			return Convert.ToInt32(argsList[0]);
		case StiFunctionType.SystemConvertToInt64:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "System.Convert.ToInt64", argsList.Count.ToString());
			}
			return Convert.ToInt64(argsList[0]);
		case StiFunctionType.SystemConvertToSByte:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "System.Convert.ToSByte", argsList.Count.ToString());
			}
			return Convert.ToSByte(argsList[0]);
		case StiFunctionType.SystemConvertToSingle:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "System.Convert.ToSingle", argsList.Count.ToString());
			}
			return Convert.ToSingle(argsList[0]);
		case StiFunctionType.SystemConvertToString:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "System.Convert.ToString", argsList.Count.ToString());
			}
			return Convert.ToString(argsList[0]);
		case StiFunctionType.SystemConvertToUInt16:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "System.Convert.ToUInt16", argsList.Count.ToString());
			}
			return Convert.ToUInt16(argsList[0]);
		case StiFunctionType.SystemConvertToUInt32:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "System.Convert.ToUInt32", argsList.Count.ToString());
			}
			return Convert.ToUInt32(argsList[0]);
		case StiFunctionType.SystemConvertToUInt64:
			if (argsList.Count != 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "System.Convert.ToUInt64", argsList.Count.ToString());
			}
			return Convert.ToUInt64(argsList[0]);
		case StiFunctionType.MathRound:
			switch (num)
			{
			case 1:
				return Math.Round(Convert.ToDouble(argsList[0]));
			case 2:
				return Math.Round(Convert.ToDouble(argsList[0]), (MidpointRounding)argsList[1]);
			case 3:
				return Math.Round(Convert.ToDouble(argsList[0]), Convert.ToInt32(argsList[1]));
			case 4:
				return Math.Round(Convert.ToDouble(argsList[0]), Convert.ToInt32(argsList[1]), (MidpointRounding)argsList[2]);
			case 5:
				return Math.Round(Convert.ToDecimal(argsList[0]));
			case 6:
				return Math.Round(Convert.ToDecimal(argsList[0]), (MidpointRounding)argsList[1]);
			case 7:
				return Math.Round(Convert.ToDecimal(argsList[0]), Convert.ToInt32(argsList[1]));
			case 8:
				return Math.Round(Convert.ToDecimal(argsList[0]), Convert.ToInt32(argsList[1]), (MidpointRounding)argsList[2]);
			}
			break;
		case StiFunctionType.MathPow:
			if (num == 1)
			{
				return Math.Pow(Convert.ToDouble(argsList[0]), Convert.ToDouble(argsList[1]));
			}
			break;
		case StiFunctionType.ConvertRtf:
		{
			if (argsList.Count < 1)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "ConvertRtf", argsList.Count.ToString());
			}
			object[] array = new object[argsList.Count];
			Array.Copy(argsList.ToArray(), array, argsList.Count);
			object obj2 = typeof(StiReport).InvokeMember("ConvertRtf", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, report, array);
			return Convert.ToString(obj2);
		}
		case StiFunctionType.GetAnchorPageNumber:
			if (num == 1)
			{
				return report.GetAnchorPageNumber(argsList[0]);
			}
			break;
		case StiFunctionType.GetAnchorPageNumberThrough:
			if (num == 1)
			{
				return report.GetAnchorPageNumberThrough(argsList[0]);
			}
			break;
		case StiFunctionType.ParseTimeSpan:
			if (num == 1)
			{
				return StiValueHelper.TryToTimeSpan((object)Convert.ToString(argsList[0]));
			}
			break;
		case StiFunctionType.ParseDateTime:
			if (num == 1)
			{
				return StiValueHelper.TryToDateTime((object)Convert.ToString(argsList[0]));
			}
			break;
		case StiFunctionType.ParseDecimal:
			if (num == 1)
			{
				return StiValueHelper.TryToDecimal((object)Convert.ToString(argsList[0]));
			}
			break;
		case StiFunctionType.ParseDouble:
			if (num == 1)
			{
				return StiValueHelper.TryToDouble((object)Convert.ToString(argsList[0]));
			}
			break;
		case StiFunctionType.ParseInt:
			if (num == 1)
			{
				return StiValueHelper.TryToInt((object)Convert.ToString(argsList[0]));
			}
			break;
		case StiFunctionType.ParseInt64:
			if (num == 1)
			{
				return StiValueHelper.TryToLong((object)Convert.ToString(argsList[0]));
			}
			break;
		case StiFunctionType.EngineHelperJoinColumnContent:
			switch (num)
			{
			case 1:
				return Func.EngineHelper.JoinColumnContent((StiDataSource)argsList[0], Convert.ToString(argsList[1]), Convert.ToString(argsList[2]));
			case 2:
				return Func.EngineHelper.JoinColumnContent((StiBusinessObject)argsList[0], Convert.ToString(argsList[1]), Convert.ToString(argsList[2]));
			case 3:
				return Func.EngineHelper.JoinColumnContent((StiDataSource)argsList[0], Convert.ToString(argsList[1]), Convert.ToString(argsList[2]), Convert.ToBoolean(argsList[3]));
			case 4:
				return Func.EngineHelper.JoinColumnContent((StiBusinessObject)argsList[0], Convert.ToString(argsList[1]), Convert.ToString(argsList[2]), Convert.ToBoolean(argsList[3]));
			}
			break;
		case StiFunctionType.EngineHelperToQueryString:
		{
			if (num <= 0 || !(argsList[0] is IEnumerable))
			{
				break;
			}
			List<object> list = new List<object>();
			try
			{
				IEnumerator enumerator = ((IEnumerable)argsList[0]).GetEnumerator();
				while (enumerator.MoveNext())
				{
					list.Add(enumerator.Current);
				}
			}
			catch
			{
			}
			switch (num)
			{
			case 1:
				return Func.EngineHelper.ToQueryString(list, Convert.ToString(argsList[1]), Convert.ToString(argsList[2]));
			case 2:
				return Func.EngineHelper.ToQueryString(list, Convert.ToString(argsList[1]), Convert.ToString(argsList[2]), Convert.ToBoolean(argsList[3]));
			}
			break;
		}
		case StiFunctionType.EngineHelperGetRealPageNumber:
			if (num == 1)
			{
				return Func.EngineHelper.GetRealPageNumber(argsList[0]);
			}
			break;
		case StiFunctionType.StiNullValuesHelperIsNull:
			if (num == 1)
			{
				return StiNullValuesHelper.IsNull(argsList[0] as StiReport, Convert.ToString(argsList[1]));
			}
			break;
		case StiFunctionType.TimeSpanFromDays:
			if (num == 1)
			{
				return TimeSpan.FromDays(Convert.ToDouble(argsList[0]));
			}
			break;
		case StiFunctionType.TimeSpanFromHours:
			if (num == 1)
			{
				return TimeSpan.FromHours(Convert.ToDouble(argsList[0]));
			}
			break;
		case StiFunctionType.TimeSpanFromMilliseconds:
			if (num == 1)
			{
				return TimeSpan.FromMilliseconds(Convert.ToDouble(argsList[0]));
			}
			break;
		case StiFunctionType.TimeSpanFromMinutes:
			if (num == 1)
			{
				return TimeSpan.FromMinutes(Convert.ToDouble(argsList[0]));
			}
			break;
		case StiFunctionType.TimeSpanFromSeconds:
			if (num == 1)
			{
				return TimeSpan.FromSeconds(Convert.ToDouble(argsList[0]));
			}
			break;
		case StiFunctionType.TimeSpanFromTicks:
			if (num == 1)
			{
				return TimeSpan.FromTicks(Convert.ToInt64(argsList[0]));
			}
			break;
		case StiFunctionType.ImageFromFile:
			if (num == 1)
			{
				return Image.FromFile(Convert.ToString(argsList[0]));
			}
			break;
		case StiFunctionType.GetLabel:
			return report.GetLabel(Convert.ToString(argsList[0]));
		case StiFunctionType.GetParam:
			return report.GetParam(Convert.ToString(argsList[0]));
		}
		if ((stiFunctionType >= StiFunctionType.rCount && stiFunctionType <= StiFunctionType.rLast) || (stiFunctionType >= StiFunctionType.riCount && stiFunctionType <= StiFunctionType.riLast) || (stiFunctionType >= StiFunctionType.cCount && stiFunctionType <= StiFunctionType.cLast) || (stiFunctionType >= StiFunctionType.crCount && stiFunctionType <= StiFunctionType.crLast) || (stiFunctionType >= StiFunctionType.ciCount && stiFunctionType <= StiFunctionType.ciLast) || (stiFunctionType >= StiFunctionType.criCount && stiFunctionType <= StiFunctionType.criLast))
		{
			ThrowError(ParserErrorCode.FunctionNotYetImplemented, stiFunctionType.ToString());
		}
		if (stiFunctionType >= StiFunctionType.UserFunction)
		{
			string text = null;
			foreach (DictionaryEntry userFunctions in UserFunctionsList)
			{
				if ((StiFunctionType)userFunctions.Value == stiFunctionType)
				{
					text = (string)userFunctions.Key;
					break;
				}
			}
			if (text != null)
			{
				int count = argsList.Count;
				Type[] array2 = new Type[count];
				object[] array3 = new object[count];
				for (int j = 0; j < count; j++)
				{
					array3[j] = argsList[j];
					if (array3[j] is StiRefVariableObject)
					{
						array3[j] = (array3[j] as StiRefVariableObject).Value;
					}
					if (array3[j] == null)
					{
						array2[j] = typeof(object);
					}
					else
					{
						array2[j] = array3[j].GetType();
					}
				}
				StiFunction[] functions = StiFunctions.GetFunctions(isCompile: false);
				StiFunction[] array4 = functions;
				foreach (StiFunction func in array4)
				{
					if (func.FunctionName != text || ((func.ArgumentTypes != null) ? func.ArgumentTypes.Length : 0) != count)
					{
						continue;
					}
					bool flag = true;
					for (int l = 0; l < count; l++)
					{
						if (!IsImplicitlyCastableTo(array2[l], func.ArgumentTypes[l]))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						continue;
					}
					IEnumerable<MethodInfo> source = from m in func.TypeOfFunction.GetMethods()
						where m.Name == func.FunctionName
						select m;
					object result4 = ((source.Count() > 1) ? source.Where((MethodInfo m) => IsMethodParametersEqual(m, func.ArgumentTypes)).FirstOrDefault() : source.FirstOrDefault())?.Invoke(report, array3);
					for (int n = 0; n < count; n++)
					{
						if (argsList[n] is StiRefVariableObject stiRefVariableObject)
						{
							report[stiRefVariableObject.Name] = array3[n];
						}
					}
					return result4;
				}
			}
		}
		return null;
	}

	private bool IsMethodParametersEqual(MethodInfo method, Type[] argumentTypes)
	{
		return (from p in method.GetParameters()
			select p.ParameterType).SequenceEqual(argumentTypes);
	}

	private object call_method(object name, ArrayList argsList, StiAsmCommand asmCommand)
	{
		object obj = argsList[0];
		int num = CheckParserMethodInfo2((StiFunctionType)name, argsList, asmCommand);
		StiMethodType stiMethodType = (StiMethodType)name;
		if (stiMethodType == StiMethodType.ToString)
		{
			switch (get_category(argsList[0]))
			{
			case 1:
				return Convert.ToString(argsList[0]);
			case 2:
			case 3:
			{
				decimal num4 = Convert.ToDecimal(argsList[0]);
				if (argsList.Count == 1)
				{
					return num4.ToString();
				}
				return num4.ToString(Convert.ToString(argsList[1]));
			}
			case 4:
			case 6:
			{
				ulong num2 = Convert.ToUInt64(argsList[0]);
				if (argsList.Count == 1)
				{
					return num2.ToString();
				}
				return num2.ToString(Convert.ToString(argsList[1]));
			}
			case 5:
			case 7:
			{
				long num3 = Convert.ToInt64(argsList[0]);
				if (argsList.Count == 1)
				{
					return num3.ToString();
				}
				return num3.ToString(Convert.ToString(argsList[1]));
			}
			case 8:
			{
				DateTime dateTime = Convert.ToDateTime(argsList[0]);
				if (argsList.Count == 1)
				{
					return dateTime.ToString();
				}
				return dateTime.ToString(Convert.ToString(argsList[1]));
			}
			case 9:
				return Convert.ToBoolean(argsList[0]).ToString();
			default:
				if (obj is TimeSpan)
				{
					TimeSpan timeSpan = (TimeSpan)argsList[0];
					if (argsList.Count == 1)
					{
						return timeSpan.ToString();
					}
					return timeSpan.ToString(Convert.ToString(argsList[1]));
				}
				if (obj == null)
				{
					return string.Empty;
				}
				return argsList[0].ToString();
			}
		}
		if (obj is string)
		{
			switch ((StiMethodType)name)
			{
			case StiMethodType.Substring:
				switch (num)
				{
				case 1:
					return ((string)argsList[0]).Substring((int)argsList[1]);
				case 2:
					return ((string)argsList[0]).Substring((int)argsList[1], (int)argsList[2]);
				}
				break;
			case StiMethodType.ToLower:
				if (num == 1)
				{
					return ((string)argsList[0]).ToLowerInvariant();
				}
				break;
			case StiMethodType.ToUpper:
				if (num == 1)
				{
					return ((string)argsList[0]).ToUpperInvariant();
				}
				break;
			case StiMethodType.IndexOf:
				if (num == 1)
				{
					return ((string)argsList[0]).IndexOf((string)argsList[1]);
				}
				break;
			case StiMethodType.StartsWith:
				if (num == 1)
				{
					return ((string)argsList[0]).StartsWith((string)argsList[1]);
				}
				break;
			case StiMethodType.EndsWith:
				if (num == 1)
				{
					return ((string)argsList[0]).EndsWith((string)argsList[1]);
				}
				break;
			case StiMethodType.Contains:
				if (num == 1)
				{
					return ((string)argsList[0]).Contains(Convert.ToString(argsList[1]));
				}
				break;
			case StiMethodType.Replace:
				switch (num)
				{
				case 1:
					return ((string)argsList[0]).Replace((char)argsList[1], (char)argsList[2]);
				case 2:
					return ((string)argsList[0]).Replace((string)argsList[1], (string)argsList[2]);
				}
				break;
			case StiMethodType.PadLeft:
				switch (num)
				{
				case 1:
					return ((string)argsList[0]).PadLeft((int)argsList[1]);
				case 2:
					return ((string)argsList[0]).PadLeft((int)argsList[1], (char)argsList[2]);
				}
				break;
			case StiMethodType.PadRight:
				switch (num)
				{
				case 1:
					return ((string)argsList[0]).PadRight((int)argsList[1]);
				case 2:
					return ((string)argsList[0]).PadRight((int)argsList[1], (char)argsList[2]);
				}
				break;
			case StiMethodType.TrimStart:
				if (num == 1)
				{
					return ((string)argsList[0]).TrimStart(new char[0]);
				}
				break;
			case StiMethodType.TrimEnd:
				if (num == 1)
				{
					return ((string)argsList[0]).TrimEnd(new char[0]);
				}
				break;
			}
		}
		if (obj is List<bool> || obj is List<char> || obj is List<DateTime> || obj is List<TimeSpan> || obj is List<decimal> || obj is List<float> || obj is List<double> || obj is List<byte> || obj is List<short> || obj is List<int> || obj is List<long> || obj is List<Guid> || obj is List<string>)
		{
			switch ((StiMethodType)name)
			{
			case StiMethodType.Contains:
				switch (num)
				{
				case 210:
					return ((List<bool>)argsList[0]).Contains(Convert.ToBoolean(argsList[1]));
				case 220:
					return ((List<char>)argsList[0]).Contains(Convert.ToChar(argsList[1]));
				case 230:
					return ((List<DateTime>)argsList[0]).Contains(Convert.ToDateTime(argsList[1]));
				case 231:
					return ((DateTimeList)argsList[0]).Contains(Convert.ToString(argsList[1]));
				case 240:
					return ((List<TimeSpan>)argsList[0]).Contains((TimeSpan)argsList[1]);
				case 241:
					return ((TimeSpanList)argsList[0]).Contains(Convert.ToString(argsList[1]));
				case 250:
					return ((List<decimal>)argsList[0]).Contains(Convert.ToDecimal(argsList[1]));
				case 251:
					return ((DecimalList)argsList[0]).Contains(Convert.ToInt64(argsList[1]));
				case 252:
					return ((DecimalList)argsList[0]).Contains(Convert.ToString(argsList[1]));
				case 260:
					return ((List<float>)argsList[0]).Contains(Convert.ToSingle(argsList[1]));
				case 261:
					return ((FloatList)argsList[0]).Contains(Convert.ToInt64(argsList[1]));
				case 262:
					return ((FloatList)argsList[0]).Contains(Convert.ToString(argsList[1]));
				case 270:
					return ((List<double>)argsList[0]).Contains(Convert.ToDouble(argsList[1]));
				case 271:
					return ((DoubleList)argsList[0]).Contains(Convert.ToInt64(argsList[1]));
				case 272:
					return ((DoubleList)argsList[0]).Contains(Convert.ToString(argsList[1]));
				case 280:
					return ((List<byte>)argsList[0]).Contains(Convert.ToByte(argsList[1]));
				case 282:
					return ((ByteList)argsList[0]).Contains(Convert.ToDecimal(argsList[1]));
				case 283:
					return ((ByteList)argsList[0]).Contains(Convert.ToString(argsList[1]));
				case 290:
					return ((List<short>)argsList[0]).Contains(Convert.ToInt16(argsList[1]));
				case 292:
					return ((ShortList)argsList[0]).Contains(Convert.ToDecimal(argsList[1]));
				case 293:
					return ((ShortList)argsList[0]).Contains(Convert.ToString(argsList[1]));
				case 300:
					return ((List<int>)argsList[0]).Contains(Convert.ToInt32(argsList[1]));
				case 302:
					return ((IntList)argsList[0]).Contains(Convert.ToDecimal(argsList[1]));
				case 303:
					return ((IntList)argsList[0]).Contains(Convert.ToString(argsList[1]));
				case 310:
					return ((List<long>)argsList[0]).Contains(Convert.ToInt64(argsList[1]));
				case 312:
					return ((LongList)argsList[0]).Contains(Convert.ToDecimal(argsList[1]));
				case 313:
					return ((LongList)argsList[0]).Contains(Convert.ToString(argsList[1]));
				case 320:
					return ((List<Guid>)argsList[0]).Contains((Guid)argsList[1]);
				case 330:
					return ((List<string>)argsList[0]).Contains(Convert.ToString(argsList[1]));
				case 331:
					return ((StringList)argsList[0]).Contains(Convert.ToInt64(argsList[1]));
				case 332:
					return ((StringList)argsList[0]).Contains(Convert.ToDecimal(argsList[1]));
				case 333:
					return ((StringList)argsList[0]).Contains(Convert.ToDouble(argsList[1]));
				}
				break;
			case StiMethodType.ToQueryString:
				if (argsList.Count == 1)
				{
					if (obj is List<bool>)
					{
						return ((BoolList)argsList[0]).ToQueryString();
					}
					if (obj is List<char>)
					{
						return ((CharList)argsList[0]).ToQueryString();
					}
					if (obj is List<DateTime>)
					{
						return ((DateTimeList)argsList[0]).ToQueryString();
					}
					if (obj is List<TimeSpan>)
					{
						return ((TimeSpanList)argsList[0]).ToQueryString();
					}
					if (obj is List<decimal>)
					{
						return ((DecimalList)argsList[0]).ToQueryString();
					}
					if (obj is List<float>)
					{
						return ((FloatList)argsList[0]).ToQueryString();
					}
					if (obj is List<double>)
					{
						return ((DoubleList)argsList[0]).ToQueryString();
					}
					if (obj is List<byte>)
					{
						return ((ByteList)argsList[0]).ToQueryString();
					}
					if (obj is List<short>)
					{
						return ((ShortList)argsList[0]).ToQueryString();
					}
					if (obj is List<int>)
					{
						return ((IntList)argsList[0]).ToQueryString();
					}
					if (obj is List<long>)
					{
						return ((LongList)argsList[0]).ToQueryString();
					}
					if (obj is List<Guid>)
					{
						return ((GuidList)argsList[0]).ToQueryString();
					}
					if (obj is List<string>)
					{
						return ((StringList)argsList[0]).ToQueryString();
					}
				}
				else if (argsList.Count == 2)
				{
					if (obj is List<bool>)
					{
						return ((BoolList)argsList[0]).ToQueryString(Convert.ToString(argsList[1]));
					}
					if (obj is List<char>)
					{
						return ((CharList)argsList[0]).ToQueryString(Convert.ToString(argsList[1]));
					}
					if (obj is List<DateTime>)
					{
						return ((DateTimeList)argsList[0]).ToQueryString(Convert.ToString(argsList[1]));
					}
					if (obj is List<TimeSpan>)
					{
						return ((TimeSpanList)argsList[0]).ToQueryString(Convert.ToString(argsList[1]));
					}
					if (obj is List<decimal>)
					{
						return ((DecimalList)argsList[0]).ToQueryString(Convert.ToString(argsList[1]));
					}
					if (obj is List<float>)
					{
						return ((FloatList)argsList[0]).ToQueryString(Convert.ToString(argsList[1]));
					}
					if (obj is List<double>)
					{
						return ((DoubleList)argsList[0]).ToQueryString(Convert.ToString(argsList[1]));
					}
					if (obj is List<byte>)
					{
						return ((ByteList)argsList[0]).ToQueryString(Convert.ToString(argsList[1]));
					}
					if (obj is List<short>)
					{
						return ((ShortList)argsList[0]).ToQueryString(Convert.ToString(argsList[1]));
					}
					if (obj is List<int>)
					{
						return ((IntList)argsList[0]).ToQueryString(Convert.ToString(argsList[1]));
					}
					if (obj is List<long>)
					{
						return ((LongList)argsList[0]).ToQueryString(Convert.ToString(argsList[1]));
					}
					if (obj is List<Guid>)
					{
						return ((GuidList)argsList[0]).ToQueryString(Convert.ToString(argsList[1]));
					}
					if (obj is List<string>)
					{
						return ((StringList)argsList[0]).ToQueryString(Convert.ToString(argsList[1]));
					}
				}
				else if (argsList.Count == 3)
				{
					if (obj is List<DateTime>)
					{
						return ((DateTimeList)argsList[0]).ToQueryString(Convert.ToString(argsList[1]), Convert.ToString(argsList[2]));
					}
					if (obj is List<TimeSpan>)
					{
						return ((TimeSpanList)argsList[0]).ToQueryString(Convert.ToString(argsList[1]), Convert.ToString(argsList[2]));
					}
				}
				else
				{
					ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "ToQueryString", (argsList.Count - 1).ToString());
				}
				break;
			}
		}
		if (obj is StiDataSource)
		{
			StiMethodType stiMethodType2 = (StiMethodType)name;
			if (stiMethodType2 == StiMethodType.GetData)
			{
				int num5 = get_category(argsList[1]);
				if (num5 != 1)
				{
					ThrowError(ParserErrorCode.MethodHasInvalidArgument, "GetData", "1", GetTypeName(argsList[0]), "string");
				}
				if (argsList.Count == 3)
				{
					num5 = get_category(argsList[2]);
					if (num5 < 4 || num5 > 7)
					{
						ThrowError(ParserErrorCode.MethodHasInvalidArgument, "GetData", "2", GetTypeName(argsList[0]), "int");
					}
					return ((StiDataSource)argsList[0]).GetData(Convert.ToString(argsList[1]), Convert.ToInt32(argsList[2]));
				}
				if (argsList.Count == 2)
				{
					return ((StiDataSource)argsList[0]).GetData(Convert.ToString(argsList[1]));
				}
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "GetData", (argsList.Count - 1).ToString());
			}
		}
		if (obj is DateTime)
		{
			switch ((StiMethodType)name)
			{
			case StiMethodType.AddDays:
			{
				int num5 = get_category(argsList[1]);
				if (num5 < 2 || num5 > 7)
				{
					ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "AddDays", "1", GetTypeName(argsList[1]), "double");
				}
				if (argsList.Count == 2)
				{
					return ((DateTime)argsList[0]).AddDays(Convert.ToDouble(argsList[1]));
				}
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "AddDays", (argsList.Count - 1).ToString());
				break;
			}
			case StiMethodType.AddHours:
			{
				int num5 = get_category(argsList[1]);
				if (num5 < 2 || num5 > 7)
				{
					ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "AddHours", "1", GetTypeName(argsList[1]), "double");
				}
				if (argsList.Count == 2)
				{
					return ((DateTime)argsList[0]).AddHours(Convert.ToDouble(argsList[1]));
				}
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "AddHours", (argsList.Count - 1).ToString());
				break;
			}
			case StiMethodType.AddMilliseconds:
			{
				int num5 = get_category(argsList[1]);
				if (num5 < 2 || num5 > 7)
				{
					ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "AddMilliseconds", "1", GetTypeName(argsList[1]), "double");
				}
				if (argsList.Count == 2)
				{
					return ((DateTime)argsList[0]).AddMilliseconds(Convert.ToDouble(argsList[1]));
				}
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "AddMilliseconds", (argsList.Count - 1).ToString());
				break;
			}
			case StiMethodType.AddMinutes:
			{
				int num5 = get_category(argsList[1]);
				if (num5 < 2 || num5 > 7)
				{
					ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "AddMinutes", "1", GetTypeName(argsList[1]), "double");
				}
				if (argsList.Count == 2)
				{
					return ((DateTime)argsList[0]).AddMinutes(Convert.ToDouble(argsList[1]));
				}
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "AddMinutes", (argsList.Count - 1).ToString());
				break;
			}
			case StiMethodType.AddMonths:
			{
				int num5 = get_category(argsList[1]);
				if (num5 < 4 || num5 > 7)
				{
					ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "AddMonths", "1", GetTypeName(argsList[1]), "int");
				}
				if (argsList.Count == 2)
				{
					return ((DateTime)argsList[0]).AddMonths(Convert.ToInt32(argsList[1]));
				}
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "AddMonths", (argsList.Count - 1).ToString());
				break;
			}
			case StiMethodType.AddSeconds:
			{
				int num5 = get_category(argsList[1]);
				if (num5 < 2 || num5 > 7)
				{
					ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "AddSeconds", "1", GetTypeName(argsList[1]), "double");
				}
				if (argsList.Count == 2)
				{
					return ((DateTime)argsList[0]).AddSeconds(Convert.ToDouble(argsList[1]));
				}
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "AddSeconds", (argsList.Count - 1).ToString());
				break;
			}
			case StiMethodType.AddYears:
			{
				int num5 = get_category(argsList[1]);
				if (num5 < 4 || num5 > 7)
				{
					ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "AddYears", "1", GetTypeName(argsList[1]), "int");
				}
				if (argsList.Count == 2)
				{
					return ((DateTime)argsList[0]).AddYears(Convert.ToInt32(argsList[1]));
				}
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "AddYears", (argsList.Count - 1).ToString());
				break;
			}
			case StiMethodType.ToShortDateString:
				if (num == 1)
				{
					return ((DateTime)argsList[0]).ToShortDateString();
				}
				break;
			case StiMethodType.ToShortTimeString:
				if (num == 1)
				{
					return ((DateTime)argsList[0]).ToShortTimeString();
				}
				break;
			case StiMethodType.ToLongDateString:
				if (num == 1)
				{
					return ((DateTime)argsList[0]).ToLongDateString();
				}
				break;
			case StiMethodType.ToLongTimeString:
				if (num == 1)
				{
					return ((DateTime)argsList[0]).ToLongTimeString();
				}
				break;
			}
		}
		if (obj is TimeSpan)
		{
			switch ((StiMethodType)name)
			{
			case StiMethodType.Add:
				if (!(argsList[1] is TimeSpan))
				{
					ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Add", "1", GetTypeName(argsList[1]), "TimeSpan");
				}
				if (argsList.Count == 2)
				{
					return ((TimeSpan)argsList[0]).Add((TimeSpan)argsList[1]);
				}
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "Add", (argsList.Count - 1).ToString());
				break;
			case StiMethodType.Subtract:
				if (!(argsList[1] is TimeSpan))
				{
					ThrowError(ParserErrorCode.FunctionHasInvalidArgument, "Subtract", "1", GetTypeName(argsList[1]), "TimeSpan");
				}
				if (argsList.Count == 2)
				{
					return ((TimeSpan)argsList[0]).Subtract((TimeSpan)argsList[1]);
				}
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "Subtract", (argsList.Count - 1).ToString());
				break;
			}
		}
		if (obj is StiGroupHeaderBand)
		{
			StiMethodType stiMethodType3 = (StiMethodType)name;
			if (stiMethodType3 == StiMethodType.GetCurrentConditionValue)
			{
				return ((StiGroupHeaderBand)argsList[0]).GetCurrentConditionValue();
			}
		}
		string message = ((obj == null) ? "null" : GetTypeName(argsList[0]));
		ThrowError(ParserErrorCode.ItemDoesNotContainDefinition, message, Enum.GetName(typeof(StiMethodType), name));
		return null;
	}

	private object op_Add(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num <= 1 || num2 <= 1)
		{
			return Convert.ToString(par1) + Convert.ToString(par2);
		}
		if (num == 2 || num2 == 2)
		{
			return Convert.ToDecimal(par1) + Convert.ToDecimal(par2);
		}
		if (num == 3 || num2 == 3)
		{
			return Convert.ToDouble(par1) + Convert.ToDouble(par2);
		}
		if (num == 4 || num2 == 4)
		{
			if (num == 5)
			{
				return Convert.ToInt64(par1) + Convert.ToInt64(par2);
			}
			return Convert.ToUInt64(par1) + Convert.ToUInt64(par2);
		}
		if (num == 5 || num2 == 5)
		{
			return Convert.ToInt64(par1) + Convert.ToInt64(par2);
		}
		if (num == 6 || num2 == 6)
		{
			if (num == 7)
			{
				return Convert.ToInt32(par1) + Convert.ToInt32(par2);
			}
			return Convert.ToUInt32(par1) + Convert.ToUInt32(par2);
		}
		if (num == 7 || num2 == 7)
		{
			return Convert.ToInt32(par1) + Convert.ToInt32(par2);
		}
		if (num == 8 && par2 is TimeSpan)
		{
			return Convert.ToDateTime(par1).Add((TimeSpan)par2);
		}
		if (num2 == 8 && par1 is TimeSpan)
		{
			return Convert.ToDateTime(par2).Add((TimeSpan)par1);
		}
		ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "+", GetTypeName(par1), GetTypeName(par2));
		return null;
	}

	private object op_Sub(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num <= 1 || num2 <= 1)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "-", GetTypeName(par1), GetTypeName(par2));
		}
		else
		{
			if (num == 2 || num2 == 2)
			{
				return Convert.ToDecimal(par1) - Convert.ToDecimal(par2);
			}
			if (num == 3 || num2 == 3)
			{
				return Convert.ToDouble(par1) - Convert.ToDouble(par2);
			}
			if (num == 4 || num2 == 4 || num == 5 || num2 == 5)
			{
				return Convert.ToInt64(par1) - Convert.ToInt64(par2);
			}
			if (num == 6 || num2 == 6 || num == 7 || num2 == 7)
			{
				return Convert.ToInt32(par1) - Convert.ToInt32(par2);
			}
			if (num == 8 && (num2 == 8 || par2 is TimeSpan))
			{
				if (num2 == 8)
				{
					return Convert.ToDateTime(par1).Subtract(Convert.ToDateTime(par2));
				}
				return Convert.ToDateTime(par1).Subtract((TimeSpan)par2);
			}
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "-", GetTypeName(par1), GetTypeName(par2));
		}
		return null;
	}

	private object op_Mult(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num <= 1 || num2 <= 1)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "*", GetTypeName(par1), GetTypeName(par2));
		}
		else
		{
			if (num == 2 || num2 == 2)
			{
				return Convert.ToDecimal(par1) * Convert.ToDecimal(par2);
			}
			if (num == 3 || num2 == 3)
			{
				return Convert.ToDouble(par1) * Convert.ToDouble(par2);
			}
			if (num == 4 || num2 == 4 || num == 5 || num2 == 5)
			{
				return Convert.ToInt64(par1) * Convert.ToInt64(par2);
			}
			if (num == 6 || num2 == 6 || num == 7 || num2 == 7)
			{
				return Convert.ToInt32(par1) * Convert.ToInt32(par2);
			}
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "*", GetTypeName(par1), GetTypeName(par2));
		}
		return null;
	}

	private object op_Div(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num <= 1 || num2 <= 1)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "/", GetTypeName(par1), GetTypeName(par2));
		}
		else
		{
			if (num == 2 || num2 == 2)
			{
				return Convert.ToDecimal(par1) / Convert.ToDecimal(par2);
			}
			if (num == 3 || num2 == 3)
			{
				return Convert.ToDouble(par1) / Convert.ToDouble(par2);
			}
			if (num == 4 || num2 == 4 || num == 5 || num2 == 5)
			{
				return Convert.ToInt64(par1) / Convert.ToInt64(par2);
			}
			if (num == 6 || num2 == 6 || num == 7 || num2 == 7)
			{
				return Convert.ToInt32(par1) / Convert.ToInt32(par2);
			}
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "/", GetTypeName(par1), GetTypeName(par2));
		}
		return null;
	}

	private object op_Mod(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num <= 1 || num2 <= 1)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "%", GetTypeName(par1), GetTypeName(par2));
		}
		else
		{
			if (num == 2 || num2 == 2)
			{
				return Convert.ToDecimal(par1) % Convert.ToDecimal(par2);
			}
			if (num == 3 || num2 == 3)
			{
				return Convert.ToDouble(par1) % Convert.ToDouble(par2);
			}
			if (num == 4 || num2 == 4 || num == 5 || num2 == 5)
			{
				return Convert.ToInt64(par1) % Convert.ToInt64(par2);
			}
			if (num == 6 || num2 == 6 || num == 7 || num2 == 7)
			{
				return Convert.ToInt32(par1) % Convert.ToInt32(par2);
			}
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "%", GetTypeName(par1), GetTypeName(par2));
		}
		return null;
	}

	private object op_Pow(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num >= 2 && num2 >= 2 && num <= 7 && num2 <= 7)
		{
			return Math.Pow(Convert.ToDouble(par1), Convert.ToDouble(par2));
		}
		ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "^", GetTypeName(par1), GetTypeName(par2));
		return null;
	}

	private object op_Neg(object par1)
	{
		int num = get_category(par1);
		if (num <= 1 || num >= 8)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "Negative", GetTypeName(par1));
		}
		return op_Mult(par1, -1);
	}

	private object op_Not(object par1)
	{
		int num = get_category(par1);
		if (num != 9)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "Not", GetTypeName(par1));
		}
		return !Convert.ToBoolean(par1);
	}

	private object op_Cast(object par1, object par2)
	{
		Type type = par2 as Type;
		if (type != null)
		{
			try
			{
				return Convert.ChangeType(par1, type);
			}
			catch
			{
				return par1;
			}
		}
		return (TypeCode)par2 switch
		{
			TypeCode.Boolean => Convert.ToBoolean(par1), 
			TypeCode.Byte => Convert.ToByte(par1), 
			TypeCode.Char => Convert.ToChar(par1), 
			TypeCode.DateTime => Convert.ToDateTime(par1), 
			TypeCode.Decimal => Convert.ToDecimal(par1), 
			TypeCode.Double => Convert.ToDouble(par1), 
			TypeCode.Int16 => Convert.ToInt16(par1), 
			TypeCode.Int32 => Convert.ToInt32(par1), 
			TypeCode.Int64 => Convert.ToInt64(par1), 
			TypeCode.SByte => Convert.ToSByte(par1), 
			TypeCode.Single => Convert.ToSingle(par1), 
			TypeCode.String => Convert.ToString(par1), 
			TypeCode.UInt16 => Convert.ToUInt16(par1), 
			TypeCode.UInt32 => Convert.ToUInt32(par1), 
			TypeCode.UInt64 => Convert.ToUInt64(par1), 
			_ => par1, 
		};
	}

	private object op_CompareLeft(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num == 0 || num2 == 0)
		{
			IComparable comparable = par1 as IComparable;
			IComparable comparable2 = par2 as IComparable;
			if (comparable != null && comparable2 != null)
			{
				return comparable.CompareTo(comparable2) < 0;
			}
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "<", GetTypeName(par1), GetTypeName(par2));
		}
		if (num == 1 || num2 == 1)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "<", GetTypeName(par1), GetTypeName(par2));
		}
		else
		{
			if (num == 2 || num2 == 2 || num == 3 || num2 == 3)
			{
				return Convert.ToDecimal(par1) < Convert.ToDecimal(par2);
			}
			if (num == 4 || num2 == 4 || num == 6 || num2 == 6)
			{
				return Convert.ToUInt64(par1) < Convert.ToUInt64(par2);
			}
			if (num == 5 || num2 == 5 || num == 7 || num2 == 7)
			{
				return Convert.ToInt64(par1) < Convert.ToInt64(par2);
			}
			if (num == 8 || num2 == 8)
			{
				return Convert.ToDateTime(par1) < Convert.ToDateTime(par2);
			}
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "<", GetTypeName(par1), GetTypeName(par2));
		}
		return null;
	}

	private object op_CompareLeftEqual(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num == 0 || num2 == 0)
		{
			IComparable comparable = par1 as IComparable;
			IComparable comparable2 = par2 as IComparable;
			if (comparable != null && comparable2 != null)
			{
				return comparable.CompareTo(comparable2) <= 0;
			}
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "<=", GetTypeName(par1), GetTypeName(par2));
		}
		if (num == 1 || num2 == 1)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "<=", GetTypeName(par1), GetTypeName(par2));
		}
		else
		{
			if (num == 2 || num2 == 2 || num == 3 || num2 == 3)
			{
				return Convert.ToDecimal(par1) <= Convert.ToDecimal(par2);
			}
			if (num == 4 || num2 == 4 || num == 6 || num2 == 6)
			{
				return Convert.ToUInt64(par1) <= Convert.ToUInt64(par2);
			}
			if (num == 5 || num2 == 5 || num == 7 || num2 == 7)
			{
				return Convert.ToInt64(par1) <= Convert.ToInt64(par2);
			}
			if (num == 8 || num2 == 8)
			{
				return Convert.ToDateTime(par1) <= Convert.ToDateTime(par2);
			}
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "<=", GetTypeName(par1), GetTypeName(par2));
		}
		return null;
	}

	private object op_CompareRight(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num == 0 || num2 == 0)
		{
			IComparable comparable = par1 as IComparable;
			IComparable comparable2 = par2 as IComparable;
			if (comparable != null && comparable2 != null)
			{
				return comparable.CompareTo(comparable2) > 0;
			}
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, ">", GetTypeName(par1), GetTypeName(par2));
		}
		if (num == 1 || num2 == 1)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, ">", GetTypeName(par1), GetTypeName(par2));
		}
		else
		{
			if (num == 2 || num2 == 2 || num == 3 || num2 == 3)
			{
				return Convert.ToDecimal(par1) > Convert.ToDecimal(par2);
			}
			if (num == 4 || num2 == 4 || num == 6 || num2 == 6)
			{
				return Convert.ToUInt64(par1) > Convert.ToUInt64(par2);
			}
			if (num == 5 || num2 == 5 || num == 7 || num2 == 7)
			{
				return Convert.ToInt64(par1) > Convert.ToInt64(par2);
			}
			if (num == 8 || num2 == 8)
			{
				return Convert.ToDateTime(par1) > Convert.ToDateTime(par2);
			}
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, ">", GetTypeName(par1), GetTypeName(par2));
		}
		return null;
	}

	private object op_CompareRightEqual(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num == 0 || num2 == 0)
		{
			IComparable comparable = par1 as IComparable;
			IComparable comparable2 = par2 as IComparable;
			if (comparable != null && comparable2 != null)
			{
				return comparable.CompareTo(comparable2) >= 0;
			}
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, ">=", GetTypeName(par1), GetTypeName(par2));
		}
		if (num == 1 || num2 == 1)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, ">=", GetTypeName(par1), GetTypeName(par2));
		}
		else
		{
			if (num == 2 || num2 == 2 || num == 3 || num2 == 3)
			{
				return Convert.ToDecimal(par1) >= Convert.ToDecimal(par2);
			}
			if (num == 4 || num2 == 4 || num == 6 || num2 == 6)
			{
				return Convert.ToUInt64(par1) >= Convert.ToUInt64(par2);
			}
			if (num == 5 || num2 == 5 || num == 7 || num2 == 7)
			{
				return Convert.ToInt64(par1) >= Convert.ToInt64(par2);
			}
			if (num == 8 || num2 == 8)
			{
				return Convert.ToDateTime(par1) >= Convert.ToDateTime(par2);
			}
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, ">=", GetTypeName(par1), GetTypeName(par2));
		}
		return null;
	}

	private object op_CompareEqual(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num == -1 || num2 == -1)
		{
			return num == num2;
		}
		if (num == 0 || num2 == 0)
		{
			IComparable comparable = par1 as IComparable;
			IComparable comparable2 = par2 as IComparable;
			if (comparable != null && comparable2 != null)
			{
				return comparable.CompareTo(comparable2) == 0;
			}
			return par1.Equals(par2);
		}
		if (num == 1 || num2 == 1)
		{
			return Convert.ToString(par1) == Convert.ToString(par2);
		}
		if (num == 2 || num2 == 2 || num == 3 || num2 == 3)
		{
			return Convert.ToDecimal(par1) == Convert.ToDecimal(par2);
		}
		if (num == 4 || num2 == 4 || num == 6 || num2 == 6)
		{
			return Convert.ToUInt64(par1) == Convert.ToUInt64(par2);
		}
		if (num == 5 || num2 == 5 || num == 7 || num2 == 7)
		{
			return Convert.ToInt64(par1) == Convert.ToInt64(par2);
		}
		if (num == 8 || num2 == 8)
		{
			return Convert.ToDateTime(par1) == Convert.ToDateTime(par2);
		}
		return Convert.ToBoolean(par1) == Convert.ToBoolean(par2);
	}

	private object op_CompareNotEqual(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num == -1 || num2 == -1)
		{
			return num != num2;
		}
		if (num == 0 || num2 == 0)
		{
			IComparable comparable = par1 as IComparable;
			IComparable comparable2 = par2 as IComparable;
			if (comparable != null && comparable2 != null)
			{
				return comparable.CompareTo(comparable2) != 0;
			}
			return !par1.Equals(par2);
		}
		if (num == 1 || num2 == 1)
		{
			return Convert.ToString(par1) != Convert.ToString(par2);
		}
		if (num == 2 || num2 == 2 || num == 3 || num2 == 3)
		{
			return Convert.ToDecimal(par1) != Convert.ToDecimal(par2);
		}
		if (num == 4 || num2 == 4 || num == 6 || num2 == 6)
		{
			return Convert.ToUInt64(par1) != Convert.ToUInt64(par2);
		}
		if (num == 5 || num2 == 5 || num == 7 || num2 == 7)
		{
			return Convert.ToInt64(par1) != Convert.ToInt64(par2);
		}
		if (num == 8 || num2 == 8)
		{
			return Convert.ToDateTime(par1) != Convert.ToDateTime(par2);
		}
		return Convert.ToBoolean(par1) != Convert.ToBoolean(par2);
	}

	private object op_Shl(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num < 4 || num >= 8 || num2 != 7)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "<<", GetTypeName(par1), GetTypeName(par2));
		}
		else
		{
			if (num == 4 || num2 == 4)
			{
				return Convert.ToUInt64(par1) << Convert.ToInt32(par2);
			}
			if (num == 5 || num2 == 5)
			{
				return Convert.ToInt64(par1) << Convert.ToInt32(par2);
			}
			if (num == 6 || num2 == 6)
			{
				return Convert.ToUInt32(par1) << Convert.ToInt32(par2);
			}
			if (num == 7 || num2 == 7)
			{
				return Convert.ToInt32(par1) << Convert.ToInt32(par2);
			}
		}
		return null;
	}

	private object op_Shr(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num < 4 || num >= 8 || num2 != 7)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, ">>", GetTypeName(par1), GetTypeName(par2));
		}
		else
		{
			if (num == 4 || num2 == 4)
			{
				return Convert.ToUInt64(par1) >> Convert.ToInt32(par2);
			}
			if (num == 5 || num2 == 5)
			{
				return Convert.ToInt64(par1) >> Convert.ToInt32(par2);
			}
			if (num == 6 || num2 == 6)
			{
				return Convert.ToUInt32(par1) >> Convert.ToInt32(par2);
			}
			if (num == 7 || num2 == 7)
			{
				return Convert.ToInt32(par1) >> Convert.ToInt32(par2);
			}
		}
		return null;
	}

	private object op_And(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num < 4 || num == 8 || num2 < 4 || num2 == 8)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "&", GetTypeName(par1), GetTypeName(par2));
		}
		else
		{
			if (num == 4 || num2 == 4)
			{
				return Convert.ToUInt64(par1) & Convert.ToUInt64(par2);
			}
			if (num == 5 || num2 == 5)
			{
				return Convert.ToInt64(par1) & Convert.ToInt64(par2);
			}
			if (num == 6 || num2 == 6)
			{
				return Convert.ToUInt32(par1) & Convert.ToUInt32(par2);
			}
			if (num == 7 || num2 == 7)
			{
				return Convert.ToInt32(par1) & Convert.ToInt32(par2);
			}
			if (num == 9 || num2 == 9)
			{
				return Convert.ToBoolean(par1) & Convert.ToBoolean(par2);
			}
		}
		return null;
	}

	private object op_Or(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num < 4 || num == 8 || num2 < 4 || num2 == 8)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "|", GetTypeName(par1), GetTypeName(par2));
		}
		else
		{
			if (num == 4 || num2 == 4)
			{
				return Convert.ToUInt64(par1) | Convert.ToUInt64(par2);
			}
			if (num == 5 || num2 == 5)
			{
				return Convert.ToInt64(par1) | Convert.ToInt64(par2);
			}
			if (num == 6 || num2 == 6)
			{
				return Convert.ToUInt32(par1) | Convert.ToUInt32(par2);
			}
			if (num == 7 || num2 == 7)
			{
				return Convert.ToInt32(par1) | Convert.ToInt32(par2);
			}
			if (num == 9 || num2 == 9)
			{
				return Convert.ToBoolean(par1) | Convert.ToBoolean(par2);
			}
		}
		return null;
	}

	private object op_Xor(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num < 4 || num == 8 || num2 < 4 || num2 == 8)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "^", GetTypeName(par1), GetTypeName(par2));
		}
		else
		{
			if (num == 4 || num2 == 4)
			{
				return Convert.ToUInt64(par1) ^ Convert.ToUInt64(par2);
			}
			if (num == 5 || num2 == 5)
			{
				return Convert.ToInt64(par1) ^ Convert.ToInt64(par2);
			}
			if (num == 6 || num2 == 6)
			{
				return Convert.ToUInt32(par1) ^ Convert.ToUInt32(par2);
			}
			if (num == 7 || num2 == 7)
			{
				return Convert.ToInt32(par1) ^ Convert.ToInt32(par2);
			}
			if (num == 9 || num2 == 9)
			{
				return Convert.ToBoolean(par1) ^ Convert.ToBoolean(par2);
			}
		}
		return null;
	}

	private object op_And2(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num != 9 || num2 != 9)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "&&", GetTypeName(par1), GetTypeName(par2));
			return null;
		}
		return Convert.ToBoolean(par1) && Convert.ToBoolean(par2);
	}

	private object op_Or2(object par1, object par2)
	{
		int num = get_category(par1);
		int num2 = get_category(par2);
		if (num != 9 || num2 != 9)
		{
			ThrowError(ParserErrorCode.OperatorCannotBeAppliedToOperands, "||", GetTypeName(par1), GetTypeName(par2));
			return null;
		}
		return Convert.ToBoolean(par1) || Convert.ToBoolean(par2);
	}

	private object call_property(object name, ArrayList argsList)
	{
		object obj = argsList[0];
		if (name is string)
		{
			if (obj == null)
			{
				return null;
			}
			try
			{
				PropertyInfo property = obj.GetType().GetProperty((string)name);
				if (property != null)
				{
					return property.GetValue(obj, new object[0]);
				}
			}
			catch
			{
			}
			return null;
		}
		if (obj is DateTime)
		{
			switch ((StiPropertyType)name)
			{
			case StiPropertyType.Year:
				return ((DateTime)obj).Year;
			case StiPropertyType.Month:
				return ((DateTime)obj).Month;
			case StiPropertyType.Day:
				return ((DateTime)obj).Day;
			case StiPropertyType.Hour:
				return ((DateTime)obj).Hour;
			case StiPropertyType.Minute:
				return ((DateTime)obj).Minute;
			case StiPropertyType.Second:
				return ((DateTime)obj).Second;
			case StiPropertyType.Date:
				return ((DateTime)obj).Date;
			case StiPropertyType.DayOfWeek:
				return ((DateTime)obj).DayOfWeek;
			}
			ThrowError(ParserErrorCode.ItemDoesNotContainDefinition, GetTypeName(argsList[0]), Enum.GetName(typeof(StiPropertyType), name));
		}
		if (obj is TimeSpan)
		{
			switch ((StiPropertyType)name)
			{
			case StiPropertyType.Days:
				return ((TimeSpan)obj).Days;
			case StiPropertyType.Hours:
				return ((TimeSpan)obj).Hours;
			case StiPropertyType.Milliseconds:
				return ((TimeSpan)obj).Milliseconds;
			case StiPropertyType.Minutes:
				return ((TimeSpan)obj).Minutes;
			case StiPropertyType.Seconds:
				return ((TimeSpan)obj).Seconds;
			case StiPropertyType.Ticks:
				return ((TimeSpan)obj).Ticks;
			case StiPropertyType.TotalDays:
				return ((TimeSpan)obj).TotalDays;
			case StiPropertyType.TotalHours:
				return ((TimeSpan)obj).TotalHours;
			case StiPropertyType.TotalMinutes:
				return ((TimeSpan)obj).TotalMinutes;
			case StiPropertyType.TotalSeconds:
				return ((TimeSpan)obj).TotalSeconds;
			case StiPropertyType.TotalMilliseconds:
				return ((TimeSpan)obj).TotalMilliseconds;
			}
			ThrowError(ParserErrorCode.ItemDoesNotContainDefinition, GetTypeName(argsList[0]), Enum.GetName(typeof(StiPropertyType), name));
		}
		if (obj is string)
		{
			StiPropertyType stiPropertyType = (StiPropertyType)name;
			if (stiPropertyType == StiPropertyType.Length)
			{
				return Convert.ToString(obj).Length;
			}
			ThrowError(ParserErrorCode.ItemDoesNotContainDefinition, GetTypeName(argsList[0]), Enum.GetName(typeof(StiPropertyType), name));
		}
		if (obj == null)
		{
			StiPropertyType stiPropertyType2 = (StiPropertyType)name;
			if (stiPropertyType2 == StiPropertyType.Length)
			{
				return 0;
			}
		}
		if (obj is Range)
		{
			switch ((StiPropertyType)name)
			{
			case StiPropertyType.From:
				return (obj as Range).FromObject;
			case StiPropertyType.To:
				return (obj as Range).ToObject;
			case StiPropertyType.FromDate:
				return (obj as DateTimeRange).FromDate;
			case StiPropertyType.ToDate:
				return (obj as DateTimeRange).ToDate;
			case StiPropertyType.FromTime:
				return (obj as TimeSpanRange).FromTime;
			case StiPropertyType.ToTime:
				return (obj as TimeSpanRange).ToTime;
			}
			ThrowError(ParserErrorCode.ItemDoesNotContainDefinition, GetTypeName(argsList[0]), Enum.GetName(typeof(StiPropertyType), name));
		}
		if (obj is IStiList)
		{
			StiPropertyType stiPropertyType3 = (StiPropertyType)name;
			if (stiPropertyType3 == StiPropertyType.Count)
			{
				return (obj as IStiList).Count;
			}
			ThrowError(ParserErrorCode.ItemDoesNotContainDefinition, GetTypeName(argsList[0]), Enum.GetName(typeof(StiPropertyType), name));
		}
		if (obj is StiDataBand)
		{
			switch ((StiPropertyType)name)
			{
			case StiPropertyType.Count:
				return (obj as StiDataBand).Count;
			case StiPropertyType.SelectedLine:
				return (obj as StiDataBand).SelectedLine;
			case StiPropertyType.Position:
				return (obj as StiDataBand).Position;
			case StiPropertyType.Line:
				return (obj as StiDataBand).Line;
			case StiPropertyType.Enabled:
				return (obj as StiDataBand).Enabled;
			}
			ThrowError(ParserErrorCode.ItemDoesNotContainDefinition, GetTypeName(argsList[0]), Enum.GetName(typeof(StiPropertyType), name));
		}
		if (obj is StiDataSource)
		{
			switch ((StiPropertyType)name)
			{
			case StiPropertyType.Count:
				return (obj as StiDataSource).Count;
			case StiPropertyType.Position:
				return (obj as StiDataSource).Position;
			case StiPropertyType.Rows:
				return (obj as StiDataSource).Rows;
			}
			ThrowError(ParserErrorCode.ItemDoesNotContainDefinition, GetTypeName(argsList[0]), Enum.GetName(typeof(StiPropertyType), name));
		}
		if (obj is StiBusinessObject)
		{
			switch ((StiPropertyType)name)
			{
			case StiPropertyType.BusinessObjectValue:
				return (obj as StiBusinessObject).BusinessObjectValue;
			case StiPropertyType.Position:
				return (obj as StiBusinessObject).Position;
			}
			ThrowError(ParserErrorCode.ItemDoesNotContainDefinition, GetTypeName(argsList[0]), Enum.GetName(typeof(StiPropertyType), name));
		}
		if (obj is StiPage)
		{
			StiPropertyType stiPropertyType4 = (StiPropertyType)name;
			if (stiPropertyType4 == StiPropertyType.Skip)
			{
				return (obj as StiPage).Skip;
			}
		}
		if (obj is StiComponent)
		{
			switch ((StiPropertyType)name)
			{
			case StiPropertyType.Name:
				return (obj as StiComponent).Name;
			case StiPropertyType.TagValue:
				return (obj as StiComponent).TagValue;
			case StiPropertyType.Enabled:
				return (obj as StiComponent).Enabled;
			}
		}
		if (obj is StiComboBoxControl)
		{
			switch ((StiPropertyType)name)
			{
			case StiPropertyType.SelectedIndex:
				return (obj as StiComboBoxControl).SelectedIndex;
			case StiPropertyType.SelectedItem:
				return (obj as StiComboBoxControl).SelectedItem;
			case StiPropertyType.SelectedValue:
				return (obj as StiComboBoxControl).SelectedValue;
			case StiPropertyType.Text:
				return (obj as StiComboBoxControl).Text;
			}
		}
		if (obj is StiDateTimePickerControl)
		{
			StiPropertyType stiPropertyType5 = (StiPropertyType)name;
			if (stiPropertyType5 == StiPropertyType.Value)
			{
				return (obj as StiDateTimePickerControl).Value;
			}
		}
		return null;
	}

	private void ThrowError(ParserErrorCode code)
	{
		ThrowError(code, null, string.Empty, string.Empty, string.Empty, string.Empty);
	}

	private void ThrowError(ParserErrorCode code, string message1)
	{
		ThrowError(code, null, message1, string.Empty, string.Empty, string.Empty);
	}

	private void ThrowError(ParserErrorCode code, string message1, string message2)
	{
		ThrowError(code, null, message1, message2, string.Empty, string.Empty);
	}

	private void ThrowError(ParserErrorCode code, string message1, string message2, string message3)
	{
		ThrowError(code, null, message1, message2, message3, string.Empty);
	}

	private void ThrowError(ParserErrorCode code, string message1, string message2, string message3, string message4)
	{
		ThrowError(code, null, message1, message2, message3, message4);
	}

	private void ThrowError(ParserErrorCode code, StiToken token)
	{
		ThrowError(code, token, string.Empty, string.Empty, string.Empty, string.Empty);
	}

	private void ThrowError(ParserErrorCode code, StiToken token, string message1)
	{
		ThrowError(code, token, message1, string.Empty, string.Empty, string.Empty);
	}

	private void ThrowError(ParserErrorCode code, StiToken token, string message1, string message2)
	{
		ThrowError(code, token, message1, message2, string.Empty, string.Empty);
	}

	private void ThrowError(ParserErrorCode code, StiToken token, string message1, string message2, string message3)
	{
		ThrowError(code, token, message1, message2, message3, string.Empty);
	}

	private void ThrowError(ParserErrorCode code, StiToken token, string message1, string message2, string message3, string message4)
	{
		string text = "Unknown error";
		if ((int)code < errorsList.Length)
		{
			text = string.Format(errorsList[(int)code], message1, message2, message3, message4);
		}
		string message5 = "Parser error: " + text;
		StiParserException ex = new StiParserException(message5);
		ex.BaseMessage = text;
		if (token != null)
		{
			ex.Position = expressionPosition + token.Position;
			ex.Length = token.Length;
		}
		throw ex;
	}

	private void CheckTypes(List<StiAsmCommand> asmList)
	{
		if (asmList == null || asmList.Count == 0)
		{
			return;
		}
		Stack<StiCheckTypeData> stack = new Stack<StiCheckTypeData>();
		List<StiCheckTypeData> list = null;
		Type[] array = null;
		foreach (StiAsmCommand asm in asmList)
		{
			Type typeCode = typeof(object);
			switch (asm.Type)
			{
			case StiAsmCommandType.PushValue:
				stack.Push(new StiCheckTypeData((asm.Parameter1 == null) ? typeof(object) : asm.Parameter1.GetType(), asm.Position, asm.Length));
				break;
			case StiAsmCommandType.PushVariable:
			case StiAsmCommandType.PushRefVariable:
			{
				string text3 = (string)asm.Parameter1;
				StiVariable stiVariable = report.Dictionary.Variables[text3];
				if (stiVariable != null)
				{
					typeCode = stiVariable.Type;
				}
				else if (report.Variables != null && report.Variables.ContainsKey(text3))
				{
					object obj2 = report.Variables[text3];
					if (obj2 != null)
					{
						typeCode = obj2.GetType();
					}
				}
				stack.Push(new StiCheckTypeData(typeCode, asm.Position, asm.Length));
				break;
			}
			case StiAsmCommandType.PushSystemVariable:
			{
				object obj = get_systemVariable(asm.Parameter1);
				if (obj != null)
				{
					typeCode = obj.GetType();
				}
				stack.Push(new StiCheckTypeData(typeCode, asm.Position, asm.Length));
				break;
			}
			case StiAsmCommandType.PushComponent:
				stack.Push(new StiCheckTypeData((asm.Parameter1 == null) ? typeof(object) : asm.Parameter1.GetType(), asm.Position, asm.Length));
				break;
			case StiAsmCommandType.CopyToVariable:
				stack.Peek();
				break;
			case StiAsmCommandType.PushFunction:
			{
				list = new List<StiCheckTypeData>();
				for (int j = 0; j < asm.ArgsCount; j++)
				{
					list.Add(stack.Pop());
				}
				list.Reverse();
				array = new Type[list.Count];
				for (int k = 0; k < list.Count; k++)
				{
					array[k] = list[k].TypeCode;
				}
				StiParserMethodInfo parserMethodInfo2 = GetParserMethodInfo((StiFunctionType)asm.Parameter1, array);
				typeCode = ((parserMethodInfo2 != null) ? parserMethodInfo2.ReturnType : typeof(object));
				stack.Push(new StiCheckTypeData(typeCode, asm.Position, asm.Length));
				break;
			}
			case StiAsmCommandType.PushMethod:
			{
				list = new List<StiCheckTypeData>();
				for (int m = 0; m < asm.ArgsCount; m++)
				{
					list.Add(stack.Pop());
				}
				list.Reverse();
				array = new Type[list.Count];
				for (int n = 0; n < list.Count; n++)
				{
					array[n] = list[n].TypeCode;
				}
				typeCode = GetMethodResultType((StiMethodType)asm.Parameter1, array);
				stack.Push(new StiCheckTypeData(typeCode, asm.Position, asm.Length));
				break;
			}
			case StiAsmCommandType.PushProperty:
				typeCode = GetPropertyType(asm.Parameter1.ToString(), stack.Pop().TypeCode);
				stack.Push(new StiCheckTypeData(typeCode, asm.Position, asm.Length));
				break;
			case StiAsmCommandType.PushDataSourceField:
			{
				List<string> list2 = new List<string>(((string)asm.Parameter1).Split(new char[1] { '.' }));
				StiDataSource stiDataSource = report.Dictionary.DataSources[list2[0]];
				if (stiDataSource is StiVirtualSource && list2.Count > 2 && !stiDataSource.Columns.Contains(list2[1]))
				{
					string text = list2[1] + "." + list2[2];
					if (stiDataSource.Columns.Contains(text))
					{
						list2[1] = text;
						list2.RemoveAt(2);
					}
					else if (list2.Count > 3)
					{
						text = text + "." + list2[3];
						if (stiDataSource.Columns.Contains(text))
						{
							list2[1] = text;
							list2.RemoveAt(2);
							list2.RemoveAt(2);
						}
					}
				}
				if (list2.Count > 1)
				{
					if (list2.Count == 2)
					{
						StiDataColumn stiDataColumn = stiDataSource.Columns[list2[1]];
						if (stiDataColumn != null)
						{
							typeCode = stiDataColumn.Type;
						}
					}
					else
					{
						string relationName = list2[1];
						stiDataSource = stiDataSource.GetParentDataSource(relationName);
						int i;
						for (i = 2; i < list2.Count - 1; i++)
						{
							relationName = list2[i];
							stiDataSource = stiDataSource.GetParentDataSource(relationName);
						}
						StiDataColumn stiDataColumn2 = stiDataSource.Columns[list2[i]];
						if (stiDataColumn2 != null)
						{
							typeCode = stiDataColumn2.Type;
						}
					}
				}
				else
				{
					typeCode = stiDataSource.GetType();
				}
				stack.Push(new StiCheckTypeData(typeCode, asm.Position, asm.Length));
				break;
			}
			case StiAsmCommandType.PushBusinessObjectField:
			{
				string[] array2 = ((string)asm.Parameter1).Split(new char[1] { '.' });
				StiBusinessObject stiBusinessObject = report.Dictionary.BusinessObjects[array2[0]];
				if (array2.Length > 1)
				{
					string text2 = null;
					int l;
					for (l = 1; l < array2.Length - 1; l++)
					{
						text2 = array2[l];
						if (stiBusinessObject.Columns.Contains(text2))
						{
							break;
						}
						stiBusinessObject = stiBusinessObject.BusinessObjects[text2];
					}
					typeCode = ((!stiBusinessObject.Columns.Contains(array2[l])) ? stiBusinessObject.BusinessObjects[array2[l]].GetType() : stiBusinessObject.Columns[array2[l]].Type);
				}
				else
				{
					typeCode = stiBusinessObject.GetType();
				}
				stack.Push(new StiCheckTypeData(typeCode, asm.Position, asm.Length));
				break;
			}
			case StiAsmCommandType.PushArrayElement:
			{
				list = new List<StiCheckTypeData>();
				for (int num = 0; num < (int)asm.Parameter1; num++)
				{
					list.Add(stack.Pop());
				}
				list.Reverse();
				array = new Type[list.Count];
				for (int num2 = 0; num2 < list.Count; num2++)
				{
					array[num2] = list[num2].TypeCode;
				}
				typeCode = GetArrayElementType(array);
				stack.Push(new StiCheckTypeData(typeCode, asm.Position, asm.Length));
				break;
			}
			case StiAsmCommandType.Add:
			case StiAsmCommandType.Sub:
			case StiAsmCommandType.Mult:
			case StiAsmCommandType.Div:
			case StiAsmCommandType.Mod:
			case StiAsmCommandType.Shl:
			case StiAsmCommandType.Shr:
			case StiAsmCommandType.And:
			case StiAsmCommandType.And2:
			case StiAsmCommandType.Or:
			case StiAsmCommandType.Or2:
			case StiAsmCommandType.Xor:
			{
				StiCheckTypeData stiCheckTypeData2 = stack.Pop();
				StiCheckTypeData stiCheckTypeData = stack.Pop();
				array = new Type[2] { stiCheckTypeData.TypeCode, stiCheckTypeData2.TypeCode };
				StiParserMethodInfo parserMethodInfo3 = GetParserMethodInfo((StiFunctionType)asm.Type, array);
				typeCode = ((parserMethodInfo3 != null) ? parserMethodInfo3.ReturnType : typeof(object));
				stack.Push(new StiCheckTypeData(typeCode, asm.Position, asm.Length));
				break;
			}
			case StiAsmCommandType.Neg:
			case StiAsmCommandType.Not:
			{
				StiCheckTypeData stiCheckTypeData = stack.Pop();
				array = new Type[1] { stiCheckTypeData.TypeCode };
				StiParserMethodInfo parserMethodInfo = GetParserMethodInfo((StiFunctionType)asm.Type, array);
				typeCode = ((parserMethodInfo != null) ? parserMethodInfo.ReturnType : typeof(object));
				stack.Push(new StiCheckTypeData(typeCode, asm.Position, asm.Length));
				break;
			}
			case StiAsmCommandType.CompareLeft:
			case StiAsmCommandType.CompareLeftEqual:
			case StiAsmCommandType.CompareRight:
			case StiAsmCommandType.CompareRightEqual:
			case StiAsmCommandType.CompareEqual:
			case StiAsmCommandType.CompareNotEqual:
			{
				StiCheckTypeData stiCheckTypeData2 = stack.Pop();
				StiCheckTypeData stiCheckTypeData = stack.Pop();
				typeCode = typeof(bool);
				stack.Push(new StiCheckTypeData(typeCode, asm.Position, asm.Length));
				break;
			}
			case StiAsmCommandType.Cast:
			{
				StiCheckTypeData stiCheckTypeData = stack.Pop();
				typeCode = Type.GetType("System." + asm.Parameter1);
				stack.Push(new StiCheckTypeData(typeCode, asm.Position, asm.Length));
				break;
			}
			}
		}
	}

	private Type GetMethodResultType(StiMethodType type, Type[] args)
	{
		Type type2 = args[0];
		int num = args.Length - 1;
		string text = type.ToString();
		MethodInfo[] methods = type2.GetMethods();
		bool flag = false;
		bool flag2 = false;
		MethodInfo[] array = methods;
		foreach (MethodInfo methodInfo in array)
		{
			if (methodInfo.Name != text)
			{
				continue;
			}
			flag = true;
			ParameterInfo[] array2 = methodInfo.GetParameters();
			if (array2.Length != num)
			{
				if (num == 0 && array2.Length == 1)
				{
					object[] customAttributes = array2[0].GetCustomAttributes(typeof(ParamArrayAttribute), inherit: false);
					if (customAttributes.Length == 1)
					{
						return methodInfo.ReturnType;
					}
				}
				continue;
			}
			flag2 = true;
			bool flag3 = true;
			for (int j = 0; j < num; j++)
			{
				if (!IsImplicitlyCastableTo(args[j + 1], array2[j].ParameterType))
				{
					flag3 = false;
					break;
				}
			}
			if (flag3)
			{
				return methodInfo.ReturnType;
			}
		}
		if (!flag)
		{
			ThrowError(ParserErrorCode.FieldMethodOrPropertyNotFound, Enum.GetName(typeof(StiMethodType), type));
		}
		if (!flag2)
		{
			ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, Enum.GetName(typeof(StiMethodType), type), num.ToString());
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int k = 0; k < num; k++)
		{
			stringBuilder.Append((args[k + 1].Namespace == "System") ? args[k + 1].Name : args[k + 1].ToString());
			if (k < num - 1)
			{
				stringBuilder.Append(",");
			}
		}
		ThrowError(ParserErrorCode.NoMatchingOverloadedMethod, Enum.GetName(typeof(StiMethodType), type), stringBuilder.ToString());
		return typeof(object);
	}

	private Type GetPropertyType(string propertyName, Type baseType)
	{
		PropertyInfo[] properties = baseType.GetProperties();
		PropertyInfo[] array = properties;
		foreach (PropertyInfo propertyInfo in array)
		{
			if (propertyInfo.Name == propertyName)
			{
				return propertyInfo.PropertyType;
			}
		}
		FieldInfo[] fields = baseType.GetFields();
		FieldInfo[] array2 = fields;
		foreach (FieldInfo fieldInfo in array2)
		{
			if (fieldInfo.IsPublic && fieldInfo.Name == propertyName)
			{
				return fieldInfo.FieldType;
			}
		}
		ThrowError(ParserErrorCode.FieldMethodOrPropertyNotFound, propertyName);
		return typeof(object);
	}

	private Type GetArrayElementType(Type[] argsList)
	{
		Type type = argsList[0];
		if (argsList.Length < 2)
		{
			ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "get_ArrayElement", (argsList.Length - 1).ToString());
		}
		if (type == typeof(string))
		{
			if (argsList.Length != 2)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "string.get_Item", (argsList.Length - 1).ToString());
			}
			if (!IsImplicitlyCastableTo(argsList[1].GetType(), typeof(int)))
			{
				ThrowError(ParserErrorCode.NoMatchingOverloadedMethod, "string.get_Item", argsList[1].GetType().ToString());
			}
			return typeof(char);
		}
		PropertyInfo property = type.GetProperty("Item");
		if (property != null)
		{
			ParameterInfo[] array = property.GetGetMethod().GetParameters();
			if (array.Length != 0)
			{
				if (argsList.Length < 2)
				{
					ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "object.get_Item", (argsList.Length - 1).ToString());
				}
				return property.PropertyType;
			}
		}
		else
		{
			if (type.IsAssignableFrom(typeof(Array)))
			{
				return type.UnderlyingSystemType;
			}
			if (type.IsAssignableFrom(typeof(IList)))
			{
				return type.UnderlyingSystemType;
			}
		}
		return typeof(object);
	}

	public static bool IsImplicitlyCastableTo(Type from, Type to)
	{
		if (from == to)
		{
			return true;
		}
		if (from == null || to == null)
		{
			return false;
		}
		if (to.IsAssignableFrom(from))
		{
			return true;
		}
		KeyValuePair<Type, Type> key = new KeyValuePair<Type, Type>(from, to);
		if (ImplicitCastCache.TryGetValue(key, out var value))
		{
			return value;
		}
		if (typeof(IStiList).IsAssignableFrom(to))
		{
			if (typeof(IStiList).IsAssignableFrom(from))
			{
				return from.FullName == to.FullName;
			}
			return false;
		}
		if (typeof(IStiList).IsAssignableFrom(from))
		{
			return false;
		}
		bool flag = false;
		try
		{
			GetMethod(() => AttemptImplicitCast<object, object>()).GetGenericMethodDefinition().MakeGenericMethod(from, to).Invoke(null, new object[0]);
			flag = true;
		}
		catch
		{
		}
		ImplicitCastCache[key] = flag;
		return flag;
	}

	private static void AttemptImplicitCast<TFrom, TTo>()
	{
		List<TTo> arg = new List<TTo>(1);
		CallSiteBinder binder = Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(StiParser), new CSharpArgumentInfo[2]
		{
			CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
			CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
		});
		CallSite<Action<CallSite, object, TFrom>> callSite = CallSite<Action<CallSite, object, TFrom>>.Create(binder);
		callSite.Target(callSite, arg, default(TFrom));
	}

	private static MethodInfo GetMethod(Expression<Action> expression)
	{
		if (expression == null)
		{
			throw new ArgumentNullException("expression");
		}
		Expression body = expression.Body;
		if (body.NodeType != ExpressionType.Call)
		{
			throw new ArgumentException("expression.Body must be a Call expression.", "expression");
		}
		MethodCallExpression methodCallExpression = (MethodCallExpression)body;
		return methodCallExpression.Method;
	}

	private string GetTypeName(object value)
	{
		if (value == null)
		{
			return "null";
		}
		return value.GetType().ToString();
	}

	public object ExecuteAsm(object objectAsmList)
	{
		if (!(objectAsmList is List<StiAsmCommand> { Count: not 0 } list))
		{
			return null;
		}
		Stack stack = new Stack();
		ArrayList arrayList = null;
		object obj = 0;
		object obj2 = 0;
		for (int i = 0; i < list.Count; i++)
		{
			StiAsmCommand stiAsmCommand = list[i];
			switch (stiAsmCommand.Type)
			{
			case StiAsmCommandType.PushValue:
				stack.Push(stiAsmCommand.Parameter1);
				break;
			case StiAsmCommandType.PushVariable:
				stack.Push(getVariableValue((string)stiAsmCommand.Parameter1));
				break;
			case StiAsmCommandType.PushRefVariable:
				stack.Push(new StiRefVariableObject((string)stiAsmCommand.Parameter1, getVariableValue((string)stiAsmCommand.Parameter1)));
				break;
			case StiAsmCommandType.PushSystemVariable:
				stack.Push(get_systemVariable(stiAsmCommand.Parameter1));
				break;
			case StiAsmCommandType.PushComponent:
				stack.Push(stiAsmCommand.Parameter1);
				break;
			case StiAsmCommandType.CopyToVariable:
				report[(string)stiAsmCommand.Parameter1] = stack.Peek();
				break;
			case StiAsmCommandType.PushFunction:
			{
				arrayList = new ArrayList();
				for (int j = 0; j < stiAsmCommand.ArgsCount; j++)
				{
					arrayList.Add(stack.Pop());
				}
				if (stiAsmCommand.ArgsCount > 1)
				{
					arrayList.Reverse();
				}
				stack.Push(call_func(stiAsmCommand.Parameter1, arrayList, stiAsmCommand));
				break;
			}
			case StiAsmCommandType.PushMethod:
			{
				arrayList = new ArrayList();
				for (int m = 0; m < stiAsmCommand.ArgsCount; m++)
				{
					arrayList.Add(stack.Pop());
				}
				if (stiAsmCommand.ArgsCount > 1)
				{
					arrayList.Reverse();
				}
				stack.Push(call_method(stiAsmCommand.Parameter1, arrayList, stiAsmCommand));
				break;
			}
			case StiAsmCommandType.PushProperty:
				arrayList = new ArrayList();
				arrayList.Add(stack.Pop());
				stack.Push(call_property(stiAsmCommand.Parameter1, arrayList));
				break;
			case StiAsmCommandType.PushDataSourceField:
				stack.Push(getDataSourceField(stiAsmCommand));
				break;
			case StiAsmCommandType.PushBusinessObjectField:
			{
				string[] array = ((string)stiAsmCommand.Parameter1).Split(new char[1] { '.' });
				StiBusinessObject stiBusinessObject = report.Dictionary.BusinessObjects[array[0]];
				if (array.Length > 1)
				{
					string text = null;
					int l;
					for (l = 1; l < array.Length - 1; l++)
					{
						text = array[l];
						if (stiBusinessObject.Columns.Contains(text))
						{
							break;
						}
						stiBusinessObject = stiBusinessObject.BusinessObjects[text];
					}
					StiDataColumn stiDataColumn = stiBusinessObject.Columns[array[l]];
					if (stiDataColumn != null)
					{
						if (stiDataColumn is StiCalcDataColumn)
						{
							stack.Push(ParseTextValue("{" + (stiDataColumn as StiCalcDataColumn).Expression + "}", component));
						}
						else
						{
							stack.Push(stiBusinessObject[array[l]]);
						}
					}
					else
					{
						stack.Push(stiBusinessObject.BusinessObjects[array[l]]);
					}
				}
				else
				{
					stack.Push(stiBusinessObject);
				}
				break;
			}
			case StiAsmCommandType.PushArrayElement:
			{
				arrayList = new ArrayList();
				for (int k = 0; k < (int)stiAsmCommand.Parameter1; k++)
				{
					arrayList.Add(stack.Pop());
				}
				arrayList.Reverse();
				stack.Push(call_arrayElement(arrayList));
				break;
			}
			case StiAsmCommandType.Jump:
				i += Convert.ToInt32(stiAsmCommand.Parameter1);
				break;
			case StiAsmCommandType.JumpTrue:
				if (Convert.ToBoolean(stack.Pop()))
				{
					i += Convert.ToInt32(stiAsmCommand.Parameter1);
				}
				break;
			case StiAsmCommandType.JumpFalse:
				if (!Convert.ToBoolean(stack.Pop()))
				{
					i += Convert.ToInt32(stiAsmCommand.Parameter1);
				}
				break;
			case StiAsmCommandType.Add:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_Add(obj, obj2));
				break;
			case StiAsmCommandType.Sub:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_Sub(obj, obj2));
				break;
			case StiAsmCommandType.Mult:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_Mult(obj, obj2));
				break;
			case StiAsmCommandType.Div:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_Div(obj, obj2));
				break;
			case StiAsmCommandType.Mod:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_Mod(obj, obj2));
				break;
			case StiAsmCommandType.Power:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_Pow(obj, obj2));
				break;
			case StiAsmCommandType.Neg:
				obj = stack.Pop();
				stack.Push(op_Neg(obj));
				break;
			case StiAsmCommandType.Cast:
				obj = stack.Pop();
				obj2 = stiAsmCommand.Parameter1;
				stack.Push(op_Cast(obj, obj2));
				break;
			case StiAsmCommandType.Not:
				obj = stack.Pop();
				stack.Push(op_Not(obj));
				break;
			case StiAsmCommandType.CompareLeft:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_CompareLeft(obj, obj2));
				break;
			case StiAsmCommandType.CompareLeftEqual:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_CompareLeftEqual(obj, obj2));
				break;
			case StiAsmCommandType.CompareRight:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_CompareRight(obj, obj2));
				break;
			case StiAsmCommandType.CompareRightEqual:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_CompareRightEqual(obj, obj2));
				break;
			case StiAsmCommandType.CompareEqual:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_CompareEqual(obj, obj2));
				break;
			case StiAsmCommandType.CompareNotEqual:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_CompareNotEqual(obj, obj2));
				break;
			case StiAsmCommandType.Shl:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_Shl(obj, obj2));
				break;
			case StiAsmCommandType.Shr:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_Shr(obj, obj2));
				break;
			case StiAsmCommandType.And:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_And(obj, obj2));
				break;
			case StiAsmCommandType.Or:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_Or(obj, obj2));
				break;
			case StiAsmCommandType.Xor:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_Xor(obj, obj2));
				break;
			case StiAsmCommandType.And2:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_And2(obj, obj2));
				break;
			case StiAsmCommandType.Or2:
				obj2 = stack.Pop();
				obj = stack.Pop();
				stack.Push(op_Or2(obj, obj2));
				break;
			}
		}
		return stack.Pop();
	}

	private object getDataSourceField(StiAsmCommand asmCommand)
	{
		if (asmCommand.Parameter2 is StiParserDataSourceFieldInfo stiParserDataSourceFieldInfo)
		{
			if (getDataFieldValue != null)
			{
				StiParserGetDataFieldValueEventArgs stiParserGetDataFieldValueEventArgs = new StiParserGetDataFieldValueEventArgs(stiParserDataSourceFieldInfo.Path[0], stiParserDataSourceFieldInfo.Path[1]);
				getDataFieldValue(report, stiParserGetDataFieldValueEventArgs);
				if (stiParserGetDataFieldValueEventArgs.Processed)
				{
					return stiParserGetDataFieldValueEventArgs.Value;
				}
			}
			StiDataSource stiDataSource = stiParserDataSourceFieldInfo.Objects[0] as StiDataSource;
			if (asmCommand.ArgsCount == 1)
			{
				return stiDataSource;
			}
			if (asmCommand.ArgsCount == 2)
			{
				if (stiParserDataSourceFieldInfo.Objects[1] is StiDataRelation)
				{
					return (stiParserDataSourceFieldInfo.Objects[1] as StiDataRelation).ParentSource;
				}
				StiDataColumn stiDataColumn = stiParserDataSourceFieldInfo.Objects[1] as StiDataColumn;
				if (stiDataColumn != null && stiDataColumn is StiCalcDataColumn)
				{
					return ParseTextValue("{" + (stiDataColumn as StiCalcDataColumn).Expression + "}", component);
				}
				return StiReport.ChangeType(stiDataSource.GetData(stiParserDataSourceFieldInfo.Path[1]), stiDataColumn.Type, report.ConvertNulls);
			}
			string relation = stiParserDataSourceFieldInfo.Path[1];
			StiDataRow parentData = stiDataSource.GetParentData(relation);
			int i;
			for (i = 2; i < asmCommand.ArgsCount - 1; i++)
			{
				relation = stiParserDataSourceFieldInfo.Path[i];
				parentData = parentData.GetParentData(relation);
			}
			if (stiParserDataSourceFieldInfo.Objects[i] is StiDataRelation)
			{
				return (stiParserDataSourceFieldInfo.Objects[i] as StiDataRelation).ParentSource;
			}
			StiDataColumn stiDataColumn2 = stiParserDataSourceFieldInfo.Objects[i] as StiDataColumn;
			if (stiDataColumn2 != null && stiDataColumn2 is StiCalcDataColumn)
			{
				return ParseTextValue("{" + (stiDataColumn2 as StiCalcDataColumn).Expression + "}", component);
			}
			object value = null;
			if (parentData != null)
			{
				value = parentData[stiParserDataSourceFieldInfo.Path[i]];
			}
			return StiReport.ChangeType(value, stiDataColumn2.Type, report.ConvertNulls);
		}
		List<string> list = asmCommand.Parameter2 as List<string>;
		if (list == null)
		{
			list = new List<string>(((string)asmCommand.Parameter1).Split(new char[1] { '.' }));
		}
		if (getDataFieldValue != null)
		{
			StiParserGetDataFieldValueEventArgs stiParserGetDataFieldValueEventArgs2 = new StiParserGetDataFieldValueEventArgs(list[0], list[1]);
			getDataFieldValue(report, stiParserGetDataFieldValueEventArgs2);
			if (stiParserGetDataFieldValueEventArgs2.Processed)
			{
				return stiParserGetDataFieldValueEventArgs2.Value;
			}
		}
		StiDataSource stiDataSource2 = report.Dictionary.DataSources[list[0]];
		if (stiDataSource2 is StiVirtualSource && list.Count > 2 && !stiDataSource2.Columns.Contains(list[1]))
		{
			string text = list[1] + "." + list[2];
			if (stiDataSource2.Columns.Contains(text))
			{
				list[1] = text;
				list.RemoveAt(2);
			}
			else if (list.Count > 3)
			{
				text = text + "." + list[3];
				if (stiDataSource2.Columns.Contains(text))
				{
					list[1] = text;
					list.RemoveAt(2);
					list.RemoveAt(2);
				}
			}
		}
		if (list.Count == 1)
		{
			return stiDataSource2;
		}
		if (list.Count == 2)
		{
			StiDataColumn stiDataColumn3 = stiDataSource2.Columns[list[1]];
			if (stiDataColumn3 == null)
			{
				StiDataSource parentDataSource = stiDataSource2.GetParentDataSource(list[1]);
				if (parentDataSource != null)
				{
					return parentDataSource;
				}
			}
			if (stiDataColumn3 != null && stiDataColumn3 is StiCalcDataColumn)
			{
				return ParseTextValue("{" + (stiDataColumn3 as StiCalcDataColumn).Expression + "}", component);
			}
			return StiReport.ChangeType(stiDataSource2.GetData(list[1]), stiDataColumn3.Type, report.ConvertNulls);
		}
		string text2 = list[1];
		StiDataRow parentData2 = stiDataSource2.GetParentData(text2);
		stiDataSource2 = stiDataSource2.GetParentDataSource(text2);
		int j;
		for (j = 2; j < list.Count - 1; j++)
		{
			text2 = list[j];
			parentData2 = parentData2.GetParentData(text2);
			stiDataSource2 = stiDataSource2.GetParentDataSource(text2);
		}
		StiDataColumn stiDataColumn4 = stiDataSource2.Columns[list[j]];
		if (stiDataColumn4 == null)
		{
			StiDataSource parentDataSource2 = stiDataSource2.GetParentDataSource(list[j]);
			if (parentDataSource2 != null)
			{
				return parentDataSource2;
			}
		}
		if (stiDataColumn4 != null && stiDataColumn4 is StiCalcDataColumn)
		{
			return ParseTextValue("{" + (stiDataColumn4 as StiCalcDataColumn).Expression + "}", component);
		}
		object value2 = null;
		if (parentData2 != null)
		{
			value2 = parentData2[list[j]];
		}
		return StiReport.ChangeType(value2, stiDataColumn4.Type, report.ConvertNulls);
	}

	private object getVariableValue(string name)
	{
		if (runtimeConstants != null && runtimeConstants.Count > 0 && runtimeConstantsHash.ContainsKey(name))
		{
			return runtimeConstantsHash[name];
		}
		StiReport stiReport = ((report.CompiledReport == null) ? report : report.CompiledReport);
		FieldInfo field = stiReport.GetType().GetField(name);
		if (field != null)
		{
			return field.GetValue(stiReport);
		}
		StiVariable stiVariable = stiReport.Dictionary.Variables[name];
		if (stiVariable != null && stiVariable.ReadOnly && stiVariable.InitBy == StiVariableInitBy.Expression)
		{
			if (CheckVariableRecursion(name))
			{
				return null;
			}
			StiText stiText = new StiText();
			stiText.Name = "**ReportVariables**";
			stiText.Page = report.Pages[0];
			object result = op_Cast(ParseTextValue("{" + stiVariable.Value + "}", stiText, parameters), stiVariable.Type);
			RemoveRecursionCheck(name);
			return result;
		}
		if (stiReport.Variables != null && stiReport.Variables.ContainsKey(name))
		{
			return stiReport.Variables[name];
		}
		if (stiVariable != null)
		{
			if (stiVariable.InitBy == StiVariableInitBy.Expression)
			{
				if (CheckVariableRecursion(name))
				{
					return null;
				}
				object result2 = op_Cast(PrepareVariableValue(stiVariable, stiReport, null, fillItems: false, parameters), stiVariable.Type);
				RemoveRecursionCheck(name);
				return result2;
			}
			return stiVariable.ValueObject;
		}
		return null;
	}

	private bool CheckVariableRecursion(string varName)
	{
		if (parameters.VariablesRecursionCheck == null)
		{
			parameters.VariablesRecursionCheck = new Hashtable();
		}
		if (parameters.VariablesRecursionCheck.ContainsKey(varName))
		{
			return true;
		}
		parameters.VariablesRecursionCheck[varName] = null;
		return false;
	}

	private void RemoveRecursionCheck(string varName)
	{
		parameters.VariablesRecursionCheck.Remove(varName);
	}

	private int get_category(object par)
	{
		if (par == null)
		{
			return -1;
		}
		Type type = par.GetType();
		if (type == typeof(string) || type == typeof(char))
		{
			return 1;
		}
		if (type == typeof(decimal))
		{
			return 2;
		}
		if (type == typeof(double) || type == typeof(float))
		{
			return 3;
		}
		if (type == typeof(ulong))
		{
			return 4;
		}
		if (type == typeof(long))
		{
			return 5;
		}
		if (type == typeof(uint) || type == typeof(ushort) || type == typeof(byte))
		{
			return 6;
		}
		if (type == typeof(int) || type == typeof(short) || type == typeof(sbyte))
		{
			return 7;
		}
		if (type == typeof(DateTime))
		{
			return 8;
		}
		if (type == typeof(bool))
		{
			return 9;
		}
		if (type == typeof(object))
		{
			return 0;
		}
		if (IsImplicitlyCastableTo(type, typeof(decimal)))
		{
			return 2;
		}
		if (IsImplicitlyCastableTo(type, typeof(double)))
		{
			return 3;
		}
		if (IsImplicitlyCastableTo(type, typeof(ulong)))
		{
			return 4;
		}
		if (IsImplicitlyCastableTo(type, typeof(long)))
		{
			return 5;
		}
		if (IsImplicitlyCastableTo(type, typeof(uint)))
		{
			return 6;
		}
		if (IsImplicitlyCastableTo(type, typeof(int)))
		{
			return 7;
		}
		return 0;
	}

	private int CheckParserMethodInfo(StiFunctionType type, ArrayList args)
	{
		int count = args.Count;
		Type[] array = new Type[count];
		for (int i = 0; i < count; i++)
		{
			if (args[i] == null)
			{
				array[i] = typeof(object);
			}
			else
			{
				array[i] = args[i].GetType();
			}
		}
		return GetParserMethodInfo(type, array)?.Number ?? 0;
	}

	private int CheckParserMethodInfo2(StiFunctionType type, ArrayList args, StiAsmCommand asmCommand)
	{
		int count = args.Count;
		Type[] array = new Type[count];
		for (int i = 0; i < count; i++)
		{
			if (args[i] == null)
			{
				array[i] = typeof(object);
			}
			else
			{
				array[i] = args[i].GetType();
			}
		}
		if (asmCommand.Parameter2 is StiParserCheckMethodInfo stiParserCheckMethodInfo)
		{
			bool flag = stiParserCheckMethodInfo.LastArgsTypes.Length == count;
			if (flag)
			{
				for (int num = count - 1; num >= 0; num--)
				{
					if (array[num] != stiParserCheckMethodInfo.LastArgsTypes[num])
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				return stiParserCheckMethodInfo.LastOverload;
			}
			stiParserCheckMethodInfo.LastOverload = GetParserMethodInfo(type, array)?.Number ?? 0;
			stiParserCheckMethodInfo.LastArgsTypes = array;
			return stiParserCheckMethodInfo.LastOverload;
		}
		int num2 = GetParserMethodInfo(type, array)?.Number ?? 0;
		asmCommand.Parameter2 = new StiParserCheckMethodInfo(num2, array);
		return num2;
	}

	public StiParserMethodInfo GetParserMethodInfo(StiFunctionType type, Type[] args)
	{
		object obj = MethodsHash[type];
		if (obj == null)
		{
			return null;
		}
		int num = args.Length;
		List<StiParserMethodInfo> list = (List<StiParserMethodInfo>)obj;
		bool flag = false;
		foreach (StiParserMethodInfo item in list)
		{
			if (item.Arguments.Length != num)
			{
				continue;
			}
			flag = true;
			bool flag2 = true;
			for (int i = 0; i < num; i++)
			{
				if (!IsImplicitlyCastableTo(args[i], item.Arguments[i]))
				{
					flag2 = false;
					break;
				}
			}
			if (flag2)
			{
				return item;
			}
		}
		if (!flag)
		{
			ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, Enum.GetName(typeof(StiFunctionType), type), num.ToString());
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int j = 0; j < num; j++)
		{
			stringBuilder.Append((!(args[j] is StiUndefinedType) && args[j].Namespace == "System") ? args[j].Name : args[j].ToString());
			if (j < num - 1)
			{
				stringBuilder.Append(",");
			}
		}
		ThrowError(ParserErrorCode.NoMatchingOverloadedMethod, Enum.GetName(typeof(StiFunctionType), type), stringBuilder.ToString());
		return null;
	}

	private object call_arrayElement(ArrayList argsList)
	{
		object obj = argsList[0];
		if (argsList.Count < 2)
		{
			ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "get_ArrayElement", (argsList.Count - 1).ToString());
		}
		if (obj is string)
		{
			if (argsList.Count != 2)
			{
				ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "string.get_Item", (argsList.Count - 1).ToString());
			}
			int index = Convert.ToInt32(argsList[1]);
			return (obj as string)[index];
		}
		PropertyInfo property = obj.GetType().GetProperty("Item");
		if (property != null)
		{
			if (property.GetGetMethod().GetParameters().Length != 0)
			{
				if (argsList.Count < 2)
				{
					ThrowError(ParserErrorCode.NoOverloadForMethodTakesNArguments, "object.get_Item", (argsList.Count - 1).ToString());
				}
				object[] array = new object[argsList.Count - 1];
				for (int i = 0; i < argsList.Count - 1; i++)
				{
					array[i] = argsList[i + 1];
				}
				return property.GetValue(obj, array);
			}
		}
		else
		{
			if (obj is Array)
			{
				int[] array2 = new int[argsList.Count - 1];
				for (int j = 0; j < argsList.Count - 1; j++)
				{
					array2[j] = Convert.ToInt32(argsList[j + 1]);
				}
				return (obj as Array).GetValue(array2);
			}
			if (obj is IList)
			{
				return (obj as IList)[Convert.ToInt32(argsList[1])];
			}
		}
		return null;
	}

	private object get_systemVariable(object name)
	{
		switch ((StiSystemVariableType)name)
		{
		case StiSystemVariableType.Column:
			return report.Column;
		case StiSystemVariableType.Line:
			return report.Line;
		case StiSystemVariableType.LineThrough:
			return report.LineThrough;
		case StiSystemVariableType.LineABC:
			return report.LineABC;
		case StiSystemVariableType.LineRoman:
			return report.LineRoman;
		case StiSystemVariableType.GroupLine:
			return report.GroupLine;
		case StiSystemVariableType.PageNumber:
			return report.PageNumber;
		case StiSystemVariableType.PageNumberThrough:
			return report.PageNumberThrough;
		case StiSystemVariableType.PageNofM:
			return report.PageNofM;
		case StiSystemVariableType.PageNofMThrough:
			return report.PageNofMThrough;
		case StiSystemVariableType.TotalPageCount:
			return report.TotalPageCount;
		case StiSystemVariableType.TotalPageCountThrough:
			return report.TotalPageCountThrough;
		case StiSystemVariableType.IsFirstPage:
			return report.IsFirstPage;
		case StiSystemVariableType.IsFirstPageThrough:
			return report.IsFirstPageThrough;
		case StiSystemVariableType.IsLastPage:
			return report.IsLastPage;
		case StiSystemVariableType.IsLastPageThrough:
			return report.IsLastPageThrough;
		case StiSystemVariableType.PageCopyNumber:
			return report.PageCopyNumber;
		case StiSystemVariableType.ReportAlias:
			return report.ReportAlias;
		case StiSystemVariableType.ReportAuthor:
			return report.ReportAuthor;
		case StiSystemVariableType.ReportChanged:
			return report.ReportChanged;
		case StiSystemVariableType.ReportCreated:
			return report.ReportCreated;
		case StiSystemVariableType.ReportDescription:
			return report.ReportDescription;
		case StiSystemVariableType.ReportName:
			return report.ReportName;
		case StiSystemVariableType.Time:
			return report.Time;
		case StiSystemVariableType.Today:
			return report.Today;
		case StiSystemVariableType.ConditionValue:
			return report.Engine?.LastInvokeTextProcessValueEventArgsValue;
		case StiSystemVariableType.ConditionValue2:
			return report.Engine?.LastInvokeTextProcessValueEventArgsValue;
		case StiSystemVariableType.ConditionTag:
			if (!(component is StiText))
			{
				return null;
			}
			return (component as StiText).TagValue;
		case StiSystemVariableType.Sender:
			return sender;
		case StiSystemVariableType.DateTimeNow:
			return DateTime.Now;
		case StiSystemVariableType.DateTimeToday:
			return DateTime.Today;
		case StiSystemVariableType.DateTimeUtcNow:
			return DateTime.UtcNow;
		default:
			return null;
		}
	}

	public static object ParseTextValue(string inputExpression, StiComponent component)
	{
		return ParseTextValue(inputExpression, component, component, new StiParserParameters());
	}

	public static object ParseTextValue(string inputExpression, StiComponent component, ref bool storeToPrint, bool executeIfStoreToPrint)
	{
		StiParserParameters stiParserParameters = new StiParserParameters
		{
			StoreToPrint = storeToPrint,
			ExecuteIfStoreToPrint = executeIfStoreToPrint
		};
		object result = ParseTextValue(inputExpression, component, component, stiParserParameters);
		storeToPrint = stiParserParameters.StoreToPrint;
		return result;
	}

	public static object ParseTextValue(string inputExpression, StiComponent component, ref bool storeToPrint, bool executeIfStoreToPrint, bool returnAsmList)
	{
		StiParserParameters stiParserParameters = new StiParserParameters
		{
			StoreToPrint = storeToPrint,
			ExecuteIfStoreToPrint = executeIfStoreToPrint,
			ReturnAsmList = returnAsmList
		};
		object result = ParseTextValue(inputExpression, component, component, stiParserParameters);
		storeToPrint = stiParserParameters.StoreToPrint;
		return result;
	}

	public static object ParseTextValue(string inputExpression, StiComponent component, object sender)
	{
		return ParseTextValue(inputExpression, component, sender, new StiParserParameters());
	}

	public static object ParseTextValue(string inputExpression, StiComponent component, object sender, ref bool storeToPrint, bool executeIfStoreToPrint)
	{
		StiParserParameters stiParserParameters = new StiParserParameters
		{
			StoreToPrint = storeToPrint,
			ExecuteIfStoreToPrint = executeIfStoreToPrint
		};
		object result = ParseTextValue(inputExpression, component, sender, stiParserParameters);
		storeToPrint = stiParserParameters.StoreToPrint;
		return result;
	}

	public static object ParseTextValue(string inputExpression, StiComponent component, object sender, ref bool storeToPrint, bool executeIfStoreToPrint, bool returnAsmList)
	{
		StiParserParameters stiParserParameters = new StiParserParameters
		{
			StoreToPrint = storeToPrint,
			ExecuteIfStoreToPrint = executeIfStoreToPrint,
			ReturnAsmList = returnAsmList
		};
		object result = ParseTextValue(inputExpression, component, sender, stiParserParameters);
		storeToPrint = stiParserParameters.StoreToPrint;
		return result;
	}

	public static object ParseTextValue(string inputExpression, StiComponent component, StiParserParameters parameters)
	{
		if (parameters == null)
		{
			parameters = new StiParserParameters();
		}
		return ParseTextValue(inputExpression, component, component, parameters);
	}

	public static object ParseTextValue(string inputExpression, StiComponent component, object sender, StiParserParameters parameters)
	{
		if (string.IsNullOrEmpty(inputExpression))
		{
			return null;
		}
		if (parameters.Parser == null)
		{
			parameters.Parser = new StiParser();
		}
		if (component.Report != null)
		{
			parameters.Parser.report = component.Report;
		}
		parameters.Parser.component = component;
		parameters.Parser.sender = sender;
		parameters.Parser.syntaxCaseSensitive = parameters.SyntaxCaseSensitive ?? (parameters.Parser.report.ScriptLanguage == StiReportLanguageType.CSharp || parameters.Parser.report.ScriptLanguage == StiReportLanguageType.JS);
		parameters.Parser.checkSyntaxMode = parameters.CheckSyntaxMode;
		parameters.Parser.getDataFieldValue = parameters.GetDataFieldValue;
		parameters.Parser.useAliases = parameters.UseAliases;
		parameters.Parser.runtimeConstants = parameters.Constants;
		if (parameters.Constants != null)
		{
			parameters.Parser.CreateRuntimeConstantsHash();
		}
		parameters.Parser.parameters = parameters;
		List<StiAsmCommand> list = null;
		string key = inputExpression + component.Name + parameters.GlobalizedNameExt;
		Hashtable hashtable = parameters.ConversionStore;
		if (hashtable == null)
		{
			StiEngine stiEngine = parameters.Parser.report?.Engine;
			if (stiEngine != null)
			{
				if (stiEngine.ParserConversionStore == null)
				{
					stiEngine.ParserConversionStore = new Hashtable();
				}
				hashtable = stiEngine.ParserConversionStore;
			}
			else
			{
				hashtable = new Hashtable();
			}
		}
		if (hashtable.Contains(key))
		{
			list = (List<StiAsmCommand>)hashtable[key];
		}
		if (list == null)
		{
			try
			{
				if (!parameters.IgnoreGlobalizedName && parameters.Parser.report?.GlobalizationManager != null && component is IStiGlobalizedName stiGlobalizedName && !string.IsNullOrWhiteSpace(stiGlobalizedName.GlobalizedName))
				{
					string @string = component.Report.GlobalizationManager.GetString(stiGlobalizedName.GlobalizedName + parameters.GlobalizedNameExt);
					if (@string != null)
					{
						inputExpression = @string;
					}
				}
			}
			catch
			{
			}
			if (component is StiText && (component as StiText).OnlyText)
			{
				list = new List<StiAsmCommand>();
				list.Add(new StiAsmCommand(StiAsmCommandType.PushValue, inputExpression));
			}
			else
			{
				try
				{
					list = new List<StiAsmCommand>();
					int num = 0;
					int i = 0;
					while (i < inputExpression.Length)
					{
						int num2 = i;
						for (; i < inputExpression.Length && inputExpression[i] != '{'; i++)
						{
						}
						if (i != num2)
						{
							if (num == 1)
							{
								list.Add(new StiAsmCommand(StiAsmCommandType.Cast, TypeCode.String));
							}
							list.Add(new StiAsmCommand(StiAsmCommandType.PushValue, inputExpression.Substring(num2, i - num2)));
							num++;
							if (num > 1)
							{
								list.Add(new StiAsmCommand(StiAsmCommandType.Add));
							}
						}
						if (i >= inputExpression.Length || inputExpression[i] != '{')
						{
							continue;
						}
						i++;
						num2 = i;
						bool flag = false;
						while (i < inputExpression.Length)
						{
							if (inputExpression[i] == '"')
							{
								i++;
								int j;
								for (j = i; j < inputExpression.Length && inputExpression[j] != '"'; j++)
								{
									if (inputExpression[j] == '\\')
									{
										j++;
									}
								}
								i = j + 1;
								continue;
							}
							if (inputExpression[i] == '}')
							{
								string text = inputExpression.Substring(num2, i - num2);
								if (text != null && text.Length > 0)
								{
									parameters.Parser.expressionPosition = num2;
									list.AddRange(parameters.Parser.ParseToAsm(text));
									num++;
									if (num > 1)
									{
										list.Add(new StiAsmCommand(StiAsmCommandType.Cast, TypeCode.String));
										list.Add(new StiAsmCommand(StiAsmCommandType.Add));
									}
								}
								flag = true;
								i++;
								break;
							}
							i++;
						}
						if (!flag)
						{
							parameters.Parser.expressionPosition = num2;
							list.Add(new StiAsmCommand(StiAsmCommandType.PushValue, inputExpression.Substring(num2 - 1)));
							num++;
							if (num > 1)
							{
								list.Add(new StiAsmCommand(StiAsmCommandType.Add));
							}
						}
					}
				}
				catch (Exception ex)
				{
					hashtable[key] = new List<StiAsmCommand>();
					throw ex;
				}
			}
			hashtable[key] = list;
		}
		if (parameters.ReturnAsmList)
		{
			return list;
		}
		if (list.Count > 0)
		{
			parameters.StoreToPrint = CheckForStoreToPrint(list, component);
			if (parameters.StoreToPrint && !parameters.ExecuteIfStoreToPrint)
			{
				return inputExpression;
			}
			return parameters.Parser.ExecuteAsm(list);
		}
		return null;
	}

	private List<StiAsmCommand> ParseToAsm(string inputExpression)
	{
		this.inputExpression = inputExpression;
		MakeTokensList();
		asmList = new List<StiAsmCommand>();
		eval_exp();
		return asmList;
	}

	private static bool CheckForStoreToPrint(object objAsmList, StiComponent component)
	{
		if (component is StiText { ProcessAt: StiProcessAt.EndOfReport })
		{
			return true;
		}
		bool result = false;
		if (objAsmList is List<StiAsmCommand> list)
		{
			foreach (StiAsmCommand item in list)
			{
				if (item.Type == StiAsmCommandType.PushSystemVariable)
				{
					StiSystemVariableType stiSystemVariableType = (StiSystemVariableType)item.Parameter1;
					if (stiSystemVariableType == StiSystemVariableType.PageNumber || stiSystemVariableType == StiSystemVariableType.PageNumberThrough || stiSystemVariableType == StiSystemVariableType.TotalPageCount || stiSystemVariableType == StiSystemVariableType.TotalPageCountThrough || stiSystemVariableType == StiSystemVariableType.PageNofM || stiSystemVariableType == StiSystemVariableType.PageNofMThrough || stiSystemVariableType == StiSystemVariableType.IsFirstPage || stiSystemVariableType == StiSystemVariableType.IsFirstPageThrough || stiSystemVariableType == StiSystemVariableType.IsLastPage || stiSystemVariableType == StiSystemVariableType.IsLastPageThrough)
					{
						result = true;
						break;
					}
				}
				if (item.Type == StiAsmCommandType.PushFunction)
				{
					StiFunctionType stiFunctionType = (StiFunctionType)item.Parameter1;
					if ((stiFunctionType >= StiFunctionType.pCount && stiFunctionType <= StiFunctionType.pLast) || (stiFunctionType >= StiFunctionType.prCount && stiFunctionType <= StiFunctionType.prLast) || (stiFunctionType >= StiFunctionType.piCount && stiFunctionType <= StiFunctionType.piLast) || (stiFunctionType >= StiFunctionType.priCount && stiFunctionType <= StiFunctionType.priLast) || stiFunctionType == StiFunctionType.GetAnchorPageNumber || stiFunctionType == StiFunctionType.GetAnchorPageNumberThrough)
					{
						result = true;
						break;
					}
				}
			}
		}
		return result;
	}

	public static StiParserException CheckExpression(string inputExpression, StiComponent component, bool useAliases = false)
	{
		try
		{
			StiParserParameters stiParserParameters = new StiParserParameters
			{
				ExecuteIfStoreToPrint = false,
				ReturnAsmList = true,
				CheckSyntaxMode = true,
				UseAliases = useAliases
			};
			object obj = ParseTextValue(inputExpression, component, stiParserParameters);
			if (obj is List<StiAsmCommand> list)
			{
				stiParserParameters.Parser.CheckTypes(list);
			}
		}
		catch (StiParserException result)
		{
			return result;
		}
		return null;
	}

	internal static void CheckForDataBandsUsedInPageTotals(StiText stiText, StiReport report = null)
	{
		try
		{
			StiParserParameters stiParserParameters = new StiParserParameters
			{
				ReturnAsmList = true,
				Parser = new StiParser()
			};
			if (stiParserParameters.Parser.report == null)
			{
				stiParserParameters.Parser.report = report;
			}
			object obj = ParseTextValue(stiText.Text.Value, stiText, stiText, stiParserParameters);
		}
		catch (Exception ex)
		{
			string text = $"Expression in Text property of '{stiText.Name}' can't be evaluated! {ex.Message}";
			StiLogService.Write(((object)stiText).GetType(), text);
			StiLogService.Write(((object)stiText).GetType(), ex.Message);
			stiText.Report.WriteToReportRenderingMessages(text);
		}
	}

	public static void PrepareReportVariables(StiReport report)
	{
		report.Variables = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
		if (report.CalculationMode == StiCalculationMode.Compilation)
		{
			return;
		}
		StiText textBox = new StiText
		{
			Name = "**ReportVariables**",
			Page = report.Pages[0]
		};
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			try
			{
				PrepareVariableValue(variable, report, textBox);
			}
			catch
			{
			}
		}
		report.ModifiedVariables.Clear();
	}

	public static object PrepareVariableValue(StiVariable var, StiReport report, StiText textBox = null, bool fillItems = false, StiParserParameters parameters = null)
	{
		if (textBox == null)
		{
			textBox = new StiText
			{
				Name = "**ReportVariables**",
				Page = report.Pages[0]
			};
		}
		if (var.DialogInfo.ItemsInitializationType == StiItemsInitializationType.Columns)
		{
			var.DialogInfo.Keys = new string[0];
			var.DialogInfo.Values = new string[0];
			var.DialogInfo.CheckedStates = new bool[0];
			var.DialogInfo.ValuesBindingList = new List<object>[0];
		}
		Hashtable hashtable = null;
		if (fillItems)
		{
			hashtable = new Hashtable();
		}
		object obj = null;
		if (var.Type == null || var.Type.IsValueType || var.Type == typeof(string))
		{
			obj = ((var.InitBy != 0) ? GetExpressionValue("{" + var.Value + "}", textBox, hashtable, report, parameters) : var.ValueObject);
		}
		else
		{
			obj = ((!(var.Type == typeof(Metafile)) && !(var.Type == typeof(Bitmap)) && !(var.Type == typeof(Image))) ? Activator.CreateInstance(var.Type) : var.ValueObject);
			if (obj is Range)
			{
				Range range = obj as Range;
				if (var.InitBy == StiVariableInitBy.Value)
				{
					range.FromObject = (var.ValueObject as Range).FromObject;
					range.ToObject = (var.ValueObject as Range).ToObject;
				}
				else
				{
					range.FromObject = GetExpressionValue("{" + var.InitByExpressionFrom + "}", textBox, hashtable, report, parameters);
					range.ToObject = GetExpressionValue("{" + var.InitByExpressionTo + "}", textBox, hashtable, report, parameters);
					if (range.FromObject is DateTime)
					{
						range.FromObject = ((DateTime)range.FromObject).Date;
					}
					if (range.ToObject is DateTime)
					{
						range.ToObject = ((DateTime)range.ToObject).Date.AddDays(1.0).AddTicks(-1L);
					}
				}
			}
			if (fillItems)
			{
				bool modified = false;
				StiVariableHelper.FillItemsOfVariable(var, report, ref modified);
			}
		}
		if (fillItems)
		{
			foreach (DictionaryEntry item in hashtable)
			{
				StiDataSource dataSource = report.Dictionary.DataSources[(string)item.Key];
				StiDataLeader.Disconnect(dataSource);
			}
		}
		report[var.Name] = obj;
		return obj;
	}

	private static object GetExpressionValue(string expr, StiComponent comp, Hashtable hash, StiReport report, StiParserParameters parameters)
	{
		if (hash != null)
		{
			Hashtable hashtable = new Hashtable();
			StiDataSourceHelper.CheckExpression(expr, comp, hashtable);
			foreach (DictionaryEntry item in hashtable)
			{
				object obj = hash[item.Key];
				if (obj == null)
				{
					hash[item.Key] = true;
					StiDataSource dataSource = report.Dictionary.DataSources[(string)item.Key];
					StiDataLeader.Connect(dataSource);
				}
			}
		}
		if (parameters != null)
		{
			return ParseTextValue(expr, comp, parameters);
		}
		return ParseTextValue(expr, comp);
	}

	private StiToken GetNextLexem()
	{
		while (position < inputExpression.Length && isWhiteSpace(inputExpression[position]))
		{
			position++;
		}
		if (position >= inputExpression.Length)
		{
			return null;
		}
		StiToken stiToken = null;
		char c = inputExpression[position];
		if (char.IsLetter(c) || c == '_')
		{
			int i;
			for (i = position + 1; i < inputExpression.Length && (char.IsLetterOrDigit(inputExpression[i]) || inputExpression[i] == '_'); i++)
			{
			}
			stiToken = new StiToken();
			stiToken.Value = inputExpression.Substring(position, i - position);
			stiToken.Type = StiTokenType.Identifier;
			stiToken.Position = position;
			stiToken.Length = i - position;
			position = i;
			if (useAliases)
			{
				string text = stiToken.Value;
				if (stiToken.Position > 0 && inputExpression[stiToken.Position - 1] == '.')
				{
					text = "." + text;
				}
				if (hashAliases.ContainsKey(text))
				{
					stiToken.Value = (string)hashAliases[text];
				}
			}
			return stiToken;
		}
		if (char.IsDigit(c))
		{
			stiToken = new StiToken();
			stiToken.Type = StiTokenType.Number;
			stiToken.Position = position;
			stiToken.ValueObject = ScanNumber();
			if (stiToken.ValueObject == null)
			{
				stiToken.Type = StiTokenType.Identifier;
				stiToken.Value = inputExpression.Substring(stiToken.Position, position - stiToken.Position);
			}
			stiToken.Length = position - stiToken.Position;
			return stiToken;
		}
		if (c == '"' || (c == '@' && position < inputExpression.Length - 1 && inputExpression[position + 1] == '"'))
		{
			bool flag = true;
			if (c == '@')
			{
				flag = false;
				position++;
			}
			position++;
			int j;
			for (j = position; j < inputExpression.Length; j++)
			{
				if (flag)
				{
					if (inputExpression[j] == '"')
					{
						break;
					}
					if (inputExpression[j] == '\\')
					{
						j++;
					}
				}
				else if (inputExpression[j] == '"')
				{
					if (j + 1 >= inputExpression.Length || inputExpression[j + 1] != '"')
					{
						break;
					}
					j++;
				}
			}
			stiToken = new StiToken();
			stiToken.Type = StiTokenType.String;
			string input = inputExpression.Substring(position, j - position);
			if (flag)
			{
				stiToken.ValueObject = ReplaceBackslash(input);
			}
			else
			{
				stiToken.ValueObject = ReplaceQuotationMark(input);
			}
			stiToken.Position = position - 1;
			position = j + 1;
			stiToken.Length = position - stiToken.Position;
			return stiToken;
		}
		if (c == '\'' && position < inputExpression.Length - 2 && inputExpression[position + 2] == '\'')
		{
			position++;
			stiToken = new StiToken();
			stiToken.Type = StiTokenType.Char;
			stiToken.ValueObject = inputExpression[position];
			stiToken.Position = position;
			position += 2;
			stiToken.Length = 1;
			return stiToken;
		}
		if (c == '[')
		{
			int num = inputExpression.IndexOf(']', position);
			if (num != -1)
			{
				num++;
				string text2 = inputExpression.Substring(position, num - position);
				if (position > 0 && inputExpression[position - 1] == '.')
				{
					text2 = "." + text2;
				}
				if (hashAliases.ContainsKey(text2))
				{
					stiToken = new StiToken();
					stiToken.Value = (string)hashAliases[text2];
					stiToken.Type = StiTokenType.Identifier;
					stiToken.Position = position;
					stiToken.Length = num - position;
					position = num;
					return stiToken;
				}
			}
		}
		int num2 = position;
		position++;
		char c2 = ' ';
		if (position < inputExpression.Length)
		{
			c2 = inputExpression[position];
		}
		switch (c)
		{
		case '.':
			return new StiToken(StiTokenType.Dot, num2, 1);
		case '(':
			return new StiToken(StiTokenType.LParenthesis, num2, 1);
		case ')':
			return new StiToken(StiTokenType.RParenthesis, num2, 1);
		case '[':
			return new StiToken(StiTokenType.LBracket, num2, 1);
		case ']':
			return new StiToken(StiTokenType.RBracket, num2, 1);
		case '+':
			return new StiToken(StiTokenType.Plus, num2, 1);
		case '-':
			return new StiToken(StiTokenType.Minus, num2, 1);
		case '*':
			return new StiToken(StiTokenType.Mult, num2, 1);
		case '/':
			return new StiToken(StiTokenType.Div, num2, 1);
		case '%':
			return new StiToken(StiTokenType.Percent, num2, 1);
		case '^':
			return new StiToken(StiTokenType.Xor, num2, 1);
		case ',':
			return new StiToken(StiTokenType.Comma, num2, 1);
		case ':':
			return new StiToken(StiTokenType.Colon, num2, 1);
		case ';':
			return new StiToken(StiTokenType.SemiColon, num2, 1);
		case '?':
			return new StiToken(StiTokenType.Question, num2, 1);
		case '|':
			if (!IsVB)
			{
				if (c2 == '|')
				{
					position++;
					return new StiToken(StiTokenType.DoubleOr, num2, 2);
				}
				return new StiToken(StiTokenType.Or, num2, 1);
			}
			break;
		case '&':
			if (IsVB)
			{
				return new StiToken(StiTokenType.Plus, num2, 1);
			}
			if (c2 == '&')
			{
				position++;
				return new StiToken(StiTokenType.DoubleAnd, num2, 2);
			}
			return new StiToken(StiTokenType.And, num2, 1);
		case '!':
			if (c2 == '=' && !IsVB)
			{
				position++;
				return new StiToken(StiTokenType.NotEqual, num2, 2);
			}
			return new StiToken(StiTokenType.Not, num2, 1);
		case '=':
			if (IsVB)
			{
				return new StiToken(StiTokenType.Equal, num2, 1);
			}
			if (c2 == '=')
			{
				position++;
				return new StiToken(StiTokenType.Equal, num2, 2);
			}
			return new StiToken(StiTokenType.Assign, num2, 1);
		case '<':
			switch (c2)
			{
			case '<':
				position++;
				return new StiToken(StiTokenType.Shl, num2, 2);
			case '=':
				position++;
				return new StiToken(StiTokenType.LeftEqual, num2, 2);
			case '>':
				if (IsVB)
				{
					position++;
					return new StiToken(StiTokenType.NotEqual, num2, 2);
				}
				break;
			}
			return new StiToken(StiTokenType.Left, num2, 1);
		case '>':
			switch (c2)
			{
			case '>':
				position++;
				return new StiToken(StiTokenType.Shr, num2, 2);
			case '=':
				position++;
				return new StiToken(StiTokenType.RightEqual, num2, 2);
			default:
				return new StiToken(StiTokenType.Right, num2, 1);
			}
		}
		stiToken = new StiToken(StiTokenType.Unknown);
		stiToken.ValueObject = c;
		stiToken.Position = num2;
		stiToken.Length = 1;
		return stiToken;
	}

	private static bool isWhiteSpace(char ch)
	{
		if (!char.IsWhiteSpace(ch))
		{
			return ch < ' ';
		}
		return true;
	}

	private void BuildAliases()
	{
		if (hashAliases != null)
		{
			return;
		}
		hashAliases = new Hashtable();
		foreach (StiDataSource dataSource in report.Dictionary.DataSources)
		{
			string name = dataSource.Name;
			string correctedAlias = GetCorrectedAlias(dataSource.Alias);
			if (correctedAlias != name)
			{
				hashAliases[correctedAlias] = name;
			}
			foreach (StiDataColumn column in dataSource.Columns)
			{
				string name2 = column.Name;
				string correctedAlias2 = GetCorrectedAlias(column.Alias);
				if (correctedAlias2 != name2)
				{
					hashAliases["." + correctedAlias2] = name2;
				}
			}
		}
		foreach (StiDataSource dataSource2 in report.Dictionary.DataSources)
		{
			string name3 = dataSource2.Name;
			if (hashAliases.ContainsKey(name3))
			{
				hashAliases.Remove(name3);
			}
			foreach (StiDataColumn column2 in dataSource2.Columns)
			{
				if (column2 != null)
				{
					string name4 = column2.Name;
					if (hashAliases.ContainsKey("." + name4))
					{
						hashAliases.Remove("." + name4);
					}
				}
			}
		}
		foreach (StiBusinessObject businessObject in report.Dictionary.BusinessObjects)
		{
			BuildBusinessObject(report, businessObject);
		}
		foreach (StiDataRelation relation in report.Dictionary.Relations)
		{
			string name5 = relation.Name;
			string correctedAlias3 = GetCorrectedAlias(relation.Alias);
			if (correctedAlias3 != name5)
			{
				hashAliases["." + correctedAlias3] = name5;
			}
		}
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			string name6 = variable.Name;
			string correctedAlias4 = GetCorrectedAlias(variable.Alias);
			if (correctedAlias4 != name6)
			{
				hashAliases[correctedAlias4] = name6;
			}
		}
	}

	private void BuildBusinessObject(StiReport report, StiBusinessObject businesObject)
	{
		string name = businesObject.Name;
		string correctedAlias = GetCorrectedAlias(businesObject.Alias);
		if (correctedAlias != name)
		{
			hashAliases[correctedAlias] = name;
			hashAliases["." + correctedAlias] = name;
		}
		foreach (StiDataColumn column in businesObject.Columns)
		{
			string name2 = column.Name;
			string correctedAlias2 = GetCorrectedAlias(column.Alias);
			if (correctedAlias2 != name2)
			{
				hashAliases["." + correctedAlias2] = name2;
			}
		}
		foreach (StiBusinessObject businessObject in businesObject.BusinessObjects)
		{
			BuildBusinessObject(report, businessObject);
		}
	}

	private static bool IsValidName(string name)
	{
		if (string.IsNullOrEmpty(name) || (!char.IsLetter(name[0]) && name[0] != '_'))
		{
			return false;
		}
		for (int i = 0; i < name.Length; i++)
		{
			if (!char.IsLetterOrDigit(name[i]) && name[i] != '_')
			{
				return false;
			}
		}
		return true;
	}

	private static string GetCorrectedAlias(string alias)
	{
		if (IsValidName(alias))
		{
			return alias;
		}
		return $"[{alias}]";
	}

	private static string ReplaceBackslash(string input)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < input.Length; i++)
		{
			if (input[i] == '\\' && i < input.Length - 1)
			{
				i++;
				char c = input[i];
				switch (c)
				{
				case '\\':
					stringBuilder.Append("\\");
					break;
				case '\'':
					stringBuilder.Append('\'');
					break;
				case '"':
					stringBuilder.Append('"');
					break;
				case '0':
					stringBuilder.Append('\0');
					break;
				case 'n':
					stringBuilder.Append('\n');
					break;
				case 'r':
					stringBuilder.Append('\r');
					break;
				case 't':
					stringBuilder.Append('\t');
					break;
				case 'x':
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					int num = 0;
					while (i < input.Length - 1 && num < 4 && "0123456789abcdefABCDEF".IndexOf(input[i + 1]) != -1)
					{
						stringBuilder2.Append(input[i + 1]);
						i++;
						num++;
					}
					int result = 0;
					bool flag = int.TryParse(stringBuilder2.ToString(), NumberStyles.HexNumber, null, out result);
					stringBuilder.Append((char)result);
					break;
				}
				default:
					stringBuilder.Append("\\" + c);
					break;
				}
			}
			else
			{
				stringBuilder.Append(input[i]);
			}
		}
		return stringBuilder.ToString();
	}

	private static string ReplaceQuotationMark(string input)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < input.Length; i++)
		{
			if (input[i] == '"' && i < input.Length - 1)
			{
				i++;
				stringBuilder.Append('"');
			}
			else
			{
				stringBuilder.Append(input[i]);
			}
		}
		return stringBuilder.ToString();
	}

	private object ScanNumber()
	{
		TypeCode typeCode = TypeCode.Int32;
		int num = position;
		int num2 = position;
		while (position != inputExpression.Length && char.IsDigit(inputExpression[position]))
		{
			position++;
		}
		if (position != inputExpression.Length && inputExpression[position] == '.' && position + 1 != inputExpression.Length && char.IsDigit(inputExpression[position + 1]))
		{
			position++;
			while (position != inputExpression.Length && char.IsDigit(inputExpression[position]))
			{
				position++;
			}
			typeCode = TypeCode.Double;
		}
		string text = inputExpression.Substring(num, position - num);
		text = text.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
		if (position != inputExpression.Length && char.IsLetter(inputExpression[position]))
		{
			num = position;
			while (position != inputExpression.Length && char.IsLetter(inputExpression[position]))
			{
				position++;
			}
			switch (inputExpression.Substring(num, position - num).ToLower())
			{
			case "f":
				typeCode = TypeCode.Single;
				break;
			case "d":
				typeCode = TypeCode.Double;
				break;
			case "m":
				typeCode = TypeCode.Decimal;
				break;
			case "l":
				typeCode = TypeCode.Int64;
				break;
			case "u":
			case "ul":
			case "lu":
				typeCode = TypeCode.UInt64;
				break;
			default:
				return null;
			}
		}
		if (typeCode == TypeCode.Int32 && text.Length > 9)
		{
			typeCode = TypeCode.Int64;
		}
		object result = null;
		try
		{
			result = Convert.ChangeType(text, typeCode);
		}
		catch
		{
			if (typeCode == TypeCode.Int32 || typeCode == TypeCode.Int64 || typeCode == TypeCode.UInt32 || typeCode == TypeCode.UInt64)
			{
				ThrowError(ParserErrorCode.IntegralConstantIsTooLarge, new StiToken(StiTokenType.Number, num2, position - num2));
			}
		}
		return result;
	}

	private List<StiToken> PostProcessTokensList(List<StiToken> tokensList)
	{
		List<StiToken> list = new List<StiToken>();
		tokenPos = 0;
		while (tokenPos < tokensList.Count)
		{
			StiToken stiToken = tokensList[tokenPos];
			tokenPos++;
			if (stiToken.Type == StiTokenType.Identifier)
			{
				StiDataSource stiDataSource = report.Dictionary.DataSources[stiToken.Value];
				StiBusinessObject stiBusinessObject = report.Dictionary.BusinessObjects[stiToken.Value];
				string text = null;
				if (NamespacesList.Contains(stiToken.Value))
				{
					if (tokenPos + 1 >= tokensList.Count)
					{
						ThrowError(ParserErrorCode.UnexpectedEndOfExpression);
					}
					if (tokensList[tokenPos].Type != StiTokenType.Dot)
					{
						ThrowError(ParserErrorCode.SyntaxError, stiToken, stiToken.Value);
					}
					text = stiToken.Value + ".";
					stiToken = tokensList[tokenPos + 1];
					tokenPos += 2;
					while (NamespacesList.Contains(text + tokensList[tokenPos - 1].Value))
					{
						if (tokenPos + 1 >= tokensList.Count)
						{
							ThrowError(ParserErrorCode.UnexpectedEndOfExpression);
						}
						if (tokensList[tokenPos].Type != StiTokenType.Dot)
						{
							ThrowError(ParserErrorCode.SyntaxError, stiToken, stiToken.Value);
						}
						text = text + stiToken.Value + ".";
						stiToken = tokensList[tokenPos + 1];
						tokenPos += 2;
					}
				}
				if (stiDataSource != null)
				{
					StringBuilder stringBuilder = new StringBuilder(StiNameValidator.CorrectName(stiToken.Value, report));
					StiParserDataSourceFieldInfo stiParserDataSourceFieldInfo = new StiParserDataSourceFieldInfo();
					stiParserDataSourceFieldInfo.Path.Add(StiNameValidator.CorrectName(stiToken.Value, report));
					stiParserDataSourceFieldInfo.Objects.Add(stiDataSource);
					while (tokenPos + 1 < tokensList.Count && tokensList[tokenPos].Type == StiTokenType.Dot && stiDataSource != null)
					{
						stiToken = tokensList[tokenPos + 1];
						string text2 = StiNameValidator.CorrectName(stiToken.Value, report);
						StiDataRelation dataRelationByName = GetDataRelationByName(text2, stiDataSource);
						if (dataRelationByName != null)
						{
							stiDataSource = dataRelationByName.ParentSource;
							tokenPos += 2;
							stringBuilder.Append(".");
							stringBuilder.Append(dataRelationByName.NameInSource);
							stiParserDataSourceFieldInfo.Path.Add(dataRelationByName.NameInSource);
							stiParserDataSourceFieldInfo.Objects.Add(dataRelationByName);
							continue;
						}
						StiDataColumn dataColumnByName = GetDataColumnByName(text2, stiDataSource);
						if (dataColumnByName == null && tokenPos + 3 < tokensList.Count && tokensList[tokenPos + 2].Type == StiTokenType.Dot && tokensList[tokenPos + 3].Type == StiTokenType.Identifier)
						{
							text2 = StiNameValidator.CorrectName(stiToken.Value + "." + tokensList[tokenPos + 3].Value, report);
							dataColumnByName = GetDataColumnByName(text2, stiDataSource);
							if (dataColumnByName != null)
							{
								tokenPos += 2;
							}
							else if (tokenPos + 5 < tokensList.Count && tokensList[tokenPos + 4].Type == StiTokenType.Dot && tokensList[tokenPos + 5].Type == StiTokenType.Identifier)
							{
								text2 = StiNameValidator.CorrectName(stiToken.Value + "." + tokensList[tokenPos + 3].Value + "." + tokensList[tokenPos + 5].Value, report);
								dataColumnByName = GetDataColumnByName(text2, stiDataSource);
								if (dataColumnByName != null)
								{
									tokenPos += 4;
								}
							}
						}
						if (dataColumnByName != null)
						{
							tokenPos += 2;
							stringBuilder.Append(".");
							stringBuilder.Append(text2);
							stiParserDataSourceFieldInfo.Path.Add(dataColumnByName.Name);
							stiParserDataSourceFieldInfo.Objects.Add(dataColumnByName);
						}
						else
						{
							stiToken = tokensList[tokenPos - 1];
						}
						break;
					}
					stiToken.Type = StiTokenType.DataSourceField;
					stiToken.Value = stringBuilder.ToString();
					stiToken.ValueObject = stiParserDataSourceFieldInfo;
				}
				else if (stiBusinessObject != null)
				{
					StringBuilder stringBuilder2 = new StringBuilder(stiToken.Value);
					while (tokenPos + 1 < tokensList.Count && tokensList[tokenPos].Type == StiTokenType.Dot)
					{
						stiToken = tokensList[tokenPos + 1];
						string value = stiToken.Value;
						if (stiBusinessObject.Columns.Contains(value))
						{
							tokenPos += 2;
							stringBuilder2.Append(".");
							stringBuilder2.Append(value);
							break;
						}
						stiBusinessObject = stiBusinessObject.BusinessObjects[value];
						if (stiBusinessObject != null)
						{
							tokenPos += 2;
							stringBuilder2.Append(".");
							stringBuilder2.Append(stiBusinessObject.Name);
							continue;
						}
						stiToken = tokensList[tokenPos - 1];
						break;
					}
					stiToken.Type = StiTokenType.BusinessObjectField;
					stiToken.Value = stringBuilder2.ToString();
				}
				else if (list.Count > 0 && list[list.Count - 1].Type == StiTokenType.Dot && text == null)
				{
					if (MethodsList.Contains(stiToken.Value))
					{
						stiToken.Type = StiTokenType.Method;
					}
					else if (PropertiesList.Contains(stiToken.Value))
					{
						stiToken.Type = StiTokenType.Property;
					}
					else
					{
						if ((tokenPos >= tokensList.Count || tokensList[tokenPos].Type != StiTokenType.LParenthesis) && list.Count > 1 && list[list.Count - 2].Type == StiTokenType.DataSourceField)
						{
							string value2 = list[list.Count - 2].Value;
							List<string> list2 = new List<string>(value2.Split(new char[1] { '.' }));
							StiDataSource stiDataSource2 = report.Dictionary.DataSources[list2[0]];
							StiDataColumn stiDataColumn = null;
							if (list2.Count > 1)
							{
								if (list2.Count == 2)
								{
									stiDataColumn = stiDataSource2.Columns[list2[1]];
								}
								else
								{
									string relationName = list2[1];
									stiDataSource2 = stiDataSource2.GetParentDataSource(relationName);
									int i;
									for (i = 2; i < list2.Count - 1; i++)
									{
										relationName = list2[i];
										stiDataSource2 = stiDataSource2.GetParentDataSource(relationName);
									}
									stiDataColumn = stiDataSource2.Columns[list2[i]];
								}
							}
							if (stiDataColumn != null)
							{
								try
								{
									PropertyInfo property = stiDataColumn.Type.GetProperty(stiToken.Value);
									if (property != null)
									{
										stiToken.Type = StiTokenType.Property;
									}
								}
								catch
								{
								}
							}
						}
						if (stiToken.Type == StiTokenType.Identifier)
						{
							ThrowError(ParserErrorCode.FieldMethodOrPropertyNotFound, stiToken, stiToken.Value);
						}
					}
				}
				else if (TypesList.Contains(stiToken.Value) && tokenPos + 1 < tokensList.Count && (tokensList[tokenPos].Type == StiTokenType.Dot || tokensList[tokenPos].Type == StiTokenType.RParenthesis || tokensList[tokenPos].Type == StiTokenType.Left))
				{
					object obj2 = TypesList[stiToken.Value];
					stiToken.Type = StiTokenType.Cast;
					stiToken.ValueObject = obj2;
					if (tokensList[tokenPos].Type == StiTokenType.Dot)
					{
						string text3 = stiToken.Value + "." + tokensList[tokenPos + 1].Value;
						if (FunctionsList.Contains(text3))
						{
							stiToken.Type = StiTokenType.Function;
							stiToken.Value = text3;
							tokenPos += 2;
						}
						if (SystemVariablesList.Contains(text3))
						{
							stiToken.Type = StiTokenType.SystemVariable;
							stiToken.Value = text3;
							tokenPos += 2;
						}
						if (ConstantsList.Contains(text3))
						{
							stiToken.Type = StiTokenType.Number;
							stiToken.ValueObject = ConstantsList[text3];
							stiToken.Value = text3;
							tokenPos += 2;
						}
					}
					Type type = obj2 as Type;
					if (type != null && type == typeof(List<>) && tokensList[tokenPos].Type == StiTokenType.Left)
					{
						object obj3 = TypesList[tokensList[tokenPos + 1].Value];
						if (tokenPos + 3 >= tokensList.Count || obj3 == null || tokensList[tokenPos + 2].Type != StiTokenType.Right)
						{
							ThrowError(ParserErrorCode.SyntaxError, stiToken, stiToken.Value);
						}
						Type type2 = Type.GetType("System." + (TypeCode)obj3);
						string typeName = "System.Collections.Generic.List`1[[" + type2.FullName + "]]";
						stiToken.ValueObject = Type.GetType(typeName);
						tokenPos += 3;
					}
				}
				else if (ComponentsList.Contains(stiToken.Value) && (tokenPos >= tokensList.Count || tokensList[tokenPos].Type != StiTokenType.LParenthesis))
				{
					stiToken.Type = StiTokenType.Component;
					if (tokenPos + 1 < tokensList.Count && tokensList[tokenPos].Type == StiTokenType.Colon && ComponentsList.Contains(tokensList[tokenPos + 1].Value))
					{
						StiComponent stiComponent = (StiComponent)ComponentsList[tokensList[tokenPos + 1].Value];
						if (stiComponent != null && stiComponent is StiDataBand)
						{
							StiParserDataSourceFieldInfo stiParserDataSourceFieldInfo2 = new StiParserDataSourceFieldInfo();
							stiParserDataSourceFieldInfo2.Path.Add(StiNameValidator.CorrectName(tokensList[tokenPos + 1].Value, report));
							stiParserDataSourceFieldInfo2.Objects.Add((stiComponent as StiDataBand).DataSource);
							stiToken.Value = (stiComponent as StiDataBand).DataSourceName;
							stiToken.ValueObject = stiParserDataSourceFieldInfo2;
							stiToken.Type = StiTokenType.DataSourceField;
							tokenPos += 2;
						}
					}
				}
				else if (FunctionsList.Contains(text + stiToken.Value) && tokenPos < tokensList.Count && tokensList[tokenPos].Type == StiTokenType.LParenthesis)
				{
					stiToken.Value = text + stiToken.Value;
					stiToken.Type = StiTokenType.Function;
				}
				else if (runtimeConstants != null && runtimeConstants.Count > 0 && runtimeConstantsHash.Contains(stiToken.Value))
				{
					while (runtimeConstantsHash[stiToken.Value] == namespaceObj)
					{
						if (tokenPos + 1 >= tokensList.Count)
						{
							ThrowError(ParserErrorCode.UnexpectedEndOfExpression);
						}
						string value3 = stiToken.Value;
						StiToken stiToken2 = stiToken;
						stiToken2.Value = stiToken2.Value + "." + tokensList[tokenPos + 1].Value;
						if (!runtimeConstantsHash.Contains(stiToken.Value))
						{
							ThrowError(ParserErrorCode.ItemDoesNotContainDefinition, stiToken, value3, tokensList[tokenPos + 1].Value);
						}
						tokenPos += 2;
					}
					stiToken.Type = StiTokenType.Variable;
				}
				else if (OperatorsList.Contains(stiToken.Value) && text == null)
				{
					stiToken.Type = (StiTokenType)OperatorsList[stiToken.Value];
				}
				else if (ConstantsList.Contains(text + stiToken.Value))
				{
					while (ConstantsList[text + stiToken.Value] == namespaceObj)
					{
						if (tokenPos + 1 >= tokensList.Count)
						{
							ThrowError(ParserErrorCode.UnexpectedEndOfExpression);
						}
						string value4 = stiToken.Value;
						StiToken stiToken3 = stiToken;
						stiToken3.Value = stiToken3.Value + "." + tokensList[tokenPos + 1].Value;
						tokenPos += 2;
						if (!ConstantsList.Contains(text + stiToken.Value))
						{
							ThrowError(ParserErrorCode.ItemDoesNotContainDefinition, stiToken, value4, tokensList[tokenPos + 1].Value);
						}
					}
					stiToken.Value = text + stiToken.Value;
					stiToken.ValueObject = ConstantsList[stiToken.Value];
					stiToken.Type = StiTokenType.Number;
				}
				else if (report.Dictionary.Variables.Contains(stiToken.Value) || (report.Variables != null && report.Variables.ContainsKey(stiToken.Value)))
				{
					if (tokenPos + 1 < tokensList.Count && tokensList[tokenPos].Type == StiTokenType.Dot && tokensList[tokenPos + 1].Value == "Label")
					{
						stiToken.Type = StiTokenType.String;
						stiToken.ValueObject = stiToken.Value;
						list.Add(new StiToken(StiTokenType.Function, stiToken.Position, stiToken.Length + 6)
						{
							Value = "GetLabel"
						});
						list.Add(new StiToken(StiTokenType.LParenthesis, stiToken.Position, stiToken.Length));
						list.Add(stiToken);
						list.Add(new StiToken(StiTokenType.RParenthesis, stiToken.Position, stiToken.Length));
						tokenPos += 2;
						continue;
					}
					stiToken.Type = StiTokenType.Variable;
				}
				else if (SystemVariablesList.Contains(stiToken.Value) && (stiToken.Value != "value" || component is StiCrossCell))
				{
					stiToken.Type = StiTokenType.SystemVariable;
				}
				else if (KeywordsList.ContainsKey(stiToken.Value) && (StiTokenType)KeywordsList[stiToken.Value] == StiTokenType.RefVariable)
				{
					if (tokenPos < tokensList.Count && report.Dictionary.Variables.Contains(tokensList[tokenPos].Value))
					{
						stiToken.Type = StiTokenType.RefVariable;
						stiToken.Value = tokensList[tokenPos].Value;
						tokenPos++;
					}
					else
					{
						ThrowError(ParserErrorCode.SyntaxError, stiToken, stiToken.Value);
					}
				}
				else if (UserFunctionsList.Contains(stiToken.Value))
				{
					stiToken.Type = StiTokenType.Function;
				}
				else if (!string.IsNullOrEmpty(text))
				{
					ThrowError(ParserErrorCode.TheTypeOrNamespaceNotExistInTheNamespace, stiToken, stiToken.Value, text.Substring(0, text.Length - 1));
				}
				else
				{
					ThrowError(ParserErrorCode.NameDoesNotExistInCurrentContext, stiToken, stiToken.Value);
				}
			}
			list.Add(stiToken);
		}
		return list;
	}

	private void CreateRuntimeConstantsHash()
	{
		if (runtimeConstantsHash != null)
		{
			runtimeConstantsHash.Clear();
		}
		else
		{
			runtimeConstantsHash = new Hashtable(syntaxCaseSensitive ? StringComparer.InvariantCulture : StringComparer.InvariantCultureIgnoreCase);
		}
		foreach (DictionaryEntry runtimeConstant in runtimeConstants)
		{
			string text = runtimeConstant.Key.ToString();
			if (text.Contains("."))
			{
				string[] array = text.Split(new char[1] { '.' });
				int num = 0;
				string text2 = array[0];
				while (num < array.Length - 1)
				{
					runtimeConstantsHash[text2] = namespaceObj;
					num++;
					text2 = text2 + "." + array[num];
				}
			}
			runtimeConstantsHash[text] = runtimeConstant.Value;
		}
	}

	private StiDataRelation GetDataRelationByName(string name, StiDataSource ds)
	{
		foreach (StiDataRelation relation in report.Dictionary.Relations)
		{
			if (relation.ChildSource == ds && (relation.Name == name || relation.NameInSource == name))
			{
				return relation;
			}
		}
		foreach (StiDataRelation relation2 in report.Dictionary.Relations)
		{
			if (relation2.ChildSource == ds && (StiNameValidator.CorrectName(relation2.Name, report) == name || StiNameValidator.CorrectName(relation2.NameInSource, report) == name))
			{
				return relation2;
			}
		}
		return null;
	}

	private StiDataColumn GetDataColumnByName(string name, StiDataSource ds)
	{
		StiDataColumn stiDataColumn = ds.Columns[name];
		if (stiDataColumn == null)
		{
			string text = name.ToLower(CultureInfo.InvariantCulture);
			foreach (StiDataColumn column in ds.Columns)
			{
				if (StiNameValidator.CorrectName(column.Name.ToLower(CultureInfo.InvariantCulture)) == text)
				{
					ds.Columns.CachedDataColumns[name] = column;
					stiDataColumn = column;
					name = column.Name;
					break;
				}
			}
		}
		if (ds.DataTable != null)
		{
			int columnIndex = ds.GetColumnIndex(name);
			if (columnIndex >= 0 && columnIndex < ds.DataTable.Columns.Count)
			{
				string columnName = ds.DataTable.Columns[columnIndex].ColumnName;
				foreach (StiDataColumn column2 in ds.Columns)
				{
					if (column2.NameInSource == columnName)
					{
						return column2;
					}
				}
			}
		}
		return stiDataColumn;
	}

	private void MakeTokensList()
	{
		BuildAliases();
		tokensList = new List<StiToken>();
		position = 0;
		while (true)
		{
			StiToken nextLexem = GetNextLexem();
			if (nextLexem == null)
			{
				break;
			}
			tokensList.Add(nextLexem);
		}
		tokensList = PostProcessTokensList(tokensList);
	}

	private void eval_exp()
	{
		tokenPos = 0;
		if (tokensList.Count == 0)
		{
			ThrowError(ParserErrorCode.ExpressionIsEmpty);
			return;
		}
		eval_exp0();
		if (tokenPos <= tokensList.Count)
		{
			ThrowError(ParserErrorCode.UnprocessedLexemesRemain);
		}
	}

	private void eval_exp0()
	{
		get_token();
		eval_exp01();
	}

	private void eval_exp01()
	{
		if (currentToken.Type == StiTokenType.Variable || (currentToken.Type == StiTokenType.SystemVariable && (currentToken.Value == "ReportName" || currentToken.Value == "ReportAuthor" || currentToken.Value == "ReportAlias" || currentToken.Value == "ReportDescription")))
		{
			StiToken stiToken = currentToken;
			get_token();
			if (currentToken.Type == StiTokenType.Assign)
			{
				get_token();
				eval_exp1();
				asmList.Add(new StiAsmCommand(StiAsmCommandType.CopyToVariable, stiToken.Value));
				return;
			}
			tokenPos--;
			currentToken = tokensList[tokenPos - 1];
		}
		eval_exp1();
	}

	private void eval_exp1()
	{
		eval_exp10();
		if (currentToken.Type != StiTokenType.Question)
		{
			return;
		}
		if (checkSyntaxMode)
		{
			get_token();
			eval_exp10();
			if (currentToken.Type != StiTokenType.Colon)
			{
				ThrowError(ParserErrorCode.SyntaxError, currentToken);
			}
			get_token();
			eval_exp10();
			asmList.Add(new StiAsmCommand(StiAsmCommandType.PushFunction, StiFunctionType.IIF, 3));
			return;
		}
		get_token();
		StiAsmCommand stiAsmCommand = new StiAsmCommand(StiAsmCommandType.JumpFalse, 0);
		asmList.Add(stiAsmCommand);
		int count = asmList.Count;
		eval_exp10();
		if (currentToken.Type != StiTokenType.Colon)
		{
			ThrowError(ParserErrorCode.SyntaxError, currentToken);
		}
		StiAsmCommand stiAsmCommand2 = new StiAsmCommand(StiAsmCommandType.Jump, 0);
		asmList.Add(stiAsmCommand2);
		int count2 = asmList.Count;
		get_token();
		eval_exp10();
		stiAsmCommand.Parameter1 = count2 - count;
		stiAsmCommand2.Parameter1 = asmList.Count - count2;
	}

	private void eval_exp10()
	{
		eval_exp11();
		while (currentToken.Type == StiTokenType.DoubleOr)
		{
			get_token();
			eval_exp11();
			asmList.Add(new StiAsmCommand(StiAsmCommandType.Or2));
		}
	}

	private void eval_exp11()
	{
		eval_exp12();
		while (currentToken.Type == StiTokenType.DoubleAnd)
		{
			get_token();
			eval_exp12();
			asmList.Add(new StiAsmCommand(StiAsmCommandType.And2));
		}
	}

	private void eval_exp12()
	{
		eval_exp14();
		while (currentToken.Type == StiTokenType.Or)
		{
			get_token();
			eval_exp14();
			asmList.Add(new StiAsmCommand(StiAsmCommandType.Or));
		}
	}

	private void eval_exp14()
	{
		eval_exp15();
		if (currentToken.Type == StiTokenType.Xor)
		{
			get_token();
			eval_exp15();
			asmList.Add(new StiAsmCommand(StiAsmCommandType.Xor));
		}
	}

	private void eval_exp15()
	{
		eval_exp16();
		while (currentToken.Type == StiTokenType.And)
		{
			get_token();
			eval_exp16();
			asmList.Add(new StiAsmCommand(StiAsmCommandType.And));
		}
	}

	private void eval_exp16()
	{
		eval_exp17();
		if (currentToken.Type == StiTokenType.Equal || currentToken.Type == StiTokenType.NotEqual)
		{
			StiAsmCommand stiAsmCommand = new StiAsmCommand(StiAsmCommandType.CompareEqual);
			if (currentToken.Type == StiTokenType.NotEqual)
			{
				stiAsmCommand.Type = StiAsmCommandType.CompareNotEqual;
			}
			get_token();
			eval_exp17();
			asmList.Add(stiAsmCommand);
		}
	}

	private void eval_exp17()
	{
		eval_exp18();
		if (currentToken.Type == StiTokenType.Left || currentToken.Type == StiTokenType.LeftEqual || currentToken.Type == StiTokenType.Right || currentToken.Type == StiTokenType.RightEqual)
		{
			StiAsmCommand item = null;
			if (currentToken.Type == StiTokenType.Left)
			{
				item = new StiAsmCommand(StiAsmCommandType.CompareLeft);
			}
			if (currentToken.Type == StiTokenType.LeftEqual)
			{
				item = new StiAsmCommand(StiAsmCommandType.CompareLeftEqual);
			}
			if (currentToken.Type == StiTokenType.Right)
			{
				item = new StiAsmCommand(StiAsmCommandType.CompareRight);
			}
			if (currentToken.Type == StiTokenType.RightEqual)
			{
				item = new StiAsmCommand(StiAsmCommandType.CompareRightEqual);
			}
			get_token();
			eval_exp18();
			asmList.Add(item);
		}
	}

	private void eval_exp18()
	{
		eval_exp2();
		if (currentToken.Type == StiTokenType.Shl || currentToken.Type == StiTokenType.Shr)
		{
			StiAsmCommand stiAsmCommand = new StiAsmCommand(StiAsmCommandType.Shl);
			if (currentToken.Type == StiTokenType.Shr)
			{
				stiAsmCommand.Type = StiAsmCommandType.Shr;
			}
			get_token();
			eval_exp2();
			asmList.Add(stiAsmCommand);
		}
	}

	private void eval_exp2()
	{
		eval_exp3();
		while (currentToken.Type == StiTokenType.Plus || currentToken.Type == StiTokenType.Minus)
		{
			StiToken stiToken = currentToken;
			get_token();
			eval_exp3();
			if (stiToken.Type == StiTokenType.Minus)
			{
				asmList.Add(new StiAsmCommand(StiAsmCommandType.Sub));
			}
			else if (stiToken.Type == StiTokenType.Plus)
			{
				asmList.Add(new StiAsmCommand(StiAsmCommandType.Add));
			}
		}
	}

	private void eval_exp3()
	{
		eval_exp4();
		while (currentToken.Type == StiTokenType.Mult || currentToken.Type == StiTokenType.Div || currentToken.Type == StiTokenType.Percent)
		{
			StiToken stiToken = currentToken;
			get_token();
			eval_exp4();
			if (stiToken.Type == StiTokenType.Mult)
			{
				asmList.Add(new StiAsmCommand(StiAsmCommandType.Mult));
			}
			else if (stiToken.Type == StiTokenType.Div)
			{
				asmList.Add(new StiAsmCommand(StiAsmCommandType.Div));
			}
			if (stiToken.Type == StiTokenType.Percent)
			{
				asmList.Add(new StiAsmCommand(StiAsmCommandType.Mod));
			}
		}
	}

	private void eval_exp4()
	{
		eval_exp5();
	}

	private void eval_exp5()
	{
		StiAsmCommand stiAsmCommand = null;
		if (currentToken.Type == StiTokenType.Plus || currentToken.Type == StiTokenType.Minus || currentToken.Type == StiTokenType.Not)
		{
			if (currentToken.Type == StiTokenType.Minus)
			{
				stiAsmCommand = new StiAsmCommand(StiAsmCommandType.Neg);
			}
			if (currentToken.Type == StiTokenType.Not)
			{
				stiAsmCommand = new StiAsmCommand(StiAsmCommandType.Not);
			}
			get_token();
		}
		eval_exp6();
		if (stiAsmCommand != null)
		{
			asmList.Add(stiAsmCommand);
		}
	}

	private void eval_exp6()
	{
		if (currentToken.Type == StiTokenType.LParenthesis)
		{
			get_token();
			if (currentToken.Type == StiTokenType.Cast)
			{
				object valueObject = currentToken.ValueObject;
				get_token();
				if (currentToken.Type != StiTokenType.RParenthesis)
				{
					ThrowError(ParserErrorCode.RightParenthesisExpected);
				}
				get_token();
				eval_exp5();
				asmList.Add(new StiAsmCommand(StiAsmCommandType.Cast, valueObject));
				return;
			}
			eval_exp1();
			if (currentToken.Type != StiTokenType.RParenthesis)
			{
				ThrowError(ParserErrorCode.RightParenthesisExpected);
			}
			get_token();
			if (currentToken.Type == StiTokenType.Dot)
			{
				get_token();
				eval_exp7();
			}
			if (currentToken.Type == StiTokenType.LBracket)
			{
				eval_exp62();
			}
		}
		else
		{
			eval_exp62();
		}
	}

	private void eval_exp62()
	{
		if (currentToken.Type == StiTokenType.LBracket)
		{
			int i;
			for (i = 0; i == 0 || currentToken.Type == StiTokenType.Comma; i++)
			{
				get_token();
				eval_exp1();
			}
			if (currentToken.Type != StiTokenType.RBracket)
			{
				ThrowError(ParserErrorCode.SyntaxError, currentToken);
			}
			asmList.Add(new StiAsmCommand(StiAsmCommandType.PushArrayElement, i + 1));
			get_token();
			if (currentToken.Type == StiTokenType.LBracket)
			{
				eval_exp62();
			}
			if (currentToken.Type == StiTokenType.Dot)
			{
				get_token();
				eval_exp7();
			}
		}
		else
		{
			eval_exp7();
		}
	}

	private void eval_exp7()
	{
		atom();
		if (currentToken.Type == StiTokenType.Dot)
		{
			get_token();
			eval_exp7();
		}
		if (currentToken.Type == StiTokenType.LBracket)
		{
			eval_exp62();
		}
	}

	private void atom()
	{
		if (currentToken.Type == StiTokenType.Variable)
		{
			asmList.Add(new StiAsmCommand(StiAsmCommandType.PushVariable, currentToken.Value));
			get_token();
		}
		else if (currentToken.Type == StiTokenType.RefVariable)
		{
			asmList.Add(new StiAsmCommand(StiAsmCommandType.PushRefVariable, currentToken.Value));
			get_token();
		}
		else if (currentToken.Type == StiTokenType.SystemVariable)
		{
			asmList.Add(new StiAsmCommand(StiAsmCommandType.PushSystemVariable, SystemVariablesList[currentToken.Value]));
			get_token();
		}
		else if (currentToken.Type == StiTokenType.Function)
		{
			StiToken stiToken = currentToken;
			object obj = FunctionsList[stiToken.Value];
			StiFunctionType stiFunctionType = ((obj == null) ? ((StiFunctionType)UserFunctionsList[stiToken.Value]) : ((StiFunctionType)obj));
			if (stiFunctionType == StiFunctionType.IIF && ProcessIifAsTernary)
			{
				get_args_count(stiFunctionType);
			}
			else
			{
				asmList.Add(new StiAsmCommand(StiAsmCommandType.PushFunction, stiFunctionType, get_args_count(stiFunctionType))
				{
					Position = expressionPosition + stiToken.Position,
					Length = stiToken.Length
				});
			}
			get_token();
		}
		else if (currentToken.Type == StiTokenType.Method)
		{
			StiToken stiToken2 = currentToken;
			StiMethodType stiMethodType = (StiMethodType)MethodsList[stiToken2.Value];
			asmList.Add(new StiAsmCommand(StiAsmCommandType.PushMethod, stiMethodType, get_args_count(stiMethodType) + 1));
			get_token();
		}
		else if (currentToken.Type == StiTokenType.Property)
		{
			StiToken stiToken3 = currentToken;
			asmList.Add(new StiAsmCommand(StiAsmCommandType.PushProperty, PropertiesList[stiToken3.Value] ?? stiToken3.Value));
			get_token();
		}
		else if (currentToken.Type == StiTokenType.DataSourceField)
		{
			StiParserDataSourceFieldInfo stiParserDataSourceFieldInfo = currentToken.ValueObject as StiParserDataSourceFieldInfo;
			asmList.Add(new StiAsmCommand(StiAsmCommandType.PushDataSourceField, currentToken.Value, stiParserDataSourceFieldInfo.Path.Count, stiParserDataSourceFieldInfo));
			get_token();
		}
		else if (currentToken.Type == StiTokenType.BusinessObjectField)
		{
			asmList.Add(new StiAsmCommand(StiAsmCommandType.PushBusinessObjectField, currentToken.Value));
			get_token();
		}
		else if (currentToken.Type == StiTokenType.Component)
		{
			asmList.Add(new StiAsmCommand(StiAsmCommandType.PushComponent, ComponentsList[currentToken.Value])
			{
				Position = expressionPosition + currentToken.Position,
				Length = currentToken.Length
			});
			get_token();
		}
		else if (currentToken.Type == StiTokenType.Number)
		{
			asmList.Add(new StiAsmCommand(StiAsmCommandType.PushValue, currentToken.ValueObject));
			get_token();
		}
		else if (currentToken.Type == StiTokenType.String)
		{
			asmList.Add(new StiAsmCommand(StiAsmCommandType.PushValue, currentToken.ValueObject));
			get_token();
		}
		else if (currentToken.Type == StiTokenType.Char)
		{
			asmList.Add(new StiAsmCommand(StiAsmCommandType.PushValue, currentToken.ValueObject));
			get_token();
		}
		else
		{
			if (currentToken.Type == StiTokenType.Empty)
			{
				ThrowError(ParserErrorCode.UnexpectedEndOfExpression);
			}
			ThrowError(ParserErrorCode.SyntaxError, currentToken);
		}
	}

	private int get_args_count(object name)
	{
		int num = -1;
		bool flag = false;
		if (currentToken != null)
		{
			num = currentToken.Position;
			flag = currentToken.Value.StartsWith("Totals.");
		}
		List<List<StiAsmCommand>> args = get_args();
		StiFunctionType stiFunctionType = (StiFunctionType)name;
		if (((stiFunctionType == StiFunctionType.Count || stiFunctionType == StiFunctionType.rCount || stiFunctionType == StiFunctionType.cCount || stiFunctionType == StiFunctionType.crCount || stiFunctionType == StiFunctionType.pCount || stiFunctionType == StiFunctionType.prCount) && args.Count == 0) || (((stiFunctionType >= StiFunctionType.CountDistinct && stiFunctionType <= StiFunctionType.Last) || (stiFunctionType >= StiFunctionType.rCountDistinct && stiFunctionType <= StiFunctionType.rLast) || (stiFunctionType >= StiFunctionType.cCountDistinct && stiFunctionType <= StiFunctionType.cLast) || (stiFunctionType >= StiFunctionType.crCountDistinct && stiFunctionType <= StiFunctionType.crLast) || (stiFunctionType >= StiFunctionType.pCountDistinct && stiFunctionType <= StiFunctionType.pLast) || (stiFunctionType >= StiFunctionType.prCountDistinct && stiFunctionType <= StiFunctionType.prLast)) && args.Count == 1) || ((stiFunctionType == StiFunctionType.iCount || stiFunctionType == StiFunctionType.riCount || stiFunctionType == StiFunctionType.ciCount || stiFunctionType == StiFunctionType.criCount || stiFunctionType == StiFunctionType.piCount || stiFunctionType == StiFunctionType.priCount) && args.Count == 1) || (((stiFunctionType >= StiFunctionType.iCountDistinct && stiFunctionType <= StiFunctionType.iLast) || (stiFunctionType >= StiFunctionType.riCountDistinct && stiFunctionType <= StiFunctionType.riLast) || (stiFunctionType >= StiFunctionType.ciCountDistinct && stiFunctionType <= StiFunctionType.ciLast) || (stiFunctionType >= StiFunctionType.criCountDistinct && stiFunctionType <= StiFunctionType.criLast) || (stiFunctionType >= StiFunctionType.piCountDistinct && stiFunctionType <= StiFunctionType.piLast) || (stiFunctionType >= StiFunctionType.priCountDistinct && stiFunctionType <= StiFunctionType.priLast)) && args.Count == 2))
		{
			object obj = component.GetGroupHeaderBand();
			if (obj == null)
			{
				obj = component.GetDataBand();
			}
			if (obj == null && args.Count > 0 && args[0] != null && args[0].Count > 0)
			{
				try
				{
					string text = null;
					foreach (StiAsmCommand item in args[0])
					{
						if (item.Type == StiAsmCommandType.PushDataSourceField)
						{
							StiParserDataSourceFieldInfo stiParserDataSourceFieldInfo = item.Parameter2 as StiParserDataSourceFieldInfo;
							string text2 = stiParserDataSourceFieldInfo.Path[0];
							if (text == null)
							{
								text = text2;
							}
							else if (text != text2)
							{
								text = null;
								break;
							}
						}
					}
					if (!string.IsNullOrWhiteSpace(text))
					{
						obj = report.Dictionary.DataSources[text];
					}
				}
				catch
				{
				}
			}
			List<StiAsmCommand> list = new List<StiAsmCommand>();
			list.Add(new StiAsmCommand(StiAsmCommandType.PushComponent, obj)
			{
				Position = expressionPosition + num
			});
			args.Insert(0, list);
		}
		if ((args.Count <= 0 || args[0].Count <= 0 || args[0][0].Type != StiAsmCommandType.PushComponent || !(args[0][0].Parameter1 is StiGroupHeaderBand)) && !flag && ((stiFunctionType >= StiFunctionType.Count && stiFunctionType <= StiFunctionType.Last) || (stiFunctionType >= StiFunctionType.rCount && stiFunctionType <= StiFunctionType.rLast) || (stiFunctionType >= StiFunctionType.iCount && stiFunctionType <= StiFunctionType.iLast) || (stiFunctionType >= StiFunctionType.riCount && stiFunctionType <= StiFunctionType.riLast) || (stiFunctionType >= StiFunctionType.cCount && stiFunctionType <= StiFunctionType.cLast) || (stiFunctionType >= StiFunctionType.crCount && stiFunctionType <= StiFunctionType.crLast) || (stiFunctionType >= StiFunctionType.ciCount && stiFunctionType <= StiFunctionType.ciLast) || (stiFunctionType >= StiFunctionType.criCount && stiFunctionType <= StiFunctionType.criLast) || (stiFunctionType >= StiFunctionType.pCount && stiFunctionType <= StiFunctionType.pLast) || (stiFunctionType >= StiFunctionType.prCount && stiFunctionType <= StiFunctionType.prLast) || (stiFunctionType >= StiFunctionType.piCount && stiFunctionType <= StiFunctionType.piLast) || (stiFunctionType >= StiFunctionType.priCount && stiFunctionType <= StiFunctionType.priLast)))
		{
			StiComponent groupHeaderBand = component.GetGroupHeaderBand();
			if (groupHeaderBand != null)
			{
				List<StiAsmCommand> list2 = new List<StiAsmCommand>();
				list2.Add(new StiAsmCommand(StiAsmCommandType.PushComponent, groupHeaderBand));
				args[0] = list2;
			}
		}
		if (((stiFunctionType >= StiFunctionType.pCount && stiFunctionType <= StiFunctionType.pLast) || (stiFunctionType >= StiFunctionType.prCount && stiFunctionType <= StiFunctionType.prLast) || (stiFunctionType >= StiFunctionType.piCount && stiFunctionType <= StiFunctionType.piLast) || (stiFunctionType >= StiFunctionType.priCount && stiFunctionType <= StiFunctionType.priLast)) && args.Count > 0)
		{
			List<StiAsmCommand> list3 = args[0];
			if (list3 != null && list3.Count > 0 && list3[0].Type == StiAsmCommandType.PushComponent && list3[0].Parameter1 is StiBand)
			{
				string name2 = (list3[0].Parameter1 as StiBand).Name;
				if (report.DataBandsUsedInPageTotals == null)
				{
					report.DataBandsUsedInPageTotals = new string[1];
					report.DataBandsUsedInPageTotals[0] = name2;
				}
				else
				{
					bool flag2 = false;
					string[] dataBandsUsedInPageTotals = report.DataBandsUsedInPageTotals;
					foreach (string text3 in dataBandsUsedInPageTotals)
					{
						if (text3 == name2)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						string[] array = new string[report.DataBandsUsedInPageTotals.Length + 1];
						for (int j = 0; j < report.DataBandsUsedInPageTotals.Length; j++)
						{
							array[j] = report.DataBandsUsedInPageTotals[j];
						}
						array[report.DataBandsUsedInPageTotals.Length] = name2;
						report.DataBandsUsedInPageTotals = array;
					}
				}
			}
		}
		if (stiFunctionType == StiFunctionType.IIF && args.Count == 3 && ProcessIifAsTernary)
		{
			asmList.AddRange(args[0]);
			StiAsmCommand stiAsmCommand = new StiAsmCommand(StiAsmCommandType.JumpFalse, 0);
			asmList.Add(stiAsmCommand);
			int count = asmList.Count;
			asmList.AddRange(args[1]);
			StiAsmCommand stiAsmCommand2 = new StiAsmCommand(StiAsmCommandType.Jump, 0);
			asmList.Add(stiAsmCommand2);
			int count2 = asmList.Count;
			asmList.AddRange(args[2]);
			stiAsmCommand.Parameter1 = count2 - count;
			stiAsmCommand2.Parameter1 = asmList.Count - count2;
			return args.Count;
		}
		int num2 = 0;
		if (ParametersList.Contains(name))
		{
			num2 = (int)ParametersList[name];
		}
		int num3 = 1;
		foreach (List<StiAsmCommand> item2 in args)
		{
			if ((num2 & num3) > 0)
			{
				asmList.Add(new StiAsmCommand(StiAsmCommandType.PushValue, item2));
			}
			else
			{
				asmList.AddRange(item2);
			}
			num3 <<= 1;
		}
		return args.Count;
	}

	private List<List<StiAsmCommand>> get_args()
	{
		List<List<StiAsmCommand>> list = new List<List<StiAsmCommand>>();
		get_token();
		if (currentToken.Type != StiTokenType.LParenthesis)
		{
			ThrowError(ParserErrorCode.LeftParenthesisExpected);
		}
		get_token();
		if (currentToken.Type == StiTokenType.RParenthesis)
		{
			return list;
		}
		tokenPos--;
		currentToken = tokensList[tokenPos - 1];
		List<StiAsmCommand> list2 = asmList;
		do
		{
			asmList = new List<StiAsmCommand>();
			eval_exp0();
			list.Add(asmList);
		}
		while (currentToken.Type == StiTokenType.Comma);
		asmList = list2;
		if (currentToken.Type != StiTokenType.RParenthesis)
		{
			ThrowError(ParserErrorCode.RightParenthesisExpected);
		}
		return list;
	}

	private void get_token()
	{
		if (tokenPos < tokensList.Count)
		{
			currentToken = tokensList[tokenPos];
		}
		else
		{
			currentToken = new StiToken();
		}
		tokenPos++;
	}
}
