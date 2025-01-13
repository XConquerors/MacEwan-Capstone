import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Priority } from '../../models/priority';

@Injectable({
  providedIn: 'root'
})
export class priorityService {
  private url = "https://localhost:7197/api/Priorities"; 

  constructor(private http: HttpClient) {}

  // Fetch all priority (no pagination)
  getPriority(): Observable<Priority[]> {
    return this.http.get<Priority[]>(`${this.url}`);
  }

  // Fetch single priority by ID
  getpriority(id: number): Observable<Priority> {
    return this.http.get<Priority>(`${this.url}/${id}`);
  }

  // Add a new priority (assuming backend handles ID generation)
  addpriority(priority: Priority): Observable<Priority> {
    return this.http.post<Priority>(this.url, priority);
  }

  // Update an existing priority
  updatepriority(priority: Priority): Observable<Priority> {
    return this.http.put<Priority>(`${this.url}/${priority.priorityId}`, priority);
  }

  // Delete a Priority by ID
  deletePriority(PriorityId: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${PriorityId}`);
  }

  // Search categories by term
  searchPriorities(term: string): Observable<Priority[]> {
    return this.http.get<Priority[]>(`${this.url}/search?term=${term}`);
  }
}
