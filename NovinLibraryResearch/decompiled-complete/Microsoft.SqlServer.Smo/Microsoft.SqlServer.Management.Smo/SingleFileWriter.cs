using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

internal class SingleFileWriter : SingleFileWriterBase
{
	private StreamWriter streamWriter;

	private bool _wroteHeader;

	private void Init(string path, bool appendToFile, Encoding encoding)
	{
		CheckValidFileName(path);
		try
		{
			if (encoding != null)
			{
				streamWriter = NetCoreHelpers.CreateStreamWriter(path, appendToFile, encoding);
			}
			else
			{
				streamWriter = NetCoreHelpers.CreateStreamWriter(path, appendToFile);
			}
		}
		catch (IOException innerException)
		{
			throw new ScriptWriterException(ExceptionTemplatesImpl.FileWritingException, innerException);
		}
		catch (UnauthorizedAccessException innerException2)
		{
			throw new ScriptWriterException(ExceptionTemplatesImpl.FileWritingException, innerException2);
		}
		catch (ArgumentException innerException3)
		{
			throw new ScriptWriterException(ExceptionTemplatesImpl.FileWritingException, innerException3);
		}
		catch (SecurityException innerException4)
		{
			throw new ScriptWriterException(ExceptionTemplatesImpl.FileWritingException, innerException4);
		}
	}

	public SingleFileWriter(string path)
	{
		Init(path, appendToFile: false, null);
	}

	public SingleFileWriter(string path, bool appendToFile)
	{
		Init(path, appendToFile, null);
	}

	public SingleFileWriter(string path, bool appendToFile, Encoding encoding)
	{
		if (encoding == null)
		{
			throw new ScriptWriterException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("encoding"));
		}
		Init(path, appendToFile, encoding);
	}

	public SingleFileWriter(string path, Encoding encoding)
	{
		if (encoding == null)
		{
			throw new ScriptWriterException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("encoding"));
		}
		Init(path, appendToFile: false, encoding);
	}

	protected override void ScriptObjectImpl(IEnumerable<string> script, Urn obj)
	{
		foreach (string item in script)
		{
			streamWriter.WriteLine(item);
		}
	}

	protected override void ScriptDataImpl(IEnumerable<string> dataScript, Urn table)
	{
		foreach (string item in dataScript)
		{
			streamWriter.WriteLine(item);
		}
	}

	protected override void ScriptContextImpl(IEnumerable<string> databaseContext, Urn obj)
	{
		foreach (string item in databaseContext)
		{
			streamWriter.WriteLine(item);
		}
	}

	protected override void WriteHeaderImpl(string header)
	{
		streamWriter.WriteLine(header);
	}

	public void Close()
	{
		Dispose();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && streamWriter != null)
		{
			streamWriter.Dispose();
			streamWriter = null;
		}
	}
}
