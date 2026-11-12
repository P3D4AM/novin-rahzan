using System.Collections.Generic;
using System.IO;

namespace SharpCompress.Common.Rar;

internal sealed class RarCryptoBinaryReader : RarCrcBinaryReader
{
	private RarRijndael _rijndael;

	private byte[] _salt;

	private readonly string _password;

	private readonly Queue<byte> _data = new Queue<byte>();

	private long _readCount;

	public override long CurrentReadByteCount
	{
		get
		{
			return _readCount;
		}
		protected set
		{
		}
	}

	private bool UseEncryption => _salt != null;

	public RarCryptoBinaryReader(Stream stream, string password)
		: base(stream)
	{
		_password = password;
		_rijndael = RarRijndael.InitializeFrom(salt: _salt = ReadBytes(8), password: _password);
	}

	public override void Mark()
	{
		_readCount = 0L;
	}

	public override byte ReadByte()
	{
		if (UseEncryption)
		{
			return ReadAndDecryptBytes(1)[0];
		}
		_readCount++;
		return base.ReadByte();
	}

	public override byte[] ReadBytes(int count)
	{
		if (UseEncryption)
		{
			return ReadAndDecryptBytes(count);
		}
		_readCount += count;
		return base.ReadBytes(count);
	}

	private byte[] ReadAndDecryptBytes(int count)
	{
		int count2 = _data.Count;
		int num = count - count2;
		if (num > 0)
		{
			int num2 = num + ((~num + 1) & 0xF);
			for (int i = 0; i < num2 / 16; i++)
			{
				byte[] array = ReadBytesNoCrc(16);
				byte[] array2 = _rijndael.ProcessBlock(array);
				foreach (byte item in array2)
				{
					_data.Enqueue(item);
				}
			}
		}
		byte[] array3 = new byte[count];
		for (int k = 0; k < count; k++)
		{
			UpdateCrc(array3[k] = _data.Dequeue());
		}
		_readCount += count;
		return array3;
	}

	public void ClearQueue()
	{
		_data.Clear();
	}

	public void SkipQueue()
	{
		long position = BaseStream.Position;
		BaseStream.Position = position + _data.Count;
		ClearQueue();
	}
}
