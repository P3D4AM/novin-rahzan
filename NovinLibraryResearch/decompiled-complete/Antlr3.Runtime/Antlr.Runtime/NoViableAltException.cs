using System;
using System.Runtime.Serialization;

namespace Antlr.Runtime;

[Serializable]
public class NoViableAltException : RecognitionException
{
	private readonly string _grammarDecisionDescription;

	private readonly int _decisionNumber;

	private readonly int _stateNumber;

	public int DecisionNumber => _decisionNumber;

	public string GrammarDecisionDescription => _grammarDecisionDescription;

	public int StateNumber => _stateNumber;

	public NoViableAltException()
	{
	}

	public NoViableAltException(string grammarDecisionDescription)
	{
		_grammarDecisionDescription = grammarDecisionDescription;
	}

	public NoViableAltException(string message, string grammarDecisionDescription)
		: base(message)
	{
		_grammarDecisionDescription = grammarDecisionDescription;
	}

	public NoViableAltException(string message, string grammarDecisionDescription, Exception innerException)
		: base(message, innerException)
	{
		_grammarDecisionDescription = grammarDecisionDescription;
	}

	public NoViableAltException(string grammarDecisionDescription, int decisionNumber, int stateNumber, IIntStream input)
		: this(grammarDecisionDescription, decisionNumber, stateNumber, input, 1)
	{
	}

	public NoViableAltException(string grammarDecisionDescription, int decisionNumber, int stateNumber, IIntStream input, int k)
		: base(input, k)
	{
		_grammarDecisionDescription = grammarDecisionDescription;
		_decisionNumber = decisionNumber;
		_stateNumber = stateNumber;
	}

	public NoViableAltException(string message, string grammarDecisionDescription, int decisionNumber, int stateNumber, IIntStream input)
		: this(message, grammarDecisionDescription, decisionNumber, stateNumber, input, 1)
	{
	}

	public NoViableAltException(string message, string grammarDecisionDescription, int decisionNumber, int stateNumber, IIntStream input, int k)
		: base(message, input, k)
	{
		_grammarDecisionDescription = grammarDecisionDescription;
		_decisionNumber = decisionNumber;
		_stateNumber = stateNumber;
	}

	public NoViableAltException(string message, string grammarDecisionDescription, int decisionNumber, int stateNumber, IIntStream input, Exception innerException)
		: this(message, grammarDecisionDescription, decisionNumber, stateNumber, input, 1, innerException)
	{
	}

	public NoViableAltException(string message, string grammarDecisionDescription, int decisionNumber, int stateNumber, IIntStream input, int k, Exception innerException)
		: base(message, input, k, innerException)
	{
		_grammarDecisionDescription = grammarDecisionDescription;
		_decisionNumber = decisionNumber;
		_stateNumber = stateNumber;
	}

	protected NoViableAltException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		_grammarDecisionDescription = info.GetString("GrammarDecisionDescription");
		_decisionNumber = info.GetInt32("DecisionNumber");
		_stateNumber = info.GetInt32("StateNumber");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		base.GetObjectData(info, context);
		info.AddValue("GrammarDecisionDescription", _grammarDecisionDescription);
		info.AddValue("DecisionNumber", _decisionNumber);
		info.AddValue("StateNumber", _stateNumber);
	}

	public override string ToString()
	{
		if (base.Input is ICharStream)
		{
			return "NoViableAltException('" + (char)UnexpectedType + "'@[" + GrammarDecisionDescription + "])";
		}
		return "NoViableAltException(" + UnexpectedType + "@[" + GrammarDecisionDescription + "])";
	}
}
