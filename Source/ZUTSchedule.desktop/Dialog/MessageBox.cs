namespace ZUTSchedule.desktop
{
    /// <summary>
    /// Results that <see cref="MessageBox"/> can return
    /// </summary>
    public enum MessageBoxResult
    {
        Yes,
        No
    }

    /// <summary>
    /// MessageBox types that can be created
    /// </summary>
    public enum MessageBoxType
    {
        Ok,
        YesNo,
    }

    /// <summary>
    /// MessageBox allows to show DialogWindow.
    /// </summary>
    public static class MessageBox
    {

        /// <summary>
        /// Show dialog window with message and title
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static MessageBoxResult Show(string message, string title)
        {
            return Show(message, title, MessageBoxType.Ok);
        }

        /// <summary>
        /// Show dialog window with message and title
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static MessageBoxResult Show(string message, string title, MessageBoxType type)
        {
            DialogViewModel vm = null;
            bool? DialogResult = false;
            switch (type)
            {
                case MessageBoxType.Ok:
                    vm = new DialogViewModel()
                    {
                        Title = title,
                        Message = message,
                        Type = type,
                        ButtonTextOk = "Ok",
                    };
                    break;
                case MessageBoxType.YesNo:
                    vm = new DialogViewModel()
                    {
                        Title = title,
                        Message = message,
                        Type = type,
                        ButtonTextOk = "Yes",
                        ButtonTextNo = "No",
                    };
                    break;
            }

            DialogResult = new DialogWindow(vm).ShowDialog();

            if (DialogResult == true)
                return MessageBoxResult.Yes;
            else
                return MessageBoxResult.No;
        } 

    }
}
