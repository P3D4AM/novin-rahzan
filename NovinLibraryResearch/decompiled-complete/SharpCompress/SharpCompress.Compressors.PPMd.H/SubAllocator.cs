using System;
using System.Text;

namespace SharpCompress.Compressors.PPMd.H;

internal class SubAllocator
{
	public const int N1 = 4;

	public const int N2 = 4;

	public const int N3 = 4;

	public static readonly int N4;

	public static readonly int N_INDEXES;

	public static readonly int UNIT_SIZE;

	public const int FIXED_UNIT_SIZE = 12;

	private int _subAllocatorSize;

	private readonly int[] _indx2Units = new int[N_INDEXES];

	private readonly int[] _units2Indx = new int[128];

	private int _glueCount;

	private int _heapStart;

	private int _loUnit;

	private int _hiUnit;

	private readonly RarNode[] _freeList = new RarNode[N_INDEXES];

	private int _pText;

	private int _unitsStart;

	private int _heapEnd;

	private int _fakeUnitsStart;

	private byte[] _heap;

	private int _freeListPos;

	private int _tempMemBlockPos;

	private RarNode _tempRarNode;

	private RarMemBlock _tempRarMemBlock1;

	private RarMemBlock _tempRarMemBlock2;

	private RarMemBlock _tempRarMemBlock3;

	public virtual int FakeUnitsStart
	{
		get
		{
			return _fakeUnitsStart;
		}
		set
		{
			_fakeUnitsStart = value;
		}
	}

	public virtual int HeapEnd => _heapEnd;

	public virtual int PText
	{
		get
		{
			return _pText;
		}
		set
		{
			_pText = value;
		}
	}

	public virtual int UnitsStart
	{
		get
		{
			return _unitsStart;
		}
		set
		{
			_unitsStart = value;
		}
	}

	public virtual byte[] Heap => _heap;

	public SubAllocator()
	{
		Clean();
	}

	public virtual void Clean()
	{
		_subAllocatorSize = 0;
	}

	private void InsertNode(int p, int indx)
	{
		RarNode tempRarNode = _tempRarNode;
		tempRarNode.Address = p;
		tempRarNode.SetNext(_freeList[indx].GetNext());
		_freeList[indx].SetNext(tempRarNode);
	}

	public virtual void IncPText()
	{
		_pText++;
	}

	private int RemoveNode(int indx)
	{
		int next = _freeList[indx].GetNext();
		RarNode tempRarNode = _tempRarNode;
		tempRarNode.Address = next;
		_freeList[indx].SetNext(tempRarNode.GetNext());
		return next;
	}

	private int U2B(int nu)
	{
		return UNIT_SIZE * nu;
	}

	private int MbPtr(int basePtr, int items)
	{
		return basePtr + U2B(items);
	}

	private void SplitBlock(int pv, int oldIndx, int newIndx)
	{
		int num = _indx2Units[oldIndx] - _indx2Units[newIndx];
		int num2 = pv + U2B(_indx2Units[newIndx]);
		int num3;
		if (_indx2Units[num3 = _units2Indx[num - 1]] != num)
		{
			InsertNode(num2, --num3);
			num2 += U2B(num3 = _indx2Units[num3]);
			num -= num3;
		}
		InsertNode(num2, _units2Indx[num - 1]);
	}

	public virtual void StopSubAllocator()
	{
		if (_subAllocatorSize != 0)
		{
			_subAllocatorSize = 0;
			_heap = null;
			_heapStart = 1;
			_tempRarNode = null;
			_tempRarMemBlock1 = null;
			_tempRarMemBlock2 = null;
			_tempRarMemBlock3 = null;
		}
	}

	public virtual int GetAllocatedMemory()
	{
		return _subAllocatorSize;
	}

	public virtual bool StartSubAllocator(int saSize)
	{
		if (_subAllocatorSize == saSize)
		{
			return true;
		}
		StopSubAllocator();
		int num = saSize / 12 * UNIT_SIZE + UNIT_SIZE;
		int num2 = (_tempMemBlockPos = 1 + num + 4 * N_INDEXES) + 12;
		_heap = new byte[num2];
		_heapStart = 1;
		_heapEnd = _heapStart + num - UNIT_SIZE;
		_subAllocatorSize = saSize;
		_freeListPos = _heapStart + num;
		int num3 = 0;
		int num4 = _freeListPos;
		while (num3 < _freeList.Length)
		{
			_freeList[num3] = new RarNode(_heap);
			_freeList[num3].Address = num4;
			num3++;
			num4 += 4;
		}
		_tempRarNode = new RarNode(_heap);
		_tempRarMemBlock1 = new RarMemBlock(_heap);
		_tempRarMemBlock2 = new RarMemBlock(_heap);
		_tempRarMemBlock3 = new RarMemBlock(_heap);
		return true;
	}

	private void GlueFreeBlocks()
	{
		RarMemBlock tempRarMemBlock = _tempRarMemBlock1;
		tempRarMemBlock.Address = _tempMemBlockPos;
		RarMemBlock tempRarMemBlock2 = _tempRarMemBlock2;
		RarMemBlock tempRarMemBlock3 = _tempRarMemBlock3;
		if (_loUnit != _hiUnit)
		{
			_heap[_loUnit] = 0;
		}
		int i = 0;
		tempRarMemBlock.SetPrev(tempRarMemBlock);
		tempRarMemBlock.SetNext(tempRarMemBlock);
		for (; i < N_INDEXES; i++)
		{
			while (_freeList[i].GetNext() != 0)
			{
				tempRarMemBlock2.Address = RemoveNode(i);
				tempRarMemBlock2.InsertAt(tempRarMemBlock);
				tempRarMemBlock2.Stamp = 65535;
				tempRarMemBlock2.SetNu(_indx2Units[i]);
			}
		}
		tempRarMemBlock2.Address = tempRarMemBlock.GetNext();
		while (tempRarMemBlock2.Address != tempRarMemBlock.Address)
		{
			tempRarMemBlock3.Address = MbPtr(tempRarMemBlock2.Address, tempRarMemBlock2.GetNu());
			while (tempRarMemBlock3.Stamp == 65535 && tempRarMemBlock2.GetNu() + tempRarMemBlock3.GetNu() < 65536)
			{
				tempRarMemBlock3.Remove();
				tempRarMemBlock2.SetNu(tempRarMemBlock2.GetNu() + tempRarMemBlock3.GetNu());
				tempRarMemBlock3.Address = MbPtr(tempRarMemBlock2.Address, tempRarMemBlock2.GetNu());
			}
			tempRarMemBlock2.Address = tempRarMemBlock2.GetNext();
		}
		tempRarMemBlock2.Address = tempRarMemBlock.GetNext();
		while (tempRarMemBlock2.Address != tempRarMemBlock.Address)
		{
			tempRarMemBlock2.Remove();
			int num = tempRarMemBlock2.GetNu();
			while (num > 128)
			{
				InsertNode(tempRarMemBlock2.Address, N_INDEXES - 1);
				num -= 128;
				tempRarMemBlock2.Address = MbPtr(tempRarMemBlock2.Address, 128);
			}
			if (_indx2Units[i = _units2Indx[num - 1]] != num)
			{
				int num2 = num - _indx2Units[--i];
				InsertNode(MbPtr(tempRarMemBlock2.Address, num - num2), num2 - 1);
			}
			InsertNode(tempRarMemBlock2.Address, i);
			tempRarMemBlock2.Address = tempRarMemBlock.GetNext();
		}
	}

	private int AllocUnitsRare(int indx)
	{
		if (_glueCount == 0)
		{
			_glueCount = 255;
			GlueFreeBlocks();
			if (_freeList[indx].GetNext() != 0)
			{
				return RemoveNode(indx);
			}
		}
		int num = indx;
		do
		{
			if (++num == N_INDEXES)
			{
				_glueCount--;
				num = U2B(_indx2Units[indx]);
				int num2 = 12 * _indx2Units[indx];
				if (_fakeUnitsStart - _pText > num2)
				{
					_fakeUnitsStart -= num2;
					_unitsStart -= num;
					return _unitsStart;
				}
				return 0;
			}
		}
		while (_freeList[num].GetNext() == 0);
		int num3 = RemoveNode(num);
		SplitBlock(num3, num, indx);
		return num3;
	}

	public virtual int AllocUnits(int nu)
	{
		int num = _units2Indx[nu - 1];
		if (_freeList[num].GetNext() != 0)
		{
			return RemoveNode(num);
		}
		int loUnit = _loUnit;
		_loUnit += U2B(_indx2Units[num]);
		if (_loUnit <= _hiUnit)
		{
			return loUnit;
		}
		_loUnit -= U2B(_indx2Units[num]);
		return AllocUnitsRare(num);
	}

	public virtual int AllocContext()
	{
		if (_hiUnit != _loUnit)
		{
			return _hiUnit -= UNIT_SIZE;
		}
		if (_freeList[0].GetNext() != 0)
		{
			return RemoveNode(0);
		}
		return AllocUnitsRare(0);
	}

	public virtual int ExpandUnits(int oldPtr, int oldNu)
	{
		int num = _units2Indx[oldNu - 1];
		int num2 = _units2Indx[oldNu - 1 + 1];
		if (num == num2)
		{
			return oldPtr;
		}
		int num3 = AllocUnits(oldNu + 1);
		if (num3 != 0)
		{
			Array.Copy(_heap, oldPtr, _heap, num3, U2B(oldNu));
			InsertNode(oldPtr, num);
		}
		return num3;
	}

	public virtual int ShrinkUnits(int oldPtr, int oldNu, int newNu)
	{
		int num = _units2Indx[oldNu - 1];
		int num2 = _units2Indx[newNu - 1];
		if (num == num2)
		{
			return oldPtr;
		}
		if (_freeList[num2].GetNext() != 0)
		{
			int num3 = RemoveNode(num2);
			Array.Copy(_heap, oldPtr, _heap, num3, U2B(newNu));
			InsertNode(oldPtr, num);
			return num3;
		}
		SplitBlock(oldPtr, num, num2);
		return oldPtr;
	}

	public virtual void FreeUnits(int ptr, int oldNu)
	{
		InsertNode(ptr, _units2Indx[oldNu - 1]);
	}

	public virtual void DecPText(int dPText)
	{
		PText -= dPText;
	}

	public virtual void InitSubAllocator()
	{
		new Span<byte>(_heap, _freeListPos, SizeOfFreeList()).Clear();
		_pText = _heapStart;
		int num = 12 * (_subAllocatorSize / 8 / 12 * 7);
		int num2 = num / 12 * UNIT_SIZE;
		int num3 = _subAllocatorSize - num;
		int num4 = num3 / 12 * UNIT_SIZE + num3 % 12;
		_hiUnit = _heapStart + _subAllocatorSize;
		_loUnit = (_unitsStart = _heapStart + num4);
		_fakeUnitsStart = _heapStart + num3;
		_hiUnit = _loUnit + num2;
		int num5 = 0;
		int num6 = 1;
		while (num5 < 4)
		{
			_indx2Units[num5] = num6 & 0xFF;
			num5++;
			num6++;
		}
		num6++;
		while (num5 < 8)
		{
			_indx2Units[num5] = num6 & 0xFF;
			num5++;
			num6 += 2;
		}
		num6++;
		while (num5 < 12)
		{
			_indx2Units[num5] = num6 & 0xFF;
			num5++;
			num6 += 3;
		}
		num6++;
		while (num5 < 12 + N4)
		{
			_indx2Units[num5] = num6 & 0xFF;
			num5++;
			num6 += 4;
		}
		_glueCount = 0;
		num6 = 0;
		num5 = 0;
		for (; num6 < 128; num6++)
		{
			num5 += ((_indx2Units[num5] < num6 + 1) ? 1 : 0);
			_units2Indx[num6] = num5 & 0xFF;
		}
	}

	private int SizeOfFreeList()
	{
		return _freeList.Length * 4;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("SubAllocator[");
		stringBuilder.Append("\n  subAllocatorSize=");
		stringBuilder.Append(_subAllocatorSize);
		stringBuilder.Append("\n  glueCount=");
		stringBuilder.Append(_glueCount);
		stringBuilder.Append("\n  heapStart=");
		stringBuilder.Append(_heapStart);
		stringBuilder.Append("\n  loUnit=");
		stringBuilder.Append(_loUnit);
		stringBuilder.Append("\n  hiUnit=");
		stringBuilder.Append(_hiUnit);
		stringBuilder.Append("\n  pText=");
		stringBuilder.Append(_pText);
		stringBuilder.Append("\n  unitsStart=");
		stringBuilder.Append(_unitsStart);
		stringBuilder.Append("\n]");
		return stringBuilder.ToString();
	}

	static SubAllocator()
	{
		N4 = 26;
		N_INDEXES = 12 + N4;
		UNIT_SIZE = Math.Max(PpmContext.SIZE, 12);
	}
}
