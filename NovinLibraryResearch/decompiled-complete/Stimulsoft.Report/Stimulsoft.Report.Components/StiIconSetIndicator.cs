using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components;

[StiGdiIndicatorTypePainter(typeof(StiIconSetGdiIndicatorTypePainter))]
[StiWpfIndicatorTypePainter("Stimulsoft.Report.Painters.StiIconSetWpfIndicatorTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiIconSetIndicator : StiIndicator
{
	[StiSerializable]
	[DefaultValue(StiIcon.None)]
	public StiIcon Icon { get; set; }

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public ContentAlignment Alignment { get; set; } = (ContentAlignment)16;


	public byte[] CustomIcon { get; set; }

	public Size? CustomIconSize { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "Icon", (Enum)Icon, (Enum)StiIcon.None);
		JsonExtensions.AddPropertyEnum(val, "Alignment", (Enum)(object)Alignment, (Enum)(object)(ContentAlignment)16);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "Icon"))
			{
				if (name == "Alignment")
				{
					Alignment = StiJsonReportObjectHelper.DeserializeEnum<ContentAlignment>(item);
				}
			}
			else
			{
				Icon = StiJsonReportObjectHelper.DeserializeEnum<StiIcon>(item);
			}
		}
	}

	public StiIconSetIndicator()
	{
	}//IL_0003: Unknown result type (might be due to invalid IL or missing references)


	public StiIconSetIndicator(StiIcon icon, ContentAlignment alignment)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Icon = icon;
		Alignment = alignment;
	}
}
