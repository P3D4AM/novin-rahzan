using System;

namespace Stimulsoft.Editor;

[Flags]
public enum ColorFlags
{
	None = 0,
	WhiteSpace = 1,
	Tabulation = 2,
	OutlineSection = 4,
	MisSpelledWord = 8,
	HyperText = 0x10,
	Brace = 0x20,
	WaveLine = 0x40,
	CodeSnippet = 0x80
}
