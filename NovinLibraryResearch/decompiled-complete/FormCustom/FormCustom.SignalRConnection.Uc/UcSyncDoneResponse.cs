using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Business;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.SignalRConnection.Uc;

public class UcSyncDoneResponse : UserControl
{
	private IContainer components;

	private SplitContainer splitContainer1;

	private FlowLayoutPanel fPanelRight;

	private FlowLayoutPanel fPanelLeft;

	private UcSyncDoneResponseDetail ucProduct;

	private UcSyncDoneResponseDetail ucProductGroup;

	private UcSyncDoneResponseDetail ucCustomer;

	private UcSyncDoneResponseDetail ucCustomerGroup;

	private UcSyncDoneResponseDetail ucDepot;

	private UcSyncDoneResponseDetail ucMoein;

	private UcSyncDoneResponseDetail ucFilrInfo;

	private UcSyncDoneResponseDetail ucKol;

	private UcSyncDoneResponseDetail ucTafsil;

	private UcSyncDoneResponseDetail ucAccount;

	private UcSyncDoneResponseDetail ucReceptionCheque;

	private UcSyncDoneResponseDetail ucAghsatM;

	private UcSyncDoneResponseDetail ucPardakhtCheque;

	private UcSyncDoneResponseDetail ucAghsatSh;

	public async Task InitAsync(string contextGuid, CancellationToken token)
	{
		try
		{
			UserInformationBusiness user = await UserInformationBusiness.GetAsync(contextGuid);
			if (user == null || ((Control)this).FindForm() == null || ((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)((Control)this).FindForm()).IsHandleCreated)
			{
				await Task.Delay(100);
				if (((Control)this).FindForm() == null || ((Control)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (((Control)this).FindForm() == null || ((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)((Control)this).FindForm()).BeginInvoke((Delegate)(Action)delegate
			{
				if (((Control)this).FindForm() == null || ((Control)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					ucProduct.Date = user.SyncInfo.SyncInfo.Product.TimeDifferenceToString();
					ucProduct.Percent = (int)user.SyncInfo.SyncInfo.ProductPercentage();
					ucProductGroup.Date = user.SyncInfo.SyncInfo.ProductGroup.TimeDifferenceToString();
					ucProductGroup.Percent = (int)user.SyncInfo.SyncInfo.ProductGroupPercentage();
					ucDepot.Date = user.SyncInfo.SyncInfo.Depot.TimeDifferenceToString();
					ucDepot.Percent = (int)user.SyncInfo.SyncInfo.DepotPercentage();
					ucMoein.Date = user.SyncInfo.SyncInfo.Moein.TimeDifferenceToString();
					ucMoein.Percent = (int)user.SyncInfo.SyncInfo.MoeinPercentage();
					ucFilrInfo.Date = user.SyncInfo.SyncInfo.FileInfo.TimeDifferenceToString();
					ucFilrInfo.Percent = (int)user.SyncInfo.SyncInfo.FileInfoPercentage();
					ucReceptionCheque.Date = user.SyncInfo.SyncInfo.ViewCheckM.TimeDifferenceToString();
					ucReceptionCheque.Percent = (int)user.SyncInfo.SyncInfo.ViewChequeMPercentage();
					ucAghsatM.Date = user.SyncInfo.SyncInfo.AghsatAshkhas.TimeDifferenceToString();
					ucAghsatM.Percent = (int)user.SyncInfo.SyncInfo.AghsatMPercentage();
					ucCustomer.Date = user.SyncInfo.SyncInfo.Customer.TimeDifferenceToString();
					ucCustomer.Percent = (int)user.SyncInfo.SyncInfo.CustomerPercentage();
					ucCustomerGroup.Date = user.SyncInfo.SyncInfo.CustomerGroup.TimeDifferenceToString();
					ucCustomerGroup.Percent = (int)user.SyncInfo.SyncInfo.CustomerGroupPercentage();
					ucKol.Date = user.SyncInfo.SyncInfo.Kol.TimeDifferenceToString();
					ucKol.Percent = (int)user.SyncInfo.SyncInfo.KolPercentage();
					ucTafsil.Date = user.SyncInfo.SyncInfo.Tafsil.TimeDifferenceToString();
					ucTafsil.Percent = (int)user.SyncInfo.SyncInfo.TafsilPercentage();
					ucAccount.Date = user.SyncInfo.SyncInfo.Account.TimeDifferenceToString();
					ucAccount.Percent = (int)user.SyncInfo.SyncInfo.AccountPercentage();
					ucPardakhtCheque.Date = user.SyncInfo.SyncInfo.ViewCheckSh.TimeDifferenceToString();
					ucPardakhtCheque.Percent = (int)user.SyncInfo.SyncInfo.ViewChequeShPercentage();
					ucAghsatSh.Date = user.SyncInfo.SyncInfo.AghsatShakhsi.TimeDifferenceToString();
					ucAghsatSh.Percent = (int)user.SyncInfo.SyncInfo.AghsatShPercentage();
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncDoneResponse.cs", 72);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncDoneResponse.cs", 75);
		}
	}

	public UcSyncDoneResponse()
	{
		try
		{
			InitializeComponent();
			((Control)(object)this).Transelate();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncDoneResponse.cs", 85);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Expected O, but got Unknown
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_063e: Expected O, but got Unknown
		//IL_065f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0705: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Expected O, but got Unknown
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Expected O, but got Unknown
		//IL_0801: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ab: Expected O, but got Unknown
		//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a78: Expected O, but got Unknown
		//IL_0a96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b46: Expected O, but got Unknown
		//IL_0b64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c14: Expected O, but got Unknown
		//IL_0c35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce5: Expected O, but got Unknown
		//IL_0d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db6: Expected O, but got Unknown
		//IL_0dd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e87: Expected O, but got Unknown
		//IL_0ea8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6b: Expected O, but got Unknown
		//IL_0f70: Unknown result type (might be due to invalid IL or missing references)
		splitContainer1 = new SplitContainer();
		fPanelRight = new FlowLayoutPanel();
		ucProduct = new UcSyncDoneResponseDetail();
		ucProductGroup = new UcSyncDoneResponseDetail();
		ucDepot = new UcSyncDoneResponseDetail();
		ucMoein = new UcSyncDoneResponseDetail();
		ucFilrInfo = new UcSyncDoneResponseDetail();
		ucReceptionCheque = new UcSyncDoneResponseDetail();
		ucAghsatM = new UcSyncDoneResponseDetail();
		fPanelLeft = new FlowLayoutPanel();
		ucCustomer = new UcSyncDoneResponseDetail();
		ucCustomerGroup = new UcSyncDoneResponseDetail();
		ucKol = new UcSyncDoneResponseDetail();
		ucTafsil = new UcSyncDoneResponseDetail();
		ucAccount = new UcSyncDoneResponseDetail();
		ucPardakhtCheque = new UcSyncDoneResponseDetail();
		ucAghsatSh = new UcSyncDoneResponseDetail();
		((ISupportInitialize)splitContainer1).BeginInit();
		((Control)splitContainer1.Panel1).SuspendLayout();
		((Control)splitContainer1.Panel2).SuspendLayout();
		((Control)splitContainer1).SuspendLayout();
		((Control)fPanelRight).SuspendLayout();
		((Control)fPanelLeft).SuspendLayout();
		((Control)this).SuspendLayout();
		splitContainer1.Dock = (DockStyle)5;
		((Control)splitContainer1).Location = new Point(0, 0);
		((Control)splitContainer1).Name = "splitContainer1";
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)fPanelRight);
		((Control)splitContainer1.Panel1).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)fPanelLeft);
		((Control)splitContainer1.Panel2).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1).Size = new Size(697, 436);
		splitContainer1.SplitterDistance = 344;
		((Control)splitContainer1).TabIndex = 0;
		((ScrollableControl)fPanelRight).AutoScroll = true;
		((Control)fPanelRight).Controls.Add((Control)(object)ucProduct);
		((Control)fPanelRight).Controls.Add((Control)(object)ucProductGroup);
		((Control)fPanelRight).Controls.Add((Control)(object)ucDepot);
		((Control)fPanelRight).Controls.Add((Control)(object)ucMoein);
		((Control)fPanelRight).Controls.Add((Control)(object)ucFilrInfo);
		((Control)fPanelRight).Controls.Add((Control)(object)ucReceptionCheque);
		((Control)fPanelRight).Controls.Add((Control)(object)ucAghsatM);
		((Control)fPanelRight).Dock = (DockStyle)5;
		((Control)fPanelRight).Location = new Point(0, 0);
		((Control)fPanelRight).Name = "fPanelRight";
		((Control)fPanelRight).Size = new Size(344, 436);
		((Control)fPanelRight).TabIndex = 3;
		((Control)ucProduct).BackColor = Color.Transparent;
		ucProduct.Date = "---";
		((Control)ucProduct).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucProduct).Location = new Point(22, 5);
		((Control)ucProduct).Margin = new Padding(4, 5, 4, 5);
		((Control)ucProduct).Name = "ucProduct";
		ucProduct.Percent = new decimal(new int[4]);
		((Control)ucProduct).RightToLeft = (RightToLeft)1;
		((Control)ucProduct).Size = new Size(318, 45);
		((Control)ucProduct).TabIndex = 0;
		ucProduct.Title = "کــالــاها";
		((Control)ucProductGroup).BackColor = Color.Transparent;
		ucProductGroup.Date = "---";
		((Control)ucProductGroup).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucProductGroup).Location = new Point(22, 60);
		((Control)ucProductGroup).Margin = new Padding(4, 5, 4, 5);
		((Control)ucProductGroup).Name = "ucProductGroup";
		ucProductGroup.Percent = new decimal(new int[4]);
		((Control)ucProductGroup).RightToLeft = (RightToLeft)1;
		((Control)ucProductGroup).Size = new Size(318, 45);
		((Control)ucProductGroup).TabIndex = 0;
		ucProductGroup.Title = "گروه کالاها";
		((Control)ucDepot).BackColor = Color.Transparent;
		ucDepot.Date = "---";
		((Control)ucDepot).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucDepot).Location = new Point(22, 115);
		((Control)ucDepot).Margin = new Padding(4, 5, 4, 5);
		((Control)ucDepot).Name = "ucDepot";
		ucDepot.Percent = new decimal(new int[4]);
		((Control)ucDepot).RightToLeft = (RightToLeft)1;
		((Control)ucDepot).Size = new Size(318, 45);
		((Control)ucDepot).TabIndex = 0;
		ucDepot.Title = "انبـــــــارها";
		((Control)ucMoein).BackColor = Color.Transparent;
		ucMoein.Date = "---";
		((Control)ucMoein).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucMoein).Location = new Point(22, 170);
		((Control)ucMoein).Margin = new Padding(4, 5, 4, 5);
		((Control)ucMoein).Name = "ucMoein";
		ucMoein.Percent = new decimal(new int[4]);
		((Control)ucMoein).RightToLeft = (RightToLeft)1;
		((Control)ucMoein).Size = new Size(318, 45);
		((Control)ucMoein).TabIndex = 0;
		ucMoein.Title = "دفتــــر معین";
		((Control)ucFilrInfo).BackColor = Color.Transparent;
		ucFilrInfo.Date = "---";
		((Control)ucFilrInfo).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucFilrInfo).Location = new Point(22, 225);
		((Control)ucFilrInfo).Margin = new Padding(4, 5, 4, 5);
		((Control)ucFilrInfo).Name = "ucFilrInfo";
		ucFilrInfo.Percent = new decimal(new int[4]);
		((Control)ucFilrInfo).RightToLeft = (RightToLeft)1;
		((Control)ucFilrInfo).Size = new Size(318, 45);
		((Control)ucFilrInfo).TabIndex = 0;
		ucFilrInfo.Title = "تصـــــــــاویر";
		((Control)ucReceptionCheque).BackColor = Color.Transparent;
		ucReceptionCheque.Date = "---";
		((Control)ucReceptionCheque).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucReceptionCheque).Location = new Point(22, 280);
		((Control)ucReceptionCheque).Margin = new Padding(4, 5, 4, 5);
		((Control)ucReceptionCheque).Name = "ucReceptionCheque";
		ucReceptionCheque.Percent = new decimal(new int[4]);
		((Control)ucReceptionCheque).RightToLeft = (RightToLeft)1;
		((Control)ucReceptionCheque).Size = new Size(318, 45);
		((Control)ucReceptionCheque).TabIndex = 0;
		ucReceptionCheque.Title = "اسناد دریافتنی";
		((Control)ucAghsatM).BackColor = Color.Transparent;
		ucAghsatM.Date = "---";
		((Control)ucAghsatM).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucAghsatM).Location = new Point(22, 335);
		((Control)ucAghsatM).Margin = new Padding(4, 5, 4, 5);
		((Control)ucAghsatM).Name = "ucAghsatM";
		ucAghsatM.Percent = new decimal(new int[4]);
		((Control)ucAghsatM).RightToLeft = (RightToLeft)1;
		((Control)ucAghsatM).Size = new Size(318, 45);
		((Control)ucAghsatM).TabIndex = 1;
		ucAghsatM.Title = "اقساط دریافتنی";
		((ScrollableControl)fPanelLeft).AutoScroll = true;
		((Control)fPanelLeft).Controls.Add((Control)(object)ucCustomer);
		((Control)fPanelLeft).Controls.Add((Control)(object)ucCustomerGroup);
		((Control)fPanelLeft).Controls.Add((Control)(object)ucKol);
		((Control)fPanelLeft).Controls.Add((Control)(object)ucTafsil);
		((Control)fPanelLeft).Controls.Add((Control)(object)ucAccount);
		((Control)fPanelLeft).Controls.Add((Control)(object)ucPardakhtCheque);
		((Control)fPanelLeft).Controls.Add((Control)(object)ucAghsatSh);
		((Control)fPanelLeft).Dock = (DockStyle)5;
		((Control)fPanelLeft).Location = new Point(0, 0);
		((Control)fPanelLeft).Name = "fPanelLeft";
		((Control)fPanelLeft).Size = new Size(349, 436);
		((Control)fPanelLeft).TabIndex = 3;
		((Control)ucCustomer).BackColor = Color.Transparent;
		ucCustomer.Date = "---";
		((Control)ucCustomer).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucCustomer).Location = new Point(27, 5);
		((Control)ucCustomer).Margin = new Padding(4, 5, 4, 5);
		((Control)ucCustomer).Name = "ucCustomer";
		ucCustomer.Percent = new decimal(new int[4]);
		((Control)ucCustomer).RightToLeft = (RightToLeft)1;
		((Control)ucCustomer).Size = new Size(318, 45);
		((Control)ucCustomer).TabIndex = 0;
		ucCustomer.Title = "اشخــــــاص";
		((Control)ucCustomerGroup).BackColor = Color.Transparent;
		ucCustomerGroup.Date = "---";
		((Control)ucCustomerGroup).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucCustomerGroup).Location = new Point(27, 60);
		((Control)ucCustomerGroup).Margin = new Padding(4, 5, 4, 5);
		((Control)ucCustomerGroup).Name = "ucCustomerGroup";
		ucCustomerGroup.Percent = new decimal(new int[4]);
		((Control)ucCustomerGroup).RightToLeft = (RightToLeft)1;
		((Control)ucCustomerGroup).Size = new Size(318, 45);
		((Control)ucCustomerGroup).TabIndex = 0;
		ucCustomerGroup.Title = "گروه اشخاص";
		((Control)ucKol).BackColor = Color.Transparent;
		ucKol.Date = "---";
		((Control)ucKol).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucKol).Location = new Point(27, 115);
		((Control)ucKol).Margin = new Padding(4, 5, 4, 5);
		((Control)ucKol).Name = "ucKol";
		ucKol.Percent = new decimal(new int[4]);
		((Control)ucKol).RightToLeft = (RightToLeft)1;
		((Control)ucKol).Size = new Size(318, 45);
		((Control)ucKol).TabIndex = 0;
		ucKol.Title = "دفتــــر کـــل";
		((Control)ucTafsil).BackColor = Color.Transparent;
		ucTafsil.Date = "---";
		((Control)ucTafsil).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucTafsil).Location = new Point(27, 170);
		((Control)ucTafsil).Margin = new Padding(4, 5, 4, 5);
		((Control)ucTafsil).Name = "ucTafsil";
		ucTafsil.Percent = new decimal(new int[4]);
		((Control)ucTafsil).RightToLeft = (RightToLeft)1;
		((Control)ucTafsil).Size = new Size(318, 45);
		((Control)ucTafsil).TabIndex = 0;
		ucTafsil.Title = "دفتـر تفصیلی";
		((Control)ucAccount).BackColor = Color.Transparent;
		ucAccount.Date = "---";
		((Control)ucAccount).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucAccount).Location = new Point(27, 225);
		((Control)ucAccount).Margin = new Padding(4, 5, 4, 5);
		((Control)ucAccount).Name = "ucAccount";
		ucAccount.Percent = new decimal(new int[4]);
		((Control)ucAccount).RightToLeft = (RightToLeft)1;
		((Control)ucAccount).Size = new Size(318, 45);
		((Control)ucAccount).TabIndex = 0;
		ucAccount.Title = "مانده حساب ها";
		((Control)ucPardakhtCheque).BackColor = Color.Transparent;
		ucPardakhtCheque.Date = "---";
		((Control)ucPardakhtCheque).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucPardakhtCheque).Location = new Point(27, 280);
		((Control)ucPardakhtCheque).Margin = new Padding(4, 5, 4, 5);
		((Control)ucPardakhtCheque).Name = "ucPardakhtCheque";
		ucPardakhtCheque.Percent = new decimal(new int[4]);
		((Control)ucPardakhtCheque).RightToLeft = (RightToLeft)1;
		((Control)ucPardakhtCheque).Size = new Size(318, 45);
		((Control)ucPardakhtCheque).TabIndex = 0;
		ucPardakhtCheque.Title = "اسناد پرداختنی";
		((Control)ucAghsatSh).BackColor = Color.Transparent;
		ucAghsatSh.Date = "---";
		((Control)ucAghsatSh).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucAghsatSh).Location = new Point(27, 335);
		((Control)ucAghsatSh).Margin = new Padding(4, 5, 4, 5);
		((Control)ucAghsatSh).Name = "ucAghsatSh";
		ucAghsatSh.Percent = new decimal(new int[4]);
		((Control)ucAghsatSh).RightToLeft = (RightToLeft)1;
		((Control)ucAghsatSh).Size = new Size(318, 45);
		((Control)ucAghsatSh).TabIndex = 1;
		ucAghsatSh.Title = "اقساط پرداختنی";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)splitContainer1);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcSyncDoneResponse";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(697, 436);
		((Control)splitContainer1.Panel1).ResumeLayout(false);
		((Control)splitContainer1.Panel2).ResumeLayout(false);
		((ISupportInitialize)splitContainer1).EndInit();
		((Control)splitContainer1).ResumeLayout(false);
		((Control)fPanelRight).ResumeLayout(false);
		((Control)fPanelLeft).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
