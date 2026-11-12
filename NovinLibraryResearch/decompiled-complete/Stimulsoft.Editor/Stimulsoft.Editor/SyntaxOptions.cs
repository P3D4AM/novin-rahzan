using System;

namespace Stimulsoft.Editor;

[Flags]
public enum SyntaxOptions
{
	None = 0,
	Outline = 1,
	SmartIndent = 2,
	CodeCompletion = 4,
	SyntaxErrors = 8,
	ReparseOnLineChange = 0x10,
	QuickInfoTips = 0x20,
	AutoComplete = 0x40
}
