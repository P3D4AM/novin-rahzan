using System.Globalization;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlFedAuthInfo
{
	internal string spn;

	internal string stsurl;

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "STSURL: {0}, SPN: {1}", stsurl ?? string.Empty, spn ?? string.Empty);
	}
}
