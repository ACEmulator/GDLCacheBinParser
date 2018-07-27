using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using PhatACCacheBinParser.ACE_Helpers;

namespace PhatACCacheBinParser.SQLWriters
{
    static class CraftingSQLWriter
    {
        public static void WriteFiles(CraftTableExtensions.CraftTableExtensionsConversionResult input, Dictionary<uint, string> weenieNames,  string outputFolder, bool includeDELETEStatementBeforeInsert = false)
        {
            // Sort the cookBooks by recipe
            var sortedCookBooks = new Dictionary<uint, List<ACE.Database.Models.World.CookBook>>();

            foreach (var value in input.CookBooks)
            {
                if (!sortedCookBooks.ContainsKey(value.RecipeId))
                    sortedCookBooks.Add(value.RecipeId, new List<ACE.Database.Models.World.CookBook>());

                sortedCookBooks[value.RecipeId].Add(value);
            }

            Parallel.ForEach(input.Recipies, value =>
            //foreach (var value in input)
            {
                List<ACE.Database.Models.World.CookBook> cookBooks;
                sortedCookBooks.TryGetValue(value.RecipeId, out cookBooks);

                WriteFile(value, cookBooks, outputFolder, weenieNames, includeDELETEStatementBeforeInsert);
            });
        }

        public static void WriteFile(ACE.Database.Models.World.Recipe input, IList<ACE.Database.Models.World.CookBook> cookBooks, string outputFolder, Dictionary<uint, string> weenieNames, bool includeDELETEStatementBeforeInsert = false)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            var sqlWriter = new ACE.Database.SQLFormatters.World.RecipeSQLWriter();

            sqlWriter.WeenieNames = weenieNames;

            var cookBookWriter = new ACE.Database.SQLFormatters.World.CookBookSQLWriter();

            cookBookWriter.WeenieNames = weenieNames;

            string fileName = sqlWriter.GetDefaultFileName(input, cookBooks);

            using (StreamWriter writer = new StreamWriter(outputFolder + fileName))
            {
                if (includeDELETEStatementBeforeInsert)
                {
                    sqlWriter.CreateSQLDELETEStatement(input, writer);
                    writer.WriteLine();
                }

                sqlWriter.CreateSQLINSERTStatement(input, writer);

                if (cookBooks != null && cookBooks.Count > 0)
                {
                    writer.WriteLine();

                    if (includeDELETEStatementBeforeInsert)
                    {
                        cookBookWriter.CreateSQLDELETEStatement(cookBooks, writer);
                        writer.WriteLine();
                    }

                    cookBookWriter.CreateSQLINSERTStatement(cookBooks, writer);
                }
            }
        }
    }
}
