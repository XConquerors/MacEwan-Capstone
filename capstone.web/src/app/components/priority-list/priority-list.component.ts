import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { priorityService } from '../../services/priority/priority.service';
import { Priority } from '../../models/priority';

@Component({
  selector: 'app-Priority-list',
  templateUrl: './Priority-list.component.html',
  styleUrl: './Priority-list.component.css'
})
export class PriorityListComponent implements OnInit {

  Priorities: Priority[] = [];
  searchTerm : string = '';
  searchTermChanged: Subject<string> = new Subject<string>();

  constructor(private PriorityService: priorityService, private router: Router) {
    this.searchTermChanged.pipe(debounceTime(300)).subscribe(term => {
      this.searchCategories(term);
    });
  }

  ngOnInit() {
    this.getPriorities();  
  }

  getPriorities() {
    this.PriorityService.getPriority().subscribe((data: Priority[]) => {
      console.log('Priorities fetched:', data); // Log data
      this.Priorities = data;
    });
  }

  onSearchTermChange(term: string): void {
    this.searchTermChanged.next(term); // Notify subject
  }

  viewPriority(id: number) {
    this.router.navigate(["/Priority", id]);
  }

  addPriority() {
    this.router.navigate(["/Priority"]);
  }

  searchCategories(term: string): void {
    if(term && term.trim() !== '') {
      this.PriorityService.searchPriorities(term).subscribe((data: Priority[]) => {
        this.Priorities = data; 
      });
    } else {
      this.getPriorities(); // Reset to full Priority list if no search term
    }
  }

  goBack(){
    this.router.navigate(["/"]);
  }
}




