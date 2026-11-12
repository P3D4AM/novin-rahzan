using System.Collections.Generic;

namespace Microsoft.Identity.Client;

internal delegate void TelemetryCallback(List<Dictionary<string, string>> events);
