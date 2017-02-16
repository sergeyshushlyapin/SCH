using System.Linq;
using System.Web.Http;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Hypermedia.SimpleWebApi
{
    public class PagesController : ApiController
    {
        private static readonly TemplateID CommenTemplateId =
            new TemplateID(new ID("{3595418B-6538-40DE-A73A-2FC02467050E}"));

        private readonly Database _database = Database.GetDatabase("master");

        [Route("api/pages")]
        public string[] Get()
        {
            return _database.GetItem("/sitecore/content")
                .Children
                .Select(x => x.Name)
                .ToArray();
        }

        [Route("api/pages/{pageNum}")]
        public string Get(int pageNum)
        {
            return _database.GetItem("/sitecore/content")
                .Children[pageNum - 1]
                .Name;
        }

        [Route("api/pages/{pageNum}/comments")]
        public Comment[] GetComments(int pageNum)
        {
            return _database.GetItem("/sitecore/content")
                .Children[pageNum - 1]
                .Children
                .Select(x =>
                    new Comment { Author = x["Author"], Text = x["Content"] })
                .ToArray();
        }

        [HttpPost]
        [Route("api/pages/{pageNum}/comments")]
        public void NewComment(int pageNum, Comment comment)
        {
            var page = _database.GetItem("/sitecore/content")
                .Children[pageNum - 1];
            var commentItem = page.Add(comment.Author, CommenTemplateId);
            using (new EditContext(commentItem))
            {
                commentItem["Author"] = comment.Author;
                commentItem["Content"] = comment.Text;
            }
        }
    }
}