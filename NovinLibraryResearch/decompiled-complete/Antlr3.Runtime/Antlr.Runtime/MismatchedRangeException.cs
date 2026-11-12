using System;
using System.Runtime.Serialization;

namespace Antlr.Runtime;

[Serializable]
public class MismatchedRangeException : RecognitionException
{
	private readonly int _a;

	private readonly int _b;

	public int A => _a;

	public int B => _b;

	public MismatchedRangeException()
	{
	}

	public MismatchedRangeException(string message)
		: base(message)
	{
	}

	public MismatchedRangeException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public MismatchedRangeException(int a, int b, IIntStream input)
		: base(input)
	{
		_a = a;
		_b = b;
	}

	public MismatchedRangeException(string message, int a, int b, IIntStream input)
		: base(message, input)
	{
		_a = a;
		_b = b;
	}

	public MismatchedRangeException(string message, int a, int b, IIntStream input, Exception innerException)
		: base(message, input, innerException)
	{
		_a = a;
		_b = b;
	}

	protected MismatchedRangeException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		_a = info.GetInt32("A");
		_b = info.GetInt32("B");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		base.GetObjectData(info, context);
		info.AddValue("A", _a);
		info.AddValue("B", _b);
	}

	public override string ToString()
	{
		return "MismatchedRangeException(" + UnexpectedType + " not in [" + A + "," + B + "])";
	}
}
