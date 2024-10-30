import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

import { ToDo } from '../../models/todo';
import { TodoService } from '../../services/todo.service';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent implements OnInit {

  todos: ToDo[] = [];
  searchTerm: string = '';
  searchTermChanged: Subject<string> = new Subject<string>();

  constructor(private todoService: TodoService, private router: Router) {
    this.searchTermChanged.pipe(debounceTime(300)).subscribe(term => {
      this.searchTodos(term);
    });
  }

  ngOnInit() {
    this.getTodos();  
  }

  getTodos() {
    this.todoService.getTodos().subscribe((data: ToDo[]) => {
      console.log('ToDos fetched:', data); // Log data
      this.todos = data;
    });
  }

  onSearchTermChange(term: string): void {
    this.searchTermChanged.next(term); // Notify subject
  }

  viewToDo(id: number) {
    this.router.navigate(["/todo", id]);
  }

  addToDo() {
    this.router.navigate(["/todo"]);
  }

  searchTodos(term: string): void {
    if (term && term.trim() !== '') {
      this.todoService.searchTodos(term).subscribe((data: ToDo[]) => {
        this.todos = data; 
      });
    } else {
      this.getTodos(); // Reset to full ToDo list if no search term
    }
  }

  goBack() {
    this.router.navigate(["/"]);
  }
}
