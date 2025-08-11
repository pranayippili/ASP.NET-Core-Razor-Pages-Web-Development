using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
	public class AuthDbContext : IdentityDbContext
	{
		public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			var superAdminRoleId = "37955b76-189c-46d3-b21d-32eacc64d3df";
			var adminRoleId = "154b712b-15b0-4269-98f3-99ddb2f5d442";
			var userRoleId = "4ee76c42-513d-4df8-84a4-d5080ff4cb39";

			// Seed Roles (User, Admin, SuperAdmin)
			var roles = new List<IdentityRole>
			{
				new IdentityRole() { Name = "SuperAdmin", NormalizedName = "SuperAdmin", Id = superAdminRoleId, ConcurrencyStamp = superAdminRoleId },
				new IdentityRole() { Name = "Admin", NormalizedName = "Admin", Id = adminRoleId, ConcurrencyStamp = adminRoleId },
				new IdentityRole() { Name = "User", NormalizedName = "User", Id = userRoleId, ConcurrencyStamp = userRoleId }
			};
			builder.Entity<IdentityRole>().HasData(roles);

			// Seed Super Admin User
			var superAdminId = "07d01052-fc9d-4e50-af50-d772f9e2ff1f";
			var superAdminUser = new IdentityUser
			{
				Id = superAdminId,
				UserName = "superadmin@bloggie.com",
				NormalizedUserName = "SUPERADMIN@BLOGGIE.COM",
				Email = "superadmin@bloggie.com",
				NormalizedEmail = "SUPERADMIN@BLOGGIE.COM",
			};

			superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
				.HashPassword(superAdminUser, "superadmin123");

			builder.Entity<IdentityUser>().HasData(superAdminUser);

			//Add All Roles To Super Admin User
			var superAdminRoles = new List<IdentityUserRole<string>>()
			{
				new IdentityUserRole<string> { UserId = superAdminId, RoleId = superAdminRoleId },
				new IdentityUserRole<string> { UserId = superAdminId, RoleId = adminRoleId },
				new IdentityUserRole<string> { UserId = superAdminId, RoleId = userRoleId }
			};


			builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
		}
	}
}
