namespace Microsoft.SqlServer.Management.Collector;

internal class Helpers
{
	internal const int SYSNAME_MAX_LENGTH = 128;

	internal const string TraceSourceName = "DC_OM";

	internal static void CheckPropertyValueLength(string value, int maxLength, string propertyName)
	{
		if (value.Length > maxLength)
		{
			throw new CollectorException(ExceptionTemplatesSR.ValueTooLong(propertyName, maxLength));
		}
	}
}
