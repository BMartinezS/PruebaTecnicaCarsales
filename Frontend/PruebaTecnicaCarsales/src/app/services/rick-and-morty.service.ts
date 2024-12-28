import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../environments/environment';
import { EpisodeResponse, Episode } from '../models/episode.model';
import { Character } from '../models/character.model';
import { map, of } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class RickAndMortyService {
    private http = inject(HttpClient);
    private baseUrl = environment.apiUrl;
    private characterCache = new Map<string, Character>();

    getEpisodes(page: number = 1) {
        return this.http.get<EpisodeResponse>(`${this.baseUrl}/episodes?page=${page}`);
    }

    getEpisodeById(id: number) {
        return this.http.get<Episode>(`${this.baseUrl}/episodes/${id}`);
    }

    private extractCharacterId(url: string): number {
        const parts = url.split('/');
        return parseInt(parts[parts.length - 1]);
    }

    getCharactersByUrls(urls: string[]) {
        // Filtrar las URLs que no están en caché
        const uncachedUrls = urls.filter(url => !this.characterCache.has(url));

        if (uncachedUrls.length === 0) {
            // Si todos los personajes están en caché, devolverlos inmediatamente
            return of(urls.map(url => this.characterCache.get(url)!));
        }

        // Obtener los IDs no cacheados
        const idsString = uncachedUrls
            .map(url => this.extractCharacterId(url))
            .join(',');

        return this.http.get<Character[]>(`${this.baseUrl}/characters/batch?ids=${idsString}`).pipe(
            map(characters => {
                // Guardar los nuevos personajes en caché
                characters.forEach(character => {
                    this.characterCache.set(character.url, character);
                });

                // Devolver todos los personajes (cacheados y nuevos)
                return urls.map(url => this.characterCache.get(url)!);
            })
        );
    }

    getCharacterById(id: number) {
        return this.http.get<Character>(`${this.baseUrl}/characters/${id}`);
    }
}