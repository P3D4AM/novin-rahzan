using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.DescriptionParser;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Compression;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace EntityCache.Business.Global;

public class CardBusiness : BaseCardBusiness, ICard, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, INotifyPropertyChanged
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private string _title = "";

	private CancellationTokenSource _fillTitleForDisplayToken;

	public EnCardTheme Theme { get; set; }

	public string ThemeString => Theme.GetDisplay().GetTranslate();

	public string ToolTipMessage { get; set; }

	public byte[] Image { get; set; }

	public string BackColor { get; set; }

	public string TypeString
	{
		get
		{
			try
			{
				if (base.Type == EnCardType.IncreaseProduct || base.Type == EnCardType.GoToMenu || base.Type == EnCardType.DynamicGroups)
				{
					return base.Type.GetDisplay().GetTranslate();
				}
				return "عملیات".GetTranslate();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TypeString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardBusiness.cs", 39);
			}
			return "";
		}
	}

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

	public event PropertyChangedEventHandler PropertyChanged;

	public async Task<List<string>> GetSuggestedItemsAsync(string contextGuid, EnSystem system, CancellationToken token)
	{
		List<string> result = new List<string>();
		try
		{
			if (base.Type == EnCardType.IncreaseProduct)
			{
				result = await ProductDescParser.GetProductDescListAsync(contextGuid, system, token);
			}
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSuggestedItemsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardBusiness.cs", 68);
			throw;
		}
	}

	public CardBusiness(string contextGuid)
		: base(contextGuid)
	{
	}

	public static async Task<string> GetTargetAsync(string contextGuid, EnCardType type, List<Guid> targetGuidList, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		checked
		{
			try
			{
				string text = "";
				if (type != EnCardType.IncreaseProduct && type != EnCardType.DynamicGroups && type != 0)
				{
					return type.GetDisplay();
				}
				if (!(targetGuidList?.Any() ?? false))
				{
					return text;
				}
				if (targetGuidList.Count <= 2)
				{
					foreach (Guid targetGuid in targetGuidList)
					{
						string text2 = text;
						text = text2 + await GetTargetAsync(contextGuid, type, targetGuid, system, token, tranName) + ", ";
					}
				}
				else
				{
					Guid guid = targetGuidList[0];
					Guid secondGuid = targetGuidList[1];
					string text2 = text;
					text = text2 + await GetTargetAsync(contextGuid, type, guid, system, token, tranName) + ", ";
					text2 = text;
					text = text2 + $"{await GetTargetAsync(contextGuid, type, secondGuid, system, token, tranName)} و {targetGuidList.Count - 2} داده دیگر";
				}
				if (text.EndsWith(", "))
				{
					text = text.Substring(0, text.Length - 2);
				}
				return text;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetTargetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardBusiness.cs", 102);
				throw;
			}
		}
	}

	public static async Task<string> GetTargetAsync(string contextGuid, EnCardType type, Guid guid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			string target = "";
			return type switch
			{
				EnCardType.GoToMenu => target + (await MenuBusiness.GetAsync(contextGuid, guid, token))?.Title, 
				EnCardType.IncreaseProduct => target + (await ProductCompactBusiness.GetAsync(contextGuid, guid, system, null, tranName, token, true))?.Name, 
				EnCardType.DynamicGroups => target + (await PrdGroupBusiness.GetAsync(contextGuid, guid, token, tranName))?.Name, 
				_ => target + type.GetDisplay(), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTargetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardBusiness.cs", 131);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CardBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardBusiness.cs", 145), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardBusiness.cs", 145);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).CacheRepository.Card.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardBusiness.cs", 147);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardBusiness.cs", 144);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardBusiness.cs", 151);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardBusiness.cs", 152);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(base.ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardBusiness.cs", 157), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardBusiness.cs", 157);
			}
		}
		return ret;
	}

	public static Task<CardBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Card.GetAsync(guid, token, tranName);
	}

	private async Task UpdateTitleAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			await Task.Delay(new Random().Next(500));
			if (token.IsCancellationRequested || base.IsDisposed)
			{
				return;
			}
			_title = await GetTargetAsync(base.ContextGuid, base.Type, base.TargetGuidList, EnSystem.UserInterface, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "UpdateTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardBusiness.cs", 178);
				}
			}, "UpdateTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardBusiness.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardBusiness.cs", 181);
		}
	}

	public static Task<IEnumerable<CardBusiness>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Card.GetAllAsync(token, tranName);
	}

	public static async Task<CardBusiness> GenerateWithDefualtsAsync(string contextGuid, EnSystem system, EnCardType type, Guid? priceTypeGuid, Guid? guid = null, Guid? targetGuid = null, CancellationToken token = default(CancellationToken))
	{
		_ = 5;
		try
		{
			string name = "";
			string price = "";
			CardBusiness item = new CardBusiness(contextGuid)
			{
				BackColor = "#dae1fe",
				Modified = DateTime.Now,
				Status = true,
				Type = type
			};
			if (targetGuid.HasValue && targetGuid != Guid.Empty)
			{
				switch (type)
				{
				case EnCardType.IncreaseProduct:
				{
					ProductCompactBusiness prd = await ProductCompactBusiness.GetAsync(contextGuid, targetGuid.Value, EnSystem.UserInterface, null, "", token, true);
					name = prd?.Name;
					if (prd != null && (priceTypeGuid ?? Guid.Empty) != Guid.Empty)
					{
						price = (await prd.GetSellAsync(system, priceTypeGuid.Value, isLoadOriginalPrice: false)).NpToString("N0", 0);
					}
					if (string.IsNullOrEmpty(prd.Pic))
					{
						item.Theme = EnCardTheme.Theme1;
						break;
					}
					item.Theme = EnCardTheme.Theme9;
					FileInfoBusiness fileInfoBusiness = (await FileInfoBusiness.GetAsync(contextGuid, prd.Guid, FileManagement_Parts.PicPrd, token))?.Where((FileInfoBusiness q) => q.Status)?.SingleOrDefault();
					if (fileInfoBusiness != null)
					{
						if (fileInfoBusiness.CompressMode != 0)
						{
							CardBusiness cardBusiness = item;
							cardBusiness.Image = await GZip.Decompress(fileInfoBusiness.FileContent);
						}
						else
						{
							item.Image = fileInfoBusiness.FileContent;
						}
					}
					break;
				}
				case EnCardType.DynamicGroups:
					name = (await PrdGroupBusiness.GetAsync(contextGuid, targetGuid.Value, token))?.Name;
					item.Theme = EnCardTheme.Theme1;
					break;
				}
			}
			if (guid.HasValue && guid != Guid.Empty)
			{
				item.Guid = guid.Value;
			}
			if ((targetGuid ?? Guid.Empty) != Guid.Empty)
			{
				await item.AddToDetailListAsync(targetGuid.Value, token);
			}
			item.Text1 = new CardAttribute
			{
				Bold = true,
				FontName = "B Titr",
				FontSize = ((type == EnCardType.DynamicGroups) ? 12 : 10),
				ForeColor = "#4949fd",
				IsItalic = false,
				Text = name,
				TextAlign = ContentAlignment.MiddleCenter
			};
			if (!string.IsNullOrEmpty(price))
			{
				item.Text2 = new CardAttribute
				{
					Bold = true,
					FontName = "B Titr",
					FontSize = 10f,
					ForeColor = "#4949fd",
					IsItalic = false,
					Text = price,
					TextAlign = ContentAlignment.MiddleCenter
				};
			}
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateWithDefualtsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardBusiness.cs", 264);
			throw;
		}
	}
}
