using System;
using System.Runtime.Serialization;

namespace Antlr.Runtime;

[Serializable]
public class FailedPredicateException : RecognitionException
{
	private readonly string _ruleName;

	private readonly string _predicateText;

	public string RuleName => _ruleName;

	public string PredicateText => _predicateText;

	public FailedPredicateException()
	{
	}

	public FailedPredicateException(string message)
		: base(message)
	{
	}

	public FailedPredicateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public FailedPredicateException(IIntStream input, string ruleName, string predicateText)
		: base(input)
	{
		_ruleName = ruleName;
		_predicateText = predicateText;
	}

	public FailedPredicateException(string message, IIntStream input, string ruleName, string predicateText)
		: base(message, input)
	{
		_ruleName = ruleName;
		_predicateText = predicateText;
	}

	public FailedPredicateException(string message, IIntStream input, string ruleName, string predicateText, Exception innerException)
		: base(message, input, innerException)
	{
		_ruleName = ruleName;
		_predicateText = predicateText;
	}

	protected FailedPredicateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		_ruleName = info.GetString("RuleName");
		_predicateText = info.GetString("PredicateText");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		base.GetObjectData(info, context);
		info.AddValue("RuleName", _ruleName);
		info.AddValue("PredicateText", _predicateText);
	}

	public override string ToString()
	{
		return "FailedPredicateException(" + RuleName + ",{" + PredicateText + "}?)";
	}
}
