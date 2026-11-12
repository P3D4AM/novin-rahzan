using System.Text;

namespace Stimulsoft.Base;

public sealed class StiBaseOptions
{
	public static bool FullTrust { get; set; } = true;


	public static bool FIPSCompliance { get; set; }

	public static Encoding WebClientEncoding { get; set; } = Encoding.UTF8;


	public static bool WebClientCheckTlsProtocols { get; set; } = true;


	public static bool JsonParseCreatingTableProperties { get; set; }

	public static bool JsonParseTypeAsString { get; set; }

	public static bool TryParseDateTime { get; set; } = true;


	public static StiJsonConverterVersion DefaultJsonConverterVersion { get; set; } = StiJsonConverterVersion.ConverterV2;


	public static string RetrieveSchemaNamePostgreSql { get; set; } = "public";


	public static bool FixBandedGradients { get; set; }

	public static bool? ForceLoadExtendedRichTextLibrary { get; set; } = true;


	public static string ExtendedRichTextLibraryClassName { get; set; } = "RICHEDIT50W";


	public static bool AllowInsertLineBreaksWhenSavingByteArray { get; set; } = true;


	internal static bool IsDashboardViewerWPF { get; set; }

	private static bool allowSetCurrentDirectory { get; set; } = true;


	public static bool AllowSetCurrentDirectory
	{
		get
		{
			if (allowSetCurrentDirectory)
			{
				return FullTrust;
			}
			return false;
		}
		set
		{
			allowSetCurrentDirectory = value;
		}
	}
}
