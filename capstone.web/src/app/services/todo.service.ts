import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ToDo } from '../models/todo';


@Injectable({
  providedIn: 'root'
})
export class TodoService {
  private url = "https://localhost:7197/api/ToDo"; 

  constructor(private http: HttpClient) {}

  // Fetch all ToDos (no pagination)
  getTodos(): Observable<ToDo[]> {
    return this.http.get<ToDo[]>(`${this.url}`);
  }

  // Fetch a single ToDo by ID
  getTodo(id: number): Observable<ToDo> {
    return this.http.get<ToDo>(`${this.url}/${id}`);
  }

  // Add a new ToDo (assuming backend handles ID generation)
  addTodo(todo: ToDo): Observable<ToDo> {
    return this.http.post<ToDo>(this.url, todo);
  }

  // Update an existing ToDo
  updateTodo(todo: ToDo): Observable<ToDo> {
    return this.http.put<ToDo>(`${this.url}/${todo.toDoId}`, todo);
  }

  // Delete a ToDo by ID
  deleteTodo(toDoId: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${toDoId}`);
  }

  // Search ToDos by term
  searchTodos(term: string): Observable<ToDo[]> {
    return this.http.get<ToDo[]>(`${this.url}/search?term=${term}`);
  }
}
