using System.IO;
using System.Text;

namespace Stimulsoft.Editor;

public interface IExport
{
	bool SaveFile(string fileName);

	bool SaveFile(string fileName, Encoding encoding);

	bool SaveStream(TextWriter writer);

	bool SaveStream(Stream stream);

	bool SaveStream(Stream stream, Encoding encoding);
}
