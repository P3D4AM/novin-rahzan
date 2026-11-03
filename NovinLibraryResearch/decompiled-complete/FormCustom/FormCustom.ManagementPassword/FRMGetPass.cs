using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom.Properties;
using PacketParser.Entities;

namespace FormCustom.ManagementPassword;

public class FRMGetPass : AccountingBaseForm
{
	private bool _isDisposed;

	private Container components;

	private PanelEx panelEx2;

	private Label label1;

	internal TextBox TXTPas;

	private PanelEx panelEx1;

	private Label lblOK;

	private PanelEx panelEx3;

	private Label lblBack;

	private Label label2;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\FRMGetPass.cs", 25);
		}
	}

	public FRMGetPass(string contextGuid)
		: base(contextGuid)
	{
		InitializeComponent();
	}

	private async void OK_Button_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)TXTPas).Text == (string)CLSSystem.GlobalPass)
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
			else
			{
				await this.ShowErrorAsync("رمز وارد شده صحیح نیست");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OK_Button_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\FRMGetPass.cs", 41);
		}
	}

	private void Cancel_Button_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Cancel_Button_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\FRMGetPass.cs", 50);
		}
	}

	private void TXTPas_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				OK_Button_Click(sender, (EventArgs)(object)e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTPas_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\FRMGetPass.cs", 59);
		}
	}

	private void FRMGetPass_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 27)
			{
				Cancel_Button_Click(sender, (EventArgs)(object)e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMGetPass_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\FRMGetPass.cs", 69);
		}
	}

	private void FRMGetPass_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	[DebuggerStepThrough]
	private void InitializeComponent()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Expected O, but got Unknown
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Expected O, but got Unknown
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Unknown result type (might be due to invalid IL or missing references)
		//IL_067f: Expected O, but got Unknown
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Expected O, but got Unknown
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Expected O, but got Unknown
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dc: Expected O, but got Unknown
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d5: Expected O, but got Unknown
		//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0929: Unknown result type (might be due to invalid IL or missing references)
		//IL_0933: Expected O, but got Unknown
		//IL_093b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Expected O, but got Unknown
		panelEx2 = new PanelEx();
		panelEx3 = new PanelEx();
		lblBack = new Label();
		panelEx1 = new PanelEx();
		lblOK = new Label();
		TXTPas = new TextBox();
		label2 = new Label();
		label1 = new Label();
		((Control)panelEx2).SuspendLayout();
		((Control)panelEx3).SuspendLayout();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx2.CanvasColor = Color.Transparent;
		((Control)panelEx2).Controls.Add((Control)(object)panelEx3);
		((Control)panelEx2).Controls.Add((Control)(object)panelEx1);
		((Control)panelEx2).Controls.Add((Control)(object)TXTPas);
		((Control)panelEx2).Controls.Add((Control)(object)label2);
		((Control)panelEx2).Controls.Add((Control)(object)label1);
		panelEx2.DisabledBackColor = Color.Empty;
		((Control)panelEx2).Dock = (DockStyle)5;
		((Control)panelEx2).Location = new Point(0, 0);
		((Control)panelEx2).Margin = new Padding(5, 8, 5, 8);
		((Control)panelEx2).Name = "panelEx2";
		((Control)panelEx2).Size = new Size(301, 172);
		panelEx2.Style.BackColor1.Color = Color.Transparent;
		panelEx2.Style.BackColor2.Color = Color.Transparent;
		panelEx2.Style.BackgroundImage = (Image)(object)FormCustom.Properties.Resources.RightSide;
		panelEx2.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx2.Style.BorderWidth = 2;
		panelEx2.Style.CornerDiameter = 15;
		panelEx2.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx2).TabIndex = 38;
		((Control)panelEx3).Anchor = (AnchorStyles)14;
		panelEx3.CanvasColor = Color.Transparent;
		((Control)panelEx3).Controls.Add((Control)(object)lblBack);
		panelEx3.DisabledBackColor = Color.Empty;
		((Control)panelEx3).Location = new Point(23, 96);
		((Control)panelEx3).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx3).Name = "panelEx3";
		((Control)panelEx3).Size = new Size(102, 39);
		panelEx3.Style.BackColor1.Color = Color.Transparent;
		panelEx3.Style.BackColor2.Color = Color.Transparent;
		panelEx3.Style.Border = eBorderType.SingleLine;
		panelEx3.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx3.Style.BorderWidth = 2;
		panelEx3.Style.CornerDiameter = 15;
		panelEx3.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx3).TabIndex = 53;
		((Control)lblBack).Cursor = Cursors.Hand;
		((Control)lblBack).Dock = (DockStyle)5;
		((Control)lblBack).Font = new Font("B Yekan", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblBack).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lblBack).Location = new Point(0, 0);
		((Control)lblBack).Margin = new Padding(4, 0, 4, 0);
		((Control)lblBack).Name = "lblBack";
		((Control)lblBack).Size = new Size(102, 39);
		((Control)lblBack).TabIndex = 0;
		((Control)lblBack).Text = "[انصراف]";
		lblBack.TextAlign = (ContentAlignment)32;
		((Control)lblBack).Click += Cancel_Button_Click;
		((Control)panelEx1).Anchor = (AnchorStyles)14;
		panelEx1.CanvasColor = Color.Transparent;
		((Control)panelEx1).Controls.Add((Control)(object)lblOK);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Location = new Point(177, 96);
		((Control)panelEx1).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(102, 39);
		panelEx1.Style.BackColor1.Color = Color.FromArgb(65, 81, 220);
		panelEx1.Style.BackColor2.Color = Color.FromArgb(65, 81, 220);
		panelEx1.Style.Border = eBorderType.SingleLine;
		panelEx1.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx1.Style.CornerDiameter = 15;
		panelEx1.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx1).TabIndex = 49;
		((Control)lblOK).Cursor = Cursors.Hand;
		((Control)lblOK).Dock = (DockStyle)5;
		((Control)lblOK).Font = new Font("B Yekan", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblOK).ForeColor = Color.White;
		((Control)lblOK).Location = new Point(0, 0);
		((Control)lblOK).Margin = new Padding(4, 0, 4, 0);
		((Control)lblOK).Name = "lblOK";
		((Control)lblOK).Size = new Size(102, 39);
		((Control)lblOK).TabIndex = 0;
		((Control)lblOK).Text = "[تایید]";
		lblOK.TextAlign = (ContentAlignment)32;
		((Control)lblOK).Click += OK_Button_Click;
		((Control)TXTPas).Anchor = (AnchorStyles)13;
		((Control)TXTPas).BackColor = Color.Gainsboro;
		((Control)TXTPas).Font = new Font("B Yekan", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)TXTPas).Location = new Point(23, 49);
		((Control)TXTPas).Name = "TXTPas";
		((Control)TXTPas).Size = new Size(256, 32);
		((Control)TXTPas).TabIndex = 3;
		TXTPas.TextAlign = (HorizontalAlignment)2;
		TXTPas.UseSystemPasswordChar = true;
		((Control)TXTPas).KeyDown += new KeyEventHandler(TXTPas_KeyDown);
		((Control)label2).Anchor = (AnchorStyles)14;
		((Control)label2).Font = new Font("B Yekan", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label2).ForeColor = Color.Gray;
		((Control)label2).Location = new Point(13, 145);
		((Control)label2).Margin = new Padding(4, 0, 4, 0);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(275, 18);
		((Control)label2).TabIndex = 0;
		((Control)label2).Text = "[رمز پیش فرض]:05137597590";
		label2.TextAlign = (ContentAlignment)32;
		((Control)label1).Anchor = (AnchorStyles)13;
		((Control)label1).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label1).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)label1).Location = new Point(4, 9);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(293, 32);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "[لطفا رمز مدیریت را وارد کنید]";
		label1.TextAlign = (ContentAlignment)32;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Black;
		((Form)this).ClientSize = new Size(301, 172);
		((Control)this).Controls.Add((Control)(object)panelEx2);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMGetPass";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "FRMGetPass";
		((Form)this).TransparencyKey = Color.Black;
		((Form)this).FormClosed += new FormClosedEventHandler(FRMGetPass_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(FRMGetPass_KeyDown);
		((Control)panelEx2).ResumeLayout(false);
		((Control)panelEx2).PerformLayout();
		((Control)panelEx3).ResumeLayout(false);
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
