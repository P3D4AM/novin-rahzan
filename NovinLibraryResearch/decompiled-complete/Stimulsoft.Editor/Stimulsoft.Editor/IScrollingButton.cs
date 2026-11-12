using System.Windows.Forms;

namespace Stimulsoft.Editor;

public interface IScrollingButton
{
	string Name { get; set; }

	string Description { get; set; }

	int ImageIndex { get; set; }

	ImageList Images { get; set; }

	bool Visible { get; set; }

	EditBorderStyle BorderStyle { get; set; }

	void Assign(IScrollingButton source);
}
