using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge;

namespace Stimulsoft.Report.CodeDom;

public class StiCodeDomSerializator
{
	private class StiEventItem
	{
		public string EventName;

		public string Name;

		public StiEvent Event;

		public string CodeOfRemit;

		public CodeStatementCollection Stats;

		public override string ToString()
		{
			return EventName;
		}

		public StiEventItem(string eventName, string name, StiEvent eventt, string codeOfRemit, CodeStatementCollection stats)
		{
			EventName = eventName;
			Name = name;
			Event = eventt;
			CodeOfRemit = codeOfRemit;
			Stats = stats;
		}
	}

	internal static string[] IdentsToEnd = new string[12]
	{
		"TotalPageCount", "PageNofM", "PageNumber", "TotalPageCountThrough", "PageNofMThrough", "PageNumberThrough", "IsFirstPage", "IsFirstPageThrough", "IsLastPage", "IsLastPageThrough",
		"GetAnchorPageNumber", "GetAnchorPageNumberThrough"
	};

	internal static string[] ReservedWords = new string[48]
	{
		"States", "PageNumber", "Line", "Column", "Date", "Time", "Today", "BusinessObjectsStore", "ReportDataSources", "Variables",
		"AggregateFunctions", "Dictionary", "DataSources", "DataStore", "Script", "Language", "ReportFile", "Load", "Save", "ExportDocument",
		"Design", "Render", "Show", "Print", "Click", "Paint", "Exporting", "Exported", "Printing", "Printed",
		"Unit", "Convert", "Container", "Site", "MetaTags", "ReportResources", "Engine", "Progress", "SubReports", "Tag",
		"Pages", "Info", "Designer", "Bookmark", "Password", "Styles", "PrinterSettings", "About"
	};

	public const string StrGenCode = "StiReport Designer generated code - do not modify";

	public const string StiCheckerInfoString = "CheckerInfo:";

	internal int TotalIndex = 1;

	internal CodeMemberMethod memberMethod;

	internal StiReport report;

	internal StiComponentsCollection components;

	private string thisReportTypeName;

	private ArrayList reference = new ArrayList();

	private ArrayList compsRemittedSE = new ArrayList();

	private ArrayList compsRemittedBuild = new ArrayList();

	private List<StiRemit> compsProcessAtEndRemitted = new List<StiRemit>();

	private List<StiRemit> compsWordsToEndRemitted = new List<StiRemit>();

	internal StiCodeDomProvider provider;

	internal StiCodeGenerator generator;

	private StiGraphs Graphs;

	internal CodeTypeMemberCollection Members = new CodeTypeMemberCollection();

	internal CodeStatementCollection Statements = new CodeStatementCollection();

	private CodeStatementCollection CollectionStatements = new CodeStatementCollection();

	private void GenGetValueEvent(StiEvent ev, string propName, string eventName, StiExpression expression, string parent, object comp)
	{
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Expected O, but got Unknown
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Expected O, but got Unknown
		string value = expression.Value;
		string text = string.Empty;
		if (comp is StiComponent)
		{
			text = StiNameValidator.CorrectName(((StiComponent)comp).Name, report);
		}
		bool flag = false;
		if (comp is IStiText)
		{
			flag = ((IStiText)comp).OnlyText;
		}
		string text2 = null;
		text2 = ((comp is StiRichText) ? XmlConvert.DecodeName(expression.Value) : expression.Value);
		bool flag2 = true;
		if (!flag && comp != null)
		{
			if (comp is StiSeries)
			{
				StiText comp2 = new StiText();
				StiCodeDomFunctions.ParseFunctions(this, propName, comp2, ref text2, expression);
			}
			if (comp is StiComponent)
			{
				flag2 = StiCodeDomFunctions.ParseFunctions(this, propName, comp as StiComponent, ref text2, expression);
			}
			text2 = StiCodeDomTotalsFunctionsParser.ProcessTotals(this, text2, parent);
		}
		StiFormatService format = null;
		if (comp is IStiTextFormat)
		{
			format = ((IStiTextFormat)comp).TextFormat;
		}
		if (comp is IStiCrossTabField)
		{
			format = null;
		}
		if (!expression.ApplyFormat)
		{
			format = null;
		}
		bool fullConvert = expression.FullConvert;
		bool isTextExpression = propName == "Text";
		if (!flag2 && (ev is StiGetValueEvent || ev is StiGetExcelValueEvent))
		{
			CodeExpression expr = null;
			if (comp is StiText && ((comp as StiText).CanShrink || (comp as StiText).CanGrow) && !text2.Contains("sender."))
			{
				expr = GetTextScriptExpression(format, text2, fullConvert, flag, text, null, isTextExpression: false);
			}
			GenGetValueEventMethod(eventName, storeToPrinted: true, "#%#" + value, ev, expr);
			if (expression.GenAddEvent)
			{
				GenAddEvent(eventName, parent, ev);
			}
			if (((CollectionBase)(object)Members).Count > 0)
			{
				string componentName = parent;
				if (comp is StiComponent)
				{
					componentName = (comp as StiComponent).Name;
				}
				if (comp is StiSeries && (comp as StiSeries).Chart is StiComponent stiComponent)
				{
					componentName = stiComponent.Name + ".Series";
				}
				CodeTypeMember obj = Members[((CollectionBase)(object)Members).Count - 1];
				CodeMemberMethod val = (CodeMemberMethod)(object)((obj is CodeMemberMethod) ? obj : null);
				if (val != null)
				{
					val.Statements.Insert(0, (CodeStatement)new CodeCommentStatement(GetCheckerInfoString(componentName, propName)));
				}
			}
			string text3 = "_GetValue_End";
			if (ev is StiGetExcelValueEvent)
			{
				text3 = "_GetExcelValue_End";
			}
			GenReturnMethod(text, text + text3, text2, fullConvert, flag, format, comp as StiComponent, isTextExpression);
		}
		else
		{
			if (expression.GenAddEvent)
			{
				GenAddEvent(eventName, parent, ev);
			}
			if (!(comp is StiComponent))
			{
				text = "Test";
			}
			GenGetValueEventMethod(text, eventName, format, text2, fullConvert, flag, ev, comp as StiComponent, isTextExpression);
		}
		if (((CollectionBase)(object)Members).Count > 0)
		{
			string componentName2 = parent;
			if (comp is StiComponent)
			{
				componentName2 = (comp as StiComponent).Name;
			}
			if (comp is StiSeries && (comp as StiSeries).Chart is StiComponent stiComponent2)
			{
				componentName2 = stiComponent2.Name + ".Series";
			}
			CodeTypeMember obj2 = Members[((CollectionBase)(object)Members).Count - 1];
			CodeMemberMethod val2 = (CodeMemberMethod)(object)((obj2 is CodeMemberMethod) ? obj2 : null);
			if (val2 != null)
			{
				val2.Statements.Insert(0, (CodeStatement)new CodeCommentStatement(GetCheckerInfoString(componentName2, propName)));
			}
		}
	}

	internal void GenAddEvent(string eventName, string parent, StiEvent ev)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003d: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		Type eventType = ev.GetEventType();
		CodeExpression val = null;
		val = (CodeExpression)((!string.IsNullOrEmpty(parent)) ? ((object)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parent)) : ((object)new CodeThisReferenceExpression()));
		CodeAttachEventStatement val2 = new CodeAttachEventStatement(val, ev.ToString(), (CodeExpression)new CodeDelegateCreateExpression(new CodeTypeReference(eventType), (CodeExpression)new CodeThisReferenceExpression(), eventName));
		Statements.Add((CodeStatement)(object)val2);
	}

	private void GenGetValueEventMethod(string compName, string eventName, StiFormatService format, string expression, bool fullConvert, bool onlyText, StiEvent ev, StiComponent component, bool isTextExpression)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ee: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_011c: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		bool flag = !(component is StiRichText) || eventName.EndsWith("_GetValue");
		CodeStatementCollection val = new CodeStatementCollection();
		CodeStatement val2 = (CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeArgumentReferenceExpression("e"), "Value"), GetTextScriptExpression(format, expression, fullConvert, onlyText, compName, flag ? component : null, isTextExpression));
		if (component is StiText && isTextExpression)
		{
			StiText stiText = component as StiText;
			if (stiText.Type == StiSystemTextType.DataColumn && !string.IsNullOrEmpty(stiText.NullValue) && expression.StartsWith("{", StringComparison.InvariantCulture) && expression.EndsWith("}", StringComparison.InvariantCulture) && expression.Length > 2)
			{
				expression = expression.Substring(1, expression.Length - 2);
				CodeAssignStatement val3 = new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeArgumentReferenceExpression("e"), "Value"), (CodeExpression)new CodePrimitiveExpression((object)stiText.NullValue));
				CodeMethodInvokeExpression val4 = new CodeMethodInvokeExpression((CodeExpression)new CodeTypeReferenceExpression("StiNullValuesHelper"), "IsNull", (CodeExpression[])(object)new CodeExpression[2]
				{
					(CodeExpression)new CodeThisReferenceExpression(),
					(CodeExpression)new CodePrimitiveExpression((object)expression)
				});
				CodeStatement[] array = (CodeStatement[])(object)new CodeAssignStatement[1] { val3 };
				val2 = (CodeStatement)new CodeConditionStatement((CodeExpression)(object)val4, array, (CodeStatement[])(object)new CodeStatement[1] { val2 });
			}
		}
		val.Add(val2);
		GenEventMethod(eventName, ev, val);
	}

	internal static string GetParentRelationName(StiReport report, string name)
	{
		string text = StiNameValidator.CorrectName(name, report);
		if (text.IndexOf("Parent", StringComparison.InvariantCulture) != 0)
		{
			text = "Parent" + text;
		}
		if (report != null && report.Dictionary != null && report.Dictionary.DataSources != null)
		{
			foreach (StiDataSource dataSource in report.Dictionary.DataSources)
			{
				if (dataSource.Name == text || StiNameValidator.CorrectName(dataSource.Name) == text)
				{
					text = "Parent" + text;
					break;
				}
			}
		}
		return text;
	}

	internal CodeExpression GetTextScriptExpression(StiFormatService format, string expression, bool fullConvert, bool onlyText, string compName, StiComponent component, bool isTextExpression)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_0168: Expected O, but got Unknown
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f9: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00ae: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		if (format is StiGeneralFormatService || format == null)
		{
			string text = StiCodeDomExpressionHelper.ConvertToString(generator, expression, fullConvert, onlyText, component, isTextExpression);
			return (CodeExpression)new CodeSnippetExpression(text);
		}
		string text2 = StiCodeDomExpressionHelper.ConvertToString(generator, expression, fullConvert: false, onlyText, component, isTextExpression);
		if (format is StiCustomFormatService)
		{
			CodeExpression val = null;
			if (format.IsFormatStringFromVariable)
			{
				val = (CodeExpression)new CodeSnippetExpression("\"{0:\" + " + format.StringFormat.Trim().Substring(1, format.StringFormat.Length - 2) + " + \"}\"");
				return (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeTypeReferenceExpression(typeof(string)), "Format", (CodeExpression[])(object)new CodeExpression[2]
				{
					val,
					(CodeExpression)new CodeSnippetExpression(text2)
				});
			}
			val = (CodeExpression)new CodePrimitiveExpression((object)("{0:" + format.StringFormat + "}"));
			return (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), compName), "TextFormat"), "Format", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeSnippetExpression(text2) });
		}
		if (component is StiText stiText && (stiText == null || stiText.ExcelValue.Value == null || stiText.ExcelValue.Value.Length <= 0))
		{
			text2 = "CheckExcelValue(sender, " + text2 + ")";
		}
		return (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), compName), "TextFormat"), "Format", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeSnippetExpression(text2) });
	}

	private static string RemoveComments(string text)
	{
		if (text == null || text.Length < 2 || text.Trim().Length < 2)
		{
			return text;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < text.Length; i++)
		{
			char c = text[i];
			if (!flag3)
			{
				if (flag)
				{
					if (c == '*' && i < text.Length - 1 && text[i + 1] == '/')
					{
						flag = false;
						i++;
					}
					continue;
				}
				if (flag2)
				{
					if (c != '\r' && c != '\n')
					{
						continue;
					}
					flag2 = false;
				}
				if (c == '/' && i < text.Length - 1 && text[i + 1] == '*')
				{
					flag = true;
					i++;
					continue;
				}
				if (c == '/' && i < text.Length - 1 && text[i + 1] == '/')
				{
					flag2 = true;
					i++;
					continue;
				}
			}
			switch (c)
			{
			case '\\':
				stringBuilder.Append(c);
				if (i < text.Length - 1)
				{
					i++;
					stringBuilder.Append(text[i]);
				}
				continue;
			case '"':
				flag3 = !flag3;
				break;
			}
			stringBuilder.Append(c);
		}
		return stringBuilder.ToString();
	}

	internal void GenEventMethod(string eventName, string script, StiEvent ev, string parent)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		CodeStatementCollection val = new CodeStatementCollection();
		if (StiOptions.Engine.RemoveCommentsFromEventScript)
		{
			script = RemoveComments(script);
		}
		script = StiCodeDomTotalsFunctionsParser.ProcessTotals(this, script, parent);
		val.Add((CodeStatement)new CodeCommentStatement(GetCheckerInfoString(string.IsNullOrEmpty(parent) ? "Report" : parent, ev.ToString() + "Event")));
		val.Add((CodeExpression)new CodeSnippetExpression(script));
		GenEventMethod(eventName, ev, val);
	}

	private void GenGetValueEventMethod(string eventName, bool storeToPrinted, string script, StiEvent ev, CodeExpression expr = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_002e: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00cc: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0072: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		CodeStatementCollection val = new CodeStatementCollection();
		if (storeToPrinted)
		{
			val.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeArgumentReferenceExpression("e"), "StoreToPrinted"), (CodeExpression)new CodePrimitiveExpression((object)true)));
		}
		if (script.StartsWith("#%#", StringComparison.InvariantCulture))
		{
			if (expr != null)
			{
				val.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeArgumentReferenceExpression("e"), "Value"), (CodeExpression)new CodeBinaryOperatorExpression((CodeExpression)new CodePrimitiveExpression((object)"#%#"), (CodeBinaryOperatorType)0, expr)));
			}
			else
			{
				val.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeArgumentReferenceExpression("e"), "Value"), (CodeExpression)new CodePrimitiveExpression((object)script)));
			}
		}
		else
		{
			script = StiCodeDomTotalsFunctionsParser.ProcessTotals(this, script, null);
			val.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeArgumentReferenceExpression("e"), "Value"), (CodeExpression)new CodeSnippetExpression(script)));
		}
		GenEventMethod(eventName, ev, val);
	}

	internal void GenEventMethod(string eventName, StiEvent ev, CodeStatementCollection stats)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		CodeMemberMethod val = new CodeMemberMethod();
		((CodeTypeMember)val).Attributes = (MemberAttributes)24578;
		((CodeTypeMember)val).Name = eventName;
		CodeParameterDeclarationExpressionCollection val2 = new CodeParameterDeclarationExpressionCollection();
		StiParameterInfo[] parameters = ev.GetParameters();
		StiParameterInfo[] array = parameters;
		foreach (StiParameterInfo stiParameterInfo in array)
		{
			CodeParameterDeclarationExpression val3 = new CodeParameterDeclarationExpression(stiParameterInfo.Type, stiParameterInfo.Name);
			val2.Add(val3);
		}
		val.Parameters.AddRange(val2);
		val.Statements.AddRange(stats);
		Members.Add((CodeTypeMember)(object)val);
	}

	internal void AddEvent(string parent, StiPropertyInfo prop)
	{
		AddEvent(parent, prop.Value as StiEvent);
	}

	internal void AddEvent(string parent, StiEvent ev)
	{
		if (!string.IsNullOrWhiteSpace(ev.Script))
		{
			string script = ev.Script;
			script = StiCodeDomFunctions.ParseFunctions(this, script);
			string eventName = ((parent.Length > 0 && parent != null) ? $"{parent}_{ev}" : $"{report.GetReportName()}_{ev}");
			GenAddEvent(eventName, parent, ev);
			GenEventMethod(eventName, script, ev, parent);
		}
	}

	public static string GetCheckerInfoString(string componentName, string propertyName)
	{
		return string.Format("{0} {1} {2}", "CheckerInfo:", propertyName, componentName);
	}

	private void GenReturnMethod(string compName, string methodName, string expression, bool fullConvert, bool onlyText, StiFormatService format, StiComponent component, bool isTextExpression)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		CodeMemberMethod val = new CodeMemberMethod();
		((CodeTypeMember)val).Attributes = (MemberAttributes)24578;
		((CodeTypeMember)val).Name = methodName;
		val.ReturnType = new CodeTypeReference(typeof(string));
		val.Parameters.Add(new CodeParameterDeclarationExpression(typeof(StiComponent), "sender"));
		val.Statements.Add((CodeStatement)new CodeMethodReturnStatement(GetTextScriptExpression(format, expression, fullConvert, onlyText, compName, component, isTextExpression)));
		Members.Add((CodeTypeMember)(object)val);
	}

	internal void GenReturnMethodForExpresion(string methodName, string expression, Type returnType)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		CodeMemberMethod val = new CodeMemberMethod();
		((CodeTypeMember)val).Attributes = (MemberAttributes)24578;
		((CodeTypeMember)val).Name = methodName;
		val.ReturnType = new CodeTypeReference(returnType);
		val.Statements.Add((CodeStatement)new CodeMethodReturnStatement((CodeExpression)new CodeSnippetExpression(expression)));
		foreach (object item in (CollectionBase)(object)Members)
		{
			CodeMemberMethod val2 = (CodeMemberMethod)((item is CodeMemberMethod) ? item : null);
			if (val2 != null && ((CodeTypeMember)val2).Name == methodName && ((CodeTypeMember)val2).Attributes == ((CodeTypeMember)val).Attributes && val2.ReturnType.BaseType == returnType.ToString() && ((CollectionBase)(object)val2.Statements).Count == 2)
			{
				CodeStatement obj = val2.Statements[1];
				CodeMethodReturnStatement val3 = (CodeMethodReturnStatement)(object)((obj is CodeMethodReturnStatement) ? obj : null);
				if (val3 != null && val3.Expression is CodeSnippetExpression && ((CodeSnippetExpression)/*isinst with value type is only supported in some contexts*/).Value == expression)
				{
					return;
				}
			}
		}
		Members.Add((CodeTypeMember)(object)val);
	}

	public StiRemit AddToRemittedSE(string name, StiComponent comp, string funcName, string scriptCalcItem, string scriptGetValue, StiAggregateFunctionService function, bool usePrintEvent, bool useNewColumnEventEvent, bool isRunningTotal, StiExpression expression)
	{
		name = GetCompNameAggregateBeginEnd(name);
		int num = compsRemittedSE.IndexOf(name);
		if (num == -1)
		{
			compsRemittedSE.Add(name);
			compsRemittedSE.Add(new ArrayList());
			num = compsRemittedSE.Count - 2;
		}
		ArrayList arrayList = (ArrayList)compsRemittedSE[num + 1];
		StiRemit stiRemit = new StiRemit(comp, funcName, scriptCalcItem, scriptGetValue, string.Empty, function, usePrintEvent, useNewColumnEventEvent, expression, isRunningTotal);
		arrayList.Add(stiRemit);
		return stiRemit;
	}

	public void AddToRemittedBuild(string name, StiComponent comp, string funcName, string scriptCalcItem, string scriptGetValue, string scriptCondition, StiAggregateFunctionService function, StiExpression expression, bool isRunningTotal)
	{
		name = GetCompNameAggregateBuild(name);
		int num = compsRemittedBuild.IndexOf(name);
		if (num == -1)
		{
			compsRemittedBuild.Add(name);
			compsRemittedBuild.Add(new ArrayList());
			num = compsRemittedBuild.Count - 2;
		}
		ArrayList arrayList = (ArrayList)compsRemittedBuild[num + 1];
		arrayList.Add(new StiRemit(comp, funcName, scriptCalcItem, scriptGetValue, scriptCondition, function, usePrintEvent: false, useColumn: false, expression, isRunningTotal));
	}

	public bool ContainsRemittedComponentInLast(StiComponent comp)
	{
		foreach (StiRemit item in compsProcessAtEndRemitted)
		{
			if (item.Component == comp)
			{
				return true;
			}
		}
		foreach (StiRemit item2 in compsWordsToEndRemitted)
		{
			if (item2.Component == comp)
			{
				return true;
			}
		}
		return false;
	}

	private bool IsSimple(Type type)
	{
		if (type == typeof(decimal))
		{
			return true;
		}
		if (type.IsEnum || type.IsValueType)
		{
			return type.IsPrimitive;
		}
		return true;
	}

	private bool CanConvertTo(Type type)
	{
		return TypeDescriptor.GetConverter(type).CanConvertTo(typeof(InstanceDescriptor));
	}

	private CodeExpression GetValueExpression(object value)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		if (!IsSimple(value.GetType()))
		{
			return GetPropertyExpression(value);
		}
		return (CodeExpression)new CodePrimitiveExpression(value);
	}

	private Type GetTypeCollection(Type type)
	{
		PropertyInfo[] properties = type.GetProperties();
		PropertyInfo[] array = properties;
		foreach (PropertyInfo propertyInfo in array)
		{
			if (propertyInfo.Name == "Item")
			{
				return propertyInfo.PropertyType;
			}
		}
		return typeof(object);
	}

	public static string ConvertExpressionToString(StiCodeGenerator generator, CodeExpression expression)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		CodeGeneratorOptions val = new CodeGeneratorOptions();
		val.BlankLinesBetweenMembers = true;
		val.BracingStyle = "C";
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder);
		generator.GenerateCodeFromExpression(expression, stringWriter, val);
		stringWriter.Flush();
		stringWriter.Close();
		return stringBuilder.ToString();
	}

	internal bool TestProcessAtEnd(StiComponent comp, string text)
	{
		IStiText stiText = comp as IStiText;
		IStiProcessAt stiProcessAt = comp as IStiProcessAt;
		if (stiText != null && stiProcessAt != null)
		{
			if (stiProcessAt.ProcessAt == StiProcessAt.EndOfReport)
			{
				compsProcessAtEndRemitted.Add(new StiRemit(comp, string.Empty, "", text, "", null, usePrintEvent: false, useColumn: false, null, isRunningTotal: false));
				return true;
			}
			if (text.Contains("Totals.c"))
			{
				foreach (StiRemit item in compsWordsToEndRemitted)
				{
					if (item.Component == comp)
					{
						return false;
					}
				}
				StiRemit stiRemit = new StiRemit(comp, string.Empty, "", text, "", null, usePrintEvent: false, useColumn: false, null, isRunningTotal: false);
				stiRemit.isPageTotal = true;
				compsWordsToEndRemitted.Add(stiRemit);
				return true;
			}
			if (TestWordsToEnd(text))
			{
				compsWordsToEndRemitted.Add(new StiRemit(comp, string.Empty, "", text, "", null, usePrintEvent: false, useColumn: false, null, isRunningTotal: false));
				return true;
			}
		}
		return false;
	}

	private bool TestWordsToEnd(string text)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Invalid comparison between Unknown and I4
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Invalid comparison between Unknown and I4
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Invalid comparison between Unknown and I4
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Invalid comparison between Unknown and I4
		if (!string.IsNullOrEmpty(text))
		{
			List<string> lexem = StiCodeDomExpressionHelper.GetLexem(text);
			foreach (string item in lexem)
			{
				if (!item.StartsWith("{", StringComparison.InvariantCulture))
				{
					continue;
				}
				List<StiToken> list = null;
				string[] identsToEnd = IdentsToEnd;
				foreach (string text2 in identsToEnd)
				{
					if (item.IndexOf(text2, StringComparison.InvariantCulture) < 0)
					{
						continue;
					}
					if (list == null)
					{
						list = new List<StiToken>();
						StiLexer val = new StiLexer(text);
						StiToken token = val.GetToken();
						list.Add(token);
						while ((int)token.Type != 42)
						{
							if ((int)token.Type == 40 && token.Data.ToString() == text2)
							{
								return true;
							}
							token = val.GetToken();
							list.Add(token);
						}
						continue;
					}
					foreach (StiToken item2 in list)
					{
						if ((int)item2.Type == 42)
						{
							break;
						}
						if ((int)item2.Type == 40 && item2.Data.ToString() == text2)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	private void AddComment(string comment)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		Statements.Add((CodeStatement)new CodeCommentStatement(comment));
	}

	internal void AddDeclare(StiPropertyInfo prop)
	{
		AddDeclare(prop.Type, prop.Name);
	}

	internal void AddDeclare(Type type, string name)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		CodeMemberField val = new CodeMemberField(type, StiNameValidator.CorrectName(name, report));
		((CodeTypeMember)val).Attributes = (MemberAttributes)24576;
		Members.Add((CodeTypeMember)(object)val);
	}

	internal void AddCreate(string parent, StiPropertyInfo prop, bool isList)
	{
		AddCreate(-1, parent, prop.Value, prop.Name, prop.Type, isList);
	}

	internal void AddCreate(Type type, string name)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		Statements.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), name), (CodeExpression)new CodeObjectCreateExpression(type, (CodeExpression[])(object)new CodeExpression[0])));
	}

	internal void AddCreate(int index, string parent, object value, string name, Type type, bool isList)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		name = StiNameValidator.CorrectName(name, report);
		if (value != null && parent.Length == 0)
		{
			Statements.Add((CodeStatement)new CodeCommentStatement(string.Empty));
			Statements.Add((CodeStatement)new CodeCommentStatement(name));
			Statements.Add((CodeStatement)new CodeCommentStatement(string.Empty));
		}
		if (!isList)
		{
			CodeExpression val = (CodeExpression)((parent.Length <= 0) ? new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), name) : new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parent), name));
			CodeAssignStatement val2 = ((value == null) ? new CodeAssignStatement(val, (CodeExpression)new CodePrimitiveExpression((object)null)) : new CodeAssignStatement(val, GetObjectCreateExpression(type, value)));
			if (index != -1)
			{
				Statements.Insert(index, (CodeStatement)(object)val2);
			}
			else
			{
				Statements.Add((CodeStatement)(object)val2);
			}
		}
		else if (value != null)
		{
			CodeExpression val3 = (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parent), "Add", (CodeExpression[])(object)new CodeExpression[1] { GetObjectCreateExpression(type, value) });
			Statements.Add(val3);
		}
	}

	internal void AddInit(string parent, bool isReference, StiPropertyInfo prop)
	{
		AddInit(parent, isReference, prop, -1);
	}

	private CodeExpression GetVariableReference(string str)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		StiReportLanguageType scriptLanguage = report.ScriptLanguage;
		if (scriptLanguage != 0 && scriptLanguage != StiReportLanguageType.JS)
		{
			return (CodeExpression)new CodeSnippetExpression("me." + str);
		}
		return (CodeExpression)new CodeSnippetExpression("this." + str);
	}

	private void AddInit(string parent, bool isReference, StiPropertyInfo prop, int pos)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		CodeExpression val = (CodeExpression)((!isReference) ? GetValueExpression(prop.Value) : ((!((string)prop.Value != "this")) ? ((object)new CodeThisReferenceExpression()) : ((object)GetVariableReference(prop.Value.ToString()))));
		CodeExpression val2 = (CodeExpression)((parent.Length <= 0) ? new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), prop.Name) : new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parent), prop.Name));
		CodeAssignStatement val3 = new CodeAssignStatement(val2, val);
		if (pos != -1)
		{
			Statements.Insert(pos, (CodeStatement)(object)val3);
		}
		else
		{
			Statements.Add((CodeStatement)(object)val3);
		}
	}

	private void AddDataSource(string parent, StiPropertyInfo prop)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		CodeExpression val = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), ReplaceSymbols(((StiDataSource)prop.Value).Name, report));
		CodeExpression val2 = (CodeExpression)((parent.Length <= 0) ? new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), ReplaceSymbols(prop.Name, report)) : new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parent), ReplaceSymbols(prop.Name, report)));
		CodeAssignStatement val3 = new CodeAssignStatement(val2, val);
		Statements.Add((CodeStatement)(object)val3);
	}

	private void AddDataRelation(string parent, StiPropertyInfo prop)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		CodeExpression val = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), GetParentRelationName(report, ((StiDataRelation)prop.Value).Name));
		CodeExpression val2 = (CodeExpression)((parent.Length <= 0) ? new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), GetParentRelationName(report, prop.Name)) : new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parent), GetParentRelationName(report, prop.Name)));
		CodeAssignStatement val3 = new CodeAssignStatement(val2, val);
		Statements.Add((CodeStatement)(object)val3);
	}

	private void AddColorArray(string parent, StiPropertyInfo prop)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		Color[] array = prop.Value as Color[];
		CodeExpression val = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parent), prop.Name);
		CodeExpression[] array2 = (CodeExpression[])(object)new CodeExpression[array.Length];
		int num = 0;
		Color[] array3 = array;
		foreach (Color color in array3)
		{
			array2[num] = GetColorExpression(color);
			num++;
		}
		CodeExpression val2 = (CodeExpression)new CodeArrayCreateExpression(typeof(Color), array2);
		CodeAssignStatement val3 = new CodeAssignStatement(val, val2);
		Statements.Add((CodeStatement)(object)val3);
	}

	private void AddByteArray(string parent, StiPropertyInfo prop)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0068: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		byte[] inArray = prop.Value as byte[];
		CodeExpression val = (CodeExpression)(string.IsNullOrWhiteSpace(parent) ? new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), prop.Name) : new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parent), prop.Name));
		string text = Convert.ToBase64String(inArray);
		CodeMethodInvokeExpression val2 = new CodeMethodInvokeExpression((CodeExpression)new CodeTypeReferenceExpression("System.Convert"), "FromBase64String", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodePrimitiveExpression((object)text) });
		CodeAssignStatement val3 = new CodeAssignStatement(val, (CodeExpression)(object)val2);
		Statements.Add((CodeStatement)(object)val3);
	}

	private void AddImage(string parent, string name, StiPropertyInfo prop)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00fa: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		CodeFieldReferenceExpression val = (string.IsNullOrEmpty(parent) ? new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), name) : new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parent), name));
		if (prop.Value != null)
		{
			object value = prop.Value;
			Image val2 = (Image)((value is Image) ? value : null);
			if (prop.Name == "Image" && prop.Parent != null && prop.Parent.Value is IStiGlobalizedName stiGlobalizedName && !string.IsNullOrEmpty(stiGlobalizedName.GlobalizedName) && prop.Parent.Value is StiComponent { Report: not null } stiComponent && stiComponent.Report.GlobalizationManager != null)
			{
				object @object = stiComponent.Report.GlobalizationManager.GetObject(stiGlobalizedName.GlobalizedName);
				Image val3 = (Image)((@object is Image) ? @object : null);
				if (val3 != null)
				{
					val2 = val3;
				}
			}
			string text = StiImageConverter.ImageToString(val2, false);
			CodeMethodInvokeExpression val4 = new CodeMethodInvokeExpression((CodeExpression)new CodeTypeReferenceExpression("StiImageConverter"), "StringToImage", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodePrimitiveExpression((object)text) });
			Statements.Add((CodeStatement)new CodeAssignStatement((CodeExpression)(object)val, (CodeExpression)(object)val4));
		}
		else
		{
			Statements.Add((CodeStatement)new CodeAssignStatement((CodeExpression)(object)val, (CodeExpression)new CodePrimitiveExpression((object)null)));
		}
	}

	private void AddExpression(string parent, StiPropertyInfo prop)
	{
		StiExpression stiExpression = prop.Value as StiExpression;
		if (stiExpression == null)
		{
			return;
		}
		IStiGlobalizedName stiGlobalizedName = prop.Parent.Value as IStiGlobalizedName;
		bool flag = stiGlobalizedName != null && !string.IsNullOrWhiteSpace(stiGlobalizedName.GlobalizedName) && prop.Parent != null;
		if (flag && prop.Name == "Text" && prop.Parent.Value is StiComponent { Report: not null } stiComponent && stiComponent.Report.GlobalizationManager != null)
		{
			string @string = stiComponent.Report.GlobalizationManager.GetString(stiGlobalizedName.GlobalizedName);
			if (@string != null)
			{
				StiExpression stiExpression2 = stiExpression.Clone() as StiExpression;
				stiExpression2.Value = @string;
				stiExpression = stiExpression2;
			}
		}
		if (flag && !string.IsNullOrWhiteSpace(stiExpression.Value))
		{
			if (prop.Name == "Tag" && prop.Parent.Value is StiComponent { Report: not null } stiComponent2 && stiComponent2.Report.GlobalizationManager != null)
			{
				string string2 = stiComponent2.Report.GlobalizationManager.GetString(stiGlobalizedName.GlobalizedName + ".Tag");
				if (string2 != null)
				{
					StiExpression stiExpression3 = stiExpression.Clone() as StiExpression;
					stiExpression3.Value = string2;
					stiExpression = stiExpression3;
				}
			}
			if (prop.Name == "ToolTip" && prop.Parent.Value is StiComponent { Report: not null } stiComponent3 && stiComponent3.Report.GlobalizationManager != null)
			{
				string string3 = stiComponent3.Report.GlobalizationManager.GetString(stiGlobalizedName.GlobalizedName + ".ToolTip");
				if (string3 != null)
				{
					StiExpression stiExpression4 = stiExpression.Clone() as StiExpression;
					stiExpression4.Value = string3;
					stiExpression = stiExpression4;
				}
			}
			if (prop.Name == "Hyperlink" && prop.Parent.Value is StiComponent { Report: not null } stiComponent4 && stiComponent4.Report.GlobalizationManager != null)
			{
				string string4 = stiComponent4.Report.GlobalizationManager.GetString(stiGlobalizedName.GlobalizedName + ".Hyperlink");
				if (string4 != null)
				{
					StiExpression stiExpression5 = stiExpression.Clone() as StiExpression;
					stiExpression5.Value = string4;
					stiExpression = stiExpression5;
				}
			}
		}
		if (!string.IsNullOrWhiteSpace(stiExpression.Value))
		{
			StiEvent defaultEvent = stiExpression.GetDefaultEvent();
			string eventName = parent + "__" + defaultEvent.ToString() + string.Empty;
			GenGetValueEvent(defaultEvent, prop.Name, eventName, stiExpression, parent, prop.Parent.Value);
		}
	}

	private void AddHighlight(string parent, StiPropertyInfo prop)
	{
	}

	private void AddCollection(string parent, StiPropertyInfo prop, List<string> names)
	{
		AddCollection(parent, prop.Name, prop.Type, names);
	}

	private void AddStringArrayList(StiPropertyInfo prop, string name, string parent)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_00f5: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_00c4: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		ArrayList arrayList = prop.Value as ArrayList;
		CodeExpression val = (CodeExpression)((parent.Length <= 0) ? new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), name), "Clear", (CodeExpression[])(object)new CodeExpression[0]) : new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parent), name), "Clear", (CodeExpression[])(object)new CodeExpression[0]));
		CodeExpression[] array = (CodeExpression[])(object)new CodeExpression[arrayList.Count];
		for (int i = 0; i < arrayList.Count; i++)
		{
			array[i] = (CodeExpression)new CodePrimitiveExpression(arrayList[i]);
		}
		CodeExpression val2 = (CodeExpression)((parent.Length <= 0) ? new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), name), "AddRange", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeArrayCreateExpression(typeof(string), array) }) : new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parent), name), "AddRange", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeArrayCreateExpression(typeof(string), array) }));
		CollectionStatements.Add(val);
		CollectionStatements.Add(val2);
	}

	private void AddCollection(string parent, string name, Type type, List<string> names)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_00f5: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00c7: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		if (names != null && names.Count > 0)
		{
			CodeExpression[] array = (CodeExpression[])(object)new CodeExpression[names.Count];
			for (int i = 0; i < names.Count; i++)
			{
				array[i] = GetVariableReference(names[i]);
			}
			CodeExpression val = (CodeExpression)((parent.Length <= 0) ? new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), name), "Clear", (CodeExpression[])(object)new CodeExpression[0]) : new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parent), name), "Clear", (CodeExpression[])(object)new CodeExpression[0]));
			CodeExpression val2 = (CodeExpression)((parent.Length <= 0) ? new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), name), "AddRange", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeArrayCreateExpression(GetTypeCollection(type), array) }) : new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parent), name), "AddRange", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeArrayCreateExpression(GetTypeCollection(type), array) }));
			CollectionStatements.Add((CodeStatement)new CodeCommentStatement(string.Empty, false));
			if (parent.Length > 0)
			{
				CollectionStatements.Add((CodeStatement)new CodeCommentStatement($"Add to {parent}.{name}", false));
			}
			else
			{
				CollectionStatements.Add((CodeStatement)new CodeCommentStatement("Add to " + name, false));
			}
			CollectionStatements.Add((CodeStatement)new CodeCommentStatement(string.Empty, false));
			CollectionStatements.Add(val);
			CollectionStatements.Add(val2);
		}
	}

	private void AddArray(string parent, StiPropertyInfo prop)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		CodeExpression[] array = (CodeExpression[])(object)new CodeExpression[prop.Count];
		int num = 0;
		foreach (StiPropertyInfo item in (CollectionBase)(object)prop.Properties)
		{
			StiPropertyInfo val = item;
			array[num++] = (CodeExpression)new CodePrimitiveExpression(val.Value);
		}
		CodeArrayCreateExpression val2 = new CodeArrayCreateExpression(StiSerializing.GetTypeOfArrayElement(prop.Value), array);
		CodeExpression val3 = (CodeExpression)((parent.Length <= 0) ? new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), prop.Name) : new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parent), prop.Name));
		Statements.Add((CodeStatement)new CodeAssignStatement(val3, (CodeExpression)(object)val2));
	}

	private void AddArray(string componentName, string propertyName, Array array)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		CodeExpression[] array2 = (CodeExpression[])(object)new CodeExpression[array.GetLength(0)];
		for (int i = 0; i < array.GetLength(0); i++)
		{
			array2[i] = (CodeExpression)new CodePrimitiveExpression(array.GetValue(i));
		}
		CodeArrayCreateExpression val = new CodeArrayCreateExpression(StiSerializing.GetTypeOfArrayElement((object)array), array2);
		CodeExpression val2 = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), componentName), propertyName);
		Statements.Add((CodeStatement)new CodeAssignStatement(val2, (CodeExpression)(object)val));
	}

	private CodeExpression GetColorExpression(Color color)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00cf: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		if (color.IsSystemColor)
		{
			string text = new StiObjectStringConverter().ObjectToString((object)color);
			return (CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeTypeReferenceExpression(typeof(SystemColors)), text);
		}
		if (color.IsNamedColor)
		{
			string text = new StiObjectStringConverter().ObjectToString((object)color);
			return (CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeTypeReferenceExpression(typeof(Color)), text);
		}
		return (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeTypeReferenceExpression(typeof(Color)), "FromArgb", (CodeExpression[])(object)new CodeExpression[4]
		{
			(CodeExpression)new CodePrimitiveExpression((object)color.A),
			(CodeExpression)new CodePrimitiveExpression((object)color.R),
			(CodeExpression)new CodePrimitiveExpression((object)color.G),
			(CodeExpression)new CodePrimitiveExpression((object)color.B)
		});
	}

	internal CodeExpression GetPropertyExpression(object obj)
	{
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		if (obj is Color)
		{
			return GetColorExpression((Color)obj);
		}
		if (obj is Enum && !Enum.IsDefined(obj.GetType(), obj))
		{
			Type type = obj.GetType();
			List<int> list = new List<int>();
			Array values = Enum.GetValues(type);
			foreach (int item in values)
			{
				if (item == 0)
				{
					if ((int)obj == item)
					{
						list.Add(item);
					}
				}
				else if ((item & (int)obj) == item)
				{
					list.Add(item);
				}
			}
			if (list.Count >= 2)
			{
				CodeBinaryOperatorExpression val = new CodeBinaryOperatorExpression(GetPropertyExpression(Enum.ToObject(type, list[0])), (CodeBinaryOperatorType)9, GetPropertyExpression(Enum.ToObject(type, list[1])));
				for (int i = 2; i < list.Count; i++)
				{
					CodeBinaryOperatorExpression val2 = new CodeBinaryOperatorExpression((CodeExpression)(object)val, (CodeBinaryOperatorType)9, GetPropertyExpression(Enum.ToObject(type, list[i])));
					val = val2;
				}
				return (CodeExpression)(object)val;
			}
		}
		string text = new StiObjectStringConverter().ObjectToString(obj);
		return (CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeTypeReferenceExpression(obj.GetType()), text);
	}

	internal CodeExpression GetObjectCreateExpression(Type type, object value)
	{
		return GetObjectCreateExpressionWithParent(type, value, null);
	}

	internal CodeExpression GetObjectCreateExpressionWithParent(Type type, object value, object parentValue)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		return GetObjectCreateExpressionWithParent(type, new CodeTypeReference(GetTypeString(type)), value, parentValue);
	}

	private CodeExpression GetDashboardDrillDownParametersListExpression(object value)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		IEnumerable<IStiDashboardDrillDownParameter> enumerable = ListExt.ToList(value).Cast<IStiDashboardDrillDownParameter>();
		if (enumerable == null)
		{
			return null;
		}
		string parameterType = "Stimulsoft.Dashboard.Interactions.StiDashboardDrillDownParameter";
		CodeObjectCreateExpression[] array = enumerable.Select((Func<IStiDashboardDrillDownParameter, CodeObjectCreateExpression>)((IStiDashboardDrillDownParameter v) => new CodeObjectCreateExpression(parameterType, (CodeExpression[])(object)new CodeExpression[2]
		{
			(CodeExpression)new CodePrimitiveExpression((object)v.Name),
			(CodeExpression)new CodePrimitiveExpression((object)v.Expression)
		}))).ToArray();
		string text = parameterType;
		CodeExpression[] array2 = (CodeExpression[])(object)array;
		CodeArrayCreateExpression val = new CodeArrayCreateExpression(text, array2);
		return (CodeExpression)new CodeMethodInvokeExpression(new CodeMethodReferenceExpression((CodeExpression)new CodeTypeReferenceExpression(parameterType), "ToList"), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)val });
	}

	private bool IsDashboardDrillDownParameterList(Type type)
	{
		if (type == null || !type.IsGenericType)
		{
			return false;
		}
		type = type.GenericTypeArguments.FirstOrDefault();
		if (type == null)
		{
			return false;
		}
		return type.GetInterfaces().Contains(typeof(IStiDashboardDrillDownParameter));
	}

	private bool IsLocation(Type type)
	{
		if (type == typeof(Size))
		{
			return true;
		}
		if (type == typeof(SizeF))
		{
			return true;
		}
		if (type == typeof(SizeD))
		{
			return true;
		}
		if (type == typeof(StiMargins))
		{
			return true;
		}
		if (type == typeof(Point))
		{
			return true;
		}
		if (type == typeof(PointF))
		{
			return true;
		}
		if (type == typeof(PointD))
		{
			return true;
		}
		if (type == typeof(Rectangle))
		{
			return true;
		}
		if (type == typeof(RectangleF))
		{
			return true;
		}
		if (type == typeof(RectangleD))
		{
			return true;
		}
		return false;
	}

	internal CodeExpression[] GetArguments(Type type, object value)
	{
		return GetArgumentsWithParent(type, value, null);
	}

	internal CodeExpression[] GetArgumentsWithParent(Type type, object value, object parentValue)
	{
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Expected O, but got Unknown
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Expected O, but got Unknown
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01d3: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_0194: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Expected O, but got Unknown
		InstanceDescriptor instanceDescriptor = (InstanceDescriptor)TypeDescriptor.GetConverter(type).ConvertTo(value, typeof(InstanceDescriptor));
		CodeExpression[] array = (CodeExpression[])(object)new CodeExpression[instanceDescriptor.Arguments.Count];
		object[] array2 = new object[instanceDescriptor.Arguments.Count];
		instanceDescriptor.Arguments.CopyTo(array2, 0);
		for (int i = 0; i < array2.Length; i++)
		{
			object obj = array2[i];
			if (obj != null)
			{
				Type type2 = obj.GetType();
				if (type == typeof(StiDialogInfo) && obj is string[])
				{
					string[] array3 = obj as string[];
					CodeExpression[] array4 = (CodeExpression[])(object)new CodeExpression[array3.Length];
					int num = 0;
					string[] array5 = array3;
					foreach (string text in array5)
					{
						CodeExpression val = null;
						if (text != null && text.StartsWith("{") && text.EndsWith("}"))
						{
							string text2 = text.Substring(1, text.Length - 2);
							if (text2.Contains("<<|>>"))
							{
								StiVariable stiVariable = parentValue as StiVariable;
								CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
								try
								{
									Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
									string[] array6 = text2.Split(new string[1] { "<<|>>" }, StringSplitOptions.None);
									val = (CodeExpression)new CodeMethodInvokeExpression(new CodeMethodReferenceExpression((CodeExpression)new CodeTypeReferenceExpression(typeof(StiDialogInfo)), "Convert"), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeObjectCreateExpression(stiVariable.Type, (CodeExpression[])(object)new CodeExpression[2]
									{
										(CodeExpression)new CodeSnippetExpression(array6[0]),
										(CodeExpression)new CodeSnippetExpression(array6[1])
									}) });
								}
								finally
								{
									Thread.CurrentThread.CurrentCulture = currentCulture;
								}
							}
							else
							{
								val = (CodeExpression)new CodeMethodInvokeExpression(new CodeMethodReferenceExpression((CodeExpression)new CodeTypeReferenceExpression(typeof(StiDialogInfo)), "Convert"), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeSnippetExpression(text2) });
							}
						}
						else
						{
							val = (CodeExpression)new CodePrimitiveExpression((object)text);
						}
						array4[num] = val;
						num++;
					}
					array[i] = (CodeExpression)new CodeArrayCreateExpression(typeof(string[]), array4);
				}
				else if (type == typeof(StiDialogInfo) && obj is bool[])
				{
					bool[] source = obj as bool[];
					CodeExpression[] array7 = ((IEnumerable<bool>)source).Select((Func<bool, CodePrimitiveExpression>)((bool b) => new CodePrimitiveExpression((object)b))).Cast<CodeExpression>().ToArray();
					array[i] = (CodeExpression)new CodeArrayCreateExpression(typeof(bool[]), array7);
				}
				else if (obj is Type)
				{
					array[i] = (CodeExpression)new CodeTypeOfExpression(GetTypeString(obj as Type));
				}
				else if (obj is Image)
				{
					array[i] = GetImageExpression(obj);
				}
				else if (obj is byte[])
				{
					array[i] = GetByteArrayExpression(obj as byte[]);
				}
				else if (obj is Color[])
				{
					array[i] = GetColorArrayExpression(obj as Color[]);
				}
				else if (obj is string[])
				{
					array[i] = GetStringArrayExpression(obj as string[]);
				}
				else if (IsDashboardDrillDownParameterList(type2))
				{
					array[i] = GetDashboardDrillDownParametersListExpression(obj);
				}
				else if (IsLocation(type2))
				{
					array[i] = GetObjectCreateExpressionWithParent(type2, obj, value);
				}
				else if (!IsSimple(type2))
				{
					array[i] = GetPropertyExpression(obj);
				}
				else if (type2.IsClass)
				{
					array[i] = GetObjectCreateExpressionWithParent(type2, obj, value);
				}
				else
				{
					array[i] = (CodeExpression)new CodePrimitiveExpression(obj);
				}
			}
			else
			{
				array[i] = (CodeExpression)new CodePrimitiveExpression((object)null);
			}
		}
		return array;
	}

	private CodeExpression GetImageExpression(object imageObj)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_006a: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_003e: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		Image val = (Image)((imageObj is Image) ? imageObj : null);
		if (val is Metafile)
		{
			string text = StiMetafileConverter.MetafileToString((Metafile)(object)((val is Metafile) ? val : null));
			return (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeTypeReferenceExpression("StiMetafileConverter"), "StringToMetafile", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodePrimitiveExpression((object)text) });
		}
		string text2 = StiImageConverter.ImageToString(val, false);
		return (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeTypeReferenceExpression("StiImageConverter"), "StringToImage", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodePrimitiveExpression((object)text2) });
	}

	private CodeExpression GetByteArrayExpression(byte[] bytes)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002f: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		string text = StiPacker.PackToString(bytes);
		return (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeTypeReferenceExpression(typeof(StiPacker)), "UnpackFromString", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodePrimitiveExpression((object)text) });
	}

	private CodeExpression GetColorArrayExpression(Color[] colors)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		return (CodeExpression)new CodeArrayCreateExpression(new CodeTypeReference(typeof(Color)), colors.Select(GetColorExpression).ToArray());
	}

	private CodeExpression GetStringArrayExpression(string[] strs)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		CodeTypeReference val = new CodeTypeReference(typeof(string));
		CodeExpression[] array = (CodeExpression[])(object)((IEnumerable<string>)strs).Select((Func<string, CodePrimitiveExpression>)((string s) => new CodePrimitiveExpression((object)s))).ToArray();
		return (CodeExpression)new CodeArrayCreateExpression(val, array);
	}

	internal CodeObjectCreateExpression GetCodePrimitiveForPointF(CodeTypeReference createType, object value)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		CodeExpression[] array = (CodeExpression[])(object)new CodeExpression[2];
		PointF pointF = (PointF)value;
		array[0] = (CodeExpression)new CodePrimitiveExpression((object)pointF.X);
		array[1] = (CodeExpression)new CodePrimitiveExpression((object)pointF.Y);
		return new CodeObjectCreateExpression(createType, array);
	}

	internal CodeMethodInvokeExpression GetCodeExpressionForFont(CodeTypeReference createType, object value)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		Font val = (Font)((value is Font) ? value : null);
		CodeExpression[] array = (CodeExpression[])(object)new CodeExpression[3]
		{
			(CodeExpression)new CodePrimitiveExpression((object)val.Name),
			(CodeExpression)new CodePrimitiveExpression((object)val.Size),
			GetPropertyExpression(val.Style)
		};
		return new CodeMethodInvokeExpression((CodeExpression)new CodeTypeReferenceExpression("Stimulsoft.Base.StiFontCollection"), "CreateFont", array);
	}

	private CodeExpression GetObjectCreateExpressionWithParent(Type type, CodeTypeReference createType, object value, object parentValue)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		if (type == typeof(string))
		{
			return (CodeExpression)new CodePrimitiveExpression(value);
		}
		if (type == typeof(PointF))
		{
			return (CodeExpression)(object)GetCodePrimitiveForPointF(createType, value);
		}
		if (type == typeof(Font) && StiFontCollection.IsCustomFont(((Font)((value is Font) ? value : null)).Name))
		{
			return (CodeExpression)(object)GetCodeExpressionForFont(createType, value);
		}
		if (TypeDescriptor.GetConverter(type).CanConvertTo(typeof(InstanceDescriptor)))
		{
			CodeExpression[] argumentsWithParent = GetArgumentsWithParent(type, value, parentValue);
			return (CodeExpression)new CodeObjectCreateExpression(createType, argumentsWithParent);
		}
		return (CodeExpression)new CodeObjectCreateExpression(type, (CodeExpression[])(object)new CodeExpression[0]);
	}

	public static string ReplaceSymbols(string str)
	{
		return ReplaceSymbols(str, checkKeywords: true, null);
	}

	public static string ReplaceSymbols(string str, StiReport report)
	{
		return ReplaceSymbols(str, checkKeywords: true, report);
	}

	public static string ReplaceSymbols(string str, bool checkKeywords, StiReport report)
	{
		return StiNameValidator.CorrectName(str, checkKeywords, report);
	}

	public static string ConvertTypeToString(Type type, StiReportLanguageType language)
	{
		if (language == StiReportLanguageType.CSharp || language == StiReportLanguageType.JS)
		{
			if (type == typeof(int))
			{
				return "int";
			}
			if (type == typeof(uint))
			{
				return "uint";
			}
			if (type == typeof(long))
			{
				return "long";
			}
			if (type == typeof(ulong))
			{
				return "ulong";
			}
			if (type == typeof(string))
			{
				return "string";
			}
			if (type == typeof(bool))
			{
				return "bool";
			}
			if (type == typeof(object))
			{
				return "object";
			}
			if (type == typeof(void))
			{
				return "void";
			}
			if (type == typeof(byte))
			{
				return "byte";
			}
			if (type == typeof(sbyte))
			{
				return "sbyte";
			}
			if (type == typeof(short))
			{
				return "short";
			}
			if (type == typeof(ushort))
			{
				return "ushort";
			}
			if (type == typeof(char))
			{
				return "char";
			}
			if (type == typeof(double))
			{
				return "double";
			}
			if (type == typeof(float))
			{
				return "float";
			}
			if (type == typeof(decimal))
			{
				return "decimal";
			}
			if (type == typeof(DateTime))
			{
				return "DateTime";
			}
			if (type == typeof(TimeSpan))
			{
				return "TimeSpan";
			}
			if (type == typeof(byte?))
			{
				return "byte?";
			}
			if (type == typeof(sbyte?))
			{
				return "sbyte?";
			}
			if (type == typeof(bool?))
			{
				return "bool?";
			}
			if (type == typeof(byte?))
			{
				return "char?";
			}
			if (type == typeof(short?))
			{
				return "short?";
			}
			if (type == typeof(ushort?))
			{
				return "ushort?";
			}
			if (type == typeof(int?))
			{
				return "int?";
			}
			if (type == typeof(uint?))
			{
				return "uint?";
			}
			if (type == typeof(long?))
			{
				return "long?";
			}
			if (type == typeof(ulong?))
			{
				return "ulong?";
			}
			if (type == typeof(double?))
			{
				return "double?";
			}
			if (type == typeof(float?))
			{
				return "float?";
			}
			if (type == typeof(decimal?))
			{
				return "decimal?";
			}
			if (type == typeof(DateTime?))
			{
				return "DateTime?";
			}
			if (type == typeof(TimeSpan?))
			{
				return "TimeSpan?";
			}
			if (type == typeof(Guid?))
			{
				return "Guid?";
			}
			if (type == null)
			{
				return null;
			}
			if (Nullable.GetUnderlyingType(type) != null)
			{
				return $"{Nullable.GetUnderlyingType(type)}?";
			}
			if (type.IsGenericType && !StiOptions.Engine.AllowUseGenericTypesInCodeDom)
			{
				return "Object";
			}
		}
		else
		{
			if (type == typeof(int))
			{
				return "Integer";
			}
			if (type == typeof(long))
			{
				return "Long";
			}
			if (type == typeof(string))
			{
				return "String";
			}
			if (type == typeof(bool))
			{
				return "Boolean";
			}
			if (type == typeof(object))
			{
				return "Object";
			}
			if (type == typeof(void))
			{
				return "Void";
			}
			if (type == typeof(byte))
			{
				return "Byte";
			}
			if (type == typeof(short))
			{
				return "Short";
			}
			if (type == typeof(char))
			{
				return "Char";
			}
			if (type == typeof(double))
			{
				return "Double";
			}
			if (type == typeof(float))
			{
				return "Single";
			}
			if (type == typeof(decimal))
			{
				return "Decimal";
			}
			if (type == typeof(DateTime))
			{
				return "DateTime";
			}
			if (type == typeof(byte?))
			{
				return "Nullable(Of Byte)";
			}
			if (type == typeof(bool?))
			{
				return "Nullable(Of Boolean)";
			}
			if (type == typeof(byte?))
			{
				return "Nullable(Of Char)";
			}
			if (type == typeof(short?))
			{
				return "Nullable(Of Short)";
			}
			if (type == typeof(int?))
			{
				return "Nullable(Of Integer)";
			}
			if (type == typeof(long?))
			{
				return "Nullable(Of Long)";
			}
			if (type == typeof(double?))
			{
				return "Nullable(Of Double)";
			}
			if (type == typeof(float?))
			{
				return "Nullable(Of Single)";
			}
			if (type == typeof(decimal?))
			{
				return "Nullable(Of Decimal)";
			}
			if (type == typeof(DateTime?))
			{
				return "Nullable(Of DateTime)";
			}
			if (type == typeof(TimeSpan?))
			{
				return "Nullable(Of TimeSpan)";
			}
			if (type == typeof(Guid?))
			{
				return "Nullable(Of Guid)";
			}
			if (Nullable.GetUnderlyingType(type) != null)
			{
				return $"Nullable(Of {Nullable.GetUnderlyingType(type)})";
			}
			if (type.IsGenericType && !StiOptions.Engine.AllowUseGenericTypesInCodeDom)
			{
				return "Object";
			}
		}
		return null;
	}

	internal string GetTypeString(Type type)
	{
		if (type == null)
		{
			return "Object";
		}
		string text = ConvertTypeToString(type, report.ScriptLanguage);
		if (text == null)
		{
			text = type.ToString();
		}
		return text;
	}

	internal CodeMemberProperty GetColumnProperty(StiDataColumn column, bool buildRelations)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01c1: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0236: Expected O, but got Unknown
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_018c: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0115: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Expected O, but got Unknown
		CodeMemberProperty val = new CodeMemberProperty
		{
			Attributes = (MemberAttributes)24576,
			HasGet = true,
			Name = ReplaceSymbols(column.Name, checkKeywords: true, column.DataSource?.Dictionary?.Report),
			Type = new CodeTypeReference(GetTypeString(column.Type))
		};
		CodeExpression val3;
		if (column is StiCalcDataColumn)
		{
			string text = ((column.DataSource != null) ? ("Get" + ReplaceSymbols(column.DataSource.Name, report) + "_" + ReplaceSymbols(column.Name, report)) : ("Get" + ReplaceSymbols(column.BusinessObject.Name, report) + "_" + ReplaceSymbols(column.Name, report)));
			if (!buildRelations)
			{
				CodeMemberMethod val2 = new CodeMemberMethod
				{
					Attributes = (MemberAttributes)24578,
					Name = text,
					ReturnType = new CodeTypeReference(GetTypeString(column.Type))
				};
				string value = ((StiCalcDataColumn)column).Value;
				value = StiCodeDomFunctions.ParseFunctions(this, value);
				value = StiCodeDomTotalsFunctionsParser.ProcessTotals(this, value, column.Name);
				val2.Statements.Add((CodeStatement)new CodeMethodReturnStatement((CodeExpression)new CodeSnippetExpression(value)));
				Members.Add((CodeTypeMember)(object)val2);
			}
			val3 = (CodeExpression)new CodeMethodInvokeExpression(new CodeMethodReferenceExpression((CodeExpression)new CodeCastExpression(new CodeTypeReference(thisReportTypeName), (CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "Dictionary"), "Report")), text), (CodeExpression[])(object)new CodeExpression[0]);
		}
		else
		{
			CodeIndexerExpression val4 = new CodeIndexerExpression((CodeExpression)new CodeThisReferenceExpression(), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodePrimitiveExpression((object)column.NameInSource) });
			val3 = (CodeExpression)((!(column.Type == typeof(object))) ? ((object)new CodeCastExpression(GetTypeString(column.Type), (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeTypeReferenceExpression("StiReport"), "ChangeType", (CodeExpression[])(object)new CodeExpression[3]
			{
				(CodeExpression)val4,
				(CodeExpression)new CodeTypeOfExpression(GetTypeString(column.Type)),
				(CodeExpression)new CodePrimitiveExpression((object)report.ConvertNulls)
			}))) : ((object)val4));
		}
		CodeMethodReturnStatement val5 = new CodeMethodReturnStatement(val3);
		val.GetStatements.Add((CodeStatement)(object)val5);
		return val;
	}

	private StiDataBand GetDataBandOnWhichPlacedComponent(StiComponent comp)
	{
		StiComponent stiComponent = comp.Parent;
		if (stiComponent is StiChildBand)
		{
			StiBand master = (stiComponent as StiChildBand).GetMaster();
			if (master != null)
			{
				stiComponent = master;
			}
		}
		while (stiComponent != null && !(stiComponent is StiDataBand) && !(stiComponent is StiPage))
		{
			stiComponent = stiComponent.Parent;
			if (stiComponent is StiChildBand)
			{
				StiBand master2 = (stiComponent as StiChildBand).GetMaster();
				if (master2 != null)
				{
					stiComponent = master2;
				}
			}
		}
		return stiComponent as StiDataBand;
	}

	public void ConnectAggregatesBeginEnd()
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_012a: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_0207: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01ce: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Expected O, but got Unknown
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Expected O, but got Unknown
		//IL_0511: Expected O, but got Unknown
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0517: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Expected O, but got Unknown
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Expected O, but got Unknown
		//IL_04c3: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Expected O, but got Unknown
		//IL_04d7: Expected O, but got Unknown
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Expected O, but got Unknown
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Expected O, but got Unknown
		//IL_05a7: Expected O, but got Unknown
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Expected O, but got Unknown
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Expected O, but got Unknown
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Expected O, but got Unknown
		//IL_056c: Expected O, but got Unknown
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Expected O, but got Unknown
		if (compsRemittedSE.Count <= 0)
		{
			return;
		}
		List<StiEventItem> list = new List<StiEventItem>();
		for (int i = 0; i < compsRemittedSE.Count; i += 2)
		{
			string text = GetCompNameAggregateBeginEnd((string)compsRemittedSE[i]);
			string text2 = null;
			if (text.IndexOf(":", StringComparison.InvariantCulture) != -1)
			{
				string[] array = text.Split(new char[1] { ':' });
				text = array[0];
			}
			ArrayList arrayList = (ArrayList)compsRemittedSE[i + 1];
			bool flag = false;
			bool flag2 = false;
			CodeStatementCollection val = new CodeStatementCollection();
			CodeStatementCollection val2 = new CodeStatementCollection();
			foreach (StiRemit item in arrayList)
			{
				StiComponent component = item.Component;
				if (text2 == null)
				{
					text2 = item.RunningFooterBandName;
				}
				if (item.UsePrintEvent)
				{
					flag = true;
				}
				if (item.UseColumn)
				{
					flag2 = true;
					text = ReplaceSymbols(component.Page.Name, report);
				}
				string name = item.Name;
				if (item.Name.Contains("_SumRunning"))
				{
					CodeAssignStatement val3 = new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), name), "IsFirstInit"), (CodeExpression)new CodePrimitiveExpression((object)true));
					if (item.ConnectOnlyBegin)
					{
						val2.Add((CodeStatement)(object)val3);
					}
					else
					{
						val.Add((CodeStatement)(object)val3);
					}
				}
				CodeMethodInvokeExpression val4 = new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), name), "Init", (CodeExpression[])(object)new CodeExpression[0]);
				if (item.ConnectOnlyBegin)
				{
					val2.Add((CodeExpression)(object)val4);
				}
				else
				{
					val.Add((CodeExpression)(object)val4);
				}
				CodeAssignStatement val5 = null;
				val5 = ((!(item.Expression is StiExcelValueExpression)) ? new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), ReplaceSymbols(component.Name, report)), "TextValue"), (CodeExpression)new CodePrimitiveExpression((object)string.Empty)) : new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), ReplaceSymbols(component.Name, report)), "ExcelDataValue"), (CodeExpression)new CodePrimitiveExpression((object)"-")));
				if (item.ConnectOnlyBegin)
				{
					val2.Add((CodeStatement)(object)val5);
				}
				else
				{
					val.Add((CodeStatement)(object)val5);
				}
			}
			string text3;
			StiEvent ev;
			if (flag2)
			{
				text3 = text + "__ColumnBeginRender";
				ev = new StiColumnBeginRenderEvent();
			}
			else if (flag)
			{
				text3 = text + "__BeforePrint";
				ev = new StiBeforePrintEvent();
			}
			else
			{
				text3 = text + "__BeginRender";
				ev = new StiBeginRenderEvent();
			}
			if (((CollectionBase)(object)val).Count > 0)
			{
				GenAddEvent(text3, text, ev);
				GenEventMethod(text3, ev, val);
			}
			if (((CollectionBase)(object)val2).Count > 0)
			{
				string text4 = text + "__BeforePrint";
				if (text3 != text4)
				{
					StiEvent ev2 = new StiBeforePrintEvent();
					GenAddEvent(text4, text, ev2);
					GenEventMethod(text4, ev2, val2);
				}
			}
			Hashtable hashtable = new Hashtable();
			foreach (StiRemit item2 in arrayList)
			{
				if (item2.ConnectOnlyBegin)
				{
					continue;
				}
				flag = false;
				flag2 = false;
				StiDataBand stiDataBand = null;
				val = new CodeStatementCollection();
				if (ContainsRemittedComponentInLast(item2.Component))
				{
					continue;
				}
				string text5 = item2.Component.Name + "_GetValue_End";
				if (item2.Expression is StiExcelValueExpression)
				{
					text5 = item2.Component.Name + "_GetExcelValue_End";
				}
				if (item2.UsePrintEvent)
				{
					flag = true;
				}
				if (item2.UseColumn)
				{
					flag2 = true;
					text = item2.Component.Page.Name;
				}
				stiDataBand = GetDataBandOnWhichPlacedComponent(item2.Component);
				if (item2.isRunningTotal && stiDataBand != null && !flag2)
				{
					flag = true;
				}
				else
				{
					stiDataBand = null;
				}
				CodeMethodInvokeExpression val6 = null;
				string text6 = ((item2.Expression is StiExcelValueExpression) ? "SetExcelText" : "SetText");
				string text7 = item2.Component.Name + ":" + text6 + ":" + text5;
				if (hashtable[text7] == null)
				{
					hashtable[text7] = text7;
					if (report.ScriptLanguage != 0 && report.ScriptLanguage != StiReportLanguageType.JS)
					{
						val6 = ((!item2.isRunningTotal) ? new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), item2.Component.Name), text6, (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeSnippetExpression("AddressOf " + text5) }) : new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), item2.Component.Name), text6, (CodeExpression[])(object)new CodeExpression[2]
						{
							(CodeExpression)new CodeSnippetExpression("AddressOf " + text5),
							(CodeExpression)new CodePrimitiveExpression((object)true)
						}));
					}
					else
					{
						Type typeFromHandle = typeof(StiGetValue);
						if (item2.Expression is StiExcelValueExpression)
						{
							typeFromHandle = typeof(StiGetExcelValue);
						}
						val6 = ((!item2.isRunningTotal) ? new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), item2.Component.Name), text6, (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeDelegateCreateExpression(new CodeTypeReference(typeFromHandle), (CodeExpression)new CodeThisReferenceExpression(), text5) }) : new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), item2.Component.Name), text6, (CodeExpression[])(object)new CodeExpression[2]
						{
							(CodeExpression)new CodeDelegateCreateExpression(new CodeTypeReference(typeFromHandle), (CodeExpression)new CodeThisReferenceExpression(), text5),
							(CodeExpression)new CodePrimitiveExpression((object)true)
						}));
					}
					val.Add((CodeExpression)(object)val6);
				}
				string text8 = text;
				if (stiDataBand != null)
				{
					text8 = ReplaceSymbols(stiDataBand.Name, report);
				}
				if (flag2)
				{
					text3 = text8 + "__ColumnEndRender";
					ev = new StiColumnEndRenderEvent();
				}
				else if (flag)
				{
					text3 = text8 + "__AfterPrint";
					ev = new StiAfterPrintEvent();
				}
				else
				{
					text3 = text8 + "__EndRender";
					ev = new StiEndRenderEvent();
				}
				if (text2 != null)
				{
					text3 = text2 + "_MoveFooterToBottom";
					text8 = text2;
					ev = new StiMoveFooterToBottomEvent();
				}
				list.Add(new StiEventItem(text3, text8, ev, text7, val));
			}
		}
		if (list.Count <= 0)
		{
			return;
		}
		for (int num = list.Count - 1; num > 0; num--)
		{
			StiEventItem stiEventItem = list[num];
			for (int j = 0; j < num; j++)
			{
				StiEventItem stiEventItem2 = list[j];
				if (stiEventItem.EventName == stiEventItem2.EventName && stiEventItem.Name == stiEventItem2.Name && stiEventItem.CodeOfRemit == stiEventItem2.CodeOfRemit)
				{
					list.RemoveAt(num);
					break;
				}
			}
		}
		for (int num2 = list.Count - 1; num2 > 0; num2--)
		{
			StiEventItem stiEventItem3 = list[num2];
			for (int k = 0; k < num2; k++)
			{
				StiEventItem stiEventItem4 = list[k];
				if (stiEventItem3.EventName == stiEventItem4.EventName && stiEventItem3.Name == stiEventItem4.Name)
				{
					stiEventItem4.Stats.AddRange(stiEventItem3.Stats);
					list.RemoveAt(num2);
					break;
				}
			}
		}
		foreach (StiEventItem item3 in list)
		{
			GenAddEvent(item3.EventName, item3.Name, item3.Event);
			GenEventMethod(item3.EventName, item3.Event, item3.Stats);
		}
	}

	private string GetCompNameAggregateBuild(string compName)
	{
		if (compName.IndexOf(":", StringComparison.InvariantCulture) != -1)
		{
			string[] array = compName.Split(new char[1] { ':' });
			compName = array[1];
		}
		return ReplaceSymbols(compName, report);
	}

	private string GetCompNameAggregateBeginEnd(string compName)
	{
		if (compName.IndexOf(":", StringComparison.InvariantCulture) != -1)
		{
			string[] array = compName.Split(new char[1] { ':' });
			compName = array[0];
		}
		return ReplaceSymbols(compName, report);
	}

	public void ConnectAggreagatesBuild()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01bd: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Expected O, but got Unknown
		//IL_01f9: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_0194: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0165: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected O, but got Unknown
		//IL_0299: Expected O, but got Unknown
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Expected O, but got Unknown
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Expected O, but got Unknown
		//IL_02ef: Expected O, but got Unknown
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Expected O, but got Unknown
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Expected O, but got Unknown
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Expected O, but got Unknown
		//IL_032a: Expected O, but got Unknown
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		if (compsRemittedBuild.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < compsRemittedBuild.Count; i += 2)
		{
			string text = GetCompNameAggregateBuild((string)compsRemittedBuild[i]);
			ArrayList arrayList = (ArrayList)compsRemittedBuild[i + 1];
			CodeStatementCollection val = new CodeStatementCollection();
			foreach (StiRemit item in arrayList)
			{
				StiComponent component = item.Component;
				if (item.UseColumn)
				{
					text = ReplaceSymbols(component.GetDataBand().Name, report);
				}
				string name = item.Name;
				bool flag = item.ScriptCondition.Length > 0 && item.Name.StartsWith(item.Component.Name + "_SumDistinct");
				val.Add((CodeStatement)new CodeCommentStatement(GetCheckerInfoString(item.Component.Name, "Text")));
				CodeMethodInvokeExpression val2 = null;
				CodeStatement val3 = null;
				val2 = ((!item.Function.RecureParam || item.ScriptCalcItem == null || item.ScriptCalcItem.Trim().Length <= 0) ? new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), name), "CalcItem", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodePrimitiveExpression((object)null) }) : ((!flag) ? new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), name), "CalcItem", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeSnippetExpression(item.ScriptCalcItem) }) : new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), name), "CalcItem", (CodeExpression[])(object)new CodeExpression[2]
				{
					(CodeExpression)new CodeSnippetExpression(item.ScriptCalcItem),
					(CodeExpression)new CodeSnippetExpression(item.ScriptCondition)
				})));
				if (item.ScriptCondition.Length > 0 && !flag)
				{
					val3 = (CodeStatement)new CodeConditionStatement((CodeExpression)new CodeSnippetExpression(item.ScriptCondition), (CodeStatement[])(object)new CodeStatement[1] { (CodeStatement)new CodeExpressionStatement((CodeExpression)(object)val2) }, (CodeStatement[])(object)new CodeStatement[0]);
				}
				if (StiOptions.Engine.ForceAggregateFunctionsExceptionProcessing)
				{
					CodeTryCatchFinallyStatement val4 = new CodeTryCatchFinallyStatement();
					if (val3 != null)
					{
						val4.TryStatements.Add(val3);
					}
					else
					{
						val4.TryStatements.Add((CodeExpression)(object)val2);
					}
					CodeCatchClause val5 = new CodeCatchClause("ex");
					val5.Statements.Add((CodeExpression)new CodeMethodInvokeExpression(new CodeMethodReferenceExpression((CodeExpression)new CodeTypeReferenceExpression("StiLogService"), "Write"), (CodeExpression[])(object)new CodeExpression[2]
					{
						(CodeExpression)new CodeMethodInvokeExpression(new CodeMethodReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "GetType"), (CodeExpression[])(object)new CodeExpression[0]),
						(CodeExpression)new CodePrimitiveExpression((object)(text + " RenderingEvent " + name + " ...ERROR"))
					}));
					val5.Statements.Add((CodeExpression)new CodeMethodInvokeExpression(new CodeMethodReferenceExpression((CodeExpression)new CodeTypeReferenceExpression("StiLogService"), "Write"), (CodeExpression[])(object)new CodeExpression[2]
					{
						(CodeExpression)new CodeMethodInvokeExpression(new CodeMethodReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "GetType"), (CodeExpression[])(object)new CodeExpression[0]),
						(CodeExpression)new CodeArgumentReferenceExpression("ex")
					}));
					val5.Statements.Add((CodeExpression)new CodeMethodInvokeExpression(new CodeMethodReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "WriteToReportRenderingMessages"), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeSnippetExpression($"\"{name} \" + ex.Message") }));
					val4.CatchClauses.Add(val5);
					val3 = (CodeStatement)(object)val4;
				}
				if (val3 != null)
				{
					val.Add(val3);
				}
				else
				{
					val.Add((CodeExpression)(object)val2);
				}
			}
			string eventName = text + "__Rendering";
			StiRenderingEvent ev = new StiRenderingEvent();
			GenAddEvent(eventName, text, ev);
			GenEventMethod(eventName, ev, val);
		}
	}

	public void ConnectTextBoxReportEnd(List<StiRemit> comps)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_00d4: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00da: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_010b: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		if (comps.Count <= 0)
		{
			return;
		}
		CodeStatementCollection val = new CodeStatementCollection();
		foreach (StiRemit comp in comps)
		{
			string text = ReplaceSymbols(comp.Component.Name, report);
			string text2 = text + "_GetValue_End";
			string text3 = ((comp.Expression is StiExcelValueExpression) ? "SetExcelText" : "SetText");
			CodeMethodInvokeExpression val2;
			if (report.ScriptLanguage == StiReportLanguageType.CSharp || report.ScriptLanguage == StiReportLanguageType.JS)
			{
				Type typeFromHandle = typeof(StiGetValue);
				if (comp.Expression is StiExcelValueExpression)
				{
					typeFromHandle = typeof(StiGetExcelValue);
				}
				val2 = new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text), text3, (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeDelegateCreateExpression(new CodeTypeReference(typeFromHandle), (CodeExpression)new CodeThisReferenceExpression(), text2) });
			}
			else
			{
				val2 = new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text), text3, (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeSnippetExpression("AddressOf " + text2) });
			}
			val.Add((CodeExpression)(object)val2);
		}
		string eventName = ((comps != compsProcessAtEndRemitted) ? (report.GetReportName() + "WordsToEnd__EndRender") : "ProcessAtEnd__EndRender");
		StiEndRenderEvent ev = new StiEndRenderEvent();
		GenAddEvent(eventName, null, ev);
		GenEventMethod(eventName, ev, val);
	}

	public void ConnectChartProcessAtEnd()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		CodeStatementCollection val = new CodeStatementCollection();
		StiComponentsCollection stiComponentsCollection = components;
		foreach (StiComponent item in stiComponentsCollection)
		{
			IStiChartComponent stiChartComponent = item as IStiChartComponent;
			IStiProcessAtEnd stiProcessAtEnd = item as IStiProcessAtEnd;
			if (stiChartComponent != null && stiProcessAtEnd != null && stiProcessAtEnd.ProcessAtEnd)
			{
				CodeMethodInvokeExpression val2 = new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), item.Name), "RenderAtEnd", (CodeExpression[])(object)new CodeExpression[0]);
				val.Add((CodeExpression)(object)val2);
			}
		}
		if (((CollectionBase)(object)val).Count > 0)
		{
			string eventName = "ChartProcessAtEnd__EndRender";
			StiEndRenderEvent ev = new StiEndRenderEvent();
			GenAddEvent(eventName, null, ev);
			GenEventMethod(eventName, ev, val);
		}
	}

	public void BuildArrayAggregates()
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		if (compsRemittedBuild.Count <= 0)
		{
			return;
		}
		List<CodeExpression> list = new List<CodeExpression>();
		for (int i = 0; i < compsRemittedBuild.Count; i += 2)
		{
			string text = (string)compsRemittedBuild[i];
			ArrayList arrayList = (ArrayList)compsRemittedBuild[i + 1];
			foreach (StiRemit item in arrayList)
			{
				list.Add((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), item.Name));
			}
		}
		CodeArrayCreateExpression val = new CodeArrayCreateExpression(typeof(object), list.ToArray());
		CodeFieldReferenceExpression val2 = new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "AggregateFunctions");
		Statements.Add((CodeStatement)new CodeAssignStatement((CodeExpression)(object)val2, (CodeExpression)(object)val));
	}

	internal CodeExpression GetArrayCreateExpression(object obj)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		Array array = obj as Array;
		CodeExpression[] array2 = (CodeExpression[])(object)new CodeExpression[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = (CodeExpression)new CodePrimitiveExpression(array.GetValue(i));
		}
		return (CodeExpression)new CodeArrayCreateExpression(obj.GetType(), array2);
	}

	internal void Assign(CodeMemberMethod memberMethod, string fieldName1, string fieldName2, object obj)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		memberMethod.Statements.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), fieldName1), fieldName2), GetArrayCreateExpression(obj)));
	}

	private List<string> SerializeProps(StiPropertyInfoCollection props, bool isList, string parentName, string referenceName)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Expected O, but got Unknown
		//IL_10f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fd: Expected O, but got Unknown
		//IL_1122: Unknown result type (might be due to invalid IL or missing references)
		//IL_1129: Expected O, but got Unknown
		//IL_114a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1159: Expected O, but got Unknown
		//IL_1154: Unknown result type (might be due to invalid IL or missing references)
		//IL_115b: Expected O, but got Unknown
		//IL_116a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1174: Expected O, but got Unknown
		//IL_116f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1179: Expected O, but got Unknown
		//IL_113a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1141: Expected O, but got Unknown
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Expected O, but got Unknown
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Expected O, but got Unknown
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Expected O, but got Unknown
		//IL_06f9: Expected O, but got Unknown
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Expected O, but got Unknown
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_0d27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3d: Expected O, but got Unknown
		//IL_0d38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3f: Expected O, but got Unknown
		//IL_0d56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d67: Expected O, but got Unknown
		//IL_0d67: Expected O, but got Unknown
		//IL_0d62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6c: Expected O, but got Unknown
		//IL_0d67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d71: Expected O, but got Unknown
		//IL_0d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8c: Expected O, but got Unknown
		//IL_0da2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dac: Expected O, but got Unknown
		//IL_0dc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcd: Expected O, but got Unknown
		//IL_0f66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6d: Expected O, but got Unknown
		//IL_0e15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1f: Expected O, but got Unknown
		//IL_0e79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e83: Expected O, but got Unknown
		//IL_0ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed3: Expected O, but got Unknown
		//IL_08da: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f0: Expected O, but got Unknown
		//IL_08eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f2: Expected O, but got Unknown
		//IL_0909: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Unknown result type (might be due to invalid IL or missing references)
		//IL_091a: Expected O, but got Unknown
		//IL_091a: Expected O, but got Unknown
		//IL_0915: Unknown result type (might be due to invalid IL or missing references)
		//IL_091f: Expected O, but got Unknown
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Expected O, but got Unknown
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092c: Expected O, but got Unknown
		//IL_0957: Unknown result type (might be due to invalid IL or missing references)
		//IL_0961: Expected O, but got Unknown
		//IL_0978: Unknown result type (might be due to invalid IL or missing references)
		//IL_0982: Expected O, but got Unknown
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b4: Expected O, but got Unknown
		//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cb: Expected O, but got Unknown
		//IL_09da: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e4: Expected O, but got Unknown
		//IL_09f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Expected O, but got Unknown
		//IL_0c5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c66: Expected O, but got Unknown
		List<string> list = new List<string>();
		foreach (StiPropertyInfo item in (CollectionBase)(object)props)
		{
			StiPropertyInfo val = item;
			string text = ReplaceSymbols(val.Name, report);
			if (val.Value is StiUserViewState)
			{
				object obj;
				if (val == null)
				{
					obj = null;
				}
				else
				{
					StiPropertyInfo parent = val.Parent;
					obj = ((parent != null) ? parent.Parent : null);
				}
				if (obj != null)
				{
					val.Name = val.Parent.Parent.Name + "_" + text;
				}
			}
			if (val.Value is StiBaseStyle && (val.Parent == null || !(val.Parent.Value is StiChart)) && (val.Parent == null || !(val.Parent.Value is StiGauge)) && (val.Parent == null || !(val.Parent.Value is StiTable)))
			{
				val.Name = "Style" + text;
			}
			if (!val.IsReference && val.ReferenceCode != -1)
			{
				if (Graphs[(object)text] == -1)
				{
					Graphs.Add((object)text, val.ReferenceCode);
				}
				else if (val.Name != "TextFormat")
				{
					string text2 = $"{text}_copy_{val.ReferenceCode}";
					while (Graphs[(object)text2] != -1)
					{
						text2 += "_";
					}
					Graphs.Add((object)text2, val.ReferenceCode);
				}
			}
			if (val.IsDefaultValueSpecified && object.Equals(val.DefaultValue, val.Value))
			{
				continue;
			}
			if (val.Type.IsValueType || val.Type == typeof(string))
			{
				if (val.Value is DateTime || val.Value is Rectangle || val.Value is Size || val.Value is Point || val.Value is RectangleF || val.Value is SizeF || val.Value is PointF || val.Value is RectangleD || val.Value is SizeD || val.Value is StiMargins || val.Value is PointD || val.Value is Guid)
				{
					AddCreate(parentName, val, isList: false);
				}
				else
				{
					AddInit(parentName, isReference: false, val);
				}
			}
			else if (val.Value is StiExpression)
			{
				AddExpression(parentName, val);
			}
			else if (val.Value is StiFiltersCollection)
			{
				StiCodeDomFilters.AddFilters(this, parentName, val.Parent.Value as IStiFilter);
			}
			else if (val.Value is StiConditionsCollection)
			{
				StiCodeDomConditions.AddConditions(this, parentName, val);
			}
			else if (val.Value is StiHighlightCondition)
			{
				AddHighlight(parentName, val);
			}
			else if (val.IsList && val.Value is Array)
			{
				if (val.Name == "Sort")
				{
					string[] array = ((string[])val.Value).Clone() as string[];
					for (int i = 0; i < array.Length; i++)
					{
						string text3 = array[i];
						if (text3.StartsWith("{", StringComparison.InvariantCulture) && text3.EndsWith("}", StringComparison.InvariantCulture))
						{
							string text4 = $"Get{StiNameValidator.CorrectName(val.Parent.Name, report)}_Sort{i}";
							text3 = text3.Substring(1, text3.Length - 2).Trim();
							if (val.Parent.Value is StiDataBand { DataSource: not null } stiDataBand && stiDataBand.DataSource.Columns.Contains(text3))
							{
								text3 = StiNameValidator.CorrectName(stiDataBand.DataSource.Name, report) + "." + text3;
							}
							GenReturnMethodForExpresion(text4, StiCodeDomFunctions.ParseFunctions(this, text3), typeof(object));
							array[i] = "{" + text4 + "}";
							CodeTypeMember obj2 = Members[((CollectionBase)(object)Members).Count - 1];
							CodeMemberMethod val2 = (CodeMemberMethod)(object)((obj2 is CodeMemberMethod) ? obj2 : null);
							if (val2 != null)
							{
								val2.Statements.Insert(0, (CodeStatement)new CodeCommentStatement(GetCheckerInfoString(parentName, "Sort")));
							}
						}
					}
					AddArray(StiNameValidator.CorrectName(val.Parent.Name, report), "Sort", array);
				}
				else
				{
					AddArray(parentName, val);
				}
			}
			else if (val.Value is StiDataSource)
			{
				AddDataSource(parentName, val);
			}
			else if (val.Value is Image)
			{
				AddImage(parentName, text, val);
			}
			else if (val.Value is StiDataRelation)
			{
				AddDataRelation(parentName, val);
			}
			else if (val.Value is Color[])
			{
				AddColorArray(parentName, val);
			}
			else if (val.Value is byte[])
			{
				AddByteArray(parentName, val);
			}
			else if ((val.IsReference || val.Value is IStiSerializeToCodeAsClass) && (!(val.Value is IStiInteractionClass) || !((IStiDefault)val.Value).IsDefault))
			{
				reference.Add(val);
				reference.Add(parentName);
			}
		}
		foreach (StiPropertyInfo item2 in (CollectionBase)(object)props)
		{
			StiPropertyInfo val3 = item2;
			if ((val3.IsDefaultValueSpecified && object.Equals(val3.DefaultValue, val3.Value)) || val3.Type.IsValueType || val3.Type == typeof(string) || val3.IsReference || val3.Value is StiExpression || val3.Value is StiFiltersCollection || val3.Value is StiConditionsCollection || val3.Value is StiEvent || val3.Value is Image || val3.Value is StiHighlightCondition)
			{
				continue;
			}
			if (val3.Value is IStiSerializeToCodeAsClass)
			{
				string text5 = null;
				if (val3.Value is IStiInteractionClass && ((IStiDefault)val3.Value).IsDefault)
				{
					Statements.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), StiNameValidator.CorrectName(val3.Parent.Name, report)), val3.Name), (CodeExpression)new CodePrimitiveExpression((object)null)));
					continue;
				}
				text5 = ((val3.Parent != null) ? (StiNameValidator.CorrectName(val3.Parent.Name, report) + "_" + val3.Name) : (report.GetReportName() + "_" + val3.Name));
				AddDeclare(val3.Type, text5);
				AddCreate(val3.Type, text5);
				SerializeProps(val3.Properties, isList: false, text5, text5);
				if (!(val3.Value is StiInteraction stiInteraction))
				{
					continue;
				}
				bool flag = stiInteraction.DrillDownParameter1 == null || stiInteraction.DrillDownParameter1.IsDefault;
				bool flag2 = stiInteraction.DrillDownParameter2 == null || stiInteraction.DrillDownParameter2.IsDefault;
				bool flag3 = stiInteraction.DrillDownParameter3 == null || stiInteraction.DrillDownParameter3.IsDefault;
				bool flag4 = stiInteraction.DrillDownParameter4 == null || stiInteraction.DrillDownParameter4.IsDefault;
				bool flag5 = stiInteraction.DrillDownParameter5 == null || stiInteraction.DrillDownParameter5.IsDefault;
				bool flag6 = stiInteraction.DrillDownParameter6 == null || stiInteraction.DrillDownParameter6.IsDefault;
				bool flag7 = stiInteraction.DrillDownParameter7 == null || stiInteraction.DrillDownParameter7.IsDefault;
				bool flag8 = stiInteraction.DrillDownParameter8 == null || stiInteraction.DrillDownParameter8.IsDefault;
				bool flag9 = stiInteraction.DrillDownParameter9 == null || stiInteraction.DrillDownParameter9.IsDefault;
				bool flag10 = stiInteraction.DrillDownParameter10 == null || stiInteraction.DrillDownParameter10.IsDefault;
				if (!(flag && flag2 && flag3 && flag4 && flag5 && flag6 && flag7 && flag8 && flag9 && flag10))
				{
					string text6 = val3.Parent.Name + "_FillDrillDownParameters";
					CodeExpression val4 = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), val3.Parent.Name);
					Statements.Add((CodeStatement)new CodeAttachEventStatement(val4, "BeforePrint", (CodeExpression)new CodeDelegateCreateExpression(new CodeTypeReference(typeof(EventHandler)), (CodeExpression)new CodeThisReferenceExpression(), text6)));
					CodeMemberMethod val5 = new CodeMemberMethod();
					((CodeTypeMember)val5).Attributes = (MemberAttributes)24578;
					((CodeTypeMember)val5).Name = text6;
					val5.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "sender"));
					val5.Parameters.Add(new CodeParameterDeclarationExpression(typeof(EventArgs), "e"));
					if (report.ScriptLanguage == StiReportLanguageType.CSharp || report.ScriptLanguage == StiReportLanguageType.JS)
					{
						val5.Statements.Add((CodeStatement)new CodeSnippetStatement("StiComponent comp = sender as StiComponent;"));
						val5.Statements.Add((CodeStatement)new CodeSnippetStatement("comp.DrillDownParameters = new System.Collections.Generic.Dictionary<string, object>();"));
					}
					else
					{
						val5.Statements.Add((CodeStatement)new CodeSnippetStatement("Dim comp As StiComponent = TryCast(sender,StiComponent)"));
						val5.Statements.Add((CodeStatement)new CodeSnippetStatement("comp.DrillDownParameters = New System.Collections.Generic.Dictionary(Of String, Object)"));
					}
					if (!flag)
					{
						GenerateDrillDownParameter(parentName, 1, stiInteraction.DrillDownParameter1, val5);
					}
					if (!flag2)
					{
						GenerateDrillDownParameter(parentName, 2, stiInteraction.DrillDownParameter2, val5);
					}
					if (!flag3)
					{
						GenerateDrillDownParameter(parentName, 3, stiInteraction.DrillDownParameter3, val5);
					}
					if (!flag4)
					{
						GenerateDrillDownParameter(parentName, 4, stiInteraction.DrillDownParameter4, val5);
					}
					if (!flag5)
					{
						GenerateDrillDownParameter(parentName, 5, stiInteraction.DrillDownParameter5, val5);
					}
					if (!flag6)
					{
						GenerateDrillDownParameter(parentName, 6, stiInteraction.DrillDownParameter6, val5);
					}
					if (!flag7)
					{
						GenerateDrillDownParameter(parentName, 7, stiInteraction.DrillDownParameter7, val5);
					}
					if (!flag8)
					{
						GenerateDrillDownParameter(parentName, 8, stiInteraction.DrillDownParameter8, val5);
					}
					if (!flag9)
					{
						GenerateDrillDownParameter(parentName, 9, stiInteraction.DrillDownParameter9, val5);
					}
					if (!flag10)
					{
						GenerateDrillDownParameter(parentName, 10, stiInteraction.DrillDownParameter10, val5);
					}
					Members.Add((CodeTypeMember)(object)val5);
				}
				if (!StiOptions.Engine.Interaction.ForceSortingWithFullTypeConversion || string.IsNullOrWhiteSpace(stiInteraction.SortingColumn))
				{
					continue;
				}
				int num = stiInteraction.SortingColumn.IndexOf(".");
				if (num == -1)
				{
					continue;
				}
				string componentName = stiInteraction.SortingColumn.Substring(0, num);
				StiDataSource stiDataSource = ((report.GetComponentByName(componentName) is StiDataBand stiDataBand2) ? stiDataBand2.DataSource : null);
				if (stiDataSource == null)
				{
					continue;
				}
				string methodName = "Get" + StiNameValidator.CorrectName(stiInteraction.SortingColumn, report) + "_Sort";
				string[] array2 = stiInteraction.SortingColumn.Substring(num + 1).Split(new char[1] { '.' });
				if (array2.Length > 1)
				{
					for (int j = 0; j < array2.Length - 1; j++)
					{
						StiDataRelation stiDataRelation = report.Dictionary.Relations[array2[j]];
						if (stiDataRelation != null)
						{
							array2[j] = stiDataRelation.Name;
						}
					}
				}
				string text7 = StiNameValidator.CorrectName(stiDataSource.Name, report) + "." + string.Join(".", array2);
				GenReturnMethodForExpresion(methodName, StiCodeDomFunctions.ParseFunctions(this, text7), typeof(object));
				CodeTypeMember obj3 = Members[((CollectionBase)(object)Members).Count - 1];
				CodeMemberMethod val6 = (CodeMemberMethod)(object)((obj3 is CodeMemberMethod) ? obj3 : null);
				if (val6 != null)
				{
					val6.Statements.Insert(0, (CodeStatement)new CodeCommentStatement(GetCheckerInfoString(parentName, "Sort")));
				}
			}
			else if (val3.IsList && !(val3.Value is Array))
			{
				if (val3.Value is ArrayList && ((ArrayList)val3.Value).Count > 0 && ((ArrayList)val3.Value)[0] is string)
				{
					AddStringArrayList(val3, val3.Name, val3.Parent.Name);
					continue;
				}
				if (val3.Value is StiParametersCollection)
				{
					StiParametersCollection stiParametersCollection = val3.Value as StiParametersCollection;
					if (stiParametersCollection.Count <= 0)
					{
						continue;
					}
					string text8 = val3.Parent.Name + "_FillParametersItems";
					CodeExpression val7 = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), val3.Parent.Name);
					Statements.Add((CodeStatement)new CodeAttachEventStatement(val7, "FillParameters", (CodeExpression)new CodeDelegateCreateExpression(new CodeTypeReference(typeof(StiFillParametersEventHandler)), (CodeExpression)new CodeThisReferenceExpression(), text8)));
					CodeMemberMethod val8 = new CodeMemberMethod
					{
						Attributes = (MemberAttributes)24578,
						Name = text8
					};
					val8.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "sender"));
					val8.Parameters.Add(new CodeParameterDeclarationExpression(typeof(StiFillParametersEventArgs), "e"));
					foreach (StiParameter item3 in stiParametersCollection)
					{
						if (!item3.IsDefault)
						{
							val8.Statements.Add((CodeStatement)new CodeCommentStatement(GetCheckerInfoString(val3.Parent.Name, item3.Name)));
							StiReportLanguageType scriptLanguage = report.ScriptLanguage;
							if (scriptLanguage == StiReportLanguageType.CSharp || scriptLanguage == StiReportLanguageType.JS)
							{
								val8.Statements.Add((CodeStatement)new CodeSnippetStatement("e.Value.Add(\"" + item3.Name + "\", " + item3.Expression.Value + ");"));
							}
							else
							{
								val8.Statements.Add((CodeStatement)new CodeSnippetStatement("e.Value.Add(\"" + item3.Name + "\", " + item3.Expression.Value + ")"));
							}
						}
					}
					Members.Add((CodeTypeMember)(object)val8);
					continue;
				}
				List<string> list2 = SerializeProps(val3.Properties, isList: true, string.Empty, val3.Name);
				if (val3.Parent != null && val3.Parent.Type == typeof(StiForm))
				{
					foreach (StiPropertyInfo item4 in (CollectionBase)(object)val3.Properties)
					{
						StiPropertyInfo val9 = item4;
						if (val9.Type.IsSubclassOf(typeof(Control)))
						{
							list2.Remove(val9.Name);
						}
					}
				}
				AddCollection(parentName, val3, list2);
			}
			else if (val3.IsKey && isList)
			{
				string text9 = ReplaceSymbols(val3.Name, report);
				AddDeclare(val3);
				AddCreate(parentName, val3, isList: false);
				list.Add(text9);
				SerializeProps(val3.Properties, isList: false, text9, text9);
			}
			else
			{
				if (!val3.IsKey && !CanConvertTo(val3.Type))
				{
					continue;
				}
				if (val3.Name == "value" && parentName.Length == 0)
				{
					if (val3.Parent.Parent == null)
					{
						AddCreate(val3.Parent.Name, val3, isList: true);
					}
					else
					{
						AddCreate(val3.Parent.Parent.Name + "." + val3.Parent.Name, val3, isList: true);
					}
				}
				else if (val3.Name != "Dictionary")
				{
					AddCreate(parentName, val3, isList: false);
				}
			}
		}
		foreach (StiPropertyInfo item5 in (CollectionBase)(object)props)
		{
			StiPropertyInfo val10 = item5;
			if (!(val10.Value is StiEvent stiEvent))
			{
				continue;
			}
			if (stiEvent.Script.StartsWith("<xml xmlns=\"https://developers.google.com/blockly/xml\">"))
			{
				CodeExpression val11 = (CodeExpression)new CodeThisReferenceExpression();
				if (referenceName != "this")
				{
					val11 = (CodeExpression)new CodePropertyReferenceExpression(val11, parentName);
				}
				CodePropertyReferenceExpression val12 = new CodePropertyReferenceExpression((CodeExpression)new CodePropertyReferenceExpression(val11, stiEvent.PropertyName), "Script");
				Statements.Add((CodeStatement)new CodeAssignStatement((CodeExpression)(object)val12, (CodeExpression)new CodePrimitiveExpression((object)stiEvent.Script)));
			}
			else
			{
				AddEvent(parentName, val10);
			}
		}
		return list;
	}

	private void GenerateDrillDownParameter(string parentName, int parameterIndex, StiDrillDownParameter parameter, CodeMemberMethod method)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		string text = parameter.Expression.Value;
		if (string.IsNullOrWhiteSpace(text))
		{
			text = "\"\"";
		}
		method.Statements.Add((CodeStatement)new CodeCommentStatement(GetCheckerInfoString(parentName, $"DrillDownParameter{parameterIndex}")));
		string text2 = "comp.DrillDownParameters.Add(\"" + parameter.Name + "\", " + text + ")";
		if (report.ScriptLanguage == StiReportLanguageType.CSharp || report.ScriptLanguage == StiReportLanguageType.JS)
		{
			text2 += ";";
		}
		method.Statements.Add((CodeStatement)new CodeSnippetStatement(text2));
	}

	internal string GetReportTypeName(string name)
	{
		string script = report.Script;
		int num = 0;
		if (script == null)
		{
			return name;
		}
		num = ((report.ScriptLanguage != 0 && report.ScriptLanguage != StiReportLanguageType.JS) ? script.IndexOf("Class", StringComparison.InvariantCulture) : script.IndexOf("class", StringComparison.InvariantCulture));
		for (num += 5; num < script.Length && !char.IsLetterOrDigit(script[num]); num++)
		{
		}
		StringBuilder stringBuilder = new StringBuilder(20);
		for (; num < script.Length && (char.IsLetterOrDigit(script[num]) || script[num] == '_'); num++)
		{
			stringBuilder.Append(script[num]);
		}
		return stringBuilder.ToString();
	}

	public string Serialize(StiReport report, string name, StiLanguage language)
	{
		return Serialize(report, name, language, saveForInheritedReports: false);
	}

	public string Serialize(StiReport report, string name, StiLanguage language, object standaloneReportType)
	{
		return Serialize(report, name, language, serializeData: true, saveForInheritedReports: false, standaloneReportType);
	}

	public string Serialize(StiReport report, string name, StiLanguage language, bool saveForInheritedReports)
	{
		return Serialize(report, name, language, serializeData: true, saveForInheritedReports);
	}

	public string Serialize(StiReport report, string name, StiLanguage language, bool serializeData, bool saveForInheritedReports)
	{
		return Serialize(report, name, language, serializeData, saveForInheritedReports, null);
	}

	public string Serialize(StiReport report, string name, StiLanguage language, bool serializeData, bool saveForInheritedReports, object standaloneReportType)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Expected O, but got Unknown
		//IL_03cc: Expected O, but got Unknown
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Expected O, but got Unknown
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Expected O, but got Unknown
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Expected O, but got Unknown
		//IL_053f: Expected O, but got Unknown
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Expected O, but got Unknown
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Expected O, but got Unknown
		//IL_0593: Expected O, but got Unknown
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Expected O, but got Unknown
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Expected O, but got Unknown
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Expected O, but got Unknown
		//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Expected O, but got Unknown
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Expected O, but got Unknown
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Expected O, but got Unknown
		//IL_071f: Expected O, but got Unknown
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Expected O, but got Unknown
		//IL_074e: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Expected O, but got Unknown
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_076d: Expected O, but got Unknown
		//IL_076d: Expected O, but got Unknown
		//IL_0768: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Expected O, but got Unknown
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b1: Expected O, but got Unknown
		//IL_0871: Unknown result type (might be due to invalid IL or missing references)
		//IL_0878: Expected O, but got Unknown
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_0969: Expected O, but got Unknown
		//IL_0964: Unknown result type (might be due to invalid IL or missing references)
		//IL_096e: Expected O, but got Unknown
		if (report.IsPageDesigner)
		{
			return "";
		}
		this.report = report;
		components = report.GetComponents();
		provider = language.GetProvider();
		thisReportTypeName = GetReportTypeName(name);
		memberMethod = new CodeMemberMethod();
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder);
		generator = provider.CreateGenerator() as StiCodeGenerator;
		if (generator == null)
		{
			throw new Exception("Provider must be inherited from StiCodeGenerator");
		}
		CodeGeneratorOptions o = new CodeGeneratorOptions
		{
			BlankLinesBetweenMembers = true,
			BracingStyle = "C"
		};
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		Graphs = new StiGraphs();
		Graphs.Add((object)"this", 0);
		((CollectionBase)(object)Members).Clear();
		if (!saveForInheritedReports)
		{
			StiCodeDomVariables.Serialize(this, report);
			StiCodeDomResources.Serialize(this, report);
			StiPropertyInfoCollection props = val.Serialize((object)report, (StiSerializeTypes)1);
			val.SetReferenceSerializing();
			SerializeProps(props, isList: false, string.Empty, "this");
			if (report.DataBandsUsedInPageTotals != null && report.DataBandsUsedInPageTotals.Length != 0)
			{
				StiPropertyInfo val2 = new StiPropertyInfo("DataBandsUsedInPageTotals", (object)report.DataBandsUsedInPageTotals, (object)null, false, false, false, true, report.DataBandsUsedInPageTotals.GetType().ToString());
				int num = 0;
				foreach (object item in (IEnumerable)report.DataBandsUsedInPageTotals)
				{
					if (item != null)
					{
						val2.Properties.Add(new StiPropertyInfo("value", item, (object)null, false, false, false, false, (string)null));
						num++;
					}
				}
				val2.Count = num;
				StiPropertyInfoCollection val3 = new StiPropertyInfoCollection();
				val3.Add(val2);
				SerializeProps(val3, isList: false, string.Empty, "this");
			}
			if (report.RetrieveOnlyUsedData)
			{
				string[] array = StiDataSourceHelper.GetUsedDataSourcesNamesList(report).ToArray();
				if (array.Length != 0)
				{
					StiPropertyInfo val4 = new StiPropertyInfo("ListOfUsedData", (object)array, (object)null, false, false, false, true, array.GetType().ToString());
					int num2 = 0;
					string[] array2 = array;
					foreach (string text in array2)
					{
						if (text != null)
						{
							val4.Properties.Add(new StiPropertyInfo("value", (object)text, (object)null, false, false, false, false, (string)null));
							num2++;
						}
					}
					val4.Count = num2;
					StiPropertyInfoCollection val5 = new StiPropertyInfoCollection();
					val5.Add(val4);
					SerializeProps(val5, isList: false, string.Empty, "this");
				}
			}
		}
		for (int j = 0; j < reference.Count; j += 2)
		{
			object? obj = reference[j];
			StiPropertyInfo val6 = (StiPropertyInfo)((obj is StiPropertyInfo) ? obj : null);
			if (val6.Name != "Item")
			{
				if (val6.Value is IStiSerializeToCodeAsClass)
				{
					if (val6.Parent != null)
					{
						val6.Value = StiNameValidator.CorrectName(val6.Parent.Name, report) + "_" + val6.Name;
					}
					else
					{
						val6.Value = report.GetReportName() + "_" + val6.Name;
					}
					AddInit((string)reference[j + 1], isReference: true, val6);
				}
				else
				{
					val6.Value = Graphs[val6.ReferenceCode];
					AddInit((string)reference[j + 1], isReference: true, val6);
				}
			}
			else if (val6.Value is StiComponent stiComponent)
			{
				CodeExpression val7 = (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), val6.Parent.Parent.Name), val6.Parent.Name), "Add", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeSnippetExpression(stiComponent.Name) });
				Statements.Add(val7);
			}
		}
		CodeNamespace val8 = new CodeNamespace(StiOptions.Engine.DefaultNamespace);
		string[] namespaces = StiOptions.Engine.Namespaces;
		foreach (string text2 in namespaces)
		{
			val8.Imports.Add(new CodeNamespaceImport(text2));
		}
		CodeTypeDeclaration val9 = new CodeTypeDeclaration(name);
		val9.IsClass = true;
		if (string.IsNullOrEmpty(report.MasterReport))
		{
			val9.BaseTypes.Add(report.GetType());
		}
		else
		{
			val9.BaseTypes.Add(typeof(StiReport));
		}
		val9.TypeAttributes = TypeAttributes.Public;
		if (!saveForInheritedReports)
		{
			ConnectAggregatesBeginEnd();
			ConnectAggreagatesBuild();
			ConnectTextBoxReportEnd(compsProcessAtEndRemitted);
			ConnectTextBoxReportEnd(compsWordsToEndRemitted);
			ConnectChartProcessAtEnd();
			BuildArrayAggregates();
		}
		bool flag = false;
		foreach (StiPage page in report.Pages)
		{
			if (page.IsPage)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			CodeStatementCollection val10 = new CodeStatementCollection();
			CodeMethodInvokeExpression val11 = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression((CodeExpression)new CodeTypeReferenceExpression(typeof(StiSimpleText)), "CheckEndRenderRuntimes"), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeThisReferenceExpression() });
			val10.Add((CodeExpression)(object)val11);
			if (compsWordsToEndRemitted.Count > 0 || compsProcessAtEndRemitted.Count > 0)
			{
				CodeMethodInvokeExpression val12 = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression((CodeExpression)new CodeTypeReferenceExpression(typeof(StiSimpleText)), "ProcessEndRenderSetText"), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeThisReferenceExpression() });
				val10.Add((CodeExpression)(object)val12);
			}
			string eventName = "CheckEndRenderRuntimes__EndRender";
			StiEndRenderEvent ev = new StiEndRenderEvent();
			GenAddEvent(eventName, null, ev);
			GenEventMethod(eventName, ev, val10);
		}
		int num3 = -1;
		string text3 = report.GetReportName() + "_EndRender";
		foreach (CodeStatement item2 in (CollectionBase)(object)Statements)
		{
			CodeStatement val13 = item2;
			CodeAttachEventStatement val14 = (CodeAttachEventStatement)(object)((val13 is CodeAttachEventStatement) ? val13 : null);
			if (val14 != null && val14.Event.EventName == "EndRender" && ((CodeDelegateCreateExpression)/*isinst with value type is only supported in some contexts*/).MethodName == text3)
			{
				num3 = Statements.IndexOf(val13);
				break;
			}
		}
		if (num3 != -1)
		{
			CodeStatement val15 = Statements[num3];
			((CollectionBase)(object)Statements).RemoveAt(num3);
			Statements.Add(val15);
		}
		Members.Add((CodeTypeMember)(object)memberMethod);
		((CodeTypeMember)memberMethod).Attributes = (MemberAttributes)20480;
		((CodeTypeMember)memberMethod).Name = "InitializeComponent";
		CodeConstructor val16 = new CodeConstructor();
		((CodeTypeMember)val16).Attributes = (MemberAttributes)24576;
		((CodeMemberMethod)val16).Statements.Add((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeThisReferenceExpression(), "InitializeComponent", (CodeExpression[])(object)new CodeExpression[0]));
		if (saveForInheritedReports)
		{
			CodeAssignStatement val17 = new CodeAssignStatement((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "MasterReport"), (CodeExpression)new CodePrimitiveExpression((object)report.SaveToString()));
			((CodeMemberMethod)val16).Statements.Add((CodeStatement)(object)val17);
		}
		val9.Members.Add((CodeTypeMember)(object)val16);
		if (!saveForInheritedReports)
		{
			memberMethod.Statements.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "NeedsCompiling"), (CodeExpression)new CodePrimitiveExpression((object)false)));
		}
		memberMethod.Statements.AddRange(Statements);
		((CollectionBase)(object)Statements).Clear();
		memberMethod.Statements.AddRange(CollectionStatements);
		((CollectionBase)(object)CollectionStatements).Clear();
		if (serializeData && !saveForInheritedReports)
		{
			StiCodeDomRelations.Serialize(this, report);
			StiCodeDomDataSources.Serialize(this, report);
			StiCodeDomDatabases.Serialize(this, report);
			StiCodeDomBusinessObjects.Serialize(this, report);
		}
		StiCodeDomCharts.Serialize(this, report);
		memberMethod.Statements.AddRange(Statements);
		memberMethod.Statements.AddRange(CollectionStatements);
		StiCodeDomStandalone.Serialize(this, report, standaloneReportType, val9, name);
		int num4 = 1000;
		if (((CollectionBase)(object)memberMethod.Statements).Count > num4)
		{
			List<CodeMemberMethod> list = new List<CodeMemberMethod>();
			list.Add(memberMethod);
			int num5 = Members.IndexOf((CodeTypeMember)(object)memberMethod) + 1;
			CodeMemberMethod val18 = null;
			int num6 = 2;
			int num7 = 0;
			while (((CollectionBase)(object)memberMethod.Statements).Count > num4)
			{
				if (num7 == 0)
				{
					val18 = new CodeMemberMethod();
					Members.Insert(num5++, (CodeTypeMember)(object)val18);
					((CodeTypeMember)val18).Attributes = (MemberAttributes)24578;
					((CodeTypeMember)val18).Name = $"InitializeComponent{num6}";
					num6++;
					list.Add(val18);
				}
				val18.Statements.Add(memberMethod.Statements[num4]);
				((CollectionBase)(object)memberMethod.Statements).RemoveAt(num4);
				num7++;
				if (num7 == num4)
				{
					num7 = 0;
				}
			}
			num6 = 0;
			foreach (CodeMemberMethod item3 in list)
			{
				if (num6 < list.Count - 1)
				{
					CodeMemberMethod val19 = list[num6 + 1];
					memberMethod.Statements.Add((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeThisReferenceExpression(), ((CodeTypeMember)val19).Name, (CodeExpression[])(object)new CodeExpression[0]));
				}
				num6++;
			}
		}
		CodeTypeMember val20 = null;
		CodeTypeMember val21 = null;
		foreach (CodeTypeMember item4 in (CollectionBase)(object)Members)
		{
			CodeTypeMember val22 = item4;
			if (val22 is CodeMemberField && val20 == null)
			{
				val20 = val22;
			}
			else if (val22 is CodeMemberMethod && !(val21 is CodeTypeDeclaration))
			{
				val21 = val22;
			}
			else if (val22 is CodeTypeDeclaration)
			{
				val21 = val22;
			}
		}
		if (val20 == null)
		{
			val20 = val21;
		}
		val20.Comments.Add((CodeCommentStatement)(object)new StiCodeRegionStart("StiReport Designer generated code - do not modify"));
		val21.Comments.Add((CodeCommentStatement)(object)new StiCodeRegionEnd("StiReport Designer generated code - do not modify"));
		val9.Members.AddRange(Members);
		val8.Types.Add(val9);
		generator.GenerateCodeFromNamespace(val8, stringWriter, o);
		stringWriter.Flush();
		stringWriter.Close();
		if (!StiOptions.Engine.GeneratePartialReportClass)
		{
			return stringBuilder.ToString();
		}
		string text4 = stringBuilder.ToString();
		string text5 = text4.ToLower(CultureInfo.InvariantCulture);
		int startIndex = text5.IndexOf("class", StringComparison.InvariantCulture);
		StiReportLanguageType scriptLanguage = report.ScriptLanguage;
		if (scriptLanguage == StiReportLanguageType.CSharp || scriptLanguage == StiReportLanguageType.JS)
		{
			return text4.Insert(startIndex, "partial ");
		}
		return text4.Insert(startIndex, "Partial ");
	}
}
