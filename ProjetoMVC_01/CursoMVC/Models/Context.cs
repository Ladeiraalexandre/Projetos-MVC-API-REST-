using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CursoMVC.Models
{
    public class Context : DbContext
    {
        public virtual DbSet<Categoria> Categorias { get; set; } //informar para o Entity que vc vai ter a tabela X
        public DbSet<Produto> Produtos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=Cursomvc;Trusted_Connection=True");                                          
        }
        /*public Context(DbContextOptions<Context> options) : base(options)
        { }
        */
        public virtual void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }
    }
}
