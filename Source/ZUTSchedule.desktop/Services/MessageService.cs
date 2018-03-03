using System;
using System.Collections.Generic;
using ZUTSchedule.core;

namespace ZUTSchedule.desktop
{
    public class MessageService : IMessageService
    {
        public bool ShowAlert(string message)
        {
            var result = MessageBox.Show(message, "Alert", MessageBoxType.Ok);

            return result == MessageBoxResult.Yes;
        }

        public T ShowPickList<T>(List<T> itemsToChooseFrom)
        {
            throw new NotImplementedException();
        }

        public bool ShowYesNoSelector(string title, string message)
        {
            throw new NotImplementedException();
        }
    }
}
