using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiAxisInteraction : IStiAxisInteraction, ICloneable, IStiJsonReportObject, IStiDefault
{
	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (!ShowScrollBar)
			{
				return RangeScrollEnabled;
			}
			return false;
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that scroll bar will be shown.")]
	[DefaultValue(false)]
	public bool ShowScrollBar { get; set; }

	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether the range of axis can be scrolled.")]
	public virtual bool RangeScrollEnabled { get; set; } = true;


	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "ShowScrollBar", ShowScrollBar, false, false);
		JsonExtensions.AddPropertyBool(val, "RangeScrollEnabled", RangeScrollEnabled, true, false);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "ShowScrollBar"))
			{
				if (name == "RangeScrollEnabled")
				{
					RangeScrollEnabled = StiJsonReportObjectHelper.DeserializeBool(item);
				}
			}
			else
			{
				ShowScrollBar = StiJsonReportObjectHelper.DeserializeBool(item);
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone() as IStiAxisInteraction;
	}

	public StiAxisInteraction()
	{
	}

	[StiUniversalConstructor("Interaction")]
	public StiAxisInteraction(bool showScrollBar, bool rangeScrollEnabled)
	{
		ShowScrollBar = showScrollBar;
		RangeScrollEnabled = rangeScrollEnabled;
	}
}
