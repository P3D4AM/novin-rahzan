using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
public class XmlCommentParser : XmlParser
{
	private Hashtable indents;

	public XmlCommentParser()
	{
		indents = new Hashtable();
	}

	private bool IsXmlComment(ref string source, out int indent)
	{
		indent = 0;
		if (source == null)
		{
			return false;
		}
		if (source.TrimStart(new char[0]).StartsWith("///"))
		{
			indent = source.Length;
			source = source.TrimStart(new char[0]).Substring("///".Length);
			indent -= source.Length;
			return true;
		}
		if (source.TrimStart(new char[0]).StartsWith("'''"))
		{
			indent = source.Length;
			source = source.TrimStart(new char[0]).Substring("'''".Length);
			indent -= source.Length;
			return true;
		}
		return false;
	}

	protected override void ResetLine(int line)
	{
		string text = source;
		base.ResetLine(line);
		while (source != null)
		{
			if (IsXmlComment(ref text, out var indent))
			{
				indents[line] = indent;
				break;
			}
			line++;
			base.ResetLine(line);
		}
		lineIndex = line;
		source = text;
	}

	protected virtual void ShiftAttribute(ISyntaxAttribute attr)
	{
		object obj = indents[attr.Position.Y];
		if (obj != null)
		{
			attr.Position = new Point(attr.Position.X + (int)obj, attr.Position.Y);
		}
	}

	protected virtual void ShiftError(ISyntaxError err)
	{
		object obj = indents[err.Position.Y];
		if (obj != null)
		{
			err.Position = new Point(err.Position.X + (int)obj, err.Position.Y);
		}
	}

	protected virtual void ShiftNode(ISyntaxNode node)
	{
		node.Options = SyntaxNodeOptions.None;
		object obj = indents[node.Range.StartPoint.Y];
		if (obj != null)
		{
			node.Range.StartPoint = new Point(node.Range.StartPoint.X + (int)obj, node.Range.StartPoint.Y);
		}
		obj = indents[node.Range.EndPoint.Y];
		if (obj != null)
		{
			node.Range.EndPoint = new Point(node.Range.EndPoint.X + (int)obj, node.Range.EndPoint.Y);
		}
		if (node.HasAttributes)
		{
			foreach (ISyntaxAttribute attribute in node.AttributeList)
			{
				ShiftAttribute(attribute);
			}
		}
		if (node.HasErrors)
		{
			foreach (ISyntaxError error in node.ErrorList)
			{
				ShiftError(error);
			}
		}
		if (!node.HasChildren)
		{
			return;
		}
		foreach (ISyntaxNode child in node.ChildList)
		{
			ShiftNode(child);
		}
	}

	public override void Reset()
	{
		indents.Clear();
		base.Reset();
	}

	protected override bool ParseUnit()
	{
		bool result = base.ParseUnit();
		ShiftNode(base.SyntaxTree.Root);
		return result;
	}
}
