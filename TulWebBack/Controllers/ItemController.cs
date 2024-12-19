using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TulWebBack.All_Views;
using TulWebBack.Entities;

namespace TulWebBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController(Context _cx) : ControllerBase
    {
        public Context Context { get; set; } = _cx;

        [HttpGet("all")]
        public async Task<ActionResult<ItemView>> GetAllItems()
        {
            return Ok(Context.Items.Select(i =>
                new ItemView()
                {
                    Id = i.Id,
                    Title = i.Props.Title,
                    Description = i.Props.Description,
                    Upakovka = i.Props.Upakovka,
                    GroupUpakovka = i.Props.GroupUpakovka,
                    Evropallet = i.Props.Evropallet,
                    ShtrihCode = i.Props.ShtrihCode,
                    Brand = i.Props.Brand,
                    Image = i.Image.Image,
                })
                );
        }

        [HttpGet("search")]
        public async Task<ActionResult<ItemView>> Search([FromQuery] string name)
        {
            return Ok(
                Context
                .Items
                .Where(i => i.Props.Title.Contains(name))
                .Select(i =>
                new ItemView()
                {
                    Id = i.Id,
                    Title = i.Props.Title,
                    Description = i.Props.Description,
                    Upakovka = i.Props.Upakovka,
                    GroupUpakovka = i.Props.GroupUpakovka,
                    Evropallet = i.Props.Evropallet,
                    ShtrihCode = i.Props.ShtrihCode,
                    Brand = i.Props.Brand,
                    Image = i.Image.Image,
                })
                );
        }


        [HttpGet("allbrand")]
        public async Task<ActionResult<ItemView>> GetAllByBrand([FromQuery] string _brand)
        {
            return Ok(
                Context
                .Items
                .Where(i => i.Props.Brand == _brand)
                .Select(i =>
                new ItemView()
                {
                    Id = i.Id,
                    Title = i.Props.Title,
                    Description = i.Props.Description,
                    Upakovka = i.Props.Upakovka,
                    GroupUpakovka = i.Props.GroupUpakovka,
                    Evropallet = i.Props.Evropallet,
                    ShtrihCode = i.Props.ShtrihCode,
                    Brand = i.Props.Brand,
                    Image = i.Image.Image,
                })
                );
        }

        [HttpGet("details")]
        public async Task<ActionResult<ItemView>> FindOnId([FromQuery] Guid id)
        {
            return Ok(
                Context
                .Items
                .Where(i => i.Id == id)
                .Select(i =>
                new ItemView()
                {
                    Id = i.Id,
                    Title = i.Props.Title,
                    Description = i.Props.Description,
                    Upakovka = i.Props.Upakovka,
                    GroupUpakovka = i.Props.GroupUpakovka,
                    Evropallet = i.Props.Evropallet,
                    ShtrihCode = i.Props.ShtrihCode,
                    Brand = i.Props.Brand,
                    Image = i.Image.Image,
                })
                );
        }

        [HttpGet("deleteorder")]
        public async Task<ActionResult<ItemView>> DeleteOrder([FromQuery] Guid idItem, [FromQuery] string Login)
        {
            var user = Context.Users.FirstOrDefault(u => u.Login == Login);
            var item = Context.Items.FirstOrDefault(u => u.Id == idItem);
            if (user == null || item == null)
                return BadRequest();
            item.Customers.Remove(user);
            user.Items.Remove(item);
            await Context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("place")]
        public async Task<ActionResult<ItemView>> Placing([FromQuery] Guid idItem, [FromQuery] string Login) 
        {
            var user = Context.Users.FirstOrDefault(u => u.Login == Login);
            var item = Context.Items.FirstOrDefault(u => u.Id == idItem);
            if (user == null || item == null)
                return BadRequest();
            item.Customers.Add(user);
            user.Items.Add(item);
            await Context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> AddItem([FromBody] ItemView _request)
        {
            var Image = new ItemImage() { Id = Guid.NewGuid(), Image = _request.Image };
            var Props = new ItemProperties()
            {
                Id = Guid.NewGuid(),
                Title = _request.Title,
                Description = _request.Description,
                Upakovka = _request.Upakovka,
                GroupUpakovka = _request.GroupUpakovka,
                Evropallet = _request.Evropallet,
                ShtrihCode = _request.ShtrihCode,
                Brand = _request.Brand
            };

            var Item = new Item() { Id = Guid.NewGuid(), Image = Image, Props = Props };

            Props.Item = Item;
            Image.Item = Item;


            await Context.Items.AddAsync(Item);
            await Context.ItemProperties.AddAsync(Props);
            await Context.ItemImages.AddAsync(Image);

            await Context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id::guid}")]
        public async Task<ActionResult<Guid>> DeleteItem(Guid id)
        {
            if (await Context.Items.FirstOrDefaultAsync(m => m.Id == id) == null)
                return BadRequest();

            await Context.Items.Where(m => m.Id == id).ExecuteDeleteAsync();
             
            return Ok();
        }

        [HttpPut("{id::guid}")]
        public async Task<ActionResult<Guid>> UpdateItem(Guid id, [FromBody] ItemView request)
        {
            var item = await Context.Items.FirstOrDefaultAsync(m => m.Id == id);

            if (item is null)
                return BadRequest();


            item.Props.Title = request.Title;
            item.Props.Description = request.Description;
            item.Props.Brand = request.Brand;
            item.Props.Evropallet = request.Evropallet;
            item.Props.ShtrihCode = request.ShtrihCode;
            item.Props.Upakovka = request.Upakovka;
            item.Props.GroupUpakovka = request.GroupUpakovka;
            item.Image.Image = request.Image;

            await Context.SaveChangesAsync();

            return Ok(id);

        }
    }
}
