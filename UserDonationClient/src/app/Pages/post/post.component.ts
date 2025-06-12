import { Component, OnInit, HostListener } from '@angular/core';
import { PostService } from '../../Services/post.service';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css'],
})
export class PostComponent implements OnInit {
  posts: any[] = [];
  loading: boolean = false;
  currentPage: number = 1;
  newComment: string = '';

  constructor(private postService: PostService) {}

  ngOnInit(): void {
    this.loadPosts();
  }

  loadPosts(): void {
    if (this.loading) return;
    this.loading = true;
    this.postService.getPosts(this.currentPage).subscribe((data: any) => {
      this.posts = [...this.posts, ...data.posts];
      this.loading = false;
      this.currentPage++;
    });
  }

  addComment(postId: number): void {
    if (!this.newComment.trim()) return;
    this.postService.addComment(postId, this.newComment).subscribe((comment: any) => {
      const post = this.posts.find((p) => p.id === postId);
      if (post) {
        post.comments.push(comment);
      }
      this.newComment = '';
    });
  }

  @HostListener('window:scroll', [])
  onScroll(): void {
    if (
      window.innerHeight + window.scrollY >= document.body.offsetHeight - 100 &&
      !this.loading
    ) {
      this.loadPosts();
    }
  }
}