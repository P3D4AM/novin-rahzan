using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using Stimulsoft.Report.Viewer;

namespace Stimulsoft.Report.Render;

[ToolboxBitmap(typeof(StiViewerControl), "Toolbox.StiViewerControl.png")]
[Guid("4493AF49-D474-4dc0-9CFF-5E993B375108")]
[ToolboxItem(false)]
public class StiPreviewControl : StiViewerControl
{
	public StiPreviewControl()
		: this(null)
	{
	}

	public StiPreviewControl(StiReport report)
		: base(report)
	{
	}
}
