//--------------------------------------------------------------------------
// File:    Examples_CoreMin_Error_ExceptionHelper.cs
// Content:	Implementation of class ExamplesCoreMinErrorExceptionHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using SmartExpert.CUI;
using SmartExpert.Error;
using SmartExpert.IO;

#endregion

namespace SmartExpert.Examples
{
	///<summary>ExamplesCoreMinErrorExceptionHelper</summary>
	public static class ExamplesCoreMinErrorExceptionHelper
	{
		[Description("ExceptionHelper examples")]
		public static void RunAll()
		{
			FileHelper.GetFiles(@".\", "Sample_CoreMin_Error_EXM_Exc*.txt").ForEach(FileHelper.DeleteFile);
			FileHelper.GetFiles(@".\", "Sample_CoreMin_Error_M_Exc*.txt").ForEach(FileHelper.DeleteFile);
			
			ExceptionHelperIsFatalExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExceptionHelperRenderExceptionSummaryExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExceptionHelperRenderExceptionMessageExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExceptionHelperRenderExceptionDetailsExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExceptionHelperRenderExceptionExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExceptionHelperGetExceptionText();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExceptionHelperGetExceptionHashExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExceptionHelperFindMatchingExceptionExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
		}
		
		[Description("ExceptionHelper.IsFatal example")]
		public static void ExceptionHelperIsFatalExample()
		{
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Error_EXM_ExceptionHelper_IsFatal.txt";

				#region Sample_CoreMin_Error_EXM_ExceptionHelper_IsFatal

				int tmp = 42;
				try
				{
					int result = 42 / (42 - tmp);
				}
				catch (Exception exception)
				{
					// Rethrow exception only if the exception or any of the inner exception is a fatal exception.
					// Fatal Exceptions are:
					//   OutOfMemoryException, InsufficientMemoryException, 
					//   ThreadAbortException, AccessViolationException, SEHException.
					if (exception.IsFatal())
						throw;

					ConsoleHelper.WriteLineRed("Exception is not fatal");
				}

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("ExceptionHelper.RenderExceptionSummary example")]
		public static void ExceptionHelperRenderExceptionSummaryExample()
		{
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Error_EXM_ExceptionHelper_RenderExceptionSummary.txt";

				#region Sample_CoreMin_Error_EXM_ExceptionHelper_RenderExceptionSummary

				int tmp = 42;
				try
				{
					int result = 42 / (42 - tmp);
				}
				catch (Exception exception)
				{
					if (exception.IsFatal())
						throw;

					ConsoleHelper.WriteLineRed(exception.RenderExceptionSummary());
				}

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("ExceptionHelper.RenderExceptionMessage example")]
		public static void ExceptionHelperRenderExceptionMessageExample()
		{
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Error_EXM_ExceptionHelper_RenderExceptionMessage.txt";

				#region Sample_CoreMin_Error_EXM_ExceptionHelper_RenderExceptionMessage

				int tmp = 42;
				try
				{
					int result = 42 / (42 - tmp);
				}
				catch (Exception exception)
				{
					if (exception.IsFatal())
						throw;

					ConsoleHelper.WriteLineRed(exception.RenderExceptionMessage());
				}

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("ExceptionHelper.RenderExceptionDetails example")]
		public static void ExceptionHelperRenderExceptionDetailsExample()
		{
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Error_EXM_ExceptionHelper_RenderExceptionDetails.txt";

				#region Sample_CoreMin_Error_EXM_ExceptionHelper_RenderExceptionDetails

				int tmp = 42;
				try
				{
					int result = 42 / (42 - tmp);
				}
				catch (Exception exception)
				{
					if (exception.IsFatal())
						throw;

					ConsoleHelper.WriteLineRed(exception.RenderExceptionDetails());
				}

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("ExceptionHelper.RenderException example")]
		public static void ExceptionHelperRenderExceptionExample()
		{
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Error_EXM_ExceptionHelper_RenderException.txt";

				#region Sample_CoreMin_Error_EXM_ExceptionHelper_RenderException

				int tmp = 42;
				try
				{
					var denominator = 42 - tmp;

					// Denominator validation
					if (denominator == 0)
					{
						var argException = new ArgOutOfRangeException<int>(denominator, "denominator")
											{
												UserFriendlyMessage = "Validation failed!"
											};
						throw argException;
					}

					int result = 42 / denominator;
					ConsoleHelper.WriteNameValue("result", result);
				}
				catch (Exception exception)
				{
					if (exception.IsFatal())
						throw;

					ExceptionText exceptionText = exception.RenderException();
					ConsoleHelper.WriteNameValue("exceptionText", exceptionText.ToString());
					ConsoleHelper.WriteNameValue("exceptionText.Message", exceptionText.Message);
					ConsoleHelper.WriteNameValue("exceptionText.UserFriendlyMessage", exceptionText.UserFriendlyMessage);
					ConsoleHelper.WriteNameValue("exceptionText.FullText", exceptionText.FullText);
				}

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("ExceptionHelper.GetExceptionText example")]
		public static void ExceptionHelperGetExceptionText()
		{
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Error_M_ExceptionHelper_GetExceptionText.txt";

				OuterGetExceptionText();
			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		#region Sample_CoreMin_Error_M_ExceptionHelper_GetExceptionText

		public static void OuterGetExceptionText()
		{
			try
			{
				InnerGetExceptionText();
			}
			catch (Exception exception)
			{
				if (exception.IsFatal())
					throw;

				ConsoleHelper.WriteLineRed(ExceptionHelper.GetExceptionText(exception));
			}

		}

		public static void InnerGetExceptionText()
		{
			List<Exception> errorContainer = new List<Exception>();

			for (var i = 0; i < 2; i++)
			{
				try
				{
					if (i == 0)
					{
						var argException = new ArgOutOfRangeException<int>(i, "argName1")
						{
							UserFriendlyMessage = "Range Validation failed!"
						};
						throw argException;
					}
					if (i == 1)
					{
						var argException = new ArgNullException("argName2")
						{
							UserFriendlyMessage = "No value was provided!"
						};
						throw argException;
					}
				}
				catch (Exception exception)
				{
					if (exception.IsFatal())
						throw;

					errorContainer.Add(exception);
				}
			}

			if (errorContainer.Count > 0)
			{
				throw new CombinedException("Multible Validations failed.", errorContainer);
			}

		}

		#endregion


		[Description("ExceptionHelper.GetExceptionHash example")]
		public static void ExceptionHelperGetExceptionHashExample()
		{
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Error_EXM_ExceptionHelper_GetExceptionHash.txt";

				#region Sample_CoreMin_Error_EXM_ExceptionHelper_GetExceptionHash

				int tmp = 42;
				try
				{
					int result = 42 / (42 - tmp);
				}
				catch (Exception exception)
				{
					if (exception.IsFatal())
						throw;

					ConsoleHelper.WriteNameValue("Exception Hash", exception.GetExceptionHash());
				}

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("ExceptionHelper.FindMatchingException example")]
		public static void ExceptionHelperFindMatchingExceptionExample()
		{
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_Error_EXM_ExceptionHelper_FindMatchingException.txt";

				#region Sample_CoreMin_Error_EXM_ExceptionHelper_FindMatchingException

				try
				{
					int tmp = 42;
					try
					{
						int result = 42 / (42 - tmp);
					}
					catch (Exception exception)
					{
						if (exception.IsFatal())
							throw;

						throw new OperationExecutionFailedException("ExceptionHelperExample", exception);
					}
				}
				catch (Exception outerException)
				{
					if (outerException.IsFatal())
						throw;

					ConsoleHelper.WriteNameValue("Matches TechException",
						outerException.FindMatchingException(typeof(TechException), false).IsNotNull()); 
					// Output: Matches TechException = true

					ConsoleHelper.WriteNameValue("Matches DivideByZeroException",
						outerException.FindMatchingException(typeof(DivideByZeroException), true).IsNotNull());
					// Output: Matches DivideByZeroException = true

					bool result = outerException.FindMatchingException(typeof (BusinessException), false).IsNotNull();
					ConsoleHelper.WriteNameValue("Matches BusinessException", BooleanBoxes.Box(result));
					// Output: Matches BusinessException = false
				}
				

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}
	}

}
