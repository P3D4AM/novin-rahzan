using System;
using System.IO;
using System.Threading;

namespace Microsoft.Identity.Client.Extensions.Msal;

internal static class FileIOWithRetries
{
	private const int FileLockRetryCount = 20;

	private const int FileLockRetryWaitInMs = 200;

	internal static void DeleteCacheFile(string filePath, TraceSourceLogger logger)
	{
		bool flag = File.Exists(filePath);
		logger.LogInformation($"DeleteCacheFile Cache file exists '{flag}'");
		TryProcessFile(delegate
		{
			logger.LogInformation("Before deleting the cache file");
			try
			{
				File.Delete(filePath);
			}
			catch (Exception arg)
			{
				logger.LogError($"Problem deleting the cache file '{arg}'");
			}
			logger.LogInformation("After deleting the cache file.");
		}, logger);
	}

	internal static void WriteDataToFile(string filePath, byte[] data, TraceSourceLogger logger)
	{
		EnsureParentDirectoryExists(filePath, logger);
		logger.LogInformation("Writing cache file");
		TryProcessFile(delegate
		{
			File.WriteAllBytes(filePath, data);
		}, logger);
	}

	private static void EnsureParentDirectoryExists(string filePath, TraceSourceLogger logger)
	{
		if (!Directory.Exists(Path.GetDirectoryName(filePath)))
		{
			string directoryName = Path.GetDirectoryName(filePath);
			logger.LogInformation("Creating directory '" + directoryName + "'");
			Directory.CreateDirectory(directoryName);
		}
	}

	internal static void TouchFile(string filePath, TraceSourceLogger logger)
	{
		EnsureParentDirectoryExists(filePath, logger);
		logger.LogInformation("Touching file...");
		TryProcessFile(delegate
		{
			if (!File.Exists(filePath))
			{
				logger.LogInformation("File " + filePath + " does not exist. Creating it..");
				File.Create(filePath).Dispose();
			}
			File.SetLastWriteTimeUtc(filePath, DateTime.UtcNow);
		}, logger);
	}

	internal static void TryProcessFile(Action action, TraceSourceLogger logger)
	{
		for (int i = 0; i <= 20; i++)
		{
			try
			{
				action();
				break;
			}
			catch (Exception arg)
			{
				Thread.Sleep(TimeSpan.FromMilliseconds(200.0));
				if (i == 20)
				{
					logger.LogError($"An exception was encountered while processing the cache file ex:'{arg}'");
				}
				else
				{
					logger.LogWarning($"An exception was encountered while processing the cache file. Operation will be retried. Ex:'{arg}'");
				}
			}
		}
	}
}
