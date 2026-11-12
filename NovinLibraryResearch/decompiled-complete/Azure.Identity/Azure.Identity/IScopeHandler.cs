using System;
using Azure.Core.Pipeline;

namespace Azure.Identity;

internal interface IScopeHandler
{
	Azure.Core.Pipeline.DiagnosticScope CreateScope(Azure.Core.Pipeline.ClientDiagnostics diagnostics, string name);

	void Start(string name, in Azure.Core.Pipeline.DiagnosticScope scope);

	void Dispose(string name, in Azure.Core.Pipeline.DiagnosticScope scope);

	void Fail(string name, in Azure.Core.Pipeline.DiagnosticScope scope, Exception exception);
}
