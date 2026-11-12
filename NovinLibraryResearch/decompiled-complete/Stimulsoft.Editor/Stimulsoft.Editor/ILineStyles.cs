using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public interface ILineStyles : IList, ICollection, IEnumerable
{
	void Assign(ILineStyles source);

	void RemoveLineStyle(int line);

	void ToggleLineStyle(int line, int style);

	void ToggleLineStyle(int line, int priority, int style);

	int GetLineStyle(int index);

	int GetLineStyles(int line, IList list);

	void SetLineStyle(int index, int style);

	void SetLineStyle(int line, int priority, int style);

	void SetLineStyle(Point position, int priority, int style);

	void SetLineStyle(IRange range, int priority, int style);
}
