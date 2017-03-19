using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using Sitecore.Data.Items;
using Sitecore.Hypermedia.Model;
using Sitecore.Hypermedia.Services;

namespace Sitecore.Hypermedia.Controllers
{
    [RoutePrefix("api/sch/items")]
    public class ItemsController : ApiController
    {
        private readonly IItemService _service;

        public ItemsController(IItemService service)
        {
            _service = service;
        }

        [Route("", Name = "SchItems")]
        public IEnumerable<ItemModel> Get()
        {
            var items = _service.GetContentItems();
            var urlHelper = new UrlHelper(Request);
            return items?.Select(i => Create(i, urlHelper))
                ?? Enumerable.Empty<ItemModel>();
        }

        [Route("{itemId}", Name = "SchItem")]
        public ItemModel Get(Guid itemId)
        {
            var item = _service.GetItem(itemId);
            return item != null ?
                Create(item, new UrlHelper(Request)) : null;
        }

        private ItemModel Create(Item item, UrlHelper urlHelper)
        {
            var url = urlHelper.Link(
                "SchItem",
                new
                {
                    itemId = item.ID.Guid,
                    language = item.Language.Name,
                    version = item.Version.Number
                });

            return new ItemModel
            {
                Name = item.Name,
                Title = item["Title"],
                Url = url
            };
        }
    }
}