using System;

namespace Stimulsoft.Controls.Win.Editors;

[Flags]
public enum eInputFieldNavigation
{
	None = 0,
	Tab = 1,
	Arrows = 2,
	Enter = 4,
	All = 7
}
