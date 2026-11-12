using System;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public interface IScrolling
{
	RichTextBoxScrollBars ScrollBars { get; set; }

	int DefaultHorzScrollSize { get; set; }

	int WindowOriginX { get; set; }

	int WindowOriginY { get; set; }

	ScrollBar HScrollBar { get; }

	ScrollBar VScrollBar { get; }

	bool HasHScrollBar { get; }

	bool HasVScrollBar { get; }

	IScrollingButtons HorzButtons { get; }

	IScrollingButtons VertButtons { get; }

	ScrollingOptions Options { get; set; }

	bool VertScrollbarVisible { get; }

	bool HorzScrollbarVisible { get; }

	event EventHandler VerticalScroll;

	event EventHandler HorizontalScroll;

	event EventHandler ScrollButtonClick;

	void Assign(IScrolling source);

	void ResetScrollBars();

	void ResetDefaultHorzScrollSize();

	void ResetOptions();
}
