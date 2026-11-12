using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public interface ICodeCompletionRepository
{
	bool FillBaseMembers { get; set; }

	bool CaseSensitive { get; }

	ISyntaxTree SyntaxTree { get; }

	IList SyntaxTrees { get; }

	event MemberLookupEvent MemberLookup;

	event DescriptionLookupEvent DescriptionLookup;

	object GetNodeType(string text, ISyntaxNode node, Point position);

	object GetMemberType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out StaticScope scope);

	object GetSpecialMemberType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out StaticScope scope);

	object GetMethodType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out int paramIndex, out int paramCount, out StaticScope scope);

	void FillMembers(ISyntaxNode node, Point position, IListMembers members, object member, string name, StaticScope scope, ref int selIndex);

	void FillMember(IListMembers members, object member, string name, int paramIndex, StaticScope scope);

	object GetMemberType(ISyntaxNode node, Point position, object member, string name, out StaticScope scope);

	string GetDescription(IListMembers members, ISyntaxNode node, object member, string name, bool fullDescription);

	object FindDeclaration(string text, ISyntaxNode node, Point position);

	int FindReferences(ISyntaxNode node, ISyntaxNodes references);

	void RegisterSyntaxTree(ISyntaxTree tree);

	bool UnregisterSyntaxTree(ISyntaxTree tree);

	void RegisterSnippet(string snippet, bool isStatement);

	bool UnregisterSnippet(string snippet);

	ICodeSnippetsProvider GetCodeSnippets(string language);
}
