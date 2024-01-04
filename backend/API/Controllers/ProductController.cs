using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Data;

namespace API.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly NikeContext _context;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper, NikeContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> Get()
        {
            var result = await _unitOfWork.Products.GetAllAsync();
            if (result.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<List<ProductDto>>(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var result = await _unitOfWork.Products.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ProductDto>(result));
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Post(ProductDto dto)
        {
            var result = _mapper.Map<Product>(dto);
            _unitOfWork.Products.Add(result);
            await _unitOfWork.SaveAsync();
            if (result == null)
            {
                return BadRequest();
            }
            dto.Id = result.Id;
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Put(int id, ProductDto dto)
        {
            var exists = await _unitOfWork.Products.GetByIdAsync(id);
            if (exists == null)
            {
                return NotFound();
            }
            if (dto.Id == 0)
            {
                dto.Id = exists.Id;
            }
            if (dto.Id != id)
            {
                return BadRequest();
            }
            _mapper.Map(dto, exists);
            await _unitOfWork.SaveAsync();
            return Ok(_mapper.Map<ProductDto>(exists));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _unitOfWork.Products.GetByIdAsync(id);
            if (exists == null)
            {
                return NotFound();
            }
            _unitOfWork.Products.Remove(exists);
            await _unitOfWork.SaveAsync();
            return Ok("Product was removed successfully.");
        }

        [HttpPut("incar{id}")]
        public async Task<ActionResult> PutInCar(int id, string username)
        {
            var exists = await _unitOfWork.Products.GetByIdAsync(id);
            var user = await _unitOfWork.Users.GetByUsernameAsync(username);
            var usercars = await _unitOfWork.UserCars.GetAllAsync();
            var usercar = await _unitOfWork.UserCars.GetUserCarByIds(user.Id, exists.Id);
            if (exists == null)
            {
                return NotFound();
            }
            if (usercar != null)
            {
                usercar.QuantityInCar += 1;
                _unitOfWork.UserCars.Update(usercar);
                await _unitOfWork.SaveAsync();
                return Ok("+1");
            }
            else if (usercar == null)
            {
                exists.IdStateFk = 1;
                UserCar x = new UserCar()
                {
                    Id = 1,
                    QuantityInCar = 1,
                    IdUserFk = user.Id,
                    IdProductFk = exists.Id
                };
                if (usercars.Count != 0)
                {
                    x.Id = usercars[usercars.Count() - 1].Id + 1;
                }
                _unitOfWork.UserCars.Add(x);
                _unitOfWork.Products.Update(exists);
                await _unitOfWork.SaveAsync();
                return Ok("In");
            }
            return BadRequest();
        }

        [HttpPut("outcar{id}")]
        public async Task<ActionResult> PutOutCar(int id, string username)
        {
            var exists = await _unitOfWork.Products.GetByIdAsync(id);
            var user = await _unitOfWork.Users.GetByUsernameAsync(username);
            var usercar = await _unitOfWork.UserCars.GetUserCarByIds(user.Id, exists.Id);
            if (exists == null)
            {
                return NotFound();
            }
            if (usercar.QuantityInCar > 1)
            {
                usercar.QuantityInCar -= 1;
                _unitOfWork.UserCars.Update(usercar);
                await _unitOfWork.SaveAsync();
                return Ok("-1");
            }
            if (usercar.QuantityInCar == 1)
            {
                usercar.QuantityInCar -= 1;
                exists.IdStateFk = 2;
                _unitOfWork.UserCars.Remove(usercar);
                _unitOfWork.Products.Update(exists);
                await _unitOfWork.SaveAsync();
                return Ok("Out");
            }
            return BadRequest();
        }

        [HttpGet("type{type}")]
        public async Task<ActionResult<List<ProductDto>>> GetByType(string type)
        {
            var result = await _unitOfWork.Products.GetProductByType(type);
            if (result.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<List<ProductDto>>(result));
        }

        [HttpGet("car{user}")]
        public async Task<ActionResult<List<object>>> GetInCar(string user)
        {
            var result = await _unitOfWork.Products.GetProductInCar(user);
            if (result.Count() == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}