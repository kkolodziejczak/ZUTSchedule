using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace ZUTSchedule.core
{
    public class NewsContainerViewModel :BaseViewModel
    {
        /// <summary>
        /// All news Downloaded form sites
        /// </summary>
        public ObservableCollection<NewsRecordViewModel> News { get; set; }

        /// <summary>
        /// Base Constructor
        /// </summary>
        public NewsContainerViewModel()
        {
            //TODO: normalize Titles

            News = new ObservableCollection<NewsRecordViewModel>()
            {
                new NewsRecordViewModel()
                {
                    IsNew = true,
                    Title = "Okresowe badania lekarskie – studia niestacjonarne...",
                    Type = NewsType.Wi,
                    Url = "http://google.pl/",

                },
                new NewsRecordViewModel()
                {
                    IsNew = false,
                    Title = "Okresowe badania lekarskie – studia stacjonarne...",
                    Type = NewsType.Global,
                    Url = "http://google.pl/",

                },
                new NewsRecordViewModel()
                {
                    IsNew = true,
                    Title = "Akcja DKMS \"HELPERS' GENERATION\" - przyjdź i zarejestruj ...",
                    Type = NewsType.Global,
                    Url = "http://google.pl/",

                },
                new NewsRecordViewModel()
                {
                    IsNew = true,
                    Title = "Akcja DKMS \"HELPERS' GENERATION\" - przyjdź i zarejestruj ...",
                    Type = NewsType.Global,
                    Url = "http://google.pl/",

                },
                new NewsRecordViewModel()
                {
                    IsNew = false,
                    Title = "Akcja DKMS \"HELPERS' GENERATION\" - przyjdź i zarejestruj ...",
                    Type = NewsType.Wi,
                    Url = "http://google.pl/",

                },

            };

        }
        
    }
}
