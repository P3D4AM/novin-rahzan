using System.Globalization;
using System.IO;
using System.Text;

namespace Microsoft.Identity.Client.Utils;

internal class StringWriterWithEncoding : StringWriter
{
	public override Encoding Encoding { get; }

	public StringWriterWithEncoding(Encoding encoding)
		: base(CultureInfo.InvariantCulture)
	{
		Encoding = encoding;
	}
}
