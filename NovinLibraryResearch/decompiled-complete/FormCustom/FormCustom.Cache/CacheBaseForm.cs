using System;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache;
using FormCustom.Assistance;
using NPResourceManagement;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace FormCustom.Cache;

public class CacheBaseForm : Form, INpDisposable, IDisposable, ICacheBaseForm
{
	private TaskCompletionSource<DialogResult> _dialogResult = new TaskCompletionSource<DialogResult>();

	public static bool _inputLanguageChangedToFa_Ir;

	private bool _isDisposed;

	public EnSystem system => EnSystem.UserInterface;

	public TaskCompletionSource<DialogResult> DialogResultTask { get; private set; } = new TaskCompletionSource<DialogResult>();


	public virtual CacheContext Context { get; set; }

	public string ContextGuid => Context?.ContextGuid;

	public string ConnectionString => ContextGuid.GetSqlConnectionString();

	public virtual bool DataIsValid { get; set; }

	public ClsCache Cache => ClsCache.GetCache(ContextGuid);

	public bool IsDisposed
	{
		get
		{
			if (!((Control)this).IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	protected static bool IsInDesigner => Assembly.GetEntryAssembly() == null;

	protected CacheBaseForm(string contextGuid)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		if (ImageResourceManagement.Icon != null)
		{
			((Form)this).Icon = ImageResourceManagement.Icon;
		}
		((Form)this).FormClosed += new FormClosedEventHandler(CacheBaseForm_FormClosed);
		Context = new CacheContext(contextGuid);
		ChangeInputLanguage();
	}

	private void CacheBaseForm_FormClosed(object sender, FormClosedEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Form)sender).FormClosed -= new FormClosedEventHandler(CacheBaseForm_FormClosed);
			DialogResultTask.TrySetResult(((Form)this).DialogResult);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CacheBaseForm_FormClosed", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Cache\\CacheBaseForm.cs", 61);
		}
	}

	private void ChangeInputLanguage()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		try
		{
			if (_inputLanguageChangedToFa_Ir)
			{
				return;
			}
			_inputLanguageChangedToFa_Ir = true;
			UiInvokerInstance.Initialize(this);
			foreach (InputLanguage item in (ReadOnlyCollectionBase)(object)InputLanguage.InstalledInputLanguages)
			{
				InputLanguage val = item;
				if (val.Culture.Name == "fa-IR")
				{
					Application.CurrentInputLanguage = val;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeInputLanguage", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Cache\\CacheBaseForm.cs", 78);
		}
	}

	protected override void Dispose(bool disposing)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				((Form)this).FormClosed -= new FormClosedEventHandler(OnFormClosed);
				((Form)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Cache\\CacheBaseForm.cs", 100);
		}
	}

	public virtual void OnFormClosed(object sender, FormClosedEventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Form)this).FormClosed -= new FormClosedEventHandler(OnFormClosed);
			_dialogResult?.TrySetResult(((Form)this).DialogResult);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnFormClosed", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Cache\\CacheBaseForm.cs", 113);
		}
	}

	public async Task<DialogResult> ShowAsync(IWin32Window owner)
	{
		try
		{
			((Form)this).Show(owner);
			CacheBaseForm cacheBaseForm = this;
			CacheBaseForm cacheBaseForm2 = this;
			((Form)cacheBaseForm).FormClosed += new FormClosedEventHandler(cacheBaseForm2.OnFormClosed);
			return await _dialogResult.Task;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Cache\\CacheBaseForm.cs", 126);
			throw;
		}
	}
}
