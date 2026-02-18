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
            TgUser user = await context.TgUsers.SingleOrDefaultAsync(e => e.TgUserId == id);
            if (user == null) return;
            context.TgUsers.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}
