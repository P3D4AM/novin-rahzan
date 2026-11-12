using System.IO;
using SharpCompress.Common;

namespace SharpCompress.Readers;

public static class IReaderExtensions
{
	public static void WriteEntryTo(this IReader reader, string filePath)
	{
		using Stream writableStream = File.Open(filePath, FileMode.Create, FileAccess.Write);
		reader.WriteEntryTo(writableStream);
	}

	public static void WriteEntryTo(this IReader reader, FileInfo filePath)
	{
		using Stream writableStream = filePath.Open(FileMode.Create);
		reader.WriteEntryTo(writableStream);
	}

	public static void WriteAllToDirectory(this IReader reader, string destinationDirectory, ExtractionOptions? options = null)
	{
		while (reader.MoveToNextEntry())
		{
			reader.WriteEntryToDirectory(destinationDirectory, options);
		}
	}

	public static void WriteEntryToDirectory(this IReader reader, string destinationDirectory, ExtractionOptions? options = null)
	{
		ExtractionMethods.WriteEntryToDirectory(reader.Entry, destinationDirectory, options, reader.WriteEntryToFile);
	}

	public static void WriteEntryToFile(this IReader reader, string destinationFileName, ExtractionOptions? options = null)
	{
		string destinationFileName2 = destinationFileName;
		IReader reader2 = reader;
		ExtractionMethods.WriteEntryToFile(reader2.Entry, destinationFileName2, options, delegate(string x, FileMode fm)
		{
			using FileStream writableStream = File.Open(destinationFileName2, fm);
			reader2.WriteEntryTo(writableStream);
		});
	}
}
