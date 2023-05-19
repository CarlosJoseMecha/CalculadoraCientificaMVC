using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace CalculadoraMVC.Migrations
{
   public partial class SeedRoles : Migration
   {
      private string SuperAdminRoleid = Guid.NewGuid().ToString();
      private string AdminRoleId = Guid.NewGuid().ToString();
      private string UserRoleId = Guid.NewGuid().ToString();

      private string User1Id = Guid.NewGuid().ToString();

      protected override void Up(MigrationBuilder migrationBuilder)
      {
         SeedRolesSQL(migrationBuilder);
         SeedUser(migrationBuilder);
         SeedUserRoles(migrationBuilder);
      }

      private void SeedRolesSQL(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.Sql($@"INSERT INTO AspNetRoles ([Id],[Name],[NormalizedName],[ConcurrencyStamp])
                VALUES ('{SuperAdminRoleid}', 'SuperAdmin', 'SUPERADMIN', null);");
         migrationBuilder.Sql($@"INSERT INTO AspNetRoles ([Id],[Name],[NormalizedName],[ConcurrencyStamp])
                VALUES ('{AdminRoleId}', 'Administrator', 'ADMINISTRATOR', null);");
         migrationBuilder.Sql($@"INSERT INTO AspNetRoles ([Id],[Name],[NormalizedName],[ConcurrencyStamp])
                VALUES ('{UserRoleId}', 'User', 'USER', null);");
      }

      private void SeedUser(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.Sql(
             @$"INSERT INTO AspNetUsers ([Id], [FirstName], [LastName], [UserName], [NormalizedUserName],
                [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp],
                [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount])
                VALUES
                ('{SuperAdminRoleid}', 'SuperAdmin', 'SuperAdmin', 'superAdmin@admin.com', 'SUPERADMIN@ADMIN.COM',
                'superAdmin@admin.com', 'SUPERADMIN@ADMIN.COM', 0,
                'AQAAAAEAACcQAAAAEDGQ5wwj6Iz0lXHIZ5IwuvgSO88jrSBT1etWcDYjJN5CBNDKvddZcEeixYBYmcdFag==',
                'YUPAFWNGZI2UC5FOITC7PX5J7XZTAZAA', '8e150555-a20d-4610-93ff-49c5af44f749', NULL, 0, 0, NULL, 1, 0)");
      }

      private void SeedUserRoles(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.Sql($@"
                INSERT INTO AspNetUserRoles ([UserId], [RoleId])
                VALUES ('{SuperAdminRoleid}', '{SuperAdminRoleid}');
                INSERT INTO AspNetUserRoles ([UserId], [RoleId])
                VALUES ('{SuperAdminRoleid}', '{AdminRoleId}');");
      }

      protected override void Down(MigrationBuilder migrationBuilder)
      {
      }
   }
}