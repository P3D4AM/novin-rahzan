using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using EntityCache.Business.Accounting.Cheque;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.UC;

public class UcCHequeReport : UserControl
{
	private IContainer components;

	private PanelEx pnlHeader;

	private Label lblBank;

	private Label lblPrice;

	private Label lblNumber;

	private Label lblSarresid;

	private Label lblTafsilName;

	private Label lblType;

	private PictureBox picBox;

	private ContextMenuStrip ContextMenu;

	public string ContextGuid { get; private set; }

	public BedehiTalabReportBusiness Config { get; private set; }

	public event Action<Guid> OnTafsilSelected;

	public event Func<HesabType?, BedehiTalabAction, Task> SelectTafsilGuid;

	public event Func<Guid, EnBedehiTalabType, Task> OnElamPardakht;

	public event Func<Guid, EnBedehiTalabType, Task> OnRemove;

	public event Func<Guid, EnBedehiTalabType, Task> OnEdit;

	public event Func<Guid, EnBedehiTalabType, Task> OnShowSanad;

	private void SetColor(Color color)
	{
		try
		{
			pnlHeader.Style.BackColor1.Color = color;
			pnlHeader.Style.BackColor2.Color = color;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetColor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcCHequeReport.cs", 31);
		}
	}

	private void SetLabelColor(Color color)
	{
		try
		{
			Label obj = lblType;
			Color foreColor = (((Control)lblTafsilName).ForeColor = color);
			((Control)obj).ForeColor = foreColor;
			Label obj2 = lblSarresid;
			foreColor = (((Control)lblNumber).ForeColor = color);
			((Control)obj2).ForeColor = foreColor;
			Label obj3 = lblPrice;
			foreColor = (((Control)lblBank).ForeColor = color);
			((Control)obj3).ForeColor = foreColor;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetLabelColor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcCHequeReport.cs", 41);
		}
	}

	public UcCHequeReport()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	public void Init(string contextGuid, BedehiTalabReportBusiness config)
	{
		try
		{
			ContextGuid = contextGuid;
			Config = config;
			if (config == null)
			{
				((Control)picBox).Visible = false;
				((Control)lblType).Text = "نوع";
				((Control)lblTafsilName).Text = "طرف حساب";
				((Control)lblSarresid).Text = "سررسید";
				((Control)lblNumber).Text = "شماره";
				((Control)lblPrice).Text = "مبلغ";
				((Control)lblBank).Text = "مقصد";
				return;
			}
			Label obj = lblType;
			string text = (((Control)lblType).Text = config.TypeName);
			((Control)obj).Text = text + " (" + config.StatusStr + ")";
			((Control)lblTafsilName).Text = config.TafsilName;
			((Control)lblSarresid).Text = config.DateSh;
			((Control)lblNumber).Text = config.Number;
			((Control)lblPrice).Text = config.Price.NpToString("N0", 0);
			((Control)lblBank).Text = config.BankName;
			switch (config.Type)
			{
			case EnBedehiTalabType.CheckM:
				SetColor(Color.FromArgb(99, 228, 166));
				SetLabelColor(Color.Black);
				break;
			case EnBedehiTalabType.CheckSh:
				SetColor(Color.FromArgb(254, 101, 135));
				SetLabelColor(Color.White);
				break;
			case EnBedehiTalabType.AghsatM:
				SetColor(Color.FromArgb(254, 207, 83));
				SetLabelColor(Color.Black);
				break;
			case EnBedehiTalabType.AghsatSh:
				SetColor(Color.FromArgb(65, 81, 220));
				SetLabelColor(Color.White);
				break;
			}
			SetContextMenu();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcCHequeReport.cs", 96);
		}
	}

	private void picBox_Click(object sender, EventArgs e)
	{
		try
		{
			((ToolStripDropDown)ContextMenu).Show(Cursor.Position.X, Cursor.Position.Y);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picBox_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcCHequeReport.cs", 104);
		}
	}

	private void SetContextMenu()
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		try
		{
			((ToolStrip)ContextMenu).Items.Clear();
			List<BedehiTalabAction> actions = Config.Actions;
			if (actions != null && actions.Any())
			{
				foreach (BedehiTalabAction action in Config.Actions)
				{
					ToolStripMenuItem val = new ToolStripMenuItem(action.Operation.GetDisplay().GetTranslate(), (Image)null, (EventHandler)MenuItem_Click);
					((ToolStrip)ContextMenu).Items.Add((ToolStripItem)(object)val);
				}
				return;
			}
			EnBedehiTalabType type = Config.Type;
			if ((uint)(type - 3) <= 1u)
			{
				List<ToolStripMenuItem> list = new List<ToolStripMenuItem>
				{
					new ToolStripMenuItem("اعلام پرداخت", (Image)null, (EventHandler)ElamPardakht_Click),
					new ToolStripMenuItem("حذف", (Image)null, (EventHandler)Remove_Click),
					new ToolStripMenuItem("ویرایش", (Image)null, (EventHandler)Edit_Click),
					new ToolStripMenuItem("مشاهده سند دریافت", (Image)null, (EventHandler)Sanad_Click)
				};
				ToolStripItemCollection items = ((ToolStrip)ContextMenu).Items;
				ToolStripItem[] array = (ToolStripItem[])(object)list.ToArray();
				items.AddRange(array);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetContextMenu", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcCHequeReport.cs", 135);
		}
	}

	private void MenuItem_Click(object sender, EventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		try
		{
			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			BedehiTalabAction bedehiTalabAction = Config?.Actions?.FirstOrDefault((BedehiTalabAction q) => q.Operation.GetDisplay() == ((ToolStripItem)menu).Text);
			if (bedehiTalabAction != null)
			{
				switch (bedehiTalabAction.Operation)
				{
				case EnChequeOperation.VagozarKardan:
					this.SelectTafsilGuid?.Invoke(HesabType.Bank, bedehiTalabAction);
					break;
				case EnChequeOperation.NaghdKardan:
					this.SelectTafsilGuid?.Invoke(HesabType.Sandogh, bedehiTalabAction);
					break;
				case EnChequeOperation.BargashtZadan:
				case EnChequeOperation.BatelKardan:
				case EnChequeOperation.BargashtoToEx:
				case EnChequeOperation.OdatBargashti:
				case EnChequeOperation.OdatKharjShode:
				case EnChequeOperation.VagozarToMojod:
				case EnChequeOperation.VosolKardan:
				case EnChequeOperation.NaghdToMojod:
				case EnChequeOperation.BatelToMojod:
				case EnChequeOperation.VosolToVagozar:
				case EnChequeOperation.ElamPardakht:
				case EnChequeOperation.PardakhtShodeToPardakhtani:
				case EnChequeOperation.BatelToPardakhtani:
				case EnChequeOperation.BargashtiToPardakhtani:
					this.SelectTafsilGuid?.Invoke(null, bedehiTalabAction);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MenuItem_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcCHequeReport.cs", 170);
		}
	}

	private void lblTafsilName_Click(object sender, EventArgs e)
	{
		this.OnTafsilSelected?.Invoke(Config.TafsilGuid);
	}

	private void ElamPardakht_Click(object sender, EventArgs e)
	{
		this.OnElamPardakht?.Invoke(Config.Guid, Config.Type);
	}

	private void Remove_Click(object sender, EventArgs e)
	{
		this.OnRemove?.Invoke(Config.Guid, Config.Type);
	}

	private void Edit_Click(object sender, EventArgs e)
	{
		this.OnEdit?.Invoke(Config.Guid, Config.Type);
	}

	private void Sanad_Click(object sender, EventArgs e)
	{
		this.OnShowSanad?.Invoke(Config.Guid, Config.Type);
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
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
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Expected O, but got Unknown
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Expected O, but got Unknown
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_075f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Expected O, but got Unknown
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Expected O, but got Unknown
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0888: Expected O, but got Unknown
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		pnlHeader = new PanelEx();
		picBox = new PictureBox();
		lblBank = new Label();
		lblPrice = new Label();
		lblNumber = new Label();
		lblSarresid = new Label();
		lblTafsilName = new Label();
		lblType = new Label();
		ContextMenu = new ContextMenuStrip(components);
		((Control)pnlHeader).SuspendLayout();
		((ISupportInitialize)picBox).BeginInit();
		((Control)this).SuspendLayout();
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)picBox);
		((Control)pnlHeader).Controls.Add((Control)(object)lblBank);
		((Control)pnlHeader).Controls.Add((Control)(object)lblPrice);
		((Control)pnlHeader).Controls.Add((Control)(object)lblNumber);
		((Control)pnlHeader).Controls.Add((Control)(object)lblSarresid);
		((Control)pnlHeader).Controls.Add((Control)(object)lblTafsilName);
		((Control)pnlHeader).Controls.Add((Control)(object)lblType);
		pnlHeader.DisabledBackColor = Color.Empty;
		((Control)pnlHeader).Dock = (DockStyle)5;
		((Control)pnlHeader).Location = new Point(0, 0);
		((Control)pnlHeader).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlHeader).Name = "pnlHeader";
		((Control)pnlHeader).Size = new Size(653, 37);
		pnlHeader.Style.BackColor1.Color = Color.White;
		pnlHeader.Style.BackColor2.Color = Color.White;
		pnlHeader.Style.Border = eBorderType.SingleLine;
		pnlHeader.Style.BorderColor.Color = Color.White;
		pnlHeader.Style.BorderWidth = 0;
		pnlHeader.Style.CornerDiameter = 10;
		pnlHeader.Style.CornerType = eCornerType.Rounded;
		((Control)pnlHeader).TabIndex = 139;
		((Control)picBox).Cursor = Cursors.Hand;
		picBox.Image = (Image)(object)FormCustom.Properties.Resources.More;
		((Control)picBox).Location = new Point(-4, 5);
		((Control)picBox).Name = "picBox";
		((Control)picBox).Size = new Size(25, 25);
		picBox.SizeMode = (PictureBoxSizeMode)1;
		picBox.TabIndex = 143;
		picBox.TabStop = false;
		((Control)picBox).Click += picBox_Click;
		((Control)lblBank).Anchor = (AnchorStyles)7;
		((Control)lblBank).BackColor = Color.Transparent;
		((Control)lblBank).Cursor = Cursors.Default;
		((Control)lblBank).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblBank).ForeColor = Color.Gray;
		((Control)lblBank).Location = new Point(20, 0);
		((Control)lblBank).Margin = new Padding(4, 0, 4, 0);
		((Control)lblBank).Name = "lblBank";
		((Control)lblBank).Size = new Size(134, 37);
		((Control)lblBank).TabIndex = 8;
		((Control)lblBank).Text = "---";
		lblBank.TextAlign = (ContentAlignment)32;
		((Control)lblPrice).Anchor = (AnchorStyles)7;
		((Control)lblPrice).BackColor = Color.Transparent;
		((Control)lblPrice).Cursor = Cursors.Default;
		((Control)lblPrice).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblPrice).ForeColor = Color.Gray;
		((Control)lblPrice).Location = new Point(152, 0);
		((Control)lblPrice).Margin = new Padding(4, 0, 4, 0);
		((Control)lblPrice).Name = "lblPrice";
		((Control)lblPrice).Size = new Size(104, 37);
		((Control)lblPrice).TabIndex = 8;
		((Control)lblPrice).Text = "---";
		lblPrice.TextAlign = (ContentAlignment)32;
		((Control)lblNumber).Anchor = (AnchorStyles)7;
		((Control)lblNumber).BackColor = Color.Transparent;
		((Control)lblNumber).Cursor = Cursors.Default;
		((Control)lblNumber).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblNumber).ForeColor = Color.Gray;
		((Control)lblNumber).Location = new Point(264, 0);
		((Control)lblNumber).Margin = new Padding(4, 0, 4, 0);
		((Control)lblNumber).Name = "lblNumber";
		((Control)lblNumber).Size = new Size(60, 37);
		((Control)lblNumber).TabIndex = 8;
		((Control)lblNumber).Text = "---";
		lblNumber.TextAlign = (ContentAlignment)32;
		((Control)lblSarresid).Anchor = (AnchorStyles)7;
		((Control)lblSarresid).BackColor = Color.Transparent;
		((Control)lblSarresid).Cursor = Cursors.Default;
		((Control)lblSarresid).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblSarresid).ForeColor = Color.Gray;
		((Control)lblSarresid).Location = new Point(332, 0);
		((Control)lblSarresid).Margin = new Padding(4, 0, 4, 0);
		((Control)lblSarresid).Name = "lblSarresid";
		((Control)lblSarresid).Size = new Size(66, 37);
		((Control)lblSarresid).TabIndex = 8;
		((Control)lblSarresid).Text = "---";
		lblSarresid.TextAlign = (ContentAlignment)32;
		((Control)lblTafsilName).Anchor = (AnchorStyles)15;
		((Control)lblTafsilName).BackColor = Color.Transparent;
		((Control)lblTafsilName).Cursor = Cursors.Hand;
		((Control)lblTafsilName).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblTafsilName).ForeColor = Color.Gray;
		((Control)lblTafsilName).Location = new Point(398, 0);
		((Control)lblTafsilName).Margin = new Padding(4, 0, 4, 0);
		((Control)lblTafsilName).Name = "lblTafsilName";
		((Control)lblTafsilName).Size = new Size(144, 37);
		((Control)lblTafsilName).TabIndex = 7;
		((Control)lblTafsilName).Text = "---";
		lblTafsilName.TextAlign = (ContentAlignment)32;
		((Control)lblTafsilName).Click += lblTafsilName_Click;
		((Control)lblType).Anchor = (AnchorStyles)11;
		((Control)lblType).BackColor = Color.Transparent;
		((Control)lblType).Cursor = Cursors.Default;
		((Control)lblType).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblType).ForeColor = Color.Gray;
		((Control)lblType).Location = new Point(550, 0);
		((Control)lblType).Margin = new Padding(4, 0, 4, 0);
		((Control)lblType).Name = "lblType";
		((Control)lblType).Size = new Size(101, 37);
		((Control)lblType).TabIndex = 6;
		((Control)lblType).Text = "---";
		lblType.TextAlign = (ContentAlignment)16;
		((Control)ContextMenu).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ContextMenu).Name = "ContextMenu";
		((Control)ContextMenu).Size = new Size(61, 4);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pnlHeader);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcCHequeReport";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(653, 37);
		((Control)pnlHeader).ResumeLayout(false);
		((ISupportInitialize)picBox).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
