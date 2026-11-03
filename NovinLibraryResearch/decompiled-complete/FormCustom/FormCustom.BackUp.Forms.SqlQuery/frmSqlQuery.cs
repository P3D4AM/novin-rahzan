using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccSqlServerPersistence;
using AccSqlServerPersistence.AssistanceClasses;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.BackUp.Forms.SqlQuery;

public class frmSqlQuery : AccountingBaseForm
{
	private AccSqlServerPersistence.AssistanceClasses.SqlQuery cls;

	private string _connectionString;

	private ENSource _source;

	private bool _isDisposed;

	private IContainer components;

	internal Label LBLSearch;

	internal TextBox txtTitle;

	internal TextBox txtQuery;

	internal Label label1;

	private Button btnFinish;

	private Button btnCancel;

	private Button btnExecute;

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

	private void SetData()
	{
		try
		{
			((Control)txtTitle).Text = cls?.Title;
			((Control)txtQuery).Text = cls?.Query;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetData", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmSqlQuery.cs", 30);
		}
	}

	private async Task<ReturnedSaveFuncInfo> SaveDataAsync(string connectionString)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(((Control)txtQuery).Text))
			{
				res.AddError(EnSystem.None, "دستور SQL نمی تواند خالی باشد", "", "SaveDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmSqlQuery.cs", 40);
				return res;
			}
			cls.Query = ((Control)txtQuery).Text.FixString();
			cls.Title = ((Control)txtTitle).Text.FixString();
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await cls.SaveAsync(connectionString), ReturnedState.None, "SaveDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmSqlQuery.cs", 47);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmSqlQuery.cs", 51);
			res.AddReturnedValue(ex, "", "SaveDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmSqlQuery.cs", 52);
		}
		return res;
	}

	public frmSqlQuery(string contextGuid, ENSource source, string connectionString)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			_connectionString = connectionString;
			_source = source;
			cls = new AccSqlServerPersistence.AssistanceClasses.SqlQuery(BackupSettingsRepo.GetInstance(contextGuid), connectionString, source);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmSqlQuery.cs", 67);
		}
	}

	public frmSqlQuery(string contextGuid, AccSqlServerPersistence.AssistanceClasses.SqlQuery query, ENSource source, string connectionString)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			_connectionString = connectionString;
			_source = source;
			cls = query;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmSqlQuery.cs", 80);
		}
	}

	private void frmSqlQuery_Load(object sender, EventArgs e)
	{
		SetData();
	}

	private void txtTitle_Enter(object sender, EventArgs e)
	{
		((Control)txtTitle).BackColor = Color.Yellow;
	}

	private void txtQuery_Enter(object sender, EventArgs e)
	{
		((Control)txtQuery).BackColor = Color.Yellow;
	}

	private void txtQuery_Leave(object sender, EventArgs e)
	{
		((Control)txtQuery).BackColor = Color.White;
	}

	private void txtTitle_Leave(object sender, EventArgs e)
	{
		((Control)txtTitle).BackColor = Color.White;
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private void frmSqlQuery_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 27)
			{
				if ((int)keyCode != 116)
				{
					if ((int)keyCode == 121)
					{
						btnExecute.PerformClick();
					}
				}
				else
				{
					btnFinish.PerformClick();
				}
			}
			else
			{
				btnCancel.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmSqlQuery_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmSqlQuery.cs", 104);
		}
	}

	private async void btnFinish_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = await SaveDataAsync(_connectionString);
			if (returnedSaveFuncInfo.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(returnedSaveFuncInfo, "خطا در درج اطلاعات SQLQuery", "btnFinish_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmSqlQuery.cs");
				return;
			}
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnFinish_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmSqlQuery.cs", 121);
		}
	}

	private async void btnExecute_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SaveDataAsync(_connectionString), ReturnedState.None, "btnExecute_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmSqlQuery.cs", 129);
				if (ret.HasError)
				{
					goto end_IL_0042;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(base.ContextGuid), "customQuery_frmShowQuery", checkCrcBeforeExecution: false, _source, ((Control)txtQuery).Text, _connectionString), ReturnedState.None, "btnExecute_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmSqlQuery.cs", 131);
				goto end_IL_0033;
				end_IL_0042:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "btnExecute_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmSqlQuery.cs", 135);
				ret.AddReturnedValue(ex, "", "btnExecute_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmSqlQuery.cs", 136);
				goto end_IL_0033;
			}
			end_IL_0033:;
		}
		finally
		{
			if (ret.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(ret, "پیغام سیستم", "btnExecute_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmSqlQuery.cs");
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmSqlQuery.cs", 167);
		}
	}

	private void frmSqlQuery_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
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
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Expected O, but got Unknown
		LBLSearch = new Label();
		txtTitle = new TextBox();
		txtQuery = new TextBox();
		label1 = new Label();
		btnFinish = new Button();
		btnCancel = new Button();
		btnExecute = new Button();
		((Control)this).SuspendLayout();
		((Control)LBLSearch).Anchor = (AnchorStyles)9;
		((Control)LBLSearch).AutoSize = true;
		((Control)LBLSearch).Location = new Point(433, 15);
		((Control)LBLSearch).Name = "LBLSearch";
		((Control)LBLSearch).Size = new Size(37, 13);
		((Control)LBLSearch).TabIndex = 42;
		((Control)LBLSearch).Text = "[عنوان]:";
		((Control)txtTitle).Anchor = (AnchorStyles)13;
		((Control)txtTitle).Location = new Point(12, 12);
		((Control)txtTitle).Name = "txtTitle";
		((Control)txtTitle).Size = new Size(383, 20);
		((Control)txtTitle).TabIndex = 0;
		((Control)txtTitle).Enter += txtTitle_Enter;
		((Control)txtTitle).Leave += txtTitle_Leave;
		((Control)txtQuery).Anchor = (AnchorStyles)13;
		((Control)txtQuery).Location = new Point(12, 38);
		((TextBoxBase)txtQuery).Multiline = true;
		((Control)txtQuery).Name = "txtQuery";
		txtQuery.ScrollBars = (ScrollBars)2;
		((Control)txtQuery).Size = new Size(383, 363);
		((Control)txtQuery).TabIndex = 1;
		((Control)txtQuery).Enter += txtQuery_Enter;
		((Control)txtQuery).Leave += txtQuery_Leave;
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(407, 41);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(63, 13);
		((Control)label1).TabIndex = 42;
		((Control)label1).Text = "[دستور]SQL:";
		((Control)btnFinish).Location = new Point(273, 409);
		((Control)btnFinish).Name = "btnFinish";
		((Control)btnFinish).Size = new Size(123, 31);
		((Control)btnFinish).TabIndex = 2;
		((Control)btnFinish).Text = "[تایید](F5)";
		((ButtonBase)btnFinish).UseVisualStyleBackColor = true;
		((Control)btnFinish).Click += btnFinish_Click;
		((Control)btnCancel).Location = new Point(13, 409);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).Size = new Size(105, 31);
		((Control)btnCancel).TabIndex = 4;
		((Control)btnCancel).Text = "[انصراف](Esc)";
		((ButtonBase)btnCancel).UseVisualStyleBackColor = true;
		((Control)btnCancel).Click += btnCancel_Click;
		((Control)btnExecute).Location = new Point(124, 409);
		((Control)btnExecute).Name = "btnExecute";
		((Control)btnExecute).Size = new Size(143, 31);
		((Control)btnExecute).TabIndex = 3;
		((Control)btnExecute).Text = "[تایید و اجرای دستور](F10)";
		((ButtonBase)btnExecute).UseVisualStyleBackColor = true;
		((Control)btnExecute).Click += btnExecute_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(476, 450);
		((Control)this).Controls.Add((Control)(object)btnCancel);
		((Control)this).Controls.Add((Control)(object)btnExecute);
		((Control)this).Controls.Add((Control)(object)btnFinish);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)LBLSearch);
		((Control)this).Controls.Add((Control)(object)txtQuery);
		((Control)this).Controls.Add((Control)(object)txtTitle);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmSqlQuery";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).FormClosed += new FormClosedEventHandler(frmSqlQuery_FormClosed);
		((Form)this).Load += frmSqlQuery_Load;
		((Control)this).KeyDown += new KeyEventHandler(frmSqlQuery_KeyDown);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
