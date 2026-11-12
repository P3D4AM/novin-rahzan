using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public interface ISuperTooltipInfoProvider
{
	Rectangle ComponentRectangle { get; }

	event EventHandler DisplayTooltip;

	event EventHandler HideTooltip;
}
