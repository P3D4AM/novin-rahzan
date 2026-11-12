using System;
using System.Collections;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ParentMsgHandler : NativeWindow
{
	private const int WM_WINDOWPOSCHANGING = 70;

	private ArrayList m_Managers = new ArrayList();

	private ArrayList m_Owners = new ArrayList();

	private bool m_DesignMode;

	public int RegisteredCount => m_Managers.Count;

	public int RegisteredOwnersCount => m_Owners.Count;

	public ParentMsgHandler(bool designmode)
	{
		m_DesignMode = designmode;
	}

	public void Register(IOwner manager)
	{
		m_Managers.Add(manager);
	}

	public void Unregister(IOwner manager)
	{
		m_Managers.Remove(manager);
	}

	public bool IsRegistered(IOwner manager)
	{
		return m_Managers.Contains(manager);
	}

	public void RegisterOwner(IOwner owner)
	{
		m_Owners.Add(owner);
	}

	public void UnregisterOwner(IOwner owner)
	{
		m_Owners.Remove(owner);
	}

	public bool IsOwnerRegistered(IOwner owner)
	{
		return m_Owners.Contains(owner);
	}

	public void ApplicationActivate()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		if (m_Managers.Count <= 0)
		{
			return;
		}
		IOwner[] array;
		lock (this)
		{
			array = (IOwner[])m_Managers.ToArray(typeof(IOwner));
		}
		IOwner[] array2 = array;
		foreach (IOwner owner in array2)
		{
			if (owner is Control && ((Control)owner).InvokeRequired)
			{
				((Control)owner).BeginInvoke((Delegate)new InvokeActivateDelegate(owner.OnApplicationActivate));
			}
			else
			{
				owner.OnApplicationActivate();
			}
		}
	}

	public void ApplicationDeactivate()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		if (m_Managers.Count <= 0)
		{
			return;
		}
		IOwner[] array;
		lock (this)
		{
			array = (IOwner[])m_Managers.ToArray(typeof(IOwner));
		}
		IOwner[] array2 = array;
		foreach (IOwner owner in array2)
		{
			if (owner is Control && ((Control)owner).InvokeRequired)
			{
				((Control)owner).BeginInvoke((Delegate)new InvokeActivateDelegate(owner.OnApplicationDeactivate));
			}
			else
			{
				owner.OnApplicationDeactivate();
			}
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 70)
		{
			if (((NativeFunctions.WINDOWPOS)((Message)(ref m)).GetLParam(default(NativeFunctions.WINDOWPOS).GetType())).hwndInsertAfter != 0 && m_Managers.Count > 0)
			{
				IOwner[] array;
				lock (this)
				{
					array = (IOwner[])m_Managers.ToArray(typeof(IOwner));
				}
				IOwner[] array2 = array;
				foreach (IOwner owner in array2)
				{
					owner.OnParentPositionChanging();
				}
			}
		}
		else
		{
			int num = WinApi.ToInt(((Message)(ref m)).WParam);
			if ((long)((Message)(ref m)).Msg == 28)
			{
				if (num != 0)
				{
					DotNetBarManager.RelayApplicationActivate(bActivate: true);
				}
				else
				{
					DotNetBarManager.RelayApplicationActivate(bActivate: false);
				}
			}
			else if (((Message)(ref m)).Msg == 6)
			{
				if ((num & 0xFF) == 0)
				{
					Control val = Control.FromChildHandle(((Message)(ref m)).LParam);
					bool flag = false;
					foreach (IOwner manager in m_Managers)
					{
						if (manager is DotNetBarManager && ((DotNetBarManager)manager).IsCustomizing)
						{
							flag = true;
							break;
						}
					}
					if (val != null && !flag)
					{
						while (val.Parent != null)
						{
							val = val.Parent;
						}
						if (!(val is Bar) && !(val is MenuPanel) && !(val is PopupContainerControl) && !(val is PopupContainer) && m_Managers.Count > 0)
						{
							IOwner[] array3;
							lock (this)
							{
								array3 = (IOwner[])m_Managers.ToArray(typeof(IOwner));
							}
							IOwner[] array4 = array3;
							foreach (IOwner owner3 in array4)
							{
								owner3.SetExpandedItem(null);
							}
						}
					}
				}
			}
			else if (((Message)(ref m)).Msg == 126)
			{
				NativeFunctions.OnDisplayChange();
			}
			else if (((Message)(ref m)).Msg == 134 && m_DesignMode && ((Message)(ref m)).WParam == IntPtr.Zero && m_Managers.Count > 0)
			{
				IOwner[] array5;
				lock (this)
				{
					array5 = (IOwner[])m_Managers.ToArray(typeof(IOwner));
				}
				IOwner[] array6 = array5;
				foreach (IOwner owner4 in array6)
				{
					owner4.SetExpandedItem(null);
				}
				IOwner[] array7;
				lock (this)
				{
					array7 = (IOwner[])m_Owners.ToArray(typeof(IOwner));
				}
				IOwner[] array8 = array7;
				foreach (IOwner owner5 in array8)
				{
					owner5.SetExpandedItem(null);
				}
			}
		}
		((NativeWindow)this).WndProc(ref m);
	}
}
