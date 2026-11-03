using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ClientHub.AccountingHubs;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PayamakUi.Classes.Entities;
using PayamakUi.Properties;
using PayamakUi.UC.UCSMS;

namespace PayamakUi.Forms.FRMOprate;

public class FRMTemplate : CacheBaseForm
{
	private SMSTemplate _cls;

	private CancellationTokenSource _tokenSave = new CancellationTokenSource();

	private PayamakClientHubInstances _instance;

	private bool _isDisposed;

	private IContainer components;

	internal GroupPanel GroupPanel1;

	internal ButtonX BtnAccept;

	internal GroupPanel GroupPanel2;

	internal Label LblContent;

	internal Label LBlTitle;

	internal DateBoxFa TXTDate;

	internal ButtonX BtnExit_Sound;

	internal Label LabelX2;

	internal TextBoxX TxtTitle;

	internal Label LabelX1;

	internal BindingSource PhoneBookGroupBindingSource;

	internal TextBoxX TXTGroupName;

	internal UCSMS TXTContent;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
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
				_tokenSave?.Cancel();
				_tokenSave?.Dispose();
				_tokenSave = null;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMTemplate.cs", 39);
		}
	}

	public FRMTemplate(string contextGuid)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			_cls = new SMSTemplate();
			TXTDate.MiladiDate = DateTime.Now;
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMTemplate.cs", 51);
		}
	}

	public FRMTemplate(string contextGuid, SMSTemplate template)
		: base(contextGuid)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		if (template == null)
		{
			throw new ArgumentNullException("template");
		}
		try
		{
			InitializeComponent();
			_cls = template;
			((Control)this).KeyDown += new KeyEventHandler(FRMTemplate_KeyDown);
			((Control)TXTGroupName).Focus();
			TXTContent.Text = template.Content;
			TXTDate.MiladiDate = template.DateRegister;
			((Control)TXTGroupName).Text = template.GroupName;
			((Control)TxtTitle).Text = template.Title;
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMTemplate.cs", 68);
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			try
			{
				if (!((Control)BtnAccept).Enabled || !((Control)BtnAccept).Visible)
				{
					goto end_IL_003b;
				}
				if (string.IsNullOrEmpty(((Control)TxtTitle).Text))
				{
					res.AddError("لطفا عنوان الگو را پر نمائید", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMTemplate.cs", 77);
				}
				if (string.IsNullOrEmpty(TXTContent.Text))
				{
					res.AddError("لطفا متن الگو را پر نمائید", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMTemplate.cs", 78);
				}
				if (res.HasError)
				{
					goto end_IL_003b;
				}
				_cls.Title = ((Control)TxtTitle).Text.FixString().Trim();
				_cls.Content = TXTContent.Text.FixString().Trim();
				_cls.DateRegister = TXTDate.MiladiDate;
				_cls.GroupName = ((Control)TXTGroupName).Text;
				_tokenSave?.Cancel();
				_tokenSave = new CancellationTokenSource();
				CancellationToken token = _tokenSave.Token;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _instance.SmsTemplate.SaveAsync(_cls, token), ReturnedState.None, "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMTemplate.cs", 88);
				goto end_IL_0032;
				end_IL_003b:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMTemplate.cs", 92);
				res.AddReturnedValue(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMTemplate.cs", 93);
				goto end_IL_0032;
			}
			end_IL_0032:;
		}
		finally
		{
			if (res.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(res, "پیغام سیستم", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMTemplate.cs");
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
	}

	private void BtnExit_Sound_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnExit_Sound_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMTemplate.cs", 112);
		}
	}

	private void FRMTemplate_KeyDown(object sender, KeyEventArgs e)
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
			if ((int)keyCode != 13)
			{
				if ((int)keyCode != 27)
				{
					if ((int)keyCode == 116)
					{
						BtnAccept.PerformClick();
					}
				}
				else
				{
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
			}
			else
			{
				SendKeys.Send("{Tab}");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMTemplate_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMTemplate.cs", 125);
		}
	}

	private void TXTGroupName_Enter(object sender, EventArgs e)
	{
		try
		{
			TextBox val = (TextBox)((sender is TextBox) ? sender : null);
			if (val != null)
			{
				((Control)val).BackColor = Color.Yellow;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTGroupName_Enter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMTemplate.cs", 134);
		}
	}

	private void TXTGroupName_Leave(object sender, EventArgs e)
	{
		try
		{
			TextBox val = (TextBox)((sender is TextBox) ? sender : null);
			if (val != null)
			{
				((Control)val).BackColor = Color.White;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTGroupName_Leave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOprate\\FRMTemplate.cs", 143);
		}
	}

	private void FRMTemplate_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void InitializeComponent()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_09a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ac: Expected O, but got Unknown
		//IL_09d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e2: Expected O, but got Unknown
		//IL_0c06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c10: Expected O, but got Unknown
		components = new Container();
		GroupPanel1 = new GroupPanel();
		BtnExit_Sound = new ButtonX();
		GroupPanel2 = new GroupPanel();
		TXTContent = new UCSMS();
		TXTGroupName = new TextBoxX();
		LabelX2 = new Label();
		TxtTitle = new TextBoxX();
		LabelX1 = new Label();
		LblContent = new Label();
		LBlTitle = new Label();
		TXTDate = new DateBoxFa();
		BtnAccept = new ButtonX();
		PhoneBookGroupBindingSource = new BindingSource(components);
		((Control)GroupPanel1).SuspendLayout();
		((Control)GroupPanel2).SuspendLayout();
		((ISupportInitialize)PhoneBookGroupBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		GroupPanel1.CanvasColor = SystemColors.Control;
		GroupPanel1.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel1).Controls.Add((Control)(object)BtnExit_Sound);
		((Control)GroupPanel1).Controls.Add((Control)(object)GroupPanel2);
		((Control)GroupPanel1).Controls.Add((Control)(object)BtnAccept);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Dock = (DockStyle)5;
		((Control)GroupPanel1).Location = new Point(0, 0);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(393, 456);
		GroupPanel1.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel1.Style.BackColorGradientAngle = 90;
		GroupPanel1.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel1.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderBottomWidth = 1;
		GroupPanel1.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel1.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderLeftWidth = 1;
		GroupPanel1.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderRightWidth = 1;
		GroupPanel1.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderTopWidth = 1;
		GroupPanel1.Style.CornerDiameter = 4;
		GroupPanel1.Style.CornerType = eCornerType.Rounded;
		GroupPanel1.Style.TextAlignment = eStyleTextAlignment.Center;
		GroupPanel1.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GroupPanel1.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel1.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel1.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel1).TabIndex = 0;
		((Control)BtnExit_Sound).AccessibleRole = (AccessibleRole)43;
		((Control)BtnExit_Sound).Anchor = (AnchorStyles)6;
		BtnExit_Sound.ColorTable = eButtonColor.Office2007WithBackground;
		BtnExit_Sound.Image = (Image)(object)Resources._1231;
		((Control)BtnExit_Sound).Location = new Point(3, 417);
		((Control)BtnExit_Sound).Name = "BtnExit_Sound";
		((Control)BtnExit_Sound).Size = new Size(29, 33);
		BtnExit_Sound.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnExit_Sound).TabIndex = 2;
		((Control)BtnExit_Sound).TabStop = false;
		BtnExit_Sound.Tooltip = "خروج";
		((Control)BtnExit_Sound).Click += BtnExit_Sound_Click;
		((Control)GroupPanel2).Anchor = (AnchorStyles)15;
		((Control)GroupPanel2).BackColor = Color.Transparent;
		GroupPanel2.CanvasColor = SystemColors.Control;
		GroupPanel2.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel2).Controls.Add((Control)(object)TXTContent);
		((Control)GroupPanel2).Controls.Add((Control)(object)TXTGroupName);
		((Control)GroupPanel2).Controls.Add((Control)(object)LabelX2);
		((Control)GroupPanel2).Controls.Add((Control)(object)TxtTitle);
		((Control)GroupPanel2).Controls.Add((Control)(object)LabelX1);
		((Control)GroupPanel2).Controls.Add((Control)(object)LblContent);
		((Control)GroupPanel2).Controls.Add((Control)(object)LBlTitle);
		((Control)GroupPanel2).Controls.Add((Control)(object)TXTDate);
		GroupPanel2.DisabledBackColor = Color.Empty;
		((Control)GroupPanel2).Location = new Point(3, 3);
		((Control)GroupPanel2).Name = "GroupPanel2";
		((Control)GroupPanel2).Size = new Size(382, 408);
		GroupPanel2.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel2.Style.BackColorGradientAngle = 90;
		GroupPanel2.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel2.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderBottomWidth = 2;
		GroupPanel2.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel2.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderLeftWidth = 2;
		GroupPanel2.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderRightWidth = 2;
		GroupPanel2.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderTopWidth = 2;
		GroupPanel2.Style.CornerDiameter = 4;
		GroupPanel2.Style.CornerType = eCornerType.Rounded;
		GroupPanel2.Style.TextAlignment = eStyleTextAlignment.Far;
		GroupPanel2.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GroupPanel2.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel2.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel2.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel2).TabIndex = 0;
		((Control)GroupPanel2).Text = "الگو";
		((Control)TXTContent).Anchor = (AnchorStyles)15;
		TXTContent.ContextGuid = null;
		((Control)TXTContent).Location = new Point(3, 111);
		((Control)TXTContent).Name = "TXTContent";
		((Control)TXTContent).Size = new Size(311, 256);
		((Control)TXTContent).TabIndex = 3;
		((Control)TXTGroupName).Anchor = (AnchorStyles)13;
		TXTGroupName.Border.Class = "TextBoxBorder";
		TXTGroupName.Border.CornerType = eCornerType.Square;
		((Control)TXTGroupName).Location = new Point(134, 7);
		((Control)TXTGroupName).Name = "TXTGroupName";
		((Control)TXTGroupName).Size = new Size(170, 20);
		((Control)TXTGroupName).TabIndex = 0;
		((Control)TXTGroupName).Enter += TXTGroupName_Enter;
		((Control)TXTGroupName).Leave += TXTGroupName_Leave;
		((Control)LabelX2).Location = new Point(311, 63);
		((Control)LabelX2).Name = "LabelX2";
		((Control)LabelX2).Size = new Size(53, 23);
		((Control)LabelX2).TabIndex = 2;
		((Control)LabelX2).Text = "تاریخ ثبت  :";
		((Control)TxtTitle).Anchor = (AnchorStyles)13;
		TxtTitle.Border.Class = "TextBoxBorder";
		TxtTitle.Border.CornerType = eCornerType.Square;
		((Control)TxtTitle).Location = new Point(134, 32);
		((Control)TxtTitle).Name = "TxtTitle";
		((Control)TxtTitle).Size = new Size(170, 20);
		((Control)TxtTitle).TabIndex = 1;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((Control)LabelX1).Location = new Point(319, 4);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(45, 23);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "نام گروه :";
		((Control)LblContent).Anchor = (AnchorStyles)9;
		((Control)LblContent).Location = new Point(330, 111);
		((Control)LblContent).Name = "LblContent";
		((Control)LblContent).Size = new Size(27, 23);
		((Control)LblContent).TabIndex = 6;
		((Control)LblContent).Text = "متن :";
		((Control)LBlTitle).Anchor = (AnchorStyles)9;
		((Control)LBlTitle).Location = new Point(331, 31);
		((Control)LBlTitle).Name = "LBlTitle";
		((Control)LBlTitle).Size = new Size(34, 23);
		((Control)LBlTitle).TabIndex = 4;
		((Control)LBlTitle).Text = "عنوان :";
		TXTDate.AllowNull = false;
		TXTDate.BackcolorTooltip = Color.Red;
		TXTDate.Border.CornerType = eCornerType.Square;
		TXTDate.EnterToTab = true;
		TXTDate.EscToClose = true;
		TXTDate.FarsyDateStyle = DateBoxFa.ENFarsyDateStyle.DisplayToolTip;
		TXTDate.FocusBackColor = Color.Yellow;
		TXTDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTDate.FocusForeColor = Color.Blue;
		TXTDate.FocusTextSelect = true;
		((Control)TXTDate).Font = new Font("Times New Roman", 12f);
		((Control)TXTDate).Location = new Point(134, 59);
		TXTDate.MessageEmptyShowDialog = false;
		TXTDate.MessegeEmpty = "";
		TXTDate.MessegeEmptyInFormRight = true;
		TXTDate.MessegeEmptyShowInForm = false;
		TXTDate.MiladiDate = new DateTime(2025, 7, 15, 16, 47, 33, 529);
		((Control)TXTDate).Name = "TXTDate";
		((Control)TXTDate).RightToLeft = (RightToLeft)0;
		TXTDate.ShowNameOfDayOnToolTip = false;
		TXTDate.ShowToolTipText = false;
		((Control)TXTDate).Size = new Size(170, 20);
		((Control)TXTDate).TabIndex = 2;
		((Control)TXTDate).Text = "7/15/2025 ";
		((TextBox)TXTDate).TextAlign = (HorizontalAlignment)2;
		TXTDate.TootTipDelay = 500;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = eButtonColor.Office2007WithBackground;
		BtnAccept.Image = (Image)(object)Resources.accept;
		((Control)BtnAccept).Location = new Point(37, 417);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).Size = new Size(29, 33);
		BtnAccept.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnAccept).TabIndex = 1;
		BtnAccept.Tooltip = "تایید";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(393, 456);
		((Control)this).Controls.Add((Control)(object)GroupPanel1);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMTemplate";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "الگوی پیام";
		((Form)this).FormClosed += new FormClosedEventHandler(FRMTemplate_FormClosed);
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)GroupPanel2).ResumeLayout(false);
		((ISupportInitialize)PhoneBookGroupBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
