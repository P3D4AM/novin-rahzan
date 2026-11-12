using System;
using System.Security.Claims;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens;

public abstract class SecurityTokenHandler : TokenHandler, ISecurityTokenValidator
{
	public virtual bool CanValidateToken => false;

	public virtual bool CanWriteToken => false;

	public abstract Type TokenType { get; }

	public virtual SecurityKeyIdentifierClause CreateSecurityTokenReference(SecurityToken token, bool attached)
	{
		throw new NotImplementedException();
	}

	public virtual SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
	{
		throw new NotImplementedException();
	}

	public virtual bool CanReadToken(XmlReader reader)
	{
		return false;
	}

	public virtual bool CanReadToken(string tokenString)
	{
		return false;
	}

	public virtual SecurityToken ReadToken(string tokenString)
	{
		return null;
	}

	public virtual SecurityToken ReadToken(XmlReader reader)
	{
		return null;
	}

	public virtual string WriteToken(SecurityToken token)
	{
		return null;
	}

	public abstract void WriteToken(XmlWriter writer, SecurityToken token);

	public abstract SecurityToken ReadToken(XmlReader reader, TokenValidationParameters validationParameters);

	public virtual ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
	{
		throw new NotImplementedException();
	}

	public virtual ClaimsPrincipal ValidateToken(XmlReader reader, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
	{
		throw new NotImplementedException();
	}
}
