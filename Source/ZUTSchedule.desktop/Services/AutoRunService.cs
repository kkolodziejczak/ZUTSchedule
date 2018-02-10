using Microsoft.Win32;
using System;
using System.Reflection;
using ZUTSchedule.core;

namespace ZUTSchedule.desktop
{
    public class AutoRunService : IAutoRun
    {
        private RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public void DisableAutoRun()
        {
            if (IsAutoRunEnabled())
                registryKey.DeleteValue("ZUTSchedule");
        }

        public void EnableAutoRun()
        {
            var path = Assembly.GetExecutingAssembly().GetName().CodeBase.Replace("file:///", "").Replace("/", @"\");
            registryKey.SetValue("ZUTSchedule", path);
        }

        public bool IsAutoRunEnabled()
        {
            return registryKey.GetValue("ZUTSchedule") != null;
        }
    }
}
