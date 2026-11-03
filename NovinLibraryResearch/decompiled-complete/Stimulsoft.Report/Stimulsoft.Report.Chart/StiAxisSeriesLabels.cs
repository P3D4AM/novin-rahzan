using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Chart;

public abstract class StiAxisSeriesLabels : StiSeriesLabels, IStiAxisSeriesLabels, IStiSeriesLabels, ICloneable, IStiJsonReportObject
{
	private bool showInPercent;

	[StiSerializable]
	[StiOrder(250)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that values from series must be shown as percents.")]
	public bool ShowInPercent
	{
		get
		{
			return showInPercent;
		}
		set
		{
			showInPercent = value;
			if (showInPercent && !base.Format.StartsWith("P"))
			{
				base.Format = "P2";
			}
			if (!showInPercent)
			{
				base.Format = "";
			}
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "ShowInPercent", showInPercent, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "ShowInPercent")
			{
				showInPercent = StiJsonReportObjectHelper.DeserializeBool(item);
			}
		}
	}
}
