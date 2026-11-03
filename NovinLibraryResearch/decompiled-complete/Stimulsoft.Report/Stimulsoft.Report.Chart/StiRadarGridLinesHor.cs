using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiRadarGridLinesHor : StiRadarGridLines, IStiRadarGridLinesHor, IStiRadarGridLines, ICloneable, IStiJsonReportObject
{
	public StiRadarGridLinesHor()
	{
	}

	[StiUniversalConstructor("GridLinesHor")]
	public StiRadarGridLinesHor(Color color, StiPenStyle style, bool visible, bool allowApplyStyle)
		: base(color, style, visible, allowApplyStyle)
	{
	}//IL_0002: Unknown result type (might be due to invalid IL or missing references)

}
