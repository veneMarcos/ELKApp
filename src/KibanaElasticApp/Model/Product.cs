using System;
using Nest;

namespace KibanaElasticApp.Model
{
    public class Product
    {
        public Guid Id { get; set; }

        [Text(Name="name")]
        public string Name { get; set; }

        [Text(Name="description")]
        public string Description { get; set; }

        [Text(Name="tags")]
        public string[] Tags { get; set; }  
    }
}
