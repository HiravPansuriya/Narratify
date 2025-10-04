using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Narratify.Migrations
{
    /// <inheritdoc />
    public partial class FixUserTableColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the missing LastLoginAt column
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
                
            // Rename Status column to UserStatus if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                          WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'Status')
                BEGIN
                    EXEC sp_rename 'AspNetUsers.Status', 'UserStatus', 'COLUMN';
                END
            ");
            
            // Add UserStatus column if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                              WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'UserStatus')
                BEGIN
                    ALTER TABLE AspNetUsers ADD UserStatus int NOT NULL DEFAULT 0;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the LastLoginAt column
            migrationBuilder.DropColumn(
                name: "LastLoginAt",
                table: "AspNetUsers");
                
            // Rename UserStatus back to Status
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                          WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'UserStatus')
                BEGIN
                    EXEC sp_rename 'AspNetUsers.UserStatus', 'Status', 'COLUMN';
                END
            ");
        }
    }
}
