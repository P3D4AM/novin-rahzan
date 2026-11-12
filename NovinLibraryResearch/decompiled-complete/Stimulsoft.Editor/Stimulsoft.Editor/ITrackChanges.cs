namespace Stimulsoft.Editor;

public interface ITrackChanges
{
	bool IsLineModified(int line, out bool saved);
}
