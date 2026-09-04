using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeoToProjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Seo_CanonicalUrl",
                table: "ProjectTranslations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Seo_MetaDescription",
                table: "ProjectTranslations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Seo_MetaTitle",
                table: "ProjectTranslations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Seo_NoFollow",
                table: "ProjectTranslations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Seo_NoIndex",
                table: "ProjectTranslations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Seo_OgDescription",
                table: "ProjectTranslations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Seo_OgImageUrl",
                table: "ProjectTranslations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Seo_OgTitle",
                table: "ProjectTranslations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seo_CanonicalUrl",
                table: "ProjectTranslations");

            migrationBuilder.DropColumn(
                name: "Seo_MetaDescription",
                table: "ProjectTranslations");

            migrationBuilder.DropColumn(
                name: "Seo_MetaTitle",
                table: "ProjectTranslations");

            migrationBuilder.DropColumn(
                name: "Seo_NoFollow",
                table: "ProjectTranslations");

            migrationBuilder.DropColumn(
                name: "Seo_NoIndex",
                table: "ProjectTranslations");

            migrationBuilder.DropColumn(
                name: "Seo_OgDescription",
                table: "ProjectTranslations");

            migrationBuilder.DropColumn(
                name: "Seo_OgImageUrl",
                table: "ProjectTranslations");

            migrationBuilder.DropColumn(
                name: "Seo_OgTitle",
                table: "ProjectTranslations");
        }
    }
}
