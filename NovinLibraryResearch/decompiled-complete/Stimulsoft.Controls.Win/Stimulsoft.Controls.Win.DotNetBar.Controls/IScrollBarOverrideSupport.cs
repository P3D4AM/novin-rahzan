using System;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

public interface IScrollBarOverrideSupport
{
	event EventHandler NonClientSizeChanged;

	event ScrollValueChangedHandler ScrollBarValueChanged;
}
