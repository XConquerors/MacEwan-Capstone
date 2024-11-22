import { Component, OnInit } from '@angular/core';
import { ToDo } from '../../models/todo';
import { ActivatedRoute, Router } from '@angular/router';
import { TodoService } from '../../services/todo.service';


@Component({
  selector: 'app-todo',
  templateUrl: './todo.component.html',
  styleUrls: ['./todo.component.css']
})
export class TodoComponent implements OnInit {
  todo: ToDo = {
    toDoId: 0,
    name: "",
    description: "",
    categoryId: 0,
    priorityId: 0
  };

  todos: ToDo[] = [];
  id: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private todoService: TodoService
  ) {}

  ngOnInit() {
    const snapshot = this.route.snapshot;
    if (snapshot) {
      const idString = snapshot.paramMap.get('id');
      this.id = idString ? +idString : null;
      if (this.id !== null) {
        this.todoService.getTodo(this.id).subscribe(
          (data) => {
            this.todo = data;
            console.log("Fetched todo:", data);
          },
          (error) => {
            console.error('Error fetching todo:', error); // Handle error
          }
        );
      }
    }
  }

  saveToDo() {
    if (this.todo.toDoId) {
      this.todoService.updateTodo(this.todo).subscribe(() => this.goBack());
    } else {
      this.todoService.addTodo(this.todo).subscribe(() => this.goBack());
    }
  }

  deleteToDo() {
    if (this.todo.toDoId) {
      this.todoService.deleteTodo(this.todo.toDoId).subscribe(() => this.goBack());
    }
  }

  goBack() {
    this.router.navigate(["/todos"]);
  }
}
