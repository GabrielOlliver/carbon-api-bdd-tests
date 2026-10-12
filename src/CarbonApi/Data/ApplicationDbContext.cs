using CarbonApi.Models;
using CarbonApi.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarbonApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<EmissaoCarbono> EmissoesCarbono { get; set; }
    public DbSet<AgendamentoReducao> AgendamentosReducao { get; set; }
    public DbSet<CompensacaoCarbono> CompensacoesCarbono { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Empresa
        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.ToTable("TB_EMPRESA");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID").UseIdentityColumn();
            entity.Property(e => e.Nome).HasColumnName("NOME").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Cnpj).HasColumnName("CNPJ").HasMaxLength(18).IsRequired();
            entity.Property(e => e.Setor).HasColumnName("SETOR").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Pais).HasColumnName("PAIS").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Ativa).HasColumnName("ATIVA").IsRequired();
            entity.Property(e => e.DataCadastro).HasColumnName("DATA_CADASTRO").IsRequired();
            entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO").IsRequired();
            entity.HasIndex(e => e.Cnpj).IsUnique();
        });

        // EmissaoCarbono
        modelBuilder.Entity<EmissaoCarbono>(entity =>
        {
            entity.ToTable("TB_EMISSAO_CARBONO");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID").UseIdentityColumn();
            entity.Property(e => e.EmpresaId).HasColumnName("EMPRESA_ID").IsRequired();
            entity.Property(e => e.QuantidadeTco2e).HasColumnName("QUANTIDADE_TCO2E")
                  .HasPrecision(15, 4).IsRequired();
            entity.Property(e => e.Escopo).HasColumnName("ESCOPO")
                  .HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(e => e.Descricao).HasColumnName("DESCRICAO").HasMaxLength(500);
            entity.Property(e => e.DataEmissao).HasColumnName("DATA_EMISSAO").IsRequired();
            entity.Property(e => e.DataRegistro).HasColumnName("DATA_REGISTRO").IsRequired();
            entity.HasOne(e => e.Empresa)
                  .WithMany(emp => emp.Emissoes)
                  .HasForeignKey(e => e.EmpresaId);
        });

        // AgendamentoReducao
        modelBuilder.Entity<AgendamentoReducao>(entity =>
        {
            entity.ToTable("TB_AGENDAMENTO_REDUCAO");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID").UseIdentityColumn();
            entity.Property(e => e.EmpresaId).HasColumnName("EMPRESA_ID").IsRequired();
            entity.Property(e => e.Titulo).HasColumnName("TITULO").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Descricao).HasColumnName("DESCRICAO").HasMaxLength(1000);
            entity.Property(e => e.MetaReducaoTco2e).HasColumnName("META_REDUCAO_TCO2E")
                  .HasPrecision(15, 4).IsRequired();
            entity.Property(e => e.Status).HasColumnName("STATUS")
                  .HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(e => e.DataInicio).HasColumnName("DATA_INICIO").IsRequired();
            entity.Property(e => e.DataFimPrevista).HasColumnName("DATA_FIM_PREVISTA").IsRequired();
            entity.Property(e => e.DataCadastro).HasColumnName("DATA_CADASTRO").IsRequired();
            entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO").IsRequired();
            entity.HasOne(e => e.Empresa)
                  .WithMany(emp => emp.Agendamentos)
                  .HasForeignKey(e => e.EmpresaId);
        });

        // CompensacaoCarbono
        modelBuilder.Entity<CompensacaoCarbono>(entity =>
        {
            entity.ToTable("TB_COMPENSACAO_CARBONO");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID").UseIdentityColumn();
            entity.Property(e => e.EmpresaId).HasColumnName("EMPRESA_ID").IsRequired();
            entity.Property(e => e.Tipo).HasColumnName("TIPO")
                  .HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(e => e.QuantidadeTco2e).HasColumnName("QUANTIDADE_TCO2E")
                  .HasPrecision(15, 4).IsRequired();
            entity.Property(e => e.Descricao).HasColumnName("DESCRICAO").HasMaxLength(500);
            entity.Property(e => e.CertificadoUrl).HasColumnName("CERTIFICADO_URL").HasMaxLength(500);
            entity.Property(e => e.DataCompensacao).HasColumnName("DATA_COMPENSACAO").IsRequired();
            entity.Property(e => e.DataRegistro).HasColumnName("DATA_REGISTRO").IsRequired();
            entity.HasOne(e => e.Empresa)
                  .WithMany(emp => emp.Compensacoes)
                  .HasForeignKey(e => e.EmpresaId);
        });

        // Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("TB_USUARIO");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID").UseIdentityColumn();
            entity.Property(e => e.Nome).HasColumnName("NOME").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Email).HasColumnName("EMAIL").HasMaxLength(200).IsRequired();
            entity.Property(e => e.SenhaHash).HasColumnName("SENHA").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Role).HasColumnName("ROLE").HasMaxLength(20).IsRequired();
            entity.Property(e => e.EmpresaId).HasColumnName("EMPRESA_ID");
            entity.Property(e => e.Ativo).HasColumnName("ATIVO").IsRequired();
            entity.Property(e => e.DataCadastro).HasColumnName("DATA_CADASTRO").IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasOne(e => e.Empresa)
                  .WithMany()
                  .HasForeignKey(e => e.EmpresaId);
        });

        // Seed de dados iniciais
        modelBuilder.Entity<Empresa>().HasData(new Empresa
        {
            Id = 1,
            Nome = "EcoTech Brasil",
            Cnpj = "12.345.678/0001-99",
            Setor = "Tecnologia",
            Pais = "Brasil",
            Ativa = true,
            DataCadastro = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            DataAtualizacao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });

        modelBuilder.Entity<Usuario>().HasData(new Usuario
        {
            Id = 1,
            Nome = "Administrador",
            Email = "admin@esg.com",
            SenhaHash = "$2a$10$.KMru7cwP9MPAksH5rIg2eoEYTaOIZ3QFAekd5o6RZNZJGIPrmkkK",
            Role = "ADMIN",
            Ativo = true,
            DataCadastro = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}