using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard;

public interface IStiTitle : IStiFont, IStiHorAlignment, IStiForeColor, IStiBackColor
{
	string Text { get; set; }

	bool Visible { get; set; }

	StiTextSizeMode SizeMode { get; set; }
}
