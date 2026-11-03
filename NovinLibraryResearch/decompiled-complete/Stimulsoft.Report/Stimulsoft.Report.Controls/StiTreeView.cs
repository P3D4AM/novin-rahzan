using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Controls;

[ToolboxItem(false)]
public class StiTreeView : TreeView
{
	private struct RECT
	{
		internal int left;

		internal int top;

		internal int right;

		internal int bottom;
	}

	private struct NMHDR
	{
		internal IntPtr hwndFrom;

		internal IntPtr idFrom;

		internal int code;
	}

	private struct NMCUSTOMDRAW
	{
		internal NMHDR hdr;

		internal int dwDrawStage;

		internal IntPtr hdc;

		internal RECT rc;

		internal IntPtr dwItemSpec;

		internal int uItemState;

		internal IntPtr lItemlParam;
	}

	private struct NMTVCUSTOMDRAW
	{
		internal NMCUSTOMDRAW nmcd;

		internal int clrText;

		internal int clrTextBk;

		internal int iLevel;
	}

	private const int NM_FIRST = 0;

	private const int NM_CUSTOMDRAW = -12;

	private const int CDDS_PREPAINT = 1;

	private const int CDDS_POSTPAINT = 2;

	private const int CDDS_PREERASE = 3;

	private const int CDDS_POSTERASE = 4;

	private const int CDDS_ITEM = 65536;

	private const int CDDS_ITEMPREPAINT = 65537;

	private const int CDDS_ITEMPOSTPAINT = 65538;

	private const int CDDS_ITEMPREERASE = 65539;

	private const int CDDS_ITEMPOSTERASE = 65540;

	private const int CDDS_SUBITEM = 131072;

	private const int CDRF_DODEFAULT = 0;

	private const int CDRF_NEWFONT = 2;

	private const int CDRF_SKIPDEFAULT = 4;

	private const int CDRF_NOTIFYPOSTPAINT = 16;

	private const int CDRF_NOTIFYITEMDRAW = 32;

	private const int CDRF_NOTIFYSUBITEMDRAW = 32;

	private const int CDRF_NOTIFYPOSTERASE = 64;

	private const int WM_NOTIFY = 78;

	private StiTreeViewDrawMode drawMode;

	private const int TV_FIRST = 4352;

	private const int TVM_SETITEMW = 4415;

	private const int TVIF_HANDLE = 16;

	private const int TVIF_STATE = 8;

	private const int TVIS_BOLD = 16;

	[DefaultValue(StiTreeViewDrawMode.Normal)]
	[Category("Behavior")]
	public StiTreeViewDrawMode DrawMode
	{
		get
		{
			return drawMode;
		}
		set
		{
			if (drawMode != value)
			{
				drawMode = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("Behavior")]
	public event StiDrawTreeNodeEventHandler DrawNode;

	protected override void WndProc(ref Message msg)
	{
		if (DrawMode == StiTreeViewDrawMode.OwnerDraw && ((Message)(ref msg)).Msg == 8270 && ((Message)(ref msg)).WParam.Equals((object?)(nint)((Control)this).Handle))
		{
			((Message)(ref msg)).Result = new IntPtr(HandleNotify(ref msg));
		}
		else
		{
			((TreeView)this).WndProc(ref msg);
		}
	}

	protected override void OnClick(EventArgs e)
	{
		((Control)this).OnClick(e);
		Point point = ((Control)this).PointToClient(Cursor.Position);
		TreeNode nodeAt = ((TreeView)this).GetNodeAt(point);
		((TreeView)this).SelectedNode = nodeAt;
	}

	private int HandleNotify(ref Message m)
	{
		if (!((Message)(ref m)).LParam.Equals((object?)(nint)IntPtr.Zero))
		{
			object lParam = ((Message)(ref m)).GetLParam(typeof(NMHDR));
			if (lParam is NMHDR && ((NMHDR)lParam).code == -12)
			{
				lParam = ((Message)(ref m)).GetLParam(typeof(NMTVCUSTOMDRAW));
				if (lParam is NMTVCUSTOMDRAW)
				{
					switch (((NMTVCUSTOMDRAW)lParam).nmcd.dwDrawStage)
					{
					case 1:
						return 32;
					case 65537:
						return 16;
					case 65538:
					{
						TreeNode val = TreeNode.FromHandle((TreeView)(object)this, ((NMTVCUSTOMDRAW)lParam).nmcd.dwItemSpec);
						Graphics val2 = Graphics.FromHdc(((NMTVCUSTOMDRAW)lParam).nmcd.hdc);
						OnDrawNode(new StiDrawTreeNodeEventArgs(val2, val, val.Bounds));
						val2.Dispose();
						return 0;
					}
					}
				}
			}
		}
		return 0;
	}

	public void SetBold(TreeNode node, bool isBold)
	{
		SetBold((TreeView)(object)this, node, isBold);
	}

	public static void SetBold(TreeView treeView, TreeNode node, bool isBold)
	{
		try
		{
			if (node.Handle != IntPtr.Zero)
			{
				TVITEMEX structure = default(TVITEMEX);
				structure.mask = 24;
				structure.hItem = node.Handle;
				structure.state = (isBold ? 16 : 0);
				structure.stateMask = 16;
				IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TVITEMEX)));
				Marshal.StructureToPtr(structure, intPtr, fDeleteOld: true);
				Win32.SendMessage(((Control)treeView).Handle, 4415, 0, intPtr);
				Marshal.FreeHGlobal(intPtr);
			}
		}
		catch
		{
		}
	}

	public void Lock()
	{
		Win32.LockWindowUpdate(((Control)this).Handle);
	}

	public void Unlock()
	{
		Win32.LockWindowUpdate(IntPtr.Zero);
	}

	protected virtual void OnDrawNode(StiDrawTreeNodeEventArgs e)
	{
		if (this.DrawNode != null)
		{
			this.DrawNode(this, e);
		}
	}
}
