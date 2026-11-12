namespace Stimulsoft.Editor;

public interface IEdit
{
	bool Readonly { get; set; }

	bool OverWrite { get; set; }

	bool Modified { get; set; }

	bool SingleLineMode { get; set; }

	IndentOptions IndentOptions { get; set; }

	int MaxLength { get; set; }

	void ResetReadonly();

	void ResetOverWrite();

	void ResetModified();

	void ResetIndentOptions();

	void ResetMaxLength();

	void ResetSingleLineMode();
}
