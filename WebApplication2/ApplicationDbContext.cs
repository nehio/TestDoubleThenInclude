using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasOne(p => p.Information)
                .WithOne(i => i.User)
                .HasForeignKey<UserInformation>(b => b.Id);
            });

            builder.Entity<UserInformationPhoneNumber>()
            .HasKey(u => new { u.PhoneNumberId, u.UserInformationId });

            // These are not required as the relations are defined in the classes themselves
            builder.Entity<UserInformationPhoneNumber>().HasOne(s => s.UserInformation).WithMany(s => s.PhoneNumbers).HasForeignKey(s => s.UserInformationId);
            builder.Entity<UserInformationPhoneNumber>().HasOne(s => s.PhoneNumber).WithMany().HasForeignKey(s => s.PhoneNumberId);
        }

        public class ApplicationUser
        {
            [Key]
            public int Id { get; set; }

            public string LastName { get; set; }


            public string FirstName { get; set; }

            public UserInformation Information { get; set; }
        }

        public class UserInformation
        {
            [Key]
            public int Id { get; set; }
            public List<UserInformationPhoneNumber> PhoneNumbers { get; set; }

            public ApplicationUser User { get; set; }
        }
        public class UserInformationPhoneNumber
        {

            public UserInformationPhoneNumber()
            {
                PhoneNumber = new PhoneNumber();
            }

            public UserInformationPhoneNumber(PhoneNumber p)
            {
                PhoneNumber = p;
            }

            public UserInformation UserInformation { get; set; }

            [ForeignKey("UserInformation")]
            public int UserInformationId { get; set; }

            public PhoneNumber PhoneNumber { get; set; }

            [ForeignKey("PhoneNumber")]
            public int PhoneNumberId { get; set; }
        }
        public class PhoneNumber
        {

            [Key]
            public int Id { get; set; }

            public string Value { get; set; }

        }
    }
}
