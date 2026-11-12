using System.Drawing;

namespace Stimulsoft.Editor;

public interface ILineStyle
{
	string Name { get; set; }

	Color ForeColor { get; set; }

	Color BackColor { get; set; }

	Color PenColor { get; set; }

	int ImageIndex { get; set; }

	LineStyleOptions Options { get; set; }

	void Assign(ILineStyle source);

	void ResetForeColor();

	void ResetBackColor();

	void ResetPenColor();

	void ResetImageIndex();

	void ResetOptions();
}
