﻿using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Auth;
using job_search_be.Domain.Entity;
using job_search_be.Domain.Repositories;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Repositories;
using job_search_be.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.Service
{
    public class AuthService : IAuthService
    {
        private readonly JWTSettings _jwtSettings;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;

        public AuthService(IOptions<JWTSettings> jwtSettings, IUserRepository userRepository, IMapper mapper, IRoleRepository roleRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        public DataResponse<TokenDto> Login(LoginDto dto)
        {
                var user = _userRepository.GetAllData().Where(x => x.Email == dto.Email).SingleOrDefault();
                if (user == null)
                {
                    throw new ApiException(401, "Tài khoản không tồn tại");
                }
                var isPasswordValid = PasswordHelper.VerifyPassword(dto.Password, user.PassWord);
                if (!isPasswordValid)
                {
                    throw new ApiException(401, "Mật khẩu không chính xác");
                }
                else
                {
                    var roles = _roleRepository.GetAllData().Where(x=>x.RoleId==user.RoleId);
                    List<string> roleNames = new List<string>();
                    foreach (var role in roles)
                    {
                        roleNames.Add(role.NameRole);
                    }
                    //return null;
                    return new DataResponse<TokenDto>(CreateToken(user, roleNames), 200, "Đăng nhập thành công");
                }
          
                throw new ApiException(401, "Đăng nhập thất bại");
        }
        public TokenDto CreateToken(User user, List<string> roles)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_jwtSettings.RefreshTokenExpiration);
            var securityKey = Encoding.ASCII.GetBytes(_jwtSettings.SecurityKey);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey),
                SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience[0],
                expires: accessTokenExpiration,
                //expires: DateTime.Now.AddMinutes(2),
                 notBefore: DateTime.Now,
                 claims: GetClaims(user, _jwtSettings.Audience, roles),
                 signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = (int)((DateTimeOffset)accessTokenExpiration).ToUnixTimeSeconds(),
                RefreshTokenExpiration = (int)((DateTimeOffset)refreshTokenExpiration).ToUnixTimeSeconds()
          /*      AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration*/
            };
            user.Is_Active = true;
            _userRepository.Update(user);
            return tokenDto;
        }


        private IEnumerable<Claim> GetClaims(User user, List<string> audiences, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            claims.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return claims;
        }

        private string CreateRefreshToken()
        {
            var numberByte = new byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }
    }
}
