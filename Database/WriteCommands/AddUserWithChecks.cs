using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Database.Model;
using Database.ReadCommands;

namespace Database.WriteCommands
{
    public class AddUserWithChecks : IWriteCommand<long>
    {
        private readonly Context context;
        private readonly IReadCommand<bool, long> checkUserExists;

        public AddUserWithChecks(Context context, IReadCommand<bool, long> checkUserExists)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.checkUserExists = checkUserExists ?? throw new ArgumentNullException(nameof(checkUserExists));
        }

        public async Task Write(long id)
        {
            User user = new() { UserId = id };
            if (await checkUserExists.Read(id)) return;
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }
    }
}
