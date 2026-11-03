using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Global;

public class BaseCardBusiness : Serializable<BaseCardBusiness>, IHasContext
{
	private List<CardTargetRelationBusiness> _detList;

	public Guid Guid { get; set; }

	public CardAttribute Text1 { get; set; }

	public CardAttribute Text2 { get; set; }

	public CardAttribute Text3 { get; set; }

	public CardAttribute Text4 { get; set; }

	public EnCardType Type { get; set; }

	public IReadOnlyCollection<CardTargetRelationBusiness> DetailList
	{
		get
		{
			if (_detList == null)
			{
				_detList = new List<CardTargetRelationBusiness>();
			}
			_detList.RemoveAll((CardTargetRelationBusiness q) => q?.IsDisposed ?? true);
			return _detList;
		}
	}

	public List<Guid> TargetGuidList => DetailList?.Select((CardTargetRelationBusiness q) => q.TargetGuid)?.ToList();

	public CardBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.Card;

	public string ContextGuid { get; set; }

	public event Func<object, Guid, Task> OnDetailChanged;

	public event Func<object, EventArgs, Task> OnItemDisposed;

	public BaseCardBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task<string> GetText1Async(EnSystem system)
	{
		return Text1.GetTextStringAsync(ContextGuid, system, Type, TargetGuidList);
	}

	public Task<string> GetText2Async(EnSystem system)
	{
		return Text2.GetTextStringAsync(ContextGuid, system, Type, TargetGuidList);
	}

	public Task<string> GetText3Async(EnSystem system)
	{
		return Text3.GetTextStringAsync(ContextGuid, system, Type, TargetGuidList);
	}

	public Task<string> GetText4Async(EnSystem system)
	{
		return Text4.GetTextStringAsync(ContextGuid, system, Type, TargetGuidList);
	}

	public Task AddToDetailListAsync(CardTargetRelationBusiness _det, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_det == null)
			{
				return Task.CompletedTask;
			}
			if (_detList == null)
			{
				_detList = new List<CardTargetRelationBusiness>();
			}
			_det.OnDisposed += Det_OnDisposed;
			_detList.Add(_det);
			if (((BaseCardBusiness)EventManager).OnDetailChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(((BaseCardBusiness)EventManager).OnDetailChanged, this, _det.TargetGuid, token, 60, "AddToDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\BaseCardBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddToDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\BaseCardBusiness.cs", 55);
		}
		return Task.CompletedTask;
	}

	public Task AddToDetailListAsync(Guid targetGuid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (targetGuid == Guid.Empty)
			{
				return Task.CompletedTask;
			}
			if (_detList == null)
			{
				_detList = new List<CardTargetRelationBusiness>();
			}
			CardTargetRelationBusiness cardTargetRelationBusiness = new CardTargetRelationBusiness(ContextGuid)
			{
				CardGuid = Guid,
				ContextGuid = ContextGuid,
				TargetGuid = targetGuid
			};
			cardTargetRelationBusiness.OnDisposed += Det_OnDisposed;
			_detList.Add(cardTargetRelationBusiness);
			if (((BaseCardBusiness)EventManager).OnDetailChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(((BaseCardBusiness)EventManager).OnDetailChanged, this, cardTargetRelationBusiness.TargetGuid, token, 60, "AddToDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\BaseCardBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddToDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\BaseCardBusiness.cs", 75);
		}
		return Task.CompletedTask;
	}

	private Task Det_OnDisposed(object sender, EventArgs e)
	{
		using (new JaegerMonitor("Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\BaseCardBusiness.cs"))
		{
			try
			{
				if ((_detList?.ToList()?.Any()).GetValueOrDefault())
				{
					foreach (CardTargetRelationBusiness item in _detList?.ToList()?.Where((CardTargetRelationBusiness p) => p == null || p == sender)?.ToList() ?? new List<CardTargetRelationBusiness>())
					{
						item?.Dispose();
					}
				}
				_detList?.RemoveAll((CardTargetRelationBusiness p) => p == null || p == sender);
				if (((BaseCardBusiness)EventManager).OnItemDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(((BaseCardBusiness)EventManager).OnItemDisposed, this, new EventArgs(), default(CancellationToken), 60, "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\BaseCardBusiness.cs");
				}
				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\BaseCardBusiness.cs", 97);
				throw;
			}
		}
	}

	public Task RemoveDetailAsync(Guid targetGuid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			CardTargetRelationBusiness cardTargetRelationBusiness = _detList?.FirstOrDefault((CardTargetRelationBusiness q) => q.TargetGuid == targetGuid);
			if (cardTargetRelationBusiness == null)
			{
				return Task.CompletedTask;
			}
			_detList.Remove(cardTargetRelationBusiness);
			if (((BaseCardBusiness)EventManager).OnDetailChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(((BaseCardBusiness)EventManager).OnDetailChanged, this, targetGuid, token, 60, "RemoveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\BaseCardBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\BaseCardBusiness.cs", 111);
		}
		return Task.CompletedTask;
	}
}
