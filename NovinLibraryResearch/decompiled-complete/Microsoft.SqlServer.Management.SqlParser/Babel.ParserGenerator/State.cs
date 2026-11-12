using System.Collections.Generic;
using System.Globalization;

namespace Babel.ParserGenerator;

internal class State
{
	public int num;

	public Dictionary<int, int> parser_table;

	public Dictionary<int, int> Goto;

	public int defaultAction;

	public bool HasDefaultReduce => defaultAction < 0;

	public State(int[] actions, int[] gotos)
		: this(actions)
	{
		Goto = new Dictionary<int, int>();
		for (int i = 0; i < gotos.Length; i += 2)
		{
			Goto.Add(gotos[i], gotos[i + 1]);
		}
	}

	public State(int[] actions)
	{
		parser_table = new Dictionary<int, int>();
		for (int i = 0; i < actions.Length; i += 2)
		{
			parser_table.Add(actions[i], actions[i + 1]);
		}
	}

	public State(int defaultAction)
	{
		this.defaultAction = defaultAction;
	}

	public State(int defaultAction, int[] gotos)
		: this(defaultAction)
	{
		Goto = new Dictionary<int, int>();
		for (int i = 0; i < gotos.Length; i += 2)
		{
			Goto.Add(gotos[i], gotos[i + 1]);
		}
	}

	public bool ExpectsToken(int token)
	{
		int action;
		return ExpectsToken(token, out action);
	}

	public bool ExpectsToken(int token, out int action)
	{
		if (parser_table != null && parser_table.TryGetValue(token, out action))
		{
			return true;
		}
		action = 0;
		return false;
	}

	public bool ShiftsOnToken(int token)
	{
		ExpectsToken(token, out var action);
		return action > 0;
	}

	public bool ReducesOnToken(int token)
	{
		ExpectsToken(token, out var action);
		return action < 0;
	}

	public override string ToString()
	{
		return num.ToString(CultureInfo.InvariantCulture);
	}
}
