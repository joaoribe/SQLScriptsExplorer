using EnvDTE;
using EnvDTE80;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.Shell;
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
        private static DTE2 DTE      
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return Package.GetGlobalService(typeof(DTE)) as DTE2;
            }
        }

        public static void OpenTemplate(string fileName, string fileFullPath)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (File.Exists(fileFullPath))
                {
                    string fileContent = File.ReadAllText(fileFullPath);
                    var docName = Path.GetFileNameWithoutExtension(fileName);
                    var docExtension = Path.GetExtension(fileName);

                    var fileDocument = DTE.ItemOperations.NewFile(@"General\Text File", $"{docName}_Copy{docExtension}").Document;

                    TextSelection textSelection = fileDocument.Selection as TextSelection;
                    textSelection.SelectAll();
                    textSelection.Text = string.Empty;
                    textSelection.Insert(fileContent);
                    textSelection.StartOfDocument();

                    fileDocument.Save();
                }
                else
                {
                    throw new Exception($"File {fileFullPath} doesn't exist!");
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
                if (File.Exists(fileFullPath))
                {
                    DTE.ItemOperations.OpenFile(fileFullPath);
                }
                else
                {
                    throw new Exception($"File {fileFullPath} doesn't exist!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ExecuteTemplate(string fileName, string fileFullPath)
        {
            string CMD_QUERY_EXECUTE = "Query.Execute";

            try
            {
                // Ensure the document we are executing is the document we have opened by checking its name
                if (DTE.ActiveDocument != null && DTE.ActiveDocument.ProjectItem.Name.Equals(fileName))
                {
                    DTE.ExecuteCommand(CMD_QUERY_EXECUTE);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void FormatSelection()
        {
            try
            {
                if (DTE.ActiveDocument != null)
                {
                    TextSelection selection = (TextSelection)DTE.ActiveDocument.Selection;

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
            if (targetVersion == "SQL Server 2025")
            {
                return new Tuple<TSqlParser, SqlScriptGenerator>(new TSql170Parser(false), new Sql170ScriptGenerator());
            }
            else if (targetVersion == "SQL Server 2022")
            {
                return new Tuple<TSqlParser, SqlScriptGenerator>(new TSql160Parser(false), new Sql160ScriptGenerator());
            }
            else if (targetVersion == "SQL Server 2019")
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
