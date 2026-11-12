using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Azure.Core.Pipeline;

namespace Azure.Identity;

internal sealed class ScopeGroupHandler : IScopeHandler
{
	private class ChildScopeInfo
	{
		public Azure.Core.Pipeline.ClientDiagnostics Diagnostics { get; }

		public string Name { get; }

		public DateTime StartDateTime { get; set; }

		public Exception Exception { get; set; }

		public ChildScopeInfo(Azure.Core.Pipeline.ClientDiagnostics diagnostics, string name)
		{
			Diagnostics = diagnostics;
			Name = name;
		}
	}

	private static readonly AsyncLocal<IScopeHandler> _currentAsyncLocal = new AsyncLocal<IScopeHandler>();

	private readonly string _groupName;

	private Dictionary<string, ChildScopeInfo> _childScopes;

	public static IScopeHandler Current => _currentAsyncLocal.Value;

	public ScopeGroupHandler(string groupName)
	{
		_groupName = groupName;
		_currentAsyncLocal.Value = this;
	}

	public Azure.Core.Pipeline.DiagnosticScope CreateScope(Azure.Core.Pipeline.ClientDiagnostics diagnostics, string name)
	{
		if (IsGroup(name))
		{
			return diagnostics.CreateScope(name);
		}
		if (_childScopes == null)
		{
			_childScopes = new Dictionary<string, ChildScopeInfo>();
		}
		_childScopes[name] = new ChildScopeInfo(diagnostics, name);
		return default(Azure.Core.Pipeline.DiagnosticScope);
	}

	public void Start(string name, in Azure.Core.Pipeline.DiagnosticScope scope)
	{
		if (IsGroup(name))
		{
			scope.Start();
		}
		else
		{
			_childScopes[name].StartDateTime = DateTime.UtcNow;
		}
	}

	public void Dispose(string name, in Azure.Core.Pipeline.DiagnosticScope scope)
	{
		if (IsGroup(name))
		{
			ChildScopeInfo childScopeInfo = _childScopes?.Values.LastOrDefault((ChildScopeInfo i) => i.Exception == null);
			if (childScopeInfo != null)
			{
				SucceedChildScope(childScopeInfo);
			}
			scope.Dispose();
			_currentAsyncLocal.Value = null;
		}
	}

	public void Fail(string name, in Azure.Core.Pipeline.DiagnosticScope scope, Exception exception)
	{
		if (_childScopes == null)
		{
			scope.Failed(exception);
		}
		else if (IsGroup(name))
		{
			if (exception is OperationCanceledException)
			{
				FailChildScope(_childScopes.Values.Last((ChildScopeInfo i) => i.Exception == exception));
			}
			else
			{
				foreach (ChildScopeInfo value in _childScopes.Values)
				{
					FailChildScope(value);
				}
			}
			scope.Failed(exception);
		}
		else
		{
			_childScopes[name].Exception = exception;
		}
	}

	private static void SucceedChildScope(ChildScopeInfo scopeInfo)
	{
		using Azure.Core.Pipeline.DiagnosticScope diagnosticScope = scopeInfo.Diagnostics.CreateScope(scopeInfo.Name);
		diagnosticScope.SetStartTime(scopeInfo.StartDateTime);
		diagnosticScope.Start();
	}

	private static void FailChildScope(ChildScopeInfo scopeInfo)
	{
		using Azure.Core.Pipeline.DiagnosticScope diagnosticScope = scopeInfo.Diagnostics.CreateScope(scopeInfo.Name);
		diagnosticScope.SetStartTime(scopeInfo.StartDateTime);
		diagnosticScope.Start();
		diagnosticScope.Failed(scopeInfo.Exception);
	}

	private bool IsGroup(string name)
	{
		return string.Equals(name, _groupName, StringComparison.Ordinal);
	}

	void IScopeHandler.Start(string name, in Azure.Core.Pipeline.DiagnosticScope scope)
	{
		Start(name, in scope);
	}

	void IScopeHandler.Dispose(string name, in Azure.Core.Pipeline.DiagnosticScope scope)
	{
		Dispose(name, in scope);
	}

	void IScopeHandler.Fail(string name, in Azure.Core.Pipeline.DiagnosticScope scope, Exception exception)
	{
		Fail(name, in scope, exception);
	}
}
