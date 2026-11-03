using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Collections;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Gauge.Primitives;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge.Primitives;

public abstract class StiScaleBase : StiElementBase, IStiPropertyGridObject, IStiJsonReportObject
{
	internal class StiScaleHelper
	{
		public float ActualMinimum;

		public float ActualMaximum = 100f;

		public float MinWidth = 0.1f;

		public float MaxWidth = 0.1f;

		private float totalLength = 100f;

		public float TotalLength
		{
			get
			{
				return totalLength;
			}
			set
			{
				if (value == 0f)
				{
					totalLength = 1f;
				}
				else
				{
					totalLength = value;
				}
			}
		}

		public StiScaleHelper Clone()
		{
			return new StiScaleHelper
			{
				ActualMinimum = ActualMinimum,
				ActualMaximum = ActualMaximum,
				MinWidth = MinWidth,
				MaxWidth = MaxWidth,
				totalLength = totalLength
			};
		}
	}

	internal IStiScaleBarGeometry barGeometry;

	internal StiScaleHelper ScaleHelper = new StiScaleHelper();

	private float startWidth = 0.1f;

	private float endWidth = 0.1f;

	private float minimum;

	private float maximum = 100f;

	public abstract StiComponentId ComponentId { get; }

	[Browsable(false)]
	public string PropName => null;

	[Browsable(false)]
	internal bool IsUp
	{
		get
		{
			bool flag = startWidth < endWidth;
			if (IsReversed)
			{
				flag = !flag;
			}
			return flag;
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(null)]
	public StiGauge Gauge { get; set; }

	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets X coordinate of the scale relative to height of the scale container.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float Left { get; set; }

	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets Y coordinate of the scale relative to height of the scale container.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float Top { get; set; }

	[DefaultValue(0.1f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets start width of the scale bar.")]
	[StiOrder(140)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float StartWidth
	{
		get
		{
			return startWidth;
		}
		set
		{
			startWidth = value;
			CalculateWidthScaleHelper();
		}
	}

	[DefaultValue(0.1f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets end width of the scale bar.")]
	[StiOrder(150)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float EndWidth
	{
		get
		{
			return endWidth;
		}
		set
		{
			endWidth = value;
			CalculateWidthScaleHelper();
		}
	}

	[DefaultValue(10f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the major interval.")]
	[StiOrder(120)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float MajorInterval { get; set; } = 10f;


	[DefaultValue(5f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the minor interval.")]
	[StiOrder(130)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float MinorInterval { get; set; } = 5f;


	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets start value of the scale.")]
	[StiOrder(100)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float Minimum
	{
		get
		{
			return minimum;
		}
		set
		{
			minimum = value;
			CalculateMinMaxScaleHelper();
		}
	}

	[DefaultValue(100f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets end value of the scale.")]
	[StiOrder(110)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float Maximum
	{
		get
		{
			return maximum;
		}
		set
		{
			maximum = value;
			CalculateMinMaxScaleHelper();
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets value that indicates whether the scale should be shown in reverse mode.")]
	[StiOrder(160)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool IsReversed { get; set; }

	[StiSerializable]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a brush to fill a component.")]
	[StiOrder(130)]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.FromArgb(50, Color.White));


	[StiSerializable]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[Description("Gets or sets the border of the component.")]
	public StiBrush BorderBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.FromArgb(150, Color.White));


	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiGaugeElementCollection Items { get; set; }

	[Browsable(false)]
	public abstract StiGaugeElemenType ScaleType { get; }

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyFloat(val, "Left", Left, 0f);
		JsonExtensions.AddPropertyFloat(val, "Top", Top, 0f);
		JsonExtensions.AddPropertyFloat(val, "StartWidth", startWidth, 0.1f);
		JsonExtensions.AddPropertyFloat(val, "EndWidth", endWidth, 0.1f);
		JsonExtensions.AddPropertyFloat(val, "MajorInterval", MajorInterval, 10f);
		JsonExtensions.AddPropertyFloat(val, "MinorInterval", MinorInterval, 5f);
		JsonExtensions.AddPropertyBool(val, "IsReversed", IsReversed, false, false);
		JsonExtensions.AddPropertyFloat(val, "Minimum", minimum, 0f);
		JsonExtensions.AddPropertyFloat(val, "Maximum", maximum, 100f);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyBrush(val, "BorderBrush", BorderBrush);
		JsonExtensions.AddPropertyJObject(val, "Items", Items.SaveToJsonObject(mode));
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Left":
				Left = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Top":
				Top = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "StartWidth":
				StartWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "EndWidth":
				EndWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "MajorInterval":
				MajorInterval = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "MinorInterval":
				MinorInterval = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "IsReversed":
				IsReversed = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Minimum":
				Minimum = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Maximum":
				Maximum = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "BorderBrush":
				BorderBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Items":
				Items.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public abstract StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level);

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public override object Clone()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		StiScaleBase stiScaleBase = (StiScaleBase)base.Clone();
		stiScaleBase.ScaleHelper = ScaleHelper.Clone();
		stiScaleBase.Brush = (StiBrush)Brush.Clone();
		stiScaleBase.BorderBrush = (StiBrush)BorderBrush.Clone();
		stiScaleBase.Items = new StiGaugeElementCollection(stiScaleBase);
		lock (((ICollection)Items).SyncRoot)
		{
			foreach (StiGaugeElement item in Items)
			{
				stiScaleBase.Items.Add((StiGaugeElement)item.Clone());
			}
		}
		if (this is StiLinearScale)
		{
			stiScaleBase.barGeometry = new StiLinearBarGeometry((StiLinearScale)stiScaleBase);
		}
		else if (this is StiRadialScale)
		{
			stiScaleBase.barGeometry = new StiRadialBarGeometry((StiRadialScale)stiScaleBase);
		}
		return stiScaleBase;
	}

	internal void Prepare()
	{
		foreach (StiGaugeElement item in Items)
		{
			item.PrepareGaugeElement();
		}
	}

	internal void CalculateMinMaxScaleHelper()
	{
		ScaleHelper.ActualMaximum = Math.Max(Maximum, Minimum);
		ScaleHelper.ActualMinimum = Math.Min(Maximum, Minimum);
		ScaleHelper.TotalLength = ScaleHelper.ActualMaximum - ScaleHelper.ActualMinimum;
	}

	internal void CalculateWidthScaleHelper()
	{
		if (StartWidth > EndWidth)
		{
			ScaleHelper.MaxWidth = StartWidth;
			ScaleHelper.MinWidth = EndWidth;
		}
		else
		{
			ScaleHelper.MaxWidth = EndWidth;
			ScaleHelper.MinWidth = StartWidth;
		}
	}

	internal float GetPosition(float value)
	{
		float num = value;
		if (num < ScaleHelper.ActualMinimum)
		{
			num = ScaleHelper.ActualMinimum;
		}
		else if (num > ScaleHelper.ActualMaximum)
		{
			num = ScaleHelper.ActualMaximum;
		}
		return StiMathHelper.Length(ScaleHelper.ActualMinimum, num) / ScaleHelper.TotalLength;
	}

	protected abstract void InteractiveClick(MouseEventArgs e);

	public virtual StiScaleBase CreateNew()
	{
		throw new NotImplementedException();
	}

	protected internal override void DrawElement(StiGaugeContextPainter context)
	{
		if (Gauge == null)
		{
			return;
		}
		barGeometry.DrawScaleGeometry(context);
		foreach (StiElementBase item in Items)
		{
			item.DrawElement(context);
		}
	}

	public StiScaleBase()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		Items = new StiGaugeElementCollection(this);
	}
}
