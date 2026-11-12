using System;
using System.Diagnostics;

namespace Antlr.Runtime;

public class DFA
{
	protected short[] eot;

	protected short[] eof;

	protected char[] min;

	protected char[] max;

	protected short[] accept;

	protected short[] special;

	protected short[][] transition;

	protected int decisionNumber;

	protected BaseRecognizer recognizer;

	public bool debug;

	public virtual string Description => "n/a";

	public SpecialStateTransitionHandler SpecialStateTransition { get; private set; }

	public DFA()
		: this(SpecialStateTransitionDefault)
	{
	}

	public DFA(SpecialStateTransitionHandler specialStateTransition)
	{
		SpecialStateTransition = specialStateTransition ?? new SpecialStateTransitionHandler(SpecialStateTransitionDefault);
	}

	public virtual int Predict(IIntStream input)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		int marker = input.Mark();
		int num = 0;
		try
		{
			char c;
			while (true)
			{
				int num2 = special[num];
				if (num2 >= 0)
				{
					num = SpecialStateTransition(this, num2, input);
					if (num == -1)
					{
						NoViableAlt(num, input);
						return 0;
					}
					input.Consume();
					continue;
				}
				if (accept[num] >= 1)
				{
					return accept[num];
				}
				c = (char)input.LA(1);
				if (c >= min[num] && c <= max[num])
				{
					int num3 = transition[num][c - min[num]];
					if (num3 < 0)
					{
						if (eot[num] < 0)
						{
							NoViableAlt(num, input);
							return 0;
						}
						num = eot[num];
						input.Consume();
					}
					else
					{
						num = num3;
						input.Consume();
					}
				}
				else
				{
					if (eot[num] < 0)
					{
						break;
					}
					num = eot[num];
					input.Consume();
				}
			}
			if (c == '\uffff' && eof[num] >= 0)
			{
				return accept[eof[num]];
			}
			NoViableAlt(num, input);
			return 0;
		}
		finally
		{
			input.Rewind(marker);
		}
	}

	[Conditional("DEBUG_DFA")]
	private void DfaDebugMessage(string format, params object[] args)
	{
		Console.Error.WriteLine(format, args);
	}

	[Conditional("DEBUG_DFA")]
	private void DfaDebugInvalidSymbol(int s)
	{
		Console.Error.WriteLine("min[{0}]={1}", s, min[s]);
		Console.Error.WriteLine("max[{0}]={1}", s, max[s]);
		Console.Error.WriteLine("eot[{0}]={1}", s, eot[s]);
		Console.Error.WriteLine("eof[{0}]={1}", s, eof[s]);
		for (int i = 0; i < transition[s].Length; i++)
		{
			Console.Error.Write(transition[s][i] + " ");
		}
		Console.Error.WriteLine();
	}

	protected virtual void NoViableAlt(int s, IIntStream input)
	{
		if (recognizer.state.backtracking > 0)
		{
			recognizer.state.failed = true;
			return;
		}
		NoViableAltException ex = new NoViableAltException(Description, decisionNumber, s, input);
		Error(ex);
		throw ex;
	}

	public virtual void Error(NoViableAltException nvae)
	{
	}

	private static int SpecialStateTransitionDefault(DFA dfa, int s, IIntStream input)
	{
		return -1;
	}

	public static short[] UnpackEncodedString(string encodedString)
	{
		int num = 0;
		for (int i = 0; i < encodedString.Length; i += 2)
		{
			num += encodedString[i];
		}
		short[] array = new short[num];
		int num2 = 0;
		for (int j = 0; j < encodedString.Length; j += 2)
		{
			char c = encodedString[j];
			char c2 = encodedString[j + 1];
			for (int k = 1; k <= c; k++)
			{
				array[num2++] = (short)c2;
			}
		}
		return array;
	}

	public static char[] UnpackEncodedStringToUnsignedChars(string encodedString)
	{
		int num = 0;
		for (int i = 0; i < encodedString.Length; i += 2)
		{
			num += encodedString[i];
		}
		char[] array = new char[num];
		int num2 = 0;
		for (int j = 0; j < encodedString.Length; j += 2)
		{
			char c = encodedString[j];
			char c2 = encodedString[j + 1];
			for (int k = 1; k <= c; k++)
			{
				array[num2++] = c2;
			}
		}
		return array;
	}

	[Conditional("ANTLR_DEBUG")]
	protected virtual void DebugRecognitionException(RecognitionException ex)
	{
		recognizer.DebugListener?.RecognitionException(ex);
	}
}
