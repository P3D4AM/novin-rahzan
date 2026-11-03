using System;

namespace DevComponents.DotNetBar.Controls;

public class TabFormTabCloseEventArgs : EventArgs
{
	public bool Cancel;

	public readonly TabFormItem Tab;

	public readonly eEventSource Source;

	public TabFormTabCloseEventArgs(TabFormItem tab, eEventSource source)
	{
		Tab = tab;
		Source = source;
	}
}
