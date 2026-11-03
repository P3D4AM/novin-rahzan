using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Models;
using FormCustom.Properties;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace FormCustom.CheckList;

public class UcCheckList : UserControl, IHasContext
{
	private IContainer components;

	private FlowLayoutPanel pnlLocation;

	private PictureBox picAddLocation;

	public string ContextGuid { get; private set; }

	public List<CheckListModel> CheckList { get; private set; }

	public async Task InitAsync(string contextGuid, List<CheckListModel> list, CancellationToken token = default(CancellationToken))
	{
		try
		{
			ContextGuid = contextGuid;
			CheckList = list;
			if (CheckList == null)
			{
				CheckList = new List<CheckListModel>();
			}
			CheckList = CheckList?.OrderBy((CheckListModel q) => q.Ordinal)?.ToList();
			if (((Control)this).FindForm() == null || ((Control)((Control)this).FindForm()).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)((Control)this).FindForm()).IsHandleCreated)
			{
				await Task.Delay(100);
				if (((Control)this).FindForm() == null || ((Control)((Control)this).FindForm()).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (((Control)this).FindForm() == null || ((Control)((Control)this).FindForm()).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				if (((Control)this).FindForm() == null || ((Control)((Control)this).FindForm()).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					((Control)pnlLocation).Controls.Clear();
					foreach (CheckListModel item in CheckList)
					{
						if (item != null)
						{
							UcCheckListItem ucCheckListItem = new UcCheckListItem();
							((Control)ucCheckListItem).Width = ((Control)picAddLocation).Width;
							UcCheckListItem uc = ucCheckListItem;
							uc.OnDeleteRequest += Uc_OnDeleteRequest;
							uc.OnMoveDownRequest += Uc_OnMoveDownRequest;
							uc.OnMoveUpRequest += Uc_OnMoveUpRequest;
							uc.OnEditRequest += Uc_OnEditRequest;
							Task.Run(() => uc.InitAsync(item, default(CancellationToken)));
							((Control)pnlLocation).Controls.Add((Control)(object)uc);
						}
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\UcCheckList.cs", 53);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\UcCheckList.cs", 56);
		}
	}

	private async Task Uc_OnEditRequest(CheckListModel obj)
	{
		try
		{
			if (obj == null)
			{
				return;
			}
			CheckListModel existData = CheckList?.FirstOrDefault((CheckListModel q) => q.Title == obj.Title && q.Ordinal == obj.Ordinal);
			frmCheckList frm = new frmCheckList(ContextGuid, obj);
			if ((int)(await frm.ShowAsync((IWin32Window)(object)this)) == 1 && frm.CheckList != null)
			{
				if (existData != null)
				{
					CheckList.Remove(existData);
				}
				CheckList.Add(obj);
				await ReOrdinalAsync(obj.Title);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Uc_OnEditRequest", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\UcCheckList.cs", 73);
		}
	}

	private async Task Uc_OnMoveUpRequest(CheckListModel obj)
	{
		try
		{
			if (obj != null)
			{
				obj.Ordinal--;
				if (obj.Ordinal <= 0)
				{
					obj.Ordinal = 1;
				}
				await ReOrdinalAsync(obj.Title);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Uc_OnMoveUpRequest", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\UcCheckList.cs", 84);
		}
	}

	private async Task Uc_OnMoveDownRequest(CheckListModel obj)
	{
		try
		{
			if (obj != null)
			{
				obj.Ordinal++;
				int num = CheckList?.Max((CheckListModel q) => q.Ordinal) ?? 0;
				if (obj.Ordinal > num)
				{
					obj.Ordinal = num;
				}
				await ReOrdinalAsync(obj.Title);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Uc_OnMoveDownRequest", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\UcCheckList.cs", 96);
		}
	}

	private async Task Uc_OnDeleteRequest(CheckListModel obj)
	{
		try
		{
			if (obj != null)
			{
				CheckList.Remove(obj);
				await ReOrdinalAsync(obj.Title);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Uc_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\UcCheckList.cs", 106);
		}
	}

	private async Task AddAsync()
	{
		_ = 2;
		try
		{
			CheckListModel checkListItem = new CheckListModel
			{
				Ordinal = ((!(CheckList?.Any() ?? false)) ? 1 : (CheckList.Max((CheckListModel q) => q.Ordinal) + 1))
			};
			frmCheckList frm = new frmCheckList(ContextGuid, checkListItem);
			if ((int)(await frm.ShowAsync((IWin32Window)(object)this)) == 1 && frm.CheckList != null)
			{
				CheckList.Add(frm.CheckList);
				await InitAsync(ContextGuid, CheckList);
				await AddAsync();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\UcCheckList.cs", 120);
		}
	}

	private async Task ReOrdinalAsync(string currentTitle)
	{
		try
		{
			List<CheckListModel> list = CheckList?.Where((CheckListModel p) => p != null && p.Title != currentTitle)?.OrderBy((CheckListModel p) => p.Ordinal)?.ToList();
			if (list == null || !list.Any())
			{
				return;
			}
			int num = 0;
			using (List<CheckListModel>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					num = (enumerator.Current.Ordinal = num + 1);
				}
			}
			await InitAsync(ContextGuid, CheckList);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\UcCheckList.cs", 134);
		}
	}

	public UcCheckList()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private async void picAddLocation_Click(object sender, EventArgs e)
	{
		await AddAsync();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		pnlLocation = new FlowLayoutPanel();
		picAddLocation = new PictureBox();
		((ISupportInitialize)picAddLocation).BeginInit();
		((Control)this).SuspendLayout();
		((Control)pnlLocation).Anchor = (AnchorStyles)15;
		((ScrollableControl)pnlLocation).AutoScroll = true;
		((Control)pnlLocation).BackColor = Color.FromArgb(249, 250, 255);
		pnlLocation.FlowDirection = (FlowDirection)1;
		((Control)pnlLocation).Location = new Point(0, 46);
		((Control)pnlLocation).Name = "pnlLocation";
		((Control)pnlLocation).Size = new Size(360, 409);
		((Control)pnlLocation).TabIndex = 229;
		pnlLocation.WrapContents = false;
		((Control)picAddLocation).Anchor = (AnchorStyles)13;
		((Control)picAddLocation).Cursor = Cursors.Hand;
		picAddLocation.Image = (Image)(object)FormCustom.Properties.Resources.Border2;
		((Control)picAddLocation).Location = new Point(30, 1);
		((Control)picAddLocation).Name = "picAddLocation";
		((Control)picAddLocation).Size = new Size(330, 45);
		picAddLocation.SizeMode = (PictureBoxSizeMode)1;
		picAddLocation.TabIndex = 0;
		picAddLocation.TabStop = false;
		((Control)picAddLocation).Click += picAddLocation_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(249, 250, 255);
		((Control)this).Controls.Add((Control)(object)picAddLocation);
		((Control)this).Controls.Add((Control)(object)pnlLocation);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcCheckList";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(360, 455);
		((ISupportInitialize)picAddLocation).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
