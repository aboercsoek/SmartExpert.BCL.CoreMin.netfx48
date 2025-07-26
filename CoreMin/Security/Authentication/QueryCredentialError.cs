//--------------------------------------------------------------------------
// File:    QueryCredentialError.cs
// Content:	Definition of enumeration QueryCredentialError
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

namespace SmartExpert.Security.Authentication
{
	/// <summary>
	/// Query Credential Error enumeration
	/// </summary>
	public enum QueryCredentialError
	{
		/// <summary>
		/// 
		/// </summary>
		None = 0,
		/// <summary>
		/// User chose Cancel.
		/// </summary>
		Cancelled = 1223,
		/// <summary>
		/// The credential manager cannot be used. Typically, this error is handled 
		/// by calling CredUIPromptForCredentials and passing in the 
		/// <see cref="QueryCredentialOptions.DoNotPersist"/> flag.
		/// </summary>
		NoSuchLogonSession = 1312,
		/// <summary>
		/// 
		/// </summary>
		NotFound = 1168,
		/// <summary>
		/// 
		/// </summary>
		InvalidAcountName = 1315,
		/// <summary>
		/// 
		/// </summary>
		InsufficientBuffer = 122,
		/// <summary>
		/// <para>Either pszTargetName is NULL, the empty string, or longer than 
		/// CREDUI_MAX_DOMAIN_LENGTH, or pUiInfo is not NULL and the CredUI_INFO 
		/// structure pointed to did not meet one of the following requirements:</para>
		/// <list type="bullet">
		///     <item>The cbSize member must be one.</item>
		///     <item>If the hbmBanner member is not NULL, it must be of type OBJ_BITMAP.</item>
		///     <item>If the pszMessageText member is not NULL, it must not be greater than 
		///             CREDUI_MAX_MESSAGE_LENGTH. </item>
		///     <item>If the pszCaptionText member is not NULL, it must not be greater than 
		///             CREDUI_MAX_CAPTION_LENGTH. </item>
		/// </list>
		/// </summary>
		InvalidParameter = 87,
		/// <summary>
		/// This status is returned for any of the invalid flag configurations already mentioned.
		/// </summary>
		InvalidFlags = 1004,
	}
}
