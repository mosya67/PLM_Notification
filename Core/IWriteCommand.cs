using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    /// <summary>
    /// Интерфейс для команд на запись в бд
    /// </summary>
    /// <typeparam name="TIn">Что принимает</typeparam>
    public interface IWriteCommand<TIn>
    {
        Task Write(TIn parameter);
    }
}
