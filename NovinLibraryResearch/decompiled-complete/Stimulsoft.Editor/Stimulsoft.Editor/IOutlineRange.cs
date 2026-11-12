namespace Stimulsoft.Editor;

public interface IOutlineRange : IRange
{
	string Text { get; }

	string DisplayText { get; }

	bool Visible { get; set; }

	int Level { get; set; }
}
