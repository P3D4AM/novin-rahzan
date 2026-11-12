using System;
using System.Runtime.Serialization;

namespace Antlr.Runtime;

[Serializable]
public class MismatchedSetException : RecognitionException
{
	private readonly BitSet _expecting;

	public BitSet Expecting => _expecting;

	public MismatchedSetException()
	{
	}

	public MismatchedSetException(string message)
		: base(message)
	{
	}

	public MismatchedSetException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public MismatchedSetException(BitSet expecting, IIntStream input)
		: base(input)
	{
		_expecting = expecting;
	}

	public MismatchedSetException(string message, BitSet expecting, IIntStream input)
		: base(message, input)
	{
		_expecting = expecting;
	}

	public MismatchedSetException(string message, BitSet expecting, IIntStream input, Exception innerException)
		: base(message, input, innerException)
	{
		_expecting = expecting;
	}

	protected MismatchedSetException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		_expecting = (BitSet)info.GetValue("Expecting", typeof(BitSet));
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		base.GetObjectData(info, context);
		info.AddValue("Expecting", _expecting);
	}

	public override string ToString()
	{
		return string.Concat("MismatchedSetException(", UnexpectedType, "!=", Expecting, ")");
	}
}
