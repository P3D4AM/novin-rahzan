using System;
using System.IO;

namespace Stimulsoft.Report.Export;

public class DeflaterOutputStream : Stream
{
	protected byte[] buf;

	protected Deflater def;

	protected Stream baseOutputStream;

	protected bool isStreamOwnerValue;

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
		def.Flush();
		deflate();
		baseOutputStream.Flush();
	}

	public virtual void Finish()
	{
		def.Finish();
		while (!def.IsFinished)
		{
			int num = def.Deflate(buf, 0, buf.Length);
			if (num <= 0)
			{
				break;
			}
			baseOutputStream.Write(buf, 0, num);
		}
		if (!def.IsFinished)
		{
			throw new ApplicationException("Can't deflate all input?");
		}
		baseOutputStream.Flush();
	}

	public override void Close()
	{
		Finish();
		if (isStreamOwnerValue)
		{
			baseOutputStream.Close();
		}
	}

	public override void WriteByte(byte bval)
	{
		Write(new byte[1] { bval }, 0, 1);
	}

	public override void Write(byte[] buf, int off, int len)
	{
		def.SetInput(buf, off, len);
		deflate();
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

	protected void deflate()
	{
		while (!def.IsNeedingInput)
		{
			int num = def.Deflate(buf, 0, buf.Length);
			if (num <= 0)
			{
				break;
			}
			baseOutputStream.Write(buf, 0, num);
		}
		if (!def.IsNeedingInput)
		{
			throw new ApplicationException("Can't deflate all input?");
		}
	}

	public DeflaterOutputStream(Stream baseOutputStream)
		: this(baseOutputStream, new Deflater(), 512)
	{
	}

	public DeflaterOutputStream(Stream baseOutputStream, Deflater defl)
		: this(baseOutputStream, defl, 512)
	{
	}

	public DeflaterOutputStream(Stream baseOutputStream, Deflater defl, int bufsize)
	{
		this.baseOutputStream = baseOutputStream;
		if (bufsize <= 0)
		{
			throw new InvalidOperationException("bufsize <= 0");
		}
		buf = new byte[bufsize];
		def = defl;
		isStreamOwnerValue = true;
	}
}
