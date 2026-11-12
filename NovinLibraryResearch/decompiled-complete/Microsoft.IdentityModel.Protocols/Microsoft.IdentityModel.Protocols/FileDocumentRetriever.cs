using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Protocols;

public class FileDocumentRetriever : IDocumentRetriever
{
	public async Task<string> GetDocumentAsync(string address, CancellationToken cancel)
	{
		if (string.IsNullOrWhiteSpace(address))
		{
			throw LogHelper.LogArgumentNullException("address");
		}
		try
		{
			using StreamReader reader = File.OpenText(address);
			return await reader.ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception innerException)
		{
			throw LogHelper.LogExceptionMessage(new IOException(LogHelper.FormatInvariant("IDX20804: Unable to retrieve document from: '{0}'.", address), innerException));
		}
	}
}
