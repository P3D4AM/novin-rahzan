using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpCompress.Common.Rar.Headers;
using SharpCompress.IO;
using SharpCompress.Readers;

namespace SharpCompress.Common.Rar;

public abstract class RarVolume : Volume
{
	private readonly RarHeaderFactory _headerFactory;

	internal ArchiveHeader ArchiveHeader { get; private set; }

	internal StreamingMode Mode => _headerFactory.StreamingMode;

	public override bool IsFirstVolume
	{
		get
		{
			EnsureArchiveHeaderLoaded();
			return ArchiveHeader.IsFirstVolume;
		}
	}

	public override bool IsMultiVolume
	{
		get
		{
			EnsureArchiveHeaderLoaded();
			return ArchiveHeader.IsVolume;
		}
	}

	public bool IsSolidArchive
	{
		get
		{
			EnsureArchiveHeaderLoaded();
			return ArchiveHeader.IsSolid;
		}
	}

	internal RarVolume(StreamingMode mode, Stream stream, ReaderOptions options)
		: base(stream, options)
	{
		_headerFactory = new RarHeaderFactory(mode, options);
	}

	internal abstract IEnumerable<RarFilePart> ReadFileParts();

	internal abstract RarFilePart CreateFilePart(MarkHeader markHeader, FileHeader fileHeader);

	internal IEnumerable<RarFilePart> GetVolumeFileParts()
	{
		MarkHeader lastMarkHeader = null;
		foreach (IRarHeader item in _headerFactory.ReadHeaders(base.Stream))
		{
			switch (item.HeaderType)
			{
			case HeaderType.Mark:
				lastMarkHeader = (MarkHeader)item;
				break;
			case HeaderType.Archive:
				ArchiveHeader = (ArchiveHeader)item;
				break;
			case HeaderType.File:
			{
				FileHeader fileHeader = (FileHeader)item;
				yield return CreateFilePart(lastMarkHeader, fileHeader);
				break;
			}
			}
		}
	}

	private void EnsureArchiveHeaderLoaded()
	{
		if (ArchiveHeader == null)
		{
			if (Mode == StreamingMode.Streaming)
			{
				throw new InvalidOperationException("ArchiveHeader should never been null in a streaming read.");
			}
			GetVolumeFileParts().First();
			base.Stream.Position = 0L;
		}
	}
}
