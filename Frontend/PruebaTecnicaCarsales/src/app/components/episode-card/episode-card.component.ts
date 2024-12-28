import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Episode } from '../../models/episode.model';

@Component({
  selector: 'app-episode-card',
  standalone: true,
  imports: [],
  templateUrl: './episode-card.component.html',
  styleUrl: './episode-card.component.css'
})
export class EpisodeCardComponent {
  @Input({ required: true }) episode!: Episode;
  @Output() viewDetails = new EventEmitter<Episode>();

  onViewDetails() {
    this.viewDetails.emit(this.episode);
  }
}
