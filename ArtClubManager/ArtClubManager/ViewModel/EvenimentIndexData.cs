using ArtClubManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtClubManager.ViewModel
{
    public class EvenimentIndexData
    {
        public IEnumerable<Eveniment> Eveniment { get; set; }
        public IEnumerable<Membrii> Membrii { get; set; }


    }
}