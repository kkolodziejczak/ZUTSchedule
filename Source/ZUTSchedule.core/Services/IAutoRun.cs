using System;
using System.Collections.Generic;
using System.Text;

namespace ZUTSchedule.core
{
    public interface IAutoRun
    {
        void EnableAutoRun();
        void DisableAutoRun();
        bool IsAutoRunEnabled();
    }
}
