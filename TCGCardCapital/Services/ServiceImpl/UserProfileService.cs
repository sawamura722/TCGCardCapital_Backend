﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCGCardCapital.DTOs;
using TCGCardCapital.Models;
using TCGCardCapital.Services.IService;

namespace TCGCardCapital.Services.ServiceImpl
{
    public class UserProfileService : IUserProfileService
    {
        private readonly TcgcardCapitalContext _context;
        private readonly IMapper _mapper;

        public UserProfileService(TcgcardCapitalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserProfileDTO>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserProfileDTO>>(users);
        }

        public async Task<UserProfileDTO> GetUserProfileByUserIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;
            return _mapper.Map<UserProfileDTO>(user);
        }
        public async Task<bool> UpdateUserProfileAsync(int userId, UserProfileDTO userProfileDTO)
        {
            // Fetch the product based on the route id
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            // Update the necessary fields
            user.Username = userProfileDTO.Username;
            user.Location = userProfileDTO.Location;
            user.UpdatedAt = DateTime.Now;


            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.UserId == userId))
                    return false;
                throw;
            }
        }

        public async Task<bool> BuySubscription(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.SubscriptionStatus = true;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.UserId == userId))
                    return false;
                throw;
            }
        }

        public async Task<bool> IncreasePoint(int userId, IncreasePointDTO increasePointDTO)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.Point += increasePointDTO.Point;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.UserId == userId))
                    return false;
                throw;
            }
        }

        public async Task<bool> DecreasePoint(int userId, IncreasePointDTO increasePointDTO)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.Point -= increasePointDTO.Point;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.UserId == userId))
                    return false;
                throw;
            }
        }
    }
}
