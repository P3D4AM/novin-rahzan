using System.Globalization;
using System.IO;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class CodeWriter : StringWriter
{
	private const int MaxIndentLevel = 128;

	private const int IndentSize = 4;

	private readonly string[] indentations;

	private int indentLevel;

	private bool indentNext;

	private bool blockBeginning;

	public int IndentLevel
	{
		get
		{
			return indentLevel;
		}
		set
		{
			indentLevel = value;
		}
	}

	public CodeWriter()
		: base(CultureInfo.InvariantCulture)
	{
		indentations = new string[128];
		for (int i = 0; i < indentations.Length; i++)
		{
			indentations[i] = string.Empty.PadRight(i * 4);
		}
		indentNext = true;
	}

	public void BeginCodeBlock()
	{
		WriteLine("{");
		indentLevel++;
		blockBeginning = true;
	}

	public void EndCodeBlock()
	{
		indentLevel--;
		WriteLine("}");
	}

	public override void Write(char c)
	{
		Indent();
		base.Write(c);
		indentNext = false;
		blockBeginning = false;
	}

	public override void Write(string val)
	{
		Indent();
		base.Write(val);
		indentNext = false;
		blockBeginning = false;
	}

	public override void Write(char[] buffer, int index, int count)
	{
		Indent();
		base.Write(buffer, index, count);
		indentNext = count >= 2 && buffer[index + count - 2] == '\r' && buffer[index + count - 1] == '\n';
		blockBeginning = false;
	}

	public void WriteLineIgnoreFirst()
	{
		if (!blockBeginning)
		{
			base.WriteLine();
		}
	}

	private void Indent()
	{
		if (indentNext)
		{
			base.Write(indentations[indentLevel % 128]);
		}
	}
}
