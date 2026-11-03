using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Properties;
using NPResourceManagement;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom;

public class FrmShowErrorMessage : AccountingBaseForm
{
	private static FrmShowErrorMessage _lastThreadFrm = null;

	private static Bitmap errorImage = new Bitmap((Image)(object)FormCustom.Properties.Resources.Error);

	private static Bitmap infoImage = new Bitmap((Image)(object)FormCustom.Properties.Resources.icons8_info_50);

	private static Bitmap warningImage = new Bitmap((Image)(object)FormCustom.Properties.Resources.icons8_warning_50);

	private static LanguageTypes _language1 = LanguageTypes.Farsi;

	private short rowIndex;

	private bool _isDisposed;

	private Dictionary<string, string> languageDic = new Dictionary<string, string>();

	private List<clsShowError> _errprlistfordisplay = new List<clsShowError>();

	private List<enType> _enlist = new List<enType>();

	private List<clsShowError> _errorList = new List<clsShowError>();

	private bool _errclick;

	private bool _warnclick;

	private bool _messageclic;

	private bool _btnerrorshowclick;

	private int _errorCount;

	private int _warningCount;

	private int _infoCount;

	private IContainer components;

	private BindingSource ErrorBindingSource;

	private Button btnCancel;

	private Button btnAbort;

	private Button btnContinue;

	private Button btnYes;

	private Button btnNo;

	private Button btnReturn;

	private Button btnShowMessages;

	private Button btnShowWarnings;

	private Button btnShowErrors;

	private Panel panel3;

	private Label lblCountMessage;

	private Label lblCountWarning;

	private Label LblCountErrors;

	private Label LBLWarnTitle;

	private Label LBLErrorTitle;

	private DataGridView dGrid;

	private Panel panelEx1;

	private Label labelX4;

	private Label labelX1;

	private Label lblSourceName;

	private FlowLayoutPanel flowLayoutPanel1;

	private Label LBLMessageTitle;

	private DataGridViewImageColumn dgIcon;

	private DataGridViewTextBoxColumn dgDesc;

	private DataGridViewTextBoxColumn dgMore;

	private DataGridViewTextBoxColumn dgText;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

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

	public clsShowError CurrentError { get; private set; }

	private LanguageTypes LanguageType
	{
		get
		{
			return _language1;
		}
		set
		{
			try
			{
				string[] array = value switch
				{
					LanguageTypes.Farsi => TextResourceManagement.TextLanguageFarsiTimeAttendance.Split(new char[1] { '\n' }), 
					LanguageTypes.Arbic => TextResourceManagement.TextLanguageArabicTimeAttendance.Split(new char[1] { '\n' }), 
					_ => throw new ArgumentOutOfRangeException("value", value, null), 
				};
				languageDic.Clear();
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string[] array3 = array2[i].Trim(new char[1] { '\r' }).Split(new char[1] { ',' });
					if (array3.Length == 2 && array3[0].Trim().Length > 0 && !languageDic.TryGetValue(array3[0].Trim(), out var _))
					{
						languageDic.Add(array3[0].Trim(), array3[1].Trim());
					}
				}
				TransateformControls();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "LanguageType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 67);
			}
			_language1 = value;
		}
	}

	private void TransateformControls()
	{
		try
		{
			string value = "";
			if (languageDic.TryGetValue(((DataGridViewColumn)dgDesc).HeaderText, out value))
			{
				((DataGridViewColumn)dgDesc).HeaderText = value;
			}
			if (languageDic.TryGetValue(((DataGridViewColumn)dgDesc).HeaderText, out value))
			{
				((DataGridViewColumn)dgDesc).HeaderText = value;
			}
			if (languageDic.TryGetValue(((Control)btnReturn).Text, out value))
			{
				((Control)btnReturn).Text = value;
			}
			if (languageDic.TryGetValue(((Control)LBLErrorTitle).Text, out value))
			{
				((Control)LBLErrorTitle).Text = value;
			}
			if (languageDic.TryGetValue(((Control)LBLWarnTitle).Text, out value))
			{
				((Control)LBLWarnTitle).Text = value;
			}
			if (languageDic.TryGetValue(((Control)LBLMessageTitle).Text, out value))
			{
				((Control)LBLMessageTitle).Text = value;
			}
			if (languageDic.TryGetValue(((Control)btnContinue).Text, out value))
			{
				((Control)btnContinue).Text = value;
			}
			if (languageDic.TryGetValue(((Control)btnYes).Text, out value))
			{
				((Control)btnYes).Text = value;
			}
			if (languageDic.TryGetValue(((Control)btnCancel).Text, out value))
			{
				((Control)btnCancel).Text = value;
			}
			if (languageDic.TryGetValue(((Control)labelX4).Text, out value))
			{
				((Control)labelX4).Text = value;
			}
			if (languageDic.TryGetValue(((Control)btnNo).Text, out value))
			{
				((Control)btnNo).Text = value;
			}
			if (languageDic.TryGetValue(((Control)btnAbort).Text, out value))
			{
				((Control)btnAbort).Text = value;
			}
			if (languageDic.TryGetValue(((Control)labelX1).Text, out value))
			{
				((Control)labelX1).Text = value;
			}
			if (languageDic.TryGetValue(((Control)this).Text, out value))
			{
				((Control)this).Text = value;
			}
			if (languageDic.TryGetValue(((Control)btnShowErrors).Text, out value))
			{
				((Control)btnShowErrors).Text = value;
			}
			if (languageDic.TryGetValue(((Control)btnShowWarnings).Text, out value))
			{
				((Control)btnShowWarnings).Text = value;
			}
			if (languageDic.TryGetValue(((Control)btnShowMessages).Text, out value))
			{
				((Control)btnShowMessages).Text = value;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TransateformControls", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 129);
		}
	}

	private void DgridInitializer()
	{
		try
		{
			dGrid.DefaultCellStyle.WrapMode = (DataGridViewTriState)1;
			dGrid.AutoSizeRowsMode = (DataGridViewAutoSizeRowsMode)7;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DgridInitializer", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 138);
		}
	}

	private void LoadErrors(List<clsShowError> errorList, bool isShowQuestion, CancellationToken token, List<enType> enlist)
	{
		int num = 100;
		try
		{
			bool isbreake = false;
			_errprlistfordisplay.Clear();
			enlist.ForEach(delegate(enType e)
			{
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b7: Expected O, but got Unknown
				//IL_011e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0128: Expected O, but got Unknown
				//IL_018c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0196: Expected O, but got Unknown
				if (!isbreake)
				{
					_errprlistfordisplay.AddRange(errorList.Where((clsShowError er) => er.Type == e));
					if (_errprlistfordisplay.Count > 0 && !_btnerrorshowclick)
					{
						switch (e)
						{
						case enType.Error:
							((ButtonBase)btnShowErrors).FlatAppearance.BorderSize = 2;
							((Control)btnShowErrors).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
							((Control)btnShowErrors).ForeColor = Color.White;
							((Control)btnShowErrors).BackColor = Color.MediumSlateBlue;
							_errclick = true;
							break;
						case enType.Warning:
							((ButtonBase)btnShowWarnings).FlatAppearance.BorderSize = 2;
							((Control)btnShowWarnings).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
							((Control)btnShowWarnings).ForeColor = Color.White;
							((Control)btnShowWarnings).BackColor = Color.MediumSlateBlue;
							_warnclick = true;
							break;
						case enType.Information:
							((ButtonBase)btnShowMessages).FlatAppearance.BorderSize = 2;
							((Control)btnShowMessages).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
							((Control)btnShowMessages).ForeColor = Color.White;
							((Control)btnShowMessages).BackColor = Color.MediumSlateBlue;
							_messageclic = true;
							break;
						}
						isbreake = true;
					}
				}
			});
			ErrorBindingSource.DataSource = _errprlistfordisplay.OrderBy((clsShowError e) => e.Type).ToSortableBindingList(token);
			num = 103;
			_errorCount = errorList?.Count((clsShowError q) => q.Type == enType.Error) ?? 0;
			_warningCount = errorList?.Count((clsShowError q) => q.Type == enType.Warning) ?? 0;
			_infoCount = errorList?.Count((clsShowError q) => q.Type == enType.Information) ?? 0;
			((Control)LblCountErrors).Text = _errorCount.ToString();
			((Control)lblCountWarning).Text = _warningCount.ToString();
			((Control)lblCountMessage).Text = _infoCount.ToString();
			num = 6;
			((Control)lblSourceName).Text = clsShowError.Source.GetDisplay().GetTranslate();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Error in Line {num}", "LoadErrors", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 212);
		}
	}

	private Task<List<clsShowError>> FillObjectAsync(CancellationToken token, ReturnedSaveFuncInfo pList = null, WebErrorLog wb = null)
	{
		List<clsShowError> list = new List<clsShowError>();
		try
		{
			if (pList != null)
			{
				if (token.IsCancellationRequested || IsDisposed)
				{
					return null;
				}
				foreach (string error in pList.ErrorList)
				{
					if (token.IsCancellationRequested || IsDisposed)
					{
						return null;
					}
					clsShowError clsShowError2 = new clsShowError
					{
						Type = enType.Error,
						Icon = (Image)(object)errorImage
					};
					clsShowError2.SetText(error);
					clsShowError2.SetTitle(error);
					list.Add(clsShowError2);
				}
				foreach (string warning in pList.WarningList)
				{
					if (token.IsCancellationRequested || IsDisposed)
					{
						return null;
					}
					clsShowError clsShowError3 = new clsShowError
					{
						Type = enType.Warning,
						Icon = (Image)(object)warningImage
					};
					clsShowError3.SetText(warning);
					clsShowError3.SetTitle(warning);
					list.Add(clsShowError3);
				}
				foreach (string information in pList.InformationList)
				{
					if (token.IsCancellationRequested || IsDisposed)
					{
						return null;
					}
					clsShowError clsShowError4 = new clsShowError
					{
						Type = enType.Information,
						Icon = (Image)(object)infoImage
					};
					clsShowError4.SetText(information);
					clsShowError4.SetTitle(information);
					list.Add(clsShowError4);
				}
			}
			if (token.IsCancellationRequested || IsDisposed)
			{
				return null;
			}
			if (wb != null)
			{
				clsShowError clsShowError5 = new clsShowError
				{
					Type = enType.Error,
					Icon = (Image)(object)errorImage
				};
				clsShowError5.SetTitle(wb.PersianMessageForUi);
				clsShowError5.SetTitle(wb.ExceptionMessage);
				list.Add(clsShowError5);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillObjectAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 274);
			list = null;
		}
		return Task.FromResult(list);
	}

	public static FrmShowErrorMessage GetThreadInstance(string context, WebErrorLog err, CancellationToken token, ILkUiFactory owner, LanguageTypes languageType = LanguageTypes.Farsi, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		int line = 0;
		try
		{
			line = 1;
			if (_lastThreadFrm != null && !_lastThreadFrm.IsDisposed && ((Control)_lastThreadFrm).Visible)
			{
				return _lastThreadFrm;
			}
			line = 2;
			_lastThreadFrm = new FrmShowErrorMessage(context);
			line = 3;
			Task.Run(delegate
			{
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Expected O, but got Unknown
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				try
				{
					line = 4;
					if (_lastThreadFrm != null && !_lastThreadFrm.IsDisposed && ((Control)_lastThreadFrm).Visible)
					{
						((Form)(object)_lastThreadFrm).CheckVisibility("GetThreadInstance", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs");
					}
					((Form)_lastThreadFrm).ShowDialog((IWin32Window)(Form)owner);
					line = 5;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "GetThreadInstance", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 302);
					((Component)(object)_lastThreadFrm)?.Dispose();
					_lastThreadFrm = null;
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "GetThreadInstance", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 310);
			((Component)(object)_lastThreadFrm)?.Dispose();
			_lastThreadFrm = null;
		}
		finally
		{
			_lastThreadFrm?.ShowErrorAsync(languageType, err, "Errors Report", (MessageBoxButtons)0, token, isAskQuestion: false, memberName, sourceFilePath);
		}
		return _lastThreadFrm;
	}

	private async Task ShowErrorAsync(LanguageTypes languageType, ReturnedSaveFuncInfo pList, string title, bool isAskQuestion, MessageBoxButtons boxButtons, string memberName, string sourceFilePath, CancellationToken token = default(CancellationToken))
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			_errorList = await FillObjectAsync(token, pList);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				//IL_017c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0182: Invalid comparison between Unknown and I4
				//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b0: Invalid comparison between Unknown and I4
				//IL_024c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0252: Invalid comparison between Unknown and I4
				//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_02e3: Invalid comparison between Unknown and I4
				//IL_034b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0351: Invalid comparison between Unknown and I4
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						((Control)this).Text = title;
						_enlist.Add(enType.Error);
						_enlist.Add(enType.Warning);
						_enlist.Add(enType.Information);
						LoadErrors(_errorList, isAskQuestion, token, _enlist);
						if (!isAskQuestion)
						{
							if ((int)boxButtons == 0)
							{
								if (_errorCount > 0)
								{
									((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnContinue);
								}
								else
								{
									((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnReturn);
								}
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnAbort);
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnCancel);
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnYes);
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnNo);
							}
							else if ((int)boxButtons == 2)
							{
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnReturn);
							}
							else if ((int)boxButtons == 1)
							{
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnAbort);
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnReturn);
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnYes);
								((Control)btnYes).Visible = false;
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnNo);
							}
							else if ((int)boxButtons == 4)
							{
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnAbort);
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnReturn);
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnCancel);
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnContinue);
							}
							else if ((int)boxButtons == 3)
							{
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnAbort);
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnReturn);
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnContinue);
							}
							else if ((int)boxButtons == 5)
							{
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnAbort);
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnYes);
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnNo);
								((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnContinue);
							}
						}
						else
						{
							((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnCancel);
							((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnAbort);
							((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnContinue);
							((Control)flowLayoutPanel1).Controls.Remove((Control)(object)btnReturn);
						}
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath, "ShowErrorAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 392);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowErrorAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 395);
		}
	}

	private async Task ShowErrorAsync(LanguageTypes languageType, WebErrorLog message, string title, MessageBoxButtons boxButtons, CancellationToken token, bool isAskQuestion, string memberName, string sourceFilePath)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		int line = 0;
		try
		{
			line = 1;
			while (!((Control)this).IsHandleCreated)
			{
				line = 2;
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
				line = 3;
				await Task.Delay(100);
			}
			line = 4;
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			line = 5;
			_errorList = await FillObjectAsync(token, null, message);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Invalid comparison between Unknown and I4
				try
				{
					line = 6;
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						line = 7;
						if ((int)boxButtons == 0)
						{
							Button obj = btnContinue;
							Button obj2 = btnCancel;
							Button obj3 = btnAbort;
							bool flag2 = (((Control)btnReturn).Visible = false);
							bool flag4 = (((Control)obj3).Visible = flag2);
							bool visible = (((Control)obj2).Visible = flag4);
							((Control)obj).Visible = visible;
						}
						if ((int)boxButtons != 2)
						{
							((Control)btnAbort).Visible = false;
						}
						line = 11;
						new List<enType>();
						_enlist.Add(enType.Error);
						_enlist.Add(enType.Warning);
						_enlist.Add(enType.Information);
						((ButtonBase)btnShowMessages).FlatAppearance.BorderSize = 2;
						((ButtonBase)btnShowErrors).FlatAppearance.BorderSize = 2;
						((ButtonBase)btnShowWarnings).FlatAppearance.BorderSize = 2;
						LoadErrors(_errorList, isAskQuestion, token, _enlist);
						line = 12;
						((Control)btnContinue).Enabled = false;
						line = 13;
						if (languageDic.TryGetValue(title, out var value))
						{
							title = value;
						}
						((Control)this).Text = title;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, $"FrmShowErrorMessage.ShowErrorAsync//Line:{line}\r\n" + "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath, "ShowErrorAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 443);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"222//Line:{line}\r\n" + "MemberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath, "ShowErrorAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 451);
		}
	}

	private void OpenUrl(string msg)
	{
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Invalid comparison between Unknown and I4
		try
		{
			if (!msg.Contains("http"))
			{
				return;
			}
			MatchCollection matchCollection = new Regex("(http|ftp|https)://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase).Matches(msg);
			string text = "";
			foreach (Match item in matchCollection)
			{
				text = item.Value;
			}
			if ((int)MessageBox.Show((IWin32Window)(object)this, "آیا مایلید صفحه".GetTranslate() + "   " + text + " " + " را باز کنید؟".GetTranslate(), text ?? "", (MessageBoxButtons)4, (MessageBoxIcon)32) == 6)
			{
				Process.Start(text);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OpenUrl", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 469);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 480);
		}
	}

	public FrmShowErrorMessage(string context, ReturnedSaveFuncInfo pList, string title, MessageBoxButtons boxButtons = 0, bool isAskQuestion = false, LanguageTypes languageType = LanguageTypes.Farsi, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
		: base(context)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		FrmShowErrorMessage frmShowErrorMessage = this;
		try
		{
			InitializeComponent();
			DgridInitializer();
			LanguageType = languageType;
			CancellationToken token = new CancellationTokenSource(60000).Token;
			Task.Run(() => frmShowErrorMessage.ShowErrorAsync(languageType, pList, title, isAskQuestion, boxButtons, memberName, sourceFilePath, token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nSourceFilePath:" + sourceFilePath, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 495);
		}
	}

	public FrmShowErrorMessage(string context, WebErrorLog message, string title, MessageBoxButtons boxButtons = 0, CancellationToken token = default(CancellationToken), bool isAskQuestion = false, LanguageTypes languageType = LanguageTypes.Farsi, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
		: base(context)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			InitializeComponent();
			DgridInitializer();
			LanguageType = languageType;
			ShowErrorAsync(languageType, message, title, boxButtons, token, isAskQuestion, memberName, sourceFilePath);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nSourceFilePath:" + sourceFilePath, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 510);
		}
	}

	public FrmShowErrorMessage(string context)
		: base(context)
	{
		try
		{
			InitializeComponent();
			DgridInitializer();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 523);
		}
	}

	public FrmShowErrorMessage(string context, string message, string title = "System Message", MessageBoxButtons boxButtons = 0, bool isAskQuestion = false, LanguageTypes languageType = LanguageTypes.Farsi, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
		: base(context)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (languageDic.TryGetValue(title, out var value))
			{
				title = value;
			}
			InitializeComponent();
			DgridInitializer();
			LanguageType = languageType;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
			returnedSaveFuncInfo.AddInformation(message, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 536);
			CancellationToken token = new CancellationTokenSource(60000).Token;
			ShowErrorAsync(languageType, returnedSaveFuncInfo, title, isAskQuestion, boxButtons, memberName, sourceFilePath, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nSourceFilePath:" + sourceFilePath, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 542);
		}
	}

	private void btnReturn_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)4;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnReturn_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 554);
		}
	}

	private void btnContinue_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)5;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnContinue_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 563);
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 573);
		}
	}

	private void btnCancelAll_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)3;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnCancelAll_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 583);
		}
	}

	private void FrmShowErrorMessage_KeyDown(object sender, KeyEventArgs e)
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 27)
			{
				if ((int)keyCode != 13)
				{
					if ((int)keyCode == 27)
					{
						((Form)this).DialogResult = (DialogResult)2;
						((Form)this).Close();
					}
				}
				else if (((Control)btnYes).Visible)
				{
					btnYes.PerformClick();
				}
				else
				{
					btnContinue.PerformClick();
				}
			}
			else if ((int)keyCode != 69)
			{
				if ((int)keyCode != 73)
				{
					if ((int)keyCode == 87)
					{
						btnShowWarnings.PerformClick();
					}
				}
				else
				{
					btnShowMessages.PerformClick();
				}
			}
			else
			{
				btnShowErrors.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FrmShowErrorMessage_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 611);
		}
	}

	private void dGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (CurrentError == null)
			{
				return;
			}
			if (e.ColumnIndex == ((DataGridViewBand)dgMore).Index)
			{
				if (e.RowIndex == rowIndex)
				{
					rowIndex = (short)(dGrid.RowCount + 1);
				}
				else
				{
					rowIndex = (short)e.RowIndex;
				}
				frmShowErrorDetails obj = new frmShowErrorDetails(LanguageType, CurrentError.FullText.GetTranslate(), ((Form)this).WindowState, base.ContextGuid);
				((Form)(object)this).CheckVisibility("dGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs");
				((Form)obj).ShowDialog((IWin32Window)(object)this);
			}
			else if (e.ColumnIndex == ((DataGridViewBand)dgIcon).Index || e.ColumnIndex == ((DataGridViewBand)dgDesc).Index)
			{
				OpenUrl(CurrentError.FullText);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "dGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 630);
		}
	}

	private void dGrid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == ((DataGridViewBand)dgMore).Index)
			{
				DataGridViewCell obj = dGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
				string text = "Details view";
				if (languageDic.TryGetValue(text, out var value))
				{
					text = value;
				}
				obj.ToolTipText = text;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "dGrid_CellMouseEnter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 645);
		}
	}

	private void btnYes_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)6;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnYes_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 654);
		}
	}

	private void btnNo_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)7;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnNo_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 663);
		}
	}

	private void FrmShowErrorMessage_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private async void FrmShowErrorMessage_Load(object sender, EventArgs e)
	{
		try
		{
			await Task.Delay(500);
			((Control)this).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FrmShowErrorMessage_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 673);
		}
	}

	private void btnShowErrors_Click(object sender, EventArgs e)
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			_errclick = !_errclick;
			_btnerrorshowclick = true;
			if (_errclick)
			{
				if (!checkExistENType(enType.Error))
				{
					_enlist.Add(enType.Error);
				}
				((ButtonBase)btnShowErrors).FlatAppearance.BorderSize = 2;
				((Control)btnShowErrors).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
				((Control)btnShowErrors).ForeColor = Color.White;
				((Control)btnShowErrors).BackColor = Color.MediumSlateBlue;
			}
			else
			{
				if (checkExistENType(enType.Error))
				{
					_enlist.Remove(enType.Error);
				}
				((ButtonBase)btnShowErrors).FlatAppearance.BorderSize = 1;
				((Control)btnShowErrors).Font = new Font("PeydaWeb ExtBd", 7f, (FontStyle)1);
				((Control)btnShowErrors).ForeColor = Color.Black;
				((Control)btnShowErrors).BackColor = SystemColors.ButtonFace;
			}
			((ContainerControl)this).ActiveControl = null;
			CancellationToken token = new CancellationTokenSource(60000).Token;
			LoadErrors(_errorList, isShowQuestion: false, token, _enlist);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnShowErrors_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 701);
		}
	}

	private void ErrorBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			CurrentError = (clsShowError)ErrorBindingSource.Current;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ErrorBindingSource_CurrentChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 709);
		}
	}

	private void btnShowWarnings_Click(object sender, EventArgs e)
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			_warnclick = !_warnclick;
			_btnerrorshowclick = true;
			if (_warnclick)
			{
				if (!checkExistENType(enType.Warning))
				{
					_enlist.Add(enType.Warning);
				}
				((ButtonBase)btnShowWarnings).FlatAppearance.BorderSize = 2;
				((Control)btnShowWarnings).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
				((Control)btnShowWarnings).ForeColor = Color.White;
				((Control)btnShowWarnings).BackColor = Color.MediumSlateBlue;
			}
			else
			{
				if (checkExistENType(enType.Warning))
				{
					_enlist.Remove(enType.Warning);
				}
				((ButtonBase)btnShowWarnings).FlatAppearance.BorderSize = 1;
				((Control)btnShowWarnings).Font = new Font("PeydaWeb ExtBd", 7f, (FontStyle)1);
				((Control)btnShowWarnings).ForeColor = Color.Black;
				((Control)btnShowWarnings).BackColor = SystemColors.ButtonFace;
			}
			((ContainerControl)this).ActiveControl = null;
			CancellationToken token = new CancellationTokenSource(60000).Token;
			LoadErrors(_errorList, isShowQuestion: false, token, _enlist);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnShowWarnings_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 737);
		}
	}

	private void btnShowMessages_Click(object sender, EventArgs e)
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			_messageclic = !_messageclic;
			_btnerrorshowclick = true;
			if (_messageclic)
			{
				if (!checkExistENType(enType.Information))
				{
					_enlist.Add(enType.Information);
				}
				((ButtonBase)btnShowMessages).FlatAppearance.BorderSize = 2;
				((Control)btnShowMessages).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
				((Control)btnShowMessages).ForeColor = Color.White;
				((Control)btnShowMessages).BackColor = Color.MediumSlateBlue;
			}
			else
			{
				if (checkExistENType(enType.Information))
				{
					_enlist.Remove(enType.Information);
				}
				((ButtonBase)btnShowMessages).FlatAppearance.BorderSize = 1;
				((Control)btnShowMessages).Font = new Font("PeydaWeb ExtBd", 7f, (FontStyle)1);
				((Control)btnShowMessages).ForeColor = Color.Black;
				((Control)btnShowMessages).BackColor = SystemColors.ButtonFace;
			}
			((ContainerControl)this).ActiveControl = null;
			CancellationToken token = new CancellationTokenSource(60000).Token;
			LoadErrors(_errorList, isShowQuestion: false, token, _enlist);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnShowMessages_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 765);
		}
	}

	private void btn_OnHover(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Button val = (Button)sender;
			((Control)val).ForeColor = Color.White;
			((Control)val).BackColor = Color.MediumSlateBlue;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btn_OnHover", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 775);
		}
	}

	private void btn_LostHover(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Button val = (Button)sender;
			((Control)val).ForeColor = Color.Black;
			((Control)val).BackColor = SystemColors.ButtonFace;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btn_LostHover", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 785);
		}
	}

	private bool checkExistENType(enType en)
	{
		try
		{
			if (_enlist.Contains(en))
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "checkExistENType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmShowErrorMessage.cs", 794);
			return false;
		}
	}

	private void InitializeComponent()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
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
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Expected O, but got Unknown
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Expected O, but got Unknown
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Expected O, but got Unknown
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Expected O, but got Unknown
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Expected O, but got Unknown
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_0732: Expected O, but got Unknown
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_074d: Expected O, but got Unknown
		//IL_0861: Unknown result type (might be due to invalid IL or missing references)
		//IL_086b: Expected O, but got Unknown
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0886: Expected O, but got Unknown
		//IL_099d: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a7: Expected O, but got Unknown
		//IL_09f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa2: Expected O, but got Unknown
		//IL_0ab3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abd: Expected O, but got Unknown
		//IL_0afb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bab: Expected O, but got Unknown
		//IL_0bf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f01: Expected O, but got Unknown
		//IL_0f90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9a: Expected O, but got Unknown
		//IL_102c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1036: Expected O, but got Unknown
		//IL_10c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d2: Expected O, but got Unknown
		//IL_1147: Unknown result type (might be due to invalid IL or missing references)
		//IL_1151: Expected O, but got Unknown
		//IL_11c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d0: Expected O, but got Unknown
		//IL_1289: Unknown result type (might be due to invalid IL or missing references)
		//IL_1293: Expected O, but got Unknown
		//IL_1348: Unknown result type (might be due to invalid IL or missing references)
		//IL_1352: Expected O, but got Unknown
		//IL_13f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fc: Expected O, but got Unknown
		//IL_14ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b6: Expected O, but got Unknown
		//IL_14c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_14cd: Expected O, but got Unknown
		//IL_1540: Unknown result type (might be due to invalid IL or missing references)
		//IL_154a: Expected O, but got Unknown
		//IL_15f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1600: Expected O, but got Unknown
		//IL_17f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1891: Unknown result type (might be due to invalid IL or missing references)
		//IL_189b: Expected O, but got Unknown
		//IL_18ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b8: Expected O, but got Unknown
		//IL_1903: Unknown result type (might be due to invalid IL or missing references)
		//IL_190d: Expected O, but got Unknown
		//IL_1927: Unknown result type (might be due to invalid IL or missing references)
		//IL_1931: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmShowErrorMessage));
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		DataGridViewCellStyle val5 = new DataGridViewCellStyle();
		DataGridViewCellStyle val6 = new DataGridViewCellStyle();
		ErrorBindingSource = new BindingSource(components);
		btnCancel = new Button();
		btnAbort = new Button();
		btnContinue = new Button();
		btnYes = new Button();
		btnNo = new Button();
		btnReturn = new Button();
		btnShowMessages = new Button();
		btnShowWarnings = new Button();
		btnShowErrors = new Button();
		panel3 = new Panel();
		lblSourceName = new Label();
		labelX1 = new Label();
		labelX4 = new Label();
		lblCountMessage = new Label();
		lblCountWarning = new Label();
		LblCountErrors = new Label();
		LBLMessageTitle = new Label();
		LBLWarnTitle = new Label();
		LBLErrorTitle = new Label();
		dGrid = new DataGridView();
		dgIcon = new DataGridViewImageColumn();
		dgDesc = new DataGridViewTextBoxColumn();
		dgMore = new DataGridViewTextBoxColumn();
		dgText = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
		panelEx1 = new Panel();
		flowLayoutPanel1 = new FlowLayoutPanel();
		((ISupportInitialize)ErrorBindingSource).BeginInit();
		((Control)panel3).SuspendLayout();
		((ISupportInitialize)dGrid).BeginInit();
		((Control)panelEx1).SuspendLayout();
		((Control)flowLayoutPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		ErrorBindingSource.AllowNew = false;
		ErrorBindingSource.DataSource = typeof(clsShowError);
		ErrorBindingSource.CurrentChanged += ErrorBindingSource_CurrentChanged;
		((Control)btnCancel).BackColor = SystemColors.ButtonFace;
		((Control)btnCancel).Cursor = Cursors.Hand;
		((ButtonBase)btnCancel).FlatAppearance.BorderColor = Color.MediumSlateBlue;
		((Control)btnCancel).Font = new Font("PeydaWeb ExtBd", 8.3f, (FontStyle)1);
		((ButtonBase)btnCancel).Image = (Image)componentResourceManager.GetObject("btnCancel.Image");
		((ButtonBase)btnCancel).ImageAlign = (ContentAlignment)16;
		((Control)btnCancel).Location = new Point(3, 3);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).Padding = new Padding(0, 0, 10, 0);
		((Control)btnCancel).RightToLeft = (RightToLeft)0;
		((Control)btnCancel).Size = new Size(104, 42);
		((Control)btnCancel).TabIndex = 9;
		((Control)btnCancel).Text = "[لغو](ESC)";
		((ButtonBase)btnCancel).TextAlign = (ContentAlignment)64;
		((ButtonBase)btnCancel).UseVisualStyleBackColor = false;
		((Control)btnCancel).Click += btnCancel_Click;
		((Control)btnCancel).MouseLeave += btn_LostHover;
		((Control)btnCancel).MouseHover += btn_OnHover;
		((Control)btnAbort).BackColor = SystemColors.ButtonFace;
		((Control)btnAbort).Cursor = Cursors.Hand;
		((ButtonBase)btnAbort).FlatAppearance.BorderColor = Color.MediumSlateBlue;
		((Control)btnAbort).Font = new Font("PeydaWeb ExtBd", 8.3f, (FontStyle)1);
		((ButtonBase)btnAbort).Image = (Image)componentResourceManager.GetObject("btnAbort.Image");
		((ButtonBase)btnAbort).ImageAlign = (ContentAlignment)16;
		((Control)btnAbort).Location = new Point(232, 3);
		((Control)btnAbort).Name = "btnAbort";
		((Control)btnAbort).RightToLeft = (RightToLeft)0;
		((Control)btnAbort).Size = new Size(90, 42);
		((Control)btnAbort).TabIndex = 11;
		((Control)btnAbort).Text = "[لغو همه]";
		((ButtonBase)btnAbort).TextAlign = (ContentAlignment)64;
		((ButtonBase)btnAbort).UseVisualStyleBackColor = false;
		((Control)btnAbort).Click += btnCancelAll_Click;
		((Control)btnAbort).MouseLeave += btn_LostHover;
		((Control)btnAbort).MouseHover += btn_OnHover;
		((Control)btnContinue).BackColor = SystemColors.ButtonFace;
		((Control)btnContinue).Cursor = Cursors.Hand;
		((ButtonBase)btnContinue).FlatAppearance.BorderColor = Color.MediumSlateBlue;
		((Control)btnContinue).Font = new Font("PeydaWeb ExtBd", 8.3f, (FontStyle)1);
		((ButtonBase)btnContinue).Image = (Image)componentResourceManager.GetObject("btnContinue.Image");
		((ButtonBase)btnContinue).ImageAlign = (ContentAlignment)16;
		((Control)btnContinue).Location = new Point(113, 3);
		((Control)btnContinue).Name = "btnContinue";
		((Control)btnContinue).RightToLeft = (RightToLeft)0;
		((Control)btnContinue).Size = new Size(113, 42);
		((Control)btnContinue).TabIndex = 8;
		((Control)btnContinue).Text = "[ادامه](Enter)";
		((ButtonBase)btnContinue).TextAlign = (ContentAlignment)64;
		((ButtonBase)btnContinue).UseVisualStyleBackColor = false;
		((Control)btnContinue).Click += btnContinue_Click;
		((Control)btnContinue).MouseLeave += btn_LostHover;
		((Control)btnContinue).MouseHover += btn_OnHover;
		((Control)btnYes).BackColor = SystemColors.ButtonFace;
		((Control)btnYes).Cursor = Cursors.Hand;
		((ButtonBase)btnYes).FlatAppearance.BorderColor = Color.MediumSlateBlue;
		((Control)btnYes).Font = new Font("PeydaWeb ExtBd", 8.3f, (FontStyle)1);
		((ButtonBase)btnYes).Image = (Image)componentResourceManager.GetObject("btnYes.Image");
		((ButtonBase)btnYes).ImageAlign = (ContentAlignment)16;
		((Control)btnYes).Location = new Point(398, 3);
		((Control)btnYes).Name = "btnYes";
		((Control)btnYes).RightToLeft = (RightToLeft)0;
		((Control)btnYes).Size = new Size(59, 42);
		((Control)btnYes).TabIndex = 12;
		((Control)btnYes).Text = "[بله]";
		((ButtonBase)btnYes).TextAlign = (ContentAlignment)64;
		((ButtonBase)btnYes).UseVisualStyleBackColor = false;
		((Control)btnYes).Click += btnYes_Click;
		((Control)btnYes).MouseLeave += btn_LostHover;
		((Control)btnYes).MouseHover += btn_OnHover;
		((Control)btnNo).BackColor = SystemColors.ButtonFace;
		((Control)btnNo).Cursor = Cursors.Hand;
		((ButtonBase)btnNo).FlatAppearance.BorderColor = Color.MediumSlateBlue;
		((Control)btnNo).Font = new Font("PeydaWeb ExtBd", 8.3f, (FontStyle)1);
		((ButtonBase)btnNo).Image = (Image)componentResourceManager.GetObject("btnNo.Image");
		((ButtonBase)btnNo).ImageAlign = (ContentAlignment)16;
		((Control)btnNo).Location = new Point(328, 3);
		((Control)btnNo).Name = "btnNo";
		((Control)btnNo).RightToLeft = (RightToLeft)0;
		((Control)btnNo).Size = new Size(64, 42);
		((Control)btnNo).TabIndex = 13;
		((Control)btnNo).Text = "[خیر]";
		((ButtonBase)btnNo).TextAlign = (ContentAlignment)64;
		((ButtonBase)btnNo).UseVisualStyleBackColor = false;
		((Control)btnNo).Click += btnNo_Click;
		((Control)btnNo).MouseLeave += btn_LostHover;
		((Control)btnNo).MouseHover += btn_OnHover;
		((Control)btnReturn).BackColor = SystemColors.ButtonFace;
		((Control)btnReturn).Cursor = Cursors.Hand;
		((ButtonBase)btnReturn).FlatAppearance.BorderColor = Color.MediumSlateBlue;
		((Control)btnReturn).Font = new Font("PeydaWeb ExtBd", 8.3f, (FontStyle)1);
		((ButtonBase)btnReturn).Image = (Image)componentResourceManager.GetObject("btnReturn.Image");
		((ButtonBase)btnReturn).ImageAlign = (ContentAlignment)16;
		((Control)btnReturn).Location = new Point(463, 3);
		((Control)btnReturn).Name = "btnReturn";
		((Control)btnReturn).RightToLeft = (RightToLeft)0;
		((Control)btnReturn).Size = new Size(157, 42);
		((Control)btnReturn).TabIndex = 14;
		((Control)btnReturn).Text = "[بازگشت و رفع خطاها]";
		((ButtonBase)btnReturn).TextAlign = (ContentAlignment)64;
		((ButtonBase)btnReturn).UseVisualStyleBackColor = false;
		((Control)btnReturn).Click += btnReturn_Click;
		((Control)btnReturn).MouseLeave += btn_LostHover;
		((Control)btnReturn).MouseHover += btn_OnHover;
		((Control)btnShowMessages).BackColor = SystemColors.ButtonFace;
		((Control)btnShowMessages).Cursor = Cursors.Hand;
		((ButtonBase)btnShowMessages).FlatAppearance.BorderColor = Color.MediumSlateBlue;
		((Control)btnShowMessages).Font = new Font("PeydaWeb ExtBd", 7f, (FontStyle)1);
		((ButtonBase)btnShowMessages).Image = (Image)(object)FormCustom.Properties.Resources.Info;
		((ButtonBase)btnShowMessages).ImageAlign = (ContentAlignment)16;
		((Control)btnShowMessages).Location = new Point(78, 5);
		((Control)btnShowMessages).Name = "btnShowMessages";
		((Control)btnShowMessages).Padding = new Padding(0, 0, 20, 0);
		((Control)btnShowMessages).Size = new Size(139, 42);
		((Control)btnShowMessages).TabIndex = 7;
		((Control)btnShowMessages).Text = "[پیام ها]  ( i )";
		((ButtonBase)btnShowMessages).UseVisualStyleBackColor = false;
		((Control)btnShowMessages).Click += btnShowMessages_Click;
		((Control)btnShowWarnings).BackColor = SystemColors.ButtonFace;
		((Control)btnShowWarnings).Cursor = Cursors.Hand;
		((ButtonBase)btnShowWarnings).FlatAppearance.BorderColor = Color.MediumSlateBlue;
		((Control)btnShowWarnings).Font = new Font("PeydaWeb ExtBd", 7f, (FontStyle)1);
		((ButtonBase)btnShowWarnings).Image = (Image)componentResourceManager.GetObject("btnShowWarnings.Image");
		((ButtonBase)btnShowWarnings).ImageAlign = (ContentAlignment)16;
		((Control)btnShowWarnings).Location = new Point(240, 5);
		((Control)btnShowWarnings).Name = "btnShowWarnings";
		((Control)btnShowWarnings).Padding = new Padding(0, 0, 15, 0);
		((Control)btnShowWarnings).Size = new Size(139, 42);
		((Control)btnShowWarnings).TabIndex = 6;
		((Control)btnShowWarnings).Text = "[هشدارها](W)";
		((ButtonBase)btnShowWarnings).UseVisualStyleBackColor = false;
		((Control)btnShowWarnings).Click += btnShowWarnings_Click;
		((Control)btnShowErrors).BackColor = SystemColors.ButtonFace;
		((Control)btnShowErrors).Cursor = Cursors.Hand;
		((ButtonBase)btnShowErrors).FlatAppearance.BorderColor = Color.MediumSlateBlue;
		((Control)btnShowErrors).Font = new Font("PeydaWeb ExtBd", 7f, (FontStyle)1);
		((ButtonBase)btnShowErrors).Image = (Image)(object)FormCustom.Properties.Resources.stop;
		((ButtonBase)btnShowErrors).ImageAlign = (ContentAlignment)16;
		((Control)btnShowErrors).Location = new Point(400, 5);
		((Control)btnShowErrors).Name = "btnShowErrors";
		((Control)btnShowErrors).Padding = new Padding(0, 0, 20, 0);
		((Control)btnShowErrors).Size = new Size(139, 42);
		((Control)btnShowErrors).TabIndex = 5;
		((Control)btnShowErrors).Text = "[خطاها](E)";
		((ButtonBase)btnShowErrors).UseVisualStyleBackColor = false;
		((Control)btnShowErrors).Click += btnShowErrors_Click;
		panel3.BorderStyle = (BorderStyle)2;
		((Control)panel3).Controls.Add((Control)(object)lblSourceName);
		((Control)panel3).Controls.Add((Control)(object)labelX1);
		((Control)panel3).Controls.Add((Control)(object)labelX4);
		((Control)panel3).Controls.Add((Control)(object)lblCountMessage);
		((Control)panel3).Controls.Add((Control)(object)lblCountWarning);
		((Control)panel3).Controls.Add((Control)(object)LblCountErrors);
		((Control)panel3).Controls.Add((Control)(object)LBLMessageTitle);
		((Control)panel3).Controls.Add((Control)(object)LBLWarnTitle);
		((Control)panel3).Controls.Add((Control)(object)LBLErrorTitle);
		((Control)panel3).Controls.Add((Control)(object)btnShowErrors);
		((Control)panel3).Controls.Add((Control)(object)btnShowWarnings);
		((Control)panel3).Controls.Add((Control)(object)btnShowMessages);
		((Control)panel3).Location = new Point(8, 12);
		((Control)panel3).Name = "panel3";
		((Control)panel3).Size = new Size(633, 72);
		((Control)panel3).TabIndex = 14;
		((Control)lblSourceName).AutoSize = true;
		((Control)lblSourceName).Location = new Point(191, 237);
		((Control)lblSourceName).Name = "lblSourceName";
		((Control)lblSourceName).Size = new Size(0, 13);
		((Control)lblSourceName).TabIndex = 16;
		((Control)lblSourceName).Visible = false;
		((Control)labelX1).AutoSize = true;
		((Control)labelX1).Location = new Point(157, 313);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(0, 13);
		((Control)labelX1).TabIndex = 15;
		((Control)labelX1).Visible = false;
		((Control)labelX4).AutoSize = true;
		((Control)labelX4).Location = new Point(157, 256);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(0, 13);
		((Control)labelX4).TabIndex = 14;
		((Control)labelX4).Visible = false;
		((Control)lblCountMessage).AutoSize = true;
		((Control)lblCountMessage).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
		((Control)lblCountMessage).ForeColor = Color.DodgerBlue;
		((Control)lblCountMessage).Location = new Point(78, 50);
		((Control)lblCountMessage).Name = "lblCountMessage";
		((Control)lblCountMessage).Size = new Size(16, 18);
		((Control)lblCountMessage).TabIndex = 13;
		((Control)lblCountMessage).Text = "0";
		lblCountMessage.TextAlign = (ContentAlignment)32;
		((Control)lblCountWarning).AutoSize = true;
		((Control)lblCountWarning).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
		((Control)lblCountWarning).ForeColor = Color.DarkOrange;
		((Control)lblCountWarning).Location = new Point(240, 50);
		((Control)lblCountWarning).Name = "lblCountWarning";
		((Control)lblCountWarning).Size = new Size(16, 18);
		((Control)lblCountWarning).TabIndex = 12;
		((Control)lblCountWarning).Text = "0";
		lblCountWarning.TextAlign = (ContentAlignment)32;
		((Control)LblCountErrors).AutoSize = true;
		((Control)LblCountErrors).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
		((Control)LblCountErrors).ForeColor = Color.OrangeRed;
		((Control)LblCountErrors).Location = new Point(400, 50);
		((Control)LblCountErrors).Name = "LblCountErrors";
		((Control)LblCountErrors).Size = new Size(16, 18);
		((Control)LblCountErrors).TabIndex = 11;
		((Control)LblCountErrors).Text = "0";
		LblCountErrors.TextAlign = (ContentAlignment)32;
		((Control)LBLMessageTitle).AutoSize = true;
		((Control)LBLMessageTitle).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
		((Control)LBLMessageTitle).Location = new Point(152, 50);
		((Control)LBLMessageTitle).Name = "LBLMessageTitle";
		((Control)LBLMessageTitle).Size = new Size(68, 18);
		((Control)LBLMessageTitle).TabIndex = 10;
		((Control)LBLMessageTitle).Text = "[تعداد پیام] : ";
		((Control)LBLWarnTitle).AutoSize = true;
		((Control)LBLWarnTitle).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
		((Control)LBLWarnTitle).Location = new Point(301, 50);
		((Control)LBLWarnTitle).Name = "LBLWarnTitle";
		((Control)LBLWarnTitle).Size = new Size(81, 18);
		((Control)LBLWarnTitle).TabIndex = 9;
		((Control)LBLWarnTitle).Text = "[تعداد هشدار]:";
		((Control)LBLErrorTitle).AutoSize = true;
		((Control)LBLErrorTitle).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
		((Control)LBLErrorTitle).Location = new Point(475, 50);
		((Control)LBLErrorTitle).Name = "LBLErrorTitle";
		((Control)LBLErrorTitle).Size = new Size(67, 18);
		((Control)LBLErrorTitle).TabIndex = 8;
		((Control)LBLErrorTitle).Text = "[تعداد خطا]:";
		dGrid.AllowUserToAddRows = false;
		dGrid.AllowUserToDeleteRows = false;
		((Control)dGrid).Anchor = (AnchorStyles)15;
		dGrid.AutoGenerateColumns = false;
		dGrid.BackgroundColor = Color.White;
		val.Alignment = (DataGridViewContentAlignment)32;
		val.BackColor = Color.White;
		val.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val.ForeColor = SystemColors.WindowText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.HighlightText;
		val.WrapMode = (DataGridViewTriState)1;
		dGrid.ColumnHeadersDefaultCellStyle = val;
		dGrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		dGrid.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[5]
		{
			(DataGridViewColumn)dgIcon,
			(DataGridViewColumn)dgDesc,
			(DataGridViewColumn)dgMore,
			(DataGridViewColumn)dgText,
			(DataGridViewColumn)dataGridViewTextBoxColumn4
		});
		dGrid.DataSource = ErrorBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = Color.White;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = Color.Black;
		val2.SelectionBackColor = Color.Silver;
		val2.SelectionForeColor = Color.Black;
		val2.WrapMode = (DataGridViewTriState)2;
		dGrid.DefaultCellStyle = val2;
		((Control)dGrid).Location = new Point(9, 90);
		dGrid.MultiSelect = false;
		((Control)dGrid).Name = "dGrid";
		dGrid.ReadOnly = true;
		val3.Alignment = (DataGridViewContentAlignment)16;
		val3.BackColor = Color.FromArgb(255, 192, 128);
		val3.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val3.ForeColor = SystemColors.WindowText;
		val3.SelectionBackColor = Color.Silver;
		val3.SelectionForeColor = Color.Black;
		val3.WrapMode = (DataGridViewTriState)1;
		dGrid.RowHeadersDefaultCellStyle = val3;
		dGrid.RowHeadersVisible = false;
		val4.BackColor = Color.White;
		val4.SelectionBackColor = Color.Silver;
		val4.SelectionForeColor = Color.Black;
		dGrid.RowsDefaultCellStyle = val4;
		dGrid.SelectionMode = (DataGridViewSelectionMode)1;
		((Control)dGrid).Size = new Size(633, 263);
		((Control)dGrid).TabIndex = 1;
		dGrid.CellClick += new DataGridViewCellEventHandler(dGrid_CellClick);
		dGrid.CellMouseEnter += new DataGridViewCellEventHandler(dGrid_CellMouseEnter);
		((DataGridViewColumn)dgIcon).DataPropertyName = "Icon";
		((DataGridViewColumn)dgIcon).HeaderText = "";
		((DataGridViewColumn)dgIcon).Name = "dgIcon";
		((DataGridViewBand)dgIcon).ReadOnly = true;
		((DataGridViewColumn)dgIcon).Width = 50;
		((DataGridViewColumn)dgDesc).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)dgDesc).DataPropertyName = "Title";
		val5.Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
		((DataGridViewBand)dgDesc).DefaultCellStyle = val5;
		((DataGridViewColumn)dgDesc).HeaderText = "[شرح]";
		((DataGridViewColumn)dgDesc).Name = "dgDesc";
		((DataGridViewBand)dgDesc).ReadOnly = true;
		((DataGridViewColumn)dgMore).DataPropertyName = "MoreDetail";
		((DataGridViewColumn)dgMore).HeaderText = "";
		((DataGridViewColumn)dgMore).Name = "dgMore";
		((DataGridViewBand)dgMore).ReadOnly = true;
		((DataGridViewColumn)dgMore).Width = 30;
		((DataGridViewColumn)dgText).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)dgText).DataPropertyName = "Text";
		val6.Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
		((DataGridViewBand)dgText).DefaultCellStyle = val6;
		((DataGridViewColumn)dgText).HeaderText = "Explanation";
		((DataGridViewColumn)dgText).Name = "dgText";
		((DataGridViewBand)dgText).ReadOnly = true;
		((DataGridViewBand)dgText).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn4).DataPropertyName = "Type";
		((DataGridViewColumn)dataGridViewTextBoxColumn4).HeaderText = "Type";
		((DataGridViewColumn)dataGridViewTextBoxColumn4).Name = "dataGridViewTextBoxColumn4";
		((DataGridViewBand)dataGridViewTextBoxColumn4).ReadOnly = true;
		((DataGridViewBand)dataGridViewTextBoxColumn4).Visible = false;
		panelEx1.BorderStyle = (BorderStyle)1;
		((Control)panelEx1).Controls.Add((Control)(object)flowLayoutPanel1);
		((Control)panelEx1).Controls.Add((Control)(object)dGrid);
		((Control)panelEx1).Controls.Add((Control)(object)panel3);
		((Control)panelEx1).Location = new Point(8, 7);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(650, 416);
		((Control)panelEx1).TabIndex = 15;
		((Panel)flowLayoutPanel1).BorderStyle = (BorderStyle)2;
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)btnCancel);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)btnContinue);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)btnAbort);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)btnNo);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)btnYes);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)btnReturn);
		flowLayoutPanel1.FlowDirection = (FlowDirection)2;
		((Control)flowLayoutPanel1).Location = new Point(8, 359);
		((Control)flowLayoutPanel1).Name = "flowLayoutPanel1";
		((Control)flowLayoutPanel1).Padding = new Padding(0, 0, 3, 0);
		((Control)flowLayoutPanel1).RightToLeft = (RightToLeft)1;
		((Control)flowLayoutPanel1).Size = new Size(633, 50);
		((Control)flowLayoutPanel1).TabIndex = 15;
		flowLayoutPanel1.WrapContents = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(666, 431);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)2;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(672, 316);
		((Control)this).Name = "FrmShowErrorMessage";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).FormClosed += new FormClosedEventHandler(FrmShowErrorMessage_FormClosed);
		((Form)this).Load += FrmShowErrorMessage_Load;
		((Control)this).KeyDown += new KeyEventHandler(FrmShowErrorMessage_KeyDown);
		((ISupportInitialize)ErrorBindingSource).EndInit();
		((Control)panel3).ResumeLayout(false);
		((Control)panel3).PerformLayout();
		((ISupportInitialize)dGrid).EndInit();
		((Control)panelEx1).ResumeLayout(false);
		((Control)flowLayoutPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
