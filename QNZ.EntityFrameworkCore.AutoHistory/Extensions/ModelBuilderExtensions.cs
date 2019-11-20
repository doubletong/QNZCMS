﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SIG.EntityFrameworkCore.AutoHistory.Extensions
{
    /// <summary>
    /// Represents a plugin for Microsoft.EntityFrameworkCore to support automatically recording data changes history.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        private const int DefaultChangedMaxLength = 2048;

        /// <summary>
        /// Enables the automatic recording change history.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> to enable auto history feature.</param>
        /// <param name="changedMaxLength">The maximum length of the 'Changed' column. <c>null</c> will use default setting 2048.</param>
        /// <returns>The <see cref="ModelBuilder"/> had enabled auto history feature.</returns>
        public static ModelBuilder EnableAutoHistory(this ModelBuilder modelBuilder, int? changedMaxLength)
        {
            modelBuilder.Entity<AutoHistory>(b =>
            {
                b.Property(c => c.RowId).IsRequired().HasMaxLength(50);
                b.Property(c => c.TableName).IsRequired().HasMaxLength(128);

                var max = changedMaxLength ?? DefaultChangedMaxLength;
                if (max <= 0)
                {
                    max = DefaultChangedMaxLength;
                }
                b.Property(c => c.Changed).HasMaxLength(max);

                // This MSSQL only
                //b.Property(c => c.Created).HasDefaultValueSql("getdate()");
            });

            return modelBuilder;
        }
    }
}
