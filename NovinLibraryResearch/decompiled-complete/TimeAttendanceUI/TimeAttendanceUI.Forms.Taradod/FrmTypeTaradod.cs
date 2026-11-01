using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Taradod;

public class FrmTypeTaradod : FormExtention
{
	private LogonTypeTaradod cls;

	private string Frm_Text = "[لیست عملکرد کاربران]";

	private CancellationTokenSource fillFormTokenSource = new CancellationTokenSource();

	private short _traficId;

	private DeviceModelEnumType _device;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private bool _isDisposed;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private TextBoxX TxtTraficID;

	internal LabelX LabelX1;

	internal LabelX LabelX2;

	private ComboBoxEx txtConvertedTypeTaradod;

	internal LabelX labelX3;

	internal LabelX labelX4;

	private TextBoxX txtTypeTaradod;

	private TextBoxX txtModelName;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((FormExtention)this).IsDisposed;
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
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				((FormExtention)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmTypeTaradod(string contextGuid, short pTraficID, DeviceModelEnumType pDevice)
		: base(contextGuid, true, true)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		_traficId = pTraficID;
		_device = pDevice;
		Frm_Text = "[ویرایش نوع تردد]";
		((TextBoxBase)TxtTraficID).ReadOnly = true;
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			cls = await LogonTypeTaradod.GetAsync(((CacheBaseForm)this).ContextGuid, _traficId, _device, token);
			await FillFormAsync();
			((ComboBox)txtConvertedTypeTaradod).DataSource = LstTypeTaradod.GetAll();
			((ListControl)txtConvertedTypeTaradod).ValueMember = "Id";
			((ListControl)txtConvertedTypeTaradod).DisplayMember = "Name";
			base.objsCanEmpty.Add((Control)(object)TxtTraficID);
			base.objsCanEmpty.Add((Control)(object)txtModelName);
			base.objsCanEmpty.Add((Control)(object)txtTypeTaradod);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmTypeTaradod.cs", 82);
		}
	}

	private void Getdata()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		cls.ConvertedTypeTaradod = (TypeTaradodEnumType)((ListControl)txtConvertedTypeTaradod).SelectedValue;
	}

	private async Task FillFormAsync()
	{
		fillFormTokenSource?.Cancel();
		fillFormTokenSource = new CancellationTokenSource();
		CancellationToken token = fillFormTokenSource.Token;
		string typeTaradod = cls.TypeTaradodName;
		try
		{
			while (!((Control)this).IsHandleCreated && !IsDisposed && !token.IsCancellationRequested)
			{
				await Task.Delay(100);
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				//IL_0093: Unknown result type (might be due to invalid IL or missing references)
				//IL_009d: Expected I4, but got Unknown
				try
				{
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						((Control)TxtTraficID).Text = cls.TraficId.ToString();
						((Control)txtModelName).Text = cls.DeviceModelName;
						((Control)txtTypeTaradod).Text = typeTaradod;
						((ListControl)txtConvertedTypeTaradod).SelectedValue = (short)(int)cls.ConvertedTypeTaradod;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmTypeTaradod.cs", 114);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmTypeTaradod.cs", 120);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((FormExtention)this).OnLoad(e);
		((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
		((Control)BtnCancel).Click += base.BtnCancel_Click;
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnAccept).Enabled || !((Control)BtnAccept).Visible)
		{
			return;
		}
		try
		{
			Getdata();
			if (await cls.SaveAsync())
			{
				((FormExtention)this).ResetControlChanged();
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmTypeTaradod.cs", 151);
		}
	}

	private async void FrmTypeTaradod_Load(object sender, EventArgs e)
	{
		try
		{
			Frm_Text = Utilities.GetTranslate(Frm_Text);
			((Control)this).Text = Frm_Text;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			await LoadDataAsync(token);
			((Control)txtTypeTaradod).Text = cls?.TypeTaradodName;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmTypeTaradod_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmTypeTaradod.cs", 170);
		}
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
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Expected O, but got Unknown
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Expected O, but got Unknown
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Expected O, but got Unknown
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		txtConvertedTypeTaradod = new ComboBoxEx();
		labelX4 = new LabelX();
		BtnCancel = new ButtonX();
		labelX3 = new LabelX();
		BtnAccept = new ButtonX();
		LabelX1 = new LabelX();
		TxtTraficID = new TextBoxX();
		LabelX2 = new LabelX();
		txtModelName = new TextBoxX();
		txtTypeTaradod = new TextBoxX();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)txtConvertedTypeTaradod);
		((Control)panelEx1).Controls.Add((Control)(object)labelX4);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)LabelX1);
		((Control)panelEx1).Controls.Add((Control)(object)TxtTraficID);
		((Control)panelEx1).Controls.Add((Control)(object)LabelX2);
		((Control)panelEx1).Controls.Add((Control)(object)txtModelName);
		((Control)panelEx1).Controls.Add((Control)(object)txtTypeTaradod);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(328, 184);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((ListControl)txtConvertedTypeTaradod).DisplayMember = "Id";
		((ComboBox)txtConvertedTypeTaradod).DrawMode = (DrawMode)1;
		((ComboBox)txtConvertedTypeTaradod).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)txtConvertedTypeTaradod).FormattingEnabled = true;
		((ComboBox)txtConvertedTypeTaradod).ItemHeight = 16;
		((Control)txtConvertedTypeTaradod).Location = new Point(14, 93);
		((Control)txtConvertedTypeTaradod).Name = "txtConvertedTypeTaradod";
		((Control)txtConvertedTypeTaradod).Size = new Size(235, 22);
		txtConvertedTypeTaradod.Style = (eDotNetBarStyle)9;
		((Control)txtConvertedTypeTaradod).TabIndex = 7;
		((ListControl)txtConvertedTypeTaradod).ValueMember = "Id";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX4).Location = new Point(255, 93);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(55, 21);
		((Control)labelX4).TabIndex = 6;
		((Control)labelX4).Text = "[نوع تردد]:";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(14, 120);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(117, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 2;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX3).Location = new Point(255, 66);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(55, 21);
		((Control)labelX3).TabIndex = 4;
		((Control)labelX3).Text = "[نوع تردد]:";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(138, 120);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(111, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 1;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX1).Location = new Point(255, 12);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(55, 21);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "[کد تردد] :";
		LabelX1.TextAlignment = (StringAlignment)2;
		TxtTraficID.Border.Class = "TextBoxBorder";
		TxtTraficID.Border.CornerType = (eCornerType)1;
		((Control)TxtTraficID).Location = new Point(14, 12);
		((Control)TxtTraficID).Name = "TxtTraficID";
		((TextBoxBase)TxtTraficID).ReadOnly = true;
		((Control)TxtTraficID).Size = new Size(235, 21);
		((Control)TxtTraficID).TabIndex = 1;
		((BaseItemControl)LabelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX2).Location = new Point(255, 39);
		((Control)LabelX2).Name = "LabelX2";
		((Control)LabelX2).Size = new Size(66, 21);
		((Control)LabelX2).TabIndex = 2;
		((Control)LabelX2).Text = "[نام دستگاه] :";
		LabelX2.TextAlignment = (StringAlignment)2;
		txtModelName.Border.Class = "TextBoxBorder";
		txtModelName.Border.CornerType = (eCornerType)1;
		((Control)txtModelName).Location = new Point(14, 39);
		((Control)txtModelName).Name = "txtModelName";
		((TextBoxBase)txtModelName).ReadOnly = true;
		((Control)txtModelName).Size = new Size(235, 21);
		((Control)txtModelName).TabIndex = 3;
		txtTypeTaradod.Border.Class = "TextBoxBorder";
		txtTypeTaradod.Border.CornerType = (eCornerType)1;
		((Control)txtTypeTaradod).Location = new Point(14, 66);
		((Control)txtTypeTaradod).Name = "txtTypeTaradod";
		((TextBoxBase)txtTypeTaradod).ReadOnly = true;
		((Control)txtTypeTaradod).Size = new Size(235, 21);
		((Control)txtTypeTaradod).TabIndex = 5;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(328, 184);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(344, 223);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(344, 223);
		((Control)this).Name = "FrmTypeTaradod";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).Load += FrmTypeTaradod_Load;
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
