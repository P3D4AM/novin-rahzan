using System;

namespace Stimulsoft.Editor;

[Flags]
public enum SyntaxNodeOptions
{
	None = 0,
	Indentation = 1,
	BackIndentation = 2,
	KeepIndentation = 4,
	Outlining = 8,
	CodeCompletion = 0x10
}
