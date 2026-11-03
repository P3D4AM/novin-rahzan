using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FormCustom.UC;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.ServiceManagement;

public class frmChangeIp : AccountingBaseForm
{
	private bool _isDisposed;

	private IContainer components;

	internal TextBox txtIP;

	private UcButtons btnCancel;

	private UcButtons btnOk;

	private Label label1;

	private Label label2;

	public string Ip { get; private set; }

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

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Forms\\frmChangeIp.cs", 26);
		}
	}

	public frmChangeIp(string contextGuid, string ip)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			((Control)txtIP).Text = ip;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Forms\\frmChangeIp.cs", 37);
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
			WebErrorLog.Instance.StartLog(ex, "", "btnCancel_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Forms\\frmChangeIp.cs", 47);
		}
	}

	private async void btnOk_OnClick(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrEmpty(((Control)txtIP).Text))
			{
				await this.ShowErrorAsync("لطفا Ip را به درستی وارد نمایید".GetTranslate());
				return;
			}
			Ip = ((Control)txtIP).Text;
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Forms\\frmChangeIp.cs", 64);
		}
	}

	private void frmChangeIp_KeyDown(object sender, KeyEventArgs e)
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
			WebErrorLog.Instance.StartLog(ex, "", "frmChangeIp_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Forms\\frmChangeIp.cs", 76);
		}
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Expected O, but got Unknown
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Expected O, but got Unknown
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmChangeIp));
		txtIP = new TextBox();
		btnCancel = new UcButtons();
		btnOk = new UcButtons();
		label1 = new Label();
		label2 = new Label();
		((Control)this).SuspendLayout();
		((Control)txtIP).Anchor = (AnchorStyles)13;
		((Control)txtIP).Font = new Font("B Yekan", 9.75f);
		((Control)txtIP).Location = new Point(13, 240);
		((Control)txtIP).Margin = new Padding(4, 3, 4, 3);
		((Control)txtIP).Name = "txtIP";
		((Control)txtIP).Size = new Size(335, 27);
		((Control)txtIP).TabIndex = 1035;
		txtIP.TextAlign = (HorizontalAlignment)1;
		((Control)btnCancel).Anchor = (AnchorStyles)6;
		((Control)btnCancel).BackColor = Color.Transparent;
		btnCancel.CornerType = EnButtonCornerType.Round;
		btnCancel.DisplayText = "[انصراف](Esc)";
		((Control)btnCancel).Font = new Font("B Yekan", 9.75f);
		btnCancel.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnCancel.IsShowMoreDetails = false;
		((Control)btnCancel).Location = new Point(7, 283);
		((Control)btnCancel).Margin = new Padding(0);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).RightToLeft = (RightToLeft)1;
		((Control)btnCancel).Size = new Size(112, 29);
		((Control)btnCancel).TabIndex = 1037;
		btnCancel.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Type = EnButtonType.Normal;
		btnCancel.OnClick += btnCancel_OnClick;
		((Control)btnOk).Anchor = (AnchorStyles)6;
		((Control)btnOk).BackColor = Color.Transparent;
		btnOk.CornerType = EnButtonCornerType.Round;
		btnOk.DisplayText = "[تایید](F5)";
		((Control)btnOk).Font = new Font("B Yekan", 9.75f);
		btnOk.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnOk.IsShowMoreDetails = false;
		((Control)btnOk).Location = new Point(122, 283);
		((Control)btnOk).Margin = new Padding(0);
		((Control)btnOk).Name = "btnOk";
		((Control)btnOk).RightToLeft = (RightToLeft)1;
		((Control)btnOk).Size = new Size(233, 29);
		((Control)btnOk).TabIndex = 1036;
		btnOk.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnOk.Type = EnButtonType.Bold;
		btnOk.OnClick += btnOk_OnClick;
		((Control)label1).ForeColor = Color.Red;
		((Control)label1).Location = new Point(13, 9);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(339, 167);
		((Control)label1).TabIndex = 1038;
		((Control)label1).Text = componentResourceManager.GetString("label1.Text").GetTranslate();
		((Control)label2).ForeColor = Color.Black;
		((Control)label2).Location = new Point(13, 196);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(339, 41);
		((Control)label2).TabIndex = 1038;
		((Control)label2).Text = "[با توجه به توضیحات مطرح شده و در صورت نیاز Ip مورد نظر را وارد نمایید]";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(361, 321);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)btnCancel);
		((Control)this).Controls.Add((Control)(object)btnOk);
		((Control)this).Controls.Add((Control)(object)txtIP);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmChangeIp";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[تغییر]IP";
		((Control)this).KeyDown += new KeyEventHandler(frmChangeIp_KeyDown);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
