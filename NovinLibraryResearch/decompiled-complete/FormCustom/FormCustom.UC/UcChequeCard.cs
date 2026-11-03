using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using EntityCache.Business.Accounting;
using FormCustom.Properties;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;

namespace FormCustom.UC;

public class UcChequeCard : UserControl
{
	private CancellationTokenSource _tokenSource;

	private CardType _type;

	private bool _isDisposed;

	private IContainer components;

	private PanelEx pnlHeader;

	private PictureBox picBox;

	private Label lblTitle;

	private Panel pnlChequeM;

	private Label lblDate;

	private Label label4;

	private Label label5;

	private Label label6;

	private Label label3;

	private Label label2;

	private Label label1;

	private Label lblMoreDetail;

	private Label lblChequeM_VosolShode;

	private Label lblChequeM_BatelShode;

	private Label lblChequeM_NaghdShode;

	private Label lblChequeM_VagozarShode;

	private Label lblChequeM_KharjShode;

	private Label lblChequeM_Bargashti;

	private Label lblChequeM_Mojod;

	private Panel pnlChequeSh;

	private Label lblChequeSh_Bargashti;

	private Label lblChequeSh_BatelShode;

	private Label lblChequeSh_PardakhtShode;

	private Label lblChequeSh_Pardakhtani;

	private Label label17;

	private Label label18;

	private Label label19;

	private Label label20;

	public CardType CardType
	{
		get
		{
			return _type;
		}
		set
		{
			try
			{
				_type = value;
				Color foreColor = Color.Black;
				switch (_type)
				{
				case CardType.ChequeM:
					foreColor = Color.FromArgb(158, 91, 255);
					((Control)pnlChequeM).Visible = true;
					((Control)pnlChequeSh).Visible = false;
					((Control)lblTitle).Text = "اسناد دریافتنی";
					picBox.Image = (Image)(object)FormCustom.Properties.Resources.ChequeM;
					break;
				case CardType.ChequeSh:
					foreColor = Color.FromArgb(252, 79, 132);
					((Control)pnlChequeM).Visible = false;
					((Control)pnlChequeSh).Visible = true;
					((Control)lblTitle).Text = "اسناد پرداختنی";
					picBox.Image = (Image)(object)FormCustom.Properties.Resources.ChequeSh;
					break;
				}
				((Control)lblTitle).ForeColor = foreColor;
				((Control)lblDate).ForeColor = foreColor;
				((Control)lblChequeM_Bargashti).ForeColor = foreColor;
				((Control)lblChequeM_BatelShode).ForeColor = foreColor;
				((Control)lblChequeM_KharjShode).ForeColor = foreColor;
				((Control)lblChequeM_Mojod).ForeColor = foreColor;
				((Control)lblChequeM_NaghdShode).ForeColor = foreColor;
				((Control)lblChequeM_VagozarShode).ForeColor = foreColor;
				((Control)lblChequeM_VosolShode).ForeColor = foreColor;
				((Control)lblChequeSh_Bargashti).ForeColor = foreColor;
				((Control)lblChequeSh_BatelShode).ForeColor = foreColor;
				((Control)lblChequeSh_Pardakhtani).ForeColor = foreColor;
				((Control)lblChequeSh_PardakhtShode).ForeColor = foreColor;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CardType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcChequeCard.cs", 63);
			}
		}
	}

	public string ContextGuid { get; private set; }

	public DateTime? Date1 { get; private set; }

	public DateTime? Date2 { get; private set; }

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((Control)this).IsDisposed;
			}
			return true;
		}
	}

	public event Action OnDateChanged;

	public async Task InitAsync(string contextGuid, DateTime? date1, DateTime? date2)
	{
		try
		{
			_tokenSource?.Cancel();
			_tokenSource = new CancellationTokenSource();
			CancellationToken token = _tokenSource.Token;
			if (string.IsNullOrEmpty(contextGuid))
			{
				throw new Exception("contextGuid in ucChequeCard is null");
			}
			ContextGuid = contextGuid;
			Date1 = date1;
			Date2 = date2;
			IEnumerable<ChequeMSummaryViewModel> listChequeM = await ViewCheckMBusiness.GetSummaryAsync(ContextGuid, Date1, Date2);
			IEnumerable<ChequeSHSummaryViewModel> listChequeSh = await ViewCheckShBusiness.GetSummaryAsync(ContextGuid, Date1, Date2);
			do
			{
				await Task.Delay(new Random().Next(1000));
				if (IsDisposed)
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
						((Control)this).SuspendLayout();
						if (_type == CardType.ChequeM)
						{
							IEnumerable<ChequeMSummaryViewModel> enumerable = listChequeM;
							if (enumerable == null || !enumerable.Any())
							{
								return;
							}
							foreach (ChequeMSummaryViewModel item in listChequeM)
							{
								switch (item.Status)
								{
								case ENUMCheck.Mojod:
									((Control)lblChequeM_Mojod).Text = $"{item.Sum:N0}";
									break;
								case ENUMCheck.Bargashti:
									((Control)lblChequeM_Bargashti).Text = $"{item.Sum:N0}";
									break;
								case ENUMCheck.KharjShode:
									((Control)lblChequeM_KharjShode).Text = $"{item.Sum:N0}";
									break;
								case ENUMCheck.VagozarShode:
									((Control)lblChequeM_VagozarShode).Text = $"{item.Sum:N0}";
									break;
								case ENUMCheck.NaghdShode:
									((Control)lblChequeM_NaghdShode).Text = $"{item.Sum:N0}";
									break;
								case ENUMCheck.BatelShode:
									((Control)lblChequeM_BatelShode).Text = $"{item.Sum:N0}";
									break;
								case ENUMCheck.VosolShode:
									((Control)lblChequeM_VosolShode).Text = $"{item.Sum:N0}";
									break;
								}
							}
							((Control)pnlChequeM).Visible = true;
							((Control)pnlChequeSh).Visible = false;
						}
						else if (_type == CardType.ChequeSh)
						{
							IEnumerable<ChequeSHSummaryViewModel> enumerable2 = listChequeSh;
							if (enumerable2 == null || !enumerable2.Any())
							{
								return;
							}
							foreach (ChequeSHSummaryViewModel item2 in listChequeSh)
							{
								switch (item2.Status)
								{
								case ENCheckSHStatus.Pardakhtani:
									((Control)lblChequeSh_Pardakhtani).Text = $"{item2.Sum:N0}";
									break;
								case ENCheckSHStatus.VosolShode:
									((Control)lblChequeSh_PardakhtShode).Text = $"{item2.Sum:N0}";
									break;
								case ENCheckSHStatus.BargashtShode:
									((Control)lblChequeSh_Bargashti).Text = $"{item2.Sum:N0}";
									break;
								case ENCheckSHStatus.BatelShode:
									((Control)lblChequeSh_BatelShode).Text = $"{item2.Sum:N0}";
									break;
								}
							}
							((Control)pnlChequeM).Visible = false;
							((Control)pnlChequeSh).Visible = true;
						}
						if (Date1.HasValue && Date2.HasValue)
						{
							((Control)lblDate).Text = "گزارش از تاریخ " + Date1.MiladiToCalture() + " الی " + Date2.MiladiToCalture();
						}
						else if (Date1.HasValue && !Date2.HasValue)
						{
							((Control)lblDate).Text = "گزارش از تاریخ " + Date1.MiladiToCalture();
						}
						else if (!Date1.HasValue && Date2.HasValue)
						{
							((Control)lblDate).Text = "گزارش تا تاریخ " + Date2.MiladiToCalture();
						}
						else
						{
							((Control)lblDate).Text = "";
						}
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcChequeCard.cs", 146);
				}
				finally
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						((Control)this).ResumeLayout();
					}
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcChequeCard.cs", 150);
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
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcChequeCard.cs", 165);
		}
	}

	public UcChequeCard()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void lblTitle_Click(object sender, EventArgs e)
	{
		this.OnDateChanged?.Invoke();
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
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Expected O, but got Unknown
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Expected O, but got Unknown
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Expected O, but got Unknown
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bb: Expected O, but got Unknown
		//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0883: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Expected O, but got Unknown
		//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_095c: Expected O, but got Unknown
		//IL_098a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2b: Expected O, but got Unknown
		//IL_0a5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af0: Expected O, but got Unknown
		//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d55: Expected O, but got Unknown
		//IL_0d8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e30: Expected O, but got Unknown
		//IL_0e69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0b: Expected O, but got Unknown
		//IL_0f44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe6: Expected O, but got Unknown
		//IL_101f: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c1: Expected O, but got Unknown
		//IL_10fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1192: Unknown result type (might be due to invalid IL or missing references)
		//IL_119c: Expected O, but got Unknown
		//IL_11d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_126d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1277: Expected O, but got Unknown
		//IL_12af: Unknown result type (might be due to invalid IL or missing references)
		//IL_1347: Unknown result type (might be due to invalid IL or missing references)
		//IL_1351: Expected O, but got Unknown
		//IL_137f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1416: Unknown result type (might be due to invalid IL or missing references)
		//IL_1420: Expected O, but got Unknown
		//IL_144e: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ef: Expected O, but got Unknown
		//IL_1520: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c2: Expected O, but got Unknown
		//IL_15f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1687: Unknown result type (might be due to invalid IL or missing references)
		//IL_1691: Expected O, but got Unknown
		//IL_16bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1756: Unknown result type (might be due to invalid IL or missing references)
		//IL_1760: Expected O, but got Unknown
		//IL_1791: Unknown result type (might be due to invalid IL or missing references)
		//IL_1828: Unknown result type (might be due to invalid IL or missing references)
		//IL_1832: Expected O, but got Unknown
		//IL_1862: Unknown result type (might be due to invalid IL or missing references)
		//IL_1949: Unknown result type (might be due to invalid IL or missing references)
		//IL_1953: Expected O, but got Unknown
		//IL_198c: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a56: Expected O, but got Unknown
		//IL_1a5b: Unknown result type (might be due to invalid IL or missing references)
		pnlHeader = new PanelEx();
		pnlChequeSh = new Panel();
		lblChequeSh_Bargashti = new Label();
		lblChequeSh_BatelShode = new Label();
		lblChequeSh_PardakhtShode = new Label();
		lblChequeSh_Pardakhtani = new Label();
		label17 = new Label();
		label18 = new Label();
		label19 = new Label();
		label20 = new Label();
		lblDate = new Label();
		pnlChequeM = new Panel();
		lblChequeM_VosolShode = new Label();
		lblChequeM_BatelShode = new Label();
		lblChequeM_NaghdShode = new Label();
		lblChequeM_VagozarShode = new Label();
		lblChequeM_KharjShode = new Label();
		lblChequeM_Bargashti = new Label();
		lblChequeM_Mojod = new Label();
		label4 = new Label();
		label5 = new Label();
		label6 = new Label();
		label3 = new Label();
		label2 = new Label();
		label1 = new Label();
		lblMoreDetail = new Label();
		picBox = new PictureBox();
		lblTitle = new Label();
		((Control)pnlHeader).SuspendLayout();
		((Control)pnlChequeSh).SuspendLayout();
		((Control)pnlChequeM).SuspendLayout();
		((ISupportInitialize)picBox).BeginInit();
		((Control)this).SuspendLayout();
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)pnlChequeSh);
		((Control)pnlHeader).Controls.Add((Control)(object)lblDate);
		((Control)pnlHeader).Controls.Add((Control)(object)pnlChequeM);
		((Control)pnlHeader).Controls.Add((Control)(object)picBox);
		((Control)pnlHeader).Controls.Add((Control)(object)lblTitle);
		pnlHeader.DisabledBackColor = Color.Empty;
		((Control)pnlHeader).Dock = (DockStyle)5;
		((Control)pnlHeader).Location = new Point(0, 0);
		((Control)pnlHeader).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlHeader).Name = "pnlHeader";
		((Control)pnlHeader).Size = new Size(263, 222);
		pnlHeader.Style.BackColor1.Color = Color.White;
		pnlHeader.Style.BackColor2.Color = Color.White;
		pnlHeader.Style.Border = eBorderType.SingleLine;
		pnlHeader.Style.BorderColor.Color = Color.White;
		pnlHeader.Style.BorderWidth = 0;
		pnlHeader.Style.CornerDiameter = 10;
		pnlHeader.Style.CornerType = eCornerType.Rounded;
		((Control)pnlHeader).TabIndex = 139;
		((Control)pnlChequeSh).Anchor = (AnchorStyles)13;
		((Control)pnlChequeSh).Controls.Add((Control)(object)lblChequeSh_Bargashti);
		((Control)pnlChequeSh).Controls.Add((Control)(object)lblChequeSh_BatelShode);
		((Control)pnlChequeSh).Controls.Add((Control)(object)lblChequeSh_PardakhtShode);
		((Control)pnlChequeSh).Controls.Add((Control)(object)lblChequeSh_Pardakhtani);
		((Control)pnlChequeSh).Controls.Add((Control)(object)label17);
		((Control)pnlChequeSh).Controls.Add((Control)(object)label18);
		((Control)pnlChequeSh).Controls.Add((Control)(object)label19);
		((Control)pnlChequeSh).Controls.Add((Control)(object)label20);
		((Control)pnlChequeSh).Location = new Point(14, 67);
		((Control)pnlChequeSh).Name = "pnlChequeSh";
		((Control)pnlChequeSh).Size = new Size(239, 146);
		((Control)pnlChequeSh).TabIndex = 5;
		((Control)pnlChequeSh).Visible = false;
		((Control)lblChequeSh_Bargashti).Anchor = (AnchorStyles)13;
		((Control)lblChequeSh_Bargashti).AutoSize = true;
		((Control)lblChequeSh_Bargashti).Cursor = Cursors.Default;
		((Control)lblChequeSh_Bargashti).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblChequeSh_Bargashti).ForeColor = Color.FromArgb(41, 204, 161);
		((Control)lblChequeSh_Bargashti).Location = new Point(4, 63);
		((Control)lblChequeSh_Bargashti).Margin = new Padding(4, 0, 4, 0);
		((Control)lblChequeSh_Bargashti).Name = "lblChequeSh_Bargashti";
		((Control)lblChequeSh_Bargashti).Size = new Size(17, 20);
		((Control)lblChequeSh_Bargashti).TabIndex = 10;
		((Control)lblChequeSh_Bargashti).Text = "0";
		lblChequeSh_Bargashti.TextAlign = (ContentAlignment)64;
		((Control)lblChequeSh_BatelShode).Anchor = (AnchorStyles)13;
		((Control)lblChequeSh_BatelShode).AutoSize = true;
		((Control)lblChequeSh_BatelShode).Cursor = Cursors.Default;
		((Control)lblChequeSh_BatelShode).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblChequeSh_BatelShode).ForeColor = Color.FromArgb(41, 204, 161);
		((Control)lblChequeSh_BatelShode).Location = new Point(4, 43);
		((Control)lblChequeSh_BatelShode).Margin = new Padding(4, 0, 4, 0);
		((Control)lblChequeSh_BatelShode).Name = "lblChequeSh_BatelShode";
		((Control)lblChequeSh_BatelShode).Size = new Size(17, 20);
		((Control)lblChequeSh_BatelShode).TabIndex = 10;
		((Control)lblChequeSh_BatelShode).Text = "0";
		lblChequeSh_BatelShode.TextAlign = (ContentAlignment)64;
		((Control)lblChequeSh_PardakhtShode).Anchor = (AnchorStyles)13;
		((Control)lblChequeSh_PardakhtShode).AutoSize = true;
		((Control)lblChequeSh_PardakhtShode).Cursor = Cursors.Default;
		((Control)lblChequeSh_PardakhtShode).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblChequeSh_PardakhtShode).ForeColor = Color.FromArgb(41, 204, 161);
		((Control)lblChequeSh_PardakhtShode).Location = new Point(4, 24);
		((Control)lblChequeSh_PardakhtShode).Margin = new Padding(4, 0, 4, 0);
		((Control)lblChequeSh_PardakhtShode).Name = "lblChequeSh_PardakhtShode";
		((Control)lblChequeSh_PardakhtShode).Size = new Size(17, 20);
		((Control)lblChequeSh_PardakhtShode).TabIndex = 10;
		((Control)lblChequeSh_PardakhtShode).Text = "0";
		lblChequeSh_PardakhtShode.TextAlign = (ContentAlignment)64;
		((Control)lblChequeSh_Pardakhtani).Anchor = (AnchorStyles)13;
		((Control)lblChequeSh_Pardakhtani).AutoSize = true;
		((Control)lblChequeSh_Pardakhtani).Cursor = Cursors.Default;
		((Control)lblChequeSh_Pardakhtani).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblChequeSh_Pardakhtani).ForeColor = Color.FromArgb(41, 204, 161);
		((Control)lblChequeSh_Pardakhtani).Location = new Point(4, 4);
		((Control)lblChequeSh_Pardakhtani).Margin = new Padding(4, 0, 4, 0);
		((Control)lblChequeSh_Pardakhtani).Name = "lblChequeSh_Pardakhtani";
		((Control)lblChequeSh_Pardakhtani).Size = new Size(17, 20);
		((Control)lblChequeSh_Pardakhtani).TabIndex = 10;
		((Control)lblChequeSh_Pardakhtani).Text = "0";
		lblChequeSh_Pardakhtani.TextAlign = (ContentAlignment)64;
		((Control)label17).Anchor = (AnchorStyles)9;
		((Control)label17).AutoSize = true;
		((Control)label17).Cursor = Cursors.Default;
		((Control)label17).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label17).ForeColor = Color.DimGray;
		((Control)label17).Location = new Point(133, 63);
		((Control)label17).Margin = new Padding(4, 0, 4, 0);
		((Control)label17).Name = "label17";
		((Control)label17).Size = new Size(103, 20);
		((Control)label17).TabIndex = 6;
		((Control)label17).Text = "[جمع اسناد برگشتی]:";
		label17.TextAlign = (ContentAlignment)16;
		((Control)label18).Anchor = (AnchorStyles)9;
		((Control)label18).AutoSize = true;
		((Control)label18).Cursor = Cursors.Default;
		((Control)label18).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label18).ForeColor = Color.DimGray;
		((Control)label18).Location = new Point(127, 43);
		((Control)label18).Margin = new Padding(4, 0, 4, 0);
		((Control)label18).Name = "label18";
		((Control)label18).Size = new Size(111, 20);
		((Control)label18).TabIndex = 6;
		((Control)label18).Text = "[جمع اسناد باطل شده]:";
		label18.TextAlign = (ContentAlignment)16;
		((Control)label19).Anchor = (AnchorStyles)9;
		((Control)label19).AutoSize = true;
		((Control)label19).Cursor = Cursors.Default;
		((Control)label19).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label19).ForeColor = Color.DimGray;
		((Control)label19).Location = new Point(109, 24);
		((Control)label19).Margin = new Padding(4, 0, 4, 0);
		((Control)label19).Name = "label19";
		((Control)label19).Size = new Size(127, 20);
		((Control)label19).TabIndex = 6;
		((Control)label19).Text = "[جمع اسناد پرداخت شده]:";
		label19.TextAlign = (ContentAlignment)16;
		((Control)label20).Anchor = (AnchorStyles)9;
		((Control)label20).AutoSize = true;
		((Control)label20).Cursor = Cursors.Default;
		((Control)label20).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label20).ForeColor = Color.DimGray;
		((Control)label20).Location = new Point(129, 4);
		((Control)label20).Margin = new Padding(4, 0, 4, 0);
		((Control)label20).Name = "label20";
		((Control)label20).Size = new Size(109, 20);
		((Control)label20).TabIndex = 6;
		((Control)label20).Text = "[جمع اسناد پرداختنی]:";
		label20.TextAlign = (ContentAlignment)16;
		((Control)lblDate).Anchor = (AnchorStyles)13;
		((Control)lblDate).Cursor = Cursors.Default;
		((Control)lblDate).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblDate).ForeColor = Color.FromArgb(41, 204, 161);
		((Control)lblDate).Location = new Point(14, 47);
		((Control)lblDate).Margin = new Padding(4, 0, 4, 0);
		((Control)lblDate).Name = "lblDate";
		((Control)lblDate).Size = new Size(242, 17);
		((Control)lblDate).TabIndex = 4;
		lblDate.TextAlign = (ContentAlignment)16;
		((Control)pnlChequeM).Anchor = (AnchorStyles)13;
		((Control)pnlChequeM).Controls.Add((Control)(object)lblChequeM_VosolShode);
		((Control)pnlChequeM).Controls.Add((Control)(object)lblChequeM_BatelShode);
		((Control)pnlChequeM).Controls.Add((Control)(object)lblChequeM_NaghdShode);
		((Control)pnlChequeM).Controls.Add((Control)(object)lblChequeM_VagozarShode);
		((Control)pnlChequeM).Controls.Add((Control)(object)lblChequeM_KharjShode);
		((Control)pnlChequeM).Controls.Add((Control)(object)lblChequeM_Bargashti);
		((Control)pnlChequeM).Controls.Add((Control)(object)lblChequeM_Mojod);
		((Control)pnlChequeM).Controls.Add((Control)(object)label4);
		((Control)pnlChequeM).Controls.Add((Control)(object)label5);
		((Control)pnlChequeM).Controls.Add((Control)(object)label6);
		((Control)pnlChequeM).Controls.Add((Control)(object)label3);
		((Control)pnlChequeM).Controls.Add((Control)(object)label2);
		((Control)pnlChequeM).Controls.Add((Control)(object)label1);
		((Control)pnlChequeM).Controls.Add((Control)(object)lblMoreDetail);
		((Control)pnlChequeM).Location = new Point(14, 67);
		((Control)pnlChequeM).Name = "pnlChequeM";
		((Control)pnlChequeM).Size = new Size(239, 146);
		((Control)pnlChequeM).TabIndex = 3;
		((Control)pnlChequeM).Visible = false;
		((Control)lblChequeM_VosolShode).Anchor = (AnchorStyles)13;
		((Control)lblChequeM_VosolShode).AutoSize = true;
		((Control)lblChequeM_VosolShode).Cursor = Cursors.Default;
		((Control)lblChequeM_VosolShode).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblChequeM_VosolShode).ForeColor = Color.FromArgb(41, 204, 161);
		((Control)lblChequeM_VosolShode).Location = new Point(4, 122);
		((Control)lblChequeM_VosolShode).Margin = new Padding(4, 0, 4, 0);
		((Control)lblChequeM_VosolShode).Name = "lblChequeM_VosolShode";
		((Control)lblChequeM_VosolShode).Size = new Size(17, 20);
		((Control)lblChequeM_VosolShode).TabIndex = 10;
		((Control)lblChequeM_VosolShode).Text = "0";
		lblChequeM_VosolShode.TextAlign = (ContentAlignment)64;
		((Control)lblChequeM_BatelShode).Anchor = (AnchorStyles)13;
		((Control)lblChequeM_BatelShode).AutoSize = true;
		((Control)lblChequeM_BatelShode).Cursor = Cursors.Default;
		((Control)lblChequeM_BatelShode).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblChequeM_BatelShode).ForeColor = Color.FromArgb(41, 204, 161);
		((Control)lblChequeM_BatelShode).Location = new Point(4, 103);
		((Control)lblChequeM_BatelShode).Margin = new Padding(4, 0, 4, 0);
		((Control)lblChequeM_BatelShode).Name = "lblChequeM_BatelShode";
		((Control)lblChequeM_BatelShode).Size = new Size(17, 20);
		((Control)lblChequeM_BatelShode).TabIndex = 10;
		((Control)lblChequeM_BatelShode).Text = "0";
		lblChequeM_BatelShode.TextAlign = (ContentAlignment)64;
		((Control)lblChequeM_NaghdShode).Anchor = (AnchorStyles)13;
		((Control)lblChequeM_NaghdShode).AutoSize = true;
		((Control)lblChequeM_NaghdShode).Cursor = Cursors.Default;
		((Control)lblChequeM_NaghdShode).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblChequeM_NaghdShode).ForeColor = Color.FromArgb(41, 204, 161);
		((Control)lblChequeM_NaghdShode).Location = new Point(4, 83);
		((Control)lblChequeM_NaghdShode).Margin = new Padding(4, 0, 4, 0);
		((Control)lblChequeM_NaghdShode).Name = "lblChequeM_NaghdShode";
		((Control)lblChequeM_NaghdShode).Size = new Size(17, 20);
		((Control)lblChequeM_NaghdShode).TabIndex = 10;
		((Control)lblChequeM_NaghdShode).Text = "0";
		lblChequeM_NaghdShode.TextAlign = (ContentAlignment)64;
		((Control)lblChequeM_VagozarShode).Anchor = (AnchorStyles)13;
		((Control)lblChequeM_VagozarShode).AutoSize = true;
		((Control)lblChequeM_VagozarShode).Cursor = Cursors.Default;
		((Control)lblChequeM_VagozarShode).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblChequeM_VagozarShode).ForeColor = Color.FromArgb(41, 204, 161);
		((Control)lblChequeM_VagozarShode).Location = new Point(4, 63);
		((Control)lblChequeM_VagozarShode).Margin = new Padding(4, 0, 4, 0);
		((Control)lblChequeM_VagozarShode).Name = "lblChequeM_VagozarShode";
		((Control)lblChequeM_VagozarShode).Size = new Size(17, 20);
		((Control)lblChequeM_VagozarShode).TabIndex = 10;
		((Control)lblChequeM_VagozarShode).Text = "0";
		lblChequeM_VagozarShode.TextAlign = (ContentAlignment)64;
		((Control)lblChequeM_KharjShode).Anchor = (AnchorStyles)13;
		((Control)lblChequeM_KharjShode).AutoSize = true;
		((Control)lblChequeM_KharjShode).Cursor = Cursors.Default;
		((Control)lblChequeM_KharjShode).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblChequeM_KharjShode).ForeColor = Color.FromArgb(41, 204, 161);
		((Control)lblChequeM_KharjShode).Location = new Point(4, 43);
		((Control)lblChequeM_KharjShode).Margin = new Padding(4, 0, 4, 0);
		((Control)lblChequeM_KharjShode).Name = "lblChequeM_KharjShode";
		((Control)lblChequeM_KharjShode).Size = new Size(17, 20);
		((Control)lblChequeM_KharjShode).TabIndex = 10;
		((Control)lblChequeM_KharjShode).Text = "0";
		lblChequeM_KharjShode.TextAlign = (ContentAlignment)64;
		((Control)lblChequeM_Bargashti).Anchor = (AnchorStyles)13;
		((Control)lblChequeM_Bargashti).AutoSize = true;
		((Control)lblChequeM_Bargashti).Cursor = Cursors.Default;
		((Control)lblChequeM_Bargashti).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblChequeM_Bargashti).ForeColor = Color.FromArgb(41, 204, 161);
		((Control)lblChequeM_Bargashti).Location = new Point(4, 24);
		((Control)lblChequeM_Bargashti).Margin = new Padding(4, 0, 4, 0);
		((Control)lblChequeM_Bargashti).Name = "lblChequeM_Bargashti";
		((Control)lblChequeM_Bargashti).Size = new Size(17, 20);
		((Control)lblChequeM_Bargashti).TabIndex = 10;
		((Control)lblChequeM_Bargashti).Text = "0";
		lblChequeM_Bargashti.TextAlign = (ContentAlignment)64;
		((Control)lblChequeM_Mojod).Anchor = (AnchorStyles)13;
		((Control)lblChequeM_Mojod).AutoSize = true;
		((Control)lblChequeM_Mojod).Cursor = Cursors.Default;
		((Control)lblChequeM_Mojod).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblChequeM_Mojod).ForeColor = Color.FromArgb(41, 204, 161);
		((Control)lblChequeM_Mojod).Location = new Point(4, 4);
		((Control)lblChequeM_Mojod).Margin = new Padding(4, 0, 4, 0);
		((Control)lblChequeM_Mojod).Name = "lblChequeM_Mojod";
		((Control)lblChequeM_Mojod).Size = new Size(17, 20);
		((Control)lblChequeM_Mojod).TabIndex = 10;
		((Control)lblChequeM_Mojod).Text = "0";
		lblChequeM_Mojod.TextAlign = (ContentAlignment)64;
		((Control)label4).Anchor = (AnchorStyles)9;
		((Control)label4).AutoSize = true;
		((Control)label4).Cursor = Cursors.Default;
		((Control)label4).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label4).ForeColor = Color.DimGray;
		((Control)label4).Location = new Point(116, 122);
		((Control)label4).Margin = new Padding(4, 0, 4, 0);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(120, 20);
		((Control)label4).TabIndex = 7;
		((Control)label4).Text = "[جمع اسناد وصول شده]:";
		label4.TextAlign = (ContentAlignment)16;
		((Control)label5).Anchor = (AnchorStyles)9;
		((Control)label5).AutoSize = true;
		((Control)label5).Cursor = Cursors.Default;
		((Control)label5).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label5).ForeColor = Color.DimGray;
		((Control)label5).Location = new Point(125, 103);
		((Control)label5).Margin = new Padding(4, 0, 4, 0);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(111, 20);
		((Control)label5).TabIndex = 8;
		((Control)label5).Text = "[جمع اسناد باطل شده]:";
		label5.TextAlign = (ContentAlignment)16;
		((Control)label6).Anchor = (AnchorStyles)9;
		((Control)label6).AutoSize = true;
		((Control)label6).Cursor = Cursors.Default;
		((Control)label6).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label6).ForeColor = Color.DimGray;
		((Control)label6).Location = new Point(131, 83);
		((Control)label6).Margin = new Padding(4, 0, 4, 0);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(105, 20);
		((Control)label6).TabIndex = 9;
		((Control)label6).Text = "[جمع اسناد نقد شده]:";
		label6.TextAlign = (ContentAlignment)16;
		((Control)label3).Anchor = (AnchorStyles)9;
		((Control)label3).AutoSize = true;
		((Control)label3).Cursor = Cursors.Default;
		((Control)label3).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label3).ForeColor = Color.DimGray;
		((Control)label3).Location = new Point(115, 63);
		((Control)label3).Margin = new Padding(4, 0, 4, 0);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(121, 20);
		((Control)label3).TabIndex = 6;
		((Control)label3).Text = "[جمع اسناد واگذار شده]:";
		label3.TextAlign = (ContentAlignment)16;
		((Control)label2).Anchor = (AnchorStyles)9;
		((Control)label2).AutoSize = true;
		((Control)label2).Cursor = Cursors.Default;
		((Control)label2).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label2).ForeColor = Color.DimGray;
		((Control)label2).Location = new Point(127, 43);
		((Control)label2).Margin = new Padding(4, 0, 4, 0);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(109, 20);
		((Control)label2).TabIndex = 6;
		((Control)label2).Text = "[جمع اسناد خرج شده]:";
		label2.TextAlign = (ContentAlignment)16;
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).AutoSize = true;
		((Control)label1).Cursor = Cursors.Default;
		((Control)label1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label1).ForeColor = Color.DimGray;
		((Control)label1).Location = new Point(133, 24);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(103, 20);
		((Control)label1).TabIndex = 6;
		((Control)label1).Text = "[جمع اسناد برگشتی]:";
		label1.TextAlign = (ContentAlignment)16;
		((Control)lblMoreDetail).Anchor = (AnchorStyles)9;
		((Control)lblMoreDetail).AutoSize = true;
		((Control)lblMoreDetail).Cursor = Cursors.Default;
		((Control)lblMoreDetail).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblMoreDetail).ForeColor = Color.DimGray;
		((Control)lblMoreDetail).Location = new Point(141, 4);
		((Control)lblMoreDetail).Margin = new Padding(4, 0, 4, 0);
		((Control)lblMoreDetail).Name = "lblMoreDetail";
		((Control)lblMoreDetail).Size = new Size(95, 20);
		((Control)lblMoreDetail).TabIndex = 6;
		((Control)lblMoreDetail).Text = "[جمع اسناد موجود]:";
		lblMoreDetail.TextAlign = (ContentAlignment)16;
		((Control)picBox).Location = new Point(14, 10);
		((Control)picBox).Name = "picBox";
		((Control)picBox).Size = new Size(39, 34);
		picBox.SizeMode = (PictureBoxSizeMode)1;
		picBox.TabIndex = 2;
		picBox.TabStop = false;
		((Control)lblTitle).Anchor = (AnchorStyles)13;
		((Control)lblTitle).Cursor = Cursors.Hand;
		((Control)lblTitle).Font = new Font("B Yekan", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblTitle).ForeColor = Color.FromArgb(41, 204, 161);
		((Control)lblTitle).Location = new Point(60, 8);
		((Control)lblTitle).Margin = new Padding(4, 0, 4, 0);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Padding = new Padding(0, 0, 10, 0);
		((Control)lblTitle).Size = new Size(199, 39);
		((Control)lblTitle).TabIndex = 1;
		lblTitle.TextAlign = (ContentAlignment)16;
		((Control)lblTitle).Click += lblTitle_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pnlHeader);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcChequeCard";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(263, 222);
		((Control)pnlHeader).ResumeLayout(false);
		((Control)pnlChequeSh).ResumeLayout(false);
		((Control)pnlChequeSh).PerformLayout();
		((Control)pnlChequeM).ResumeLayout(false);
		((Control)pnlChequeM).PerformLayout();
		((ISupportInitialize)picBox).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
