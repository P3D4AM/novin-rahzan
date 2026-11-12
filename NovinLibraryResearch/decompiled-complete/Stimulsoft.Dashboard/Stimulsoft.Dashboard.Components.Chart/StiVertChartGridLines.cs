using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Dashboard.Components.Chart;

public class StiVertChartGridLines : StiChartGridLines
{
	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of major grid lines.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(2)]
	public override bool Visible { get; set; }

	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (base.Color == Color.Transparent)
			{
				return !Visible;
			}
			return false;
		}
	}

	public StiVertChartGridLines()
	{
	}

	[StiUniversalConstructor("GridLinesVert")]
	public StiVertChartGridLines(Color color, bool visible)
	{
		base.Color = color;
		Visible = visible;
	}
}
