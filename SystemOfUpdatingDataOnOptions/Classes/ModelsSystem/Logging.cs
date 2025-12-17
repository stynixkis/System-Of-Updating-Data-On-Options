using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace ParsingWebSite.Classes
{
    /// <summary>
    /// Класс для создания логов
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// Метод логирования
        /// </summary>
        public static void ConfigureFileLogging()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            var fileAppender = new FileAppender();
            fileAppender.Name = "FileAppender";

            string projectPath = GetProjectRootPath();
            string logPath = Path.Combine(projectPath, "Logs", $"LogDataUpdate_{DateTime.Now:dd.MM.yyyy}.log");
            Directory.CreateDirectory(Path.Combine(projectPath, "Logs"));
            fileAppender.File = logPath;

            fileAppender.AppendToFile = true;
            var patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level - %message%newline";
            patternLayout.ActivateOptions();

            fileAppender.Layout = patternLayout;
            fileAppender.ActivateOptions();

            hierarchy.Root.AddAppender(fileAppender);
            hierarchy.Root.Level = log4net.Core.Level.All;
            hierarchy.Configured = true;
        }
        /// <summary>
        /// Метод поиска проекта SystemOfUpdatingDataOnOptions (чтобы все логи были в одном месте)
        /// </summary>
        /// <returns></returns>
        public static string GetProjectRootPath()
        {
            // поиск .sln файл
            var currentDir = Directory.GetCurrentDirectory();
            var directory = new DirectoryInfo(currentDir);

            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }

            if (directory != null)
            {
                var projectPath = Path.Combine(directory.FullName, "SystemOfUpdatingDataOnOptions");
                if (Directory.Exists(projectPath))
                {
                    return projectPath;
                }
            }

            // поиск по имени проекта в структуре
            currentDir = AppContext.BaseDirectory;
            directory = new DirectoryInfo(currentDir);

            while (directory != null)
            {
                var projectPath = Path.Combine(directory.FullName, "SystemOfUpdatingDataOnOptions");
                if (Directory.Exists(projectPath))
                {
                    return projectPath;
                }
                directory = directory.Parent;
            }

            // возврат текущей директории
            return Directory.GetCurrentDirectory();
        }
    }
}