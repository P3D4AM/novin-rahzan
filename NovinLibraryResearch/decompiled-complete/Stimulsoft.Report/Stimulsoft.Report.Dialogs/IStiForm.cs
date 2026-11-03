using System.Drawing;

namespace Stimulsoft.Report.Dialogs;

public interface IStiForm
{
	StiFormStartMode StartMode { get; set; }

	Size Size { get; set; }

	bool Enabled { get; }

	bool Visible { get; set; }

	string Text { get; set; }

	string Name { get; set; }
}
