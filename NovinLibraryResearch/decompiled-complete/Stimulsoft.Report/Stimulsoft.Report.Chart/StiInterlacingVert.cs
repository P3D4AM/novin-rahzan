using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiInterlacingVert : StiInterlacing, IStiInterlacingVert, IStiInterlacing, ICloneable, IStiJsonReportObject, IStiDefault
{
	public StiInterlacingVert()
	{
	}

	[StiUniversalConstructor("InterlacingVert")]
	public StiInterlacingVert(StiBrush interlacedBrush, bool visible, bool allowApplyStyle)
		: base(interlacedBrush, visible, allowApplyStyle)
	{
	}
}
