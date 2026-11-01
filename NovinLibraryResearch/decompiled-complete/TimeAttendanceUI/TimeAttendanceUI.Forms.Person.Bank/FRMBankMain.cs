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
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Person.Bank;

public class FRMBankMain : BaseForm
{
	private TimeAttendanceDAL.Dal.FLRGharardad.Bank cls;

	private string frm_Text = "[لیست بانک ها]";

	private Font _font;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	private PanelEx panelEx2;

	internal ButtonX BtnDelete;

	internal ButtonX BtnAdd;

	internal ButtonX BtnEdit;

	internal DataGridViewX DGrid;

	private BindingSource bankBindingSource;

	internal ButtonX buttonX1;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn DGCode;

	private DataGridViewTextBoxColumn DGName;

	private DataGridViewTextBoxColumn DGShobe;

	private DataGridViewTextBoxColumn DGDesc;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((BaseForm)this).IsDisposed;
			}
			return true;
		}
	}

	public TimeAttendanceDAL.Dal.FLRGharardad.Bank SelectedItem { get; private set; }

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
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FRMBankMain(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_font = font;
			InitializeComponent();
			((Control)DGrid).Font = _font;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Bank\\FRMBankMain.cs");
			((Control)this).Text = "لیست بانک ها";
			frm_Text = "[لیست بانک ها]";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Bank\\FRMBankMain.cs", 67);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			((Control)DGrid).Focus();
			BindingSource val = bankBindingSource;
			val.DataSource = await TimeAttendanceDAL.Dal.FLRGharardad.Bank.SelectAllAsync(((CacheBaseForm)this).ContextGuid, "", token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Bank\\FRMBankMain.cs", 77);
		}
	}

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (((Control)BtnAdd).Visible && ((Control)BtnAdd).Enabled)
			{
				((Control)DGrid).Focus();
				if ((int)(await ((CacheBaseForm)new FRMBank(((CacheBaseForm)this).ContextGuid)).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					_loadDataToken?.Cancel();
					_loadDataToken = new CancellationTokenSource();
					CancellationToken token = _loadDataToken.Token;
					await LoadDataAsync(token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Bank\\FRMBankMain.cs", 99);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnEdit).Visible || !((Control)BtnEdit).Enabled)
		{
			return;
		}
		((Control)DGrid).Focus();
		try
		{
			if (SelectedItem != null && (int)(await ((CacheBaseForm)new FRMBank(((CacheBaseForm)this).ContextGuid, SelectedItem)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				await LoadDataAsync(token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Bank\\FRMBankMain.cs", 121);
		}
	}

	private async void FRMBankMain_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 45)
			{
				if ((int)keyCode != 13)
				{
					if ((int)keyCode != 27)
					{
						if ((int)keyCode == 45)
						{
							e.Handled = true;
							((PopupItemControl)BtnAdd).PerformClick();
						}
					}
					else
					{
						e.Handled = true;
						((Form)this).DialogResult = (DialogResult)2;
						((Form)this).Close();
					}
				}
				else
				{
					e.Handled = true;
					((Form)this).DialogResult = (DialogResult)1;
					((Form)this).Close();
				}
			}
			else if ((int)keyCode != 46)
			{
				if ((int)keyCode != 118)
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
					((PopupItemControl)BtnEdit).PerformClick();
				}
			}
			else
			{
				e.Handled = true;
				((PopupItemControl)BtnDelete).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMBankMain_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Bank\\FRMBankMain.cs", 163);
		}
	}

	private void DGrid_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_DoubleClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Bank\\FRMBankMain.cs", 176);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnDelete).Visible || !((Control)BtnDelete).Enabled)
		{
			return;
		}
		((Control)DGrid).Focus();
		try
		{
			if (SelectedItem != null && (int)FMessageBox.Show(Utilities.GetTranslate("[برای حذف کردن مطمئن هستید؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) != 7)
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				cls = await TimeAttendanceDAL.Dal.FLRGharardad.Bank.GetAsync(((CacheBaseForm)this).ContextGuid, SelectedItem.ID, token);
				cls.ST = false;
				await cls.SaveAsync(token);
				await LoadDataAsync(token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Bank\\FRMBankMain.cs", 199);
		}
	}

	private void FRMBankMain_Load(object sender, EventArgs e)
	{
		try
		{
			frm_Text = Utilities.GetTranslate(frm_Text);
			((Control)this).Text = frm_Text;
			((Control)DGrid).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMBankMain_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Bank\\FRMBankMain.cs", 212);
		}
	}

	private void bankBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		SelectedItem = (TimeAttendanceDAL.Dal.FLRGharardad.Bank)bankBindingSource.Current;
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Expected O, but got Unknown
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Expected O, but got Unknown
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_0863: Expected O, but got Unknown
		//IL_0963: Unknown result type (might be due to invalid IL or missing references)
		//IL_096d: Expected O, but got Unknown
		//IL_09f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		panelEx2 = new PanelEx();
		buttonX1 = new ButtonX();
		DGrid = new DataGridViewX();
		DGID = new DataGridViewTextBoxColumn();
		DGCode = new DataGridViewTextBoxColumn();
		DGName = new DataGridViewTextBoxColumn();
		DGShobe = new DataGridViewTextBoxColumn();
		DGDesc = new DataGridViewTextBoxColumn();
		bankBindingSource = new BindingSource(components);
		BtnDelete = new ButtonX();
		BtnAdd = new ButtonX();
		BtnEdit = new ButtonX();
		((Control)panelEx2).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)bankBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		panelEx2.CanvasColor = SystemColors.Control;
		panelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx2).Controls.Add((Control)(object)buttonX1);
		((Control)panelEx2).Controls.Add((Control)(object)DGrid);
		((Control)panelEx2).Controls.Add((Control)(object)BtnDelete);
		((Control)panelEx2).Controls.Add((Control)(object)BtnAdd);
		((Control)panelEx2).Controls.Add((Control)(object)BtnEdit);
		panelEx2.DisabledBackColor = Color.Empty;
		((Control)panelEx2).Dock = (DockStyle)5;
		((Control)panelEx2).Location = new Point(0, 0);
		((Control)panelEx2).Name = "panelEx2";
		((Control)panelEx2).Size = new Size(534, 311);
		panelEx2.Style.Alignment = (StringAlignment)1;
		panelEx2.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx2.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx2.Style.Border = (eBorderType)1;
		panelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx2.Style.GradientAngle = 90;
		((Control)panelEx2).TabIndex = 0;
		((Control)buttonX1).AccessibleRole = (AccessibleRole)43;
		buttonX1.ColorTable = (eButtonColor)6;
		((Control)buttonX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		buttonX1.Image = (Image)(object)Resources.check;
		buttonX1.ImageFixedSize = new Size(30, 30);
		buttonX1.ImagePosition = (eImagePosition)2;
		((Control)buttonX1).Location = new Point(14, 11);
		((Control)buttonX1).Name = "buttonX1";
		((Control)buttonX1).Size = new Size(117, 58);
		((PopupItemControl)buttonX1).Style = (eDotNetBarStyle)9;
		((Control)buttonX1).TabIndex = 4;
		((Control)buttonX1).TabStop = false;
		((Control)buttonX1).Text = "[انتخاب]  (Enter)";
		((Control)buttonX1).Click += DGrid_DoubleClick;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[5]
		{
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)DGCode,
			(DataGridViewColumn)DGName,
			(DataGridViewColumn)DGShobe,
			(DataGridViewColumn)DGDesc
		});
		((DataGridView)DGrid).DataSource = bankBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(137, 11);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(383, 288);
		((Control)DGrid).TabIndex = 0;
		((Control)DGrid).DoubleClick += DGrid_DoubleClick;
		((DataGridViewColumn)DGID).DataPropertyName = "ID";
		((DataGridViewColumn)DGID).HeaderText = "ID";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)DGCode).DataPropertyName = "Code";
		((DataGridViewColumn)DGCode).HeaderText = "کد بانک";
		((DataGridViewColumn)DGCode).Name = "DGCode";
		((DataGridViewBand)DGCode).ReadOnly = true;
		((DataGridViewColumn)DGName).DataPropertyName = "Name";
		((DataGridViewColumn)DGName).HeaderText = "نام بانک";
		((DataGridViewColumn)DGName).Name = "DGName";
		((DataGridViewBand)DGName).ReadOnly = true;
		((DataGridViewColumn)DGShobe).DataPropertyName = "Shobe";
		((DataGridViewColumn)DGShobe).HeaderText = "نام شعبه";
		((DataGridViewColumn)DGShobe).Name = "DGShobe";
		((DataGridViewBand)DGShobe).ReadOnly = true;
		((DataGridViewColumn)DGDesc).DataPropertyName = "Desc";
		((DataGridViewColumn)DGDesc).HeaderText = "توضیحات";
		((DataGridViewColumn)DGDesc).Name = "DGDesc";
		((DataGridViewBand)DGDesc).ReadOnly = true;
		bankBindingSource.DataSource = typeof(TimeAttendanceDAL.Dal.FLRGharardad.Bank);
		bankBindingSource.CurrentChanged += bankBindingSource_CurrentChanged;
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		BtnDelete.ColorTable = (eButtonColor)6;
		((Control)BtnDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnDelete.Image = (Image)(object)Resources.edit_delete;
		BtnDelete.ImagePosition = (eImagePosition)1;
		((Control)BtnDelete).Location = new Point(14, 153);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).Size = new Size(117, 33);
		((PopupItemControl)BtnDelete).Style = (eDotNetBarStyle)9;
		((Control)BtnDelete).TabIndex = 3;
		((Control)BtnDelete).TabStop = false;
		((Control)BtnDelete).Text = "[حذف] (Delete)    ";
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)BtnAdd).AccessibleRole = (AccessibleRole)43;
		BtnAdd.ColorTable = (eButtonColor)6;
		((Control)BtnAdd).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnAdd.Image = (Image)(object)Resources.edit_add1;
		BtnAdd.ImagePosition = (eImagePosition)1;
		((Control)BtnAdd).Location = new Point(14, 75);
		((Control)BtnAdd).Name = "BtnAdd";
		((Control)BtnAdd).Size = new Size(117, 33);
		((PopupItemControl)BtnAdd).Style = (eDotNetBarStyle)9;
		((Control)BtnAdd).TabIndex = 1;
		((Control)BtnAdd).TabStop = false;
		((Control)BtnAdd).Text = "[اضافه] (Insert)";
		((Control)BtnAdd).Click += BtnAdd_Click;
		((Control)BtnEdit).AccessibleRole = (AccessibleRole)43;
		BtnEdit.ColorTable = (eButtonColor)6;
		((Control)BtnEdit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnEdit.Image = (Image)(object)Resources.edit2;
		BtnEdit.ImagePosition = (eImagePosition)1;
		((Control)BtnEdit).Location = new Point(14, 114);
		((Control)BtnEdit).Name = "BtnEdit";
		((Control)BtnEdit).RightToLeft = (RightToLeft)1;
		((Control)BtnEdit).Size = new Size(117, 33);
		((PopupItemControl)BtnEdit).Style = (eDotNetBarStyle)9;
		((Control)BtnEdit).TabIndex = 2;
		((Control)BtnEdit).TabStop = false;
		((Control)BtnEdit).Text = "    [ویرایش](F7)";
		((Control)BtnEdit).Click += BtnEdit_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(534, 311);
		((Control)this).Controls.Add((Control)(object)panelEx2);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(550, 350);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(550, 350);
		((Control)this).Name = "FRMBankMain";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[لیست بانک ها]";
		((Form)this).Load += FRMBankMain_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMBankMain_KeyDown);
		((Control)panelEx2).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)bankBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
