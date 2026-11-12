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
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.PivotTable;

[TypeConverter(typeof(StiPivotRowConverter))]
public class StiPivotRow : StiMeter, IStiDimensionMeter, IStiMeter, IStiHorAlignment, IStiTextFormat, IStiTableColumnSize, IStiPivotRow, IStiMeterRules, IStiDataTopN
{
	[StiSerializable]
	[DefaultValue(StiHorAlignment.Center)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiHorAlignment HorAlignment { get; set; } = StiHorAlignment.Center;


	[StiSerializable(StiSerializationVisibility.Class)]
	[Editor("Stimulsoft.Report.Components.TextFormats.Design.StiTextFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiFormatService TextFormat { get; set; } = new StiGeneralFormatService();


	[Description("Gets or sets settings of the column size.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	public StiTableColumnSize Size { get; set; } = new StiTableColumnSize();


	[StiSerializable(StiSerializationVisibility.Class)]
	[Browsable(false)]
	public StiDataTopN TopN { get; set; } = new StiDataTopN();


	[DefaultValue(true)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value that indicates whether it is necessary to output totals or not.")]
	public virtual bool ShowTotal { get; set; } = true;


	[StiSerializable]
	[DefaultValue("")]
	[Description("Gets or sets a value that is displayed as a label for a total.")]
	public string TotalLabel { get; set; } = "";


	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.PivotRow;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("Components", "StiPivotRow");

	public void CheckRules()
	{
		Size?.CheckRules();
	}

	public override object Clone()
	{
		StiPivotRow stiPivotRow = base.Clone() as StiPivotRow;
		stiPivotRow.TextFormat = TextFormat.Clone() as StiFormatService;
		stiPivotRow.TopN = TopN.Clone() as StiDataTopN;
		stiPivotRow.Size = Size?.Clone() as StiTableColumnSize;
		return stiPivotRow;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyEnum("HorAlignment", HorAlignment, StiHorAlignment.Center);
		jObject.AddPropertyBool("ShowTotal", ShowTotal, defaultValue: true);
		jObject.AddPropertyStringNullOrEmpty("TotalLabel", TotalLabel);
		if (!(TextFormat is StiGeneralFormatService) && TextFormat != null)
		{
			jObject.AddPropertyJObject("TextFormat", TextFormat.SaveToJsonObject(mode));
		}
		if (TopN != null)
		{
			jObject.AddPropertyJObject("TopN", TopN.SaveToJsonObject(mode));
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
				HorAlignment = item.DeserializeEnum<StiHorAlignment>();
				break;
			case "ShowTotal":
				ShowTotal = item.DeserializeBool();
				break;
			case "TotalLabel":
				TotalLabel = item.DeserializeString();
				break;
			case "TextFormat":
				TextFormat = StiFormatService.CreateFromJsonObject((JObject)item.Value);
				break;
			case "TopN":
				TopN = new StiDataTopN();
				TopN.LoadFromJsonObject((JObject)item.Value);
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
		uniqueCode = (uniqueCode * 397) ^ ((TopN != null) ? TopN.GetUniqueCode() : 0);
		uniqueCode = (uniqueCode * 397) ^ ShowTotal.GetHashCode();
		uniqueCode = (uniqueCode * 397) ^ TotalLabel.GetHashCode();
		return (uniqueCode * 397) ^ ((Size != null) ? Size.GetUniqueCode() : 0);
	}

	public StiPivotRow()
	{
	}

	public StiPivotRow(string key, string expression, string label)
		: base(key, expression, label)
	{
	}

	public StiPivotRow(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat)
		: base(expression, label)
	{
		HorAlignment = horAlignment;
		TextFormat = textFormat;
	}

	public StiPivotRow(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat)
		: base(key, expression, label)
	{
		HorAlignment = horAlignment;
		TextFormat = textFormat;
	}

	public StiPivotRow(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiDataTopN topN)
		: base(key, expression, label)
	{
		HorAlignment = horAlignment;
		TextFormat = textFormat;
		TopN = topN;
	}

	public StiPivotRow(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiDataTopN topN, bool showTotal, string totalLabel, StiTableColumnSize size)
		: base(key, expression, label)
	{
		HorAlignment = horAlignment;
		TextFormat = textFormat;
		TopN = topN;
		ShowTotal = showTotal;
		TotalLabel = totalLabel;
		Size = size;
	}
}
