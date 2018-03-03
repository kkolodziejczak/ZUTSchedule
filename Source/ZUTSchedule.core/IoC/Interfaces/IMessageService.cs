using System.Collections.Generic;

namespace ZUTSchedule.core
{
    public interface IMessageService
    {
        bool ShowYesNoSelector(string title, string message);

        T ShowPickList<T>(List<T> itemsToChooseFrom);

        bool ShowAlert(string message);
    }
}
