using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public interface IBookMarks : IList, ICollection, IEnumerable
{
	new IBookMark this[int index] { get; }

	void Assign(IBookMarks source);

	int GetBookMark(int line);

	int GetBookMark(Point startPoint, Point endPoint);

	int GetBookMarks(Point startPoint, Point endPoint, IList list);

	IBookMark FindBookMark(int bookMark);

	int FindBookMark(int bookMark, int line);

	IBookMark FindBookMark(string name);

	void SetBookMark(int line, int bookMark);

	void SetBookMark(Point position, int bookMark);

	void SetBookMark(IBookMark bookMark);

	void SetBookMark(Point position, int bookMark, string name, string description, string url);

	bool FindBookMark(int bookMark, out Point position);

	void ToggleBookMark(int line, int bookMark);

	void ToggleBookMark(Point position, int bookMark);

	void ToggleBookMark(IBookMark bookMark);

	void ToggleBookMark(Point position, int bookMark, string name, string description, string url);

	void ToggleBookMark(int bookMark);

	void ToggleBookMark();

	void ClearBookMark(int bookMark);

	void ClearBookMark(int line, int bookmark);

	void ClearBookMarks(int line);

	void ClearAllBookMarks();

	int NextBookMark();

	void GotoBookMark(int bookMark);

	void GotoNextBookMark();

	void GotoPrevBookMark();

	void ClearAllUnnumberedBookmarks();
}
