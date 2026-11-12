using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Dashboard.Components.Button;

public class StiButtonVisualStates : IStiButtonVisualStates, ICloneable, IStiJsonReportObject, IStiGetFonts, IStiCopyStyleExt, IStiSerializeToCodeAsClass, IStiDefault
{
	private StiButtonVisualState hover;

	private StiButtonVisualState pressed;

	private StiButtonVisualState checkedValue;

	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (!ShouldSerializeHover() && !ShouldSerializePressed())
			{
				return !ShouldSerializeChecked();
			}
			return false;
		}
	}

	[StiOrder(1)]
	[StiLocalizationKey("Report", "VisualHover")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[Description("The hover state when a user has placed a cursor above the component.")]
	public StiButtonVisualState Hover
	{
		get
		{
			return hover;
		}
		set
		{
			if (hover != value)
			{
				hover = value;
				if (value != null)
				{
					value.Parent = Parent;
				}
			}
		}
	}

	[StiOrder(1)]
	[StiLocalizationKey("Report", "VisualPressed")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[Description("The pressed state when a user pressed on the component.")]
	public StiButtonVisualState Pressed
	{
		get
		{
			return pressed;
		}
		set
		{
			if (pressed != value)
			{
				pressed = value;
				if (value != null)
				{
					value.Parent = Parent;
				}
			}
		}
	}

	[StiOrder(1)]
	[StiLocalizationKey("Report", "VisualChecked")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[Description("The non-interactive state of the component.")]
	public StiButtonVisualState Checked
	{
		get
		{
			return checkedValue;
		}
		set
		{
			if (checkedValue != value)
			{
				checkedValue = value;
				if (value != null)
				{
					value.Parent = Parent;
				}
			}
		}
	}

	[Browsable(false)]
	internal StiButtonElement Parent { get; set; }

	public object Clone()
	{
		StiButtonVisualStates stiButtonVisualStates = MemberwiseClone() as StiButtonVisualStates;
		stiButtonVisualStates.Hover = Hover.Clone() as StiButtonVisualState;
		stiButtonVisualStates.Pressed = Pressed.Clone() as StiButtonVisualState;
		stiButtonVisualStates.Checked = Checked.Clone() as StiButtonVisualState;
		return stiButtonVisualStates;
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		if (IsDefault)
		{
			return null;
		}
		JObject jObject = new JObject();
		if (Hover != null)
		{
			jObject.AddPropertyJObject("Hover", Hover.SaveToJsonObject(mode));
		}
		if (Pressed != null)
		{
			jObject.AddPropertyJObject("Pressed", Pressed.SaveToJsonObject(mode));
		}
		if (Checked != null)
		{
			jObject.AddPropertyJObject("Checked", Checked.SaveToJsonObject(mode));
		}
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Hover":
				Hover.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Pressed":
				Pressed.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Checked":
				Checked.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public List<StiFont> GetFonts()
	{
		List<StiFont> source = new List<StiFont>
		{
			new StiFont(Hover.Font),
			new StiFont(Pressed.Font),
			new StiFont(Checked.Font)
		};
		return source.Distinct().ToList();
	}

	public void InvokeSetCopyStyle()
	{
		if (StiCopyStyleExtHub.VisualStateHover != null)
		{
			Hover = new StiButtonVisualState();
			Hover.LoadFromJsonObject(StiCopyStyleExtHub.VisualStateHover);
		}
		if (StiCopyStyleExtHub.VisualStatePressed != null)
		{
			Pressed = new StiButtonVisualState();
			Pressed.LoadFromJsonObject(StiCopyStyleExtHub.VisualStatePressed);
		}
		if (StiCopyStyleExtHub.VisualStateChecked != null)
		{
			Checked = new StiButtonVisualState();
			Checked.LoadFromJsonObject(StiCopyStyleExtHub.VisualStateDisabled);
		}
	}

	public void InvokeGetCopyStyle()
	{
		StiCopyStyleExtHub.VisualStateHover = Hover.SaveToJsonObject(StiJsonSaveMode.Report);
		StiCopyStyleExtHub.VisualStatePressed = Pressed.SaveToJsonObject(StiJsonSaveMode.Report);
		StiCopyStyleExtHub.VisualStateChecked = Checked.SaveToJsonObject(StiJsonSaveMode.Report);
	}

	public IStiButtonVisualState GetHoverState()
	{
		return Hover;
	}

	public IStiButtonVisualState GetPressedState()
	{
		return Pressed;
	}

	public IStiButtonVisualState GetCheckedState()
	{
		return Checked;
	}

	private bool ShouldSerializeHover()
	{
		if (Hover != null)
		{
			return !Hover.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializePressed()
	{
		if (Pressed != null)
		{
			return !Pressed.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeChecked()
	{
		if (Checked != null)
		{
			return !Checked.IsDefault;
		}
		return true;
	}

	public StiButtonVisualStates()
	{
		Hover = new StiButtonVisualState();
		Pressed = new StiButtonVisualState();
		Checked = new StiButtonVisualState();
	}
}
