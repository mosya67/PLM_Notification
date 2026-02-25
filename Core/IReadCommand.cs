using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    /// <summary>
    /// Интерфейс для команд на чтение из бд
    /// </summary>
    /// <typeparam name="TOut">Что возвращает</typeparam>
    public interface IReadCommand<TOut>
    {
        Task<TOut> Read();
    }

    /// <summary>
    /// Интерфейс для команд на чтение из бд
    /// </summary>
    /// <typeparam name="TOut">Что возвращает</typeparam>
    /// <typeparam name="TIn">Что принимает</typeparam>
    public interface IReadCommand<TOut, TIn>
    {
        Task<TOut> Read(TIn parameter);
    }
}
