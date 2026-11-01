using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;

namespace TimeAttendancePrint.UI;

public class FrmPrint : CacheBaseForm, IHasContext
{
	private DataSet _ds = new DataSet();

	private DataView _dv = new DataView();

	private Print.EnPrinterType _paper;

	private EnumPrintShow _printType;

	private List<object> _list;

	private List<object> _list2;

	private string _extraVar;

	private int _sanadId;

	private CancellationTokenSource tokenSetLables = new CancellationTokenSource();

	private Container components;

	internal PanelEx PanelEx1;

	internal PanelEx PanelEx2;

	internal ButtonX BtnPrintPreview;

	internal ButtonX BtnDesign;

	internal ButtonX BtnPrint;

	internal RadioButton RBtnA5;

	internal RadioButton RbtnA4;

	internal LabelX LabelX1;

	public string ContextGuid { get; set; }

	public FrmPrint(string contextGuid, EnumPrintShow printType, List<object> lst, DataView dv, int sanadId, string extraVar, List<object> lst2 = null)
		: base(contextGuid)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
		try
		{
			InitializeComponent();
			_list = lst;
			if (lst2 != null)
			{
				_list2 = lst2;
			}
			_printType = printType;
			_dv = dv;
			_sanadId = sanadId;
			_extraVar = extraVar;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\UI\\FRMPrint.cs", 44);
		}
	}

	private async Task TranslateFormControls(CancellationToken token)
	{
		try
		{
			string labelX1 = Utilities.GetTranslate("[نوع کاغذ :]");
			string btnPrintPreview = Utilities.GetTranslate("[پیش نمایش چاپ]");
			string btnPrint = Utilities.GetTranslate("[چاپ]");
			string btnDesign = Utilities.GetTranslate("[طراحی]");
			string text = Utilities.GetTranslate("[چاپ]");
			while (!((Control)this).IsHandleCreated && !((CacheBaseForm)this).IsDisposed && !token.IsCancellationRequested)
			{
				await Task.Delay(100);
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!((CacheBaseForm)this).IsDisposed && !token.IsCancellationRequested)
					{
						((Control)LabelX1).Text = labelX1;
						((Control)BtnPrintPreview).Text = btnPrintPreview;
						((Control)BtnPrint).Text = btnPrint;
						((Control)BtnDesign).Text = btnDesign;
						((Control)this).Text = text;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog("error in TranslateFormControls() in form frmPrint\r\n" + ex2.Message, "", "TranslateFormControls", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\UI\\FRMPrint.cs", 72);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TranslateFormControls", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\UI\\FRMPrint.cs", 75);
		}
	}

	private void BtnPrintPreview_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new ReportGenerator(ContextGuid, _printType, _paper, _list, _dv, _sanadId, _extraVar, _list2).PrintPreview();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPrintPreview_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\UI\\FRMPrint.cs", 87);
		}
	}

	private async void BtnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			await new ReportGenerator(ContextGuid, _printType, _paper, _list, _dv, _sanadId, _extraVar, _list2).PrintAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPrint_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\UI\\FRMPrint.cs", 100);
			AggregatorExtention.ShowError((CacheBaseForm)(object)this, "فایل های مربوط به StimulSoft پیدا نشد لطفا از طریق InternetUpdater فایل های مورد نیاز را دانلود کنید", "خطا");
		}
	}

	private void BTNDesign_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new ReportGenerator(ContextGuid, _printType, _paper, _list, _dv, _sanadId, _extraVar, _list2).Design();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNDesign_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\UI\\FRMPrint.cs", 114);
		}
	}

	private void FRMPrint_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 27)
			{
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMPrint_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\UI\\FRMPrint.cs", 127);
		}
	}

	private void RbtnA4_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (RbtnA4.Checked)
			{
				_paper = Print.EnPrinterType.AllA4Printers;
			}
			else if (RBtnA5.Checked)
			{
				_paper = Print.EnPrinterType.AllA5Printers;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RbtnA4_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\UI\\FRMPrint.cs", 142);
		}
	}

	private void RBtnA5_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (RbtnA4.Checked)
			{
				_paper = Print.EnPrinterType.AllA4Printers;
			}
			else if (RBtnA5.Checked)
			{
				_paper = Print.EnPrinterType.AllA5Printers;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RBtnA5_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\UI\\FRMPrint.cs", 156);
		}
	}

	private void FrmPrint_Load(object sender, EventArgs e)
	{
		tokenSetLables?.Cancel();
		tokenSetLables = new CancellationTokenSource();
		CancellationToken token = tokenSetLables.Token;
		try
		{
			Task.Run(async delegate
			{
				await TranslateFormControls(token);
			}, token);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "FrmPrint_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\UI\\FRMPrint.cs", 176);
		}
	}

	[DebuggerNonUserCode]
	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		finally
		{
			((CacheBaseForm)this).Dispose(disposing);
		}
	}

	[DebuggerStepThrough]
	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Expected O, but got Unknown
		//IL_078b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0795: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		PanelEx2 = new PanelEx();
		RBtnA5 = new RadioButton();
		RbtnA4 = new RadioButton();
		LabelX1 = new LabelX();
		BtnPrintPreview = new ButtonX();
		BtnPrint = new ButtonX();
		BtnDesign = new ButtonX();
		((Control)PanelEx1).SuspendLayout();
		((Control)PanelEx2).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)PanelEx2);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(354, 116);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		PanelEx2.CanvasColor = SystemColors.Control;
		PanelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx2).Controls.Add((Control)(object)RBtnA5);
		((Control)PanelEx2).Controls.Add((Control)(object)RbtnA4);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX1);
		((Control)PanelEx2).Controls.Add((Control)(object)BtnPrintPreview);
		((Control)PanelEx2).Controls.Add((Control)(object)BtnPrint);
		((Control)PanelEx2).Controls.Add((Control)(object)BtnDesign);
		PanelEx2.DisabledBackColor = Color.Empty;
		((Control)PanelEx2).Location = new Point(12, 12);
		((Control)PanelEx2).Name = "PanelEx2";
		((Control)PanelEx2).Size = new Size(330, 92);
		PanelEx2.Style.Alignment = (StringAlignment)1;
		PanelEx2.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx2.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx2.Style.Border = (eBorderType)2;
		PanelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx2.Style.BorderWidth = 3;
		PanelEx2.Style.CornerType = (eCornerType)2;
		PanelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx2.Style.GradientAngle = 90;
		((Control)PanelEx2).TabIndex = 0;
		((Control)RBtnA5).AutoSize = true;
		((Control)RBtnA5).ForeColor = Color.DarkBlue;
		((Control)RBtnA5).Location = new Point(169, 14);
		((Control)RBtnA5).Name = "RBtnA5";
		((Control)RBtnA5).Size = new Size(40, 17);
		((Control)RBtnA5).TabIndex = 4;
		RBtnA5.TabStop = true;
		((Control)RBtnA5).Text = "A5";
		((ButtonBase)RBtnA5).UseVisualStyleBackColor = true;
		RBtnA5.CheckedChanged += RBtnA5_CheckedChanged;
		((Control)RbtnA4).AutoSize = true;
		RbtnA4.Checked = true;
		((Control)RbtnA4).ForeColor = Color.DarkBlue;
		((Control)RbtnA4).Location = new Point(215, 14);
		((Control)RbtnA4).Name = "RbtnA4";
		((Control)RbtnA4).Size = new Size(40, 17);
		((Control)RbtnA4).TabIndex = 3;
		RbtnA4.TabStop = true;
		((Control)RbtnA4).Text = "A4";
		((ButtonBase)RbtnA4).UseVisualStyleBackColor = true;
		RbtnA4.CheckedChanged += RbtnA4_CheckedChanged;
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Location = new Point(261, 11);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(56, 23);
		((Control)LabelX1).TabIndex = 1;
		((Control)LabelX1).Text = "[نوع کاغذ:]";
		((Control)BtnPrintPreview).AccessibleRole = (AccessibleRole)43;
		BtnPrintPreview.ColorTable = (eButtonColor)6;
		((Control)BtnPrintPreview).Location = new Point(224, 45);
		((Control)BtnPrintPreview).Name = "BtnPrintPreview";
		((Control)BtnPrintPreview).Size = new Size(95, 35);
		((PopupItemControl)BtnPrintPreview).Style = (eDotNetBarStyle)9;
		((Control)BtnPrintPreview).TabIndex = 0;
		((Control)BtnPrintPreview).Text = "[پیش نمایش چاپ]";
		((Control)BtnPrintPreview).Click += BtnPrintPreview_Click;
		((Control)BtnPrint).AccessibleRole = (AccessibleRole)43;
		BtnPrint.ColorTable = (eButtonColor)6;
		((Control)BtnPrint).Location = new Point(14, 45);
		((Control)BtnPrint).Name = "BtnPrint";
		((Control)BtnPrint).Size = new Size(95, 35);
		((PopupItemControl)BtnPrint).Style = (eDotNetBarStyle)9;
		((Control)BtnPrint).TabIndex = 2;
		((Control)BtnPrint).Text = "[چاپ]";
		((Control)BtnPrint).Click += BtnPrint_Click;
		((Control)BtnDesign).AccessibleRole = (AccessibleRole)43;
		BtnDesign.ColorTable = (eButtonColor)6;
		((Control)BtnDesign).Location = new Point(119, 45);
		((Control)BtnDesign).Name = "BtnDesign";
		((Control)BtnDesign).Size = new Size(95, 35);
		((PopupItemControl)BtnDesign).Style = (eDotNetBarStyle)9;
		((Control)BtnDesign).TabIndex = 1;
		((Control)BtnDesign).Text = "[طراحی]";
		((Control)BtnDesign).Click += BTNDesign_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(354, 116);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(370, 155);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(370, 155);
		((Control)this).Name = "FrmPrint";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowIcon = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[چاپ]";
		((Form)this).Load += FrmPrint_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMPrint_KeyDown);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)PanelEx2).ResumeLayout(false);
		((Control)PanelEx2).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
