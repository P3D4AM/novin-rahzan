using System;
using System.Collections;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

internal class ᒦ : NativeWindow
{
	private const int ମ = 70;

	private ArrayList ᒩ = new ArrayList();

	private ArrayList ᒪ = new ArrayList();

	private bool _߈;

	public int ᒧ => ᒩ.Count;

	public int ᒨ => ᒪ.Count;

	public ᒦ(bool designmode)
	{
		_߈ = designmode;
	}

	public void ᒫ(IOwner ᒬ)
	{
		ᒩ.Add(ᒬ);
	}

	public void ᒭ(IOwner ᒬ)
	{
		ᒩ.Remove(ᒬ);
	}

	public bool ᒮ(IOwner ᒬ)
	{
		return ᒩ.Contains(ᒬ);
	}

	public void ᒯ(IOwner ॡ)
	{
		ᒪ.Add(ॡ);
	}

	public void ᒰ(IOwner ॡ)
	{
		ᒪ.Remove(ॡ);
	}

	public bool ᒱ(IOwner ॡ)
	{
		return ᒪ.Contains(ॡ);
	}

	public void ᒲ()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		if (ᒩ.Count <= 0)
		{
			return;
		}
		IOwner[] array;
		lock (this)
		{
			array = (IOwner[])ᒩ.ToArray(typeof(IOwner));
		}
		IOwner[] array2 = array;
		foreach (IOwner owner in array2)
		{
			if (owner is Control && ((Control)owner).InvokeRequired)
			{
				((Control)owner).BeginInvoke((Delegate)new ᒴ(owner.OnApplicationActivate));
			}
			else
			{
				owner.OnApplicationActivate();
			}
		}
	}

	public void ᒳ()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		if (ᒩ.Count <= 0)
		{
			return;
		}
		IOwner[] array;
		lock (this)
		{
			array = (IOwner[])ᒩ.ToArray(typeof(IOwner));
		}
		IOwner[] array2 = array;
		foreach (IOwner owner in array2)
		{
			if (owner is Control && ((Control)owner).InvokeRequired)
			{
				((Control)owner).BeginInvoke((Delegate)new ᒴ(owner.OnApplicationDeactivate));
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
			if (((_089F.WINDOWPOS)((Message)(ref m)).GetLParam(default(_089F.WINDOWPOS).GetType())).hwndInsertAfter != 0 && ᒩ.Count > 0)
			{
				IOwner[] array;
				lock (this)
				{
					array = (IOwner[])ᒩ.ToArray(typeof(IOwner));
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
			int num = _0EFE._0F11(((Message)(ref m)).WParam);
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
					foreach (IOwner item in ᒩ)
					{
						if (item is DotNetBarManager && ((DotNetBarManager)item).IsCustomizing)
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
						if (!(val is Bar) && !(val is MenuPanel) && !(val is PopupContainerControl) && !(val is PopupContainer) && ᒩ.Count > 0)
						{
							IOwner[] array3;
							lock (this)
							{
								array3 = (IOwner[])ᒩ.ToArray(typeof(IOwner));
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
				_089F.ऴ();
			}
			else if (((Message)(ref m)).Msg == 134 && _߈ && ((Message)(ref m)).WParam == IntPtr.Zero && ᒩ.Count > 0)
			{
				IOwner[] array5;
				lock (this)
				{
					array5 = (IOwner[])ᒩ.ToArray(typeof(IOwner));
				}
				IOwner[] array6 = array5;
				foreach (IOwner owner4 in array6)
				{
					owner4.SetExpandedItem(null);
				}
				IOwner[] array7;
				lock (this)
				{
					array7 = (IOwner[])ᒪ.ToArray(typeof(IOwner));
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
