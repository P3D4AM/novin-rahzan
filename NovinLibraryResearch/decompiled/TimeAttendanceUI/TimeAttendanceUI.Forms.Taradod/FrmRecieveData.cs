using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Forms.Device;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Taradod;

public class FrmRecieveData : BaseForm
{
	public short DeviceNo;

	private FRMProgress _frm;

	private string Frm_Text = "[لیست عملکرد کاربران]";

	private IContainer components;

	private PanelEx panelEx1;

	private OpenFileDialog OFD;

	private GroupPanel groupPanel7;

	private TextBox RTxt;

	internal ButtonX BtnAccept;

	private ButtonX btnMore;

	private TextBoxX TXTDownload;

	private LabelX labelX1;

	internal PictureBox PictureBox1;

	public FrmRecieveData(string contextGuid)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			InitializeComponent();
			Frm_Text = "[دریافت اطلاعات پرسنل از دستگاه]";
			((Control)TXTDownload).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmRecieveData.cs", 35);
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		StringBuilder sb = new StringBuilder();
		try
		{
			CancellationToken token = default(CancellationToken);
			if (TXTDownload != null && ((Control)TXTDownload).Text.Trim() == "")
			{
				Utility.MessageBoxShowError("[فایل تردد انتخاب نشده است]", ((CacheBaseForm)this).ContextGuid);
			}
			else
			{
				if (!((Control)BtnAccept).Enabled || !((Control)BtnAccept).Visible)
				{
					return;
				}
				FrmSelectDevice frm = new FrmSelectDevice(((CacheBaseForm)this).ContextGuid);
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) != 1)
				{
					return;
				}
				DeviceNo = short.Parse(frm.DeviceNo.ToString());
				if (DeviceNo <= 0)
				{
					Utility.MessageBoxShowError("[شماره دستگاه انتخاب نشده]", ((CacheBaseForm)this).ContextGuid);
					return;
				}
				DeviceInfo device = await DeviceInfo.GetByDeviceNoAsync(((CacheBaseForm)this).ContextGuid, DeviceNo);
				Waiter waiter = new Waiter("چند لحظه صبر کنید", (Form)(object)this, (Task)null, false, (EnWaiterType)0, (Control)null, "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmRecieveData.cs");
				try
				{
					ImportTaradodResult importTaradodResult = await LogOnPersonel.ImportFileAsync(((CacheBaseForm)this).ContextGuid, "ReceiveData1", device, sb, ((Control)TXTDownload).Text, token);
					if (importTaradodResult.Result.HasError || importTaradodResult.Result.HasInformation)
					{
						importTaradodResult.Result.Show(((CacheBaseForm)this).ContextGuid);
					}
					((Control)RTxt).Text = importTaradodResult.FileContent;
					_frm = new FRMProgress(((CacheBaseForm)this).ContextGuid, importTaradodResult.CountKol, importTaradodResult.CountCorrect, importTaradodResult.CountInCorrect, importTaradodResult.Result.ErrorList.ToList());
					await ((CacheBaseForm)_frm).ShowAsync((IWin32Window)(object)this);
					((Component)(object)_frm).Dispose();
				}
				catch (Exception ex)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmRecieveData.cs", 78);
				}
				finally
				{
					if (waiter != null)
					{
						waiter.Dispose();
					}
				}
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmRecieveData.cs", 84);
		}
	}

	private async void SaveProgress(int count, int total, short st)
	{
		try
		{
			if (_frm == null)
			{
				_frm = new FRMProgress(((CacheBaseForm)this).ContextGuid, total);
				await ((CacheBaseForm)_frm).ShowAsync((IWin32Window)(object)this);
			}
			_frm.ItemCount = count;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveProgress", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmRecieveData.cs", 98);
		}
	}

	private async void FRMRecieveData_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 27)
			{
				if ((int)keyCode != 116)
				{
					if ((int)keyCode == 123)
					{
						e.Handled = true;
						FrmCalc frm = new FrmCalc(((CacheBaseForm)this).ContextGuid);
						await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
						((Component)(object)frm).Dispose();
					}
				}
				else
				{
					e.Handled = true;
					((PopupItemControl)BtnAccept).PerformClick();
				}
			}
			else
			{
				e.Handled = true;
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMRecieveData_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmRecieveData.cs", 124);
		}
	}

	private void btnMore_Click(object sender, EventArgs e)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		if (!((Control)btnMore).Enabled || !((Control)btnMore).Visible)
		{
			return;
		}
		try
		{
			if (OFD != null)
			{
				((FileDialog)OFD).Filter = "Attendance files(*.txt;*.csv;*.dat,*.xls)|*.txt;*.csv;*.dat;*.xls|All files (*.*)|*.*";
				OFD.Multiselect = false;
				if ((int)((CommonDialog)OFD).ShowDialog((IWin32Window)(object)this) == 1 && ((FileDialog)OFD).FileName != null)
				{
					((Control)TXTDownload).Text = ((FileDialog)OFD).FileName;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnMore_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmRecieveData.cs", 142);
		}
	}

	private void TXTDownload_Enter_1(object sender, EventArgs e)
	{
		try
		{
			((TextBoxBase)TXTDownload).SelectAll();
			((Control)TXTDownload).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TXTDownload_Enter_1", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmRecieveData.cs", 154);
		}
	}

	private void TXTDownload_Leave_1(object sender, EventArgs e)
	{
		try
		{
			((Control)TXTDownload).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TXTDownload_Leave_1", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmRecieveData.cs", 166);
		}
	}

	private void FrmRecieveData_Load(object sender, EventArgs e)
	{
		try
		{
			Frm_Text = Utilities.GetTranslate(Frm_Text);
			((Control)this).Text = Frm_Text;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmRecieveData_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmRecieveData.cs", 179);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((BaseForm)this).Dispose(disposing);
	}

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
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Expected O, but got Unknown
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Expected O, but got Unknown
		//IL_086f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0879: Expected O, but got Unknown
		//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dd: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		groupPanel7 = new GroupPanel();
		RTxt = new TextBox();
		BtnAccept = new ButtonX();
		labelX1 = new LabelX();
		btnMore = new ButtonX();
		PictureBox1 = new PictureBox();
		TXTDownload = new TextBoxX();
		OFD = new OpenFileDialog();
		((Control)panelEx1).SuspendLayout();
		((Control)groupPanel7).SuspendLayout();
		((ISupportInitialize)PictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)groupPanel7);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(505, 423);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)groupPanel7).Anchor = (AnchorStyles)15;
		((Control)groupPanel7).BackColor = Color.Transparent;
		((PanelControl)groupPanel7).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel7).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel7).Controls.Add((Control)(object)RTxt);
		((Control)groupPanel7).Controls.Add((Control)(object)BtnAccept);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel7).Controls.Add((Control)(object)btnMore);
		((Control)groupPanel7).Controls.Add((Control)(object)PictureBox1);
		((Control)groupPanel7).Controls.Add((Control)(object)TXTDownload);
		groupPanel7.DisabledBackColor = Color.Empty;
		((Control)groupPanel7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)groupPanel7).Location = new Point(12, 12);
		((Control)groupPanel7).Name = "groupPanel7";
		((Control)groupPanel7).Size = new Size(481, 399);
		((PanelControl)groupPanel7).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel7).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel7).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel7).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderBottomWidth = 3;
		((PanelControl)groupPanel7).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel7).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderLeftWidth = 3;
		((PanelControl)groupPanel7).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderRightWidth = 3;
		((PanelControl)groupPanel7).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderTopWidth = 3;
		((PanelControl)groupPanel7).Style.CornerDiameter = 4;
		((PanelControl)groupPanel7).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel7).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel7).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel7).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel7).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel7).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel7).TabIndex = 1;
		((Control)RTxt).Anchor = (AnchorStyles)15;
		((Control)RTxt).Location = new Point(6, 74);
		((TextBoxBase)RTxt).Multiline = true;
		((Control)RTxt).Name = "RTxt";
		((Control)RTxt).RightToLeft = (RightToLeft)0;
		RTxt.ScrollBars = (ScrollBars)3;
		((Control)RTxt).Size = new Size(463, 316);
		((Control)RTxt).TabIndex = 3;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)13;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(6, 29);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(367, 39);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 2;
		((Control)BtnAccept).Text = "[دریافت]";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX1).Location = new Point(379, 3);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(42, 23);
		((Control)labelX1).TabIndex = 0;
		((Control)labelX1).Text = "[آدرس]:";
		((Control)btnMore).AccessibleRole = (AccessibleRole)43;
		btnMore.ColorTable = (eButtonColor)6;
		((Control)btnMore).Location = new Point(6, 6);
		((Control)btnMore).Name = "btnMore";
		((Control)btnMore).Size = new Size(28, 21);
		((PopupItemControl)btnMore).Style = (eDotNetBarStyle)9;
		((Control)btnMore).TabIndex = 2;
		((Control)btnMore).TabStop = false;
		((Control)btnMore).Text = "...";
		((Control)btnMore).Click += btnMore_Click;
		((Control)PictureBox1).Anchor = (AnchorStyles)9;
		PictureBox1.BorderStyle = (BorderStyle)1;
		PictureBox1.Image = (Image)(object)Resources.download_file;
		((Control)PictureBox1).Location = new Point(427, 6);
		((Control)PictureBox1).Name = "PictureBox1";
		((Control)PictureBox1).Size = new Size(42, 62);
		PictureBox1.SizeMode = (PictureBoxSizeMode)4;
		PictureBox1.TabIndex = 25;
		PictureBox1.TabStop = false;
		((Control)TXTDownload).Anchor = (AnchorStyles)13;
		TXTDownload.Border.Class = "TextBoxBorder";
		TXTDownload.Border.CornerType = (eCornerType)1;
		((Control)TXTDownload).Location = new Point(40, 6);
		((Control)TXTDownload).Name = "TXTDownload";
		((Control)TXTDownload).RightToLeft = (RightToLeft)1;
		((Control)TXTDownload).Size = new Size(333, 21);
		((Control)TXTDownload).TabIndex = 1;
		((TextBox)TXTDownload).TextAlign = (HorizontalAlignment)1;
		((Control)TXTDownload).Enter += TXTDownload_Enter_1;
		((Control)TXTDownload).Leave += TXTDownload_Leave_1;
		((FileDialog)OFD).FileName = "AttendanceLog";
		((FileDialog)OFD).Title = "[فایل حاوی تردد ها را انتخاب نمائید.]";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(505, 423);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(513, 450);
		((Control)this).Name = "FrmRecieveData";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[دریافت اطلاعات دستگاه]";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).Load += FrmRecieveData_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMRecieveData_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)groupPanel7).ResumeLayout(false);
		((Control)groupPanel7).PerformLayout();
		((ISupportInitialize)PictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
