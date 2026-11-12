using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

public abstract class Graph<TVertex>
{
	public abstract IEnumerable<TVertex> Vertices { get; }

	public abstract IEnumerable<TVertex> GetOutgoingNeighbors([JetBrains.Annotations.NotNull] TVertex from);

	public abstract IEnumerable<TVertex> GetIncomingNeighbors([JetBrains.Annotations.NotNull] TVertex to);

	public virtual ISet<TVertex> GetUnreachableVertices([JetBrains.Annotations.NotNull] IReadOnlyList<TVertex> roots)
	{
		HashSet<TVertex> hashSet = new HashSet<TVertex>(Vertices);
		hashSet.ExceptWith(roots);
		List<TVertex> list = new List<TVertex>(roots);
		int num = 0;
		while (num < list.Count)
		{
			TVertex from = list[num];
			num++;
			foreach (TVertex outgoingNeighbor in GetOutgoingNeighbors(from))
			{
				if (hashSet.Remove(outgoingNeighbor))
				{
					list.Add(outgoingNeighbor);
				}
			}
		}
		return hashSet;
	}
}
