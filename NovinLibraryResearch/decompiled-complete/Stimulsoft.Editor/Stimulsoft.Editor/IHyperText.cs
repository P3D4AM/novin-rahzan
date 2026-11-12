namespace Stimulsoft.Editor;

public interface IHyperText
{
	bool HighlightUrls { get; set; }

	event HyperTextEvent CheckHyperText;

	bool IsHyperText(string text);

	void ResetHighlightUrls();
}
