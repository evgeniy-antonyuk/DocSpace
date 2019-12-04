﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ASC.Core.Common.EF.Model
{
    [Table("account_links")]
    public class AccountLinks : BaseDbContext
    {
        public string Id { get; set; }
        public string UId { get; set; }
        public string Provider { get; set; }
        public string Profile { get; set; }
        public DateTime Linked { get; set; }
    }

    public static class AccountLinksExtension
    {
        public static void AddAccountLinks(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountLinks>()
                        .HasKey(c => new { c.Id, c.UId });
        }
    }
}
