using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public interface IDisplayStrings : IEnumerator, IStringList, ICollection, IEnumerable, IImport, IExport, ITabulation, IWordWrap, ITextSearch, IWordBreak, ICollapsable, IFmtExport, IFmtImport
{
	ISyntaxStrings Lines { get; set; }

	int MaxLineWidth { get; }

	Point PointToDisplayPoint(int x, int y);

	Point PointToDisplayPoint(Point position);

	Point DisplayPointToPoint(int x, int y);

	Point DisplayPointToPoint(Point position);

	int GetCount();

	short[] GetColorData(int index);

	int GetLexStyle(Point position);
}
