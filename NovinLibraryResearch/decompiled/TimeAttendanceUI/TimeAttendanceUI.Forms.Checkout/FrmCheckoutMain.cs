using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using NPResourceManagement;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRCheckout;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Checkout;

public class FrmCheckoutMain : BaseForm, ICheckout
{
	private readonly Stack<ICheckout> _pages = new Stack<ICheckout>();

	private TimeAttendanceDAL.Dal.FLRCheckout.Checkout checkout;

	private string frm_Text = "فرم تسویه حساب";

	private Font _font;

	private IContainer components;

	public PanelEx PanelEx1;

	private Panel pnlLeft;

	private Panel pnlBottom;

	private TableLayoutPanel tableLayoutPanel1;

	private Panel pnlContent;

	private ButtonX btnBack;

	private PictureBox pictureBox1;

	private ButtonX btnCancel;

	private ButtonX btnNext;

	private ButtonX BtnAccept;

	public sealed override string Text
	{
		get
		{
			return ((Form)this).Text;
		}
		set
		{
			((Form)this).Text = value;
		}
	}

	public FrmCheckoutMain(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		_font = font;
		InitializeComponent();
		try
		{
			this.checkout = new TimeAttendanceDAL.Dal.FLRCheckout.Checkout(((CacheBaseForm)this).ContextGuid);
			ICheckout checkout = new FrmCheckOutWellcome(((CacheBaseForm)this).ContextGuid, this.checkout, _font);
			_pages.Push(checkout);
			LoadTopFormAsync(checkout);
			((Control)this).Text = "فرم تسویه حساب";
			frm_Text = "[فرم تسویه حساب]";
			((Control)btnBack).Enabled = false;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmCheckoutMain.cs", 40);
		}
	}

	private async Task LoadTopFormAsync(ICheckout frm)
	{
		try
		{
			do
			{
				await Task.Delay(100);
				if (((BaseForm)this).IsDisposed)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			((Control)this).BeginInvoke((Delegate)(Func<Task>)async delegate
			{
				try
				{
					frm.TopLevel = false;
					frm.AutoScroll = true;
					((Control)pnlContent).Controls.Clear();
					((Control)pnlContent).Controls.Add((Control)(Form)frm);
					await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			});
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task BackAsync()
	{
		((Control)btnNext).Enabled = true;
		((Control)BtnAccept).Enabled = false;
		_pages.Pop();
		await LoadTopFormAsync(_pages.Peek());
		if (_pages.Count <= 1)
		{
			((Control)btnBack).Enabled = false;
		}
	}

	private async Task NextAsync()
	{
		ICheckout nextPage = _pages.Peek().NextPage(fillCheckout: true);
		if (nextPage != null)
		{
			((Control)btnBack).Enabled = true;
			_pages.Push(nextPage);
			await LoadTopFormAsync(nextPage);
			if (nextPage.NextPage(fillCheckout: false) == null)
			{
				((Control)btnNext).Enabled = false;
				((Control)BtnAccept).Enabled = true;
			}
		}
	}

	public void FillCheckout()
	{
		throw new NotImplementedException();
	}

	public ICheckout NextPage(bool fillCheckout)
	{
		return null;
	}

	private async void BtnNext_Click(object sender, EventArgs e)
	{
		await NextAsync();
	}

	private async void BtnBack_Click(object sender, EventArgs e)
	{
		await BackAsync();
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void BtnAccept_Click(object sender, EventArgs e)
	{
	}

	private void FrmCheckoutMain_Load(object sender, EventArgs e)
	{
		try
		{
			((Control)this).Text = Utilities.GetTranslate(frm_Text);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmCheckoutMain_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmCheckoutMain.cs", 160);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((BaseForm)this).Dispose(disposing);
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
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Expected O, but got Unknown
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Expected O, but got Unknown
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Expected O, but got Unknown
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Expected O, but got Unknown
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_0712: Expected O, but got Unknown
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0807: Expected O, but got Unknown
		//IL_083d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0905: Expected O, but got Unknown
		//IL_093a: Unknown result type (might be due to invalid IL or missing references)
		PanelEx1 = new PanelEx();
		pnlContent = new Panel();
		pnlLeft = new Panel();
		pictureBox1 = new PictureBox();
		pnlBottom = new Panel();
		tableLayoutPanel1 = new TableLayoutPanel();
		btnCancel = new ButtonX();
		btnBack = new ButtonX();
		btnNext = new ButtonX();
		BtnAccept = new ButtonX();
		((Control)PanelEx1).SuspendLayout();
		((Control)pnlLeft).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)pnlBottom).SuspendLayout();
		((Control)tableLayoutPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)pnlContent);
		((Control)PanelEx1).Controls.Add((Control)(object)pnlLeft);
		((Control)PanelEx1).Controls.Add((Control)(object)pnlBottom);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(2);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(784, 562);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 1;
		((Control)pnlContent).Anchor = (AnchorStyles)15;
		((Control)pnlContent).Location = new Point(0, 0);
		((Control)pnlContent).Margin = new Padding(2);
		((Control)pnlContent).Name = "pnlContent";
		((Control)pnlContent).Padding = new Padding(7, 6, 7, 6);
		((Control)pnlContent).Size = new Size(499, 500);
		((Control)pnlContent).TabIndex = 10;
		((Control)pnlLeft).BackColor = Color.FromArgb(255, 153, 0);
		((Control)pnlLeft).Controls.Add((Control)(object)pictureBox1);
		((Control)pnlLeft).Dock = (DockStyle)4;
		((Control)pnlLeft).Location = new Point(503, 0);
		((Control)pnlLeft).Margin = new Padding(2);
		((Control)pnlLeft).Name = "pnlLeft";
		((Control)pnlLeft).Size = new Size(281, 504);
		((Control)pnlLeft).TabIndex = 9;
		((Control)pictureBox1).BackColor = Color.FromArgb(0, 0, 0, 0);
		((Control)pictureBox1).Dock = (DockStyle)1;
		pictureBox1.Image = (Image)(object)ImageResourceManagement.Devices;
		pictureBox1.InitialImage = (Image)(object)Resources.blue_package;
		((Control)pictureBox1).Location = new Point(0, 0);
		((Control)pictureBox1).Margin = new Padding(2);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(281, 504);
		pictureBox1.SizeMode = (PictureBoxSizeMode)1;
		pictureBox1.TabIndex = 0;
		pictureBox1.TabStop = false;
		((Control)pnlBottom).BackColor = SystemColors.Control;
		((Control)pnlBottom).Controls.Add((Control)(object)tableLayoutPanel1);
		((Control)pnlBottom).Dock = (DockStyle)2;
		((Control)pnlBottom).Location = new Point(0, 504);
		((Control)pnlBottom).Margin = new Padding(2);
		((Control)pnlBottom).Name = "pnlBottom";
		((Control)pnlBottom).Padding = new Padding(7, 6, 7, 6);
		((Control)pnlBottom).Size = new Size(784, 58);
		((Control)pnlBottom).TabIndex = 8;
		tableLayoutPanel1.ColumnCount = 4;
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanel1.Controls.Add((Control)(object)btnCancel, 3, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)btnBack, 2, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)btnNext, 1, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)BtnAccept, 0, 0);
		((Control)tableLayoutPanel1).Dock = (DockStyle)3;
		((Control)tableLayoutPanel1).Location = new Point(7, 6);
		((Control)tableLayoutPanel1).Margin = new Padding(2);
		((Control)tableLayoutPanel1).Name = "tableLayoutPanel1";
		((Control)tableLayoutPanel1).Padding = new Padding(2);
		tableLayoutPanel1.RowCount = 1;
		tableLayoutPanel1.RowStyles.Add(new RowStyle((SizeType)2, 100f));
		((Control)tableLayoutPanel1).Size = new Size(440, 46);
		((Control)tableLayoutPanel1).TabIndex = 0;
		((Control)btnCancel).AccessibleRole = (AccessibleRole)43;
		btnCancel.ColorTable = (eButtonColor)6;
		((Control)btnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		btnCancel.Image = (Image)(object)Resources.stop;
		btnCancel.ImagePosition = (eImagePosition)1;
		((Control)btnCancel).Location = new Point(329, 4);
		((Control)btnCancel).Margin = new Padding(2);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).RightToLeft = (RightToLeft)1;
		((Control)btnCancel).Size = new Size(105, 38);
		((PopupItemControl)btnCancel).Style = (eDotNetBarStyle)9;
		((Control)btnCancel).TabIndex = 4;
		((Control)btnCancel).TabStop = false;
		((Control)btnCancel).Text = "[لغو] (Esc)";
		((Control)btnCancel).Click += BtnCancel_Click;
		((Control)btnBack).AccessibleRole = (AccessibleRole)43;
		btnBack.ColorTable = (eButtonColor)6;
		((Control)btnBack).Enabled = false;
		((Control)btnBack).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		btnBack.Image = (Image)(object)Resources.next2;
		btnBack.ImagePosition = (eImagePosition)1;
		((Control)btnBack).Location = new Point(221, 4);
		((Control)btnBack).Margin = new Padding(2);
		((Control)btnBack).Name = "btnBack";
		((Control)btnBack).RightToLeft = (RightToLeft)1;
		((Control)btnBack).Size = new Size(104, 38);
		((PopupItemControl)btnBack).Style = (eDotNetBarStyle)9;
		((Control)btnBack).TabIndex = 4;
		((Control)btnBack).TabStop = false;
		((Control)btnBack).Text = "[قبل]";
		((Control)btnBack).Click += BtnBack_Click;
		((Control)btnNext).AccessibleRole = (AccessibleRole)43;
		btnNext.ColorTable = (eButtonColor)6;
		((Control)btnNext).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		btnNext.Image = (Image)(object)Resources.back2;
		btnNext.ImagePosition = (eImagePosition)1;
		((Control)btnNext).Location = new Point(112, 4);
		((Control)btnNext).Margin = new Padding(2);
		((Control)btnNext).Name = "btnNext";
		((Control)btnNext).RightToLeft = (RightToLeft)1;
		((Control)btnNext).Size = new Size(105, 38);
		((PopupItemControl)btnNext).Style = (eDotNetBarStyle)9;
		((Control)btnNext).TabIndex = 4;
		((Control)btnNext).TabStop = false;
		((Control)btnNext).Text = "[بعدی]";
		((Control)btnNext).Click += BtnNext_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Enabled = false;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(4, 4);
		((Control)BtnAccept).Margin = new Padding(2);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(104, 38);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 3;
		((Control)BtnAccept).Text = "[ذخیره] (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 562);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmCheckoutMain";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "FrmCheckoutMain";
		((Form)this).Load += FrmCheckoutMain_Load;
		((Control)PanelEx1).ResumeLayout(false);
		((Control)pnlLeft).ResumeLayout(false);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)pnlBottom).ResumeLayout(false);
		((Control)tableLayoutPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	bool ICheckout.get_TopLevel()
	{
		return ((Form)this).TopLevel;
	}

	void ICheckout.set_TopLevel(bool value)
	{
		((Form)this).TopLevel = value;
	}

	void ICheckout.Show(IWin32Window owner)
	{
		((Form)this).Show(owner);
	}
}
