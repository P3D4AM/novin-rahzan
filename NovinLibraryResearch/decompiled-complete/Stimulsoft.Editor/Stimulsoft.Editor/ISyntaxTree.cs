using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public interface ISyntaxTree
{
	ISyntaxNode Root { get; }

	ISyntaxNode Current { get; }

	void Clear();

	void Push(ISyntaxNode node);

	ISyntaxNode Pop();

	void BlockDeleting(Rectangle rect);

	void PositionChanged(int x, int y, int deltaX, int deltaY, UpdateReason reason);

	ISyntaxNode FindNode(object obj, IComparer comparer);

	void Sort(IComparer comparer);
}
