using System;
using System.IO;
using Stimulsoft.Base.Zip;

namespace Stimulsoft.Base.Excel;

public class ZipWorker : IDisposable
{
	private bool disposed;

	private bool _isCleaned;

	private const string TMP = "TMP_Z";

	private const string FOLDER_xl = "xl";

	private const string FOLDER_worksheets = "worksheets";

	private const string FILE_sharedStrings = "sharedStrings.{0}";

	private const string FILE_styles = "styles.{0}";

	private const string FILE_workbook = "workbook.{0}";

	private const string FILE_sheet = "sheet{0}.{1}";

	private const string FOLDER_rels = "_rels";

	private const string FILE_rels = "workbook.{0}.rels";

	private string _tempPath;

	private string _tempEnv;

	private string _exceptionMessage;

	private string _xlPath;

	private string _format = "xml";

	private bool _isValid;

	public bool IsValid => _isValid;

	public string TempPath => _tempPath;

	public string ExceptionMessage => _exceptionMessage;

	public ZipWorker()
	{
		_tempEnv = Path.GetTempPath();
	}

	public bool Extract(Stream fileStream)
	{
		if (fileStream == null)
		{
			return false;
		}
		CleanFromTemp(catchIoError: false);
		NewTempPath();
		_isValid = true;
		try
		{
			using StiZipReader20 stiZipReader = new StiZipReader20(fileStream);
			StiZipReader20.StiZipEntry[] entries = stiZipReader.Entries;
			foreach (StiZipReader20.StiZipEntry stiZipEntry in entries)
			{
				string path = Path.Combine(_tempPath, stiZipEntry.FullName.Replace('/', Path.DirectorySeparatorChar));
				string directoryName = Path.GetDirectoryName(path);
				if (directoryName != null && !Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				stiZipEntry.ExtractToFile(path, overwrite: true);
			}
		}
		catch (Exception ex)
		{
			_isValid = false;
			_exceptionMessage = ex.Message;
			CleanFromTemp(catchIoError: true);
		}
		finally
		{
			fileStream.Close();
		}
		if (_isValid)
		{
			return CheckFolderTree();
		}
		return false;
	}

	public Stream GetSharedStringsStream()
	{
		return GetStream(Path.Combine(_xlPath, $"sharedStrings.{_format}"));
	}

	public Stream GetStylesStream()
	{
		return GetStream(Path.Combine(_xlPath, $"styles.{_format}"));
	}

	public Stream GetWorkbookStream()
	{
		return GetStream(Path.Combine(_xlPath, $"workbook.{_format}"));
	}

	public Stream GetWorksheetStream(int sheetId)
	{
		return GetStream(Path.Combine(Path.Combine(_xlPath, "worksheets"), $"sheet{sheetId}.{_format}"));
	}

	public Stream GetWorksheetStream(string sheetPath)
	{
		return GetStream(Path.Combine(_xlPath, sheetPath));
	}

	public Stream GetWorkbookRelsStream()
	{
		return GetStream(Path.Combine(_xlPath, Path.Combine("_rels", $"workbook.{_format}.rels")));
	}

	private void CleanFromTemp(bool catchIoError)
	{
		if (string.IsNullOrEmpty(_tempPath))
		{
			return;
		}
		_isCleaned = true;
		try
		{
			if (Directory.Exists(_tempPath))
			{
				Directory.Delete(_tempPath, recursive: true);
			}
		}
		catch (IOException)
		{
			if (!catchIoError)
			{
				throw;
			}
		}
	}

	private void NewTempPath()
	{
		_tempPath = Path.Combine(_tempEnv, "TMP_Z" + Guid.NewGuid());
		_isCleaned = false;
		Directory.CreateDirectory(_tempPath);
	}

	private bool CheckFolderTree()
	{
		_xlPath = Path.Combine(_tempPath, "xl");
		if (Directory.Exists(_xlPath) && Directory.Exists(Path.Combine(_xlPath, "worksheets")) && File.Exists(Path.Combine(_xlPath, "workbook.{0}")))
		{
			return File.Exists(Path.Combine(_xlPath, "styles.{0}"));
		}
		return false;
	}

	private static Stream GetStream(string filePath)
	{
		if (File.Exists(filePath))
		{
			return File.Open(filePath, FileMode.Open, FileAccess.Read);
		}
		return null;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing && !_isCleaned)
			{
				CleanFromTemp(catchIoError: false);
			}
			disposed = true;
		}
	}

	~ZipWorker()
	{
		Dispose(disposing: false);
	}
}
