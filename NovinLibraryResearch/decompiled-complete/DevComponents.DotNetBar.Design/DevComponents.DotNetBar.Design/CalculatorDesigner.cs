using System.Collections;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Design;

public class CalculatorDesigner : ControlDesigner
{
	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		if (((ControlDesigner)this).Control is Calculator calculator)
		{
			((Control)calculator).AutoSize = true;
		}
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ControlDesigner)this).PreFilterProperties(properties);
		RemoveDescriptors(properties, new string[22]
		{
			"AutoScrollMargin", "AlwaysDisplayKeyAccelerators", "AutoScroll", "AutoScrollMinSize", "AutoValidate", "BorderStyle", "ForeColor", "DisabledImagesGrayScale", "DispatchShortcuts", "Enabled",
			"BackColor", "BackgroundImage", "BackgroundImageLayout", "Margin", "MaximumSize", "MinimumSize", "Padding", "RightToLeft", "TabStop", "TabIndex",
			"UseWaitCursor", "ImeMode"
		});
	}

	private void RemoveDescriptors(IDictionary properties, string[] propNames)
	{
		foreach (string key in propNames)
		{
			if (properties.Contains(key))
			{
				properties.Remove(key);
			}
		}
	}

	protected override void PreFilterEvents(IDictionary events)
	{
		RemoveDescriptors(events, new string[48]
		{
			"AutoSizeChanged", "AutoValidate", "BackColorChanged", "BackgroundImageChanged", "BackgroundImageLayoutChanged", "BindingContext", "BackgroundStyle", "ButtonCheckedChanged", "CausesValidationChanged", "ChangeUICues",
			"ClientSizeChanged", "ContainerControlDeserialize", "ContainerControlSerialize", "ContainerLoadControl", "Enter", "Leave", "ContextMenuStripChanged", "ControlAdded", "ControlRemoved", "CursorChanged",
			"DefinitionLoaded", "DockChanged", "EnabledChanged", "ExpandedChange", "FontChanged", "ForeColorChanged", "ImeModeChanged", "KeyDown", "KeyPress", "KeyUp",
			"Layout", "MarginChanged", "PaddingChanged", "Paint", "ParentChanged", "PreviewKeyDown", "RegionChanged", "Scroll", "RightToLeftChanged", "StyleChanged",
			"SystemColorsChanged", "TabIndexChanged", "TabStopChanged", "TextChanged", "Resize", "Validating", "Validated", "VisibleChanged"
		});
		((ComponentDesigner)this).PreFilterEvents(events);
	}
}
