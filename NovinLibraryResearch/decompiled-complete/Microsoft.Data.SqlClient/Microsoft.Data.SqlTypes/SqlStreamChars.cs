using System;
using System.Data.SqlTypes;
using System.IO;

namespace Microsoft.Data.SqlTypes;

internal abstract class SqlStreamChars : INullable, IDisposable
{
	private class NullSqlStreamChars : SqlStreamChars
	{
		public override bool IsNull => true;

		public override bool CanRead => false;

		public override bool CanSeek => false;

		public override bool CanWrite => false;

		public override long Length
		{
			get
			{
				throw new SqlNullValueException();
			}
		}

		public override long Position
		{
			get
			{
				throw new SqlNullValueException();
			}
			set
			{
				throw new SqlNullValueException();
			}
		}

		internal NullSqlStreamChars()
		{
		}

		public override int Read(char[] buffer, int offset, int count)
		{
			throw new SqlNullValueException();
		}

		public override void Write(char[] buffer, int offset, int count)
		{
			throw new SqlNullValueException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new SqlNullValueException();
		}

		public override void SetLength(long value)
		{
			throw new SqlNullValueException();
		}

		public override void Flush()
		{
			throw new SqlNullValueException();
		}

		public override void Close()
		{
		}
	}

	public abstract bool IsNull { get; }

	public abstract bool CanRead { get; }

	public abstract bool CanSeek { get; }

	public abstract bool CanWrite { get; }

	public abstract long Length { get; }

	public abstract long Position { get; set; }

	public static SqlStreamChars Null => new NullSqlStreamChars();

	public abstract int Read(char[] buffer, int offset, int count);

	public abstract void Write(char[] buffer, int offset, int count);

	public abstract long Seek(long offset, SeekOrigin origin);

	public abstract void SetLength(long value);

	public abstract void Flush();

	public virtual void Close()
	{
		Dispose(disposing: true);
	}

	void IDisposable.Dispose()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual int ReadChar()
	{
		char[] array = new char[1];
		if (Read(array, 0, 1) == 0)
		{
			return -1;
		}
		return array[0];
	}

	public virtual void WriteChar(char value)
	{
		Write(new char[1] { value }, 0, 1);
	}
}
