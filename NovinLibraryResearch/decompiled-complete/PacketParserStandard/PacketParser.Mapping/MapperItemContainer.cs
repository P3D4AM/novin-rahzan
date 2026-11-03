using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace PacketParser.Mapping;

public class MapperItemContainer<T> : IItemAsyncMapper<MapperItemContainer<T>, MapperItemContainer<T>>
{
	public T Value { get; set; }

	public MapperItemContainer(T value)
	{
		Value = value;
	}

	public MapperItemContainer()
	{
	}

	public Task<MapperItemContainer<T>> MapAsync(string contextGuid, MapperItemContainer<T> source, MapperItemContainer<T> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new MapperItemContainer<T>();
			}
			destination.Value = source.Value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Mapping\\MapperItemContainer.cs", 34);
		}
		return Task.FromResult(destination);
	}

	public async Task<List<MapperItemContainer<T>>> MapListAsync(string contextGuid, IEnumerable<MapperItemContainer<T>> source, List<MapperItemContainer<T>> destination, string tranName, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				return destination;
			}
			if (destination == null)
			{
				destination = new List<MapperItemContainer<T>>();
			}
			foreach (MapperItemContainer<T> item in source)
			{
				List<MapperItemContainer<T>> list = destination;
				list.Add(await MapAsync(contextGuid, item));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Mapping\\MapperItemContainer.cs", 47);
		}
		return destination;
	}
}
