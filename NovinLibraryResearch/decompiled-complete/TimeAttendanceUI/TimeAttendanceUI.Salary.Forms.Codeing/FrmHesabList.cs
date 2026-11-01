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
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.Forms.Codeing;

public class FrmHesabList : BaseForm
{
	private short _type = -1;

	private short _showBaseAccounting = -1;

	private Font _font;

	private bool _isDisposed;

	private CancellationTokenSource _searchToken;

	private CancellationTokenSource _loadDataToken;

	private List<FishHesab> _lstFishHesab = new List<FishHesab>();

	private List<FishHesab> _dGridListsForDisplay = new List<FishHesab>();

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BTNEdit;

	private ButtonX BTNDelete;

	private ButtonX BTNADD;

	private ButtonX BTNSelect;

	public DataGridViewX DGrid;

	internal GroupPanel GroupPanel1;

	internal LabelX LabelX1;

	internal TextBoxX TxtSearch;

	private PanelEx panelKol;

	private ButtonX BtnSayer;

	private ButtonX BtnKosor;

	private ButtonX BtnMazaya;

	private PanelEx panelEx3;

	private BindingSource fishHesabBindingSource;

	private LabelX LblRowCount;

	private LabelX labelX22;

	private LabelX LblDesc;

	private DataGridViewCheckBoxColumn DGCheck;

	private DataGridViewTextBoxColumn DescriptionDG;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn DGCodeMoein;

	private DataGridViewTextBoxColumn DGCodeTafsil;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn DGAccount;

	private DataGridViewCheckBoxColumn Insurance;

	private DataGridViewCheckBoxColumn Taxable;

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

	public FishHesab SelectedHesab { get; private set; }

	public string SelectedItems
	{
		get
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			string text = "";
			try
			{
				bool flag = false;
				if (((DataGridView)DGrid).RowCount == 0)
				{
					return "";
				}
				for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
				{
					if (Convert.ToBoolean(((DataGridViewCell)(DataGridViewCheckBoxCell)((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i]).Value))
					{
						flag = true;
						if (text.Trim() != "")
						{
							text += ";";
						}
						text += ((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, i].Value.ToString();
					}
				}
				if (flag)
				{
					return text;
				}
				if (((DataGridView)DGrid).CurrentRow != null)
				{
					text = ((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SelectedItems", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesabList.cs", 191);
			}
			return text;
		}
	}

	private async Task SetTrDictionaryAsync()
	{
		try
		{
			if (trDictionary == null)
			{
				trDictionary = new Dictionary<string, string>();
			}
			if (!trDictionary.TryGetValue("[لیست حساب ها]", out var value))
			{
				trDictionary.Add("[لیست حساب ها]", Utilities.GetTranslate("[لیست حساب ها]"));
			}
			if (!trDictionary.TryGetValue("[تعداد رکوردها]", out value))
			{
				trDictionary.Add("[تعداد رکوردها]", Utilities.GetTranslate("[تعداد رکوردها]"));
			}
			if (!trDictionary.TryGetValue("[لیست مزایای ثابت]", out value))
			{
				trDictionary.Add("[لیست مزایای ثابت]", Utilities.GetTranslate("[لیست مزایای ثابت]"));
			}
			if (!trDictionary.TryGetValue("[لیست کسورات ثابت]", out value))
			{
				trDictionary.Add("[لیست کسورات ثابت]", Utilities.GetTranslate("[لیست کسورات ثابت]"));
			}
			if (!trDictionary.TryGetValue("[لیست سایر حساب ها]", out value))
			{
				trDictionary.Add("[لیست سایر حساب ها]", Utilities.GetTranslate("[لیست سایر حساب ها]"));
			}
			if (!trDictionary.TryGetValue("[برای حذف کردن مطمئن هستید؟]", out value))
			{
				trDictionary.Add("[برای حذف کردن مطمئن هستید؟]", Utilities.GetTranslate("[برای حذف کردن مطمئن هستید؟]"));
			}
			if (!trDictionary.TryGetValue("[هشدار]", out value))
			{
				trDictionary.Add("[هشدار]", Utilities.GetTranslate("[هشدار]"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesabList.cs", 43);
		}
	}

	public FrmHesabList(string contextGuid, Font font, short typeForm = -1, string pstr = "")
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			_font = font;
			Task.Run((Func<Task?>)SetTrDictionaryAsync);
			if (typeForm == -1)
			{
				_type = 1;
			}
			InitializeComponent();
			((Control)panelKol).Visible = true;
			((DataGridViewBand)DGCheck).Visible = false;
			((Control)BTNSelect).Visible = false;
			if (typeForm != -1)
			{
				_type = typeForm;
				_showBaseAccounting = 1;
				((Control)panelKol).Visible = false;
				((DataGridViewBand)DGCheck).Visible = true;
				((Control)BTNSelect).Visible = true;
			}
			if (!string.IsNullOrEmpty(pstr))
			{
				LoadDataAsync(int.Parse(pstr));
				((Control)panelKol).Visible = false;
				((DataGridViewBand)DGCheck).Visible = false;
				((Control)BTNSelect).Visible = true;
			}
			else
			{
				LoadDataAsync();
			}
			((Control)TxtSearch).Focus();
			((Control)this).Text = "لیست حساب ها";
			if (trDictionary.TryGetValue("[لیست حساب ها]", out var value))
			{
				((Control)this).Text = value;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesabList.cs", 139);
		}
	}

	private async Task LoadDataAsync(int id = 0, CancellationToken token = default(CancellationToken))
	{
		try
		{
			_lstFishHesab = await FishHesab.SelectRowsByTypeAsync(((CacheBaseForm)this).ContextGuid, _type, token);
			if (_showBaseAccounting == 1)
			{
				_lstFishHesab = _lstFishHesab.Where((FishHesab q) => q.Id >= 1000).ToList();
			}
			fishHesabBindingSource.DataSource = _lstFishHesab;
			int num = _lstFishHesab.FindIndex((FishHesab q) => q.Id == id);
			((Control)DGrid).Focus();
			((Control)DGrid).Font = _font;
			((Control)LblRowCount).Text = "تعداد رکوردها : " + _lstFishHesab.Count;
			if (trDictionary.TryGetValue("[تعداد رکوردها]", out var value))
			{
				((Control)LblRowCount).Text = value + ":" + _lstFishHesab.Count;
			}
			string descOfType = GetDescOfType();
			((Control)this).Text = descOfType;
			if (num >= 0 && num < ((DataGridView)DGrid).Rows.Count)
			{
				((DataGridViewBand)((DataGridView)DGrid).Rows[num]).Selected = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesabList.cs", 161);
		}
	}

	private string GetDescOfType()
	{
		string result = "";
		try
		{
			switch (_type)
			{
			case 1:
			{
				result = "لیست مزایای ثابت";
				if (trDictionary.TryGetValue("[لیست مزایای ثابت]", out var value2))
				{
					result = value2;
				}
				break;
			}
			case 2:
			{
				result = "لیست کسورات ثابت";
				if (trDictionary.TryGetValue("[لیست کسورات ثابت]", out var value3))
				{
					result = value3;
				}
				break;
			}
			case 3:
			{
				result = "لیست سایر حساب ها";
				if (trDictionary.TryGetValue("[لیست سایر حساب ها]", out var value))
				{
					result = value;
				}
				break;
			}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetDescOfType", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesabList.cs", 221);
		}
		return result;
	}

	private async void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		try
		{
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			await SearchAsync(token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_lstFishHesab == null || _lstFishHesab.Count <= 0)
			{
				_lstFishHesab = await FishHesab.SelectRowsByTypeAsync(((CacheBaseForm)this).ContextGuid, _type, token);
			}
			string text = Text.FixString(((Control)TxtSearch).Text).Trim();
			if (text == "")
			{
				_dGridListsForDisplay = _lstFishHesab;
				fishHesabBindingSource.DataSource = _lstFishHesab;
				((Control)LblRowCount).Text = _lstFishHesab.Count.ToString("N0");
				return;
			}
			string[] array = text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			_dGridListsForDisplay = _lstFishHesab.Select((FishHesab e1) => e1).ToList();
			string[] array2 = array;
			foreach (string item in array2)
			{
				_dGridListsForDisplay = _dGridListsForDisplay.Where((FishHesab e1) => e1.CodeMoein.ToString().Contains(item) || e1.CodeTafsil.ToString().Contains(item) || e1.Name.Contains(item)).ToList();
			}
			fishHesabBindingSource.DataSource = _dGridListsForDisplay;
			((Control)LblRowCount).Text = _dGridListsForDisplay.Count.ToString("N0");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesabList.cs", 264);
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == ((DataGridViewBand)DGCheck).Index && SelectedHesab != null)
			{
				DataGridViewCell currentCell = ((DataGridView)DGrid).CurrentCell;
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((currentCell is DataGridViewCheckBoxCell) ? currentCell : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = !Convert.ToBoolean(((DataGridViewCell)val).Value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesabList.cs", 276);
		}
	}

	private void FRMHesabList_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 32)
			{
				if ((int)keyCode != 13)
				{
					if ((int)keyCode != 27)
					{
						if ((int)keyCode == 32 && ((Control)DGrid).Focused && SelectedHesab != null && ((DataGridView)DGrid).CurrentCell.ColumnIndex != ((DataGridViewBand)DGCheck).Index)
						{
							e.Handled = true;
							DataGridViewCell obj = ((DataGridView)DGrid).SelectedRows[0].Cells[((DataGridViewBand)DGCheck).Index];
							DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
							if (val != null)
							{
								((DataGridViewCell)val).Value = !Convert.ToBoolean(((DataGridViewCell)val).Value);
							}
						}
					}
					else
					{
						e.Handled = true;
						((Form)this).DialogResult = (DialogResult)2;
						((Form)this).Close();
					}
				}
				else if (((Control)DGrid).Focused)
				{
					if (((Control)BTNSelect).Visible)
					{
						((PopupItemControl)BTNSelect).PerformClick();
						e.Handled = true;
					}
					else
					{
						((PopupItemControl)BTNEdit).PerformClick();
						e.Handled = true;
					}
				}
			}
			else if ((int)keyCode <= 45)
			{
				if ((int)keyCode != 40)
				{
					if ((int)keyCode == 45)
					{
						e.Handled = true;
						((PopupItemControl)BTNADD).PerformClick();
					}
				}
				else if (((Control)TxtSearch).Focused)
				{
					((Control)DGrid).Focus();
				}
			}
			else if ((int)keyCode != 46)
			{
				if ((int)keyCode == 118)
				{
					e.Handled = true;
					((PopupItemControl)BTNEdit).PerformClick();
				}
			}
			else
			{
				e.Handled = true;
				((PopupItemControl)BTNDelete).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMHesabList_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesabList.cs", 331);
		}
	}

	private void DGrid_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (((Control)BTNSelect).Visible)
			{
				((PopupItemControl)BTNSelect).PerformClick();
			}
			else
			{
				((PopupItemControl)BTNEdit).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_DoubleClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesabList.cs", 345);
		}
	}

	private async void BtnMazaya_Click(object sender, EventArgs e)
	{
		try
		{
			_type = 1;
			await LoadDataAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMazaya_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesabList.cs", 356);
		}
	}

	private async void BtnKosor_Click(object sender, EventArgs e)
	{
		try
		{
			_type = 2;
			await LoadDataAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnKosor_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesabList.cs", 366);
		}
	}

	private async void BtnSayer_Click(object sender, EventArgs e)
	{
		try
		{
			_type = 3;
			await LoadDataAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnSayer_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesabList.cs", 376);
		}
	}

	private void BTNSelect_Click(object sender, EventArgs e)
	{
		try
		{
			if (((DataGridView)DGrid).RowCount > 0)
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNSelect_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesabList.cs", 388);
		}
	}

	private async void BTNADD_Click(object sender, EventArgs e)
	{
		_ = 3;
		try
		{
			((Control)DGrid).Focus();
			if (((Control)BTNADD).Visible && ((Control)BTNADD).Enabled)
			{
				FishHesab fishHesab = new FishHesab(((CacheBaseForm)this).ContextGuid)
				{
					Type = _type
				};
				FishHesab fishHesab2 = fishHesab;
				fishHesab2.CodeMoein = await FishHesab.MaxCodeMoeinAsync(((CacheBaseForm)this).ContextGuid, _type);
				FishHesab fishHesab3 = fishHesab;
				fishHesab3.CodeTafsil = await FishHesab.NextCodeTafsilAsync(((CacheBaseForm)this).ContextGuid, _type);
				if ((int)(await ((CacheBaseForm)new FrmHesab(((CacheBaseForm)this).ContextGuid, fishHesab)).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					await LoadDataAsync();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNADD_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesabList.cs", 408);
		}
	}

	private async void BTNEdit_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (((Control)BTNEdit).Visible && ((Control)BTNEdit).Enabled)
			{
				((Control)DGrid).Focus();
				if (SelectedHesab != null && (int)(await ((CacheBaseForm)new FrmHesab(((CacheBaseForm)this).ContextGuid, SelectedHesab)).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					await LoadDataAsync();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesabList.cs", 422);
		}
	}

	private async void BTNDelete_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (!((Control)BTNDelete).Visible || !((Control)BTNDelete).Enabled)
			{
				return;
			}
			((Control)DGrid).Focus();
			if (SelectedHesab == null)
			{
				return;
			}
			if (SelectedHesab.Id < 1000)
			{
				Utility.MessageBoxShowError("[شما مجاز به حذف این رکورد نمی باشید]", ((CacheBaseForm)this).ContextGuid);
				return;
			}
			trDictionary.TryGetValue("[برای حذف کردن مطمئن هستید؟]", out var value);
			trDictionary.TryGetValue("[هشدار]", out var value2);
			if ((int)FMessageBox.Show(value, value2, FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) != 7)
			{
				SelectedHesab.St = false;
				await SelectedHesab.SaveAsync();
				await LoadDataAsync();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesabList.cs", 450);
		}
	}

	private void fishHesabBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		SelectedHesab = (FishHesab)fishHesabBindingSource.Current;
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
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

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Expected O, but got Unknown
		//IL_0998: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a2: Expected O, but got Unknown
		//IL_0a7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a87: Expected O, but got Unknown
		//IL_0b65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6f: Expected O, but got Unknown
		//IL_0d27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d31: Expected O, but got Unknown
		//IL_0e1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e24: Expected O, but got Unknown
		//IL_1627: Unknown result type (might be due to invalid IL or missing references)
		//IL_1631: Expected O, but got Unknown
		//IL_1644: Unknown result type (might be due to invalid IL or missing references)
		//IL_164e: Expected O, but got Unknown
		//IL_1696: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a0: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmHesabList));
		panelEx1 = new PanelEx();
		BTNSelect = new ButtonX();
		LblDesc = new LabelX();
		fishHesabBindingSource = new BindingSource(components);
		labelX22 = new LabelX();
		LblRowCount = new LabelX();
		GroupPanel1 = new GroupPanel();
		LabelX1 = new LabelX();
		TxtSearch = new TextBoxX();
		BTNEdit = new ButtonX();
		BTNDelete = new ButtonX();
		BTNADD = new ButtonX();
		DGrid = new DataGridViewX();
		DGCheck = new DataGridViewCheckBoxColumn();
		DescriptionDG = new DataGridViewTextBoxColumn();
		DGID = new DataGridViewTextBoxColumn();
		DGCodeMoein = new DataGridViewTextBoxColumn();
		DGCodeTafsil = new DataGridViewTextBoxColumn();
		nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		DGAccount = new DataGridViewTextBoxColumn();
		Insurance = new DataGridViewCheckBoxColumn();
		Taxable = new DataGridViewCheckBoxColumn();
		panelKol = new PanelEx();
		panelEx3 = new PanelEx();
		BtnSayer = new ButtonX();
		BtnKosor = new ButtonX();
		BtnMazaya = new ButtonX();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)fishHesabBindingSource).BeginInit();
		((Control)GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)panelKol).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BTNSelect);
		((Control)panelEx1).Controls.Add((Control)(object)LblDesc);
		((Control)panelEx1).Controls.Add((Control)(object)labelX22);
		((Control)panelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)panelEx1).Controls.Add((Control)(object)GroupPanel1);
		((Control)panelEx1).Controls.Add((Control)(object)BTNEdit);
		((Control)panelEx1).Controls.Add((Control)(object)BTNDelete);
		((Control)panelEx1).Controls.Add((Control)(object)BTNADD);
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		((Control)panelEx1).Controls.Add((Control)(object)panelKol);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(820, 562);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		panelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)BTNSelect).AccessibleRole = (AccessibleRole)43;
		BTNSelect.ColorTable = (eButtonColor)6;
		((Control)BTNSelect).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BTNSelect.ImagePosition = (eImagePosition)1;
		((Control)BTNSelect).Location = new Point(15, 74);
		((Control)BTNSelect).Name = "BTNSelect";
		((Control)BTNSelect).RightToLeft = (RightToLeft)1;
		((Control)BTNSelect).Size = new Size(128, 114);
		((PopupItemControl)BTNSelect).Style = (eDotNetBarStyle)9;
		((Control)BTNSelect).TabIndex = 6;
		((Control)BTNSelect).TabStop = false;
		((Control)BTNSelect).Text = "[انتخاب] (Enter)";
		((Control)BTNSelect).Visible = false;
		((Control)BTNSelect).Click += BTNSelect_Click;
		((Control)LblDesc).Anchor = (AnchorStyles)14;
		((BaseItemControl)LblDesc).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblDesc).DataBindings.Add(new Binding("Text", (object)fishHesabBindingSource, "Description", true));
		((Control)LblDesc).ForeColor = Color.Maroon;
		((Control)LblDesc).Location = new Point(152, 523);
		((Control)LblDesc).Name = "LblDesc";
		((Control)LblDesc).Size = new Size(587, 27);
		((Control)LblDesc).TabIndex = 8;
		((Control)LblDesc).Text = "[شرح] :";
		fishHesabBindingSource.DataSource = typeof(FishHesab);
		fishHesabBindingSource.CurrentChanged += fishHesabBindingSource_CurrentChanged;
		((Control)labelX22).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX22).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX22).Location = new Point(745, 523);
		((Control)labelX22).Name = "labelX22";
		((Control)labelX22).Size = new Size(63, 27);
		((Control)labelX22).TabIndex = 8;
		((Control)labelX22).Text = "[شرح] :";
		labelX22.TextAlignment = (StringAlignment)2;
		((Control)LblRowCount).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Location = new Point(15, 490);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(128, 27);
		((Control)LblRowCount).TabIndex = 8;
		((Control)LblRowCount).Text = "labelX2";
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GroupPanel1).Controls.Add((Control)(object)LabelX1);
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtSearch);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Location = new Point(15, 7);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(793, 60);
		((PanelControl)GroupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)GroupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)GroupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)GroupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)GroupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)GroupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)GroupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)GroupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)GroupPanel1).Style.CornerDiameter = 4;
		((PanelControl)GroupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)GroupPanel1).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)GroupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)GroupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)GroupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)GroupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)GroupPanel1).TabIndex = 0;
		((Control)GroupPanel1).Text = "[جستجو]";
		GroupPanel1.TitleImage = (Image)(object)Resources.search2;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Location = new Point(707, 3);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(50, 23);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "[جستجو]  :";
		((Control)TxtSearch).Anchor = (AnchorStyles)13;
		TxtSearch.Border.Class = "TextBoxBorder";
		TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)TxtSearch).Location = new Point(3, 5);
		((Control)TxtSearch).Name = "TxtSearch";
		((Control)TxtSearch).Size = new Size(696, 21);
		((Control)TxtSearch).TabIndex = 0;
		((Control)TxtSearch).TextChanged += TxtSearch_TextChanged;
		((Control)BTNEdit).AccessibleRole = (AccessibleRole)43;
		BTNEdit.ColorTable = (eButtonColor)6;
		((Control)BTNEdit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BTNEdit.Image = (Image)(object)Resources.edit2;
		BTNEdit.ImagePosition = (eImagePosition)1;
		((Control)BTNEdit).Location = new Point(15, 112);
		((Control)BTNEdit).Name = "BTNEdit";
		((Control)BTNEdit).RightToLeft = (RightToLeft)1;
		((Control)BTNEdit).Size = new Size(128, 34);
		((PopupItemControl)BTNEdit).Style = (eDotNetBarStyle)9;
		((Control)BTNEdit).TabIndex = 4;
		((Control)BTNEdit).TabStop = false;
		((Control)BTNEdit).Text = "[ویرایش] (F7)";
		((Control)BTNEdit).Click += BTNEdit_Click;
		((Control)BTNDelete).AccessibleRole = (AccessibleRole)43;
		BTNDelete.ColorTable = (eButtonColor)6;
		((Control)BTNDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BTNDelete.Image = (Image)(object)Resources.edit_delete;
		BTNDelete.ImagePosition = (eImagePosition)1;
		((Control)BTNDelete).Location = new Point(15, 152);
		((Control)BTNDelete).Name = "BTNDelete";
		((Control)BTNDelete).RightToLeft = (RightToLeft)1;
		((Control)BTNDelete).Size = new Size(128, 34);
		((PopupItemControl)BTNDelete).Style = (eDotNetBarStyle)9;
		((Control)BTNDelete).TabIndex = 5;
		((Control)BTNDelete).TabStop = false;
		((Control)BTNDelete).Text = "[حذف] (Delete)";
		((Control)BTNDelete).Click += BTNDelete_Click;
		((Control)BTNADD).AccessibleRole = (AccessibleRole)43;
		BTNADD.ColorTable = (eButtonColor)6;
		((Control)BTNADD).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BTNADD.Image = (Image)(object)Resources.edit_add1;
		BTNADD.ImagePosition = (eImagePosition)1;
		((Control)BTNADD).Location = new Point(15, 74);
		((Control)BTNADD).Name = "BTNADD";
		((Control)BTNADD).RightToLeft = (RightToLeft)1;
		((Control)BTNADD).Size = new Size(128, 34);
		((PopupItemControl)BTNADD).Style = (eDotNetBarStyle)9;
		((Control)BTNADD).TabIndex = 7;
		((Control)BTNADD).TabStop = false;
		((Control)BTNADD).Text = "[اضافه] (Insert)";
		((Control)BTNADD).Click += BTNADD_Click;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[9]
		{
			(DataGridViewColumn)DGCheck,
			(DataGridViewColumn)DescriptionDG,
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)DGCodeMoein,
			(DataGridViewColumn)DGCodeTafsil,
			(DataGridViewColumn)nameDataGridViewTextBoxColumn,
			(DataGridViewColumn)DGAccount,
			(DataGridViewColumn)Insurance,
			(DataGridViewColumn)Taxable
		});
		((DataGridView)DGrid).DataSource = fishHesabBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(152, 73);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(656, 444);
		((Control)DGrid).TabIndex = 2;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((Control)DGrid).DoubleClick += DGrid_DoubleClick;
		((DataGridViewColumn)DGCheck).HeaderText = "";
		((DataGridViewColumn)DGCheck).Name = "DGCheck";
		((DataGridViewBand)DGCheck).ReadOnly = true;
		((DataGridViewColumn)DGCheck).Width = 20;
		((DataGridViewColumn)DescriptionDG).DataPropertyName = "Description";
		((DataGridViewColumn)DescriptionDG).HeaderText = "[شرح]";
		((DataGridViewColumn)DescriptionDG).Name = "DescriptionDG";
		((DataGridViewBand)DescriptionDG).ReadOnly = true;
		((DataGridViewBand)DescriptionDG).Visible = false;
		((DataGridViewColumn)DGID).DataPropertyName = "Id";
		((DataGridViewColumn)DGID).HeaderText = "[شناسه]";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)DGCodeMoein).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCodeMoein).DataPropertyName = "CodeMoein";
		((DataGridViewColumn)DGCodeMoein).HeaderText = "[کد معین]";
		((DataGridViewColumn)DGCodeMoein).Name = "DGCodeMoein";
		((DataGridViewBand)DGCodeMoein).ReadOnly = true;
		((DataGridViewColumn)DGCodeMoein).Width = 104;
		((DataGridViewColumn)DGCodeTafsil).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCodeTafsil).DataPropertyName = "CodeTafsil";
		((DataGridViewColumn)DGCodeTafsil).HeaderText = "[کد تفصیل]";
		((DataGridViewColumn)DGCodeTafsil).Name = "DGCodeTafsil";
		((DataGridViewBand)DGCodeTafsil).ReadOnly = true;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).DataPropertyName = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).HeaderText = "[نام]";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).MinimumWidth = 100;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).Name = "nameDataGridViewTextBoxColumn";
		((DataGridViewBand)nameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)DGAccount).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGAccount).DataPropertyName = "Account";
		val3.Format = "N0";
		val3.NullValue = null;
		((DataGridViewBand)DGAccount).DefaultCellStyle = val3;
		((DataGridViewColumn)DGAccount).HeaderText = "[مبلغ]";
		((DataGridViewColumn)DGAccount).MinimumWidth = 150;
		((DataGridViewColumn)DGAccount).Name = "DGAccount";
		((DataGridViewBand)DGAccount).ReadOnly = true;
		((DataGridViewColumn)DGAccount).Width = 150;
		((DataGridViewColumn)Insurance).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)Insurance).DataPropertyName = "Insurance";
		((DataGridViewColumn)Insurance).HeaderText = "[مشمول بیمه]";
		((DataGridViewColumn)Insurance).MinimumWidth = 50;
		((DataGridViewColumn)Insurance).Name = "Insurance";
		((DataGridViewBand)Insurance).ReadOnly = true;
		((DataGridViewColumn)Insurance).Width = 109;
		((DataGridViewColumn)Taxable).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)Taxable).DataPropertyName = "Taxable";
		((DataGridViewColumn)Taxable).HeaderText = "[مشمول مالیات]";
		((DataGridViewColumn)Taxable).MinimumWidth = 50;
		((DataGridViewColumn)Taxable).Name = "Taxable";
		((DataGridViewBand)Taxable).ReadOnly = true;
		((DataGridViewColumn)Taxable).Width = 116;
		panelKol.CanvasColor = SystemColors.Control;
		panelKol.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelKol).Controls.Add((Control)(object)panelEx3);
		((Control)panelKol).Controls.Add((Control)(object)BtnSayer);
		((Control)panelKol).Controls.Add((Control)(object)BtnKosor);
		((Control)panelKol).Controls.Add((Control)(object)BtnMazaya);
		panelKol.DisabledBackColor = Color.Empty;
		((Control)panelKol).Location = new Point(15, 194);
		((Control)panelKol).Name = "panelKol";
		((Control)panelKol).Size = new Size(128, 179);
		panelKol.Style.Alignment = (StringAlignment)1;
		panelKol.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelKol.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelKol.Style.Border = (eBorderType)1;
		panelKol.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelKol.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelKol.Style.GradientAngle = 90;
		((Control)panelKol).TabIndex = 1;
		panelEx3.CanvasColor = SystemColors.Control;
		panelEx3.ColorSchemeStyle = (eDotNetBarStyle)9;
		panelEx3.DisabledBackColor = Color.Empty;
		((Control)panelEx3).Dock = (DockStyle)1;
		((Control)panelEx3).Location = new Point(0, 0);
		((Control)panelEx3).Name = "panelEx3";
		((Control)panelEx3).Size = new Size(128, 21);
		panelEx3.Style.Alignment = (StringAlignment)1;
		panelEx3.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx3.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx3.Style.Border = (eBorderType)1;
		panelEx3.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx3.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx3.Style.GradientAngle = 90;
		((Control)panelEx3).TabIndex = 2;
		((Control)panelEx3).Text = "[دسته بندی حساب ها]";
		((Control)BtnSayer).AccessibleRole = (AccessibleRole)43;
		((Control)BtnSayer).Anchor = (AnchorStyles)9;
		BtnSayer.ColorTable = (eButtonColor)3;
		BtnSayer.Image = (Image)(object)Resources.going_down_two_bars;
		((Control)BtnSayer).Location = new Point(6, 171);
		((Control)BtnSayer).Name = "BtnSayer";
		((Control)BtnSayer).Size = new Size(116, 66);
		((PopupItemControl)BtnSayer).Style = (eDotNetBarStyle)9;
		((Control)BtnSayer).TabIndex = 1;
		((Control)BtnSayer).Text = "[ســـایــر]";
		((Control)BtnSayer).Visible = false;
		((Control)BtnSayer).Click += BtnSayer_Click;
		((Control)BtnKosor).AccessibleRole = (AccessibleRole)43;
		((Control)BtnKosor).Anchor = (AnchorStyles)9;
		BtnKosor.ColorTable = (eButtonColor)3;
		BtnKosor.Image = (Image)(object)Resources.going_down;
		((Control)BtnKosor).Location = new Point(6, 99);
		((Control)BtnKosor).Name = "BtnKosor";
		((Control)BtnKosor).Size = new Size(116, 66);
		((PopupItemControl)BtnKosor).Style = (eDotNetBarStyle)9;
		((Control)BtnKosor).TabIndex = 0;
		((Control)BtnKosor).Text = "[کسورات]";
		((Control)BtnKosor).Click += BtnKosor_Click;
		((Control)BtnMazaya).AccessibleRole = (AccessibleRole)43;
		((Control)BtnMazaya).Anchor = (AnchorStyles)9;
		BtnMazaya.ColorTable = (eButtonColor)3;
		BtnMazaya.Image = (Image)(object)Resources.going_up;
		((Control)BtnMazaya).Location = new Point(6, 27);
		((Control)BtnMazaya).Name = "BtnMazaya";
		((Control)BtnMazaya).Size = new Size(116, 66);
		((PopupItemControl)BtnMazaya).Style = (eDotNetBarStyle)9;
		((Control)BtnMazaya).TabIndex = 3;
		((Control)BtnMazaya).Text = "[مزایا]";
		((Control)BtnMazaya).Click += BtnMazaya_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(820, 562);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).IsMdiContainer = true;
		((Control)this).MinimumSize = new Size(709, 370);
		((Control)this).Name = "FrmHesabList";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[لیست مزایای ثابت]";
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).KeyDown += new KeyEventHandler(FRMHesabList_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)fishHesabBindingSource).EndInit();
		((Control)GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((Control)panelKol).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
