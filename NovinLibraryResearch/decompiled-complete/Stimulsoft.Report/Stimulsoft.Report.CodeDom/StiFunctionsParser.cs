using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.CodeDom;

internal class StiFunctionsParser
{
	internal class StiFunctionParam
	{
		public string ComponentName;

		public string Argument;

		public string Condition;

		public string FunctionName;

		public StiFunction Function;

		public StiAggregateFunctionService AggregateFunction;

		public int Index;

		public int Length;

		public int StartOfArgs;

		public int EndOfArgs;

		public bool IsContainerFunc;

		public bool IsColumnFunc;

		public bool IsRunningTotal;

		public StiExpression Expression;

		public StiFunctionParam(string componentName, string argument, string condition, string functionName, StiAggregateFunctionService function, int index, int length, bool isContainerFunc, bool isColumnFunc, bool isRunningTotal, StiExpression expression)
		{
			ComponentName = componentName;
			Argument = argument;
			Condition = condition;
			FunctionName = functionName;
			AggregateFunction = function;
			Index = index;
			Length = length;
			IsContainerFunc = isContainerFunc;
			IsColumnFunc = isColumnFunc;
			IsRunningTotal = isRunningTotal;
			Expression = expression;
		}
	}

	private static string GetParsContent(StiLexer lexer)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Invalid comparison between Unknown and I4
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Invalid comparison between Unknown and I4
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder stringBuilder = new StringBuilder("(");
		StiToken token = lexer.GetToken();
		StiTokenType lastType = (StiTokenType)0;
		while ((int)token.Type != 42 && (int)token.Type != 34)
		{
			if ((int)token.Type == 33)
			{
				stringBuilder = stringBuilder.Append(GetParsContent(lexer));
			}
			else
			{
				if (CheckForNeedWhiteSpace(lastType, token))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder = stringBuilder.Append(lexer.Text.ToString().Substring(token.Index, token.Length));
			}
			lastType = token.Type;
			token = lexer.GetToken();
		}
		stringBuilder = stringBuilder.Append(")");
		return stringBuilder.ToString();
	}

	private static List<string> GetArguments(StiLexer lexer, ref int startOfArgs, ref int endOfArgs)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Invalid comparison between Unknown and I4
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Invalid comparison between Unknown and I4
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		StiToken token = lexer.GetToken();
		startOfArgs = token.Index;
		List<string> list = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		StiTokenType lastType = (StiTokenType)0;
		while ((int)token.Type != 42 && (int)token.Type != 34)
		{
			if ((int)token.Type == 2)
			{
				list.Add(stringBuilder.ToString());
				stringBuilder = new StringBuilder();
			}
			else if ((int)token.Type == 33)
			{
				stringBuilder = stringBuilder.Append(GetParsContent(lexer));
			}
			else
			{
				if (CheckForNeedWhiteSpace(lastType, token))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder = stringBuilder.Append(lexer.Text.ToString().Substring(token.Index, token.Length));
			}
			lastType = token.Type;
			token = lexer.GetToken();
		}
		if (stringBuilder.Length > 0)
		{
			list.Add(stringBuilder.ToString());
		}
		endOfArgs = token.Index - 1;
		return list;
	}

	private static bool CheckForNeedWhiteSpace(StiTokenType lastType, StiToken token)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Invalid comparison between Unknown and I4
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		if ((int)token.Type == 40)
		{
			if ((int)lastType == 40 || (int)lastType == 41)
			{
				return true;
			}
			if ((int)lastType == 39)
			{
				string text = token.Data as string;
				if (!string.IsNullOrWhiteSpace(text))
				{
					text = text.ToLowerInvariant();
				}
				switch (text)
				{
				case "f":
				case "d":
				case "m":
				case "u":
				case "l":
				case "ul":
					return false;
				default:
					return true;
				}
			}
		}
		if ((int)lastType == 40 && ((int)token.Type == 39 || (int)token.Type == 41))
		{
			return true;
		}
		return false;
	}

	public static List<StiFunctionParam> Parse(StiReport report, StiComponent ownerComponent, ref string text, StiExpression expression, bool checkTotals)
	{
		return Parse(report, ownerComponent, ref text, isScript: false, expression, checkTotals);
	}

	public static List<StiFunctionParam> Parse(StiReport report, StiComponent ownerComponent, ref string text, bool isScript, StiExpression expression, bool checkTotals)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0bc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bce: Invalid comparison between Unknown and I4
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Invalid comparison between Unknown and I4
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Invalid comparison between Unknown and I4
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Invalid comparison between Unknown and I4
		//IL_0b92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b98: Expected O, but got Unknown
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Invalid comparison between Unknown and I4
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Invalid comparison between Unknown and I4
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Invalid comparison between Unknown and I4
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Invalid comparison between Unknown and I4
		//IL_0713: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Expected O, but got Unknown
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Expected O, but got Unknown
		List<StiFunctionParam> list = new List<StiFunctionParam>();
		if (!isScript && (text.IndexOf('{') == -1 || text.IndexOf('}') == -1))
		{
			return list;
		}
		StiLexer val = new StiLexer(text)
		{
			IsVB = (report.ScriptLanguage == StiReportLanguageType.VB)
		};
		StiToken val2 = new StiToken((StiTokenType)42);
		int num = 0;
		int num2 = 0;
		try
		{
			if (!isScript && val.PositionInText < val.Text.Length && val.Text[val.PositionInText] == '"')
			{
				val2 = new StiToken((StiTokenType)39, val.PositionInText, 1);
				int positionInText = val.PositionInText;
				val.PositionInText = positionInText + 1;
				val2.Data = '"';
			}
			else
			{
				val2 = val.GetToken();
			}
		}
		catch
		{
		}
		bool flag = isScript;
		while ((int)val2.Type != 42)
		{
			if (!flag)
			{
				if ((int)val2.Type == 35 && !isScript)
				{
					flag = true;
				}
			}
			else if ((int)val2.Type == 36)
			{
				if (!isScript)
				{
					flag = false;
				}
			}
			else if ((int)val2.Type == 40)
			{
				bool flag2 = false;
				bool flag3 = false;
				bool isColumnFunc = false;
				bool flag4 = false;
				string text2 = (string)val2.Data;
				StiVariable stiVariable = report.Dictionary.Variables[text2];
				if (stiVariable != null)
				{
					val.SavePosToken();
					val.SavePosToken();
					StiToken token = val.GetToken();
					StiToken token2 = val.GetToken();
					val.UngetToken();
					val.UngetToken();
					if ((int)token.Type == 1 && (int)token2.Type == 40 && token2.Data as string == "Label")
					{
						string text3 = val2.Data as string;
						string text4 = "GetLabel(\"" + text3 + "\")";
						text = text.Remove(val2.Index, val2.Length + token.Length + token2.Length);
						text = text.Insert(val2.Index, text4);
						val.PositionInText += text4.Length - val2.Length - token.Length - token2.Length;
						val2.Length = text4.Length;
						val.Text = new StringBuilder(text);
					}
				}
				if (text2.EndsWith("If", StringComparison.InvariantCulture))
				{
					text2 = text2.Substring(0, text2.Length - 2);
					flag3 = true;
					if (text2.EndsWith("Running", StringComparison.InvariantCulture))
					{
						text2 = text2.Substring(0, text2.Length - 7);
						flag4 = true;
					}
				}
				if (text2.EndsWith("Running", StringComparison.InvariantCulture))
				{
					text2 = text2.Substring(0, text2.Length - 7);
					flag4 = true;
					if (text2.EndsWith("If", StringComparison.InvariantCulture))
					{
						text2 = text2.Substring(0, text2.Length - 2);
						flag3 = true;
					}
				}
				if (text2.StartsWith("col", StringComparison.InvariantCulture))
				{
					isColumnFunc = true;
					text2 = text2.Substring(3);
				}
				else if (text2.StartsWith("c", StringComparison.InvariantCulture))
				{
					flag2 = true;
					text2 = text2.Substring(1);
				}
				if (!flag2 && ownerComponent != null && flag4)
				{
					StiComponent parent = ownerComponent.Parent;
					while (parent != null && !(parent is StiPage) && !(parent is StiHeaderBand) && !(parent is StiFooterBand))
					{
						parent = parent.Parent;
					}
					if (parent is StiHeaderBand || parent is StiFooterBand)
					{
						StiDataBand stiDataBand = null;
						if (parent is StiHeaderBand stiHeaderBand)
						{
							stiDataBand = (((int)report.EngineVersion != 1) ? StiHeaderBandV2Builder.GetMaster(stiHeaderBand) : (StiHeaderBandV1Builder.GetMaster(stiHeaderBand) as StiDataBand));
							if (stiDataBand != null && !stiHeaderBand.PrintOnAllPages)
							{
								stiDataBand = null;
								flag4 = false;
							}
						}
						if (parent is StiFooterBand stiFooterBand)
						{
							stiDataBand = (((int)report.EngineVersion != 1) ? StiFooterBandV2Builder.GetMaster(stiFooterBand) : (StiFooterBandV1Builder.GetMaster(stiFooterBand) as StiDataBand));
							if (stiDataBand != null && !stiFooterBand.PrintOnAllPages)
							{
								stiDataBand = null;
								flag4 = false;
							}
						}
						if (stiDataBand != null && stiDataBand.MasterComponent == null)
						{
							flag2 = true;
						}
					}
					if (ownerComponent.Parent is StiPageHeaderBand || ownerComponent.Parent is StiPageFooterBand)
					{
						flag2 = true;
					}
				}
				StiAggregateFunctionService function = StiAggregateFunctionService.GetFunction(StiOptions.Services.AggregateFunctions.Where((StiAggregateFunctionService s) => ((StiService)s).ServiceEnabled).ToList(), text2);
				try
				{
					if (function != null)
					{
						int i = val2.Index + val2.Length;
						bool flag5 = false;
						for (; i < text.Length; i++)
						{
							if (!char.IsWhiteSpace(text[i]))
							{
								if (text[i] == '(')
								{
									flag5 = true;
								}
								break;
							}
						}
						if (flag5 && isScript && checkTotals)
						{
							int j;
							for (j = i + 1; j < text.Length && char.IsWhiteSpace(text, j); j++)
							{
							}
							if (j >= text.Length)
							{
								flag5 = false;
							}
							if (text[j] == ')')
							{
								flag5 = false;
							}
							if (flag5)
							{
								j = val2.Index - 1;
								while (j >= 0 && char.IsWhiteSpace(text, j))
								{
									j--;
								}
								if (j >= 0 && text[j] == '.')
								{
									flag5 = false;
								}
							}
						}
						if (flag5)
						{
							if (isScript && checkTotals)
							{
								string text5 = ((!flag2) ? $"Totals.{text2}" : $"Totals.c{text2}");
								if (flag4)
								{
									text5 += "Running";
								}
								text = text.Remove(val2.Index, val2.Length);
								text = text.Insert(val2.Index, text5);
								val.PositionInText += text5.Length - val2.Length;
								val2.Length = text5.Length;
								val.Text = new StringBuilder(text);
								int startOfArgs = 0;
								int endOfArgs = 0;
								StiLexer val3 = new StiLexer(val.Text.ToString());
								val3.PositionInText = val.PositionInText;
								List<string> arguments = GetArguments(val3, ref startOfArgs, ref endOfArgs);
								string text6 = arguments[0];
								if (text6.Contains(","))
								{
									text6 = text6.Substring(0, text6.IndexOf(','));
								}
								text6 = text6.Replace("(", "").Replace(")", "").Replace("{", "")
									.Replace("}", "");
								if (report.DataBandsUsedInPageTotals == null)
								{
									report.DataBandsUsedInPageTotals = new string[1];
									report.DataBandsUsedInPageTotals[0] = text6;
								}
								else
								{
									bool flag6 = false;
									string[] dataBandsUsedInPageTotals = report.DataBandsUsedInPageTotals;
									foreach (string text7 in dataBandsUsedInPageTotals)
									{
										if (text7 == text6)
										{
											flag6 = true;
											break;
										}
									}
									if (!flag6)
									{
										string[] array = new string[report.DataBandsUsedInPageTotals.Length + 1];
										for (int l = 0; l < report.DataBandsUsedInPageTotals.Length; l++)
										{
											array[l] = report.DataBandsUsedInPageTotals[l];
										}
										array[report.DataBandsUsedInPageTotals.Length] = text6;
										report.DataBandsUsedInPageTotals = array;
									}
								}
							}
							else
							{
								if (flag2)
								{
									int startOfArgs2 = 0;
									int endOfArgs2 = 0;
									StiLexer val4 = new StiLexer(val.Text.ToString());
									val4.PositionInText = val.PositionInText + 1;
									List<string> arguments2 = GetArguments(val4, ref startOfArgs2, ref endOfArgs2);
									string text8 = arguments2.FirstOrDefault();
									if (text8 == null)
									{
										text8 = string.Empty;
									}
									if (text8.Contains(","))
									{
										text8 = text8.Substring(0, text8.IndexOf(','));
									}
									text8 = text8.Replace("(", "").Replace(")", "").Replace("{", "")
										.Replace("}", "");
									if (report.DataBandsUsedInPageTotals == null)
									{
										report.DataBandsUsedInPageTotals = new string[1];
										report.DataBandsUsedInPageTotals[0] = text8;
									}
									else
									{
										bool flag7 = false;
										string[] dataBandsUsedInPageTotals2 = report.DataBandsUsedInPageTotals;
										foreach (string text9 in dataBandsUsedInPageTotals2)
										{
											if (text9 == text8)
											{
												flag7 = true;
												break;
											}
										}
										if (!flag7)
										{
											string[] array2 = new string[report.DataBandsUsedInPageTotals.Length + 1];
											for (int n = 0; n < report.DataBandsUsedInPageTotals.Length; n++)
											{
												array2[n] = report.DataBandsUsedInPageTotals[n];
											}
											array2[report.DataBandsUsedInPageTotals.Length] = text8;
											report.DataBandsUsedInPageTotals = array2;
										}
									}
								}
								num = val2.Index;
								if (val.WaitLparen2() && ((num > 0 && text[num - 1] != '.') || num == 0))
								{
									int startOfArgs3 = 0;
									int endOfArgs3 = 0;
									List<string> arguments3 = GetArguments(val, ref startOfArgs3, ref endOfArgs3);
									num2 = val.PositionInText - num;
									string componentName = string.Empty;
									string argument = string.Empty;
									string condition = string.Empty;
									if (arguments3.Count > 0)
									{
										if (flag3)
										{
											if (function.RecureParam)
											{
												if (arguments3.Count == 3)
												{
													componentName = arguments3[0];
													argument = arguments3[1];
													condition = arguments3[2];
												}
												if (arguments3.Count == 2)
												{
													argument = arguments3[0];
													condition = arguments3[1];
												}
												else if (arguments3.Count == 1)
												{
													argument = arguments3[0];
													condition = "true";
												}
											}
											else if (arguments3.Count == 2)
											{
												componentName = arguments3[0];
												condition = arguments3[1];
											}
											else if (arguments3.Count == 1)
											{
												condition = arguments3[0];
											}
										}
										else if (function.RecureParam)
										{
											if (arguments3.Count == 2)
											{
												componentName = arguments3[0];
												argument = arguments3[1];
											}
											else if (arguments3.Count == 1)
											{
												argument = arguments3[0];
											}
											else if (arguments3.Count == 3 && text2 == "SumDistinct")
											{
												componentName = arguments3[0];
												argument = arguments3[1];
												condition = arguments3[2];
											}
										}
										else
										{
											componentName = arguments3[0];
										}
									}
									list.Add(new StiFunctionParam(componentName, argument, condition, ((StiService)function).ServiceName, function, num, num2, flag2, isColumnFunc, flag4, expression));
								}
							}
						}
					}
					else
					{
						StiFunction[] functions = StiFunctions.GetFunctions(report, text2, isCompile: true);
						if (functions != null)
						{
							num = val2.Index;
							if (val.WaitLparen2() && ((num > 0 && text[num - 1] != '.') || num == 0))
							{
								int startOfArgs4 = 0;
								int endOfArgs4 = 0;
								List<string> arguments4 = GetArguments(val, ref startOfArgs4, ref endOfArgs4);
								num2 = val2.Length;
								StiFunctionParam stiFunctionParam = new StiFunctionParam(string.Empty, string.Empty, string.Empty, text2, function, num, num2, isContainerFunc: false, isColumnFunc: false, isRunningTotal: false, expression);
								stiFunctionParam.StartOfArgs = startOfArgs4;
								stiFunctionParam.EndOfArgs = endOfArgs4;
								StiFunction[] array3 = functions;
								foreach (StiFunction stiFunction in array3)
								{
									if (stiFunction.ArgumentTypes == null)
									{
										if (arguments4.Count == 0)
										{
											stiFunctionParam.Function = stiFunction;
											break;
										}
									}
									else if (stiFunction.ArgumentTypes.Length == arguments4.Count)
									{
										stiFunctionParam.Function = stiFunction;
									}
								}
								if (stiFunctionParam.Function == null && functions.Length != 0)
								{
									stiFunctionParam.Function = functions[0];
								}
								if (stiFunctionParam.Function != null)
								{
									list.Add(stiFunctionParam);
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					StiLogService.Write(typeof(StiFunctionsParser), "Parse Aggregate...ERROR");
					StiLogService.Write(typeof(StiFunctionsParser), ex.Message);
				}
			}
			try
			{
				if (!flag && val.PositionInText < val.Text.Length && val.Text[val.PositionInText] == '"')
				{
					val2 = new StiToken((StiTokenType)39, val.PositionInText, 1);
					int positionInText = val.PositionInText;
					val.PositionInText = positionInText + 1;
					val2.Data = '"';
				}
				val2 = val.GetToken();
			}
			catch
			{
				return list;
			}
		}
		return list;
	}
}
