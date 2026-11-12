using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

[Flags]
public enum eDrawCaption : uint
{
	DC_ACTIVE = 1u,
	DC_SMALLCAP = 2u,
	DC_ICON = 4u,
	DC_TEXT = 8u,
	DC_INBUTTON = 0x10u,
	DC_GRADIENT = 0x20u
}
