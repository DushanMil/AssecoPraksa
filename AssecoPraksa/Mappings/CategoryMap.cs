using AssecoPraksa.Commands;
using CsvHelper.Configuration;

namespace AssecoPraksa.Mappings
{
    public class CategoryMap : ClassMap<CreateCategoryCommand>
    {
        public CategoryMap()
        {
            Map(p => p.Code).Index(0).Name("code");
            Map(p => p.ParentCode).Index(1).Name("parent-code");
            Map(p => p.Name).Index(2).Name("name");
        }
    }
}
