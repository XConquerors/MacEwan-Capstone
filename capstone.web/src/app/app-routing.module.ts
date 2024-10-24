// src/app/app-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { HeaderComponent } from './header/header.component';
import { AuthGuard } from './core/security/auth.guard';
import { CategoryComponent } from './components/category/category.component';
import { CategoryListComponent } from './components/category-list/category-list.component';
import { PriorityComponent } from './components/priority/priority.component';
import { PriorityListComponent } from './components/priority-list/priority-list.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', component: HeaderComponent, canActivate: [AuthGuard] }, // Example default page
  { path: "categories", component : CategoryListComponent},
  { path: "category/:id", component: CategoryComponent },
  { path: "category", component: CategoryComponent},
  { path: "priorities", component : PriorityListComponent},
  { path: "priority/:id", component: PriorityComponent },
  { path: "priority", component: PriorityComponent},
  { path: "", redirectTo: "/categories", pathMatch: "full" },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }