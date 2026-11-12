using System.IO;
using System.Text;

namespace Stimulsoft.Editor;

public interface IImport
{
	bool LoadFile(string fileName);

	bool LoadFile(string fileName, Encoding encoding);

	bool LoadStream(TextReader reader);

	bool LoadStream(Stream stream);

	bool LoadStream(Stream stream, Encoding encoding);
}
