using System.IO;
using System.Text;

namespace Stimulsoft.Editor;

public interface IFmtExport : IExport
{
	bool SaveFile(string fileName, ExportFormat format);

	bool SaveFile(string fileName, ExportFormat format, Encoding encoding);

	bool SaveStream(TextWriter writer, ExportFormat format);

	bool SaveStream(Stream stream, ExportFormat format);

	bool SaveStream(Stream stream, ExportFormat format, Encoding encoding);
}
