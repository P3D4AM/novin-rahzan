using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

[Flags]
public enum eTaskDialogButton
{
	Ok = 1,
	Yes = 2,
	No = 4,
	Cancel = 8,
	Retry = 0x10,
	Close = 0x20
}
