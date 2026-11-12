using System.Drawing;

namespace Stimulsoft.Editor;

public interface ISpellingEx : ISpelling
{
	Color SpellColor { get; set; }

	void Assign(ISpellingEx source);

	void ResetSpellColor();
}
