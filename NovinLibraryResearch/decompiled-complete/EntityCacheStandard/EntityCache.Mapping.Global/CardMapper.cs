using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global;

public class CardMapper : IItemAsyncMapper<CardBusiness, WebCard>, IItemAsyncMapper<CardBusiness, CardBusiness>, IItemAsyncMapper<WebCard, CardBusiness>
{
	public static CardMapper Instance { get; private set; } = new CardMapper();


	public Task<WebCard> MapAsync(string contextGuid, CardBusiness source, WebCard destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebCard();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Theme = source.Theme;
			destination.ToolTipMessage = source.ToolTipMessage;
			destination.Image = source.Image;
			destination.BackColor = source.BackColor;
			destination.Type = source.Type;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\CardMapper.cs", 37);
			throw;
		}
	}

	public Task<CardBusiness> MapAsync(string contextGuid, CardBusiness source, CardBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CardBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Text1 = source.Text1;
			destination.Text2 = source.Text2;
			destination.Text3 = source.Text3;
			destination.Text4 = source.Text4;
			destination.Theme = source.Theme;
			destination.ToolTipMessage = source.ToolTipMessage;
			destination.Image = source.Image;
			destination.BackColor = source.BackColor;
			destination.Type = source.Type;
			destination.ContextGuid = contextGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\CardMapper.cs", 68);
			throw;
		}
	}

	public Task<CardBusiness> MapAsync(string contextGuid, WebCard source, CardBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CardBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Theme = source.Theme;
			destination.ToolTipMessage = source.ToolTipMessage;
			destination.Image = source.Image;
			destination.BackColor = source.BackColor;
			destination.Type = source.Type;
			destination.ContextGuid = contextGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\CardMapper.cs", 95);
			throw;
		}
	}

	public Task<List<WebCard>> MapListAsync(string contextGuid, IEnumerable<CardBusiness> source, List<WebCard> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CardBusiness>> MapListAsync(string contextGuid, IEnumerable<CardBusiness> source, List<CardBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CardBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCard> source, List<CardBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
