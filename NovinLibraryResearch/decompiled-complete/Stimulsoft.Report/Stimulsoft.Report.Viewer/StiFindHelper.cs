using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Viewer;

public class StiFindHelper
{
	private string prevValue = "";

	private bool prevMatchWholeWord = true;

	private bool prevMatchCase = true;

	public List<StiComponent> FindedComponents { get; private set; } = new List<StiComponent>();


	public Hashtable HashOfFindedPages { get; private set; } = new Hashtable();


	public int CurrentComponent { get; private set; }

	public bool FindActivated { get; private set; }

	public bool AllowLoop { get; set; } = true;


	public bool AlwaysFindAgain { get; set; }

	public bool IsBof { get; set; }

	public bool IsEof { get; set; }

	public event StiProgressChangedEventHandler ProgressChanged;

	public void InvokeProgressChanged(int value, int maximum)
	{
		this.ProgressChanged?.Invoke(this, new StiProgressChangedEventArgs(value, maximum));
	}

	public void ResetFind()
	{
		prevValue = null;
	}

	public void CloseFind(StiReport report)
	{
		prevValue = null;
		if (report != null)
		{
			ClearFind(report.RenderedPages);
		}
	}

	public void ClearFind(StiReport report)
	{
		if (report != null)
		{
			ClearFind(report.RenderedPages);
		}
	}

	public void ClearFind(StiPagesCollection pages)
	{
		if (pages != null && pages.Count > 0)
		{
			foreach (StiPage page in pages)
			{
				StiComponentsCollection components = page.GetComponents();
				foreach (StiComponent item in components)
				{
					item.HighlightState = StiHighlightState.Hide;
				}
			}
		}
		if (FindedComponents != null)
		{
			FindedComponents.Clear();
			FindedComponents = null;
		}
		if (HashOfFindedPages != null)
		{
			HashOfFindedPages.Clear();
			HashOfFindedPages = null;
		}
		FindActivated = false;
	}

	public void StartFind(string value, StiReport report, bool matchCase, bool matchWholeWord)
	{
		if (report != null)
		{
			StartFind(value, report.RenderedPages, matchCase, matchWholeWord);
		}
	}

	public void StartFind(string value, StiPagesCollection pages, bool matchCase, bool matchWholeWord)
	{
		IsBof = false;
		IsEof = false;
		if (pages == null || pages.Count <= 0)
		{
			return;
		}
		if (value != prevValue || matchCase != prevMatchCase || matchWholeWord != prevMatchWholeWord || AlwaysFindAgain)
		{
			RichTextBox val = null;
			Regex regex = new Regex("\\s+");
			prevValue = value;
			prevMatchWholeWord = matchWholeWord;
			prevMatchCase = matchCase;
			if (!matchCase)
			{
				value = value.ToUpper(CultureInfo.InvariantCulture);
			}
			value = regex.Replace(value, " ");
			ClearFind(pages);
			InvokeProgressChanged(0, pages.Count);
			int num = 0;
			foreach (StiPage page in pages)
			{
				bool flag = false;
				InvokeProgressChanged(num++, pages.Count);
				StiComponentsCollection components = page.GetComponents();
				foreach (StiComponent item in components)
				{
					if (!(item is IStiText { Text: not null } stiText) || stiText.GetTextInternal() == null)
					{
						continue;
					}
					string text = stiText.GetTextInternal();
					if (item is StiRichText)
					{
						if (val == null)
						{
							val = (RichTextBox)(object)new StiRichTextBox(drawTransparent: false);
						}
						val.Rtf = ((StiRichText)item).RtfText;
						text = ((Control)val).Text;
					}
					if (item is StiText { AllowHtmlTags: not false })
					{
						text = StiTextHelper.HtmlToPlainText(stiText.Text);
					}
					if (!matchCase)
					{
						text = text.ToUpper(CultureInfo.InvariantCulture);
					}
					text = regex.Replace(text, " ");
					if (matchWholeWord)
					{
						int num2 = text.IndexOf(value, StringComparison.InvariantCulture);
						if (num2 == -1 || (num2 > 0 && char.IsLetterOrDigit(text, num2 - 1)))
						{
							continue;
						}
						int num3 = num2 + value.Length;
						if (num3 < text.Length && char.IsLetterOrDigit(text, num3))
						{
							continue;
						}
						if (FindedComponents == null)
						{
							FindedComponents = new List<StiComponent>();
						}
						FindedComponents.Add(item);
						if (!flag)
						{
							if (HashOfFindedPages == null)
							{
								HashOfFindedPages = new Hashtable();
							}
							HashOfFindedPages[page] = page;
							flag = true;
						}
					}
					else
					{
						if (text.IndexOf(value, StringComparison.InvariantCulture) < 0)
						{
							continue;
						}
						if (FindedComponents == null)
						{
							FindedComponents = new List<StiComponent>();
						}
						FindedComponents.Add(item);
						if (!flag)
						{
							if (HashOfFindedPages == null)
							{
								HashOfFindedPages = new Hashtable();
							}
							HashOfFindedPages[page] = page;
							flag = true;
						}
					}
				}
			}
			if (FindedComponents != null)
			{
				foreach (StiComponent findedComponent in FindedComponents)
				{
					findedComponent.HighlightState = StiHighlightState.Show;
				}
			}
			if (FindedComponents != null && FindedComponents.Count > 0)
			{
				FindedComponents[0].HighlightState = StiHighlightState.Active;
			}
			else
			{
				IsEof = true;
			}
			CurrentComponent = 0;
			if (val != null)
			{
				((Component)(object)val).Dispose();
				val = null;
			}
		}
		else if (FindedComponents != null && FindedComponents.Count > 0)
		{
			FindedComponents[CurrentComponent].HighlightState = StiHighlightState.Show;
			CurrentComponent++;
			if (CurrentComponent >= FindedComponents.Count)
			{
				if (AllowLoop)
				{
					CurrentComponent = 0;
				}
				else
				{
					CurrentComponent--;
					IsEof = true;
				}
			}
			FindedComponents[CurrentComponent].HighlightState = StiHighlightState.Active;
		}
		else
		{
			IsEof = true;
		}
		FindActivated = true;
	}

	public void StartFindPrevious(string value, StiReport report, bool matchCase, bool matchWholeWord)
	{
		if (report != null)
		{
			StartFindPrevious(value, report.RenderedPages, matchCase, matchWholeWord);
		}
	}

	public void StartFindPrevious(string value, StiPagesCollection pages, bool matchCase, bool matchWholeWord)
	{
		IsBof = false;
		IsEof = false;
		if (pages == null || pages.Count <= 0)
		{
			return;
		}
		if (value != prevValue || matchCase != prevMatchCase || matchWholeWord != prevMatchWholeWord || AlwaysFindAgain)
		{
			prevValue = value;
			prevMatchWholeWord = matchWholeWord;
			prevMatchCase = matchCase;
			if (!matchCase)
			{
				value = value.ToUpper(CultureInfo.InvariantCulture);
			}
			ClearFind(pages);
			InvokeProgressChanged(0, pages.Count);
			int num = 0;
			foreach (StiPage page in pages)
			{
				bool flag = false;
				InvokeProgressChanged(num++, pages.Count);
				StiComponentsCollection components = page.GetComponents();
				foreach (StiComponent item in components)
				{
					if (!(item is IStiText { Text: not null } stiText) || stiText.GetTextInternal() == null)
					{
						continue;
					}
					string text = stiText.GetTextInternal();
					if (!matchCase)
					{
						text = text.ToUpper(CultureInfo.InvariantCulture);
					}
					if (matchWholeWord)
					{
						int num2 = text.IndexOf(value, StringComparison.InvariantCulture);
						if (num2 == -1 || (num2 > 0 && char.IsLetterOrDigit(text, num2 - 1)))
						{
							continue;
						}
						int num3 = num2 + value.Length;
						if (num3 < text.Length && char.IsLetterOrDigit(text, num3))
						{
							continue;
						}
						if (FindedComponents == null)
						{
							FindedComponents = new List<StiComponent>();
						}
						FindedComponents.Add(item);
						if (!flag)
						{
							if (HashOfFindedPages == null)
							{
								HashOfFindedPages = new Hashtable();
							}
							HashOfFindedPages[page] = page;
							flag = true;
						}
					}
					else
					{
						if (text.IndexOf(value, StringComparison.InvariantCulture) < 0)
						{
							continue;
						}
						if (FindedComponents == null)
						{
							FindedComponents = new List<StiComponent>();
						}
						FindedComponents.Add(item);
						if (!flag)
						{
							if (HashOfFindedPages == null)
							{
								HashOfFindedPages = new Hashtable();
							}
							HashOfFindedPages[page] = page;
							flag = true;
						}
					}
				}
			}
			if (FindedComponents != null)
			{
				foreach (StiComponent findedComponent in FindedComponents)
				{
					findedComponent.HighlightState = StiHighlightState.Show;
				}
			}
			if (FindedComponents != null && FindedComponents.Count > 0)
			{
				FindedComponents[0].HighlightState = StiHighlightState.Active;
			}
			else
			{
				IsBof = true;
			}
			CurrentComponent = 0;
		}
		else if (FindedComponents != null && FindedComponents.Count > 0)
		{
			FindedComponents[CurrentComponent].HighlightState = StiHighlightState.Show;
			CurrentComponent--;
			if (CurrentComponent == -1)
			{
				if (AllowLoop)
				{
					CurrentComponent = FindedComponents.Count - 1;
				}
				else
				{
					CurrentComponent = 0;
					IsBof = true;
				}
			}
			FindedComponents[CurrentComponent].HighlightState = StiHighlightState.Active;
		}
		else
		{
			IsBof = true;
		}
		FindActivated = true;
	}
}
