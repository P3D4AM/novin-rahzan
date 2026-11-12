namespace Stimulsoft.Editor;

public interface INotifyEx : INotify
{
	int FirstChanged { get; }

	int LastChanged { get; }
}
