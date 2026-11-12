using System.ComponentModel;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.PivotTable.Design;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.PivotTable;

[TypeConverter(typeof(StiPivotSummaryConverter))]
public class StiPivotSummary : StiMeasureMeter, IStiHorAlignment, IStiTextFormat, IStiPivotSummary, IStiMeterRules, IStiTableColumnSize
{
	[StiSerializable]
	[DefaultValue(StiHorAlignment.Right)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiHorAlignment HorAlignment { get; set; } = StiHorAlignment.Center;


	[StiSerializable(StiSerializationVisibility.Class)]
	[Editor("Stimulsoft.Report.Components.TextFormats.Design.StiTextFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiFormatService TextFormat { get; set; } = new StiGeneralFormatService();


	[Description("Gets or sets settings of the Summary size.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	public StiTableColumnSize Size { get; set; } = new StiTableColumnSize();


	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.PivotSummary;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("Components", "StiPivotSummary");

	public void CheckRules()
	{
		Size?.CheckRules();
	}

	public override object Clone()
	{
		StiPivotSummary stiPivotSummary = base.Clone() as StiPivotSummary;
		stiPivotSummary.TextFormat = TextFormat.Clone() as StiFormatService;
		stiPivotSummary.Size = Size?.Clone() as StiTableColumnSize;
		return stiPivotSummary;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyEnum("HorAlignment", HorAlignment, StiHorAlignment.Center);
		if (!(TextFormat is StiGeneralFormatService) && TextFormat != null)
		{
			jObject.AddPropertyJObject("TextFormat", TextFormat.SaveToJsonObject(mode));
		}
		if (Size != null)
		{
			jObject.AddPropertyJObject("Size", Size.SaveToJsonObject(mode));
		}
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "HorAlignment":
				try
				{
					HorAlignment = item.DeserializeEnum<StiHorAlignment>();
				}
				catch
				{
					HorAlignment = StiHorAlignment.Center;
				}
				break;
			case "TextFormat":
				TextFormat = StiFormatService.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Size":
				Size.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	private bool ShouldSerializeTextFormat()
	{
		return !(TextFormat is StiGeneralFormatService);
	}

	private bool ShouldSerializeSize()
	{
		if (Size != null)
		{
			return !Size.IsDefault;
		}
		return true;
	}

	public override int GetUniqueCode()
	{
		int uniqueCode = base.GetUniqueCode();
		uniqueCode = (uniqueCode * 397) ^ (int)HorAlignment;
		uniqueCode = (uniqueCode * 397) ^ ((TextFormat != null) ? TextFormat.GetHashCode() : 0);
		return (uniqueCode * 397) ^ ((Size != null) ? Size.GetUniqueCode() : 0);
	}

	public StiPivotSummary()
	{
	}

	public StiPivotSummary(string key, string expression, string label)
		: base(key, expression, label)
	{
	}

	public StiPivotSummary(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat)
		: base(expression, label)
	{
		HorAlignment = horAlignment;
		TextFormat = textFormat;
	}

	public StiPivotSummary(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat)
		: base(key, expression, label)
	{
		HorAlignment = horAlignment;
		TextFormat = textFormat;
	}

	public StiPivotSummary(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnSize size)
		: base(key, expression, label)
	{
		HorAlignment = horAlignment;
		TextFormat = textFormat;
		Size = size;
	}
}
