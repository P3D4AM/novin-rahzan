using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Interfaces;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Core;

public abstract class BaseSanadBusiness<T> : BaseBusiness<T>, IHasChangeStatus, IFileAttachment, IHasJustGuid
{
	private List<FileInfoBusiness> _addedFileList = new List<FileInfoBusiness>();

	private List<FileInfoBusiness> _removedFileList = new List<FileInfoBusiness>();

	public abstract Document SanadType { get; }

	public abstract Guid Guid { get; set; }

	public FileManagement_Parts Part { get; }

	public IReadOnlyCollection<FileInfoBusiness> AddedFiles => _addedFileList;

	public IReadOnlyCollection<FileInfoBusiness> RemovedFiles => _removedFileList;

	public abstract override EntityChangeSt ChangeStatus { get; set; }

	public abstract bool GetIsTemp();

	public abstract DateTime GetDate();

	public abstract Task<int> GetSanadNumberAsync(CancellationToken token, string tranName);

	public abstract Task<BaseSanadBusiness<T>> GetOldItemAsync(EnSystem system, CancellationToken token, string tranName);

	protected async Task<ReturnedSaveFuncInfo> CheckSanadValidationAsync(EnSystem system, bool isDeleteAction, int id2, string tranName, CancellationToken token, params Tuple<Document, Guid>[] relatedSanads)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
			{
				return ret;
			}
			List<Tuple<Document, Guid>> relatedSanadItems = relatedSanads?.ToList() ?? new List<Tuple<Document, Guid>>();
			relatedSanadItems.Add(new Tuple<Document, Guid>(SanadType, Guid));
			relatedSanadItems = relatedSanadItems.Where((Tuple<Document, Guid> q) => q != null)?.ToList();
			if (SanadType != Document.PishFactor && SanadType != Document.OrderPreBuy && !GetIsTemp() && system != EnSystem.Transfer)
			{
				SanadBusiness oldSanad = null;
				if (ChangeStatus != EntityChangeSt.New)
				{
					BaseSanadBusiness<T> baseSanadBusiness = await GetOldItemAsync(system, token, tranName);
					if (baseSanadBusiness != null)
					{
						string contextGuid = ContextGuid;
						oldSanad = await SanadBusiness.GetAsync(contextGuid, await baseSanadBusiness.GetSanadNumberAsync(token, tranName), token, tranName);
						SanadBusiness sanadBusiness = oldSanad;
						if (sanadBusiness != null && sanadBusiness.SanadSt == SanadSt.Daemi)
						{
							ret.AddError(system, $"{SanadType.GetDisplay().GetTranslate()}({id2}) " + " " + "به علت دائمی شدن سند غیر قابل ویرایش می باشد".GetTranslate(), "", "CheckSanadValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseSanadBusiness.cs", 52);
						}
					}
				}
				bool flag = oldSanad != null;
				if (flag)
				{
					flag = await oldSanad.GetSanadNoAsync(token, tranName) == await GetSanadNumberAsync(token, tranName);
				}
				if (flag)
				{
					if ((oldSanad?.ListSanad_Det?.Any((Sanad_DetBusiness p) => !p.CheckIsContain(relatedSanadItems))).GetValueOrDefault() && oldSanad.DateM.Date != GetDate().Date && !isDeleteAction)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						EnSystem system2 = system;
						string contextGuid = "تاریخ سند شماره".GetTranslate();
						int num = await GetSanadNumberAsync(token, tranName);
						returnedSaveFuncInfo.AddError(system2, contextGuid + " " + $" ({num})\r\n" + ". با برگه جاری قابل تجمیع نمی باشد".GetTranslate() + "\r\n" + "تاریخ سند تجمیعی جاری".GetTranslate() + ":" + oldSanad.DateSh + "\r\n" + "تاریخ".GetTranslate() + "  " + SanadType.GetDisplay().GetTranslate() + ":" + GetDate().MiladiToCalture(), "SanadNo", "CheckSanadValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseSanadBusiness.cs", 65);
					}
				}
				else
				{
					string contextGuid = ContextGuid;
					SanadBusiness existsNewSanadOnDatabase = await SanadBusiness.GetAsync(contextGuid, await GetSanadNumberAsync(token, tranName), token, tranName);
					if ((existsNewSanadOnDatabase?.ListSanad_Det?.Any((Sanad_DetBusiness p) => !p.CheckIsContain(relatedSanadItems))).GetValueOrDefault() && existsNewSanadOnDatabase.DateM.Date != GetDate().Date)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						EnSystem system2 = system;
						contextGuid = "تاریخ سند شماره".GetTranslate();
						int num = await GetSanadNumberAsync(token, tranName);
						returnedSaveFuncInfo.AddError(system2, contextGuid + " " + $"({num})\r\n" + ".با برگه جاری قابل تجمیع نمی باشد".GetTranslate() + "\r\n" + "تاریخ سند تجمیعی جدید".GetTranslate() + ":" + existsNewSanadOnDatabase.DateSh + "\r\n" + "تاریخ".GetTranslate() + " " + SanadType.GetDisplay().GetTranslate() + ":" + GetDate().MiladiToCalture(), "", "CheckSanadValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseSanadBusiness.cs", 76);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckSanadValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseSanadBusiness.cs", 85);
			ret.AddReturnedValue(ex, "", "CheckSanadValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseSanadBusiness.cs", 86);
		}
		return ret;
	}

	public Task AddFileAsync(FileInfoBusiness file)
	{
		return BaseFileAttachmentBusiness.AddFileAsync(file, _addedFileList);
	}

	public Task RemoveFileAsync(FileInfoBusiness file)
	{
		return BaseFileAttachmentBusiness.RemoveFileAsync(file, _removedFileList);
	}
}
