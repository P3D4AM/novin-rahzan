namespace Stimulsoft.Editor;

public interface INetNamespace
{
	string Namespace { get; set; }

	string Alias { get; set; }

	bool System { get; set; }

	string GetName();
}
