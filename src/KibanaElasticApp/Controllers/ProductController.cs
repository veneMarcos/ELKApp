using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KibanaElasticApp.ElasticSearch;
using KibanaElasticApp.Model;
using Microsoft.AspNetCore.Mvc;
using Nest;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KibanaElasticApp.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private ElasticClientProvider _esclientProvider;

        public ProductController(ElasticClientProvider elasticClientProvider)
        {
            _esclientProvider = elasticClientProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            product.Id = Guid.NewGuid();

            var res = await _esclientProvider.Client.IndexAsync(new IndexRequest<Product>(product));

            if (res.IsValid)
            {
                throw new InvalidOperationException(res.DebugInformation);
            }

            return Ok();
        }

        [HttpGet("find")]
        public async Task<IActionResult> Find(string term)
        {
            var res = await _esclientProvider.Client.SearchAsync<Product>(x => x.Query(
                q => q.SimpleQueryString(qs =>
                                         qs.Query(term))));

            if (!res.IsValid)
            {
                throw new InvalidOperationException(res.DebugInformation);
            }

            return Json(res.Documents);
        }
    }
}
