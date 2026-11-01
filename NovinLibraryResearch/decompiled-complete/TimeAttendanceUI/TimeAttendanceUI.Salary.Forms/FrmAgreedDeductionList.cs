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
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.Forms;

public class FrmAgreedDeductionList : FrmExtList
{
	private CancellationTokenSource _searchToken;

	private CancellationTokenSource _loadDataToken;

	private Font _font;

	private List<AgreedDeduction> _dGridLists = new List<AgreedDeduction>();

	private List<AgreedDeduction> _dGridListsForDisplay = new List<AgreedDeduction>();

	private string Frm_Text = "[کسورات توافقی]";

	private string trDetails = "[جزئیات]";

	private string trCountRecords = "[تعداد رکوردها]";

	private IContainer components;

	internal DataGridViewX DGrid;

	private LabelX LblRowCount;

	private BindingSource agreedDeductionBindingSource;

	private DataGridViewTextBoxColumn fullNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn hesabTitleDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dateCreatedShDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn PersonName;

	private DataGridViewTextBoxColumn HesabTitle;

	private DataGridViewTextBoxColumn amountPayableDG;

	private DataGridViewTextBoxColumn monthlyInstallmentDG;

	private DataGridViewTextBoxColumn ReminedDG;

	public AgreedDeduction SelectedAgreedDeduction { get; private set; }

	public FrmAgreedDeductionList(string contextGuid, Font font, bool isSelect)
		: base(contextGuid)
	{
		try
		{
			_font = font;
			InitializeComponent();
			((Control)DGrid).Focus();
			((Control)base.BtnSelect).Visible = isSelect;
			((Control)DGrid).Font = font;
			if (isSelect)
			{
				Frm_Text = "[انتخاب کسورات توافقی]";
			}
			else
			{
				Frm_Text = "[کسورات توافقی]";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreedDeductionList.cs", 44);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Users.UsersMain)
			{
				((Form)this).Close();
				return;
			}
			if ((int)((Form)this).WindowState == 2)
			{
				((DataGridViewColumn)amountPayableDG).MinimumWidth = 125;
				((DataGridViewColumn)monthlyInstallmentDG).MinimumWidth = 120;
				((DataGridViewColumn)ReminedDG).MinimumWidth = 120;
			}
			base.objDGrid = (Control)(object)DGrid;
			((FrmExtList)this).OnLoad(e);
			((FrmExtList)this).Add_Click += new CRMEventHandler(BtnAdd_Click);
			((FrmExtList)this).Edit_Click += new CRMEventHandler(BtnEdit_Click);
			((FrmExtList)this).Delete_Click += new CRMEventHandler(BtnDelete_Click);
			((Control)base.BtnOther).Visible = true;
			((Control)base.BtnOther).Text = trDetails;
			base.BtnOther.Image = (Image)(object)Resources.Detail1;
			base.BtnOther.ImagePosition = (eImagePosition)1;
			((FrmExtList)this).Other_Click += new CRMEventHandler(FrmAgreedDeductionList_Other_Click);
			((Control)DGrid).Focus();
			((Control)base.BtnAdd).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.AgreedDeduction.AgreedDeductionInsert;
			((Control)base.BtnDelete).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.AgreedDeduction.AgreedDeductionDelete;
			((Control)base.BtnEdit).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.AgreedDeduction.AgreedDeductionUpdate;
			((FrmExtList)this).Search();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreedDeductionList.cs", 85);
		}
	}

	protected override void Search()
	{
		try
		{
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			SearchAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreedDeductionList.cs", 98);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			if (_dGridLists == null || _dGridLists.Count <= 0)
			{
				_dGridLists = await AgreedDeduction.GetAllAsync(((CacheBaseForm)this).ContextGuid, "", ConstantValues.MinDateM, DateTime.MaxValue);
			}
			foreach (AgreedDeduction agreedDeduction in _dGridLists)
			{
				AgreedDeduction agreedDeduction2 = agreedDeduction;
				agreedDeduction2.PersonName = await agreedDeduction.GetFullNameAsync();
				agreedDeduction2 = agreedDeduction;
				agreedDeduction2.HesabTitle = await agreedDeduction.GetHesabTitleAsync();
			}
			string text = Text.FixString(((Control)base.TxtSearch).Text).Trim();
			if (text == "")
			{
				agreedDeductionBindingSource.DataSource = Utilities.ToSortableBindingList<AgreedDeduction>((IEnumerable<AgreedDeduction>)_dGridLists, default(CancellationToken));
				((Control)LblRowCount).Text = trCountRecords + " : " + _dGridLists.Count.ToString("N0");
				return;
			}
			string[] array = text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			_dGridListsForDisplay = new List<AgreedDeduction>();
			string[] array2 = array;
			foreach (string value in array2)
			{
				foreach (AgreedDeduction dGridList in _dGridLists)
				{
					if (dGridList.PersonName.Contains(value) || dGridList.HesabTitle.Contains(value))
					{
						_dGridListsForDisplay.Add(dGridList);
					}
				}
			}
			agreedDeductionBindingSource.DataSource = Utilities.ToSortableBindingList<AgreedDeduction>((IEnumerable<AgreedDeduction>)_dGridListsForDisplay, default(CancellationToken));
			((Control)LblRowCount).Text = trCountRecords + " : " + _dGridListsForDisplay.Count.ToString("N0");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void FrmAgreedDeductionList_Other_Click(object sender, EventArgs e)
	{
		try
		{
			if (SelectedAgreedDeduction != null)
			{
				FrmAgreDedDet frm = new FrmAgreDedDet(((CacheBaseForm)this).ContextGuid, SelectedAgreedDeduction);
				await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmAgreedDeductionList_Other_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreedDeductionList.cs", 149);
		}
	}

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)DGrid).Focus();
			FrmAgreedDeduction frm = new FrmAgreedDeduction(((CacheBaseForm)this).ContextGuid, _font, new AgreedDeduction(((CacheBaseForm)this).ContextGuid));
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_dGridLists = await AgreedDeduction.GetAllAsync(((CacheBaseForm)this).ContextGuid, "", ConstantValues.MinDateM, DateTime.MaxValue);
				((FrmExtList)this).Search();
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreedDeductionList.cs", 165);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)DGrid).Focus();
			FrmAgreedDeduction frm = new FrmAgreedDeduction(((CacheBaseForm)this).ContextGuid, _font, SelectedAgreedDeduction);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_dGridLists = await AgreedDeduction.GetAllAsync(((CacheBaseForm)this).ContextGuid, "", ConstantValues.MinDateM, DateTime.MaxValue);
				((FrmExtList)this).Search();
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreedDeductionList.cs", 181);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (((Control)base.BtnDelete).Visible && ((Control)base.BtnDelete).Enabled)
			{
				((Control)DGrid).Focus();
				if ((int)FMessageBox.Show(Utilities.GetTranslate("[آیا مطمئن هستید که حذف شود؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
				{
					await SelectedAgreedDeduction.DeleteAsync();
					_dGridLists = await AgreedDeduction.GetAllAsync(((CacheBaseForm)this).ContextGuid, "", ConstantValues.MinDateM, DateTime.MaxValue);
					((FrmExtList)this).Search();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreedDeductionList.cs", 198);
		}
	}

	private void FrmAgreedDeductionList_Load(object sender, EventArgs e)
	{
		try
		{
			Frm_Text = Utilities.GetTranslate(Frm_Text);
			((Control)this).Text = Frm_Text;
			trDetails = Utilities.GetTranslate(trDetails);
			trCountRecords = Utilities.GetTranslate(trCountRecords);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmAgreedDeductionList_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreedDeductionList.cs", 211);
		}
	}

	private void agreedDeductionBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		SelectedAgreedDeduction = (AgreedDeduction)agreedDeductionBindingSource.Current;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((FrmExtList)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		DataGridViewCellStyle val5 = new DataGridViewCellStyle();
		DGrid = new DataGridViewX();
		agreedDeductionBindingSource = new BindingSource(components);
		LblRowCount = new LabelX();
		DGID = new DataGridViewTextBoxColumn();
		PersonName = new DataGridViewTextBoxColumn();
		HesabTitle = new DataGridViewTextBoxColumn();
		amountPayableDG = new DataGridViewTextBoxColumn();
		monthlyInstallmentDG = new DataGridViewTextBoxColumn();
		ReminedDG = new DataGridViewTextBoxColumn();
		((Control)base.PanelEx1).SuspendLayout();
		((Control)base.GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)agreedDeductionBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)base.PanelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)base.PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)base.PanelEx1).Size = new Size(784, 561);
		base.PanelEx1.Style.Alignment = (StringAlignment)1;
		base.PanelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		base.PanelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		base.PanelEx1.Style.Border = (eBorderType)1;
		base.PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		base.PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		base.PanelEx1.Style.GradientAngle = 90;
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.GroupPanel1, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnEdit, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnAdd, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnDelete, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnOther, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnSelect, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)DGrid, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)LblRowCount, 0);
		((PanelControl)base.GroupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)base.GroupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)base.GroupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)base.GroupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)base.GroupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)base.GroupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)base.GroupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)base.GroupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)base.GroupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)base.GroupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)base.GroupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)base.GroupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)base.GroupPanel1).Style.CornerDiameter = 4;
		((PanelControl)base.GroupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)base.GroupPanel1).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)base.GroupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)base.GroupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)base.GroupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)base.GroupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)base.LabelX1).Location = new Point(676, 3);
		base.TxtSearch.Border.Class = "TextBoxBorder";
		base.TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)base.TxtSearch).Size = new Size(664, 21);
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[6]
		{
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)PersonName,
			(DataGridViewColumn)HesabTitle,
			(DataGridViewColumn)amountPayableDG,
			(DataGridViewColumn)monthlyInstallmentDG,
			(DataGridViewColumn)ReminedDG
		});
		((DataGridView)DGrid).DataSource = agreedDeductionBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(156, 77);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(625, 459);
		((Control)DGrid).TabIndex = 19;
		agreedDeductionBindingSource.DataSource = typeof(AgreedDeduction);
		agreedDeductionBindingSource.CurrentChanged += agreedDeductionBindingSource_CurrentChanged;
		((Control)LblRowCount).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Location = new Point(13, 512);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(127, 24);
		((Control)LblRowCount).TabIndex = 22;
		((DataGridViewColumn)DGID).DataPropertyName = "Id";
		((DataGridViewColumn)DGID).HeaderText = "[شناسه]";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)PersonName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)PersonName).DataPropertyName = "PersonName";
		((DataGridViewColumn)PersonName).HeaderText = "نام و نام خانوادگی";
		((DataGridViewColumn)PersonName).Name = "PersonName";
		((DataGridViewBand)PersonName).ReadOnly = true;
		((DataGridViewColumn)HesabTitle).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)HesabTitle).DataPropertyName = "HesabTitle";
		((DataGridViewColumn)HesabTitle).HeaderText = "عنوان حساب";
		((DataGridViewColumn)HesabTitle).Name = "HesabTitle";
		((DataGridViewBand)HesabTitle).ReadOnly = true;
		((DataGridViewColumn)amountPayableDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)amountPayableDG).DataPropertyName = "AmountPayable";
		val3.Format = "N0";
		val3.NullValue = null;
		((DataGridViewBand)amountPayableDG).DefaultCellStyle = val3;
		((DataGridViewColumn)amountPayableDG).HeaderText = "[مبلغ قابل پرداخت]";
		((DataGridViewColumn)amountPayableDG).MinimumWidth = 50;
		((DataGridViewColumn)amountPayableDG).Name = "amountPayableDG";
		((DataGridViewBand)amountPayableDG).ReadOnly = true;
		((DataGridViewColumn)amountPayableDG).Width = 121;
		((DataGridViewColumn)monthlyInstallmentDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)monthlyInstallmentDG).DataPropertyName = "MonthlyInstallment";
		val4.Format = "N0";
		val4.NullValue = null;
		((DataGridViewBand)monthlyInstallmentDG).DefaultCellStyle = val4;
		((DataGridViewColumn)monthlyInstallmentDG).HeaderText = "[قسط ماهیانه]";
		((DataGridViewColumn)monthlyInstallmentDG).MinimumWidth = 50;
		((DataGridViewColumn)monthlyInstallmentDG).Name = "monthlyInstallmentDG";
		((DataGridViewBand)monthlyInstallmentDG).ReadOnly = true;
		((DataGridViewColumn)monthlyInstallmentDG).Width = 103;
		((DataGridViewColumn)ReminedDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)ReminedDG).DataPropertyName = "Remined";
		val5.Format = "N0";
		val5.NullValue = null;
		((DataGridViewBand)ReminedDG).DefaultCellStyle = val5;
		((DataGridViewColumn)ReminedDG).HeaderText = "[مانده حساب]";
		((DataGridViewColumn)ReminedDG).MinimumWidth = 50;
		((DataGridViewColumn)ReminedDG).Name = "ReminedDG";
		((DataGridViewBand)ReminedDG).ReadOnly = true;
		((DataGridViewColumn)ReminedDG).Width = 98;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Name = "FrmAgreedDeductionList";
		((Control)this).Text = "[کسورات توافقی]";
		((Form)this).Load += FrmAgreedDeductionList_Load;
		((Control)this).Controls.SetChildIndex((Control)(object)base.PanelEx1, 0);
		((Control)base.PanelEx1).ResumeLayout(false);
		((Control)base.PanelEx1).PerformLayout();
		((Control)base.GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)agreedDeductionBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
