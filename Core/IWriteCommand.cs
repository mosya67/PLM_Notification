using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public interface IWriteCommand<TIn>
    {
        Task Write(TIn parameter);
    }
}
