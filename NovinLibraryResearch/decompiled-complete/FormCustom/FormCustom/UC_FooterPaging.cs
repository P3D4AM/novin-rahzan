using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.UcBinding;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom;

public class UC_FooterPaging : UserControl
{
	public delegate void OnBindDataReadyEventHandler<TSender, TEventArgs>(object sender, FooterBindingDataReadyEventArg e);

	private bool _isDisposed;

	private CancellationTokenSource _token_cus = new CancellationTokenSource();

	private int _pageIdx = 1;

	private int TotalCount;

	private IContainer components;

	private NumericUpDown TxtTopCount;

	internal Label label1;

	internal Label LblPage;

	private Button BtnNext;

	private Button BtnBack;

	private TextBox txtFromPage;

	public List<object> list { get; private set; }

	public List<object> Pagelist { get; private set; }

	private int CurrentPage { get; set; }

	public int PageIdx
	{
		get
		{
			try
			{
				return (_pageIdx <= 0) ? 1 : _pageIdx;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PageIdx", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UcBinding\\UC_FooterPaging.cs", 41);
				return 0;
			}
		}
		set
		{
			try
			{
				if (list == null || value < 1 || value > TotalCount)
				{
					return;
				}
				((Control)this).Invoke((Delegate)(Action)delegate
				{
					_pageIdx = value;
					if ((((Control)txtFromPage).Text?.ParseToInt() ?? 0) != value)
					{
						((Control)txtFromPage).Text = value.ToString();
					}
				});
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PageIdx", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UcBinding\\UC_FooterPaging.cs", 63);
			}
		}
	}

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

	public event OnBindDataReadyEventHandler<object, List<object>> OnBindDataReady;

	public UC_FooterPaging()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void TxtTopCount_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtTopCount).Focus();
			CurrentPage = (int)TxtTopCount.Value;
			_token_cus?.Cancel();
			_token_cus = new CancellationTokenSource(60000);
			CancellationToken tkn = _token_cus.Token;
			Task.Run(() => PaggerAsync(tkn, list));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxtTopCount_ValueChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UcBinding\\UC_FooterPaging.cs", 91);
		}
	}

	public async Task<List<object>> PagingAsync(CancellationToken token, IEnumerable<object> objList, int countPerPage, PagingPosition position)
	{
		int line = 0;
		try
		{
			while (!((Control)this).IsHandleCreated)
			{
				if (token.IsCancellationRequested || IsDisposed)
				{
					return null;
				}
				await Task.Delay(100);
			}
			if (countPerPage <= 0)
			{
				countPerPage = int.MaxValue;
			}
			line = 1;
			if (IsDisposed || token.IsCancellationRequested)
			{
				return null;
			}
			line = 1010;
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					line = 2;
					TxtTopCount.ValueChanged -= TxtTopCount_ValueChanged;
					line = 3;
					TxtTopCount.Maximum = 2147483647m;
					line = 4;
					TxtTopCount.Minimum = 1m;
					line = 5;
					TxtTopCount.Value = countPerPage;
					line = 6;
					TxtTopCount.ValueChanged += TxtTopCount_ValueChanged;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, $"2222////Error in Line {line}", "PagingAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UcBinding\\UC_FooterPaging.cs", 125);
				}
			});
			line = 7;
			CurrentPage = countPerPage;
			line = 8;
			list = objList?.ToList();
			line = 9;
			Task.Run(() => PaggerAsync(token, objList, position));
		}
		catch (Exception ex)
		{
			if (line != 1010)
			{
				WebErrorLog.Instance.StartLog(ex, $"2222////Error in Line{line}", "PagingAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UcBinding\\UC_FooterPaging.cs", 136);
			}
		}
		return null;
	}

	private async Task PaggerAsync(CancellationToken token, IEnumerable<object> objList, PagingPosition? position = null)
	{
		int line = 1;
		FooterBindingDataReadyEventArg ea = new FooterBindingDataReadyEventArg
		{
			Token = token
		};
		try
		{
			line = 2;
			await Task.Delay(300);
			if (token.IsCancellationRequested)
			{
				return;
			}
			line = 3;
			list = objList?.ToList();
			line = 4;
			if (token.IsCancellationRequested)
			{
				return;
			}
			line = 5;
			ReturnedSaveFuncInfoWithValue<WebPageContainer<object>> proccessedPagedList = list?.ProgressPages(CurrentPage, PageIdx);
			line = 6;
			if (token.IsCancellationRequested)
			{
				return;
			}
			line = 7;
			TotalCount = proccessedPagedList.value.TotalPages;
			line = 8;
			if (PageIdx > TotalCount)
			{
				PageIdx = TotalCount;
			}
			line = 9;
			if (token.IsCancellationRequested)
			{
				return;
			}
			line = 10;
			await SetLabelAsync(token);
			line = 11;
			if (token.IsCancellationRequested)
			{
				return;
			}
			line = 12;
			if (position.HasValue && position.GetValueOrDefault() == PagingPosition.GotoEndPage)
			{
				line = 13;
				((Control)this).Invoke((Delegate)(Action)delegate
				{
					PageIdx = proccessedPagedList.value.TotalPages;
				});
			}
			line = 14;
			ea.ListData = proccessedPagedList.value?.Items?.Select((NumberContainer<object> p) => p.Item)?.ToList();
			line = 15;
			if (!token.IsCancellationRequested)
			{
				line = 16;
				Pagelist = ea.ListData;
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"1111///Line:{line}", "PaggerAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UcBinding\\UC_FooterPaging.cs", 181);
		}
		finally
		{
			this.OnBindDataReady?.Invoke(this, ea);
		}
	}

	public List<object> NextItemsInPage(int start, int count)
	{
		int num = 0;
		try
		{
			num = 1;
			if (start + count > Pagelist.Count)
			{
				num = 2;
				return Pagelist.GetRange(start, Pagelist.Count - start);
			}
			num = 3;
			return Pagelist.GetRange(start, count);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}//start:{start}//count:{count}//Pagelist:{Pagelist?.Count ?? 0}", "NextItemsInPage", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UcBinding\\UC_FooterPaging.cs", 204);
			return null;
		}
	}

	private async Task SetLabelAsync(CancellationToken token)
	{
		int line = 0;
		try
		{
			int maxDelay = 100;
			if (IsDisposed)
			{
				return;
			}
			line = 1;
			while (!((Control)this).IsHandleCreated)
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				await Task.Delay(100);
				line = 2;
				maxDelay--;
				if (maxDelay <= 0)
				{
					return;
				}
			}
			line = 30;
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			line = 999;
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Expected O, but got Unknown
				try
				{
					line = 4;
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						if (TotalCount > 999)
						{
							((Control)LblPage).Font = new Font(((Control)LblPage).Font.FontFamily, 8f);
						}
						line = 5;
						Label lblPage = LblPage;
						List<object> obj = list;
						((Control)lblPage).Text = ((obj != null && obj.Count() > 0) ? $"از {TotalCount}" : "");
						line = 6;
						((Control)txtFromPage).Text = PageIdx.ToString();
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, $"Error in Line:{line}", "SetLabelAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UcBinding\\UC_FooterPaging.cs", 240);
				}
			});
		}
		catch (Exception ex)
		{
			if (line != 999)
			{
				WebErrorLog.Instance.StartLog(ex, $"Error in Line:{line}", "SetLabelAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UcBinding\\UC_FooterPaging.cs", 246);
			}
		}
	}

	private void BtnBack_Click(object sender, EventArgs e)
	{
		try
		{
			if (PageIdx > 1)
			{
				PageIdx--;
				_token_cus?.Cancel();
				_token_cus = new CancellationTokenSource(60000);
				CancellationToken tkn = _token_cus.Token;
				Task.Run(() => PaggerAsync(tkn, list));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnBack_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UcBinding\\UC_FooterPaging.cs", 264);
		}
	}

	private void BtnNext_Click(object sender, EventArgs e)
	{
		try
		{
			if (PageIdx < TotalCount)
			{
				PageIdx++;
				_token_cus?.Cancel();
				_token_cus = new CancellationTokenSource(60000);
				CancellationToken tkn = _token_cus.Token;
				Task.Run(() => PaggerAsync(tkn, list));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnNext_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UcBinding\\UC_FooterPaging.cs", 283);
		}
	}

	private void txtFromPage_TextChanged(object sender, EventArgs e)
	{
		try
		{
			PageIdx = ((Control)txtFromPage).Text.ParseToInt();
			if (PageIdx > 0 && PageIdx <= TotalCount)
			{
				_token_cus?.Cancel();
				_token_cus = new CancellationTokenSource(60000);
				CancellationToken tkn = _token_cus.Token;
				Task.Run(() => PaggerAsync(tkn, list));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtFromPage_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UcBinding\\UC_FooterPaging.cs", 302);
		}
	}

	protected override void Dispose(bool isDisposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				components = null;
				_token_cus?.Cancel();
				_token_cus?.Dispose();
				_token_cus = null;
				list = null;
				Pagelist = null;
				((ContainerControl)this).Dispose(isDisposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UcBinding\\UC_FooterPaging.cs", 324);
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
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Expected O, but got Unknown
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Expected O, but got Unknown
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Expected O, but got Unknown
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Expected O, but got Unknown
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		TxtTopCount = new NumericUpDown();
		label1 = new Label();
		LblPage = new Label();
		BtnNext = new Button();
		BtnBack = new Button();
		txtFromPage = new TextBox();
		((ISupportInitialize)TxtTopCount).BeginInit();
		((Control)this).SuspendLayout();
		((Control)TxtTopCount).Anchor = (AnchorStyles)10;
		((Control)TxtTopCount).Location = new Point(243, 1);
		TxtTopCount.Maximum = new decimal(new int[4] { 9999, 0, 0, 0 });
		TxtTopCount.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)TxtTopCount).Name = "TxtTopCount";
		((Control)TxtTopCount).Size = new Size(56, 27);
		((Control)TxtTopCount).TabIndex = 30;
		TxtTopCount.Value = new decimal(new int[4] { 100, 0, 0, 0 });
		TxtTopCount.ValueChanged += TxtTopCount_ValueChanged;
		((Control)label1).Anchor = (AnchorStyles)10;
		((Control)label1).Location = new Point(303, 1);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(142, 27);
		((Control)label1).TabIndex = 29;
		((Control)label1).Text = "[تعداد نمایش در هر صفحه]:";
		label1.TextAlign = (ContentAlignment)64;
		((Control)LblPage).Anchor = (AnchorStyles)6;
		((Control)LblPage).ForeColor = SystemColors.ActiveCaptionText;
		((Control)LblPage).Location = new Point(66, 1);
		((Control)LblPage).Name = "LblPage";
		((Control)LblPage).Size = new Size(52, 27);
		((Control)LblPage).TabIndex = 28;
		((Control)LblPage).Text = "از 000";
		LblPage.TextAlign = (ContentAlignment)32;
		((Control)BtnNext).Anchor = (AnchorStyles)6;
		((Control)BtnNext).Cursor = Cursors.Hand;
		((Control)BtnNext).Font = new Font("Tahoma", 6f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnNext).Location = new Point(7, 1);
		((Control)BtnNext).Name = "BtnNext";
		((Control)BtnNext).Size = new Size(53, 27);
		((Control)BtnNext).TabIndex = 27;
		((Control)BtnNext).Text = "[صفحه بعد]";
		((ButtonBase)BtnNext).UseVisualStyleBackColor = true;
		((Control)BtnNext).Click += BtnNext_Click;
		((Control)BtnBack).Anchor = (AnchorStyles)6;
		((Control)BtnBack).Cursor = Cursors.Hand;
		((Control)BtnBack).Font = new Font("Tahoma", 6f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnBack).Location = new Point(184, 0);
		((Control)BtnBack).Name = "BtnBack";
		((Control)BtnBack).Size = new Size(53, 28);
		((Control)BtnBack).TabIndex = 26;
		((Control)BtnBack).Text = "[صفحه قبل]";
		((ButtonBase)BtnBack).UseVisualStyleBackColor = true;
		((Control)BtnBack).Click += BtnBack_Click;
		((Control)txtFromPage).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)txtFromPage).Location = new Point(124, 0);
		((TextBoxBase)txtFromPage).Multiline = true;
		((Control)txtFromPage).Name = "txtFromPage";
		((Control)txtFromPage).Size = new Size(48, 22);
		((Control)txtFromPage).TabIndex = 31;
		((Control)txtFromPage).TextChanged += txtFromPage_TextChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)txtFromPage);
		((Control)this).Controls.Add((Control)(object)TxtTopCount);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)LblPage);
		((Control)this).Controls.Add((Control)(object)BtnNext);
		((Control)this).Controls.Add((Control)(object)BtnBack);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UC_FooterPaging";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(448, 28);
		((ISupportInitialize)TxtTopCount).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
