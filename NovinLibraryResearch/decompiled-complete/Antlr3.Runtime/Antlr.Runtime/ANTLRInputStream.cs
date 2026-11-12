using System;
using System.IO;
using System.Text;

namespace Antlr.Runtime;

[Serializable]
public class ANTLRInputStream : ANTLRReaderStream
{
	public ANTLRInputStream(Stream input)
		: this(input, null)
	{
	}

	public ANTLRInputStream(Stream input, int size)
		: this(input, size, null)
	{
	}

	public ANTLRInputStream(Stream input, Encoding encoding)
		: this(input, 1024, encoding)
	{
	}

	public ANTLRInputStream(Stream input, int size, Encoding encoding)
		: this(input, size, 1024, encoding)
	{
	}

	public ANTLRInputStream(Stream input, int size, int readBufferSize, Encoding encoding)
		: base(GetStreamReader(input, encoding), size, readBufferSize)
	{
	}

	private static StreamReader GetStreamReader(Stream input, Encoding encoding)
	{
		if (encoding != null)
		{
			return new StreamReader(input, encoding);
		}
		return new StreamReader(input);
	}
}
