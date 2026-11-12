using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal abstract class ExecutionSimulator<T> where T : class, IDatabaseObject
{
	private enum ActionType : sbyte
	{
		Create,
		Drop,
		Alter
	}

	private abstract class NodeBase : ExecutionSimulator<T>
	{
		protected readonly T m_dbObject;

		protected readonly LexicalScope m_lexicalScope;

		public sealed override T this[LexicalScope lexicalScope] => GetNodeAt(lexicalScope).m_dbObject;

		protected NodeBase(T dbObject, LexicalScope lexicalScope)
		{
			m_dbObject = dbObject;
			m_lexicalScope = lexicalScope;
		}

		public sealed override void Create(T dbObject, LexicalScope lexicalScope)
		{
			AppendAction(dbObject, lexicalScope, ActionType.Create);
		}

		public sealed override void Alter(T dbObject, LexicalScope lexicalScope)
		{
			AppendAction(dbObject, lexicalScope, ActionType.Alter);
		}

		public sealed override void Drop(T dbObject, LexicalScope lexicalScope)
		{
			AppendAction(dbObject, lexicalScope, ActionType.Drop);
		}

		public abstract void AppendAction(T dbObject, LexicalScope lexicalScope, ActionType type);

		public abstract NodeBase GetNodeAt(LexicalScope lexicalScope);
	}

	private abstract class NodeBase<N> : NodeBase where N : NodeBase<N>
	{
		protected N m_next;

		protected NodeBase(T dbObject, LexicalScope lexicalScope)
			: base(dbObject, lexicalScope)
		{
		}

		protected static N GetLast(N node)
		{
			while (node.m_next != null)
			{
				node = node.m_next;
			}
			return node;
		}
	}

	private sealed class ExecutionNode : NodeBase<ExecutionNode>
	{
		private readonly ActionType m_actionType;

		public ExecutionNode Last => NodeBase<ExecutionNode>.GetLast(this);

		public ExecutionNode(T dbObject, LexicalScope lexicalScope, ActionType actionType)
			: base((actionType != ActionType.Drop) ? dbObject : null, lexicalScope)
		{
			m_actionType = actionType;
		}

		public override void AppendAction(T dbObject, LexicalScope lexicalScope, ActionType type)
		{
			NodeBase<ExecutionNode>.GetLast(this).m_next = new ExecutionNode(dbObject, lexicalScope, type);
		}

		public override NodeBase GetNodeAt(LexicalScope lexicalScope)
		{
			ExecutionNode executionNode;
			if (IsVisibleTo(lexicalScope))
			{
				executionNode = this;
				while (executionNode.m_next != null && executionNode.m_next.IsVisibleTo(lexicalScope))
				{
					executionNode = executionNode.m_next;
				}
			}
			else
			{
				executionNode = null;
			}
			return executionNode;
		}

		public T ExecuteAll()
		{
			return NodeBase<ExecutionNode>.GetLast(this).m_dbObject;
		}

		public T ExecuteTo(LexicalScope lexicalScope, T initDbObject)
		{
			if (IsVisibleTo(lexicalScope))
			{
				ExecutionNode executionNode = this;
				while (executionNode.m_next != null && executionNode.m_next.IsVisibleTo(lexicalScope))
				{
					executionNode = executionNode.m_next;
				}
				return executionNode.m_dbObject;
			}
			return initDbObject;
		}

		private bool IsVisibleTo(LexicalScope lexicalScope)
		{
			return m_lexicalScope.IsVisibleTo(lexicalScope);
		}
	}

	private sealed class RootNode : NodeBase<RootNode>
	{
		private enum ScopeMatch
		{
			Before,
			Same,
			After
		}

		private ExecutionNode m_executionNode;

		private RootNode m_nestedNode;

		public RootNode(T dbObject, LexicalScope lexicalScope)
			: base(dbObject, lexicalScope)
		{
		}

		public override void AppendAction(T dbObject, LexicalScope lexicalScope, ActionType type)
		{
			RootNode orCreateRootNode = GetOrCreateRootNode(lexicalScope);
			if (orCreateRootNode.m_executionNode == null)
			{
				orCreateRootNode.m_executionNode = new ExecutionNode(dbObject, lexicalScope, type);
			}
			else
			{
				orCreateRootNode.m_executionNode.AppendAction(dbObject, lexicalScope, type);
			}
		}

		public override NodeBase GetNodeAt(LexicalScope lexicalScope)
		{
			LexicalScope absRoot = lexicalScope.AbsRoot;
			RootNode rootNode = this;
			ScopeMatch scopeMatch = ScopeMatch.Before;
			while (true)
			{
				if (rootNode.m_lexicalScope == absRoot)
				{
					scopeMatch = ScopeMatch.Same;
					break;
				}
				if (!rootNode.m_lexicalScope.IsBefore(absRoot))
				{
					scopeMatch = ScopeMatch.After;
					break;
				}
				if (rootNode.m_next == null)
				{
					scopeMatch = ScopeMatch.Before;
					break;
				}
				rootNode = rootNode.m_next;
			}
			switch (scopeMatch)
			{
			case ScopeMatch.Before:
				if (rootNode.m_executionNode != null)
				{
					return rootNode.m_executionNode.Last;
				}
				return rootNode;
			case ScopeMatch.Same:
				if (lexicalScope.Root != absRoot)
				{
					RootNode nestedNode = rootNode.m_nestedNode;
					if (nestedNode != null)
					{
						return nestedNode.m_executionNode.GetNodeAt(lexicalScope) ?? nestedNode;
					}
					return rootNode.m_executionNode.Last;
				}
				if (rootNode.m_executionNode != null)
				{
					return rootNode.m_executionNode.GetNodeAt(lexicalScope) ?? rootNode;
				}
				return rootNode;
			case ScopeMatch.After:
				return rootNode;
			default:
				return null;
			}
		}

		private RootNode GetOrCreateRootNode(LexicalScope lexicalScope)
		{
			LexicalScope root = lexicalScope.Root;
			LexicalScope absRoot = root.AbsRoot;
			RootNode rootNode = NodeBase<RootNode>.GetLast(this);
			if (rootNode.m_lexicalScope != absRoot)
			{
				rootNode.m_next = new RootNode(rootNode.ExecuteNode(), absRoot);
				rootNode = rootNode.m_next;
			}
			if (root != absRoot && root != lexicalScope)
			{
				if (rootNode.m_nestedNode == null)
				{
					rootNode.m_nestedNode = new RootNode(rootNode.ExecuteNode(), root);
				}
				rootNode = rootNode.m_nestedNode;
			}
			return rootNode;
		}

		private T ExecuteNode()
		{
			if (m_executionNode == null)
			{
				return m_dbObject;
			}
			return m_executionNode.ExecuteAll();
		}
	}

	public abstract T this[LexicalScope lexicalScope] { get; }

	public static ExecutionSimulator<T> New(T dbObject, LexicalScope lexicalScope)
	{
		return new RootNode(dbObject, lexicalScope.AbsRoot);
	}

	public abstract void Create(T dbObject, LexicalScope lexicalScope);

	public abstract void Alter(T dbObject, LexicalScope lexicalScope);

	public abstract void Drop(T dbObject, LexicalScope lexicalScope);
}
