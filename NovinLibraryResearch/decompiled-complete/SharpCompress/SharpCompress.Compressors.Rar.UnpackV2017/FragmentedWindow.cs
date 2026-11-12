using System;

namespace SharpCompress.Compressors.Rar.UnpackV2017;

internal class FragmentedWindow
{
	private const int MAX_MEM_BLOCKS = 32;

	private readonly byte[][] Mem = new byte[32][];

	private readonly uint[] MemSize = new uint[32];

	public byte this[uint Item]
	{
		get
		{
			if (Item < MemSize[0])
			{
				return Mem[0][Item];
			}
			for (uint num = 1u; num < MemSize.Length; num++)
			{
				if (Item < MemSize[num])
				{
					return Mem[num][Item - MemSize[num - 1]];
				}
			}
			return Mem[0][0];
		}
		set
		{
			if (Item < MemSize[0])
			{
				Mem[0][Item] = value;
				return;
			}
			for (uint num = 1u; num < MemSize.Length; num++)
			{
				if (Item < MemSize[num])
				{
					Mem[num][Item - MemSize[num - 1]] = value;
					return;
				}
			}
			Mem[0][0] = value;
		}
	}

	private void Reset()
	{
		for (uint num = 0u; num < Mem.Length; num++)
		{
			if (Mem[num] != null)
			{
				Mem[num] = null;
			}
		}
	}

	public void Init(uint WinSize)
	{
		Reset();
		uint num = 0u;
		uint num2 = 0u;
		while (num2 < WinSize && num < Mem.Length)
		{
			uint num3 = WinSize - num2;
			uint num4 = Math.Max(num3 / (uint)(Mem.Length - num), 4194304u);
			byte[] array = null;
			while (num3 >= num4)
			{
				array = new byte[num3];
				if (array != null)
				{
					break;
				}
				num3 -= num3 / 32;
			}
			if (array == null)
			{
				throw new InvalidOperationException();
			}
			Mem[num] = array;
			num2 += num3;
			MemSize[num] = num2;
			num++;
		}
		if (num2 < WinSize)
		{
			throw new InvalidOperationException();
		}
	}

	public void GetBuffer(uint Item, out byte[] buf, out uint offset)
	{
		if (Item < MemSize[0])
		{
			buf = Mem[0];
			offset = Item;
			return;
		}
		for (uint num = 1u; num < MemSize.Length; num++)
		{
			if (Item < MemSize[num])
			{
				buf = Mem[num];
				offset = Item - MemSize[num - 1];
				return;
			}
		}
		buf = Mem[0];
		offset = 0u;
	}

	public void CopyString(uint Length, uint Distance, ref uint UnpPtr, uint MaxWinMask)
	{
		uint num = UnpPtr - Distance;
		while (Length-- != 0)
		{
			this[UnpPtr] = this[num++ & MaxWinMask];
			UnpPtr = (UnpPtr + 1) & MaxWinMask;
		}
	}

	public void CopyData(byte[] Dest, uint destOffset, uint WinPos, uint Size)
	{
		for (uint num = 0u; num < Size; num++)
		{
			Dest[destOffset + num] = this[WinPos + num];
		}
	}

	public uint GetBlockSize(uint StartPos, uint RequiredSize)
	{
		for (uint num = 0u; num < MemSize.Length; num++)
		{
			if (StartPos < MemSize[num])
			{
				return Math.Min(MemSize[num] - StartPos, RequiredSize);
			}
		}
		return 0u;
	}
}
