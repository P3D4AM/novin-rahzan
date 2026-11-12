using System.Drawing;

namespace Stimulsoft.Editor;

public interface ICaret
{
	bool HideCaret { get; set; }

	void CreateCaret();

	void DestroyCaret();

	void ShowCaret(int x, int y);

	Size GetCaretSize(Point position);

	void DisplayDragCaret();

	void HideDragCaret();

	void ResetHideCaret();
}
