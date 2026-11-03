using System;
using System.Collections.Generic;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Events;

public class StiSplitTextIntoLinesEventArgs : EventArgs
{
	public StiText TextComp { get; set; }

	public RectangleD RectD { get; set; }

	public List<string> Text { get; set; }

	public List<LineInfo> LinesInfo { get; set; }

	public bool WordWrap { get; set; }
}
