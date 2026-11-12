using System;

namespace Stimulsoft.Editor;

[Flags]
public enum SelectionOptions
{
	None = 0,
	DisableSelection = 1,
	DisableDragging = 2,
	SelectBeyondEol = 4,
	UseColors = 8,
	HideSelection = 0x10,
	SelectLineOnDblClick = 0x20,
	DeselectOnCopy = 0x40,
	PersistentBlocks = 0x80,
	OverwriteBlocks = 0x100,
	SmartFormat = 0x200,
	WordSelect = 0x400,
	DrawBorder = 0x800,
	SelectLineOnTripleClick = 0x1000,
	DeselectOnDblClick = 0x2000,
	ConvertToSpacesOnPaste = 0x4000,
	RtfClipboard = 0x8000,
	ClearOnDrag = 0x10000,
	CopyLineWhenEmpty = 0x20000,
	DisableCodeSnippetOnTab = 0x40000
}
