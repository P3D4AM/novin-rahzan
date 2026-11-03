using System;

namespace DevComponents.DotNetBar.Controls;

public interface IScrollBarOverrideSupport
{
	bool DesignMode { get; }

	event EventHandler NonClientSizeChanged;

	event ScrollValueChangedHandler ScrollBarValueChanged;

	event EventHandler ControlMoved;
}
