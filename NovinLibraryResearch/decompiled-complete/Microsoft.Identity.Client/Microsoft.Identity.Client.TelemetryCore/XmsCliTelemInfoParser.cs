using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.TelemetryCore;

internal class XmsCliTelemInfoParser
{
	private const string ExpectedCliTelemHeaderVersion = "1";

	private const int ErrorCodeIndex = 1;

	private const int SubErrorCodeIndex = 2;

	private const int TokenAgeIndex = 3;

	private const int SpeInfoIndex = 4;

	internal XmsCliTelemInfo ParseXMsTelemHeader(string headerValue, ICoreLogger logger)
	{
		if (string.IsNullOrEmpty(headerValue))
		{
			return null;
		}
		string[] array = headerValue.Split(new char[1] { ',' });
		if (array.Length == 0)
		{
			logger.Warning(FormatLogMessage("Malformed x-ms-clitelem header: '{0}'", headerValue));
			return null;
		}
		string text = array[0];
		XmsCliTelemInfo xmsCliTelemInfo = new XmsCliTelemInfo
		{
			Version = text
		};
		if (!string.Equals(text, "1"))
		{
			logger.Warning(FormatLogMessage("Header version '{0}' unrecognized", text));
			return xmsCliTelemInfo;
		}
		if (MatchHeaderToExpectedFormat(headerValue).Count < 1)
		{
			logger.Warning(FormatLogMessage("Malformed x-ms-clitelem header: '{0}'", headerValue));
			return xmsCliTelemInfo;
		}
		xmsCliTelemInfo.ServerErrorCode = array[1];
		xmsCliTelemInfo.ServerSubErrorCode = array[2];
		xmsCliTelemInfo.TokenAge = array[3];
		xmsCliTelemInfo.SpeInfo = array[4];
		return xmsCliTelemInfo;
	}

	private MatchCollection MatchHeaderToExpectedFormat(string headerValue)
	{
		return new Regex("^[1-9]+\\.?[0-9|\\\\.]*,[0-9|\\\\.]*,[0-9|\\\\.]*,[^,]*[0-9\\\\.]*,[^,]*$").Matches(headerValue);
	}

	private string FormatLogMessage(string telemetryError, string valueToAppend)
	{
		if (valueToAppend != null)
		{
			return string.Format(CultureInfo.InvariantCulture, telemetryError, valueToAppend);
		}
		return string.Format(CultureInfo.InvariantCulture, telemetryError, "null");
	}
}
