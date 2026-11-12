using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public class CodeCompletionRepository : ICodeCompletionRepository
{
	private bool caseSensitive;

	private ISyntaxTree syntaxTree;

	private ArrayList syntaxTrees;

	private Hashtable snippets;

	private Hashtable membersTable;

	private bool fillBaseMembers = true;

	private MemberLookupEventArgs memberLookupArgs;

	private DescriptionLookupEventArgs descriptionLookupArgs;

	public bool FillBaseMembers
	{
		get
		{
			return fillBaseMembers;
		}
		set
		{
			fillBaseMembers = value;
		}
	}

	public bool CaseSensitive => caseSensitive;

	public ISyntaxTree SyntaxTree => syntaxTree;

	public IList SyntaxTrees => syntaxTrees;

	public Hashtable Snippets => snippets;

	public event MemberLookupEvent MemberLookup;

	public event DescriptionLookupEvent DescriptionLookup;

	public CodeCompletionRepository(bool caseSensitive, ISyntaxTree syntaxTree)
	{
		this.caseSensitive = caseSensitive;
		this.syntaxTree = syntaxTree;
		syntaxTrees = new ArrayList();
		membersTable = new Hashtable();
		snippets = new Hashtable();
		memberLookupArgs = new MemberLookupEventArgs(null, string.Empty);
		descriptionLookupArgs = new DescriptionLookupEventArgs(null, string.Empty);
	}

	protected virtual object GetMemberKey(string name, int index)
	{
		return index + "|" + name;
	}

	protected virtual bool CanAddMember(IListMembers members, string name, int index, bool overloads)
	{
		if (ShouldDuplicate(members))
		{
			return true;
		}
		object obj = membersTable[GetMemberKey(name, index)];
		if (obj != null && overloads)
		{
			((IListMember)obj).Overloads++;
		}
		return obj == null;
	}

	protected virtual IListMember AddMember(IListMembers members, string name, int index, bool showDesc)
	{
		IListMember listMember = members.AddMember();
		listMember.Name = name;
		listMember.ImageIndex = index;
		listMember.OwnDescription = showDesc && HasDescriptions(members);
		if (!ShouldDuplicate(members))
		{
			membersTable[GetMemberKey(name, index)] = listMember;
		}
		return listMember;
	}

	protected virtual bool ShouldDuplicate(IListMembers members)
	{
		return members is IParameterInfo;
	}

	protected virtual bool HasDescriptions(IListMembers members)
	{
		return !(members is IParameterInfo);
	}

	protected virtual void UpdateMethodParams(IListMember member, int paramIndex)
	{
		member.CurrentParamIndex = paramIndex;
	}

	protected virtual void UpdateMethodParams(IListMembers members, int paramIndex)
	{
		foreach (IListMember member in members)
		{
			UpdateMethodParams(member, paramIndex);
		}
	}

	protected virtual void DoFillMembers(ISyntaxNode node, Point position, IListMembers members, IList types, object member, string name, StaticScope scope, ref int selIndex)
	{
	}

	protected virtual object DoFindDeclaration(string text, ISyntaxNode node, Point position, int paramCount)
	{
		return null;
	}

	public virtual object GetNodeType(string text, ISyntaxNode node, Point position)
	{
		return DoFindDeclaration(text, node, position, 0);
	}

	public virtual object GetMemberType(ISyntaxNode node, Point position, object member, string name, out StaticScope scope)
	{
		if (this.MemberLookup != null)
		{
			memberLookupArgs.Member = member;
			memberLookupArgs.Name = name;
			memberLookupArgs.Result = null;
			memberLookupArgs.Scope = StaticScope.None;
			this.MemberLookup(this, memberLookupArgs);
			if (memberLookupArgs.Result != null)
			{
				scope = memberLookupArgs.Scope;
				return memberLookupArgs.Result;
			}
		}
		scope = StaticScope.None;
		return null;
	}

	public virtual string GetDescription(IListMembers members, ISyntaxNode node, object member, string name, bool fullDescription)
	{
		if (this.DescriptionLookup != null)
		{
			descriptionLookupArgs.Member = member;
			descriptionLookupArgs.Name = name;
			descriptionLookupArgs.Description = string.Empty;
			this.DescriptionLookup(this, descriptionLookupArgs);
			return descriptionLookupArgs.Description;
		}
		return string.Empty;
	}

	public virtual object GetMemberType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out StaticScope scope)
	{
		scope = StaticScope.None;
		return null;
	}

	public virtual object GetSpecialMemberType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out StaticScope scope)
	{
		scope = StaticScope.None;
		return null;
	}

	public virtual object GetMethodType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out int paramIndex, out int paramCount, out StaticScope scope)
	{
		paramIndex = -1;
		paramCount = 0;
		scope = StaticScope.None;
		return null;
	}

	public virtual void FillMembers(ISyntaxNode node, Point position, IListMembers members, object member, string name, StaticScope scope, ref int selIndex)
	{
		membersTable.Clear();
		IList types = new ArrayList();
		DoFillMembers(node, position, members, types, member, name, scope, ref selIndex);
	}

	public virtual void FillMember(IListMembers members, object member, string name, StaticScope scope)
	{
	}

	public virtual void FillMember(IListMembers members, object member, string name, int paramIndex, StaticScope scope)
	{
		membersTable.Clear();
		FillMember(members, member, name, scope);
		UpdateMethodParams(members, paramIndex);
	}

	public virtual object FindDeclaration(string text, ISyntaxNode node, Point position)
	{
		return DoFindDeclaration(text, node, position, 0);
	}

	public virtual int FindReferences(ISyntaxNode node, ISyntaxNodes references)
	{
		references.Clear();
		references.Add(node);
		return 0;
	}

	public virtual void RegisterSyntaxTree(ISyntaxTree tree)
	{
		syntaxTrees.Add(tree);
	}

	public virtual bool UnregisterSyntaxTree(ISyntaxTree tree)
	{
		int num = syntaxTrees.IndexOf(tree);
		if (num >= 0)
		{
			syntaxTrees.RemoveAt(num);
		}
		return num >= 0;
	}

	public virtual void RegisterSnippet(string snippet, bool isStatement)
	{
		snippets.Add(snippet, isStatement);
	}

	public virtual bool UnregisterSnippet(string snippet)
	{
		if (snippets.Contains(snippet))
		{
			snippets.Remove(snippet);
			return true;
		}
		return false;
	}

	public virtual ICodeSnippetsProvider GetCodeSnippets(string language)
	{
		return null;
	}
}
