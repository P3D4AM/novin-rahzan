using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace DevComponents.DotNetBar;

public class ProvideUITypeEditorEventArgs : EventArgs
{
	public readonly string PropertyName;

	public readonly PropertyDescriptor PropertyDescriptor;

	public UITypeEditor UITypeEditor;

	public bool EditorSpecified;

	public ProvideUITypeEditorEventArgs(string propertyName, PropertyDescriptor propertyDescriptor)
	{
		PropertyName = propertyName;
		PropertyDescriptor = propertyDescriptor;
	}
}
