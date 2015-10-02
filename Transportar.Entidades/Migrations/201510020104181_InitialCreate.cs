namespace Transportador.Entidades.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classificacao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transportadora",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        ClassificacaoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classificacao", t => t.ClassificacaoId, cascadeDelete: true)
                .Index(t => t.ClassificacaoId);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        email = c.String(),
                        Senha = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transportadora", "ClassificacaoId", "dbo.Classificacao");
            DropIndex("dbo.Transportadora", new[] { "ClassificacaoId" });
            DropTable("dbo.Usuario");
            DropTable("dbo.Transportadora");
            DropTable("dbo.Classificacao");
        }
    }
}
