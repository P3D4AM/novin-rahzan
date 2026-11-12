using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public interface ILineStylesEx : IList, ICollection, IEnumerable
{
	void Assign(ILineStylesEx source);

	int IndexOfName(string name);

	int AddLineStyle();

	int AddLineStyle(string name, Color foreColor, Color backColor, Color penColor, int imageIndex, LineStyleOptions options);

	ILineStyle GetLineStyle(int index);
}
