using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Models;
using FormCustom.Properties;
using FormCustom.UC;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.CheckList;

public class UcCheckListItem : UserControl
{
	private IContainer components;

	private UcCheckBox chb;

	private PictureBox picDelete;

	private PictureBox picEdit;

	private PictureBox picDown;

	private PictureBox picUp;

	public CheckListModel Model { get; private set; }

	public event Func<CheckListModel, Task> OnDeleteRequest;

	public event Func<CheckListModel, Task> OnMoveUpRequest;

	public event Func<CheckListModel, Task> OnMoveDownRequest;

	public event Func<CheckListModel, Task> OnEditRequest;

	public async Task InitAsync(CheckListModel model, CancellationToken token)
	{
		try
		{
			if (model == null || ((Control)this).FindForm() == null || ((Control)((Control)this).FindForm()).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			Model = model;
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
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				if (((Control)this).FindForm() == null || ((Control)((Control)this).FindForm()).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					ToolTip val = new ToolTip();
					val.SetToolTip((Control)(object)picDelete, "حذف".GetTranslate());
					val.SetToolTip((Control)(object)picEdit, "ویرایش".GetTranslate());
					val.SetToolTip((Control)(object)picUp, "انتقال به بالا".GetTranslate());
					val.SetToolTip((Control)(object)picDown, "انتقال به پایین".GetTranslate());
					chb.Title = $"{model.Ordinal}- {model.Title}";
					chb.Checked = model.IsChecked;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\UcCheckListItem.cs", 48);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\UcCheckListItem.cs", 51);
		}
	}

	public UcCheckListItem()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void chb_OnCheckedChanged(object sender, EventArgs e)
	{
		Model.IsChecked = chb.Checked;
	}

	private void picDelete_Click(object sender, EventArgs e)
	{
		this.OnDeleteRequest?.Invoke(Model);
	}

	private void picEdit_Click(object sender, EventArgs e)
	{
		this.OnEditRequest?.Invoke(Model);
	}

	private void picDown_Click(object sender, EventArgs e)
	{
		this.OnMoveDownRequest?.Invoke(Model);
	}

	private void picUp_Click(object sender, EventArgs e)
	{
		this.OnMoveUpRequest?.Invoke(Model);
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		chb = new UcCheckBox();
		picUp = new PictureBox();
		picDown = new PictureBox();
		picEdit = new PictureBox();
		picDelete = new PictureBox();
		((ISupportInitialize)picUp).BeginInit();
		((ISupportInitialize)picDown).BeginInit();
		((ISupportInitialize)picEdit).BeginInit();
		((ISupportInitialize)picDelete).BeginInit();
		((Control)this).SuspendLayout();
		((Control)chb).Anchor = (AnchorStyles)15;
		((Control)chb).BackColor = Color.Transparent;
		chb.Checked = false;
		chb.Color = Color.Transparent;
		((Control)chb).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)chb).Location = new Point(127, 0);
		((Control)chb).Margin = new Padding(4, 5, 4, 5);
		((Control)chb).Name = "chb";
		((Control)chb).RightToLeft = (RightToLeft)1;
		((Control)chb).Size = new Size(254, 29);
		((Control)chb).TabIndex = 0;
		chb.Title = "---";
		chb.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		chb.OnCheckedChanged += chb_OnCheckedChanged;
		((Control)picUp).Anchor = (AnchorStyles)6;
		((Control)picUp).BackColor = Color.Transparent;
		((Control)picUp).Cursor = Cursors.Hand;
		picUp.Image = (Image)(object)FormCustom.Properties.Resources.down_1;
		((Control)picUp).Location = new Point(90, 0);
		((Control)picUp).Name = "picUp";
		((Control)picUp).Size = new Size(30, 29);
		picUp.SizeMode = (PictureBoxSizeMode)4;
		picUp.TabIndex = 52;
		picUp.TabStop = false;
		((Control)picUp).Visible = false;
		((Control)picUp).Click += picUp_Click;
		((Control)picDown).Anchor = (AnchorStyles)6;
		((Control)picDown).BackColor = Color.Transparent;
		((Control)picDown).Cursor = Cursors.Hand;
		picDown.Image = (Image)(object)FormCustom.Properties.Resources.up_1;
		((Control)picDown).Location = new Point(60, 0);
		((Control)picDown).Name = "picDown";
		((Control)picDown).Size = new Size(30, 29);
		picDown.SizeMode = (PictureBoxSizeMode)4;
		picDown.TabIndex = 52;
		picDown.TabStop = false;
		((Control)picDown).Visible = false;
		((Control)picDown).Click += picDown_Click;
		((Control)picEdit).Anchor = (AnchorStyles)6;
		((Control)picEdit).BackColor = Color.Transparent;
		((Control)picEdit).Cursor = Cursors.Hand;
		picEdit.Image = (Image)(object)FormCustom.Properties.Resources.editOrange;
		((Control)picEdit).Location = new Point(30, 0);
		((Control)picEdit).Name = "picEdit";
		((Control)picEdit).Size = new Size(30, 29);
		picEdit.SizeMode = (PictureBoxSizeMode)4;
		picEdit.TabIndex = 52;
		picEdit.TabStop = false;
		((Control)picEdit).Click += picEdit_Click;
		((Control)picDelete).Anchor = (AnchorStyles)6;
		((Control)picDelete).BackColor = Color.Transparent;
		((Control)picDelete).Cursor = Cursors.Hand;
		picDelete.Image = (Image)(object)FormCustom.Properties.Resources.trashRed;
		((Control)picDelete).Location = new Point(0, 0);
		((Control)picDelete).Name = "picDelete";
		((Control)picDelete).Size = new Size(30, 29);
		picDelete.SizeMode = (PictureBoxSizeMode)4;
		picDelete.TabIndex = 52;
		picDelete.TabStop = false;
		((Control)picDelete).Click += picDelete_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)picUp);
		((Control)this).Controls.Add((Control)(object)picDown);
		((Control)this).Controls.Add((Control)(object)picEdit);
		((Control)this).Controls.Add((Control)(object)picDelete);
		((Control)this).Controls.Add((Control)(object)chb);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcCheckListItem";
		((Control)this).Size = new Size(381, 29);
		((ISupportInitialize)picUp).EndInit();
		((ISupportInitialize)picDown).EndInit();
		((ISupportInitialize)picEdit).EndInit();
		((ISupportInitialize)picDelete).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
