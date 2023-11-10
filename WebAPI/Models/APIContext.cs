﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using WebAPI.Models;

namespace WebAPI.Models
{
    public class APIContext : DbContext
    {

        public APIContext(DbContextOptions<APIContext> options) : base (options) { }

        public DbSet<Cuenta> Cuenta { get; set; }

        public DbSet<Compra> Compra { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {

            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");

            base.ConfigureConventions(builder);
        }

        public DbSet<Pago> Pago { get; set; }

    }
}
