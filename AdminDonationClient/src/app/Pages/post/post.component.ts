import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PostService } from './post.service';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css'],
})
export class PostComponent implements OnInit {
  postForm: FormGroup;
  isCreateMode: boolean = true;
  posts: any[] = [];
  startDate: string = '';
  endDate: string = '';
  currentPage: number = 1;
  itemsPerPage: number = 5;

  constructor(private fb: FormBuilder, private postService: PostService) {
    this.postForm = this.fb.group({
      title: [''],
      content: [''],
      postDate: [''],
    });
  }

  ngOnInit(): void {
    this.loadPosts();
  }

  loadPosts() {
    this.postService
      .getPosts(this.startDate, this.currentPage, this.itemsPerPage)
      .subscribe((data: any) => {
        this.posts = data.posts;
        this.currentPage = data.currentPage;
        this.itemsPerPage = data.itemsPerPage;
      });
  }

  searchPosts() {
    this.currentPage = 1;
    this.loadPosts();
  }

  onSubmit() {
    const postData = this.postForm.value;
    if (this.isCreateMode) {
      this.postService.createPost(postData).subscribe(() => this.loadPosts());
    } else {
      this.postService.updatePost(postData.id, postData).subscribe(() => this.loadPosts());
    }
  }

  editPost(id: number) {
    this.isCreateMode = false;
    this.postService.getPostById(id).subscribe((data) => {
      this.postForm.patchValue(data);
    });
  }

  deletePost(id: number) {
    this.postService.deletePost(id).subscribe(() => this.loadPosts());
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.loadPosts();
  }

  onFileSelect(event: any) {
    const files = event.target.files;
    console.log('Selected files:', files);
  }
}
