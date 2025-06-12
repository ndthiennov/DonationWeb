import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class PostService {
  private baseUrl = 'http://localhost:5178/api/post';

  constructor(private http: HttpClient) {}

  getPosts(postDate: string, page: number, itemsPerPage: number) {
    return this.http.get<any>(
      `${this.baseUrl}/List?postDate=${postDate}&page=${page}&itemsPerPage=${itemsPerPage}`
    );
  }

  getPostById(id: number) {
    return this.http.get<any>(`${this.baseUrl}/Detail/${id}`);
  }

  createPost(post: any) {
    return this.http.post<any>(`${this.baseUrl}/Add`, post);
  }

  updatePost(id: number, post: any) {
    return this.http.put<any>(`${this.baseUrl}/Update/${id}`, post);
  }

  deletePost(id: number) {
    return this.http.delete<any>(`${this.baseUrl}/Delete/${id}`);
  }
}