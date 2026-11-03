using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Properties;
using PacketParser.Entities;

namespace FormCustom.UC.SevenSegment;

public class UcSevenSegment_Price : UserControl
{
	private CancellationTokenSource _loadImageToken;

	private bool _isDisposed;

	private decimal _price;

	private IContainer components;

	private TableLayoutPanel tableLayoutPanel1;

	private PictureBox p2;

	private PictureBox p5;

	private PictureBox p4;

	private PictureBox p3;

	private PictureBox p8;

	private PictureBox p7;

	private PictureBox p6;

	private PictureBox p1;

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

	public decimal Price
	{
		get
		{
			return _price;
		}
		set
		{
			try
			{
				_price = Math.Floor(value);
				if (!IsDisposed)
				{
					if (_price < 0m)
					{
						_price = default(decimal);
					}
					Image img1 = null;
					Image img2 = null;
					Image img3 = null;
					Image img4 = null;
					Image img5 = null;
					Image img6 = null;
					Image img7 = null;
					Image img8 = null;
					string text = _price.ToString();
					while (text.Length < 8)
					{
						text = "o" + text;
					}
					img1 = GetImage(text[0]);
					img2 = GetImage(text[1], useDot: true);
					img3 = GetImage(text[2]);
					img4 = GetImage(text[3]);
					img5 = GetImage(text[4], useDot: true);
					img6 = GetImage(text[5]);
					img7 = GetImage(text[6]);
					img8 = GetImage(text[7]);
					_loadImageToken?.Cancel();
					_loadImageToken = new CancellationTokenSource();
					CancellationToken token = _loadImageToken.Token;
					Task.Run(() => UpdateImagesAsync(img1, img2, img3, img4, img5, img6, img7, img8, token));
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Price", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\SevenSegment\\UcSevenSegment_Price.cs", 57);
				throw;
			}
		}
	}

	public UcSevenSegment_Price()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private async Task UpdateImagesAsync(Image img1, Image img2, Image img3, Image img4, Image img5, Image img6, Image img7, Image img8, CancellationToken token)
	{
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
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					p1.Image = img1;
					p2.Image = img2;
					p3.Image = img3;
					p4.Image = img4;
					p5.Image = img5;
					p6.Image = img6;
					p7.Image = img7;
					p8.Image = img8;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "UpdateImagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\SevenSegment\\UcSevenSegment_Price.cs", 86);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateImagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\SevenSegment\\UcSevenSegment_Price.cs", 89);
		}
	}

	private Image GetImage(char number, bool useDot = false)
	{
		if (number == '0' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_0;
		}
		if (number == '1' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_1;
		}
		if (number == '2' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_2;
		}
		if (number == '3' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_3;
		}
		if (number == '4' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_4;
		}
		if (number == '5' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_5;
		}
		if (number == '6' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_6;
		}
		if (number == '7' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_7;
		}
		if (number == '8' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_8;
		}
		if (number == '9' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_9;
		}
		return (Image)(number switch
		{
			'0' => FormCustom.Properties.Resources.SevenSegment_0, 
			'1' => FormCustom.Properties.Resources.SevenSegment_1, 
			'2' => FormCustom.Properties.Resources.SevenSegment_2, 
			'3' => FormCustom.Properties.Resources.SevenSegment_3, 
			'4' => FormCustom.Properties.Resources.SevenSegment_4, 
			'5' => FormCustom.Properties.Resources.SevenSegment_5, 
			'6' => FormCustom.Properties.Resources.SevenSegment_6, 
			'7' => FormCustom.Properties.Resources.SevenSegment_7, 
			'8' => FormCustom.Properties.Resources.SevenSegment_8, 
			'9' => FormCustom.Properties.Resources.SevenSegment_9, 
			'o' => FormCustom.Properties.Resources.SevenSegment_off, 
			_ => throw new InvalidOperationException($"value({number}) is not valid!!"), 
		});
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_loadImageToken?.Cancel();
				_loadImageToken?.Dispose();
				_loadImageToken = null;
				components?.Dispose();
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\SevenSegment\\UcSevenSegment_Price.cs", 133);
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Expected O, but got Unknown
		tableLayoutPanel1 = new TableLayoutPanel();
		p5 = new PictureBox();
		p4 = new PictureBox();
		p3 = new PictureBox();
		p8 = new PictureBox();
		p7 = new PictureBox();
		p6 = new PictureBox();
		p1 = new PictureBox();
		p2 = new PictureBox();
		((Control)tableLayoutPanel1).SuspendLayout();
		((ISupportInitialize)p5).BeginInit();
		((ISupportInitialize)p4).BeginInit();
		((ISupportInitialize)p3).BeginInit();
		((ISupportInitialize)p8).BeginInit();
		((ISupportInitialize)p7).BeginInit();
		((ISupportInitialize)p6).BeginInit();
		((ISupportInitialize)p1).BeginInit();
		((ISupportInitialize)p2).BeginInit();
		((Control)this).SuspendLayout();
		((Control)tableLayoutPanel1).BackColor = Color.Transparent;
		tableLayoutPanel1.ColumnCount = 8;
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 12.5f));
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 12.5f));
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 12.5f));
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 12.5f));
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 12.5f));
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 12.5f));
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 12.5f));
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 12.5f));
		tableLayoutPanel1.Controls.Add((Control)(object)p5, 0, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)p4, 0, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)p3, 0, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)p8, 0, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)p7, 0, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)p6, 0, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)p1, 7, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)p2, 6, 0);
		((Control)tableLayoutPanel1).Dock = (DockStyle)5;
		((Control)tableLayoutPanel1).Location = new Point(0, 0);
		((Control)tableLayoutPanel1).Name = "tableLayoutPanel1";
		tableLayoutPanel1.RowCount = 1;
		tableLayoutPanel1.RowStyles.Add(new RowStyle((SizeType)2, 100f));
		((Control)tableLayoutPanel1).Size = new Size(1253, 150);
		((Control)tableLayoutPanel1).TabIndex = 0;
		((Control)p5).BackColor = Color.Transparent;
		p5.Image = (Image)(object)FormCustom.Properties.Resources.SevenSegment_0;
		((Control)p5).Location = new Point(471, 3);
		((Control)p5).Name = "p5";
		((Control)p5).Size = new Size(150, 144);
		p5.SizeMode = (PictureBoxSizeMode)1;
		p5.TabIndex = 11;
		p5.TabStop = false;
		((Control)p4).BackColor = Color.Transparent;
		p4.Image = (Image)(object)FormCustom.Properties.Resources.SevenSegment_0;
		((Control)p4).Location = new Point(627, 3);
		((Control)p4).Name = "p4";
		((Control)p4).Size = new Size(150, 144);
		p4.SizeMode = (PictureBoxSizeMode)1;
		p4.TabIndex = 10;
		p4.TabStop = false;
		((Control)p3).BackColor = Color.Transparent;
		p3.Image = (Image)(object)FormCustom.Properties.Resources.SevenSegment_0;
		((Control)p3).Location = new Point(783, 3);
		((Control)p3).Name = "p3";
		((Control)p3).Size = new Size(150, 144);
		p3.SizeMode = (PictureBoxSizeMode)1;
		p3.TabIndex = 9;
		p3.TabStop = false;
		((Control)p8).BackColor = Color.Transparent;
		p8.Image = (Image)(object)FormCustom.Properties.Resources.SevenSegment_0;
		((Control)p8).Location = new Point(3, 3);
		((Control)p8).Name = "p8";
		((Control)p8).Size = new Size(150, 144);
		p8.SizeMode = (PictureBoxSizeMode)1;
		p8.TabIndex = 8;
		p8.TabStop = false;
		((Control)p7).BackColor = Color.Transparent;
		p7.Image = (Image)(object)FormCustom.Properties.Resources.SevenSegment_0;
		((Control)p7).Location = new Point(159, 3);
		((Control)p7).Name = "p7";
		((Control)p7).Size = new Size(150, 144);
		p7.SizeMode = (PictureBoxSizeMode)1;
		p7.TabIndex = 7;
		p7.TabStop = false;
		((Control)p6).BackColor = Color.Transparent;
		p6.Image = (Image)(object)FormCustom.Properties.Resources.SevenSegment_0;
		((Control)p6).Location = new Point(315, 3);
		((Control)p6).Name = "p6";
		((Control)p6).Size = new Size(150, 144);
		p6.SizeMode = (PictureBoxSizeMode)1;
		p6.TabIndex = 6;
		p6.TabStop = false;
		((Control)p1).BackColor = Color.Transparent;
		p1.Image = (Image)(object)FormCustom.Properties.Resources.SevenSegment_0;
		((Control)p1).Location = new Point(1095, 3);
		((Control)p1).Name = "p1";
		((Control)p1).Size = new Size(153, 144);
		p1.SizeMode = (PictureBoxSizeMode)1;
		p1.TabIndex = 5;
		p1.TabStop = false;
		((Control)p2).BackColor = Color.Transparent;
		p2.Image = (Image)(object)FormCustom.Properties.Resources.SevenSegment_0;
		((Control)p2).Location = new Point(939, 3);
		((Control)p2).Name = "p2";
		((Control)p2).Size = new Size(150, 144);
		p2.SizeMode = (PictureBoxSizeMode)1;
		p2.TabIndex = 3;
		p2.TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)tableLayoutPanel1);
		((Control)this).Name = "UcSevenSegment_Price";
		((Control)this).Size = new Size(1253, 150);
		((Control)tableLayoutPanel1).ResumeLayout(false);
		((ISupportInitialize)p5).EndInit();
		((ISupportInitialize)p4).EndInit();
		((ISupportInitialize)p3).EndInit();
		((ISupportInitialize)p8).EndInit();
		((ISupportInitialize)p7).EndInit();
		((ISupportInitialize)p6).EndInit();
		((ISupportInitialize)p1).EndInit();
		((ISupportInitialize)p2).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
