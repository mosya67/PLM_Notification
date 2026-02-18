using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database.WriteCommands
{
    public class DeleteUser : IWriteCommand<long>
    {
        private readonly Context context;

        public DeleteUser(Context context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Write(long id)
        {
            User user = await context.Users.SingleOrDefaultAsync(e => e.UserId == id);
            if (user == null) return;
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}
