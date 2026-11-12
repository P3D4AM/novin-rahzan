using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public interface ISyntaxNode
{
	IRange Range { get; set; }

	Point Position { get; set; }

	Size Size { get; set; }

	string Name { get; set; }

	int NodeType { get; set; }

	ISyntaxNode Parent { get; set; }

	ISyntaxNode Root { get; }

	SyntaxNodeOptions Options { get; set; }

	ISyntaxNode[] Childs { get; set; }

	ISyntaxAttribute[] Attributes { get; set; }

	ISyntaxError[] Errors { get; set; }

	ISyntaxNodes ChildList { get; }

	ISyntaxAttributes AttributeList { get; }

	ISyntaxErrors ErrorList { get; }

	bool HasChildren { get; }

	bool HasAttributes { get; }

	bool HasErrors { get; }

	int Level { get; }

	int ChildCount { get; }

	int AttributeCount { get; }

	int ErrorCount { get; }

	int Index { get; }

	void Clear();

	int AddChild(ISyntaxNode node);

	ISyntaxNode AddChild();

	int InsertChild(ISyntaxNode node, IComparer comparer);

	void AddChildren(ISyntaxNodes nodes);

	int AddAttribute(ISyntaxAttribute attr);

	ISyntaxAttribute AddAttribute();

	void AddAttributes(ISyntaxAttributes attrs);

	int AddError(ISyntaxError err);

	ISyntaxError AddError();

	void AddErrors(ISyntaxErrors errs);

	ISyntaxNode FindNode(string name);

	ISyntaxNode FindNode(int nodeType);

	ISyntaxNode FindNode(object obj, IComparer comparer);

	ISyntaxAttribute FindAttribute(string name);

	void Sort(IComparer comparer);

	int GetIndent(int index, int indent);
}
