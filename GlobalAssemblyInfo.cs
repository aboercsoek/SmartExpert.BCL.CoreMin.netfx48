//===============================================================================
// SmartExpert Base Class Libraries (BCL)
//===============================================================================
// Copyright © 2008-2013 Andreas Börcsök.  All rights reserved.
//===============================================================================
#region Using directives

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

#endregion

[assembly:ComVisible(false)]

// Set the version, company name, copyrigt & trademarks fields (solution wide scope)
[assembly: AssemblyCompany("Andreas Börcsök")]
// © = \x00a9
[assembly: AssemblyCopyright("Copyright © Andreas Börcsök 2008-2013")]
[assembly:AssemblyTrademark("")]

#if !BuildScript

// Set the AssemblyVersion field
[assembly: AssemblyVersion("2.0.0.0")]

// Set the Assembly Version field for the Win32 file resource field
[assembly: AssemblyFileVersion("2.0.13.0630")] 

// Set the Assembly Manifest Version field
//[assembly: AssemblyInformationalVersion("2.0.13.0")]

// Set the ProductName & ProductVersion fields
[assembly: AssemblyProduct("SmartExpert.BCL.CoreMin v2.0.13.0630")]

#endif

// Set the Language field
[assembly: AssemblyCulture("")]
// Neutral culture definition
[assembly: NeutralResourcesLanguage("en-US")]


// Indicates that literal strings are not interned
[assembly: CompilationRelaxations(CompilationRelaxations.NoStringInterning)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]


#if NET4_0
// Allow this strong name assembly to be called by partial trust code.
[assembly: AllowPartiallyTrustedCallers(PartialTrustVisibilityLevel = PartialTrustVisibilityLevel.NotVisibleByDefault)]
[assembly: SecurityRules(SecurityRuleSet.Level1, SkipVerificationInFullTrust = true)]
#elif NET4_5
// Allow this strong name assembly to be called by partial trust code.
[assembly: AllowPartiallyTrustedCallers(PartialTrustVisibilityLevel = PartialTrustVisibilityLevel.NotVisibleByDefault)]
[assembly: SecurityRules(SecurityRuleSet.Level1, SkipVerificationInFullTrust = true)]
#elif NET3_5
// Allow this strong name assembly to be called by partial trust code.
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution=true)]
#endif

// Set the Common Language Specification field
[assembly: CLSCompliant(false)]

#if DEBUG
[assembly:AssemblyConfiguration("Debug")]
#else
[assembly:AssemblyConfiguration("Release")]
#endif

