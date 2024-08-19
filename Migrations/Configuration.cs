namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApplication1.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApplication1.Models.ApplicationDbContext context)
        {
            // Ensure the Classes table has the "Form 1" entry
            var form1Class = context.Classes.SingleOrDefault(c => c.Name == "Form 1");
            if (form1Class == null)
            {
                form1Class = new Class { Name = "Form 1" };
                context.Classes.Add(form1Class);
                context.SaveChanges();
            }

            // Add sample ClassStreams
            if (!context.ClassStreams.Any(cs => cs.StreamName == "Form 1A"))
            {
                context.ClassStreams.Add(new ClassStream { StreamName = "Form 1A", ClassId = form1Class.Id });
            }
            if (!context.ClassStreams.Any(cs => cs.StreamName == "Form 1B"))
            {
                context.ClassStreams.Add(new ClassStream { StreamName = "Form 1B", ClassId = form1Class.Id });
            }
            if (!context.ClassStreams.Any(cs => cs.StreamName == "Form 1C"))
            {
                context.ClassStreams.Add(new ClassStream { StreamName = "Form 1C", ClassId = form1Class.Id });
            }

            context.SaveChanges();
        }
    }
}
