using System.IO;

namespace Stimulsoft.Editor;

public interface IPersistentSettings
{
	void Assign(IPersistentSettings source);

	void SaveFile(string fileName);

	void SaveStream(TextWriter writer);

	void LoadFile(string fileName);

	void LoadStream(TextReader reader);
}
