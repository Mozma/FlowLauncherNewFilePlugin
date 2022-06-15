using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Flow.Launcher.Plugin.NewFile
{
    public class NewFile : IPlugin
    {
        private PluginInitContext _context;
        private string _fileName;
        private string _fileContent;
        private string _filePath;

        public void Init(PluginInitContext context)
        {
            _context = context;
        }

        public List<Result> Query(Query query)
        {
            SplitQuery(query);

            var result = new Result
            {
                Title = string.IsNullOrEmpty(query.Search) ? "name/content" : _fileName,
                SubTitle = _fileContent,
                Action = c =>
                {
                    CreateFile(_fileName, _fileContent);
                    OpenFile(_filePath);
                    return true;
                },

                IcoPath = "Images\\app.png"
            };

            return new List<Result> { result };
        }

        private void OpenFile(string path)
        {
            if (File.Exists(path))
            {
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            }
        }

        private void CreateFile(string name, string content = "")
        {
            string baseFolder = @"E:\trash\";

            string path = baseFolder + name + ".txt";

            using (var sw = new StreamWriter(IncrimentIfExist(path)))
            {
                sw.Write(content + "\n" + _filePath);
            }
        }

        private string IncrimentIfExist(string fullPath)
        {
            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
            string extension = Path.GetExtension(fullPath);
            string path = Path.GetDirectoryName(fullPath);
            string newFullPath = fullPath;
            string tempFileName = "";

            while (File.Exists(newFullPath))
            {
                tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);

            }
            _filePath = newFullPath;
            _fileName = tempFileName;

            return newFullPath;
        }

        private void SplitQuery(Query query)
        {
            string str = query.Search;

            if (String.IsNullOrWhiteSpace(str))
            {
                _fileName = "New file";
                _fileContent = string.Empty;
                return;
            }

            int charLocation = str.IndexOf("/", StringComparison.Ordinal);

            if (charLocation < 0)
            {
                _fileName = str;
                return;
            }

            _fileName = str.Substring(0, charLocation);
            _fileContent = str.Substring(charLocation + 1);
        }
    }
}