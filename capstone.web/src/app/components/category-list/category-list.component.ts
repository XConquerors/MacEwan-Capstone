import { Component, OnInit } from '@angular/core';
import { Category } from '../../models/category';
import { CategoryService } from '../../services/category/category.service';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.css'
})
export class CategoryListComponent implements OnInit {

  categories: Category[] = [];
  searchTerm : string = '';
  searchTermChanged: Subject<string> = new Subject<string>();

  constructor(private categoryService: CategoryService, private router: Router) {
    this.searchTermChanged.pipe(debounceTime(300)).subscribe(term => {
      this.searchCategories(term);
    });
  }

  ngOnInit() {
    this.getCategories();  
  }

  getCategories() {
    this.categoryService.getCategories().subscribe((data: Category[]) => {
      console.log('Categories fetched:', data); // Log data
      this.categories = data;
    });
  }

  onSearchTermChange(term: string): void {
    this.searchTermChanged.next(term); // Notify subject
  }

  viewCategory(id: number) {
    this.router.navigate(["/category", id]);
  }

  addCategory() {
    this.router.navigate(["/category"]);
  }

  searchCategories(term: string): void {
    if(term && term.trim() !== '') {
      this.categoryService.searchCategories(term).subscribe((data: Category[]) => {
        this.categories = data; 
      });
    } else {
      this.getCategories(); // Reset to full category list if no search term
    }
  }

  goBack(){
    this.router.navigate(["/"]);
  }
}
