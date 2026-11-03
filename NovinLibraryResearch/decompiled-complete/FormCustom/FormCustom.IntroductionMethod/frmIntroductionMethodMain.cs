using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using EntityCache.Business.Global;
using FormCustom.UC;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.IntroductionMethod;

public class frmIntroductionMethodMain : AccountingBaseForm
{
	private bool _isDisposed;

	private IntroductionMethodBusiness _cls;

	private IContainer components;

	private PanelEx pnlPublicInfoMaster;

	internal TextBox txtName;

	internal Label Label1;

	private UcButtons btnSabt;

	private UcButtons btn_cancel;

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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethodMain.cs", 26);
		}
	}

	public frmIntroductionMethodMain(string contextGuid, IntroductionMethodBusiness _obj)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			_cls = _obj;
			((Control)txtName).Text = _cls?.Name ?? "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethodMain.cs", 38);
		}
	}

	private async void btnSabt_OnClick(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (_cls.Guid == Guid.Empty)
			{
				_cls.Guid = Guid.NewGuid();
				_cls.Modified = DateTime.Now;
				_cls.Status = true;
			}
			_cls.Name = ((Control)txtName).Text;
			_cls.ServerStatus = ServerStatusForClient.None;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _cls.SaveAsync(base.system, sendToServer: true), ReturnedState.None, "btnSabt_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethodMain.cs", 55);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnSabt_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethodMain.cs", 59);
			res.AddReturnedValue(ex, "", "btnSabt_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethodMain.cs", 60);
		}
		finally
		{
			if (res.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(res, "پیغام سیستم", "btnSabt_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethodMain.cs");
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
	}

	private void btn_cancel_OnClick(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btn_cancel_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethodMain.cs", 79);
		}
	}

	private void frmIntroductionMethodMain_KeyDown(object sender, KeyEventArgs e)
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
					btnSabt.PerformClick();
				}
			}
			else
			{
				btn_cancel.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmIntroductionMethodMain_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethodMain.cs", 91);
		}
	}

	private void frmIntroductionMethodMain_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void InitializeComponent()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Expected O, but got Unknown
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Expected O, but got Unknown
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Expected O, but got Unknown
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Expected O, but got Unknown
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Expected O, but got Unknown
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmIntroductionMethodMain));
		pnlPublicInfoMaster = new PanelEx();
		txtName = new TextBox();
		Label1 = new Label();
		btnSabt = new UcButtons();
		btn_cancel = new UcButtons();
		((Control)pnlPublicInfoMaster).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)pnlPublicInfoMaster).Anchor = (AnchorStyles)13;
		pnlPublicInfoMaster.CanvasColor = Color.Transparent;
		((Control)pnlPublicInfoMaster).Controls.Add((Control)(object)txtName);
		((Control)pnlPublicInfoMaster).Controls.Add((Control)(object)Label1);
		pnlPublicInfoMaster.DisabledBackColor = Color.Empty;
		((Control)pnlPublicInfoMaster).Location = new Point(4, 11);
		((Control)pnlPublicInfoMaster).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlPublicInfoMaster).Name = "pnlPublicInfoMaster";
		((Control)pnlPublicInfoMaster).Size = new Size(360, 48);
		pnlPublicInfoMaster.Style.BackColor1.Color = Color.White;
		pnlPublicInfoMaster.Style.BackColor2.Color = Color.White;
		pnlPublicInfoMaster.Style.Border = eBorderType.SingleLine;
		pnlPublicInfoMaster.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		pnlPublicInfoMaster.Style.BorderWidth = 2;
		pnlPublicInfoMaster.Style.CornerDiameter = 10;
		pnlPublicInfoMaster.Style.CornerType = eCornerType.Rounded;
		((Control)pnlPublicInfoMaster).TabIndex = 152;
		((Control)txtName).Anchor = (AnchorStyles)13;
		((Control)txtName).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)txtName).Location = new Point(19, 12);
		((Control)txtName).Name = "txtName";
		((Control)txtName).Size = new Size(241, 27);
		((Control)txtName).TabIndex = 59;
		((Control)Label1).Anchor = (AnchorStyles)9;
		((Control)Label1).AutoSize = true;
		((Control)Label1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)Label1).Location = new Point(265, 15);
		((Control)Label1).Name = "Label1";
		((Control)Label1).Size = new Size(81, 20);
		((Control)Label1).TabIndex = 63;
		((Control)Label1).Text = "[عنوان وضعیت]:";
		Label1.TextAlign = (ContentAlignment)64;
		((Control)btnSabt).Anchor = (AnchorStyles)14;
		((Control)btnSabt).BackColor = Color.Transparent;
		btnSabt.CornerType = EnButtonCornerType.Round;
		((Control)btnSabt).Cursor = Cursors.Hand;
		btnSabt.DisplayText = "[تایید](F5)";
		((Control)btnSabt).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnSabt.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnSabt.IsShowMoreDetails = false;
		((Control)btnSabt).Location = new Point(21, 64);
		((Control)btnSabt).Margin = new Padding(4, 5, 4, 5);
		((Control)btnSabt).Name = "btnSabt";
		((Control)btnSabt).RightToLeft = (RightToLeft)1;
		((Control)btnSabt).Size = new Size(241, 30);
		((Control)btnSabt).TabIndex = 155;
		btnSabt.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnSabt.Type = EnButtonType.Bold;
		btnSabt.OnClick += btnSabt_OnClick;
		((Control)btn_cancel).Anchor = (AnchorStyles)6;
		((Control)btn_cancel).BackColor = Color.Transparent;
		btn_cancel.CornerType = EnButtonCornerType.Round;
		((Control)btn_cancel).Cursor = Cursors.Hand;
		btn_cancel.DisplayText = "[انصراف](Esc)";
		((Control)btn_cancel).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btn_cancel.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btn_cancel.IsShowMoreDetails = false;
		((Control)btn_cancel).Location = new Point(21, 97);
		((Control)btn_cancel).Margin = new Padding(4, 5, 4, 5);
		((Control)btn_cancel).Name = "btn_cancel";
		((Control)btn_cancel).RightToLeft = (RightToLeft)1;
		((Control)btn_cancel).Size = new Size(241, 30);
		((Control)btn_cancel).TabIndex = 156;
		btn_cancel.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btn_cancel.Type = EnButtonType.Normal;
		btn_cancel.OnClick += btn_cancel_OnClick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(368, 133);
		((Control)this).Controls.Add((Control)(object)btnSabt);
		((Control)this).Controls.Add((Control)(object)btn_cancel);
		((Control)this).Controls.Add((Control)(object)pnlPublicInfoMaster);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmIntroductionMethodMain";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[مدیریت نحوه آشنایی]";
		((Form)this).FormClosed += new FormClosedEventHandler(frmIntroductionMethodMain_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmIntroductionMethodMain_KeyDown);
		((Control)pnlPublicInfoMaster).ResumeLayout(false);
		((Control)pnlPublicInfoMaster).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
