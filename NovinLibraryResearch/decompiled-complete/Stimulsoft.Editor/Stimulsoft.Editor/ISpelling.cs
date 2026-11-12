namespace Stimulsoft.Editor;

public interface ISpelling
{
	bool CheckSpelling { get; set; }

	event WordSpellEvent WordSpell;

	bool IsWordCorrect(string text);

	void ResetCheckSpelling();
}
