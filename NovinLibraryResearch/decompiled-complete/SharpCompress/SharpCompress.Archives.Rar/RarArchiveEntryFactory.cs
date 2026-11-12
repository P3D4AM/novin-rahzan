using System.Collections.Generic;
using SharpCompress.Common.Rar;
using SharpCompress.Readers;

namespace SharpCompress.Archives.Rar;

internal static class RarArchiveEntryFactory
{
	private static IEnumerable<RarFilePart> GetFileParts(IEnumerable<RarVolume> parts)
	{
		foreach (RarVolume part in parts)
		{
			foreach (RarFilePart item in part.ReadFileParts())
			{
				yield return item;
			}
		}
	}

	private static IEnumerable<IEnumerable<RarFilePart>> GetMatchedFileParts(IEnumerable<RarVolume> parts)
	{
		List<RarFilePart> list = new List<RarFilePart>();
		foreach (RarFilePart filePart in GetFileParts(parts))
		{
			list.Add(filePart);
			if (!filePart.FileHeader.IsSplitAfter)
			{
				yield return list;
				list = new List<RarFilePart>();
			}
		}
		if (list.Count > 0)
		{
			yield return list;
		}
	}

	internal static IEnumerable<RarArchiveEntry> GetEntries(RarArchive archive, IEnumerable<RarVolume> rarParts, ReaderOptions readerOptions)
	{
		foreach (IEnumerable<RarFilePart> matchedFilePart in GetMatchedFileParts(rarParts))
		{
			yield return new RarArchiveEntry(archive, matchedFilePart, readerOptions);
		}
	}
}
