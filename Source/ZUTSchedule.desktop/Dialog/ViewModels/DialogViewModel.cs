namespace ZUTSchedule.desktop
{
    public class DialogViewModel
    {
        /// <summary>
        /// Title of dialog box
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Message text to be displayed
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Text that is displayed on Accepting button
        /// </summary>
        public string ButtonTextOk { get; set; }

        /// <summary>
        /// Text that is displayed on Cancel button type
        /// </summary>
        public string ButtonTextNo { get; set; }

        /// <summary>
        /// Type of MessageBox
        /// </summary>
        public MessageBoxType Type { get; set; }
    }
}
