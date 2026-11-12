using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class NETRepository : CodeCompletionRepository
{
	public enum BoolEnum
	{
		True,
		False
	}

	public class NamespaceComparer : IComparer
	{
		private ICodeCompletionRepository owner;

		public NamespaceComparer(ICodeCompletionRepository owner)
		{
			this.owner = owner;
		}

		public int Compare(object x, object y)
		{
			return string.Compare(((INetNamespace)x).Namespace, ((INetNamespace)y).Namespace, !owner.CaseSensitive);
		}
	}

	protected internal const int cEnumMemberIndex = 1;

	protected internal const int cNamespaceIndex = 2;

	protected internal const int cClassIndex = 3;

	protected internal const int cConstIndex = 4;

	protected internal const int cDelegateIndex = 5;

	protected internal const int cEnumIndex = 6;

	protected internal const int cEventIndex = 7;

	protected internal const int cFieldIndex = 8;

	protected internal const int cInterfaceIndex = 9;

	protected internal const int cMethodIndex = 10;

	protected internal const int cPropertyIndex = 11;

	protected internal const int cStructIndex = 12;

	protected internal const int cTypeIndex = 3;

	protected internal const int cXmlIndex = 40;

	protected internal const int cSnippetStatementIndex = 43;

	protected internal const int cSnippetIndex = 44;

	protected internal const int cImageDelta = 10;

	private ArrayList namespaces;

	private Hashtable objects;

	private Hashtable internalObjects;

	private IComparer namespaceComparer;

	private ImageList internalImages;

	private Hashtable codeSnippets;

	private bool allowGlobalMembers = true;

	private string baseClassType = "object";

	public IList Namespaces => namespaces;

	public Hashtable Objects => objects;

	public ImageList Images => internalImages;

	public bool AllowGlobalMembers
	{
		get
		{
			return allowGlobalMembers;
		}
		set
		{
			allowGlobalMembers = value;
		}
	}

	public string BaseClassType
	{
		get
		{
			return baseClassType;
		}
		set
		{
			baseClassType = value;
		}
	}

	public NETRepository(bool caseSensitive, ISyntaxTree syntaxTree)
		: base(caseSensitive, syntaxTree)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		objects = new Hashtable();
		internalObjects = new Hashtable();
		namespaces = new ArrayList();
		codeSnippets = new Hashtable();
		namespaceComparer = new NamespaceComparer(this);
		internalImages = new ImageList();
		internalImages.ImageSize = new Size(15, 15);
		try
		{
			ResourceManager resourceManager = new ResourceManager(typeof(NetSyntaxParser));
			_003F val = internalImages;
			object? @object = resourceManager.GetObject("NETImages.ImageStream");
			((ImageList)val).ImageStream = (ImageListStreamer)((@object is ImageListStreamer) ? @object : null);
		}
		catch
		{
		}
		internalImages.TransparentColor = SyntaxConsts.DefaultTransparentColor;
	}

	protected virtual ISyntaxNode GetQualifiedNode(ISyntaxNode root, string nspace, string name, Point position)
	{
		ISyntaxNode namespaceNode = GetNamespaceNode(root, nspace);
		if (namespaceNode == null)
		{
			return null;
		}
		return GetChildNode(null, namespaceNode, name, position, declarations: true, baseTypes: false);
	}

	protected virtual ISyntaxNode GetDeclaration(ISyntaxNode node, Point position, string name)
	{
		ISyntaxNode syntaxNode = null;
		ISyntaxNode root = node.Root;
		int num = name.LastIndexOf(".");
		if (num >= 0)
		{
			string nspace = ((num >= 0) ? name.Substring(0, num) : string.Empty);
			name = ((num >= 0) ? name.Substring(num + 1) : name);
			ISyntaxNode qualifiedNode = GetQualifiedNode(root, nspace, name, position);
			if (qualifiedNode != null)
			{
				return qualifiedNode;
			}
			foreach (ISyntaxTree syntaxTree3 in base.SyntaxTrees)
			{
				qualifiedNode = GetQualifiedNode(syntaxTree3.Root, nspace, name, new Point(int.MaxValue, int.MaxValue));
				if (qualifiedNode != null)
				{
					return qualifiedNode;
				}
			}
			return null;
		}
		ISyntaxNode declarationNode = GetDeclarationNode(node);
		if (declarationNode != null && string.Compare(declarationNode.Name, name, !base.CaseSensitive) == 0)
		{
			return declarationNode;
		}
		while (declarationNode != null)
		{
			if (declarationNode.NodeType == 7)
			{
				syntaxNode = declarationNode;
			}
			ISyntaxNode childNode = GetChildNode(null, declarationNode, name, position, declarations: true, baseTypes: false);
			if (childNode != null)
			{
				return childNode;
			}
			declarationNode = GetDeclarationNode(declarationNode.Parent);
		}
		foreach (ISyntaxTree syntaxTree4 in base.SyntaxTrees)
		{
			ISyntaxNode syntaxNode2 = ((syntaxNode != null) ? GetNamespaceNode(syntaxTree4.Root, syntaxNode.Name) : syntaxTree4.Root);
			if (syntaxNode2 != null)
			{
				syntaxNode2 = GetChildNode(null, syntaxNode2, name, new Point(int.MaxValue, int.MaxValue), declarations: true, baseTypes: false);
				if (syntaxNode2 != null)
				{
					return syntaxNode2;
				}
			}
			foreach (INetNamespace @namespace in GetNamespaces(node, system: false))
			{
				syntaxNode2 = GetNamespaceNode(syntaxTree4.Root, @namespace.Namespace);
				if (syntaxNode2 != null)
				{
					syntaxNode2 = GetChildNode(null, syntaxNode2, name, new Point(int.MaxValue, int.MaxValue), declarations: true, baseTypes: false);
					if (syntaxNode2 != null)
					{
						return syntaxNode2;
					}
				}
			}
		}
		return null;
	}

	protected virtual IList GetSameNodes(ISyntaxNode node)
	{
		string name = node.Name;
		node = node.Parent;
		while (node != null && node.NodeType == 39)
		{
			node = node.Parent;
		}
		ISyntaxNodes syntaxNodes = new SyntaxNodes();
		GetChildNode(syntaxNodes, (node != null) ? node : base.SyntaxTree.Root, name, new Point(int.MaxValue, int.MaxValue), declarations: true, baseTypes: false);
		return syntaxNodes;
	}

	protected virtual ISyntaxNode GetDeclaration(ISyntaxTree tree, ISyntaxNode node)
	{
		ISyntaxNode syntaxNode = node;
		while (syntaxNode != null && syntaxNode.NodeType != 7)
		{
			syntaxNode = syntaxNode.Parent;
		}
		string name = node.Name;
		node = ((syntaxNode != null) ? GetNamespaceNode(tree.Root, syntaxNode.Name) : tree.Root);
		if (node == null)
		{
			return null;
		}
		return GetChildNode(null, node, name, new Point(int.MaxValue, int.MaxValue), declarations: true, baseTypes: false);
	}

	protected virtual ISyntaxNode GetChildNode(ISyntaxNodes nodes, ISyntaxNode node, string name, Point position, bool declarations, bool baseTypes)
	{
		return GetChildNode(nodes, null, node, name, position, declarations, baseTypes);
	}

	protected virtual ISyntaxNode GetChildNode(ISyntaxNodes nodes, IList types, ISyntaxNode node, string name, Point position, bool declarations, bool baseTypes)
	{
		if (node.HasChildren)
		{
			foreach (ISyntaxNode child in node.ChildList)
			{
				ISyntaxNode syntaxNode2 = ((child.NodeType == 39) ? GetChildNode(null, child, name, position, declarations, baseTypes) : GetChildByName(child, name, position, declarations, recursive: false));
				if (syntaxNode2 != null)
				{
					if (nodes == null)
					{
						return syntaxNode2;
					}
					nodes.Add(syntaxNode2);
				}
			}
		}
		if ((declarations || baseTypes) && IsDeclarationNode(node) && node.Parent != null)
		{
			string baseType = GetBaseType(node);
			if (baseType != string.Empty)
			{
				if (types == null)
				{
					types = new ArrayList();
				}
				if (types.IndexOf(baseType) >= 0)
				{
					return null;
				}
				types.Add(baseType);
				StaticScope scope;
				object memberType = GetMemberType(node.Parent, node.Parent.Position, null, baseType, out scope);
				if (memberType is ISyntaxNode)
				{
					ISyntaxNode childNode = GetChildNode(null, (ISyntaxNode)memberType, name, position, declarations, baseTypes);
					if (nodes == null)
					{
						return childNode;
					}
					if (childNode != null)
					{
						nodes.Add(childNode);
					}
				}
			}
		}
		if (nodes == null || nodes.Count <= 0)
		{
			return null;
		}
		return nodes[0];
	}

	protected virtual ISyntaxNode GetChildByName(ISyntaxNode node, string name, Point position, bool declarations, bool recursive)
	{
		if ((node.Options & SyntaxNodeOptions.CodeCompletion) != 0 && string.Compare(node.Name, name, !base.CaseSensitive) == 0 && (!declarations || IsDeclarationNode(node)))
		{
			return node;
		}
		if (recursive && node.HasChildren)
		{
			foreach (ISyntaxNode child in node.ChildList)
			{
				if (declarations || (child.FindAttribute(SyntaxConsts.BlockScope) == null && (child.Position.Y < position.Y || (child.Position.Y == position.Y && child.Position.X < position.X))))
				{
					ISyntaxNode childByName = GetChildByName(child, name, position, declarations, recursive: true);
					if (childByName != null)
					{
						return childByName;
					}
				}
			}
		}
		return null;
	}

	protected virtual ISyntaxNode GetNamespaceNode(ISyntaxNode node, string name)
	{
		if (node == null)
		{
			return null;
		}
		if (node.NodeType == 7 && string.Compare(node.Name, name, !base.CaseSensitive) == 0)
		{
			return node;
		}
		if (node.HasChildren)
		{
			foreach (ISyntaxNode child in node.ChildList)
			{
				if (child.NodeType == 39 || child.NodeType == 7 || child.NodeType == 1)
				{
					ISyntaxNode namespaceNode = GetNamespaceNode(child, name);
					if (namespaceNode != null)
					{
						return namespaceNode;
					}
				}
			}
		}
		return null;
	}

	protected virtual ISyntaxNode GetNodeByName(ISyntaxNode node, string name, Point position)
	{
		ISyntaxNode declarationNode = GetDeclarationNode(node);
		ISyntaxNode syntaxNode = null;
		while (node != null && node != declarationNode)
		{
			syntaxNode = GetChildByName(node, name, position, declarations: false, recursive: true);
			if (syntaxNode != null)
			{
				return syntaxNode;
			}
			node = node.Parent;
		}
		if (declarationNode != null)
		{
			syntaxNode = GetChildNode(null, declarationNode, name, position, declarations: false, baseTypes: true);
			if (syntaxNode != null)
			{
				return syntaxNode;
			}
			if (IsNodePartial(declarationNode))
			{
				foreach (ISyntaxNode sameNode in GetSameNodes(declarationNode))
				{
					if (sameNode != declarationNode && IsNodePartial(sameNode))
					{
						syntaxNode = GetChildNode(null, null, sameNode, name, new Point(int.MaxValue, int.MaxValue), declarations: false, baseTypes: true);
						if (syntaxNode != null)
						{
							return syntaxNode;
						}
					}
				}
				foreach (ISyntaxTree syntaxTree in base.SyntaxTrees)
				{
					ISyntaxNode declaration = GetDeclaration(syntaxTree, declarationNode);
					if (declaration != null && IsNodePartial(declaration))
					{
						syntaxNode = GetChildNode(null, declaration, name, new Point(int.MaxValue, int.MaxValue), declarations: false, baseTypes: true);
						if (syntaxNode != null)
						{
							return syntaxNode;
						}
					}
				}
			}
		}
		return syntaxNode;
	}

	protected virtual object GetQualifiedType(ISyntaxNode node, Point position, string name, out StaticScope scope)
	{
		scope = StaticScope.None;
		object obj = null;
		string[] array = name.Split(new char[1] { '.' });
		foreach (string name2 in array)
		{
			obj = GetMemberType(node, position, obj, name2, out scope);
			if (obj == null)
			{
				break;
			}
		}
		return obj;
	}

	protected virtual object GetChildType(ISyntaxNode node, Point position, out StaticScope scope)
	{
		scope = StaticScope.Instance;
		if (IsDeclarationNode(node))
		{
			scope = StaticScope.Static;
			return node;
		}
		object result = null;
		string nodeDataType = GetNodeDataType(node);
		if (nodeDataType != string.Empty)
		{
			result = GetQualifiedType(node, position, nodeDataType, out scope);
			scope = StaticScope.Instance;
		}
		return result;
	}

	protected virtual object GetChildType(IList types, Point position, ISyntaxNode node, string name, out StaticScope scope)
	{
		scope = StaticScope.Instance;
		if (node.HasChildren)
		{
			foreach (ISyntaxNode child in node.ChildList)
			{
				if ((child.Options & SyntaxNodeOptions.CodeCompletion) != 0 && string.Compare(child.Name, name, !base.CaseSensitive) == 0)
				{
					return GetChildType(child, position, out scope);
				}
			}
		}
		if (IsNodePartial(node))
		{
			foreach (ISyntaxNode sameNode in GetSameNodes(node))
			{
				if (sameNode == node || !IsNodePartial(sameNode) || !sameNode.HasChildren)
				{
					continue;
				}
				foreach (ISyntaxNode child2 in sameNode.ChildList)
				{
					if ((child2.Options & SyntaxNodeOptions.CodeCompletion) != 0 && string.Compare(child2.Name, name, !base.CaseSensitive) == 0)
					{
						return GetChildType(child2, child2.Position, out scope);
					}
				}
			}
			foreach (ISyntaxTree syntaxTree in base.SyntaxTrees)
			{
				ISyntaxNode declaration = GetDeclaration(syntaxTree, node);
				if (declaration == null || !IsNodePartial(declaration) || !declaration.HasChildren)
				{
					continue;
				}
				foreach (ISyntaxNode child3 in declaration.ChildList)
				{
					if ((child3.Options & SyntaxNodeOptions.CodeCompletion) != 0 && string.Compare(child3.Name, name, !base.CaseSensitive) == 0)
					{
						return GetChildType(child3, child3.Position, out scope);
					}
				}
			}
		}
		string baseType = GetBaseType(node);
		if (baseType != string.Empty)
		{
			if (types.IndexOf(baseType) >= 0)
			{
				return null;
			}
			types.Add(baseType);
			object memberType = GetMemberType(node, position, null, baseType, out scope);
			if (memberType != null)
			{
				return GetExpressionType(types, node, position, memberType, name, out scope);
			}
		}
		return null;
	}

	protected virtual string GetBaseType(ISyntaxNode node)
	{
		Point position = node.Position;
		return GetBaseType(node, ref position);
	}

	protected virtual string GetBaseType(ISyntaxNode node, ref Point position)
	{
		ISyntaxAttribute syntaxAttribute = node.FindAttribute(NetNodeType.TypeList.ToString());
		if (syntaxAttribute != null)
		{
			position = syntaxAttribute.Position;
			string text = (string)syntaxAttribute.Value;
			int num = text.IndexOf(",");
			if (num >= 0)
			{
				text = text.Substring(0, num).Trim();
			}
			return text;
		}
		if (node.NodeType == 8)
		{
			return baseClassType;
		}
		return string.Empty;
	}

	protected internal static bool IsDeclarationNode(ref ISyntaxNode node, bool checkRegions)
	{
		while (node != null)
		{
			NetNodeType nodeType = (NetNodeType)node.NodeType;
			if ((uint)(nodeType - 7) <= 5u)
			{
				return true;
			}
			if (!checkRegions || node.NodeType != 39)
			{
				break;
			}
			node = node.Parent;
		}
		return false;
	}

	public static bool IsBlockNode(ISyntaxNode node)
	{
		return node.FindAttribute(SyntaxConsts.BlockScope) != null;
	}

	public static bool IsMethodNode(ISyntaxNode node)
	{
		NetNodeType nodeType = (NetNodeType)node.NodeType;
		if ((uint)(nodeType - 17) <= 2u || nodeType == NetNodeType.Indexer)
		{
			return true;
		}
		return false;
	}

	public static bool IsDeclarationNode(ISyntaxNode node)
	{
		return IsDeclarationNode(ref node, checkRegions: false);
	}

	public static bool IsXmlCommentNode(ISyntaxNode node)
	{
		NetNodeType nodeType = (NetNodeType)node.NodeType;
		if ((uint)(nodeType - 55) <= 5u)
		{
			return true;
		}
		return false;
	}

	public static bool IsInvocationNode(ISyntaxNode node)
	{
		NetNodeType nodeType = (NetNodeType)node.NodeType;
		if ((uint)(nodeType - 145) <= 1u || nodeType == NetNodeType.ObjectCreationExpression || (uint)(nodeType - 156) <= 1u)
		{
			return true;
		}
		return false;
	}

	public static ISyntaxNode GetInvocationNode(string text, ISyntaxNode node, Point position, out int paramIndex, out int paramCount)
	{
		paramIndex = -1;
		paramCount = 0;
		while (node != null)
		{
			if (node.NodeType == 29 && (node.Position.Y < position.Y || (node.Position.Y == position.Y && node.Position.X <= position.X)) && (text == null || position.X <= 0 || position.X > text.Length || text[position.X - 1] != ')'))
			{
				paramIndex = 0;
				if (node.HasChildren)
				{
					foreach (ISyntaxNode child in node.ChildList)
					{
						if (child.NodeType == 28)
						{
							Point endPoint = child.Range.EndPoint;
							if (endPoint.Y < position.Y || (endPoint.Y == position.Y && endPoint.X < position.X))
							{
								paramIndex++;
							}
							paramCount++;
						}
					}
				}
			}
			if (IsInvocationNode(node))
			{
				return node;
			}
			node = node.Parent;
		}
		return null;
	}

	public static ISyntaxNode GetXmlCommentNode(ISyntaxNode node)
	{
		while (node != null)
		{
			if (node.NodeType == 55)
			{
				return node;
			}
			if (!IsXmlCommentNode(node))
			{
				break;
			}
			node = node.Parent;
		}
		return null;
	}

	public static ISyntaxNode GetDeclarationNode(ISyntaxNode node)
	{
		while (node != null)
		{
			if (IsDeclarationNode(node) || node.NodeType == 1)
			{
				return node;
			}
			node = node.Parent;
		}
		return null;
	}

	public static ISyntaxNode GetMethodNode(ISyntaxNode node)
	{
		while (node != null)
		{
			if (IsMethodNode(node))
			{
				return node;
			}
			node = node.Parent;
		}
		return null;
	}

	public static ISyntaxNode GetBlockOrMethodNode(ISyntaxNode node)
	{
		while (node != null)
		{
			if (IsMethodNode(node) || IsBlockNode(node))
			{
				return node;
			}
			node = node.Parent;
		}
		return null;
	}

	public static ISyntaxNode GetBlockNode(ISyntaxNode node)
	{
		while (node != null)
		{
			if (IsBlockNode(node))
			{
				return node;
			}
			node = node.Parent;
		}
		return null;
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

	public override string GetDescription(IListMembers members, ISyntaxNode node, object member, string name, bool fullDescription)
	{
		string text = base.GetDescription(members, node, member, name, fullDescription);
		if (member is ISyntaxNode && (text == null || text == string.Empty))
		{
			ISyntaxNode syntaxNode = (ISyntaxNode)member;
			if (fullDescription)
			{
				IListMember listMember2;
				if (members == null)
				{
					IListMember listMember = new ListMember();
					listMember2 = listMember;
				}
				else
				{
					listMember2 = members.CreateListMember();
				}
				IListMember listMember3 = listMember2;
				listMember3.Qualifier = "(" + ((NetNodeType)syntaxNode.NodeType).ToString() + ")";
				listMember3.DataType = GetNodeDataType(syntaxNode);
				listMember3.Name = syntaxNode.Name;
				listMember3.OwnDescription = true;
				text = listMember3.Description;
			}
			string paramName = string.Empty;
			string paramValue = string.Empty;
			if (syntaxNode.NodeType == 26)
			{
				paramName = SyntaxConsts.NameTag;
				paramValue = syntaxNode.Name;
				if (syntaxNode.Parent != null)
				{
					syntaxNode = syntaxNode.Parent.Parent;
				}
			}
			if (syntaxNode != null && syntaxNode.Parent != null)
			{
				int num = syntaxNode.Index - 1;
				if (num >= 0)
				{
					syntaxNode = syntaxNode.Parent.ChildList[num];
					if (syntaxNode.NodeType == 55)
					{
						syntaxNode = FindXmlNode(syntaxNode, name, paramName, paramValue);
						if (syntaxNode != null)
						{
							string text2 = GetXmlComment(syntaxNode).Trim();
							if (text == null || text == string.Empty)
							{
								text = text2;
							}
							else if (text2 != string.Empty)
							{
								text = text + SyntaxConsts.DefaultBrTag + text2;
							}
						}
					}
				}
			}
		}
		return text;
	}

	private int GetNodeImageIndex(ISyntaxNode node)
	{
		int num = -1;
		switch ((NetNodeType)node.NodeType)
		{
		case NetNodeType.Namespace:
			num = 2;
			break;
		case NetNodeType.Class:
			num = 3;
			break;
		case NetNodeType.Constant:
			num = 4;
			break;
		case NetNodeType.Delegate:
			num = 5;
			break;
		case NetNodeType.Enum:
			num = 6;
			break;
		case NetNodeType.Event:
			num = 7;
			break;
		case NetNodeType.Field:
			num = 8;
			break;
		case NetNodeType.Interface:
			num = 9;
			break;
		case NetNodeType.Method:
		case NetNodeType.Constructor:
		case NetNodeType.Destructor:
			num = 10;
			break;
		case NetNodeType.Property:
			num = 11;
			break;
		case NetNodeType.Struct:
			num = 12;
			break;
		case NetNodeType.LocalVariable:
		case NetNodeType.Parameter:
			num = 8;
			break;
		case NetNodeType.LocalConst:
			num = 4;
			break;
		}
		return num + GetScopeImageIndex(node);
	}

	private int GetScopeImageIndex(ISyntaxNode node)
	{
		NetNodeType nodeType = (NetNodeType)node.NodeType;
		if (nodeType == NetNodeType.LocalVariable || nodeType == NetNodeType.Parameter || nodeType == NetNodeType.LocalConst)
		{
			return 30;
		}
		ISyntaxAttribute syntaxAttribute = node.FindAttribute(NetNodeType.Modifier.ToString());
		if (syntaxAttribute != null)
		{
			switch (((string)syntaxAttribute.Value).ToLower(CultureInfo.InvariantCulture))
			{
			case "private":
				return 10;
			case "protected":
				return 20;
			case "public":
				return 30;
			}
		}
		return 0;
	}

	protected virtual string GetNodeQualifier(ISyntaxNode node)
	{
		string text = string.Empty;
		if (node.HasAttributes)
		{
			foreach (ISyntaxAttribute attribute in node.AttributeList)
			{
				if (attribute.Name == NetNodeType.Modifier.ToString() || attribute.Name == NetNodeType.ParameterModifier.ToString())
				{
					text = text + " " + (string)attribute.Value;
				}
			}
		}
		string text2 = string.Empty;
		switch ((NetNodeType)node.NodeType)
		{
		case NetNodeType.Class:
			text2 = SyntaxConsts.ClassDataType;
			break;
		case NetNodeType.Enum:
			text2 = SyntaxConsts.EnumDataType;
			break;
		case NetNodeType.Interface:
			text2 = SyntaxConsts.InterfaceDataType;
			break;
		case NetNodeType.Struct:
			text2 = SyntaxConsts.StructDataType;
			break;
		}
		text = text.Trim() + " " + text2;
		return text.Trim();
	}

	protected virtual string GetNodeDataType(ISyntaxNode node)
	{
		Point position = node.Position;
		return GetNodeDataType(node, ref position);
	}

	protected virtual string GetNodeDataType(ISyntaxNode node, ref Point position)
	{
		ISyntaxAttribute syntaxAttribute = node.FindAttribute(NetNodeType.Type.ToString());
		if (syntaxAttribute != null)
		{
			position = syntaxAttribute.Position;
			if (node.FindAttribute(NetNodeType.ArraySpecifier.ToString()) == null)
			{
				return (string)syntaxAttribute.Value;
			}
			return (string)syntaxAttribute.Value + "[]";
		}
		return string.Empty;
	}

	protected virtual ISyntaxNode GetValidNode(ISyntaxNode node)
	{
		while (node.Parent != null && node.NodeType == 39)
		{
			node = node.Parent;
		}
		return node;
	}

	protected virtual StaticScope GetNodeStaticScope(ISyntaxNode node)
	{
		if (IsDeclarationNode(node))
		{
			return StaticScope.Static;
		}
		ISyntaxNode validNode = GetValidNode(node.Parent);
		if (validNode != null)
		{
			if (node.NodeType == 13 && validNode.NodeType == 11)
			{
				return StaticScope.Static;
			}
			if ((node.Options & SyntaxNodeOptions.CodeCompletion) != 0 && validNode.NodeType == 12)
			{
				return StaticScope.Static;
			}
		}
		if (node.HasAttributes)
		{
			foreach (ISyntaxAttribute attribute in node.AttributeList)
			{
				if (attribute.Name == NetNodeType.Modifier.ToString() && ((string)attribute.Value == "static" || string.Compare((string)attribute.Value, "shared", ignoreCase: true) == 0))
				{
					return StaticScope.Static;
				}
			}
		}
		return StaticScope.Instance;
	}

	protected virtual bool IsNodePartial(ISyntaxNode node)
	{
		if (node.HasAttributes)
		{
			foreach (ISyntaxAttribute attribute in node.AttributeList)
			{
				if (attribute.Name == NetNodeType.Modifier.ToString() && string.Compare((string)attribute.Value, "partial", !base.CaseSensitive) == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	protected virtual object FindMember(string text, Point position, ISyntaxNode node, string name, int paramCount)
	{
		return DoFindDeclaration(text, node, position, paramCount);
	}

	protected virtual object GetParameterType(object member, int param)
	{
		if (member is ISyntaxNode)
		{
			IParameterMember[] nodeParameters = GetNodeParameters((ISyntaxNode)member);
			StaticScope scope;
			if (nodeParameters != null && param < nodeParameters.Length)
			{
				return GetQualifiedType((ISyntaxNode)member, ((ISyntaxNode)member).Position, nodeParameters[param].DataType, out scope);
			}
		}
		return null;
	}

	protected virtual IParameterMember[] GetNodeParameters(ISyntaxNode node)
	{
		IParameterMember[] array = null;
		ISyntaxNode syntaxNode = node.FindNode(27);
		if (syntaxNode != null)
		{
			int num = 0;
			if (syntaxNode.HasChildren)
			{
				foreach (ISyntaxNode child in syntaxNode.ChildList)
				{
					if (child.NodeType == 26)
					{
						num++;
					}
				}
			}
			IParameterMember[] array2 = new ParameterMember[num];
			array = array2;
			num = 0;
			if (syntaxNode.HasChildren)
			{
				foreach (ISyntaxNode child2 in syntaxNode.ChildList)
				{
					if (child2.NodeType == 26)
					{
						IParameterMember parameterMember = new ParameterMember();
						parameterMember.Name = child2.Name;
						parameterMember.DataType = GetNodeDataType(child2);
						parameterMember.Qualifier = GetNodeQualifier(child2);
						array[num] = parameterMember;
						parameterMember.Description = GetDescription(null, node, child2, XmlCommentType.Param.ToString(), fullDescription: false);
						num++;
					}
				}
			}
		}
		return array;
	}

	protected virtual void FillXmlNodeMembers(IListMembers members, ISyntaxNode node, Point position, string filter, ref int selIndex)
	{
		if (node != null)
		{
			ISyntaxAttribute syntaxAttribute = node.FindAttribute(XmlLexerToken.CloseTag.ToString());
			if (syntaxAttribute != null && new Point(syntaxAttribute.Position.X + 1, syntaxAttribute.Position.Y).Equals((object?)position))
			{
				if (node.FindAttribute(XmlLexerToken.OpenEndTag.ToString()) == null)
				{
					string name = node.Name;
					if ((filter == string.Empty || string.Compare(filter, name, !base.CaseSensitive) == 0) && CanAddMember(members, name, 40, overloads: false))
					{
						string text = (members.UseHtmlFormatting ? ("&lt;/" + name + "&gt;") : ("</" + name + ">"));
						IListMember listMember = AddMember(members, "|" + text, 40, showDesc: false);
						selIndex = members.Count - 1;
						listMember.DisplayText = text;
					}
				}
				return;
			}
		}
		string[] names = Enum.GetNames(typeof(XmlCommentType));
		foreach (string text2 in names)
		{
			string text3 = text2.ToLower(CultureInfo.InvariantCulture);
			if ((filter == string.Empty || string.Compare(filter, text3, !base.CaseSensitive) == 0) && CanAddMember(members, text3, 40, overloads: false))
			{
				IListMember listMember2 = AddMember(members, text3, 40, showDesc: false);
				listMember2.DisplayText = (members.UseHtmlFormatting ? ("&lt;" + text3 + "&gt;") : ("<" + text3 + ">"));
			}
		}
	}

	protected virtual void FillMembers(IListMembers members, ISyntaxNode node, Point position, string filter, StaticScope scope, bool recursive)
	{
		FillMember(members, node, filter, scope);
		if ((!recursive && node.NodeType != 39) || !node.HasChildren)
		{
			return;
		}
		foreach (ISyntaxNode child in node.ChildList)
		{
			if (child.FindAttribute(SyntaxConsts.BlockScope) == null && (child.Position.Y < position.Y || (child.Position.Y == position.Y && child.Position.X < position.X)))
			{
				FillMembers(members, child, position, filter, scope, recursive);
			}
		}
	}

	protected virtual bool IsEmbeddedDeclaration(ISyntaxNode node)
	{
		switch ((NetNodeType)node.NodeType)
		{
		case NetNodeType.Constant:
		case NetNodeType.LocalVariable:
		case NetNodeType.FixedVariable:
		case NetNodeType.Parameter:
		case NetNodeType.ParameterList:
		case NetNodeType.ForInitializerStatement:
		case NetNodeType.ForEachInitializerStatement:
			return true;
		default:
			return false;
		}
	}

	protected virtual void FillDeclarations(IListMembers members, ISyntaxNode node, Point position, string filter, StaticScope scope)
	{
		if (!node.HasChildren)
		{
			return;
		}
		foreach (ISyntaxNode child in node.ChildList)
		{
			if (IsEmbeddedDeclaration(child))
			{
				FillMembers(members, child, position, filter, scope, recursive: true);
			}
		}
	}

	protected virtual object FindDeclarations(ISyntaxNode node, Point position, string name, StaticScope scope)
	{
		object obj = null;
		if (node.HasChildren)
		{
			foreach (ISyntaxNode child in node.ChildList)
			{
				if (IsEmbeddedDeclaration(child))
				{
					obj = FindDeclaration(child, position, name, scope, recursive: true);
					if (obj != null)
					{
						break;
					}
				}
			}
		}
		return obj;
	}

	protected virtual void AddUnitMember(IListMembers members, ISyntaxNode node, string filter, StaticScope scope)
	{
		if (IsDeclarationNode(node) && node.NodeType != 7)
		{
			FillMember(members, node, filter, scope);
		}
		int nodeType = node.NodeType;
		if ((nodeType != 1 && nodeType != 7 && nodeType != 39) || !node.HasChildren)
		{
			return;
		}
		foreach (ISyntaxNode child in node.ChildList)
		{
			AddUnitMember(members, child, filter, scope);
		}
	}

	protected virtual void FillBaseTypeMembers(IListMembers members, IList types, ISyntaxNode node, string filter, StaticScope scope, ref int selIndex)
	{
		string baseType = GetBaseType(node);
		if (!(baseType != string.Empty))
		{
			return;
		}
		StaticScope scope2;
		object memberType = GetMemberType(node, node.Position, null, baseType, out scope2);
		if (memberType != null)
		{
			if (filter == string.Empty)
			{
				DoFillMembers(node, node.Position, members, types, memberType, filter, scope & ~StaticScope.Private, ref selIndex);
			}
			else
			{
				FillMember(members, memberType, filter, scope & ~StaticScope.Private);
			}
		}
	}

	protected virtual void FillMembers(IListMembers members, IList types, ISyntaxNode node, Point position, string filter, StaticScope scope, ref int selIndex)
	{
		if (IsXmlCommentNode(node))
		{
			FillXmlNodeMembers(members, node, position, filter, ref selIndex);
			return;
		}
		ISyntaxNode syntaxNode = GetDeclarationNode(node);
		ISyntaxNode syntaxNode2 = node;
		while (node != null && !IsDeclarationNode(node))
		{
			if (node.NodeType != 39)
			{
				FillMembers(members, node, position, filter, scope, recursive: false);
				FillDeclarations(members, node, position, filter, scope);
			}
			syntaxNode2 = node;
			node = node.Parent;
		}
		while (syntaxNode != null)
		{
			if (types.IndexOf(syntaxNode) < 0)
			{
				types.Add(syntaxNode);
				if (syntaxNode.HasChildren)
				{
					bool flag = IsBlockNode(syntaxNode);
					foreach (ISyntaxNode child in syntaxNode.ChildList)
					{
						if (child != syntaxNode2 && (!flag || child.Position.Y < position.Y || (child.Position.Y == position.Y && child.Position.X < position.X)))
						{
							FillMembers(members, child, new Point(int.MaxValue, int.MaxValue), filter, scope, recursive: false);
						}
					}
				}
				if (IsNodePartial(syntaxNode))
				{
					foreach (ISyntaxNode sameNode in GetSameNodes(syntaxNode))
					{
						if (sameNode == syntaxNode || !IsNodePartial(sameNode) || !sameNode.HasChildren)
						{
							continue;
						}
						foreach (ISyntaxNode child2 in sameNode.ChildList)
						{
							FillMembers(members, child2, new Point(int.MaxValue, int.MaxValue), filter, scope, recursive: false);
						}
						FillBaseTypeMembers(members, types, sameNode, filter, scope, ref selIndex);
					}
					foreach (ISyntaxTree syntaxTree in base.SyntaxTrees)
					{
						ISyntaxNode declaration = GetDeclaration(syntaxTree, syntaxNode);
						if (declaration == null || !IsNodePartial(declaration) || !declaration.HasChildren)
						{
							continue;
						}
						foreach (ISyntaxNode child3 in declaration.ChildList)
						{
							FillMembers(members, child3, new Point(int.MaxValue, int.MaxValue), filter, scope, recursive: false);
						}
						FillBaseTypeMembers(members, types, declaration, filter, scope, ref selIndex);
					}
				}
				FillBaseTypeMembers(members, types, syntaxNode, filter, scope, ref selIndex);
			}
			if (node != syntaxNode)
			{
				syntaxNode2 = null;
				syntaxNode = syntaxNode.Parent;
				continue;
			}
			break;
		}
	}

	protected virtual ISyntaxNode FindUnitMember(ISyntaxNode node, string name, StaticScope scope)
	{
		ISyntaxNode syntaxNode = null;
		if (IsDeclarationNode(node) && name != string.Empty && CanFilter(null, node, name) && IsScopeValid(scope, node))
		{
			return node;
		}
		int nodeType = node.NodeType;
		if ((nodeType == 1 || nodeType == 7 || nodeType == 39) && node.HasChildren)
		{
			foreach (ISyntaxNode child in node.ChildList)
			{
				syntaxNode = FindUnitMember(child, name, scope);
				if (syntaxNode != null)
				{
					return syntaxNode;
				}
			}
		}
		return null;
	}

	protected virtual bool IsScopeValid(StaticScope scope, ISyntaxNode node)
	{
		bool flag = false;
		flag = ((scope & StaticScope.Instance) != 0 && (scope & StaticScope.Static) != 0) || (GetNodeStaticScope(node) & scope) != 0;
		if (flag && (scope & StaticScope.Private) == 0 && GetBlockNode(node) == null && GetMethodNode(node) == null)
		{
			ISyntaxNode declarationNode = GetDeclarationNode(node);
			if (declarationNode != null && declarationNode.Parent != null)
			{
				NetNodeType nodeType = (NetNodeType)declarationNode.NodeType;
				if (nodeType == NetNodeType.Class || nodeType == NetNodeType.Module)
				{
					flag = false;
					if (node.HasAttributes)
					{
						foreach (ISyntaxAttribute attribute in node.AttributeList)
						{
							if (attribute.Name == NetNodeType.Modifier.ToString())
							{
								if (string.Compare((string)attribute.Value, "public", ignoreCase: true) == 0)
								{
									flag = true;
								}
								else if (string.Compare((string)attribute.Value, "protected", ignoreCase: true) == 0)
								{
									flag = (scope & StaticScope.Protected) != 0;
								}
								if (flag)
								{
									break;
								}
							}
						}
					}
				}
			}
		}
		return flag;
	}

	protected virtual object FindDeclaration(ISyntaxNode node, Point position, string name, StaticScope scope, bool recursive)
	{
		if (name != string.Empty && CanFilter(null, node, name) && IsScopeValid(scope, node))
		{
			return node;
		}
		if ((recursive || node.NodeType == 39) && node.HasChildren)
		{
			foreach (ISyntaxNode child in node.ChildList)
			{
				if (child.FindAttribute(SyntaxConsts.BlockScope) == null && (child.Position.Y < position.Y || (child.Position.Y == position.Y && child.Position.X < position.X)))
				{
					object obj = FindDeclaration(child, position, name, scope, recursive);
					if (obj != null)
					{
						return obj;
					}
				}
			}
		}
		return null;
	}

	protected virtual object FindDeclaration(object member, IList types, Point position, string name, StaticScope scope, int paramCount)
	{
		object obj = null;
		ISyntaxNode syntaxNode = ((member is ISyntaxNode) ? ((ISyntaxNode)member) : null);
		if (syntaxNode == null)
		{
			return obj;
		}
		ISyntaxNode syntaxNode2 = GetDeclarationNode(syntaxNode);
		ISyntaxNode syntaxNode3 = syntaxNode;
		while (syntaxNode != null && !IsDeclarationNode(syntaxNode))
		{
			obj = FindDeclaration(syntaxNode, position, name, scope, recursive: false);
			if (obj != null)
			{
				return obj;
			}
			obj = FindDeclarations(syntaxNode, position, name, scope);
			if (obj != null)
			{
				return obj;
			}
			syntaxNode3 = syntaxNode;
			syntaxNode = syntaxNode.Parent;
		}
		while (syntaxNode2 != null)
		{
			if (types.IndexOf(syntaxNode2) < 0)
			{
				types.Add(syntaxNode2);
				if (syntaxNode2.HasChildren)
				{
					bool flag = IsBlockNode(syntaxNode2);
					foreach (ISyntaxNode child in syntaxNode2.ChildList)
					{
						if (child == syntaxNode3 || (flag && child.Position.Y >= position.Y && (child.Position.Y != position.Y || child.Position.X >= position.X)))
						{
							continue;
						}
						object obj2 = FindDeclaration(child, new Point(int.MaxValue, int.MaxValue), name, scope, recursive: false);
						if (obj2 != null)
						{
							obj = obj2;
							paramCount--;
							if (paramCount <= 0)
							{
								return obj;
							}
						}
					}
					if (obj != null)
					{
						return obj;
					}
				}
				string baseType = GetBaseType(syntaxNode2);
				if (baseType != string.Empty)
				{
					StaticScope scope2;
					object memberType = GetMemberType(syntaxNode2, syntaxNode2.Position, null, baseType, out scope2);
					if (memberType != null)
					{
						obj = FindDeclaration(memberType, types, new Point(int.MaxValue, int.MaxValue), name, scope & ~StaticScope.Private, paramCount);
						if (obj != null)
						{
							return obj;
						}
					}
				}
			}
			if (syntaxNode == syntaxNode2)
			{
				break;
			}
			syntaxNode3 = null;
			syntaxNode2 = syntaxNode2.Parent;
		}
		foreach (ISyntaxTree syntaxTree in base.SyntaxTrees)
		{
			obj = FindUnitMember(syntaxTree.Root, name, scope);
		}
		return obj;
	}

	protected virtual bool IsDeclarationReference(ISyntaxNode node)
	{
		if (IsDeclarationNode(node))
		{
			return true;
		}
		switch ((NetNodeType)node.NodeType)
		{
		case NetNodeType.Using:
		case NetNodeType.Field:
		case NetNodeType.Constant:
		case NetNodeType.LocalVariable:
		case NetNodeType.FixedVariable:
		case NetNodeType.Method:
		case NetNodeType.Constructor:
		case NetNodeType.Destructor:
		case NetNodeType.Delegate:
		case NetNodeType.Property:
		case NetNodeType.Event:
		case NetNodeType.Operator:
		case NetNodeType.Indexer:
		case NetNodeType.Parameter:
		case NetNodeType.Attribute:
		case NetNodeType.LocalConst:
			return true;
		default:
			return false;
		}
	}

	protected virtual bool IsReferenceType(ISyntaxNode node)
	{
		if (IsDeclarationReference(node))
		{
			return true;
		}
		switch ((NetNodeType)node.NodeType)
		{
		case NetNodeType.OperatorType:
		case NetNodeType.Argument:
		case NetNodeType.AttributeTarget:
		case NetNodeType.AsIsExpression:
		case NetNodeType.ObjectCreationExpression:
		case NetNodeType.MemberAccessExpression:
		case NetNodeType.PointerMemberAccessExpression:
			return true;
		case NetNodeType.PrimaryExpression:
			return node.Name != string.Empty;
		default:
			return false;
		}
	}

	protected virtual void FindReferences(ISyntaxNode node, ISyntaxNodes references, ISyntaxNode root, Point position)
	{
		if (string.Compare(root.Name, node.Name, !base.CaseSensitive) == 0 && FindDeclaration(null, root, root.Position) == node)
		{
			references.Add(root);
		}
		if (!root.HasChildren)
		{
			return;
		}
		foreach (ISyntaxNode child in root.ChildList)
		{
			if (child.Position.Y > position.Y || (child.Position.Y == position.Y && child.Position.X >= position.X))
			{
				FindReferences(node, references, child, position);
			}
		}
	}

	protected virtual bool CanFilter(IListMembers members, ISyntaxNode node, string filter)
	{
		bool flag = node.NodeType == 18;
		bool flag2 = node.NodeType == 25;
		if ((node.Options & SyntaxNodeOptions.CodeCompletion) != 0 || ((flag || flag2) && (members == null || ShouldDuplicate(members))))
		{
			if (!(filter == string.Empty) && (!flag || !(filter == ".ctor")) && (!flag2 || !(filter == ".ElementAccess")))
			{
				return string.Compare(node.Name, filter, !base.CaseSensitive) == 0;
			}
			return true;
		}
		return false;
	}

	protected virtual string GetNodeName(ISyntaxNode node, StaticScope scope)
	{
		if ((scope & StaticScope.TypeName) != 0)
		{
			ISyntaxNode declarationNode = GetDeclarationNode(node);
			if (declarationNode != null && declarationNode.Name != string.Empty)
			{
				return declarationNode.Name + "." + node.Name;
			}
		}
		return node.Name;
	}

	protected virtual void FillMember(IListMembers members, ISyntaxNode node, string filter, StaticScope scope)
	{
		if (CanFilter(members, node, filter) && IsScopeValid(scope, node))
		{
			string name = GetNodeName(node, scope);
			if (filter == ".ctor" || filter == ".ElementAccess")
			{
				ISyntaxNode declarationNode = GetDeclarationNode(node);
				name = ((declarationNode != null) ? declarationNode.Name : node.Name);
			}
			int nodeImageIndex = GetNodeImageIndex(node);
			if (CanAddMember(members, name, nodeImageIndex, !IsDeclarationNode(node)))
			{
				IListMember listMember = AddMember(members, name, nodeImageIndex, showDesc: true);
				listMember.Qualifier = GetNodeQualifier(node);
				listMember.DataType = GetNodeDataType(node);
				listMember.Parameters = GetNodeParameters(node);
				listMember.Description = GetDescription(members, node, node, XmlCommentType.Summary.ToString(), fullDescription: false);
				listMember.UpdateParamText();
			}
		}
	}

	protected virtual void FillNamespaceTypes(IListMembers members, IList namespaces)
	{
		foreach (INetNamespace @namespace in namespaces)
		{
			if (base.SyntaxTree != null)
			{
				ISyntaxNode namespaceNode = GetNamespaceNode(base.SyntaxTree.Root, @namespace.Namespace);
				if (namespaceNode != null)
				{
					FillNamespaceTypes(members, namespaceNode, recursive: true);
				}
			}
			foreach (ISyntaxTree syntaxTree in base.SyntaxTrees)
			{
				ISyntaxNode namespaceNode = GetNamespaceNode(syntaxTree.Root, @namespace.Namespace);
				if (namespaceNode != null)
				{
					FillNamespaceTypes(members, namespaceNode, recursive: true);
				}
			}
		}
	}

	protected virtual void FillNamespaceTypes(IListMembers members, ISyntaxNode node, bool recursive)
	{
		if (recursive)
		{
			if (!node.HasChildren)
			{
				return;
			}
			{
				foreach (ISyntaxNode child in node.ChildList)
				{
					FillNamespaceTypes(members, child, recursive: false);
				}
				return;
			}
		}
		if (IsDeclarationNode(node))
		{
			FillMember(members, node, string.Empty, StaticScope.Static | StaticScope.Instance);
		}
		if (node.NodeType != 39 || !node.HasChildren)
		{
			return;
		}
		foreach (ISyntaxNode child2 in node.ChildList)
		{
			FillNamespaceTypes(members, child2, recursive: false);
		}
	}

	protected virtual void FillNamespaceTypes(IListMembers members, INetNamespace nspace, ISyntaxNode node)
	{
		if (node != null)
		{
			ISyntaxNode namespaceNode = GetNamespaceNode(node, nspace.Namespace);
			if (namespaceNode != null)
			{
				FillNamespaceTypes(members, namespaceNode, recursive: true);
			}
		}
	}

	protected virtual void FillMembers(IListMembers members, INetNamespace nspace, ISyntaxNode node, StaticScope scope)
	{
		string text = nspace.GetName() + ".";
		foreach (INetNamespace namespace2 in GetNamespaces(nspace.System ? null : node, system: true))
		{
			string @namespace = namespace2.Namespace;
			if (base.CaseSensitive ? @namespace.StartsWith(text) : @namespace.ToLower(CultureInfo.InvariantCulture).StartsWith(text.ToLower()))
			{
				string text2 = @namespace.Substring(text.Length);
				int num = text2.IndexOf('.');
				if (num >= 0)
				{
					text2 = text2.Substring(0, num);
				}
				if (text2.Trim() != string.Empty && members.IndexOfName(text2, base.CaseSensitive) < 0)
				{
					FillMember(members, new NetNamespace(text2, nspace.System));
				}
			}
		}
		if (scope == StaticScope.None || base.SyntaxTree == null)
		{
			return;
		}
		FillNamespaceTypes(members, nspace, base.SyntaxTree.Root);
		foreach (ISyntaxTree syntaxTree in base.SyntaxTrees)
		{
			FillNamespaceTypes(members, nspace, syntaxTree.Root);
		}
	}

	protected virtual void FillMember(IListMembers members, INetNamespace nspace)
	{
		string text = nspace.Namespace;
		int num = text.IndexOf('.');
		if (num >= 0)
		{
			text = text.Substring(0, num);
		}
		if (CanAddMember(members, text, 2, overloads: false))
		{
			IListMember listMember = AddMember(members, text, 2, showDesc: false);
			listMember.DataType = SyntaxConsts.NamespaceDataType;
		}
	}

	protected virtual void FillMembers(IListMembers members, IList namespaces)
	{
		ArrayList arrayList = new ArrayList(namespaces);
		for (int num = arrayList.Count - 1; num >= 0; num--)
		{
			string text = ((INetNamespace)arrayList[num]).Namespace;
			int num2 = text.IndexOf(".");
			if (num2 >= 0)
			{
				text = text.Substring(0, num2);
			}
			for (int i = 0; i < num; i++)
			{
				string @namespace = ((INetNamespace)arrayList[i]).Namespace;
				if (base.CaseSensitive ? text.StartsWith(@namespace) : text.ToLower(CultureInfo.InvariantCulture).StartsWith(@namespace.ToLower(CultureInfo.InvariantCulture)))
				{
					arrayList.RemoveAt(num);
					break;
				}
			}
		}
		foreach (INetNamespace item in arrayList)
		{
			FillMember(members, item);
		}
	}

	protected override object DoFindDeclaration(string text, ISyntaxNode node, Point position, int paramCount)
	{
		if (IsReferenceType(node))
		{
			switch ((NetNodeType)node.NodeType)
			{
			case NetNodeType.MemberAccessExpression:
			case NetNodeType.PointerMemberAccessExpression:
			{
				IList types2 = new ArrayList();
				string name2 = string.Empty;
				Point endPos = new Point(-1, -1);
				StaticScope scope2;
				object memberType = GetMemberType(text, node, ref name2, ref position, ref endPos, out scope2);
				if (memberType == null)
				{
					return null;
				}
				return FindDeclaration(memberType, types2, position, name2, scope2, paramCount);
			}
			case NetNodeType.PrimaryExpression:
			{
				IList types3 = new ArrayList();
				return FindDeclaration(node, types3, position, node.Name, StaticScope.Static | StaticScope.Instance | StaticScope.Private, paramCount);
			}
			default:
			{
				if (position.Y == node.Position.Y && position.X > node.Position.X)
				{
					string subname = string.Empty;
					Point position2 = position;
					string name;
					if (node.NodeType == 3)
					{
						name = node.Name;
						position2 = node.Position;
					}
					else
					{
						name = GetNodeDataType(node, ref position2);
						if (name == "" && IsDeclarationNode(node))
						{
							name = GetBaseType(node, ref position2);
						}
					}
					StaticScope scope;
					if (name != string.Empty && GetQualifiedName(ref name, ref subname, ref position2, position, direction: true))
					{
						return GetQualifiedType(node, position, name, out scope);
					}
					if (IsDeclarationReference(node))
					{
						return null;
					}
				}
				IList types = new ArrayList();
				return FindDeclaration(node, types, position, node.Name, StaticScope.Static | StaticScope.Instance | StaticScope.Private, paramCount);
			}
			}
		}
		return null;
	}

	protected virtual bool IsUnaryExpressionNode(ISyntaxNode node)
	{
		switch ((NetNodeType)node.NodeType)
		{
		case NetNodeType.Expression:
		case NetNodeType.PrefixedUnaryExpression:
		case NetNodeType.CastExpression:
		case NetNodeType.PrimaryExpression:
		case NetNodeType.BaseAccessExpression:
		case NetNodeType.ThisAccessExpression:
		case NetNodeType.MemberAccessExpression:
		case NetNodeType.PointerMemberAccessExpression:
		case NetNodeType.ElementAccessExpression:
		case NetNodeType.InvocationExpression:
		case NetNodeType.PostIncrementExpression:
		case NetNodeType.PostDecrementExpression:
		case NetNodeType.GenericExpression:
			return true;
		default:
			return false;
		}
	}

	protected virtual ISyntaxNode GetCastExpression(ISyntaxNode node)
	{
		ISyntaxNode syntaxNode = null;
		if (node.NodeType == 144)
		{
			syntaxNode = node;
		}
		else if (node.HasChildren)
		{
			foreach (ISyntaxNode child in node.ChildList)
			{
				syntaxNode = GetCastExpression(child);
				if (syntaxNode != null)
				{
					break;
				}
			}
		}
		return syntaxNode;
	}

	private bool IsQualifiedChar(char ch)
	{
		if ((ch < 'a' || ch > 'z') && (ch < 'A' || ch > 'Z') && (ch < '0' || ch > '9') && ch != '_')
		{
			return ch == '.';
		}
		return true;
	}

	private bool GetQualifiedName(string text, ref string name, ref Point position)
	{
		name = string.Empty;
		if (text == null)
		{
			return false;
		}
		if (text.Trim() == string.Empty)
		{
			return true;
		}
		int num = Math.Min(text.Length, position.X);
		int i = num;
		int num2 = num - 1;
		while (num2 >= 0 && IsQualifiedChar(text[num2]))
		{
			num = num2;
			num2--;
		}
		for (; i < text.Length && IsQualifiedChar(text[i]); i++)
		{
		}
		position = new Point(num, position.Y);
		if (i > num)
		{
			name = text.Substring(num, i - num);
		}
		return i > num;
	}

	private bool GetQualifiedName(ref string name, ref string subname, ref Point pt, Point position, bool direction)
	{
		subname = string.Empty;
		if (name != string.Empty && pt.Y == position.Y && position.X >= pt.X && position.X <= pt.X + name.Length)
		{
			int num = ((!direction) ? name.LastIndexOf(".", Math.Min(position.X - pt.X, name.Length)) : name.IndexOf(".", Math.Min(position.X - pt.X, name.Length)));
			if (num >= 0)
			{
				subname = name.Substring(num + 1);
				name = name.Substring(0, num);
				num = subname.IndexOf('.');
				if (num >= 0)
				{
					subname = subname.Substring(0, num);
				}
				pt = new Point(pt.X + name.Length + 1, pt.Y);
			}
			else
			{
				pt = new Point(-1, -1);
			}
			return true;
		}
		return false;
	}

	protected virtual object GetExpressionType(ISyntaxNode node, Point position, string name, out StaticScope scope)
	{
		scope = StaticScope.Instance;
		ISyntaxNode nodeByName = GetNodeByName(node, name, position);
		if (nodeByName == null)
		{
			return null;
		}
		return GetChildType(nodeByName, position, out scope);
	}

	protected virtual object GetExpressionType(IList types, ISyntaxNode node, Point position, object member, string name, out StaticScope scope)
	{
		scope = StaticScope.Instance;
		if (member == null)
		{
			if (node != null)
			{
				object expressionType = GetExpressionType(node, position, name, out scope);
				if (expressionType != null)
				{
					return expressionType;
				}
			}
		}
		else if (member is ISyntaxNode)
		{
			if (types == null)
			{
				types = new ArrayList();
			}
			return GetChildType(types, position, (ISyntaxNode)member, name, out scope);
		}
		return GetMemberType(node, position, member, name, out scope);
	}

	protected virtual object GetNamespace(string name, ISyntaxNode node)
	{
		foreach (INetNamespace namespace2 in GetNamespaces(node, system: true))
		{
			string @namespace = namespace2.Namespace;
			if (!(base.CaseSensitive ? @namespace.StartsWith(name) : @namespace.ToLower(CultureInfo.InvariantCulture).StartsWith(name.ToLower(CultureInfo.InvariantCulture))))
			{
				continue;
			}
			if (string.Compare(name, @namespace, !base.CaseSensitive) == 0)
			{
				if (namespace2.Alias != string.Empty)
				{
					object typeByName = GetTypeByName(namespace2.Alias);
					if (typeByName != null)
					{
						return typeByName;
					}
				}
				return namespace2;
			}
			return new NetNamespace(name, node == null);
		}
		return null;
	}

	protected virtual object GetObjectType(object obj)
	{
		return obj.GetType();
	}

	public virtual object GetTypeByName(string type)
	{
		return null;
	}

	protected virtual object GetTypeByName(ISyntaxNode node, string name)
	{
		return null;
	}

	protected virtual object GetElementAccessType(object member)
	{
		if (member is ISyntaxNode)
		{
			ISyntaxNode syntaxNode = ((ISyntaxNode)member).FindNode(25);
			StaticScope scope;
			if (syntaxNode != null)
			{
				return GetChildType(syntaxNode, syntaxNode.Position, out scope);
			}
		}
		return null;
	}

	protected virtual void AddUnitNamespaces(ISyntaxNode node, IList namespaces)
	{
		switch (node.NodeType)
		{
		case 7:
			AddNetNamespace(namespaces, node);
			break;
		case 1:
		case 39:
			if (!node.HasChildren)
			{
				break;
			}
			{
				foreach (ISyntaxNode child in node.ChildList)
				{
					AddUnitNamespaces(child, namespaces);
				}
				break;
			}
		}
	}

	protected virtual string GetUsingName(ISyntaxNode node)
	{
		ISyntaxAttribute syntaxAttribute = node.FindAttribute(NetNodeType.UsingAlias.ToString());
		if (syntaxAttribute == null)
		{
			return node.Name;
		}
		return (string)syntaxAttribute.Value;
	}

	protected virtual int IndexOfNamespace(IList namespaces, string name)
	{
		for (int i = 0; i < namespaces.Count; i++)
		{
			INetNamespace netNamespace = (INetNamespace)namespaces[i];
			if (string.Compare(netNamespace.Namespace, name, !base.CaseSensitive) == 0)
			{
				return i;
			}
		}
		return -1;
	}

	protected virtual void AddNetNamespace(IList namespaces, ISyntaxNode node)
	{
		if (IndexOfNamespace(namespaces, node.Name) < 0)
		{
			INetNamespace netNamespace = new NetNamespace(node.Name, system: false);
			ISyntaxAttribute syntaxAttribute = node.FindAttribute(NetNodeType.UsingAlias.ToString());
			if (syntaxAttribute != null)
			{
				netNamespace.Alias = (string)syntaxAttribute.Value;
			}
			namespaces.Add(netNamespace);
		}
	}

	protected virtual ISyntaxNode FindUsingList(ISyntaxNode node)
	{
		if (node.HasChildren)
		{
			foreach (ISyntaxNode child in node.ChildList)
			{
				if (child.NodeType == 2)
				{
					return child;
				}
				if (child.NodeType == 39)
				{
					ISyntaxNode syntaxNode2 = FindUsingList(child);
					if (syntaxNode2 != null)
					{
						return syntaxNode2;
					}
				}
			}
		}
		return null;
	}

	protected virtual void GetNamespaces(ISyntaxNode node, IList namespaces)
	{
		while (node != null)
		{
			NetNodeType nodeType = (NetNodeType)node.NodeType;
			if (nodeType == NetNodeType.Unit || nodeType == NetNodeType.Namespace)
			{
				if (node.NodeType == 7)
				{
					AddNetNamespace(namespaces, node);
				}
				else
				{
					AddUnitNamespaces(node, namespaces);
				}
				if (node.HasChildren)
				{
					ISyntaxNode syntaxNode = FindUsingList(node);
					if (syntaxNode != null && syntaxNode.HasChildren)
					{
						foreach (ISyntaxNode child in syntaxNode.ChildList)
						{
							if (child.NodeType == 3)
							{
								AddNetNamespace(namespaces, child);
							}
						}
					}
				}
			}
			node = node.Parent;
		}
	}

	protected virtual IList GetNamespaces(ISyntaxNode node, bool system)
	{
		IList list = new ArrayList();
		foreach (string @namespace in namespaces)
		{
			list.Add(new NetNamespace(@namespace, system: true));
		}
		if (node != null)
		{
			GetNamespaces(node, list);
			foreach (ISyntaxTree syntaxTree in base.SyntaxTrees)
			{
				AddUnitNamespaces(syntaxTree.Root, list);
			}
		}
		if (system)
		{
			foreach (string namespace2 in GetNamespaces())
			{
				if (IndexOfNamespace(list, namespace2) < 0)
				{
					list.Add(new NetNamespace(namespace2, system: true));
				}
			}
		}
		((ArrayList)list).Sort(namespaceComparer);
		return list;
	}

	protected virtual IList GetNamespaces()
	{
		return namespaces;
	}

	protected virtual object GetExpressionType(Point position, ISyntaxNode node, out StaticScope scope)
	{
		object obj = null;
		scope = StaticScope.None;
		switch ((NetNodeType)node.NodeType)
		{
		case NetNodeType.PrimaryExpression:
			obj = GetExpressionType(null, node, position, null, node.Name, out scope);
			break;
		case NetNodeType.MemberAccessExpression:
		case NetNodeType.PointerMemberAccessExpression:
			if (!node.HasChildren)
			{
				break;
			}
			obj = GetExpressionType(node.ChildList[0].Position, node.ChildList[0], out scope);
			if (obj != null)
			{
				obj = GetExpressionType(null, node, position, obj, node.Name, out scope);
				if (obj != null && (scope & StaticScope.Method) != 0 && (node.Parent == null || node.Parent.NodeType != 157))
				{
					obj = null;
				}
			}
			break;
		case NetNodeType.InvocationExpression:
			if (node.HasChildren)
			{
				obj = GetExpressionType(node.ChildList[0].Position, node.ChildList[0], out scope);
				scope = StaticScope.Instance;
			}
			break;
		case NetNodeType.CastExpression:
		case NetNodeType.CastInvocationExpression:
		{
			string nodeDataType = GetNodeDataType(node);
			obj = GetExpressionType(null, node, position, null, (nodeDataType != string.Empty) ? nodeDataType : node.Name, out scope);
			scope = StaticScope.Instance;
			break;
		}
		case NetNodeType.ThisAccessExpression:
			obj = GetDeclarationNode(node);
			scope = StaticScope.Static | StaticScope.Instance | StaticScope.Protected | StaticScope.Private;
			break;
		case NetNodeType.BaseAccessExpression:
			obj = GetDeclarationNode(node);
			if (obj != null)
			{
				obj = GetExpressionType(null, node, position, null, GetBaseType((ISyntaxNode)obj), out scope);
			}
			scope = StaticScope.Static | StaticScope.Instance;
			break;
		case NetNodeType.ParenthesizedExpression:
			if (node.HasChildren)
			{
				obj = GetExpressionType(node.ChildList[0].Position, node.ChildList[0], out scope);
			}
			break;
		case NetNodeType.ElementAccessExpression:
			if (node.HasChildren)
			{
				obj = GetExpressionType(node.ChildList[0].Position, node.ChildList[0], out scope);
				if (obj != null)
				{
					obj = GetElementAccessType(obj);
				}
				scope = StaticScope.Instance;
			}
			break;
		}
		return obj;
	}

	protected virtual object GetWithStatementExpression(ISyntaxNode node, out StaticScope scope)
	{
		scope = StaticScope.None;
		if (!node.HasChildren)
		{
			return null;
		}
		return GetExpressionType(node.ChildList[0].Position, node.ChildList[0], out scope);
	}

	protected virtual object GetMemberAccessType(ISyntaxNode node, ref string name, ref Point position, out StaticScope scope)
	{
		scope = StaticScope.None;
		name = node.Name;
		object obj = (node.HasChildren ? GetExpressionType(node.ChildList[0].Position, node.ChildList[0], out scope) : null);
		if (obj != null)
		{
			ISyntaxAttribute syntaxAttribute = node.FindAttribute(NetNodeType.Name.ToString());
			if (syntaxAttribute != null)
			{
				position = syntaxAttribute.Position;
			}
			if (IsEnumTypeRegistered() && GetEnumType(obj) != null)
			{
				scope |= StaticScope.Static;
			}
		}
		return obj;
	}

	protected virtual bool IsEnumTypeRegistered()
	{
		return false;
	}

	protected virtual object GetEnumType(object member)
	{
		if (!(member is ISyntaxNode) || ((ISyntaxNode)member).NodeType != 11)
		{
			return null;
		}
		return member;
	}

	protected virtual object GetUsingType(ISyntaxNode node, ref string name, ref Point position, out StaticScope scope)
	{
		scope = StaticScope.None;
		name = node.Name;
		string subname = string.Empty;
		Point pt = node.Position;
		if (GetQualifiedName(ref name, ref subname, ref pt, position, direction: false) && pt.Y >= 0)
		{
			object @namespace = GetNamespace(name, null);
			name = subname;
			if (@namespace != null)
			{
				position = pt;
			}
			return @namespace;
		}
		return null;
	}

	protected virtual object GetBlockNodeType(ISyntaxNode node, ref Point position, bool checkBlock, bool updatePos, out StaticScope scope)
	{
		if ((!checkBlock || GetBlockNode(node) != null) && GetDeclarationNode(node) != null)
		{
			if (updatePos)
			{
				position = node.Position;
			}
			scope = StaticScope.Static | StaticScope.Instance | StaticScope.Global | StaticScope.Protected | StaticScope.Private;
			return node;
		}
		scope = StaticScope.None;
		return null;
	}

	protected virtual object GetTypeOfNode(ISyntaxNode node, ref string name, ref Point position, bool localVar, bool checkBlock, out StaticScope scope)
	{
		Point position2 = position;
		string nodeDataType = GetNodeDataType(node, ref position2);
		return GetTypeOfNode(node, nodeDataType, position2, ref name, ref position, localVar, checkBlock, out scope);
	}

	protected virtual object GetTypeOfNode(ISyntaxNode node, string typeName, Point typePos, ref string name, ref Point position, bool localVar, bool checkBlock, out StaticScope scope)
	{
		scope = StaticScope.None;
		if (typeName != string.Empty)
		{
			name = typeName;
			string subname = string.Empty;
			Point pt = typePos;
			if (GetQualifiedName(ref name, ref subname, ref pt, position, direction: false))
			{
				object obj = null;
				if (pt.Y >= 0)
				{
					obj = GetQualifiedType(node, position, name, out scope);
					if (obj != null)
					{
						position = pt;
					}
					name = subname;
				}
				else
				{
					obj = GetBlockNodeType(node, ref position, !localVar, localVar, out scope);
					position = typePos;
				}
				return obj;
			}
		}
		if (!localVar)
		{
			name = string.Empty;
			return GetBlockNodeType(node, ref position, checkBlock, updatePos: false, out scope);
		}
		return null;
	}

	protected virtual void FillSnippet(IListMembers members, string name, int index)
	{
		if (CanAddMember(members, name, index, overloads: false))
		{
			AddMember(members, name, index, showDesc: true);
		}
	}

	protected virtual void FillSnippets(IListMembers members)
	{
		IDictionaryEnumerator enumerator = base.Snippets.GetEnumerator();
		enumerator.Reset();
		while (enumerator.MoveNext())
		{
			FillSnippet(members, (string)enumerator.Key, ((bool)enumerator.Value) ? 43 : 44);
		}
	}

	protected virtual void FillGlobalObject(IListMembers members, string name, int index)
	{
		if (CanAddMember(members, name, index, overloads: false))
		{
			AddMember(members, name, index, showDesc: true);
		}
	}

	protected virtual void FillGlobalObjects(IListMembers members)
	{
		IDictionaryEnumerator enumerator = internalObjects.GetEnumerator();
		enumerator.Reset();
		while (enumerator.MoveNext())
		{
			FillGlobalObject(members, (string)enumerator.Key, 38);
		}
	}

	protected virtual void FillGlobalTypes(IListMembers members, IList namespaces)
	{
	}

	protected virtual bool SkipGlobalMembers(ISyntaxNode node)
	{
		if (allowGlobalMembers)
		{
			return IsDeclarationNode(node);
		}
		return true;
	}

	protected override void DoFillMembers(ISyntaxNode node, Point position, IListMembers members, IList types, object member, string name, StaticScope scope, ref int selIndex)
	{
		base.DoFillMembers(node, position, members, types, member, name, scope, ref selIndex);
		if (member is ISyntaxNode)
		{
			ISyntaxNode node2 = (ISyntaxNode)member;
			FillMembers(members, types, node2, position, string.Empty, scope, ref selIndex);
			if (!SkipGlobalMembers(node2) && !IsXmlCommentNode(node2))
			{
				FillMembers(members, GetNamespaces(node2, system: true));
				IList list = GetNamespaces(node2, system: false);
				FillNamespaceTypes(members, list);
				FillGlobalTypes(members, list);
				FillGlobalObjects(members);
				FillSnippets(members);
				foreach (ISyntaxTree syntaxTree in base.SyntaxTrees)
				{
					AddUnitMember(members, syntaxTree.Root, string.Empty, scope);
				}
			}
		}
		else if (member is INetNamespace)
		{
			FillMembers(members, (INetNamespace)member, node, scope);
		}
		members.Images = internalImages;
	}

	public override object GetNodeType(string text, ISyntaxNode node, Point position)
	{
		if (node != null)
		{
			Point position2 = position;
			string text2 = GetNodeDataType(node, ref position2);
			if (text2 == string.Empty && IsDeclarationNode(node))
			{
				text2 = GetBaseType(node, ref position2);
			}
			StaticScope scope;
			if (text2 != string.Empty && position2.Y == position.Y && position2.X <= position.X && position2.X + text2.Length > position.X)
			{
				return GetQualifiedType(node, position, text2, out scope);
			}
		}
		return base.GetNodeType(text, node, position);
	}

	public override object GetMemberType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out StaticScope scope)
	{
		string name2 = string.Empty;
		Point position2 = position;
		Point endPos2 = endPos;
		object specialMemberType = GetSpecialMemberType(text, node, ref name2, ref position2, ref endPos2, out scope);
		if (specialMemberType != null)
		{
			name = name2;
			position = position2;
			endPos = endPos2;
			return specialMemberType;
		}
		name = string.Empty;
		scope = StaticScope.None;
		switch ((NetNodeType)node.NodeType)
		{
		case NetNodeType.MemberAccessExpression:
		case NetNodeType.PointerMemberAccessExpression:
			return GetMemberAccessType(node, ref name, ref position, out scope);
		case NetNodeType.Using:
			return GetUsingType(node, ref name, ref position, out scope);
		case NetNodeType.Field:
		case NetNodeType.LocalVariable:
			return GetTypeOfNode(node, ref name, ref position, localVar: true, checkBlock: true, out scope);
		case NetNodeType.PrimaryExpression:
			name = node.Name;
			return GetBlockNodeType(node, ref position, checkBlock: true, updatePos: true, out scope);
		case NetNodeType.AsIsExpression:
		case NetNodeType.ObjectCreationExpression:
			if (position.Y == node.Position.Y && position.X > node.Position.X + node.Name.Length)
			{
				return GetTypeOfNode(node, ref name, ref position, localVar: false, checkBlock: true, out scope);
			}
			break;
		case NetNodeType.XmlTag:
		{
			ISyntaxAttribute syntaxAttribute2 = node.FindAttribute(XmlLexerToken.CloseTag.ToString());
			if (syntaxAttribute2 == null)
			{
				position = new Point(node.Position.X + 1, node.Position.Y);
			}
			name = node.Name;
			return node;
		}
		case NetNodeType.XmlComment:
			return node;
		case NetNodeType.WithStatementMember:
		{
			ISyntaxAttribute syntaxAttribute = node.FindAttribute(NetNodeType.WithStatement.ToString());
			if (syntaxAttribute != null)
			{
				specialMemberType = GetWithStatementExpression((ISyntaxNode)syntaxAttribute.Value, out scope);
				if (specialMemberType != null)
				{
					return specialMemberType;
				}
			}
			break;
		}
		}
		Point position3 = position;
		string name3 = string.Empty;
		if (GetQualifiedName(text, ref name3, ref position3))
		{
			specialMemberType = GetTypeOfNode(node, name3, position3, ref name, ref position, localVar: false, checkBlock: true, out scope);
			if (name != null)
			{
				int num = name.IndexOf(".");
				if (num >= 0)
				{
					name = name.Substring(0, num);
				}
			}
			return specialMemberType;
		}
		return null;
	}

	public override object GetSpecialMemberType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out StaticScope scope)
	{
		name = string.Empty;
		scope = StaticScope.None;
		object obj = null;
		switch ((NetNodeType)node.NodeType)
		{
		case NetNodeType.ObjectCreationExpression:
			if (position.Y != node.Position.Y || position.X <= node.Position.X + node.Name.Length)
			{
				break;
			}
			obj = GetTypeOfNode(node, ref name, ref position, localVar: false, checkBlock: true, out scope);
			if (obj != null)
			{
				ISyntaxNode parent = node.Parent;
				while (parent != null && parent.NodeType != 15 && parent.NodeType != 129)
				{
					parent = parent.Parent;
				}
				if (parent != null)
				{
					if (parent.NodeType == 15)
					{
						string nodeDataType = GetNodeDataType(parent);
						if (nodeDataType != string.Empty)
						{
							name = nodeDataType;
						}
					}
					else if (parent.HasChildren)
					{
						object expressionType = GetExpressionType(parent.ChildList[0].Position, parent.ChildList[0], out scope);
						if (expressionType != null)
						{
							name = expressionType.ToString();
						}
					}
				}
			}
			return obj;
		case NetNodeType.AssignmentExpression:
			if (node.HasChildren && node.Name == "+=")
			{
				ISyntaxNode syntaxNode3 = node.ChildList[0];
				obj = GetExpressionType(syntaxNode3.Position, syntaxNode3, out scope);
				if (obj is Type && ((Type)obj).BaseType == typeof(MulticastDelegate))
				{
					ISyntaxNode declarationNode = GetDeclarationNode(node);
					name = ((declarationNode != null) ? (declarationNode.Name + "_" + syntaxNode3.Name) : syntaxNode3.Name);
					scope = StaticScope.Delegate;
					return obj;
				}
			}
			break;
		case NetNodeType.EqualityOrAssignmentExpression:
			if (!node.HasChildren)
			{
				break;
			}
			obj = GetExpressionType(node.ChildList[0].Position, node.ChildList[0], out scope);
			if (obj != null)
			{
				obj = GetEnumType(obj);
				if (obj != null)
				{
					scope = StaticScope.Static | StaticScope.TypeName | StaticScope.ShortType;
					return obj;
				}
			}
			break;
		case NetNodeType.Field:
		case NetNodeType.LocalVariable:
		{
			if (!node.HasAttributes)
			{
				break;
			}
			ISyntaxAttribute[] attributes = node.Attributes;
			foreach (ISyntaxAttribute syntaxAttribute in attributes)
			{
				if (!(syntaxAttribute.Name == NetNodeType.Modifier.ToString()) || string.Compare((string)syntaxAttribute.Value, "as", !base.CaseSensitive) != 0)
				{
					continue;
				}
				if (syntaxAttribute.Position.Y >= position.Y && (syntaxAttribute.Position.Y != position.Y || syntaxAttribute.Position.X >= position.X))
				{
					break;
				}
				obj = GetChildType(node, node.Position, out scope);
				if (obj != null)
				{
					obj = GetEnumType(obj);
					if (obj != null)
					{
						scope = StaticScope.Static | StaticScope.TypeName | StaticScope.ShortType;
						return obj;
					}
				}
				return GetTypeOfNode(node, ref name, ref position, localVar: false, node.NodeType == 15, out scope);
			}
			break;
		}
		case NetNodeType.CaseStatement:
		{
			if (node.Parent == null || node.Parent.NodeType != 120)
			{
				break;
			}
			ISyntaxNode syntaxNode2 = node.Parent.ChildList[0];
			obj = GetExpressionType(syntaxNode2.Position, syntaxNode2, out scope);
			if (obj != null)
			{
				obj = GetEnumType(obj);
				if (obj != null)
				{
					scope = StaticScope.Static | StaticScope.TypeName | StaticScope.ShortType;
					return obj;
				}
			}
			break;
		}
		case NetNodeType.ArgumentList:
		{
			int paramIndex;
			int paramCount;
			ISyntaxNode invocationNode = GetInvocationNode(text, node, position, out paramIndex, out paramCount);
			if (invocationNode == null || !invocationNode.HasChildren)
			{
				break;
			}
			ISyntaxNode syntaxNode = invocationNode.ChildList[0];
			obj = DoFindDeclaration(text, syntaxNode, syntaxNode.Position, paramCount);
			if (obj == null || paramIndex < 0)
			{
				break;
			}
			obj = GetParameterType(obj, paramIndex);
			if (obj != null)
			{
				obj = GetEnumType(obj);
				if (obj != null)
				{
					scope = StaticScope.Static | StaticScope.TypeName | StaticScope.ShortType;
					return obj;
				}
			}
			break;
		}
		}
		return null;
	}

	public override object GetMemberType(ISyntaxNode node, Point position, object member, string name, out StaticScope scope)
	{
		object obj = base.GetMemberType(node, position, member, name, out scope);
		if (obj == null && (member == null || member is INetNamespace || (member is ISyntaxNode && ((ISyntaxNode)member).NodeType == 7)))
		{
			scope = StaticScope.Static;
			if (member is ISyntaxNode)
			{
				return GetDeclaration((ISyntaxNode)member, ((ISyntaxNode)member).Position, name);
			}
			if (member == null)
			{
				ISyntaxNode declaration = GetDeclaration(node, position, name);
				if (declaration != null)
				{
					obj = declaration;
				}
			}
			if (obj == null)
			{
				string name2 = name;
				if (member is INetNamespace)
				{
					name2 = ((name != string.Empty) ? (((INetNamespace)member).GetName() + "." + name) : ((INetNamespace)member).GetName());
				}
				obj = GetRegisteredObject(name2);
				if (obj != null)
				{
					scope = StaticScope.Instance;
					obj = GetObjectType(obj);
				}
				if (obj == null)
				{
					obj = GetTypeByName(node, name2);
					if (obj is Type && NetTypes.GlobalTypes.Contains(obj))
					{
						scope |= StaticScope.Instance;
					}
				}
				if (obj == null)
				{
					obj = GetNamespace(name2, node);
				}
			}
		}
		return obj;
	}

	public virtual object GetRegisteredObject(string name)
	{
		return objects[base.CaseSensitive ? name : name.ToLower(CultureInfo.InvariantCulture)];
	}

	public override object GetMethodType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out int paramIndex, out int paramCount, out StaticScope scope)
	{
		ISyntaxNode invocationNode = GetInvocationNode(text, node, position, out paramIndex, out paramCount);
		name = string.Empty;
		object result = null;
		scope = StaticScope.None;
		if (invocationNode != null)
		{
			position = invocationNode.Position;
			endPos = invocationNode.Range.EndPoint;
			switch ((NetNodeType)invocationNode.NodeType)
			{
			case NetNodeType.InvocationExpression:
				if (invocationNode.HasChildren)
				{
					ISyntaxNode syntaxNode = invocationNode.ChildList[0];
					name = syntaxNode.Name;
					result = ((!IsUnaryExpressionNode(syntaxNode)) ? GetDeclarationNode(invocationNode) : GetMemberType(text, syntaxNode, ref name, ref position, ref endPos, out scope));
				}
				else
				{
					result = GetDeclarationNode(invocationNode);
				}
				scope = StaticScope.Static | StaticScope.Instance | StaticScope.TypeName | StaticScope.ShortType | (scope & StaticScope.Global) | (scope & StaticScope.Protected) | (scope & StaticScope.Private);
				break;
			case NetNodeType.CastInvocationExpression:
			case NetNodeType.CastInvocationTargetExpression:
				result = GetMemberType(text, invocationNode, ref name, ref position, ref endPos, out scope);
				break;
			case NetNodeType.ElementAccessExpression:
				result = GetMemberAccessType(invocationNode, ref name, ref position, out scope);
				if (invocationNode.HasChildren)
				{
					position = invocationNode.ChildList[0].Position;
				}
				name = ".ElementAccess";
				scope = StaticScope.Instance | StaticScope.TypeName;
				break;
			case NetNodeType.ObjectCreationExpression:
			{
				Point position2 = invocationNode.Position;
				string nodeDataType = GetNodeDataType(invocationNode, ref position2);
				name = ".ctor";
				if (nodeDataType != string.Empty)
				{
					result = GetExpressionType(null, invocationNode, position2, null, nodeDataType, out scope);
				}
				scope = StaticScope.Static | StaticScope.Instance | StaticScope.TypeName;
				break;
			}
			}
			position = invocationNode.Position;
		}
		if (!(name != string.Empty))
		{
			return null;
		}
		return result;
	}

	public override void FillMember(IListMembers members, object member, string name, StaticScope scope)
	{
		base.FillMember(members, member, name, scope);
		if (member is ISyntaxNode)
		{
			IList types = new ArrayList();
			int selIndex = -1;
			FillMembers(members, types, (ISyntaxNode)member, new Point(int.MaxValue, int.MaxValue), name, scope, ref selIndex);
		}
	}

	public override void FillMember(IListMembers members, object member, string name, int paramIndex, StaticScope scope)
	{
		base.FillMember(members, member, name, paramIndex, scope);
	}

	public override int FindReferences(ISyntaxNode node, ISyntaxNodes references)
	{
		base.FindReferences(node, references);
		ISyntaxNode syntaxNode = GetBlockOrMethodNode(node);
		Point position = node.Position;
		if (syntaxNode == null)
		{
			position = Point.Empty;
			syntaxNode = base.SyntaxTree.Root;
		}
		FindReferences(node, references, syntaxNode, position);
		return references.Count;
	}

	protected virtual bool HasSnippetMembers(string language)
	{
		return language == "vb";
	}

	public override ICodeSnippetsProvider GetCodeSnippets(string language)
	{
		object obj = codeSnippets[language];
		if (obj != null)
		{
			return (ICodeSnippetsProvider)obj;
		}
		ICodeSnippetsProvider codeSnippetsProvider = ((!HasSnippetMembers(language)) ? ((ICodeSnippetsProvider)new CodeSnippets()) : ((ICodeSnippetsProvider)new CodeSnippetMembers()));
		codeSnippetsProvider.Images = internalImages;
		ResourceManager resourceManager = new ResourceManager(typeof(NetSyntaxParser));
		obj = resourceManager.GetObject("CodeSnippets." + language);
		if (obj != null)
		{
			codeSnippetsProvider.LoadStream(new StringReader((string)obj));
		}
		codeSnippets[language] = codeSnippetsProvider;
		return codeSnippetsProvider;
	}

	public virtual void RegisterNamespace(string nspace)
	{
		namespaces.Add(nspace);
	}

	public virtual bool UnregisterNamespace(string nspace)
	{
		int num = namespaces.IndexOf(nspace);
		if (num >= 0)
		{
			namespaces.RemoveAt(num);
		}
		return num >= 0;
	}

	public virtual void RegisterObject(string name, object obj)
	{
		objects[base.CaseSensitive ? name : name.ToLower(CultureInfo.InvariantCulture)] = obj;
		internalObjects[name] = obj;
	}

	public virtual bool UnregisterObject(string name)
	{
		if (internalObjects.Contains(name))
		{
			internalObjects.Remove(name);
		}
		name = (base.CaseSensitive ? name : name.ToLower(CultureInfo.InvariantCulture));
		bool flag = objects.Contains(name);
		if (flag)
		{
			objects.Remove(name);
		}
		return flag;
	}

	public virtual void ClearNamespaces()
	{
		namespaces.Clear();
	}

	public virtual void ClearObjects()
	{
		objects.Clear();
		internalObjects.Clear();
	}
}
