using System.IO;
using SharpCompress.Common;
using SharpCompress.IO;

namespace SharpCompress.Archives;

public static class IArchiveEntryExtensions
{
	public static void WriteTo(this IArchiveEntry archiveEntry, Stream streamToWriteTo)
	{
		if (archiveEntry.IsDirectory)
		{
			throw new ExtractionException("Entry is a file directory and cannot be extracted.");
		}
		IArchiveExtractionListener archiveExtractionListener = (IArchiveExtractionListener)archiveEntry.Archive;
		archiveExtractionListener.EnsureEntriesLoaded();
		archiveExtractionListener.FireEntryExtractionBegin(archiveEntry);
		archiveExtractionListener.FireFilePartExtractionBegin(archiveEntry.Key, archiveEntry.Size, archiveEntry.CompressedSize);
		Stream stream = archiveEntry.OpenEntryStream();
		if (stream == null)
		{
			return;
		}
		using (stream)
		{
			using Stream source = new ListeningStream(archiveExtractionListener, stream);
			source.TransferTo(streamToWriteTo);
		}
		archiveExtractionListener.FireEntryExtractionEnd(archiveEntry);
	}

	public static void WriteToDirectory(this IArchiveEntry entry, string destinationDirectory, ExtractionOptions? options = null)
	{
		ExtractionMethods.WriteEntryToDirectory(entry, destinationDirectory, options, entry.WriteToFile);
	}

	public static void WriteToFile(this IArchiveEntry entry, string destinationFileName, ExtractionOptions? options = null)
	{
		string destinationFileName2 = destinationFileName;
		IArchiveEntry entry2 = entry;
		ExtractionMethods.WriteEntryToFile(entry2, destinationFileName2, options, delegate(string x, FileMode fm)
		{
			using FileStream streamToWriteTo = File.Open(destinationFileName2, fm);
			entry2.WriteTo(streamToWriteTo);
		});
	}
}
