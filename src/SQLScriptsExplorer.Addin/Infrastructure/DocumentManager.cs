using EnvDTE;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SQLScriptsExplorer.Addin.Repository;
using SQLScriptsExplorer.Addin.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQLScriptsExplorer.Addin.Infrastructure
{
    public static class DocumentManager
    {
        public static void OpenTemplate(string fileName, string fileFullPath)
        {
            try
            {
                // TODO: This needs to open in a new window, not edit the current one.
                ThreadHelper.ThrowIfNotOnUIThread();

                if (!File.Exists(fileFullPath))
                {
                    throw new FileNotFoundException($"File {fileFullPath} doesn't exist!");
                }

                var openDoc = Package.GetGlobalService(typeof(SVsUIShellOpenDocument)) as IVsUIShellOpenDocument;
                if (openDoc == null)
                {
                    throw new InvalidOperationException("Could not get OpenDocument service.");
                }

                Guid logicalView = VSConstants.LOGVIEWID_Primary;
                IVsUIHierarchy hierarchy;
                uint itemId;
                IVsWindowFrame windowFrame;
                Microsoft.VisualStudio.OLE.Interop.IServiceProvider docServiceProvider;

                int hr = openDoc.OpenDocumentViaProject(
                    fileFullPath,
                    ref logicalView,
                    out docServiceProvider,
                    out hierarchy,
                    out itemId,
                    out windowFrame);

                if (ErrorHandler.Succeeded(hr) && windowFrame != null)
                {
                    windowFrame.Show();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditTemplate(string fileName, string fileFullPath)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (!File.Exists(fileFullPath))
                {
                    throw new FileNotFoundException($"File {fileFullPath} doesn't exist!");
                }

                var openDoc = Package.GetGlobalService(typeof(SVsUIShellOpenDocument)) as IVsUIShellOpenDocument;
                if (openDoc == null)
                {
                    throw new InvalidOperationException("Could not get OpenDocument service.");
                }

                Guid logicalView = VSConstants.LOGVIEWID_Primary;
                IVsUIHierarchy hierarchy;
                uint itemId;
                IVsWindowFrame windowFrame;
                Microsoft.VisualStudio.OLE.Interop.IServiceProvider docServiceProvider;

                int hr = openDoc.OpenDocumentViaProject(
                    fileFullPath,
                    ref logicalView,
                    out docServiceProvider,
                    out hierarchy,
                    out itemId,
                    out windowFrame);

                if (ErrorHandler.Succeeded(hr) && windowFrame != null)
                {
                    windowFrame.Show();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ExecuteTemplate(string fileName, string fileFullPath)
        {
            // TODO: This is opening the document, however it's not executing the query (automatically removing the first line instead).

            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (!File.Exists(fileFullPath))
                {
                    throw new FileNotFoundException($"File {fileFullPath} doesn't exist!");
                }

                var openDoc = Package.GetGlobalService(typeof(SVsUIShellOpenDocument)) as IVsUIShellOpenDocument;
                if (openDoc == null)
                {
                    throw new InvalidOperationException("Could not get OpenDocument service.");
                }

                Guid logicalView = VSConstants.LOGVIEWID_Primary;
                IVsUIHierarchy hierarchy;
                uint itemId;
                IVsWindowFrame windowFrame;
                Microsoft.VisualStudio.OLE.Interop.IServiceProvider docServiceProvider;

                int hr = openDoc.OpenDocumentViaProject(
                    fileFullPath,
                    ref logicalView,
                    out docServiceProvider,
                    out hierarchy,
                    out itemId,
                    out windowFrame);

                if (ErrorHandler.Succeeded(hr) && windowFrame != null)
                {
                    windowFrame.Show();

                    // Now, get the command interface from the window frame
                    var commandTarget = GetCommandTarget(windowFrame);
                    if (commandTarget != null)
                    {
                        // Execute the Query.Execute command
                        Guid cmdGroup = new Guid("5EFC7975-14BC-11CF-9B2B-00AA00573819");
                        uint cmdId = 16; // Command ID for Query.Execute in SSMS

                        commandTarget.Exec(
                            ref cmdGroup,
                            cmdId,
                            (uint)OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER,
                            IntPtr.Zero,
                            IntPtr.Zero);
                    }
                    else
                    {
                        throw new InvalidOperationException("Could not get command target to execute the query.");
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Failed to open document: {fileFullPath}");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static IOleCommandTarget GetCommandTarget(IVsWindowFrame windowFrame)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            object docView;
            if (ErrorHandler.Succeeded(windowFrame.GetProperty((int)__VSFPROPID.VSFPROPID_DocView, out docView)) && docView != null)
            {
                return docView as IOleCommandTarget;
            }
            return null;
        }

        public static void FormatSelection()
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                DTE dte = Package.GetGlobalService(typeof(DTE)) as DTE;

                if (dte.ActiveDocument != null)
                {
                    TextSelection selection = (TextSelection)dte.ActiveDocument.Selection;

                    // Format whole text: selection.SelectAll();
                    string selectedText = selection.Text;

                    // Nothing is selected
                    if (string.IsNullOrEmpty(selectedText))
                        return;

                    string formattedText = FormatSelectionUsingSQLServer(selectedText);

                    selection.Delete();
                    selection.Insert(formattedText);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string FormatSelectionUsingSQLServer(string code)
        {
            ISettingsRepository settingsRepository = new SettingsRepository();

            var tupleSQLParser = GetSQLParser(settingsRepository.SQLParserVersion);

            TSqlParser sqlParser = tupleSQLParser.Item1;
            SqlScriptGenerator sqlScriptGeneration = tupleSQLParser.Item2;

            // Parse selected code and look for errors
            IList<ParseError> lstParseErrors = new List<ParseError>();
            var result = sqlParser.Parse(new StringReader(code), out lstParseErrors);

            if (lstParseErrors.Count > 0)
            {
                string error = lstParseErrors.Select(p => p.Message).Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");

                throw new Exception($"SQL Parser unable format selected text due to a syntax error.{Environment.NewLine}{Environment.NewLine}{error}");
            }

            // Generate formatted code
            string resultCode = string.Empty;
            sqlScriptGeneration.Options.IncludeSemicolons = false;
            sqlScriptGeneration.Options.AlignClauseBodies = false;
            sqlScriptGeneration.GenerateScript(result, out resultCode);

            return resultCode;
        }

        private static Tuple<TSqlParser, SqlScriptGenerator> GetSQLParser(string targetVersion)
        {
            if (targetVersion == "SQL Server 2019")
            {
                return new Tuple<TSqlParser, SqlScriptGenerator>(new TSql150Parser(false), new Sql150ScriptGenerator());
            }
            else if (targetVersion == "SQL Server 2017")
            {
                return new Tuple<TSqlParser, SqlScriptGenerator>(new TSql140Parser(false), new Sql140ScriptGenerator());
            }
            else if (targetVersion == "SQL Server 2016")
            {
                return new Tuple<TSqlParser, SqlScriptGenerator>(new TSql130Parser(false), new Sql130ScriptGenerator());
            }

            throw new ArgumentOutOfRangeException($"Couldn't find the SQL Parser for {targetVersion}.");
        }
    }
}
