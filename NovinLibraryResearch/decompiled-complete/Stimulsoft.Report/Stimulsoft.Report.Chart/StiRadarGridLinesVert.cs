using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiRadarGridLinesVert : StiRadarGridLines, IStiRadarGridLinesVert, IStiRadarGridLines, ICloneable, IStiJsonReportObject
{
	public StiRadarGridLinesVert()
	{
	}

	[StiUniversalConstructor("GridLinesVert")]
	public StiRadarGridLinesVert(Color color, StiPenStyle style, bool visible, bool allowApplyStyle)
		: base(color, style, visible, allowApplyStyle)
	{
	}//IL_0002: Unknown result type (might be due to invalid IL or missing references)

}
