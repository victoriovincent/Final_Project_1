using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using PaymentAPI.Models;
using PaymentAPI.Data;
using System.Collections.Generic;
using System;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentController : ControllerBase
    {
        private ApiDbContext _context;

        public PaymentController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "Get All")]
        public async Task<ActionResult> GetPaymentDetailsItems()
        {
            var item = await _context.PaymentDetail.ToListAsync();
            return Ok(item);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.PaymentDetail.FirstOrDefaultAsync(x => x.id == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(Payment_details data)
        {
            if (ModelState.IsValid)
            {
                await _context.PaymentDetail.AddAsync(data);
                await _context.SaveChangesAsync();

                return new JsonResult("Data Added!");
            }

            return new JsonResult("Something went worng") { StatusCode = 500 };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, Payment_details item)
        {
            if (id != item.id)
                return BadRequest();

            var exsitItem = await _context.PaymentDetail.FirstOrDefaultAsync(x => x.id == id);

            if (exsitItem == null)
                return NotFound();

            exsitItem.id = item.id;
            exsitItem.cardOwnerName = item.cardOwnerName;
            exsitItem.cardNumber = item.cardNumber;
            exsitItem.expirationDate = item.expirationDate;
            exsitItem.securityCode = item.securityCode;

            await _context.SaveChangesAsync();

           return new JsonResult("Data Updated!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var exsitItem = await _context.PaymentDetail.FirstOrDefaultAsync(x => x.id == id);

            if (exsitItem == null)
                return NotFound();

            _context.PaymentDetail.Remove(exsitItem);
            await _context.SaveChangesAsync();

            return Ok(exsitItem);
        }
    }
}