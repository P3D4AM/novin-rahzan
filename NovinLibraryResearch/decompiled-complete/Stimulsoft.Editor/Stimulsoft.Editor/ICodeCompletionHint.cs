namespace Stimulsoft.Editor;

public interface ICodeCompletionHint : ICodeCompletionWindow, IControl, ISyntaxPaint
{
	bool AutoHide { get; set; }

	int AutoHidePause { get; set; }

	void ResetAutoHide();

	void ResetAutoHidePause();
}
