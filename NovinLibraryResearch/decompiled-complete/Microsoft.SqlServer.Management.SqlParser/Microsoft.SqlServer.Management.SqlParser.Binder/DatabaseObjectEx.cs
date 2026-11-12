using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal abstract class DatabaseObjectEx<P> : IDatabaseObject, IMetadataObject, IProxyMetadataObject where P : class, IDatabaseObject
{
	protected abstract class ObjectExCache<T, TEX> where T : class, IDatabaseObject where TEX : DatabaseObjectEx<T>, T
	{
		private readonly Dictionary<T, TEX> m_objectToObjectExMap;

		public TEX this[T proxyObject]
		{
			get
			{
				if (!m_objectToObjectExMap.TryGetValue(proxyObject, out var value))
				{
					value = CreateMetadataObjectEx(proxyObject);
					m_objectToObjectExMap.Add(proxyObject, value);
					if (value.m_proxyObject != proxyObject)
					{
						m_objectToObjectExMap.Add(value.m_proxyObject, value);
					}
				}
				return value;
			}
		}

		protected ObjectExCache(int initialCapacity)
		{
			m_objectToObjectExMap = new Dictionary<T, TEX>(initialCapacity);
		}

		public void Add(TEX metadataObjectEx)
		{
			m_objectToObjectExMap.Add(metadataObjectEx.m_proxyObject, metadataObjectEx);
		}

		protected abstract TEX CreateMetadataObjectEx(T proxyObject);
	}

	protected readonly P m_proxyObject;

	public P ProxyObject => m_proxyObject;

	public IDatabaseObject Parent => m_proxyObject.Parent;

	public bool IsSystemObject => m_proxyObject.IsSystemObject;

	public string Name => m_proxyObject.Name;

	IMetadataObject IProxyMetadataObject.UnderlyingObject => m_proxyObject;

	protected DatabaseObjectEx(P proxyObject)
	{
		m_proxyObject = proxyObject;
	}

	public override string ToString()
	{
		return m_proxyObject.ToString();
	}

	public T Accept<T>(IDatabaseObjectVisitor<T> visitor)
	{
		return m_proxyObject.Accept(visitor);
	}

	public T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		return m_proxyObject.Accept(visitor);
	}
}
