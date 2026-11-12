using System.Drawing;

namespace Stimulsoft.Editor;

public interface INavigate
{
	NavigateOptions NavigateOptions { get; set; }

	Point Position { get; set; }

	void Navigate(int deltaX, int deltaY);

	void MoveTo(int x, int y);

	void MoveTo(Point position);

	void MoveToChar(int x);

	void MoveToLine(int y);

	void ValidatePosition(ref Point position);

	int StorePosition(Point position);

	Point RestorePosition(int index);

	void ResetNavigateOptions();
}
