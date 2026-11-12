using System.Drawing;

namespace Stimulsoft.Editor;

public interface IHyperTextEx : IHyperText
{
	bool ShowHints { get; set; }

	FontStyle UrlStyle { get; set; }

	Color UrlColor { get; set; }

	event UrlJumpEvent JumpToUrl;

	void Assign(IHyperTextEx source);

	void UrlJump(string text);

	void ResetUrlStyle();

	void ResetUrlColor();

	void ResetShowHints();
}
