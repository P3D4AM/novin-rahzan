using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Utilities;
using PacketParser.Assistance;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace EntityCache.Business.Global;

public class CardTargetRelationBusiness : Serializable<CardTargetRelationBusiness>, ICardTargetRelation, IHasContext, INotifyPropertyChanged
{
	private string _title = "";

	private CancellationTokenSource _fillTitleForDisplayToken;

	public Guid CardGuid { get; set; }

	public Guid TargetGuid { get; set; }

	public string Title
	{
		get
		{
			if (!string.IsNullOrEmpty(_title))
			{
				return _title;
			}
			_fillTitleForDisplayToken?.Cancel();
			_fillTitleForDisplayToken = new CancellationTokenSource(60000);
			CancellationToken token = _fillTitleForDisplayToken.Token;
			UpdateTitleAsync(token);
			return _title;
		}
	}

	public string ContextGuid { get; set; }

	public byte[] DeleteImage => ImageResourceManager.OrderDeleteImage;

	public event Func<object, EventArgs, Task> OnDisposed;

	public event PropertyChangedEventHandler PropertyChanged;

	public CardTargetRelationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	private async Task UpdateTitleAsync(CancellationToken token)
	{
		_ = 3;
		try
		{
			await Task.Delay(new Random().Next(500));
			if (token.IsCancellationRequested || base.IsDisposed)
			{
				return;
			}
			_title = await GetTargetAsync(ContextGuid, EnSystem.UserInterface, token);
			if (string.IsNullOrEmpty(_title))
			{
				_title = (await ProductCompactBusiness.GetAsync(ContextGuid, TargetGuid, EnSystem.UserInterface, null, "", default(CancellationToken), true))?.Name;
			}
			if (string.IsNullOrEmpty(_title))
			{
				_title = (await PrdGroupBusiness.GetAsync(ContextGuid, TargetGuid))?.Name;
			}
			if (token.IsCancellationRequested || base.IsDisposed || this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged(this, new PropertyChangedEventArgs("Title"));
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "UpdateTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardTargetRelationBusiness.cs", 54);
				}
			}, "UpdateTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardTargetRelationBusiness.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardTargetRelationBusiness.cs", 56);
		}
	}

	private async Task<string> GetTargetAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			string target = "";
			CardBusiness cardBusiness = await CardBusiness.GetAsync(contextGuid, CardGuid, token, tranName);
			if (cardBusiness == null)
			{
				return target;
			}
			return await CardBusiness.GetTargetAsync(contextGuid, cardBusiness.Type, TargetGuid, system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTargetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardTargetRelationBusiness.cs", 70);
			throw;
		}
	}
}
