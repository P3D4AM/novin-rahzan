using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiCrossHeaderInteraction : StiInteraction
{
	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (base.IsDefault)
			{
				return !CollapsingEnabled;
			}
			return false;
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether it is allowed or not data collapsing in the report viewer.")]
	[StiOrder(110)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public virtual bool CollapsingEnabled { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "CollapsingEnabled", CollapsingEnabled, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "CollapsingEnabled")
			{
				CollapsingEnabled = StiJsonReportObjectHelper.DeserializeBool(item);
			}
		}
	}
}
