using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        //Criado para fazer nossas configs com a base
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAttendee>  ActivityAttendees { get; set; }

        public DbSet<Photo> Photos { get; set; }

        //Criando esse metodo para criar nossa relacao de user para activity
        //"ef migration" na mao - entre varias aspas pq nao e migration o que fazemos aqui
        //Many to many relationship
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //configura a primary key na nossa tabela ActivityAttendee
            builder.Entity<ActivityAttendee>(x => x.HasKey(aa => new {aa.AppUserId, aa.ActivityId}));
            
            //chave primaria
            builder.Entity<ActivityAttendee>()
            .HasOne(u => u.AppUser)
            .WithMany(a => a.Activities)
            .HasForeignKey(aa => aa.AppUserId);
            
            //chave primaria
            builder.Entity<ActivityAttendee>()
            .HasOne(u => u.Activity)
            .WithMany(a => a.Attendees)
            .HasForeignKey(aa => aa.ActivityId);
        }

    }
}