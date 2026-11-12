namespace Stimulsoft.Editor;

public interface INavigateEx : INavigate
{
	void MoveCharLeft();

	void MoveCharRight();

	void MoveWordLeft();

	void MoveWordRight();

	void MoveLineUp();

	void MoveLineDown();

	void MoveLineBegin();

	void MoveLineEnd();

	void MoveLineBeginCycled();

	void MoveLineEndCycled();

	void MoveFileBegin();

	void MoveFileEnd();

	void MovePageUp();

	void MovePageDown();

	void MoveScreenTop();

	void MoveScreenBottom();

	void ScrollLineUp();

	void ScrollLineDown();

	void MoveToOpenBrace();

	void MoveToCloseBrace();

	void MoveToBrace();
}
