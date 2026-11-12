namespace Microsoft.IdentityModel.Logging;

internal static class LogMessages
{
	internal const string MIML10000 = "MIML10000: eventData.Payload is null or empty. Not logging any messages.";

	internal const string MIML10001 = "MIML10001: Cannot create the fileStream or StreamWriter to write logs. See inner exception.";

	internal const string MIML10002 = "MIML10002: Unknown log level: {0}.";
}
