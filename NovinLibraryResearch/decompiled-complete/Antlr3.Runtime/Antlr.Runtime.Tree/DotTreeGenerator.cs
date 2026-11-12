using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Antlr.Runtime.Tree;

public class DotTreeGenerator
{
	private const string Footer = "}";

	private const string NodeFormat = "  {0} [label=\"{1}\"];";

	private const string EdgeFormat = "  {0} -> {1} // \"{2}\" -> \"{3}\"";

	private readonly string[] HeaderLines = new string[8] { "digraph {", "", "\tordering=out;", "\tranksep=.4;", "\tbgcolor=\"lightgrey\"; node [shape=box, fixedsize=false, fontsize=12, fontname=\"Helvetica-bold\", fontcolor=\"blue\"", "\t\twidth=.25, height=.25, color=\"black\", fillcolor=\"white\", style=\"filled, solid, bold\"];", "\tedge [arrowsize=.5, color=\"black\", style=\"bold\"]", "" };

	private Dictionary<object, int> nodeToNumberMap = new Dictionary<object, int>();

	private int nodeNumber;

	public virtual string ToDot(object tree, ITreeAdaptor adaptor)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string[] headerLines = HeaderLines;
		foreach (string value in headerLines)
		{
			stringBuilder.AppendLine(value);
		}
		nodeNumber = 0;
		IEnumerable<string> enumerable = DefineNodes(tree, adaptor);
		nodeNumber = 0;
		IEnumerable<string> enumerable2 = DefineEdges(tree, adaptor);
		foreach (string item in enumerable)
		{
			stringBuilder.AppendLine(item);
		}
		stringBuilder.AppendLine();
		foreach (string item2 in enumerable2)
		{
			stringBuilder.AppendLine(item2);
		}
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("}");
		return stringBuilder.ToString();
	}

	public virtual string ToDot(ITree tree)
	{
		return ToDot(tree, new CommonTreeAdaptor());
	}

	protected virtual IEnumerable<string> DefineNodes(object tree, ITreeAdaptor adaptor)
	{
		if (tree == null)
		{
			yield break;
		}
		int j = adaptor.GetChildCount(tree);
		if (j == 0)
		{
			yield break;
		}
		yield return GetNodeText(adaptor, tree);
		for (int i = 0; i < j; i++)
		{
			object child = adaptor.GetChild(tree, i);
			yield return GetNodeText(adaptor, child);
			foreach (string item in DefineNodes(child, adaptor))
			{
				yield return item;
			}
		}
	}

	protected virtual IEnumerable<string> DefineEdges(object tree, ITreeAdaptor adaptor)
	{
		if (tree == null)
		{
			yield break;
		}
		int j = adaptor.GetChildCount(tree);
		if (j == 0)
		{
			yield break;
		}
		string parentName = "n" + GetNodeNumber(tree);
		string parentText = adaptor.GetText(tree);
		for (int i = 0; i < j; i++)
		{
			object child = adaptor.GetChild(tree, i);
			string childText = adaptor.GetText(child);
			string childName = "n" + GetNodeNumber(child);
			yield return $"  {parentName} -> {childName} // \"{FixString(parentText)}\" -> \"{FixString(childText)}\"";
			foreach (string item in DefineEdges(child, adaptor))
			{
				yield return item;
			}
		}
	}

	protected virtual string GetNodeText(ITreeAdaptor adaptor, object t)
	{
		string text = adaptor.GetText(t);
		string arg = "n" + GetNodeNumber(t);
		return $"  {arg} [label=\"{FixString(text)}\"];";
	}

	protected virtual int GetNodeNumber(object t)
	{
		if (nodeToNumberMap.TryGetValue(t, out var value))
		{
			return value;
		}
		nodeToNumberMap[t] = nodeNumber;
		nodeNumber++;
		return nodeNumber - 1;
	}

	protected virtual string FixString(string text)
	{
		if (text != null)
		{
			text = Regex.Replace(text, "\"", "\\\\\"");
			text = Regex.Replace(text, "\\t", "    ");
			text = Regex.Replace(text, "\\n", "\\\\n");
			text = Regex.Replace(text, "\\r", "\\\\r");
			if (text.Length > 20)
			{
				text = text.Substring(0, 8) + "..." + text.Substring(text.Length - 8);
			}
		}
		return text;
	}
}
