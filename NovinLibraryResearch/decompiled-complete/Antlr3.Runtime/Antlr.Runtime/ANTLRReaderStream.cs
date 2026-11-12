using System;
using System.IO;

namespace Antlr.Runtime;

[Serializable]
public class ANTLRReaderStream : ANTLRStringStream
{
	public const int ReadBufferSize = 1024;

	public const int InitialBufferSize = 1024;

	public ANTLRReaderStream(TextReader r)
		: this(r, 1024, 1024)
	{
	}

	public ANTLRReaderStream(TextReader r, int size)
		: this(r, size, 1024)
	{
	}

	public ANTLRReaderStream(TextReader r, int size, int readChunkSize)
	{
		Load(r, size, readChunkSize);
	}

	public virtual void Load(TextReader r, int size, int readChunkSize)
	{
		if (r == null)
		{
			return;
		}
		if (size <= 0)
		{
			size = 1024;
		}
		if (readChunkSize <= 0)
		{
			readChunkSize = 1024;
		}
		try
		{
			data = r.ReadToEnd().ToCharArray();
			n = data.Length;
		}
		finally
		{
			r.Close();
		}
	}
}
