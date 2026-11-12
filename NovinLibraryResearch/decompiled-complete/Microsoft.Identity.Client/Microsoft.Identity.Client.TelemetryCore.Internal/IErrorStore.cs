using System.Collections.Generic;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal interface IErrorStore
{
	void ReportError(string errorMessage, ErrorType errorType, ErrorSeverity errorSeverity);

	IEnumerable<IPropertyBag> GetEventsForUpload();

	void Append(IErrorStore errorStore);

	void Clear();
}
