using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using PacketParser.Entities;

namespace FormCustom;

public static class Utilities
{
	public static void CheckVisibility(this IWin32Window owner, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		if (sourceFilePath == null)
		{
			sourceFilePath = "sourceFilePath is null";
		}
		if (memberName == null)
		{
			memberName = "memberName is null";
		}
		Utilities.CheckVisibility((Form)owner, memberName, sourceFilePath);
	}

	public static void CheckVisibility(this Form owner, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		try
		{
			string text = " Desc=> name:" + ((Control)owner).Name;
			if (owner == null)
			{
				WebErrorLog.Instance.StartLog("owner is null! " + text + " and memberName is " + memberName + " and sourceFilePath=" + sourceFilePath, "", "CheckVisibility", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Utilities.cs", 26);
			}
			if (!((Control)owner).Visible)
			{
				WebErrorLog.Instance.StartLog("owner(is form) is not visible!!!! " + text + " and memberName is " + memberName + " and sourceFilePath=" + sourceFilePath, "", "CheckVisibility", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Utilities.cs", 27);
			}
			if (!((Control)owner).IsHandleCreated)
			{
				WebErrorLog.Instance.StartLog("owner(is form) handler has not been created!!!! " + text + " and memberName is " + memberName + " and sourceFilePath=" + sourceFilePath, "", "CheckVisibility", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Utilities.cs", 28);
			}
			if (((Control)owner).InvokeRequired)
			{
				WebErrorLog.Instance.StartLog("owner(is form) invokeRequired!!!! " + text, "", "CheckVisibility", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Utilities.cs", 29);
			}
			if (((Control)owner).IsDisposed)
			{
				WebErrorLog.Instance.StartLog("owner(is form) isDisposed!!!! " + text + " and memberName is " + memberName + " and sourceFilePath=" + sourceFilePath, "", "CheckVisibility", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Utilities.cs", 30);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "error in catch of checkVisibility.  memberName is " + memberName + " and sourceFilePath=" + sourceFilePath, "CheckVisibility", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Utilities.cs", 34);
		}
	}

	public static void CheckVisibility(this UserControl owner, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		try
		{
			string[] obj = new string[5]
			{
				"ownerName(",
				((Control)owner).Name,
				") on Form(",
				null,
				null
			};
			Form obj2 = ((Control)owner).FindForm();
			obj[3] = ((obj2 != null) ? ((Control)obj2).Name : null);
			obj[4] = ")";
			string text = string.Concat(obj);
			if (owner == null)
			{
				WebErrorLog.Instance.StartLog("owner(control) is null! desc:" + text + " and memberName is " + memberName + " and sourceFilePath=" + sourceFilePath, "", "CheckVisibility", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Utilities.cs", 42);
			}
			if (!((Control)owner).Visible)
			{
				WebErrorLog.Instance.StartLog("owner(control) is not visible!!!! desc:" + text + " and memberName is " + memberName + " and sourceFilePath=" + sourceFilePath, "", "CheckVisibility", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Utilities.cs", 43);
			}
			if (!((Control)owner).IsHandleCreated)
			{
				WebErrorLog.Instance.StartLog("owner(control) handler has not been created!!!! desc:" + text + " and memberName is " + memberName + " and sourceFilePath=" + sourceFilePath, "", "CheckVisibility", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Utilities.cs", 44);
			}
			if (((Control)owner).InvokeRequired)
			{
				WebErrorLog.Instance.StartLog("owner(control) invokeRequired!!!! desc:" + text + " and memberName is " + memberName + " and sourceFilePath=" + sourceFilePath, "", "CheckVisibility", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Utilities.cs", 45);
			}
			if (((Control)owner).IsDisposed)
			{
				WebErrorLog.Instance.StartLog("owner(control) isDisposed!!!! desc:" + text + " and memberName is " + memberName + " and sourceFilePath=" + sourceFilePath, "", "CheckVisibility", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Utilities.cs", 46);
			}
			((Control)owner).FindForm().CheckVisibility("CheckVisibility", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Utilities.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "error in catch  of checkVisibility2.  memberName is " + memberName + " and sourceFilePath=" + sourceFilePath, "CheckVisibility", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Utilities.cs", 51);
		}
	}

	public static BindingList<T> ToBindingList<T>(this IEnumerable<T> items, CancellationToken token = default(CancellationToken))
	{
		int num = 0;
		BindingList<T> bindingList = new BindingList<T>();
		try
		{
			num = 1;
			List<T> list = items?.ToList();
			num = 2;
			if (list != null && list.Any())
			{
				num = 3;
				foreach (T item in list.Where((T item) => item != null))
				{
					if (token.IsCancellationRequested)
					{
						return null;
					}
					num = 4;
					bindingList.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"edited in 1399/12/06 - line={num}", "ToBindingList", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Utilities.cs", 74);
		}
		return bindingList;
	}

	public static SortableBindingList<T> ToSortableBindingList<T>(this IEnumerable<T> items, CancellationToken token = default(CancellationToken))
	{
		int num = 0;
		SortableBindingList<T> sortableBindingList = new SortableBindingList<T>();
		try
		{
			num = 10;
			if (items == null || !items.Any())
			{
				return sortableBindingList;
			}
			num = 11;
			List<T> list = items?.ToList();
			num = 12;
			if (list != null && list.Any())
			{
				num = 13;
				foreach (T item in list.Where((T item) => item != null))
				{
					if (token.IsCancellationRequested)
					{
						return null;
					}
					num = 14;
					sortableBindingList.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"edited in 1399/12/06 - line={num}", "ToSortableBindingList", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Utilities.cs", 99);
		}
		return sortableBindingList;
	}
}
