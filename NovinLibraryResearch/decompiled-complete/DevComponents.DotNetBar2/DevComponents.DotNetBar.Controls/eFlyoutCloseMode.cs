using System;

namespace DevComponents.DotNetBar.Controls;

[Flags]
public enum eFlyoutCloseMode
{
	Manual = 0,
	ClickOutside = 1,
	TargetControlHidden = 2,
	TargetControlLostFocus = 3,
	ParentFormDeactivate = 4
}
