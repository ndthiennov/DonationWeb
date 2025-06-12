import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class PostService {
  private baseUrl = 'http://localhost:5178/api/post';

  constructor(private http: HttpClient) {}

  getPosts(page: number) {
    return this.http.get<any>(`${this.baseUrl}/User/List?page=${page}&pageSize=5`);
  }

  addComment(postId: number, content: string) {
    return this.http.post<any>(`${this.baseUrl}/Comment/Add`, { 
      postId, 
      content 
    });
  }
}