using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZUTSchedule.core
{
    public interface INewsService
    {
        /// <summary>
        /// Return a collection of news records
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<NewsRecordViewModel>> GetNews();
    }
}
