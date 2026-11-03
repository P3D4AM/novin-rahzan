using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace PacketParser.Mapping;

public static class MapperUtilities
{
	public static async Task<List<TDestination>> MapListAsync<TSource, TDestination>(this IEnumerable<TSource> source, string contextGuid, IItemAsyncMapper<TSource, TDestination> mapper, List<TDestination> destination = null, string tranName = "", short maxParalelAsyncTasks = 20) where TSource : class where TDestination : class
	{
		Stopwatch sw = Stopwatch.StartNew();
		using (new JaegerMonitor("MapListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Mapping\\MapperUtilities.cs"))
		{
			_ = 2;
			try
			{
				List<Task<TDestination>> taskList = new List<Task<TDestination>>();
				if (source == null)
				{
					return destination;
				}
				if (destination == null)
				{
					destination = new List<TDestination>();
				}
				if (!string.IsNullOrEmpty(tranName))
				{
					maxParalelAsyncTasks = 1;
				}
				List<TDestination> list;
				foreach (TSource item in source)
				{
					if (item == null)
					{
						destination.Add(null);
						continue;
					}
					taskList.Add(mapper.MapAsync(contextGuid, item, null, tranName));
					if (taskList.Count >= maxParalelAsyncTasks)
					{
						Task<TDestination> t = await Task.WhenAny(taskList);
						list = destination;
						list.Add(await t);
						taskList.Remove(t);
					}
				}
				list = destination;
				list.AddRange(await Task.WhenAll(taskList));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Mapping\\MapperUtilities.cs", 99);
			}
			finally
			{
				string arg = $"Count({source?.Count() ?? (-1)})/map from {typeof(TSource).Name} to {typeof(TDestination).Name}";
				if (sw.ElapsedMilliseconds > 30000)
				{
					WebErrorLog.Instance.StartLog($"MapList time {sw.ElapsedMilliseconds}(ms) {arg}", "", "MapListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Mapping\\MapperUtilities.cs", 104);
				}
			}
			return destination;
		}
	}
}
