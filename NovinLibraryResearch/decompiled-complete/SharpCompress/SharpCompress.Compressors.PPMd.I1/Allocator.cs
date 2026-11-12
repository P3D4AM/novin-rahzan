namespace SharpCompress.Compressors.PPMd.I1;

internal class Allocator
{
	private const uint UNIT_SIZE = 12u;

	private const uint LOCAL_OFFSET = 4u;

	private const uint NODE_OFFSET = 16u;

	private const uint HEAP_OFFSET = 472u;

	private const uint N1 = 4u;

	private const uint N2 = 4u;

	private const uint N3 = 4u;

	private const uint N4 = 26u;

	private const uint INDEX_COUNT = 38u;

	private static readonly byte[] INDEX_TO_UNITS;

	private static readonly byte[] UNITS_TO_INDEX;

	public uint _allocatorSize;

	public uint _glueCount;

	public Pointer _baseUnit;

	public Pointer _lowUnit;

	public Pointer _highUnit;

	public Pointer _text;

	public Pointer _heap;

	public MemoryNode[] _memoryNodes;

	public byte[] _memory;

	static Allocator()
	{
		INDEX_TO_UNITS = new byte[38];
		uint num = 0u;
		uint num2 = 1u;
		while (num < 4)
		{
			INDEX_TO_UNITS[num] = (byte)num2;
			num++;
			num2++;
		}
		num2++;
		while (num < 8)
		{
			INDEX_TO_UNITS[num] = (byte)num2;
			num++;
			num2 += 2;
		}
		num2++;
		while (num < 12)
		{
			INDEX_TO_UNITS[num] = (byte)num2;
			num++;
			num2 += 3;
		}
		num2++;
		while (num < 38)
		{
			INDEX_TO_UNITS[num] = (byte)num2;
			num++;
			num2 += 4;
		}
		UNITS_TO_INDEX = new byte[128];
		for (num2 = (num = 0u); num2 < 128; num2++)
		{
			num += (uint)((INDEX_TO_UNITS[num] < num2 + 1) ? 1 : 0);
			UNITS_TO_INDEX[num2] = (byte)num;
		}
	}

	public Allocator()
	{
		_memoryNodes = new MemoryNode[38];
	}

	public void Initialize()
	{
		for (int i = 0; (long)i < 38L; i++)
		{
			_memoryNodes[i] = new MemoryNode((uint)(16uL + (ulong)(i * 12)), _memory);
			_memoryNodes[i].Stamp = 0u;
			_memoryNodes[i].Next = MemoryNode.ZERO;
			_memoryNodes[i].UnitCount = 0u;
		}
		_text = _heap;
		uint num = 12 * (_allocatorSize / 8 / 12 * 7);
		_highUnit = _heap + _allocatorSize;
		_lowUnit = _highUnit - num;
		_baseUnit = _highUnit - num;
		_glueCount = 0u;
	}

	public void Start(int allocatorSize)
	{
		if (_allocatorSize != (uint)allocatorSize)
		{
			Stop();
			_memory = new byte[472 + allocatorSize];
			_heap = new Pointer(472u, _memory);
			_allocatorSize = (uint)allocatorSize;
		}
	}

	public void Stop()
	{
		if (_allocatorSize != 0)
		{
			_allocatorSize = 0u;
			_memory = null;
			_heap = Pointer.ZERO;
		}
	}

	public uint GetMemoryUsed()
	{
		uint num = _allocatorSize - (_highUnit - _lowUnit) - (_baseUnit - _text);
		for (uint num2 = 0u; num2 < 38; num2++)
		{
			num -= (uint)(12 * INDEX_TO_UNITS[num2] * (int)_memoryNodes[num2].Stamp);
		}
		return num;
	}

	public Pointer AllocateUnits(uint unitCount)
	{
		uint num = UNITS_TO_INDEX[unitCount - 1];
		if (_memoryNodes[num].Available)
		{
			return _memoryNodes[num].Remove();
		}
		Pointer lowUnit = _lowUnit;
		_lowUnit += (uint)(INDEX_TO_UNITS[num] * 12);
		if (_lowUnit <= _highUnit)
		{
			return lowUnit;
		}
		_lowUnit -= (uint)(INDEX_TO_UNITS[num] * 12);
		return AllocateUnitsRare(num);
	}

	public Pointer AllocateContext()
	{
		if (_highUnit != _lowUnit)
		{
			return _highUnit -= 12u;
		}
		if (_memoryNodes[0].Available)
		{
			return _memoryNodes[0].Remove();
		}
		return AllocateUnitsRare(0u);
	}

	public Pointer ExpandUnits(Pointer oldPointer, uint oldUnitCount)
	{
		uint num = UNITS_TO_INDEX[oldUnitCount - 1];
		uint num2 = UNITS_TO_INDEX[oldUnitCount];
		if (num == num2)
		{
			return oldPointer;
		}
		Pointer pointer = AllocateUnits(oldUnitCount + 1);
		if (pointer != Pointer.ZERO)
		{
			CopyUnits(pointer, oldPointer, oldUnitCount);
			_memoryNodes[num].Insert(oldPointer, oldUnitCount);
		}
		return pointer;
	}

	public Pointer ShrinkUnits(Pointer oldPointer, uint oldUnitCount, uint newUnitCount)
	{
		uint num = UNITS_TO_INDEX[oldUnitCount - 1];
		uint num2 = UNITS_TO_INDEX[newUnitCount - 1];
		if (num == num2)
		{
			return oldPointer;
		}
		if (_memoryNodes[num2].Available)
		{
			Pointer pointer = _memoryNodes[num2].Remove();
			CopyUnits(pointer, oldPointer, newUnitCount);
			_memoryNodes[num].Insert(oldPointer, INDEX_TO_UNITS[num]);
			return pointer;
		}
		SplitBlock(oldPointer, num, num2);
		return oldPointer;
	}

	public void FreeUnits(Pointer pointer, uint unitCount)
	{
		uint num = UNITS_TO_INDEX[unitCount - 1];
		_memoryNodes[num].Insert(pointer, INDEX_TO_UNITS[num]);
	}

	public void SpecialFreeUnits(Pointer pointer)
	{
		if (pointer != _baseUnit)
		{
			_memoryNodes[0].Insert(pointer, 1u);
			return;
		}
		MemoryNode memoryNode = pointer;
		memoryNode.Stamp = uint.MaxValue;
		_baseUnit += 12u;
	}

	public Pointer MoveUnitsUp(Pointer oldPointer, uint unitCount)
	{
		uint num = UNITS_TO_INDEX[unitCount - 1];
		if (oldPointer > _baseUnit + 16384 || oldPointer > _memoryNodes[num].Next)
		{
			return oldPointer;
		}
		Pointer pointer = _memoryNodes[num].Remove();
		CopyUnits(pointer, oldPointer, unitCount);
		unitCount = INDEX_TO_UNITS[num];
		if (oldPointer != _baseUnit)
		{
			_memoryNodes[num].Insert(oldPointer, unitCount);
		}
		else
		{
			_baseUnit += unitCount * 12;
		}
		return pointer;
	}

	public void ExpandText()
	{
		uint[] array = new uint[38];
		while (true)
		{
			MemoryNode memoryNode;
			MemoryNode memoryNode2 = (memoryNode = _baseUnit);
			if (memoryNode2.Stamp != uint.MaxValue)
			{
				break;
			}
			_baseUnit = memoryNode + memoryNode.UnitCount;
			array[UNITS_TO_INDEX[memoryNode.UnitCount - 1]]++;
			memoryNode.Stamp = 0u;
		}
		for (uint num = 0u; num < 38; num++)
		{
			MemoryNode memoryNode = _memoryNodes[num];
			while (array[num] != 0)
			{
				while (memoryNode.Next.Stamp == 0)
				{
					memoryNode.Unlink();
					_memoryNodes[num].Stamp--;
					if (--array[num] == 0)
					{
						break;
					}
				}
				memoryNode = memoryNode.Next;
			}
		}
	}

	private Pointer AllocateUnitsRare(uint index)
	{
		if (_glueCount == 0)
		{
			GlueFreeBlocks();
			if (_memoryNodes[index].Available)
			{
				return _memoryNodes[index].Remove();
			}
		}
		uint num = index;
		do
		{
			if (++num == 38)
			{
				_glueCount--;
				num = (uint)(INDEX_TO_UNITS[index] * 12);
				if (_baseUnit - _text <= num)
				{
					return Pointer.ZERO;
				}
				return _baseUnit -= num;
			}
		}
		while (!_memoryNodes[num].Available);
		Pointer pointer = _memoryNodes[num].Remove();
		SplitBlock(pointer, num, index);
		return pointer;
	}

	private void SplitBlock(Pointer pointer, uint oldIndex, uint newIndex)
	{
		uint num = (uint)(INDEX_TO_UNITS[oldIndex] - INDEX_TO_UNITS[newIndex]);
		Pointer pointer2 = pointer + (uint)(INDEX_TO_UNITS[newIndex] * 12);
		uint num2 = UNITS_TO_INDEX[num - 1];
		if (INDEX_TO_UNITS[num2] != num)
		{
			uint num3 = INDEX_TO_UNITS[--num2];
			_memoryNodes[num2].Insert(pointer2, num3);
			pointer2 += num3 * 12;
			num -= num3;
		}
		_memoryNodes[UNITS_TO_INDEX[num - 1]].Insert(pointer2, num);
	}

	private void GlueFreeBlocks()
	{
		MemoryNode memoryNode = new MemoryNode(4u, _memory);
		memoryNode.Stamp = 0u;
		memoryNode.Next = MemoryNode.ZERO;
		memoryNode.UnitCount = 0u;
		if (_lowUnit != _highUnit)
		{
			_lowUnit[0] = 0;
		}
		MemoryNode memoryNode2 = memoryNode;
		for (uint num = 0u; num < 38; num++)
		{
			while (_memoryNodes[num].Available)
			{
				MemoryNode memoryNode3 = _memoryNodes[num].Remove();
				if (memoryNode3.UnitCount == 0)
				{
					continue;
				}
				while (true)
				{
					MemoryNode memoryNode4;
					MemoryNode memoryNode5 = (memoryNode4 = memoryNode3 + memoryNode3.UnitCount);
					if (memoryNode5.Stamp != uint.MaxValue)
					{
						break;
					}
					memoryNode3.UnitCount += memoryNode4.UnitCount;
					memoryNode4.UnitCount = 0u;
				}
				memoryNode2.Link(memoryNode3);
				memoryNode2 = memoryNode3;
			}
		}
		while (memoryNode.Available)
		{
			MemoryNode memoryNode3 = memoryNode.Remove();
			uint num2 = memoryNode3.UnitCount;
			if (num2 != 0)
			{
				while (num2 > 128)
				{
					_memoryNodes[37].Insert(memoryNode3, 128u);
					num2 -= 128;
					memoryNode3 += 128;
				}
				uint num3 = UNITS_TO_INDEX[num2 - 1];
				if (INDEX_TO_UNITS[num3] != num2)
				{
					uint num4 = num2 - INDEX_TO_UNITS[--num3];
					_memoryNodes[num4 - 1].Insert(memoryNode3 + (num2 - num4), num4);
				}
				_memoryNodes[num3].Insert(memoryNode3, INDEX_TO_UNITS[num3]);
			}
		}
		_glueCount = 8192u;
	}

	private void CopyUnits(Pointer target, Pointer source, uint unitCount)
	{
		do
		{
			target[0] = source[0];
			target[1] = source[1];
			target[2] = source[2];
			target[3] = source[3];
			target[4] = source[4];
			target[5] = source[5];
			target[6] = source[6];
			target[7] = source[7];
			target[8] = source[8];
			target[9] = source[9];
			target[10] = source[10];
			target[11] = source[11];
			target += 12u;
			source += 12u;
		}
		while (--unitCount != 0);
	}
}
