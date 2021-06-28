using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public PostRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<PostDto>> GetPostsAsync()
        {
            return await _context.Posts
                .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}