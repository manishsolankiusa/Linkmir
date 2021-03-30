using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Linkmir.AzFunctions.Models;
using Linkmir.AzFunctions.Repository;
using Linkmir.AzFunctions.DBContexts;

/*
 * Author   : Manish Solanki
 * Date     : March, 2021
 * All rights reserved.
 */

namespace Linkmir.AzFunctions.Repository
{
    public class LinkRepository : ILinkRepository
    {
        private readonly LinkContext linkContext;

        public LinkRepository(LinkContext linkContext)
        {
            this.linkContext = linkContext;
        }

        /*public void DeleteLink(long linkID)
        {
            var link = _dbContext.Links.Find(linkID);
            _dbContext.Links.Remove(link);
            Save();
        }

        public Link GetLinkByID(long linkID)
        {
            return _dbContext.Links.Find(linkID);
        }

        public IEnumerable<Link> GetLinks()
        {
            return _dbContext.Links.ToList();
        }
        */

        public void GetLinkByShortName(Link link)
        {
            linkContext.GetLinkByShortName(link);
            linkContext.RetrieveLinks();
        }

        public void InsertLink(Link link)
        {
            linkContext.Insert(link);
            Save();
        }

        public void Save()
        {
            linkContext.SaveChanges();
        }
        /*
        public void UpdateLink(Link link)
        {
            _dbContext.Entry(link).State = EntityState.Modified;
            Save();
        }*/
    }
}
