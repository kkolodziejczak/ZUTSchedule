using System.Collections.Generic;
using ZUTSchedule.core;

namespace ZUTSchedule.mobile
{
    public class MessageService : IMessageService
    {
        public bool ShowAlert(string message)
        {
            return true;
        }

        public T ShowPickList<T>(List<T> itemsToChooseFrom)
        {
            return default(T);
        }

        public bool ShowYesNoSelector(string title, string message)
        {
            return true;
        }
    }
}