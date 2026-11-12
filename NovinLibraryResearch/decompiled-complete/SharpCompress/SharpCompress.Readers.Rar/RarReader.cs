using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpCompress.Common;
using SharpCompress.Common.Rar;
using SharpCompress.Compressors.Rar;
using SharpCompress.Compressors.Rar.UnpackV1;
using SharpCompress.Compressors.Rar.UnpackV2017;

namespace SharpCompress.Readers.Rar;

public abstract class RarReader : AbstractReader<RarReaderEntry, RarVolume>
{
	private RarVolume? volume;

	internal Lazy<IRarUnpack> UnpackV2017 { get; } = new Lazy<IRarUnpack>(() => new SharpCompress.Compressors.Rar.UnpackV2017.Unpack());


	internal Lazy<IRarUnpack> UnpackV1 { get; } = new Lazy<IRarUnpack>(() => new SharpCompress.Compressors.Rar.UnpackV1.Unpack());


	public override RarVolume Volume => volume;

	internal RarReader(ReaderOptions options)
		: base(options, ArchiveType.Rar)
	{
	}

	internal abstract void ValidateArchive(RarVolume archive);

	public static RarReader Open(Stream stream, ReaderOptions? options = null)
	{
		stream.CheckNotNull("stream");
		return new SingleVolumeRarReader(stream, options ?? new ReaderOptions());
	}

	public static RarReader Open(IEnumerable<Stream> streams, ReaderOptions? options = null)
	{
		streams.CheckNotNull("streams");
		return new MultiVolumeRarReader(streams, options ?? new ReaderOptions());
	}

	protected override IEnumerable<RarReaderEntry> GetEntries(Stream stream)
	{
		volume = new RarReaderVolume(stream, base.Options);
		foreach (RarFilePart item in volume.ReadFileParts())
		{
			ValidateArchive(volume);
			yield return new RarReaderEntry(volume.IsSolidArchive, item);
		}
	}

	protected virtual IEnumerable<FilePart> CreateFilePartEnumerableForCurrentEntry()
	{
		return base.Entry.Parts;
	}

	protected override EntryStream GetEntryStream()
	{
		MultiVolumeReadOnlyStream readStream = new MultiVolumeReadOnlyStream(CreateFilePartEnumerableForCurrentEntry().Cast<RarFilePart>(), this);
		if (base.Entry.IsRarV3)
		{
			return CreateEntryStream(new RarCrcStream(UnpackV1.Value, base.Entry.FileHeader, readStream));
		}
		return CreateEntryStream(new RarCrcStream(UnpackV2017.Value, base.Entry.FileHeader, readStream));
	}
}
