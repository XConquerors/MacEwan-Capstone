import { Component } from '@angular/core';
import { Category } from '../../models/category';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryService } from '../../services/category/category.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrl: './category.component.css'
})
export class CategoryComponent {
    category : Category =
    {
      categoryId : 0,
      categoryName : "",
      description : ""
    }

    categories : Category[] = [];
    id: number | null = null;

    constructor(
      private route : ActivatedRoute,
      private router : Router,
      private categoryService : CategoryService
    ){ }

    ngOnInit(){
      const snapshot = this.route.snapshot;
      if(snapshot) {
        const idString = snapshot.paramMap.get('id');
        this.id = idString ? +idString : null;
        if(this.id !==null){
            this.categoryService.getCategory(this.id).subscribe((data) => {this.category = data;
              console.log("Fetched category:", data);
            },
              (error) => {
                console.error('Error fetching category:', error);  // Handle error
              }
            );
        }
      }     
    }

    saveCategory(){
      if(this.category.categoryId){
        this.categoryService.updateCategory(this.category).subscribe(() => this.goBack());
      }else{
        this.categoryService.addCategory(this.category).subscribe(() => this.goBack() )
      }
    }

    deleteCategory(){
      if(this.category.categoryId){
        this.categoryService.deleteCategory(this.category.categoryId).subscribe(() => this.goBack());
      }
    }

    goBack(){
      this.router.navigate(["/categories"]);
    }
}
