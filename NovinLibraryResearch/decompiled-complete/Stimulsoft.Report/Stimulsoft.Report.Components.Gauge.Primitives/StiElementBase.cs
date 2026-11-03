using System;
using System.ComponentModel;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.Gauge.Primitives;

public class StiElementBase : ICloneable, IStiApplyStyleGauge
{
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(true)]
	public virtual bool AllowApplyStyle { get; set; } = true;


	public virtual void ApplyStyle(IStiGaugeStyle style)
	{
	}

	public virtual object Clone()
	{
		return MemberwiseClone();
	}

	protected internal virtual void DrawElement(StiGaugeContextPainter context)
	{
	}
}
