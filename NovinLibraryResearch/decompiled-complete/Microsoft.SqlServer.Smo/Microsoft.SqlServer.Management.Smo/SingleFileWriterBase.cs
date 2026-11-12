using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

internal abstract class SingleFileWriterBase : ISmoScriptWriter, IDisposable
{
	private string currentContext;

	private bool _wroteHeader;

	public string BatchTerminator { get; set; }

	public bool ScriptBatchTerminator { get; set; }

	public int InsertBatchSize { get; set; }

	public bool Indented { get; set; }

	public string Header
	{
		set
		{
			if (!_wroteHeader && !string.IsNullOrEmpty(value))
			{
				WriteHeaderImpl(value);
				_wroteHeader = true;
			}
		}
	}

	protected SingleFileWriterBase()
	{
		BatchTerminator = Globals.Go;
		currentContext = string.Empty;
		InsertBatchSize = 100;
		ScriptBatchTerminator = true;
	}

	protected abstract void WriteHeaderImpl(string header);

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	~SingleFileWriterBase()
	{
		Dispose(disposing: false);
	}

	protected abstract void Dispose(bool disposing);

	public void ScriptContext(string databaseContext, Urn obj)
	{
		if (!databaseContext.Equals(currentContext, StringComparison.Ordinal))
		{
			if (ScriptBatchTerminator)
			{
				ScriptContextImpl(new string[2] { databaseContext, BatchTerminator }, obj);
			}
			else
			{
				ScriptContextImpl(new string[1] { databaseContext }, obj);
			}
			currentContext = databaseContext;
		}
	}

	protected abstract void ScriptContextImpl(IEnumerable<string> context, Urn obj);

	public void ScriptData(IEnumerable<string> dataScript, Urn table)
	{
		if (ScriptBatchTerminator && InsertBatchSize > 0)
		{
			foreach (IEnumerable<string> item in from b in dataScript.Batch(InsertBatchSize)
				where b.Any()
				select b)
			{
				ScriptDataImpl(item.Union(new string[1] { BatchTerminator }), table);
			}
			return;
		}
		ScriptDataImpl(dataScript, table);
	}

	protected abstract void ScriptDataImpl(IEnumerable<string> dataScript, Urn table);

	public void ScriptObject(IEnumerable<string> script, Urn obj)
	{
		if (ScriptBatchTerminator)
		{
			ScriptObjectImpl(script.WithBatches(BatchTerminator), obj);
		}
		else
		{
			ScriptObjectImpl(script, obj);
		}
	}

	protected abstract void ScriptObjectImpl(IEnumerable<string> script, Urn obj);

	protected void CheckValidFileName(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			throw new ScriptWriterException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("path"));
		}
		string fileName;
		try
		{
			if (!Directory.Exists(Path.GetDirectoryName(Path.GetFullPath(path))))
			{
				throw new ScriptWriterException(ExceptionTemplatesImpl.FolderPathNotFound);
			}
			fileName = Path.GetFileName(path);
		}
		catch (ArgumentException innerException)
		{
			throw new ScriptWriterException(ExceptionTemplatesImpl.FileWritingException, innerException);
		}
		if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
		{
			throw new ScriptWriterException(ExceptionTemplatesImpl.InvalideFileName);
		}
	}
}
