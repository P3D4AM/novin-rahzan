using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Chart.Design;

namespace Stimulsoft.Report.Chart;

public abstract class StiBaseLineSeries : StiSeries, IStiBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyColorNegative, IStiShowNullsSeries, IStiShowZerosSeries
{
	private bool showNulls = true;

	private bool showZeros = true;

	private IStiMarker marker = new StiMarker();

	private IStiLineMarker lineMarker = new StiLineMarker();

	private Color lineColor = Color.Black;

	private StiPenStyle lineStyle;

	private bool lighting = true;

	private float lineWidth = 2f;

	private int labelsOffset;

	private Color lineColorNegative = Color.Firebrick;

	private bool allowApplyColorNegative;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[Description("Gets or sets value which indicates whether it is necessary to show the series element, if the series value of this bar is null.")]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ShowNulls
	{
		get
		{
			return showNulls;
		}
		set
		{
			showNulls = value;
		}
	}

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets value which indicates whether it is necessary to show the series element, if the series value is 0.")]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ShowZeros
	{
		get
		{
			return showZeros;
		}
		set
		{
			showZeros = value;
		}
	}

	[Obsolete("Please use Marker.Visible property instead ShowMarker property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[StiNonSerialized]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool ShowMarker
	{
		get
		{
			return Marker.Visible;
		}
		set
		{
			Marker.Visible = value;
		}
	}

	[Obsolete("Please use Marker.Brush property instead MarkerColor property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[StiNonSerialized]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color MarkerColor
	{
		get
		{
			return StiBrush.ToColor(Marker.Brush);
		}
		set
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			Marker.Brush = (StiBrush)new StiSolidBrush(value);
			Marker.BorderColor = StiColorUtils.Dark(value, (byte)50);
		}
	}

	[Obsolete("Please use Marker.Size property instead MarkerSize property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[StiNonSerialized]
	[DefaultValue(6f)]
	public float MarkerSize
	{
		get
		{
			return Marker.Size;
		}
		set
		{
			Marker.Size = value;
		}
	}

	[Obsolete("Please use Marker.Type property instead MarkerType property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[StiNonSerialized]
	[DefaultValue(StiMarkerType.Circle)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiMarkerType MarkerType
	{
		get
		{
			return ((StiMarker)Marker).Type;
		}
		set
		{
			((StiMarker)Marker).Type = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets marker settings.")]
	[TypeConverter(typeof(StiMarkerConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual IStiMarker Marker
	{
		get
		{
			return marker;
		}
		set
		{
			marker = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets line marker settings.")]
	[TypeConverter(typeof(StiLineMarkerConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual IStiLineMarker LineMarker
	{
		get
		{
			return lineMarker;
		}
		set
		{
			lineMarker = value;
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets line color of series.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color LineColor
	{
		get
		{
			return lineColor;
		}
		set
		{
			lineColor = value;
		}
	}

	[Editor("Stimulsoft.Base.Drawing.Design.StiPenStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a line style of series.")]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiPenStyle LineStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return lineStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			lineStyle = value;
		}
	}

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that light effect will be shown.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool Lighting
	{
		get
		{
			return lighting;
		}
		set
		{
			lighting = value;
		}
	}

	[DefaultValue(2f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets line width of series.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float LineWidth
	{
		get
		{
			return lineWidth;
		}
		set
		{
			if (value > 0f)
			{
				lineWidth = value;
			}
		}
	}

	[DefaultValue(0)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets vertical labels offset.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int LabelsOffset
	{
		get
		{
			return labelsOffset;
		}
		set
		{
			labelsOffset = value;
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a line color of series for negative values.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color LineColorNegative
	{
		get
		{
			return lineColorNegative;
		}
		set
		{
			lineColorNegative = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates that the specific color for negative values will be used.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool AllowApplyColorNegative
	{
		get
		{
			return allowApplyColorNegative;
		}
		set
		{
			allowApplyColorNegative = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public StiShowEmptyCellsAs ShowNullsAs { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public StiShowEmptyCellsAs ShowZerosAs { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "ShowZeros", ShowZeros, true, false);
		JsonExtensions.AddPropertyBool(val, "ShowNulls", ShowNulls, true, false);
		JsonExtensions.AddPropertyJObject(val, "Marker", ((IStiJsonReportObject)Marker).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "LineMarker", ((IStiJsonReportObject)LineMarker).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyColor(val, "LineColor", LineColor, Color.Black);
		JsonExtensions.AddPropertyEnum(val, "LineStyle", (Enum)(object)LineStyle, (Enum)(object)(StiPenStyle)0);
		JsonExtensions.AddPropertyBool(val, "Lighting", Lighting, true, false);
		JsonExtensions.AddPropertyFloat(val, "LineWidth", LineWidth, 2f);
		JsonExtensions.AddPropertyInt(val, "LabelsOffset", labelsOffset, 0);
		JsonExtensions.AddPropertyColor(val, "LineColorNegative", LineColorNegative, Color.Firebrick);
		JsonExtensions.AddPropertyBool(val, "AllowApplyColorNegative", AllowApplyColorNegative, false, false);
		JsonExtensions.AddPropertyEnum(val, "ShowNullsAs", (Enum)ShowNullsAs, (Enum)StiShowEmptyCellsAs.Gap);
		JsonExtensions.AddPropertyEnum(val, "ShowZerosAs", (Enum)ShowZerosAs, (Enum)StiShowEmptyCellsAs.Gap);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ShowZeros":
				ShowZeros = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowNulls":
				ShowNulls = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Marker":
				((IStiJsonReportObject)Marker).LoadFromJsonObject((JObject)item.Value);
				break;
			case "LineMarker":
				((IStiJsonReportObject)LineMarker).LoadFromJsonObject((JObject)item.Value);
				break;
			case "LineColor":
				LineColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "LineStyle":
				LineStyle = StiJsonReportObjectHelper.DeserializeEnum<StiPenStyle>(item);
				break;
			case "Lighting":
				Lighting = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "LineWidth":
				LineWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "LabelsOffset":
				labelsOffset = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "LineColorNegative":
				LineColorNegative = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "AllowApplyColorNegative":
				AllowApplyColorNegative = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowNullsAs":
				ShowNullsAs = StiJsonReportObjectHelper.DeserializeEnum<StiShowEmptyCellsAs>(item);
				break;
			case "ShowZerosAs":
				ShowZerosAs = StiJsonReportObjectHelper.DeserializeEnum<StiShowEmptyCellsAs>(item);
				break;
			}
		}
	}

	public override object Clone()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		IStiBaseLineSeries stiBaseLineSeries = base.Clone() as IStiBaseLineSeries;
		stiBaseLineSeries.Marker = Marker.Clone() as IStiMarker;
		stiBaseLineSeries.LineStyle = LineStyle;
		return stiBaseLineSeries;
	}

	public StiBaseLineSeries()
	{
		base.SeriesLabels = new StiOutsideEndAxisLabels();
	}
}
