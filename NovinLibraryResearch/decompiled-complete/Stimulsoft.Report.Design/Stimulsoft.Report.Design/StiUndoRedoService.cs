using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design;

public class StiUndoRedoService : StiDesignerInitService
{
	public sealed override Type ServiceType => typeof(StiUndoRedoService);

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(15)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public virtual int UndoLimit { get; set; } = 15;


	public virtual Stack<StiUndoRedo> UndoStack { get; set; } = new Stack<StiUndoRedo>();


	public virtual Stack<StiUndoRedo> RedoStack { get; set; } = new Stack<StiUndoRedo>();


	public virtual bool CanRedo => RedoStack.Count > 0;

	public virtual bool CanUndo => UndoStack.Count > 0;

	public virtual string UndoText
	{
		get
		{
			if (UndoStack.Count <= 0)
			{
				return string.Empty;
			}
			return UndoStack.Peek().Name;
		}
		set
		{
			if (UndoStack.Count > 0)
			{
				UndoStack.Peek().Name = value;
			}
		}
	}

	public virtual string RedoText
	{
		get
		{
			if (RedoStack.Count <= 0)
			{
				return string.Empty;
			}
			return RedoStack.Peek().Name;
		}
		set
		{
			if (RedoStack.Count > 0)
			{
				RedoStack.Peek().Name = value;
			}
		}
	}

	public override object Clone()
	{
		StiUndoRedoService stiUndoRedoService = base.Clone() as StiUndoRedoService;
		stiUndoRedoService.UndoStack = new Stack<StiUndoRedo>();
		stiUndoRedoService.RedoStack = new Stack<StiUndoRedo>();
		return stiUndoRedoService;
	}

	public virtual StiUndoRedo Undo(object obj, StiDesignerControl designer)
	{
		StiUndoRedo stiUndoRedo = UndoStack.Pop();
		stiUndoRedo.Value = Encoding.UTF8.GetString(StiGZipHelper.Unpack((byte[])stiUndoRedo.Value));
		StiReport stiReport = obj as StiReport;
		if (stiReport != null && stiUndoRedo.PageIndex != -1)
		{
			obj = stiReport.Pages[stiUndoRedo.PageIndex];
		}
		string str = obj.GetType().ToString();
		StiUndoRedo stiUndoRedo2 = StiUndoRedo.CreateURFromObject(stiUndoRedo.Name, obj, stiReport.CurrentPage);
		stiUndoRedo2.PageIndex = stiUndoRedo.PageIndex;
		if (str.EndsWithInvariant("StiForm"))
		{
			stiUndoRedo2.PageType = StiPageType.Form;
		}
		else if (str.EndsWithInvariant("StiScreen"))
		{
			stiUndoRedo2.PageType = StiPageType.Screen;
		}
		else if (str.EndsWithInvariant("StiDashboard"))
		{
			stiUndoRedo2.PageType = StiPageType.Dashboard;
		}
		else
		{
			stiUndoRedo2.PageType = StiPageType.Page;
		}
		stiUndoRedo2.Value = StiGZipHelper.Pack(Encoding.UTF8.GetBytes((string)stiUndoRedo2.Value));
		RedoStack.Push(stiUndoRedo2);
		return stiUndoRedo;
	}

	public virtual StiUndoRedo Redo(object obj, StiDesignerControl designer)
	{
		StiUndoRedo stiUndoRedo = RedoStack.Pop();
		stiUndoRedo.Value = Encoding.UTF8.GetString(StiGZipHelper.Unpack((byte[])stiUndoRedo.Value));
		StiReport stiReport = obj as StiReport;
		if (stiReport != null && stiUndoRedo.PageIndex != -1)
		{
			obj = stiReport.Pages[stiUndoRedo.PageIndex];
		}
		string str = obj.GetType().ToString();
		StiUndoRedo stiUndoRedo2 = StiUndoRedo.CreateURFromObject(stiUndoRedo.Name, obj, stiReport.CurrentPage);
		stiUndoRedo2.PageIndex = stiUndoRedo.PageIndex;
		if (str.EndsWithInvariant("StiForm"))
		{
			stiUndoRedo2.PageType = StiPageType.Form;
		}
		else if (str.EndsWithInvariant("StiScreen"))
		{
			stiUndoRedo2.PageType = StiPageType.Screen;
		}
		else if (str.EndsWithInvariant("StiDashboard"))
		{
			stiUndoRedo2.PageType = StiPageType.Dashboard;
		}
		else
		{
			stiUndoRedo2.PageType = StiPageType.Page;
		}
		stiUndoRedo2.Value = StiGZipHelper.Pack(Encoding.UTF8.GetBytes((string)stiUndoRedo2.Value));
		UndoStack.Push(stiUndoRedo2);
		return stiUndoRedo;
	}

	public virtual void Save(string name, object obj, StiDesignerControl designer)
	{
		Save(name, obj, onlyCurrentPage: false, designer);
	}

	public virtual void Save(string name, object obj, bool onlyCurrentPage, StiDesignerControl designer)
	{
		StiReport stiReport = obj as StiReport;
		if (!StiOptions.Designer.AdvancedModeOfUndoRedoService)
		{
			onlyCurrentPage = false;
		}
		if (onlyCurrentPage)
		{
			StiPage currentPage = stiReport.GetCurrentPage();
			StiComponentsCollection components = stiReport.GetComponents();
			foreach (StiComponent item in components)
			{
				if (item is IStiMasterComponent { MasterComponent: not null } stiMasterComponent && stiMasterComponent.MasterComponent.Page != item.Page && (stiMasterComponent.MasterComponent.Page == currentPage || item.Page == currentPage))
				{
					onlyCurrentPage = false;
					break;
				}
				if (item is IStiClone { Container: not null } stiClone && stiClone.Container.Page != item.Page && (stiClone.Container.Page == currentPage || item.Page == currentPage))
				{
					onlyCurrentPage = false;
					break;
				}
			}
		}
		int pageIndex = -1;
		StiPage stiPage = null;
		if (stiReport != null)
		{
			stiReport.ReportChanged = DateTime.Now;
			if (onlyCurrentPage)
			{
				obj = stiReport.GetCurrentPage();
				stiPage = obj as StiPage;
				stiPage.Report = null;
				pageIndex = stiReport.CurrentPage;
			}
		}
		if (UndoLimit != 0)
		{
			string str = obj.GetType().ToString();
			StiUndoRedo stiUndoRedo = StiUndoRedo.CreateURFromObject(name, obj, stiReport.CurrentPage);
			stiUndoRedo.PageIndex = pageIndex;
			if (str.EndsWithInvariant("StiForm"))
			{
				stiUndoRedo.PageType = StiPageType.Form;
			}
			else if (str.EndsWithInvariant("StiScreen"))
			{
				stiUndoRedo.PageType = StiPageType.Screen;
			}
			else if (str.EndsWithInvariant("StiDashboard"))
			{
				stiUndoRedo.PageType = StiPageType.Dashboard;
			}
			else
			{
				stiUndoRedo.PageType = StiPageType.Page;
			}
			stiUndoRedo.Value = StiGZipHelper.Pack(Encoding.UTF8.GetBytes((string)stiUndoRedo.Value));
			UndoStack.Push(stiUndoRedo);
			if (UndoStack.Count > UndoLimit)
			{
				StiUndoRedo[] array = new StiUndoRedo[UndoStack.Count];
				UndoStack.CopyTo(array, 0);
				UndoStack.Clear();
				for (int num = array.Length - 2; num >= 0; num--)
				{
					UndoStack.Push(array[num]);
				}
			}
		}
		RedoStack.Clear();
		if (stiPage != null)
		{
			stiPage.Report = stiReport;
		}
	}

	public virtual void Clear()
	{
		RedoStack.Clear();
		UndoStack.Clear();
	}

	public override void Init(StiDesignerControl designer)
	{
		base.Init(designer);
		Clear();
	}

	public override void Dispose()
	{
		base.Dispose();
		RedoStack.Clear();
		UndoStack.Clear();
	}

	public void UndoStackPop()
	{
		if (UndoLimit != 0 && base.ServiceEnabled)
		{
			UndoStack.Pop();
		}
	}

	public void RedoStackPop()
	{
		if (UndoLimit != 0 && base.ServiceEnabled)
		{
			RedoStack.Pop();
		}
	}
}
