using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Gharardads;

public class FrmBaseYearsSalary : FormExtention
{
	private BaseYearsSalary _BaseYearsSalary;

	private string frm_Text = "[پایه سنواتی]";

	private List<GroupSanavat> _listData;

	private SortableBindingList<GroupSanavat> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	private PanelEx panelEx1;

	private NumericUpDown NumYear;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private LabelX labelX3;

	private TextBoxFa TxtPrice;

	private LabelX labelX1;

	private ButtonX BtnMoreGroup;

	private ComboBoxEx CmbGroup;

	private LabelX labelX6;

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

	public FrmBaseYearsSalary(string contextGuid)
		: base(contextGuid, true, true)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		try
		{
			_BaseYearsSalary = new BaseYearsSalary(((CacheBaseForm)this).ContextGuid);
			((Control)this).Text = "پایه سنواتی";
			frm_Text = "[پایه سنواتی]";
			((Control)NumYear).Enabled = true;
			NumYear.Value = 1m;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalary.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalary.cs", 46);
		}
	}

	public FrmBaseYearsSalary(string contextGuid, BaseYearsSalary pBaseYearsSalary)
		: base(contextGuid, true, true)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		try
		{
			_BaseYearsSalary = pBaseYearsSalary;
			((Control)this).Text = "ویرایش پایه سنواتی";
			frm_Text = "[ویرایش پایه سنواتی]";
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalary.cs");
			((Control)NumYear).Enabled = false;
			((ListControl)CmbGroup).SelectedValue = _BaseYearsSalary.GroupId;
			((Control)CmbGroup).Enabled = false;
			((Control)BtnMoreGroup).Enabled = false;
			NumYear.Value = _BaseYearsSalary.nYear;
			((Control)TxtPrice).Text = _BaseYearsSalary.Price.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalary.cs", 70);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			ComboBoxEx cmbGroup = CmbGroup;
			((ComboBox)cmbGroup).DataSource = await GroupSanavat.SelectAllAsync(((CacheBaseForm)this).ContextGuid, "", token);
			((ListControl)CmbGroup).DisplayMember = "Name";
			((ListControl)CmbGroup).ValueMember = "Id";
			((ListControl)CmbGroup).SelectedValue = 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalary.cs", 84);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		try
		{
			((FormExtention)this).OnLoad(e);
			((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
			((ToolStrip)base.statusStrip1).BackColor = ((Control)panelEx1).BackColor;
			((ListControl)CmbGroup).SelectedValue = _BaseYearsSalary.GroupId;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalary.cs", 98);
		}
	}

	private bool ValidObj()
	{
		bool flag = ((Control)BtnAccept).Enabled && ((Control)BtnAccept).Visible;
		try
		{
			_BaseYearsSalary.nYear = Convert.ToInt16(NumYear.Value);
			_BaseYearsSalary.GroupId = Convert.ToInt16(((ListControl)CmbGroup).SelectedValue);
			if (_BaseYearsSalary.GroupId == 0)
			{
				Utility.MessageBoxShowError("[مشخص کردن گروه الزامی است]", ((CacheBaseForm)this).ContextGuid);
				((Control)CmbGroup).Focus();
				flag = false;
			}
			_BaseYearsSalary.Price = Convert.ToDecimal(TxtPrice.Value);
			if (flag && _BaseYearsSalary.Price <= 0m)
			{
				Utility.MessageBoxShowError("[مبلغ نمیتواند کمتر از صفر باشد]", ((CacheBaseForm)this).ContextGuid);
				((Control)TxtPrice).Focus();
				flag = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ValidObj", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalary.cs", 126);
		}
		return flag;
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		try
		{
			if (!ValidObj())
			{
				return;
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			if (await _BaseYearsSalary.SaveAsync(token))
			{
				((FormExtention)this).ResetControlChanged();
				((Form)this).DialogResult = (DialogResult)1;
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)2;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalary.cs", 149);
		}
		((Form)this).Close();
	}

	private async void BtnMoreGroup_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)CmbGroup).Focus();
			FrmGroupSanavatList frm = new FrmGroupSanavatList(((CacheBaseForm)this).ContextGuid, pIsSelect: true);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnMoreGroup_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalary.cs");
				((ListControl)CmbGroup).SelectedValue = frm.ID;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMoreGroup_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalary.cs", 169);
		}
	}

	private void FrmBaseYearsSalary_Load(object sender, EventArgs e)
	{
		try
		{
			frm_Text = Utilities.GetTranslate(frm_Text);
			((Control)this).Text = frm_Text;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmBaseYearsSalary_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmBaseYearsSalary.cs", 182);
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
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Expected O, but got Unknown
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
		panelEx1 = new PanelEx();
		TxtPrice = new TextBoxFa();
		labelX1 = new LabelX();
		NumYear = new NumericUpDown();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		labelX3 = new LabelX();
		BtnMoreGroup = new ButtonX();
		CmbGroup = new ComboBoxEx();
		labelX6 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)NumYear).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnMoreGroup);
		((Control)panelEx1).Controls.Add((Control)(object)CmbGroup);
		((Control)panelEx1).Controls.Add((Control)(object)labelX6);
		((Control)panelEx1).Controls.Add((Control)(object)TxtPrice);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)NumYear);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(275, 172);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		TxtPrice.AllowEmpty = true;
		TxtPrice.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPrice.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPrice.BackcolorTooltip = Color.Red;
		TxtPrice.CurrencyFormat = "";
		TxtPrice.ForeColorTooltip = Color.Black;
		TxtPrice.GetColoresFromSettingDLL = true;
		TxtPrice.HandleKeydown = true;
		((Control)TxtPrice).Location = new Point(45, 33);
		((TextBoxBase)TxtPrice).MaxLength = 19;
		((Control)TxtPrice).Name = "TxtPrice";
		TxtPrice.SendShiftTabAfterDown = true;
		TxtPrice.SendShiftTabAfterEnter = true;
		TxtPrice.SendTabAfterEnter = true;
		TxtPrice.SendTabAfterUP = true;
		TxtPrice.ShowHorofi = (ENShowHorofi)3;
		TxtPrice.ShowToolTipText = true;
		((Control)TxtPrice).Size = new Size(110, 21);
		((Control)TxtPrice).TabIndex = 6;
		((Control)TxtPrice).Text = "0";
		((TextBox)TxtPrice).TextAlign = (HorizontalAlignment)2;
		TxtPrice.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPrice.ToolTipPostFix = "";
		TxtPrice.TootTipDelay = 1000;
		TxtPrice.Value = new decimal(new int[4]);
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(159, 34);
		((Control)labelX1).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(103, 19);
		((Control)labelX1).TabIndex = 5;
		((Control)labelX1).Text = "[مبلغ]:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)NumYear).Enabled = false;
		((Control)NumYear).Location = new Point(45, 7);
		NumYear.Maximum = new decimal(new int[4] { 30, 0, 0, 0 });
		NumYear.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)NumYear).Name = "NumYear";
		((Control)NumYear).Size = new Size(110, 21);
		((Control)NumYear).TabIndex = 1;
		((UpDownBase)NumYear).TextAlign = (HorizontalAlignment)2;
		NumYear.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(11, 102);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(117, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 8;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو](Esc)";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(153, 102);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		BtnAccept.Shortcuts.Add((eShortcut)116);
		((Control)BtnAccept).Size = new Size(111, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 7;
		((Control)BtnAccept).Text = "[ذخیره](F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(159, 8);
		((Control)labelX3).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(103, 19);
		((Control)labelX3).TabIndex = 0;
		((Control)labelX3).Text = "مدت سنوات(سال):";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)BtnMoreGroup).AccessibleRole = (AccessibleRole)43;
		BtnMoreGroup.ColorTable = (eButtonColor)6;
		((Control)BtnMoreGroup).Location = new Point(11, 59);
		((Control)BtnMoreGroup).Name = "BtnMoreGroup";
		((Control)BtnMoreGroup).Size = new Size(30, 23);
		((PopupItemControl)BtnMoreGroup).Style = (eDotNetBarStyle)9;
		((Control)BtnMoreGroup).TabIndex = 11;
		((Control)BtnMoreGroup).TabStop = false;
		((Control)BtnMoreGroup).Text = "...";
		((ListControl)CmbGroup).DisplayMember = "Name";
		((ComboBox)CmbGroup).DrawMode = (DrawMode)1;
		((ComboBox)CmbGroup).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbGroup).FormattingEnabled = true;
		((ComboBox)CmbGroup).ItemHeight = 16;
		((Control)CmbGroup).Location = new Point(45, 59);
		((Control)CmbGroup).Name = "CmbGroup";
		((Control)CmbGroup).Size = new Size(110, 22);
		CmbGroup.Style = (eDotNetBarStyle)9;
		((Control)CmbGroup).TabIndex = 10;
		((ListControl)CmbGroup).ValueMember = "Id";
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(159, 61);
		((Control)labelX6).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(110, 19);
		((Control)labelX6).TabIndex = 9;
		((Control)labelX6).Text = "[گروه]:";
		labelX6.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnCancel;
		((Form)this).ClientSize = new Size(275, 172);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmBaseYearsSalary";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[پایه سنواتی]*";
		((Form)this).Load += FrmBaseYearsSalary_Load;
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((ISupportInitialize)NumYear).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
