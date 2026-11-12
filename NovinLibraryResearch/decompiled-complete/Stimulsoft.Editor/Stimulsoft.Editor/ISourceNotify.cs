namespace Stimulsoft.Editor;

public interface ISourceNotify : INotifyEx, INotify
{
	NotifyState State { get; set; }

	int BeginUpdate(UpdateReason reason);

	void LinesChanged(int first, int last);

	void LinesChanged(int first, int last, bool modified);
}
