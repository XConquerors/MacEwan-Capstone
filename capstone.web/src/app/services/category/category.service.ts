import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from '../../models/category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private url = "https://localhost:7197/api/Categories"; 

  constructor(private http: HttpClient) {}

  // Fetch all categories (no pagination)
  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.url}`);
  }

  // Fetch single category by ID
  getCategory(id: number): Observable<Category> {
    return this.http.get<Category>(`${this.url}/${id}`);
  }

  // Add a new category (assuming backend handles ID generation)
  addCategory(category: Category): Observable<Category> {
    return this.http.post<Category>(this.url, category);
  }

  // Update an existing category
  updateCategory(category: Category): Observable<Category> {
    return this.http.put<Category>(`${this.url}/${category.categoryId}`, category);
  }

  // Delete a category by ID
  deleteCategory(categoryId: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${categoryId}`);
  }

  // Search categories by term
  searchCategories(term: string): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.url}/search?query=${term}`);
  }
}
