using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.CodeDom;

internal class StiCodeDomConditions
{
	private static CodeStatement[] GetConditionActions(StiCodeDomSerializator serializator, StiComponent component, StiCondition condition, bool isAssignTextExpression)
	{
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Expected O, but got Unknown
		//IL_0348: Expected O, but got Unknown
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Expected O, but got Unknown
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Expected O, but got Unknown
		//IL_0408: Expected O, but got Unknown
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Expected O, but got Unknown
		//IL_040d: Expected O, but got Unknown
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0086: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00e6: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Expected O, but got Unknown
		//IL_0278: Expected O, but got Unknown
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Expected O, but got Unknown
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		List<CodeStatement> list = new List<CodeStatement>();
		if (!isAssignTextExpression)
		{
			if (condition.Enabled || StiOptions.Engine.ApplyConditionStyleForDisabledComponents)
			{
				if (string.IsNullOrEmpty(condition.Style))
				{
					if (component is IStiTextBrush && (condition.Permissions & StiConditionPermissions.TextColor) > StiConditionPermissions.None)
					{
						list.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeCastExpression(typeof(IStiTextBrush), (CodeExpression)new CodeArgumentReferenceExpression("sender")), "TextBrush"), serializator.GetObjectCreateExpression(typeof(StiSolidBrush), (object)new StiSolidBrush(condition.TextColor))));
					}
					if (component is IStiBrush && (condition.Permissions & StiConditionPermissions.BackColor) > StiConditionPermissions.None)
					{
						list.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeCastExpression(typeof(IStiBrush), (CodeExpression)new CodeArgumentReferenceExpression("sender")), "Brush"), serializator.GetObjectCreateExpression(typeof(StiSolidBrush), (object)new StiSolidBrush(condition.BackColor))));
					}
					if (component is IStiFont && ((condition.Permissions & StiConditionPermissions.Font) > StiConditionPermissions.None || (condition.Permissions & StiConditionPermissions.FontSize) > StiConditionPermissions.None || (condition.Permissions & StiConditionPermissions.FontStyleBold) > StiConditionPermissions.None || (condition.Permissions & StiConditionPermissions.FontStyleItalic) > StiConditionPermissions.None || (condition.Permissions & StiConditionPermissions.FontStyleUnderline) > StiConditionPermissions.None || (condition.Permissions & StiConditionPermissions.FontStyleStrikeout) > StiConditionPermissions.None))
					{
						CodeExpression[] array = (CodeExpression[])(object)new CodeExpression[3]
						{
							(CodeExpression)new CodeSnippetExpression("sender"),
							serializator.GetObjectCreateExpression(typeof(Font), condition.Font),
							serializator.GetPropertyExpression(condition.Permissions)
						};
						list.Add((CodeStatement)new CodeExpressionStatement((CodeExpression)new CodeMethodInvokeExpression(new CodeMethodReferenceExpression((CodeExpression)new CodeTypeReferenceExpression(typeof(StiConditionHelper)), "ApplyFont"), array)));
					}
					if (component is IStiBorder && condition.BorderSides != StiConditionBorderSides.NotAssigned && (condition.Permissions & StiConditionPermissions.Borders) > StiConditionPermissions.None)
					{
						StiBorderSides val = (StiBorderSides)0;
						if ((condition.BorderSides & StiConditionBorderSides.Left) > StiConditionBorderSides.None)
						{
							val = (StiBorderSides)(val | 2);
						}
						if ((condition.BorderSides & StiConditionBorderSides.Right) > StiConditionBorderSides.None)
						{
							val = (StiBorderSides)(val | 4);
						}
						if ((condition.BorderSides & StiConditionBorderSides.Top) > StiConditionBorderSides.None)
						{
							val = (StiBorderSides)(val | 1);
						}
						if ((condition.BorderSides & StiConditionBorderSides.Bottom) > StiConditionBorderSides.None)
						{
							val = (StiBorderSides)(val | 8);
						}
						list.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeCastExpression(typeof(IStiBorder), (CodeExpression)new CodeArgumentReferenceExpression("sender")), "Border"), (CodeExpression)new CodeCastExpression(typeof(StiBorder), (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeCastExpression(typeof(IStiBorder), (CodeExpression)new CodeArgumentReferenceExpression("sender")), "Border"), "Clone", (CodeExpression[])(object)new CodeExpression[0]))));
						list.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeCastExpression(typeof(IStiBorder), (CodeExpression)new CodeArgumentReferenceExpression("sender")), "Border"), "Side"), serializator.GetPropertyExpression(val)));
					}
				}
				else
				{
					CodeExpression[] array2 = (CodeExpression[])(object)new CodeExpression[2]
					{
						(CodeExpression)new CodeSnippetExpression("sender"),
						(CodeExpression)new CodePrimitiveExpression((object)condition.Style)
					};
					list.Add((CodeStatement)new CodeExpressionStatement((CodeExpression)new CodeMethodInvokeExpression(new CodeMethodReferenceExpression((CodeExpression)new CodeTypeReferenceExpression(typeof(StiConditionHelper)), "Apply"), array2)));
				}
			}
			list.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeCastExpression(typeof(StiComponent), (CodeExpression)new CodeArgumentReferenceExpression("sender")), "Enabled"), (CodeExpression)new CodePrimitiveExpression((object)condition.Enabled)));
		}
		else if (component is StiText && condition.CanAssignExpression && condition.AssignExpression != null && condition.AssignExpression.Trim().Length > 0)
		{
			string assignExpression = condition.AssignExpression;
			assignExpression = StiCodeDomFunctions.ParseFunctions(serializator, assignExpression);
			assignExpression = StiCodeDomTotalsFunctionsParser.ProcessTotals(serializator, assignExpression, component.Name);
			list.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeCastExpression(typeof(StiText), (CodeExpression)new CodeArgumentReferenceExpression("sender")), "TextValue"), (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeThisReferenceExpression(), "ToString", (CodeExpression[])(object)new CodeExpression[3]
			{
				(CodeExpression)new CodeSnippetExpression("sender"),
				(CodeExpression)new CodeSnippetExpression(assignExpression),
				(CodeExpression)new CodeSnippetExpression("true")
			})));
		}
		if (StiOptions.Engine.StopProcessingAtFirstTrueCondition || condition.BreakIfTrue)
		{
			list.Add((CodeStatement)((serializator.report.ScriptLanguage == StiReportLanguageType.CSharp || serializator.report.ScriptLanguage == StiReportLanguageType.JS) ? new CodeSnippetStatement("return;") : new CodeSnippetStatement("Return")));
		}
		return list.ToArray();
	}

	internal static void AddConditions(StiCodeDomSerializator serializator, string parent, StiPropertyInfo prop)
	{
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Expected O, but got Unknown
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Expected O, but got Unknown
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Expected O, but got Unknown
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_0546: Expected O, but got Unknown
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Expected O, but got Unknown
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Expected O, but got Unknown
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Expected O, but got Unknown
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected O, but got Unknown
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_0299: Expected O, but got Unknown
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Expected O, but got Unknown
		//IL_029f: Expected O, but got Unknown
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Expected O, but got Unknown
		//IL_050c: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Expected O, but got Unknown
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Expected O, but got Unknown
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Expected O, but got Unknown
		StiComponent stiComponent = prop.Parent.Value as StiComponent;
		bool flag = prop.Parent.Value is IStiCrossTabField;
		StiConditionsCollection stiConditionsCollection = prop.Value as StiConditionsCollection;
		List<StiBaseCondition> list = new List<StiBaseCondition>();
		List<StiBaseCondition> list2 = new List<StiBaseCondition>();
		foreach (StiBaseCondition item in stiConditionsCollection)
		{
			if (item is StiCondition)
			{
				list.Add(item);
			}
			else if (item is IStiIndicatorCondition)
			{
				list2.Add(item);
			}
		}
		if (list != null && list.Count > 0)
		{
			int num = 1;
			num++;
			for (int i = 0; i < num; i++)
			{
				bool flag2 = i == 1;
				CodeMemberMethod val = new CodeMemberMethod();
				((CodeTypeMember)val).Name = parent + "_Conditions";
				if (flag2)
				{
					((CodeTypeMember)val).Name = ((CodeTypeMember)val).Name + "TextValue";
				}
				val.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "sender"));
				if (stiComponent is StiText && (flag || flag2))
				{
					val.Parameters.Add(new CodeParameterDeclarationExpression(typeof(StiValueEventArgs), "e"));
				}
				else
				{
					val.Parameters.Add(new CodeParameterDeclarationExpression(typeof(EventArgs), "e"));
				}
				if (stiComponent is StiText && (flag || flag2))
				{
					string text = null;
					string text2 = null;
					string text3 = null;
					string text4 = null;
					if (serializator.report.ScriptLanguage == StiReportLanguageType.VB || serializator.report.ScriptLanguage == StiReportLanguageType.JS)
					{
						text = (flag ? "TypeOf e.Value is Decimal" : "TypeOf e.Value is String");
						text2 = "Dim tag As String = CType(CType(sender, StiComponent).TagValue, String)";
						text3 = "Dim tooltip As String = CType(CType(sender, StiComponent).TooltipValue, String)";
						text4 = "Dim hyperlink As String = CType(CType(sender, StiComponent).HyperlinkValue, String)";
					}
					else
					{
						text = (flag ? "e.Value is decimal" : "e.Value is string");
						text2 = "string tag = ((StiComponent)sender).TagValue as string;";
						text3 = "string tooltip = ((StiComponent)sender).ToolTipValue as string;";
						text4 = "string hyperlink = ((StiComponent)sender).HyperlinkValue as string;";
					}
					if (flag)
					{
						StiCrossSummary stiCrossSummary = stiComponent as StiCrossSummary;
						Type typeFromHandle = typeof(decimal);
						if (stiCrossSummary != null && stiCrossSummary.Summary == StiSummaryType.None)
						{
							typeFromHandle = typeof(object);
							text = ((serializator.report.ScriptLanguage != StiReportLanguageType.VB) ? "e.Value is object" : "TypeOf e.Value is Object");
						}
						CodeVariableDeclarationStatement val2 = new CodeVariableDeclarationStatement(typeFromHandle, "value", (CodeExpression)new CodePrimitiveExpression((object)0));
						CodeConditionStatement val3 = new CodeConditionStatement((CodeExpression)new CodeSnippetExpression(text), (CodeStatement[])(object)new CodeStatement[1] { (CodeStatement)new CodeAssignStatement((CodeExpression)new CodeVariableReferenceExpression("value"), (CodeExpression)new CodeCastExpression(typeFromHandle, (CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeArgumentReferenceExpression("e"), "Value"))) });
						val.Statements.Add((CodeStatement)(object)val2);
						val.Statements.Add((CodeStatement)(object)val3);
					}
					else
					{
						CodeVariableDeclarationStatement val4 = new CodeVariableDeclarationStatement(typeof(string), "value", (CodeExpression)new CodeCastExpression(typeof(string), (CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeArgumentReferenceExpression("e"), "Value")));
						val.Statements.Add((CodeStatement)(object)val4);
					}
					val.Statements.Add((CodeStatement)new CodeSnippetStatement(text2));
					val.Statements.Add((CodeStatement)new CodeSnippetStatement(text3));
					val.Statements.Add((CodeStatement)new CodeSnippetStatement(text4));
				}
				for (int j = 0; j < list.Count; j++)
				{
					int index = (StiOptions.Engine.InvertConditionsProcessingOrder ? (list.Count - 1 - j) : j);
					StiCondition stiCondition = list[index] as StiCondition;
					CodeStatement[] conditionActions = GetConditionActions(serializator, prop.Parent.Value as StiComponent, stiCondition, flag2);
					if (conditionActions != null && conditionActions.Length != 0)
					{
						CodeExpression val5 = null;
						val5 = ((!(stiCondition is StiMultiCondition)) ? StiCodeDomFilters.GetFilterExpression(serializator, prop.Parent.Value as IStiFilter, stiCondition, isConditions: true) : StiCodeDomFilters.AddFilters(serializator, stiCondition as IStiFilter));
						if (val5 != null)
						{
							CodeConditionStatement val6 = new CodeConditionStatement(val5, conditionActions, (CodeStatement[])(object)new CodeStatement[0]);
							val.Statements.Add((CodeStatement)(object)val6);
						}
					}
				}
				bool flag3 = false;
				if (flag2)
				{
					foreach (StiCondition item2 in list)
					{
						if (item2.CanAssignExpression && item2.AssignExpression != null && item2.AssignExpression.Trim().Length > 0)
						{
							flag3 = true;
							break;
						}
					}
				}
				if (((CollectionBase)(object)val.Statements).Count > 0 && (flag3 || !flag2))
				{
					if (((CollectionBase)(object)val.Statements).Count > 0)
					{
						val.Statements.Insert(0, (CodeStatement)new CodeCommentStatement(StiCodeDomSerializator.GetCheckerInfoString(parent, prop.Name)));
					}
					serializator.Members.Add((CodeTypeMember)(object)val);
					CodeExpression val7 = (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parent);
					if (stiComponent is StiText && (flag || flag2))
					{
						serializator.Statements.Add((CodeStatement)new CodeAttachEventStatement(val7, "TextProcess", (CodeExpression)new CodeDelegateCreateExpression(new CodeTypeReference(typeof(StiValueEventHandler)), (CodeExpression)new CodeThisReferenceExpression(), ((CodeTypeMember)val).Name)));
					}
					else
					{
						serializator.Statements.Add((CodeStatement)new CodeAttachEventStatement(val7, "BeforePrint", (CodeExpression)new CodeDelegateCreateExpression(new CodeTypeReference(typeof(EventHandler)), (CodeExpression)new CodeThisReferenceExpression(), ((CodeTypeMember)val).Name)));
					}
				}
			}
		}
		if (list2.Count <= 0)
		{
			return;
		}
		using List<StiBaseCondition>.Enumerator enumerator3 = list2.GetEnumerator();
		if (enumerator3.MoveNext())
		{
			StiBaseCondition current = enumerator3.Current;
			CodeExpression val8 = (CodeExpression)new CodeObjectCreateExpression(current.GetType(), serializator.GetArguments(current.GetType(), current));
			serializator.Statements.Add((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), parent), "Conditions"), "Add", (CodeExpression[])(object)new CodeExpression[1] { val8 }));
		}
	}
}
