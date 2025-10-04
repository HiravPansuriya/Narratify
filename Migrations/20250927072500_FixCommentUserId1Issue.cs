using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Narratify.Migrations
{
    /// <inheritdoc />
    public partial class FixCommentUserId1Issue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the UserId1 column if it exists (this was created as a shadow property)
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                          WHERE TABLE_NAME = 'Comments' AND COLUMN_NAME = 'UserId1')
                BEGIN
                    -- Drop foreign key constraint on UserId1 if exists
                    DECLARE @constraintName NVARCHAR(128)
                    SELECT @constraintName = name 
                    FROM sys.foreign_keys 
                    WHERE parent_object_id = OBJECT_ID('Comments') 
                    AND referenced_object_id = OBJECT_ID('AspNetUsers')
                    AND name LIKE '%UserId1%'
                    
                    IF @constraintName IS NOT NULL
                        EXEC('ALTER TABLE Comments DROP CONSTRAINT ' + @constraintName)
                    
                    -- Drop the UserId1 column
                    ALTER TABLE Comments DROP COLUMN UserId1;
                END
            ");
            
            // Ensure the proper UserId foreign key constraint exists
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.foreign_keys 
                              WHERE parent_object_id = OBJECT_ID('Comments') 
                              AND referenced_object_id = OBJECT_ID('AspNetUsers')
                              AND name LIKE '%FK_Comments_AspNetUsers_UserId%')
                BEGIN
                    ALTER TABLE Comments 
                    ADD CONSTRAINT FK_Comments_AspNetUsers_UserId 
                    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
                    ON DELETE SET NULL;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // This is intentionally left empty as we don't want to recreate the UserId1 issue
        }
    }
}
