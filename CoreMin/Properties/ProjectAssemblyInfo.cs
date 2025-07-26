//===============================================================================
// SmartExpert Base Class Library
// BCL.CoreMin
//===============================================================================
// Copyright © 2008-2013 Andreas Börcsök.  All rights reserved.
//===============================================================================
#region Using directives

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using SmartExpert;
using SmartExpert.Linq;

#endregion

// Set the Assembly Title, Alias and Description
#if NET4_0
    [assembly: AssemblyTitle("SmartExpert.BCL.CoreMin for .NET Framework 4")]
#elif NET4_8
    [assembly: AssemblyTitle("SmartExpert.BCL.CoreMin for .NET Framework 4.8")]
#else
#error Unrecognized build target - please update ProjectAssemblyInfo.cs
#endif

//[assembly: AssemblyTitle("SmartExpert.BCL.CoreMin.dll")]
[assembly: AssemblyDefaultAlias("SmartExpert.BCL.CoreMin.dll")]
[assembly: AssemblyDescription("SmartExpert Base Class Library CoreMin")]


// Assembly Friend Definition
[assembly: InternalsVisibleTo("SmartExpert.BCL.Core, PublicKey=002400000480000094000000060200000024000052534131000400000100010077b108eb9862ea6bc8ee23e068642888ce567d2b9013f0a6210e6da82f98323f0a37f45cfe815c2b509f4a1d207f1662b48df0f50b43f03bad75d3a4de7fde63d77ae13a631e20322043c9b3c5790f6aebfa986d597b3121c1d083a63caad4c0bde4958a6ac1a935cd5264edf51c6bdcf65fea2c02b68c4412375d638f09fddd")]

[assembly: InternalsVisibleTo("SmartExpert.BCL.CoreLt, PublicKey=002400000480000094000000060200000024000052534131000400000100010077b108eb9862ea6bc8ee23e068642888ce567d2b9013f0a6210e6da82f98323f0a37f45cfe815c2b509f4a1d207f1662b48df0f50b43f03bad75d3a4de7fde63d77ae13a631e20322043c9b3c5790f6aebfa986d597b3121c1d083a63caad4c0bde4958a6ac1a935cd5264edf51c6bdcf65fea2c02b68c4412375d638f09fddd")]

[assembly: InternalsVisibleTo("SmartExpert.BCL.CoreMin.Test, PublicKey=002400000480000094000000060200000024000052534131000400000100010077b108eb9862ea6bc8ee23e068642888ce567d2b9013f0a6210e6da82f98323f0a37f45cfe815c2b509f4a1d207f1662b48df0f50b43f03bad75d3a4de7fde63d77ae13a631e20322043c9b3c5790f6aebfa986d597b3121c1d083a63caad4c0bde4958a6ac1a935cd5264edf51c6bdcf65fea2c02b68c4412375d638f09fddd")]

