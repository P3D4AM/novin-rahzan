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

public abstract class StiStackedBaseLineSeries : StiSeries, IStiStackedBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiShowNullsSeries
{
	private bool showNulls = true;

	private IStiMarker marker = new StiMarker();

	private IStiLineMarker lineMarker = new StiLineMarker();

	private bool lighting = true;

	private Color lineColor = Color.MediumBlue;

	private float lineWidth = 2f;

	private StiPenStyle lineStyle;

	private Color lineColorNegative = Color.Firebrick;

	private bool allowApplyColorNegative;

	[Browsable(false)]
	[StiNonSerialized]
	public override StiTrendLinesCollection TrendLines
	{
		get
		{
			return base.TrendLines;
		}
		set
		{
			base.TrendLines = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool TrendLineAllowed => false;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[Description("Gets or sets value which indicates whether it is necessary to show the series element, if the series value of this bar is null.")]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool ShowNulls
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
	[Browsable(false)]
	[TypeConverter(typeof(StiMarkerConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public IStiMarker Marker
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
	[Browsable(false)]
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

	[StiNonSerialized]
	[Browsable(false)]
	public override StiChartConditionsCollection Conditions
	{
		get
		{
			return base.Conditions;
		}
		set
		{
			base.Conditions = value;
		}
	}

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool Lighting
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

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Color LineColor
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

	[DefaultValue(2f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float LineWidth
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

	[Editor("Stimulsoft.Base.Drawing.Design.StiPenStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a border style.")]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiPenStyle LineStyle
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
	public bool AllowApplyColorNegative
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

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "TrendLine");
		JsonExtensions.RemoveProperty(val, "Conditions");
		JsonExtensions.AddPropertyBool(val, "ShowNulls", showNulls, true, false);
		JsonExtensions.AddPropertyJObject(val, "Marker", ((IStiJsonReportObject)marker).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "LineMarker", ((IStiJsonReportObject)LineMarker).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyBool(val, "Lighting", lighting, true, false);
		JsonExtensions.AddPropertyColor(val, "LineColor", lineColor, Color.MediumBlue);
		JsonExtensions.AddPropertyFloat(val, "LineWidth", lineWidth, 2f);
		JsonExtensions.AddPropertyEnum(val, "LineStyle", (Enum)(object)lineStyle, (Enum)(object)(StiPenStyle)0);
		JsonExtensions.AddPropertyColor(val, "LineColorNegative", LineColorNegative, Color.Firebrick);
		JsonExtensions.AddPropertyBool(val, "AllowApplyColorNegative", allowApplyColorNegative, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ShowNulls":
				showNulls = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Marker":
				((IStiJsonReportObject)marker).LoadFromJsonObject((JObject)item.Value);
				break;
			case "LineMarker":
				((IStiJsonReportObject)LineMarker).LoadFromJsonObject((JObject)item.Value);
				break;
			case "Lighting":
				lighting = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "LineColor":
				lineColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "LineWidth":
				lineWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "LineStyle":
				lineStyle = StiJsonReportObjectHelper.DeserializeEnum<StiPenStyle>(item);
				break;
			case "LineColorNegative":
				LineColorNegative = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "AllowApplyColorNegative":
				allowApplyColorNegative = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override object Clone()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		IStiStackedBaseLineSeries stiStackedBaseLineSeries = base.Clone() as IStiStackedBaseLineSeries;
		stiStackedBaseLineSeries.LineStyle = LineStyle;
		return stiStackedBaseLineSeries;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiStackedColumnArea);
	}
}
