using Glossary.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Glossary.Api.Data
{
    /// <summary>
    /// Seed data for Glossary API
    /// </summary>
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new GlossaryDbContext(serviceProvider.GetRequiredService<DbContextOptions<GlossaryDbContext>>());

            // Look for any movies.
            if (context.GlossaryItems.Any())
            {
                return;   // DB has been seeded
            }

            context.GlossaryItems.AddRange(
                new GlossaryItem
                {
                    Term = "Abbreviation",
                    Definition = "a short form of a word or phrase, for example: tbc = to be confirmed; CIA = the Central Intelligence Agency"
                },
                new GlossaryItem
                {
                    Term = "Idiom",
                    Definition = "an expression whose meaning is different from the meaning of the individual words"
                },
                new GlossaryItem
                {
                    Term = "Learner's Dictionary",
                    Definition = "a dictionary that is designed to be used by people who are learning a language that is not their first language. Macmillan Dictionary is a learner's dictionary"
                }
            );

            context.SaveChanges();
        }
    }
}
