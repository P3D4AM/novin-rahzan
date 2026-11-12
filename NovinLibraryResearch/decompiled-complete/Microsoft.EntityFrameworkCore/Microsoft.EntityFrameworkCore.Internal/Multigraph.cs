using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Microsoft.EntityFrameworkCore.Internal;

public class Multigraph<TVertex, TEdge> : Graph<TVertex>
{
	private readonly HashSet<TVertex> _vertices = new HashSet<TVertex>();

	private readonly Dictionary<TVertex, Dictionary<TVertex, List<TEdge>>> _successorMap = new Dictionary<TVertex, Dictionary<TVertex, List<TEdge>>>();

	public virtual IEnumerable<TEdge> Edges => _successorMap.Values.SelectMany((Dictionary<TVertex, List<TEdge>> s) => s.Values).SelectMany((List<TEdge> e) => e).Distinct();

	public override IEnumerable<TVertex> Vertices => _vertices;

	protected virtual string ToString([JetBrains.Annotations.NotNull] TVertex vertex)
	{
		return vertex.ToString();
	}

	public virtual IEnumerable<TEdge> GetEdges([JetBrains.Annotations.NotNull] TVertex from, [JetBrains.Annotations.NotNull] TVertex to)
	{
		if (_successorMap.TryGetValue(from, out var value) && value.TryGetValue(to, out var value2))
		{
			return value2;
		}
		return Enumerable.Empty<TEdge>();
	}

	public virtual void AddVertex([JetBrains.Annotations.NotNull] TVertex vertex)
	{
		_vertices.Add(vertex);
	}

	public virtual void AddVertices([JetBrains.Annotations.NotNull] IEnumerable<TVertex> vertices)
	{
		_vertices.UnionWith(vertices);
	}

	public virtual void AddEdge([JetBrains.Annotations.NotNull] TVertex from, [JetBrains.Annotations.NotNull] TVertex to, [JetBrains.Annotations.CanBeNull] TEdge edge)
	{
		AddEdges(from, to, new TEdge[1] { edge });
	}

	public virtual void AddEdges([JetBrains.Annotations.NotNull] TVertex from, [JetBrains.Annotations.NotNull] TVertex to, [JetBrains.Annotations.NotNull] IEnumerable<TEdge> edges)
	{
		if (!_vertices.Contains(from))
		{
			throw new InvalidOperationException(CoreStrings.GraphDoesNotContainVertex(from));
		}
		if (!_vertices.Contains(to))
		{
			throw new InvalidOperationException(CoreStrings.GraphDoesNotContainVertex(to));
		}
		if (!_successorMap.TryGetValue(from, out var value))
		{
			value = new Dictionary<TVertex, List<TEdge>>();
			_successorMap.Add(from, value);
		}
		if (!value.TryGetValue(to, out var value2))
		{
			value2 = new List<TEdge>();
			value.Add(to, value2);
		}
		value2.AddRange(edges);
	}

	public virtual IReadOnlyList<TVertex> TopologicalSort()
	{
		return TopologicalSort(null, null);
	}

	public virtual IReadOnlyList<TVertex> TopologicalSort([JetBrains.Annotations.CanBeNull] Func<TVertex, TVertex, IEnumerable<TEdge>, bool> canBreakEdge)
	{
		return TopologicalSort(canBreakEdge, null);
	}

	public virtual IReadOnlyList<TVertex> TopologicalSort([JetBrains.Annotations.CanBeNull] Func<IEnumerable<Tuple<TVertex, TVertex, IEnumerable<TEdge>>>, string> formatCycle)
	{
		return TopologicalSort(null, formatCycle);
	}

	public virtual IReadOnlyList<TVertex> TopologicalSort([JetBrains.Annotations.CanBeNull] Func<TVertex, TVertex, IEnumerable<TEdge>, bool> canBreakEdge, [JetBrains.Annotations.CanBeNull] Func<IReadOnlyList<Tuple<TVertex, TVertex, IEnumerable<TEdge>>>, string> formatCycle)
	{
		List<TVertex> list = new List<TVertex>();
		Dictionary<TVertex, int> predecessorCounts = new Dictionary<TVertex, int>();
		foreach (TVertex vertex in _vertices)
		{
			foreach (TVertex outgoingNeighbor in GetOutgoingNeighbors(vertex))
			{
				if (predecessorCounts.ContainsKey(outgoingNeighbor))
				{
					predecessorCounts[outgoingNeighbor]++;
				}
				else
				{
					predecessorCounts[outgoingNeighbor] = 1;
				}
			}
		}
		foreach (TVertex vertex2 in _vertices)
		{
			if (!predecessorCounts.ContainsKey(vertex2))
			{
				list.Add(vertex2);
			}
		}
		int i = 0;
		while (list.Count < _vertices.Count)
		{
			for (; i < list.Count; i++)
			{
				TVertex from = list[i];
				foreach (TVertex item in from neighbor in GetOutgoingNeighbors(@from)
					where predecessorCounts.ContainsKey(neighbor)
					select neighbor)
				{
					predecessorCounts[item]--;
					if (predecessorCounts[item] == 0)
					{
						list.Add(item);
						predecessorCounts.Remove(item);
					}
				}
			}
			if (list.Count >= _vertices.Count)
			{
				continue;
			}
			bool flag = false;
			List<TVertex> list2 = predecessorCounts.Keys.ToList();
			for (int j = 0; j < list2.Count; j++)
			{
				if (flag)
				{
					break;
				}
				if (canBreakEdge == null)
				{
					break;
				}
				TVertex val = list2[j];
				foreach (TVertex item2 in (from neighbor in GetIncomingNeighbors(val)
					where predecessorCounts.ContainsKey(neighbor)
					select neighbor).ToList())
				{
					if (canBreakEdge(item2, val, _successorMap[item2][val]))
					{
						predecessorCounts[val]--;
						if (predecessorCounts[val] == 0)
						{
							list.Add(val);
							predecessorCounts.Remove(val);
							flag = true;
							break;
						}
					}
				}
			}
			if (flag)
			{
				continue;
			}
			TVertex val2 = _vertices.First((TVertex v) => predecessorCounts.ContainsKey(v));
			List<TVertex> list3 = new List<TVertex> { val2 };
			bool flag2 = false;
			while (!flag2)
			{
				foreach (TVertex item3 in from neighbor in GetIncomingNeighbors(val2)
					where predecessorCounts.ContainsKey(neighbor)
					select neighbor)
				{
					if (predecessorCounts[item3] != 0)
					{
						predecessorCounts[val2] = -1;
						val2 = item3;
						list3.Add(val2);
						flag2 = predecessorCounts[item3] == -1;
						break;
					}
				}
			}
			list3.Reverse();
			ThrowCycle(list3, formatCycle);
		}
		return list;
	}

	private void ThrowCycle(List<TVertex> cycle, Func<IReadOnlyList<Tuple<TVertex, TVertex, IEnumerable<TEdge>>>, string> formatCycle)
	{
		string cycle2;
		if (formatCycle == null)
		{
			cycle2 = cycle.Select(ToString).Join(" -> ");
		}
		else
		{
			TVertex val = cycle.First();
			List<Tuple<TVertex, TVertex, IEnumerable<TEdge>>> list = new List<Tuple<TVertex, TVertex, IEnumerable<TEdge>>>();
			foreach (TVertex item in cycle.Skip(1))
			{
				list.Add(Tuple.Create(val, item, GetEdges(val, item)));
				val = item;
			}
			cycle2 = formatCycle(list);
		}
		throw new InvalidOperationException(CoreStrings.CircularDependency(cycle2));
	}

	public virtual IReadOnlyList<List<TVertex>> BatchingTopologicalSort()
	{
		return BatchingTopologicalSort(null);
	}

	public virtual IReadOnlyList<List<TVertex>> BatchingTopologicalSort([JetBrains.Annotations.CanBeNull] Func<IReadOnlyList<Tuple<TVertex, TVertex, IEnumerable<TEdge>>>, string> formatCycle)
	{
		List<TVertex> list = new List<TVertex>();
		Dictionary<TVertex, int> predecessorCounts = new Dictionary<TVertex, int>();
		foreach (TVertex vertex in _vertices)
		{
			foreach (TVertex outgoingNeighbor in GetOutgoingNeighbors(vertex))
			{
				if (predecessorCounts.ContainsKey(outgoingNeighbor))
				{
					predecessorCounts[outgoingNeighbor]++;
				}
				else
				{
					predecessorCounts[outgoingNeighbor] = 1;
				}
			}
		}
		foreach (TVertex vertex2 in _vertices)
		{
			if (!predecessorCounts.ContainsKey(vertex2))
			{
				list.Add(vertex2);
			}
		}
		List<List<TVertex>> list2 = new List<List<TVertex>>();
		List<TVertex> list3 = new List<TVertex>();
		int num = 0;
		while (num < list.Count)
		{
			TVertex from = list[num];
			num++;
			foreach (TVertex outgoingNeighbor2 in GetOutgoingNeighbors(from))
			{
				predecessorCounts[outgoingNeighbor2]--;
				if (predecessorCounts[outgoingNeighbor2] == 0)
				{
					list3.Add(outgoingNeighbor2);
				}
			}
			if (num == list.Count)
			{
				list2.Add(list);
				list = list3;
				num = 0;
				if (list.Count != 0)
				{
					list3 = new List<TVertex>();
				}
			}
		}
		if (list2.Sum((List<TVertex> b) => b.Count) != _vertices.Count)
		{
			int value2;
			TVertex val = _vertices.First((TVertex v) => predecessorCounts.TryGetValue(v, out value2) && value2 != 0);
			List<TVertex> list4 = new List<TVertex> { val };
			bool flag = false;
			while (!flag)
			{
				foreach (TVertex incomingNeighbor in GetIncomingNeighbors(val))
				{
					if (predecessorCounts.TryGetValue(incomingNeighbor, out var value) && value != 0)
					{
						predecessorCounts[val] = -1;
						val = incomingNeighbor;
						list4.Add(val);
						flag = predecessorCounts[incomingNeighbor] == -1;
						break;
					}
				}
			}
			list4.Reverse();
			List<TVertex> list5 = new List<TVertex>();
			TVertex val2 = list4.First();
			list5.Add(val2);
			foreach (TVertex item in list4.Skip(1))
			{
				if (!item.Equals(val2))
				{
					list5.Add(item);
					continue;
				}
				break;
			}
			list5.Add(val2);
			ThrowCycle(list5, formatCycle);
		}
		return list2;
	}

	public override IEnumerable<TVertex> GetOutgoingNeighbors(TVertex from)
	{
		if (!_successorMap.TryGetValue(from, out var value))
		{
			return Enumerable.Empty<TVertex>();
		}
		return value.Keys;
	}

	public override IEnumerable<TVertex> GetIncomingNeighbors(TVertex to)
	{
		return from kvp in _successorMap
			where kvp.Value.ContainsKey(to)
			select kvp.Key;
	}
}
