using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Microsoft.EntityFrameworkCore;

namespace Database.ReadCommands
{
    public class CheckUserExists : IReadCommand<bool, long>
    {
        private readonly Context context;

        public CheckUserExists(Context context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> Read(long id)
        {
            return await context.Users.AnyAsync(e => e.UserId == id);
        }
    }
}
