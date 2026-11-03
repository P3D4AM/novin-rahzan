using System;
using System.CodeDom;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.CodeDom;

internal class StiCodeDomVariables
{
	internal static void Serialize(StiCodeDomSerializator serializator, StiReport report)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		if (report.Dictionary.Variables.Count <= 0)
		{
			return;
		}
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			CodeObjectCreateExpression val = new CodeObjectCreateExpression(variable.GetType(), serializator.GetArguments(variable.GetType(), variable));
			serializator.memberMethod.Statements.Add((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "Dictionary"), "Variables"), "Add", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)val }));
		}
		SerializeVariablesToCode(serializator, report);
	}

	private static string ConvertArgumentFromString(StiCodeDomSerializator serializator, Type argumentType, string argumentStr)
	{
		if (argumentType == typeof(string))
		{
			argumentStr = serializator.generator.QuoteSnippetString(argumentStr);
			if (!argumentStr.StartsWith("\"", StringComparison.InvariantCulture) && !argumentStr.StartsWith("@\"", StringComparison.InvariantCulture))
			{
				argumentStr = "\"" + argumentStr;
			}
			if (!argumentStr.EndsWith("\"", StringComparison.InvariantCulture))
			{
				argumentStr += "\"";
			}
			return argumentStr;
		}
		if (argumentType == typeof(Guid) || argumentType == typeof(Guid?))
		{
			return "new Guid(\"" + argumentStr + "\")";
		}
		if (argumentType == typeof(double) || argumentType == typeof(double?))
		{
			argumentStr = argumentStr.Replace(",", ".");
			if (!argumentStr.EndsWith("d", StringComparison.InvariantCulture) && !argumentStr.EndsWith("D", StringComparison.InvariantCulture))
			{
				argumentStr += "d";
			}
			return argumentStr;
		}
		if (argumentType == typeof(float) || argumentType == typeof(float?))
		{
			argumentStr = argumentStr.Replace(",", ".");
			if (!argumentStr.EndsWith("f", StringComparison.InvariantCulture) && !argumentStr.EndsWith("F", StringComparison.InvariantCulture))
			{
				argumentStr += "f";
			}
			return argumentStr;
		}
		if (argumentType == typeof(decimal) || argumentType == typeof(decimal?))
		{
			argumentStr = argumentStr.Replace(",", ".");
			if ((serializator.report.ScriptLanguage == StiReportLanguageType.CSharp || serializator.report.ScriptLanguage == StiReportLanguageType.JS) && !argumentStr.EndsWith("m", StringComparison.InvariantCulture) && !argumentStr.EndsWith("M", StringComparison.InvariantCulture))
			{
				argumentStr += "m";
			}
			return argumentStr;
		}
		if (argumentType == typeof(char) || argumentType == typeof(char?))
		{
			if (string.IsNullOrEmpty(argumentStr))
			{
				argumentStr = " ";
			}
			if (serializator.report.ScriptLanguage == StiReportLanguageType.CSharp || serializator.report.ScriptLanguage == StiReportLanguageType.JS)
			{
				if (!argumentStr.StartsWith("'", StringComparison.InvariantCulture))
				{
					argumentStr = "'" + argumentStr;
				}
				if (!argumentStr.EndsWith("'", StringComparison.InvariantCulture))
				{
					argumentStr += "'";
				}
				if (argumentStr.Length > 3)
				{
					argumentStr = argumentStr.Substring(0, 2) + "'";
				}
			}
			else
			{
				argumentStr = ((argumentStr.Length > 0) ? $"Microsoft.VisualBasic.ChrW({(int)argumentStr[0]})" : "Microsoft.VisualBasic.ChrW(32)");
			}
			return argumentStr;
		}
		return argumentStr;
	}

	private static void SerializeVariablesToCode(StiCodeDomSerializator serializator, StiReport report)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_127f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1285: Invalid comparison between Unknown and I4
		//IL_131a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1320: Invalid comparison between Unknown and I4
		//IL_128a: Unknown result type (might be due to invalid IL or missing references)
		//IL_128f: Unknown result type (might be due to invalid IL or missing references)
		//IL_129a: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a6: Expected O, but got Unknown
		//IL_12bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c5: Expected O, but got Unknown
		//IL_12cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e9: Expected O, but got Unknown
		//IL_12ee: Expected O, but got Unknown
		//IL_12e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f3: Expected O, but got Unknown
		//IL_1325: Unknown result type (might be due to invalid IL or missing references)
		//IL_132a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1335: Unknown result type (might be due to invalid IL or missing references)
		//IL_1341: Expected O, but got Unknown
		//IL_1356: Unknown result type (might be due to invalid IL or missing references)
		//IL_1360: Expected O, but got Unknown
		//IL_1367: Unknown result type (might be due to invalid IL or missing references)
		//IL_137e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1384: Expected O, but got Unknown
		//IL_1389: Expected O, but got Unknown
		//IL_1384: Unknown result type (might be due to invalid IL or missing references)
		//IL_138e: Expected O, but got Unknown
		//IL_0e99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea0: Expected O, but got Unknown
		//IL_0e8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e95: Expected O, but got Unknown
		//IL_0fe1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff3: Expected O, but got Unknown
		//IL_0edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef5: Expected O, but got Unknown
		//IL_0efe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f04: Expected O, but got Unknown
		//IL_0f09: Expected O, but got Unknown
		//IL_0f04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0e: Expected O, but got Unknown
		//IL_0f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f10: Expected O, but got Unknown
		//IL_0ec2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec9: Expected O, but got Unknown
		//IL_103c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1046: Expected O, but got Unknown
		//IL_104d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1065: Expected O, but got Unknown
		//IL_1060: Unknown result type (might be due to invalid IL or missing references)
		//IL_106c: Expected O, but got Unknown
		//IL_1067: Unknown result type (might be due to invalid IL or missing references)
		//IL_1071: Expected O, but got Unknown
		//IL_100a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7a: Expected O, but got Unknown
		//IL_0f7c: Expected O, but got Unknown
		//IL_0f94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9e: Expected O, but got Unknown
		//IL_0fa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb2: Expected O, but got Unknown
		//IL_115e: Unknown result type (might be due to invalid IL or missing references)
		//IL_116d: Expected O, but got Unknown
		//IL_1168: Unknown result type (might be due to invalid IL or missing references)
		//IL_1186: Unknown result type (might be due to invalid IL or missing references)
		//IL_118c: Expected O, but got Unknown
		//IL_118e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1194: Expected O, but got Unknown
		//IL_119d: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a3: Expected O, but got Unknown
		//IL_11a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b6: Expected O, but got Unknown
		//IL_11b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b7: Expected O, but got Unknown
		//IL_11bc: Expected O, but got Unknown
		//IL_11b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11be: Expected O, but got Unknown
		//IL_11c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d6: Expected O, but got Unknown
		//IL_11d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f5: Expected O, but got Unknown
		//IL_11f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fd: Expected O, but got Unknown
		//IL_1206: Unknown result type (might be due to invalid IL or missing references)
		//IL_120c: Expected O, but got Unknown
		//IL_1211: Expected O, but got Unknown
		//IL_120c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1213: Expected O, but got Unknown
		//IL_1233: Unknown result type (might be due to invalid IL or missing references)
		//IL_1244: Expected O, but got Unknown
		//IL_123f: Unknown result type (might be due to invalid IL or missing references)
		//IL_124b: Expected O, but got Unknown
		//IL_1246: Unknown result type (might be due to invalid IL or missing references)
		//IL_1250: Expected O, but got Unknown
		//IL_122b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1232: Expected O, but got Unknown
		if (report.Dictionary.Variables.Count <= 0)
		{
			return;
		}
		CodeStatementCollection val = new CodeStatementCollection();
		CodeStatementCollection val2 = new CodeStatementCollection();
		serializator.Statements.Add((CodeStatement)new CodeCommentStatement(""));
		serializator.Statements.Add((CodeStatement)new CodeCommentStatement("Variables init"));
		serializator.Statements.Add((CodeStatement)new CodeCommentStatement(""));
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			if (string.IsNullOrEmpty(variable.Name))
			{
				continue;
			}
			string text = ((variable.Value != null) ? variable.Value.Trim() : "");
			if (typeof(IStiList).IsAssignableFrom(variable.Type))
			{
				text = string.Empty;
			}
			if (text.Length == 0)
			{
				if (variable.Type == typeof(int) || variable.Type == typeof(uint) || variable.Type == typeof(long) || variable.Type == typeof(ulong) || variable.Type == typeof(byte) || variable.Type == typeof(sbyte) || variable.Type == typeof(short) || variable.Type == typeof(ushort))
				{
					text = "0";
				}
				else if (variable.Type == typeof(string))
				{
					text = "\"\"";
				}
				else if (variable.Type == typeof(bool))
				{
					text = "false";
				}
				else if (variable.Type == typeof(char))
				{
					text = ((report.ScriptLanguage == StiReportLanguageType.CSharp || report.ScriptLanguage == StiReportLanguageType.JS) ? "' '" : "Microsoft.VisualBasic.ChrW(32)");
				}
				else if (variable.Type == typeof(double))
				{
					text = "0d";
				}
				else if (variable.Type == typeof(float))
				{
					text = "0f";
				}
				else if (variable.Type == typeof(decimal))
				{
					text = ((report.ScriptLanguage == StiReportLanguageType.CSharp || report.ScriptLanguage == StiReportLanguageType.JS) ? "0m" : "0");
				}
				else if (variable.Type == typeof(DateTime))
				{
					text = "DateTime.Now";
				}
				else if (variable.Type == typeof(int?) || variable.Type == typeof(uint?) || variable.Type == typeof(long?) || variable.Type == typeof(ulong?) || variable.Type == typeof(byte?) || variable.Type == typeof(sbyte?) || variable.Type == typeof(short?) || variable.Type == typeof(ushort?))
				{
					text = "0";
				}
				else if (variable.Type == typeof(bool?))
				{
					text = "false";
				}
				else if (variable.Type == typeof(char?))
				{
					text = ((report.ScriptLanguage == StiReportLanguageType.CSharp || report.ScriptLanguage == StiReportLanguageType.JS) ? "' '" : "Microsoft.VisualBasic.ChrW(32)");
				}
				else if (variable.Type == typeof(double?))
				{
					text = "0d";
				}
				else if (variable.Type == typeof(float?))
				{
					text = "0f";
				}
				else if (variable.Type == typeof(decimal?))
				{
					text = ((report.ScriptLanguage == StiReportLanguageType.CSharp || report.ScriptLanguage == StiReportLanguageType.JS) ? "0m" : "0");
				}
				else if (variable.Type == typeof(DateTime?))
				{
					text = ((report.ScriptLanguage == StiReportLanguageType.CSharp || report.ScriptLanguage == StiReportLanguageType.JS) ? "null" : "Nothing");
				}
				else if (variable.Type == typeof(TimeSpan?))
				{
					text = ((report.ScriptLanguage == StiReportLanguageType.CSharp || report.ScriptLanguage == StiReportLanguageType.JS) ? "null" : "Nothing");
				}
				else if (variable.Type == typeof(Guid) || variable.Type == typeof(Guid?))
				{
					text = "new Guid()";
				}
				else if (!StiTypeFinder.FindType(variable.Type, typeof(Range)))
				{
					text = ((report.ScriptLanguage != 0 && report.ScriptLanguage != StiReportLanguageType.JS) ? ((!StiTypeFinder.FindType(variable.Type, typeof(Array))) ? $"CType(Activator.CreateInstance(GetType({serializator.GetTypeString(variable.Type)})), {variable.Type})" : ("New " + serializator.GetTypeString(variable.Type.GetElementType()) + "(0-0){}")) : ((!StiTypeFinder.FindType(variable.Type, typeof(Array))) ? $"({serializator.GetTypeString(variable.Type)})Activator.CreateInstance(typeof({variable.Type}))" : ("new " + serializator.GetTypeString(variable.Type.GetElementType()) + "[0]")));
				}
				else
				{
					Range range = StiActivator.CreateObject(variable.Type) as Range;
					text = "new " + range.RangeName + "()";
				}
			}
			else if (variable.InitBy == StiVariableInitBy.Value)
			{
				bool flag = variable.Type == typeof(DateTime);
				bool flag2 = variable.Type == typeof(TimeSpan);
				if (variable.Type == typeof(DateTime?))
				{
					flag = true;
				}
				if (variable.Type == typeof(TimeSpan?))
				{
					flag2 = true;
				}
				if (variable.Type == typeof(string))
				{
					text = serializator.generator.QuoteSnippetString(text);
					if (!text.StartsWith("\"", StringComparison.InvariantCulture) && !text.StartsWith("@\"", StringComparison.InvariantCulture))
					{
						text = "\"" + text;
					}
					if (!text.EndsWith("\"", StringComparison.InvariantCulture))
					{
						text += "\"";
					}
				}
				else if (!StiTypeFinder.FindType(variable.Type, typeof(Image)))
				{
					if (variable.Type == typeof(Guid) || variable.Type == typeof(Guid?))
					{
						text = "new Guid(\"" + variable.Value + "\")";
					}
					else if (StiTypeFinder.FindType(variable.Type, typeof(Range)))
					{
						Range range2 = variable.ValueObject as Range;
						DateTimeRange dateTimeRange = range2 as DateTimeRange;
						TimeSpanRange timeSpanRange = range2 as TimeSpanRange;
						if (dateTimeRange != null)
						{
							CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
							try
							{
								Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
								string text2 = (dateTimeRange.From.HasValue ? $"ParseDateTime(\"{dateTimeRange.FromObject}\")" : ((serializator.report.ScriptLanguage == StiReportLanguageType.CSharp || serializator.report.ScriptLanguage == StiReportLanguageType.JS) ? "null" : "Nothing"));
								string text3 = (dateTimeRange.To.HasValue ? $"ParseDateTime(\"{dateTimeRange.ToObject}\")" : ((serializator.report.ScriptLanguage == StiReportLanguageType.CSharp || serializator.report.ScriptLanguage == StiReportLanguageType.JS) ? "null" : "Nothing"));
								text = "new " + dateTimeRange.RangeName + "(" + text2 + ", " + text3 + ")";
							}
							finally
							{
								Thread.CurrentThread.CurrentCulture = currentCulture;
							}
						}
						else if (timeSpanRange != null)
						{
							CultureInfo currentCulture2 = Thread.CurrentThread.CurrentCulture;
							try
							{
								Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
								string text4 = (timeSpanRange.From.HasValue ? $"ParseDateTime(\"{timeSpanRange.FromObject}\").TimeOfDay" : ((serializator.report.ScriptLanguage == StiReportLanguageType.CSharp || serializator.report.ScriptLanguage == StiReportLanguageType.JS) ? "null" : "Nothing"));
								string text5 = (timeSpanRange.To.HasValue ? $"ParseDateTime(\"{timeSpanRange.ToObject}\").TimeOfDay" : ((serializator.report.ScriptLanguage == StiReportLanguageType.CSharp || serializator.report.ScriptLanguage == StiReportLanguageType.JS) ? "null" : "Nothing"));
								text = "new " + timeSpanRange.RangeName + "(" + text4 + ", " + text5 + ")";
							}
							finally
							{
								Thread.CurrentThread.CurrentCulture = currentCulture2;
							}
						}
						else
						{
							text = $"new {range2.RangeName}({ConvertArgumentFromString(serializator, range2.FromObject.GetType(), range2.FromObject.ToString())}, {ConvertArgumentFromString(serializator, range2.ToObject.GetType(), range2.ToObject.ToString())})";
						}
					}
					else if (flag)
					{
						if (!StringExt.StartsWithInvariant(text, "\"") && char.IsDigit(text[0]))
						{
							text = "ParseDateTime(\"" + variable.GetNativeValue() + "\")";
						}
					}
					else if (flag2)
					{
						if (!StringExt.StartsWithInvariant(text, "\"") && char.IsDigit(text[0]))
						{
							text = "ParseTimeSpan(\"" + variable.GetNativeValue() + "\")";
						}
					}
					else if (variable.Type == typeof(double) || variable.Type == typeof(double?))
					{
						text = text.Replace(",", ".");
						if (!StringExt.EndsWithInvariant(text, "d") && !StringExt.EndsWithInvariant(text, "D"))
						{
							text += "d";
						}
					}
					else if (variable.Type == typeof(float) || variable.Type == typeof(float?))
					{
						text = text.Replace(",", ".");
						if (!StringExt.EndsWithInvariant(text, "f") && !StringExt.EndsWithInvariant(text, "F"))
						{
							text += "f";
						}
					}
					else if (variable.Type == typeof(decimal) || variable.Type == typeof(decimal?))
					{
						text = text.Replace(",", ".");
						if ((report.ScriptLanguage == StiReportLanguageType.CSharp || report.ScriptLanguage == StiReportLanguageType.JS) && !StringExt.EndsWithInvariant(text, "m") && !StringExt.EndsWithInvariant(text, "M"))
						{
							text += "m";
						}
					}
					else if (variable.Type == typeof(char) || variable.Type == typeof(char?))
					{
						if (string.IsNullOrEmpty(text))
						{
							text = " ";
						}
						if (report.ScriptLanguage == StiReportLanguageType.CSharp || report.ScriptLanguage == StiReportLanguageType.JS)
						{
							if (!StringExt.StartsWithInvariant(text, "'"))
							{
								text = "'" + text;
							}
							if (!StringExt.EndsWithInvariant(text, "'"))
							{
								text += "'";
							}
							if (text.Length > 3)
							{
								text = text.Substring(0, 2) + "'";
							}
						}
						else
						{
							text = ((text.Length > 0) ? $"Microsoft.VisualBasic.ChrW({(int)text[0]})" : "Microsoft.VisualBasic.ChrW(32)");
						}
					}
				}
			}
			if (!(variable.Type == typeof(Image)) || variable.InitBy != 0)
			{
				text = StiCodeDomFunctions.ParseFunctions(serializator, text);
				text = StiCodeDomTotalsFunctionsParser.ProcessTotals(serializator, text, variable.Name);
			}
			CodeExpression val3;
			if (StiTypeFinder.FindType(variable.Type, typeof(Range)) && variable.InitBy == StiVariableInitBy.Expression)
			{
				string script = StiCodeDomFunctions.ParseFunctions(serializator, variable.InitByExpressionFrom);
				string script2 = StiCodeDomFunctions.ParseFunctions(serializator, variable.InitByExpressionTo);
				script = StiCodeDomTotalsFunctionsParser.ProcessTotals(serializator, script, variable.Name);
				script2 = StiCodeDomTotalsFunctionsParser.ProcessTotals(serializator, script2, variable.Name);
				if (variable.Type == typeof(DateTimeRange) && variable.DialogInfo.DateTimeType == StiDateTimeType.Date && variable.InitBy == StiVariableInitBy.Expression)
				{
					script += ".Date";
					script2 += ".Date.AddDays(1).AddTicks(-1)";
				}
				Range range3 = Activator.CreateInstance(variable.Type) as Range;
				val3 = (CodeExpression)new CodeSnippetExpression("new " + range3.RangeName + "(" + script + ", " + script2 + ")");
			}
			else
			{
				val3 = (CodeExpression)new CodeSnippetExpression(text);
			}
			if (StiTypeFinder.FindType(variable.Type, typeof(Image)))
			{
				val3 = (CodeExpression)((variable.ValueObject != null) ? ((object)new CodeCastExpression(serializator.GetTypeString(variable.Type), (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeTypeReferenceExpression("StiVariableLoader"), "GetImage", (CodeExpression[])(object)new CodeExpression[2]
				{
					(CodeExpression)new CodeThisReferenceExpression(),
					(CodeExpression)new CodePrimitiveExpression((object)variable.Name)
				}))) : ((object)new CodePrimitiveExpression((object)null)));
			}
			if (variable.ReadOnly && !StiTypeFinder.FindType(variable.Type, typeof(Image)))
			{
				CodeMemberProperty val4 = new CodeMemberProperty
				{
					Attributes = (MemberAttributes)24576,
					Name = StiNameValidator.CorrectName(variable.Name, report),
					HasGet = true,
					Type = new CodeTypeReference(serializator.GetTypeString(variable.Type))
				};
				val4.GetStatements.Add((CodeStatement)new CodeCommentStatement(StiCodeDomSerializator.GetCheckerInfoString(variable.Name, "Value")));
				val4.GetStatements.Add((CodeStatement)new CodeMethodReturnStatement(val3));
				serializator.Members.Add((CodeTypeMember)(object)val4);
				continue;
			}
			CodeMemberField val5 = new CodeMemberField(serializator.GetTypeString(variable.Type), StiNameValidator.CorrectName(variable.Name, report))
			{
				Attributes = (MemberAttributes)24576
			};
			if (Array.IndexOf(StiCodeDomSerializator.ReservedWords, variable.Name) != -1)
			{
				((CodeTypeMember)val5).Attributes = (MemberAttributes)(((CodeTypeMember)val5).Attributes | 0x10);
			}
			serializator.Members.Add((CodeTypeMember)(object)val5);
			serializator.Statements.Add((CodeStatement)new CodeCommentStatement(StiCodeDomSerializator.GetCheckerInfoString(variable.Name, "Value")));
			serializator.Statements.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), StiNameValidator.CorrectName(variable.Name, report)), val3));
			if (!StiTypeFinder.FindType(variable.Type, typeof(Image)))
			{
				string text6 = "";
				if (variable.Type == typeof(int))
				{
					text6 = "Int";
				}
				else if (variable.Type == typeof(long))
				{
					text6 = "Int64";
				}
				else if (variable.Type == typeof(bool))
				{
					text6 = "Bool";
				}
				else if (variable.Type == typeof(float))
				{
					text6 = "Float";
				}
				else if (variable.Type == typeof(double))
				{
					text6 = "Double";
				}
				else if (variable.Type == typeof(decimal))
				{
					text6 = "Decimal";
				}
				CodeMethodInvokeExpression val6 = new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeBaseReferenceExpression(), "States"), "Push" + text6, (CodeExpression[])(object)new CodeExpression[4]
				{
					(CodeExpression)new CodeArgumentReferenceExpression("stateName"),
					(CodeExpression)new CodeThisReferenceExpression(),
					(CodeExpression)new CodePrimitiveExpression((object)variable.Name),
					(CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), variable.Name)
				});
				val.Add((CodeExpression)(object)val6);
				CodeExpression val7 = (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeBaseReferenceExpression(), "States"), "Pop" + text6, (CodeExpression[])(object)new CodeExpression[3]
				{
					(CodeExpression)new CodeArgumentReferenceExpression("stateName"),
					(CodeExpression)new CodeThisReferenceExpression(),
					(CodeExpression)new CodePrimitiveExpression((object)variable.Name)
				});
				if (text6.Length == 0)
				{
					val7 = (CodeExpression)new CodeCastExpression(serializator.GetTypeString(variable.Type), val7);
				}
				val2.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), variable.Name), val7));
			}
		}
		if (((CollectionBase)(object)val).Count > 0 && (int)report.EngineVersion == 1)
		{
			CodeMemberMethod val8 = new CodeMemberMethod
			{
				Attributes = (MemberAttributes)24580,
				Name = "SaveState"
			};
			val8.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "stateName"));
			val8.Statements.Add((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeBaseReferenceExpression(), "SaveState", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeArgumentReferenceExpression("stateName") }));
			val8.Statements.AddRange(val);
			serializator.Members.Add((CodeTypeMember)(object)val8);
		}
		if (((CollectionBase)(object)val2).Count > 0 && (int)report.EngineVersion == 1)
		{
			CodeMemberMethod val8 = new CodeMemberMethod
			{
				Attributes = (MemberAttributes)24580,
				Name = "RestoreState"
			};
			val8.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "stateName"));
			val8.Statements.Add((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeBaseReferenceExpression(), "RestoreState", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeArgumentReferenceExpression("stateName") }));
			val8.Statements.AddRange(val2);
			serializator.Members.Add((CodeTypeMember)(object)val8);
		}
	}
}
