using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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
using TimeAttendance.ValidatorAttribute.Salary.FLRShift;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.Shift;

public class FrmShift : BaseForm
{
	private readonly Shifts _clsShift;

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private Font _font;

	private bool _isDisposed;

	private CancellationTokenSource _loadDataToken;

	private IContainer components;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private GroupPanel groupPanel7;

	internal TextBoxX TXTDes;

	internal LabelX labelX7;

	private TextBoxX TxtName;

	private LabelX labelX1;

	private LabelX labelX5;

	private ButtonX BtnDelDetail;

	private ButtonX BtnEditDetail;

	private ButtonX BtnAddDeTail;

	internal DataGridViewX DGrid;

	private PanelEx panelEx1;

	internal PictureBox PictureBox1;

	private NumericUpDown numCodeShift;

	private CheckBoxX ChbSTTatili;

	private BindingSource shiftDetBindingSource;

	private ButtonX BtnSplit;

	private DataGridViewCheckBoxColumn DGCheck;

	private DataGridViewTextBoxColumn DGDescription;

	private DataGridViewTextBoxColumn DGIDShiftCodeing;

	private DataGridViewTextBoxColumn DGAzTar;

	private DataGridViewTextBoxColumn DGTaTar;

	private DataGridViewTextBoxColumn StartTime;

	private DataGridViewTextBoxColumn FinishTime;

	private DataGridViewCheckBoxColumn DGSat;

	private DataGridViewCheckBoxColumn DGSun;

	private DataGridViewCheckBoxColumn DGMon;

	private DataGridViewCheckBoxColumn DGTue;

	private DataGridViewCheckBoxColumn DGWed;

	private DataGridViewCheckBoxColumn DGThr;

	private DataGridViewCheckBoxColumn DGFri;

	private DataGridViewTextBoxColumn shiftIdDataGridViewTextBoxColumn;

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

	public ShiftDet SelectedShiftDet { get; private set; }

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
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void SetTrDictionary()
	{
		try
		{
			if (trDictionary == null)
			{
				trDictionary = new Dictionary<string, string>();
			}
			if (!trDictionary.TryGetValue("[افزودن شیفت جدید]", out var value))
			{
				trDictionary.Add("[افزودن شیفت جدید]", Utilities.GetTranslate("[افزودن شیفت جدید]"));
			}
			if (!trDictionary.TryGetValue("[شیفت]", out value))
			{
				trDictionary.Add("[شیفت]", Utilities.GetTranslate("[شیفت]"));
			}
			if (!trDictionary.TryGetValue("[ویرایش شیفت]", out value))
			{
				trDictionary.Add("[ویرایش شیفت]", Utilities.GetTranslate("[ویرایش شیفت]"));
			}
			if (!trDictionary.TryGetValue("[مشاهده لاگ عملکرد کاربران ( فقط خواندنی ) اطلاعات شیفت]", out value))
			{
				trDictionary.Add("[مشاهده لاگ عملکرد کاربران ( فقط خواندنی ) اطلاعات شیفت]", Utilities.GetTranslate("[مشاهده لاگ عملکرد کاربران ( فقط خواندنی ) اطلاعات شیفت]"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionary", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShift.cs", 58);
		}
	}

	public FrmShift(string contextGuid, Shifts shift, Font font, bool isReadOnly)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		if (shift == null)
		{
			throw new NullReferenceException("please fill shift before call this form");
		}
		try
		{
			_font = font;
			Task.Run((Action)SetTrDictionary);
			InitializeComponent();
			((Control)DGrid).Font = font;
			_clsShift = shift;
			((Control)numCodeShift).Focus();
			((Control)numCodeShift).Text = _clsShift.Code.ToString();
			((Control)TXTDes).Text = shift.Description;
			((Control)TxtName).Text = shift.Name;
			if (shift.Id <= 0)
			{
				((Control)TxtName).Text = "شیفت" + ((Control)numCodeShift).Text;
				if (trDictionary.TryGetValue("[شیفت]", out var value))
				{
					((Control)TxtName).Text = value + ((Control)numCodeShift).Text;
				}
				((Control)this).Text = "افزودن شیفت جدید";
				if (trDictionary.TryGetValue("[افزودن شیفت جدید]", out var value2))
				{
					((Control)this).Text = value2;
				}
			}
			else if (isReadOnly)
			{
				((Control)BtnAccept).Enabled = false;
				FillForm();
				((Control)this).Text = "مشاهده لاگ عملکرد کاربران ( فقط خواندنی ) اطلاعات شیفت";
				if (trDictionary.TryGetValue("[مشاهده لاگ عملکرد کاربران ( فقط خواندنی ) اطلاعات شیفت]", out var value3))
				{
					((Control)this).Text = value3;
				}
			}
			else
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				new Waiter("بارگذاری داده", (Form)(object)this, LoadListShiftDetAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShift.cs");
				((Control)this).Text = "ویرایش شیفت";
				if (trDictionary.TryGetValue("[ویرایش شیفت]", out var value4))
				{
					((Control)this).Text = value4;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShift.cs", 110);
		}
	}

	private async Task LoadListShiftDetAsync(CancellationToken token)
	{
		try
		{
			Shifts clsShift = _clsShift;
			clsShift.LstShiftDet = await ShiftDet.GetAllAsync(((CacheBaseForm)this).ContextGuid, _clsShift.Id, token);
			FillForm();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void FillForm()
	{
		try
		{
			((Control)TxtName).Text = _clsShift.Name;
			((Control)numCodeShift).Text = _clsShift.Code;
			((Control)TXTDes).Text = _clsShift.Description;
			ChbSTTatili.Checked = _clsShift.StTatili;
			shiftDetBindingSource.DataSource = Utilities.ToSortableBindingList<ShiftDet>((IEnumerable<ShiftDet>)_clsShift.LstShiftDet, default(CancellationToken));
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShift.cs", 133);
		}
	}

	private void GetData()
	{
		try
		{
			_clsShift.Name = Text.FixString(((Control)TxtName).Text.Trim());
			_clsShift.Code = Text.FixString(((Control)numCodeShift).Text.Trim());
			_clsShift.Description = Text.FixString(((Control)TXTDes).Text.Trim());
			_clsShift.StTatili = ChbSTTatili.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShift.cs", 147);
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			if (!((Control)BtnAccept).Enabled || !((Control)BtnAccept).Visible)
			{
				return;
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			GetData();
			DialogResult val = await ShiftsCheckValidation.CheckValidationAsync(((CacheBaseForm)this).ContextGuid, _clsShift, isShowMessage: true, token);
			if ((int)val != 2)
			{
				if (val - 5 <= 1)
				{
					await ShiftDet.DeleteAsync(((CacheBaseForm)this).ContextGuid, _clsShift.Id, token);
					((Form)this).DialogResult = (DialogResult)((await _clsShift.SaveAsync(token)) ? 1 : 2);
					((Form)this).Close();
				}
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShift.cs", 176);
		}
	}

	private void FRMShift_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
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
							((PopupItemControl)BtnAddDeTail).PerformClick();
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
					((Control)this).SelectNextControl(((ContainerControl)this).ActiveControl, true, true, true, true);
				}
			}
			else if ((int)keyCode <= 116)
			{
				if ((int)keyCode != 46)
				{
					if ((int)keyCode == 116)
					{
						e.Handled = true;
						((PopupItemControl)BtnAccept).PerformClick();
					}
				}
				else if (((Control)DGrid).Focused)
				{
					e.Handled = true;
					((PopupItemControl)BtnDelDetail).PerformClick();
				}
			}
			else if ((int)keyCode != 118)
			{
				if ((int)keyCode == 123)
				{
				}
			}
			else
			{
				e.Handled = true;
				((PopupItemControl)BtnEditDetail).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMShift_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShift.cs", 221);
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShift.cs", 234);
		}
	}

	private void DGrid_DoubleClick(object sender, EventArgs e)
	{
		((PopupItemControl)BtnEditDetail).PerformClick();
	}

	private void DGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				((PopupItemControl)BtnEditDetail).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShift.cs", 251);
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == ((DataGridViewBand)DGCheck).Index)
			{
				DataGridViewCell currentCell = ((DataGridView)DGrid).CurrentCell;
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((currentCell is DataGridViewCheckBoxCell) ? currentCell : null);
				if (val != null && Convert.ToBoolean(((DataGridViewCell)val).Value))
				{
					((DataGridViewCell)val).Value = false;
				}
				else if (val != null)
				{
					((DataGridViewCell)val).Value = true;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShift.cs", 268);
		}
	}

	private void BtnAddDetail_Click(object sender, EventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		try
		{
			FrmShiftDet frm = new FrmShiftDet(((CacheBaseForm)this).ContextGuid, _font);
			if ((int)((Form)frm).ShowDialog() == 1)
			{
				if ((from p in _clsShift.LstShiftDet
					where p.DateStartM == frm.ClsShiftDet.DateStartM
					where p.ShiftCodingId == frm.ClsShiftDet.ShiftCodingId
					select p).ToList().Count > 0)
				{
					Utility.MessageBoxShowError("[تاریخ شروع و نوع تردد دو تا شیفت با یکدیگر نمیتواند برابر باشد]", ((CacheBaseForm)this).ContextGuid);
					return;
				}
				_clsShift.LstShiftDet.Add(frm.ClsShiftDet);
				shiftDetBindingSource.DataSource = Utilities.ToBindingList<ShiftDet>((IEnumerable<ShiftDet>)_clsShift.LstShiftDet, default(CancellationToken));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAddDetail_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShift.cs", 294);
		}
	}

	private void BtnEditDetail_Click(object sender, EventArgs e)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Invalid comparison between Unknown and I4
		try
		{
			if (((Control)BtnEditDetail).Visible && ((Control)BtnEditDetail).Enabled && ((DataGridView)DGrid).CurrentRow != null && SelectedShiftDet != null)
			{
				FrmShiftDet frmShiftDet = new FrmShiftDet(((CacheBaseForm)this).ContextGuid, _font, SelectedShiftDet);
				if ((int)((Form)frmShiftDet).ShowDialog() == 1)
				{
					_clsShift.LstShiftDet.Remove(SelectedShiftDet);
					_clsShift.LstShiftDet.Add(frmShiftDet.ClsShiftDet);
					shiftDetBindingSource.DataSource = Utilities.ToBindingList<ShiftDet>((IEnumerable<ShiftDet>)_clsShift.LstShiftDet, default(CancellationToken));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEditDetail_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShift.cs", 309);
		}
	}

	private async void BtnDelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (!((Control)BtnDelDetail).Visible || !((Control)BtnDelDetail).Enabled || ((DataGridView)DGrid).RowCount <= 0)
			{
				return;
			}
			List<ShiftDet> list = new List<ShiftDet>();
			for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
			{
				if (Convert.ToBoolean(((DataGridViewCell)(DataGridViewCheckBoxCell)((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i]).Value))
				{
					list.Add(_clsShift.LstShiftDet[i]);
				}
			}
			if (list.Count <= 0)
			{
				FMessageBox.Show("هیچ نوع ترددی انتخاب نشده است", "حذف نوع تردد");
			}
			else
			{
				if ((int)FMessageBox.Show(Utilities.GetTranslate("[مایل به حذف شیفت (های) انتخاب شده هستید ؟]"), "", FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) != 6)
				{
					return;
				}
				foreach (ShiftDet item in list)
				{
					_clsShift.LstShiftDet.Remove(item);
				}
				shiftDetBindingSource.DataSource = Utilities.ToBindingList<ShiftDet>((IEnumerable<ShiftDet>)_clsShift.LstShiftDet, default(CancellationToken));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelDetail_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShift.cs", 337);
		}
	}

	private void btnSplit_Click(object sender, EventArgs e)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		try
		{
			if (SelectedShiftDet != null && ((Control)BtnEditDetail).Visible && ((Control)BtnEditDetail).Enabled)
			{
				FrmShiftSplit frmShiftSplit = new FrmShiftSplit(((CacheBaseForm)this).ContextGuid, SelectedShiftDet);
				if ((int)((Form)frmShiftSplit).ShowDialog() == 1)
				{
					_clsShift.LstShiftDet.Remove(SelectedShiftDet);
					_clsShift.LstShiftDet.Add(frmShiftSplit.ClsShiftDet1);
					_clsShift.LstShiftDet.Add(frmShiftSplit.ClsShiftDet2);
					_clsShift.LstShiftDet.Add(frmShiftSplit.ClsShiftDet3);
					shiftDetBindingSource.DataSource = Utilities.ToBindingList<ShiftDet>((IEnumerable<ShiftDet>)_clsShift.LstShiftDet, default(CancellationToken));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnSplit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShift.cs", 357);
		}
	}

	private void shiftDetBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		SelectedShiftDet = (ShiftDet)shiftDetBindingSource.Current;
	}

	private void InitializeComponent()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Expected O, but got Unknown
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Expected O, but got Unknown
		//IL_0978: Unknown result type (might be due to invalid IL or missing references)
		//IL_0982: Expected O, but got Unknown
		//IL_0a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2d: Expected O, but got Unknown
		//IL_0ace: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad8: Expected O, but got Unknown
		//IL_0c18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c22: Expected O, but got Unknown
		//IL_0f75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7f: Expected O, but got Unknown
		//IL_105c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1066: Expected O, but got Unknown
		//IL_108a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1094: Expected O, but got Unknown
		//IL_1813: Unknown result type (might be due to invalid IL or missing references)
		//IL_181d: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmShift));
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		groupPanel7 = new GroupPanel();
		BtnSplit = new ButtonX();
		ChbSTTatili = new CheckBoxX();
		numCodeShift = new NumericUpDown();
		PictureBox1 = new PictureBox();
		BtnDelDetail = new ButtonX();
		BtnEditDetail = new ButtonX();
		BtnAddDeTail = new ButtonX();
		TXTDes = new TextBoxX();
		labelX7 = new LabelX();
		TxtName = new TextBoxX();
		labelX1 = new LabelX();
		labelX5 = new LabelX();
		DGrid = new DataGridViewX();
		DGCheck = new DataGridViewCheckBoxColumn();
		DGDescription = new DataGridViewTextBoxColumn();
		DGIDShiftCodeing = new DataGridViewTextBoxColumn();
		DGAzTar = new DataGridViewTextBoxColumn();
		DGTaTar = new DataGridViewTextBoxColumn();
		StartTime = new DataGridViewTextBoxColumn();
		FinishTime = new DataGridViewTextBoxColumn();
		DGSat = new DataGridViewCheckBoxColumn();
		DGSun = new DataGridViewCheckBoxColumn();
		DGMon = new DataGridViewCheckBoxColumn();
		DGTue = new DataGridViewCheckBoxColumn();
		DGWed = new DataGridViewCheckBoxColumn();
		DGThr = new DataGridViewCheckBoxColumn();
		DGFri = new DataGridViewCheckBoxColumn();
		shiftIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		shiftDetBindingSource = new BindingSource(components);
		panelEx1 = new PanelEx();
		((Control)groupPanel7).SuspendLayout();
		((ISupportInitialize)numCodeShift).BeginInit();
		((ISupportInitialize)PictureBox1).BeginInit();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)shiftDetBindingSource).BeginInit();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(7, 516);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(86, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 2;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(98, 516);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(81, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 1;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)groupPanel7).Anchor = (AnchorStyles)15;
		((Control)groupPanel7).BackColor = Color.Transparent;
		((PanelControl)groupPanel7).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel7).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel7).Controls.Add((Control)(object)BtnSplit);
		((Control)groupPanel7).Controls.Add((Control)(object)ChbSTTatili);
		((Control)groupPanel7).Controls.Add((Control)(object)numCodeShift);
		((Control)groupPanel7).Controls.Add((Control)(object)PictureBox1);
		((Control)groupPanel7).Controls.Add((Control)(object)BtnDelDetail);
		((Control)groupPanel7).Controls.Add((Control)(object)BtnEditDetail);
		((Control)groupPanel7).Controls.Add((Control)(object)BtnAddDeTail);
		((Control)groupPanel7).Controls.Add((Control)(object)TXTDes);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX7);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtName);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX5);
		((Control)groupPanel7).Controls.Add((Control)(object)DGrid);
		groupPanel7.DisabledBackColor = Color.Empty;
		((Control)groupPanel7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)groupPanel7).Location = new Point(7, 12);
		((Control)groupPanel7).Name = "groupPanel7";
		((Control)groupPanel7).Size = new Size(857, 496);
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
		((Control)groupPanel7).TabIndex = 0;
		((Control)BtnSplit).AccessibleRole = (AccessibleRole)43;
		BtnSplit.AutoSizeMode = (AutoSizeMode)0;
		BtnSplit.ColorTable = (eButtonColor)3;
		BtnSplit.Image = (Image)componentResourceManager.GetObject("BtnSplit.Image");
		((Control)BtnSplit).Location = new Point(5, 251);
		((Control)BtnSplit).Name = "BtnSplit";
		((Control)BtnSplit).Size = new Size(45, 28);
		((PopupItemControl)BtnSplit).Style = (eDotNetBarStyle)9;
		((Control)BtnSplit).TabIndex = 12;
		((Control)BtnSplit).TabStop = false;
		BtnSplit.Tooltip = "[تکه کردن شیفت مانند جدا کردن  ماه رمضان بعنوان قسمت میانی شیفت]";
		((Control)BtnSplit).Click += btnSplit_Click;
		((Control)ChbSTTatili).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbSTTatili).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbSTTatili).Location = new Point(286, 8);
		((Control)ChbSTTatili).Name = "ChbSTTatili";
		((Control)ChbSTTatili).Size = new Size(308, 21);
		((BaseItemControl)ChbSTTatili).Style = (eDotNetBarStyle)9;
		((Control)ChbSTTatili).TabIndex = 2;
		((Control)ChbSTTatili).Text = "[( در نظر گرفتن تعطیلات رسمی بعنوان روزکاری ( بجز جمعه ها ]";
		((Control)numCodeShift).Anchor = (AnchorStyles)9;
		((Control)numCodeShift).Location = new Point(599, 8);
		numCodeShift.Maximum = new decimal(new int[4] { 10000000, 0, 0, 0 });
		numCodeShift.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)numCodeShift).Name = "numCodeShift";
		((Control)numCodeShift).Size = new Size(123, 21);
		((Control)numCodeShift).TabIndex = 1;
		((UpDownBase)numCodeShift).TextAlign = (HorizontalAlignment)2;
		numCodeShift.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)PictureBox1).Anchor = (AnchorStyles)9;
		PictureBox1.BorderStyle = (BorderStyle)1;
		PictureBox1.Image = (Image)(object)Resources.FunClock;
		((Control)PictureBox1).Location = new Point(791, 6);
		((Control)PictureBox1).Name = "PictureBox1";
		((Control)PictureBox1).Size = new Size(51, 52);
		PictureBox1.SizeMode = (PictureBoxSizeMode)4;
		PictureBox1.TabIndex = 11;
		PictureBox1.TabStop = false;
		((Control)BtnDelDetail).AccessibleRole = (AccessibleRole)43;
		BtnDelDetail.ColorTable = (eButtonColor)3;
		BtnDelDetail.Image = (Image)componentResourceManager.GetObject("BtnDelDetail.Image");
		((Control)BtnDelDetail).Location = new Point(5, 219);
		((Control)BtnDelDetail).Name = "BtnDelDetail";
		((Control)BtnDelDetail).Size = new Size(45, 28);
		((PopupItemControl)BtnDelDetail).Style = (eDotNetBarStyle)9;
		((Control)BtnDelDetail).TabIndex = 10;
		((Control)BtnDelDetail).TabStop = false;
		((Control)BtnDelDetail).Click += BtnDelDetail_Click;
		((Control)BtnEditDetail).AccessibleRole = (AccessibleRole)43;
		BtnEditDetail.ColorTable = (eButtonColor)3;
		BtnEditDetail.Image = (Image)componentResourceManager.GetObject("BtnEditDetail.Image");
		((Control)BtnEditDetail).Location = new Point(5, 187);
		((Control)BtnEditDetail).Name = "BtnEditDetail";
		((Control)BtnEditDetail).Size = new Size(45, 28);
		((PopupItemControl)BtnEditDetail).Style = (eDotNetBarStyle)9;
		((Control)BtnEditDetail).TabIndex = 9;
		((Control)BtnEditDetail).TabStop = false;
		((Control)BtnEditDetail).Click += BtnEditDetail_Click;
		((Control)BtnAddDeTail).AccessibleRole = (AccessibleRole)43;
		BtnAddDeTail.ColorTable = (eButtonColor)3;
		BtnAddDeTail.Image = (Image)componentResourceManager.GetObject("BtnAddDeTail.Image");
		((Control)BtnAddDeTail).Location = new Point(5, 155);
		((Control)BtnAddDeTail).Name = "BtnAddDeTail";
		((Control)BtnAddDeTail).Size = new Size(45, 28);
		((PopupItemControl)BtnAddDeTail).Style = (eDotNetBarStyle)9;
		((Control)BtnAddDeTail).TabIndex = 8;
		((Control)BtnAddDeTail).TabStop = false;
		((Control)BtnAddDeTail).Click += BtnAddDetail_Click;
		((Control)TXTDes).Anchor = (AnchorStyles)13;
		TXTDes.Border.Class = "TextBoxBorder";
		TXTDes.Border.CornerType = (eCornerType)1;
		((Control)TXTDes).Location = new Point(5, 64);
		((TextBoxBase)TXTDes).Multiline = true;
		((Control)TXTDes).Name = "TXTDes";
		((Control)TXTDes).Size = new Size(717, 85);
		((Control)TXTDes).TabIndex = 6;
		((Control)TXTDes).Text = "-";
		((Control)labelX7).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX7).Location = new Point(727, 64);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(51, 23);
		((Control)labelX7).TabIndex = 5;
		((Control)labelX7).Text = "[شرح] :";
		labelX7.TextAlignment = (StringAlignment)2;
		((Control)TxtName).Anchor = (AnchorStyles)9;
		TxtName.Border.Class = "TextBoxBorder";
		TxtName.Border.CornerType = (eCornerType)1;
		((Control)TxtName).Location = new Point(599, 37);
		((Control)TxtName).Name = "TxtName";
		((Control)TxtName).Size = new Size(123, 21);
		((Control)TxtName).TabIndex = 4;
		((Control)TxtName).Text = "[شیفت1]";
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(727, 35);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(61, 23);
		((Control)labelX1).TabIndex = 3;
		((Control)labelX1).Text = "[نام شیفت] * :";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(727, 6);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(59, 23);
		((Control)labelX5).TabIndex = 0;
		((Control)labelX5).Text = "[کد شیفت] * :";
		labelX5.TextAlignment = (StringAlignment)2;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[15]
		{
			(DataGridViewColumn)DGCheck,
			(DataGridViewColumn)DGDescription,
			(DataGridViewColumn)DGIDShiftCodeing,
			(DataGridViewColumn)DGAzTar,
			(DataGridViewColumn)DGTaTar,
			(DataGridViewColumn)StartTime,
			(DataGridViewColumn)FinishTime,
			(DataGridViewColumn)DGSat,
			(DataGridViewColumn)DGSun,
			(DataGridViewColumn)DGMon,
			(DataGridViewColumn)DGTue,
			(DataGridViewColumn)DGWed,
			(DataGridViewColumn)DGThr,
			(DataGridViewColumn)DGFri,
			(DataGridViewColumn)shiftIdDataGridViewTextBoxColumn
		});
		((DataGridView)DGrid).DataSource = shiftDetBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(56, 155);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(786, 332);
		((Control)DGrid).TabIndex = 7;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((Control)DGrid).DoubleClick += DGrid_DoubleClick;
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		((DataGridViewColumn)DGCheck).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCheck).HeaderText = "   ";
		((DataGridViewColumn)DGCheck).Name = "DGCheck";
		((DataGridViewBand)DGCheck).ReadOnly = true;
		((DataGridViewBand)DGCheck).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)DGCheck).Width = 20;
		((DataGridViewColumn)DGDescription).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGDescription).DataPropertyName = "Description";
		((DataGridViewColumn)DGDescription).HeaderText = "[شرح]";
		((DataGridViewColumn)DGDescription).MinimumWidth = 100;
		((DataGridViewColumn)DGDescription).Name = "DGDescription";
		((DataGridViewBand)DGDescription).ReadOnly = true;
		((DataGridViewColumn)DGIDShiftCodeing).DataPropertyName = "ShiftCodingId";
		((DataGridViewColumn)DGIDShiftCodeing).HeaderText = "DGIDShiftCodeing";
		((DataGridViewColumn)DGIDShiftCodeing).Name = "DGIDShiftCodeing";
		((DataGridViewBand)DGIDShiftCodeing).ReadOnly = true;
		((DataGridViewBand)DGIDShiftCodeing).Visible = false;
		((DataGridViewColumn)DGAzTar).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGAzTar).DataPropertyName = "DateStartCalture";
		((DataGridViewColumn)DGAzTar).HeaderText = "از تاریخ";
		((DataGridViewColumn)DGAzTar).MinimumWidth = 100;
		((DataGridViewColumn)DGAzTar).Name = "DGAzTar";
		((DataGridViewBand)DGAzTar).ReadOnly = true;
		((DataGridViewColumn)DGTaTar).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGTaTar).DataPropertyName = "DateFinishCalture";
		((DataGridViewColumn)DGTaTar).HeaderText = "تا تاریخ";
		((DataGridViewColumn)DGTaTar).MinimumWidth = 100;
		((DataGridViewColumn)DGTaTar).Name = "DGTaTar";
		((DataGridViewBand)DGTaTar).ReadOnly = true;
		((DataGridViewColumn)StartTime).DataPropertyName = "StartTime";
		((DataGridViewColumn)StartTime).HeaderText = "[ساعت شروع]";
		((DataGridViewColumn)StartTime).MinimumWidth = 50;
		((DataGridViewColumn)StartTime).Name = "StartTime";
		((DataGridViewBand)StartTime).ReadOnly = true;
		((DataGridViewColumn)StartTime).Width = 50;
		((DataGridViewColumn)FinishTime).DataPropertyName = "FinishTime";
		((DataGridViewColumn)FinishTime).HeaderText = "[ساعت پایان]";
		((DataGridViewColumn)FinishTime).MinimumWidth = 50;
		((DataGridViewColumn)FinishTime).Name = "FinishTime";
		((DataGridViewBand)FinishTime).ReadOnly = true;
		((DataGridViewColumn)FinishTime).Width = 50;
		((DataGridViewColumn)DGSat).DataPropertyName = "Sat";
		((DataGridViewColumn)DGSat).HeaderText = "[شنبه]";
		((DataGridViewColumn)DGSat).MinimumWidth = 38;
		((DataGridViewColumn)DGSat).Name = "DGSat";
		((DataGridViewBand)DGSat).ReadOnly = true;
		((DataGridViewBand)DGSat).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)DGSat).SortMode = (DataGridViewColumnSortMode)1;
		((DataGridViewColumn)DGSat).Width = 38;
		((DataGridViewColumn)DGSun).DataPropertyName = "Sun";
		((DataGridViewColumn)DGSun).HeaderText = "1 شنبه";
		((DataGridViewColumn)DGSun).MinimumWidth = 38;
		((DataGridViewColumn)DGSun).Name = "DGSun";
		((DataGridViewBand)DGSun).ReadOnly = true;
		((DataGridViewBand)DGSun).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)DGSun).SortMode = (DataGridViewColumnSortMode)1;
		((DataGridViewColumn)DGSun).Width = 38;
		((DataGridViewColumn)DGMon).DataPropertyName = "Mon";
		((DataGridViewColumn)DGMon).HeaderText = "2 شنبه";
		((DataGridViewColumn)DGMon).MinimumWidth = 38;
		((DataGridViewColumn)DGMon).Name = "DGMon";
		((DataGridViewBand)DGMon).ReadOnly = true;
		((DataGridViewBand)DGMon).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)DGMon).SortMode = (DataGridViewColumnSortMode)1;
		((DataGridViewColumn)DGMon).Width = 38;
		((DataGridViewColumn)DGTue).DataPropertyName = "Tue";
		((DataGridViewColumn)DGTue).HeaderText = "[3 شنبه]";
		((DataGridViewColumn)DGTue).MinimumWidth = 38;
		((DataGridViewColumn)DGTue).Name = "DGTue";
		((DataGridViewBand)DGTue).ReadOnly = true;
		((DataGridViewBand)DGTue).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)DGTue).SortMode = (DataGridViewColumnSortMode)1;
		((DataGridViewColumn)DGTue).Width = 38;
		((DataGridViewColumn)DGWed).DataPropertyName = "Wed";
		((DataGridViewColumn)DGWed).HeaderText = "[4 شنبه]";
		((DataGridViewColumn)DGWed).MinimumWidth = 38;
		((DataGridViewColumn)DGWed).Name = "DGWed";
		((DataGridViewBand)DGWed).ReadOnly = true;
		((DataGridViewBand)DGWed).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)DGWed).SortMode = (DataGridViewColumnSortMode)1;
		((DataGridViewColumn)DGWed).Width = 38;
		((DataGridViewColumn)DGThr).DataPropertyName = "Tur";
		((DataGridViewColumn)DGThr).HeaderText = "[5 شنبه]";
		((DataGridViewColumn)DGThr).MinimumWidth = 38;
		((DataGridViewColumn)DGThr).Name = "DGThr";
		((DataGridViewBand)DGThr).ReadOnly = true;
		((DataGridViewBand)DGThr).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)DGThr).SortMode = (DataGridViewColumnSortMode)1;
		((DataGridViewColumn)DGThr).Width = 38;
		((DataGridViewColumn)DGFri).DataPropertyName = "Fri";
		((DataGridViewColumn)DGFri).HeaderText = "[جمعه]";
		((DataGridViewColumn)DGFri).MinimumWidth = 38;
		((DataGridViewColumn)DGFri).Name = "DGFri";
		((DataGridViewBand)DGFri).ReadOnly = true;
		((DataGridViewBand)DGFri).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)DGFri).SortMode = (DataGridViewColumnSortMode)1;
		((DataGridViewColumn)DGFri).Width = 38;
		((DataGridViewColumn)shiftIdDataGridViewTextBoxColumn).DataPropertyName = "ShiftId";
		((DataGridViewColumn)shiftIdDataGridViewTextBoxColumn).HeaderText = "ShiftId";
		((DataGridViewColumn)shiftIdDataGridViewTextBoxColumn).Name = "shiftIdDataGridViewTextBoxColumn";
		((DataGridViewBand)shiftIdDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)shiftIdDataGridViewTextBoxColumn).Visible = false;
		shiftDetBindingSource.DataSource = typeof(ShiftDet);
		shiftDetBindingSource.CurrentChanged += shiftDetBindingSource_CurrentChanged;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)groupPanel7);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(870, 562);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(870, 562);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmShift";
		((Form)this).ShowIcon = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).KeyDown += new KeyEventHandler(FRMShift_KeyDown);
		((Control)groupPanel7).ResumeLayout(false);
		((ISupportInitialize)numCodeShift).EndInit();
		((ISupportInitialize)PictureBox1).EndInit();
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)shiftDetBindingSource).EndInit();
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
