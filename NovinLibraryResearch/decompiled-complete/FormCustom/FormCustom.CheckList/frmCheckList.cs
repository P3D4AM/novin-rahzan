using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Models;
using FormCustom.UC;
using PacketParser.Entities;

namespace FormCustom.CheckList;

public class frmCheckList : AccountingBaseForm
{
	private bool _isDisposed;

	private CancellationTokenSource _token;

	private IContainer components;

	private TextBox txtTitle;

	private Label label1;

	private UcButtons btnCancel;

	private UcButtons btnOk;

	private UcGroupBox ucGroupBox1;

	public CheckListModel CheckList { get; private set; }

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	private async Task SetDataAsync(CancellationToken token)
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				await Task.Delay(1000);
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					((Control)txtTitle).Text = CheckList?.Title;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "SetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\frmCheckList.cs", 38);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\frmCheckList.cs", 41);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_token?.Cancel();
				_token?.Dispose();
				_token = null;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\frmCheckList.cs", 57);
		}
	}

	public frmCheckList(string contextGuid, CheckListModel checkListItem)
		: base(contextGuid)
	{
		frmCheckList frmCheckList2 = this;
		try
		{
			InitializeComponent();
			CheckList = checkListItem;
			_token = new CancellationTokenSource(60000);
			CancellationToken token = _token.Token;
			Task.Run(() => frmCheckList2.SetDataAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\frmCheckList.cs", 72);
		}
	}

	private void btnOk_OnClick(object sender, EventArgs e)
	{
		try
		{
			CheckList.Title = ((Control)txtTitle).Text;
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\frmCheckList.cs", 83);
		}
	}

	private void btnCancel_OnClick(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnCancel_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\frmCheckList.cs", 92);
		}
	}

	private void frmCheckList_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void frmCheckList_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 116)
				{
					btnOk.PerformClick();
				}
			}
			else
			{
				btnCancel.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmCheckList_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\frmCheckList.cs", 105);
		}
	}

	private void txtDescription_Enter(object sender, EventArgs e)
	{
		try
		{
			TextBox val = (TextBox)((sender is TextBox) ? sender : null);
			if (val != null)
			{
				((Control)val).BackColor = Color.FromArgb(210, 221, 251);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtDescription_Enter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\frmCheckList.cs", 114);
		}
	}

	private void txtDescription_Leave(object sender, EventArgs e)
	{
		try
		{
			TextBox val = (TextBox)((sender is TextBox) ? sender : null);
			if (val != null)
			{
				((Control)val).BackColor = Color.White;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtDescription_Leave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\frmCheckList.cs", 123);
		}
	}

	private void txtMinFrom_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				btnOk.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtMinFrom_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CheckList\\frmCheckList.cs", 132);
		}
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Expected O, but got Unknown
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Expected O, but got Unknown
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Expected O, but got Unknown
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Expected O, but got Unknown
		//IL_060c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Expected O, but got Unknown
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Expected O, but got Unknown
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmCheckList));
		txtTitle = new TextBox();
		label1 = new Label();
		btnCancel = new UcButtons();
		btnOk = new UcButtons();
		ucGroupBox1 = new UcGroupBox();
		((Control)this).SuspendLayout();
		((Control)txtTitle).Anchor = (AnchorStyles)9;
		((Control)txtTitle).Font = new Font("B Yekan", 9f);
		((Control)txtTitle).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)txtTitle).Location = new Point(13, 13);
		((Control)txtTitle).Margin = new Padding(4);
		((Control)txtTitle).Name = "txtTitle";
		((Control)txtTitle).Size = new Size(341, 26);
		((Control)txtTitle).TabIndex = 4;
		((Control)txtTitle).Enter += txtDescription_Enter;
		((Control)txtTitle).KeyDown += new KeyEventHandler(txtMinFrom_KeyDown);
		((Control)txtTitle).Leave += txtDescription_Leave;
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label1).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label1).Location = new Point(358, 17);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(54, 20);
		((Control)label1).TabIndex = 6;
		((Control)label1).Text = "[عنوان]:";
		((Control)btnCancel).Anchor = (AnchorStyles)6;
		((Control)btnCancel).BackColor = Color.FromArgb(249, 250, 255);
		btnCancel.CornerType = EnButtonCornerType.Round;
		((Control)btnCancel).Cursor = Cursors.Hand;
		btnCancel.DisplayText = "[انصراف](Esc)";
		((Control)btnCancel).Font = new Font("B Yekan", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnCancel.IsShowMoreDetails = false;
		((Control)btnCancel).Location = new Point(13, 57);
		((Control)btnCancel).Margin = new Padding(0);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).RightToLeft = (RightToLeft)1;
		((Control)btnCancel).Size = new Size(121, 35);
		((Control)btnCancel).TabIndex = 21;
		btnCancel.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Type = EnButtonType.Normal;
		btnCancel.OnClick += btnCancel_OnClick;
		((Control)btnOk).Anchor = (AnchorStyles)6;
		((Control)btnOk).BackColor = Color.FromArgb(249, 250, 255);
		btnOk.CornerType = EnButtonCornerType.Round;
		((Control)btnOk).Cursor = Cursors.Hand;
		btnOk.DisplayText = "[تایید](F5)";
		((Control)btnOk).Font = new Font("B Yekan", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnOk.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnOk.IsShowMoreDetails = false;
		((Control)btnOk).Location = new Point(134, 57);
		((Control)btnOk).Margin = new Padding(0);
		((Control)btnOk).Name = "btnOk";
		((Control)btnOk).RightToLeft = (RightToLeft)1;
		((Control)btnOk).Size = new Size(220, 35);
		((Control)btnOk).TabIndex = 20;
		btnOk.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnOk.Type = EnButtonType.Bold;
		btnOk.OnClick += btnOk_OnClick;
		((Control)ucGroupBox1).BackColor = Color.Transparent;
		ucGroupBox1.DisplayFont = new Font("PeydaWeb Black", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ucGroupBox1).Dock = (DockStyle)5;
		((Control)ucGroupBox1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucGroupBox1.HeadHeight = 40;
		ucGroupBox1.IsShowHeader = false;
		((Control)ucGroupBox1).Location = new Point(0, 0);
		((Control)ucGroupBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucGroupBox1).Name = "ucGroupBox1";
		((Control)ucGroupBox1).RightToLeft = (RightToLeft)1;
		((Control)ucGroupBox1).Size = new Size(419, 101);
		((Control)ucGroupBox1).TabIndex = 22;
		ucGroupBox1.Title = "[عنوان]";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 18f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(419, 101);
		((Control)this).Controls.Add((Control)(object)btnCancel);
		((Control)this).Controls.Add((Control)(object)btnOk);
		((Control)this).Controls.Add((Control)(object)txtTitle);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)ucGroupBox1);
		((Control)this).Font = new Font("B Yekan", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).Margin = new Padding(5, 4, 5, 4);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmCheckList";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[مدیریت چک لیست]";
		((Form)this).FormClosed += new FormClosedEventHandler(frmCheckList_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmCheckList_KeyDown);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
