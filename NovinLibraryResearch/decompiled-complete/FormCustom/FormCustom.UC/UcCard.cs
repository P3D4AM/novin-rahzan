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
using EntityCache.Business.Accounting.Coding;
using FormCustom.Properties;
using PacketParser;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Filters;

namespace FormCustom.UC;

public class UcCard : UserControl
{
	private CancellationTokenSource _token;

	private CardType _type;

	private int _height;

	private bool _isDisposed;

	private IContainer components;

	private PanelEx pnlHeader;

	private Label lblTitle;

	private PictureBox picBox;

	private Label lblValue;

	private Label lblMoreDetail;

	private FlowLayoutPanel panel2;

	public CardType CardType
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
			Color foreColor = Color.Black;
			((Control)this).Height = 94;
			switch (_type)
			{
			case CardType.Bank:
				foreColor = Color.FromArgb(21, 190, 157);
				((Control)lblTitle).Text = "موجودی بانک".GetTranslate();
				picBox.Image = (Image)(object)FormCustom.Properties.Resources.Bank;
				break;
			case CardType.Sandogh:
				foreColor = Color.FromArgb(253, 111, 0);
				((Control)lblTitle).Text = "موجودی صندوق".GetTranslate();
				picBox.Image = (Image)(object)FormCustom.Properties.Resources.Sandogh;
				break;
			}
			((Control)lblTitle).ForeColor = foreColor;
			((Control)lblValue).ForeColor = foreColor;
		}
	}

	public string ContextGuid { get; private set; }

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

	public event Action<Guid> TodayGardeshEvent;

	public event Action<Guid> Last30GardeshEvent;

	public event Action<Guid> AllGardeshEvent;

	public event Action<Guid> MoreGardeshEvent;

	public async Task InitAsync(string contextGuid, int maxHeight)
	{
		try
		{
			_token?.Cancel();
			_token = new CancellationTokenSource();
			CancellationToken token = _token.Token;
			if (string.IsNullOrEmpty(contextGuid))
			{
				throw new Exception("contextGuid in ucCard is null");
			}
			ContextGuid = contextGuid;
			_height = maxHeight;
			Guid moeinGuid = Guid.Empty;
			TafsilFilter tafsilFilter = new TafsilFilter
			{
				Status = true
			};
			if (_type == CardType.Bank)
			{
				tafsilFilter.HesabType = HesabType.Bank;
				tafsilFilter.MoeinGuidList = new List<Guid> { ParentDefaults.MoeinCoding.CLSMoein4 };
				moeinGuid = ParentDefaults.MoeinCoding.CLSMoein4;
			}
			else
			{
				tafsilFilter.HesabType = HesabType.Sandogh;
				tafsilFilter.MoeinGuidList = new List<Guid> { ParentDefaults.MoeinCoding.CLSMoein7 };
				moeinGuid = ParentDefaults.MoeinCoding.CLSMoein7;
			}
			IEnumerable<TafsilBusiness> list = await TafsilBusiness.GetAllAsync(ContextGuid, EnSystem.UserInterface, tafsilFilter, default(CancellationToken), "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcCard.cs");
			if (!(list?.Any() ?? false))
			{
				return;
			}
			int flag = 1;
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
			((Control)this).BeginInvoke((Delegate)(Func<Task>)async delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						((Control)this).SuspendLayout();
						((Control)panel2).Controls.Clear();
						decimal sum = default(decimal);
						foreach (TafsilBusiness item in list)
						{
							UcFlatCard ucFlatCard = new UcFlatCard();
							((Control)ucFlatCard).Width = ((Control)panel2).Width - 27;
							((Control)ucFlatCard).Height = 39;
							UcFlatCard det = ucFlatCard;
							if (flag % 2 == 0)
							{
								det.CardType = FlatCardType.Dark;
							}
							else
							{
								det.CardType = FlatCardType.Light;
							}
							decimal num = await AccountBusiness.GetAccountSumAsync(ContextGuid, moeinGuid, item.Guid, true);
							sum += num;
							det.Guid = item.Guid;
							det.Title = item.Name;
							if (num < 0m)
							{
								det.Value = $"{-num:N0} بد";
							}
							else if (num > 0m)
							{
								det.Value = $"{num:N0} بس";
							}
							else
							{
								det.Value = "بی حساب".GetTranslate();
							}
							det.AllGardeshEvent += Det_AllGardeshEvent;
							det.Last30GardeshEvent += Det_Last30GardeshEvent;
							det.TodayGardeshEvent += Det_TodayGardeshEvent;
							det.MoreGardeshEvent += Det_MoreGardeshEvent;
							((Control)panel2).Controls.Add((Control)(object)det);
							flag++;
						}
						if (sum > 0m)
						{
							((Control)lblValue).Text = $"{sum:N0} بس";
						}
						else if (sum < 0m)
						{
							((Control)lblValue).Text = $"{-sum:N0} بد";
						}
						else
						{
							((Control)lblValue).Text = $"{sum:N0}";
						}
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcCard.cs", 128);
				}
				finally
				{
					((Control)this).ResumeLayout();
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcCard.cs", 132);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcCard.cs", 147);
		}
	}

	private void Det_TodayGardeshEvent(Guid guid)
	{
		this.TodayGardeshEvent?.Invoke(guid);
	}

	private void Det_Last30GardeshEvent(Guid guid)
	{
		this.Last30GardeshEvent?.Invoke(guid);
	}

	private void Det_AllGardeshEvent(Guid guid)
	{
		this.AllGardeshEvent?.Invoke(guid);
	}

	private void Det_MoreGardeshEvent(Guid guid)
	{
		this.MoreGardeshEvent?.Invoke(guid);
	}

	public UcCard()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void lblMoreDetail_MouseEnter(object sender, EventArgs e)
	{
		((Control)lblMoreDetail).ForeColor = Color.Red;
	}

	private void lblMoreDetail_MouseLeave(object sender, EventArgs e)
	{
		((Control)lblMoreDetail).ForeColor = Color.DimGray;
	}

	private void lblMoreDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)lblMoreDetail).Text == "جمع کل")
			{
				((Control)this).Height = 94;
				((Control)lblMoreDetail).Text = "جزییات".GetTranslate();
			}
			else
			{
				((Control)this).Height = _height;
				((Control)this).BringToFront();
				((Control)lblMoreDetail).Text = "جمع کل".GetTranslate();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblMoreDetail_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcCard.cs", 178);
		}
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
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Expected O, but got Unknown
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Expected O, but got Unknown
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		pnlHeader = new PanelEx();
		panel2 = new FlowLayoutPanel();
		lblMoreDetail = new Label();
		lblValue = new Label();
		picBox = new PictureBox();
		lblTitle = new Label();
		((Control)pnlHeader).SuspendLayout();
		((ISupportInitialize)picBox).BeginInit();
		((Control)this).SuspendLayout();
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)panel2);
		((Control)pnlHeader).Controls.Add((Control)(object)lblMoreDetail);
		((Control)pnlHeader).Controls.Add((Control)(object)lblValue);
		((Control)pnlHeader).Controls.Add((Control)(object)picBox);
		((Control)pnlHeader).Controls.Add((Control)(object)lblTitle);
		pnlHeader.DisabledBackColor = Color.Empty;
		((Control)pnlHeader).Dock = (DockStyle)5;
		((Control)pnlHeader).Location = new Point(0, 0);
		((Control)pnlHeader).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlHeader).Name = "pnlHeader";
		((Control)pnlHeader).Size = new Size(263, 247);
		pnlHeader.Style.BackColor1.Color = Color.White;
		pnlHeader.Style.BackColor2.Color = Color.White;
		pnlHeader.Style.Border = eBorderType.SingleLine;
		pnlHeader.Style.BorderColor.Color = Color.White;
		pnlHeader.Style.BorderWidth = 0;
		pnlHeader.Style.CornerDiameter = 10;
		pnlHeader.Style.CornerType = eCornerType.Rounded;
		((Control)pnlHeader).TabIndex = 135;
		((Control)panel2).Anchor = (AnchorStyles)15;
		((ScrollableControl)panel2).AutoScroll = true;
		((Control)panel2).Location = new Point(3, 53);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(257, 153);
		((Control)panel2).TabIndex = 6;
		((Control)lblMoreDetail).Anchor = (AnchorStyles)10;
		((Control)lblMoreDetail).AutoSize = true;
		((Control)lblMoreDetail).Cursor = Cursors.Hand;
		((Control)lblMoreDetail).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblMoreDetail).ForeColor = Color.DimGray;
		((Control)lblMoreDetail).Location = new Point(213, 214);
		((Control)lblMoreDetail).Margin = new Padding(4, 0, 4, 0);
		((Control)lblMoreDetail).Name = "lblMoreDetail";
		((Control)lblMoreDetail).Size = new Size(44, 20);
		((Control)lblMoreDetail).TabIndex = 5;
		((Control)lblMoreDetail).Text = "[جزییات]";
		lblMoreDetail.TextAlign = (ContentAlignment)16;
		((Control)lblMoreDetail).Click += lblMoreDetail_Click;
		((Control)lblMoreDetail).MouseEnter += lblMoreDetail_MouseEnter;
		((Control)lblMoreDetail).MouseLeave += lblMoreDetail_MouseLeave;
		((Control)lblValue).Anchor = (AnchorStyles)14;
		((Control)lblValue).Cursor = Cursors.Default;
		((Control)lblValue).Font = new Font("B Yekan", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblValue).ForeColor = Color.FromArgb(41, 204, 161);
		((Control)lblValue).Location = new Point(14, 203);
		((Control)lblValue).Margin = new Padding(4, 0, 4, 0);
		((Control)lblValue).Name = "lblValue";
		((Control)lblValue).Size = new Size(198, 39);
		((Control)lblValue).TabIndex = 4;
		((Control)lblValue).Text = "0";
		lblValue.TextAlign = (ContentAlignment)64;
		((Control)picBox).Location = new Point(14, 10);
		((Control)picBox).Name = "picBox";
		((Control)picBox).Size = new Size(39, 34);
		picBox.SizeMode = (PictureBoxSizeMode)1;
		picBox.TabIndex = 2;
		picBox.TabStop = false;
		((Control)lblTitle).Anchor = (AnchorStyles)13;
		((Control)lblTitle).Cursor = Cursors.Default;
		((Control)lblTitle).Font = new Font("B Yekan", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblTitle).ForeColor = Color.FromArgb(41, 204, 161);
		((Control)lblTitle).Location = new Point(60, 8);
		((Control)lblTitle).Margin = new Padding(4, 0, 4, 0);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Padding = new Padding(0, 0, 10, 0);
		((Control)lblTitle).Size = new Size(199, 39);
		((Control)lblTitle).TabIndex = 1;
		((Control)lblTitle).Text = "[اسناد دریافتنی]";
		lblTitle.TextAlign = (ContentAlignment)16;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pnlHeader);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcCard";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(263, 247);
		((Control)pnlHeader).ResumeLayout(false);
		((Control)pnlHeader).PerformLayout();
		((ISupportInitialize)picBox).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
