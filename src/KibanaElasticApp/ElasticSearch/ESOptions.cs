﻿using System;
namespace KibanaElasticApp.ElasticSearch
{
    public class ESOptions
    {
        public string Uri { get; set; }
        public string DefaultIndex { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
