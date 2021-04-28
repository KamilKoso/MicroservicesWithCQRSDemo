using Discount.API.Models;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository discountRepository;

        public DiscountController(IDiscountRepository discountRepository)
        {
            this.discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
        }

        [HttpGet("GetDiscount", Name = "GetDiscount")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscount([FromQuery]string productName)
        {
            return Ok(await discountRepository.GetDiscount(productName));
        }

        [HttpPost("CreateDiscount")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
        {
            await discountRepository.CreateDiscount(coupon);
            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
        }

        [HttpPut("UpdateDiscount")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateDiscount([FromBody] Coupon coupon)
        {
            return Ok(await discountRepository.UpdateDiscount(coupon));
        }

        [HttpDelete("DeleteDiscount")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteDiscount([FromQuery] string productName)
        {
            return Ok(await discountRepository.DeleteDiscount(productName));
        }
    }
}
