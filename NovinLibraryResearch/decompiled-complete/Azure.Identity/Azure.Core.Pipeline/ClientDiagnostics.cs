using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Core.Pipeline;

internal sealed class ClientDiagnostics : Azure.Core.Pipeline.DiagnosticScopeFactory
{
	private const string DefaultMessage = "Service request failed.";

	private readonly Azure.Core.HttpMessageSanitizer _sanitizer;

	public ClientDiagnostics(ClientOptions options)
		: base(options.GetType().Namespace, GetResourceProviderNamespace(options.GetType().Assembly), options.Diagnostics.IsDistributedTracingEnabled)
	{
		_sanitizer = new Azure.Core.HttpMessageSanitizer(options.Diagnostics.LoggedQueryParameters.ToArray(), options.Diagnostics.LoggedHeaderNames.ToArray());
	}

	public async ValueTask<RequestFailedException> CreateRequestFailedExceptionAsync(Response response, string? message = null, string? errorCode = null, IDictionary<string, string>? additionalInfo = null, Exception? innerException = null)
	{
		return CreateRequestFailedExceptionWithContent(response, message, await ReadContentAsync(response, async: true).ConfigureAwait(continueOnCapturedContext: false), errorCode, additionalInfo, innerException);
	}

	public RequestFailedException CreateRequestFailedException(Response response, string? message = null, string? errorCode = null, IDictionary<string, string>? additionalInfo = null, Exception? innerException = null)
	{
		string content = ReadContentAsync(response, async: false).EnsureCompleted();
		return CreateRequestFailedExceptionWithContent(response, message, content, errorCode, additionalInfo, innerException);
	}

	public RequestFailedException CreateRequestFailedExceptionWithContent(Response response, string? message = null, string? content = null, string? errorCode = null, IDictionary<string, string>? additionalInfo = null, Exception? innerException = null)
	{
		string message2 = CreateRequestFailedMessageWithContent(response, message, content, errorCode, additionalInfo);
		RequestFailedException ex = new RequestFailedException(response.Status, message2, errorCode, innerException);
		if (additionalInfo != null)
		{
			foreach (KeyValuePair<string, string> item in additionalInfo)
			{
				ex.Data.Add(item.Key, item.Value);
			}
		}
		return ex;
	}

	public async ValueTask<string> CreateRequestFailedMessageAsync(Response response, string? message, string? errorCode, IDictionary<string, string>? additionalInfo, bool async)
	{
		return CreateRequestFailedMessageWithContent(response, message, await ReadContentAsync(response, async).ConfigureAwait(continueOnCapturedContext: false), errorCode, additionalInfo);
	}

	public string CreateRequestFailedMessageWithContent(Response response, string? message, string? content, string? errorCode, IDictionary<string, string>? additionalInfo)
	{
		StringBuilder stringBuilder = new StringBuilder().AppendLine(message ?? "Service request failed.").Append("Status: ").Append(response.Status.ToString(CultureInfo.InvariantCulture));
		if (!string.IsNullOrEmpty(response.ReasonPhrase))
		{
			stringBuilder.Append(" (").Append(response.ReasonPhrase).AppendLine(")");
		}
		else
		{
			stringBuilder.AppendLine();
		}
		if (!string.IsNullOrWhiteSpace(errorCode))
		{
			stringBuilder.Append("ErrorCode: ").Append(errorCode).AppendLine();
		}
		if (additionalInfo != null && additionalInfo.Count > 0)
		{
			stringBuilder.AppendLine().AppendLine("Additional Information:");
			foreach (KeyValuePair<string, string> item in additionalInfo)
			{
				stringBuilder.Append(item.Key).Append(": ").AppendLine(item.Value);
			}
		}
		if (content != null)
		{
			stringBuilder.AppendLine().AppendLine("Content:").AppendLine(content);
		}
		stringBuilder.AppendLine().AppendLine("Headers:");
		foreach (HttpHeader header in response.Headers)
		{
			string text = _sanitizer.SanitizeHeader(header.Name, header.Value);
			stringBuilder.AppendLine(header.Name + ": " + text);
		}
		return stringBuilder.ToString();
	}

	private static async ValueTask<string?> ReadContentAsync(Response response, bool async)
	{
		string result = null;
		if (response.ContentStream != null && Azure.Core.Pipeline.ContentTypeUtilities.TryGetTextEncoding(response.Headers.ContentType, out var encoding))
		{
			using StreamReader streamReader = new StreamReader(response.ContentStream, encoding);
			string text = ((!async) ? streamReader.ReadToEnd() : (await streamReader.ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: false)));
			result = text;
		}
		return result;
	}

	internal static string? GetResourceProviderNamespace(Assembly assembly)
	{
		object[] customAttributes = assembly.GetCustomAttributes(inherit: true);
		foreach (object obj in customAttributes)
		{
			Type type = obj.GetType();
			if (type.Name == "AzureResourceProviderNamespaceAttribute")
			{
				return type.GetProperty("ResourceProviderNamespace")?.GetValue(obj) as string;
			}
		}
		return null;
	}
}
