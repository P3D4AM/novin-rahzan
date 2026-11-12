using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Helpers;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiDatabaseSelectForm : StiFormUX
{
	internal enum StiDbCategoryType
	{
		All,
		ReportConnection,
		SQL,
		NoSQL,
		Azure,
		Google,
		OnlineServices,
		File,
		Rest,
		Objects,
		Favorites,
		Recent
	}

	internal enum StiDbButtonType
	{
		FavoriteButton,
		ToggleButton,
		RecentFavoriteButton
	}

	internal class StiDbCategory
	{
		private StiDatabaseSelectForm form;

		private List<StiDatabaseItem> finalItems = new List<StiDatabaseItem>();

		private List<StiDatabaseItem> favoriteItems = new List<StiDatabaseItem>();

		private StiLabelUX labelReportConnection;

		private StiLabelUX labelFavorite;

		private StiLabelUX labelRecent;

		private StiLabelUX labelGroup;

		public StiDbCategoryType CategoryType { get; }

		public List<StiDatabaseItem> RecentItems { get; set; } = new List<StiDatabaseItem>();


		public List<StiDatabaseItem> ReportConnectionItems { get; set; } = new List<StiDatabaseItem>();


		public List<StiDatabaseItem> Items { get; set; } = new List<StiDatabaseItem>();


		public StiDbCategory(StiDbCategoryType categoryType, StiDatabaseSelectForm form)
		{
			CategoryType = categoryType;
			this.form = form;
		}

		public StiLabelUX GetGroupLabel(string text)
		{
			double num = Math.Floor((double)((Control)form.panelItems).Width / (double)StiScale.XXI(164.0));
			int width = StiScale.XXI(164.0 * num + 18.0);
			StiLabelUX stiLabelUX = new StiLabelUX();
			((Control)stiLabelUX).BackColor = StiUX.BackgroundLight;
			((Control)stiLabelUX).Text = text;
			((Control)stiLabelUX).ForeColor = StiUX.Foreground;
			((Control)stiLabelUX).Size = new Size(width, StiScale.YYI(24.0));
			((Label)stiLabelUX).TextAlign = (ContentAlignment)16;
			return stiLabelUX;
		}

		public string GetName()
		{
			return GetName(CategoryType);
		}

		private string GetName(StiDbCategoryType categoryType)
		{
			return categoryType switch
			{
				StiDbCategoryType.All => StiLocalization.Get("PropertyEnum", "StiRestrictionsAll"), 
				StiDbCategoryType.ReportConnection => StiLocalization.Get("FormDatabaseEdit", "ReportConnections"), 
				StiDbCategoryType.SQL => "SQL", 
				StiDbCategoryType.NoSQL => "NoSQL", 
				StiDbCategoryType.Azure => "Azure", 
				StiDbCategoryType.Google => "Google", 
				StiDbCategoryType.OnlineServices => "Online Services", 
				StiDbCategoryType.File => StiLocalization.Get("PropertyMain", "File"), 
				StiDbCategoryType.Rest => "REST", 
				StiDbCategoryType.Objects => StiLocalization.Get("QueryBuilder", "Objects"), 
				StiDbCategoryType.Favorites => StiLocalization.Get("FormDatabaseEdit", "Favorites"), 
				StiDbCategoryType.Recent => StiLocalization.Get("FormDatabaseEdit", "RecentConnections"), 
				_ => "", 
			};
		}

		public void PrepareItems()
		{
			favoriteItems = (from x in Items
				where x.Database != null && x.IsFavorite
				orderby x.Database.ConnectionOrder
				select x).ToList();
			List<StiDatabaseItem> collection = (from x in Items
				where x.Database != null && !x.IsFavorite
				orderby x.Database.ConnectionOrder
				select x).ToList();
			List<StiDatabaseItem> collection2 = (from x in Items
				where x.Adapter != null
				orderby x.Adapter.ServiceName
				select x).ToList();
			finalItems.Clear();
			finalItems.AddRange(collection);
			finalItems.AddRange(collection2);
		}

		public void ProcessViewData()
		{
			if (CategoryType == StiDbCategoryType.All)
			{
				ProcessAllViewData();
			}
			else
			{
				ProcessDefaultViewData();
			}
		}

		private void ProcessAllViewData()
		{
			((Control)form.panelItems).Controls.Clear();
			if (labelReportConnection == null)
			{
				labelReportConnection = GetGroupLabel(StiLocalization.Get("FormDatabaseEdit", "ReportConnections"));
				labelFavorite = GetGroupLabel(StiLocalization.Get("FormDatabaseEdit", "Favorites"));
				labelRecent = GetGroupLabel(StiLocalization.Get("FormDatabaseEdit", "RecentConnections"));
			}
			int num = 0;
			int num2 = 0;
			List<Control> list = new List<Control>();
			if (ReportConnectionItems.Count > 0)
			{
				if (string.IsNullOrEmpty(((Control)form.buttonFindComponent).Text))
				{
					foreach (StiDatabaseItem reportConnectionItem in ReportConnectionItems)
					{
						list.Add(reportConnectionItem.GetControl());
					}
				}
				else
				{
					foreach (StiDatabaseItem item in ReportConnectionItems.Where((StiDatabaseItem x) => x.Text.ToLowerInvariant().Contains(((Control)form.buttonFindComponent).Text.ToLowerInvariant())))
					{
						list.Add(item.GetControl());
					}
				}
				if (list.Count > 0)
				{
					((Control)labelReportConnection).Location = new Point(0, num2);
					((Control)form.panelItems).Controls.Add((Control)(object)labelReportConnection);
					num2 += ((Control)labelReportConnection).Height + StiScale.YYI(6.0);
					bool flag = true;
					foreach (Control item2 in list)
					{
						if (num + item2.Width > ((Control)form.panelItems).Width)
						{
							num = 0;
							num2 += StiScale.YYI(48.0);
							flag = true;
						}
						item2.Left = num;
						item2.Top = num2;
						num += item2.Width;
						num += StiScale.XXI(6.0);
						((Control)form.panelItems).Controls.Add(item2);
						flag = false;
					}
					if (!flag)
					{
						num = 0;
						num2 += StiScale.YYI(48.0);
					}
				}
			}
			list = new List<Control>();
			if (favoriteItems.Count > 0)
			{
				if (string.IsNullOrEmpty(((Control)form.buttonFindComponent).Text))
				{
					foreach (StiDatabaseItem favoriteItem in favoriteItems)
					{
						list.Add(favoriteItem.GetControl());
					}
				}
				else
				{
					foreach (StiDatabaseItem item3 in favoriteItems.Where((StiDatabaseItem x) => x.Text.ToLowerInvariant().Contains(((Control)form.buttonFindComponent).Text.ToLowerInvariant())))
					{
						list.Add(item3.GetControl());
					}
				}
				if (list.Count > 0)
				{
					((Control)labelFavorite).Location = new Point(0, num2);
					((Control)form.panelItems).Controls.Add((Control)(object)labelFavorite);
					num2 += ((Control)labelFavorite).Height + StiScale.YYI(6.0);
					bool flag2 = true;
					foreach (Control item4 in list)
					{
						if (num + item4.Width > ((Control)form.panelItems).Width)
						{
							num = 0;
							num2 += StiScale.YYI(48.0);
							flag2 = true;
						}
						item4.Left = num;
						item4.Top = num2;
						num += item4.Width;
						num += StiScale.XXI(6.0);
						((Control)form.panelItems).Controls.Add(item4);
						flag2 = false;
					}
					if (!flag2)
					{
						num = 0;
						num2 += StiScale.YYI(48.0);
					}
				}
			}
			list = new List<Control>();
			if (RecentItems.Count > 0)
			{
				if (string.IsNullOrEmpty(((Control)form.buttonFindComponent).Text))
				{
					foreach (StiDatabaseItem recentItem in RecentItems)
					{
						list.Add(recentItem.GetControl());
					}
				}
				else
				{
					foreach (StiDatabaseItem item5 in RecentItems.Where((StiDatabaseItem x) => x.Text.ToLowerInvariant().Contains(((Control)form.buttonFindComponent).Text.ToLowerInvariant())))
					{
						list.Add(item5.GetControl());
					}
				}
				if (list.Count > 0)
				{
					((Control)labelRecent).Location = new Point(0, num2);
					((Control)form.panelItems).Controls.Add((Control)(object)labelRecent);
					num2 += ((Control)labelRecent).Height + StiScale.YYI(6.0);
					bool flag3 = true;
					foreach (Control item6 in list)
					{
						if (num + item6.Width > ((Control)form.panelItems).Width)
						{
							num = 0;
							num2 += StiScale.YYI(48.0);
							flag3 = true;
						}
						item6.Left = num;
						item6.Top = num2;
						num += item6.Width;
						num += StiScale.XXI(6.0);
						((Control)form.panelItems).Controls.Add(item6);
						flag3 = false;
					}
					if (!flag3)
					{
						num = 0;
						num2 += StiScale.YYI(48.0);
					}
				}
			}
			list.Clear();
			foreach (StiDbCategoryType catType in Enum.GetValues(typeof(StiDbCategoryType)))
			{
				IEnumerable<StiDatabaseItem> enumerable = finalItems.Where((StiDatabaseItem i) => i.CategoryType == catType);
				if (enumerable.Count() == 0)
				{
					continue;
				}
				if (string.IsNullOrEmpty(((Control)form.buttonFindComponent).Text))
				{
					foreach (StiDatabaseItem item7 in enumerable)
					{
						list.Add(item7.GetControl());
					}
				}
				else
				{
					foreach (StiDatabaseItem item8 in enumerable.Where((StiDatabaseItem x) => x.Text.ToLowerInvariant().Contains(((Control)form.buttonFindComponent).Text.ToLowerInvariant())))
					{
						list.Add(item8.GetControl());
					}
				}
				if (list.Count > 0)
				{
					StiLabelUX groupLabel = GetGroupLabel(GetName(catType));
					((Control)groupLabel).Location = new Point(0, num2);
					((Control)form.panelItems).Controls.Add((Control)(object)groupLabel);
					num2 = ((Control)groupLabel).Bottom + StiScale.YYI(6.0);
					num = 0;
					foreach (Control item9 in list)
					{
						if (num + item9.Width > ((Control)form.panelItems).Width)
						{
							num = 0;
							num2 += StiScale.YYI(48.0);
						}
						item9.Left = num;
						item9.Top = num2;
						num += item9.Width;
						num += StiScale.XXI(6.0);
						((Control)form.panelItems).Controls.Add(item9);
					}
					num2 += StiScale.YYI(48.0);
				}
				list.Clear();
			}
		}

		private void ProcessDefaultViewData()
		{
			((Control)form.panelItems).Controls.Clear();
			if (labelGroup == null)
			{
				double num = Math.Floor((double)((Control)form.panelItems).Width / (double)StiScale.XXI(164.0));
				int width = StiScale.XXI(164.0 * num + 18.0);
				StiLabelUX stiLabelUX = new StiLabelUX();
				((Control)stiLabelUX).BackColor = StiUX.BackgroundLight;
				((Control)stiLabelUX).Text = GetName();
				((Control)stiLabelUX).ForeColor = StiUX.Foreground;
				((Control)stiLabelUX).Size = new Size(width, StiScale.YYI(24.0));
				((Label)stiLabelUX).TextAlign = (ContentAlignment)16;
				labelGroup = stiLabelUX;
			}
			((Control)form.panelItems).Controls.Add((Control)(object)labelGroup);
			List<Control> list = new List<Control>();
			if (string.IsNullOrEmpty(((Control)form.buttonFindComponent).Text))
			{
				foreach (StiDatabaseItem item in Items)
				{
					list.Add(item.GetControl());
				}
			}
			else
			{
				foreach (StiDatabaseItem item2 in Items.Where((StiDatabaseItem x) => x.Text.ToLowerInvariant().Contains(((Control)form.buttonFindComponent).Text.ToLowerInvariant())))
				{
					list.Add(item2.GetControl());
				}
			}
			int num2 = 0;
			int num3 = StiScale.YYI(6.0) + ((Control)labelGroup).Height;
			foreach (Control item3 in list)
			{
				if (num2 + item3.Width > ((Control)form.panelItems).Width)
				{
					num2 = 0;
					num3 += StiScale.YYI(48.0);
				}
				item3.Left = num2;
				item3.Top = num3;
				num2 += item3.Width;
				num2 += StiScale.XXI(5.0);
				((Control)form.panelItems).Controls.Add(item3);
			}
		}

		public override string ToString()
		{
			return GetName();
		}
	}

	internal class StiDatabaseItem
	{
		internal StiDatabaseSelectForm form;

		private Image image;

		private StiDbButtonType buttonType;

		private string tooltip;

		private Control control;

		public bool IsCreatedConnection { get; }

		public string Text { get; }

		public StiDatabase Database { get; }

		public StiDataAdapterService Adapter { get; }

		public StiRecentConnections.Connection Recent { get; }

		public bool IsFavorite { get; set; }

		public StiDbCategoryType? CategoryType { get; set; }

		public StiDatabaseItem(StiDatabaseSelectForm form, StiDatabase database, string text, Image image, StiDbButtonType buttonType, StiDbCategoryType categoryType, bool isCreatedConnection = false)
		{
			this.form = form;
			Database = database;
			Text = text;
			this.image = image;
			this.buttonType = buttonType;
			CategoryType = categoryType;
			IsCreatedConnection = isCreatedConnection;
		}

		public StiDatabaseItem(StiDatabaseSelectForm form, StiDataAdapterService adapter, string text, Image image, StiDbButtonType buttonType)
		{
			this.form = form;
			Adapter = adapter;
			Text = text;
			this.image = image;
			this.buttonType = buttonType;
			CategoryType = StiDbCategoryType.Objects;
		}

		public StiDatabaseItem(StiDatabaseSelectForm form, StiRecentConnections.Connection recent, string text, string tooltip, Image image, StiDbButtonType buttonType)
		{
			this.form = form;
			Recent = recent;
			Text = text;
			this.tooltip = tooltip;
			this.image = image;
			this.buttonType = buttonType;
		}

		public Control GetControl()
		{
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Expected O, but got Unknown
			if (control == null)
			{
				switch (buttonType)
				{
				case StiDbButtonType.FavoriteButton:
				{
					DatabaseFavoriteButton databaseFavoriteButton = new DatabaseFavoriteButton(this, Text, image);
					((Control)databaseFavoriteButton).Tag = this;
					databaseFavoriteButton.IsFavorite = IsFavorite;
					DatabaseFavoriteButton databaseFavoriteButton2 = databaseFavoriteButton;
					((Control)databaseFavoriteButton2).Click += Button_Click;
					control = (Control)(object)databaseFavoriteButton2;
					break;
				}
				case StiDbButtonType.ToggleButton:
				{
					DatabaseToggleButton databaseToggleButton = new DatabaseToggleButton(Text, image);
					((Control)databaseToggleButton).Tag = this;
					DatabaseToggleButton databaseToggleButton2 = databaseToggleButton;
					((Control)databaseToggleButton2).Click += Button_Click;
					control = (Control)(object)databaseToggleButton2;
					break;
				}
				case StiDbButtonType.RecentFavoriteButton:
				{
					DatabaseRecentFavoriteButton databaseRecentFavoriteButton = new DatabaseRecentFavoriteButton(this, Text, image);
					((Control)databaseRecentFavoriteButton).Tag = this;
					databaseRecentFavoriteButton.IsFavorite = IsFavorite;
					DatabaseRecentFavoriteButton databaseRecentFavoriteButton2 = databaseRecentFavoriteButton;
					((Control)databaseRecentFavoriteButton2).Click += Button_Click;
					((Control)databaseRecentFavoriteButton2).ContextMenuStrip = GetConextMenu(this);
					if (tooltip != null)
					{
						ToolTip val = new ToolTip();
						val.SetToolTip((Control)(object)databaseRecentFavoriteButton2, tooltip);
					}
					control = (Control)(object)databaseRecentFavoriteButton2;
					break;
				}
				}
			}
			return control;
		}

		private ContextMenuStrip GetConextMenu(StiDatabaseItem databaseItem)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			ContextMenuStrip val = new ContextMenuStrip();
			ToolStripMenuItem val2 = new ToolStripMenuItem(StiLocalization.Get("Buttons", "Remove"), StiReportDesignImages.Actions.Remove(), (EventHandler)MenuItemRemove_Click);
			((ToolStripItem)val2).Tag = databaseItem;
			((ToolStrip)val).Items.Add((ToolStripItem)(object)val2);
			return val;
		}

		private void MenuItemRemove_Click(object sender, EventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			StiDatabaseItem item = (StiDatabaseItem)((ToolStripItem)(ToolStripMenuItem)sender).Tag;
			form.ButtonRemoveItem_Click(item);
		}

		private void Button_Click(object sender, EventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			StiDatabaseItem item = (StiDatabaseItem)((Control)sender).Tag;
			form.ButtonItem_Click(item);
		}
	}

	[ToolboxItem(false)]
	internal class DatabaseFavoriteButton : Control
	{
		private StiDatabaseItem item;

		private bool isMouseOver;

		private bool isPressed;

		private bool isFavoriteMouseOver;

		private Image icon;

		private Rectangle favoriteRect;

		private bool isFavorite;

		public bool IsFavorite
		{
			get
			{
				return isFavorite;
			}
			set
			{
				isFavorite = value;
				((Control)this).Invalidate();
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			((Control)this).OnMouseEnter(e);
			isMouseOver = true;
			isFavoriteMouseOver = false;
			((Control)this).Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			((Control)this).OnMouseLeave(e);
			isMouseOver = false;
			isPressed = false;
			isFavoriteMouseOver = false;
			((Control)this).Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			((Control)this).OnMouseDown(e);
			isPressed = true;
			((Control)this).Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			((Control)this).OnMouseUp(e);
			isPressed = false;
			((Control)this).Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			((Control)this).OnMouseMove(e);
			isFavoriteMouseOver = favoriteRect.Contains(e.Location);
		}

		protected override void OnClick(EventArgs e)
		{
			if (isFavoriteMouseOver)
			{
				isFavorite = !isFavorite;
				item.IsFavorite = isFavorite;
				StiSettings.Set("DatabaseSelectWindow", item.Database.GetType().Name + "IsFavorite", isFavorite);
				item.form.Favorite_Click(item, isFavorite);
			}
			else
			{
				((Control)this).OnClick(e);
			}
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Expected O, but got Unknown
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Expected O, but got Unknown
			((Control)this).OnPaint(pevent);
			Graphics graphics = pevent.Graphics;
			Rectangle rect = new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
			if (isPressed)
			{
				StiDrawing.FillRectangle(graphics, StiUX.IsDark ? StiUX.ButtonBackgroundPressed : Color.FromArgb(255, 163, 189, 227), rect);
			}
			else if (isMouseOver)
			{
				StiDrawing.FillRectangle(graphics, StiUX.IsDark ? StiUX.ButtonBackgroundHover : Color.FromArgb(255, 213, 225, 242), rect);
			}
			else
			{
				StiDrawing.FillRectangle(graphics, StiUX.ButtonBackground, rect);
			}
			if (!StiUX.IsDark)
			{
				StiDrawing.DrawRectangle(graphics, Color.FromArgb(255, 181, 193, 206), rect);
			}
			if (icon != null)
			{
				graphics.DrawImage(icon, StiScale.I5, StiScale.I5, StiScale.I(32), StiScale.I(32));
			}
			if (((Control)this).Text != null)
			{
				SolidBrush val = new SolidBrush(StiUX.IsDark ? StiUX.ButtonForeground : Color.FromArgb(255, 68, 68, 68));
				try
				{
					StringFormat val2 = new StringFormat();
					try
					{
						val2.Alignment = (StringAlignment)0;
						val2.LineAlignment = (StringAlignment)1;
						graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)(object)val, new RectangleF(StiScale.I(42), StiScale.I5, ((Control)this).Width - StiScale.I(45), StiScale.I(32)), val2);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			if (isFavorite || isMouseOver || isPressed)
			{
				Image val3 = (isFavorite ? favoriteSelectIcon : favoriteIcon);
				if (val3 != null)
				{
					graphics.DrawImage(val3, favoriteRect);
				}
			}
		}

		public DatabaseFavoriteButton(StiDatabaseItem item, string text, Image icon)
		{
			this.item = item;
			((Control)this).Text = text;
			this.icon = icon;
			((Control)this).Width = StiScale.XXI(164.0);
			((Control)this).Height = StiScale.YYI(42.0);
			favoriteRect = new Rectangle(StiScale.I(144), StiScale.I4, StiScale.I16, StiScale.I16);
		}
	}

	[ToolboxItem(false)]
	internal class DatabaseToggleButton : Control
	{
		private bool isMouseOver;

		private bool isPressed;

		private Image icon;

		protected override void OnMouseEnter(EventArgs e)
		{
			((Control)this).OnMouseEnter(e);
			isMouseOver = true;
			((Control)this).Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			((Control)this).OnMouseLeave(e);
			isMouseOver = false;
			isPressed = false;
			((Control)this).Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			((Control)this).OnMouseDown(e);
			isPressed = true;
			((Control)this).Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			((Control)this).OnMouseUp(e);
			isPressed = false;
			((Control)this).Invalidate();
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Expected O, but got Unknown
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Expected O, but got Unknown
			((Control)this).OnPaint(pevent);
			Graphics graphics = pevent.Graphics;
			Rectangle rect = new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
			if (isPressed)
			{
				StiDrawing.FillRectangle(graphics, StiUX.IsDark ? StiUX.ButtonBackgroundPressed : Color.FromArgb(255, 163, 189, 227), rect);
			}
			else if (isMouseOver)
			{
				StiDrawing.FillRectangle(graphics, StiUX.IsDark ? StiUX.ButtonBackgroundHover : Color.FromArgb(255, 213, 225, 242), rect);
			}
			else
			{
				StiDrawing.FillRectangle(graphics, StiUX.ButtonBackground, rect);
			}
			if (!StiUX.IsDark)
			{
				StiDrawing.DrawRectangle(graphics, Color.FromArgb(255, 181, 193, 206), rect);
			}
			if (icon != null)
			{
				graphics.DrawImage(icon, StiScale.I5, StiScale.I5, StiScale.I(32), StiScale.I(32));
			}
			if (((Control)this).Text == null)
			{
				return;
			}
			SolidBrush val = new SolidBrush(StiUX.IsDark ? StiUX.ButtonForeground : Color.FromArgb(255, 68, 68, 68));
			try
			{
				StringFormat val2 = new StringFormat();
				try
				{
					val2.Alignment = (StringAlignment)0;
					val2.LineAlignment = (StringAlignment)1;
					graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)(object)val, new RectangleF(StiScale.I(42), StiScale.I(5), ((Control)this).Width - StiScale.I(45), StiScale.I(32)), val2);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		public DatabaseToggleButton(string text, Image icon)
		{
			((Control)this).Text = text;
			this.icon = icon;
			((Control)this).Width = StiScale.I(164);
			((Control)this).Height = StiScale.I(42);
		}
	}

	[ToolboxItem(false)]
	internal class DatabaseRecentFavoriteButton : Control
	{
		private StiDatabaseItem item;

		private bool isMouseOver;

		private bool isPressed;

		private Image icon;

		private SizeF textSize;

		private bool allowDrawText;

		private bool isFavoriteMouseOver;

		private Rectangle favoriteRect;

		private bool calculateTextSize;

		private bool isFavorite;

		public bool IsFavorite
		{
			get
			{
				return isFavorite;
			}
			set
			{
				isFavorite = value;
				((Control)this).Invalidate();
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			((Control)this).OnMouseEnter(e);
			isMouseOver = true;
			isFavoriteMouseOver = false;
			((Control)this).Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			((Control)this).OnMouseLeave(e);
			isMouseOver = false;
			isPressed = false;
			isFavoriteMouseOver = false;
			((Control)this).Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			((Control)this).OnMouseDown(e);
			isPressed = true;
			((Control)this).Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			((Control)this).OnMouseUp(e);
			isPressed = false;
			((Control)this).Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			((Control)this).OnMouseMove(e);
			isFavoriteMouseOver = favoriteRect.Contains(e.Location);
		}

		protected override void OnClick(EventArgs e)
		{
			if (isFavoriteMouseOver)
			{
				isFavorite = !isFavorite;
				item.Recent.Pinned = !item.Recent.Pinned;
				item.form.FavoriteRecent_Click();
			}
			else
			{
				((Control)this).OnClick(e);
			}
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Expected O, but got Unknown
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Expected O, but got Unknown
			((Control)this).OnPaint(pevent);
			Graphics graphics = pevent.Graphics;
			if (!calculateTextSize)
			{
				calculateTextSize = true;
				if (!string.IsNullOrEmpty(((Control)this).Text))
				{
					textSize = graphics.MeasureString(((Control)this).Text, ((Control)this).Font);
					allowDrawText = true;
				}
				else
				{
					textSize = SizeF.Empty;
				}
			}
			Rectangle rect = new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
			if (isPressed)
			{
				StiDrawing.FillRectangle(graphics, StiUX.IsDark ? StiUX.ButtonBackgroundPressed : Color.FromArgb(255, 163, 189, 227), rect);
			}
			else if (isMouseOver)
			{
				StiDrawing.FillRectangle(graphics, StiUX.IsDark ? StiUX.ButtonBackgroundHover : Color.FromArgb(255, 213, 225, 242), rect);
			}
			else
			{
				StiDrawing.FillRectangle(graphics, StiUX.ButtonBackground, rect);
			}
			if (!StiUX.IsDark)
			{
				StiDrawing.DrawRectangle(graphics, Color.FromArgb(255, 181, 193, 206), rect);
			}
			if (icon != null)
			{
				graphics.DrawImage(icon, StiScale.I5, StiScale.I5, StiScale.I(32), StiScale.I(32));
			}
			if (allowDrawText)
			{
				RectangleF rectangleF = new RectangleF(StiScale.XXI(42.0), StiScale.YYI(5.0), ((Control)this).Width - StiScale.XXI(45.0), StiScale.YYI(32.0));
				SolidBrush val = new SolidBrush(StiUX.IsDark ? StiUX.ButtonForeground : Color.FromArgb(255, 68, 68, 68));
				try
				{
					StringFormat val2 = new StringFormat();
					try
					{
						val2.Alignment = (StringAlignment)0;
						val2.LineAlignment = (StringAlignment)1;
						val2.Trimming = (StringTrimming)3;
						if (textSize.Width > 0f)
						{
							graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)(object)val, rectangleF, val2);
						}
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			if (isFavorite || isMouseOver || isPressed)
			{
				graphics.DrawImage(isFavorite ? pinSelectIcon : pinIcon, favoriteRect);
			}
		}

		public DatabaseRecentFavoriteButton(StiDatabaseItem item, string text, Image icon)
		{
			this.item = item;
			((Control)this).Text = text;
			this.icon = icon;
			((Control)this).Width = StiScale.XXI(164.0);
			((Control)this).Height = StiScale.YYI(42.0);
			favoriteRect = new Rectangle(StiScale.XXI(144.0), StiScale.I4, StiScale.I16, StiScale.I16);
		}
	}

	[ToolboxItem(false)]
	public class RibbonFavoriteButton2016 : Control
	{
		private bool isMouseOver;

		private bool isPressed;

		private bool isFavoriteMouseOver;

		public static Image favoriteIcon;

		public static Image favoriteSelectIcon;

		private Rectangle favoriteRect = new Rectangle(StiScale.I(140), StiScale.I9, StiScale.I16, StiScale.I16);

		private bool isFavorite;

		public Image Icon { get; set; }

		public bool IsFavorite
		{
			get
			{
				return isFavorite;
			}
			set
			{
				isFavorite = value;
				((Control)this).Invalidate();
			}
		}

		public event EventHandler FavoriteClick;

		protected override void OnMouseEnter(EventArgs e)
		{
			((Control)this).OnMouseEnter(e);
			isMouseOver = true;
			isFavoriteMouseOver = false;
			((Control)this).Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			((Control)this).OnMouseLeave(e);
			isMouseOver = false;
			isPressed = false;
			isFavoriteMouseOver = false;
			((Control)this).Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			((Control)this).OnMouseDown(e);
			isPressed = true;
			((Control)this).Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			((Control)this).OnMouseUp(e);
			isPressed = false;
			((Control)this).Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			((Control)this).OnMouseMove(e);
			isFavoriteMouseOver = favoriteRect.Contains(e.Location);
		}

		protected override void OnClick(EventArgs e)
		{
			if (isFavoriteMouseOver)
			{
				isFavorite = !isFavorite;
				this.FavoriteClick?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				((Control)this).OnClick(e);
			}
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Expected O, but got Unknown
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Expected O, but got Unknown
			((Control)this).OnPaint(pevent);
			Graphics graphics = pevent.Graphics;
			Rectangle rect = new Rectangle(StiScale.I7, StiScale.I7, StiScale.I(151), StiScale.I(47));
			if (isPressed)
			{
				StiDrawing.FillRectangle(graphics, StiUX.IsDark ? StiUX.ButtonBackground : Color.FromArgb(255, 163, 189, 227), rect);
			}
			else if (isMouseOver)
			{
				StiDrawing.FillRectangle(graphics, StiUX.IsDark ? StiUX.ButtonBackgroundHover : Color.FromArgb(255, 213, 225, 242), rect);
			}
			else
			{
				StiDrawing.FillRectangle(graphics, StiUX.ButtonBackground, rect);
			}
			if (!StiUX.IsDark)
			{
				StiDrawing.DrawRectangle(graphics, Color.FromArgb(255, 181, 193, 206), rect);
			}
			if (Icon != null)
			{
				graphics.DrawImage(Icon, StiScale.I18, StiScale.I16, StiScale.I(32), StiScale.I(32));
			}
			if (((Control)this).Text != null)
			{
				SolidBrush val = new SolidBrush(StiUX.IsDark ? StiUX.ButtonForeground : Color.FromArgb(255, 68, 68, 68));
				try
				{
					StringFormat val2 = new StringFormat();
					try
					{
						val2.Alignment = (StringAlignment)0;
						val2.LineAlignment = (StringAlignment)1;
						graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)(object)val, new RectangleF(StiScale.I(54), StiScale.I16, StiScale.I(91), StiScale.I(32)), val2);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			if (isFavorite || isMouseOver || isPressed)
			{
				Image val3 = (isFavorite ? favoriteSelectIcon : favoriteIcon);
				if (val3 != null)
				{
					graphics.DrawImage(val3, favoriteRect);
				}
			}
		}

		public RibbonFavoriteButton2016()
		{
			((Control)this).Width = StiScale.XXI(165.0);
			((Control)this).Height = StiScale.YYI(61.0);
		}
	}

	private const int ButtonWidth = 164;

	private const int ButtonHeight = 42;

	private static Image favoriteIcon;

	private static Image favoriteSelectIcon;

	private static Image pinIcon;

	private static Image pinSelectIcon;

	private StiReport report;

	private List<StiDbCategory> categories = new List<StiDbCategory>();

	private Dictionary<StiDbCategoryType, StiDbCategory> cacheCategories = new Dictionary<StiDbCategoryType, StiDbCategory>();

	private IContainer components;

	private StiButtonPanelUX panelFooter;

	private StiButtonUX btCancel;

	private StiPanelUX panelRoot;

	private ContextMenuStrip contextMenuRecent;

	private ToolStripMenuItem contextMenuItemDelete;

	private ToolStripMenuItem contextMenuItemPin;

	private ToolStripMenuItem contextMenuItemUnpin;

	private StiCheckBoxUX checkBoxSkipSchemaWizard;

	private StiListBox listBoxCategories;

	private StiPanelUX panelItems;

	private StiPanelUX headerPanel;

	private StiPanelUX panelSeparator;

	private StiButtonEdit buttonFindComponent;

	private StiPanelUX panelList;

	internal bool UseOriginalName { get; set; }

	public StiDataAdapterService AdapterService { get; private set; }

	public StiDatabase SelectedDatabase { get; private set; }

	public StiDatabase CreatedDatabase { get; private set; }

	public bool SkipSchemaWizard => ((CheckBox)checkBoxSkipSchemaWizard).Checked;

	public StiDatabaseSelectForm(StiReport report)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			this.report = report;
			StiSettings.Load();
			((CheckBox)checkBoxSkipSchemaWizard).Checked = StiSettings.GetBool("StiDatabaseSelectWindow", "SkipSchemaWizard", nullValue: false);
			StiFormSettings.Load((Form)(object)this, skipLocation: false, skipSize: true);
			Build();
			Localize();
			((Form)this).ShowIcon = false;
			((Control)buttonFindComponent).Focus();
			DoTheming();
			DoScaling();
			((Form)this).Load += This_Load;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormDictionaryDesigner", "DataSourceNew");
		((Control)checkBoxSkipSchemaWizard).Text = StiLocalization.Get("FormDictionaryDesigner", "SkipSchemaWizard");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((ToolStripItem)contextMenuItemPin).Text = StiLocalization.Get("FormDatabaseEdit", "Pin");
		((ToolStripItem)contextMenuItemUnpin).Text = StiLocalization.Get("FormDatabaseEdit", "Unpin");
		((ToolStripItem)contextMenuItemDelete).Text = StiLocalization.Get("FormDictionaryDesigner", "Delete");
	}

	private Image GetDatabaseIcon(StiDatabase database)
	{
		Image image = (Image)(object)StiDesignerIconAttribute.GetImage(database.GetType());
		if (image != null)
		{
			return image;
		}
		try
		{
			return StiReportDesignImages.GetImage("Dictionary." + database.GetType().Name, StiImageSize.Double);
		}
		catch
		{
			return StiReportDesignImages.GetImage("Dictionary.StiSqlDatabase", StiImageSize.Double);
		}
	}

	private Image GetAdapterIcon(StiDataAdapterService adapter)
	{
		Image image = (Image)(object)StiDesignerIconAttribute.GetImage(adapter.GetType());
		if (image != null)
		{
			return image;
		}
		try
		{
			return StiReportDesignImages.GetImage("Dictionary." + adapter.GetType().Name + StiScale.StepName, StiImageSize.Double);
		}
		catch
		{
			return StiReportDesignImages.GetImage("Dictionary.StiDataTableAdapterService" + StiScale.StepName, StiImageSize.Double);
		}
	}

	private Image GetRecentConnectionsIcon(StiRecentConnections.Connection recent)
	{
		try
		{
			Image image = StiReportDesignImages.GetImage("Dictionary." + recent.DataAdapterType + StiScale.StepName, StiImageSize.Double);
			return (image is Bitmap) ? image : null;
		}
		catch
		{
			Image image2 = StiReportDesignImages.GetImage("Dictionary.StiDataTableAdapterService" + StiScale.StepName, StiImageSize.Double);
			return (image2 is Bitmap) ? image2 : null;
		}
	}

	private Dictionary<StiConnectionType, List<StiDatabase>> GetAllAvailableDatabases()
	{
		Dictionary<StiConnectionType, List<StiDatabase>> dictionary = new Dictionary<StiConnectionType, List<StiDatabase>>();
		foreach (StiDatabase database in StiOptions.Services.Databases)
		{
			if (!(database is StiUndefinedDatabase) && database.ServiceEnabled)
			{
				List<StiDatabase> list;
				if (dictionary.ContainsKey(database.ConnectionType))
				{
					list = dictionary[database.ConnectionType];
				}
				else
				{
					list = new List<StiDatabase>();
					dictionary.Add(database.ConnectionType, list);
				}
				list.Add(database);
			}
		}
		return dictionary;
	}

	private void DoTheming()
	{
		listBoxCategories.IsFormBackground = true;
	}

	private void DoScaling()
	{
		((ToolStripItem)contextMenuItemDelete).Image = StiReportDesignImages.Actions.Remove();
		((ToolStripItem)contextMenuItemPin).Image = StiReportDesignImages.Dictionary.FavoritesYellow();
		((ToolStripItem)contextMenuItemUnpin).Image = StiReportDesignImages.Dictionary.Favorites();
		favoriteIcon = StiReportDesignImages.Dictionary.Favorites();
		favoriteSelectIcon = StiReportDesignImages.Dictionary.FavoritesYellow();
		pinIcon = StiReportDesignImages.Dictionary.Pin();
		pinSelectIcon = StiReportDesignImages.Dictionary.Pinned();
	}

	private void Build()
	{
		categories.Add(new StiDbCategory(StiDbCategoryType.All, this));
		categories.Add(new StiDbCategory(StiDbCategoryType.ReportConnection, this));
		categories.Add(new StiDbCategory(StiDbCategoryType.Favorites, this));
		categories.Add(new StiDbCategory(StiDbCategoryType.Recent, this));
		categories.Add(new StiDbCategory(StiDbCategoryType.SQL, this));
		categories.Add(new StiDbCategory(StiDbCategoryType.NoSQL, this));
		categories.Add(new StiDbCategory(StiDbCategoryType.Azure, this));
		categories.Add(new StiDbCategory(StiDbCategoryType.Google, this));
		categories.Add(new StiDbCategory(StiDbCategoryType.OnlineServices, this));
		categories.Add(new StiDbCategory(StiDbCategoryType.File, this));
		categories.Add(new StiDbCategory(StiDbCategoryType.Rest, this));
		if (!StiOptions.Designer.IsBeginnerDesigner)
		{
			categories.Add(new StiDbCategory(StiDbCategoryType.Objects, this));
		}
		foreach (StiDbCategory category in categories)
		{
			cacheCategories.Add(category.CategoryType, category);
		}
		List<StiDatabase> list = (from x in report?.Dictionary.Databases.ToList()
			where report.Dictionary.Restrictions.IsAllowShow(x.Name, StiDataType.Database)
			select x).ToList();
		if (list != null && list.Count > 0)
		{
			foreach (StiDatabase item3 in list)
			{
				Image databaseIcon = GetDatabaseIcon(item3);
				StiDatabaseItem item = new StiDatabaseItem(this, item3, item3.Name, databaseIcon, StiDbButtonType.ToggleButton, StiDbCategoryType.ReportConnection, isCreatedConnection: true);
				cacheCategories[StiDbCategoryType.ReportConnection].Items.Add(item);
				cacheCategories[StiDbCategoryType.All].ReportConnectionItems.Add(item);
			}
		}
		Dictionary<StiConnectionType, List<StiDatabase>> allAvailableDatabases = GetAllAvailableDatabases();
		if (allAvailableDatabases.ContainsKey(StiConnectionType.Sql))
		{
			IOrderedEnumerable<StiDatabase> orderedEnumerable = allAvailableDatabases[StiConnectionType.Sql].OrderBy((StiDatabase x) => x.ConnectionOrder);
			foreach (StiDatabase item4 in orderedEnumerable)
			{
				Image databaseIcon2 = GetDatabaseIcon(item4);
				StiDatabaseItem stiDatabaseItem = new StiDatabaseItem(this, item4, item4.ServiceName, databaseIcon2, StiDbButtonType.FavoriteButton, StiDbCategoryType.SQL);
				if (StiSettings.GetBool("DatabaseSelectWindow", item4.GetType().Name + "IsFavorite", nullValue: false))
				{
					cacheCategories[StiDbCategoryType.Favorites].Items.Add(stiDatabaseItem);
					stiDatabaseItem.IsFavorite = true;
				}
				cacheCategories[StiDbCategoryType.SQL].Items.Add(stiDatabaseItem);
				cacheCategories[StiDbCategoryType.All].Items.Add(stiDatabaseItem);
			}
		}
		if (allAvailableDatabases.ContainsKey(StiConnectionType.NoSql))
		{
			IOrderedEnumerable<StiDatabase> orderedEnumerable2 = allAvailableDatabases[StiConnectionType.NoSql].OrderBy((StiDatabase x) => x.ConnectionOrder);
			foreach (StiDatabase item5 in orderedEnumerable2)
			{
				Image databaseIcon3 = GetDatabaseIcon(item5);
				StiDatabaseItem stiDatabaseItem2 = new StiDatabaseItem(this, item5, item5.ServiceName, databaseIcon3, StiDbButtonType.FavoriteButton, StiDbCategoryType.NoSQL);
				if (StiSettings.GetBool("DatabaseSelectWindow", item5.GetType().Name + "IsFavorite", nullValue: false))
				{
					cacheCategories[StiDbCategoryType.Favorites].Items.Add(stiDatabaseItem2);
					stiDatabaseItem2.IsFavorite = true;
				}
				cacheCategories[StiDbCategoryType.NoSQL].Items.Add(stiDatabaseItem2);
				cacheCategories[StiDbCategoryType.All].Items.Add(stiDatabaseItem2);
			}
		}
		if (allAvailableDatabases.ContainsKey(StiConnectionType.Azure))
		{
			IOrderedEnumerable<StiDatabase> orderedEnumerable3 = allAvailableDatabases[StiConnectionType.Azure].OrderBy((StiDatabase x) => x.ConnectionOrder);
			foreach (StiDatabase item6 in orderedEnumerable3)
			{
				Image databaseIcon4 = GetDatabaseIcon(item6);
				StiDatabaseItem stiDatabaseItem3 = new StiDatabaseItem(this, item6, item6.ServiceName, databaseIcon4, StiDbButtonType.FavoriteButton, StiDbCategoryType.Azure);
				if (StiSettings.GetBool("DatabaseSelectWindow", item6.GetType().Name + "IsFavorite", nullValue: false))
				{
					cacheCategories[StiDbCategoryType.Favorites].Items.Add(stiDatabaseItem3);
					stiDatabaseItem3.IsFavorite = true;
				}
				cacheCategories[StiDbCategoryType.Azure].Items.Add(stiDatabaseItem3);
				cacheCategories[StiDbCategoryType.All].Items.Add(stiDatabaseItem3);
			}
		}
		if (allAvailableDatabases.ContainsKey(StiConnectionType.Google))
		{
			IOrderedEnumerable<StiDatabase> orderedEnumerable4 = allAvailableDatabases[StiConnectionType.Google].OrderBy((StiDatabase x) => x.ConnectionOrder);
			foreach (StiDatabase item7 in orderedEnumerable4)
			{
				Image databaseIcon5 = GetDatabaseIcon(item7);
				StiDatabaseItem stiDatabaseItem4 = new StiDatabaseItem(this, item7, item7.ServiceName, databaseIcon5, StiDbButtonType.FavoriteButton, StiDbCategoryType.Google);
				if (StiSettings.GetBool("DatabaseSelectWindow", item7.GetType().Name + "IsFavorite", nullValue: false))
				{
					cacheCategories[StiDbCategoryType.Favorites].Items.Add(stiDatabaseItem4);
					stiDatabaseItem4.IsFavorite = true;
				}
				cacheCategories[StiDbCategoryType.Google].Items.Add(stiDatabaseItem4);
				cacheCategories[StiDbCategoryType.All].Items.Add(stiDatabaseItem4);
			}
		}
		if (allAvailableDatabases.ContainsKey(StiConnectionType.OnlineServices))
		{
			IOrderedEnumerable<StiDatabase> orderedEnumerable5 = allAvailableDatabases[StiConnectionType.OnlineServices].OrderBy((StiDatabase x) => x.ConnectionOrder);
			foreach (StiDatabase item8 in orderedEnumerable5)
			{
				Image databaseIcon6 = GetDatabaseIcon(item8);
				StiDatabaseItem stiDatabaseItem5 = new StiDatabaseItem(this, item8, item8.ServiceName, databaseIcon6, StiDbButtonType.FavoriteButton, StiDbCategoryType.OnlineServices);
				if (StiSettings.GetBool("DatabaseSelectWindow", item8.GetType().Name + "IsFavorite", nullValue: false))
				{
					cacheCategories[StiDbCategoryType.Favorites].Items.Add(stiDatabaseItem5);
					stiDatabaseItem5.IsFavorite = true;
				}
				cacheCategories[StiDbCategoryType.OnlineServices].Items.Add(stiDatabaseItem5);
				cacheCategories[StiDbCategoryType.All].Items.Add(stiDatabaseItem5);
			}
		}
		if (allAvailableDatabases.ContainsKey(StiConnectionType.Rest))
		{
			IOrderedEnumerable<StiDatabase> orderedEnumerable6 = allAvailableDatabases[StiConnectionType.Rest].OrderBy((StiDatabase x) => x.ConnectionOrder);
			foreach (StiDatabase item9 in orderedEnumerable6)
			{
				Image databaseIcon7 = GetDatabaseIcon(item9);
				StiDatabaseItem stiDatabaseItem6 = new StiDatabaseItem(this, item9, item9.ServiceName, databaseIcon7, StiDbButtonType.FavoriteButton, StiDbCategoryType.Rest);
				if (StiSettings.GetBool("DatabaseSelectWindow", item9.GetType().Name + "IsFavorite", nullValue: false))
				{
					cacheCategories[StiDbCategoryType.Favorites].Items.Add(stiDatabaseItem6);
					stiDatabaseItem6.IsFavorite = true;
				}
				cacheCategories[StiDbCategoryType.Rest].Items.Add(stiDatabaseItem6);
				cacheCategories[StiDbCategoryType.All].Items.Add(stiDatabaseItem6);
			}
		}
		if (allAvailableDatabases.ContainsKey(StiConnectionType.Other))
		{
			IOrderedEnumerable<StiDatabase> orderedEnumerable7 = allAvailableDatabases[StiConnectionType.Other].OrderBy((StiDatabase x) => x.ConnectionOrder);
			foreach (StiDatabase item10 in orderedEnumerable7)
			{
				Image databaseIcon8 = GetDatabaseIcon(item10);
				StiDatabaseItem stiDatabaseItem7 = new StiDatabaseItem(this, item10, item10.ServiceName, databaseIcon8, StiDbButtonType.FavoriteButton, StiDbCategoryType.File);
				if (StiSettings.GetBool("DatabaseSelectWindow", item10.GetType().Name + "IsFavorite", nullValue: false))
				{
					cacheCategories[StiDbCategoryType.Favorites].Items.Add(stiDatabaseItem7);
					stiDatabaseItem7.IsFavorite = true;
				}
				cacheCategories[StiDbCategoryType.File].Items.Add(stiDatabaseItem7);
				cacheCategories[StiDbCategoryType.All].Items.Add(stiDatabaseItem7);
			}
		}
		IEnumerable<StiDataAdapterService> mainList = StiOptions.Services.DataAdapters.Where((StiDataAdapterService x) => x.ServiceEnabled && x.IsObjectAdapter);
		mainList = StiDesignerSpecificationHelper.CheckShowDatabases(mainList);
		if (mainList.Count() > 0)
		{
			foreach (StiDataAdapterService item11 in mainList.OrderBy((StiDataAdapterService x) => x.ServiceName))
			{
				Image adapterIcon = GetAdapterIcon(item11);
				StiDatabaseItem item2 = new StiDatabaseItem(this, item11, item11.ServiceName, adapterIcon, StiDbButtonType.ToggleButton);
				cacheCategories[StiDbCategoryType.Objects].Items.Add(item2);
				cacheCategories[StiDbCategoryType.All].Items.Add(item2);
			}
		}
		StiRecentConnections.RecentConnections = new StiRecentConnections();
		List<StiRecentConnections.Item> recentConnections = StiRecentConnections.RecentConnections.GetRecentConnections();
		if (recentConnections != null && recentConnections.Count > 0)
		{
			List<StiRecentConnections.Item> list2 = new List<StiRecentConnections.Item>();
			foreach (StiRecentConnections.Item item12 in recentConnections)
			{
				StiRecentConnections.Connection connection = item12.Connection;
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(connection.ConnectionString))
				{
					stringBuilder.AppendFormat("{0} {1}", StiLocalization.Get("FormDatabaseEdit", "ConnectionString"), connection.ConnectionString);
				}
				if (!string.IsNullOrEmpty(connection.PathData))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append("\n");
					}
					stringBuilder.AppendFormat("{0} {1}", StiLocalization.Get("FormDatabaseEdit", "PathToData"), connection.PathData);
				}
				if (!string.IsNullOrEmpty(connection.PathSchema))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append("\n");
					}
					stringBuilder.AppendFormat("{0} {1}", StiLocalization.Get("FormDatabaseEdit", "PathSchema"), connection.PathSchema);
				}
				Image recentConnectionsIcon = GetRecentConnectionsIcon(connection);
				string[] recentConnectionText = GetRecentConnectionText(connection, item12.Name);
				StiDatabaseItem stiDatabaseItem8 = new StiDatabaseItem(this, connection, recentConnectionText[1], stringBuilder.ToString(), recentConnectionsIcon, StiDbButtonType.RecentFavoriteButton);
				stiDatabaseItem8.IsFavorite = connection.Pinned;
				cacheCategories[StiDbCategoryType.Recent].Items.Add(stiDatabaseItem8);
				cacheCategories[StiDbCategoryType.All].RecentItems.Add(stiDatabaseItem8);
			}
		}
		PrepareRecent();
		cacheCategories[StiDbCategoryType.All].PrepareItems();
		foreach (StiDbCategory category2 in categories)
		{
			if (category2.Items.Count > 0)
			{
				((ListBox)listBoxCategories).Items.Add((object)category2);
			}
		}
		((ListBox)listBoxCategories).SelectedIndexChanged += TreeViewCategories_SelectedIndexChanged;
		if (((ListBox)listBoxCategories).Items.Count > 0)
		{
			((ListControl)listBoxCategories).SelectedIndex = 0;
		}
		((Control)buttonFindComponent).TextChanged += ButtonFindComponent_TextChanged;
	}

	internal void PrepareRecent()
	{
		StiDbCategory stiDbCategory = cacheCategories[StiDbCategoryType.Recent];
		StiDbCategory stiDbCategory2 = cacheCategories[StiDbCategoryType.All];
		List<StiDatabaseItem> list = new List<StiDatabaseItem>();
		foreach (StiDatabaseItem item in stiDbCategory.Items)
		{
			if (item.Recent.Pinned)
			{
				list.Add(item);
			}
		}
		foreach (StiDatabaseItem item2 in stiDbCategory.Items)
		{
			if (!item2.Recent.Pinned)
			{
				list.Add(item2);
			}
		}
		stiDbCategory.Items.Clear();
		stiDbCategory.Items.AddRange(list);
		stiDbCategory2.RecentItems.Clear();
		stiDbCategory2.RecentItems.AddRange(list);
	}

	internal void RefreshSelectedViewData()
	{
		if (((ListBox)listBoxCategories).SelectedItem is StiDbCategory stiDbCategory)
		{
			stiDbCategory.ProcessViewData();
		}
	}

	private string[] GetRecentConnectionText(StiRecentConnections.Connection recent, string text)
	{
		StiDatabase stiDatabase = StiOptions.Services.Databases.FirstOrDefault((StiDatabase x) => x.GetType().Name == recent.DataAdapterType);
		return new string[2]
		{
			"[" + stiDatabase.ServiceName + "]",
			text.Substring(text.IndexOf(".") + 1)
		};
	}

	private void This_Load(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		Timer timer = new Timer
		{
			Interval = 300
		};
		timer.Tick += delegate
		{
			timer.Stop();
			((Control)buttonFindComponent).Focus();
		};
		timer.Start();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/index.html?data_data_dictionary_datasources.htm");
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			if (((Control)buttonFindComponent).Text.Length > 0)
			{
				((Control)buttonFindComponent).Text = "";
			}
			else
			{
				((Form)this).Close();
			}
		}
	}

	private void This_FormClosed(object sender, FormClosedEventArgs e)
	{
		StiRecentConnections.RecentConnections.Save();
		StiFormSettings.Save((Form)(object)this);
		StiSettings.Load();
		StiSettings.Set("StiDatabaseSelectWindow", "SkipSchemaWizard", ((CheckBox)checkBoxSkipSchemaWizard).Checked);
		StiSettings.Save();
	}

	private void PanelSeparator_Paint(object sender, PaintEventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Pen val = new Pen(StiUX.IsDark ? StiUX.Separator : Color.FromArgb(170, 170, 170));
		try
		{
			if (!StiUX.IsDark)
			{
				val.DashStyle = (DashStyle)2;
			}
			graphics.DrawLine(val, 1, 0, 1, ((Control)panelSeparator).Height);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ButtonFindComponent_TextChanged(object sender, EventArgs e)
	{
		if (((ListBox)listBoxCategories).SelectedItem is StiDbCategory stiDbCategory)
		{
			stiDbCategory.ProcessViewData();
		}
	}

	private void TreeViewCategories_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)buttonFindComponent).Text = "";
		if (((ListBox)listBoxCategories).SelectedItem is StiDbCategory stiDbCategory)
		{
			stiDbCategory.ProcessViewData();
		}
	}

	private void OnCollapsedChange(object sender, EventArgs e)
	{
		StiDatabaseSelectGroupBox stiDatabaseSelectGroupBox = (StiDatabaseSelectGroupBox)sender;
		StiSettings.Set("DatabaseSelectWindow", (string)((Control)stiDatabaseSelectGroupBox).Tag, !stiDatabaseSelectGroupBox.Collapsed);
	}

	private void Favorite_Click(StiDatabaseItem item, bool isAdd)
	{
		StiDbCategory stiDbCategory = cacheCategories[StiDbCategoryType.Favorites];
		StiDbCategory stiDbCategory2 = ((ListBox)listBoxCategories).SelectedItem as StiDbCategory;
		if (isAdd)
		{
			stiDbCategory.Items.Insert(0, item);
			cacheCategories[StiDbCategoryType.All].PrepareItems();
			if (stiDbCategory.Items.Count == 1)
			{
				((ListBox)listBoxCategories).Items.Insert(1, (object)stiDbCategory);
				((ListBox)listBoxCategories).SelectedItem = null;
				((ListBox)listBoxCategories).SelectedItem = stiDbCategory2;
			}
		}
		else
		{
			stiDbCategory.Items.Remove(item);
			cacheCategories[StiDbCategoryType.All].PrepareItems();
			if (stiDbCategory.Items.Count == 0)
			{
				((ListBox)listBoxCategories).Items.Remove((object)stiDbCategory);
				if (stiDbCategory2 == null || stiDbCategory2.CategoryType == StiDbCategoryType.Favorites)
				{
					((ListControl)listBoxCategories).SelectedIndex = 0;
					stiDbCategory2 = (StiDbCategory)((ListBox)listBoxCategories).SelectedItem;
				}
				else
				{
					((ListBox)listBoxCategories).SelectedItem = null;
					((ListBox)listBoxCategories).SelectedItem = stiDbCategory2;
				}
			}
		}
		if (stiDbCategory2.CategoryType == StiDbCategoryType.Favorites || stiDbCategory2.CategoryType == StiDbCategoryType.All)
		{
			stiDbCategory2.ProcessViewData();
		}
	}

	private void FavoriteRecent_Click()
	{
		PrepareRecent();
		if (((ListBox)listBoxCategories).SelectedItem is StiDbCategory stiDbCategory)
		{
			stiDbCategory.ProcessViewData();
		}
	}

	internal void ButtonItem_Click(StiDatabaseItem item)
	{
		if (!(((ListBox)listBoxCategories).SelectedItem is StiDbCategory))
		{
			return;
		}
		if (item.Database != null)
		{
			if (item.IsCreatedConnection)
			{
				CreatedDatabase = item.Database;
			}
			else
			{
				SelectedDatabase = item.Database;
				if (SelectedDatabase is StiSybaseAdsDatabase)
				{
					StiSybaseAdsWarningForm.ShowWarning((Form)(object)this);
				}
			}
			((Form)this).DialogResult = (DialogResult)1;
		}
		else if (item.Adapter != null)
		{
			AdapterService = item.Adapter;
			((Form)this).DialogResult = (DialogResult)1;
		}
		else if (item.Recent != null)
		{
			SelectedDatabase = StiRecentConnections.RecentConnections.GetDatabase(item.Recent);
			UseOriginalName = true;
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	internal void ButtonRemoveItem_Click(StiDatabaseItem item)
	{
		if (item?.Recent != null)
		{
			StiDbCategory stiDbCategory = cacheCategories[StiDbCategoryType.All];
			if (stiDbCategory.RecentItems.Contains(item))
			{
				stiDbCategory.RecentItems.Remove(item);
			}
			StiRecentConnections.RecentConnections.Remove(item.Recent);
			RefreshSelectedViewData();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Expected O, but got Unknown
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Expected O, but got Unknown
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ba: Expected O, but got Unknown
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_074e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0758: Expected O, but got Unknown
		//IL_08da: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e4: Expected O, but got Unknown
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f6: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDatabaseSelectForm));
		panelFooter = new StiButtonPanelUX();
		btCancel = new StiButtonUX();
		checkBoxSkipSchemaWizard = new StiCheckBoxUX();
		panelRoot = new StiPanelUX();
		listBoxCategories = new StiListBox();
		headerPanel = new StiPanelUX();
		buttonFindComponent = new StiButtonEdit();
		panelSeparator = new StiPanelUX();
		panelItems = new StiPanelUX();
		contextMenuRecent = new ContextMenuStrip(components);
		contextMenuItemDelete = new ToolStripMenuItem();
		contextMenuItemPin = new ToolStripMenuItem();
		contextMenuItemUnpin = new ToolStripMenuItem();
		panelList = new StiPanelUX();
		((Control)panelFooter).SuspendLayout();
		((Control)panelRoot).SuspendLayout();
		((Control)headerPanel).SuspendLayout();
		((Control)contextMenuRecent).SuspendLayout();
		((Control)panelList).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelFooter).BackColor = Color.White;
		((Control)panelFooter).Controls.Add((Control)(object)btCancel);
		((Control)panelFooter).Controls.Add((Control)(object)checkBoxSkipSchemaWizard);
		((Control)panelFooter).Dock = (DockStyle)2;
		panelFooter.LinePosition = StiLinePosition.Top;
		panelFooter.LineStyle = StiLineStyle.Dot;
		((Control)panelFooter).Location = new Point(0, 393);
		((Control)panelFooter).Name = "panelFooter";
		((Control)panelFooter).Size = new Size(770, 46);
		((Control)panelFooter).TabIndex = 30;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(658, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(100, 24);
		((Control)btCancel).TabIndex = 6;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)checkBoxSkipSchemaWizard).AutoSize = true;
		((Control)checkBoxSkipSchemaWizard).Location = new Point(12, 12);
		((Control)checkBoxSkipSchemaWizard).Name = "checkBoxSkipSchemaWizard";
		((Control)checkBoxSkipSchemaWizard).Size = new Size(125, 17);
		((Control)checkBoxSkipSchemaWizard).TabIndex = 31;
		((Control)checkBoxSkipSchemaWizard).Text = "Skip Schema Wizard";
		((ButtonBase)checkBoxSkipSchemaWizard).UseVisualStyleBackColor = true;
		((Control)panelRoot).BackColor = Color.White;
		((Control)panelRoot).Controls.Add((Control)(object)panelList);
		((Control)panelRoot).Controls.Add((Control)(object)headerPanel);
		((Control)panelRoot).Controls.Add((Control)(object)panelSeparator);
		((Control)panelRoot).Controls.Add((Control)(object)panelItems);
		((Control)panelRoot).Dock = (DockStyle)5;
		((Control)panelRoot).Location = new Point(0, 0);
		((Control)panelRoot).Name = "panelRoot";
		((Control)panelRoot).Size = new Size(770, 393);
		((Control)panelRoot).TabIndex = 20;
		((Control)listBoxCategories).Anchor = (AnchorStyles)15;
		((Control)listBoxCategories).BackColor = Color.White;
		((ListBox)listBoxCategories).BorderStyle = (BorderStyle)0;
		((Control)listBoxCategories).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)listBoxCategories).Location = new Point(9, 4);
		((Control)listBoxCategories).Margin = new Padding(2);
		((Control)listBoxCategories).Name = "listBoxCategories";
		((Control)listBoxCategories).Size = new Size(152, 343);
		((Control)listBoxCategories).TabIndex = 21;
		((Control)headerPanel).BackColor = Color.White;
		((Control)headerPanel).Controls.Add((Control)(object)buttonFindComponent);
		((Control)headerPanel).Dock = (DockStyle)1;
		((Control)headerPanel).Location = new Point(0, 0);
		((Control)headerPanel).Margin = new Padding(2);
		((Control)headerPanel).Name = "headerPanel";
		((Control)headerPanel).Size = new Size(167, 33);
		((Control)headerPanel).TabIndex = 10;
		((Control)buttonFindComponent).Anchor = (AnchorStyles)7;
		((Control)buttonFindComponent).BackColor = SystemColors.Window;
		buttonFindComponent.Flat = false;
		((Control)buttonFindComponent).ForeColor = SystemColors.ControlText;
		((Control)buttonFindComponent).Location = new Point(9, 6);
		((Control)buttonFindComponent).Margin = new Padding(2);
		((Control)buttonFindComponent).Name = "buttonFindComponent";
		buttonFindComponent.ShowButton = false;
		((Control)buttonFindComponent).Size = new Size(152, 21);
		((Control)buttonFindComponent).TabIndex = 11;
		((Control)panelSeparator).BackColor = Color.White;
		((Control)panelSeparator).Dock = (DockStyle)4;
		((Control)panelSeparator).Location = new Point(167, 0);
		((Control)panelSeparator).Margin = new Padding(0);
		((Control)panelSeparator).Name = "panelSeparator";
		((Control)panelSeparator).Size = new Size(10, 393);
		((Control)panelSeparator).TabIndex = 23;
		((Control)panelSeparator).Paint += new PaintEventHandler(PanelSeparator_Paint);
		((ScrollableControl)panelItems).AutoScroll = true;
		((Control)panelItems).BackColor = Color.White;
		((Control)panelItems).Dock = (DockStyle)4;
		((Control)panelItems).Location = new Point(177, 0);
		((Control)panelItems).Margin = new Padding(2);
		((Control)panelItems).Name = "panelItems";
		((Control)panelItems).Size = new Size(593, 393);
		((Control)panelItems).TabIndex = 22;
		((ToolStrip)contextMenuRecent).ImageScalingSize = new Size(32, 32);
		((ToolStrip)contextMenuRecent).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
		{
			(ToolStripItem)contextMenuItemDelete,
			(ToolStripItem)contextMenuItemPin,
			(ToolStripItem)contextMenuItemUnpin
		});
		((Control)contextMenuRecent).Name = "contextMenuStrip1";
		((Control)contextMenuRecent).Size = new Size(124, 118);
		((Control)contextMenuRecent).Text = "123";
		((ToolStripItem)contextMenuItemDelete).Image = (Image)componentResourceManager.GetObject("contextMenuItemDelete.Image");
		((ToolStripItem)contextMenuItemDelete).Name = "contextMenuItemDelete";
		((ToolStripItem)contextMenuItemDelete).Size = new Size(123, 38);
		((ToolStripItem)contextMenuItemDelete).Text = "Delete";
		((ToolStripItem)contextMenuItemPin).Image = (Image)componentResourceManager.GetObject("contextMenuItemPin.Image");
		((ToolStripItem)contextMenuItemPin).Name = "contextMenuItemPin";
		((ToolStripItem)contextMenuItemPin).Size = new Size(123, 38);
		((ToolStripItem)contextMenuItemPin).Text = "Pin";
		((ToolStripItem)contextMenuItemUnpin).Image = (Image)componentResourceManager.GetObject("contextMenuItemUnpin.Image");
		((ToolStripItem)contextMenuItemUnpin).Name = "contextMenuItemUnpin";
		((ToolStripItem)contextMenuItemUnpin).Size = new Size(123, 38);
		((ToolStripItem)contextMenuItemUnpin).Text = "Unpin";
		((Control)panelList).Controls.Add((Control)(object)listBoxCategories);
		((Control)panelList).Dock = (DockStyle)5;
		((Control)panelList).Location = new Point(0, 33);
		((Control)panelList).Name = "panelList";
		((Control)panelList).Size = new Size(167, 360);
		((Control)panelList).TabIndex = 24;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(770, 439);
		((Control)this).Controls.Add((Control)(object)panelRoot);
		((Control)this).Controls.Add((Control)(object)panelFooter);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(914, 690);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(739, 467);
		((Control)this).Name = "StiDatabaseSelectForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Select Type of Database ";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).FormClosed += new FormClosedEventHandler(This_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)panelFooter).ResumeLayout(false);
		((Control)panelFooter).PerformLayout();
		((Control)panelRoot).ResumeLayout(false);
		((Control)headerPanel).ResumeLayout(false);
		((Control)contextMenuRecent).ResumeLayout(false);
		((Control)panelList).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
