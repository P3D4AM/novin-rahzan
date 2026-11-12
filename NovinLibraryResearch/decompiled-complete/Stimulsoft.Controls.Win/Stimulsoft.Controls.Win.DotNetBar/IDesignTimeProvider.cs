using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public interface IDesignTimeProvider
{
	InsertPosition GetInsertPosition(Point pScreen, BaseItem DragItem);

	void DrawReversibleMarker(int iPos, bool Before);

	void InsertItemAt(BaseItem objItem, int iPos, bool Before);
}
