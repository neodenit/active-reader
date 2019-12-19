﻿using Microsoft.EntityFrameworkCore;
using Neodenit.ActiveReader.Common.Models;

namespace Neodenit.ActiveReader.DataAccess
{
    public class ActiveReaderDbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }

        public DbSet<Stat> Statistics { get; set; }

        public DbSet<Word> Words { get; set; }
    }
}