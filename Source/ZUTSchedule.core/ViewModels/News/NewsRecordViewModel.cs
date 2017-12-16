using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace ZUTSchedule.core
{
    public class NewsRecordViewModel : BaseViewModel
    {

        /// <summary>
        /// Where news come form
        /// </summary>
        public NewsType Type { get; set; }

        /// <summary>
        /// News title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Url to page with news content
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Indicates if news is new 
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// Command that will open site
        /// </summary>
        public ICommand GoToPageCommand { get; private set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public NewsRecordViewModel()
        {
            GoToPageCommand = new RelayCommand(async () => Process.Start(Url));
        }

    }
}
