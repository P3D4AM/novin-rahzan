using System;
using System.CodeDom;
using System.Collections.Generic;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.CodeDom;

internal class StiCodeDomFunctions
{
	internal static string ParseFunctions(StiCodeDomSerializator serializator, string text)
	{
		return ParseFunctions(serializator, null, text);
	}

	internal static string ParseFunctions(StiCodeDomSerializator serializator, string propertyName, string text)
	{
		string text2 = text;
		ParseFunctions(serializator, propertyName, null, ref text2, null, isScript: true);
		return text2;
	}

	internal static bool ParseFunctions(StiCodeDomSerializator serializator, string propertyName, StiComponent comp, ref string text, StiExpression expression)
	{
		return ParseFunctions(serializator, propertyName, comp, ref text, expression, isScript: false);
	}

	internal static bool ParseFunctions(StiCodeDomSerializator serializator, string propertyName, StiComponent comp, ref string text, StiExpression expression, bool isScript)
	{
		string text2 = text;
		int functionIndex = 1;
		bool? flag = null;
		do
		{
			text2 = text;
			bool flag2 = ParseFunctionsInternal(ref functionIndex, serializator, propertyName, comp, ref text, expression, isScript);
			if (!flag.HasValue || !flag2)
			{
				flag = flag2;
			}
		}
		while (text2 != text);
		return flag.GetValueOrDefault();
	}

	private static bool ParseFunctionsInternal(ref int functionIndex, StiCodeDomSerializator serializator, string propertyName, StiComponent comp, ref string text, StiExpression expression, bool isScript)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Invalid comparison between Unknown and I4
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Expected O, but got Unknown
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0660: Expected O, but got Unknown
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_066e: Expected O, but got Unknown
		//IL_0673: Expected O, but got Unknown
		//IL_066e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Expected O, but got Unknown
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Expected O, but got Unknown
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_0742: Expected O, but got Unknown
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_074f: Expected O, but got Unknown
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0773: Expected O, but got Unknown
		//IL_0773: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Expected O, but got Unknown
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_077f: Expected O, but got Unknown
		bool flag = false;
		if ((int)serializator.report.EngineVersion == 2 && comp != null && StiSubReport.GetSubReportForPage(comp.Page) == null)
		{
			int num = -1;
			while ((num = text.IndexOf("cSum(", num + 1, StringComparison.InvariantCulture)) > 0)
			{
				if (num > 0 && (char.IsLetterOrDigit(text[num - 1]) || text[num - 1] == '_' || text[num - 1] == '.'))
				{
					continue;
				}
				try
				{
					bool storeToPrint = false;
					if (StiParser.ParseTextValue(text, comp, comp, ref storeToPrint, executeIfStoreToPrint: false, returnAsmList: true) is List<StiParser.StiAsmCommand> list)
					{
						foreach (StiParser.StiAsmCommand item in list)
						{
							if (item.Type == StiParser.StiAsmCommandType.PushComponent && item.Position == num && item.Length == -1)
							{
								if (item.Parameter1 is StiComponent stiComponent && !string.IsNullOrWhiteSpace(stiComponent.Name))
								{
									text = text.Insert(num + 5, stiComponent.Name + ",");
								}
								break;
							}
						}
					}
				}
				catch
				{
				}
				text = text.Insert(num, "Totals.");
				num += 12;
				flag = true;
			}
			num = -1;
			while ((num = text.IndexOf("cSumRunning(", num + 1, StringComparison.InvariantCulture)) > 0)
			{
				if (num > 0 && (char.IsLetterOrDigit(text[num - 1]) || text[num - 1] == '_' || text[num - 1] == '.'))
				{
					continue;
				}
				try
				{
					bool storeToPrint2 = false;
					if (StiParser.ParseTextValue(text, comp, comp, ref storeToPrint2, executeIfStoreToPrint: false, returnAsmList: true) is List<StiParser.StiAsmCommand> list2)
					{
						foreach (StiParser.StiAsmCommand item2 in list2)
						{
							if (item2.Type == StiParser.StiAsmCommandType.PushComponent && item2.Position == num && item2.Length == -1)
							{
								if (item2.Parameter1 is StiComponent stiComponent2 && !string.IsNullOrWhiteSpace(stiComponent2.Name))
								{
									text = text.Insert(num + 12, stiComponent2.Name + ",");
								}
								break;
							}
						}
					}
				}
				catch
				{
				}
				text = text.Insert(num, "Totals.");
				num += 12;
				flag = true;
			}
		}
		List<StiFunctionsParser.StiFunctionParam> list3 = StiFunctionsParser.Parse(serializator.report, comp, ref text, isScript, expression, isScript);
		if (list3.Count > 0)
		{
			int num2 = 0;
			List<StiFunctionsParser.StiFunctionParam> list4 = new List<StiFunctionsParser.StiFunctionParam>();
			foreach (StiFunctionsParser.StiFunctionParam item3 in list3)
			{
				if (item3.StartOfArgs < item3.EndOfArgs)
				{
					string text2 = text.Substring(item3.StartOfArgs, item3.EndOfArgs - item3.StartOfArgs + 1);
					string text3 = text2;
					List<StiFunctionsParser.StiFunctionParam> list5 = StiFunctionsParser.Parse(serializator.report, comp, ref text2, isScript: true, expression, isScript);
					if (text2 != text3)
					{
						text = text.Remove(item3.StartOfArgs, item3.EndOfArgs - item3.StartOfArgs + 1);
						text = text.Insert(item3.StartOfArgs, text2);
						int num3 = text2.Length - text3.Length;
						foreach (StiFunctionsParser.StiFunctionParam item4 in list5)
						{
							item4.Index += num3;
						}
						int index = item3.Index;
						for (int i = num2 + 1; i < list3.Count; i++)
						{
							StiFunctionsParser.StiFunctionParam stiFunctionParam = list3[i];
							if (stiFunctionParam.Index >= index)
							{
								stiFunctionParam.Index += num3;
								stiFunctionParam.StartOfArgs += num3;
								stiFunctionParam.EndOfArgs += num3;
							}
						}
					}
					foreach (StiFunctionsParser.StiFunctionParam item5 in list5)
					{
						item5.Index += item3.StartOfArgs;
					}
					list4.AddRange(list5);
				}
				num2++;
			}
			list3.AddRange(list4);
			bool flag2 = flag;
			int num4 = 1;
			int length = text.Length;
			num2 = 0;
			foreach (StiFunctionsParser.StiFunctionParam item6 in list3)
			{
				if (item6.AggregateFunction != null)
				{
					flag2 = true;
					string text4 = item6.ComponentName;
					StiComponent stiComponent3 = null;
					StiComponent stiComponent4 = null;
					if (comp != null)
					{
						stiComponent4 = comp.GetGroupHeaderBand();
					}
					if (string.IsNullOrEmpty(text4) && comp != null)
					{
						stiComponent3 = comp.GetGroupHeaderBand();
						if (stiComponent3 == null)
						{
							stiComponent3 = comp.GetDataBand();
						}
						if (stiComponent3 != null)
						{
							text4 = stiComponent3.Name;
						}
					}
					string text5 = null;
					if (item6.IsContainerFunc)
					{
						if (!string.IsNullOrEmpty(text4) && stiComponent3 == null)
						{
							stiComponent3 = serializator.components[text4] as StiDataBand;
						}
						if (stiComponent3 != null)
						{
							text5 = ((serializator.report == null || (int)serializator.report.EngineVersion != 2) ? stiComponent3.GetContainer().Name : stiComponent3.Page.Name);
						}
					}
					string text6 = null;
					text6 = ((comp == null) ? (text4 + "_" + item6.FunctionName) : (comp.Name + "_" + item6.FunctionName));
					if (stiComponent4 != null && stiComponent4.Name == item6.ComponentName && item6.IsRunningTotal && item6.FunctionName.StartsWith("Sum"))
					{
						text6 += "Running";
					}
					if (list3.Count > 1)
					{
						text6 += functionIndex;
					}
					functionIndex++;
					if (expression is StiExcelValueExpression)
					{
						text6 += "_ExcelValue";
					}
					CodeCastExpression expression2 = new CodeCastExpression(item6.AggregateFunction.GetResultType(), (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeTypeReferenceExpression("StiReport"), "ChangeType", (CodeExpression[])(object)new CodeExpression[3]
					{
						(CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text6), "GetValue", (CodeExpression[])(object)new CodeExpression[0]),
						(CodeExpression)new CodeTypeOfExpression(item6.AggregateFunction.GetResultType()),
						(CodeExpression)new CodePrimitiveExpression((object)true)
					}));
					string value = StiCodeDomSerializator.ConvertExpressionToString(serializator.generator, (CodeExpression)(object)expression2);
					int index2 = item6.Index;
					int length2 = text.Length;
					text = text.Remove(item6.Index, item6.Length);
					text = text.Insert(item6.Index, value);
					length2 = text.Length - length2;
					for (int j = num2 + 1; j < list3.Count; j++)
					{
						StiFunctionsParser.StiFunctionParam stiFunctionParam2 = list3[j];
						if (stiFunctionParam2.Index >= index2)
						{
							stiFunctionParam2.Index += length2;
						}
					}
					serializator.AddDeclare(((object)item6.AggregateFunction).GetType(), text6);
					if (item6.IsRunningTotal)
					{
						serializator.Statements.Add((CodeStatement)new CodeCommentStatement(text6));
						CodeExpression val = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text6);
						CodeAssignStatement val2 = new CodeAssignStatement(val, (CodeExpression)new CodeObjectCreateExpression(((object)item6.AggregateFunction).GetType(), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodePrimitiveExpression((object)true) }));
						serializator.Statements.Add((CodeStatement)(object)val2);
					}
					else
					{
						serializator.AddCreate(num4 - 1, string.Empty, item6.AggregateFunction, text6, ((object)item6.AggregateFunction).GetType(), isList: false);
					}
					string runningFooterBandName = null;
					if (item6.IsRunningTotal)
					{
						StiContainer parent = comp.Parent;
						while (parent != null && !(parent is StiBand))
						{
							parent = parent.Parent;
						}
						if (parent is StiFooterBand && ((StiFooterBand)parent).PrintOnAllPages && (parent.Parent == null || !(parent.Parent is StiPage)))
						{
							runningFooterBandName = parent.Name;
						}
					}
					serializator.TestProcessAtEnd(comp, text);
					item6.Argument = ParseFunctions(serializator, item6.Argument);
					item6.Condition = ParseFunctions(serializator, item6.Condition);
					if (StiOptions.Engine.AllowFixSumTimeArgumentConversion && item6.FunctionName == "SumTime" && !string.IsNullOrEmpty(item6.Argument))
					{
						try
						{
							int num5 = item6.Argument.LastIndexOf('.');
							if (num5 != -1)
							{
								bool storeToPrint3 = false;
								object obj3 = StiParser.ParseTextValue("{" + item6.Argument + "}", comp, ref storeToPrint3, executeIfStoreToPrint: false, returnAsmList: true);
								if (obj3 is List<StiParser.StiAsmCommand> { Count: 1 } list6 && (list6[0].Type == StiParser.StiAsmCommandType.PushDataSourceField || list6[0].Type == StiParser.StiAsmCommandType.PushBusinessObjectField))
								{
									string[] array = item6.Argument.Split(new char[1] { '.' });
									bool flag3 = false;
									if (list6[0].Type == StiParser.StiAsmCommandType.PushDataSourceField)
									{
										StiDataSource stiDataSource = comp.Report.Dictionary.DataSources[array[^2]];
										if (stiDataSource != null)
										{
											StiDataColumn stiDataColumn = stiDataSource.Columns[array[^1]];
											if (stiDataColumn != null && stiDataColumn.Type == typeof(TimeSpan) && !(stiDataColumn is StiCalcDataColumn))
											{
												flag3 = true;
											}
										}
									}
									if (list6[0].Type == StiParser.StiAsmCommandType.PushBusinessObjectField)
									{
										StiBusinessObject stiBusinessObject = comp.Report.Dictionary.BusinessObjects[array[^2]];
										if (stiBusinessObject != null)
										{
											StiDataColumn stiDataColumn2 = stiBusinessObject.Columns[array[^1]];
											if (stiDataColumn2 != null && stiDataColumn2.Type == typeof(TimeSpan))
											{
												flag3 = true;
											}
										}
									}
									if (flag3)
									{
										string text7 = ((serializator.report.ScriptLanguage == StiReportLanguageType.CSharp || serializator.report.ScriptLanguage == StiReportLanguageType.JS) ? "[]" : "()");
										item6.Argument = item6.Argument.Substring(0, num5) + text7[0] + "\"" + item6.Argument.Substring(num5 + 1) + "\"" + text7[1];
									}
								}
							}
						}
						catch
						{
						}
					}
					if (StiOptions.Engine.AllowFixDateTimeArgumentConversion && !string.IsNullOrWhiteSpace(item6.Argument) && (item6.FunctionName == "AvgDate" || item6.FunctionName == "AvgTime" || item6.FunctionName == "MaxDate" || item6.FunctionName == "MaxTime" || item6.FunctionName == "MinDate" || item6.FunctionName == "MinTime" || item6.FunctionName == "First" || item6.FunctionName == "Last" || item6.FunctionName == "SumTime"))
					{
						try
						{
							int num6 = item6.Argument.LastIndexOf('.');
							if (num6 != -1)
							{
								bool storeToPrint4 = false;
								object obj5 = StiParser.ParseTextValue("{" + item6.Argument + "}", comp, ref storeToPrint4, executeIfStoreToPrint: false, returnAsmList: true);
								if (obj5 is List<StiParser.StiAsmCommand> { Count: 1 } list7 && (list7[0].Type == StiParser.StiAsmCommandType.PushDataSourceField || list7[0].Type == StiParser.StiAsmCommandType.PushBusinessObjectField))
								{
									string[] array2 = item6.Argument.Split(new char[1] { '.' });
									bool flag4 = false;
									if (list7[0].Type == StiParser.StiAsmCommandType.PushDataSourceField)
									{
										StiDataSource stiDataSource2 = comp.Report.Dictionary.DataSources[array2[^2]];
										if (stiDataSource2 != null)
										{
											StiDataColumn stiDataColumn3 = stiDataSource2.Columns[array2[^1]];
											if (stiDataColumn3 != null && !(stiDataColumn3 is StiCalcDataColumn) && (stiDataColumn3.Type == typeof(DateTime) || stiDataColumn3.Type == typeof(TimeSpan)))
											{
												flag4 = true;
											}
										}
									}
									if (list7[0].Type == StiParser.StiAsmCommandType.PushBusinessObjectField)
									{
										StiBusinessObject stiBusinessObject2 = comp.Report.Dictionary.BusinessObjects[array2[^2]];
										if (stiBusinessObject2 != null)
										{
											StiDataColumn stiDataColumn4 = stiBusinessObject2.Columns[array2[^1]];
											if (stiDataColumn4 != null && !(stiDataColumn4 is StiCalcDataColumn) && (stiDataColumn4.Type == typeof(DateTime) || stiDataColumn4.Type == typeof(TimeSpan)))
											{
												flag4 = true;
											}
										}
									}
									if (flag4)
									{
										string text8 = ((serializator.report.ScriptLanguage == StiReportLanguageType.CSharp || serializator.report.ScriptLanguage == StiReportLanguageType.JS) ? "[]" : "()");
										item6.Argument = item6.Argument.Substring(0, num6) + text8[0] + "\"" + item6.Argument.Substring(num6 + 1) + "\"" + text8[1];
									}
								}
							}
						}
						catch
						{
						}
					}
					serializator.AddToRemittedBuild(text4, comp, text6, item6.Argument, text, item6.Condition, item6.AggregateFunction, item6.Expression, item6.IsRunningTotal);
					if (stiComponent4 != null && item6.ComponentName != null)
					{
						text4 = stiComponent4.Name;
					}
					if (text5 != null)
					{
						StiRemit stiRemit = serializator.AddToRemittedSE(text5, comp, text6, item6.Argument, text, item6.AggregateFunction, usePrintEvent: true, item6.IsColumnFunc, item6.IsRunningTotal, item6.Expression);
						stiRemit.RunningFooterBandName = runningFooterBandName;
					}
					else
					{
						StiRemit stiRemit2 = serializator.AddToRemittedSE(text4, comp, text6, item6.Argument, text, item6.AggregateFunction, usePrintEvent: false, item6.IsColumnFunc, item6.IsRunningTotal, item6.Expression);
						stiRemit2.RunningFooterBandName = runningFooterBandName;
						StiDataBand dataBand = comp.GetDataBand();
						if (dataBand != null)
						{
							string text9 = text4;
							if (text4.IndexOf(":", StringComparison.InvariantCulture) != -1)
							{
								string[] array3 = text4.Split(new char[1] { ':' });
								text9 = array3[0];
							}
							if (dataBand.Name != text9 && IsBandHasDetailWithName(dataBand, text9))
							{
								StiRemit stiRemit3 = serializator.AddToRemittedSE(dataBand.Name, comp, text6, item6.Argument, text, item6.AggregateFunction, usePrintEvent: false, item6.IsColumnFunc, item6.IsRunningTotal, item6.Expression);
								stiRemit3.ConnectOnlyBegin = true;
							}
						}
					}
				}
				else if (item6.Function != null && item6.Function.UseFullPath)
				{
					string value2 = $"{item6.Function.TypeOfFunction.ToString()}.{item6.Function.FunctionName}";
					int index3 = item6.Index;
					int length3 = text.Length;
					text = text.Remove(item6.Index, item6.Length);
					text = text.Insert(item6.Index, value2);
					length3 = text.Length - length3;
					for (int k = num2 + 1; k < list3.Count; k++)
					{
						StiFunctionsParser.StiFunctionParam stiFunctionParam3 = list3[k];
						if (stiFunctionParam3.Index >= index3)
						{
							stiFunctionParam3.Index += length3;
						}
					}
				}
				num2++;
				num4++;
			}
			if (flag2)
			{
				return false;
			}
			return !serializator.TestProcessAtEnd(comp, text);
		}
		if (propertyName == "Text" || propertyName == "ExcelValue")
		{
			return !serializator.TestProcessAtEnd(comp, text);
		}
		return false;
	}

	private static bool IsBandHasDetailWithName(StiDataBand dataBand, string compName)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((int)dataBand.Report.EngineVersion != 2)
		{
			return false;
		}
		StiDataBandV2Builder stiDataBandV2Builder = new StiDataBandV2Builder();
		stiDataBandV2Builder.FindDetailDataBands(dataBand);
		foreach (StiDataBand detailDataBand in dataBand.DataBandInfoV2.DetailDataBands)
		{
			if (detailDataBand.Name == compName)
			{
				return true;
			}
			if (IsBandHasDetailWithName(detailDataBand, compName))
			{
				return true;
			}
		}
		return false;
	}
}
