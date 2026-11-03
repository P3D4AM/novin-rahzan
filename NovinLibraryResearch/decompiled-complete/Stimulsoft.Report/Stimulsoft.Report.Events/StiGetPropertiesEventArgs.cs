using System;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Events;

public class StiGetPropertiesEventArgs : EventArgs
{
	public object[] SelectedObjects { get; set; }

	public PropertyDescriptorCollection Properties { get; set; }

	public PropertySort PropertySort { get; set; }

	public StiGuiMode GuiMode { get; set; }

	public StiGetPropertiesEventArgs(object[] selectedObjects, PropertyDescriptorCollection properties, PropertySort propertySort, StiGuiMode guiMode)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		SelectedObjects = selectedObjects;
		Properties = properties;
		PropertySort = propertySort;
		GuiMode = guiMode;
	}
}
