using System;
using System.IO;

namespace Serilog.Sinks.Http.Private.Durable;

public class BookmarkFile : IDisposable
{
	private readonly FileStream fileStream;

	public BookmarkFile(string bookmarkFileName)
	{
		if (bookmarkFileName == null)
		{
			throw new ArgumentNullException("bookmarkFileName");
		}
		fileStream = System.IO.File.Open(bookmarkFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
	}

	public void TryReadBookmark(out long nextLineBeginsAtOffset, out string? currentFile)
	{
		nextLineBeginsAtOffset = 0L;
		currentFile = null;
		if (fileStream.Length == 0L)
		{
			return;
		}
		string text = new StreamReader(fileStream, Encoding.UTF8WithoutBom, detectEncodingFromByteOrderMarks: false, 128).ReadLine();
		if (text != null)
		{
			fileStream.Position = 0L;
			string[] array = text.Split(new string[1] { ":::" }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 2)
			{
				nextLineBeginsAtOffset = long.Parse(array[0]);
				currentFile = array[1];
			}
		}
	}

	public void WriteBookmark(long nextLineBeginsAtOffset, string currentFile)
	{
		using StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8WithoutBom);
		streamWriter.WriteLine("{0}:::{1}", nextLineBeginsAtOffset, currentFile);
	}

	public void Dispose()
	{
		fileStream.Dispose();
	}
}
