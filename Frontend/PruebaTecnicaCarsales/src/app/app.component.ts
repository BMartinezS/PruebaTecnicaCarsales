import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { EpisodeListComponent } from './components/episode-list/episode-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, EpisodeListComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'PruebaTecnicaCarsales';
}
