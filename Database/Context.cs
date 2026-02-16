using System;
using System.Collections.Generic;
using System.Text;
using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    // TODO: попробовать использовать redis. Пр падении приложения, бд не падает. бд умрет только если упадет сервер redis'а, но можно настроить сохранение на диск
    // Кстати надо настраивать redis
    // "Все работает стабильно, но раз в сутки все данные почему-то пропадают. Просто затираются. И далее запись БД (redis-cli save) невожможна. Error и все."
    // "Ваша проблема: дефолтные настройки директивы save не подходят к Вашему приложению."
    // https://qna.habr.com/q/491956
    public class Context : DbContext
    {
        protected Context(string ConnectionString)
        {
            Database.EnsureCreated();
        }

        public DbSet<TgUser> TgUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Data Source=usersdata.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
