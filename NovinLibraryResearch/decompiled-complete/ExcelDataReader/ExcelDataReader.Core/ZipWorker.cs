using System;
using System.IO;
using System.IO.Compression;

namespace ExcelDataReader.Core;

internal class ZipWorker : IDisposable
{
	private const string FileSharedStrings = "xl/sharedStrings.{0}";

	private const string FileStyles = "xl/styles.{0}";

	private const string FileWorkbook = "xl/workbook.{0}";

	private const string FileSheet = "xl/worksheets/sheet{0}.{1}";

	private const string FileRels = "xl/_rels/workbook.{0}.rels";

	private const string Format = "xml";

	private bool _disposed;

	private Stream _zipStream;

	private ZipArchive _zipFile;

	public ZipWorker(Stream fileStream)
	{
		_zipStream = fileStream ?? throw new ArgumentNullException("fileStream");
		_zipFile = new ZipArchive(fileStream);
	}

	public Stream GetSharedStringsStream()
	{
		return FindEntry(string.Format("xl/sharedStrings.{0}", "xml"))?.Open();
	}

	public Stream GetStylesStream()
	{
		return FindEntry(string.Format("xl/styles.{0}", "xml"))?.Open();
	}

	public Stream GetWorkbookStream()
	{
		return FindEntry(string.Format("xl/workbook.{0}", "xml"))?.Open();
	}

	public Stream GetWorksheetStream(int sheetId)
	{
		return FindEntry(string.Format("xl/worksheets/sheet{0}.{1}", sheetId, "xml"))?.Open();
	}

	public Stream GetWorksheetStream(string sheetPath)
	{
		sheetPath = ((!sheetPath.StartsWith("/xl/", StringComparison.OrdinalIgnoreCase)) ? ("xl/" + sheetPath) : sheetPath.Substring(1));
		return FindEntry(sheetPath)?.Open();
	}

	public Stream GetWorkbookRelsStream()
	{
		return FindEntry(string.Format("xl/_rels/workbook.{0}.rels", "xml"))?.Open();
	}

	private ZipArchiveEntry FindEntry(string name)
	{
		foreach (ZipArchiveEntry entry in _zipFile.Entries)
		{
			if (entry.FullName.Replace('\\', '/').Equals(name, StringComparison.OrdinalIgnoreCase))
			{
				return entry;
			}
		}
		return null;
	}

	~ZipWorker()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (_disposed)
		{
			return;
		}
		if (disposing)
		{
			if (_zipFile != null)
			{
				_zipFile.Dispose();
				_zipFile = null;
			}
			if (_zipStream != null)
			{
				_zipStream.Dispose();
				_zipStream = null;
			}
		}
		_disposed = true;
	}
}
