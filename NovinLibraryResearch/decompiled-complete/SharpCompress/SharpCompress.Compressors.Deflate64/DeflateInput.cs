namespace SharpCompress.Compressors.Deflate64;

internal sealed class DeflateInput
{
	internal struct InputState
	{
		internal readonly int _count;

		internal readonly int _startIndex;

		internal InputState(int count, int startIndex)
		{
			_count = count;
			_startIndex = startIndex;
		}
	}

	public byte[] Buffer { get; }

	public int Count { get; set; }

	public int StartIndex { get; set; }

	public DeflateInput(byte[] buffer)
	{
		Buffer = buffer;
	}

	internal void ConsumeBytes(int n)
	{
		StartIndex += n;
		Count -= n;
	}

	internal InputState DumpState()
	{
		return new InputState(Count, StartIndex);
	}

	internal void RestoreState(InputState state)
	{
		Count = state._count;
		StartIndex = state._startIndex;
	}
}
