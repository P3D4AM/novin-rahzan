namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal abstract class LexicalScope
{
	private sealed class GlobalLexicalScope : LexicalScope
	{
		public static class Singleton
		{
			public static readonly GlobalLexicalScope Instance;

			static Singleton()
			{
				Instance = new GlobalLexicalScope();
			}
		}

		public override LexicalScope AbsRoot => this;

		protected override LexicalScope OwnerScope => this;

		private GlobalLexicalScope()
			: base(1)
		{
		}

		public override LexicalScope CreateChildScope()
		{
			return this;
		}

		public override bool IsVisibleTo(LexicalScope other)
		{
			return true;
		}
	}

	private sealed class ScriptLexicalScope : LexicalScope
	{
		public override LexicalScope AbsRoot => null;

		protected override LexicalScope OwnerScope => null;

		public ScriptLexicalScope()
			: base(1)
		{
		}

		public override LexicalScope CreateChildScope()
		{
			return new BatchLexicalScope(this, ++lastChildOrderTag);
		}

		public override bool IsVisibleTo(LexicalScope other)
		{
			return true;
		}
	}

	private sealed class BatchLexicalScope : LexicalScope
	{
		private readonly ScriptLexicalScope scriptScope;

		public override LexicalScope AbsRoot => this;

		protected override LexicalScope OwnerScope => scriptScope;

		public BatchLexicalScope(ScriptLexicalScope script, int order)
			: base(order)
		{
			scriptScope = script;
		}

		public override LexicalScope CreateModuleScope()
		{
			return new ModuleLexicalScope(this, ++lastChildOrderTag);
		}
	}

	private sealed class ModuleLexicalScope : LexicalScope
	{
		private readonly BatchLexicalScope batchScope;

		public override LexicalScope AbsRoot => batchScope;

		protected override LexicalScope OwnerScope => batchScope;

		public ModuleLexicalScope(BatchLexicalScope batch, int order)
			: base(order)
		{
			batchScope = batch;
		}
	}

	private sealed class ChildLexicalScope : LexicalScope
	{
		public override LexicalScope AbsRoot => Parent.AbsRoot;

		protected override LexicalScope OwnerScope => Parent;

		public ChildLexicalScope(LexicalScope parent, int order)
			: base(parent, order)
		{
		}
	}

	private readonly LexicalScope Parent;

	private readonly int OrderTag;

	private int lastChildOrderTag;

	public static LexicalScope Global => GlobalLexicalScope.Singleton.Instance;

	private int DepthTag
	{
		get
		{
			if (Parent == null)
			{
				return 0;
			}
			return Parent.DepthTag + 1;
		}
	}

	public LexicalScope Root
	{
		get
		{
			LexicalScope lexicalScope = this;
			while (lexicalScope.Parent != null)
			{
				lexicalScope = lexicalScope.Parent;
			}
			return lexicalScope;
		}
	}

	public abstract LexicalScope AbsRoot { get; }

	protected bool IsRootScope => Parent == null;

	protected abstract LexicalScope OwnerScope { get; }

	public static LexicalScope CreateScirptScope()
	{
		return new ScriptLexicalScope();
	}

	protected LexicalScope(int order)
		: this(null, order)
	{
	}

	protected LexicalScope(LexicalScope parent, int order)
	{
		Parent = parent;
		OrderTag = order;
	}

	public virtual LexicalScope CreateChildScope()
	{
		return new ChildLexicalScope(this, ++lastChildOrderTag);
	}

	public virtual LexicalScope CreateModuleScope()
	{
		return AbsRoot.CreateModuleScope();
	}

	public bool IsBefore(LexicalScope other)
	{
		if (OrderTag < other.OrderTag)
		{
			if (Parent == null || Parent != other.Parent)
			{
				return OwnerScope == other.OwnerScope;
			}
			return true;
		}
		return false;
	}

	public virtual bool IsVisibleTo(LexicalScope other)
	{
		if (this == other)
		{
			return false;
		}
		if (IsRootScope)
		{
			return other.Root == this;
		}
		if (other.IsRootScope)
		{
			return false;
		}
		return IsVisible(this, other);
	}

	private static bool IsVisible(LexicalScope defScope, LexicalScope useScope)
	{
		int num = defScope.DepthTag;
		int depthTag = useScope.DepthTag;
		int num2 = num - depthTag;
		if (num > depthTag)
		{
			for (int i = 0; i < num - depthTag; i++)
			{
				defScope = defScope.Parent;
			}
			num = depthTag;
		}
		else if (depthTag > num)
		{
			for (int j = 0; j < depthTag - num; j++)
			{
				useScope = useScope.Parent;
			}
			depthTag = num;
		}
		bool result;
		if (defScope == useScope)
		{
			result = num2 < 0;
		}
		else
		{
			result = false;
			for (int k = 0; k < num; k++)
			{
				LexicalScope parent = defScope.Parent;
				LexicalScope parent2 = useScope.Parent;
				if (parent == parent2)
				{
					result = defScope.OrderTag < useScope.OrderTag;
					break;
				}
				defScope = parent;
				useScope = parent2;
			}
		}
		return result;
	}
}
