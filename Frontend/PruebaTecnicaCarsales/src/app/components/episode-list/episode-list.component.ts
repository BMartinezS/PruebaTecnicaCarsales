import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RickAndMortyService } from '../../services/rick-and-morty.service';
import { Episode, PaginationInfo } from '../../models/episode.model';
import { EpisodeCardComponent } from '../episode-card/episode-card.component';
import { firstValueFrom } from 'rxjs';
import { EpisodeDetailsComponent } from '../episode-details/episode-details.component';

@Component({
    selector: 'app-episode-list',
    standalone: true,
    imports: [CommonModule, EpisodeCardComponent, EpisodeDetailsComponent],
    templateUrl: './episode-list.component.html',
    styleUrl: './episode-list.component.css'
})

export class EpisodeListComponent {
    private service = inject(RickAndMortyService);

    episodes = signal<Episode[]>([]);
    pagination = signal<PaginationInfo | null>(null);
    currentPage = signal(1);
    loading = signal(false);
    error = signal<string | null>(null);

    selectedEpisode = signal<Episode | null>(null);

    constructor() {
        this.loadEpisodes();
    }

    async loadEpisodes() {
        try {
            this.loading.set(true);
            this.error.set(null);
            const response = await firstValueFrom(this.service.getEpisodes(this.currentPage()));
            this.episodes.set(response.results);
            this.pagination.set(response.info);
        } catch (err) {
            this.error.set('Error loading episodes. Please try again later.');
        } finally {
            this.loading.set(false);
        }
    }

    async changePage(page: number) {
        this.currentPage.set(page);
        await this.loadEpisodes();
    }

    showDetails(episode: Episode) {
        this.selectedEpisode.set(episode);
    }

    closeDetails() {
        this.selectedEpisode.set(null);
    }
}