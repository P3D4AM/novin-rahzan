using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.CodeDom;

internal class StiCodeDomFilters
{
	internal static object ConvertValueToObject(StiCodeDomSerializator serializator, StiFilterDataType type, string value)
	{
		string numberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		switch (type)
		{
		case StiFilterDataType.Numeric:
			return decimal.Parse(value.Replace(".", ",").Replace(",", numberDecimalSeparator));
		case StiFilterDataType.DateTime:
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			if (!DateTime.TryParse(value, out var result))
			{
				result = DateTime.Now;
			}
			Thread.CurrentThread.CurrentCulture = currentCulture;
			return result;
		}
		case StiFilterDataType.Boolean:
			return value.ToLower(CultureInfo.InvariantCulture) == "true";
		default:
			return value.ToLower(CultureInfo.InvariantCulture);
		}
	}

	internal static CodeExpression GetCodeExpressionFromFilterValue(StiCodeDomSerializator serializator, StiFilterDataType dataType, string textValue)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_004d: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		if (dataType != StiFilterDataType.Expression)
		{
			object obj = ConvertValueToObject(serializator, dataType, textValue);
			if (obj is DateTime)
			{
				return serializator.GetObjectCreateExpression(typeof(DateTime), obj);
			}
			if (!(obj is string))
			{
				return (CodeExpression)new CodePrimitiveExpression(obj);
			}
			return (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeThisReferenceExpression(), "ToString", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodePrimitiveExpression(obj) }), "ToLower", (CodeExpression[])(object)new CodeExpression[0]);
		}
		return (CodeExpression)new CodeSnippetExpression(textValue);
	}

	internal static CodeExpression GetFilterExpression(StiCodeDomSerializator serializator, IStiFilter filterComp, StiFilter filter, bool isConditions, bool isSingle = false)
	{
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01af: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Expected O, but got Unknown
		//IL_027d: Expected O, but got Unknown
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Expected O, but got Unknown
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Expected O, but got Unknown
		//IL_02bc: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Expected O, but got Unknown
		//IL_02f6: Expected O, but got Unknown
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Expected O, but got Unknown
		//IL_032a: Expected O, but got Unknown
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Expected O, but got Unknown
		//IL_0364: Expected O, but got Unknown
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Expected O, but got Unknown
		//IL_0398: Expected O, but got Unknown
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Expected O, but got Unknown
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Expected O, but got Unknown
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Expected O, but got Unknown
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Expected O, but got Unknown
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f8: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Expected O, but got Unknown
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Expected O, but got Unknown
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Expected O, but got Unknown
		//IL_04c7: Expected O, but got Unknown
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Expected O, but got Unknown
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Expected O, but got Unknown
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Expected O, but got Unknown
		IStiDataSource stiDataSource = filterComp as IStiDataSource;
		try
		{
			if (filter.Item == StiFilterItem.Expression)
			{
				string text = filter.Expression.Value.Trim();
				if (text.StartsWith("{", StringComparison.InvariantCulture))
				{
					text = text.Substring(1);
				}
				if (text.EndsWith("}", StringComparison.InvariantCulture))
				{
					text = text.Substring(0, text.Length - 1);
				}
				if (string.IsNullOrWhiteSpace(text))
				{
					return null;
				}
				text = StiCodeDomFunctions.ParseFunctions(serializator, text);
				text = StiCodeDomTotalsFunctionsParser.ProcessTotals(serializator, text, null);
				if ((text.StartsWith("(") && text.EndsWith(")")) || isSingle)
				{
					return (CodeExpression)new CodeSnippetExpression(text);
				}
				return (CodeExpression)new CodeSnippetExpression("(" + text + ")");
			}
			string text2 = string.Empty;
			if (!isConditions && stiDataSource != null && !string.IsNullOrEmpty(stiDataSource.DataSourceName))
			{
				text2 = StiNameValidator.CorrectName(stiDataSource.DataSourceName, serializator.report);
			}
			if (filterComp is StiVirtualSource)
			{
				text2 = StiNameValidator.CorrectName(((StiVirtualSource)filterComp).NameInSource, serializator.report);
			}
			if (filter.Column.Length > 0)
			{
				string text3 = filter.Column;
				if (text2.Length > 0)
				{
					text3 = text2 + "." + filter.Column;
				}
				if (filter.Condition == StiFilterCondition.IsNull || filter.Condition == StiFilterCondition.IsNotNull)
				{
					CodeExpression val = null;
					int num = text3.LastIndexOf('.');
					if (num < 0)
					{
						val = (CodeExpression)new CodeSnippetExpression(text3);
					}
					else
					{
						string text4 = text3.Substring(0, num);
						string text5 = text3.Substring(num + 1);
						val = (CodeExpression)new CodeIndexerExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text4), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodePrimitiveExpression((object)text5) });
					}
					CodeBinaryOperatorExpression val2 = new CodeBinaryOperatorExpression(val, (CodeBinaryOperatorType)7, (CodeExpression)new CodePrimitiveExpression((object)null));
					CodeBinaryOperatorExpression val3 = new CodeBinaryOperatorExpression(val, (CodeBinaryOperatorType)7, (CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeTypeReferenceExpression("DBNull"), "Value"));
					CodeExpression val4 = (CodeExpression)new CodeBinaryOperatorExpression((CodeExpression)(object)val2, (CodeBinaryOperatorType)11, (CodeExpression)(object)val3);
					return (CodeExpression)((filter.Condition == StiFilterCondition.IsNull) ? ((object)val4) : ((object)new CodeBinaryOperatorExpression(val4, (CodeBinaryOperatorType)8, (CodeExpression)new CodePrimitiveExpression((object)false))));
				}
				CodeExpression val5 = GetCodeExpressionFromFilterValue(serializator, filter.DataType, filter.Value1);
				if (filter.Condition != StiFilterCondition.BeginningWith && filter.Condition != StiFilterCondition.EndingWith && filter.Condition != StiFilterCondition.Containing && filter.Condition != StiFilterCondition.NotContaining)
				{
					CodeExpression val6 = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text3);
					CodeExpression val7 = val6;
					if (filter.DataType == StiFilterDataType.String)
					{
						val7 = (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeMethodInvokeExpression(val7, "ToString", (CodeExpression[])(object)new CodeExpression[0]), "ToLower", (CodeExpression[])(object)new CodeExpression[0]);
					}
					else if (filter.DataType != StiFilterDataType.Expression)
					{
						object obj = ConvertValueToObject(serializator, filter.DataType, filter.Value1);
						val7 = (CodeExpression)new CodeCastExpression(obj.GetType(), (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeTypeReferenceExpression("StiReport"), "ChangeType", (CodeExpression[])(object)new CodeExpression[3]
						{
							val6,
							(CodeExpression)new CodeTypeOfExpression(obj.GetType()),
							(CodeExpression)new CodePrimitiveExpression((object)true)
						}));
					}
					if (filter.Condition == StiFilterCondition.Between || filter.Condition == StiFilterCondition.NotBetween)
					{
						CodeExpression codeExpressionFromFilterValue = GetCodeExpressionFromFilterValue(serializator, filter.DataType, filter.Value2);
						CodeExpression val8 = (CodeExpression)new CodeBinaryOperatorExpression(val7, (CodeBinaryOperatorType)16, val5);
						CodeExpression val9 = (CodeExpression)new CodeBinaryOperatorExpression(val7, (CodeBinaryOperatorType)14, codeExpressionFromFilterValue);
						CodeExpression val10 = (CodeExpression)new CodeBinaryOperatorExpression(val8, (CodeBinaryOperatorType)12, val9);
						if (filter.Condition == StiFilterCondition.Between)
						{
							return val10;
						}
						return (CodeExpression)new CodeBinaryOperatorExpression(val10, (CodeBinaryOperatorType)7, (CodeExpression)new CodePrimitiveExpression((object)false));
					}
					CodeBinaryOperatorType val11 = (CodeBinaryOperatorType)12;
					switch (filter.Condition)
					{
					case StiFilterCondition.GreaterThan:
						val11 = (CodeBinaryOperatorType)15;
						break;
					case StiFilterCondition.GreaterThanOrEqualTo:
						val11 = (CodeBinaryOperatorType)16;
						break;
					case StiFilterCondition.LessThan:
						val11 = (CodeBinaryOperatorType)13;
						break;
					case StiFilterCondition.LessThanOrEqualTo:
						val11 = (CodeBinaryOperatorType)14;
						break;
					case StiFilterCondition.EqualTo:
						val11 = (CodeBinaryOperatorType)8;
						break;
					case StiFilterCondition.NotEqualTo:
						val11 = (CodeBinaryOperatorType)6;
						break;
					}
					return (CodeExpression)new CodeBinaryOperatorExpression(val7, val11, val5);
				}
				if (filter.DataType == StiFilterDataType.Expression)
				{
					val5 = (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeThisReferenceExpression(), "ToString", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeSnippetExpression(filter.Value1) }), "ToLower", (CodeExpression[])(object)new CodeExpression[0]);
				}
				if (filter.Condition == StiFilterCondition.Containing)
				{
					CodeMethodInvokeExpression val12 = new CodeMethodInvokeExpression((CodeExpression)new CodeThisReferenceExpression(), "ToString", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text3) });
					return (CodeExpression)new CodeBinaryOperatorExpression((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)(object)val12, "ToLower", (CodeExpression[])(object)new CodeExpression[0]), "IndexOf", (CodeExpression[])(object)new CodeExpression[1] { val5 }), (CodeBinaryOperatorType)6, (CodeExpression)new CodePrimitiveExpression((object)(-1)));
				}
				if (filter.Condition == StiFilterCondition.NotContaining)
				{
					CodeExpression val13 = (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeThisReferenceExpression(), "ToString", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text3) });
					return (CodeExpression)new CodeBinaryOperatorExpression((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeMethodInvokeExpression(val13, "ToLower", (CodeExpression[])(object)new CodeExpression[0]), "IndexOf", (CodeExpression[])(object)new CodeExpression[1] { val5 }), (CodeBinaryOperatorType)8, (CodeExpression)new CodePrimitiveExpression((object)(-1)));
				}
				if (filter.Condition == StiFilterCondition.BeginningWith)
				{
					CodeExpression val14 = (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeThisReferenceExpression(), "ToString", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text3) });
					return (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeMethodInvokeExpression(val14, "ToLower", (CodeExpression[])(object)new CodeExpression[0]), "StartsWith", (CodeExpression[])(object)new CodeExpression[1] { val5 });
				}
				if (filter.Condition == StiFilterCondition.EndingWith)
				{
					CodeExpression val15 = (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeThisReferenceExpression(), "ToString", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text3) });
					return (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeMethodInvokeExpression(val15, "ToLower", (CodeExpression[])(object)new CodeExpression[0]), "EndsWith", (CodeExpression[])(object)new CodeExpression[1] { val5 });
				}
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(typeof(StiCodeDomFilters), e);
		}
		return null;
	}

	internal static CodeExpression AddFilters(StiCodeDomSerializator serializator, IStiFilter filterComp)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		if (filterComp == null)
		{
			return null;
		}
		StiFilterMode filterMode = filterComp.FilterMode;
		StiFiltersCollection filters = filterComp.Filters;
		if (filters == null || filters.Count == 0)
		{
			return null;
		}
		List<CodeExpression> list = new List<CodeExpression>();
		foreach (StiFilter item in filters)
		{
			CodeExpression filterExpression = GetFilterExpression(serializator, filterComp, item, isConditions: false, filters.Count == 1);
			if (filterExpression != null)
			{
				list.Add(filterExpression);
			}
		}
		CodeBinaryOperatorType val = (CodeBinaryOperatorType)12;
		if (filterMode == StiFilterMode.Or)
		{
			val = (CodeBinaryOperatorType)11;
		}
		CodeExpression val2 = null;
		if (list.Count == 0)
		{
			return null;
		}
		foreach (CodeExpression item2 in list)
		{
			val2 = (CodeExpression)((val2 != null) ? ((object)new CodeBinaryOperatorExpression(val2, val, item2)) : ((object)item2));
		}
		return val2;
	}

	internal static void AddFilters(StiCodeDomSerializator serializator, string parent, IStiFilter filterComp)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		CodeExpression val = AddFilters(serializator, filterComp);
		if (val != null)
		{
			StiGetFilterEvent stiGetFilterEvent = new StiGetFilterEvent();
			string eventName = parent + "__" + stiGetFilterEvent?.ToString() + string.Empty;
			CodeStatementCollection val2 = new CodeStatementCollection();
			val2.Add((CodeStatement)new CodeCommentStatement(StiCodeDomSerializator.GetCheckerInfoString(parent, "Filter")));
			val2.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeArgumentReferenceExpression("e"), "Value"), val));
			serializator.GenEventMethod(eventName, stiGetFilterEvent, val2);
		}
	}
}
