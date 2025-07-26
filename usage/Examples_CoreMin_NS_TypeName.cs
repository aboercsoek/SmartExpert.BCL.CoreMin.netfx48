//--------------------------------------------------------------------------
// File:    Examples_CoreMin_NS_TypeName.cs
// Content:	Implementation of class ExamplesCoreMinNamespaceTypeName
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2013 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SmartExpert;
using SmartExpert.CUI;
using SmartExpert.Error;
using SmartExpert.IO;
using SmartExpert.Linq;

#endregion

// ::: This file should be used as a template for examples used as source in code doc example sections :::

// <example>
// <code lang="cs" title="TypeName.MemberA Example." outlining="true" source=".\examples\Examples_CoreMin_NS_TypeName.cs" region="Example_MT_MemberA" />
// <code title="Example Output:" source=".\examples\Sample_CoreMin_Namespace_MT_TypeName_MemberA.txt" />
// </example>

namespace SmartExpert.Examples
{
	///<summary>TypeName examples</summary>
	public static class ExamplesCoreMinNamespaceTypeName
	{
		[Description("TypeName examples")]
		public static void RunAll()
		{
			FileHelper.GetFiles(@".\", "Sample_CoreMin_Namespace_M_TypeName*.txt").ForEach(FileHelper.DeleteFile);
			FileHelper.GetFiles(@".\", "Sample_CoreMin_Namespace_EXM_TypeName*.txt").ForEach(FileHelper.DeleteFile);
			FileHelper.GetFiles(@".\", "Sample_CoreMin_Namespace_P_TypeName*.txt").ForEach(FileHelper.DeleteFile);
			
			ExampleMemberA();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExampleMemberB();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExampleMemberC();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExampleMemberD();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExampleMemberE();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExampleMemberF();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExampleMemberG();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExampleMemberH();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExampleMemberI();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			//ExampleMemberJ();
			//ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			
					// ::: Comment out not used member examples :::
		}
		
		[Description("TypeName.MemberA example")]
		public static void ExampleMemberA()
		{
			try
			{
				// LibName = Assembly Short Name (CoreMin, CoreLt)
				// Namespace = Namespace last part
				// TypeName = The name of the type
				// Member[A..Z] = Members of type TypeName
				// MemberType(MT): M=Method; EXM=Extension Method; P=Property; T=Type
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Namespace_MT_TypeName_MemberA.txt";

				#region Example_MT_MemberA

				// Example code

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}
		
		[Description("TypeName.MemberB example")]
		public static void ExampleMemberB()
		{
			try
			{
				// MemberType(MT): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Namespace_MT_TypeName_MemberB.txt";

				#region Example_MT_MemberB

				// Example code

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}
		
		[Description("TypeName.MemberC example")]
		public static void ExampleMemberC()
		{
			try
			{
				// MemberType(MT): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Namespace_MT_TypeName_MemberC.txt";

				#region Example_MT_MemberC

				// Example code

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}
		
		[Description("TypeName.MemberD example")]
		public static void ExampleMemberD()
		{
			try
			{
				// MemberType(MT): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Namespace_MT_TypeName_MemberD.txt";

				#region Example_MT_MemberD

				// Example code

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}
		
		[Description("TypeName.MemberE example")]
		public static void ExampleMemberE()
		{
			try
			{
				// MemberType(MT): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Namespace_MT_TypeName_MemberE.txt";

				#region Example_MT_MemberE

				// Example code

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}
		
		[Description("TypeName.MemberF example")]
		public static void ExampleMemberF()
		{
			try
			{
				// MemberType(MT): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Namespace_MT_TypeName_MemberF.txt";

				#region Example_MT_MemberF

				// Example code

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}
		
		
		[Description("TypeName.MemberG example")]
		public static void ExampleMemberG()
		{
			try
			{
				// MemberType(MT): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Namespace_MT_TypeName_MemberG.txt";

				#region Example_MT_MemberG

				// Example code

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}
		
		[Description("TypeName.MemberH example")]
		public static void ExampleMemberH()
		{
			try
			{
				// MemberType(MT): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Namespace_MT_TypeName_MemberH.txt";

				#region Example_MT_MemberH

				// Example code

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}
		
		[Description("TypeName.MemberI example")]
		public static void ExampleMemberI()
		{
			try
			{
				// MemberType(MT): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Namespace_MT_TypeName_MemberI.txt";

				#region Example_MT_MemberI

				// Example code

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}
		
		[Description("TypeName.MemberJ example")]
		public static void ExampleMemberJ()
		{
			try
			{
				// MemberType(MT): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Namespace_MT_TypeName_MemberJ.txt";

				#region Example_MT_MemberJ

				// Example code

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		
	}

}
