using System.IO;

namespace Stimulsoft.Report.Export.Tools;

public class StiCachedStream : MemoryStream
{
	private Stream baseOutputStream;

	private bool isStreamOwnerValue;

	private string fileName;

	private bool tryCache;

	public static int SizeLimitToStartUseCache { get; set; } = 50000000;


	public override bool CanRead => baseOutputStream.CanRead;

	public override bool CanSeek => baseOutputStream.CanSeek;

	public override bool CanWrite => baseOutputStream.CanWrite;

	public override long Length => baseOutputStream.Length;

	public override long Position
	{
		get
		{
			return baseOutputStream.Position;
		}
		set
		{
			baseOutputStream.Position = value;
		}
	}

	public bool IsStreamOwner
	{
		get
		{
			return isStreamOwnerValue;
		}
		set
		{
			isStreamOwnerValue = value;
		}
	}

	public override void Flush()
	{
		baseOutputStream.Flush();
	}

	public virtual void Finish()
	{
		baseOutputStream.Flush();
	}

	public override void Close()
	{
		Finish();
		if (isStreamOwnerValue)
		{
			baseOutputStream.Close();
		}
		if (fileName != null && File.Exists(fileName))
		{
			File.Delete(fileName);
			fileName = null;
		}
	}

	public override void WriteByte(byte bval)
	{
		Write(new byte[1] { bval }, 0, 1);
	}

	public override void Write(byte[] buf, int off, int len)
	{
		baseOutputStream.Write(buf, off, len);
		if (!tryCache || baseOutputStream.Length <= SizeLimitToStartUseCache)
		{
			return;
		}
		try
		{
			fileName = Path.GetTempFileName();
			FileStream stream = new FileStream(fileName, FileMode.Create);
			(baseOutputStream as MemoryStream).WriteTo(stream);
			baseOutputStream.Close();
			baseOutputStream = stream;
			tryCache = false;
		}
		catch
		{
			tryCache = false;
			try
			{
				if (fileName != null && File.Exists(fileName))
				{
					File.Delete(fileName);
				}
			}
			catch
			{
			}
			finally
			{
				fileName = null;
			}
		}
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return baseOutputStream.Seek(offset, origin);
	}

	public override void SetLength(long val)
	{
		baseOutputStream.SetLength(val);
	}

	public override int ReadByte()
	{
		return baseOutputStream.ReadByte();
	}

	public override int Read(byte[] b, int off, int len)
	{
		return baseOutputStream.Read(b, off, len);
	}

	public override void WriteTo(Stream stream)
	{
		if (baseOutputStream is MemoryStream)
		{
			(baseOutputStream as MemoryStream).WriteTo(stream);
			return;
		}
		FileStream fileStream = baseOutputStream as FileStream;
		fileStream.Seek(0L, SeekOrigin.Begin);
		byte[] array = new byte[1048576];
		long num = fileStream.Length;
		while (num > 0)
		{
			int num2 = fileStream.Read(array, 0, array.Length);
			if (num2 > 0)
			{
				stream.Write(array, 0, num2);
			}
			num = num2;
		}
	}

	public StiCachedStream()
	{
		baseOutputStream = new MemoryStream();
		isStreamOwnerValue = true;
		tryCache = true;
	}
}
