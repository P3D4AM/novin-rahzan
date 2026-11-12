using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.UI;

namespace Microsoft.Identity.Client.Platforms.net45;

internal class SilentWebUI : WebUI, IDisposable
{
	private const int NavigationWaitMiliSecs = 250;

	private const int NavigationOverallTimeout = 10000;

	private SilentWindowsFormsAuthenticationDialog _dialog;

	private bool _disposed;

	private WindowsFormsSynchronizationContext _formsSyncContext;

	private AuthorizationResult _result;

	private ManualResetEvent _threadInitializedEvent;

	private Exception _uiException;

	public SilentWebUI(CoreUIParent parent, RequestContext requestContext)
	{
		base.OwnerWindow = parent?.OwnerWindow;
		base.SynchronizationContext = parent?.SynchronizationContext;
		base.RequestContext = requestContext;
		_threadInitializedEvent = new ManualResetEvent(initialState: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	~SilentWebUI()
	{
		Dispose(disposing: false);
	}

	private void WaitForCompletionOrTimeout(Thread uiThread)
	{
		long num = 10000L;
		long ticks = DateTime.Now.Ticks;
		if (!_threadInitializedEvent.WaitOne((int)num))
		{
			return;
		}
		long num2 = (DateTime.Now.Ticks - ticks) / 10000;
		num -= num2;
		if (!uiThread.Join((int)((num > 0) ? num : 0)))
		{
			base.RequestContext.Logger.Info("Silent login thread did not complete on time.");
			((SynchronizationContext)(object)_formsSyncContext).Post((SendOrPostCallback)delegate
			{
				_dialog.CloseBrowser();
			}, (object?)null);
		}
	}

	private Thread StartUIThread()
	{
		Thread thread = new Thread((ThreadStart)delegate
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			try
			{
				_formsSyncContext = new WindowsFormsSynchronizationContext();
				_dialog = new SilentWindowsFormsAuthenticationDialog(base.OwnerWindow)
				{
					NavigationWaitMiliSecs = 250,
					RequestContext = base.RequestContext
				};
				_dialog.Done += UIDoneHandler;
				_threadInitializedEvent.Set();
				_dialog.AuthenticateAAD(base.RequestUri, base.CallbackUri);
				Application.Run();
				_result = _dialog.Result;
			}
			catch (Exception ex)
			{
				base.RequestContext.Logger.ErrorPii(ex);
				_uiException = ex;
			}
		});
		thread.SetApartmentState(ApartmentState.STA);
		thread.IsBackground = true;
		thread.Start();
		return thread;
	}

	protected override AuthorizationResult OnAuthenticate()
	{
		if (null == base.CallbackUri)
		{
			throw new InvalidOperationException("CallbackUri cannot be null");
		}
		Thread uiThread = StartUIThread();
		WaitForCompletionOrTimeout(uiThread);
		Cleanup();
		ThrowIfTransferredException();
		if (_result == null)
		{
			throw new MsalUiRequiredException("no_prompt_failed", "One of two conditions was encountered: 1. The Prompt.Never flag was passed, but the constraint could not be honored, because user interaction was required. 2. An error occurred during a silent web authentication that prevented the http authentication flow from completing in a short enough time frame");
		}
		return _result;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed)
		{
			return;
		}
		if (disposing)
		{
			if (_threadInitializedEvent != null)
			{
				_threadInitializedEvent.Dispose();
				_threadInitializedEvent = null;
			}
			if (_formsSyncContext != null)
			{
				_formsSyncContext.Dispose();
				_formsSyncContext = null;
			}
		}
		_disposed = true;
	}

	private void Cleanup()
	{
		_threadInitializedEvent.Dispose();
		_threadInitializedEvent = null;
	}

	private void ThrowIfTransferredException()
	{
		if (_uiException != null)
		{
			throw _uiException;
		}
	}

	private void UIDoneHandler(object sender, SilentWebUIDoneEventArgs e)
	{
		if (_uiException == null)
		{
			_uiException = e.TransferedException;
		}
		((Component)(object)(SilentWindowsFormsAuthenticationDialog)sender).Dispose();
		Application.ExitThread();
	}
}
