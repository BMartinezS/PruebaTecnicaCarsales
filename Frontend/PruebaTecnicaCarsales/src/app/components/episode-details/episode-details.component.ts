import { Component, EventEmitter, inject, Input, Output, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { RickAndMortyService } from '../../services/rick-and-morty.service';
import { Character } from '../../models/character.model';
import { Episode } from '../../models/episode.model';

@Component({
  selector: 'app-episode-details',
  standalone: true,
  imports: [],
  templateUrl: './episode-details.component.html',
  styleUrl: './episode-details.component.css'
})
export class EpisodeDetailsComponent {
  @Input({ required: true }) episode!: Episode;
  @Output() closeModal = new EventEmitter<void>();
  private service = inject(RickAndMortyService);

  loading = signal(false);
  error = signal<string | null>(null);
  characters = signal<Character[]>([]);
  episodeData = signal<Episode | null>(null);

  ngOnInit() {
    this.loadCharacters();
  }

  private async loadCharacters() {
    try {
      this.loading.set(true);
      this.error.set(null);

      // Dividir las URLs en grupos para hacer peticiones más pequeñas
      const characterUrls = this.episode.characters;
      const batchSize = 20;
      const batches = [];

      for (let i = 0; i < characterUrls.length; i += batchSize) {
        batches.push(characterUrls.slice(i, i + batchSize));
      }

      // Hacer las peticiones en paralelo por lotes
      const batchResults = await Promise.all(
        batches.map(batch =>
          firstValueFrom(this.service.getCharactersByUrls(batch))
        )
      );

      // Combina los resultados
      const allCharacters = batchResults.flat();
      this.characters.set(allCharacters);
    } catch (err) {
      this.error.set('Error loading characters. Please try again.');
    } finally {
      this.loading.set(false);
    }
  }
} 
