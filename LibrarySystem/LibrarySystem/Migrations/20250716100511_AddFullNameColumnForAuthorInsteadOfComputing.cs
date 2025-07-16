using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class AddFullNameColumnForAuthorInsteadOfComputing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Author",
                type: "nvarchar(max)",
                nullable: true);

            // Add a trigger to set the FullName column after insert or update
            migrationBuilder.Sql(@"
                CREATE TRIGGER TRG_Authors_SetFullName
                ON Author
                AFTER INSERT, UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE A
                    SET FullName = I.FirstName + ' ' + I.LastName
                    FROM Author A
                    INNER JOIN inserted I ON A.Id = I.Id;
                END"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop trigger
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS TRG_Authors_SetFullName");


            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Author");
        }
    }
}
