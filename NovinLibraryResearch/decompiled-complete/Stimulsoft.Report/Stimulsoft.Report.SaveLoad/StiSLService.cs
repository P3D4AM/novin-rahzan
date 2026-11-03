using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.SaveLoad;

[StiServiceBitmap(typeof(StiSLService), "Stimulsoft.Report.Bmp.SL.SL.bmp")]
[StiServiceCategoryBitmap(typeof(StiSLService), "Stimulsoft.Report.Bmp.SL.SLCategory.bmp")]
public abstract class StiSLService : StiService
{
	public sealed override string ServiceCategory => StiLocalization.Get("Services", "categorySL");

	public event EventHandler Saving;

	public event EventHandler Loading;

	protected virtual void OnSaving(EventArgs e)
	{
	}

	protected void InvokeSaving()
	{
		OnSaving(EventArgs.Empty);
		this.Saving?.Invoke(null, EventArgs.Empty);
	}

	protected void OnSaving(object sender, EventArgs e)
	{
		InvokeSaving();
	}

	protected virtual void OnLoading(EventArgs e)
	{
	}

	protected void InvokeLoading()
	{
		OnLoading(EventArgs.Empty);
		this.Loading?.Invoke(null, EventArgs.Empty);
	}

	protected void OnLoading(object sender, EventArgs e)
	{
		InvokeLoading();
	}

	public abstract StiSLActions GetAction();

	public abstract string GetFilter();

	public static List<StiReportSLService> GetReportSLServices(StiSLActions action)
	{
		return (from s in StiOptions.Services.ReportSLs
			where ((StiService)s).ServiceEnabled
			where (s.GetAction() & action) > (StiSLActions)0
			select s).ToList();
	}

	public static List<StiPageSLService> GetPageSLServices(StiSLActions action)
	{
		return (from s in StiOptions.Services.PageSLs
			where ((StiService)s).ServiceEnabled
			where (s.GetAction() & action) > (StiSLActions)0
			select s).ToList();
	}

	public static List<StiDocumentSLService> GetDocumentSLServices(StiSLActions action)
	{
		return (from s in StiOptions.Services.DocumentSLs
			where ((StiService)s).ServiceEnabled
			where (s.GetAction() & action) > (StiSLActions)0
			select s).ToList();
	}

	public static List<StiDictionarySLService> GetDictionarySLServices(StiSLActions action)
	{
		return (from s in StiOptions.Services.DictionarySLs
			where ((StiService)s).ServiceEnabled
			where (s.GetAction() & action) > (StiSLActions)0
			select s).ToList();
	}

	public static string GetPageFilters(StiSLActions action)
	{
		string text = "";
		bool flag = true;
		List<StiPageSLService> pageSLServices = GetPageSLServices(action);
		foreach (StiPageSLService item in pageSLServices)
		{
			if (!flag)
			{
				text += "|";
			}
			text += item.GetFilter();
			flag = false;
		}
		return text;
	}

	public static string GetDocumentFilters(StiSLActions action)
	{
		string text = "";
		bool flag = true;
		List<StiDocumentSLService> documentSLServices = GetDocumentSLServices(action);
		foreach (StiDocumentSLService item in documentSLServices)
		{
			string text2 = item.GetFilter();
			if (action == StiSLActions.Load || action == StiSLActions.Merge)
			{
				if (item is StiPackedDocumentSLService || item is StiEncryptedDocumentSLService || item is StiJsonDocumentSLService)
				{
					continue;
				}
				if (item is StiXmlDocumentSLService)
				{
					text2 = text2.Replace("mdc", "mdc;*.mdz;*.mdx");
				}
			}
			if (!flag)
			{
				text += "|";
			}
			text += text2;
			flag = false;
		}
		return text;
	}

	public static string GetReportFilters(StiSLActions action)
	{
		string text = "";
		bool flag = true;
		List<StiReportSLService> reportSLServices = GetReportSLServices(action);
		foreach (StiReportSLService item in reportSLServices)
		{
			string text2 = item.GetFilter();
			if (action == StiSLActions.Load || action == StiSLActions.Merge)
			{
				if (item is StiPackedReportSLService || item is StiEncryptedReportSLService || item is StiJsonReportSLService)
				{
					continue;
				}
				if (item is StiXmlReportSLService)
				{
					text2 = text2.Replace("mrt", "mrt;*.mrz;*.mrx");
				}
			}
			if (!flag)
			{
				text += "|";
			}
			text += text2;
			flag = false;
		}
		return text;
	}

	public static string GetDictionaryFilters(StiSLActions action)
	{
		string text = "";
		bool flag = true;
		List<StiDictionarySLService> dictionarySLServices = GetDictionarySLServices(action);
		foreach (StiDictionarySLService item in dictionarySLServices)
		{
			if (!flag)
			{
				text += "|";
			}
			text += item.GetFilter();
			flag = false;
		}
		return text;
	}
}
