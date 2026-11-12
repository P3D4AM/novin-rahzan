using System.Text;

namespace SharpCompress.Compressors.PPMd.H;

internal class StateRef
{
	private int _symbol;

	private int _freq;

	private int _successor;

	internal int Symbol
	{
		get
		{
			return _symbol;
		}
		set
		{
			_symbol = value & 0xFF;
		}
	}

	internal int Freq
	{
		get
		{
			return _freq;
		}
		set
		{
			_freq = value & 0xFF;
		}
	}

	internal State Values
	{
		set
		{
			Freq = value.Freq;
			SetSuccessor(value.GetSuccessor());
			Symbol = value.Symbol;
		}
	}

	public virtual void IncrementFreq(int dFreq)
	{
		_freq = (_freq + dFreq) & 0xFF;
	}

	public virtual void DecrementFreq(int dFreq)
	{
		_freq = (_freq - dFreq) & 0xFF;
	}

	public virtual int GetSuccessor()
	{
		return _successor;
	}

	public virtual void SetSuccessor(PpmContext successor)
	{
		SetSuccessor(successor.Address);
	}

	public virtual void SetSuccessor(int successor)
	{
		_successor = successor;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("State[");
		stringBuilder.Append("\n  symbol=");
		stringBuilder.Append(Symbol);
		stringBuilder.Append("\n  freq=");
		stringBuilder.Append(Freq);
		stringBuilder.Append("\n  successor=");
		stringBuilder.Append(GetSuccessor());
		stringBuilder.Append("\n]");
		return stringBuilder.ToString();
	}
}
