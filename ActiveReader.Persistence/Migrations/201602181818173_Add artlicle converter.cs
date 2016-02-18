namespace ActiveReader.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addartlicleconverter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Words",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ArticleID = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        CorrectedWord = c.String(),
                        OriginalWord = c.String(),
                        NextSpace = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Articles", t => t.ArticleID, cascadeDelete: true)
                .Index(t => t.ArticleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Words", "ArticleID", "dbo.Articles");
            DropIndex("dbo.Words", new[] { "ArticleID" });
            DropTable("dbo.Words");
        }
    }
}
