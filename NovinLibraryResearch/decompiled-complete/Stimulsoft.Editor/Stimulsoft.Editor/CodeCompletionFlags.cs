using System;

namespace Stimulsoft.Editor;

[Flags]
public enum CodeCompletionFlags
{
	None = 0,
	CloseOnEscape = 1,
	AcceptOnEnter = 2,
	CloseOnMouseLeave = 4,
	CloseOnLostFocus = 8,
	AcceptOnClick = 0x10,
	AcceptOnDblClick = 0x20,
	FeetToScreen = 0x40,
	KeepActive = 0x80,
	AcceptOnDelimiter = 0x100,
	AcceptOnTab = 0x200,
	AcceptOnSpace = 0x400
}
