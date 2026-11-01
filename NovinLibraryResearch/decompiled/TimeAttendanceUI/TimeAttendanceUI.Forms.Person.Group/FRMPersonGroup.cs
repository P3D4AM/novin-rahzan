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
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Person.Group;

public class FRMPersonGroup : CacheBaseForm, IHasContext
{
	private PersonGroup _cls;

	private List<PersonGroup> _listData;

	private SortableBindingList<PersonGroup> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	internal PanelEx PanelEx1;

	internal ButtonX BtnRegister;

	internal ButtonX Btncancela;

	internal PanelEx PanelEx2;

	internal PictureBox PictureBox1;

	internal TextBoxX TXTGroup;

	internal LabelX LabelX1;

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
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				((CacheBaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FRMPersonGroup(string contextGuid)
		: base(contextGuid)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		_loadDataToken?.Cancel();
		_loadDataToken = new CancellationTokenSource();
		CancellationToken token = _loadDataToken.Token;
		new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(0, token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Group\\FRMPersonGroup.cs");
		((Control)this).Text = "افزودن گروه جدید";
	}

	public FRMPersonGroup(string contextGuid, short ID)
		: base(contextGuid)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		_loadDataToken?.Cancel();
		_loadDataToken = new CancellationTokenSource();
		CancellationToken token = _loadDataToken.Token;
		new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(ID, token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Group\\FRMPersonGroup.cs");
		FillForm();
		((Control)this).Text = "ویرایش گروه";
	}

	private async Task LoadDataAsync(short personGroupId, CancellationToken token)
	{
		try
		{
			if (personGroupId > 0)
			{
				_cls = await PersonGroup.GetAsync(((CacheBaseForm)this).ContextGuid, personGroupId, token);
				((Control)TXTGroup).Text = _cls.Name;
			}
			else
			{
				_cls = new PersonGroup(((CacheBaseForm)this).ContextGuid);
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

	protected override void OnLoad(EventArgs e)
	{
		((Form)this).OnLoad(e);
		if (!UserAttendance.SelectedUser.AccessLevelMerg.Personnel.PersonnelGroup)
		{
			((Form)this).Close();
		}
	}

	private void FillForm()
	{
		((Control)TXTGroup).Text = _cls?.Name;
	}

	private void GetData()
	{
		_cls.Name = Text.FixString(((Control)TXTGroup).Text.Trim());
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnRegister_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Group\\FRMPersonGroup.cs", 130);
		}
	}

	private void Btncancela_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private void TXTGroup_Enter(object sender, EventArgs e)
	{
		((TextBoxBase)TXTGroup).SelectAll();
		((Control)TXTGroup).BackColor = Color.Yellow;
	}

	private void TXTGroup_Leave(object sender, EventArgs e)
	{
		((Control)TXTGroup).BackColor = Color.White;
	}

	private void FRMPersonGroup_KeyDown(object sender, KeyEventArgs e)
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
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Expected O, but got Unknown
		//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Expected O, but got Unknown
		//IL_075e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0768: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		BtnRegister = new ButtonX();
		Btncancela = new ButtonX();
		PanelEx2 = new PanelEx();
		PictureBox1 = new PictureBox();
		TXTGroup = new TextBoxX();
		LabelX1 = new LabelX();
		((Control)PanelEx1).SuspendLayout();
		((Control)PanelEx2).SuspendLayout();
		((ISupportInitialize)PictureBox1).BeginInit();
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
		((Control)PanelEx1).Size = new Size(249, 112);
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
		((Control)BtnRegister).Location = new Point(137, 69);
		((Control)BtnRegister).Name = "BtnRegister";
		((Control)BtnRegister).Size = new Size(107, 33);
		((PopupItemControl)BtnRegister).Style = (eDotNetBarStyle)9;
		((Control)BtnRegister).TabIndex = 1;
		((Control)BtnRegister).Text = "ثبت(F5)";
		((Control)BtnRegister).Click += BtnRegister_Click;
		((Control)Btncancela).AccessibleRole = (AccessibleRole)43;
		((Control)Btncancela).Anchor = (AnchorStyles)6;
		Btncancela.ColorTable = (eButtonColor)3;
		((Control)Btncancela).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		Btncancela.Image = (Image)(object)Resources.stop1;
		Btncancela.ImagePosition = (eImagePosition)1;
		((Control)Btncancela).Location = new Point(3, 69);
		((Control)Btncancela).Name = "Btncancela";
		((Control)Btncancela).Size = new Size(107, 33);
		((PopupItemControl)Btncancela).Style = (eDotNetBarStyle)9;
		((Control)Btncancela).TabIndex = 2;
		((Control)Btncancela).TabStop = false;
		((Control)Btncancela).Text = "لغـو(Esc)";
		((Control)Btncancela).Click += Btncancela_Click;
		((Control)PanelEx2).Anchor = (AnchorStyles)15;
		PanelEx2.CanvasColor = SystemColors.Control;
		PanelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx2).Controls.Add((Control)(object)PictureBox1);
		((Control)PanelEx2).Controls.Add((Control)(object)TXTGroup);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX1);
		PanelEx2.DisabledBackColor = Color.Empty;
		((Control)PanelEx2).Location = new Point(3, 12);
		((Control)PanelEx2).Name = "PanelEx2";
		((Control)PanelEx2).Size = new Size(241, 48);
		PanelEx2.Style.Alignment = (StringAlignment)1;
		PanelEx2.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx2.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx2.Style.Border = (eBorderType)1;
		PanelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx2.Style.GradientAngle = 90;
		((Control)PanelEx2).TabIndex = 0;
		((Control)PictureBox1).Anchor = (AnchorStyles)9;
		PictureBox1.Image = (Image)(object)Resources.user_group;
		((Control)PictureBox1).Location = new Point(183, 7);
		((Control)PictureBox1).Name = "PictureBox1";
		((Control)PictureBox1).Size = new Size(52, 33);
		PictureBox1.SizeMode = (PictureBoxSizeMode)4;
		PictureBox1.TabIndex = 2;
		PictureBox1.TabStop = false;
		TXTGroup.Border.Class = "TextBoxBorder";
		TXTGroup.Border.CornerType = (eCornerType)1;
		((Control)TXTGroup).Location = new Point(9, 13);
		((Control)TXTGroup).Name = "TXTGroup";
		((Control)TXTGroup).Size = new Size(129, 21);
		((Control)TXTGroup).TabIndex = 1;
		((Control)TXTGroup).Enter += TXTGroup_Enter;
		((Control)TXTGroup).Leave += TXTGroup_Leave;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX1).Location = new Point(134, 12);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(45, 23);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "گروه: ";
		LabelX1.TextAlignment = (StringAlignment)1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(249, 112);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(265, 151);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(265, 151);
		((Control)this).Name = "FRMPersonGroup";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "گروه کارکنان";
		((Control)this).KeyDown += new KeyEventHandler(FRMPersonGroup_KeyDown);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)PanelEx2).ResumeLayout(false);
		((ISupportInitialize)PictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
