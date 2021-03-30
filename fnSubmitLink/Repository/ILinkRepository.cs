using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Linkmir.AzFunctions.Models;

/*
 * Author   : Manish Solanki
 * Date     : March, 2021
 * All rights reserved.
 */

namespace Linkmir.AzFunctions.Repository
{
    public interface ILinkRepository
    {
        //IEnumerable<Link> GetLinks();
        void GetLinkByShortName(Link link);
        //Link GetLinkByID(long link);
        void InsertLink(Link link);
        //void DeleteLink(long linkID);
        //void UpdateLink(Link link);
        void Save();
    }
}
