using System;
using System.Runtime.Serialization;
using Antlr.Runtime.Tree;

namespace Antlr.Runtime;

[Serializable]
public class MismatchedTreeNodeException : RecognitionException
{
	private readonly int _expecting;

	public int Expecting => _expecting;

	public MismatchedTreeNodeException()
	{
	}

	public MismatchedTreeNodeException(string message)
		: base(message)
	{
	}

	public MismatchedTreeNodeException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public MismatchedTreeNodeException(int expecting, ITreeNodeStream input)
		: base(input)
	{
		_expecting = expecting;
	}

	public MismatchedTreeNodeException(string message, int expecting, ITreeNodeStream input)
		: base(message, input)
	{
		_expecting = expecting;
	}

	public MismatchedTreeNodeException(string message, int expecting, ITreeNodeStream input, Exception innerException)
		: base(message, input, innerException)
	{
		_expecting = expecting;
	}

	protected MismatchedTreeNodeException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		_expecting = info.GetInt32("Expecting");
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
		return "MismatchedTreeNodeException(" + UnexpectedType + "!=" + Expecting + ")";
	}
}
