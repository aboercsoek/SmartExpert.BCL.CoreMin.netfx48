//--------------------------------------------------------------------------
// File:    QueryCredentialOptions.cs
// Content:	Definition of enumeration QueryCredentialOptions
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

using System;

namespace SmartExpert.Security.Authentication
{
	/// <summary>
	/// Specifies special behavior for the <see cref="QueryCredentialDialog"/>.
	/// </summary>
	[Flags]
	public enum QueryCredentialOptions
	{
		/// <summary>
		/// Notify the user of insufficient credentials by displaying the "Logon unsuccessful" balloon tip.
		/// </summary>
		IncorrectPassword = 0x1,
		/// <summary>
		/// Do not store credentials or display check boxes. 
		/// You can pass <see cref="ShowSaveCheckBox"/> with 
		/// this flag to display the Save check box only, and the 
		/// result is returned in the <see cref="QueryCredentialDialog.SavePassword"/> output parameter.
		/// </summary>
		DoNotPersist = 0x2,
		/// <summary>
		/// Populate the combo box with local administrators only.
		/// </summary>
		RequestAdministrator = 0x4,
		/// <summary>
		/// Populate the combo box with user name/password only.
		/// Do not display certificates or smart cards in the combo box.
		/// </summary>
		ExcludeCertificates = 0x8,
		/// <summary>
		/// Populate the combo box with certificates and smart cards only. 
		/// Do not allow a user name to be entered.
		/// </summary>
		RequireCertificate = 0x10,
		/// <summary>
		/// If the check box is selected, show the Save check box and return 
		/// true in the <see cref="QueryCredentialDialog.SavePassword"/> output parameter, otherwise return false.
		/// <see cref="DoNotPersist"/> must be specified to use this flag. 
		/// Check box uses the value in <see cref="QueryCredentialDialog.SavePassword"/> by default.
		/// </summary>
		ShowSaveCheckBox = 0x40,
		/// <summary>
		/// Specifies that a user interface will be shown even 
		/// if the credentials can be returned from an existing 
		/// credential in credential manager. This flag is permitted only 
		/// if <see cref="GenericCredentials"/> is also specified.
		/// </summary>
		AlwaysShowUI = 0x80,
		/// <summary>
		/// Populate the combo box with certificates or smart cards only. 
		/// Do not allow a user name to be entered.
		/// </summary>
		RequireSmartCard = 0x100,
		/// <summary>
		/// Unknown.
		/// </summary>
		PasswordOnlyOk = 0x200,
		/// <summary>
		/// Ensures that only a syntactically correct username can be entered by the user. That
		/// is either "username", "domain\username", "username@domain".
		/// </summary>
		ValidateUsername = 0x400,
		/// <summary>
		/// Ensures that only a complete username can be entered by the user, that ist either
		/// "domain\username" or "username@domain".
		/// </summary>
		CompleteUsername = 0x800,
		/// <summary>
		/// Do not show the Save check box, but the credential is saved as though 
		/// the box were shown and selected.
		/// </summary>
		Persist = 0x1000,
		/// <summary>
		/// This flag is meaningful only in locating a matching credential to prefill the 
		/// dialog box, should authentication fail. When this flag is specified, wildcard 
		/// credentials will not be matched. It has no effect when writing a credential. 
		/// CredUI does not create credentials that contain wildcard characters. Any found 
		/// were either created explicitly by the user or created programmatically, as 
		/// happens when a RAS connection is made.
		/// </summary>
		ServerCredential = 0x4000,
		/// <summary>
		/// Specifies that the caller will call CredUIConfirmCredentials 
		/// after checking to determine whether the returned credentials 
		/// are actually valid. This mechanism ensures that invalid credentials 
		/// are not saved to the credential manager. Specify this flag in all 
		/// cases unless <see cref="DoNotPersist"/> is specified.
		/// </summary>
		ExpectConfirmation = 0x20000,
		/// <summary>
		/// Consider the credentials entered by the user to be generic credentials.
		/// </summary>
		GenericCredentials = 0x40000,
		/// <summary>
		/// The credential is a "runas" credential. The <see cref="QueryCredentialDialog.TargetName" /> 
		/// parameter specifies the name of the command or program being run. 
		/// It is used for prompting purposes only.
		/// </summary>
		UsernameTargetCredential = 0x80000,
		/// <summary>
		/// Specified, that the dialog is prepopulated with the <see cref="QueryCredentialDialog.Username"/>
		/// field and cannot be changed by the user.
		/// </summary>
		KeepUsername = 0x100000,
	}
}
