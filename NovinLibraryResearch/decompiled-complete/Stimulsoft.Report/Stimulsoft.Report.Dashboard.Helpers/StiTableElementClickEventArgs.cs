using System;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Report.Dashboard.Helpers;

public class StiTableElementClickEventArgs : EventArgs
{
	public StiDataTable DataTable { get; set; }

	public string ColumnKey { get; set; }

	public RectangleD Rect { get; set; }
}
