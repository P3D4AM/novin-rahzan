using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Protocols;

public abstract class AuthenticationProtocolMessage
{
	private string _postTitle = "Working...";

	private string _script = "<script language=\"javascript\">window.setTimeout('document.forms[0].submit()', 0);</script>";

	private string _scriptButtonText = "Submit";

	private string _scriptDisabledText = "Script is disabled. Click Submit to continue.";

	private Dictionary<string, string> _parameters = new Dictionary<string, string>();

	private string _issuerAddress = string.Empty;

	public string IssuerAddress
	{
		get
		{
			return _issuerAddress;
		}
		set
		{
			if (value == null)
			{
				throw LogHelper.LogArgumentNullException("IssuerAddress");
			}
			_issuerAddress = value;
		}
	}

	public IDictionary<string, string> Parameters => _parameters;

	public string PostTitle
	{
		get
		{
			return _postTitle;
		}
		set
		{
			if (value == null)
			{
				throw LogHelper.LogArgumentNullException("PostTitle");
			}
			_postTitle = value;
		}
	}

	public string Script
	{
		get
		{
			return _script;
		}
		set
		{
			if (value == null)
			{
				throw LogHelper.LogArgumentNullException("Script");
			}
			_script = value;
		}
	}

	public string ScriptButtonText
	{
		get
		{
			return _scriptButtonText;
		}
		set
		{
			if (value == null)
			{
				throw LogHelper.LogArgumentNullException("ScriptButtonText");
			}
			_scriptButtonText = value;
		}
	}

	public string ScriptDisabledText
	{
		get
		{
			return _scriptDisabledText;
		}
		set
		{
			if (value == null)
			{
				throw LogHelper.LogArgumentNullException("ScriptDisabledText");
			}
			_scriptDisabledText = value;
		}
	}

	public virtual string BuildFormPost()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<html><head><title>");
		stringBuilder.Append(WebUtility.HtmlEncode(PostTitle));
		stringBuilder.Append("</title></head><body><form method=\"POST\" name=\"hiddenform\" action=\"");
		stringBuilder.Append(WebUtility.HtmlEncode(IssuerAddress));
		stringBuilder.Append("\">");
		foreach (KeyValuePair<string, string> parameter in _parameters)
		{
			stringBuilder.Append("<input type=\"hidden\" name=\"");
			stringBuilder.Append(WebUtility.HtmlEncode(parameter.Key));
			stringBuilder.Append("\" value=\"");
			stringBuilder.Append(WebUtility.HtmlEncode(parameter.Value));
			stringBuilder.Append("\" />");
		}
		stringBuilder.Append("<noscript><p>");
		stringBuilder.Append(WebUtility.HtmlEncode(ScriptDisabledText));
		stringBuilder.Append("</p><input type=\"submit\" value=\"");
		stringBuilder.Append(WebUtility.HtmlEncode(ScriptButtonText));
		stringBuilder.Append("\" /></noscript>");
		stringBuilder.Append("</form>");
		stringBuilder.Append(Script);
		stringBuilder.Append("</body></html>");
		return stringBuilder.ToString();
	}

	public virtual string BuildRedirectUrl()
	{
		StringBuilder stringBuilder = new StringBuilder(_issuerAddress);
		bool flag = _issuerAddress.Contains("?");
		foreach (KeyValuePair<string, string> parameter in _parameters)
		{
			if (parameter.Value != null)
			{
				if (!flag)
				{
					stringBuilder.Append('?');
					flag = true;
				}
				else
				{
					stringBuilder.Append('&');
				}
				stringBuilder.Append(Uri.EscapeDataString(parameter.Key));
				stringBuilder.Append('=');
				stringBuilder.Append(Uri.EscapeDataString(parameter.Value));
			}
		}
		return stringBuilder.ToString();
	}

	public virtual string GetParameter(string parameter)
	{
		if (string.IsNullOrEmpty(parameter))
		{
			throw LogHelper.LogArgumentNullException("parameter");
		}
		string value = null;
		_parameters.TryGetValue(parameter, out value);
		return value;
	}

	public virtual void RemoveParameter(string parameter)
	{
		if (string.IsNullOrEmpty(parameter))
		{
			throw LogHelper.LogArgumentNullException("parameter");
		}
		if (_parameters.ContainsKey(parameter))
		{
			_parameters.Remove(parameter);
		}
	}

	public void SetParameter(string parameter, string value)
	{
		if (string.IsNullOrEmpty(parameter))
		{
			throw LogHelper.LogArgumentNullException("parameter");
		}
		if (value == null)
		{
			RemoveParameter(parameter);
		}
		else
		{
			_parameters[parameter] = value;
		}
	}

	public virtual void SetParameters(NameValueCollection nameValueCollection)
	{
		if (nameValueCollection != null)
		{
			string[] allKeys = nameValueCollection.AllKeys;
			foreach (string text in allKeys)
			{
				SetParameter(text, nameValueCollection[text]);
			}
		}
	}
}
