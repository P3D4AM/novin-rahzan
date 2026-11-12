using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using DevComponents.Editors.DateTimeAdv;

namespace DevComponents.DotNetBar.Design;

public class DateTimeInputDesigner : VisualControlBaseDesigner
{
	public override SelectionRules SelectionRules
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			SelectionRules val = ((ControlDesigner)this).SelectionRules;
			if (!((ControlDesigner)this).Control.AutoSize)
			{
				val = (SelectionRules)(val & -4);
			}
			return val;
		}
	}

	public DateTimeInputDesigner()
	{
		((ControlDesigner)this).AutoResizeHandles = true;
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		if (((ControlDesigner)this).Control is DateTimeInput dateTimeInput)
		{
			dateTimeInput.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = eColorSchemePart.PanelBackground;
			dateTimeInput.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
			dateTimeInput.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
			dateTimeInput.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = eColorSchemePart.BarBackground;
			dateTimeInput.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = eColorSchemePart.BarBackground2;
			dateTimeInput.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
			dateTimeInput.MonthCalendar.CommandsBackgroundStyle.BorderTop = eStyleBorderType.Solid;
			dateTimeInput.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = eColorSchemePart.BarDockedBorder;
			dateTimeInput.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
			dateTimeInput.ButtonDropDown.Visible = true;
			dateTimeInput.MonthCalendar.TodayButtonVisible = true;
			dateTimeInput.MonthCalendar.ClearButtonVisible = true;
			dateTimeInput.ButtonDropDown.Shortcut = eShortcut.AltDown;
			dateTimeInput.Style = eDotNetBarStyle.StyleManagerControlled;
		}
		base.InitializeNewComponent(defaultValues);
	}

	private void RemoveDescriptors(IDictionary properties, string[] propNames)
	{
		foreach (string key in propNames)
		{
			if (properties.Contains(key))
			{
				if (properties[key] is PropertyDescriptor)
				{
					properties[key] = TypeDescriptor.CreateProperty(((ComponentDesigner)this).Component.GetType(), (PropertyDescriptor)properties[key], new BrowsableAttribute(browsable: false));
				}
				else if (properties[key] is EventDescriptor)
				{
					properties[key] = TypeDescriptor.CreateEvent(((ComponentDesigner)this).Component.GetType(), (EventDescriptor)properties[key], new BrowsableAttribute(browsable: false));
				}
				else
				{
					properties.Remove(key);
				}
			}
		}
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ControlDesigner)this).PreFilterProperties(properties);
		RemoveDescriptors(properties, new string[7] { "AllowDrop", "BackColor", "MaximumSize", "MinimumSize", "RightToLeft", "UseWaitCursor", "ImeMode" });
	}

	protected override void PreFilterEvents(IDictionary events)
	{
		RemoveDescriptors(events, new string[41]
		{
			"BackColorChanged", "BackgroundImageChanged", "BackgroundImageLayoutChanged", "BackgroundStyle", "CausesValidationChanged", "ChangeUICues", "ClientSizeChanged", "Colors", "ContextMenuStripChanged", "ControlAdded",
			"ControlRemoved", "CursorChanged", "DefinitionLoaded", "DockChanged", "EnabledChanged", "FontChanged", "ForeColorChanged", "ImeModeChanged", "KeyDown", "KeyPress",
			"KeyUp", "Layout", "MarginChanged", "MonthCalendar", "PaddingChanged", "Paint", "ParentChanged", "PopupClose", "PopupContainerLoad", "PopupContainerUnload",
			"PopupOpen", "PopupShowing", "RegionChanged", "RightToLeftChanged", "StyleChanged", "SystemColorsChanged", "TabIndexChanged", "TabStopChanged", "Resize", "SizeChanged",
			"VisibleChanged"
		});
		((ComponentDesigner)this).PreFilterEvents(events);
	}
}
