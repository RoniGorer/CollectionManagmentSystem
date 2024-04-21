using System.Collections.Generic;

namespace CollectionManagmentSystem
{
    public class Collection
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Items { get; set; }

        public Collection()
        {
            Items = new List<string>();
        }
    }
}
