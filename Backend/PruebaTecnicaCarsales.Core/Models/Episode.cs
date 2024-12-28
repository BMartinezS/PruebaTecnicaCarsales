using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaCarsales.Core.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AirDate { get; set; }
        public string Episodeq { get; set; }
        public List<string> Characters { get; set; }
        public string Url { get; set; }
        public string Created { get; set; }
    }

    public class EpisodeResponse
    {
        public PaginationInfo Info { get; set; }
        public List<Episode> Results { get; set; }
    }

    public class PaginationInfo
    {
        public int Count { get; set; }
        public int Pages { get; set; }
        public string Next { get; set; }
        public string Prev { get; set; }
    }
}
