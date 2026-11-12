using System;

namespace Stimulsoft.Report.Design;

[Flags]
public enum StiCodeEditorMode
{
	ShowDictionary = 1,
	Expression = 2,
	SQL = 4,
	ShortLineTerminator = 8,
	Collapsing = 0x10,
	StatusBar = 0x20,
	WantTabs = 0x40,
	WantReturns = 0x80,
	ReportCode = 0x100,
	DataExpression = 0x200,
	OkButtonOnly = 0x400,
	IgnoreAskOnClosing = 0x800,
	ExpressionTitle = 0x1000,
	EditorDashboardExpression = 0x2C1,
	EditorText = 0xC0,
	EditorCode = 0xD1,
	EditorReportCode = 0x1F0,
	EditorExpression = 0xCB,
	EditorAppExpression = 0x10C9,
	EditorInteraction = 0x8C9,
	EditorSQL = 0xC4,
	EditorEvent = 0xD1
}
