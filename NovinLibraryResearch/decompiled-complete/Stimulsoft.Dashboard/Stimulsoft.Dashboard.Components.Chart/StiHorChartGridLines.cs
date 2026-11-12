using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Dashboard.Components.Chart;

public class StiHorChartGridLines : StiChartGridLines
{
	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of major grid lines.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(2)]
	public override bool Visible { get; set; } = true;


	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (base.Color == Color.Transparent)
			{
				return Visible;
			}
			return false;
		}
	}

	public StiHorChartGridLines()
	{
	}

	[StiUniversalConstructor("GridLinesHor")]
	public StiHorChartGridLines(Color color, bool visible)
	{
		base.Color = color;
		Visible = visible;
	}
}
