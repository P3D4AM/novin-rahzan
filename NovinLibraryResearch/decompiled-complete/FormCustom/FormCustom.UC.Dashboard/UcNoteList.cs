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
using EntityCache.Business.Accounting.Note;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace FormCustom.UC.Dashboard;

public class UcNoteList : UserControl, IHasContext
{
	private CancellationTokenSource _tokenSource;

	private IContainer components;

	private PanelEx pnlHeader;

	private FlowLayoutPanel flowLayoutPanel1;

	public string ContextGuid { get; private set; }

	public event Action<Guid> OnNoteSelected;

	public event Action<int> OnBindComleted;

	public async Task SetContextAsync(string contextGuid, int count, DateTime? date1, DateTime? date2)
	{
		try
		{
			_tokenSource?.Cancel();
			_tokenSource = new CancellationTokenSource();
			CancellationToken token = _tokenSource.Token;
			await Task.Delay(new Random().Next(1000));
			if (((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			ContextGuid = contextGuid;
			Guid currentCashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
			NoteFilter filter = new NoteFilter
			{
				Date1 = date1,
				Date2 = date2,
				CashierGuid = currentCashierGuid
			};
			IEnumerable<NoteBusiness> list = await NoteBusiness.GetAllAsync(contextGuid, EnSystem.UserInterface, filter, default(CancellationToken));
			if (count <= 0)
			{
				count = 25;
			}
			list = list?.Where((NoteBusiness q) => q.Status == NoteStatus.UnRead)?.OrderByDescending((NoteBusiness q) => q.DateSarResidM)?.Take(count)?.ToList();
			if (!(list?.Any() ?? false) || ((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					((Control)this).SuspendLayout();
					if (!((Control)this).IsDisposed && !token.IsCancellationRequested)
					{
						((Control)flowLayoutPanel1).Controls.Clear();
						foreach (NoteBusiness item in list)
						{
							UcNote ucNote = new UcNote();
							((Control)ucNote).Width = ((Control)flowLayoutPanel1).Width - 25;
							((Control)ucNote).Dock = (DockStyle)1;
							UcNote ucNote2 = ucNote;
							ucNote2.SetNote(item);
							ucNote2.OnNoteSelected += Uc_OnNoteSelected;
							((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucNote2);
						}
						this.OnBindComleted?.Invoke(list?.Count() ?? 0);
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "SetContextAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcNoteList.cs", 65);
				}
				finally
				{
					if (!((Control)this).IsDisposed && !token.IsCancellationRequested)
					{
						((Control)this).ResumeLayout();
					}
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetContextAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcNoteList.cs", 69);
		}
	}

	private void Uc_OnNoteSelected(Guid guid)
	{
		this.OnNoteSelected?.Invoke(guid);
	}

	public UcNoteList()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
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
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		pnlHeader = new PanelEx();
		flowLayoutPanel1 = new FlowLayoutPanel();
		((Control)pnlHeader).SuspendLayout();
		((Control)this).SuspendLayout();
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)flowLayoutPanel1);
		pnlHeader.DisabledBackColor = Color.Empty;
		((Control)pnlHeader).Dock = (DockStyle)5;
		((Control)pnlHeader).Location = new Point(0, 0);
		((Control)pnlHeader).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlHeader).Name = "pnlHeader";
		((Control)pnlHeader).Size = new Size(495, 299);
		pnlHeader.Style.BackColor1.Color = Color.White;
		pnlHeader.Style.BackColor2.Color = Color.White;
		pnlHeader.Style.Border = eBorderType.SingleLine;
		pnlHeader.Style.BorderColor.Color = Color.White;
		pnlHeader.Style.BorderWidth = 0;
		pnlHeader.Style.CornerDiameter = 10;
		pnlHeader.Style.CornerType = eCornerType.Rounded;
		((Control)pnlHeader).TabIndex = 147;
		((ScrollableControl)flowLayoutPanel1).AutoScroll = true;
		((Control)flowLayoutPanel1).Dock = (DockStyle)5;
		((Control)flowLayoutPanel1).Location = new Point(0, 0);
		((Control)flowLayoutPanel1).Name = "flowLayoutPanel1";
		((Control)flowLayoutPanel1).Size = new Size(495, 299);
		((Control)flowLayoutPanel1).TabIndex = 6;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pnlHeader);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcNoteList";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(495, 299);
		((Control)pnlHeader).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
