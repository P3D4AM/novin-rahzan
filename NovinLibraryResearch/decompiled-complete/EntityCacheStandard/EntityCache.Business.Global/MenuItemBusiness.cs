using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Utilities;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;
using PacketParser.EventArgs.Sanad;

namespace EntityCache.Business.Global;

public class MenuItemBusiness : Serializable<MenuItemBusiness>, IMenuItem, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasContext, INotifyPropertyChanged
{
	private bool _isDisposed;

	private string _title = "";

	private CancellationTokenSource _fillTitleForDisplayToken;

	private MenuBusiness _master;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


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

	public EnMenuItemType Type { get; set; }

	public Guid TypeGuid { get; set; }

	public int Ordinal { get; set; }

	public Guid MasterGuid { get; set; }

	public string ContextGuid { get; set; }

	public byte[] InsertImage => ImageResourceManager.MenuInsert;

	public byte[] UpdateImage => ImageResourceManager.MenuUpdate;

	public byte[] DeleteImage => ImageResourceManager.MenuDelete;

	public MenuBusiness Master => _master;

	public event PropertyChangedEventHandler PropertyChanged;

	public event Func<object, EventArgs, Task> OnDisposed;

	public event Func<object, OrdinalChangedEventArgs, Task> OnOrdinalChanged;

	public MenuItemBusiness(string contextGuid, MenuBusiness masterMenu)
	{
		ContextGuid = contextGuid;
		_master = masterMenu;
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		checked
		{
			try
			{
				if (_isDisposed)
				{
					return;
				}
				_isDisposed = true;
				List<MenuItemBusiness> list = (from p in Master?.DetailList?.Where((MenuItemBusiness p) => p.Guid != Guid)
					orderby p.Ordinal
					select p)?.ToList();
				if (list != null && list.Any())
				{
					for (int i = 0; i < list.Count(); i++)
					{
						list[i].Ordinal = i + 1;
					}
				}
				if (this.OnDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuItemBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuItemBusiness.cs", 74);
			}
		}
	}

	private async Task UpdateTitleAsync(CancellationToken token)
	{
		_ = 4;
		try
		{
			await Task.Delay(new Random().Next(500));
			if (token.IsCancellationRequested || base.IsDisposed)
			{
				return;
			}
			if (Type == EnMenuItemType.Menu)
			{
				MenuBusiness menuBusiness = await MenuBusiness.GetAsync(ContextGuid, TypeGuid);
				if (menuBusiness != null)
				{
					_title = "منوی".GetTranslate() + " " + $" {menuBusiness.RowCount} * {menuBusiness.ColumnCount}";
				}
			}
			else
			{
				CardBusiness cardBusiness = await CardBusiness.GetAsync(ContextGuid, TypeGuid, token);
				if (!(cardBusiness?.TargetGuidList?.Any()).GetValueOrDefault())
				{
					_title = "کارت".GetTranslate() + "  " + cardBusiness.Type.GetDisplay().GetTranslate();
				}
				else
				{
					switch (cardBusiness.Type)
					{
					case EnCardType.GoToMenu:
					{
						MenuBusiness menuBusiness2 = await MenuBusiness.GetAsync(ContextGuid, cardBusiness.TargetGuidList.First(), token);
						if (menuBusiness2 != null)
						{
							_title = "کارت".GetTranslate() + " " + (menuBusiness2.Title ?? "").GetTranslate() + " " + $" ({menuBusiness2.RowCount} * {menuBusiness2.ColumnCount})";
						}
						break;
					}
					case EnCardType.IncreaseProduct:
					{
						ProductCompactBusiness productCompactBusiness = await ProductCompactBusiness.GetAsync(ContextGuid, cardBusiness.TargetGuidList.First(), EnSystem.UserInterface, null, "", token, true);
						if (productCompactBusiness != null)
						{
							_title = "کارت کالای".GetTranslate() + " " + (productCompactBusiness.Name ?? "").GetTranslate();
						}
						break;
					}
					default:
						_title = "کارت عملیات".GetTranslate() + "  " + cardBusiness.Type.GetDisplay().GetTranslate();
						break;
					}
				}
			}
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
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
					WebErrorLog.Instance.StartLog(ex2, "", "UpdateTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuItemBusiness.cs", 111);
				}
			}, "UpdateTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuItemBusiness.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuItemBusiness.cs", 113);
		}
	}

	public void SetOrdinalChange(EnSystem system, int value, object sender, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!base.IsDisposed)
			{
				if (value < 0)
				{
					throw new ArgumentException($"value({value}) is not valid for SanadDetail.Ordinal");
				}
				int ordinal = Ordinal;
				Ordinal = value;
				if (system == EnSystem.UserInterface && Ordinal >= 0 && this.OnOrdinalChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnOrdinalChanged, sender, new OrdinalChangedEventArgs(ordinal, value, Guid, system), token, 60, "SetOrdinalChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuItemBusiness.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetOrdinalChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuItemBusiness.cs", 126);
		}
	}

	public async Task<EnCardType> GetCardTypeAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (Type == EnMenuItemType.Menu)
			{
				return EnCardType.GoToMenu;
			}
			if (TypeGuid != Guid.Empty)
			{
				if (await ProductCompactBusiness.GetAsync(ContextGuid, TypeGuid, EnSystem.UserInterface, null, tranName, token, true) != null)
				{
					return EnCardType.IncreaseProduct;
				}
				if (await PrdGroupBusiness.GetAsync(ContextGuid, TypeGuid, token, tranName) != null)
				{
					return EnCardType.DynamicGroups;
				}
			}
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCardTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Menu\\MenuItemBusiness.cs", 145);
			throw;
		}
	}
}
