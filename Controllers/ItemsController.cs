using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    //GET /items
    [ApiController]
    [Route("items")]
    public class ItemController : ControllerBase
    {
        private readonly IItemsRepository repository;
        
        public  ItemController(IItemsRepository repository)
        {
            this.repository = repository;
        }

        //GET
        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {
            var items = repository.GetItems().Select(item => item.AsDto());
            return items;
        }

        //GET /items/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = repository.GetItem(id).AsDto();
            if(item is null)
            {
                return NotFound();
            }
            return item;
        }

        //POST /items
        [HttpPost]
        public ActionResult<ItemDto> CreatedItem(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id=Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow

            };

            repository.CreateItem(item);

            return CreatedAtAction(nameof(GetItem), new {id=item.Id}, item.AsDto());
        }

        //PUT /items
        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = repository.GetItem(id);
            if(existingItem is null)
            {
                return NotFound();
            }

            Item updatedItem = existingItem with
            {
                Name= itemDto.Name,
                Price = itemDto.Price
            };

            repository.UpdateItem(updatedItem);
            return NoContent();
        }

        //DELETE /items
        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = repository.GetItem(id);
            if (existingItem is null)
            {
                return NotFound();
            }

            repository.DeleteItem(id);
            return NoContent();
        }
    }

  
}