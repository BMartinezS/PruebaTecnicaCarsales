using PruebaTecnicaCarsales.Core.Models;

namespace PruebaTecnicaCarsales.Core.Interfaces
{
    public interface IRickAndMortyService
    {
        Task<EpisodeResponse> GetEpisodesAsync(int page);
        Task<Episode> GetEpisodeByIdAsync(int id);
        Task<Character> GetCharacterByIdAsync(int id);
        Task<List<Character>> GetCharactersByIdsAsync(List<int> ids);
    }
}
