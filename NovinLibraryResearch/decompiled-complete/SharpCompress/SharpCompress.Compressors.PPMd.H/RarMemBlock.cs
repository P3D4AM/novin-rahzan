using System;
using System.Buffers.Binary;

namespace SharpCompress.Compressors.PPMd.H;

internal class RarMemBlock : Pointer
{
	public const int SIZE = 12;

	private int _stamp;

	private int _nu;

	private int _next;

	private int _prev;

	internal int Stamp
	{
		get
		{
			if (base.Memory != null)
			{
				_stamp = BinaryPrimitives.ReadInt16LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address)) & 0xFFFF;
			}
			return _stamp;
		}
		set
		{
			_stamp = value;
			if (base.Memory != null)
			{
				BinaryPrimitives.WriteInt16LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address), (short)value);
			}
		}
	}

	public RarMemBlock(byte[] memory)
		: base(memory)
	{
	}

	internal void InsertAt(RarMemBlock p)
	{
		RarMemBlock rarMemBlock = new RarMemBlock(base.Memory);
		SetPrev(p.Address);
		rarMemBlock.Address = GetPrev();
		SetNext(rarMemBlock.GetNext());
		rarMemBlock.SetNext(this);
		rarMemBlock.Address = GetNext();
		rarMemBlock.SetPrev(this);
	}

	internal void Remove()
	{
		RarMemBlock rarMemBlock = new RarMemBlock(base.Memory);
		rarMemBlock.Address = GetPrev();
		rarMemBlock.SetNext(GetNext());
		rarMemBlock.Address = GetNext();
		rarMemBlock.SetPrev(GetPrev());
	}

	internal int GetNext()
	{
		if (base.Memory != null)
		{
			_next = BinaryPrimitives.ReadInt32LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address + 4));
		}
		return _next;
	}

	internal void SetNext(RarMemBlock next)
	{
		SetNext(next.Address);
	}

	internal void SetNext(int next)
	{
		_next = next;
		if (base.Memory != null)
		{
			BinaryPrimitives.WriteInt32LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address + 4), next);
		}
	}

	internal int GetNu()
	{
		if (base.Memory != null)
		{
			_nu = BinaryPrimitives.ReadInt16LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address + 2)) & 0xFFFF;
		}
		return _nu;
	}

	internal void SetNu(int nu)
	{
		_nu = nu & 0xFFFF;
		if (base.Memory != null)
		{
			BinaryPrimitives.WriteInt16LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address + 2), (short)nu);
		}
	}

	internal int GetPrev()
	{
		if (base.Memory != null)
		{
			_prev = BinaryPrimitives.ReadInt32LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address + 8));
		}
		return _prev;
	}

	internal void SetPrev(RarMemBlock prev)
	{
		SetPrev(prev.Address);
	}

	internal void SetPrev(int prev)
	{
		_prev = prev;
		if (base.Memory != null)
		{
			BinaryPrimitives.WriteInt32LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address + 8), prev);
		}
	}
}
