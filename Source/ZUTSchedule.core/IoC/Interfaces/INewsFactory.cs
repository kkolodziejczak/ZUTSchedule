using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZUTSchedule.core
{
    public interface INewsFactory
    {
        /// <summary>
        /// Adds new news service
        /// </summary>
        /// <param name="service"></param>
        void AddNewsService(INewsService service);

        /// <summary>
        /// Removes news service
        /// </summary>
        /// <param name="service"></param>
        void RemoveNewsService(INewsService service);

        /// <summary>
        /// Returns <see cref="IEnumerable{T}"/> with <see cref="NewsRecordViewModel"/>
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<NewsRecordViewModel>> GetNewsAsync();
    }
}
