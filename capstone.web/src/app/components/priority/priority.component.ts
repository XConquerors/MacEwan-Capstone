import { Component } from '@angular/core';
import { Priority } from '../../models/priority';
import { ActivatedRoute, Router } from '@angular/router';
import { priorityService } from '../../services/priority/priority.service';

@Component({
  selector: 'app-priority',
  templateUrl: './priority.component.html',
  styleUrl: './priority.component.css'
})
export class PriorityComponent {
    priority : Priority =
    {
      priorityId : 0,
      priorityName : "",
      description : "",
      priorityLevel :0
    }

    priorities : Priority[] = [];
    id: number | null = null;

    constructor(
      private route : ActivatedRoute,
      private router : Router,
     private priorityService : priorityService
    ){ }

    ngOnInit(){
      const snapshot = this.route.snapshot;
      if(snapshot) {
        const idString = snapshot.paramMap.get('id');
        this.id = idString ? +idString : null;
        if(this.id !==null){
            this.priorityService.getpriority(this.id).subscribe((data) => {this.priority = data;
              console.log("Fetched priority:", data);
            },
              (error) => {
                console.error('Error fetching priority:', error);  // Handle error
              }
            );
        }
      }     
    }

    savePriority(){
      if(this.priority.priorityId){
        this.priorityService.updatepriority(this.priority).subscribe(() => this.goBack());
      }else{
        this.priorityService.addpriority(this.priority).subscribe(() => this.goBack() )
      }
    }

    deletePriority(){
      if(this.priority.priorityId){
        this.priorityService.deletePriority(this.priority.priorityId).subscribe(() => this.goBack());
      }
    }

    goBack(){
      this.router.navigate(["/priorities"]);
    }
}
