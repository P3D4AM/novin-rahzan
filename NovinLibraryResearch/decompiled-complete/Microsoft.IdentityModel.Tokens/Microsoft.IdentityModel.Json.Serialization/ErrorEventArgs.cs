using System;

namespace Microsoft.IdentityModel.Json.Serialization;

internal class ErrorEventArgs : EventArgs
{
	public object CurrentObject { get; }

	public ErrorContext ErrorContext { get; }

	public ErrorEventArgs(object currentObject, ErrorContext errorContext)
	{
		CurrentObject = currentObject;
		ErrorContext = errorContext;
	}
}
