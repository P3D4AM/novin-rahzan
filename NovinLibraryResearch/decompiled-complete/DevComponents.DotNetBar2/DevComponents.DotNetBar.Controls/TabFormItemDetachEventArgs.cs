using System;

namespace DevComponents.DotNetBar.Controls;

public class TabFormItemDetachEventArgs : EventArgs
{
	public TabParentForm TabForm;

	public TabFormControl TabControl;

	public readonly TabFormItem TabFormItem;

	public bool Cancel;

	public TabFormItemDetachEventArgs(TabFormItem tab)
	{
		TabFormItem = tab;
	}
}
