using System;

namespace Stimulsoft.Editor;

[Flags]
public enum IndentOptions
{
	None = 0,
	AutoIndent = 1,
	SmartIndent = 2,
	UsePrevIndent = 4,
	JumpToIndent = 8
}
