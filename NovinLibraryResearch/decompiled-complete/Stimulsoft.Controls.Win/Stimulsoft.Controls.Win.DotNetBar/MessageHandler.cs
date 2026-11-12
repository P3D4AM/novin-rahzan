using System;
using System.Collections;
using System.Threading;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class MessageHandler
{
	private static ArrayList m_Clients;

	private static Hashtable m_FilterOnThread;

	private static ReaderWriterLock rwClientsListLock;

	static MessageHandler()
	{
		m_Clients = new ArrayList();
		m_FilterOnThread = new Hashtable();
		rwClientsListLock = new ReaderWriterLock();
	}

	public static void RegisterMessageClient(IMessageHandlerClient client)
	{
		if (m_Clients.Contains(client))
		{
			return;
		}
		if (!m_FilterOnThread.ContainsKey(Thread.CurrentThread.GetHashCode()))
		{
			InstallMessageFilter();
		}
		LockCookie lockCookie = default(LockCookie);
		bool isReaderLockHeld = rwClientsListLock.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = rwClientsListLock.UpgradeToWriterLock(-1);
		}
		else
		{
			rwClientsListLock.AcquireWriterLock(-1);
		}
		try
		{
			m_Clients.Add(client);
		}
		finally
		{
			if (isReaderLockHeld)
			{
				rwClientsListLock.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				rwClientsListLock.ReleaseWriterLock();
			}
		}
	}

	public static void UnregisterMessageClient(IMessageHandlerClient client)
	{
		if (!m_Clients.Contains(client))
		{
			return;
		}
		LockCookie lockCookie = default(LockCookie);
		bool isReaderLockHeld = rwClientsListLock.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = rwClientsListLock.UpgradeToWriterLock(-1);
		}
		else
		{
			rwClientsListLock.AcquireWriterLock(-1);
		}
		bool flag = false;
		try
		{
			m_Clients.Remove(client);
			flag = m_Clients.Count == 0;
		}
		finally
		{
			if (isReaderLockHeld)
			{
				rwClientsListLock.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				rwClientsListLock.ReleaseWriterLock();
			}
		}
		if (BarUtilities.AutoRemoveMessageFilter && flag && m_FilterOnThread.ContainsKey(Thread.CurrentThread.GetHashCode()))
		{
			UninstallMessageFilter();
		}
	}

	private static void InstallMessageFilter()
	{
		if (!m_FilterOnThread.ContainsKey(Thread.CurrentThread.GetHashCode()))
		{
			MessageFilterWrapper messageFilterWrapper = new MessageFilterWrapper();
			Application.AddMessageFilter((IMessageFilter)(object)messageFilterWrapper);
			m_FilterOnThread.Add(Thread.CurrentThread.GetHashCode(), messageFilterWrapper);
		}
	}

	private static void UninstallMessageFilter()
	{
		if (m_FilterOnThread.ContainsKey(Thread.CurrentThread.GetHashCode()))
		{
			MessageFilterWrapper messageFilterWrapper = m_FilterOnThread[Thread.CurrentThread.GetHashCode()] as MessageFilterWrapper;
			Application.RemoveMessageFilter((IMessageFilter)(object)messageFilterWrapper);
			m_FilterOnThread.Remove(Thread.CurrentThread.GetHashCode());
		}
	}

	private static IMessageHandlerClient GetModalClient()
	{
		rwClientsListLock.AcquireReaderLock(-1);
		try
		{
			foreach (IMessageHandlerClient client in m_Clients)
			{
				if (client.IsModal)
				{
					return client;
				}
			}
		}
		finally
		{
			rwClientsListLock.ReleaseReaderLock();
		}
		return null;
	}

	private static IMessageHandlerClient[] GetMessageClients()
	{
		rwClientsListLock.AcquireReaderLock(-1);
		try
		{
			return (IMessageHandlerClient[])m_Clients.ToArray(typeof(IMessageHandlerClient));
		}
		finally
		{
			rwClientsListLock.ReleaseReaderLock();
		}
	}

	public static bool OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		IMessageHandlerClient modalClient = GetModalClient();
		if (modalClient != null)
		{
			if (!IsInvokeRequired(modalClient) && modalClient.OnSysKeyDown(hWnd, wParam, lParam))
			{
				return true;
			}
			return false;
		}
		IMessageHandlerClient[] messageClients = GetMessageClients();
		IMessageHandlerClient[] array = messageClients;
		foreach (IMessageHandlerClient messageHandlerClient in array)
		{
			if (!IsInvokeRequired(messageHandlerClient) && messageHandlerClient.OnSysKeyDown(hWnd, wParam, lParam))
			{
				return true;
			}
		}
		return false;
	}

	public static bool OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		IMessageHandlerClient modalClient = GetModalClient();
		if (modalClient != null)
		{
			if (!IsInvokeRequired(modalClient) && modalClient.OnSysKeyUp(hWnd, wParam, lParam))
			{
				return true;
			}
			return false;
		}
		IMessageHandlerClient[] messageClients = GetMessageClients();
		IMessageHandlerClient[] array = messageClients;
		foreach (IMessageHandlerClient messageHandlerClient in array)
		{
			if (!IsInvokeRequired(messageHandlerClient) && messageHandlerClient.OnSysKeyUp(hWnd, wParam, lParam))
			{
				return true;
			}
		}
		return false;
	}

	public static bool OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		IMessageHandlerClient[] messageClients = GetMessageClients();
		IMessageHandlerClient[] array = messageClients;
		foreach (IMessageHandlerClient messageHandlerClient in array)
		{
			if (!IsInvokeRequired(messageHandlerClient) && messageHandlerClient.OnKeyDown(hWnd, wParam, lParam))
			{
				return true;
			}
		}
		return false;
	}

	public static bool OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		IMessageHandlerClient[] messageClients = GetMessageClients();
		IMessageHandlerClient[] array = messageClients;
		foreach (IMessageHandlerClient messageHandlerClient in array)
		{
			if (!IsInvokeRequired(messageHandlerClient) && messageHandlerClient.OnMouseDown(hWnd, wParam, lParam))
			{
				return true;
			}
		}
		return false;
	}

	private static bool IsInvokeRequired(IMessageHandlerClient client)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		if (client is DotNetBarManager { ParentForm: not null } dotNetBarManager && ((Control)dotNetBarManager.ParentForm).InvokeRequired)
		{
			return true;
		}
		if (client is Control && ((Control)client).InvokeRequired)
		{
			return true;
		}
		return false;
	}

	public static bool OnMouseMove(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		IMessageHandlerClient[] messageClients = GetMessageClients();
		IMessageHandlerClient[] array = messageClients;
		foreach (IMessageHandlerClient messageHandlerClient in array)
		{
			if (!IsInvokeRequired(messageHandlerClient) && messageHandlerClient.OnMouseMove(hWnd, wParam, lParam))
			{
				return true;
			}
		}
		return false;
	}

	public static bool OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		IMessageHandlerClient modalClient = GetModalClient();
		if (modalClient != null)
		{
			if (!IsInvokeRequired(modalClient) && modalClient.OnMouseWheel(hWnd, wParam, lParam))
			{
				return true;
			}
			return false;
		}
		IMessageHandlerClient[] messageClients = GetMessageClients();
		IMessageHandlerClient[] array = messageClients;
		foreach (IMessageHandlerClient messageHandlerClient in array)
		{
			if (!IsInvokeRequired(messageHandlerClient) && messageHandlerClient.OnMouseWheel(hWnd, wParam, lParam))
			{
				return true;
			}
		}
		return false;
	}
}
