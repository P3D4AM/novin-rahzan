using System;

namespace Stimulsoft.Editor;

[Flags]
public enum SearchOptions
{
	None = 0,
	CaseSensitive = 1,
	WholeWordsOnly = 2,
	RegularExpressions = 4,
	BackwardSearch = 8,
	SelectionOnly = 0x10,
	EntireScope = 0x20,
	SearchHiddenText = 0x40,
	FindTextAtCursor = 0x80,
	PromptOnReplace = 0x100,
	FindSelectedText = 0x200,
	CycledSearch = 0x400,
	SilentSearch = 0x800
}
