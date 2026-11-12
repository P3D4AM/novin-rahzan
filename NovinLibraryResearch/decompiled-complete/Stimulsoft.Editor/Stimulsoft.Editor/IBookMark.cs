using System.Drawing;

namespace Stimulsoft.Editor;

public interface IBookMark
{
	int Line { get; }

	int Pos { get; }

	int Index { get; }

	Point Position { get; }

	void Assign(IBookMark source);
}
