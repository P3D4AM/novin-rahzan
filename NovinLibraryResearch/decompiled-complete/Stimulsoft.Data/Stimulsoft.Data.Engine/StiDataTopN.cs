using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Data.Design;

namespace Stimulsoft.Data.Engine;

[TypeConverter(typeof(StiDataTopNConverter))]
public class StiDataTopN : IStiJsonReportObject, ICloneable
{
	[DefaultValue(StiDataTopNMode.None)]
	[StiSerializable]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets output values mode.")]
	[RefreshProperties(RefreshProperties.All)]
	public StiDataTopNMode Mode { get; set; }

	[DefaultValue(5)]
	[StiSerializable]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[Description("Gets or sets the number of output values.")]
	[RefreshProperties(RefreshProperties.All)]
	public int Count { get; set; } = 5;


	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether to display other values.")]
	[RefreshProperties(RefreshProperties.All)]
	public bool ShowOthers { get; set; } = true;


	[StiSerializable]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[Description("Gets or sets signature for other values.")]
	[RefreshProperties(RefreshProperties.All)]
	public string OthersText { get; set; } = "";


	[StiSerializable]
	[DefaultValue("")]
	[Description("Gets or sets a measure field name.")]
	[Browsable(false)]
	public string MeasureField { get; set; } = "";


	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (Mode == StiDataTopNMode.None && Count == 5 && ShowOthers && OthersText == "")
			{
				return MeasureField == "";
			}
			return false;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyEnum("Mode", Mode, StiDataTopNMode.None);
		jObject.AddPropertyInt("Count", Count, 5);
		jObject.AddPropertyBool("ShowOthers", ShowOthers, defaultValue: true);
		jObject.AddPropertyString("OthersText", OthersText, "");
		jObject.AddPropertyString("MeasureField", MeasureField, "");
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Mode":
			{
				string text = item.DeserializeString();
				if (text != null)
				{
					Mode = (StiDataTopNMode)Enum.Parse(typeof(StiDataTopNMode), text);
				}
				break;
			}
			case "Count":
				Count = item.DeserializeInt();
				break;
			case "ShowOthers":
				ShowOthers = item.DeserializeBool();
				break;
			case "OthersText":
				OthersText = item.DeserializeString();
				break;
			case "MeasureField":
				MeasureField = item.DeserializeString();
				break;
			}
		}
	}

	internal static StiDataTopN CreateFromJsonObject(JObject jObject)
	{
		StiDataTopN stiDataTopN = new StiDataTopN();
		stiDataTopN.LoadFromJsonObject(jObject);
		return stiDataTopN;
	}

	public object Clone()
	{
		return MemberwiseClone() as StiDataTopN;
	}

	public override string ToString()
	{
		if (Mode == StiDataTopNMode.None)
		{
			return Loc.GetEnum("SelectionModeNone");
		}
		return string.Format("{0} {1}{2}", (Mode == StiDataTopNMode.Bottom) ? Loc.GetMain("Bottom") : Loc.GetMain("Top"), Count, (!string.IsNullOrEmpty(MeasureField)) ? (" [" + MeasureField + "]") : "");
	}

	public int GetUniqueCode()
	{
		int mode = (int)Mode;
		mode = (mode * 397) ^ Count;
		mode = (mode * 397) ^ ShowOthers.GetHashCode();
		mode = (mode * 397) ^ ((OthersText != null) ? OthersText.GetHashCode() : 0);
		return (mode * 397) ^ ((MeasureField != null) ? MeasureField.GetHashCode() : 0);
	}

	public StiDataTopN()
	{
	}

	public StiDataTopN(StiDataTopNMode mode, int count, bool showOthers, string othersText, string measureField)
	{
		Mode = mode;
		Count = count;
		ShowOthers = showOthers;
		OthersText = othersText;
		MeasureField = measureField;
	}
}
