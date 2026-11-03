using System.Drawing;

namespace Stimulsoft.Report.Components;

public interface IStiFontAppearance
{
	Font GetFont();

	void SetFont(Font font);
}
