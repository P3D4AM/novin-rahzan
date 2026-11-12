using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;

namespace Stimulsoft.Editor;

public class NetSyntaxParser : SyntaxParser
{
	private XmlCommentParser xmlParser;

	protected Point prevPosition;

	protected Point savePrevPosition;

	protected ISyntaxNodes comments;

	protected const int reswordStyle = 2;

	protected XmlParser XmlParser
	{
		get
		{
			if (xmlParser == null)
			{
				xmlParser = new XmlCommentParser();
				xmlParser.Strings = new StringList();
			}
			return xmlParser;
		}
	}

	[Description("Gets or sets a boolean value that indicates whether \"NetSyntaxParser\" should perform case-sensitive analysis of its content.")]
	public override bool CaseSensitive => true;

	public NetSyntaxParser()
	{
		comments = new SyntaxNodes();
	}

	protected override void InitStyles()
	{
		AddStyle(StringConsts.IdentsInternalName, SystemColors.ControlText);
		AddStyle(StringConsts.NumbersInternalName, Color.Green);
		AddStyle(StringConsts.ReswordsInternalName, Color.Blue);
		AddStyle(StringConsts.CommentsInternalName, Color.Green, (FontStyle)0, plainText: true);
		AddStyle(StringConsts.XMLCommentsInternalName, Color.Gray, (FontStyle)0, plainText: false);
		AddStyle(StringConsts.SymbolsInternalName, Color.Gray);
		AddStyle(StringConsts.WhiteSpaceInternalName, Color.Empty);
		AddStyle(StringConsts.StringsInternalName, Color.Maroon, (FontStyle)0, plainText: true);
		AddStyle(StringConsts.DirectivesInternalName, Color.Blue);
		AddStyle(StringConsts.SyntaxErrorsInternalName, Color.Red);
		AddStyle(StringConsts.CodeSnippetsInternalName, Color.Black, Color.FromArgb(180, 228, 180), (FontStyle)0, plainText: false);
	}

	private string RemoveQuotes(string s)
	{
		if (s.Length < 2 || (s[0] != '"' && s[0] != '\'') || s[s.Length - 1] != s[0])
		{
			return s;
		}
		return s.Substring(1, s.Length - 2);
	}

	protected ISyntaxNode FindXmlNode(ISyntaxNode node, string tagName, string paramName, string paramValue)
	{
		ISyntaxNode syntaxNode = null;
		if (node.NodeType == 57 && string.Compare(tagName, node.Name, ignoreCase: true) == 0)
		{
			if (paramName != string.Empty)
			{
				ISyntaxNode syntaxNode2 = node.FindNode(59);
				if (syntaxNode2 != null && syntaxNode2.HasChildren)
				{
					foreach (ISyntaxNode child in syntaxNode2.ChildList)
					{
						if (string.Compare(child.Name, paramName, ignoreCase: true) == 0)
						{
							ISyntaxAttribute syntaxAttribute = child.FindAttribute(NetNodeType.XmlParameter.ToString());
							if (syntaxAttribute != null && syntaxAttribute.Value is string && paramValue == RemoveQuotes(((string)syntaxAttribute.Value).Trim()))
							{
								syntaxNode = node;
								break;
							}
						}
					}
				}
			}
			else
			{
				syntaxNode = node;
			}
		}
		if (syntaxNode == null && node.HasChildren)
		{
			foreach (ISyntaxNode child2 in node.ChildList)
			{
				syntaxNode = FindXmlNode(child2, tagName, paramName, paramValue);
				if (syntaxNode != null)
				{
					break;
				}
			}
		}
		return syntaxNode;
	}

	protected virtual string GetXmlComment(ISyntaxNode node)
	{
		string text = ((node.NodeType == 60) ? node.Name : string.Empty);
		if (node.HasChildren)
		{
			foreach (ISyntaxNode child in node.ChildList)
			{
				text += GetXmlComment(child);
			}
		}
		return text;
	}

	protected virtual string GetXmlDescription(ISyntaxNode node, string tagName, string paramName, string paramValue)
	{
		if (node.Parent != null)
		{
			int num = node.Index - 1;
			if (num >= 0)
			{
				node = node.Parent.ChildList[num];
				if (node.NodeType == 55)
				{
					node = FindXmlNode(node, tagName, paramName, paramValue);
					if (node != null)
					{
						return GetXmlComment(node);
					}
				}
			}
		}
		return string.Empty;
	}

	protected int XmlToNetNodeType(int nodeType)
	{
		return 55 + nodeType;
	}

	protected virtual bool ParseXmlComment(ISyntaxNode node)
	{
		XmlParser xmlParser = XmlParser;
		((StringList)xmlParser.Strings).Clear();
		for (int i = node.Range.StartPoint.Y; i <= node.Range.EndPoint.Y; i++)
		{
			((StringList)xmlParser.Strings).Add((i == node.Range.StartPoint.Y) ? Strings[i].Substring(node.Range.StartPoint.X) : Strings[i]);
		}
		xmlParser.ReparseText();
		if (xmlParser.SyntaxTree.Root.HasChildren)
		{
			foreach (ISyntaxNode child in xmlParser.SyntaxTree.Root.ChildList)
			{
				AddXmlNode(node, child, node.Position);
			}
		}
		return true;
	}

	protected Point ShiftPoint(Point destPos, Point srcPos)
	{
		return new Point(destPos.X + ((destPos.Y == 0) ? srcPos.X : 0), destPos.Y + srcPos.Y);
	}

	protected virtual void AddXmlNode(ISyntaxNode parent, ISyntaxNode node, Point pos)
	{
		ISyntaxNode syntaxNode = new SyntaxNode(ShiftPoint(node.Position, pos), node.Name, XmlToNetNodeType(node.NodeType));
		syntaxNode.Range.EndPoint = ShiftPoint(node.Range.EndPoint, pos);
		parent.AddChild(syntaxNode);
		if (node.HasAttributes)
		{
			foreach (ISyntaxAttribute attribute in node.AttributeList)
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(ShiftPoint(attribute.Position, pos), attribute.Name, attribute.Value));
			}
		}
		if (!node.HasChildren)
		{
			return;
		}
		foreach (ISyntaxNode child in node.ChildList)
		{
			AddXmlNode(syntaxNode, child, pos);
		}
	}

	public override ICodeCompletionRepository CreateRepository()
	{
		ReflectionRepository reflectionRepository = new ReflectionRepository(CaseSensitive, base.SyntaxTree);
		reflectionRepository.RegisterDefaultAssemblies();
		return reflectionRepository;
	}

	public override CodeCompletionType GetCompletionType(CodeCompletionArgs e)
	{
		if (e.CompletionType == CodeCompletionType.None)
		{
			switch (e.KeyChar)
			{
			case '.':
				return CodeCompletionType.ListMembers;
			case '(':
				return CodeCompletionType.ParameterInfo;
			}
			if (Array.IndexOf(base.CodeCompletionChars, e.KeyChar) >= 0)
			{
				return CodeCompletionType.ListMembers;
			}
		}
		return base.GetCompletionType(e);
	}

	public void RegisterNamespace(string nspace)
	{
		((IReflectionRepository)CompletionRepository).RegisterNamespace(nspace);
	}

	public bool UnregisterNamespace(string nspace)
	{
		return ((IReflectionRepository)CompletionRepository).UnregisterNamespace(nspace);
	}

	public void RegisterObject(string name, object obj)
	{
		((IReflectionRepository)CompletionRepository).RegisterObject(name, obj);
	}

	public bool UnregisterObject(string name)
	{
		return ((IReflectionRepository)CompletionRepository).UnregisterObject(name);
	}

	public void RegisterType(string name, Type type)
	{
		((IReflectionRepository)CompletionRepository).RegisterType(name, type);
	}

	public void RegisterType(string name, Type type, bool global)
	{
		((IReflectionRepository)CompletionRepository).RegisterType(name, type, global);
	}

	public bool UnregisterType(string name)
	{
		return ((IReflectionRepository)CompletionRepository).UnregisterType(name);
	}

	public void RegisterAssembly(Assembly assembly)
	{
		((IReflectionRepository)CompletionRepository).RegisterAssembly(assembly);
	}

	public bool RegisterAssembly(string name)
	{
		return ((IReflectionRepository)CompletionRepository).RegisterAssembly(name);
	}

	public bool UnregisterAssembly(Assembly assembly, bool removeReferences)
	{
		return ((IReflectionRepository)CompletionRepository).UnregisterAssembly(assembly, removeReferences);
	}

	public bool UnregisterAssembly(string name, bool removeReferences)
	{
		return ((IReflectionRepository)CompletionRepository).UnregisterAssembly(name, removeReferences);
	}

	public void RegisterAllAssemblies()
	{
		((IReflectionRepository)CompletionRepository).RegisterAllAssemblies();
	}

	public void RegisterDefaultAssemblies()
	{
		((IReflectionRepository)CompletionRepository).RegisterDefaultAssemblies();
	}

	protected virtual ISyntaxNode GetLastChild(ISyntaxNode node)
	{
		if (node.HasChildren)
		{
			return GetLastChild(node.ChildList[node.ChildList.Count - 1]);
		}
		return node;
	}

	protected virtual void AddNode(ISyntaxNode node)
	{
		base.SyntaxTree.Current.AddChild(node);
	}

	protected virtual void AddAttribute(ISyntaxAttribute attr)
	{
		base.SyntaxTree.Current.AddAttribute(attr);
	}

	protected virtual ISyntaxNode GetValidNode(ISyntaxNode node)
	{
		while (node.Parent != null && node.NodeType == 39)
		{
			node = node.Parent;
		}
		return node;
	}

	protected virtual void SyntaxError(Point position, Point endPos, string name, string error)
	{
		if (base.Stack.Count == 0)
		{
			ISyntaxError syntaxError = new SyntaxError(position, name, error);
			syntaxError.Range.EndPoint = endPos;
			base.SyntaxTree.Current.AddError(syntaxError);
		}
	}

	protected virtual void SyntaxError()
	{
		if (base.Stack.Count == 0)
		{
			SyntaxError(StringConsts.ErrSyntax);
		}
	}

	protected virtual void SyntaxError(string error)
	{
		if (base.Stack.Count == 0)
		{
			base.SyntaxTree.Current.AddError(new SyntaxError(base.TokenPosition, base.TokenString, error));
		}
	}

	protected virtual bool BeforeDeclaration(ISyntaxNode node)
	{
		bool result = true;
		NetNodeType nodeType = (NetNodeType)GetValidNode(base.SyntaxTree.Current).NodeType;
		switch ((NetNodeType)node.NodeType)
		{
		case NetNodeType.UsingList:
		case NetNodeType.Using:
			if (nodeType != NetNodeType.Unit && nodeType != NetNodeType.Namespace)
			{
				result = false;
				SyntaxError();
			}
			break;
		case NetNodeType.Namespace:
			if (nodeType != NetNodeType.Unit && nodeType != NetNodeType.Namespace)
			{
				SyntaxError();
			}
			else
			{
				if (!node.HasAttributes)
				{
					break;
				}
				foreach (ISyntaxAttribute attribute in node.AttributeList)
				{
					if (attribute.Name == NetNodeType.Modifier.ToString() && attribute.Value is string)
					{
						switch ((string)attribute.Value)
						{
						case "internal":
						case "private":
						case "protected":
							SyntaxError(StringConsts.ErrNamespaceModifiers);
							result = false;
							goto end_IL_015b;
						}
					}
					continue;
					end_IL_015b:
					break;
				}
			}
			break;
		case NetNodeType.Class:
		case NetNodeType.Struct:
		case NetNodeType.Interface:
		case NetNodeType.Enum:
		case NetNodeType.Operator:
			if (nodeType == NetNodeType.Interface)
			{
				SyntaxError(node.Position, base.TokenPosition, node.Name, StringConsts.ErrInterfaceDeclaration);
				result = false;
			}
			break;
		case NetNodeType.Constructor:
		case NetNodeType.Destructor:
			switch (nodeType)
			{
			case NetNodeType.Unit:
			case NetNodeType.Namespace:
				SyntaxError(node.Position, base.TokenPosition, node.Name, StringConsts.ErrNamespaceDeclaration);
				result = false;
				break;
			case NetNodeType.Interface:
				SyntaxError(node.Position, base.TokenPosition, node.Name, StringConsts.ErrInterfaceDeclaration);
				result = false;
				break;
			}
			break;
		case NetNodeType.Event:
			if (nodeType == NetNodeType.Namespace || nodeType == NetNodeType.Unit)
			{
				SyntaxError(node.Position, base.TokenPosition, node.Name, StringConsts.ErrNamespaceDeclaration);
				result = false;
			}
			break;
		case NetNodeType.Delegate:
			switch (nodeType)
			{
			case NetNodeType.Interface:
				SyntaxError(node.Position, base.TokenPosition, node.Name, StringConsts.ErrInterfaceDeclaration);
				result = false;
				break;
			case NetNodeType.Struct:
				SyntaxError();
				result = false;
				break;
			}
			break;
		case NetNodeType.Method:
		case NetNodeType.Property:
		case NetNodeType.Indexer:
			if (nodeType == NetNodeType.Namespace || nodeType == NetNodeType.Unit)
			{
				SyntaxError(node.Position, prevPosition, node.Name, StringConsts.ErrNamespaceDeclaration);
				result = false;
			}
			break;
		case NetNodeType.Field:
			switch (nodeType)
			{
			case NetNodeType.Unit:
			case NetNodeType.Namespace:
				SyntaxError(node.Position, prevPosition, node.Name, StringConsts.ErrNamespaceDeclaration);
				result = false;
				break;
			case NetNodeType.Interface:
				SyntaxError(node.Position, prevPosition, node.Name, StringConsts.ErrInterfaceDeclaration);
				result = false;
				break;
			}
			break;
		}
		return result;
	}

	protected virtual bool AfterDeclaration(ISyntaxNode node)
	{
		bool flag = true;
		NetNodeType nodeType = (NetNodeType)GetValidNode(base.SyntaxTree.Current).NodeType;
		NetNodeType nodeType2 = (NetNodeType)node.NodeType;
		if ((uint)(nodeType2 - 17) <= 2u || nodeType2 == NetNodeType.Event || nodeType2 == NetNodeType.PropertyAccessor)
		{
			ISyntaxAttribute syntaxAttribute = node.FindAttribute(SyntaxConsts.DefinitionScope);
			if (syntaxAttribute != null)
			{
				node.Options |= SyntaxNodeOptions.Outlining;
				if (nodeType == NetNodeType.Interface)
				{
					node.AddError(new SyntaxError(syntaxAttribute.Position, syntaxAttribute.Name, StringConsts.ErrInterfaceMemberDeclaration));
					flag = false;
				}
			}
		}
		if (flag && node.HasChildren)
		{
			foreach (ISyntaxNode child in node.ChildList)
			{
				if (!AfterDeclaration(child))
				{
					flag = false;
				}
			}
		}
		return flag;
	}

	protected virtual bool ParseUnit()
	{
		bool flag = true;
		ISyntaxNode root = base.SyntaxTree.Root;
		root.NodeType = 1;
		root.Position = Point.Empty;
		flag = ParseUnitBody();
		base.SyntaxTree.Root.Range.EndPoint = prevPosition;
		if (!ClearStack())
		{
			flag = false;
		}
		if (!FixupRegions(base.SyntaxTree.Current))
		{
			flag = false;
		}
		return flag;
	}

	protected virtual bool ClearStack()
	{
		bool result = true;
		ISyntaxNode current = base.SyntaxTree.Current;
		while (current != base.SyntaxTree.Root)
		{
			SyntaxError(current.Position, new Point(current.Position.X + 1, current.Position.Y), current.Name, StringConsts.ErrEndOfFileFound);
			current.Range.EndPoint = prevPosition;
			base.SyntaxTree.Pop();
			current = base.SyntaxTree.Current;
			result = false;
		}
		return result;
	}

	protected virtual bool FixupRegions(ISyntaxNode node)
	{
		bool result = true;
		if (node.HasChildren)
		{
			for (int num = node.ChildList.Count - 1; num >= 0; num--)
			{
				if (!FixupRegions(node.ChildList[num]))
				{
					result = false;
				}
			}
		}
		if (node.NodeType == 39 && (node.Options & SyntaxNodeOptions.Outlining) == 0 && node.Parent != null)
		{
			if (node.HasChildren)
			{
				foreach (ISyntaxNode child in node.ChildList)
				{
					node.Parent.AddChild(child);
				}
				node.ChildList.Clear();
			}
			node.Parent.AddError(new SyntaxError(node.Position, node.Name, StringConsts.ErrEndOfFileFound));
		}
		return result;
	}

	protected virtual bool IsComment(int token)
	{
		return false;
	}

	protected virtual bool ParseComment()
	{
		while (IsComment(base.Token))
		{
			NextToken();
		}
		return true;
	}

	protected virtual int MoveNext()
	{
		prevPosition = base.CurrentPosition;
		int result = NextToken();
		while (!base.Eof && !IsValidToken(result))
		{
			if (IsComment(base.Token))
			{
				ParseComment();
				result = base.Token;
			}
			else
			{
				result = NextToken();
			}
		}
		return result;
	}

	protected virtual bool IsContentDivider(ISyntaxNode node)
	{
		int nodeType = node.NodeType;
		if ((uint)(nodeType - 17) <= 2u)
		{
			return true;
		}
		return false;
	}

	protected virtual bool ShouldOutlineCommentNode(ISyntaxNode node)
	{
		return NETRepository.IsDeclarationNode(GetValidNode(node));
	}

	protected virtual void FixupComments()
	{
		foreach (ISyntaxNode comment in comments)
		{
			ISyntaxNode syntaxNode2 = base.SyntaxTree.FindNode(comment.Position, pointNodeComparer);
			if (syntaxNode2 != null)
			{
				if (ShouldOutlineCommentNode(syntaxNode2))
				{
					comment.Options |= SyntaxNodeOptions.Outlining;
				}
				syntaxNode2.InsertChild(comment, nodeComparer);
			}
		}
		comments.Clear();
	}

	protected virtual bool ParseBlock()
	{
		return false;
	}

	protected virtual bool ParseUsingDeclaration(ISyntaxNode node)
	{
		return false;
	}

	protected virtual bool ParseUsingList(ISyntaxNode node)
	{
		return false;
	}

	protected virtual bool ParseUnitBody()
	{
		return false;
	}

	protected virtual bool ParseDeclaration(ISyntaxNode node)
	{
		return false;
	}

	protected virtual bool ReparseXmlComment(ISyntaxNode node)
	{
		Reset(node.Position.Y, node.Position.X, 0);
		node.Clear();
		MoveNext();
		ParseXmlComment(node);
		comments.Clear();
		return true;
	}

	protected virtual bool ReparseUsingList(ISyntaxNode node)
	{
		Reset(node.Position.Y, node.Position.X, 0);
		node.Clear();
		MoveNext();
		ParseUsingList(node);
		return true;
	}

	protected virtual bool ReparseUsing(ISyntaxNode node)
	{
		Reset(node.Position.Y, node.Position.X, 0);
		node.Clear();
		MoveNext();
		ParseUsingDeclaration(node);
		return true;
	}

	protected virtual bool ReparseDeclaration(ISyntaxNode node)
	{
		Reset(node.Position.Y, node.Position.X, 0);
		node.Clear();
		MoveNext();
		ParseDeclaration(node);
		return true;
	}

	protected virtual ISyntaxNode GetBlockNode(ISyntaxNode node, Point position)
	{
		ISyntaxNode syntaxNode = NETRepository.GetBlockNode(node);
		while (syntaxNode != null && syntaxNode.FindAttribute(SyntaxConsts.SuppressReparsingScope) != null)
		{
			node = NETRepository.GetBlockNode(syntaxNode.Parent);
			if (node == null)
			{
				break;
			}
			syntaxNode = node;
		}
		return syntaxNode;
	}

	protected virtual bool ReparseBlock(ISyntaxNode node, Point position)
	{
		ISyntaxAttribute syntaxAttribute = node.FindAttribute(SyntaxConsts.BlockScope);
		if (syntaxAttribute != null)
		{
			Reset(syntaxAttribute.Position.Y, syntaxAttribute.Position.X, 0);
			node.Clear();
			base.SyntaxTree.Push(node);
			try
			{
				MoveNext();
				ParseBlock();
				if (node.Parent != null)
				{
					syntaxAttribute = node.FindAttribute(SyntaxConsts.DefinitionScope);
					if (syntaxAttribute != null)
					{
						ISyntaxAttribute syntaxAttribute2 = node.Parent.FindAttribute(SyntaxConsts.DefinitionScope);
						if (syntaxAttribute2 != null)
						{
							syntaxAttribute2.Position = syntaxAttribute.Position;
						}
					}
					syntaxAttribute = node.FindAttribute(SyntaxConsts.DefinitionScopeEnd);
					if (syntaxAttribute != null)
					{
						ISyntaxAttribute syntaxAttribute2 = node.Parent.FindAttribute(SyntaxConsts.DefinitionScopeEnd);
						if (syntaxAttribute2 != null)
						{
							syntaxAttribute2.Position = syntaxAttribute.Position;
						}
					}
				}
			}
			finally
			{
				base.SyntaxTree.Pop();
			}
			return true;
		}
		return false;
	}

	public override ISyntaxNode GetBlock(Point position, out Point blockPt)
	{
		blockPt = Point.Empty;
		ISyntaxNode syntaxNode = base.SyntaxTree.FindNode(position, pointNodeComparer);
		ISyntaxNode syntaxNode2 = ((syntaxNode != null) ? NETRepository.GetBlockNode(syntaxNode) : null);
		if (syntaxNode2 != null)
		{
			blockPt = syntaxNode2.Position;
			blockPt.X++;
			return syntaxNode2.Parent;
		}
		syntaxNode2 = ((syntaxNode != null) ? NETRepository.GetDeclarationNode(syntaxNode) : null);
		if (syntaxNode2 != null)
		{
			ISyntaxAttribute syntaxAttribute = syntaxNode2.FindAttribute(SyntaxConsts.DefinitionScope);
			blockPt = syntaxAttribute?.Position ?? syntaxNode2.Position;
			if (syntaxAttribute != null)
			{
				blockPt.X++;
			}
			return syntaxNode;
		}
		return null;
	}

	public override bool IsDeclaration(ISyntaxNode node)
	{
		if (node != null)
		{
			return NETRepository.IsDeclarationNode(node);
		}
		return false;
	}

	public override bool IsContentDivider(int index)
	{
		ISyntaxNode syntaxNode = base.SyntaxTree.Root;
		while (syntaxNode != null && (syntaxNode.Position.Y != index || syntaxNode == base.SyntaxTree.Root))
		{
			syntaxNode = syntaxNode.FindNode(index, lineNodeComparer);
			if (syntaxNode != null && syntaxNode.Range.EndPoint.Y == index && IsContentDivider(syntaxNode))
			{
				return true;
			}
		}
		return false;
	}

	public override void SaveState()
	{
		base.SaveState();
		savePrevPosition = prevPosition;
	}

	public override void RestoreState(bool restore)
	{
		base.RestoreState(restore);
		if (restore)
		{
			prevPosition = savePrevPosition;
		}
	}

	public override void Reset()
	{
		base.Reset();
		comments.Clear();
	}

	public override void ReparseText()
	{
		Reset();
		MoveNext();
		ParseUnit();
		FixupComments();
		base.ReparseText();
	}

	public override bool ReparseBlock(Point position)
	{
		bool result = false;
		comments.Clear();
		ISyntaxNode node = base.SyntaxTree.FindNode(position, pointNodeComparer);
		if (node != null)
		{
			if (NETRepository.IsXmlCommentNode(node))
			{
				node = NETRepository.GetXmlCommentNode(node);
				result = node != null && ReparseXmlComment(node);
			}
			else if (node.NodeType == 3)
			{
				result = ReparseUsing(node);
			}
			else if (node.NodeType == 2)
			{
				result = ReparseUsingList(node);
			}
			else if (NETRepository.IsDeclarationNode(ref node, checkRegions: true))
			{
				result = ReparseDeclaration(node);
			}
			else if (node.NodeType == 1)
			{
				ISyntaxNode syntaxNode = node.FindNode(2);
				if (syntaxNode != null)
				{
					result = ReparseUsingList(syntaxNode);
				}
			}
			else
			{
				node = GetBlockNode(node, position);
				result = node != null && ReparseBlock(node, position);
			}
		}
		FixupComments();
		return result;
	}

	public override void ResetOptions()
	{
		Options = SyntaxConsts.DefaultNetSyntaxOptions;
	}

	public override void ResetCodeCompletionChars()
	{
		base.CodeCompletionChars = SyntaxConsts.DefaultNetCodeCompletionChars.ToCharArray();
	}

	public override void ResetCodeCompletionStopChars()
	{
		base.CodeCompletionStopChars = SyntaxConsts.DefaultNetCodeCompletionStopChars.ToCharArray();
	}

	public override void ResetAutoIndentChars()
	{
		base.AutoIndentChars = SyntaxConsts.DefaultNetAutoIndentChars.ToCharArray();
	}

	public override bool ShouldSerializeCodeCompletionChars()
	{
		return !IsEqualArrays(base.CodeCompletionChars, SyntaxConsts.DefaultNetCodeCompletionChars.ToCharArray());
	}

	public override bool ShouldSerializeCodeCompletionStopChars()
	{
		return !IsEqualArrays(base.CodeCompletionStopChars, SyntaxConsts.DefaultNetCodeCompletionStopChars.ToCharArray());
	}

	public override bool ShouldSerializeAutoIndentChars()
	{
		return !IsEqualArrays(base.AutoIndentChars, SyntaxConsts.DefaultNetAutoIndentChars.ToCharArray());
	}
}
