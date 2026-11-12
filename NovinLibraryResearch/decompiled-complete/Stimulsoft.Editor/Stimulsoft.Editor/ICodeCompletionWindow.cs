using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public interface ICodeCompletionWindow : IControl
{
	ICodeCompletionProvider Provider { get; set; }

	bool Sizeable { get; set; }

	bool AutoSize { get; set; }

	ImageList Images { get; set; }

	CodeCompletionFlags CompletionFlags { get; set; }

	Control OwnerControl { get; set; }

	Point StartPos { get; set; }

	Point EndPos { get; set; }

	Point DisplayPos { get; set; }

	event ClosePopupEvent ClosePopup;

	event ShowPopupEvent ShowPopup;

	event EventHandler Disposed;

	event KeyPreviewEvent KeyPreviewEvent;

	void Popup();

	void PopupAt(int x, int y);

	void PopupAt(Point position);

	void Close(bool accept);

	void CloseDelayed(bool accept);

	bool IsFocused();

	void ResetContent();

	void ResetSizeable();

	void ResetAutoSize();

	void ResetCodeCompletionFlags();
}
