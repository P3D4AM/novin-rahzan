using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiInterlacingHor : StiInterlacing, IStiInterlacingHor, IStiInterlacing, ICloneable, IStiJsonReportObject, IStiDefault
{
	public StiInterlacingHor()
	{
	}

	[StiUniversalConstructor("InterlacingHor")]
	public StiInterlacingHor(StiBrush interlacedBrush, bool visible, bool allowApplyStyle)
		: base(interlacedBrush, visible, allowApplyStyle)
	{
	}
}
