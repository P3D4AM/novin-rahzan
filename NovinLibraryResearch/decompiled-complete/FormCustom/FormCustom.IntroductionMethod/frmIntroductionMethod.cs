using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Business.Accounting;
using EntityCache.Business.Global;
using FormCustom.Cache;
using FormCustom.Properties;
using FormCustom.UC;
using PacketParser;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.IntroductionMethod;

public class frmIntroductionMethod : AccountingBaseForm
{
	private bool _isDisposed;

	private CancellationTokenSource _token = new CancellationTokenSource();

	private CancellationTokenSource _searchToken = new CancellationTokenSource();

	private CancellationTokenSource _accessToken;

	private IEnumerable<IntroductionMethodBusiness> _listData;

	private IEnumerable<IntroductionMethodBusiness> _filteredList;

	private IContainer components;

	private Label label1;

	private UcButtons btn_cancel;

	private BindingSource IntroductionMethodBindingSource;

	internal DataGridView DGrid;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn modifiedDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn statusDataGridViewCheckBoxColumn;

	private DataGridViewTextBoxColumn serverStatusDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deliveryDateDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deliveryIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deliveryMessageDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deliveryShortMessageDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn hostRefrencedDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn contextGuidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn databaseManagerDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn changeStatusDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn isDisposedDataGridViewCheckBoxColumn;

	private UcSearchBox txtSearch;

	private UcGroupBox ucGroupBox1;

	private UcActionPictureBox btnDelete;

	private UcActionPictureBox btnEdit;

	private UcActionPictureBox btnAdd;

	private UcActionPictureBox btnSelect;

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

	public string SearchString { get; private set; }

	public IntroductionMethodBusiness Current { get; private set; }

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			_listData = await IntroductionMethodBusiness.GetAllAsync(base.ContextGuid, true);
			if (!token.IsCancellationRequested && !IsDisposed)
			{
				_searchToken?.Cancel();
				_searchToken = new CancellationTokenSource(6000);
				CancellationTokenSource t = CancellationTokenSource.CreateLinkedTokenSource(token, _searchToken.Token);
				Task.Run(() => SearchAsync(t.Token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 39);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			_filteredList = _listData?.OrderBy((IntroductionMethodBusiness q) => q.Name);
			if (!(_filteredList?.Any() ?? false))
			{
				((Control)this).Invoke((Delegate)(MethodInvoker)delegate
				{
					IntroductionMethodBindingSource.DataSource = null;
				});
			}
			else
			{
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
				if (string.IsNullOrEmpty(SearchString))
				{
					SearchString = "";
				}
				List<string> list = SearchString.SplitString();
				if (list != null && list.Count > 0)
				{
					foreach (string item in list)
					{
						if (!string.IsNullOrEmpty(item) && item.Trim() != "")
						{
							if (token.IsCancellationRequested || IsDisposed)
							{
								return;
							}
							item.ParseToDecimal();
							_filteredList = _filteredList?.Where((IntroductionMethodBusiness x) => x.Name.StringComparisons(item));
						}
					}
				}
				while (!((Control)this).IsHandleCreated)
				{
					await Task.Delay(100);
					if (token.IsCancellationRequested || IsDisposed)
					{
						return;
					}
				}
				((Control)this).BeginInvoke((Delegate)(Action)delegate
				{
					try
					{
						if (!token.IsCancellationRequested && !IsDisposed)
						{
							IntroductionMethodBindingSource.DataSource = _filteredList?.ToSortableBindingList(token);
						}
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 75);
					}
				});
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 78);
		}
	}

	private async Task AccessControlAsync(CancellationToken token)
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			UserAccBusiness user = await (await CashierBusiness.GetCurrentCashierAsync(base.ContextGuid, token)).GetUserAsync(token);
			do
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						btnDelete.Enabled = user.UserAccess.Sell.Sell_InsertIntroductionMethodDelete;
						btnAdd.Enabled = user.UserAccess.Sell.Sell_InsertIntroductionMethod;
						btnEdit.Enabled = user.UserAccess.Sell.Sell_InsertIntroductionMethodEdit;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "AccessControlAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 102);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AccessControlAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 105);
		}
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
				_token?.Cancel();
				_token?.Dispose();
				_token = null;
				_accessToken?.Cancel();
				_accessToken?.Dispose();
				_accessToken = null;
				if (disposing)
				{
					components?.Dispose();
				}
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 129);
		}
	}

	public frmIntroductionMethod(string contextGuid, Font font, Font regularFont)
		: base(contextGuid)
	{
		frmIntroductionMethod frmIntroductionMethod2 = this;
		try
		{
			InitializeComponent();
			DGrid.Initialize(font);
			btnAdd.Init("[افزودن] (Insert)", regularFont);
			btnEdit.Init("[ویرایش] (F7)".GetTranslate(), regularFont);
			btnDelete.Init("[حذف] (Delete)".GetTranslate(), regularFont);
			btnSelect.Init("[انتخاب]".GetTranslate(), regularFont);
			_token = new CancellationTokenSource(60000);
			CancellationToken token = _token.Token;
			Task.Run(() => frmIntroductionMethod2.LoadDataAsync(token));
			_accessToken = new CancellationTokenSource(60000);
			CancellationToken aToken = _accessToken.Token;
			Task.Run(() => frmIntroductionMethod2.AccessControlAsync(aToken));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 152);
		}
	}

	private void frmIntroductionMethod_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 45)
			{
				if ((int)keyCode != 27)
				{
					if ((int)keyCode == 45)
					{
						btnAdd.PerformClick();
					}
				}
				else
				{
					btn_cancel.PerformClick();
				}
			}
			else if ((int)keyCode != 46)
			{
				if ((int)keyCode == 118)
				{
					btnEdit.PerformClick();
				}
			}
			else
			{
				btnDelete.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmIntroductionMethod_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 167);
		}
	}

	private void txtSearch_OnTextChanged(object sender, EventArgs e)
	{
		try
		{
			SearchString = ((Control)txtSearch).Text;
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource(6000);
			CancellationToken tkn = _searchToken.Token;
			Task.Run(() => SearchAsync(tkn));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtSearch_OnTextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 179);
		}
	}

	private void txtSearch_OnKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		try
		{
			if (((int)e.KeyCode == 13) | ((int)e.KeyCode == 40))
			{
				e.Handled = true;
				((Control)DGrid).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtSearch_OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 189);
		}
	}

	private void DGrid_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (Text.IsStringOrNumber(e.KeyChar))
			{
				((Control)txtSearch).Focus();
				((Control)txtSearch).Text = e.KeyChar.ToString();
				txtSearch.SelectionStart = 9999;
				txtSearch.SelectionLength = 0;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_KeyPress", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 201);
		}
	}

	private void DGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 13)
			{
				if ((int)keyCode == 46)
				{
					btnDelete.PerformClick();
				}
			}
			else
			{
				btnSelect.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 213);
		}
	}

	private void IntroductionMethodBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			Current = (IntroductionMethodBusiness)IntroductionMethodBindingSource.Current;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IntroductionMethodBindingSource_CurrentChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 221);
		}
	}

	private void DGrid_DoubleClick(object sender, EventArgs e)
	{
		btnSelect.PerformClick();
	}

	private void btnEdit_OnClick(object sender, EventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		try
		{
			if (Current != null && (int)((Form)new frmIntroductionMethodMain(base.ContextGuid, Current)).ShowDialog((IWin32Window)(object)this) == 1)
			{
				_token?.Cancel();
				_token = new CancellationTokenSource(60000);
				CancellationToken token = _token.Token;
				Task.Run(() => LoadDataAsync(token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnEdit_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 237);
		}
	}

	private void btnSelect_OnClick(object sender, EventArgs e)
	{
		try
		{
			if (Current != null)
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnSelect_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 247);
		}
	}

	private async void btnDelete_OnClick(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			try
			{
				((Control)DGrid).Focus();
				if (Current == null)
				{
					goto end_IL_0057;
				}
				string translate = "آیا برای حذف کردن مطمئن هستید؟".GetTranslate();
				if ((int)((Form)new FrmShowErrorMessage(base.ContextGuid, translate, "پیغام سیستم".GetTranslate(), (MessageBoxButtons)4, isAskQuestion: true, LanguageTypes.Farsi, "btnDelete_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs")).ShowDialog((IWin32Window)(object)this) != 6)
				{
					goto end_IL_0057;
				}
				Current.Status = false;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await Current.SaveAsync(base.system, sendToServer: true), ReturnedState.None, "btnDelete_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 260);
				goto end_IL_004e;
				end_IL_0057:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "btnDelete_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 264);
				res.AddReturnedValue(ex, "", "btnDelete_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 265);
				goto end_IL_004e;
			}
			end_IL_004e:;
		}
		finally
		{
			if (res.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(res, "پیغام سیستم", "btnDelete_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs");
			}
			else
			{
				_token?.Cancel();
				_token = new CancellationTokenSource(60000);
				CancellationToken token = _token.Token;
				Task.Run(() => LoadDataAsync(token));
			}
		}
	}

	private void btn_cancel_OnClick(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btn_cancel_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 286);
		}
	}

	private void btnAdd_OnClick(object sender, EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		try
		{
			if ((int)((Form)new frmIntroductionMethodMain(base.ContextGuid, new IntroductionMethodBusiness(base.ContextGuid))).ShowDialog((IWin32Window)(object)this) == 1)
			{
				_token?.Cancel();
				_token = new CancellationTokenSource(60000);
				CancellationToken token = _token.Token;
				Task.Run(() => LoadDataAsync(token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnAdd_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\IntroductionMethod\\frmIntroductionMethod.cs", 300);
		}
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
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
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Expected O, but got Unknown
		//IL_099e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Expected O, but got Unknown
		//IL_09d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9b: Expected O, but got Unknown
		//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Expected O, but got Unknown
		//IL_0aef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bed: Expected O, but got Unknown
		//IL_0c2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d30: Expected O, but got Unknown
		//IL_0d74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e76: Expected O, but got Unknown
		//IL_0eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbc: Expected O, but got Unknown
		//IL_0ffd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1137: Unknown result type (might be due to invalid IL or missing references)
		//IL_1141: Expected O, but got Unknown
		//IL_1154: Unknown result type (might be due to invalid IL or missing references)
		//IL_115e: Expected O, but got Unknown
		//IL_1163: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11aa: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmIntroductionMethod));
		label1 = new Label();
		btn_cancel = new UcButtons();
		IntroductionMethodBindingSource = new BindingSource(components);
		DGrid = new DataGridView();
		nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		guidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		modifiedDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		statusDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		serverStatusDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deliveryDateDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deliveryIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deliveryMessageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deliveryShortMessageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		hostRefrencedDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		contextGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		databaseManagerDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		changeStatusDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		isDisposedDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		txtSearch = new UcSearchBox();
		ucGroupBox1 = new UcGroupBox();
		btnDelete = new UcActionPictureBox();
		btnEdit = new UcActionPictureBox();
		btnAdd = new UcActionPictureBox();
		btnSelect = new UcActionPictureBox();
		((ISupportInitialize)IntroductionMethodBindingSource).BeginInit();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("B Yekan", 20.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label1).Location = new Point(52, 4);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(345, 41);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "[لطفا نحوه آشنایی را انتخاب نمایید]";
		((Control)btn_cancel).Anchor = (AnchorStyles)6;
		((Control)btn_cancel).BackColor = Color.Transparent;
		btn_cancel.CornerType = EnButtonCornerType.Round;
		((Control)btn_cancel).Cursor = Cursors.Hand;
		btn_cancel.DisplayText = "[انصراف](Esc)";
		((Control)btn_cancel).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btn_cancel.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btn_cancel.IsShowMoreDetails = false;
		((Control)btn_cancel).Location = new Point(6, 669);
		((Control)btn_cancel).Margin = new Padding(4, 5, 4, 5);
		((Control)btn_cancel).Name = "btn_cancel";
		((Control)btn_cancel).RightToLeft = (RightToLeft)1;
		((Control)btn_cancel).Size = new Size(447, 41);
		((Control)btn_cancel).TabIndex = 3;
		btn_cancel.TitleFont = new Font("B Yekan", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		btn_cancel.Type = EnButtonType.Normal;
		btn_cancel.OnClick += btn_cancel_OnClick;
		IntroductionMethodBindingSource.DataSource = typeof(IntroductionMethodBusiness);
		IntroductionMethodBindingSource.CurrentChanged += IntroductionMethodBindingSource_CurrentChanged;
		DGrid.AllowUserToAddRows = false;
		DGrid.AllowUserToDeleteRows = false;
		DGrid.AllowUserToResizeRows = false;
		DGrid.AutoGenerateColumns = false;
		DGrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		DGrid.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[14]
		{
			(DataGridViewColumn)nameDataGridViewTextBoxColumn,
			(DataGridViewColumn)guidDataGridViewTextBoxColumn,
			(DataGridViewColumn)modifiedDataGridViewTextBoxColumn,
			(DataGridViewColumn)statusDataGridViewCheckBoxColumn,
			(DataGridViewColumn)serverStatusDataGridViewTextBoxColumn,
			(DataGridViewColumn)deliveryDateDataGridViewTextBoxColumn,
			(DataGridViewColumn)deliveryIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)deliveryMessageDataGridViewTextBoxColumn,
			(DataGridViewColumn)deliveryShortMessageDataGridViewTextBoxColumn,
			(DataGridViewColumn)hostRefrencedDataGridViewTextBoxColumn,
			(DataGridViewColumn)contextGuidDataGridViewTextBoxColumn,
			(DataGridViewColumn)databaseManagerDataGridViewTextBoxColumn,
			(DataGridViewColumn)changeStatusDataGridViewTextBoxColumn,
			(DataGridViewColumn)isDisposedDataGridViewCheckBoxColumn
		});
		DGrid.DataSource = IntroductionMethodBindingSource;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		DGrid.DefaultCellStyle = val;
		DGrid.GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(4, 184);
		DGrid.MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		DGrid.ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		DGrid.RowHeadersVisible = false;
		DGrid.RowTemplate.Height = 29;
		DGrid.SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(456, 475);
		((Control)DGrid).TabIndex = 1;
		((Control)DGrid).DoubleClick += DGrid_DoubleClick;
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		((Control)DGrid).KeyPress += new KeyPressEventHandler(DGrid_KeyPress);
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).DataPropertyName = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).HeaderText = "[عنوان]";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).Name = "nameDataGridViewTextBoxColumn";
		((DataGridViewBand)nameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).DataPropertyName = "Guid";
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).HeaderText = "Guid";
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).Name = "guidDataGridViewTextBoxColumn";
		((DataGridViewBand)guidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)guidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)modifiedDataGridViewTextBoxColumn).DataPropertyName = "Modified";
		((DataGridViewColumn)modifiedDataGridViewTextBoxColumn).HeaderText = "Modified";
		((DataGridViewColumn)modifiedDataGridViewTextBoxColumn).Name = "modifiedDataGridViewTextBoxColumn";
		((DataGridViewBand)modifiedDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)modifiedDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)statusDataGridViewCheckBoxColumn).DataPropertyName = "Status";
		((DataGridViewColumn)statusDataGridViewCheckBoxColumn).HeaderText = "Status";
		((DataGridViewColumn)statusDataGridViewCheckBoxColumn).Name = "statusDataGridViewCheckBoxColumn";
		((DataGridViewBand)statusDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewBand)statusDataGridViewCheckBoxColumn).Visible = false;
		((DataGridViewColumn)serverStatusDataGridViewTextBoxColumn).DataPropertyName = "ServerStatus";
		((DataGridViewColumn)serverStatusDataGridViewTextBoxColumn).HeaderText = "ServerStatus";
		((DataGridViewColumn)serverStatusDataGridViewTextBoxColumn).Name = "serverStatusDataGridViewTextBoxColumn";
		((DataGridViewBand)serverStatusDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)serverStatusDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deliveryDateDataGridViewTextBoxColumn).DataPropertyName = "DeliveryDate";
		((DataGridViewColumn)deliveryDateDataGridViewTextBoxColumn).HeaderText = "DeliveryDate";
		((DataGridViewColumn)deliveryDateDataGridViewTextBoxColumn).Name = "deliveryDateDataGridViewTextBoxColumn";
		((DataGridViewBand)deliveryDateDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)deliveryDateDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deliveryIdDataGridViewTextBoxColumn).DataPropertyName = "DeliveryId";
		((DataGridViewColumn)deliveryIdDataGridViewTextBoxColumn).HeaderText = "DeliveryId";
		((DataGridViewColumn)deliveryIdDataGridViewTextBoxColumn).Name = "deliveryIdDataGridViewTextBoxColumn";
		((DataGridViewBand)deliveryIdDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)deliveryIdDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deliveryMessageDataGridViewTextBoxColumn).DataPropertyName = "DeliveryMessage";
		((DataGridViewColumn)deliveryMessageDataGridViewTextBoxColumn).HeaderText = "DeliveryMessage";
		((DataGridViewColumn)deliveryMessageDataGridViewTextBoxColumn).Name = "deliveryMessageDataGridViewTextBoxColumn";
		((DataGridViewBand)deliveryMessageDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)deliveryMessageDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deliveryShortMessageDataGridViewTextBoxColumn).DataPropertyName = "DeliveryShortMessage";
		((DataGridViewColumn)deliveryShortMessageDataGridViewTextBoxColumn).HeaderText = "DeliveryShortMessage";
		((DataGridViewColumn)deliveryShortMessageDataGridViewTextBoxColumn).Name = "deliveryShortMessageDataGridViewTextBoxColumn";
		((DataGridViewBand)deliveryShortMessageDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)deliveryShortMessageDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)hostRefrencedDataGridViewTextBoxColumn).DataPropertyName = "HostRefrenced";
		((DataGridViewColumn)hostRefrencedDataGridViewTextBoxColumn).HeaderText = "HostRefrenced";
		((DataGridViewColumn)hostRefrencedDataGridViewTextBoxColumn).Name = "hostRefrencedDataGridViewTextBoxColumn";
		((DataGridViewBand)hostRefrencedDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)hostRefrencedDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)contextGuidDataGridViewTextBoxColumn).DataPropertyName = "ContextGuid";
		((DataGridViewColumn)contextGuidDataGridViewTextBoxColumn).HeaderText = "ContextGuid";
		((DataGridViewColumn)contextGuidDataGridViewTextBoxColumn).Name = "contextGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)contextGuidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)contextGuidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)databaseManagerDataGridViewTextBoxColumn).DataPropertyName = "DatabaseManager";
		((DataGridViewColumn)databaseManagerDataGridViewTextBoxColumn).HeaderText = "DatabaseManager";
		((DataGridViewColumn)databaseManagerDataGridViewTextBoxColumn).Name = "databaseManagerDataGridViewTextBoxColumn";
		((DataGridViewBand)databaseManagerDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)databaseManagerDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)changeStatusDataGridViewTextBoxColumn).DataPropertyName = "ChangeStatus";
		((DataGridViewColumn)changeStatusDataGridViewTextBoxColumn).HeaderText = "ChangeStatus";
		((DataGridViewColumn)changeStatusDataGridViewTextBoxColumn).Name = "changeStatusDataGridViewTextBoxColumn";
		((DataGridViewBand)changeStatusDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)changeStatusDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)isDisposedDataGridViewCheckBoxColumn).DataPropertyName = "IsDisposed";
		((DataGridViewColumn)isDisposedDataGridViewCheckBoxColumn).HeaderText = "IsDisposed";
		((DataGridViewColumn)isDisposedDataGridViewCheckBoxColumn).Name = "isDisposedDataGridViewCheckBoxColumn";
		((DataGridViewBand)isDisposedDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewBand)isDisposedDataGridViewCheckBoxColumn).Visible = false;
		((Control)txtSearch).Anchor = (AnchorStyles)13;
		((Control)txtSearch).BackColor = Color.Transparent;
		((Control)txtSearch).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		txtSearch.IsSaerchBox = true;
		((Control)txtSearch).Location = new Point(40, 50);
		((Control)txtSearch).Margin = new Padding(4, 5, 4, 5);
		((Control)txtSearch).Name = "txtSearch";
		((Control)txtSearch).RightToLeft = (RightToLeft)1;
		txtSearch.SelectionLength = 0;
		txtSearch.SelectionStart = 0;
		((Control)txtSearch).Size = new Size(380, 44);
		((Control)txtSearch).TabIndex = 4;
		txtSearch.OnTextChanged += txtSearch_OnTextChanged;
		txtSearch.OnKeyDown += txtSearch_OnKeyDown;
		((Control)ucGroupBox1).Anchor = (AnchorStyles)13;
		((Control)ucGroupBox1).BackColor = Color.Transparent;
		ucGroupBox1.DisplayFont = new Font("PeydaWeb Black", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ucGroupBox1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucGroupBox1.HeadHeight = 40;
		ucGroupBox1.IsShowHeader = false;
		((Control)ucGroupBox1).Location = new Point(4, 99);
		((Control)ucGroupBox1).Margin = new Padding(5);
		((Control)ucGroupBox1).Name = "ucGroupBox1";
		((Control)ucGroupBox1).RightToLeft = (RightToLeft)1;
		((Control)ucGroupBox1).Size = new Size(456, 77);
		((Control)ucGroupBox1).TabIndex = 5;
		ucGroupBox1.Title = "[عنوان]";
		((Control)btnDelete).Anchor = (AnchorStyles)9;
		((Control)btnDelete).BackColor = Color.FromArgb(249, 250, 255);
		btnDelete.DisableColor = Color.FromArgb(199, 205, 255);
		btnDelete.DisableImage = (Image)(object)FormCustom.Properties.Resources.DeleteAllRowsEnable;
		btnDelete.EnableColor = Color.FromArgb(65, 81, 219);
		btnDelete.EnableImage = (Image)(object)FormCustom.Properties.Resources.DeleteAllRowsHover;
		((Control)btnDelete).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnDelete.HoverColor = Color.FromArgb(30, 46, 75);
		btnDelete.HoverImage = (Image)(object)FormCustom.Properties.Resources.DeleteAllRowsHover2;
		((Control)btnDelete).Location = new Point(119, 104);
		((Control)btnDelete).Margin = new Padding(5);
		((Control)btnDelete).Name = "btnDelete";
		((Control)btnDelete).RightToLeft = (RightToLeft)1;
		((Control)btnDelete).Size = new Size(122, 56);
		((Control)btnDelete).TabIndex = 8;
		btnDelete.OnClick += btnDelete_OnClick;
		((Control)btnEdit).Anchor = (AnchorStyles)9;
		((Control)btnEdit).BackColor = Color.FromArgb(249, 250, 255);
		btnEdit.DisableColor = Color.FromArgb(199, 205, 255);
		btnEdit.DisableImage = (Image)(object)FormCustom.Properties.Resources.DescriptionEnable;
		btnEdit.EnableColor = Color.FromArgb(65, 81, 219);
		btnEdit.EnableImage = (Image)(object)FormCustom.Properties.Resources.DescriptionHover;
		((Control)btnEdit).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnEdit.HoverColor = Color.FromArgb(30, 46, 75);
		btnEdit.HoverImage = (Image)(object)FormCustom.Properties.Resources.DescriptionHover2;
		((Control)btnEdit).Location = new Point(241, 104);
		((Control)btnEdit).Margin = new Padding(5);
		((Control)btnEdit).Name = "btnEdit";
		((Control)btnEdit).RightToLeft = (RightToLeft)1;
		((Control)btnEdit).Size = new Size(99, 56);
		((Control)btnEdit).TabIndex = 7;
		btnEdit.OnClick += btnEdit_OnClick;
		((Control)btnAdd).Anchor = (AnchorStyles)9;
		((Control)btnAdd).BackColor = Color.FromArgb(249, 250, 255);
		btnAdd.DisableColor = Color.FromArgb(199, 205, 255);
		btnAdd.DisableImage = (Image)(object)FormCustom.Properties.Resources.AddEnable;
		btnAdd.EnableColor = Color.FromArgb(65, 81, 219);
		btnAdd.EnableImage = (Image)(object)FormCustom.Properties.Resources.AddHover;
		((Control)btnAdd).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnAdd.HoverColor = Color.FromArgb(30, 46, 75);
		btnAdd.HoverImage = (Image)(object)FormCustom.Properties.Resources.AddHover2;
		((Control)btnAdd).Location = new Point(340, 104);
		((Control)btnAdd).Margin = new Padding(5);
		((Control)btnAdd).Name = "btnAdd";
		((Control)btnAdd).RightToLeft = (RightToLeft)1;
		((Control)btnAdd).Size = new Size(112, 56);
		((Control)btnAdd).TabIndex = 6;
		btnAdd.OnClick += btnAdd_OnClick;
		((Control)btnSelect).Anchor = (AnchorStyles)9;
		((Control)btnSelect).BackColor = Color.FromArgb(249, 250, 255);
		btnSelect.DisableColor = Color.FromArgb(199, 205, 255);
		btnSelect.DisableImage = (Image)(object)FormCustom.Properties.Resources.ListManualEnable;
		btnSelect.EnableColor = Color.FromArgb(65, 81, 219);
		btnSelect.EnableImage = (Image)(object)FormCustom.Properties.Resources.ListManualHover;
		((Control)btnSelect).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnSelect.HoverColor = Color.FromArgb(30, 46, 75);
		btnSelect.HoverImage = (Image)(object)FormCustom.Properties.Resources.ListManualHover2;
		((Control)btnSelect).Location = new Point(12, 104);
		((Control)btnSelect).Margin = new Padding(5);
		((Control)btnSelect).Name = "btnSelect";
		((Control)btnSelect).RightToLeft = (RightToLeft)1;
		((Control)btnSelect).Size = new Size(112, 56);
		((Control)btnSelect).TabIndex = 9;
		btnSelect.OnClick += btnSelect_OnClick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(463, 720);
		((Control)this).Controls.Add((Control)(object)btnSelect);
		((Control)this).Controls.Add((Control)(object)btnDelete);
		((Control)this).Controls.Add((Control)(object)btnEdit);
		((Control)this).Controls.Add((Control)(object)btnAdd);
		((Control)this).Controls.Add((Control)(object)ucGroupBox1);
		((Control)this).Controls.Add((Control)(object)txtSearch);
		((Control)this).Controls.Add((Control)(object)DGrid);
		((Control)this).Controls.Add((Control)(object)btn_cancel);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmIntroductionMethod";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[نحوه آشنایی]";
		((Control)this).KeyDown += new KeyEventHandler(frmIntroductionMethod_KeyDown);
		((ISupportInitialize)IntroductionMethodBindingSource).EndInit();
		((ISupportInitialize)DGrid).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
