using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class SearchDialog : ISearchDialog
{
	private static DlgSearch searchDlg = null;

	private static SearchSettings searchSettings = new SearchSettings();

	private static Form ownerForm;

	private static bool regularExpressionsVisible = true;

	private static bool regularExpressionsEnabled = true;

	public bool Visible
	{
		get
		{
			if (searchDlg != null)
			{
				return ((Control)searchDlg).Visible;
			}
			return false;
		}
		set
		{
			if (searchDlg != null)
			{
				((Control)searchDlg).Visible = value;
			}
		}
	}

	public Form OwnerForm
	{
		get
		{
			if (searchDlg == null)
			{
				return ownerForm;
			}
			return ((Form)searchDlg).Owner;
		}
		set
		{
			ownerForm = value;
			if (searchDlg != null)
			{
				ownerForm = value;
			}
		}
	}

	public SearchSettings SearchSettings => searchSettings;

	private void CopyList(IList FromList, IList ToList)
	{
		ToList.Clear();
		foreach (object From in FromList)
		{
			ToList.Add(From);
		}
	}

	private void CheckOption(SearchOptions value)
	{
		if ((searchSettings.SearchOptions & value) == 0)
		{
			searchSettings.SearchOptions |= value;
		}
		else
		{
			searchSettings.SearchOptions &= ~value;
		}
	}

	private void DoClose(object Sender, EventArgs e)
	{
		if (searchDlg != null)
		{
			searchSettings.SearchOptions = searchDlg.Options;
			searchSettings.ClearBookmarks = searchDlg.ClearBookmarks;
			CopyList((IList)searchDlg.cbFindWhat.Items, searchSettings.SearchList);
			CopyList((IList)searchDlg.cbReplaceWith.Items, searchSettings.ReplaceList);
		}
		searchDlg = null;
	}

	public DialogResult Execute(ISearch search, bool isModal, bool isReplace)
	{
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		string selectedText = string.Empty;
		bool flag = search.CanSearchSelection(out selectedText) && selectedText.Trim() != string.Empty;
		bool flag2 = flag && StrItem.Split(selectedText).Length <= 1;
		if (searchDlg == null)
		{
			searchDlg = new DlgSearch();
			SearchOptions searchOptions = searchSettings.SearchOptions;
			searchDlg.ClearBookmarks = searchSettings.ClearBookmarks;
			searchOptions = ((!flag || (flag2 && (searchSettings.SearchOptions & SearchOptions.FindSelectedText) != 0)) ? (searchOptions & ~SearchOptions.SelectionOnly) : (searchOptions | SearchOptions.SelectionOnly));
			searchDlg.Options = searchOptions;
			ShowRegularExpressions(regularExpressionsVisible);
			EnableRegularExpressions(regularExpressionsEnabled);
			CopyList(searchSettings.SearchList, (IList)searchDlg.cbFindWhat.Items);
			CopyList(searchSettings.ReplaceList, (IList)searchDlg.cbReplaceWith.Items);
			((Form)searchDlg).Closed += DoClose;
		}
		((Control)searchDlg.rbSelectionOnly).Enabled = flag;
		if (flag)
		{
			selectedText = selectedText.Trim();
			if (flag2 && (searchSettings.SearchOptions & SearchOptions.FindSelectedText) != 0)
			{
				if (selectedText != string.Empty)
				{
					((Control)searchDlg.cbFindWhat).Text = selectedText;
				}
			}
			else
			{
				selectedText = string.Empty;
			}
		}
		if ((searchSettings.SearchOptions & SearchOptions.FindTextAtCursor) != 0 && selectedText == string.Empty)
		{
			selectedText = search.GetTextToSearchAtCursor().Trim();
			if (selectedText != string.Empty)
			{
				((Control)searchDlg.cbFindWhat).Text = selectedText;
			}
		}
		searchDlg.Search = search;
		searchDlg.Init();
		searchDlg.IsReplace = isReplace;
		if (isModal)
		{
			Form val = (Form)(object)searchDlg;
			DialogResult result = ((Form)searchDlg).ShowDialog();
			((Component)(object)val).Dispose();
			return result;
		}
		if (ownerForm == null)
		{
			((Form)searchDlg).TopMost = true;
			((Form)searchDlg).Owner = null;
		}
		else
		{
			((Form)searchDlg).TopMost = false;
			((Form)searchDlg).Owner = ownerForm;
		}
		((Control)searchDlg).Show();
		return (DialogResult)0;
	}

	public void DoneSearch(ISearch search)
	{
		if (searchDlg != null && searchDlg.Search == search)
		{
			searchDlg.Search = null;
		}
	}

	public void Close()
	{
		if (searchDlg != null)
		{
			((Form)searchDlg).Close();
		}
	}

	public void EnsureVisible(Rectangle rect)
	{
		if (searchDlg == null)
		{
			return;
		}
		Rectangle rectangle = rect;
		rectangle.Intersect(((Control)searchDlg).Bounds);
		if (!rectangle.IsEmpty)
		{
			if (rect.Bottom + ((Control)searchDlg).Height < SystemInformation.WorkingArea.Height)
			{
				((Control)searchDlg).Top = rect.Bottom;
			}
			else
			{
				((Control)searchDlg).Top = Math.Max(rect.Top - ((Control)searchDlg).Height, 0);
			}
			if (rect.Right + ((Control)searchDlg).Width < SystemInformation.WorkingArea.Width)
			{
				((Control)searchDlg).Left = rect.Right;
			}
			else
			{
				((Control)searchDlg).Left = Math.Max(rect.Left - ((Control)searchDlg).Width, 0);
			}
		}
	}

	public void ToggleHiddenText()
	{
		CheckOption(SearchOptions.SearchHiddenText);
		if (searchDlg != null)
		{
			searchDlg.chbSearchHiddenText.Checked = !searchDlg.chbSearchHiddenText.Checked;
		}
	}

	public void ToggleMatchCase()
	{
		CheckOption(SearchOptions.CaseSensitive);
		if (searchDlg != null)
		{
			searchDlg.chbMatchCase.Checked = !searchDlg.chbMatchCase.Checked;
		}
	}

	public void ToggleRegularExpressions()
	{
		CheckOption(SearchOptions.RegularExpressions);
		if (searchDlg != null)
		{
			searchDlg.chbUseRegularExpressions.Checked = !searchDlg.chbUseRegularExpressions.Checked;
		}
	}

	public void EnableRegularExpressions(bool enable)
	{
		regularExpressionsEnabled = enable;
		if (searchDlg != null)
		{
			((Control)searchDlg.chbUseRegularExpressions).Enabled = enable;
		}
	}

	public void ShowRegularExpressions(bool show)
	{
		regularExpressionsVisible = show;
		if (searchDlg != null)
		{
			((Control)searchDlg.chbUseRegularExpressions).Visible = show;
		}
	}

	public void ToggleSearchUp()
	{
		CheckOption(SearchOptions.BackwardSearch);
		if (searchDlg != null)
		{
			searchDlg.chbSearchUp.Checked = !searchDlg.chbSearchUp.Checked;
		}
	}

	public void TogglePromptOnReplace()
	{
		CheckOption(SearchOptions.PromptOnReplace);
		if (searchDlg != null)
		{
			searchDlg.chbPromptOnReplace.Checked = !searchDlg.chbPromptOnReplace.Checked;
		}
	}

	public void ToggleWholeWord()
	{
		CheckOption(SearchOptions.WholeWordsOnly);
		if (searchDlg != null)
		{
			searchDlg.chbMatchWholeWord.Checked = !searchDlg.chbMatchWholeWord.Checked;
		}
	}
}
