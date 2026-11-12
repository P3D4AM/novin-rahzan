using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Serilog.Debugging;
using Serilog.Sinks.Http.Private.Time;

namespace Serilog.Sinks.Http.Private.Durable;

public class HttpLogShipper : IDisposable
{
	private readonly IHttpClient httpClient;

	private readonly string requestUri;

	private readonly long? logEventLimitBytes;

	private readonly int? logEventsInBatchLimit;

	private readonly long? batchSizeLimitBytes;

	private readonly IBufferFiles bufferFiles;

	private readonly ExponentialBackoffConnectionSchedule connectionSchedule;

	private readonly PortableTimer timer;

	private readonly object syncRoot = new object();

	private readonly IBatchFormatter batchFormatter;

	private volatile bool disposed;

	public HttpLogShipper(IHttpClient httpClient, string requestUri, IBufferFiles bufferFiles, long? logEventLimitBytes, int? logEventsInBatchLimit, long? batchSizeLimitBytes, TimeSpan period, IBatchFormatter batchFormatter)
	{
		if (logEventsInBatchLimit <= 0)
		{
			throw new ArgumentException("logEventsInBatchLimit must be 1 or greater", "logEventsInBatchLimit");
		}
		this.httpClient = httpClient ?? throw new ArgumentNullException("httpClient");
		this.requestUri = requestUri ?? throw new ArgumentNullException("requestUri");
		this.bufferFiles = bufferFiles ?? throw new ArgumentNullException("bufferFiles");
		this.logEventLimitBytes = logEventLimitBytes;
		this.logEventsInBatchLimit = logEventsInBatchLimit;
		this.batchSizeLimitBytes = batchSizeLimitBytes;
		this.batchFormatter = batchFormatter ?? throw new ArgumentNullException("batchFormatter");
		connectionSchedule = new ExponentialBackoffConnectionSchedule(period);
		timer = new PortableTimer(OnTick);
		SetTimer();
	}

	public void Dispose()
	{
		lock (syncRoot)
		{
			if (disposed)
			{
				return;
			}
			disposed = true;
		}
		timer.Dispose();
		OnTick().GetAwaiter().GetResult();
		httpClient.Dispose();
	}

	private void SetTimer()
	{
		timer.Start(connectionSchedule.NextInterval);
	}

	private async Task OnTick()
	{
		_ = 2;
		try
		{
			Batch batch = null;
			do
			{
				string currentFile;
				using (BookmarkFile bookmark = new BookmarkFile(bufferFiles.BookmarkFileName))
				{
					bookmark.TryReadBookmark(out long nextLineBeginsAtOffset, out currentFile);
					string[] array = bufferFiles.Get();
					if (currentFile == null || !System.IO.File.Exists(currentFile))
					{
						nextLineBeginsAtOffset = 0L;
						currentFile = array.FirstOrDefault();
					}
					if (currentFile == null)
					{
						continue;
					}
					batch = BufferFileReader.Read(currentFile, ref nextLineBeginsAtOffset, logEventLimitBytes, logEventsInBatchLimit, batchSizeLimitBytes);
					HttpResponseMessage val;
					if (batch.LogEvents.Count > 0)
					{
						using MemoryStream contentStream = new MemoryStream();
						using StreamWriter contentWriter = new StreamWriter(contentStream, Encoding.UTF8WithoutBom);
						batchFormatter.Format(batch.LogEvents, contentWriter);
						await contentWriter.FlushAsync();
						contentStream.Position = 0L;
						if (contentStream.Length == 0L)
						{
							continue;
						}
						val = await httpClient.PostAsync(requestUri, contentStream).ConfigureAwait(continueOnCapturedContext: false);
						goto IL_025f;
					}
					connectionSchedule.MarkSuccess();
					if (array.Length == 2 && array.First() == currentFile && IsUnlockedAtLength(currentFile, nextLineBeginsAtOffset))
					{
						bookmark.WriteBookmark(0L, array[1]);
					}
					if (array.Length > 2)
					{
						System.IO.File.Delete(array[0]);
					}
					goto IL_03ae;
					IL_025f:
					if (val.IsSuccessStatusCode)
					{
						connectionSchedule.MarkSuccess();
						bookmark.WriteBookmark(nextLineBeginsAtOffset, currentFile);
						goto IL_03ae;
					}
					connectionSchedule.MarkFailure();
					object arg = val.StatusCode;
					SelfLog.WriteLine("Received failed HTTP shipping result {0}: {1}", arg, await val.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
					break;
				}
				IL_03ae:
				currentFile = null;
			}
			while (batch != null && batch.HasReachedLimit);
		}
		catch (Exception arg2)
		{
			SelfLog.WriteLine("Exception while emitting periodic batch from {0}: {1}", this, arg2);
			connectionSchedule.MarkFailure();
		}
		finally
		{
			lock (syncRoot)
			{
				if (!disposed)
				{
					SetTimer();
				}
			}
		}
	}

	private static bool IsUnlockedAtLength(string file, long maxLength)
	{
		try
		{
			using FileStream fileStream = System.IO.File.Open(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
			return fileStream.Length <= maxLength;
		}
		catch (IOException ex)
		{
			int num = Marshal.GetHRForException(ex) & 0xFFFF;
			if (num != 32 && num != 33)
			{
				SelfLog.WriteLine("Unexpected I/O exception while testing locked status of {0}: {1}", file, ex);
			}
		}
		catch (Exception arg)
		{
			SelfLog.WriteLine("Unexpected exception while testing locked status of {0}: {1}", file, arg);
		}
		return false;
	}
}
