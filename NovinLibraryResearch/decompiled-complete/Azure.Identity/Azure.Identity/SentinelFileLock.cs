using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Identity;

internal class SentinelFileLock : IDisposable
{
	private FileStream _lockFileStream;

	private const int DefaultFileBufferSize = 4096;

	private SentinelFileLock(FileStream lockFileStream)
	{
		_lockFileStream = lockFileStream;
	}

	public static async Task<SentinelFileLock> AcquireAsync(string lockfilePath, int lockFileRetryCount, TimeSpan lockFileRetryDelay)
	{
		Exception exception = null;
		FileStream fileStream = null;
		Directory.CreateDirectory(Path.GetDirectoryName(lockfilePath));
		for (int tryCount = 0; tryCount < lockFileRetryCount; tryCount++)
		{
			try
			{
				fileStream = new FileStream(lockfilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read, 4096, FileOptions.DeleteOnClose);
				using StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8, 4096, leaveOpen: true);
				await writer.WriteLineAsync($"{Process.GetCurrentProcess().Id} {Process.GetCurrentProcess().ProcessName}").ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				exception = ex;
				await Task.Delay(lockFileRetryDelay).ConfigureAwait(continueOnCapturedContext: false);
				continue;
			}
			break;
		}
		if (fileStream == null)
		{
			throw new InvalidOperationException("Could not get access to the shared lock file.", exception);
		}
		return new SentinelFileLock(fileStream);
	}

	public void Dispose()
	{
		_lockFileStream?.Dispose();
		_lockFileStream = null;
	}
}
