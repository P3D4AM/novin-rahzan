using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using EntityCache.Business;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.UC.Dashboard;

public class UcCustomerSummary : UserControl
{
	private IContainer components;

	private PanelEx pnlHeader;

	private Label lblCreditSum;

	private Label lblCreditCount;

	private Label lblBiHesabCount;

	private Label lblDebitSum;

	private Label lblDebitCount;

	private Label lblAllCount;

	private Label lblCreditTitle;

	private Label lblNoneCountTitle;

	private Label lblDebitTitle;

	private Label lblAllTitle;

	public event Action<decimal> OnCustomerSelected;

	public UcCustomerSummary()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	public async Task InitAsync(string contextGuid)
	{
		try
		{
			IEnumerable<CusCompactBusiness> allCustomers = await CusCompactBusiness.GetAllAsync(contextGuid, EnSystem.UserInterface);
			allCustomers = allCustomers?.Where((CusCompactBusiness q) => q.Status);
			if (!(allCustomers?.Any() ?? false))
			{
				return;
			}
			IEnumerable<CusCompactBusiness> noneCountCustomers = allCustomers?.Where((CusCompactBusiness q) => q.Account == 0m);
			IEnumerable<CusCompactBusiness> creditCustomers = allCustomers?.Where((CusCompactBusiness q) => q.Account > 0m);
			IEnumerable<CusCompactBusiness> debitCustomers = allCustomers?.Where((CusCompactBusiness q) => q.Account < 0m);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					((Control)lblAllCount).Text = $"({allCustomers?.Count() ?? 0})";
					((Control)lblBiHesabCount).Text = $"({noneCountCustomers?.Count() ?? 0})";
					((Control)lblCreditCount).Text = $"({creditCustomers?.Count() ?? 0})";
					((Control)lblCreditSum).Text = $"{creditCustomers?.Sum((CusCompactBusiness q) => q.Account) ?? 0m:N0}";
					((Control)lblDebitCount).Text = $"({debitCustomers?.Count() ?? 0})";
					((Control)lblDebitSum).Text = $"{-(debitCustomers?.Sum((CusCompactBusiness q) => q.Account) ?? 0m):N0}";
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcCustomerSummary.cs", 42);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcCustomerSummary.cs", 45);
		}
	}

	private void lblAllTitle_Click(object sender, EventArgs e)
	{
		this.OnCustomerSelected?.Invoke(0m);
	}

	private void lblNoneCountTitle_Click(object sender, EventArgs e)
	{
		this.OnCustomerSelected?.Invoke(0m);
	}

	private void lblDebitTitle_Click(object sender, EventArgs e)
	{
		this.OnCustomerSelected?.Invoke(-((Control)lblDebitSum).Text.ParseToDecimal());
	}

	private void lblCreditTitle_Click(object sender, EventArgs e)
	{
		this.OnCustomerSelected?.Invoke(((Control)lblCreditSum).Text.ParseToDecimal());
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
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
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Expected O, but got Unknown
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Expected O, but got Unknown
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Expected O, but got Unknown
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0799: Expected O, but got Unknown
		//IL_0861: Unknown result type (might be due to invalid IL or missing references)
		//IL_086b: Expected O, but got Unknown
		//IL_0933: Unknown result type (might be due to invalid IL or missing references)
		//IL_093d: Expected O, but got Unknown
		//IL_0a17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a21: Expected O, but got Unknown
		//IL_0a26: Unknown result type (might be due to invalid IL or missing references)
		pnlHeader = new PanelEx();
		lblCreditSum = new Label();
		lblCreditCount = new Label();
		lblBiHesabCount = new Label();
		lblDebitSum = new Label();
		lblDebitCount = new Label();
		lblAllCount = new Label();
		lblCreditTitle = new Label();
		lblNoneCountTitle = new Label();
		lblDebitTitle = new Label();
		lblAllTitle = new Label();
		((Control)pnlHeader).SuspendLayout();
		((Control)this).SuspendLayout();
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)lblCreditSum);
		((Control)pnlHeader).Controls.Add((Control)(object)lblCreditCount);
		((Control)pnlHeader).Controls.Add((Control)(object)lblBiHesabCount);
		((Control)pnlHeader).Controls.Add((Control)(object)lblDebitSum);
		((Control)pnlHeader).Controls.Add((Control)(object)lblDebitCount);
		((Control)pnlHeader).Controls.Add((Control)(object)lblAllCount);
		((Control)pnlHeader).Controls.Add((Control)(object)lblCreditTitle);
		((Control)pnlHeader).Controls.Add((Control)(object)lblNoneCountTitle);
		((Control)pnlHeader).Controls.Add((Control)(object)lblDebitTitle);
		((Control)pnlHeader).Controls.Add((Control)(object)lblAllTitle);
		pnlHeader.DisabledBackColor = Color.Empty;
		((Control)pnlHeader).Dock = (DockStyle)5;
		((Control)pnlHeader).Location = new Point(0, 0);
		((Control)pnlHeader).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlHeader).Name = "pnlHeader";
		((Control)pnlHeader).Size = new Size(188, 185);
		pnlHeader.Style.BackColor1.Color = Color.White;
		pnlHeader.Style.BackColor2.Color = Color.White;
		pnlHeader.Style.Border = eBorderType.SingleLine;
		pnlHeader.Style.BorderColor.Color = Color.White;
		pnlHeader.Style.BorderWidth = 0;
		pnlHeader.Style.CornerDiameter = 10;
		pnlHeader.Style.CornerType = eCornerType.Rounded;
		((Control)pnlHeader).TabIndex = 143;
		((Control)lblCreditSum).Anchor = (AnchorStyles)13;
		((Control)lblCreditSum).Font = new Font("B Yekan", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblCreditSum).ForeColor = Color.FromArgb(252, 85, 141);
		((Control)lblCreditSum).Location = new Point(0, 156);
		((Control)lblCreditSum).Name = "lblCreditSum";
		((Control)lblCreditSum).Size = new Size(185, 25);
		((Control)lblCreditSum).TabIndex = 5;
		((Control)lblCreditSum).Text = "0";
		lblCreditSum.TextAlign = (ContentAlignment)32;
		((Control)lblCreditCount).Anchor = (AnchorStyles)13;
		((Control)lblCreditCount).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblCreditCount).ForeColor = Color.FromArgb(252, 85, 141);
		((Control)lblCreditCount).Location = new Point(0, 128);
		((Control)lblCreditCount).Name = "lblCreditCount";
		((Control)lblCreditCount).Size = new Size(119, 25);
		((Control)lblCreditCount).TabIndex = 0;
		((Control)lblCreditCount).Text = "0";
		lblCreditCount.TextAlign = (ContentAlignment)64;
		((Control)lblBiHesabCount).Anchor = (AnchorStyles)13;
		((Control)lblBiHesabCount).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblBiHesabCount).ForeColor = Color.FromArgb(37, 195, 164);
		((Control)lblBiHesabCount).Location = new Point(3, 39);
		((Control)lblBiHesabCount).Name = "lblBiHesabCount";
		((Control)lblBiHesabCount).Size = new Size(108, 25);
		((Control)lblBiHesabCount).TabIndex = 0;
		((Control)lblBiHesabCount).Text = "0";
		lblBiHesabCount.TextAlign = (ContentAlignment)64;
		((Control)lblDebitSum).Anchor = (AnchorStyles)13;
		((Control)lblDebitSum).Font = new Font("B Yekan", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblDebitSum).ForeColor = Color.FromArgb(155, 90, 255);
		((Control)lblDebitSum).Location = new Point(3, 99);
		((Control)lblDebitSum).Name = "lblDebitSum";
		((Control)lblDebitSum).Size = new Size(185, 25);
		((Control)lblDebitSum).TabIndex = 0;
		((Control)lblDebitSum).Text = "0";
		lblDebitSum.TextAlign = (ContentAlignment)32;
		((Control)lblDebitCount).Anchor = (AnchorStyles)13;
		((Control)lblDebitCount).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblDebitCount).ForeColor = Color.FromArgb(155, 90, 255);
		((Control)lblDebitCount).Location = new Point(3, 72);
		((Control)lblDebitCount).Name = "lblDebitCount";
		((Control)lblDebitCount).Size = new Size(120, 25);
		((Control)lblDebitCount).TabIndex = 0;
		((Control)lblDebitCount).Text = "0";
		lblDebitCount.TextAlign = (ContentAlignment)64;
		((Control)lblAllCount).Anchor = (AnchorStyles)13;
		((Control)lblAllCount).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblAllCount).ForeColor = Color.FromArgb(66, 82, 219);
		((Control)lblAllCount).Location = new Point(3, 7);
		((Control)lblAllCount).Name = "lblAllCount";
		((Control)lblAllCount).Size = new Size(105, 25);
		((Control)lblAllCount).TabIndex = 0;
		((Control)lblAllCount).Text = "0";
		lblAllCount.TextAlign = (ContentAlignment)64;
		((Control)lblCreditTitle).Anchor = (AnchorStyles)9;
		((Control)lblCreditTitle).Cursor = Cursors.Hand;
		((Control)lblCreditTitle).Font = new Font("B Yekan", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblCreditTitle).ForeColor = Color.FromArgb(252, 85, 141);
		((Control)lblCreditTitle).Location = new Point(119, 124);
		((Control)lblCreditTitle).Name = "lblCreditTitle";
		((Control)lblCreditTitle).Size = new Size(66, 32);
		((Control)lblCreditTitle).TabIndex = 0;
		((Control)lblCreditTitle).Text = "[بستانکار]";
		lblCreditTitle.TextAlign = (ContentAlignment)16;
		((Control)lblCreditTitle).Click += lblCreditTitle_Click;
		((Control)lblNoneCountTitle).Anchor = (AnchorStyles)9;
		((Control)lblNoneCountTitle).Cursor = Cursors.Hand;
		((Control)lblNoneCountTitle).Font = new Font("B Yekan", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblNoneCountTitle).ForeColor = Color.FromArgb(37, 195, 164);
		((Control)lblNoneCountTitle).Location = new Point(106, 34);
		((Control)lblNoneCountTitle).Name = "lblNoneCountTitle";
		((Control)lblNoneCountTitle).Size = new Size(77, 32);
		((Control)lblNoneCountTitle).TabIndex = 0;
		((Control)lblNoneCountTitle).Text = "[بی حساب]";
		lblNoneCountTitle.TextAlign = (ContentAlignment)16;
		((Control)lblNoneCountTitle).Click += lblNoneCountTitle_Click;
		((Control)lblDebitTitle).Anchor = (AnchorStyles)9;
		((Control)lblDebitTitle).Cursor = Cursors.Hand;
		((Control)lblDebitTitle).Font = new Font("B Yekan", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblDebitTitle).ForeColor = Color.FromArgb(155, 90, 255);
		((Control)lblDebitTitle).Location = new Point(119, 67);
		((Control)lblDebitTitle).Name = "lblDebitTitle";
		((Control)lblDebitTitle).Size = new Size(64, 32);
		((Control)lblDebitTitle).TabIndex = 0;
		((Control)lblDebitTitle).Text = "[بدهکار]";
		lblDebitTitle.TextAlign = (ContentAlignment)16;
		((Control)lblDebitTitle).Click += lblDebitTitle_Click;
		((Control)lblAllTitle).Anchor = (AnchorStyles)9;
		((Control)lblAllTitle).Cursor = Cursors.Hand;
		((Control)lblAllTitle).Font = new Font("B Yekan", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblAllTitle).ForeColor = Color.FromArgb(66, 82, 219);
		((Control)lblAllTitle).Location = new Point(106, 2);
		((Control)lblAllTitle).Name = "lblAllTitle";
		((Control)lblAllTitle).Size = new Size(79, 32);
		((Control)lblAllTitle).TabIndex = 0;
		((Control)lblAllTitle).Text = "[کل اشخاص]";
		lblAllTitle.TextAlign = (ContentAlignment)16;
		((Control)lblAllTitle).Click += lblAllTitle_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pnlHeader);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcCustomerSummary";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(188, 185);
		((Control)pnlHeader).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
