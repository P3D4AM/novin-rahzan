using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiGridLinesVert : StiGridLines, IStiGridLinesVert, IStiGridLines, ICloneable, IStiJsonReportObject, IStiDefault
{
	public StiGridLinesVert()
	{
	}

	[StiUniversalConstructor("GridLinesVert")]
	public StiGridLinesVert(Color color, StiPenStyle style, bool visible, Color minorColor, StiPenStyle minorStyle, bool minorVisible, int minorCount, bool allowApplyStyle)
		: base(color, style, visible, minorColor, minorStyle, minorVisible, minorCount, allowApplyStyle)
	{
	}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)

}
