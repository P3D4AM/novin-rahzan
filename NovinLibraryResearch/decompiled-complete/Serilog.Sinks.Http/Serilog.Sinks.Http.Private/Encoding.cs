using System.Text;

namespace Serilog.Sinks.Http.Private;

public static class Encoding
{
	public static readonly System.Text.Encoding UTF8WithoutBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
}
