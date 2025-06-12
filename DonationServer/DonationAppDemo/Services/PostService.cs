using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using DonationAppDemo.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DonationAppDemo.Services
{
    public class PostService : IPostService
    {
        private readonly IPostDal _postDal;
        private readonly ICommentDal _commentDal;
        private readonly IImagePostDal _imagePostDal;
        private readonly IWebHostEnvironment _env;

        public PostService(IPostDal postDal, ICommentDal commentDal, IImagePostDal imagePostDal, IWebHostEnvironment env)
        {
            _postDal = postDal;
            _commentDal = commentDal;
            _imagePostDal = imagePostDal;
            _env = env;
        }

        public async Task AddPostAsync(PostDto postDto)
        {
            var post = new Post
            {
                ContentPost = postDto.ContentPost,
                PostDate = DateTime.Now,
                AdminId = postDto.AdminId,
                Disabled = false
            };

            await _postDal.AddAsync(post);

            if (postDto.Image != null)
            {
                var filePath = await SaveImage(postDto.Image);
                var imagePost = new ImagePost
                {
                    PostId = post.Id,
                    ImageSrc = filePath
                };
                await _imagePostDal.AddAsync(imagePost);
            }
        }

        public async Task UpdatePostAsync(int id, PostDto postDto)
        {
            var post = await _postDal.GetByIdAsync(id);
            if (post == null) throw new KeyNotFoundException("Post not found");

            post.ContentPost = postDto.ContentPost;

            await _postDal.UpdateAsync(post);

            if (postDto.Image != null)
            {
                var filePath = await SaveImage(postDto.Image);
                var imagePost = await _imagePostDal.GetByPostIdAsync(post.Id);

                if (imagePost == null)
                {
                    imagePost = new ImagePost
                    {
                        PostId = post.Id,
                        ImageSrc = filePath
                    };
                    await _imagePostDal.AddAsync(imagePost);
                }
                else
                {
                    DeleteImage(imagePost.ImageSrc);

                    imagePost.ImageSrc = filePath;
                    await _imagePostDal.UpdateAsync(imagePost);
                }
            }
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _postDal.GetByIdAsync(id);
            if (post == null) throw new KeyNotFoundException("Post not found");

            var imagePost = await _imagePostDal.GetByPostIdAsync(post.Id);
            if (imagePost != null)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                DeleteImage(imagePost.ImageSrc);
#pragma warning restore CS8604 // Possible null reference argument.
                await _imagePostDal.DeleteByPostIdAsync(post.Id);
            }

            await _postDal.DeleteAsync(post);
        }

        public async Task<object> GetPostDetailAsync(int id)
        {
            var post = await _postDal.GetByIdAsync(id);
            if (post == null) throw new KeyNotFoundException("Post not found");

            var imagePost = await _imagePostDal.GetByPostIdAsync(post.Id);

            return new
            {
                post.ContentPost,
                post.PostDate,
                ImagePath = imagePost?.ImageSrc
            };
        }

        public async Task<object> GetPostsForAdminAsync(string postDate)
        {
            var posts = await _postDal.GetAllAsync();

            if (!string.IsNullOrEmpty(postDate))
            {
                posts = posts.Where(p => p.PostDate.ToString("yyyy-MM-dd") == postDate).ToList();
            }

            return posts.Select(p => new { p.Id, p.ContentPost, p.PostDate });
        }

        public async Task<object> GetPostsForUserAsync(int page, int pageSize)
        {
            var posts = await _postDal.GetAllAsync();
            var paginatedPosts = posts.Skip(page * pageSize).Take(pageSize).ToList();

            return new
            {
                Posts = paginatedPosts.Select(p => new { p.ContentPost, p.PostDate }),
                HasMore = posts.Count() > (page + 1) * pageSize
            };
        }

        public async Task AddCommentAsync(CommentDto commentDto)
        {
            var comment = new CommentPost
            {
                Comment = commentDto.Content, 
                UserId = commentDto.UserId,
                PostId = commentDto.PostId,
                UserRole = commentDto.Role
            };

            await _commentDal.AddAsync(comment);
        }

        public async Task UpdateCommentAsync(int id, CommentDto commentDto)
        {
            var comment = await _commentDal.GetByIdAsync(id);
            if (comment == null) throw new KeyNotFoundException("Comment not found");

            comment.Comment = commentDto.Content;
            await _commentDal.UpdateAsync(comment);
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _commentDal.GetByIdAsync(id);
            if (comment == null) throw new KeyNotFoundException("Comment not found");

            await _commentDal.DeleteAsync(comment);
        }

        public async Task<object> GetCommentsByPostAsync(int postId)
        {
            var comments = await _commentDal.GetByPostIdAsync(postId);
            return comments.Select(c => new { c.Comment, c.UserRole });
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return filePath;
        }

        private void DeleteImage(string imagePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, imagePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}