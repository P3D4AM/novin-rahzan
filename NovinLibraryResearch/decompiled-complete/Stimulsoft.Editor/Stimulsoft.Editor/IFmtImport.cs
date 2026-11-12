using System.IO;
using System.Text;

namespace Stimulsoft.Editor;

public interface IFmtImport : IImport
{
	bool LoadFile(string fileName, ExportFormat format);

	bool LoadFile(string fileName, ExportFormat format, Encoding encoding);

	bool LoadStream(TextReader reader, ExportFormat format);

	bool LoadStream(Stream stream, ExportFormat format);

	bool LoadStream(Stream stream, ExportFormat format, Encoding encoding);
}
