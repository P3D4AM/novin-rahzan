using System.Drawing;

namespace Stimulsoft.Editor;

public interface IWhiteSpace
{
	bool Visible { get; set; }

	char TabSymbol { get; set; }

	char SpaceSymbol { get; set; }

	char EolSymbol { get; set; }

	char EofSymbol { get; set; }

	char WordWrapSymbol { get; set; }

	string SpaceString { get; }

	string TabString { get; }

	string EolString { get; }

	string EofString { get; }

	string WordWrapString { get; }

	Color SymbolColor { get; set; }

	void Assign(IWhiteSpace source);

	void ResetVisible();

	void ResetTabSymbol();

	void ResetSpaceSymbol();

	void ResetEolSymbol();

	void ResetEofSymbol();

	void ResetWordWrapSymbol();

	void ResetSymbolColor();
}
