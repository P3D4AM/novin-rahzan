using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Microsoft.Identity.Client.Extensions.Msal;

internal sealed class CrossPlatLock : IDisposable
{
	internal const int LockfileRetryDelayDefault = 100;

	internal const int LockfileRetryCountDefault = 600;

	private FileStream _lockFileStream;

	public CrossPlatLock(string lockfilePath, int lockFileRetryDelay = 100, int lockFileRetryCount = 600)
	{
		Exception innerException = null;
		FileStream fileStream = null;
		Directory.CreateDirectory(Path.GetDirectoryName(lockfilePath));
		for (int i = 0; i < lockFileRetryCount; i++)
		{
			try
			{
				FileShare share = FileShare.None;
				if (SharedUtilities.IsWindowsPlatform())
				{
					share = FileShare.Read;
				}
				FileOptions options = FileOptions.DeleteOnClose;
				if (SharedUtilities.IsMonoPlatform())
				{
					options = FileOptions.None;
				}
				fileStream = new FileStream(lockfilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, share, 4096, options);
				if (SharedUtilities.IsMonoPlatform())
				{
					fileStream.Lock(0L, 0L);
				}
				using StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8, 4096, leaveOpen: true);
				streamWriter.WriteLine($"{Process.GetCurrentProcess().Id} {Process.GetCurrentProcess().ProcessName}");
			}
			catch (IOException ex)
			{
				fileStream?.Dispose();
				fileStream = null;
				innerException = ex;
				Thread.Sleep(lockFileRetryDelay);
				continue;
			}
			catch (UnauthorizedAccessException ex2)
			{
				fileStream?.Dispose();
				fileStream = null;
				innerException = ex2;
				Thread.Sleep(lockFileRetryCount);
				continue;
			}
			break;
		}
		_lockFileStream = fileStream ?? throw new InvalidOperationException("Could not get access to the shared lock file.", innerException);
	}

	public void Dispose()
	{
		_lockFileStream?.Dispose();
		_lockFileStream = null;
	}
}
