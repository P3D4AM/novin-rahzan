namespace Stimulsoft.Editor;

public interface IAutoCorrect
{
	char[] AutoCorrectDelimiters { get; set; }

	bool AutoCorrection { get; set; }

	event AutoCorrectEvent AutoCorrect;

	bool HasAutoCorrection(string word, out string correctWord);
}
