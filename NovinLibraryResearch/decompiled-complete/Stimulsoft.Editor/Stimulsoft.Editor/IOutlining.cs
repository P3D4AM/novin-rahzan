using System.Drawing;

namespace Stimulsoft.Editor;

public interface IOutlining : ICollapsable
{
	Color OutlineColor { get; set; }

	void Assign(IOutlining source);

	void OutlineText();

	void UnOutlineText();

	void ResetOutlineColor();
}
