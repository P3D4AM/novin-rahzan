using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal sealed class LocalSymbolTable
{
	private sealed class LocalSymbol<S> : IMetadataObject where S : class, ILocalVariable
	{
		public readonly S Variable;

		public readonly LexicalScope LexicalScope;

		public string Name => Variable.Name;

		public LocalSymbol(S variable, LexicalScope lexicalScope)
		{
			Variable = variable;
			LexicalScope = lexicalScope;
		}

		public T Accept<T>(IMetadataObjectVisitor<T> visitor)
		{
			return Variable.Accept(visitor);
		}
	}

	private const int InitialCapacity = 32;

	private readonly CollationInfo m_collationInfo;

	private DictionaryCollection<LocalSymbol<IScalarVariable>> m_scalarVariables;

	private DictionaryCollection<LocalSymbol<ITableVariable>> m_tableVariables;

	private DictionaryCollection<LocalSymbol<ICursorVariable>> m_cursorVariables;

	public bool IsEmpty
	{
		get
		{
			if (m_scalarVariables == null && m_tableVariables == null)
			{
				return m_cursorVariables == null;
			}
			return false;
		}
	}

	public LocalSymbolTable(CollationInfo collationInfo)
	{
		m_collationInfo = collationInfo;
	}

	public IScalarVariable FindScalarVariable(string name, LexicalScope lexicalScope)
	{
		return Find(name, m_scalarVariables, lexicalScope);
	}

	public ITableVariable FindTableVariable(string name, LexicalScope lexicalScope)
	{
		return Find(name, m_tableVariables, lexicalScope);
	}

	public ICursorVariable FindCursorVariable(string name, LexicalScope lexicalScope)
	{
		return Find(name, m_cursorVariables, lexicalScope);
	}

	public ILocalVariable FindVariable(string name, LexicalScope lexicalScope)
	{
		ILocalVariable localVariable = FindScalarVariable(name, lexicalScope);
		object obj = localVariable;
		if (obj == null)
		{
			localVariable = FindTableVariable(name, lexicalScope);
			obj = localVariable ?? FindCursorVariable(name, lexicalScope);
		}
		return (ILocalVariable)obj;
	}

	private static T Find<T>(string name, DictionaryCollection<LocalSymbol<T>> collection, LexicalScope lexicalScope) where T : class, ILocalVariable
	{
		T result = null;
		if (collection != null)
		{
			LocalSymbol<T> localSymbol = collection[name];
			if (localSymbol != null && (lexicalScope == null || localSymbol.LexicalScope.IsVisibleTo(lexicalScope)))
			{
				return localSymbol.Variable;
			}
		}
		return result;
	}

	public bool Add(IScalarVariable variable, LexicalScope lexicalScope)
	{
		return Add(variable, lexicalScope, ref m_scalarVariables);
	}

	public bool Add(ITableVariable variable, LexicalScope lexicalScope)
	{
		return Add(variable, lexicalScope, ref m_tableVariables);
	}

	public bool Add(ICursorVariable variable, LexicalScope lexicalScope)
	{
		return Add(variable, lexicalScope, ref m_cursorVariables);
	}

	private bool Add<T>(T variable, LexicalScope lexicalScope, ref DictionaryCollection<LocalSymbol<T>> collection) where T : class, ILocalVariable
	{
		bool flag;
		if (!Contains(variable.Name))
		{
			if (collection == null)
			{
				collection = new DictionaryCollection<LocalSymbol<T>>(32, m_collationInfo);
			}
			collection.Add(new LocalSymbol<T>(variable, lexicalScope));
			flag = false;
		}
		else
		{
			flag = true;
		}
		return !flag;
	}

	public LexicalScope GetDeclarationScope(ILocalVariable variable)
	{
		string name = variable.Name;
		return GetDeclarationScope(name, m_scalarVariables) ?? GetDeclarationScope(name, m_tableVariables) ?? GetDeclarationScope(name, m_cursorVariables);
	}

	private static LexicalScope GetDeclarationScope<T>(string name, DictionaryCollection<LocalSymbol<T>> collection) where T : class, ILocalVariable
	{
		return collection?[name]?.LexicalScope;
	}

	public IMetadataCollection<IScalarVariable> GetScalarVariablesAt(LexicalScope lexicalScope)
	{
		return GetVariablesAt(lexicalScope, m_scalarVariables);
	}

	public IMetadataCollection<ITableVariable> GetTableVariablesAt(LexicalScope lexicalScope)
	{
		return GetVariablesAt(lexicalScope, m_tableVariables);
	}

	public IMetadataCollection<ICursorVariable> GetCursorVariablesAt(LexicalScope lexicalScope)
	{
		return GetVariablesAt(lexicalScope, m_cursorVariables);
	}

	private IMetadataCollection<T> GetVariablesAt<T>(LexicalScope lexicalScope, DictionaryCollection<LocalSymbol<T>> collection) where T : class, ILocalVariable
	{
		if (collection != null)
		{
			T[] array = new T[collection.Count];
			int count = 0;
			foreach (LocalSymbol<T> item in collection)
			{
				if (item.LexicalScope.IsVisibleTo(lexicalScope))
				{
					array[count++] = item.Variable;
				}
			}
			return Collection<T>.CreateOrderedCollection(m_collationInfo, array, count);
		}
		return Collection<T>.Empty;
	}

	private bool Contains(string name)
	{
		if (!Contains(name, m_scalarVariables) && !Contains(name, m_tableVariables))
		{
			return Contains(name, m_cursorVariables);
		}
		return true;
	}

	private static bool Contains<T>(string name, DictionaryCollection<LocalSymbol<T>> collection) where T : class, ILocalVariable
	{
		return collection?.Contains(name) ?? false;
	}
}
