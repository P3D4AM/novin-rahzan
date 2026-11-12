using System.Diagnostics;

namespace Azure.Core.Pipeline;

internal class DiagnosticScopeFactory
{
	private readonly string? _resourceProviderNamespace;

	private readonly DiagnosticListener? _source;

	public bool IsActivityEnabled { get; }

	public DiagnosticScopeFactory(string clientNamespace, string? resourceProviderNamespace, bool isActivityEnabled)
	{
		_resourceProviderNamespace = resourceProviderNamespace;
		IsActivityEnabled = isActivityEnabled;
		if (IsActivityEnabled)
		{
			_source = new DiagnosticListener(clientNamespace);
		}
	}

	public DiagnosticScope CreateScope(string name)
	{
		if (_source == null)
		{
			return default(DiagnosticScope);
		}
		DiagnosticScope result = new DiagnosticScope(name, _source);
		if (_resourceProviderNamespace != null)
		{
			result.AddAttribute("az.namespace", _resourceProviderNamespace);
		}
		return result;
	}
}
