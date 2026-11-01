using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Cache;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Person.Eisar;

public class FRMEisar : CacheBaseForm, IHasContext
{
	private Per_Eisar _cls;

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private CancellationTokenSource _loadDataToken;

	private bool _isDisposed;

	private IContainer components;

	internal PanelEx PanelEx1;

	internal ButtonX BtnRegister;

	internal ButtonX Btncancela;

	internal PanelEx PanelEx2;

	internal TextBoxX TxtType;

	internal LabelX LabelX1;

	internal LabelX labelX2;

	internal TextBoxX TxtDesc;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((CacheBaseForm)this).IsDisposed;
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
				components?.Dispose();
				((CacheBaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
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
			if (!trDictionary.TryGetValue("[ذخیره]", out var value))
			{
				trDictionary.Add("[ذخیره]", Utilities.GetTranslate("[ذخیره]"));
			}
			if (!trDictionary.TryGetValue("[افزودن نوع ایثارگری]", out value))
			{
				trDictionary.Add("[افزودن نوع ایثارگری]", Utilities.GetTranslate("[افزودن نوع ایثارگری]"));
			}
			if (!trDictionary.TryGetValue("[ویرایش نوع ایثارگری]", out value))
			{
				trDictionary.Add("[ویرایش نوع ایثارگری]", Utilities.GetTranslate("[ویرایش نوع ایثارگری]"));
			}
			if (!trDictionary.TryGetValue("[لغو]", out value))
			{
				trDictionary.Add("[لغو]", Utilities.GetTranslate("[لغو]"));
			}
			if (!trDictionary.TryGetValue("[شرح]", out value))
			{
				trDictionary.Add("[شرح]", Utilities.GetTranslate("[شرح]"));
			}
			if (!trDictionary.TryGetValue("[نوع ایثارگری]", out value))
			{
				trDictionary.Add("[نوع ایثارگری]", Utilities.GetTranslate("[نوع ایثارگری]"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Eisar\\FRMEisar.cs", 58);
		}
	}

	public FRMEisar(string contextGuid)
		: base(contextGuid)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			InitializeComponent();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(0, token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Eisar\\FRMEisar.cs");
			((Control)this).Text = "افزودن ایثارگری";
			if (trDictionary.TryGetValue("[افزودن نوع ایثارگری]", out var value))
			{
				((Control)this).Text = value;
			}
			TranslateControlText();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Eisar\\FRMEisar.cs", 77);
		}
	}

	public FRMEisar(string contextGuid, short ID)
		: base(contextGuid)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			InitializeComponent();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(ID, token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Eisar\\FRMEisar.cs");
			FillForm();
			((Control)this).Text = "ویرایش ایثارگری";
			if (trDictionary.TryGetValue("[ویرایش نوع ایثارگری]", out var value))
			{
				((Control)this).Text = value;
			}
			TranslateControlText();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Eisar\\FRMEisar.cs", 99);
		}
	}

	private async Task LoadDataAsync(short eisarId, CancellationToken token)
	{
		_ = 1;
		try
		{
			await SetTrDictionaryAsync();
			if (eisarId > 0)
			{
				_cls = await Per_Eisar.GetAsync(((CacheBaseForm)this).ContextGuid, eisarId, token);
			}
			else
			{
				_cls = new Per_Eisar(((CacheBaseForm)this).ContextGuid);
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private void TranslateControlText()
	{
		try
		{
			if (trDictionary.TryGetValue("[ذخیره]", out var value))
			{
				((Control)BtnRegister).Text = value + " (F5)";
			}
			if (trDictionary.TryGetValue("[لغو]", out var value2))
			{
				((Control)Btncancela).Text = value2 + " (Esc)";
			}
			if (trDictionary.TryGetValue("[شرح]", out var value3))
			{
				((Control)labelX2).Text = value3 + ":";
			}
			if (trDictionary.TryGetValue("[نوع ایثارگری]", out var value4))
			{
				((Control)LabelX1).Text = value4 + ":";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TranslateControlText", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Eisar\\FRMEisar.cs", 131);
		}
	}

	private void FillForm()
	{
		((Control)TxtType).Text = _cls.Name;
		((Control)TxtDesc).Text = _cls.Desc;
	}

	private void GetData()
	{
		_cls.Name = Text.FixString(((Control)TxtType).Text.Trim());
		_cls.Desc = Text.FixString(((Control)TxtDesc).Text.Trim());
	}

	private async void BtnRegister_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnRegister).Visible || !((Control)BtnRegister).Enabled)
		{
			return;
		}
		try
		{
			GetData();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			if (await _cls.SaveAsync(token))
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnRegister_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Eisar\\FRMEisar.cs", 162);
		}
	}

	private void Btncancela_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private void TxtType_Enter(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		((TextBoxBase)(TextBox)sender).SelectAll();
		((Control)(TextBox)sender).BackColor = Color.Yellow;
	}

	private void FRMEisar_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		Keys keyCode = e.KeyCode;
		if ((int)keyCode != 13)
		{
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 116)
				{
					e.Handled = true;
					((PopupItemControl)BtnRegister).PerformClick();
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

	private void FRMEisar_Load(object sender, EventArgs e)
	{
		((Control)TxtType).Focus();
	}

	private void TxtType_Leave(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		((Control)(TextBox)sender).BackColor = Color.White;
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
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Expected O, but got Unknown
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d7: Expected O, but got Unknown
		//IL_085b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0865: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		BtnRegister = new ButtonX();
		Btncancela = new ButtonX();
		PanelEx2 = new PanelEx();
		TxtDesc = new TextBoxX();
		labelX2 = new LabelX();
		TxtType = new TextBoxX();
		LabelX1 = new LabelX();
		((Control)PanelEx1).SuspendLayout();
		((Control)PanelEx2).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)PanelEx1).Controls.Add((Control)(object)BtnRegister);
		((Control)PanelEx1).Controls.Add((Control)(object)Btncancela);
		((Control)PanelEx1).Controls.Add((Control)(object)PanelEx2);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(368, 147);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.SteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((Control)BtnRegister).AccessibleRole = (AccessibleRole)43;
		((Control)BtnRegister).Anchor = (AnchorStyles)6;
		BtnRegister.ColorTable = (eButtonColor)3;
		((Control)BtnRegister).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnRegister.Image = (Image)(object)Resources.accept;
		BtnRegister.ImagePosition = (eImagePosition)1;
		((Control)BtnRegister).Location = new Point(118, 102);
		((Control)BtnRegister).Name = "BtnRegister";
		((Control)BtnRegister).Size = new Size(106, 33);
		((PopupItemControl)BtnRegister).Style = (eDotNetBarStyle)9;
		((Control)BtnRegister).TabIndex = 1;
		((Control)BtnRegister).Text = " ذخیره (F5)";
		((Control)BtnRegister).Click += BtnRegister_Click;
		((Control)Btncancela).AccessibleRole = (AccessibleRole)43;
		((Control)Btncancela).Anchor = (AnchorStyles)6;
		Btncancela.ColorTable = (eButtonColor)3;
		((Control)Btncancela).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		Btncancela.Image = (Image)(object)Resources.stop1;
		Btncancela.ImagePosition = (eImagePosition)1;
		((Control)Btncancela).Location = new Point(3, 102);
		((Control)Btncancela).Name = "Btncancela";
		((Control)Btncancela).Size = new Size(106, 33);
		((PopupItemControl)Btncancela).Style = (eDotNetBarStyle)9;
		((Control)Btncancela).TabIndex = 2;
		((Control)Btncancela).TabStop = false;
		((Control)Btncancela).Text = "لغو (Esc)";
		((Control)Btncancela).Click += Btncancela_Click;
		((Control)PanelEx2).Anchor = (AnchorStyles)15;
		PanelEx2.CanvasColor = SystemColors.Control;
		PanelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx2).Controls.Add((Control)(object)TxtDesc);
		((Control)PanelEx2).Controls.Add((Control)(object)labelX2);
		((Control)PanelEx2).Controls.Add((Control)(object)TxtType);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX1);
		PanelEx2.DisabledBackColor = Color.Empty;
		((Control)PanelEx2).Location = new Point(3, 12);
		((Control)PanelEx2).Name = "PanelEx2";
		((Control)PanelEx2).Size = new Size(359, 84);
		PanelEx2.Style.Alignment = (StringAlignment)1;
		PanelEx2.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx2.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx2.Style.Border = (eBorderType)1;
		PanelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx2.Style.GradientAngle = 90;
		((Control)PanelEx2).TabIndex = 0;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(9, 41);
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(262, 40);
		((Control)TxtDesc).TabIndex = 3;
		((Control)TxtDesc).Enter += TxtType_Enter;
		((Control)TxtDesc).Leave += TxtType_Leave;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX2).Location = new Point(276, 50);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(72, 23);
		((Control)labelX2).TabIndex = 2;
		((Control)labelX2).Text = "شرح :";
		labelX2.TextAlignment = (StringAlignment)2;
		TxtType.Border.Class = "TextBoxBorder";
		TxtType.Border.CornerType = (eCornerType)1;
		((Control)TxtType).Location = new Point(121, 14);
		((Control)TxtType).Name = "TxtType";
		((Control)TxtType).Size = new Size(150, 21);
		((Control)TxtType).TabIndex = 1;
		((Control)TxtType).Enter += TxtType_Enter;
		((Control)TxtType).Leave += TxtType_Leave;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX1).Location = new Point(276, 13);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(72, 23);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "نوع ایثارگری:";
		LabelX1.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(368, 147);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(384, 186);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(384, 186);
		((Control)this).Name = "FRMEisar";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "ایثارگری";
		((Form)this).Load += FRMEisar_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMEisar_KeyDown);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)PanelEx2).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
