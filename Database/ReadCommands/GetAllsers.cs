using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database.ReadCommands
{
    public class GetAllsers : IReadCommand<IEnumerable<User>>
    {
        private readonly Context context;

        public GetAllsers(Context context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<User>> Read()
        {
            return await context.Users.ToListAsync();
        }
    }
}
