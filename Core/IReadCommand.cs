using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public interface IReadCommand<TOut>
    {
        Task<TOut> Read();
    }

    public interface IReadCommand<TOut, TIn>
    {
        Task<TOut> Read(TIn parameter);
    }
}
