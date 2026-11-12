using System;

namespace Stimulsoft.Editor;

[Flags]
public enum EditorSettingsTab
{
	None = 0,
	General = 1,
	Additional = 2,
	FontsAndColors = 4,
	Keymapping = 8
}
