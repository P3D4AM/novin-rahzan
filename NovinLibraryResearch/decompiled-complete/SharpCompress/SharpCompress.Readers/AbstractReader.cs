using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpCompress.Common;

namespace SharpCompress.Readers;

public abstract class AbstractReader<TEntry, TVolume> : IReader, IDisposable, IReaderExtractionListener, IExtractionListener where TEntry : Entry where TVolume : Volume
{
	private bool completed;

	private IEnumerator<TEntry>? entriesForCurrentReadStream;

	private bool wroteCurrentEntry;

	internal ReaderOptions Options { get; }

	public ArchiveType ArchiveType { get; }

	public abstract TVolume Volume { get; }

	public TEntry Entry => entriesForCurrentReadStream.Current;

	public bool Cancelled { get; private set; }

	IEntry IReader.Entry => Entry;

	public event EventHandler<ReaderExtractionEventArgs<IEntry>>? EntryExtractionProgress;

	public event EventHandler<CompressedBytesReadEventArgs>? CompressedBytesRead;

	public event EventHandler<FilePartExtractionBeginEventArgs>? FilePartExtractionBegin;

	internal AbstractReader(ReaderOptions options, ArchiveType archiveType)
	{
		ArchiveType = archiveType;
		Options = options;
	}

	public void Dispose()
	{
		entriesForCurrentReadStream?.Dispose();
		Volume?.Dispose();
	}

	public void Cancel()
	{
		if (!completed)
		{
			Cancelled = true;
		}
	}

	public bool MoveToNextEntry()
	{
		if (completed)
		{
			return false;
		}
		if (Cancelled)
		{
			throw new InvalidOperationException("Reader has been cancelled.");
		}
		if (entriesForCurrentReadStream == null)
		{
			return LoadStreamForReading(RequestInitialStream());
		}
		if (!wroteCurrentEntry)
		{
			SkipEntry();
		}
		wroteCurrentEntry = false;
		if (NextEntryForCurrentStream())
		{
			return true;
		}
		completed = true;
		return false;
	}

	protected bool LoadStreamForReading(Stream stream)
	{
		entriesForCurrentReadStream?.Dispose();
		if (stream == null || !stream.CanRead)
		{
			throw new MultipartStreamRequiredException("File is split into multiple archives: '" + Entry.Key + "'. A new readable stream is required.  Use Cancel if it was intended.");
		}
		entriesForCurrentReadStream = GetEntries(stream).GetEnumerator();
		return entriesForCurrentReadStream.MoveNext();
	}

	protected virtual Stream RequestInitialStream()
	{
		return Volume.Stream;
	}

	internal virtual bool NextEntryForCurrentStream()
	{
		return entriesForCurrentReadStream.MoveNext();
	}

	protected abstract IEnumerable<TEntry> GetEntries(Stream stream);

	private void SkipEntry()
	{
		if (!Entry.IsDirectory)
		{
			Skip();
		}
	}

	private void Skip()
	{
		if (ArchiveType != 0 && !Entry.IsSolid && Entry.CompressedSize > 0)
		{
			FilePart filePart = Entry.Parts.First();
			Stream rawStream = filePart.GetRawStream();
			if (rawStream != null)
			{
				long compressedSize = Entry.CompressedSize;
				rawStream.Skip(compressedSize);
				filePart.Skipped = true;
				return;
			}
		}
		using EntryStream source = OpenEntryStream();
		source.Skip();
	}

	public void WriteEntryTo(Stream writableStream)
	{
		if (wroteCurrentEntry)
		{
			throw new ArgumentException("WriteEntryTo or OpenEntryStream can only be called once.");
		}
		if (writableStream == null || !writableStream.CanWrite)
		{
			throw new ArgumentNullException("A writable Stream was required.  Use Cancel if that was intended.");
		}
		Write(writableStream);
		wroteCurrentEntry = true;
	}

	internal void Write(Stream writeStream)
	{
		using Stream source = OpenEntryStream();
		source.TransferTo(writeStream, Entry, this);
	}

	public EntryStream OpenEntryStream()
	{
		if (wroteCurrentEntry)
		{
			throw new ArgumentException("WriteEntryTo or OpenEntryStream can only be called once.");
		}
		EntryStream entryStream = GetEntryStream();
		wroteCurrentEntry = true;
		return entryStream;
	}

	protected EntryStream CreateEntryStream(Stream decompressed)
	{
		return new EntryStream(this, decompressed);
	}

	protected virtual EntryStream GetEntryStream()
	{
		return CreateEntryStream(Entry.Parts.First().GetCompressedStream());
	}

	void IExtractionListener.FireCompressedBytesRead(long currentPartCompressedBytes, long compressedReadBytes)
	{
		EventHandler<CompressedBytesReadEventArgs>? compressedBytesRead = this.CompressedBytesRead;
		if (compressedBytesRead != null)
		{
			compressedBytesRead(this, new CompressedBytesReadEventArgs(compressedReadBytes, currentPartCompressedBytes));
		}
	}

	void IExtractionListener.FireFilePartExtractionBegin(string name, long size, long compressedSize)
	{
		EventHandler<FilePartExtractionBeginEventArgs>? filePartExtractionBegin = this.FilePartExtractionBegin;
		if (filePartExtractionBegin != null)
		{
			filePartExtractionBegin(this, new FilePartExtractionBeginEventArgs(name, size, compressedSize));
		}
	}

	void IReaderExtractionListener.FireEntryExtractionProgress(Entry entry, long bytesTransferred, int iterations)
	{
		this.EntryExtractionProgress?.Invoke(this, new ReaderExtractionEventArgs<IEntry>(entry, new ReaderProgress(entry, bytesTransferred, iterations)));
	}
}
