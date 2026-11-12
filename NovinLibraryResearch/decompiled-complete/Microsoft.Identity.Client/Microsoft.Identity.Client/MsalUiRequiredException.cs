using System;

namespace Microsoft.Identity.Client;

public class MsalUiRequiredException : MsalServiceException
{
	private readonly UiRequiredExceptionClassification _classification;

	public UiRequiredExceptionClassification Classification
	{
		get
		{
			if (string.Equals(base.SubError, "basic_action", StringComparison.OrdinalIgnoreCase))
			{
				return UiRequiredExceptionClassification.BasicAction;
			}
			if (string.Equals(base.SubError, "additional_action", StringComparison.OrdinalIgnoreCase))
			{
				return UiRequiredExceptionClassification.AdditionalAction;
			}
			if (string.Equals(base.SubError, "message_only", StringComparison.OrdinalIgnoreCase))
			{
				return UiRequiredExceptionClassification.MessageOnly;
			}
			if (string.Equals(base.SubError, "consent_required", StringComparison.OrdinalIgnoreCase))
			{
				return UiRequiredExceptionClassification.ConsentRequired;
			}
			if (string.Equals(base.SubError, "user_password_expired", StringComparison.OrdinalIgnoreCase))
			{
				return UiRequiredExceptionClassification.UserPasswordExpired;
			}
			return _classification;
		}
	}

	public MsalUiRequiredException(string errorCode, string errorMessage)
		: this(errorCode, errorMessage, null)
	{
	}

	public MsalUiRequiredException(string errorCode, string errorMessage, Exception innerException)
		: this(errorCode, errorMessage, innerException, UiRequiredExceptionClassification.None)
	{
	}

	public MsalUiRequiredException(string errorCode, string errorMessage, Exception innerException, UiRequiredExceptionClassification classification)
		: base(errorCode, errorMessage, innerException)
	{
		_classification = classification;
	}
}
