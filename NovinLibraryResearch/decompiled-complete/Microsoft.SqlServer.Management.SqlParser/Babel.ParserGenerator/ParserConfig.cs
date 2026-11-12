using System.Collections.Generic;
using System.IO;
using Microsoft.SqlServer.Management.SqlParser;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Babel.ParserGenerator;

internal abstract class ParserConfig
{
	public readonly string[] NonTerminals;

	public readonly State[] States;

	public readonly Rule[] Rules;

	private const string Azure = "Azure";

	private string parserVersion;

	protected ParserConfig(string version)
	{
		parserVersion = version;
		NonTerminals = GetNonTerminals();
		States = GetStates();
		Rules = GetRules();
	}

	private Rule[] GetRules()
	{
		using Stream input = GetType().GetAssembly().GetManifestResourceStream("Microsoft.SqlServer.Management.SqlParser.parser.rules." + parserVersion + ".bin");
		using BinaryReader binaryReader = new BinaryReader(input);
		int num = binaryReader.ReadInt16();
		Rule[] array = new Rule[num];
		int num2 = 0;
		int[] array2 = null;
		for (int i = 1; i < num; i++)
		{
			num2 = binaryReader.ReadInt16();
			ReadArray(out array2, binaryReader);
			array[i] = new Rule(num2, array2);
		}
		return array;
	}

	private void ReadArray(out int[] array, BinaryReader reader)
	{
		int num = reader.ReadInt16();
		if (num >= 0)
		{
			array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.ReadInt16();
			}
		}
		else
		{
			array = null;
		}
	}

	private void ReadArray(out string[] array, BinaryReader reader)
	{
		int num = reader.ReadInt16();
		if (num >= 0)
		{
			array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.ReadString();
			}
		}
		else
		{
			array = null;
		}
	}

	private string[] GetNonTerminals()
	{
		using Stream input = GetType().GetAssembly().GetManifestResourceStream("Microsoft.SqlServer.Management.SqlParser.parser.nonterminals." + parserVersion + ".bin");
		using BinaryReader reader = new BinaryReader(input);
		ReadArray(out string[] array, reader);
		return array;
	}

	private State[] GetStates()
	{
		using Stream input = GetType().GetAssembly().GetManifestResourceStream("Microsoft.SqlServer.Management.SqlParser.parser.states." + parserVersion + ".bin");
		using BinaryReader binaryReader = new BinaryReader(input);
		int num = binaryReader.ReadInt16();
		State[] array = new State[num];
		for (int i = 0; i < num; i++)
		{
			State state = new State(binaryReader.ReadInt16());
			ReadDictionary(out state.parser_table, binaryReader);
			ReadDictionary(out state.Goto, binaryReader);
			state.num = i;
			array[i] = state;
		}
		return array;
	}

	private void ReadDictionary(out Dictionary<int, int> dict, BinaryReader reader)
	{
		int num = reader.ReadInt16();
		if (num >= 0)
		{
			dict = new Dictionary<int, int>(num);
			for (int i = 0; i < num; i++)
			{
				short key = reader.ReadInt16();
				short value = reader.ReadInt16();
				dict.Add(key, value);
			}
		}
		else
		{
			dict = null;
		}
	}

	public abstract OptLookup GetOptLookupByContext(TokenTransformContext context);

	public abstract TokenTransformContext GetTransformContext(State state, int token);
}
