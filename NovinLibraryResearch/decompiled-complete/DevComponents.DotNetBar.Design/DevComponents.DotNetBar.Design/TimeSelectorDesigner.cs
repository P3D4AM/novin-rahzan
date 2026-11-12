using System.Collections;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors.DateTimeAdv;

namespace DevComponents.DotNetBar.Design;

public class TimeSelectorDesigner : ControlDesigner
{
	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		if (((ControlDesigner)this).Control is TimeSelector timeSelector)
		{
			timeSelector.BackgroundStyle.Class = ElementStyleClassKeys.ItemPanelKey;
			((Control)timeSelector).AutoSize = true;
		}
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ControlDesigner)this).PreFilterProperties(properties);
		RemoveDescriptors(properties, new string[26]
		{
			"AutoScrollMargin", "AlwaysDisplayKeyAccelerators", "AutoScroll", "AutoScrollMinSize", "DisabledImagesGrayScale", "DispatchShortcuts", "Enabled", "Images", "ImageSize", "ImagesLarge",
			"ImagesMedium", "KeyTipsFont", "BackColor", "BackgroundImage", "BackgroundImageLayout", "Margin", "MaximumSize", "MinimumSize", "Padding", "RightToLeft",
			"ShowShortcutKeysInToolTips", "ShowToolTips", "TabStop", "TabIndex", "UseWaitCursor", "ImeMode"
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
		RemoveDescriptors(events, new string[56]
		{
			"BackColorChanged", "BackgroundImageChanged", "BackgroundImageLayoutChanged", "BackgroundStyle", "ButtonCheckedChanged", "CausesValidationChanged", "ChangeUICues", "ClientSizeChanged", "ContainerControlDeserialize", "ContainerControlSerialize",
			"ContainerLoadControl", "Enter", "Leave", "ContextMenuStripChanged", "ControlAdded", "ControlRemoved", "CursorChanged", "DefinitionLoaded", "DockChanged", "EnabledChanged",
			"ExpandedChange", "FontChanged", "ForeColorChanged", "ImeModeChanged", "ItemAdded", "ItemClick", "ItemDoubleClick", "ItemLayoutUpdated", "ItemRemoved", "ItemTextChanged",
			"KeyDown", "KeyPress", "KeyUp", "Layout", "MarginChanged", "OptionGroupChanging", "PaddingChanged", "Paint", "ParentChanged", "PopupClose",
			"PopupContainerLoad", "PopupContainerUnload", "PopupOpen", "PopupShowing", "RegionChanged", "Scroll", "RightToLeftChanged", "StyleChanged", "SystemColorsChanged", "TabIndexChanged",
			"TabStopChanged", "TextChanged", "Resize", "ToolTipShowing", "UserCustomize", "VisibleChanged"
		});
		((ComponentDesigner)this).PreFilterEvents(events);
	}
}
