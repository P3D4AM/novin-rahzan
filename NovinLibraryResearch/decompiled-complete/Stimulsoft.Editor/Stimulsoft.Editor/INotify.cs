namespace Stimulsoft.Editor;

public interface INotify
{
	int UpdateCount { get; }

	int BeginUpdate();

	int EndUpdate();

	void AddNotifier(INotifier sender);

	void RemoveNotifier(INotifier sender);

	void Notify();
}
