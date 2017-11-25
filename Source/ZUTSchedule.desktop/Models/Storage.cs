using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZUTSchedule.core
{
    public static class Storage
    {

        /// <summary>
        /// Path to Archivist data folder
        /// </summary>
        private static string SettingsFolderPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ZUTSchedule";

        /// <summary>
        /// Path where user settings are stored
        /// </summary>
        private static string SettingsFilePath = $@"{SettingsFolderPath}\Settings.ini";

        /// <summary>
        /// User Settings
        /// </summary>
        private static UserSettings _Settings;

        /// <summary>
        /// Path to directory what will be created for temprorary use
        /// </summary>
        public static string TemporaryDirectoryPath { get; } = $@"{SettingsFolderPath}\Temp";

        /// <summary>
        /// Users settings
        /// </summary>
        public static UserSettings Settings
        {
            get
            {
                if (_Settings == null)
                    Load();

                return _Settings;
            }
            private set
            {
                if (value == _Settings)
                    return;

                _Settings = value;

            }
        }


        /// <summary>
        /// Loads <see cref="Settings"/> from file
        /// </summary>
        public static void Load()
        {

            XmlSerializer xs = new XmlSerializer(typeof(UserSettings));
            try
            {
                using (var sr = new StreamReader(new FileStream(SettingsFilePath,
                                                                FileMode.Open,
                                                                FileAccess.Read,
                                                                FileShare.ReadWrite)))
                {
                    Settings = xs.Deserialize(sr) as UserSettings;
                }
            }
            catch
            {
                // Some error with settings file, create new one from scratch.
                Settings = new UserSettings();
                Save();
            }
        }

        /// <summary>
        /// Sets default <see cref="UserSettings"/>
        /// </summary>
        public static void SetDefaultSettings()
        {
            Settings = new UserSettings();
        }

        /// <summary>
        /// Saves <see cref="Settings"/> into file
        /// </summary>
        public static void Save()
        {
            if (Settings == null)
                return;

            if (!Directory.Exists(Path.GetDirectoryName(SettingsFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath));
            }

            XmlSerializer xs = new XmlSerializer(typeof(UserSettings));
            using (TextWriter tw = new StreamWriter(SettingsFilePath))
            {
                xs.Serialize(tw, Settings);
            }

        }

    }
}
