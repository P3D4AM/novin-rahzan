using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom.UC;
using PacketParser.Entities;

namespace FormCustom.Forms;

public class frmReturnedState : AccountingBaseForm
{
	private IContainer components;

	internal RadioButton rbtnError;

	internal RadioButton rbtnNone;

	internal RadioButton rbtnWarning;

	private PanelEx pnlPublicInfo;

	private UcButtons btn_sabt;

	private UcButtons btn_cancel;

	public ReturnedState State { get; private set; }

	public frmReturnedState(string contextGuid)
		: base(contextGuid)
	{
		InitializeComponent();
	}

	private void btn_sabt_OnClick(object sender, EventArgs e)
	{
		try
		{
			if (rbtnError.Checked)
			{
				State = ReturnedState.Error;
			}
			else if (rbtnWarning.Checked)
			{
				State = ReturnedState.Warning;
			}
			else
			{
				State = ReturnedState.None;
			}
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btn_sabt_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmReturnedState.cs", 26);
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
			WebErrorLog.Instance.StartLog(ex, "", "btn_cancel_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmReturnedState.cs", 35);
		}
	}

	private void frmReturnedState_KeyDown(object sender, KeyEventArgs e)
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
					btn_sabt.PerformClick();
				}
			}
			else
			{
				btn_cancel.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmReturnedState_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmReturnedState.cs", 47);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Expected O, but got Unknown
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Expected O, but got Unknown
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Expected O, but got Unknown
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Expected O, but got Unknown
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Expected O, but got Unknown
		rbtnError = new RadioButton();
		rbtnNone = new RadioButton();
		rbtnWarning = new RadioButton();
		pnlPublicInfo = new PanelEx();
		btn_sabt = new UcButtons();
		btn_cancel = new UcButtons();
		((Control)pnlPublicInfo).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)rbtnError).AutoSize = true;
		((Control)rbtnError).Cursor = Cursors.Hand;
		((Control)rbtnError).Font = new Font("B Yekan", 9.75f);
		((Control)rbtnError).ForeColor = Color.Black;
		((Control)rbtnError).Location = new Point(107, 6);
		((Control)rbtnError).Name = "rbtnError";
		((Control)rbtnError).Size = new Size(43, 24);
		((Control)rbtnError).TabIndex = 3;
		((Control)rbtnError).Text = "[خطا]";
		((ButtonBase)rbtnError).UseVisualStyleBackColor = true;
		((Control)rbtnNone).AutoSize = true;
		rbtnNone.Checked = true;
		((Control)rbtnNone).Cursor = Cursors.Hand;
		((Control)rbtnNone).Font = new Font("B Yekan", 9.75f);
		((Control)rbtnNone).ForeColor = Color.Black;
		((Control)rbtnNone).Location = new Point(86, 59);
		((Control)rbtnNone).Name = "rbtnNone";
		((Control)rbtnNone).Size = new Size(64, 24);
		((Control)rbtnNone).TabIndex = 5;
		rbtnNone.TabStop = true;
		((Control)rbtnNone).Text = "[بی تاثیر]";
		((ButtonBase)rbtnNone).UseVisualStyleBackColor = true;
		((Control)rbtnWarning).AutoSize = true;
		((Control)rbtnWarning).Cursor = Cursors.Hand;
		((Control)rbtnWarning).Font = new Font("B Yekan", 9.75f);
		((Control)rbtnWarning).ForeColor = Color.Black;
		((Control)rbtnWarning).Location = new Point(89, 33);
		((Control)rbtnWarning).Name = "rbtnWarning";
		((Control)rbtnWarning).Size = new Size(61, 24);
		((Control)rbtnWarning).TabIndex = 4;
		((Control)rbtnWarning).Text = "[هشدار]";
		((ButtonBase)rbtnWarning).UseVisualStyleBackColor = true;
		pnlPublicInfo.CanvasColor = Color.Transparent;
		((Control)pnlPublicInfo).Controls.Add((Control)(object)rbtnError);
		((Control)pnlPublicInfo).Controls.Add((Control)(object)rbtnNone);
		((Control)pnlPublicInfo).Controls.Add((Control)(object)rbtnWarning);
		pnlPublicInfo.DisabledBackColor = Color.Empty;
		((Control)pnlPublicInfo).Location = new Point(3, 4);
		((Control)pnlPublicInfo).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlPublicInfo).Name = "pnlPublicInfo";
		((Control)pnlPublicInfo).Size = new Size(162, 93);
		pnlPublicInfo.Style.BackColor1.Color = SystemColors.ActiveCaption;
		pnlPublicInfo.Style.BackColor2.Color = SystemColors.ActiveCaption;
		pnlPublicInfo.Style.Border = eBorderType.SingleLine;
		pnlPublicInfo.Style.BorderColor.Color = Color.Black;
		pnlPublicInfo.Style.BorderWidth = 2;
		pnlPublicInfo.Style.CornerDiameter = 10;
		pnlPublicInfo.Style.CornerType = eCornerType.Rounded;
		((Control)pnlPublicInfo).TabIndex = 10;
		((Control)btn_sabt).Anchor = (AnchorStyles)14;
		((Control)btn_sabt).BackColor = Color.Transparent;
		btn_sabt.CornerType = EnButtonCornerType.Round;
		((Control)btn_sabt).Cursor = Cursors.Hand;
		btn_sabt.DisplayText = "[تایید] (F5)";
		((Control)btn_sabt).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btn_sabt.IsShowMoreDetails = false;
		((Control)btn_sabt).Location = new Point(3, 108);
		((Control)btn_sabt).Margin = new Padding(4, 5, 4, 5);
		((Control)btn_sabt).Name = "btn_sabt";
		((Control)btn_sabt).RightToLeft = (RightToLeft)1;
		((Control)btn_sabt).Size = new Size(162, 30);
		((Control)btn_sabt).TabIndex = 11;
		btn_sabt.Type = EnButtonType.Bold;
		btn_sabt.OnClick += btn_sabt_OnClick;
		((Control)btn_cancel).Anchor = (AnchorStyles)6;
		((Control)btn_cancel).BackColor = Color.Transparent;
		btn_cancel.CornerType = EnButtonCornerType.Round;
		((Control)btn_cancel).Cursor = Cursors.Hand;
		btn_cancel.DisplayText = "[انصراف] (Esc)";
		((Control)btn_cancel).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btn_cancel.IsShowMoreDetails = false;
		((Control)btn_cancel).Location = new Point(3, 141);
		((Control)btn_cancel).Margin = new Padding(4, 5, 4, 5);
		((Control)btn_cancel).Name = "btn_cancel";
		((Control)btn_cancel).RightToLeft = (RightToLeft)1;
		((Control)btn_cancel).Size = new Size(162, 30);
		((Control)btn_cancel).TabIndex = 12;
		btn_cancel.Type = EnButtonType.Normal;
		btn_cancel.OnClick += btn_cancel_OnClick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = SystemColors.ActiveCaption;
		((Form)this).ClientSize = new Size(168, 180);
		((Control)this).Controls.Add((Control)(object)btn_cancel);
		((Control)this).Controls.Add((Control)(object)btn_sabt);
		((Control)this).Controls.Add((Control)(object)pnlPublicInfo);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmReturnedState";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).KeyDown += new KeyEventHandler(frmReturnedState_KeyDown);
		((Control)pnlPublicInfo).ResumeLayout(false);
		((Control)pnlPublicInfo).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
