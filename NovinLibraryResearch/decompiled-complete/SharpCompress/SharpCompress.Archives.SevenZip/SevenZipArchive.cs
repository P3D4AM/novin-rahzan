using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpCompress.Common;
using SharpCompress.Common.SevenZip;
using SharpCompress.Compressors.LZMA.Utilites;
using SharpCompress.IO;
using SharpCompress.Readers;

namespace SharpCompress.Archives.SevenZip;

public class SevenZipArchive : AbstractArchive<SevenZipArchiveEntry, SevenZipVolume>
{
	private sealed class SevenZipReader : AbstractReader<SevenZipEntry, SevenZipVolume>
	{
		private readonly SevenZipArchive archive;

		private CFolder currentFolder;

		private Stream currentStream;

		private CFileItem currentItem;

		public override SevenZipVolume Volume => archive.Volumes.Single();

		internal SevenZipReader(ReaderOptions readerOptions, SevenZipArchive archive)
			: base(readerOptions, ArchiveType.SevenZip)
		{
			this.archive = archive;
		}

		protected override IEnumerable<SevenZipEntry> GetEntries(Stream stream)
		{
			List<SevenZipArchiveEntry> entries = archive.Entries.ToList();
			stream.Position = 0L;
			foreach (SevenZipArchiveEntry item in entries.Where((SevenZipArchiveEntry x) => x.IsDirectory))
			{
				yield return item;
			}
			foreach (IGrouping<CFolder, SevenZipArchiveEntry> item2 in from x in entries
				where !x.IsDirectory
				group x by x.FilePart.Folder)
			{
				currentFolder = item2.Key;
				if (item2.Key == null)
				{
					currentStream = Stream.Null;
				}
				else
				{
					currentStream = archive.database.GetFolderStream(stream, currentFolder, new PasswordProvider(base.Options.Password));
				}
				foreach (SevenZipArchiveEntry item3 in item2)
				{
					currentItem = item3.FilePart.Header;
					yield return item3;
				}
			}
		}

		protected override EntryStream GetEntryStream()
		{
			return CreateEntryStream(new ReadOnlySubStream(currentStream, currentItem.Size));
		}
	}

	private class PasswordProvider : IPasswordProvider
	{
		private readonly string _password;

		public PasswordProvider(string password)
		{
			_password = password;
		}

		public string CryptoGetTextPassword()
		{
			return _password;
		}
	}

	private ArchiveDatabase database;

	private static ReadOnlySpan<byte> SIGNATURE => new byte[6] { 55, 122, 188, 175, 39, 28 };

	public override bool IsSolid => (from x in Entries
		where !x.IsDirectory
		group x by x.FilePart.Folder).Count() > 1;

	public override long TotalSize
	{
		get
		{
			_ = Entries.Count;
			return database._packSizes.Aggregate(0L, (long total, long packSize) => total + packSize);
		}
	}

	public static SevenZipArchive Open(string filePath, ReaderOptions readerOptions = null)
	{
		filePath.CheckNotNullOrEmpty("filePath");
		return Open(new FileInfo(filePath), readerOptions ?? new ReaderOptions());
	}

	public static SevenZipArchive Open(FileInfo fileInfo, ReaderOptions readerOptions = null)
	{
		fileInfo.CheckNotNull("fileInfo");
		return new SevenZipArchive(fileInfo, readerOptions ?? new ReaderOptions());
	}

	public static SevenZipArchive Open(Stream stream, ReaderOptions readerOptions = null)
	{
		stream.CheckNotNull("stream");
		return new SevenZipArchive(stream, readerOptions ?? new ReaderOptions());
	}

	internal SevenZipArchive(FileInfo fileInfo, ReaderOptions readerOptions)
		: base(ArchiveType.SevenZip, fileInfo, readerOptions)
	{
	}

	protected override IEnumerable<SevenZipVolume> LoadVolumes(FileInfo file)
	{
		return new SevenZipVolume(file.OpenRead(), base.ReaderOptions).AsEnumerable();
	}

	public static bool IsSevenZipFile(string filePath)
	{
		return IsSevenZipFile(new FileInfo(filePath));
	}

	public static bool IsSevenZipFile(FileInfo fileInfo)
	{
		if (!fileInfo.Exists)
		{
			return false;
		}
		using Stream stream = fileInfo.OpenRead();
		return IsSevenZipFile(stream);
	}

	internal SevenZipArchive(Stream stream, ReaderOptions readerOptions)
		: base(ArchiveType.SevenZip, stream.AsEnumerable(), readerOptions)
	{
	}

	internal SevenZipArchive()
		: base(ArchiveType.SevenZip)
	{
	}

	protected override IEnumerable<SevenZipVolume> LoadVolumes(IEnumerable<Stream> streams)
	{
		foreach (Stream stream in streams)
		{
			if (!stream.CanRead || !stream.CanSeek)
			{
				throw new ArgumentException("Stream is not readable and seekable");
			}
			yield return new SevenZipVolume(stream, base.ReaderOptions);
		}
	}

	protected override IEnumerable<SevenZipArchiveEntry> LoadEntries(IEnumerable<SevenZipVolume> volumes)
	{
		Stream stream = volumes.Single().Stream;
		LoadFactory(stream);
		for (int i = 0; i < database._files.Count; i++)
		{
			CFileItem fileEntry = database._files[i];
			yield return new SevenZipArchiveEntry(this, new SevenZipFilePart(stream, database, i, fileEntry, base.ReaderOptions.ArchiveEncoding));
		}
	}

	private void LoadFactory(Stream stream)
	{
		if (database == null)
		{
			stream.Position = 0L;
			ArchiveReader archiveReader = new ArchiveReader();
			archiveReader.Open(stream);
			database = archiveReader.ReadDatabase(new PasswordProvider(base.ReaderOptions.Password));
		}
	}

	public static bool IsSevenZipFile(Stream stream)
	{
		try
		{
			return SignatureMatch(stream);
		}
		catch
		{
			return false;
		}
	}

	private static bool SignatureMatch(Stream stream)
	{
		return ((ReadOnlySpan<byte>)new BinaryReader(stream).ReadBytes(6)).SequenceEqual(SIGNATURE);
	}

	protected override IReader CreateReaderForSolidExtraction()
	{
		return new SevenZipReader(base.ReaderOptions, this);
	}
}
