using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom.Cache;
using PacketParser.Entities;
using PayamakUi.UC.UCSMS;

namespace PayamakUi.Forms.FRMOprate;

public class FRMSend : CacheBaseForm
{
	private CancellationTokenSource tokenSource = new CancellationTokenSource();

	private IContainer components;

	internal PanelEx PanelEx1;

	internal UCSend UcSend1;

	public string Message
	{
		get
		{
			return UcSend1.Message;
		}
		set
		{
			UcSend1.Message = value;
		}
	}

	public string Mobiles
	{
		get
		{
			return UcSend1.Mobiles;
		}
		set
		{
			UcSend1.Mobiles = value;
		}
	}

	public FRMSend(string contextGuid, string message)
		: base(contextGuid)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		FRMSend fRMSend = this;
		try
		{
			InitializeComponent();
			((Control)this).KeyDown += new KeyEventHandler(FRMSend_KeyDown);
			UcSend1.ContextGuid = contextGuid;
			_ = UcSend1.TXTMessage.Text;
			UcSend1.TXTMessage.Text = Message + " " + message;
			UcSend1.SetListAsync(null);
			tokenSource = new CancellationTokenSource(60000);
			CancellationToken tkn = tokenSource.Token;
			Task.Run(() => fRMSend.UcSend1.LoadDataAsync(tkn));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMSend.cs", 33);
		}
	}

	public FRMSend(string contextGuid, List<object> list)
		: base(contextGuid)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		FRMSend fRMSend = this;
		try
		{
			InitializeComponent();
			((Control)this).KeyDown += new KeyEventHandler(FRMSend_KeyDown);
			UcSend1.ContextGuid = contextGuid;
			UcSend1.SetListAsync(list);
			tokenSource = new CancellationTokenSource(60000);
			CancellationToken tkn = tokenSource.Token;
			Task.Run(() => fRMSend.UcSend1.LoadDataAsync(tkn));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMSend.cs", 48);
		}
	}

	public FRMSend(string contextGuid)
		: base(contextGuid)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		FRMSend fRMSend = this;
		try
		{
			InitializeComponent();
			((Control)this).KeyDown += new KeyEventHandler(FRMSend_KeyDown);
			UcSend1.ContextGuid = contextGuid;
			UcSend1.SetListAsync(null);
			tokenSource = new CancellationTokenSource(60000);
			CancellationToken tkn = tokenSource.Token;
			Task.Run(() => fRMSend.UcSend1.LoadDataAsync(tkn));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMSend.cs", 63);
		}
	}

	private void FRMSend_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 27)
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMSend_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMSend.cs", 80);
		}
	}

	private void UcSend1_Exit_UserControl_2()
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UcSend1_Exit_UserControl_2", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMSend.cs", 89);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		PanelEx1 = new PanelEx();
		UcSend1 = new UCSend();
		((Control)PanelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)PanelEx1).Controls.Add((Control)(object)UcSend1);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(784, 506);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		PanelEx1.Style.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		PanelEx1.Style.Border = eBorderType.SingleLine;
		PanelEx1.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		PanelEx1.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 1;
		((Control)UcSend1).Anchor = (AnchorStyles)15;
		((Control)UcSend1).BackColor = Color.Transparent;
		UcSend1.ContextGuid = null;
		((Control)UcSend1).Location = new Point(0, 0);
		UcSend1.Message = "";
		UcSend1.Mobiles = "";
		((Control)UcSend1).Name = "UcSend1";
		UcSend1.Number_Currect = 0;
		UcSend1.Number_HamraheAval = 0;
		UcSend1.Number_InCorrect = 0;
		UcSend1.Number_IranCell = 0;
		UcSend1.Number_Rightel = 0;
		UcSend1.Number_Tekrari = 0;
		UcSend1.Number_Total = 0;
		((Control)UcSend1).Size = new Size(784, 506);
		((Control)UcSend1).TabIndex = 0;
		UcSend1.Exit_UserControl += UcSend1_Exit_UserControl_2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 506);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).KeyPreview = true;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMSend";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "ارسال";
		((Control)PanelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
